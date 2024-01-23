using Daybreak.Attributes;
using Daybreak.Models.LaunchConfigurations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class LaunchConfigurationServiceOptions
{
    [JsonProperty(nameof(LaunchConfigurations))]
    public List<LaunchConfiguration> LaunchConfigurations { get; set; } = [];
}
