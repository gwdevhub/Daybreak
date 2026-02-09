using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Keyboard;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Daybreak.Linux.Services.Keyboard;

public partial class KeyboardHookService : IHostedService, IKeyboardHookService, IDisposable
{
    private const string X11Lib = "libX11.so.6";
    private const string XtstLib = "libXtst.so.6";

    // X11 event types
    private const int KeyPress = 2;
    private const int KeyRelease = 3;

    // XRecord constants
    private const int XRecordFromServer = 0;
    private const int XRecordAllClients = 3;

    // XK key codes (from X11/keysymdef.h)
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

    // XRecordRange structure
    [StructLayout(LayoutKind.Sequential)]
    private struct XRecordRange
    {
        public XRecordRange8 core_requests;
        public XRecordRange8 core_replies;
        public XRecordExtRange ext_requests;
        public XRecordExtRange ext_replies;
        public XRecordRange8 delivered_events;
        public XRecordRange8 device_events;
        public XRecordRange8 errors;
        public byte client_started;
        public byte client_died;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XRecordRange8
    {
        public byte first;
        public byte last;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XRecordExtRange
    {
        public XRecordRange16 ext_major;
        public XRecordRange8 ext_minor;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XRecordRange16
    {
        public ushort first;
        public ushort last;
    }

    // Callback delegate for XRecordEnableContext
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void XRecordInterceptProc(nint closure, ref XRecordInterceptData recorded_data);

    // XRecordInterceptData structure
    [StructLayout(LayoutKind.Sequential)]
    private struct XRecordInterceptData
    {
        public nint id_base;
        public nint server_time;
        public nint client_seq;
        public int category;
        public byte client_swapped;
        private byte pad1;
        private byte pad2;
        private byte pad3;
        public nint data;
        public uint data_len;
    }

    // X11 P/Invoke
    [LibraryImport(X11Lib, EntryPoint = "XOpenDisplay")]
    private static partial nint XOpenDisplay(nint display_name);

    [LibraryImport(X11Lib, EntryPoint = "XCloseDisplay")]
    private static partial int XCloseDisplay(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XFlush")]
    private static partial int XFlush(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XKeycodeToKeysym")]
    private static partial uint XKeycodeToKeysym(nint display, byte keycode, int index);

    [LibraryImport(X11Lib, EntryPoint = "XFree")]
    private static partial int XFree(nint data);

    // XRecord extension P/Invoke
    [LibraryImport(XtstLib, EntryPoint = "XRecordQueryVersion")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool XRecordQueryVersion(nint display, out int major, out int minor);

    [LibraryImport(XtstLib, EntryPoint = "XRecordAllocRange")]
    private static partial nint XRecordAllocRange();

    [LibraryImport(XtstLib, EntryPoint = "XRecordCreateContext")]
    private static partial ulong XRecordCreateContext(
        nint display,
        int flags,
        nint[] client_specs,
        int nclients,
        nint[] ranges,
        int nranges);

    [LibraryImport(XtstLib, EntryPoint = "XRecordEnableContext")]
    private static partial int XRecordEnableContext(
        nint display,
        ulong context,
        XRecordInterceptProc callback,
        nint closure);

    [LibraryImport(XtstLib, EntryPoint = "XRecordDisableContext")]
    private static partial int XRecordDisableContext(nint display, ulong context);

    [LibraryImport(XtstLib, EntryPoint = "XRecordFreeContext")]
    private static partial int XRecordFreeContext(nint display, ulong context);

    // Instance fields
    private nint controlDisplay;
    private nint dataDisplay;
    private ulong recordContext;
    private Thread? recordThread;
    private CancellationTokenSource? cts;
    private bool isStarted;
    private XRecordInterceptProc? callbackDelegate;

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

        // We need two display connections for XRecord:
        // - controlDisplay: for controlling the recording (enable/disable)
        // - dataDisplay: for receiving the recorded data (blocks in XRecordEnableContext)
        this.controlDisplay = XOpenDisplay(nint.Zero);
        this.dataDisplay = XOpenDisplay(nint.Zero);

        if (this.controlDisplay == nint.Zero || this.dataDisplay == nint.Zero)
        {
            this.Cleanup();
            return;
        }

        // Check if XRecord extension is available
        if (!XRecordQueryVersion(this.controlDisplay, out _, out _))
        {
            this.Cleanup();
            return;
        }

        // Create a record range for keyboard events only
        var rangePtr = XRecordAllocRange();
        if (rangePtr == nint.Zero)
        {
            this.Cleanup();
            return;
        }

        // Set up the range to capture keyboard events (KeyPress and KeyRelease)
        var range = Marshal.PtrToStructure<XRecordRange>(rangePtr);
        range.device_events.first = KeyPress;
        range.device_events.last = KeyRelease;
        Marshal.StructureToPtr(range, rangePtr, false);

        // Create client spec for all clients
        var clientSpec = new nint[1];
        clientSpec[0] = XRecordAllClients;

        var ranges = new nint[1];
        ranges[0] = rangePtr;

        // Create the record context on the DATA display (not control)
        // The context must be created and enabled on the same display
        this.recordContext = XRecordCreateContext(
            this.dataDisplay,
            0,
            clientSpec,
            1,
            ranges,
            1);

        XFree(rangePtr);

        if (this.recordContext == 0)
        {
            this.Cleanup();
            return;
        }

        // Keep a reference to prevent GC
        this.callbackDelegate = this.RecordCallback;

        this.cts = new CancellationTokenSource();
        this.recordThread = new Thread(this.RecordLoop)
        {
            IsBackground = true,
            Name = "X11 XRecord Event Loop"
        };
        this.recordThread.Start();

        this.isStarted = true;
    }

    public void Stop()
    {
        if (!this.isStarted)
        {
            return;
        }

        this.cts?.Cancel();

        // Disable the context to unblock XRecordEnableContext
        if (this.controlDisplay != nint.Zero && this.recordContext != 0)
        {
            XRecordDisableContext(this.controlDisplay, this.recordContext);
            XFlush(this.controlDisplay);
        }

        this.recordThread?.Join(TimeSpan.FromSeconds(2));

        this.Cleanup();
        this.isStarted = false;
    }

    private void Cleanup()
    {
        if (this.controlDisplay != nint.Zero && this.recordContext != 0)
        {
            XRecordFreeContext(this.controlDisplay, this.recordContext);
            this.recordContext = 0;
        }

        if (this.dataDisplay != nint.Zero)
        {
            XCloseDisplay(this.dataDisplay);
            this.dataDisplay = nint.Zero;
        }

        if (this.controlDisplay != nint.Zero)
        {
            XCloseDisplay(this.controlDisplay);
            this.controlDisplay = nint.Zero;
        }

        this.cts?.Dispose();
        this.cts = null;
        this.callbackDelegate = null;
    }

    public void Dispose()
    {
        this.Stop();
    }

    private void RecordLoop()
    {
        // This call blocks until XRecordDisableContext is called
        XRecordEnableContext(this.dataDisplay, this.recordContext, this.callbackDelegate!, nint.Zero);
    }

    private void RecordCallback(nint closure, ref XRecordInterceptData data)
    {
        if (data.category != XRecordFromServer || data.data == nint.Zero)
        {
            return;
        }

        // The data contains the raw X11 event
        // First byte is the event type, second byte is the keycode
        var eventType = Marshal.ReadByte(data.data, 0);
        var keycode = Marshal.ReadByte(data.data, 1);

        if (eventType is not KeyPress and not KeyRelease)
        {
            return;
        }

        // Convert keycode to keysym
        var keysym = XKeycodeToKeysym(this.controlDisplay, keycode, 0);

        // Only process F1-F12 keys
        if (!TryMapXKeyToVirtualKey(keysym, out var virtualKey))
        {
            return;
        }

        if (eventType == KeyPress)
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
        // Only map F1-F12 - these are the only keys we need to detect
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
            _ => default
        };

        return virtualKey != default;
    }
}
