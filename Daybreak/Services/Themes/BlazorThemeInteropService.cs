using Daybreak.Shared.Models;
using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Windows.Media;

namespace Daybreak.Services.Themes;

public class BlazorThemeInteropService(
    ILogger<BlazorThemeInteropService> logger)
{
    private const string LightThemeValue = "Light";
    private const string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes";
    private const string PersonalizeKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string AppsUseLightThemeValue = "AppsUseLightTheme";

    private readonly ILogger<BlazorThemeInteropService> logger = logger;

    public string AccentBaseColor { get; private set; } = string.Empty;
    public string NeutralBaseColor { get; private set; } = string.Empty;
    public float BaseLayerLuminance { get; private set; } = 0.0f;

    public void InitializeTheme()
    {
        var lightMode = IsWindowsLight();
        var backgroundColor = lightMode ? Colors.Snow : Colors.DarkGray;
        var baseLayerLuminance = lightMode ? 0.98f : 0.23f;
        var accentColor = ColorPalette.DeepOrange;
        this.AccentBaseColor = ColorToHex(accentColor);
        this.NeutralBaseColor = ColorToHex(backgroundColor);
        this.BaseLayerLuminance = baseLayerLuminance;
    }

    private static string ColorToHex(Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /*
     * By default, windows 10 comes with dark mode and windows 11 with light mode.
     * When migrating from windows 7 to windows 10, the theme will be called roamed.
     * As such, it is easier to check for the light theme and treat everything else as dark theme.
     */
    private static bool IsWindowsLight()
    {
        var theme = Microsoft.Win32.Registry.GetValue(RegistryKey, "CurrentTheme", string.Empty)?.As<string>();
        // Windows 11 light theme is called aero.theme
        if (theme?.EndsWith("aero.theme") is true)
        {
            return true;
        }


        var themeMode = Microsoft.Win32.Registry.GetValue(PersonalizeKey, AppsUseLightThemeValue, null);
        if (themeMode is int themeModeValue && themeModeValue is 1)
        {
            return true;
        }

        return LightThemeValue.Equals(theme?.Split('\\').Last().Split('.').First().ToString(), System.StringComparison.OrdinalIgnoreCase);
    }
}
