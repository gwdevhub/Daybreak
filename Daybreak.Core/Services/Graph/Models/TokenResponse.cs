using System.Text.Json.Serialization;

namespace Daybreak.Services.Graph.Models;

public sealed class TokenResponse
{
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
}
