using Daybreak.Shared.Converters;
using Newtonsoft.Json;

namespace Daybreak.Shared.Models;
[JsonConverter(typeof(ThemeJsonConverter))]
public sealed class Theme(string name, ColorPalette.AccentColor accentColor, string backdrop, Theme.LightDarkMode mode)
{
    public enum LightDarkMode
    {
        SystemSynchronized,
        Light,
        Dark
    }

    public string Name { get; init; } = name;
    public ColorPalette.AccentColor AccentColor { get; init; } = accentColor;
    public string Backdrop { get; init; } = backdrop;
    public LightDarkMode Mode { get; init; } = mode;

    public static readonly List<Theme> Themes =
    [
    ];

    public override string ToString()
    {
        return this.Name;
    }
}
