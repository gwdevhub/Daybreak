using System.Core.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Screens;
using Daybreak.Windows.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photino.NET;

namespace Daybreak.Windows.Services.Screens;

internal sealed class ScreenManager(
    PhotinoWindow photinoWindow,
    IOptionsProvider optionsProvider,
    IOptionsMonitor<ScreenManagerOptions> liveUpdateableOptions,
    IOptionsMonitor<LauncherOptions> liveUpdateableLauncherOptions,
    ILogger<ScreenManager> logger
) : IScreenManager, IHostedService
{
    private const int DefaultDpi = 96;

    private readonly PhotinoWindow photinoWindow = photinoWindow.ThrowIfNull();
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IOptionsMonitor<ScreenManagerOptions> liveUpdateableOptions =
        liveUpdateableOptions.ThrowIfNull();
    private readonly IOptionsMonitor<LauncherOptions> liveUpdateableLauncherOptions =
        liveUpdateableLauncherOptions.ThrowIfNull();
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
        if (!this.liveUpdateableLauncherOptions.CurrentValue.SaveWindowPosition)
        {
            this.ResetSavedPosition();
            return;
        }

        var position = new Rectangle(
            this.photinoWindow.Left,
            this.photinoWindow.Top,
            this.photinoWindow.Width,
            this.photinoWindow.Height
        );

        var currentDpi = GetDpiForPosition(position.Left, position.Top);

        var options = this.liveUpdateableOptions.CurrentValue;
        options.X = position.Left;
        options.Y = position.Top;
        options.Width = position.Width;
        options.Height = position.Height;
        options.DpiX = currentDpi.X;
        options.DpiY = currentDpi.Y;
        this.optionsProvider.SaveOption(options);
    }

    public void ResetSavedPosition()
    {
        var options = this.liveUpdateableOptions.CurrentValue;
        options.X = 0;
        options.Y = 0;
        options.Width = 0;
        options.Height = 0;
        options.DpiX = DefaultDpi;
        options.DpiY = DefaultDpi;
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

        NativeMethods.SetWindowPos(
            hwnd.Value,
            NativeMethods.HWND_TOP,
            screen.Size.Left,
            screen.Size.Top,
            screen.Size.Width,
            screen.Size.Height,
            NativeMethods.SWP_SHOWWINDOW
        );
        return true;
    }

    public Rectangle GetSavedPosition()
    {
        var savedOptions = this.liveUpdateableOptions.CurrentValue;
        var savedPosition = new Rectangle(
            (int)savedOptions.X,
            (int)savedOptions.Y,
            (int)savedOptions.Width,
            (int)savedOptions.Height
        );

        if (
            savedPosition.Width is 0
            || savedPosition.Height is 0
            || !this.liveUpdateableLauncherOptions.CurrentValue.SaveWindowPosition
        )
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
                firstScreen.Size.Height / 2
            );
        }

        // Adjust position based on DPI scaling changes
        var savedDpiX = savedOptions.DpiX > 0 ? savedOptions.DpiX : DefaultDpi;
        var savedDpiY = savedOptions.DpiY > 0 ? savedOptions.DpiY : DefaultDpi;
        var currentDpi = GetDpiForPosition(savedPosition.Left, savedPosition.Top);
        
        if (savedDpiX != currentDpi.X || savedDpiY != currentDpi.Y)
        {
            var scaleX = (double)currentDpi.X / savedDpiX;
            var scaleY = (double)currentDpi.Y / savedDpiY;
            savedPosition = new Rectangle(
                (int)(savedPosition.X * scaleX),
                (int)(savedPosition.Y * scaleY),
                (int)(savedPosition.Width * scaleX),
                (int)(savedPosition.Height * scaleY)
            );
        }

        // Validate that the position is within visible screen bounds
        savedPosition = this.EnsurePositionIsOnScreen(savedPosition);

        return savedPosition;
    }

    private Rectangle EnsurePositionIsOnScreen(Rectangle position)
    {
        var screens = this.Screens.ToList();
        if (screens.Count == 0)
        {
            return position;
        }

        // Check if the window center is within any screen
        var centerX = position.Left + (position.Width / 2);
        var centerY = position.Top + (position.Height / 2);

        var isOnScreen = screens.Any(s =>
            centerX >= s.Size.Left && centerX <= s.Size.Right &&
            centerY >= s.Size.Top && centerY <= s.Size.Bottom);

        if (isOnScreen)
        {
            return position;
        }

        // Window is off-screen, move to the first available screen
        var firstScreen = screens.First();
        return new Rectangle(
            firstScreen.Size.X + (firstScreen.Size.Width / 4),
            firstScreen.Size.Y + (firstScreen.Size.Height / 4),
            Math.Min(position.Width, firstScreen.Size.Width / 2),
            Math.Min(position.Height, firstScreen.Size.Height / 2)
        );
    }

    private static Point GetDpiForPosition(int x, int y)
    {
        var point = new NativeMethods.POINT { X = x, Y = y };
        var monitor = NativeMethods.MonitorFromPoint(point, NativeMethods.MONITOR_DEFAULTTONEAREST);
        
        if (monitor != IntPtr.Zero && 
            NativeMethods.GetDpiForMonitor(monitor, NativeMethods.MonitorDpiType.MDT_EFFECTIVE_DPI, out var dpiX, out var dpiY) == 0)
        {
            return new Point(dpiX, dpiY);
        }

        return new Point(DefaultDpi, DefaultDpi);
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

        NativeMethods.EnumDisplayMonitors(
            IntPtr.Zero,
            IntPtr.Zero,
            (hMonitor, hdcMonitor, ref lprcMonitor, dwData) =>
            {
                var monitorInfo = new NativeMethods.MonitorInfoEx
                {
                    CbSize = (uint)Marshal.SizeOf<NativeMethods.MonitorInfoEx>(),
                };

                if (NativeMethods.GetMonitorInfo(hMonitor, ref monitorInfo))
                {
                    screens.Add(
                        new Screen(
                            index++,
                            new Rectangle(
                                monitorInfo.RcMonitor.Left,
                                monitorInfo.RcMonitor.Top,
                                monitorInfo.RcMonitor.Width,
                                monitorInfo.RcMonitor.Height
                            )
                        )
                    );
                }

                return true;
            },
            IntPtr.Zero
        );

        return screens;
    }
}
