namespace Daybreak.Shared.Models.Notifications.Handling;

public interface INotificationHandler
{
    void OpenNotification(Notification notification);
}
