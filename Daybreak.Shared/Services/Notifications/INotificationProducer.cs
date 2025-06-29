using Daybreak.Shared.Models.Notifications;

namespace Daybreak.Shared.Services.Notifications;

public interface INotificationProducer
{
    Task OpenNotification(Notification notification, bool storeNotification = true, CancellationToken cancellationToken = default);
    Task RemoveNotification(Notification notification, CancellationToken cancellationToken);
    Task RemoveAllNotifications(CancellationToken cancellationToken);
    ValueTask<IEnumerable<Notification>> GetAllNotifications(CancellationToken cancellationToken);
    ValueTask<IEnumerable<Notification>> GetPendingNotifications(CancellationToken cancellationToken);
    IAsyncEnumerable<Notification> Consume(CancellationToken cancellationToken);
}
