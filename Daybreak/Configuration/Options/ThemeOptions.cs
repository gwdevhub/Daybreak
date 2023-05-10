using Daybreak.Attributes;
using Daybreak.Models;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Theme")]
public sealed class ThemeOptions
{
    [OptionName(Name = "Dark Mode", Description = "If true, the client will switch to dark mode")]
    public bool DarkMode { get; set; } = true;

    [OptionName(Name = "Color Scheme", Description = "The client will apply the selected color scheme")]
    public ColorScheme Theme { get; set; } = ColorScheme.Steel;
}
