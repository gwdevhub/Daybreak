using Daybreak.Attributes;
using Daybreak.Shared.Models.LaunchConfigurations;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class LaunchConfigurationServiceOptions
{
    [JsonProperty(nameof(LaunchConfigurations))]
    public List<LaunchConfiguration> LaunchConfigurations { get; set; } = [];
}
