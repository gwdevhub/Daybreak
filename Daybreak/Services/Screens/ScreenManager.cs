using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Windows;
using System.Windows.Extensions.Services;
using System.Windows.Media;
using Windows.Foundation;
using WpfExtended.Blazor.Launch;

namespace Daybreak.Services.Screens;

internal sealed class ScreenManager(
    BlazorHostWindow host,
    ILiveUpdateableOptions<ScreenManagerOptions> liveUpdateableOptions,
    ILogger<ScreenManager> logger) : IScreenManager, IApplicationLifetimeService
{
    private readonly BlazorHostWindow host = host.ThrowIfNull();
    private readonly ILiveUpdateableOptions<ScreenManagerOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<ScreenManager> logger = logger.ThrowIfNull();

    public IEnumerable<Screen> Screens { get; } = WpfScreenHelper.Screen.AllScreens
        .Select((screen, index) => new Screen(index, new Windows.Foundation.Rect(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height)));

    public void MoveWindowToSavedPosition()
    {
        var screenOptions = this.liveUpdateableOptions.Value;
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
        var desiredRect = new System.Windows.Rect(desiredX, desiredY, desiredWidth, desiredHeight);
        foreach(var screen in this.Screens)
        {
            // TODO:
            //if (desiredRect.IntersectsWith(screen.Size))
            //{
            //    validPosition = true;
            //    break;
            //}

            validPosition = true;
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
        var config = this.liveUpdateableOptions.Value;
        config.X = this.host.Left;
        config.Y = this.host.Top;
        config.Width = this.host.ActualWidth;
        config.Height = this.host.ActualHeight;
        config.DpiX = dpiScale.DpiScaleX;
        config.DpiY = dpiScale.DpiScaleY;
        this.liveUpdateableOptions.UpdateOption();
    }

    public bool MoveGuildwarsToScreen(Screen screen)
    {
        this.logger.LogDebug("Attempting to move guildwars to screen {screenId}", screen.Id);
        var hwnd = GetMainWindowHandle();
        if (hwnd.HasValue is false)
        {
            return false;
        }

        NativeMethods.SetWindowPos(hwnd.Value, NativeMethods.HWND_TOP, screen.Size.Left.ToInt(), screen.Size.Top.ToInt(), screen.Size.Width.ToInt(), screen.Size.Height.ToInt(), NativeMethods.SWP_SHOWWINDOW);
        return true;
    }

    private static IntPtr? GetMainWindowHandle()
    {
        var process = Process.GetProcessesByName("gw").FirstOrDefault();
        return process is not null ? process.MainWindowHandle : default;
    }

    public void OnStartup()
    {
        //this.host.WindowParametersChanged += (_, _) => this.SaveWindowPositionAndSize();
    }

    public void OnClosing()
    {
        this.SaveWindowPositionAndSize();
    }
}
