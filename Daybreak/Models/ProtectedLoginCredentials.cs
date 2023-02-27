using Newtonsoft.Json;

namespace Daybreak.Models;

public sealed class ProtectedLoginCredentials
{
    [JsonProperty("ProtectedUsername")]
    public string? ProtectedUsername { get; set; }
    [JsonProperty("ProtectedPassword")]
    public string? ProtectedPassword { get; set; }
    [JsonProperty("CharacterName")]
    public string? CharacterName { get; set; }
    [JsonProperty("Default")]
    public bool Default { get; set; }
}
