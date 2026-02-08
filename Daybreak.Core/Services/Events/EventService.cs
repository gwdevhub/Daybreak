using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.Events;

namespace Daybreak.Services.Events;
internal sealed class EventService : IEventService
{
    private static readonly TimeOnly EventStartTime = new(19, 0);

    public ICollection<Event> GetCurrentActiveEvents()
    {
        return [.. GetActiveEventsInternal(DateTime.UtcNow)];
    }

    public ICollection<Event> GetActiveEvents(DateTime dateTime)
    {
        return [.. GetActiveEventsInternal(dateTime)];
    }

    public Event GetUpcomingEvent()
    {
        var currentTime = DateTime.UtcNow;
        return Event.Events.OrderBy(TimeToStart).First();
    }

    public TimeOnly GetLocalizedEventStartTime()
    {
        var today = DateTime.UtcNow.Date;
        var utcDateTime = new DateTime(today.Year, today.Month, today.Day, EventStartTime.Hour, EventStartTime.Minute, EventStartTime.Second, DateTimeKind.Utc);
        var localizedDateTime = utcDateTime.ToLocalTime();
        var localTime = TimeOnly.FromDateTime(localizedDateTime);
        return localTime;
    }

    public DateTime GetEventStartTime(Event e)
    {
        var timeToEvent = TimeToStart(e);
        return DateTime.UtcNow + timeToEvent;
    }

    private static IEnumerable<Event> GetActiveEventsInternal(DateTime dateTime)
    {
        var maybeEvents = Event.Events.Where(e => EventIsActive(e, dateTime));
        foreach (var e in maybeEvents)
        {
            yield return e;
        }
    }

    private static TimeSpan TimeToStart(Event e)
    {
        var currentTime = DateTime.UtcNow;
        var startEventTime = new DateTime(currentTime.Year, e.From.Month, e.From.Day, EventStartTime.Hour, EventStartTime.Minute, EventStartTime.Second, DateTimeKind.Utc);
        if (startEventTime < currentTime)
        {
            startEventTime = startEventTime.AddYears(1);
        }

        return startEventTime - currentTime;
    }

    private static bool EventIsActive(Event e, DateTime dateTime)
    {
        return EventDateFilter(e, dateTime.Year, (_, eventStartTime, eventEndTime) =>
        {
            if (dateTime >= eventStartTime &&
                dateTime <= eventEndTime)
            {
                return true;
            }

            return false;
        });
    }

    private static TFilterReturn EventDateFilter<TFilterReturn>(Event e, int year, Func<Event, DateTime, DateTime, TFilterReturn> filter)
    {
        // 12pm pst is the equivalent of 19.00 utc
        var eventStartTime = new DateTime(year, e.From.Month, e.From.Day, EventStartTime.Hour, EventStartTime.Minute, EventStartTime.Second, DateTimeKind.Utc);
        var eventEndTime = new DateTime(year, e.To.Month, e.To.Day, EventStartTime.Hour, EventStartTime.Minute, EventStartTime.Second, DateTimeKind.Utc);
        return filter(e, eventStartTime, eventEndTime);
    }
}
