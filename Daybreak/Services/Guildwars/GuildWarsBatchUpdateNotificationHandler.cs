using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.Guildwars;

internal sealed class GuildWarsBatchUpdateNotificationHandler(
    IGuildWarsInstaller guildWarsInstaller,
    IGuildWarsExecutableManager guildWarsExecutableManager,
    INotificationService notificationService,
    ILogger<GuildWarsBatchUpdateNotificationHandler> logger) : INotificationHandler
{
    //private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly IGuildWarsInstaller guildWarsInstaller = guildWarsInstaller.ThrowIfNull();
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<GuildWarsBatchUpdateNotificationHandler> logger = logger.ThrowIfNull();

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
        var cancellationTokenSource = new CancellationTokenSource();
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
            });
        }

        if (updateList.None())
        {
            scopedLogger.LogDebug("All executables are up to date");
            return;
        }

        await foreach (var result in this.guildWarsInstaller.CheckAndUpdateGuildWarsExecutables(updateList, cancellationTokenSource.Token))
        {
            if (result.Result)
            {
                scopedLogger.LogDebug($"Updated {result.ExecutablePath}");
                this.notificationService.NotifyInformation(
                    title: "Updated executable",
                    description: $"Updated executable at {result.ExecutablePath}");
            }
            else
            {
                scopedLogger.LogDebug($"Failed to update {result.ExecutablePath}");
                this.notificationService.NotifyInformation(
                    title: "Failed to update executable",
                    description: $"Failed to update executable at {result.ExecutablePath}");
            }
        }
    }
}
