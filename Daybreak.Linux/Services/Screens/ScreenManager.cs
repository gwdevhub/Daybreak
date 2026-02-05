using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Screens;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photino.NET;
using System.Core.Extensions;
using System.Drawing;
using System.Extensions;

namespace Daybreak.Linux.Services.Screens;

// TODO: Implement proper Linux screen management using X11/Wayland APIs
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

    // TODO: Implement proper screen enumeration for Linux
    public IEnumerable<Screen> Screens => GetDummyScreens();

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
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

    // TODO: Implement for Linux - this is Windows-specific
    public bool MoveGuildwarsToScreen(Screen screen)
    {
        this.logger.LogWarning("MoveGuildwarsToScreen is not implemented on Linux");
        return false;
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
                // Return a reasonable default instead of throwing
                return new Rectangle(0, 100, 1000, 900);
            }

            return new Rectangle(
                    firstScreen.Size.X + (firstScreen.Size.Width / 4),
                    firstScreen.Size.Y + (firstScreen.Size.Height / 4),
                    firstScreen.Size.Width / 2,
                    firstScreen.Size.Height / 2);
        }

        return savedPosition;
    }

    private static List<Screen> GetDummyScreens()
    {
        // TODO: Use X11/Wayland APIs to enumerate actual screens
        // For now, return a single dummy screen with reasonable defaults
        return [new Screen(0, new Rectangle(0, 0, 1920, 1080))];
    }
}
