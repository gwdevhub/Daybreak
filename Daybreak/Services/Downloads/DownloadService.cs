using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Metrics;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics.Metrics;
using System.Extensions.Core;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Downloads;

internal sealed class DownloadService : IDownloadService
{
    private const double StatusUpdateInterval = 50;
    private const string MetricUnits = "bytes/sec";
    private const string MetricDescription = "Average download speed. Specified in bytes per second";

    private readonly Histogram<double> averageDownloadSpeed;
    private readonly IHttpClient<DownloadService> httpClient;
    private readonly ILogger<DownloadService> logger;

    public DownloadService(
        IMetricsService metricsService,
        IHttpClient<DownloadService> httpClient,
        ILogger<DownloadService> logger)
    {
        this.averageDownloadSpeed = metricsService.ThrowIfNull().CreateHistogram<double>(nameof(DownloadService), MetricUnits, MetricDescription, AggregationTypes.NoAggregate);
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<bool> DownloadFile(string downloadUri, string destinationPath, DownloadStatus downloadStatus, CancellationToken cancellationToken = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        downloadStatus.CurrentStep = DownloadStatus.InitializingDownload;
        using var response = await this.httpClient.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead);
        if (response.IsSuccessStatusCode is false)
        {
            downloadStatus.CurrentStep = DownloadStatus.FailedDownload;
            scopedLogger.LogError($"Failed to download installer. Status: {response.StatusCode}. Details: {await response.Content.ReadAsStringAsync()}");
            return false;
        }

        using var downloadStream = await this.httpClient.GetStreamAsync(downloadUri);
        this.logger.LogDebug("Beginning download");
        var fileInfo = new FileInfo(destinationPath);
        fileInfo.Directory?.Create();
        var fileStream = File.Open(destinationPath, FileMode.Create, FileAccess.Write);
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
            await fileStream.WriteAsync(buffer, 0, length, cancellationToken);
            if ((DateTime.Now - tickTime).TotalMilliseconds > StatusUpdateInterval)
            {
                tickTime = DateTime.Now;
                var downloadedInSecond = downloadedPerTimeframe * 1000d / StatusUpdateInterval;
                this.averageDownloadSpeed.Record(downloadedInSecond);

                var avgSpeed = downloaded / (tickTime - startTime).TotalSeconds;
                var remainingSize = downloadSize - downloaded;
                var secondsRemaining = remainingSize / avgSpeed;
                downloadedPerTimeframe = 0d;
                downloadStatus.CurrentStep = DownloadStatus.Downloading(
                    downloaded / downloadSize,
                    double.IsFinite(secondsRemaining) ?
                        TimeSpan.FromSeconds(secondsRemaining) :
                        TimeSpan.Zero);
            }
        }

        downloadStatus.CurrentStep = DownloadStatus.Downloading(1, TimeSpan.Zero);
        downloadStatus.CurrentStep = DownloadStatus.DownloadFinished;
        fileStream.Close();
        scopedLogger.LogDebug("Downloaded file");
        return true;
    }
}
