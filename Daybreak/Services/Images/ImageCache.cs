using Daybreak.Models.Metrics;
using Daybreak.Services.Images.Models;
using Daybreak.Services.Metrics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.IO;
using System.Logging;
using System.Net.Cache;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Images;

public sealed class ImageCache : IImageCache
{
    private const string LatencyMetricName = "Image retrieval latency";
    private const string LatencyMetricDescription = "Number of milliseconds spent while retrieving images";
    private const string LatencyMetricUnitName = "ms";
    private const string CacheSizeMetricName = "Image cache size";
    private const string CacheSizeMetricDescription = "Size of the image cache in bytes";
    private const string CacheSizeMetricUnitName = "bytes";
    private const double MaxCacheSize = 10e7; // 100mbs maximum image cache size

    private static readonly object CacheLock = new();

    private readonly ILogger<ImageCache> logger;
    private readonly Dictionary<string, ImageEntry> imageEntryCache = new();
    private readonly Histogram<double> imageRetrievalLatency;
    private readonly Histogram<double> imageCacheSize;
    private double currentCacheSize = 0;

    public ImageCache(
        IMetricsService metricsService,
        ILogger<ImageCache> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.imageRetrievalLatency = metricsService.ThrowIfNull()
            .CreateHistogram<double>(LatencyMetricName, LatencyMetricUnitName, LatencyMetricDescription, AggregationTypes.NoAggregate);
        this.imageCacheSize = metricsService.ThrowIfNull()
            .CreateHistogram<double>(CacheSizeMetricName, CacheSizeMetricUnitName, CacheSizeMetricDescription, AggregationTypes.NoAggregate);
    }

    public ImageSource? GetImage(string? uri)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetImage), uri?.ToString() ?? string.Empty);
        if (uri is null)
        {
            return default;
        }

        try
        {
            lock (CacheLock)
            {
                var imageSource = this.GetImageInternal(uri, scopedLogger);
                return imageSource;
            }
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return default;
        }
    }

    private ImageSource GetImageInternal(string uri, ScopedLogger<ImageCache> scopedLogger)
    {
        var stopwatch = Stopwatch.StartNew();
        if (this.imageEntryCache.TryGetValue(uri, out var entry))
        {
            this.imageRetrievalLatency.Record(stopwatch.ElapsedMilliseconds);
            this.imageCacheSize.Record(this.currentCacheSize);
            scopedLogger.LogInformation("Found image in cache. Returning");
            return entry.ImageSource;
        }

        var fileInfo = new FileInfo(uri);
        if (this.currentCacheSize + fileInfo.Length > MaxCacheSize)
        {
            var spaceToFree = (this.currentCacheSize + fileInfo.Length) - MaxCacheSize;
            scopedLogger.LogInformation($"Exceeding cache size. Freeing up {spaceToFree} bytes");
            this.FreeCache(spaceToFree);
            scopedLogger.LogInformation($"New cache size: {this.currentCacheSize} bytes");
        }

        var imageEntry = this.AddToCache(uri);
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

    private ImageEntry AddToCache(string uri)
    {
        var memoryStream = new MemoryStream(File.ReadAllBytes(uri));
        this.currentCacheSize += memoryStream.Length;
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memoryStream;
        bitmapImage.EndInit();

        var imageEntry = new ImageEntry
        {
            ImageSource = bitmapImage,
            Size = memoryStream.Length,
            Uri = uri
        };

        this.imageEntryCache[uri] = imageEntry;
        return imageEntry;
    }
}
