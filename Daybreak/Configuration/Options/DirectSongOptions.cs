using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "DirectSong")]
[OptionsIgnore]
internal sealed class DirectSongOptions
{
    [JsonPropertyName(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will setup DirectSong when launching Guild Wars")]
    public bool Enabled { get; set; } = false;
}
