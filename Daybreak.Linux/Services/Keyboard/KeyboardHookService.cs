using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Keyboard;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Daybreak.Linux.Services.Keyboard;

public partial class KeyboardHookService : IHostedService, IKeyboardHookService, IDisposable
{
    private const string X11Lib = "libX11.so.6";

    // X11 event types
    private const int KeyPress = 2;
    private const int KeyRelease = 3;

    // XK key codes (from X11/keysymdef.h)
    private const uint XK_Escape = 0xff1b;
    private const uint XK_Return = 0xff0d;
    private const uint XK_space = 0x0020;
    private const uint XK_F1 = 0xffbe;
    private const uint XK_F2 = 0xffbf;
    private const uint XK_F3 = 0xffc0;
    private const uint XK_F4 = 0xffc1;
    private const uint XK_F5 = 0xffc2;
    private const uint XK_F6 = 0xffc3;
    private const uint XK_F7 = 0xffc4;
    private const uint XK_F8 = 0xffc5;
    private const uint XK_F9 = 0xffc6;
    private const uint XK_F10 = 0xffc7;
    private const uint XK_F11 = 0xffc8;
    private const uint XK_F12 = 0xffc9;

    [StructLayout(LayoutKind.Sequential)]
    private struct XKeyEvent
    {
        public int type;
        public ulong serial;
        public int send_event;
        public nint display;
        public ulong window;
        public ulong root;
        public ulong subwindow;
        public ulong time;
        public int x, y;
        public int x_root, y_root;
        public uint state;
        public uint keycode;
        public int same_screen;
    }

    // XEvent is a union, we need 192 bytes to cover all cases
    [StructLayout(LayoutKind.Explicit, Size = 192)]
    private struct XEvent
    {
        [FieldOffset(0)]
        public int type;

        [FieldOffset(0)]
        public XKeyEvent xkey;
    }

    [LibraryImport(X11Lib, EntryPoint = "XOpenDisplay")]
    private static partial nint XOpenDisplay(nint display_name);

    [LibraryImport(X11Lib, EntryPoint = "XCloseDisplay")]
    private static partial int XCloseDisplay(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XDefaultRootWindow")]
    private static partial ulong XDefaultRootWindow(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XPending")]
    private static partial int XPending(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XNextEvent")]
    private static partial int XNextEvent(nint display, ref XEvent event_return);

    [LibraryImport(X11Lib, EntryPoint = "XLookupKeysym")]
    private static partial uint XLookupKeysym(ref XKeyEvent key_event, int index);

    [LibraryImport(X11Lib, EntryPoint = "XGrabKey")]
    private static partial int XGrabKey(
        nint display,
        int keycode,
        uint modifiers,
        ulong grab_window,
        int owner_events,
        int pointer_mode,
        int keyboard_mode);

    [LibraryImport(X11Lib, EntryPoint = "XUngrabKey")]
    private static partial int XUngrabKey(nint display, int keycode, uint modifiers, ulong grab_window);

    [LibraryImport(X11Lib, EntryPoint = "XKeysymToKeycode")]
    private static partial byte XKeysymToKeycode(nint display, uint keysym);

    private nint display;
    private ulong rootWindow;
    private Thread? eventThread;
    private CancellationTokenSource? cts;
    private bool isStarted;
    private readonly List<byte> grabbedKeycodes = [];

    public event EventHandler<KeyboardEventArgs>? KeyDown;
    public event EventHandler<KeyboardEventArgs>? KeyUp;

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        this.Stop();
        return Task.CompletedTask;
    }

    public void Start()
    {
        if (this.isStarted)
        {
            return;
        }

        this.display = XOpenDisplay(nint.Zero);
        if (this.display == nint.Zero)
        {
            return;
        }

        this.rootWindow = XDefaultRootWindow(this.display);

        // Grab the keys we're interested in on the root window
        this.GrabKeys();

        this.cts = new CancellationTokenSource();
        this.eventThread = new Thread(this.EventLoop)
        {
            IsBackground = true,
            Name = "X11 Keyboard Event Loop"
        };
        this.eventThread.Start();

        this.isStarted = true;
    }

    public void Stop()
    {
        if (!this.isStarted)
        {
            return;
        }

        this.cts?.Cancel();
        this.eventThread?.Join(TimeSpan.FromSeconds(1));

        if (this.display != nint.Zero)
        {
            this.UngrabKeys();
            XCloseDisplay(this.display);
            this.display = nint.Zero;
        }

        this.cts?.Dispose();
        this.cts = null;
        this.isStarted = false;
    }

    public void Dispose()
    {
        this.Stop();
    }

    private void GrabKeys()
    {
        uint[] keysyms = [XK_F1, XK_F2, XK_F3, XK_F4, XK_F5, XK_F6, XK_F7, XK_F8, XK_F9, XK_F10, XK_F11, XK_F12, XK_Escape, XK_Return, XK_space];

        foreach (var keysym in keysyms)
        {
            var keycode = XKeysymToKeycode(this.display, keysym);
            if (keycode != 0)
            {
                // GrabModeAsync = 1
                XGrabKey(this.display, keycode, 0, this.rootWindow, 1, 1, 1);
                this.grabbedKeycodes.Add(keycode);
            }
        }
    }

    private void UngrabKeys()
    {
        foreach (var keycode in this.grabbedKeycodes)
        {
            XUngrabKey(this.display, keycode, 0, this.rootWindow);
        }

        this.grabbedKeycodes.Clear();
    }

    private void EventLoop()
    {
        while (this.cts is { IsCancellationRequested: false })
        {
            if (XPending(this.display) > 0)
            {
                var xEvent = new XEvent();
                XNextEvent(this.display, ref xEvent);
                this.ProcessEvent(ref xEvent);
            }
            else
            {
                Thread.Sleep(10);
            }
        }
    }

    private void ProcessEvent(ref XEvent xEvent)
    {
        if (xEvent.type is not KeyPress and not KeyRelease)
        {
            return;
        }

        var keysym = XLookupKeysym(ref xEvent.xkey, 0);

        if (!TryMapXKeyToVirtualKey(keysym, out var virtualKey))
        {
            return;
        }

        if (xEvent.type == KeyPress)
        {
            this.KeyDown?.Invoke(this, new KeyboardEventArgs(virtualKey));
        }
        else
        {
            this.KeyUp?.Invoke(this, new KeyboardEventArgs(virtualKey));
        }
    }

    private static bool TryMapXKeyToVirtualKey(uint keysym, out VirtualKey virtualKey)
    {
        virtualKey = keysym switch
        {
            XK_F1 => VirtualKey.F1,
            XK_F2 => VirtualKey.F2,
            XK_F3 => VirtualKey.F3,
            XK_F4 => VirtualKey.F4,
            XK_F5 => VirtualKey.F5,
            XK_F6 => VirtualKey.F6,
            XK_F7 => VirtualKey.F7,
            XK_F8 => VirtualKey.F8,
            XK_F9 => VirtualKey.F9,
            XK_F10 => VirtualKey.F10,
            XK_F11 => VirtualKey.F11,
            XK_F12 => VirtualKey.F12,
            XK_Escape => VirtualKey.Escape,
            XK_Return => VirtualKey.Enter,
            XK_space => VirtualKey.Space,
            _ => default
        };

        return virtualKey != default;
    }
}
