using Daybreak.Configuration.Options;
using Daybreak.Services.Notifications.Handlers;
using Daybreak.Shared.Converters;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.Events;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;

namespace Daybreak.Services.Events;

internal sealed class EventNotifierService(
    IEventService eventService,
    INotificationService notificationService,
    ILiveOptions<EventNotifierOptions> liveOptions,
    ILogger<IEventNotifierService> logger) : IEventNotifierService, IHostedService
{
    private readonly IEventService eventService = eventService.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILiveOptions<EventNotifierOptions> liveOptions = liveOptions.ThrowIfNull();
    private readonly ILogger<IEventNotifierService> logger = logger.ThrowIfNull();

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        if (!this.liveOptions.Value.Enabled)
        {
            return;
        }

        await Task.Delay(5000, cancellationToken);
        foreach (var e in this.eventService.GetCurrentActiveEvents())
        {
            this.notificationService.NotifyInformation<NavigateToCalendarViewHandler>(e.Title!, $"{GetRemainingTime(e)}\n{e.Description!}", expirationTime: DateTime.Now + TimeSpan.FromSeconds(15), metaData: e.Title);
        }
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static string? GetRemainingTime(Event e)
    {
        var currentTime = DateTime.UtcNow;
        var eventEndTime = new DateTime(currentTime.Year, e.To.Month, e.To.Day, 19, 0, 0);

        var remainingTime = eventEndTime - currentTime;
        return TimespanToETAConverter.GetETAString(remainingTime);
    }
}
