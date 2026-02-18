using System.Collections.Concurrent;
using System.Extensions.Core;
using System.Numerics;
using System.Runtime.InteropServices;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Services.Screenshots;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static Daybreak.Shared.Models.Themes.Theme;
using Color = System.Drawing.Color;

namespace Daybreak.Services.Screenshots;

public sealed class ScreenshotService(
    ILogger<ScreenshotService> logger)
    : IScreenshotService
{
    private const string GuildWarsFolder = "Guild Wars";
    private const string ScreensFolder = "Screens";
    private const int SampleQuality = 10; // Sample every Nth pixel for performance

    private static readonly string ScreenshotsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), GuildWarsFolder, ScreensFolder, "");
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
            using var image = Image.Load<Rgba32>(fileStream);
            
            var (dominantColor, isDark) = GetDominantColor(image);
            var closestAccent = GetClosestAccentColor(dominantColor);
            var entry = new ScreenshotEntry(path, closestAccent, isDark ? LightDarkMode.Dark : LightDarkMode.Light);
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

    private static (Color Color, bool IsDark) GetDominantColor(Image<Rgba32> image)
    {
        long totalR = 0, totalG = 0, totalB = 0;
        int pixelCount = 0;

        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y += SampleQuality)
            {
                var rowSpan = accessor.GetRowSpan(y);
                var byteSpan = MemoryMarshal.AsBytes(rowSpan);

                // Process using SIMD where possible
                // Each Rgba32 pixel is 4 bytes: R, G, B, A
                var vectorSize = Vector<byte>.Count;
                var pixelsPerVector = vectorSize / 4; // 4 bytes per pixel

                int x = 0;

                // SIMD path: process multiple sampled pixels at once
                if (Vector.IsHardwareAccelerated && rowSpan.Length >= pixelsPerVector * SampleQuality)
                {
                    var sumR = Vector<long>.Zero;
                    var sumG = Vector<long>.Zero;
                    var sumB = Vector<long>.Zero;
                    int simdPixelCount = 0;

                    // Collect sampled pixels for SIMD processing
                    while (x + (Vector<int>.Count * SampleQuality) <= rowSpan.Length)
                    {
                        // Gather sampled pixels into arrays for vectorization
                        Span<int> rValues = stackalloc int[Vector<int>.Count];
                        Span<int> gValues = stackalloc int[Vector<int>.Count];
                        Span<int> bValues = stackalloc int[Vector<int>.Count];

                        for (int i = 0; i < Vector<int>.Count && x < rowSpan.Length; i++, x += SampleQuality)
                        {
                            var pixel = rowSpan[x];
                            rValues[i] = pixel.R;
                            gValues[i] = pixel.G;
                            bValues[i] = pixel.B;
                            simdPixelCount++;
                        }

                        var rVec = new Vector<int>(rValues);
                        var gVec = new Vector<int>(gValues);
                        var bVec = new Vector<int>(bValues);

                        // Widen to long and accumulate
                        Vector.Widen(rVec, out var rLow, out var rHigh);
                        Vector.Widen(gVec, out var gLow, out var gHigh);
                        Vector.Widen(bVec, out var bLow, out var bHigh);

                        sumR += rLow + rHigh;
                        sumG += gLow + gHigh;
                        sumB += bLow + bHigh;
                    }

                    // Sum up the vector lanes
                    for (int i = 0; i < Vector<long>.Count; i++)
                    {
                        totalR += sumR[i];
                        totalG += sumG[i];
                        totalB += sumB[i];
                    }
                    pixelCount += simdPixelCount;
                }

                // Scalar fallback for remaining pixels
                for (; x < rowSpan.Length; x += SampleQuality)
                {
                    var pixel = rowSpan[x];
                    totalR += pixel.R;
                    totalG += pixel.G;
                    totalB += pixel.B;
                    pixelCount++;
                }
            }
        });

        if (pixelCount == 0)
        {
            return (Color.Black, true);
        }

        var avgR = (byte)(totalR / pixelCount);
        var avgG = (byte)(totalG / pixelCount);
        var avgB = (byte)(totalB / pixelCount);

        var color = Color.FromArgb(255, avgR, avgG, avgB);
        
        // Calculate perceived brightness using standard formula
        // Values > 128 are generally considered "light"
        var brightness = (0.299 * avgR) + (0.587 * avgG) + (0.114 * avgB);
        var isDark = brightness < 128;

        return (color, isDark);
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
}
