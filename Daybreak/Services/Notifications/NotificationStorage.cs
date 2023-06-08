using Daybreak.Services.Notifications.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Core.Extensions;

namespace Daybreak.Services.Notifications;

internal sealed class NotificationStorage : INotificationStorage
{
    private readonly ILiteCollection<NotificationDTO> liteCollection;

    public NotificationStorage(
        ILiteCollection<NotificationDTO> liteCollection)
    {
        this.liteCollection = liteCollection.ThrowIfNull();
    }

    public IEnumerable<NotificationDTO> GetPendingNotifications(int maxCount = int.MaxValue)
    {
        return this.liteCollection.Find(dto => dto.Closed == false && dto.ExpirationTime > DateTime.Now, limit: maxCount);
    }

    public IEnumerable<NotificationDTO> GetNotifications()
    {
        return this.liteCollection.FindAll();
    }

    public void StoreNotification(NotificationDTO notification)
    {
        notification.ThrowIfNull();
        this.liteCollection.Upsert(notification.Id, notification);
    }

    public void OpenNotification(NotificationDTO notificationDTO)
    {
        notificationDTO.ThrowIfNull();
        notificationDTO.Closed = true;
        this.liteCollection.Update(notificationDTO);
    }

    public void RemoveNotification(NotificationDTO notificationDTO)
    {
        this.liteCollection.Delete(notificationDTO.Id);
    }
}
