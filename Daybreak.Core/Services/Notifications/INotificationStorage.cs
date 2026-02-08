using Daybreak.Services.Notifications.Models;

namespace Daybreak.Services.Notifications;

public interface INotificationStorage
{
    ValueTask<IEnumerable<NotificationDTO>> GetPendingNotifications(CancellationToken cancellationToken);

    ValueTask<IEnumerable<NotificationDTO>> GetNotifications(CancellationToken cancellationToken);

    ValueTask StoreNotification(NotificationDTO notification, CancellationToken cancellationToken);

    ValueTask OpenNotification(NotificationDTO notificationDTO, CancellationToken cancellationToken);

    ValueTask RemoveNotification(NotificationDTO notificationDTO, CancellationToken cancellationToken);

    ValueTask RemoveAllNotifications(CancellationToken cancellationToken);
}
