using ControlzEx.Theming;
using System.Windows.Media;

namespace Daybreak.Shared.Services.Themes;
public interface IThemeManager
{
    Theme GetCurrentTheme();
    Color GetForegroundColor();
}
