using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.Themes;
using static Daybreak.Shared.Models.Themes.Theme;

namespace Daybreak.Themes;

public static class LiveThemes
{
    public static readonly Theme EotN = new("Eye of the North", AccentColor.LightBlue, new EmbedBackground(EmbeddedBackgrounds.EyeOfTheNorth), LightDarkMode.Light, string.Empty);

    public static readonly IReadOnlyList<Theme> Themes = [
        EotN,
        ];
}
