namespace Daybreak.Services.Notifications.Models;

public partial class NotificationDTO
{
    public required string Id { get; init; } = string.Empty;
    public int Level { get; init; }
    public long ExpirationTime { get; set; }
    public required long CreationTime { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? MetaData { get; init; }
    public string? HandlerType { get; init; }
    public bool Dismissible { get; init; }
    public bool Closed { get; set; }
}
