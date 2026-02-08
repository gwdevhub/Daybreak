using System.Text.Json.Serialization;

namespace Daybreak.Services.Graph.Models;

public sealed class User
{
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("mail")]
    public string? Email { get; set; }
}
