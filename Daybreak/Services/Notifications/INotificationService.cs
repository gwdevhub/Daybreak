using System;

namespace Daybreak.Services.Notifications;

public interface INotificationService
{
    void NotifyInformation(string title, string description, Action? onClick = default);
    void NotifyError(string title, string description, Action? onClick = default);
}
