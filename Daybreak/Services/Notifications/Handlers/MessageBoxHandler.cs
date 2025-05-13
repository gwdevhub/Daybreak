using Daybreak.Launch;
using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;

namespace Daybreak.Services.Notifications.Handlers;

public sealed class MessageBoxHandler : INotificationHandler
{
    public void OpenNotification(Notification notification)
    {
        ExceptionDialog.ShowException(notification.Title, notification.Description);
    }
}
