using System.Windows.Threading;

namespace Daybreak.Models.Notifications;
public sealed class NotificationWrapper
{
    public string? Title { get => this.Notification?.Title; }
    public string? Description { get => this.Notification?.Description; }

    internal Notification? Notification { get; init; }
    internal DispatcherTimer? DispatcherTimer { get; init; }
}
