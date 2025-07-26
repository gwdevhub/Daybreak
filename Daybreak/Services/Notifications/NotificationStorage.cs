using Daybreak.Services.Notifications.Models;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Notifications;

internal sealed class NotificationStorage(
    NotificationsDbContext liteCollection,
    ILogger<NotificationStorage> logger) : INotificationStorage
{
    private List<NotificationDTO>? notificationsCache;
    private readonly NotificationsDbContext liteCollection = liteCollection.ThrowIfNull();
    private readonly ILogger<NotificationStorage> logger = logger.ThrowIfNull();

    public async ValueTask<IEnumerable<NotificationDTO>> GetPendingNotifications(CancellationToken cancellationToken)
    {
        try
        {
            this.notificationsCache ??= await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
            return this.notificationsCache
                .Where(dto => dto.Closed == false && dto.ExpirationTime < DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }
        catch (Exception ex)
        {
            this.logger.CreateScopedLogger().LogError(ex, "Failed to get pending notifications");
            throw;
        }
    }

    public async ValueTask<IEnumerable<NotificationDTO>> GetNotifications(CancellationToken cancellationToken)
    {
        try
        {
            this.notificationsCache ??= await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
            return this.notificationsCache;
        }
        catch (Exception ex)
        {
            this.logger.CreateScopedLogger().LogError(ex, "Failed to get notifications");
            throw;
        }
    }

    public async ValueTask StoreNotification(NotificationDTO notification, CancellationToken cancellationToken)
    {
        notification.ThrowIfNull();
        try
        {
            await this.liteCollection.Insert(notification, cancellationToken);
            this.notificationsCache ??= await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
            this.notificationsCache.Add(notification);
        }
        catch (Exception ex)
        {
            this.logger.CreateScopedLogger().LogError(ex, "Failed to store notification");
            throw;
        }
    }

    public async ValueTask OpenNotification(NotificationDTO notificationDTO, CancellationToken cancellationToken)
    {
        notificationDTO.ThrowIfNull();
        try
        {
            notificationDTO.Closed = true;
            await this.liteCollection.Update(notificationDTO, cancellationToken);
            this.notificationsCache = await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            this.logger.CreateScopedLogger().LogError(ex, "Failed to open notification");
            throw;
        }
    }

    public async ValueTask RemoveNotification(NotificationDTO notificationDTO, CancellationToken cancellationToken)
    {
        try
        {
            await this.liteCollection.Delete(notificationDTO.Id, cancellationToken);
            this.notificationsCache ??= await this.liteCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
            this.notificationsCache.Remove(notificationDTO);
        }
        catch (Exception ex)
        {
            this.logger.CreateScopedLogger().LogError(ex, "Failed to remove notification");
            throw;
        }
    }

    public async ValueTask RemoveAllNotifications(CancellationToken cancellationToken)
    {
        try
        {
            await this.liteCollection.DeleteAll(cancellationToken);
            this.notificationsCache = default;
        }
        catch (Exception ex)
        {
            this.logger.CreateScopedLogger().LogError(ex, "Failed to remove all notifications");
            throw;
        }
    }
}
