using Daybreak.Shared.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Synchronization")]
[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class SynchronizationOptions
{
    [JsonProperty(nameof(ProtectedGraphAccessToken))]
    public string? ProtectedGraphAccessToken { get; set; }
    [JsonProperty(nameof(ProtectedGraphRefreshToken))]
    public string? ProtectedGraphRefreshToken { get; set; }
}
