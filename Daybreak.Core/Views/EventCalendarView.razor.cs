using Daybreak.Models;
using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.Events;
using System.Core.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class EventCalendarViewModel(IEventService eventService)
    : ViewModelBase<EventCalendarViewModel, EventCalendarView>
{
    private readonly IEventService eventService = eventService.ThrowIfNull();

    public List<EventViewModel> Events { get; } = [.. Event.Events.Select(e => new EventViewModel { SeasonalEvent = e })];

    public EventViewModel? SelectedEvent { get; set; }

    public EventViewModel? UpcomingEvent { get; set; }

    public TimeOnly UpcomingEventStartTime { get; set; }

    public string FrameAddress { get; set; } = string.Empty;

    public DateTime SelectedDate { get; set; }

    public override ValueTask ParametersSet(EventCalendarView view, CancellationToken cancellationToken)
    {
        this.RefreshEvents();
        this.NavigateToActiveEvent();
        return base.ParametersSet(view, cancellationToken);
    }

    public void NavigateToEvent(Event e)
    {
        this.FrameAddress = e.WikiUrl ?? string.Empty;
        this.SelectedDate = this.eventService.GetEventStartTime(e);
    }

    public void NavigateToActiveEvent()
    {
        var selectedEvent = this.Events.FirstOrDefault(e => e.Active);
        selectedEvent ??= this.Events.FirstOrDefault(e => e.Upcoming);
        if (selectedEvent is null ||
            selectedEvent.SeasonalEvent is null)
        {
            return;
        }

        this.SelectedEvent = selectedEvent;
        this.NavigateToEvent(selectedEvent.SeasonalEvent);
    }

    public void RefreshEvents()
    {
        var upcomingEvent = this.eventService.GetUpcomingEvent();
        var activeEvents = this.eventService.GetCurrentActiveEvents();
        this.UpcomingEventStartTime = this.eventService.GetLocalizedEventStartTime();
        this.UpcomingEvent = this.Events.FirstOrDefault(ev => ev.SeasonalEvent == upcomingEvent);
        foreach (var ev in this.Events)
        {
            ev.Active = activeEvents.Contains(ev.SeasonalEvent!);
            ev.Upcoming = ev.SeasonalEvent == upcomingEvent;
        }
    }

    public AccentColor GetEventColor(Event eventItem)
    {
        var eventIndex = Event.Events.ToList().IndexOf(eventItem);
        if (eventIndex == -1)
        {
            return AccentColor.Accents.First();
        }

        return AccentColor.Accents[eventIndex % AccentColor.Accents.Length];
    }

    public void SelectEvent(EventViewModel eventViewModel)
    {
        this.SelectedEvent = eventViewModel;
        if (eventViewModel.SeasonalEvent is not null)
        {
            this.NavigateToEvent(eventViewModel.SeasonalEvent);
        }
    }

    public List<DateTime> GetCalendarDates()
    {
        var today = DateTime.Today;
        var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
        var dates = new List<DateTime>();
        for (int day = 1; day <= daysInMonth; day++)
        {
            dates.Add(new DateTime(today.Year, today.Month, day));
        }
        
        return dates;
    }

    public List<Event> GetEventsForDate(DateTime date)
    {
        var events = new List<Event>();
        foreach (var eventItem in Event.Events)
        {
            var eventDates = this.eventService.GetActiveEvents(date);
            if (eventDates.Contains(eventItem))
            {
                events.Add(eventItem);
            }
        }
        return events;
    }
}
