using System.Runtime.InteropServices;

namespace Daybreak.Linux.Utils;

/// <summary>
/// Linux-specific native method declarations for X11, GTK3, GDK3, GLib, and XRecord APIs.
/// </summary>
public static partial class NativeMethods
{
    #region Library Names

    public const string X11Lib = "libX11.so.6";
    public const string XtstLib = "libXtst.so.6";
    public const string GtkLib = "libgtk-3.so.0";
    public const string GdkLib = "libgdk-3.so.0";
    public const string GlibLib = "libglib-2.0.so.0";

    #endregion

    #region X11 Constants

    // X11 event types
    public const int KeyPress = 2;
    public const int KeyRelease = 3;

    // XRecord constants
    public const int XRecordFromServer = 0;
    public const int XRecordAllClients = 3;

    // XK key codes (from X11/keysymdef.h)
    public const uint XK_F1 = 0xffbe;
    public const uint XK_F2 = 0xffbf;
    public const uint XK_F3 = 0xffc0;
    public const uint XK_F4 = 0xffc1;
    public const uint XK_F5 = 0xffc2;
    public const uint XK_F6 = 0xffc3;
    public const uint XK_F7 = 0xffc4;
    public const uint XK_F8 = 0xffc5;
    public const uint XK_F9 = 0xffc6;
    public const uint XK_F10 = 0xffc7;
    public const uint XK_F11 = 0xffc8;
    public const uint XK_F12 = 0xffc9;

    #endregion

    #region GDK Constants

    // GDK window edge constants (from gdktypes.h)
    public const int GDK_WINDOW_EDGE_NORTH_WEST = 0;
    public const int GDK_WINDOW_EDGE_NORTH = 1;
    public const int GDK_WINDOW_EDGE_NORTH_EAST = 2;
    public const int GDK_WINDOW_EDGE_WEST = 3;
    public const int GDK_WINDOW_EDGE_EAST = 4;
    public const int GDK_WINDOW_EDGE_SOUTH_WEST = 5;
    public const int GDK_WINDOW_EDGE_SOUTH = 6;
    public const int GDK_WINDOW_EDGE_SOUTH_EAST = 7;

    #endregion

    #region XRecord Structures

    [StructLayout(LayoutKind.Sequential)]
    public struct XRecordRange
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
    public struct XRecordRange8
    {
        public byte first;
        public byte last;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XRecordExtRange
    {
        public XRecordRange16 ext_major;
        public XRecordRange8 ext_minor;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XRecordRange16
    {
        public ushort first;
        public ushort last;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XRecordInterceptData
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

    #endregion

    #region Delegates

    /// <summary>
    /// Callback delegate for XRecordEnableContext.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void XRecordInterceptProc(nint closure, ref XRecordInterceptData recorded_data);

    #endregion

    #region X11 P/Invoke

    [LibraryImport(X11Lib, EntryPoint = "XOpenDisplay")]
    public static partial nint XOpenDisplay(nint display_name);

    [LibraryImport(X11Lib, EntryPoint = "XCloseDisplay")]
    public static partial int XCloseDisplay(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XFlush")]
    public static partial int XFlush(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XKeycodeToKeysym")]
    public static partial uint XKeycodeToKeysym(nint display, byte keycode, int index);

    [LibraryImport(X11Lib, EntryPoint = "XFree")]
    public static partial int XFree(nint data);

    [LibraryImport(X11Lib, EntryPoint = "XGetInputFocus")]
    public static partial int XGetInputFocus(nint display, out ulong focus_return, out int revert_to_return);

    [LibraryImport(X11Lib, EntryPoint = "XQueryTree")]
    public static partial int XQueryTree(
        nint display,
        ulong window,
        out ulong root_return,
        out ulong parent_return,
        out nint children_return,
        out uint nchildren_return);

    [LibraryImport(X11Lib, EntryPoint = "XDefaultRootWindow")]
    public static partial ulong XDefaultRootWindow(nint display);

    [LibraryImport(X11Lib, EntryPoint = "XInternAtom", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ulong XInternAtom(nint display, string atomName, [MarshalAs(UnmanagedType.Bool)] bool onlyIfExists);

    [LibraryImport(X11Lib, EntryPoint = "XGetWindowProperty")]
    public static partial int XGetWindowProperty(
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

    #endregion

    #region XRecord P/Invoke

    [LibraryImport(XtstLib, EntryPoint = "XRecordQueryVersion")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool XRecordQueryVersion(nint display, out int major, out int minor);

    [LibraryImport(XtstLib, EntryPoint = "XRecordAllocRange")]
    public static partial nint XRecordAllocRange();

    [LibraryImport(XtstLib, EntryPoint = "XRecordCreateContext")]
    public static partial ulong XRecordCreateContext(
        nint display,
        int flags,
        nint[] client_specs,
        int nclients,
        nint[] ranges,
        int nranges);

    [LibraryImport(XtstLib, EntryPoint = "XRecordEnableContext")]
    public static partial int XRecordEnableContext(
        nint display,
        ulong context,
        XRecordInterceptProc callback,
        nint closure);

    [LibraryImport(XtstLib, EntryPoint = "XRecordDisableContext")]
    public static partial int XRecordDisableContext(nint display, ulong context);

    [LibraryImport(XtstLib, EntryPoint = "XRecordFreeContext")]
    public static partial int XRecordFreeContext(nint display, ulong context);

    #endregion

    #region GTK3 P/Invoke

    [LibraryImport(GtkLib, EntryPoint = "gtk_widget_get_window")]
    public static partial nint gtk_widget_get_window(nint widget);

    [LibraryImport(GtkLib, EntryPoint = "gtk_widget_get_visible")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool gtk_widget_get_visible(nint widget);

    [LibraryImport(GtkLib, EntryPoint = "gtk_get_current_event")]
    public static partial nint gtk_get_current_event();

    [LibraryImport(GtkLib, EntryPoint = "gtk_window_list_toplevels")]
    public static partial nint gtk_window_list_toplevels();

    #endregion

    #region GLib P/Invoke

    [LibraryImport(GlibLib, EntryPoint = "g_list_free")]
    public static partial void g_list_free(nint list);

    /// <summary>
    /// Gets the data pointer from a GList node.
    /// </summary>
    public static nint G_list_data(nint list) =>
        list != nint.Zero ? Marshal.ReadIntPtr(list) : nint.Zero;

    /// <summary>
    /// Gets the next node pointer from a GList node.
    /// </summary>
    public static nint G_list_next(nint list) =>
        list != nint.Zero ? Marshal.ReadIntPtr(list + nint.Size) : nint.Zero;

    #endregion

    #region GDK3 P/Invoke

    [LibraryImport(GdkLib, EntryPoint = "gdk_display_get_default")]
    public static partial nint gdk_display_get_default();

    [LibraryImport(GdkLib, EntryPoint = "gdk_window_get_display")]
    public static partial nint gdk_window_get_display(nint window);

    [LibraryImport(GdkLib, EntryPoint = "gdk_display_get_default_seat")]
    public static partial nint gdk_display_get_default_seat(nint display);

    [LibraryImport(GdkLib, EntryPoint = "gdk_seat_get_pointer")]
    public static partial nint gdk_seat_get_pointer(nint seat);

    [LibraryImport(GdkLib, EntryPoint = "gdk_device_get_position")]
    public static partial void gdk_device_get_position(
        nint device,
        out nint screen,
        out int x,
        out int y);

    [LibraryImport(GdkLib, EntryPoint = "gdk_window_begin_move_drag_for_device")]
    public static partial void gdk_window_begin_move_drag_for_device(
        nint window,
        nint device,
        int button,
        int rootX,
        int rootY,
        uint timestamp);

    [LibraryImport(GdkLib, EntryPoint = "gdk_window_begin_resize_drag_for_device")]
    public static partial void gdk_window_begin_resize_drag_for_device(
        nint window,
        int edge,
        nint device,
        int button,
        int rootX,
        int rootY,
        uint timestamp);

    [LibraryImport(GdkLib, EntryPoint = "gdk_event_get_time")]
    public static partial uint gdk_event_get_time(nint gdkEvent);

    [LibraryImport(GdkLib, EntryPoint = "gdk_event_free")]
    public static partial void gdk_event_free(nint gdkEvent);

    [LibraryImport(GdkLib, EntryPoint = "gdk_x11_window_foreign_new_for_display")]
    public static partial nint gdk_x11_window_foreign_new_for_display(nint display, ulong window);

    [LibraryImport(GdkLib, EntryPoint = "gdk_x11_window_get_xid")]
    public static partial ulong gdk_x11_window_get_xid(nint gdkWindow);

    #endregion
}
