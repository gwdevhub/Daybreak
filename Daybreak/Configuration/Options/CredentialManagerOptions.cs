using Daybreak.Attributes;
using Daybreak.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class CredentialManagerOptions
{
    [JsonProperty(nameof(ProtectedLoginCredentials))]
    public List<ProtectedLoginCredentials> ProtectedLoginCredentials { get; set; } = [];
}
