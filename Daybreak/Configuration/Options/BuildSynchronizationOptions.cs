using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Build Synchronization")]
[OptionsIgnore]
public sealed class BuildSynchronizationOptions
{
    [JsonProperty(nameof(ProtectedGraphAccessToken))]
    public string? ProtectedGraphAccessToken { get; set; }
    [JsonProperty(nameof(ProtectedGraphRefreshToken))]
    public string? ProtectedGraphRefreshToken { get; set; }
}
