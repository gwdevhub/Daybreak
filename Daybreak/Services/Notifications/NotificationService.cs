using Daybreak.Services.Notifications.Handlers;
using Daybreak.Services.Notifications.Models;
using Daybreak.Shared;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slim;
using System.Collections.Concurrent;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Daybreak.Services.Notifications;

internal sealed class NotificationService(
    IServiceManager serviceManager,
    INotificationStorage notificationStorage,
    ILogger<NotificationService> logger) : INotificationService, INotificationProducer, INotificationHandlerProducer
{
    private readonly ConcurrentQueue<Notification> pendingNotifications = new();
    private readonly IServiceManager serviceManager = serviceManager.ThrowIfNull();
    private readonly INotificationStorage storage = notificationStorage.ThrowIfNull();
    private readonly ILogger<NotificationService> logger = logger.ThrowIfNull();

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

   async Task INotificationProducer.OpenNotification(Notification notification, bool storeNotification, CancellationToken cancellationToken)
    {
        if (storeNotification)
        {
            try
            {
                await this.storage.OpenNotification(new NotificationDTO
                {
                    Title = notification.Title,
                    Description = notification.Description,
                    Id = notification.Id,
                    Level = (int)notification.Level,
                    MetaData = notification.Metadata,
                    HandlerType = notification.HandlingType?.AssemblyQualifiedName,
                    ExpirationTime = notification.ExpirationTime.ToBinary(),
                    CreationTime = notification.CreationTime.ToBinary(),
                    Closed = true
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.CreateScopedLogger().LogError(ex, "Failed to open notification");
                return;
            }
        }

        if (notification.HandlingType is null)
        {
            return;
        }

        var handler = (INotificationHandler)this.serviceManager.GetRequiredService(notification.HandlingType);
        handler.OpenNotification(notification);
    }

    async Task INotificationProducer.RemoveNotification(Notification notification, CancellationToken cancellationToken)
    {
        await this.storage.RemoveNotification(ToDTO(notification), cancellationToken);
    }

    async Task INotificationProducer.RemoveAllNotifications(CancellationToken cancellationToken)
    {
        await this.storage.RemoveAllNotifications(cancellationToken);
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

    async ValueTask<IEnumerable<Notification>> INotificationProducer.GetAllNotifications(CancellationToken cancellationToken)
    {
        return (await this.storage.GetNotifications(cancellationToken)).Select(FromDTO);
    }

    async ValueTask<IEnumerable<Notification>> INotificationProducer.GetPendingNotifications(CancellationToken cancellationToken)
    {
        return (await this.storage.GetPendingNotifications(cancellationToken)).Select(FromDTO);
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
            ExpirationTime = expirationTime ?? (DateTime.Now + TimeSpan.FromSeconds(5)),
            Dismissible = dismissible,
            Level = logLevel,
        };

        if (Global.CoreWebView2 is null && logLevel is LogLevel.Error or LogLevel.Critical)
        {
            // If WebView2 is not initialized, we cannot show UI notifications.
            MessageBox.Show(
                description,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        else
        {
            this.EnqueueNotification(notification, persistent);
        }

        return new NotificationToken(notification);
    }

    private async void EnqueueNotification(Notification notification, bool persistent)
    {
        if (persistent)
        {
            await this.storage.StoreNotification(ToDTO(notification), CancellationToken.None);
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
            ExpirationTime = DateTime.FromBinary(dto.ExpirationTime),
            CreationTime = DateTime.FromBinary(dto.CreationTime),
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
            ExpirationTime = notification.ExpirationTime.ToBinary(),
            CreationTime = notification.CreationTime.ToBinary(),
            MetaData = notification.Metadata,
            Dismissible = notification.Dismissible,
            Closed = notification.Closed,
            HandlerType = notification.HandlingType?.AssemblyQualifiedName
        };
    }
}
