using Daybreak.Configuration;
using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Telemetry;
internal sealed class TelemetryHost : IDisposable, IApplicationLifetimeService
{
    private const string ServiceName = "Daybreak";

    private static readonly Uri ApmEndpoint = new UriBuilder(SecretManager.GetSecret(SecretKeys.ApmUri)).Uri;
    private static readonly string ApmApiKey = SecretManager.GetSecret(SecretKeys.ApmApiKey);

    public static readonly ActivitySource Source = new(ServiceName);

    private readonly ResourceBuilder resourceBuilder;
    private readonly ILoggerFactory loggerFactory;
    private readonly SwappableLoggerProvider swappableLoggerProvider;
    private readonly ILiveOptions<TelemetryOptions> options;
    private readonly IOptionsMonitor<OpenTelemetryLoggerOptions> logOptions;

    private TracerProvider? tracer;
    private MeterProvider? meter;
    private OpenTelemetryLoggerProvider? logger;

    public TelemetryHost(
        ResourceBuilder resourceBuilder,
        IOptionsUpdateHook optionsHook,
        ILoggerFactory loggerFactory,
        SwappableLoggerProvider swappableLoggerProvider,
        ILiveOptions<TelemetryOptions> liveOptions)
    {
        this.resourceBuilder = resourceBuilder.ThrowIfNull();
        this.options = liveOptions.ThrowIfNull();
        this.loggerFactory = loggerFactory.ThrowIfNull();
        this.swappableLoggerProvider = swappableLoggerProvider.ThrowIfNull();
        optionsHook.RegisterHook<TelemetryOptions>(this.OnOptionsUpdated);
        var logOpts = new OpenTelemetryLoggerOptions
        {
            IncludeFormattedMessage = true,
            IncludeScopes = true,
            ParseStateValues = true
        };

        logOpts.SetResourceBuilder(this.resourceBuilder);
        logOpts.AddOtlpExporter(exp =>
        {
            exp.Endpoint = new Uri(ApmEndpoint, "v1/logs");
            exp.Headers = $"Authorization=ApiKey {ApmApiKey}";
            exp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        });

        this.logOptions = new StaticOptionsMonitor<OpenTelemetryLoggerOptions>(logOpts);
        this.BuildTelemetryProvider();
    }

    public void OnClosing()
    {
        this.Dispose();
    }

    public void OnStartup()
    {
    }

    public void Dispose()
    {
        this.tracer?.Dispose();
        this.meter?.Dispose();
        this.tracer = default;
        this.meter = default;
    }

    private void OnOptionsUpdated()
    {
        this.BuildTelemetryProvider();
    }

    private void BuildTelemetryProvider()
    {
        this.tracer?.Dispose();
        this.meter?.Dispose();
        this.tracer = default;
        this.meter = default;
        this.swappableLoggerProvider.SetInner(null);
        this.logger?.Dispose();
        this.logger = default;

        if (!this.options.Value.Enabled)
        {
            return;
        }

        this.tracer = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(this.resourceBuilder)
            .AddSource(ServiceName)
            .ConfigureResource(r => r.AddService(
                serviceName: ServiceName,
                serviceVersion: ProjectConfiguration.CurrentVersion.ToString()))
            .AddHttpClientInstrumentation(o =>
            {
                o.RecordException = true;
                o.EnrichWithHttpRequestMessage = (activity, request) =>
                {
                    activity.DisplayName = $"{request.Method} {request.RequestUri}";
                };
                o.EnrichWithHttpResponseMessage = (activity, response) =>
                {
                    activity.SetTag("http.status_code", (int)response.StatusCode);
                    activity.SetTag("http.response_content_length", response.Content?.Headers.ContentLength ?? 0);
                };
                o.EnrichWithException = (activity, exception) =>
                {
                    activity.SetTag("exception.type", exception.GetType().FullName);
                    activity.SetTag("exception.message", exception.Message);
                    activity.SetTag("exception.stacktrace", exception.StackTrace);
                };
            })
            .AddSqlClientInstrumentation()
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(ApmEndpoint, "v1/traces");
                o.Headers = $"Authorization=ApiKey {ApmApiKey}";
                o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            })
            .Build();

        this.meter = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(this.resourceBuilder)
            .AddProcessInstrumentation()
            .AddRuntimeInstrumentation()
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(ApmEndpoint, "v1/metrics");
                o.Headers = $"Authorization=ApiKey {ApmApiKey}";
                o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            })
            .Build();

        this.logger = new OpenTelemetryLoggerProvider(this.logOptions);
        this.swappableLoggerProvider.SetInner(this.logger);
    }
}
