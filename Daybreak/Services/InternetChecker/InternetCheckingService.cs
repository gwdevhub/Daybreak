using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Services.InternetChecker;
using Daybreak.Shared.Services.Metrics;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.InternetChecker;

internal sealed class InternetCheckingService : IInternetCheckingService
{
    private const string ConnectionVerificationLatency = "Connection verification latency";
    private const string ConnectionVerificationUnit = "ms";
    private const string ConnectionVerificationDescription = "Measure how long it takes to verify internet connectivity (in ms).";
    private const string GoogleUrl = "https://google.com";
    private const string BingUrl = "https://bing.com";
    private const string GuildwarsUrl = "https://guildwars.com";

    private readonly Histogram<double> connectionCheckingLatency;
    private readonly IHttpClient<InternetCheckingService> httpClient;
    private readonly ILogger<InternetCheckingService> logger;

    public InternetCheckingService(
        IMetricsService metricsService,
        IHttpClient<InternetCheckingService> httpClient,
        ILogger<InternetCheckingService> logger)
    {
        this.connectionCheckingLatency = metricsService.ThrowIfNull().CreateHistogram<double>(ConnectionVerificationLatency, ConnectionVerificationUnit, ConnectionVerificationDescription, AggregationTypes.NoAggregate);
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<InternetConnectionState> CheckConnectionAvailable(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        this.logger.LogInformation("Verifying connection state");
        var networkAvailable = NetworkInterface.GetIsNetworkAvailable();
        if (!networkAvailable)
        {
            this.logger.LogError("Outage detected. Network is unavailable");
            return InternetConnectionState.Unavailable;
        }

        var queryTasks = new Task<bool>[]
        {
            this.UrlAvailable(GoogleUrl),
            this.UrlAvailable(BingUrl),
            this.UrlAvailable(GuildwarsUrl),
        };

        var googleQueryResult = await queryTasks[0];
        var bingQueryResult = await queryTasks[1];
        var guildwarsQueryResult = await queryTasks[2];

        if (googleQueryResult && bingQueryResult && guildwarsQueryResult)
        {
            this.logger.LogInformation("Connection is available");
            this.connectionCheckingLatency.Record(sw.ElapsedMilliseconds);
            return InternetConnectionState.Available;
        }

        if (googleQueryResult || bingQueryResult)
        {
            if (guildwarsQueryResult)
            {
                this.logger.LogWarning($"Partial outage detected. {GuildwarsUrl} is available");
                this.connectionCheckingLatency.Record(sw.ElapsedMilliseconds);
                return InternetConnectionState.PartialOutage;
            }
            else
            {
                this.logger.LogError($"Partial outage detected. {GuildwarsUrl} is unavailable");
                this.connectionCheckingLatency.Record(sw.ElapsedMilliseconds);
                return InternetConnectionState.GuildwarsOutage;
            }
        }

        this.logger.LogError($"Outage detected. Internet is unavailable");
        this.connectionCheckingLatency.Record(sw.ElapsedMilliseconds);
        return InternetConnectionState.Unavailable;
    }

    private async Task<bool> UrlAvailable(string url)
    {
        this.logger.LogInformation($"Verifying connection to {url}");
        try
        {
            _ = await this.httpClient.GetAsync(GoogleUrl);
            return true;
        }
        catch
        {
            this.logger.LogWarning($"Failed to verify connection to {url}");
            return false;
        }
    }
}
