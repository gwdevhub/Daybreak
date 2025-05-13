using Daybreak.Models.Guildwars;
using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;
using Daybreak.Services.Navigation;
using Daybreak.Views;
using System.Core.Extensions;
using System.Linq;

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
