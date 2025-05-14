using Newtonsoft.Json;

namespace Daybreak.Shared.Models.LaunchConfigurations;

public sealed class LaunchConfiguration
{
    [JsonProperty(nameof(Identifier))]
    public string? Identifier { get; set; }

    [JsonProperty(nameof(Executable))]
    public string? Executable { get; set; }

    [JsonProperty(nameof(Arguments))]
    public string? Arguments { get; set; }

    [JsonProperty(nameof(CredentialsIdentifier))]
    public string? CredentialsIdentifier { get; set; }
}
