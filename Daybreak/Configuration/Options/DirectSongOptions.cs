using Daybreak.Shared.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "DirectSong")]
internal sealed class DirectSongOptions
{
    [JsonProperty(nameof(Path))]
    [OptionName(Name = "Path", Description = "Folder that contains the DirectSong files")]
    [OptionSynchronizationIgnore]
    public string Path { get; set; } = string.Empty;

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will setup DirectSong when launching Guild Wars")]
    public bool Enabled { get; set; } = false;
}
