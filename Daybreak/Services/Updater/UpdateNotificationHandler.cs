using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;
using Daybreak.Models.Versioning;
using Daybreak.Services.Navigation;
using Daybreak.Views;
using System.Core.Extensions;

namespace Daybreak.Services.Updater;
internal sealed class UpdateNotificationHandler : INotificationHandler
{
    private readonly IViewManager viewManager;

    public UpdateNotificationHandler(IViewManager viewManager)
    {
        this.viewManager = viewManager.ThrowIfNull();
    }

    public void OpenNotification(Notification notification)
    {
        var maybeVersion = notification.Metadata;
        if (!Version.TryParse(maybeVersion, out var version))
        {
            return;
        }

        version.HasPrefix = true;
        this.viewManager.ShowView<UpdateConfirmationView>(version);
    }
}
