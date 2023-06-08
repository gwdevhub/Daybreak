using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Sounds")]
public sealed class SoundOptions
{
    [OptionName(Name = "Sounds Enabled", Description = "If enabled, the client will play sounds.")]
    public bool Enabled { get; set; }
}
