using System.Text.Json.Serialization;

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

    [JsonPropertyName(nameof(EnabledMods))]
    public List<string>? EnabledMods { get; set; }

    [JsonPropertyName(nameof(CustomModLoadoutEnabled))]
    public bool? CustomModLoadoutEnabled { get; set; }
}
