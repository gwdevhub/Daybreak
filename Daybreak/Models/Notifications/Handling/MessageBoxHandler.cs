using System.Windows;

namespace Daybreak.Models.Notifications.Handling;

public sealed class MessageBoxHandler : INotificationHandler
{
    public void OpenNotification(Notification notification)
    {
        MessageBox.Show(notification.Description, notification.Title);
    }
}
