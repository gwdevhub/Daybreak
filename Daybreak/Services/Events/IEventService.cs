using Daybreak.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Services.Events;
public interface IEventService
{
    ICollection<Event> GetCurrentEvents();

    Event GetUpcomingEvent();
}
