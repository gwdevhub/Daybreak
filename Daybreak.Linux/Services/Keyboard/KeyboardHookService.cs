using Daybreak.Linux.Utils;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Keyboard;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Daybreak.Linux.Services.Keyboard;

public sealed class KeyboardHookService : IHostedService, IKeyboardHookService, IDisposable
{
    private nint controlDisplay;
    private nint dataDisplay;
    private ulong recordContext;
    private Task? recordTask;
    private CancellationTokenSource? cts;
    private bool isStarted;
    private NativeMethods.XRecordInterceptProc? callbackDelegate;
    private ulong appWindowId;

    public event EventHandler<KeyboardEventArgs>? KeyDown;
    public event EventHandler<KeyboardEventArgs>? KeyUp;

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        this.appWindowId = FindAppWindowId();
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
        this.controlDisplay = NativeMethods.XOpenDisplay(nint.Zero);
        this.dataDisplay = NativeMethods.XOpenDisplay(nint.Zero);

        if (this.controlDisplay == nint.Zero || this.dataDisplay == nint.Zero)
        {
            this.Cleanup();
            return;
        }

        // Check if XRecord extension is available
        if (!NativeMethods.XRecordQueryVersion(this.controlDisplay, out _, out _))
        {
            this.Cleanup();
            return;
        }

        // Create a record range for keyboard events only
        var rangePtr = NativeMethods.XRecordAllocRange();
        if (rangePtr == nint.Zero)
        {
            this.Cleanup();
            return;
        }

        // Set up the range to capture keyboard events (KeyPress and KeyRelease)
        var range = Marshal.PtrToStructure<NativeMethods.XRecordRange>(rangePtr);
        range.device_events.first = NativeMethods.KeyPress;
        range.device_events.last = NativeMethods.KeyRelease;
        Marshal.StructureToPtr(range, rangePtr, false);

        // Create client spec for all clients
        var clientSpec = new nint[1];
        clientSpec[0] = NativeMethods.XRecordAllClients;

        var ranges = new nint[1];
        ranges[0] = rangePtr;

        // Create the record context on the DATA display (not control)
        // The context must be created and enabled on the same display
        this.recordContext = NativeMethods.XRecordCreateContext(
            this.dataDisplay,
            0,
            clientSpec,
            1,
            ranges,
            1);

        NativeMethods.XFree(rangePtr);

        if (this.recordContext == 0)
        {
            this.Cleanup();
            return;
        }

        // Keep a reference to prevent GC
        this.callbackDelegate = this.RecordCallback;

        this.cts = new CancellationTokenSource();
        this.recordTask = Task.Factory.StartNew(
            this.RecordLoop,
            this.cts.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);

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
            NativeMethods.XRecordDisableContext(this.controlDisplay, this.recordContext);
            NativeMethods.XFlush(this.controlDisplay);
        }

        this.recordTask = null;

        this.Cleanup();
        this.isStarted = false;
    }

    private void Cleanup()
    {
        if (this.controlDisplay != nint.Zero && this.recordContext != 0)
        {
            NativeMethods.XRecordFreeContext(this.controlDisplay, this.recordContext);
            this.recordContext = 0;
        }

        if (this.dataDisplay != nint.Zero)
        {
            NativeMethods.XCloseDisplay(this.dataDisplay);
            this.dataDisplay = nint.Zero;
        }

        if (this.controlDisplay != nint.Zero)
        {
            NativeMethods.XCloseDisplay(this.controlDisplay);
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
        NativeMethods.XRecordEnableContext(this.dataDisplay, this.recordContext, this.callbackDelegate!, nint.Zero);
    }

    private void RecordCallback(nint closure, ref NativeMethods.XRecordInterceptData data)
    {
        if (data.category != NativeMethods.XRecordFromServer || data.data == nint.Zero)
        {
            return;
        }

        // Only process events when our app window is focused
        if (!this.IsAppWindowFocused())
        {
            return;
        }

        // The data contains the raw X11 event
        // First byte is the event type, second byte is the keycode
        var eventType = Marshal.ReadByte(data.data, 0);
        var keycode = Marshal.ReadByte(data.data, 1);

        if (eventType is not NativeMethods.KeyPress and not NativeMethods.KeyRelease)
        {
            return;
        }

        // Convert keycode to keysym
        var keysym = NativeMethods.XKeycodeToKeysym(this.controlDisplay, keycode, 0);

        // Only process F1-F12 keys
        if (!TryMapXKeyToVirtualKey(keysym, out var virtualKey))
        {
            return;
        }

        if (eventType == NativeMethods.KeyPress)
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
            NativeMethods.XK_F1 => VirtualKey.F1,
            NativeMethods.XK_F2 => VirtualKey.F2,
            NativeMethods.XK_F3 => VirtualKey.F3,
            NativeMethods.XK_F4 => VirtualKey.F4,
            NativeMethods.XK_F5 => VirtualKey.F5,
            NativeMethods.XK_F6 => VirtualKey.F6,
            NativeMethods.XK_F7 => VirtualKey.F7,
            NativeMethods.XK_F8 => VirtualKey.F8,
            NativeMethods.XK_F9 => VirtualKey.F9,
            NativeMethods.XK_F10 => VirtualKey.F10,
            NativeMethods.XK_F11 => VirtualKey.F11,
            NativeMethods.XK_F12 => VirtualKey.F12,
            _ => default
        };

        return virtualKey != default;
    }

    /// <summary>
    /// Checks if the application window is currently focused.
    /// </summary>
    private bool IsAppWindowFocused()
    {
        if (this.controlDisplay == nint.Zero)
        {
            return false;
        }

        // Lazily get app window ID if not yet cached
        if (this.appWindowId == 0)
        {
            this.appWindowId = FindAppWindowId();
            if (this.appWindowId == 0)
            {
                return false;
            }
        }

        if (NativeMethods.XGetInputFocus(this.controlDisplay, out var focusedWindow, out _) == 0)
        {
            return false;
        }

        // Check if focused window is the app window or a descendant of it
        return this.IsWindowOrDescendant(focusedWindow, this.appWindowId);
    }

    /// <summary>
    /// Checks if the focused window is the target window or a descendant (child) of it.
    /// This handles cases where focus is on a child widget within the main window.
    /// </summary>
    private bool IsWindowOrDescendant(ulong focusedWindow, ulong targetWindow)
    {
        if (focusedWindow == 0)
        {
            return false;
        }

        var current = focusedWindow;
        while (current != 0)
        {
            if (current == targetWindow)
            {
                return true;
            }

            // Get parent of current window
            if (NativeMethods.XQueryTree(this.controlDisplay, current, out var root, out var parent, out var children, out _) == 0)
            {
                break;
            }

            // Free the children list if allocated
            if (children != nint.Zero)
            {
                NativeMethods.XFree(children);
            }

            // Stop if we've reached the root
            if (parent == root || parent == 0)
            {
                break;
            }

            current = parent;
        }

        return false;
    }

    /// <summary>
    /// Finds the X11 window ID for the application using GTK's toplevel window list.
    /// </summary>
    private static ulong FindAppWindowId()
    {
        var gtkWindow = FindGtkWindowForCurrentProcess();
        if (gtkWindow == nint.Zero)
        {
            return 0;
        }

        var gdkWindow = NativeMethods.gtk_widget_get_window(gtkWindow);
        if (gdkWindow == nint.Zero)
        {
            return 0;
        }

        return NativeMethods.gdk_x11_window_get_xid(gdkWindow);
    }

    /// <summary>
    /// Finds the GTK window belonging to the current process using gtk_window_list_toplevels.
    /// </summary>
    private static nint FindGtkWindowForCurrentProcess()
    {
        var list = NativeMethods.gtk_window_list_toplevels();
        if (list == nint.Zero)
        {
            return nint.Zero;
        }

        var current = list;
        nint result = nint.Zero;

        while (current != nint.Zero)
        {
            var window = NativeMethods.G_list_data(current);
            if (window != nint.Zero && NativeMethods.gtk_widget_get_visible(window))
            {
                // Return the first visible toplevel window (Photino creates one main window)
                result = window;
                break;
            }
            current = NativeMethods.G_list_next(current);
        }

        NativeMethods.g_list_free(list);
        return result;
    }
}
