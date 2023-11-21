using Daybreak.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;
[OptionsIgnore]
internal sealed class GuildwarsExecutableOptions
{
    [JsonProperty(nameof(ExecutablePaths))]
    public List<string> ExecutablePaths { get; set; } = [];
}
