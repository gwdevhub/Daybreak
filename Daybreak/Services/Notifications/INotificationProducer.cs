using Daybreak.Models.Notifications;
using System.Collections.Generic;
using System.Threading;

namespace Daybreak.Services.Notifications;

public interface INotificationProducer
{
    IAsyncEnumerable<INotification> Consume(CancellationToken cancellationToken);
}
