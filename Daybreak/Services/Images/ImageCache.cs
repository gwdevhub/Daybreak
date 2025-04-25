using Daybreak.Configuration.Options;
using Daybreak.Models.Metrics;
using Daybreak.Services.Images.Models;
using Daybreak.Services.Metrics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Extensions;
using System.IO;
using System.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Images;

internal sealed class ImageCache(
    IMetricsService metricsService,
    ILiveOptions<ImageCacheOptions> options,
    ILogger<ImageCache> logger) : IImageCache
{
    private const string LatencyMetricName = "Image retrieval latency";
    private const string LatencyMetricDescription = "Number of milliseconds spent while retrieving images";
    private const string LatencyMetricUnitName = "ms";
    private const string CacheSizeMetricName = "Image cache size";
    private const string CacheSizeMetricDescription = "Size of the image cache in bytes";
    private const string CacheSizeMetricUnitName = "bytes";

    private readonly SemaphoreSlim cacheSemaphore = new(1, 1);
    private readonly ILiveOptions<ImageCacheOptions> options = options.ThrowIfNull();
    private readonly ILogger<ImageCache> logger = logger.ThrowIfNull();
    private readonly ConcurrentDictionary<string, ImageEntry> imageEntryCache = new();
    private readonly Histogram<double> imageRetrievalLatency = metricsService.ThrowIfNull()
            .CreateHistogram<double>(LatencyMetricName, LatencyMetricUnitName, LatencyMetricDescription, AggregationTypes.NoAggregate);
    private readonly Histogram<double> imageCacheSize = metricsService.ThrowIfNull()
            .CreateHistogram<double>(CacheSizeMetricName, CacheSizeMetricUnitName, CacheSizeMetricDescription, AggregationTypes.NoAggregate);
    private double currentCacheSize = 0;

    public async Task<ImageSource?> GetImage(string? uri)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetImage), uri?.ToString() ?? string.Empty);
        if (uri is null ||
            !File.Exists(uri))
        {
            return default;
        }

        try
        {
            var imageSource = await new TaskFactory().StartNew(() => this.GetImageInternal(uri, scopedLogger), TaskCreationOptions.LongRunning).Unwrap();
            return imageSource;
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return default;
        }
    }

    private async Task<ImageSource> GetImageInternal(string uri, ScopedLogger<ImageCache> scopedLogger)
    {
        var stopwatch = Stopwatch.StartNew();
        if (this.imageEntryCache.TryGetValue($"{uri}", out var entry))
        {
            this.imageRetrievalLatency.Record(stopwatch.ElapsedMilliseconds);
            this.imageCacheSize.Record(this.currentCacheSize);
            scopedLogger.LogInformation("Found image in cache. Returning");
            return entry.ImageSource;
        }

        await this.cacheSemaphore.WaitAsync();
        var fileInfo = new FileInfo(uri);
        if (this.currentCacheSize + fileInfo.Length > this.options.Value.MemoryImageCacheLimit * 10e5)
        {
            var spaceToFree = this.currentCacheSize + fileInfo.Length - (this.options.Value.MemoryImageCacheLimit * 10e5);
            scopedLogger.LogInformation($"Exceeding cache size. Freeing up {spaceToFree} bytes");
            this.FreeCache(spaceToFree);
            scopedLogger.LogInformation($"New cache size: {this.currentCacheSize} bytes");
        }

        var imageEntry = await this.AddToCache(uri);
        this.cacheSemaphore.Release();
        this.imageRetrievalLatency.Record(stopwatch.ElapsedMilliseconds);
        this.imageCacheSize.Record(this.currentCacheSize);
        scopedLogger.LogInformation("Added image to cache");
        return imageEntry.ImageSource;
    }

    private void FreeCache(double spaceToFree)
    {
        var freedSpace = 0d;
        var imagesToRemove = new List<ImageEntry>();
        foreach(var imageToRemove in this.imageEntryCache.Values)
        {
            if (freedSpace >= spaceToFree)
            {
                break;
            }

            freedSpace += imageToRemove.Size;
            imagesToRemove.Add(imageToRemove);
        }

        foreach(var imageToRemove in imagesToRemove)
        {
            this.imageEntryCache.Remove(imageToRemove.Uri, out _);
        }

        this.currentCacheSize -= freedSpace;
    }

    private async Task<ImageEntry> AddToCache(string uri)
    {
        using var fileStream = File.OpenRead(uri);
        return await Task.Run(() =>
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = fileStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            var size = bitmapImage.Format.BitsPerPixel / 8 * bitmapImage.PixelWidth * bitmapImage.PixelHeight;
            var imageEntry = new ImageEntry
            {
                ImageSource = bitmapImage,
                Size = size,
                Uri = uri
            };

            this.imageEntryCache[uri] = imageEntry;
            this.currentCacheSize += size;
            return imageEntry;
        });
    }

    private static BitmapSource BitmapToBitmapSource(Bitmap bmp)
    {
        var bitmapData = bmp.LockBits(
               new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadOnly, bmp.PixelFormat);

        var bitmapSource = BitmapSource.Create(
            bitmapData.Width, bitmapData.Height,
            bmp.HorizontalResolution, bmp.VerticalResolution,
            PixelFormats.Bgr24, null,
            bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

        bmp.UnlockBits(bitmapData);

        return bitmapSource;
    }
}
