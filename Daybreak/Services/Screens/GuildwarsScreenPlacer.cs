using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Screens;
using Daybreak.Views.Mods;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions.Core;
using TrailBlazr.Services;

namespace Daybreak.Services.Screens;

internal sealed class GuildwarsScreenPlacer(
    IViewManager viewManager,
    INotificationService notificationService,
    ILiveUpdateableOptions<GuildWarsScreenPlacerOptions> liveOptions,
    IScreenManager screenManager,
    ILogger<GuildwarsScreenPlacer> logger) : IGuildwarsScreenPlacer
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(10);

    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILiveUpdateableOptions<GuildWarsScreenPlacerOptions> liveOptions = liveOptions.ThrowIfNull();
    private readonly IScreenManager screenManager = screenManager.ThrowIfNull();
    private readonly ILogger<GuildwarsScreenPlacer> logger = logger.ThrowIfNull();

    public string Name => "Auto screen placer";
    public string Description => "Moves the Guildwars window to the desired screen on launch";
    public bool IsVisible => true;
    public bool IsEnabled
    {
        get => this.liveOptions.Value.Enabled;
        set
        {
            this.liveOptions.Value.Enabled = value;
            this.liveOptions.UpdateOption();
        }
    }
    public bool CanCustomManage => true;
    public bool IsInstalled => true;

    public Task<bool> IsUpdateAvailable(CancellationToken cancellationToken) => Task.FromResult(false);

    public Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GuildwarsScreenPlacer mod does not support manual updates");
    }

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GuildwarsScreenPlacer does not support manual installation.");
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        this.viewManager.ShowView<ScreenSelectorView>();
        return Task.CompletedTask;
    }

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var screen = this.screenManager.Screens.Skip(this.liveOptions.Value.DesiredScreen).FirstOrDefault();
        if (screen.Id != this.liveOptions.Value.DesiredScreen)
        {
            this.notificationService.NotifyError(
                title: "Failed to move Guild Wars window",
                description: $"Failed to move Guild Wars to desired screen {this.liveOptions.Value.DesiredScreen}. Screen id is invalid");
            scopedLogger.LogError("Failed to move guild wars to desired screen {screenId}. Screen id is invalid", this.liveOptions.Value.DesiredScreen);
            return;
        }

        await Task.Delay(Delay, cancellationToken);
        if (!this.screenManager.MoveGuildwarsToScreen(screen))
        {
            this.notificationService.NotifyError(
                title: "Failed to move Guild Wars window",
                description: $"Failed to move Guild Wars to desired screen {this.liveOptions.Value.DesiredScreen}. Failed to move window");
            scopedLogger.LogError("Failed to move guild wars to desired screen {screenId}. Failed to move window", this.liveOptions.Value.DesiredScreen);
        }
        else
        {
            this.notificationService.NotifyInformation(
                title: "Moved Guild Wars window",
                description: $"Moved Guild Wars to desired screen {this.liveOptions.Value.DesiredScreen}");
            scopedLogger.LogInformation("Moved guild wars to desired screen {screenId}", this.liveOptions.Value.DesiredScreen);
        }

        return;
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public IEnumerable<Screen> GetScreens()
    {
        return this.screenManager.Screens;
    }

    public void SetDesiredScreen(Screen screen)
    {
        var option = this.liveOptions.Value;
        option.DesiredScreen = screen.Id;
        this.liveOptions.UpdateOption();
    }

    public Screen? GetDesiredScreen()
    {
        return this.screenManager.Screens.Skip(this.liveOptions.Value.DesiredScreen).FirstOrDefault();
    }
}
