namespace Daybreak.Shared.Models.Notifications;

internal interface ICancellableNotification : INotification
{
    bool CancellationRequested { get; set; }
    bool Closed { get; set; }
}
