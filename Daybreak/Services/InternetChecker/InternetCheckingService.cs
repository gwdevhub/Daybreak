using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Services.InternetChecker;
using Daybreak.Shared.Services.Metrics;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Extensions.Core;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace Daybreak.Services.InternetChecker;

internal sealed class InternetCheckingService(
    IMetricsService metricsService,
    IHttpClient<InternetCheckingService> httpClient,
    ILogger<InternetCheckingService> logger) : IInternetCheckingService
{
    private const string ConnectionVerificationLatency = "Connection verification latency";
    private const string ConnectionVerificationUnit = "ms";
    private const string ConnectionVerificationDescription = "Measure how long it takes to verify internet connectivity (in ms).";
    private const string GStaticUrl = "http://www.gstatic.com/generate_204";

    private readonly Histogram<double> connectionCheckingLatency = metricsService.ThrowIfNull().CreateHistogram<double>(ConnectionVerificationLatency, ConnectionVerificationUnit, ConnectionVerificationDescription, AggregationTypes.NoAggregate);
    private readonly IHttpClient<InternetCheckingService> httpClient = httpClient.ThrowIfNull();
    private readonly ILogger<InternetCheckingService> logger = logger.ThrowIfNull();

    public async Task<InternetConnectionState> CheckConnectionAvailable(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        this.logger.LogDebug("Verifying connection state");
        var networkAvailable = NetworkInterface.GetIsNetworkAvailable();
        if (!networkAvailable)
        {
            this.logger.LogError("Outage detected. Network is unavailable");
            return InternetConnectionState.Unavailable;
        }

        var gstaticQuery = await this.UrlAvailable(GStaticUrl);
        if (gstaticQuery)
        {
            this.logger.LogDebug("Connection is available");
            this.connectionCheckingLatency.Record(sw.ElapsedMilliseconds);
            return InternetConnectionState.Available;
        }

        this.logger.LogError($"Outage detected. Internet is unavailable");
        this.connectionCheckingLatency.Record(sw.ElapsedMilliseconds);
        return InternetConnectionState.Unavailable;
    }

    private async Task<bool> UrlAvailable(string url)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug($"Verifying connection to {url}");
        try
        {
            _ = await this.httpClient.GetAsync(GoogleUrl);
            return true;
        }
        catch
        {
            scopedLogger.LogWarning($"Failed to verify connection to {url}");
            return false;
        }
    }
}
