using Daybreak.Attributes;
using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models;
using Daybreak.Themes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Theme")]
public sealed class ThemeOptions
{
    [OptionName(Name = "Theme", Description = "The application theme dictates the color scheme of the application")]
    [DaybreakThemeOptions]
    public Theme ApplicationTheme { get; set; } = CoreThemes.Daybreak;
}
