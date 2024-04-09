using Daybreak.Models.Guildwars;
using Daybreak.Services.Events;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for EventCalendarView.xaml
/// </summary>
public partial class EventCalendarView : UserControl
{
    private readonly IEventService eventService;

    public IEnumerable<Event> Events { get; init; } = Event.Events;

    [GenerateDependencyProperty]
    private Event selectedListEvent = default!;

    [GenerateDependencyProperty]
    private Event upcomingEvent = default!;

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    public EventCalendarView(
        IEventService eventService)
    {
        this.eventService = eventService.ThrowIfNull();
        this.InitializeComponent();
        this.LoadUpcomingEvent();
    }

    private void LoadUpcomingEvent()
    {
        this.UpcomingEvent = this.eventService.GetUpcomingEvent();
    }

    private void EventListItem_MouseLeftButtonUp(object _, MouseButtonEventArgs e)
    {
        if (this.SelectedListEvent?.WikiUrl is null or "")
        {
            return;
        }

        this.BrowserAddress = this.SelectedListEvent.WikiUrl;
    }
}
