using Daybreak.Models;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.Events;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
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

    public ObservableCollection<EventViewModel> Events { get; } = new(Event.Events.Select(e => new EventViewModel { SeasonalEvent = e }));

    [GenerateDependencyProperty]
    private EventViewModel selectedListEvent = default!;

    [GenerateDependencyProperty]
    private EventViewModel upcomingEvent = default!;

    [GenerateDependencyProperty]
    private TimeOnly upcomingEventStartTime;

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    [GenerateDependencyProperty]
    private DateTime selectedDate;

    public EventCalendarView(
        IEventService eventService)
    {
        this.eventService = eventService.ThrowIfNull();
        this.InitializeComponent();
        this.RefreshEvents();
        this.NavigateToEvent();
        this.SelectedDate = DateTime.UtcNow;
    }

    private void NavigateToEvent()
    {
        var selectedEvent = this.Events.FirstOrDefault(e => e.Active);
        selectedEvent ??= this.Events.FirstOrDefault(e => e.Upcoming);
        if (selectedEvent is null ||
            selectedEvent.SeasonalEvent is null)
        {
            return;
        }

        this.SelectedListEvent = selectedEvent;
        this.NavigateToEvent(selectedEvent.SeasonalEvent);
    }

    private void RefreshEvents()
    {
        var upcomingEvent = this.eventService.GetUpcomingEvent();
        var activeEvents = this.eventService.GetCurrentActiveEvents();
        this.UpcomingEventStartTime = this.eventService.GetLocalizedEventStartTime();
        this.UpcomingEvent = this.Events.FirstOrDefault(ev => ev.SeasonalEvent == upcomingEvent);
        foreach(var ev in this.Events)
        {
            ev.Active = activeEvents.Contains(ev.SeasonalEvent!);
            ev.Upcoming = ev.SeasonalEvent == upcomingEvent;
        }
    }

    private void NavigateToEvent(Event e)
    {
        this.BrowserAddress = e.WikiUrl;
        this.SelectedDate = this.eventService.GetEventStartTime(e);
    }

    private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count < 1)
        {
            return;
        }

        if (e.AddedItems[0] is not DateTime dateTime)
        {
            return;
        }

        dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 19, 0, 0);
        var currentActiveEvent = this.eventService.GetActiveEvents(dateTime);
        if (currentActiveEvent.FirstOrDefault() is not Event ev)
        {
            return;
        }

        this.NavigateToEvent(ev);
        this.SelectedListEvent = this.Events.FirstOrDefault(e => e.SeasonalEvent == ev);
    }

    private void EventListItem_MouseLeftButtonUp(object _, MouseButtonEventArgs e)
    {
        if (this.SelectedListEvent?.SeasonalEvent?.WikiUrl is null or "")
        {
            return;
        }

        this.NavigateToEvent(this.SelectedListEvent.SeasonalEvent);
    }
}
