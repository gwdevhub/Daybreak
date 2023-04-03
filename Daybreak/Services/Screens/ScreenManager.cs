using Daybreak.Configuration;
using Daybreak.Launch;
using Daybreak.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Extensions.Services;
using System.Windows.Media;

namespace Daybreak.Services.Screens;

public sealed class ScreenManager : IScreenManager, IApplicationLifetimeService
{
    private readonly MainWindow host;
    private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions;
    private readonly ILogger<ScreenManager> logger;

    public IEnumerable<Screen> Screens { get; } = WpfScreenHelper.Screen.AllScreens
        .Select((screen, index) => new Screen { Id = index, Size = screen.Bounds });

    public ScreenManager(
        MainWindow host,
        ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions,
        ILogger<ScreenManager> logger)
    {
        this.host = host.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public void MoveWindowToSavedPosition()
    {
        var screenOptions = this.liveUpdateableOptions.Value.ScreenManagerOptions;
        var dpiScale = VisualTreeHelper.GetDpi(this.host);
        var desiredX = screenOptions.X;
        var desiredY = screenOptions.Y;
        var desiredWidth = screenOptions.Width;
        var desiredHeight = screenOptions.Height;
        if (screenOptions.DpiX > 0 && screenOptions.DpiY > 0)
        {
            desiredX *= dpiScale.DpiScaleX / screenOptions.DpiX;
            desiredY *= dpiScale.DpiScaleY / screenOptions.DpiY;
            desiredWidth *= dpiScale.DpiScaleX / screenOptions.DpiX;
            desiredHeight *= dpiScale.DpiScaleY / screenOptions.DpiY;
        }

        // Validate that the desired position will be at least partially on screen.
        var validPosition = false;
        foreach(var screen in this.Screens)
        {
            if (desiredX < screen.Size.Right - 10 &&
                desiredY < screen.Size.Bottom - 10)
            {
                validPosition = true;
                break;
            }
        }

        // If the desired window position is not valid, we let the window use the default values.
        if (validPosition is true)
        {
            this.host.Left = desiredX;
            this.host.Top = desiredY;
        }

        // If the desired size of the window is not valid, we let the window use the default values.
        if (desiredWidth > 0 &&
            desiredHeight > 0)
        {
            this.host.Width = desiredWidth;
            this.host.Height = desiredHeight;
        }
    }

    public void SaveWindowPositionAndSize()
    {
        if (this.host.WindowState is not WindowState.Normal)
        {
            return;
        }

        var dpiScale = VisualTreeHelper.GetDpi(this.host);
        var options = new ScreenManagerOptions
        {
            X = this.host.Left,
            Y = this.host.Top,
            Width = this.host.ActualWidth,
            Height = this.host.ActualHeight,
            DpiX = dpiScale.DpiScaleX,
            DpiY = dpiScale.DpiScaleY
        };

        this.liveUpdateableOptions.Value.ScreenManagerOptions = options;
        this.liveUpdateableOptions.UpdateOption();
    }

    public void MoveGuildwarsToScreen(Screen screen)
    {
        this.logger.LogInformation($"Attempting to move guildwars to screen {screen.Id}");
        var hwnd = GetMainWindowHandle();
        NativeMethods.SetWindowPos(hwnd, NativeMethods.HWND_TOP, screen.Size.Left.ToInt(), screen.Size.Top.ToInt(), screen.Size.Width.ToInt(), screen.Size.Height.ToInt(), NativeMethods.SWP_SHOWWINDOW);
    }

    private static IntPtr GetMainWindowHandle()
    {
        var process = Process.GetProcessesByName("gw").FirstOrDefault();
        return process is not null ? process.MainWindowHandle : throw new InvalidOperationException("Could not find guildwars process");
    }

    public void OnStartup()
    {
        this.host.WindowParametersChanged += (_, _) => this.SaveWindowPositionAndSize();
        this.MoveWindowToSavedPosition();
    }

    public void OnClosing()
    {
        this.SaveWindowPositionAndSize();
    }
}
