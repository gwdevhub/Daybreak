using Daybreak.Linux.Utils;
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
internal sealed class WindowManipulationService(ILogger<WindowManipulationService> logger) : IWindowManipulationService
{
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

        var display = NativeMethods.gdk_window_get_display(gdkWindow);
        var seat = NativeMethods.gdk_display_get_default_seat(display);
        var device = NativeMethods.gdk_seat_get_pointer(seat);

        // Get root (screen) coordinates, not window-relative coordinates
        NativeMethods.gdk_device_get_position(device, out _, out var rootX, out var rootY);
        var timestamp = GetCurrentEventTime();

        scopedLogger.LogDebug("DragWindow: root position=({rootX}, {rootY}), timestamp={timestamp}", rootX, rootY, timestamp);
        NativeMethods.gdk_window_begin_move_drag_for_device(gdkWindow, device, 1, rootX, rootY, timestamp);
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

        var display = NativeMethods.gdk_window_get_display(gdkWindow);
        var seat = NativeMethods.gdk_display_get_default_seat(display);
        var device = NativeMethods.gdk_seat_get_pointer(seat);

        // Get root (screen) coordinates, not window-relative coordinates
        NativeMethods.gdk_device_get_position(device, out _, out var rootX, out var rootY);
        var timestamp = GetCurrentEventTime();

        var edge = MapResizeDirectionToGdkEdge(direction);
        scopedLogger.LogDebug("ResizeWindow: edge={edge}, root position=({rootX}, {rootY}), timestamp={timestamp}", edge, rootX, rootY, timestamp);

        NativeMethods.gdk_window_begin_resize_drag_for_device(gdkWindow, edge, device, 1, rootX, rootY, timestamp);
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
            var gdkWindow = NativeMethods.gtk_widget_get_window(gtkWindow);
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
            var display = NativeMethods.gdk_display_get_default();
            if (display != nint.Zero)
            {
                var gdkWindow = NativeMethods.gdk_x11_window_foreign_new_for_display(display, x11Window);
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

    /// <summary>
    /// Finds an X11 window by process ID by traversing the window tree.
    /// </summary>
    private static ulong FindX11WindowByPid(int targetPid)
    {
        var display = NativeMethods.XOpenDisplay(nint.Zero);
        if (display == nint.Zero)
        {
            return 0;
        }

        try
        {
            var rootWindow = NativeMethods.XDefaultRootWindow(display);
            return FindWindowByPidRecursive(display, rootWindow, targetPid);
        }
        finally
        {
            NativeMethods.XCloseDisplay(display);
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
        if (NativeMethods.XQueryTree(display, window, out _, out _, out var children, out var nChildren) != 0 && children != nint.Zero)
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
                NativeMethods.XFree(children);
            }
        }

        return 0;
    }

    private static int GetWindowPid(nint display, ulong window)
    {
        var pidAtom = NativeMethods.XInternAtom(display, "_NET_WM_PID", true);
        if (pidAtom == 0)
        {
            return -1;
        }

        var result = NativeMethods.XGetWindowProperty(
            display, window, pidAtom, 0, 1, false, 6 /* XA_CARDINAL */,
            out _, out _, out var nitems, out _, out var data);

        if (result != 0 || nitems == 0 || data == nint.Zero)
        {
            if (data != nint.Zero) NativeMethods.XFree(data);
            return -1;
        }

        var pid = Marshal.ReadInt32(data);
        NativeMethods.XFree(data);
        return pid;
    }

    private static bool HasWmState(nint display, ulong window)
    {
        var wmStateAtom = NativeMethods.XInternAtom(display, "WM_STATE", true);
        if (wmStateAtom == 0)
        {
            return false;
        }

        var result = NativeMethods.XGetWindowProperty(
            display, window, wmStateAtom, 0, 0, false, 0 /* AnyPropertyType */,
            out var actualType, out _, out _, out _, out var data);

        if (data != nint.Zero) NativeMethods.XFree(data);
        return result == 0 && actualType != 0;
    }

    private static int MapResizeDirectionToGdkEdge(ResizeDirection direction)
    {
        return direction switch
        {
            ResizeDirection.Left => NativeMethods.GDK_WINDOW_EDGE_WEST,
            ResizeDirection.Right => NativeMethods.GDK_WINDOW_EDGE_EAST,
            ResizeDirection.Top => NativeMethods.GDK_WINDOW_EDGE_NORTH,
            ResizeDirection.TopLeft => NativeMethods.GDK_WINDOW_EDGE_NORTH_WEST,
            ResizeDirection.TopRight => NativeMethods.GDK_WINDOW_EDGE_NORTH_EAST,
            ResizeDirection.Bottom => NativeMethods.GDK_WINDOW_EDGE_SOUTH,
            ResizeDirection.BottomLeft => NativeMethods.GDK_WINDOW_EDGE_SOUTH_WEST,
            ResizeDirection.BottomRight => NativeMethods.GDK_WINDOW_EDGE_SOUTH_EAST,
            _ => NativeMethods.GDK_WINDOW_EDGE_SOUTH_EAST
        };
    }

    private static uint GetCurrentEventTime()
    {
        var currentEvent = NativeMethods.gtk_get_current_event();
        if (currentEvent != nint.Zero)
        {
            var time = NativeMethods.gdk_event_get_time(currentEvent);
            NativeMethods.gdk_event_free(currentEvent);
            return time;
        }

        // GDK_CURRENT_TIME = 0
        return 0;
    }
}
