namespace Daybreak.Models.Notifications.Handling;

public interface INotificationHandler
{
    void OpenNotification(Notification notification);
}
