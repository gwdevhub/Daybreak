using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Views;
using System.Core.Extensions;

namespace Daybreak.Services.Notifications.Handlers;
public sealed class NavigateToCalendarViewHandler : INotificationHandler
{
    private readonly IViewManager viewManager;

    public NavigateToCalendarViewHandler(
        IViewManager viewManager)
    {
        this.viewManager = viewManager.ThrowIfNull();
    }

    public void OpenNotification(Notification notification)
    {
        var eventTitle = notification.Metadata;
        var ev = Event.Events.FirstOrDefault(e => e.Title == eventTitle);
        if (ev is null)
        {
            return;
        }

        this.viewManager.ShowView<EventCalendarView>();
    }
}
