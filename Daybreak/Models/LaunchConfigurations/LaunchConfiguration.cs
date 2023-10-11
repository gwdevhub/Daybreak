using Newtonsoft.Json;

namespace Daybreak.Models.LaunchConfigurations;

public sealed class LaunchConfiguration
{
    [JsonProperty(nameof(Identifier))]
    public string? Identifier { get; set; }

    [JsonProperty(nameof(Executable))]
    public string? Executable { get; set; }

    [JsonProperty(nameof(CredentialsIdentifier))]
    public string? CredentialsIdentifier { get; set; }
}
