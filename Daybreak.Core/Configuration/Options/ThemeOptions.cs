using Daybreak.Attributes;
using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models.Themes;
using Daybreak.Themes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Theme")]
public sealed class ThemeOptions
{
    [OptionName(Name = "Theme", Description = "The application theme dictates the color scheme of the application")]
    [DaybreakThemeOptions]
    public Theme ApplicationTheme { get; set; } = CoreThemes.Daybreak;

    [OptionName(Name = "UI Scale", Description = "The UI scale adjusts the size of the user interface elements")]
    [OptionRange<double>(MaxValue = 1.5d, MinValue = 0.5d)]
    public double ApplicationScale { get; set; } = 1.0;
}
