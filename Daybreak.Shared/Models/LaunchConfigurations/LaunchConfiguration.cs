using System.Text.Json.Serialization;
using Daybreak.Shared.Models.Mods;

namespace Daybreak.Shared.Models.LaunchConfigurations;

public sealed class LaunchConfiguration
{
    [JsonPropertyName(nameof(Name))]
    public string? Name { get; set; }

    [JsonPropertyName(nameof(Identifier))]
    public string? Identifier { get; set; }

    [JsonPropertyName(nameof(Executable))]
    public string? Executable { get; set; }

    [JsonPropertyName(nameof(Arguments))]
    public string? Arguments { get; set; }

    [JsonPropertyName(nameof(CredentialsIdentifier))]
    public string? CredentialsIdentifier { get; set; }

    [JsonPropertyName(nameof(SteamSupport))]
    public bool? SteamSupport { get; set; } = true;

    public Dictionary<string, ModLoadoutEntry> ModLoadout { get; set; } = [];

    // Read legacy loadouts without writing the old representation back to disk.
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? EnabledMods
    {
        get => null;
        set
        {
            foreach (var name in value ?? [])
            {
                this.ModLoadout.TryAdd(name, new ModLoadoutEntry { IsEnabled = true });
            }
        }
    }

    [JsonPropertyName(nameof(CustomModLoadoutEnabled))]
    public bool? CustomModLoadoutEnabled { get; set; }

    [JsonPropertyName(nameof(Color))]
    public string? Color { get; set; }
}
