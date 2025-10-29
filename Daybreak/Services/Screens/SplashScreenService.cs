using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Themes;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;

namespace Daybreak.Services.Screens;

internal sealed class SplashScreenService : ISplashScreenService
{
    private readonly IThemeManager themeManager;
    private readonly IOptions<ScreenManagerOptions> options;
    private readonly SplashWindow splashWindow;

    public SplashScreenService(
        IThemeManager themeManager,
        IOptions<ScreenManagerOptions> options,
        SplashWindow splashWindow)
    {
        this.themeManager = themeManager.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.splashWindow = splashWindow.ThrowIfNull();
    }

    public void HideSplashScreen()
    {
        this.splashWindow.Close();
    }

    public void ShowSplashScreen()
    {
        /*
         * Determine the screen where the launcher will show, in order to show the splash screen on the same screen
         */
        var launcherCoords = new Rect((int)this.options.Value.X, (int)this.options.Value.Y, (int)this.options.Value.Width, (int)this.options.Value.Height);
        var targetScreen = GetScreens().MaxBy(s => CalculateAreaOfRectIntersection(s.Size, launcherCoords));
        if (targetScreen.Size.Width > 0 && targetScreen.Size.Height > 0)
        {
            this.splashWindow.Left = targetScreen.Size.Left + (targetScreen.Size.Width / 2) - (this.splashWindow.Width / 2);
            this.splashWindow.Top = targetScreen.Size.Top + (targetScreen.Size.Height / 2) - (this.splashWindow.Height / 2);
        }

        this.splashWindow.Show();
    }

    private static double CalculateAreaOfRectIntersection(Rect rect1, Rect rect2)
    {
        var rect = rect1;
        rect.Intersect(rect2);
        if (rect.IsEmpty)
        {
            return 0;
        }

        return rect.Width * rect.Height;
    }

    private static IEnumerable<Screen> GetScreens() => WpfScreenHelper.Screen.AllScreens
        .Select((screen, index) => new Screen(index, screen.Bounds));
}
