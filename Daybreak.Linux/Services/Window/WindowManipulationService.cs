using Daybreak.Shared.Services.Window;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;
using System.Runtime.InteropServices;

namespace Daybreak.Linux.Services.Window;

/// <summary>
/// Linux-specific window manipulation service using GTK3, GDK, and X11 APIs.
/// Provides drag-move and resize functionality via the window manager.
/// Uses GTK's toplevel window list or X11 PID lookup to find the application window.
/// </summary>
/// <remarks>
/// This service requires GDK_BACKEND=x11 to be set when running under Wayland compositors.
/// Interactive drag/resize may not work properly under XWayland on some compositors.
/// </remarks>
internal sealed partial class WindowManipulationService(ILogger<WindowManipulationService> logger) : IWindowManipulationService
{
    private const string GtkLib = "libgtk-3.so.0";
    private const string GdkLib = "libgdk-3.so.0";
    private const string GlibLib = "libglib-2.0.so.0";
    private const string X11Lib = "libX11.so.6";

    // GDK window edge constants (from gdktypes.h)
    private const int GDK_WINDOW_EDGE_NORTH_WEST = 0;
    private const int GDK_WINDOW_EDGE_NORTH = 1;
    private const int GDK_WINDOW_EDGE_NORTH_EAST = 2;
    private const int GDK_WINDOW_EDGE_WEST = 3;
    private const int GDK_WINDOW_EDGE_EAST = 4;
    private const int GDK_WINDOW_EDGE_SOUTH_WEST = 5;
    private const int GDK_WINDOW_EDGE_SOUTH = 6;
    private const int GDK_WINDOW_EDGE_SOUTH_EAST = 7;

    private readonly ILogger<WindowManipulationService> logger = logger;
    private readonly int currentPid = Environment.ProcessId;

    private nint cachedGdkWindow = nint.Zero;

    public void DragWindow()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var gdkWindow = this.GetGdkWindow();
        if (gdkWindow == nint.Zero)
        {
            scopedLogger.LogWarning("No GdkWindow found for current process. Drag move will not work.");
            return;
        }

        var display = gdk_window_get_display(gdkWindow);
        var seat = gdk_display_get_default_seat(display);
        var device = gdk_seat_get_pointer(seat);

        // Get root (screen) coordinates, not window-relative coordinates
        gdk_device_get_position(device, out _, out var rootX, out var rootY);
        var timestamp = GetCurrentEventTime();

        scopedLogger.LogDebug("DragWindow: root position=({rootX}, {rootY}), timestamp={timestamp}", rootX, rootY, timestamp);
        gdk_window_begin_move_drag_for_device(gdkWindow, device, 1, rootX, rootY, timestamp);
    }

    public void ResizeWindow(ResizeDirection direction)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var gdkWindow = this.GetGdkWindow();
        if (gdkWindow == nint.Zero)
        {
            scopedLogger.LogWarning("No GdkWindow found for current process. Resize will not work.");
            return;
        }

        var display = gdk_window_get_display(gdkWindow);
        var seat = gdk_display_get_default_seat(display);
        var device = gdk_seat_get_pointer(seat);

        // Get root (screen) coordinates, not window-relative coordinates
        gdk_device_get_position(device, out _, out var rootX, out var rootY);
        var timestamp = GetCurrentEventTime();

        var edge = MapResizeDirectionToGdkEdge(direction);
        scopedLogger.LogDebug("ResizeWindow: edge={edge}, root position=({rootX}, {rootY}), timestamp={timestamp}", edge, rootX, rootY, timestamp);

        gdk_window_begin_resize_drag_for_device(gdkWindow, edge, device, 1, rootX, rootY, timestamp);
    }

    /// <summary>
    /// Gets the GdkWindow for the current process by finding the X11 window
    /// that belongs to this process ID, then converting it to a GdkWindow.
    /// </summary>
    private nint GetGdkWindow()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        // Return cached window if still valid
        if (this.cachedGdkWindow != nint.Zero)
        {
            return this.cachedGdkWindow;
        }

        // First, try to get the window from GTK's toplevel list (most reliable for current process)
        var gtkWindow = FindGtkWindowForCurrentProcess();
        if (gtkWindow != nint.Zero)
        {
            var gdkWindow = gtk_widget_get_window(gtkWindow);
            if (gdkWindow != nint.Zero)
            {
                this.cachedGdkWindow = gdkWindow;
                return gdkWindow;
            }
        }

        // Fallback: Find via X11 by PID
        var x11Window = FindX11WindowByPid(this.currentPid);
        if (x11Window != 0)
        {
            var display = gdk_display_get_default();
            if (display != nint.Zero)
            {
                var gdkWindow = gdk_x11_window_foreign_new_for_display(display, x11Window);
                if (gdkWindow != nint.Zero)
                {
                    this.cachedGdkWindow = gdkWindow;
                    return gdkWindow;
                }
            }
        }

        scopedLogger.LogWarning("Failed to find GdkWindow for current process. Drag and resize will not work.");
        return nint.Zero;
    }

    /// <summary>
    /// Finds the GTK window belonging to the current process using gtk_window_list_toplevels.
    /// </summary>
    private static nint FindGtkWindowForCurrentProcess()
    {
        var list = gtk_window_list_toplevels();
        if (list == nint.Zero)
        {
            return nint.Zero;
        }

        var current = list;
        nint result = nint.Zero;

        while (current != nint.Zero)
        {
            var window = G_list_data(current);
            if (window != nint.Zero && gtk_widget_get_visible(window))
            {
                // Return the first visible toplevel window (Photino creates one main window)
                result = window;
                break;
            }
            current = G_list_next(current);
        }

        g_list_free(list);
        return result;
    }

    /// <summary>
    /// Finds an X11 window by process ID by traversing the window tree.
    /// </summary>
    private static ulong FindX11WindowByPid(int targetPid)
    {
        var display = XOpenDisplay(nint.Zero);
        if (display == nint.Zero)
        {
            return 0;
        }

        try
        {
            var rootWindow = XDefaultRootWindow(display);
            return FindWindowByPidRecursive(display, rootWindow, targetPid);
        }
        finally
        {
            XCloseDisplay(display);
        }
    }

    private static ulong FindWindowByPidRecursive(nint display, ulong window, int targetPid)
    {
        // Check if this window belongs to our PID
        var pid = GetWindowPid(display, window);
        if (pid == targetPid)
        {
            // Verify it's a real application window (has WM_STATE property)
            if (HasWmState(display, window))
            {
                return window;
            }
        }

        // Recurse into children
        if (XQueryTree(display, window, out _, out _, out var children, out var nChildren) != 0 && children != nint.Zero)
        {
            try
            {
                var childArray = new ulong[nChildren];
                Marshal.Copy(children, (long[])(object)childArray, 0, (int)nChildren);

                foreach (var child in childArray)
                {
                    var result = FindWindowByPidRecursive(display, child, targetPid);
                    if (result != 0)
                    {
                        return result;
                    }
                }
            }
            finally
            {
                XFree(children);
            }
        }

        return 0;
    }

    private static int GetWindowPid(nint display, ulong window)
    {
        var pidAtom = XInternAtom(display, "_NET_WM_PID", true);
        if (pidAtom == 0)
        {
            return -1;
        }

        var result = XGetWindowProperty(
            display, window, pidAtom, 0, 1, false, 6 /* XA_CARDINAL */,
            out _, out _, out var nitems, out _, out var data);

        if (result != 0 || nitems == 0 || data == nint.Zero)
        {
            if (data != nint.Zero) XFree(data);
            return -1;
        }

        var pid = Marshal.ReadInt32(data);
        XFree(data);
        return pid;
    }

    private static bool HasWmState(nint display, ulong window)
    {
        var wmStateAtom = XInternAtom(display, "WM_STATE", true);
        if (wmStateAtom == 0)
        {
            return false;
        }

        var result = XGetWindowProperty(
            display, window, wmStateAtom, 0, 0, false, 0 /* AnyPropertyType */,
            out var actualType, out _, out _, out _, out var data);

        if (data != nint.Zero) XFree(data);
        return result == 0 && actualType != 0;
    }

    private static int MapResizeDirectionToGdkEdge(ResizeDirection direction)
    {
        return direction switch
        {
            ResizeDirection.Left => GDK_WINDOW_EDGE_WEST,
            ResizeDirection.Right => GDK_WINDOW_EDGE_EAST,
            ResizeDirection.Top => GDK_WINDOW_EDGE_NORTH,
            ResizeDirection.TopLeft => GDK_WINDOW_EDGE_NORTH_WEST,
            ResizeDirection.TopRight => GDK_WINDOW_EDGE_NORTH_EAST,
            ResizeDirection.Bottom => GDK_WINDOW_EDGE_SOUTH,
            ResizeDirection.BottomLeft => GDK_WINDOW_EDGE_SOUTH_WEST,
            ResizeDirection.BottomRight => GDK_WINDOW_EDGE_SOUTH_EAST,
            _ => GDK_WINDOW_EDGE_SOUTH_EAST
        };
    }

    private static uint GetCurrentEventTime()
    {
        var currentEvent = gtk_get_current_event();
        if (currentEvent != nint.Zero)
        {
            var time = gdk_event_get_time(currentEvent);
            gdk_event_free(currentEvent);
            return time;
        }

        // GDK_CURRENT_TIME = 0
        return 0;
    }

    // GTK3 P/Invoke declarations
    [LibraryImport(GtkLib, EntryPoint = "gtk_widget_get_window")]
    private static partial nint gtk_widget_get_window(nint widget);

    [LibraryImport(GtkLib, EntryPoint = "gtk_widget_get_visible")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool gtk_widget_get_visible(nint widget);

    [LibraryImport(GtkLib, EntryPoint = "gtk_get_current_event")]
    private static partial nint gtk_get_current_event();

    [LibraryImport(GtkLib, EntryPoint = "gtk_window_list_toplevels")]
    private static partial nint gtk_window_list_toplevels();

    // GLib P/Invoke declarations
    [LibraryImport(GlibLib, EntryPoint = "g_list_free")]
    private static partial void g_list_free(nint list);

    private static nint G_list_data(nint list) =>
        list != nint.Zero ? Marshal.ReadIntPtr(list) : nint.Zero;
    
    private static nint G_list_next(nint list) =>
        list != nint.Zero ? Marshal.ReadIntPtr(list + nint.Size) : nint.Zero;

    // GDK3 P/Invoke declarations
    [LibraryImport(GdkLib, EntryPoint = "gdk_display_get_default")]
    private static partial nint gdk_display_get_default();

    [LibraryImport(GdkLib, EntryPoint = "gdk_window_get_display")]
    private static partial nint gdk_window_get_display(nint window);

    [LibraryImport(GdkLib, EntryPoint = "gdk_display_get_default_seat")]
    private static partial nint gdk_display_get_default_seat(nint display);

    [LibraryImport(GdkLib, EntryPoint = "gdk_seat_get_pointer")]
    private static partial nint gdk_seat_get_pointer(nint seat);

    [LibraryImport(GdkLib, EntryPoint = "gdk_device_get_position")]
    private static partial void gdk_device_get_position(
        nint device,
        out nint screen,
        out int x,
        out int y);

    [LibraryImport(GdkLib, EntryPoint = "gdk_window_begin_move_drag_for_device")]
    private static partial void gdk_window_begin_move_drag_for_device(
        nint window,
        nint device,
        int button,
        int rootX,
        int rootY,
        uint timestamp);

    [LibraryImport(GdkLib, EntryPoint = "gdk_window_begin_resize_drag_for_device")]
    private static partial void gdk_window_begin_resize_drag_for_device(
        nint window,
        int edge,
        nint device,
        int button,
        int rootX,
        int rootY,
        uint timestamp);

    [LibraryImport(GdkLib, EntryPoint = "gdk_event_get_time")]
    private static partial uint gdk_event_get_time(nint gdkEvent);

    [LibraryImport(GdkLib, EntryPoint = "gdk_event_free")]
    private static partial void gdk_event_free(nint gdkEvent);

    [LibraryImport("libgdk-3.so.0", EntryPoint = "gdk_x11_window_foreign_new_for_display")]
    private static partial nint gdk_x11_window_foreign_new_for_display(nint display, ulong window);

    // X11 P/Invoke declarations
    [LibraryImport(X11Lib, EntryPoint = "XOpenDisplay")]
    private static partial nint XOpenDisplay(nint displayName);

    [LibraryImport(X11Lib, EntryPoint = "XCloseDisplay")]
    private static partial int XCloseDisplay(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XDefaultRootWindow")]
    private static partial ulong XDefaultRootWindow(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XQueryTree")]
    private static partial int XQueryTree(
        nint display,
        ulong window,
        out ulong rootReturn,
        out ulong parentReturn,
        out nint childrenReturn,
        out uint nChildrenReturn);

    [LibraryImport(X11Lib, EntryPoint = "XFree")]
    private static partial int XFree(nint data);

    [LibraryImport(X11Lib, EntryPoint = "XInternAtom", StringMarshalling = StringMarshalling.Utf8)]
    private static partial ulong XInternAtom(nint display, string atomName, [MarshalAs(UnmanagedType.Bool)] bool onlyIfExists);

    [LibraryImport(X11Lib, EntryPoint = "XGetWindowProperty")]
    private static partial int XGetWindowProperty(
        nint display,
        ulong window,
        ulong property,
        long longOffset,
        long longLength,
        [MarshalAs(UnmanagedType.Bool)] bool delete,
        ulong reqType,
        out ulong actualTypeReturn,
        out int actualFormatReturn,
        out ulong nitemsReturn,
        out ulong bytesAfterReturn,
        out nint propReturn);
}
