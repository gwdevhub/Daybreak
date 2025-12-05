using Daybreak.Shared.Models.Notifications;

namespace Daybreak.Models;
public sealed class NotificationWrapper
{
    public required Notification Notification { get; init; }
    public DateTimeOffset ExpirationTime { get; set; }
}
