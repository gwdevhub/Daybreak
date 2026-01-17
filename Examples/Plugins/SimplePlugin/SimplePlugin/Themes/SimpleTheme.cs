using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.Themes;

namespace SimplePlugin.Themes;

internal class SimpleTheme
{
    public static readonly Theme Instance = new ("Simple", AccentColor.Red, new StaticBackground("img/Simple.png"), Theme.LightDarkMode.Light, string.Empty);
}
