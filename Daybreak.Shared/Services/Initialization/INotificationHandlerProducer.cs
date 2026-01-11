using Daybreak.Shared.Models.Notifications.Handling;

namespace Daybreak.Shared.Services.Initialization;

public interface INotificationHandlerProducer
{
    void RegisterNotificationHandler<T>()
        where T : class, INotificationHandler;
}
