using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;
[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class GuildwarsExecutableOptions
{
    [JsonPropertyName(nameof(ExecutablePaths))]
    public List<string> ExecutablePaths { get; set; } = [];
}
