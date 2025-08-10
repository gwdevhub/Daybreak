using Daybreak.Shared.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Sounds")]
internal sealed class SoundOptions
{
    [OptionName(Name = "Sounds Enabled", Description = "If enabled, the client will play sounds.")]
    public bool Enabled { get; set; }
}
