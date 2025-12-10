using System.Extensions;
using Daybreak.Services.Notifications.Models;

namespace Daybreak.Services.Notifications;

public sealed class InMemoryNotificationStorage : INotificationStorage
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly List<NotificationDTO> notifications = [];

    public async ValueTask<IEnumerable<NotificationDTO>> GetNotifications(CancellationToken cancellationToken)
    {
        using var ctx = await this.semaphore.Acquire(cancellationToken);
        return [.. this.notifications];
    }

    public async ValueTask<IEnumerable<NotificationDTO>> GetPendingNotifications(CancellationToken cancellationToken)
    {
        using var ctx = await this.semaphore.Acquire(cancellationToken);
        return [.. this.notifications.Where(dto => !dto.Closed)];
    }

    public ValueTask OpenNotification(NotificationDTO notificationDTO, CancellationToken cancellationToken)
    {
        notificationDTO.Closed = true;
        return ValueTask.CompletedTask;
    }

    public async ValueTask RemoveAllNotifications(CancellationToken cancellationToken)
    {
        using var ctx = await this.semaphore.Acquire(cancellationToken);
        this.notifications.Clear();
    }

    public async ValueTask RemoveNotification(NotificationDTO notificationDTO, CancellationToken cancellationToken)
    {
        using var ctx = await this.semaphore.Acquire(cancellationToken);
        this.notifications.Remove(notificationDTO);
    }

    public async ValueTask StoreNotification(NotificationDTO notification, CancellationToken cancellationToken)
    {
        using var ctx = await this.semaphore.Acquire(cancellationToken);
        this.notifications.Add(notification);
    }
}
