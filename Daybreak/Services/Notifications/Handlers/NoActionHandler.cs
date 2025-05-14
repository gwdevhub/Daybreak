using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;

namespace Daybreak.Models.Notifications.Handling;

public sealed class NoActionHandler : INotificationHandler
{
    public void OpenNotification(Notification notification)
    {
    }
}
