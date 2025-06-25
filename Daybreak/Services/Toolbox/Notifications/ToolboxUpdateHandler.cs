using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Views.Onboarding.Toolbox;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Toolbox.Notifications;
internal sealed class ToolboxUpdateHandler : INotificationHandler
{
    private readonly IViewManager viewManager;
    private readonly ILogger<ToolboxUpdateHandler> logger;

    public ToolboxUpdateHandler(
        IViewManager viewManager,
        ILogger<ToolboxUpdateHandler> logger)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public void OpenNotification(Notification notification)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Updating toolbox");
        this.viewManager.ShowView<ToolboxInstallationView>();
    }
}
