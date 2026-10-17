using System.Text.Json;

namespace Daybreak.Shared.Models.Mods;

public sealed class ModLoadoutEntry
{
    public bool IsEnabled { get; set; }
    public Dictionary<string, JsonElement> Settings { get; set; } = [];
}
