using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.Themes;
using Daybreak.Shared.Services.Screenshots;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Shared.Services.Themes;

/// <summary>
/// This is a custom theme that cycles through game screenshots periodically.
/// Implementation is kinda hacky atm and should be improved later.
/// </summary>
public sealed class GameScreenshotsTheme(
    IThemeManager themeManager,
    IScreenshotService screenshotService,
    ILogger<GameScreenshotsTheme> logger)
    : Theme(ThemeName, AccentColor.Orange, new StaticBackground(string.Empty), LightDarkMode.SystemSynchronized, string.Empty), IHostedService
{
    public const string ThemeName = "Dynamic Screenshots";

    private readonly IScreenshotService screenshotService = screenshotService;
    private readonly IThemeManager themeManager = themeManager;
    private readonly ILogger<GameScreenshotsTheme> logger = logger;

    private CancellationTokenSource? cancellationTokenSource;

    private async Task PeriodicallyUpdateGameScreenshots(CancellationToken cancellationToken)
    {
        /*
         * The theme needs to always cycle through entries, so that when the user selects this theme, it's already initialized.
         * We rely on ScreenshotService to cache screenshot entries in memory.
         */
        var scopedLogger = this.logger.CreateScopedLogger();
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var entry = await this.screenshotService.GetRandomScreenshot(cancellationToken);
                if (entry is null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
                    continue;
                }

                this.AccentColor = entry.AccentColor;
                this.Background = new StaticBackground(entry.FilePath);
                this.Mode = entry.LightDarkMode;
                this.Filter = entry.LightDarkMode is LightDarkMode.Light
                    ? "blur(3px) brightness(1.3) sepia(0.2) saturate(1.2)"
                    : "blur(2px) brightness(1.2) hue-rotate(10deg) saturate(1.1)";
                if (this == this.themeManager.CurrentTheme)
                {
                    this.themeManager.ReapplyTheme();
                }

                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "An error occurred while updating game screenshots theme.");
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        Task.Factory.StartNew(() => this.PeriodicallyUpdateGameScreenshots(this.cancellationTokenSource.Token), this.cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
        return Task.CompletedTask;
    }
}
