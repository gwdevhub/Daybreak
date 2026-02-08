using Daybreak.Services.Logging;
using Daybreak.Services.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Extensions.Core;

namespace Daybreak.Launch;

public partial class Launcher
{
    public const string OutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Level:u4}: [{EnvironmentName}] [{ThreadId}:{ThreadName}] [{SourceContext}]{NewLine}{Message:lj}{NewLine}{Exception}";

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
}
