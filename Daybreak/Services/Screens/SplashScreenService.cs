﻿using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Models;
using Daybreak.Services.Themes;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Linq;
using System.Windows;

namespace Daybreak.Services.Screens;

public sealed class SplashScreenService : ISplashScreenService
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
        this.SetupThemeResources();
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
        if (targetScreen is not null)
        {
            this.splashWindow.Left = targetScreen.Size.Left + targetScreen.Size.Width / 2 - this.splashWindow.Width / 2;
            this.splashWindow.Top = targetScreen.Size.Top + targetScreen.Size.Height / 2 - this.splashWindow.Height / 2;
        }

        this.splashWindow.Show();
        this.splashWindow.Topmost = true;
    }

    private void SetupThemeResources()
    {
        var theme = this.themeManager.GetCurrentTheme();
        foreach(var key in theme.Resources.Keys)
        {
            this.splashWindow.Resources.Add(key, theme.Resources[key]);
        }
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
        .Select((screen, index) => new Screen { Id = index, Size = screen.Bounds });
}
