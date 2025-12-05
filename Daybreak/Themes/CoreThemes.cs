using Daybreak.Shared.Models;
using Daybreak.Shared.Models.ColorPalette;
using static Daybreak.Shared.Models.Theme;

namespace Daybreak.Themes;
public static class CoreThemes
{
    public static readonly Theme Daybreak = new("Daybreak", AccentColor.Yellow, "img/backdrops/daybreak.png", LightDarkMode.Light);
    public static readonly Theme Nightfall = new("Nightfall", AccentColor.DarkGreen, "img/backdrops/nightfall.png", LightDarkMode.Dark);
    public static readonly Theme WhiteMantle = new("White Mantle", AccentColor.Red, "img/backdrops/whitemantle.png", LightDarkMode.Dark);
    public static readonly Theme ShiningBlade = new("Shining Blade", AccentColor.DarkGreen, "img/backdrops/shiningblade.png", LightDarkMode.Dark);
    public static readonly Theme Jade = new("Jade", AccentColor.LightTeal, "img/backdrops/jade.png", LightDarkMode.Light);
    public static readonly Theme HarbingerOfTheDeceiver = new("Harbinger of the Deceiver", AccentColor.DarkPurple, "img/backdrops/harbinger.png", LightDarkMode.Dark);

    public static readonly IReadOnlyList<Theme> Themes = [
        Daybreak,
        Nightfall,
        WhiteMantle,
        ShiningBlade,
        Jade
        ];
}
