using Serilog.Settings.Configuration;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using Net.Sdk.Web.Options;

namespace Daybreak.API.Logging;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration, new ConfigurationReaderOptions
            {
                SectionName = "Logging"
            })
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Level:u4}: [{SourceContext}] [{CorrelationVector}] {NewLine}{Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Sixteen)
            .WriteTo.File(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Level:u4}: [{SourceContext}] [{CorrelationVector}] {NewLine}{Message:lj}{NewLine}{Exception}",
                path: "Daybreak.API.log",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 50 * 1024 * 1024,
                retainedFileCountLimit: 1,
                shared: true)
            .CreateLogger());
        builder.Services.AddScoped<LoggingEnrichmentMiddleware>();
        builder.Services.AddOptions<CorrelationVectorOptions>()
            .Bind(builder.Configuration.GetSection("Correlation"));
        return builder;
    }
}
