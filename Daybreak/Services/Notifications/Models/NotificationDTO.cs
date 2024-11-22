using Realms;
using System;

namespace Daybreak.Services.Notifications.Models;

public sealed class NotificationDTO : RealmObject
{
    [PrimaryKey]
    public string Id { get; init; } = string.Empty;
    public int Level { get; init; }
    public DateTimeOffset ExpirationTime { get; init; }
    public DateTimeOffset CreationTime { get; init; } = DateTimeOffset.Now;
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? MetaData { get; init; }
    public string? HandlerType { get; init; }
    public bool Dismissible { get; init; }
    public bool Closed { get; set; }
}
