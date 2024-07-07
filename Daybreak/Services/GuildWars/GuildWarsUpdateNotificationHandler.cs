using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;
using Daybreak.Models.Progress;
using Daybreak.Services.GuildWars.Models;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Views.Installation;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Threading;

namespace Daybreak.Services.GuildWars;
internal sealed class GuildWarsUpdateNotificationHandler : INotificationHandler
{
    private readonly IViewManager viewManager;
    private readonly IGuildWarsInstaller guildWarsInstaller;
    private readonly INotificationService notificationService;
    private readonly ILogger<GuildWarsUpdateNotificationHandler> logger;

    public GuildWarsUpdateNotificationHandler(
        IViewManager viewManager,
        IGuildWarsInstaller guildWarsInstaller,
        INotificationService notificationService,
        ILogger<GuildWarsUpdateNotificationHandler> logger)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.guildWarsInstaller = guildWarsInstaller.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

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

        var status = new GuildwarsInstallationStatus();
        var cancellationTokenSource = new CancellationTokenSource();
        var context = new GuildWarsDownloadContext { CancellationTokenSource = cancellationTokenSource, GuildwarsInstallationStatus = status };
        this.viewManager.ShowView<GuildWarsDownloadView>(context);
        var response = await this.guildWarsInstaller.UpdateGuildwars(path, status, cancellationTokenSource.Token);
        scopedLogger.LogInformation($"Update result {response}");
    }
}
