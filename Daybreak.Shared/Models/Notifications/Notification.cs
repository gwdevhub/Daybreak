using Daybreak.Shared.Models.Notifications.Handling;
using Microsoft.Extensions.Logging;

namespace Daybreak.Shared.Models.Notifications;

public class Notification : ICancellableNotification
{
    public LogLevel Level { get; init; }
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Metadata { get; init; } = string.Empty;
    public DateTime ExpirationTime { get; init; }
    public DateTime CreationTime { get; init; } = DateTime.Now;

    public bool Dismissible { get; init; }
    public virtual Type? HandlingType { get; init; }

    public bool CancellationRequested { get; set; }
    public bool Closed { get; set; }

    internal Notification()
    {
    }
}

public sealed class Notification<T> : Notification
    where T : INotificationHandler
{
    public override Type? HandlingType => typeof(T);

    internal Notification()
    {
    }
}
