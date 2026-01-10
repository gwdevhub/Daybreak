using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Photino.NET;

namespace Daybreak.Services.Notifications.Handlers;

public sealed class MessageBoxHandler(PhotinoWindow photinoWindow)
    : INotificationHandler
{
    private readonly PhotinoWindow photinoWindow = photinoWindow;

    public void OpenNotification(Notification notification)
    {
        this.photinoWindow.ShowMessage(notification.Title, notification.Description);
    }
}
