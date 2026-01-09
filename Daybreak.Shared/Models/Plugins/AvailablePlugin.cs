namespace Daybreak.Shared.Models.Plugins;

public sealed class AvailablePlugin
{
    public required string Name { get; init; } = string.Empty;
    public required string Path { get; init; } = string.Empty;
    public bool Enabled { get; set; }
}
