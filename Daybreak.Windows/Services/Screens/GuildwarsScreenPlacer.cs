using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Screens;
using Daybreak.Views.Mods;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions.Core;
using TrailBlazr.Services;

namespace Daybreak.Windows.Services.Screens;

internal sealed class GuildwarsScreenPlacer(
    IOptionsProvider optionsProvider,
    IModSettingsProvider modSettingsProvider,
    IViewManager viewManager,
    INotificationService notificationService,
    IOptionsMonitor<GuildWarsScreenPlacerOptions> liveOptions,
    IScreenManager screenManager,
    ILogger<GuildwarsScreenPlacer> logger) : IGuildwarsScreenPlacer
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(10);

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IOptionsMonitor<GuildWarsScreenPlacerOptions> liveOptions = liveOptions.ThrowIfNull();
    private readonly IScreenManager screenManager = screenManager.ThrowIfNull();
    private readonly ILogger<GuildwarsScreenPlacer> logger = logger.ThrowIfNull();

    public string Name => "Auto screen placer";
    public string Description => "Moves the Guildwars window to the desired screen on launch";
    public bool IsVisible => true;
    public bool IsEnabled
    {
        get => this.liveOptions.CurrentValue.Enabled;
        set
        {
            var option = this.liveOptions.CurrentValue;
            option.Enabled = value;
            this.optionsProvider.SaveOption(option);
        }
    }
    public bool CanCustomManage => true;
    public bool IsInstalled => true;

    public bool CanUninstall => false;
    public bool CanDisable => true;

    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GuildwarsScreenPlacer mod does not support manual uninstallation");
    }

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
        => this.OnCustomManagement(null, cancellationToken);

    public Task OnCustomManagement(LaunchConfigurationWithCredentials? configuration, CancellationToken cancellationToken)
    {
        this.viewManager.ShowView<ScreenSelectorView>(
            (nameof(ScreenSelectorView.LaunchConfigurationIdentifier), configuration?.Identifier ?? string.Empty));
        return Task.CompletedTask;
    }

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var options = modSettingsProvider.Get(this.Name, this.liveOptions.CurrentValue, guildWarsStartedContext.ApplicationLauncherContext.LaunchConfiguration);
        var screen = this.screenManager.Screens.Cast<Screen?>().FirstOrDefault(s => s?.Id == options.DesiredScreen);
        if (screen is null)
        {
            this.notificationService.NotifyError(
                title: "Failed to move Guild Wars window",
                description: $"Failed to move Guild Wars to desired screen {options.DesiredScreen}. Screen id is invalid");
            scopedLogger.LogError("Failed to move guild wars to desired screen {screenId}. Screen id is invalid", options.DesiredScreen);
            return;
        }

        await Task.Delay(Delay, cancellationToken);
        if (!this.screenManager.MoveGuildwarsToScreen(screen.Value, guildWarsStartedContext.ApplicationLauncherContext.ProcessId))
        {
            this.notificationService.NotifyError(
                title: "Failed to move Guild Wars window",
                description: $"Failed to move Guild Wars to desired screen {options.DesiredScreen}. Failed to move window");
            scopedLogger.LogError("Failed to move guild wars to desired screen {screenId}. Failed to move window", options.DesiredScreen);
        }
        else
        {
            this.notificationService.NotifyInformation(
                title: "Moved Guild Wars window",
                description: $"Moved Guild Wars to desired screen {options.DesiredScreen}");
            scopedLogger.LogInformation("Moved guild wars to desired screen {screenId}", options.DesiredScreen);
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

    public void SetDesiredScreen(Screen screen) => this.SetDesiredScreen(screen, null);

    public void SetDesiredScreen(Screen screen, LaunchConfigurationWithCredentials? configuration)
    {
        var options = modSettingsProvider.Get(this.Name, this.liveOptions.CurrentValue, configuration);
        options.DesiredScreen = screen.Id;
        modSettingsProvider.Save(this.Name, options, configuration);
    }

    public Screen? GetDesiredScreen() => this.GetDesiredScreen(null);

    public Screen? GetDesiredScreen(LaunchConfigurationWithCredentials? configuration)
    {
        var options = modSettingsProvider.Get(this.Name, this.liveOptions.CurrentValue, configuration);
        return this.screenManager.Screens.Cast<Screen?>().FirstOrDefault(screen => screen?.Id == options.DesiredScreen);
    }
}
