using Daybreak.Linux.Configuration;
using System.Diagnostics;

namespace Daybreak.Linux;

public static partial class Launcher
{
    public static void Main(string[] args)
    {
        ConfigureEnvironment();

        var bootstrap = Launch.Launcher.SetupBootstrap();
        var platformConfiguration = new LinuxPlatformConfiguration();
        Launch.Launcher.LaunchSequence(args, bootstrap, platformConfiguration);
    }

    /// <summary>
    /// Configures environment variables needed for WebKitGTK/Photino to work
    /// reliably across different Linux display servers and GPU drivers.
    /// Variables are only set when not already defined by the user, so they
    /// can always be overridden externally.
    /// </summary>
    private static void ConfigureEnvironment()
    {
        // These environment variables must ideally be set BEFORE the process
        // starts (e.g. via the launch wrapper script) so that native libraries
        // like Mesa/EGL and libwayland see them at load time. Setting them here
        // serves as a fallback for cases where the wrapper isn't used, but may
        // not prevent all Wayland-related crashes.

        // WebKitGTK's DMA-BUF renderer can crash or produce artifacts on
        // certain GPU driver combinations (especially Nvidia proprietary).
        SetIfUnset("WEBKIT_DISABLE_DMABUF_RENDERER", "1");

        // GPU-accelerated compositing in WebKitGTK can cause blank windows
        // or crashes with some Mesa/Nvidia drivers.
        SetIfUnset("WEBKIT_DISABLE_COMPOSITING_MODE", "1");

        // Photino (via WebKitGTK + GTK3) does not fully support native Wayland.
        // Force X11 (via XWayland on Wayland sessions).
        SetIfUnset("GDK_BACKEND", "x11");
    }

    private static void SetIfUnset(string variable, string value)
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variable)))
        {
            Environment.SetEnvironmentVariable(variable, value);
        }
    }
}
