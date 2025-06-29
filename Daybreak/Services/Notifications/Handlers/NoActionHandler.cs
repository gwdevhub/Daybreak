using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;

namespace Daybreak.Services.Notifications.Handlers;

public sealed class NoActionHandler : INotificationHandler
{
    public void OpenNotification(Notification notification)
    {
    }
}
