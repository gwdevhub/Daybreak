using Daybreak.Launch;
using System.Windows;

namespace Daybreak.Models.Notifications.Handling;

public sealed class MessageBoxHandler : INotificationHandler
{
    public void OpenNotification(Notification notification)
    {
        ExceptionDialog.ShowException(notification.Title, notification.Description);
    }
}
