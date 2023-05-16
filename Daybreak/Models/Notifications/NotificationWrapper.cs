using System.Windows.Threading;

namespace Daybreak.Models.Notifications;
public sealed class NotificationWrapper
{
    public INotification? Notification { get; init; }
    public DispatcherTimer? DispatcherTimer { get; init; }
}
