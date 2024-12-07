using Daybreak.Services.Database;
using Daybreak.Services.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Core.Extensions;

namespace Daybreak.Services.Notifications;

internal sealed class NotificationStorage : INotificationStorage
{
    private readonly IDatabaseCollection<NotificationDTO> liteCollection;

    public NotificationStorage(
        IDatabaseCollection<NotificationDTO> liteCollection)
    {
        this.liteCollection = liteCollection.ThrowIfNull();
    }

    public IEnumerable<NotificationDTO> GetPendingNotifications()
    {
        return this.liteCollection.FindAll(dto => dto.Closed == false && dto.ExpirationTime < DateTimeOffset.Now);
    }

    public IEnumerable<NotificationDTO> GetNotifications()
    {
        return this.liteCollection.FindAll();
    }

    public void StoreNotification(NotificationDTO notification)
    {
        notification.ThrowIfNull();
        this.liteCollection.Add(notification);
    }

    public void OpenNotification(NotificationDTO notificationDTO)
    {
        notificationDTO.ThrowIfNull();
        notificationDTO.Closed = true;
        this.liteCollection.Update(notificationDTO);
    }

    public void RemoveNotification(NotificationDTO notificationDTO)
    {
        this.liteCollection.Delete(notificationDTO);
    }

    public void RemoveAllNotifications()
    {
        this.liteCollection.DeleteAll();
    }
}
