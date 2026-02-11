using Daybreak.Configuration.Options;
using Daybreak.Services.Guildwars;
using Daybreak.Shared;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.GuildWars;

internal sealed class GuildWarsVersionChecker(
    IOptionsProvider optionsProvider,
    IOptionsMonitor<GuildWarsVersionCheckerOptions> options,
    IGuildWarsExecutableManager guildWarsExecutableManager,
    IGuildWarsInstaller guildWarsInstaller,
    INotificationService notificationService,
    ILogger<GuildWarsVersionChecker> logger) : IGuildWarsVersionChecker
{
    public string Name => "GuildWars Version Checker";
    public string Description => "Checks if the Guild Wars executable is up to date before launching";
    public bool IsVisible => true;
    public bool IsEnabled
    {
        get => this.options.CurrentValue.IsEnabled;
        set
        {
            var options = this.options.CurrentValue;
            options.IsEnabled = value;
            this.optionsProvider.SaveOption(options);
        }
    }
    public bool IsInstalled => true;
    public bool CanCustomManage => false;

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IOptionsMonitor<GuildWarsVersionCheckerOptions> options = options.ThrowIfNull();
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly IGuildWarsInstaller guildWarsInstaller = guildWarsInstaller.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<GuildWarsVersionChecker> logger = logger.ThrowIfNull();

    public bool CanUninstall => false;
    public bool CanDisable => true;

    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GuildWars Version Checker mod does not support manual uninstallation");
    }

    public Task<bool> IsUpdateAvailable(CancellationToken cancellationToken) => Task.FromResult(false);

    public Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GuildWars Version Checker mod does not support manual updates");
    }

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GuildWars Version Checker does not support manual installation");
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("GuildWars Version Checker does not support custom management");
    }

    public IEnumerable<string> GetCustomArguments()
    {
        return [];
    }

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath);
        if (!this.IsEnabled)
        {
            scopedLogger.LogDebug("Guild Wars Version Checker is disabled. Skipping version check");
            return;
        }

        var notificationToken = this.notificationService.NotifyInformation(
            title: "Checking Guild Wars version",
            description: "Checking if Guild Wars needs an update",
            expirationTime: DateTime.MaxValue,
            persistent: false);
        if (await this.guildWarsInstaller.GetLatestVersionId(cancellationToken) is not int latestVersion)
        {
            scopedLogger.LogInformation("Failed to fetch latest version. Skipping version check");
            notificationToken.Cancel();
            return;
        }

        if (await this.guildWarsInstaller.GetVersionId(guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath, cancellationToken) is not int version)
        {
            scopedLogger.LogInformation("Failed to read current version. Executable is probably damaged");
            notificationToken.Cancel();
            this.notificationService.NotifyError(
                title: "Potentially damaged executable",
                description: $"Daybreak has detected a potentially damaged executable at {guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath}. " +
                "It's recommended that you download the executable again through the menu. Daybreak will still attempt to launch it");
            return;
        }

        if (version == latestVersion)
        {
            scopedLogger.LogDebug("Executable is up to date");
            notificationToken.Cancel();
            return;
        }

        notificationToken.Cancel();
        scopedLogger.LogInformation("Found out of date executable. Prompting user to update");
        guildWarsStartingContext.CancelStartup = true;
        this.notificationService.NotifyError<GuildWarsBatchUpdateNotificationHandler>(
            title: "Guild Wars needs an update",
            description: $"Click here to update the executable located at {guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath}",
            expirationTime: DateTime.Now + TimeSpan.FromSeconds(15));
    }

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async ValueTask CheckExecutables()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (await this.guildWarsInstaller.GetLatestVersionId(CancellationToken.None) is not int latestVersion)
        {
            scopedLogger.LogError("Failed to fetch latest version. Skipping version check");
            return;
        }

        foreach (var executable in this.guildWarsExecutableManager.GetExecutableList())
        {
            if (await this.guildWarsInstaller.GetVersionId(executable, CancellationToken.None) is int version &&
                version == latestVersion)
            {
                continue;
            }

            scopedLogger.LogInformation("Discovered out of date executable. Prompting user to update");
            this.notificationService.NotifyInformation<GuildWarsBatchUpdateNotificationHandler>(
                title: "Guild Wars needs an update",
                description: "One or more Guild Wars executables are out of date and require an update. Click here to start the update",
                expirationTime: Global.NotificationNeverExpire);
            return;
        }
    }
}
