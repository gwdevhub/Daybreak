using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;
using Daybreak.Services.Notifications.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using Slim;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Notifications;

internal sealed class NotificationService : INotificationService, INotificationProducer, INotificationHandlerProducer
{
    private readonly ConcurrentQueue<Notification> pendingNotifications = new();
    private readonly IServiceManager serviceManager;
    private readonly INotificationStorage storage;
    private readonly ILogger<NotificationService> logger;

    public NotificationService(
        IServiceManager serviceManager,
        INotificationStorage notificationStorage,
        ILogger<NotificationService> logger)
    {
        this.serviceManager = serviceManager.ThrowIfNull();
        this.storage = notificationStorage.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public NotificationToken NotifyInformation(
        string title,
        string description,
        string? metaData = default,
        DateTime? expirationTime = default,
        bool dismissible = true,
        bool persistent = true)
    {
        return this.NotifyInternal<NoActionHandler>(title, description, metaData, expirationTime, dismissible, LogLevel.Information, persistent);
    }

    public NotificationToken NotifyError(
        string title,
        string description,
        string? metaData = default,
        DateTime? expirationTime = default,
        bool dismissible = true,
        bool persistent = true)
    {
        return this.NotifyInternal<NoActionHandler>(title, description, metaData, expirationTime, dismissible, LogLevel.Error, persistent);
    }

    public NotificationToken NotifyInformation<THandlingType>(
        string title,
        string description,
        string? metaData = default,
        DateTime? expirationTime = default,
        bool dismissible = true,
        bool persistent = true)
        where THandlingType : class, INotificationHandler
    {
        return this.NotifyInternal<THandlingType>(title, description, metaData, expirationTime, dismissible, LogLevel.Information, persistent);
    }

    public NotificationToken NotifyError<THandlingType>(string title,
        string description,
        string? metaData = default,
        DateTime? expirationTime = default,
        bool dismissible = true,
        bool persistent = true)
        where THandlingType : class, INotificationHandler
    {
        return this.NotifyInternal<THandlingType>(title, description, metaData, expirationTime, dismissible, LogLevel.Error, persistent);
    }

    void INotificationProducer.OpenNotification(Notification notification, bool storeNotification)
    {
        if (storeNotification)
        {
            this.storage.OpenNotification(new NotificationDTO
            {
                Title = notification.Title,
                Description = notification.Description,
                Id = notification.Id,
                Level = (int)notification.Level,
                MetaData = notification.Metadata,
                HandlerType = notification.HandlingType?.AssemblyQualifiedName,
                ExpirationTime = notification.ExpirationTime.ToSafeDateTimeOffset(),
                Closed = true
            });
        }

        if (notification.HandlingType is null)
        {
            return;
        }

        var handler = this.serviceManager.GetService(notification.HandlingType) as INotificationHandler;
        handler?.OpenNotification(notification);
    }

    void INotificationProducer.RemoveNotification(Notification notification)
    {
        this.storage.RemoveNotification(ToDTO(notification));
    }

    void INotificationProducer.RemoveAllNotifications()
    {
        this.storage.RemoveAllNotifications();
    }

    async IAsyncEnumerable<Notification> INotificationProducer.Consume([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (this.pendingNotifications.TryDequeue(out var notification))
            {
                yield return notification;
            }

            await Task.Delay(1000, cancellationToken);
        }
    }

    IEnumerable<Notification> INotificationProducer.GetAllNotifications()
    {
        return this.storage.GetNotifications().Select(FromDTO);
    }

    IEnumerable<Notification> INotificationProducer.GetPendingNotifications()
    {
        return this.storage.GetPendingNotifications().Select(FromDTO);
    }

    void INotificationHandlerProducer.RegisterNotificationHandler<T>()
    {
        this.serviceManager.RegisterScoped<T>();
    }

    private NotificationToken NotifyInternal<THandlingType>(
        string title,
        string description,
        string? metaData,
        DateTime? expirationTime,
        bool dismissible,
        LogLevel logLevel,
        bool persistent)
        where THandlingType : class, INotificationHandler
    {
        var notification = new Notification<THandlingType>
        {
            Title = title,
            Description = description,
            Metadata = metaData ?? string.Empty,
            ExpirationTime = expirationTime ?? DateTime.Now + TimeSpan.FromSeconds(5),
            Dismissible = dismissible,
            Level = logLevel,
        };

        this.EnqueueNotification(notification, persistent);
        return new NotificationToken(notification);
    }

    private void EnqueueNotification(Notification notification, bool persistent)
    {
        if (persistent)
        {
            this.storage.StoreNotification(ToDTO(notification));
        }

        this.pendingNotifications.Enqueue(notification);
    }

    private static Notification FromDTO(NotificationDTO dto)
    {
        var handlingType = dto.HandlerType!.IsNullOrWhiteSpace() ?
                default :
                Type.GetType(dto.HandlerType!);

        return new Notification
        {
            Id = dto.Id,
            Level = (LogLevel)dto.Level,
            Title = dto.Title ?? string.Empty,
            Description = dto.Description ?? string.Empty,
            ExpirationTime = dto.ExpirationTime.LocalDateTime,
            CreationTime = dto.CreationTime.LocalDateTime,
            Metadata = dto.MetaData ?? string.Empty,
            Dismissible = dto.Dismissible,
            Closed = dto.Closed,
            HandlingType = handlingType ?? typeof(NoActionHandler),
        };
    }

    private static NotificationDTO ToDTO(Notification notification)
    {
        return new NotificationDTO
        {
            Id = notification.Id,
            Level = (int)notification.Level,
            Title = notification.Title,
            Description = notification.Description,
            ExpirationTime = notification.ExpirationTime.ToSafeDateTimeOffset(),
            CreationTime = notification.CreationTime.ToSafeDateTimeOffset(),
            MetaData = notification.Metadata,
            Dismissible = notification.Dismissible,
            Closed = notification.Closed,
            HandlerType = notification.HandlingType?.AssemblyQualifiedName
        };
    }
}
