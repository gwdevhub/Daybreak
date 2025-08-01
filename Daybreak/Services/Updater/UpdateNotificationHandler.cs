using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Version = Daybreak.Shared.Models.Versioning.Version;

namespace Daybreak.Services.Updater;
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

        version.HasPrefix = true;
        //this.viewManager.ShowView<UpdateConfirmationView>(version);
    }
}
