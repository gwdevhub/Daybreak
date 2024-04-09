using Daybreak.Models.Guildwars;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Services.Events;
internal sealed class EventService : IEventService
{
    public ICollection<Event> GetCurrentEvents()
    {
        return GetCurrentEventsInternal().ToList();
    }

    public Event GetUpcomingEvent()
    {
        var currentTime = DateTime.UtcNow;
        return Event.Events.OrderBy(TimeToStart).First();
    }

    private static IEnumerable<Event> GetCurrentEventsInternal()
    {
        var maybeEvents = Event.Events.Where(EventIsActive);
        foreach (var e in maybeEvents)
        {
            yield return e;
        }
    }

    private static TimeSpan TimeToStart(Event e)
    {
        var currentTime = DateTime.UtcNow;
        return EventDateFilter(e, (_, eventStartTime, _) =>
        {
            if (eventStartTime < currentTime)
            {
                return TimeSpan.MaxValue;
            }

            return eventStartTime - currentTime;
        });
    }

    private static bool EventIsActive(Event e)
    {
        var currentTime = DateTime.UtcNow;

        return EventDateFilter(e, (_, eventStartTime, eventEndTime) =>
        {
            if (currentTime > eventStartTime &&
                currentTime < eventEndTime)
            {
                return true;
            }

            return false;
        });
    }

    private static TFilterReturn EventDateFilter<TFilterReturn>(Event e, Func<Event, DateTime, DateTime, TFilterReturn> filter)
    {
        var currentTime = DateTime.UtcNow;

        // 12pm pst is the equivalent of 19.00 utc
        var eventStartTime = new DateTime(currentTime.Year, e.From.Month, e.From.Day, 19, 0, 0);
        var eventEndTime = new DateTime(currentTime.Year, e.To.Month, e.To.Day, 19, 0, 0);
        if (eventEndTime < eventStartTime)
        {
            // Edge case when the event spans a new year. In this case, we need to move the start time in the previous year
            eventStartTime = new DateTime(eventStartTime.Year - 1, eventStartTime.Month, eventStartTime.Day, eventStartTime.Hour, eventStartTime.Minute, eventStartTime.Second);
        }

        return filter(e, eventStartTime, eventEndTime);
    }
}
