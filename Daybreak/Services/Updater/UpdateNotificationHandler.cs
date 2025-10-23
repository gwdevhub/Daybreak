using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;

namespace Daybreak.Services.Updater;
//TODO: Implement update handling
internal sealed class UpdateNotificationHandler() : INotificationHandler
{
    //private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public void OpenNotification(Notification notification)
    {
        var maybeVersion = notification.Metadata;
        if (!Version.TryParse(maybeVersion, out var version))
        {
            return;
        }

        //this.viewManager.ShowView<UpdateConfirmationView>(version);
    }
}
