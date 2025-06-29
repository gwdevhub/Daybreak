using Daybreak.Services.Notifications.Models;
using System.Core.Extensions;

namespace Daybreak.Services.Notifications;

internal sealed class NotificationStorage : INotificationStorage
{
    private List<NotificationDTO>? notificationsCache;
    private readonly NotificationsDbContext liteCollection;

    public NotificationStorage(
        NotificationsDbContext liteCollection)
    {
        this.liteCollection = liteCollection.ThrowIfNull();
    }

    public async ValueTask<IEnumerable<NotificationDTO>> GetPendingNotifications(CancellationToken cancellationToken)
    {
        this.notificationsCache ??= await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
        return this.notificationsCache
            .Where(dto => dto.Closed == false && dto.ExpirationTime < DateTimeOffset.Now.ToUnixTimeMilliseconds());
    }

    public async ValueTask<IEnumerable<NotificationDTO>> GetNotifications(CancellationToken cancellationToken)
    {
        this.notificationsCache ??= await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
        return this.notificationsCache;
    }

    public async ValueTask StoreNotification(NotificationDTO notification, CancellationToken cancellationToken)
    {
        notification.ThrowIfNull();
        await this.liteCollection.Insert(notification, cancellationToken);
        this.notificationsCache ??= await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
        this.notificationsCache.Add(notification);
    }

    public async ValueTask OpenNotification(NotificationDTO notificationDTO, CancellationToken cancellationToken)
    {
        notificationDTO.ThrowIfNull();
        notificationDTO.Closed = true;
        await this.liteCollection.Update(notificationDTO, cancellationToken);
        this.notificationsCache = await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
    }

    public async ValueTask RemoveNotification(NotificationDTO notificationDTO, CancellationToken cancellationToken)
    {
        await this.liteCollection.Delete(notificationDTO.Id, cancellationToken);
        this.notificationsCache ??= await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
        this.notificationsCache.Remove(notificationDTO);
    }

    public async ValueTask RemoveAllNotifications(CancellationToken cancellationToken)
    {
        await this.liteCollection.DeleteAll(cancellationToken);
        this.notificationsCache = default;
    }
}
