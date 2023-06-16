using Daybreak.Services.Notifications.Models;
using System.Collections.Generic;

namespace Daybreak.Services.Notifications;

public interface INotificationStorage
{
    IEnumerable<NotificationDTO> GetPendingNotifications(int maxCount = int.MaxValue);

    IEnumerable<NotificationDTO> GetNotifications();

    void StoreNotification(NotificationDTO notification);

    void OpenNotification(NotificationDTO notificationDTO);

    void RemoveNotification(NotificationDTO notificationDTO);

    void RemoveAllNotifications();
}
