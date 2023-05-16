using Daybreak.Models.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Notifications;

public sealed class NotificationService : INotificationService, INotificationProducer
{
    private readonly ConcurrentQueue<INotification> pendingNotifications = new();
    private readonly ILogger<NotificationService> logger;

    public NotificationService(
        ILogger<NotificationService> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public void NotifyInformation(string title, string description, Action? onClick = null)
    {
        var notification = new SimpleInformationNotification { Title = title, Description = description, OnClick = onClick };
        this.EnqueueNotification(notification);
    }

    public void NotifyError(string title, string description, Action? onClick = null)
    {
        var notification = new SimpleErrorNotification { Title = title, Description = description, OnClick = onClick };
        this.EnqueueNotification(notification);
    }

    public async IAsyncEnumerable<INotification> Consume([EnumeratorCancellation]CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while(this.pendingNotifications.TryDequeue(out var notification))
            {
                yield return notification;
            }

            await Task.Delay(1000, cancellationToken);
        }
    }

    private void EnqueueNotification(INotification notification)
    {
        this.pendingNotifications.Enqueue(notification);
    }
}
