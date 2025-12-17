using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.Themes;
using static Daybreak.Shared.Models.Themes.Theme;

namespace Daybreak.Themes;
public static class HeroThemes
{
    public static readonly Theme Abaddon = new("Abaddon", AccentColor.OrangeLight, new StaticBackground("img/backdrops/abaddon.png"), LightDarkMode.Dark, string.Empty);
    public static readonly Theme Gwen = new("Gwen", AccentColor.Purple, new StaticBackground("img/backdrops/gwen.png"), LightDarkMode.Dark, string.Empty);
    public static readonly Theme ZeiRi = new("Zei Ri", AccentColor.DarkTeal, new StaticBackground("img/backdrops/zei-ri.png"), LightDarkMode.Light, string.Empty);
    public static readonly Theme Livia = new("Livia", AccentColor.DarkRed, new StaticBackground("img/backdrops/livia.png"), LightDarkMode.Light, string.Empty);
    public static readonly Theme Razah = new("Razah", AccentColor.LightBlue, new StaticBackground("img/backdrops/razah.png"), LightDarkMode.Light, string.Empty);

    public static readonly IReadOnlyList<Theme> Themes = [
        Abaddon,
        Gwen,
        ZeiRi,
        Livia,
        Razah
        ];
}
