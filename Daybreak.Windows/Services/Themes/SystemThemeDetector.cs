using Daybreak.Shared.Services.Themes;
using System.Extensions;
using WinRegistry = Microsoft.Win32.Registry;

namespace Daybreak.Windows.Services.Themes;

internal sealed class SystemThemeDetector : ISystemThemeDetector
{
    private const string LightThemeValue = "Light";
    private const string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes";
    private const string PersonalizeKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string AppsUseLightThemeValue = "AppsUseLightTheme";

    /*
     * By default, windows 10 comes with dark mode and windows 11 with light mode.
     * When migrating from windows 7 to windows 10, the theme will be called roamed.
     * As such, it is easier to check for the light theme and treat everything else as dark theme.
     */
    public bool IsLightTheme()
    {
        var theme = WinRegistry.GetValue(RegistryKey, "CurrentTheme", string.Empty)?.As<string>();
        // Windows 11 light theme is called aero.theme
        if (theme?.EndsWith("aero.theme") is true)
        {
            return true;
        }

        var themeMode = WinRegistry.GetValue(PersonalizeKey, AppsUseLightThemeValue, null);
        if (themeMode is int themeModeValue && themeModeValue is 1)
        {
            return true;
        }

        return LightThemeValue.Equals(theme?.Split('\\').Last().Split('.').First().ToString(), StringComparison.OrdinalIgnoreCase);
    }
}
