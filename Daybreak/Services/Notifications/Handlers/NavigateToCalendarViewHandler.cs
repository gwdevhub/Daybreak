using System.Core.Extensions;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Views;
using TrailBlazr.Services;

namespace Daybreak.Services.Notifications.Handlers;

public sealed class NavigateToCalendarViewHandler(IViewManager viewManager) : INotificationHandler
{
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

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
