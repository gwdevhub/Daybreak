using Daybreak.Shared.Models;
using static Daybreak.Shared.Models.Theme;

namespace Daybreak.Themes;
public static class HeroThemes
{
    public static readonly Theme Abaddon = new("Abaddon", ColorPalette.AccentColor.OrangeLight, "img/backdrops/abaddon.png", LightDarkMode.Dark);
    public static readonly Theme Gwen = new("Gwen", ColorPalette.AccentColor.Purple, "img/backdrops/gwen.png", LightDarkMode.Dark);
    public static readonly Theme Jora = new("Jora", ColorPalette.AccentColor.DarkBlue, "img/backdrops/jora.png", LightDarkMode.Light);

    public static readonly IReadOnlyList<Theme> Themes = [
        Abaddon,
        Gwen,
        Jora
        ];
}
