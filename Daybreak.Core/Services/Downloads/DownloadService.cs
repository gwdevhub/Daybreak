using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Metrics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Diagnostics.Metrics;
using System.Extensions.Core;
using System.Logging;

namespace Daybreak.Services.Downloads;

internal sealed class DownloadService(
    IMetricsService metricsService,
    IHttpClient<DownloadService> httpClient,
    IOptionsMonitor<DownloadOptions> options,
    ILogger<DownloadService> logger) : IDownloadService
{
    private const double StatusUpdateInterval = 50;
    private const int BufferSize = 81920;
    private const string MetricName = "download.speed";
    private const string MetricUnits = "bytes/sec";
    private const string MetricDescription = "Average download speed. Specified in bytes per second";

    private readonly static ProgressUpdate ProgressInitialize = new(0, "Initializing download");
    private readonly static ProgressUpdate ProgressFailed = new(1, "Download failed");
    private readonly static ProgressUpdate ProgressCompleted = new(1, "Download finished");
    private static ProgressUpdate ProgressDownload(double progress) => new(progress, "Downloading");
    private static ProgressUpdate ProgressRetrying(int attempt, int retries) => new(0, $"Download failed. Retrying ({attempt}/{retries})");

    private readonly Histogram<double> averageDownloadSpeed = metricsService.ThrowIfNull().CreateHistogram<double>(MetricName, MetricUnits, MetricDescription, AggregationTypes.NoAggregate);
    private readonly IHttpClient<DownloadService> httpClient = httpClient.ThrowIfNull();
    private readonly IOptionsMonitor<DownloadOptions> options = options.ThrowIfNull();
    private readonly ILogger<DownloadService> logger = logger.ThrowIfNull();

    public async Task<bool> DownloadFile(string downloadUri, string destinationPath, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var currentOptions = this.options.CurrentValue;
        var retries = Math.Max(0, currentOptions.Retries);
        var attempts = retries + 1;

        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                if (await this.TryDownloadFile(downloadUri, destinationPath, progress, currentOptions, cancellationToken))
                {
                    progress.Report(ProgressCompleted);
                    scopedLogger.LogDebug("Downloaded file");
                    return true;
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // The caller gave up. The partial file is still removed, because leaving it behind
                // lets a later run mistake it for a complete download.
                DeletePartialDownload(destinationPath, scopedLogger);
                throw;
            }
            catch (OperationCanceledException)
            {
                scopedLogger.LogError("Download timed out on attempt {Attempt} of {Attempts}", attempt, attempts);
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "Download failed on attempt {Attempt} of {Attempts}", attempt, attempts);
            }

            DeletePartialDownload(destinationPath, scopedLogger);
            if (attempt < attempts)
            {
                progress.Report(ProgressRetrying(attempt, retries));
                if (currentOptions.RetryDelay > 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(currentOptions.RetryDelay), cancellationToken);
                }
            }
        }

        progress.Report(ProgressFailed);
        return false;
    }

    private async Task<bool> TryDownloadFile(
        string downloadUri,
        string destinationPath,
        IProgress<ProgressUpdate> progress,
        DownloadOptions currentOptions,
        CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        progress.Report(ProgressInitialize);

        // A server that accepts the connection but never answers would otherwise hang the launch.
        using var headerCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        headerCts.CancelAfter(TimeSpan.FromSeconds(currentOptions.HeaderTimeout));
        using var response = await this.httpClient.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead, headerCts.Token);
        if (response.IsSuccessStatusCode is false)
        {
            scopedLogger.LogError($"Failed to download installer. Status: {response.StatusCode}. Details: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            return false;
        }

        using var downloadStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        this.logger.LogDebug("Beginning download");
        var fileInfo = new FileInfo(destinationPath);
        fileInfo.Directory?.Create();
        var downloadSize = response.Content?.Headers?.ContentLength ?? double.MaxValue;
        var downloaded = 0d;
        var downloadedPerTimeframe = 0d;
        var tickTime = DateTime.Now;
        var chunkTimeout = TimeSpan.FromSeconds(currentOptions.ChunkTimeout);
        var buffer = new byte[BufferSize];

        // Scoped so the handle is released before a failed download is deleted.
        using (var fileStream = File.Open(destinationPath, FileMode.Create, FileAccess.Write))
        {
            while (downloadStream.CanRead)
            {
                // A stalled connection blocks inside ReadAsync indefinitely, so each chunk gets its own deadline.
                using var chunkCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                chunkCts.CancelAfter(chunkTimeout);
                var length = await downloadStream.ReadAsync(buffer, chunkCts.Token);
                if (length <= 0)
                {
                    break;
                }

                downloaded += length;
                downloadedPerTimeframe += length;
                await fileStream.WriteAsync(buffer.AsMemory(0, length), cancellationToken);
                if ((DateTime.Now - tickTime).TotalMilliseconds > StatusUpdateInterval)
                {
                    tickTime = DateTime.Now;
                    var downloadedInSecond = downloadedPerTimeframe * 1000d / StatusUpdateInterval;
                    this.averageDownloadSpeed.Record(downloadedInSecond);

                    downloadedPerTimeframe = 0d;
                    progress.Report(ProgressDownload(downloaded / downloadSize));
                }
            }
        }

        // Verify the full file was downloaded when Content-Length is known
        if (downloadSize != double.MaxValue && downloaded < downloadSize)
        {
            scopedLogger.LogError("Download incomplete. Expected {ExpectedSize} bytes but received {ActualSize} bytes", (long)downloadSize, (long)downloaded);
            return false;
        }

        return true;
    }

    private static void DeletePartialDownload(string destinationPath, ScopedLogger<DownloadService> scopedLogger)
    {
        try
        {
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to delete partial download at {DestinationPath}", destinationPath);
        }
    }
}
