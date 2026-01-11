using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Initialization;

public sealed class NotificationHandlerProducer(
    IServiceCollection services, ILogger<NotificationHandlerProducer> logger)
    : INotificationHandlerProducer
{
    private readonly IServiceCollection services = services;
    private readonly ILogger<NotificationHandlerProducer> logger = logger;

    public void RegisterNotificationHandler<T>()
        where T : class, INotificationHandler
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        this.services.AddSingleton<INotificationHandler, T>();
        scopedLogger.LogDebug("Registered {Handler.Name}", typeof(T).Name);
    }
}
