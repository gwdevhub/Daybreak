using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsName(Name = "Credentials")]
internal sealed class CredentialManagerOptions
{
    [JsonPropertyName(nameof(ProtectedLoginCredentials))]
    public List<ProtectedLoginCredentials> ProtectedLoginCredentials { get; set; } = [];
}
