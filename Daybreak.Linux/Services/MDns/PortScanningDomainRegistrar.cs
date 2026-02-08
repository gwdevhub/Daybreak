using System.Net.Http.Json;
using System.Text.Json;
using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Services.MDns;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Linux.Services.MDns;

/// <summary>
/// Linux-specific implementation of <see cref="IMDomainRegistrar"/>.
/// Since mDNS announcements from Wine don't propagate to the host,
/// this implementation scans a known port range (5080–5100) for running
/// Daybreak API instances and matches them to Guild Wars processes
/// by querying the API's health endpoint for its Wine PID, then
/// converting it to a Linux PID via <see cref="IWinePidMapper"/>.
/// All probes fire in parallel with a 500ms total timeout.
/// </summary>
public sealed class PortScanningDomainRegistrar(
    IWinePidMapper winePidMapper,
    ILogger<PortScanningDomainRegistrar> logger)
    : IMDomainRegistrar, IHostedService, IDisposable
{
    private const int StartPort = 5080;
    private const int EndPort = 5100;
    private const string HealthPath = "/api/v1/rest/health";
    private const string DaybreakApiServicePrefix = "daybreak-api-";
    private static readonly TimeSpan ProbeTimeout = TimeSpan.FromMilliseconds(500);
    private static readonly TimeSpan ScanInterval = TimeSpan.FromSeconds(15);
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly IWinePidMapper winePidMapper = winePidMapper;
    private readonly ILogger<PortScanningDomainRegistrar> logger = logger;
    private readonly HttpClient httpClient = new() { Timeout = ProbeTimeout };

    // Rebuilt on each scan — survives Daybreak restarts since it's based on live port probing.
    private volatile Dictionary<string, List<Uri>> discoveredServices = [];
    private CancellationTokenSource? backgroundCts;

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        this.backgroundCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _ = Task.Factory.StartNew(
            () => this.ScanPeriodically(this.backgroundCts.Token),
            this.backgroundCts.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Current);
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        this.backgroundCts?.Cancel();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        this.backgroundCts?.Dispose();
        this.httpClient.Dispose();
    }

    public IReadOnlyList<Uri>? Resolve(string service)
    {
        if (this.discoveredServices.TryGetValue(service, out var uris) && uris.Count > 0)
        {
            return uris;
        }

        return default;
    }

    public IReadOnlyList<Uri>? QueryByServiceName(Func<string, bool> query)
    {
        var results = new List<Uri>();
        foreach (var (serviceName, uris) in this.discoveredServices)
        {
            if (query(serviceName))
            {
                results.AddRange(uris);
            }
        }

        return results.Count > 0 ? results : default;
    }

    public void QueryAllServices()
    {
        _ = Task.Run(() => this.ScanPortsAsync(CancellationToken.None));
    }

    private async Task ScanPeriodically(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await this.ScanPortsAsync(cancellationToken);
            await Task.Delay(ScanInterval, cancellationToken);
        }
    }

    private async Task ScanPortsAsync(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var newServices = new Dictionary<string, List<Uri>>();

        using var timeoutCts = new CancellationTokenSource(ProbeTimeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

        var probeTasks = new List<Task<(int Port, int? ProcessId)>>();
        for (var port = StartPort; port <= EndPort; port++)
        {
            probeTasks.Add(this.ProbePortAsync(port, linkedCts.Token));
        }

        try
        {
            await Task.WhenAll(probeTasks);
        }
        catch (OperationCanceledException)
        {
        }

        foreach (var task in probeTasks)
        {
            if (!task.IsCompletedSuccessfully)
            {
                continue;
            }

            var (port, processId) = task.Result;
            if (processId is null)
            {
                continue;
            }

            // The API reports its Wine PID. Convert to Linux PID.
            var linuxPid = this.winePidMapper.WinePidToLinuxPid(processId.Value, "Gw.exe");
            var effectivePid = linuxPid ?? processId.Value;

            var serviceName = $"{DaybreakApiServicePrefix}{effectivePid}";
            var uri = new Uri($"http://localhost:{port}");

            scopedLogger.LogDebug(
                "Found Daybreak API on port {Port} with Wine PID {WinePid} (Linux PID {LinuxPid})",
                port,
                processId.Value,
                effectivePid);

            if (!newServices.TryGetValue(serviceName, out var uriList))
            {
                uriList = [];
                newServices[serviceName] = uriList;
            }

            uriList.Add(uri);
        }

        this.discoveredServices = newServices;
        scopedLogger.LogDebug("Port scan complete. Found {Count} Daybreak API instance(s)", newServices.Count);
    }

    private async Task<(int Port, int? ProcessId)> ProbePortAsync(int port, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"http://localhost:{port}{HealthPath}";
            using var response = await this.httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return (port, null);
            }

            var processIdResponse = await response.Content.ReadFromJsonAsync<ProcessIdResponse>(JsonOptions, cancellationToken);
            return (port, processIdResponse?.ProcessId);
        }
        catch
        {
            return (port, null);
        }
    }
}
