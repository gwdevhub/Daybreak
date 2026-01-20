using System.Collections.Concurrent;
using System.Drawing;
using System.Extensions.Core;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Services.Screenshots;
using Microsoft.Extensions.Logging;
using static Daybreak.Shared.Models.Themes.Theme;

namespace Daybreak.Services.Screenshots;

public sealed class ScreenshotService(
    ILogger<ScreenshotService> logger)
    : IScreenshotService
{
    private const string GuildWarsFolder = "Guild Wars";
    private const string ScreensFolder = "Screens";

    private static readonly string ScreenshotsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), GuildWarsFolder, ScreensFolder, "");
    private static readonly ColorThiefDotNet.ColorThief ColorThief = new();
    private static readonly ConcurrentDictionary<string, ScreenshotEntry> EntryCache = [];

    private readonly ILogger<ScreenshotService> logger = logger;

    public Task<ScreenshotEntry?> GetRandomScreenshot(CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(this.GetRandomScreenshotInternal, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
    }

    private async Task<ScreenshotEntry?> GetRandomScreenshotInternal()
    {
        if (!Directory.Exists(ScreenshotsDirectory))
        {
            return default;
        }

        var screenshots = Directory.GetFiles(ScreenshotsDirectory, "*", SearchOption.AllDirectories);
        if (screenshots is null || screenshots.Length is 0)
        {
            return default;
        }

        var selectedScreenshot = screenshots.Skip(Random.Shared.Next(0, screenshots.Length)).FirstOrDefault();
        if (selectedScreenshot is null)
        {
            return default;
        }

        return this.GetEntryFromPath(selectedScreenshot);
    }

    private ScreenshotEntry? GetEntryFromPath(string path)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (EntryCache.TryGetValue(path, out var cachedEntry))
        {
            scopedLogger.LogDebug("Screenshot entry found in cache for path {path}", path);
            return cachedEntry;
        }

        try
        {
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var bitmap = new Bitmap(fileStream);
            var dominantColor = ColorThief.GetColor(bitmap, quality: 2);
            var closestAccent = GetClosestAccentColor(Color.FromArgb(dominantColor.Color.A, dominantColor.Color.R, dominantColor.Color.G, dominantColor.Color.B));
            var entry = new ScreenshotEntry(path, closestAccent, dominantColor.IsDark ? LightDarkMode.Dark : LightDarkMode.Light);
            EntryCache[path] = entry;
            scopedLogger.LogDebug("Screenshot entry created and cached for path {path}", path);
            return entry;
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to get screenshot entry from path {path}", path);
            return default;
        }
    }

    private static AccentColor GetClosestAccentColor(Color color)
    {
        var closest = AccentColor.Accents[0];
        var minDistance = double.MaxValue;
        foreach (var accent in AccentColor.Accents)
        {
            double distance = GetColorDistance(color, accent.Color);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = accent;
            }
        }

        return closest;
    }

    private static double GetColorDistance(Color c1, Color c2)
    {
        var rDiff = c1.R - c2.R;
        var gDiff = c1.G - c2.G;
        var bDiff = c1.B - c2.B;
        return (rDiff * rDiff) + (gDiff * gDiff) + (bDiff * bDiff);
    }

    private static Bitmap? GetBitmap(string path)
    {
        var image = Bitmap.FromFile(path);
        if (image is not Bitmap bitmapImage)
        {
            return default;
        }

        return bitmapImage;
    }
}
