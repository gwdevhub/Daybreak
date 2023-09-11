using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Background Provider")]
public sealed class BackgroundProviderOptions
{
    [OptionName(Name = "Bloogum Client Enabled", Description = "When enabled, Background Provider will make use of Bloogum to get background images")]
    public bool BloogumEnabled { get; set; } = true;
    [OptionName(Name = "Local Screenshots Enabled", Description = "When enabled, Background Provider will make use of the local Guild Wars screenshots to get background images")]
    public bool LocalScreenshotsEnabled { get; set; } = true;
}
