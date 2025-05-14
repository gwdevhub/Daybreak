using Daybreak.Shared.Models.Guildwars;
using System;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.Events;
public interface IEventService
{
    TimeOnly GetLocalizedEventStartTime();

    ICollection<Event> GetActiveEvents(DateTime dateTime);

    ICollection<Event> GetCurrentActiveEvents();

    DateTime GetEventStartTime(Event e);

    Event GetUpcomingEvent();
}
