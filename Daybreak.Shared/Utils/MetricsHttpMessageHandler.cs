using Daybreak.Shared.Services.Metrics;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Http;

namespace Daybreak.Shared.Utils;

public sealed class MetricsHttpMessageHandler<T> : DelegatingHandler
{
    private const string LatencyUnitsName = "ms";
    private const string LatencyDescription = "Http Request Latency";

    private readonly string name;
    private readonly Histogram<double> latencyHistogram;

    public MetricsHttpMessageHandler(IMetricsService metricsService, HttpMessageHandler innerHandler) : base(innerHandler)
    {
        this.name = typeof(T).Name;
        this.latencyHistogram = metricsService.CreateHistogram<double>(this.name, LatencyUnitsName, LatencyDescription, Models.Metrics.AggregationTypes.P95);
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var response = base.Send(request, cancellationToken);
        this.latencyHistogram.Record(sw.ElapsedMilliseconds);
        return response;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var response = await base.SendAsync(request, cancellationToken);
        this.latencyHistogram.Record(sw.ElapsedMilliseconds);
        return response;
    }
}
