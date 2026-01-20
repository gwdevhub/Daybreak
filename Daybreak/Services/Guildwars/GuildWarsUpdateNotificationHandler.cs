using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.Guildwars;
internal sealed class GuildWarsUpdateNotificationHandler(
    //IViewManager viewManager,
    IGuildWarsInstaller guildWarsInstaller,
    INotificationService notificationService,
    ILogger<GuildWarsUpdateNotificationHandler> logger) : INotificationHandler
{
    //private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly IGuildWarsInstaller guildWarsInstaller = guildWarsInstaller.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<GuildWarsUpdateNotificationHandler> logger = logger.ThrowIfNull();

    public async void OpenNotification(Notification notification)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.OpenNotification), notification.Metadata);
        if (notification.Metadata is not string path)
        {
            scopedLogger.LogError("Notification does not have metadata");
            return;
        }

        if (!File.Exists(path))
        {
            scopedLogger.LogError("File does not exist");
            this.notificationService.NotifyInformation(
                title: "Guild Wars update failed",
                description: $"Executable does not exist at {path}");
            return;
        }

        var progress = new Progress<ProgressUpdate>();
        var cancellationTokenSource = new CancellationTokenSource();
        //this.viewManager.ShowView<GuildWarsDownloadView>(context);
        var response = await this.guildWarsInstaller.UpdateGuildwars(path, progress, cancellationTokenSource.Token);
        scopedLogger.LogDebug($"Update result {response}");
    }
}
