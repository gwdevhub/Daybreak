using Daybreak.Shared.Models;
using static Daybreak.Shared.Models.Theme;

namespace Daybreak.Themes;
public static class HeroThemes
{
    public static readonly Theme Abaddon = new("Abaddon", ColorPalette.AccentColor.OrangeLight, "img/backdrops/abaddon2.png", LightDarkMode.Dark);
    public static readonly Theme Gwen = new("Gwen", ColorPalette.AccentColor.Purple, "img/backdrops/gwen2.png", LightDarkMode.Dark);
    public static readonly Theme Jora = new("Jora", ColorPalette.AccentColor.LightBlue, "img/backdrops/jora2.png", LightDarkMode.Light);
    public static readonly Theme ZeiRi = new("Zei Ri", ColorPalette.AccentColor.DarkTeal, "img/backdrops/zei-ri.png", LightDarkMode.Light);
    public static readonly Theme Morgahn = new("General Morgahn", ColorPalette.AccentColor.OrangeLighter, "img/backdrops/morgahn.png", LightDarkMode.Dark);
    public static readonly Theme PyreFierceshot = new("Pyre Fierceshot", ColorPalette.AccentColor.DarkGreen, "img/backdrops/pyre-fierceshot.png", LightDarkMode.Dark);
    public static readonly Theme Livia = new("Livia", ColorPalette.AccentColor.DarkRed, "img/backdrops/livia.png", LightDarkMode.Light);
    public static readonly Theme Razah = new("Razah", ColorPalette.AccentColor.LightGray, "img/backdrops/razah.png", LightDarkMode.Light);

    public static readonly IReadOnlyList<Theme> Themes = [
        Abaddon,
        Gwen,
        Jora,
        ZeiRi,
        Morgahn,
        PyreFierceshot,
        Livia,
        Razah
        ];
}
