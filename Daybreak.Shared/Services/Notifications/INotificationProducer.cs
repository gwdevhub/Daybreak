using Daybreak.Models.Notifications;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Notifications;

public interface INotificationProducer
{
    ValueTask OpenNotification(Notification notification, bool storeNotification = true, CancellationToken cancellationToken = default);
    ValueTask RemoveNotification(Notification notification, CancellationToken cancellationToken);
    ValueTask RemoveAllNotifications(CancellationToken cancellationToken);
    ValueTask<IEnumerable<Notification>> GetAllNotifications(CancellationToken cancellationToken);
    ValueTask<IEnumerable<Notification>> GetPendingNotifications(CancellationToken cancellationToken);
    IAsyncEnumerable<Notification> Consume(CancellationToken cancellationToken);
}
