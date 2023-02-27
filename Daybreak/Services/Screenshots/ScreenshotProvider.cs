using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Screenshots;

public sealed class ScreenshotProvider : IScreenshotProvider
{
    private const string ScreenshotsFolder = "Screenshots";

    private readonly List<string> Screenshots = new();
    private readonly ILogger<ScreenshotProvider> logger;
    private int innerCount = 0;

    public ScreenshotProvider(ILogger<ScreenshotProvider> logger)
    {
        this.logger = logger.ThrowIfNull(nameof(logger));
        if (Directory.Exists(ScreenshotsFolder) is false)
        {
            Directory.CreateDirectory(ScreenshotsFolder);
        }
    }

    public Optional<ImageSource> GetRandomScreenShot()
    {
        if (this.Screenshots.Count == 0)
        {
            this.logger.LogWarning("Attempted to retrieve a random screenshot. No screenshots present");
            return Optional.None<ImageSource>();
        }

        var screenshot = this.Screenshots[this.innerCount++ % this.Screenshots.Count];
        return Optional.FromValue<ImageSource>(new BitmapImage(new Uri(Path.GetFullPath(screenshot))));
    }

    public Optional<ImageSource> GetScreenshot(string name)
    {
        if (this.Screenshots.Count == 0)
        {
            this.logger.LogWarning("Attempted to retrieve a random screenshot. No screenshots present");
            return Optional.None<ImageSource>();
        }

        var screenshot = this.Screenshots.FirstOrDefault(s => s == name);
        if (screenshot == default)
        {
            this.logger.LogWarning($"No screenshot found with name {name}");
            return Optional.None<ImageSource>();
        }

        return Optional.FromValue<ImageSource>(new BitmapImage(new Uri(Path.GetFullPath(screenshot))));
    }

    public void OnClosing()
    {
    }

    public void OnStartup()
    {
        this.LoadScreenshots();
    }

    private void LoadScreenshots()
    {
        var rand = new Random();
        var fileList = Directory.GetFiles(ScreenshotsFolder);
        if (!fileList.Any())
        {
            this.logger.LogWarning($"No screenshots found in {ScreenshotsFolder}");
            return;
        }

        this.logger.LogInformation($"Loaded {fileList.Length} screenshots");
        this.Screenshots.AddRange(fileList.OrderBy(s => rand.Next()));
    }
}
