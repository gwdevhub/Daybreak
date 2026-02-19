using Daybreak.Configuration;
using Daybreak.Extensions;
using Daybreak.Windows.Services.Api;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.Initialization;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Keyboard;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Shortcuts;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Services.SevenZip;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.UMod;
using Daybreak.Shared.Services.ReShade;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.ExceptionHandling;
using Daybreak.Shared.Services.Window;
using Daybreak.Windows.Services.ApplicationLauncher;
using Daybreak.Windows.Services.Credentials;
using Daybreak.Windows.Services.Injection;
using Daybreak.Windows.Services.Keyboard;
using Daybreak.Windows.Services.Monitoring;
using Daybreak.Windows.Services.Privilege;
using Daybreak.Windows.Services.Screens;
using Daybreak.Windows.Services.Shortcuts;
using Daybreak.Windows.Services.SevenZip;
using Daybreak.Windows.Services.UMod;
using Daybreak.Windows.Services.Window;
using Daybreak.Windows.Services.ExceptionHandling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Daybreak.Windows.Services.DirectSong;
using Daybreak.Windows.Services.Registry;
using Daybreak.Windows.Services.Themes;
using Daybreak.Services.ReShade;
using Daybreak.Services.Startup.Actions;
using Daybreak.Windows.Utils;
using Photino.Blazor;
using System.Drawing;
using System.Extensions.Core;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Daybreak.Windows.Configuration;

/// <summary>
/// Windows-specific platform configuration.
/// Registers Windows-only services like shortcuts, screen management, and monitoring.
/// Graph/MSAL authentication is now handled cross-platform in ProjectConfiguration.
/// </summary>
public sealed class WindowsPlatformConfiguration : PluginConfigurationBase
{
    private const string IconResourceName = "Daybreak.Daybreak.ico";

    private static nint OriginalWndProc;
    private static NativeMethods.WndProcDelegate? WndProcDelegate;
    private static Icon? WindowIcon;
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddHostedSingleton<IScreenManager, ScreenManager>();
        services.AddHostedSingleton<IShortcutManager, ShortcutManager>();
        services.AddHostedService<MemoryUsageMonitor>();
        services.AddHostedService<DiskUsageMonitor>();
        services.AddHostedSingleton<IKeyboardHookService, KeyboardHookService>();
        services.AddScoped<IPrivilegeManager, PrivilegeManager>();
        services.AddSingleton<ICredentialProtector, CredentialProtector>();
        services.AddScoped<IDaybreakInjector, DaybreakInjector>();
        services.AddScoped<IGuildWarsReadyChecker, GuildWarsReadyChecker>();
        services.AddScoped<IGuildWarsProcessFinder, GuildWarsProcessFinder>();
        services.AddSingleton<IModPathResolver, ModPathResolver>();
        services.AddSingleton<IDirectSongRegistrar, DirectSongRegistrar>();
        services.AddScoped<IRegistryService, RegistryService>();
        services.AddSingleton<ISystemThemeDetector, SystemThemeDetector>();
        services.AddSingleton<ISevenZipExtractor, SevenZipArchiveExtractor>();
        services.AddSingleton<IPidProvider, PidProvider>();
        services.AddSingleton<IWindowManipulationService, WindowManipulationService>();
        services.AddSingleton<ICrashDumpService, CrashDumpService>();
        services.AddSingleton<IDaybreakRestartingService, DaybreakRestartingService>();
    }

    public override void RegisterMods(IModsProducer modsProducer)
    {
        modsProducer.RegisterMod<IReShadeService, ReShadeService>();
        modsProducer.RegisterMod<IGuildwarsScreenPlacer, GuildwarsScreenPlacer>();
    }

    public override void RegisterStartupActions(IStartupActionProducer startupActionProducer)
    {
        startupActionProducer.RegisterAction<UpdateReShadeAction>();
    }

    [SupportedOSPlatform("windows")]
    public override void ConfigureWindow(PhotinoBlazorApp app)
    {
        app.MainWindow.RegisterWindowCreatedHandler((_, __) => SetupWindowIcon(app));
        app.MainWindow.RegisterWindowCreatedHandler((_, __) => SetupRoundedWindows(app));
        app.MainWindow.RegisterWindowCreatedHandler((_, __) => SetupBorderless(app));
    }

    [SupportedOSPlatform("windows")]
    private static void SetupRoundedWindows(PhotinoBlazorApp app)
    {
        var scopedLogger = app.Services.GetRequiredService<ILogger<WindowsPlatformConfiguration>>().CreateScopedLogger();
        try
        {
            // DWMWA_WINDOW_CORNER_PREFERENCE is only supported on Windows 11 (Build 22000+)
            if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
            {
                scopedLogger.LogWarning("Rounded corners are not supported on this version of Windows.");
                return;
            }

            var hwnd = app.MainWindow.WindowHandle;
            var preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            NativeMethods.DwmSetWindowAttribute(
                hwnd,
                NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
                ref preference,
                sizeof(uint));
            scopedLogger.LogDebug("Setup rounded corners");
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to set rounded corners on window.");
        }
    }

    [SupportedOSPlatform("windows")]
    private static void SetupBorderless(PhotinoBlazorApp app)
    {
        var hwnd = app.MainWindow.WindowHandle;
        if (hwnd == IntPtr.Zero)
        {
            return;
        }

        // Keep WS_THICKFRAME and WS_CAPTION for snap-to-edge and proper window behavior
        // WM_NCCALCSIZE will hide them visually
        var style = NativeMethods.GetWindowLongPtr(hwnd, NativeMethods.GWL_STYLE);
        style = (nint)((uint)style | NativeMethods.WS_THICKFRAME);
        style = (nint)((uint)style | NativeMethods.WS_CAPTION);
        style = (nint)((uint)style | NativeMethods.WS_MINIMIZEBOX);
        style = (nint)((uint)style | NativeMethods.WS_MAXIMIZEBOX);
        NativeMethods.SetWindowLongPtr(hwnd, NativeMethods.GWL_STYLE, style);

        // Subclass the window to handle WM_NCCALCSIZE
        WndProcDelegate = WndProc;
        var newWndProc = Marshal.GetFunctionPointerForDelegate(WndProcDelegate);
        OriginalWndProc = NativeMethods.SetWindowLongPtr(hwnd, NativeMethods.GWLP_WNDPROC, newWndProc);

        // Force frame recalculation
        NativeMethods.SetWindowPos(
            hwnd,
            IntPtr.Zero,
            0, 0, 0, 0,
            0x0001 | 0x0002 | 0x0004 | 0x0020); // SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED
    }

    [SupportedOSPlatform("windows")]
    private static void SetupWindowIcon(PhotinoBlazorApp app)
    {
        var hwnd = app.MainWindow.WindowHandle;
        if (hwnd == IntPtr.Zero)
        {
            return;
        }

        var scopedLogger = app.Services.GetRequiredService<ILogger<WindowsPlatformConfiguration>>().CreateScopedLogger();
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly.GetManifestResourceInfo(IconResourceName) is not ManifestResourceInfo info)
            {
                scopedLogger.LogWarning("Icon resource '{IconResourceName}' not found in assembly.", IconResourceName);
                return;
            }

            var embeddedIconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(IconResourceName);
            if (embeddedIconStream is null)
            {
                scopedLogger.LogWarning("Icon resource '{IconResourceName}' stream is null.", IconResourceName);
                return;
            }

            WindowIcon = new Icon(embeddedIconStream);
            var hIcon = WindowIcon.Handle;
            if (hIcon is 0)
            {
                scopedLogger.LogWarning("Failed to get handle for window icon.");
                return;
            }

            NativeMethods.SendMessage(hwnd, NativeMethods.WM_SETICON, NativeMethods.ICON_BIG, hIcon);
            NativeMethods.SendMessage(hwnd, NativeMethods.WM_SETICON, NativeMethods.ICON_SMALL, hIcon);
            scopedLogger.LogDebug("Window icon set successfully.");
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to set window icon.");
        }
    }

    [SupportedOSPlatform("windows")]
    private static nint WndProc(nint hwnd, uint msg, nint wParam, nint lParam)
    {
        if (msg == NativeMethods.WM_NCCALCSIZE && wParam != 0)
        {
            // Check if the window is maximized
            var placement = new NativeMethods.WINDOWPLACEMENT
            {
                length = Marshal.SizeOf<NativeMethods.WINDOWPLACEMENT>()
            };
            NativeMethods.GetWindowPlacement(hwnd, ref placement);

            if (placement.showCmd == NativeMethods.SW_SHOWMAXIMIZED)
            {
                // When maximized, we need to adjust for the frame that extends beyond the screen
                // Get the monitor's work area to properly size the window
                var monitor = NativeMethods.MonitorFromWindow(hwnd, NativeMethods.MONITOR_DEFAULTTONEAREST);
                var monitorInfo = new NativeMethods.MONITORINFO();
                monitorInfo.cbSize = Marshal.SizeOf<NativeMethods.MONITORINFO>();
                NativeMethods.GetMonitorInfo(monitor, ref monitorInfo);

                // The NCCALCSIZE_PARAMS structure is at lParam
                var ncParams = Marshal.PtrToStructure<NativeMethods.NCCALCSIZE_PARAMS>(lParam);

                // Set the client area to match the monitor's work area
                ncParams.rgrc0.Left = monitorInfo.rcWork.Left;
                ncParams.rgrc0.Top = monitorInfo.rcWork.Top;
                ncParams.rgrc0.Right = monitorInfo.rcWork.Right;
                ncParams.rgrc0.Bottom = monitorInfo.rcWork.Bottom;

                Marshal.StructureToPtr(ncParams, lParam, false);
            }

            return 0;
        }

        return NativeMethods.CallWindowProc(OriginalWndProc, hwnd, msg, wParam, lParam);
    }
}
