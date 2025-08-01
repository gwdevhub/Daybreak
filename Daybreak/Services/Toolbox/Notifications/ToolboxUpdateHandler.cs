using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Toolbox.Notifications;
internal sealed class ToolboxUpdateHandler(
    //IViewManager viewManager,
    ILogger<ToolboxUpdateHandler> logger) : INotificationHandler
{
    //private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly ILogger<ToolboxUpdateHandler> logger = logger.ThrowIfNull();

    public void OpenNotification(Notification notification)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Updating toolbox");
        //this.viewManager.ShowView<ToolboxInstallationView>();
    }
}
