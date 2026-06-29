using Daybreak.Linux.Configuration;
using Daybreak.Linux.Utils;

namespace Daybreak.Linux;

public static partial class Launcher
{
    public static void Main(string[] args)
    {
        ForceX11Backend();

        var bootstrap = Launch.Launcher.SetupBootstrap();
        var platformConfiguration = new LinuxPlatformConfiguration();
        Launch.Launcher.LaunchSequence(args, bootstrap, platformConfiguration);
    }

    /// <summary>
    /// Forces GTK to use the X11 backend (via XWayland under Wayland sessions).
    /// Photino/WebKitGTK/GTK3 and Daybreak's window interop rely on X11-only APIs
    /// (gdk_x11_window_get_xid); without this, GTK selects the Wayland backend on
    /// Wayland sessions and those calls abort with
    /// "gdk_x11_window_get_xid: assertion 'GDK_IS_X11_WINDOW (window)' failed"
    /// followed by a fatal Wayland protocol error (see issue #1551).
    ///
    /// Must run before any GTK initialization (which happens lazily when the first
    /// Photino window is created) and must use the native libc setenv: managed
    /// Environment.SetEnvironmentVariable does not propagate to native getenv,
    /// which is how GTK reads GDK_BACKEND. overwrite=0 lets a user who explicitly
    /// exports GDK_BACKEND (e.g. to test native Wayland) keep their choice.
    /// </summary>
    private static void ForceX11Backend()
    {
        NativeMethods.setenv("GDK_BACKEND", "x11", 0);
    }
}
