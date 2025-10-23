using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Metrics;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics.Metrics;
using System.Extensions.Core;
using System.IO;
using System.Net.Http;

namespace Daybreak.Services.Downloads;

internal sealed class DownloadService(
    IMetricsService metricsService,
    IHttpClient<DownloadService> httpClient,
    ILogger<DownloadService> logger) : IDownloadService
{
    private const double StatusUpdateInterval = 50;
    private const string MetricUnits = "bytes/sec";
    private const string MetricDescription = "Average download speed. Specified in bytes per second";

    private readonly static ProgressUpdate ProgressInitialize = new(0, "Initializing download");
    private readonly static ProgressUpdate ProgressFailed = new(1, "Download failed");
    private readonly static ProgressUpdate ProgressCompleted = new(1, "Download finished");
    private static ProgressUpdate ProgressDownload(double progress) => new(progress, "Downloading");

    private readonly Histogram<double> averageDownloadSpeed = metricsService.ThrowIfNull().CreateHistogram<double>(nameof(DownloadService), MetricUnits, MetricDescription, AggregationTypes.NoAggregate);
    private readonly IHttpClient<DownloadService> httpClient = httpClient.ThrowIfNull();
    private readonly ILogger<DownloadService> logger = logger.ThrowIfNull();

    public async Task<bool> DownloadFile(string downloadUri, string destinationPath, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        progress.Report(ProgressInitialize);
        using var response = await this.httpClient.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (response.IsSuccessStatusCode is false)
        {
            progress.Report(ProgressFailed);
            scopedLogger.LogError($"Failed to download installer. Status: {response.StatusCode}. Details: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            return false;
        }

        using var downloadStream = await this.httpClient.GetStreamAsync(downloadUri);
        this.logger.LogDebug("Beginning download");
        var fileInfo = new FileInfo(destinationPath);
        fileInfo.Directory?.Create();
        using var fileStream = File.Open(destinationPath, FileMode.Create, FileAccess.Write);
        var downloadSize = response.Content?.Headers?.ContentLength ?? double.MaxValue;
        var buffer = new byte[1024];
        var length = 0;
        var downloaded = 0d;
        var downloadedPerTimeframe = 0d;
        var tickTime = DateTime.Now;
        var startTime = DateTime.Now;
        while (downloadStream.CanRead && (length = await downloadStream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            downloaded += length;
            downloadedPerTimeframe += length;
            await fileStream.WriteAsync(buffer.AsMemory(0, length), cancellationToken);
            if ((DateTime.Now - tickTime).TotalMilliseconds > StatusUpdateInterval)
            {
                tickTime = DateTime.Now;
                var downloadedInSecond = downloadedPerTimeframe * 1000d / StatusUpdateInterval;
                this.averageDownloadSpeed.Record(downloadedInSecond);

                // var avgSpeed = downloaded / (tickTime - startTime).TotalSeconds;
                // var remainingSize = downloadSize - downloaded;
                // var secondsRemaining = remainingSize / avgSpeed;
                downloadedPerTimeframe = 0d;
                progress.Report(ProgressDownload(downloaded / downloadSize));
            }
        }

        progress.Report(ProgressCompleted);
        scopedLogger.LogDebug("Downloaded file");
        return true;
    }
}
