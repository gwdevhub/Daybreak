using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models;

public sealed class ProtectedLoginCredentials
{
    [JsonPropertyName(nameof(Identifier))]
    public string? Identifier { get; set; }
    [JsonPropertyName(nameof(ProtectedUsername))]
    public string? ProtectedUsername { get; set; }
    [JsonPropertyName(nameof(ProtectedPassword))]
    public string? ProtectedPassword { get; set; }
}
