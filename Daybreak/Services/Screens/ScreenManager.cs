using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;

namespace Daybreak.Services.Screens;

internal sealed class ScreenManager(
    ILiveUpdateableOptions<ScreenManagerOptions> liveUpdateableOptions,
    ILogger<ScreenManager> logger) : IScreenManager, IHostedService
{
    private readonly ILiveUpdateableOptions<ScreenManagerOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<ScreenManager> logger = logger.ThrowIfNull();

    public IEnumerable<Screen> Screens { get; } = []; //TODO: Implement screen detection

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

        NativeMethods.SetWindowPos(hwnd.Value, NativeMethods.HWND_TOP, screen.Size.Left.ToInt(), screen.Size.Top.ToInt(), screen.Size.Width.ToInt(), screen.Size.Height.ToInt(), NativeMethods.SWP_SHOWWINDOW);
        return true;
    }

    private static IntPtr? GetMainWindowHandle()
    {
        var process = Process.GetProcessesByName("gw").FirstOrDefault();
        return process is not null ? process.MainWindowHandle : default;
    }
}
