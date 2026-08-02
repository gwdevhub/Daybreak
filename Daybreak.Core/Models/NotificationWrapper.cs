using Daybreak.Shared.Models.Notifications;

namespace Daybreak.Models;
public sealed class NotificationWrapper
{
    public required Notification Notification { get; init; }
    public DateTime StartTime { get; set; }
    public DateTime ExpirationTime { get; set; }

    /// <summary>
    /// Incremented every time the notification lifetime is prolonged so the UI can
    /// restart the expiration progress animation.
    /// </summary>
    public int LifetimeGeneration { get; set; }

    public double LifetimeMilliseconds => Math.Max(0, (this.ExpirationTime - this.StartTime).TotalMilliseconds);
}
