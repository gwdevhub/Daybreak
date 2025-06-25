using Daybreak.Shared.Services.Images;
using Daybreak.Shared.Services.Screenshots;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Services.Screenshots;

internal sealed class ScreenshotProvider : IScreenshotProvider
{
    private readonly static string ScreenshotsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Guild Wars\\Screens";

    private readonly IImageCache imageCache;
    private readonly ILogger<ScreenshotProvider> logger;

    public ScreenshotProvider(
        IImageCache imageCache,
        ILogger<ScreenshotProvider> logger)
    {
        this.imageCache = imageCache.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        if (Directory.Exists(ScreenshotsPath) is false)
        {
            Directory.CreateDirectory(ScreenshotsPath);
        }
    }

    public async Task<ImageSource?> GetRandomScreenShot()
    {
        var screenshots = GetScreenshots();
        if (screenshots.Count == 0)
        {
            this.logger.LogDebug("Attempted to retrieve a random screenshot. No screenshots present");
            return default;
        }

        var screenShot = screenshots[Random.Shared.Next(0, screenshots.Count)];
        return await this.GetScreenshotInternal(screenShot);
    }

    private async Task<ImageSource?> GetScreenshotInternal(string name)
    {
        var screenshots = GetScreenshots();
        if (screenshots.Count == 0)
        {
            this.logger.LogDebug("Attempted to retrieve a random screenshot. No screenshots present");
            return default;
        }

        return await this.imageCache.GetImage(name);
    }

    private static IList<string> GetScreenshots()
    {
        return Directory.GetFiles(ScreenshotsPath);
    }
}
