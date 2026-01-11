using Daybreak.Services.Logging;
using Daybreak.Services.Telemetry;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Runtime.InteropServices;

namespace Daybreak.Launch;

public partial class Launcher
{
    public const string OutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Level:u4}: [{EnvironmentName}] [{ThreadId}:{ThreadName}] [{SourceContext}]{NewLine}{Message:lj}{NewLine}{Exception}";

    private static nint OriginalWndProc;
    private static NativeMethods.WndProcDelegate? WndProcDelegate;

    public static void SetupLogging(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Daybreak.Services.TradeChat.TradeChatService", LogEventLevel.Information)
            .MinimumLevel.Override("Daybreak.Services.Credentials.CredentialManager", LogEventLevel.Information)
            .MinimumLevel.Override("Daybreak.Shared.Services.BuildTemplates.BuildTemplateManager", LogEventLevel.Information)

            .Enrich.FromLogContext()

            .Enrich.WithThreadId()
            .Enrich.WithThreadName()

            .Enrich.WithEnvironmentName()

            .WriteTo.Console(
                outputTemplate: OutputTemplate,
                theme: AnsiConsoleTheme.Sixteen)
            .WriteTo.Sink(InMemorySink.Instance)
            .WriteTo.Sink(TelemetryLogSink.Instance)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }

    private static void StartHostedServices(PhotinoBlazorApp app, CancellationTokenSource cts)
    {
        var hostedServices = app.Services.GetServices<IHostedService>();
        foreach (var hostedService in hostedServices)
        {
            Task.Factory.StartNew(() => hostedService.StartAsync(cts.Token), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }

    private static bool StopHostedServices(PhotinoBlazorApp app, CancellationTokenSource cts)
    {
        cts.CancelAfter(TimeSpan.FromSeconds(2));
        var hostedServices = app.Services.GetServices<IHostedService>();
        var tasks = hostedServices.Select(s => s.StopAsync(cts.Token));
        Task.WaitAll(tasks, cts.Token);

        cts.Dispose();
        return false;
    }

    private static void SetupRoundedWindows(PhotinoBlazorApp app)
    {
        var hwnd = app.MainWindow.WindowHandle;
        var preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
        NativeMethods.DwmSetWindowAttribute(
            hwnd,
            NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
            ref preference,
            sizeof(uint));
    }

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
