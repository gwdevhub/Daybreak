using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photino.NET;
using System.Core.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;

namespace Daybreak.Services.Screens;

internal sealed class ScreenManager(
    PhotinoWindow photinoWindow,
    IOptionsProvider optionsProvider,
    IOptionsMonitor<ScreenManagerOptions> liveUpdateableOptions,
    ILogger<ScreenManager> logger) : IScreenManager, IHostedService
{
    private readonly PhotinoWindow photinoWindow = photinoWindow.ThrowIfNull();
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IOptionsMonitor<ScreenManagerOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<ScreenManager> logger = logger.ThrowIfNull();

    public IEnumerable<Screen> Screens => GetAllScreens();

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        // No need to move window on start here. Window position is set in the launcher before creation
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        this.SaveWindowPositionAndSize();
        return Task.CompletedTask;
    }

    public void MoveWindowToSavedPosition()
    {
        var savedPosition = this.GetSavedPosition();
        this.photinoWindow.Left = savedPosition.Left;
        this.photinoWindow.Top = savedPosition.Top;
        this.photinoWindow.Width = savedPosition.Width;
        this.photinoWindow.Height = savedPosition.Height;
    }

    public void SaveWindowPositionAndSize()
    {
        var position = new Rectangle(
            this.photinoWindow.Left,
            this.photinoWindow.Top,
            this.photinoWindow.Width,
            this.photinoWindow.Height);

        var options = this.liveUpdateableOptions.CurrentValue;
        options.X = position.Left;
        options.Y = position.Top;
        options.Width = position.Width;
        options.Height = position.Height;
        this.optionsProvider.SaveOption(options);
    }

    public void ResetSavedPosition()
    {
        var options = this.liveUpdateableOptions.CurrentValue;
        options.X = 0;
        options.Y = 0;
        options.Width = 0;
        options.Height = 0;
        this.optionsProvider.SaveOption(options);
    }

    public bool MoveGuildwarsToScreen(Screen screen)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Attempting to move guildwars to screen {screenId}", screen.Id);
        var hwnd = GetMainWindowHandle();
        if (!hwnd.HasValue)
        {
            return false;
        }

        NativeMethods.SetWindowPos(hwnd.Value, NativeMethods.HWND_TOP, screen.Size.Left, screen.Size.Top, screen.Size.Width, screen.Size.Height, NativeMethods.SWP_SHOWWINDOW);
        return true;
    }

    public Rectangle GetSavedPosition()
    {
        var savedPosition = new Rectangle(
            (int)this.liveUpdateableOptions.CurrentValue.X,
            (int)this.liveUpdateableOptions.CurrentValue.Y,
            (int)this.liveUpdateableOptions.CurrentValue.Width,
            (int)this.liveUpdateableOptions.CurrentValue.Height);

        if (savedPosition.Width is 0 || savedPosition.Height is 0)
        {
            var firstScreen = this.Screens.FirstOrDefault();
            if (firstScreen.Size.IsEmpty)
            {
                throw new InvalidOperationException("Could not detect any screen");
            }

            return new Rectangle(
                    firstScreen.Size.X + (firstScreen.Size.Width / 4),
                    firstScreen.Size.Y + (firstScreen.Size.Height / 4),
                    firstScreen.Size.Width / 2,
                    firstScreen.Size.Height / 2);
        }

        return savedPosition;
    }

    private static IntPtr? GetMainWindowHandle()
    {
        var process = Process.GetProcessesByName("gw").FirstOrDefault();
        return (process?.MainWindowHandle) ?? default;
    }

    private static List<Screen> GetAllScreens()
    {
        var screens = new List<Screen>();
        int index = 0;

        NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (hMonitor, hdcMonitor, ref lprcMonitor, dwData) =>
        {
            var monitorInfo = new NativeMethods.MonitorInfoEx
            {
                CbSize = (uint)Marshal.SizeOf<NativeMethods.MonitorInfoEx>()
            };

            if (NativeMethods.GetMonitorInfo(hMonitor, ref monitorInfo))
            {
                screens.Add(new Screen(index++, new Rectangle(monitorInfo.RcMonitor.Left, monitorInfo.RcMonitor.Top, monitorInfo.RcMonitor.Width, monitorInfo.RcMonitor.Height)));
            }

            return true;
        }, IntPtr.Zero);

        return screens;
    }
}
