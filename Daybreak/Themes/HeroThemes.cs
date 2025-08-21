using Daybreak.Shared.Models;
using static Daybreak.Shared.Models.Theme;

namespace Daybreak.Themes;
public static class HeroThemes
{
    public static readonly Theme Abaddon = new("Abaddon", ColorPalette.AccentColor.OrangeLight, "img/backdrops/abaddon.png", LightDarkMode.Dark);
    public static readonly Theme Gwen = new("Gwen", ColorPalette.AccentColor.Purple, "img/backdrops/gwen2.png", LightDarkMode.Dark);
    public static readonly Theme ZeiRi = new("Zei Ri", ColorPalette.AccentColor.DarkTeal, "img/backdrops/zei-ri.png", LightDarkMode.Light);
    public static readonly Theme Livia = new("Livia", ColorPalette.AccentColor.DarkRed, "img/backdrops/livia.png", LightDarkMode.Light);
    public static readonly Theme Razah = new("Razah", ColorPalette.AccentColor.LightBlue, "img/backdrops/razah.png", LightDarkMode.Light);

    public static readonly IReadOnlyList<Theme> Themes = [
        Abaddon,
        Gwen,
        ZeiRi,
        Livia,
        Razah
        ];
}
