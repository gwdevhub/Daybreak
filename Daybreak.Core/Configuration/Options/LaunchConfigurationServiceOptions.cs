using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models.LaunchConfigurations;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class LaunchConfigurationServiceOptions
{
    [JsonPropertyName(nameof(LaunchConfigurations))]
    public List<LaunchConfiguration> LaunchConfigurations { get; set; } = [];
}
