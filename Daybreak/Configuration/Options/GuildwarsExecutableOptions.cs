using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;
[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class GuildwarsExecutableOptions
{
    [JsonProperty(nameof(ExecutablePaths))]
    public List<string> ExecutablePaths { get; set; } = [];
}
