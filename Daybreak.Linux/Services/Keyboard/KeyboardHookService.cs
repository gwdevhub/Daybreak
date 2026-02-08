using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Keyboard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Photino.Blazor;
using System.Runtime.InteropServices;

namespace Daybreak.Linux.Services.Keyboard;

public partial class KeyboardHookService(IServiceProvider serviceProvider) : IHostedService, IKeyboardHookService, IDisposable
{
    private const string GtkLib = "libgtk-3.so.0";
    private const string GObjectLib = "libgobject-2.0.so.0";

    // GTK signal names
    private const string KeyPressEvent = "key-press-event";
    private const string KeyReleaseEvent = "key-release-event";

    // GDK key codes (from gdk/gdkkeysyms.h)
    private const uint GDK_KEY_Escape = 0xff1b;
    private const uint GDK_KEY_Return = 0xff0d;
    private const uint GDK_KEY_space = 0x020;
    private const uint GDK_KEY_F1 = 0xffbe;
    private const uint GDK_KEY_F2 = 0xffbf;
    private const uint GDK_KEY_F3 = 0xffc0;
    private const uint GDK_KEY_F4 = 0xffc1;
    private const uint GDK_KEY_F5 = 0xffc2;
    private const uint GDK_KEY_F6 = 0xffc3;
    private const uint GDK_KEY_F7 = 0xffc4;
    private const uint GDK_KEY_F8 = 0xffc5;
    private const uint GDK_KEY_F9 = 0xffc6;
    private const uint GDK_KEY_F10 = 0xffc7;
    private const uint GDK_KEY_F11 = 0xffc8;
    private const uint GDK_KEY_F12 = 0xffc9;

    [StructLayout(LayoutKind.Sequential)]
    private struct GdkEventKey
    {
        public int type;
        public nint window;
        public sbyte send_event;
        public uint time;
        public uint state;
        public uint keyval;
        public int length;
        public nint @string;
        public ushort hardware_keycode;
        public byte group;
        public uint is_modifier;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool GtkKeyEventHandler(nint widget, nint eventKey, nint userData);

    [LibraryImport(GObjectLib, EntryPoint = "g_signal_connect_data")]
    private static partial nuint GSignalConnectData(
        nint instance,
        [MarshalAs(UnmanagedType.LPStr)] string detailed_signal,
        nint c_handler,
        nint data,
        nint destroy_data,
        int connect_flags);

    [LibraryImport(GObjectLib, EntryPoint = "g_signal_handler_disconnect")]
    private static partial void GSignalHandlerDisconnect(nint instance, nuint handler_id);

    [LibraryImport(GtkLib, EntryPoint = "gtk_widget_get_window")]
    private static partial nint GtkWidgetGetWindow(nint widget);

    private readonly IServiceProvider serviceProvider = serviceProvider;

    private nint windowHandle;
    private nuint keyPressHandlerId;
    private nuint keyReleaseHandlerId;
    private GtkKeyEventHandler? keyPressDelegate;
    private GtkKeyEventHandler? keyReleaseDelegate;
    private bool isStarted;

    public event EventHandler<KeyboardEventArgs>? KeyDown;
    public event EventHandler<KeyboardEventArgs>? KeyUp;

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        var app = this.serviceProvider.GetRequiredService<PhotinoBlazorApp>();
        this.windowHandle = app.MainWindow.WindowHandle;
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Start()
    {
        if (this.isStarted || this.windowHandle == nint.Zero)
        {
            return;
        }

        // Keep delegates alive to prevent GC
        this.keyPressDelegate = this.OnKeyPress;
        this.keyReleaseDelegate = this.OnKeyRelease;

        var keyPressPtr = Marshal.GetFunctionPointerForDelegate(this.keyPressDelegate);
        var keyReleasePtr = Marshal.GetFunctionPointerForDelegate(this.keyReleaseDelegate);

        this.keyPressHandlerId = GSignalConnectData(
            this.windowHandle,
            KeyPressEvent,
            keyPressPtr,
            nint.Zero,
            nint.Zero,
            0);

        this.keyReleaseHandlerId = GSignalConnectData(
            this.windowHandle,
            KeyReleaseEvent,
            keyReleasePtr,
            nint.Zero,
            nint.Zero,
            0);

        this.isStarted = true;
    }

    public void Stop()
    {
        if (!this.isStarted || this.windowHandle == nint.Zero)
        {
            return;
        }

        if (this.keyPressHandlerId != 0)
        {
            GSignalHandlerDisconnect(this.windowHandle, this.keyPressHandlerId);
            this.keyPressHandlerId = 0;
        }

        if (this.keyReleaseHandlerId != 0)
        {
            GSignalHandlerDisconnect(this.windowHandle, this.keyReleaseHandlerId);
            this.keyReleaseHandlerId = 0;
        }

        this.keyPressDelegate = null;
        this.keyReleaseDelegate = null;
        this.isStarted = false;
    }

    public void Dispose()
    {
        this.Stop();
    }

    private bool OnKeyPress(nint widget, nint eventKeyPtr, nint userData)
    {
        var eventKey = Marshal.PtrToStructure<GdkEventKey>(eventKeyPtr);
        if (TryMapGdkKeyToVirtualKey(eventKey.keyval, out var virtualKey))
        {
            this.KeyDown?.Invoke(this, new KeyboardEventArgs(virtualKey));
        }

        return false; // Allow event to propagate
    }

    private bool OnKeyRelease(nint widget, nint eventKeyPtr, nint userData)
    {
        var eventKey = Marshal.PtrToStructure<GdkEventKey>(eventKeyPtr);
        if (TryMapGdkKeyToVirtualKey(eventKey.keyval, out var virtualKey))
        {
            this.KeyUp?.Invoke(this, new KeyboardEventArgs(virtualKey));
        }

        return false;
    }

    private static bool TryMapGdkKeyToVirtualKey(uint gdkKey, out VirtualKey virtualKey)
    {
        virtualKey = gdkKey switch
        {
            GDK_KEY_F1 => VirtualKey.F1,
            GDK_KEY_F2 => VirtualKey.F2,
            GDK_KEY_F3 => VirtualKey.F3,
            GDK_KEY_F4 => VirtualKey.F4,
            GDK_KEY_F5 => VirtualKey.F5,
            GDK_KEY_F6 => VirtualKey.F6,
            GDK_KEY_F7 => VirtualKey.F7,
            GDK_KEY_F8 => VirtualKey.F8,
            GDK_KEY_F9 => VirtualKey.F9,
            GDK_KEY_F10 => VirtualKey.F10,
            GDK_KEY_F11 => VirtualKey.F11,
            GDK_KEY_F12 => VirtualKey.F12,
            GDK_KEY_Escape => VirtualKey.Escape,
            GDK_KEY_Return => VirtualKey.Enter,
            GDK_KEY_space => VirtualKey.Space,
            _ => default
        };

        return virtualKey != default;
    }
}
