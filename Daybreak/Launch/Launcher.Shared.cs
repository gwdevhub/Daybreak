using Daybreak.Services.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Serilog;

namespace Daybreak.Launch;

public static partial class Launcher
{
    public static void SetupLogging(PhotinoBlazorAppBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()

            .Enrich.FromLogContext()

            .Enrich.WithMachineName()

            .Enrich.WithProcessId()
            .Enrich.WithProcessName()

            .Enrich.WithThreadId()
            .Enrich.WithThreadName()

            .Enrich.WithEnvironmentName()

            .Enrich.WithAssemblyName()
            .Enrich.WithAssemblyInformationalVersion()

            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Sink(InMemorySink.Instance)
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }
}
