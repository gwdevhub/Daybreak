using Microsoft.Extensions.Logging;
using System;

namespace Daybreak.Services.Notifications.Models;

public sealed class NotificationDTO
{
    public string Id { get; init; }
    public LogLevel Level { get; init; }
    public DateTime ExpirationTime { get; init; }
    public DateTime CreationTime { get; init; } = DateTime.Now;
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? MetaData { get; init; }
    public string? HandlerType { get; init; }
    public bool Dismissible { get; init; }
    public bool Closed { get; set; }
}
