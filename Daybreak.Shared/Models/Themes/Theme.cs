using Daybreak.Shared.Converters;
using Daybreak.Shared.Models.ColorPalette;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Themes;
[JsonConverter(typeof(ThemeJsonConverter))]
public class Theme(
    string name,
    AccentColor accentColor,
    IAppBackground background,
    Theme.LightDarkMode mode,
    string filter)
{
    public enum LightDarkMode
    {
        SystemSynchronized,
        Light,
        Dark
    }

    public string Name { get; init; } = name;
    public AccentColor AccentColor { get; internal set; } = accentColor;
    public IAppBackground Background { get; internal set; } = background;
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
