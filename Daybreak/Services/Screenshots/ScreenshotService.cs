using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Services.Screenshots;
using static Daybreak.Shared.Models.Theme;

namespace Daybreak.Services.Screenshots;

public sealed class ScreenshotService : IScreenshotService
{
    private const string GuildWarsFolder = "Guild Wars";
    private const string ScreensFolder = "Screens";

    private static readonly string ScreenshotsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), GuildWarsFolder, ScreensFolder, "");
    private static readonly ColorThiefDotNet.ColorThief ColorThief = new();
    private static readonly ConcurrentDictionary<string, ScreenshotEntry> EntryCache = [];

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

        return GetEntryFromPath(selectedScreenshot);
    }

    private static ScreenshotEntry? GetEntryFromPath(string path)
    {
        if (EntryCache.TryGetValue(path, out var cachedEntry))
        {
            return cachedEntry;
        }

        using var bitmap = GetBitmap(path);
        if (bitmap is null)
        {
            return default;
        }

        var dominantColor = ColorThief.GetColor(bitmap, quality: 2);
        var closestAccent = GetClosestAccentColor(Color.FromArgb(dominantColor.Color.A, dominantColor.Color.R, dominantColor.Color.G, dominantColor.Color.B));
        var entry = new ScreenshotEntry(path, closestAccent, dominantColor.IsDark ? LightDarkMode.Dark : LightDarkMode.Light);
        EntryCache[path] = entry;
        return entry;
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
