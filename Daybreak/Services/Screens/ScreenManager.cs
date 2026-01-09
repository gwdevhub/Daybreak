using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.Extensions;
using System.Runtime.InteropServices;

namespace Daybreak.Services.Screens;

//TODO: Fix live updateable options usage
internal sealed class ScreenManager(
    //ILiveUpdateableOptions<ScreenManagerOptions> liveUpdateableOptions,
    ILogger<ScreenManager> logger) : IScreenManager, IHostedService
{
    //private readonly ILiveUpdateableOptions<ScreenManagerOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<ScreenManager> logger = logger.ThrowIfNull();

    public IEnumerable<Screen> Screens => GetAllScreens();

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        this.SaveWindowPositionAndSize();
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        this.SaveWindowPositionAndSize();
        return Task.CompletedTask;
    }

    public void MoveWindowToSavedPosition()
    {
        //TODO: Implement moving window to saved position
    }

    public void SaveWindowPositionAndSize()
    {
        //TODO: Implement saving window position and size
    }

    public bool MoveGuildwarsToScreen(Screen screen)
    {
        this.logger.LogDebug("Attempting to move guildwars to screen {screenId}", screen.Id);
        var hwnd = GetMainWindowHandle();
        if (hwnd.HasValue is false)
        {
            return false;
        }

        NativeMethods.SetWindowPos(hwnd.Value, NativeMethods.HWND_TOP, screen.Size.Left, screen.Size.Top, screen.Size.Width, screen.Size.Height, NativeMethods.SWP_SHOWWINDOW);
        return true;
    }

    private static IntPtr? GetMainWindowHandle()
    {
        var process = Process.GetProcessesByName("gw").FirstOrDefault();
        return process is not null ? process.MainWindowHandle : default;
    }

    private static List<Screen> GetAllScreens()
    {
        var screens = new List<Screen>();
        int index = 0;

        NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hMonitor, IntPtr hdcMonitor, ref NativeMethods.RECT lprcMonitor, IntPtr dwData) =>
        {
            var monitorInfo = new NativeMethods.MonitorInfoEx();
            monitorInfo.CbSize = (uint)Marshal.SizeOf<NativeMethods.MonitorInfoEx>();

            if (NativeMethods.GetMonitorInfo(hMonitor, ref monitorInfo))
            {
                screens.Add(new Screen(index++, new Rectangle(monitorInfo.RcMonitor.Left, monitorInfo.RcMonitor.Top, monitorInfo.RcMonitor.Width, monitorInfo.RcMonitor.Height)));
            }

            return true;
        }, IntPtr.Zero);

        return screens;
    }
}
