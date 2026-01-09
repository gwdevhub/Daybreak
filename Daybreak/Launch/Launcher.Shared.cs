using Daybreak.Services.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Daybreak.Launch;

public static partial class Launcher
{
    private const string OutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Level:u4}: [{EnvironmentName}] [{ThreadId}:{ThreadName}] [{SourceContext}]{NewLine}{Message:lj}{NewLine}{Exception}";

    public static void SetupLogging(PhotinoBlazorAppBuilder builder)
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
            .WriteTo.Debug(
                outputTemplate: OutputTemplate)
            .WriteTo.Sink(InMemorySink.Instance)
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }

    internal static void StartHostedServices(PhotinoBlazorApp app, CancellationTokenSource cts)
    {
        var hostedServices = app.Services.GetServices<IHostedService>();
        foreach (var hostedService in hostedServices)
        {
            Task.Factory.StartNew(() => hostedService.StartAsync(cts.Token), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }

    private static bool StopHostedServicesAsync(PhotinoBlazorApp app, CancellationTokenSource cts)
    {
        cts.CancelAfter(TimeSpan.FromSeconds(2));
        var hostedServices = app.Services.GetServices<IHostedService>();
        var tasks = hostedServices.Select(s => s.StopAsync(cts.Token));
        Task.WaitAll(tasks, cts.Token);

        cts.Dispose();
        return false;
    }
}
