﻿using Daybreak.Services.Guildwars;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.GuildWars;
internal sealed class GuildWarsVersionChecker(
    IGuildWarsExecutableManager guildWarsExecutableManager,
    IGuildWarsInstaller guildWarsInstaller,
    INotificationService notificationService,
    ILogger<GuildWarsVersionChecker> logger) : IGuildWarsVersionChecker, IApplicationLifetimeService
{
    public string Name => "GuildWars Version Checker";
    public bool IsEnabled { get; set; } = true;
    public bool IsInstalled => true;

    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly IGuildWarsInstaller guildWarsInstaller = guildWarsInstaller.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<GuildWarsVersionChecker> logger = logger.ThrowIfNull();

    public IEnumerable<string> GetCustomArguments()
    {
        return [];
    }

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.OnGuildWarsStarting), guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath);
        var notificationToken = this.notificationService.NotifyInformation(
            title: "Checking Guild Wars version",
            description: "Checking if Guild Wars needs an update",
            persistent: false,
            expirationTime: DateTime.MaxValue);
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
                $"It's recommended that you download the executable again through the menu. Daybreak will still attempt to launch it");
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
        this.notificationService.NotifyError<GuildWarsUpdateNotificationHandler>(
            title: "Guild Wars needs an update",
            description: $"Click here to update the executable located at {guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath}",
            metaData: guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath,
            expirationTime: DateTime.Now + TimeSpan.FromSeconds(15));
    }

    public void OnStartup()
    {
        _ = new TaskFactory().StartNew(this.CheckExecutables, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public void OnClosing()
    {
    }

    private async void CheckExecutables()
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.OnStartup), string.Empty);
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
                description: "One or more Guild Wars executables are out of date and require an update. Click here to start the update");
            return;
        }
    }
}
