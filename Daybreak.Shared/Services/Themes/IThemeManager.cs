using System.Windows.Media;

namespace Daybreak.Shared.Services.Themes;
public interface IThemeManager
{
    // Hacky way to pass the theme without having to take a dependency on the theme manager
    object? GetCurrentTheme();
    Color GetForegroundColor();
}
