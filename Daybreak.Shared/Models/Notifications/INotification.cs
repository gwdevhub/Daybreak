using Microsoft.Extensions.Logging;
using System;

namespace Daybreak.Shared.Models.Notifications;

internal interface INotification
{
    string Id { get; }
    LogLevel Level { get; }
    string Title { get; }
    string Description { get; }
    string Metadata { get; }
    DateTime ExpirationTime { get; }
    DateTime CreationTime { get; }
    bool Dismissible { get; }
}
