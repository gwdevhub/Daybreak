using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Themes;
using Daybreak.Themes;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Extensions;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Themes;

public class BlazorThemeInteropService(
    ILiveOptions<ThemeOptions> themeOptions,
    IOptionsUpdateHook optionsUpdateHook,
    ILogger<BlazorThemeInteropService> logger) : IThemeManager, IThemeProducer, IApplicationLifetimeService
{
    private const string LightThemeValue = "Light";
    private const string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes";
    private const string PersonalizeKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string AppsUseLightThemeValue = "AppsUseLightTheme";

    private readonly ILiveOptions<ThemeOptions> themeOptions = themeOptions;
    private readonly IOptionsUpdateHook optionsUpdateHook = optionsUpdateHook;
    private readonly ILogger<BlazorThemeInteropService> logger = logger;

    public event EventHandler? ThemeChanged;

    public bool IsLightMode { get; private set; }
    public string AccentBaseColor { get; private set; } = string.Empty;
    public string NeutralBaseColor { get; private set; } = string.Empty;
    public string BackdropImage { get; private set; } = string.Empty;
    public float BaseLayerLuminance { get; private set; } = 0.0f;

    public void OnStartup()
    {
        this.optionsUpdateHook.RegisterHook<ThemeOptions>(this.OnThemeUpdated);
        this.OnThemeUpdated();
    }

    public void OnClosing()
    {
    }

    public void RegisterTheme(Theme theme)
    {
        Theme.Themes.Add(theme);
    }

    private void OnThemeUpdated()
    {
        var themeOptions = this.themeOptions.Value;
        var theme = themeOptions.ApplicationTheme ?? CoreThemes.Daybreak;
        var accentColor = theme.AccentColor;
        var lightMode = theme.Mode is Theme.LightDarkMode.SystemSynchronized 
            ? IsWindowsLight()
            : theme.Mode is Theme.LightDarkMode.Light;

        var backgroundColor = lightMode ? ColorPalette.BackgroundColor.Gray40 : ColorPalette.BackgroundColor.Gray210;
        var baseLayerLuminance = lightMode ? 0.98f : 0.23f;
        this.BackdropImage = theme.Backdrop;
        this.IsLightMode = lightMode;
        this.AccentBaseColor = accentColor.Hex;
        this.NeutralBaseColor = backgroundColor.Hex;
        this.BaseLayerLuminance = baseLayerLuminance;
        this.ThemeChanged?.Invoke(this, EventArgs.Empty);
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
