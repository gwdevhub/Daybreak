using Daybreak.Attributes;
using Daybreak.Shared.Models;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsName(Name = "Credentials")]
internal sealed class CredentialManagerOptions
{
    [JsonProperty(nameof(ProtectedLoginCredentials))]
    public List<ProtectedLoginCredentials> ProtectedLoginCredentials { get; set; } = [];
}
