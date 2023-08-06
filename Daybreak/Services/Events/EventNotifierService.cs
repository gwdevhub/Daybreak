using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Daybreak.Services.Events;

public sealed class EventNotifierService : IEventNotifierService
{
    private readonly INotificationService notificationService;
    private readonly ILiveOptions<EventNotifierOptions> liveOptions;
    private readonly ILogger<IEventNotifierService> logger;

    public EventNotifierService(
        INotificationService notificationService,
        ILiveOptions<EventNotifierOptions> liveOptions,
        ILogger<IEventNotifierService> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public void OnClosing()
    {
    }

    public async void OnStartup()
    {
        if (!this.liveOptions.Value.Enabled)
        {
            return;
        }

        await Task.Delay(5000);
        var maybeEvents = Event.Events.Where(EventIsActive);
        foreach(var e in maybeEvents)
        {
            this.notificationService.NotifyInformation(e.Title!, e.Description!, expirationTime: DateTime.Now + TimeSpan.FromMinutes(1));
        }
    }

    private static bool EventIsActive(Event e)
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

        if (currentTime > eventStartTime &&
            currentTime < eventEndTime)
        {
            return true;
        }

        return false;
    }
}
