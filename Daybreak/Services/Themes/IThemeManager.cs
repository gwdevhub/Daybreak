using ControlzEx.Theming;
using System.Windows.Media;

namespace Daybreak.Services.Themes;
public interface IThemeManager
{
    Theme GetCurrentTheme();
    Color GetForegroundColor();
}
