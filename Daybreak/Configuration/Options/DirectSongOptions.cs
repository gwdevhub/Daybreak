using Daybreak.Shared.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "DirectSong")]
[OptionsIgnore]
internal sealed class DirectSongOptions
{
    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will setup DirectSong when launching Guild Wars")]
    public bool Enabled { get; set; } = false;
}
