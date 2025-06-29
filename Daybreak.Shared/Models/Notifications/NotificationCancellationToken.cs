using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Shared.Models.Notifications;

public readonly struct NotificationToken : IDisposable
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

    public void Dispose()
    {
        this.Cancel();
    }
}
