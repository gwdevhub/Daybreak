using Newtonsoft.Json;

namespace Daybreak.Models;

public sealed class ProtectedLoginCredentials
{
    [JsonProperty(nameof(ProtectedUsername))]
    public string? ProtectedUsername { get; set; }
    [JsonProperty(nameof(ProtectedPassword))]
    public string? ProtectedPassword { get; set; }
    [JsonProperty(nameof(CharacterName))]
    public string? CharacterName { get; set; }
    [JsonProperty(nameof(Default))]
    public bool Default { get; set; }
}
