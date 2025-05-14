using Newtonsoft.Json;

namespace Daybreak.Shared.Models;

public sealed class ProtectedLoginCredentials
{
    [JsonProperty(nameof(Identifier))]
    public string? Identifier { get; set; }
    [JsonProperty(nameof(ProtectedUsername))]
    public string? ProtectedUsername { get; set; }
    [JsonProperty(nameof(ProtectedPassword))]
    public string? ProtectedPassword { get; set; }
}
