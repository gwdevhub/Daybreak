using System;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Models.Notifications;

public readonly struct NotificationToken
{
    private readonly Notification notification;

    public readonly DateTime? ExpirationTime;

    public bool Closed => this.notification.Closed;

    internal NotificationToken(Notification notification)
    {
        this.notification = notification.ThrowIfNull();
    }

    public void Cancel()
    {
        this.notification.CancellationRequested = true;
    }
}
