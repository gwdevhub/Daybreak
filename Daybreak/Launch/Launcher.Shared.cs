using Daybreak.Services.Logging;
using Daybreak.Services.Telemetry;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Daybreak.Launch;

public partial class Launcher
{
    private const string OutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Level:u4}: [{EnvironmentName}] [{ThreadId}:{ThreadName}] [{SourceContext}]{NewLine}{Message:lj}{NewLine}{Exception}";

    public static void SetupLogging(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()

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
}
