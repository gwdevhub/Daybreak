using Daybreak.Configuration.Options;
using Daybreak.Converters;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Notifications.Handling;
using Daybreak.Services.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Globalization;
using System.Threading.Tasks;

namespace Daybreak.Services.Events;

internal sealed class EventNotifierService : IEventNotifierService
{
    private readonly TimespanToETAConverter timespanToETAConverter = new();
    private readonly IEventService eventService;
    private readonly INotificationService notificationService;
    private readonly ILiveOptions<EventNotifierOptions> liveOptions;
    private readonly ILogger<IEventNotifierService> logger;

    public EventNotifierService(
        IEventService eventService,
        INotificationService notificationService,
        ILiveOptions<EventNotifierOptions> liveOptions,
        ILogger<IEventNotifierService> logger)
    {
        this.eventService = eventService.ThrowIfNull();
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
        foreach(var e in this.eventService.GetCurrentActiveEvents())
        {
            this.notificationService.NotifyInformation<NavigateToCalendarViewHandler>(e.Title!, $"{this.GetRemainingTime(e)}\n{e.Description!}", expirationTime: DateTime.Now + TimeSpan.FromSeconds(15), metaData: e.Title);
        }
    }

    private string? GetRemainingTime(Event e)
    {
        var currentTime = DateTime.UtcNow;
        var eventEndTime = new DateTime(currentTime.Year, e.To.Month, e.To.Day, 19, 0, 0);

        var remainingTime = eventEndTime - currentTime;
        return this.timespanToETAConverter.Convert(remainingTime, typeof(string), default!, CultureInfo.CurrentCulture) as string;
    }
}
