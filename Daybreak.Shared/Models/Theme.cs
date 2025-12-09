using Daybreak.Shared.Converters;
using Daybreak.Shared.Models.ColorPalette;
using Newtonsoft.Json;

namespace Daybreak.Shared.Models;
[JsonConverter(typeof(ThemeJsonConverter))]
public class Theme(string name, AccentColor accentColor, string backdrop, Theme.LightDarkMode mode, string filter)
{
    public enum LightDarkMode
    {
        SystemSynchronized,
        Light,
        Dark
    }

    public string Name { get; init; } = name;
    public AccentColor AccentColor { get; internal set; } = accentColor;
    public string Backdrop { get; internal set; } = backdrop;
    public LightDarkMode Mode { get; internal set; } = mode;
    public string Filter { get; internal set; } = filter;

    public static readonly List<Theme> Themes =
    [
    ];

    public override string ToString()
    {
        return this.Name;
    }
}
