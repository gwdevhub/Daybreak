using Daybreak.Models.Notifications;
using System.Collections.Generic;
using System.Threading;

namespace Daybreak.Services.Notifications;

public interface INotificationProducer
{
    void OpenNotification(Notification notification, bool storeNotification = true);
    void RemoveNotification(Notification notification);
    IEnumerable<Notification> GetAllNotifications();
    IEnumerable<Notification> GetPendingNotifications();
    IAsyncEnumerable<Notification> Consume(CancellationToken cancellationToken);
}
