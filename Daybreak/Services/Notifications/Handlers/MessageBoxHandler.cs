using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using System.Windows;

namespace Daybreak.Services.Notifications.Handlers;

public sealed class MessageBoxHandler : INotificationHandler
{
    public void OpenNotification(Notification notification)
    {
        MessageBox.Show(notification.Description, notification.Title);
    }
}
