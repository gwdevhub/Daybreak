using Daybreak.Models;
using Daybreak.Services.Metrics;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Net;
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
    private const string DNSUrl1 = "8.8.8.8";
    private const string DNSUrl2 = "8.8.4.4";
    private const string DNSUrl3 = "1.1.1.1";

    private readonly Histogram<double> connectionCheckingLatency;
    private readonly IHttpClient<InternetCheckingService> httpClient;
    private readonly ILogger<InternetCheckingService> logger;

    public InternetCheckingService(
        IMetricsService metricsService,
        IHttpClient<InternetCheckingService> httpClient,
        ILogger<InternetCheckingService> logger)
    {
        this.connectionCheckingLatency = metricsService.ThrowIfNull().CreateHistogram<double>(ConnectionVerificationLatency, ConnectionVerificationUnit, ConnectionVerificationDescription, Models.Metrics.AggregationTypes.NoAggregate);
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

        var dnsAvailable = await this.PingDNS();
        if (!dnsAvailable)
        {
            this.logger.LogWarning("DNS servers are not available. Attempts to connect to the internet will probably fail");
        }

        var queryTasks = new Task<bool>[]
        {
            this.UrlAvailable(GoogleUrl),
            this.UrlAvailable(BingUrl),
            this.UrlAvailable(GuildwarsUrl),
        };

        await Task.WhenAny(queryTasks);
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

    private async Task<bool> PingDNS()
    {
        try
        {
            using var ping1 = new Ping();
            using var ping2 = new Ping();
            using var ping3 = new Ping();
            var tasks = new[]
            {
                ping1.SendPingAsync(IPAddress.Parse(DNSUrl1)),
                ping2.SendPingAsync(IPAddress.Parse(DNSUrl2)),
                ping3.SendPingAsync(IPAddress.Parse(DNSUrl3)),
            };

            await Task.WhenAny(tasks);
            var result1 = await tasks[0];
            var result2 = await tasks[1];
            var result3 = await tasks[2];
            this.logger.LogInformation($"Ping to {result1.Address} resulted in {result1.Status}");
            this.logger.LogInformation($"Ping to {result2.Address} resulted in {result2.Status}");
            this.logger.LogInformation($"Ping to {result3.Address} resulted in {result3.Status}");

            if (result1.Status is not IPStatus.Success &&
                result2.Status is not IPStatus.Success &&
                result3.Status is not IPStatus.Success)
            {
                return false;
            }

            return true;
        }
        catch(Exception e)
        {
            this.logger.LogError(e, $"Encountered exception during DNS pings. Marking connection as unavailable");
            return false;
        }
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
