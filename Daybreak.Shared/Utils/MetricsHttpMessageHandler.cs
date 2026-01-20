using Daybreak.Shared.Services.Metrics;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace Daybreak.Shared.Utils;

public sealed partial class MetricsHttpMessageHandler<T> : DelegatingHandler
{
    private const string LatencyUnitsName = "ms";
    private const string LatencyDescription = "Http Request Latency";

    private readonly string name;
    private readonly Histogram<double> latencyHistogram;

    public MetricsHttpMessageHandler(IMetricsService metricsService, HttpMessageHandler innerHandler) : base(innerHandler)
    {
        // Sanitize the type name to be OpenTelemetry compliant
        // OpenTelemetry instrument names must match: [a-zA-Z][a-zA-Z0-9_.\-/]*
        var typeName = typeof(T).Name;
        // Remove generic arity suffix (e.g., `1, `2) and replace invalid characters
        var sanitizedName = InvalidCharsRegex().Replace(typeName, "").ToLower();
        this.name = $"http.client.latency.{sanitizedName}";
        this.latencyHistogram = metricsService.CreateHistogram<double>(this.name, LatencyUnitsName, LatencyDescription, Models.Metrics.AggregationTypes.P95);
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var response = base.Send(request, cancellationToken);
            return response;
        }
        finally
        {
            this.latencyHistogram.Record(sw.ElapsedMilliseconds);
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
        finally
        {
            this.latencyHistogram.Record(sw.ElapsedMilliseconds);
        }
    }

    [GeneratedRegex(@"[^a-zA-Z0-9_.\-/]")]
    private static partial Regex InvalidCharsRegex();
}
