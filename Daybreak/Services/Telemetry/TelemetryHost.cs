using Daybreak.Configuration;
using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog.Events;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Extensions.Core;
using System.Xml.Linq;
using TrailBlazr.Services;

namespace Daybreak.Services.Telemetry;

internal sealed class TelemetryHost : IDisposable, IHostedService
{
    private const string ServiceName = "Daybreak";
    private const string UnknownHost = "Unknown";
    private const string MaskedValue = "[REDACTED]";

    private static readonly string ApmEndpoint = SecretManager.GetSecret(SecretKeys.ApmUri);
    private static readonly string ApmApiKey = SecretManager.GetSecret(SecretKeys.ApmApiKey);

    // Headers that should be masked for security
    private static readonly HashSet<string> SensitiveHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization",
        "Api-Key",
        "X-API-Key",
        "Bearer",
        "Cookie",
        "Set-Cookie",
        "X-Auth-Token",
        "X-Access-Token",
        "Authentication"
    };

    public static readonly ActivitySource Source = new(ServiceName);

    private readonly ResourceBuilder resourceBuilder;
    private readonly IViewManager viewManager;
    private readonly IOptionsMonitor<TelemetryOptions> options;
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TelemetryHost> logger;

    private TracerProvider? tracer;
    private MeterProvider? meter;
    private OpenTelemetryLoggerProvider? otlpLoggerProvider;
    private ILogger? otlpLogger;
    private Activity? currentActivity;

    public TelemetryHost(
        ResourceBuilder resourceBuilder,
        IViewManager viewManager,
        IOptionsMonitor<TelemetryOptions> liveOptions,
        ILoggerFactory loggerFactory,
        ILogger<TelemetryHost> logger)
    {
        this.resourceBuilder = resourceBuilder.ThrowIfNull();
        this.options = liveOptions.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.loggerFactory = loggerFactory.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.options.OnChange(this.OnOptionsUpdated);
        this.viewManager.ShowViewRequested += this.ViewManager_ShowViewRequested;
        this.EnableSelfDiagnostics();
        this.BuildTelemetryProvider();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        TelemetryLogSink.Instance.LoggingHandler = default;
        this.tracer?.Dispose();
        this.meter?.Dispose();
        this.otlpLoggerProvider?.Dispose();
        this.tracer = default;
        this.meter = default;
        this.otlpLoggerProvider = default;
        this.otlpLogger = default;
    }

    private void ViewManager_ShowViewRequested(object? sender, TrailBlazr.Models.ViewRequest e)
    {
        this.currentActivity?.Stop();
        this.currentActivity?.Dispose();
        this.currentActivity = Source.StartActivity("ViewNavigation");
        if (this.currentActivity is null)
        {
            return;
        }

        this.currentActivity.AddEvent(new ActivityEvent("View Requested"));
        this.currentActivity.SetTag("viewType", e.ViewType?.Name ?? "Unknown");
        this.currentActivity.SetTag("viewModelType", e.ViewModelType?.Name ?? "Unknown");
        this.currentActivity.SetStatus(ActivityStatusCode.Ok);
    }

    private void EnableSelfDiagnostics()
    {
        // Subscribe to OpenTelemetry's internal diagnostic events
        var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == ServiceName,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            ActivityStarted = activity => Debug.WriteLine($"[OTel] Activity started: {activity.DisplayName}"),
            ActivityStopped = activity => Debug.WriteLine($"[OTel] Activity stopped: {activity.DisplayName}, Duration: {activity.Duration}")
        };
        ActivitySource.AddActivityListener(listener);

        // Enable verbose logging for OpenTelemetry SDK
        AppContext.SetSwitch("OpenTelemetry.Exporter.Otlp.EnableDebugLogging", true);

        // Enable OpenTelemetry self-diagnostics to file (writes to log file in specified directory)
        Environment.SetEnvironmentVariable("OTEL_DIAGNOSTICS_ENABLED", "true");

        // Subscribe to OpenTelemetry's EventSource for internal logs
        var otelListener = new OpenTelemetryEventListener(this.loggerFactory);
    }

    private void OnOptionsUpdated(TelemetryOptions _, string? __)
    {
        this.BuildTelemetryProvider();
    }

    private void BuildTelemetryProvider()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        TelemetryLogSink.Instance.LoggingHandler = default;
        this.tracer?.Dispose();
        this.meter?.Dispose();
        this.tracer = default;
        this.meter = default;
        this.otlpLoggerProvider?.Dispose();
        this.otlpLoggerProvider = default;
        this.otlpLogger = default;

        if (!this.options.CurrentValue.Enabled)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(ApmApiKey) || string.IsNullOrWhiteSpace(ApmEndpoint))
        {
            scopedLogger.LogError("ApmApiKey or ApmEndpoint is not configured. Telemetry is disabled.");
            return;
        }

        var apmUri = new Uri(ApmEndpoint);
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
                    activity.DisplayName = request.RequestUri?.IdnHost ?? UnknownHost;
                    activity.SetTag("http.url", request.RequestUri?.ToString() ?? string.Empty);
                    activity.SetTag("http.method", request.Method.Method);
                    AddRequestHeaders(activity, request);
                };
                o.EnrichWithHttpResponseMessage = (activity, response) =>
                {
                    activity.SetTag("http.status_code", (int)response.StatusCode);
                    activity.SetStatus(response.IsSuccessStatusCode ? ActivityStatusCode.Ok : ActivityStatusCode.Error);
                    AddResponseHeaders(activity, response);
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
                o.Endpoint = new Uri(apmUri, "v1/traces");
                o.Headers = $"Authorization=ApiKey {ApmApiKey}";
                o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            })
            .Build();

        this.meter = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(this.resourceBuilder)
            .AddProcessInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter("Daybreak.MetricsStore")
            .AddMeter("System.Net.Http")
            .AddMeter("System.Net.NameResolution")
            .AddMeter("OpenTelemetry.Instrumentation.SqlClient")
            .AddOtlpExporter((exporterOptions, readerOptions) =>
            {
                exporterOptions.Endpoint = new Uri(apmUri, "v1/metrics");
                exporterOptions.Headers = $"Authorization=ApiKey {ApmApiKey}";
                exporterOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                readerOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 300_000;
                readerOptions.TemporalityPreference = MetricReaderTemporalityPreference.Delta;
            })
            .Build();

        var logOpts = new OpenTelemetryLoggerOptions
        {
            IncludeFormattedMessage = true,
            IncludeScopes = true,
            ParseStateValues = true
        };
        logOpts.SetResourceBuilder(this.resourceBuilder);
        logOpts.AddOtlpExporter(exp =>
        {
            exp.Endpoint = new Uri(apmUri, "v1/logs");
            exp.Headers = $"Authorization=ApiKey {ApmApiKey}";
            exp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        });

        this.otlpLoggerProvider = new OpenTelemetryLoggerProvider(new StaticOptionsMonitor<OpenTelemetryLoggerOptions>(logOpts));
        this.otlpLogger = this.otlpLoggerProvider.CreateLogger(ServiceName);

        // Register handler to forward Serilog events to OpenTelemetry
        TelemetryLogSink.Instance.LoggingHandler = this.ForwardLogEvent;
    }

    private void ForwardLogEvent(LogEvent logEvent)
    {
        if (this.otlpLogger is null)
        {
            return;
        }

        if (logEvent.Level <= LogEventLevel.Information)
        {
            return;
        }

        var logLevel = logEvent.Level switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => LogLevel.None
        };

        this.otlpLogger.Log(logLevel, logEvent.Exception, logEvent.RenderMessage());
    }

    private static void AddRequestHeaders(Activity activity, HttpRequestMessage request)
    {
        foreach (var header in request.Headers.Concat(request.Content?.Headers.AsEnumerable() ?? []))
        {
            var headerName = header.Key;
            var headerValue = SensitiveHeaders.Contains(headerName) 
                ? MaskedValue 
                : string.Join(", ", header.Value);
            
            activity.SetTag($"http.request.header.{headerName.ToLowerInvariant()}", headerValue);
        }
    }

    private static void AddResponseHeaders(Activity activity, HttpResponseMessage response)
    {
        foreach (var header in response.Headers.Concat(response.Content?.Headers.AsEnumerable() ?? []))
        {
            var headerName = header.Key;
            var headerValue = SensitiveHeaders.Contains(headerName) 
                ? MaskedValue 
                : string.Join(", ", header.Value);
            
            activity.SetTag($"http.response.header.{headerName.ToLowerInvariant()}", headerValue);
        }
    }

    internal sealed class OpenTelemetryEventListener(ILoggerFactory loggerFactory) : EventListener
    {
        private readonly ILogger logger = loggerFactory.CreateLogger("OpenTelemetry.Diagnostics");

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            base.OnEventSourceCreated(eventSource);

            if (eventSource.Name.StartsWith("OpenTelemetry", StringComparison.OrdinalIgnoreCase))
            {
                this.EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            var message = eventData.Message is not null && eventData.Payload is not null
                ? string.Format(eventData.Message, [.. eventData.Payload])
                : eventData.Message ?? string.Empty;

            if (eventData.Level is EventLevel.Verbose or EventLevel.Informational or EventLevel.LogAlways)
            {
                // Ignore verbose logs for brevity
                return;
            }

            var logLevel = eventData.Level switch
            {
                EventLevel.Critical => LogLevel.Critical,
                EventLevel.Error => LogLevel.Error,
                EventLevel.Warning => LogLevel.Warning,
                _ => LogLevel.Debug
            };

            this.logger.Log(logLevel, "[{Source}] {Message}", eventData.EventSource.Name, message);
        }
    }
}
