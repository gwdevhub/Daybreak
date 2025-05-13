using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;
using System;

namespace Daybreak.Services.Notifications;

public interface INotificationService
{
    NotificationToken NotifyInformation(string title, string description, string? metaData = default, DateTime? expirationTime = default, bool clickClosable = true, bool persistent = true);
    NotificationToken NotifyError(string title, string description, string? metaData = default, DateTime? expirationTime = default, bool clickClosable = true, bool persistent = true);
    NotificationToken NotifyInformation<THandlingType>(string title, string description, string? metaData = default, DateTime? expirationTime = default, bool dismissable = true, bool persistent = true)
        where THandlingType : class, INotificationHandler;
    NotificationToken NotifyError<THandlingType>(string title, string description, string? metaData = default, DateTime? expirationTime = default, bool dismissable = true, bool persistent = true)
        where THandlingType : class, INotificationHandler;
}
