using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.Themes;
using static Daybreak.Shared.Models.Themes.Theme;

namespace Daybreak.Themes;
public static class CoreThemes
{
    public static readonly Theme Daybreak = new("Daybreak", AccentColor.Yellow, new StaticBackground("img/backdrops/daybreak.png"), LightDarkMode.Light, string.Empty);
    public static readonly Theme Nightfall = new("Nightfall", AccentColor.DarkGreen, new StaticBackground("img/backdrops/nightfall.png"), LightDarkMode.Dark, string.Empty);
    public static readonly Theme WhiteMantle = new("White Mantle", AccentColor.Red, new StaticBackground("img/backdrops/whitemantle.png"), LightDarkMode.Dark, string.Empty);
    public static readonly Theme ShiningBlade = new("Shining Blade", AccentColor.DarkGreen, new StaticBackground("img/backdrops/shiningblade.png"), LightDarkMode.Dark, string.Empty);
    public static readonly Theme Jade = new("Jade", AccentColor.LightTeal, new StaticBackground("img/backdrops/jade.png"), LightDarkMode.Light, string.Empty);
    public static readonly Theme Dervish = new("Dervish", AccentColor.MidBlue, new StaticBackground("img/backdrops/dervish.png"), LightDarkMode.Dark, string.Empty);

    public static readonly IReadOnlyList<Theme> Themes = [
        Daybreak,
        Nightfall,
        WhiteMantle,
        ShiningBlade,
        Jade,
        Dervish,
        ];
}
