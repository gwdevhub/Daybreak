using System.Net.Http.Json;
using System.Text.Json;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Services.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Api;

/// <summary>
/// Scans a known port range (5080–5100) for running Daybreak API instances
/// and matches them to Guild Wars processes by querying the API's health endpoint.
/// All probes fire in parallel with a 500ms total timeout.
/// </summary>
public sealed class ApiScanningService(
    IPidProvider pidProvider,
    ILogger<ApiScanningService> logger)
    : IApiScanningService, IHostedService, IDisposable
{
    private const int StartPort = 5080;
    private const int EndPort = 5100;
    private const string HealthPath = "/api/v1/rest/health";
    private const string GuildWarsExecutable = "Gw.exe";
    private static readonly TimeSpan ProbeTimeout = TimeSpan.FromMilliseconds(2000);
    private static readonly TimeSpan ScanInterval = TimeSpan.FromSeconds(10);
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly IPidProvider pidProvider = pidProvider;
    private readonly ILogger<ApiScanningService> logger = logger;
    private readonly HttpClient httpClient = new() { Timeout = ProbeTimeout };

    // Rebuilt on each scan — survives Daybreak restarts since it's based on live port probing.
    private volatile Dictionary<int, Uri> discoveredApis = [];
    private CancellationTokenSource? backgroundCts;

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        this.backgroundCts = new CancellationTokenSource();
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

    public Uri? GetApiUriByProcessId(int processId)
    {
        if (this.discoveredApis.TryGetValue(processId, out var uri))
        {
            return uri;
        }

        return default;
    }

    public IReadOnlyList<(int ProcessId, Uri Uri)>? QueryByProcessId(Func<int, bool> predicate)
    {
        var results = new List<(int ProcessId, Uri Uri)>();
        foreach (var (processId, uri) in this.discoveredApis)
        {
            if (predicate(processId))
            {
                results.Add((processId, uri));
            }
        }

        return results.Count > 0 ? results : default;
    }

    public void RequestScan()
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
        var newApis = new Dictionary<int, Uri>();

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

            var (port, reportedPid) = task.Result;
            if (reportedPid is null)
            {
                continue;
            }

            // Convert the reported PID (Wine PID on Linux) to system PID
            var systemPid = this.pidProvider.ResolveSystemPid(reportedPid.Value, GuildWarsExecutable);
            var uri = new Uri($"http://localhost:{port}");

            scopedLogger.LogDebug(
                "Found Daybreak API on port {Port} with reported PID {ReportedPid} (system PID {SystemPid})",
                port,
                reportedPid.Value,
                systemPid);

            newApis[systemPid] = uri;
        }

        this.discoveredApis = newApis;
        scopedLogger.LogDebug("Port scan complete. Found {Count} Daybreak API instance(s)", newApis.Count);
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
