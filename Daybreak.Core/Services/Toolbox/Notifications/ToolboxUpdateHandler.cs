using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Toolbox;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;
using TrailBlazr.Services;

namespace Daybreak.Services.Toolbox.Notifications;
internal sealed class ToolboxUpdateHandler(
    IToolboxService toolboxService,
    IViewManager viewManager,
    ILogger<ToolboxUpdateHandler> logger) : INotificationHandler
{
    private readonly IToolboxService toolboxService = toolboxService.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly ILogger<ToolboxUpdateHandler> logger = logger.ThrowIfNull();

    public void OpenNotification(Notification notification)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Updating toolbox");
        this.viewManager.ShowView<ModInstallationView>(
            (nameof(ModInstallationView.Name), this.toolboxService.Name),
            (nameof(ModInstallationView.ForceInstallation), true));
    }
}
