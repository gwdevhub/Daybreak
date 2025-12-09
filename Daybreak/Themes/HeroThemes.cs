using Daybreak.Shared.Models;
using Daybreak.Shared.Models.ColorPalette;
using static Daybreak.Shared.Models.Theme;

namespace Daybreak.Themes;
public static class HeroThemes
{
    public static readonly Theme Abaddon = new("Abaddon", AccentColor.OrangeLight, "img/backdrops/abaddon.png", LightDarkMode.Dark, string.Empty);
    public static readonly Theme Gwen = new("Gwen", AccentColor.Purple, "img/backdrops/gwen2.png", LightDarkMode.Dark, string.Empty);
    public static readonly Theme ZeiRi = new("Zei Ri", AccentColor.DarkTeal, "img/backdrops/zei-ri.png", LightDarkMode.Light, string.Empty);
    public static readonly Theme Livia = new("Livia", AccentColor.DarkRed, "img/backdrops/livia.png", LightDarkMode.Light, string.Empty);
    public static readonly Theme Razah = new("Razah", AccentColor.LightBlue, "img/backdrops/razah.png", LightDarkMode.Light, string.Empty);

    public static readonly IReadOnlyList<Theme> Themes = [
        Abaddon,
        Gwen,
        ZeiRi,
        Livia,
        Razah
        ];
}
