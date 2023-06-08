using Daybreak.Models.Notifications.Handling;

namespace Daybreak.Services.Notifications;

public interface INotificationHandlerProducer
{
    void RegisterNotificationHandler<T>()
        where T : class, INotificationHandler;
}
