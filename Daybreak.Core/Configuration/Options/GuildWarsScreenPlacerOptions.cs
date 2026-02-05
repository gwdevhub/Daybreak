using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsSynchronizationIgnore]
[OptionsIgnore]
public sealed class GuildWarsScreenPlacerOptions
{
    [JsonPropertyName(nameof(Enabled))]
    public bool Enabled { get; set; }

    [JsonPropertyName(nameof(DesiredScreen))]
    public int DesiredScreen { get; set; }
}
