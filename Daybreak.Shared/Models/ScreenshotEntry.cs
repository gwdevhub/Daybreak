using Daybreak.Shared.Models.ColorPalette;
using static Daybreak.Shared.Models.Theme;

namespace Daybreak.Shared.Models;

public sealed record ScreenshotEntry(string FilePath, AccentColor AccentColor, LightDarkMode LightDarkMode)
{
}
