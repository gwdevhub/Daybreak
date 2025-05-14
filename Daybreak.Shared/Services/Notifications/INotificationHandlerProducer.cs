using Daybreak.Shared.Models.Notifications.Handling;

namespace Daybreak.Shared.Services.Notifications;

public interface INotificationHandlerProducer
{
    void RegisterNotificationHandler<T>()
        where T : class, INotificationHandler;
}
