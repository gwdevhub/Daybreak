using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Synchronization")]
[OptionsIgnore]
[OptionsSynchronizationIgnore]
public sealed class SynchronizationOptions
{
    [JsonPropertyName(nameof(ProtectedGraphAccessToken))]
    public string? ProtectedGraphAccessToken { get; set; }
    [JsonPropertyName(nameof(ProtectedGraphRefreshToken))]
    public string? ProtectedGraphRefreshToken { get; set; }
}
