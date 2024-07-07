using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;
using Daybreak.Models.Progress;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Services.GuildWars.Models;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Views.Installation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;

namespace Daybreak.Services.GuildWars;
internal sealed class GuildWarsBatchUpdateNotificationHandler : INotificationHandler
{
    private readonly IViewManager viewManager;
    private readonly IGuildWarsInstaller guildWarsInstaller;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager;
    private readonly INotificationService notificationService;
    private readonly ILogger<GuildWarsBatchUpdateNotificationHandler> logger;

    public GuildWarsBatchUpdateNotificationHandler(
        IViewManager viewManager,
        IGuildWarsInstaller guildWarsInstaller,
        IGuildWarsExecutableManager guildWarsExecutableManager,
        INotificationService notificationService,
        ILogger<GuildWarsBatchUpdateNotificationHandler> logger)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.guildWarsInstaller = guildWarsInstaller.ThrowIfNull();
        this.guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async void OpenNotification(Notification notification)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.OpenNotification), string.Empty);
        if (await this.guildWarsInstaller.GetLatestVersionId(CancellationToken.None) is not int latestVersion)
        {
            scopedLogger.LogError("Failed to fetch latest version");
            this.notificationService.NotifyError(
                title: "Guild Wars batch update failed",
                description: "Failed to fetch latest Guild Wars version");
            return;
        }

        var updateList = new List<GuildWarsUpdateRequest>();
        var status = new GuildwarsInstallationStatus();
        var cancellationTokenSource = new CancellationTokenSource();
        var context = new GuildWarsDownloadContext { CancellationTokenSource = cancellationTokenSource, GuildwarsInstallationStatus = status };
        foreach(var executable in this.guildWarsExecutableManager.GetExecutableList())
        {
            if (await this.guildWarsInstaller.GetVersionId(executable, CancellationToken.None) is int version &&
                latestVersion == version)
            {
                continue;
            }

            updateList.Add(new GuildWarsUpdateRequest
            {
                ExecutablePath = executable,
                CancellationToken = cancellationTokenSource.Token,
                Status = status
            });
        }

        if (updateList.None())
        {
            scopedLogger.LogInformation("All executables are up to date");
            return;
        }

        this.viewManager.ShowView<GuildWarsDownloadView>(context);
        await foreach (var result in this.guildWarsInstaller.CheckAndUpdateGuildWarsExecutables(updateList, cancellationTokenSource.Token))
        {
            if (result.Result)
            {
                scopedLogger.LogInformation($"Updated {result.ExecutablePath}");
                this.notificationService.NotifyInformation(
                    title: "Updated executable",
                    description: $"Updated executable at {result.ExecutablePath}");
            }
            else
            {
                scopedLogger.LogInformation($"Failed to update {result.ExecutablePath}");
                this.notificationService.NotifyInformation(
                    title: "Failed to update executable",
                    description: $"Failed to update executable at {result.ExecutablePath}");
            }
        }
    }
}
