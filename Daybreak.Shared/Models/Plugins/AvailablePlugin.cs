namespace Daybreak.Shared.Models.Plugins;

public sealed class AvailablePlugin
{
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public bool Enabled { get; set; }
}
