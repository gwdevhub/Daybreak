using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.Themes;
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
    private const double XXSmallFontSizeValue = 0.56;
    private const double XSmallFontSizeValue = 0.625;
    private const double SmallFontSizeValue = 0.8125;
    private const double MediumFontSizeValue = 1.0;
    private const double LargeFontSizeValue = 1.125;
    private const double XLargeFontSizeValue = 1.5;
    private const double XXLargeFontSizeValue = 2.0;
    private const string LightThemeValue = "Light";
    private const string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes";
    private const string PersonalizeKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string AppsUseLightThemeValue = "AppsUseLightTheme";

    private readonly ILiveOptions<ThemeOptions> themeOptions = themeOptions;
    private readonly IOptionsUpdateHook optionsUpdateHook = optionsUpdateHook;
    private readonly ILogger<BlazorThemeInteropService> logger = logger;

    public event EventHandler<Theme>? ThemeChanged;

    public bool IsLightMode { get; private set; }
    public AccentColor AccentBaseColor { get; private set; } = AccentColor.Blue;
    public BackgroundColor NeutralBaseColor { get; private set; } = BackgroundColor.Gray40;
    public string AccentBaseColorHex { get; private set; } = string.Empty;
    public string NeutralBaseColorHex { get; private set; } = string.Empty;
    public string? BackdropImage { get; private set; }
    public string? BackdropEmbed { get; private set; }
    public float BaseLayerLuminance { get; private set; } = 0.0f;
    public double UIScale { get; private set; } = 1.0;
    public double XXSmallFontSize { get; private set; } = XXSmallFontSizeValue;
    public double XSmallFontSize { get; private set; } = XSmallFontSizeValue;
    public double SmallFontSize { get; private set; } = SmallFontSizeValue;
    public double MediumFontSize { get; private set; } = MediumFontSizeValue;
    public double LargeFontSize { get; private set; } = LargeFontSizeValue;
    public double XLargeFontSize { get; private set; } = XLargeFontSizeValue;
    public double XXLargeFontSize { get; private set; } = XXLargeFontSizeValue;
    public Theme? CurrentTheme { get; private set; }

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

    public void ReapplyTheme()
    {
        this.OnThemeUpdated();
    }

    private void OnThemeUpdated()
    {
        var themeOptions = this.themeOptions.Value;
        var theme = themeOptions.ApplicationTheme ?? CoreThemes.Daybreak;
        var accentColor = theme.AccentColor;
        var lightMode = theme.Mode is Theme.LightDarkMode.SystemSynchronized 
            ? IsWindowsLight()
            : theme.Mode is Theme.LightDarkMode.Light;

        var backgroundColor = lightMode ? BackgroundColor.Gray40 : BackgroundColor.Gray210;
        var baseLayerLuminance = lightMode ? 0.98f : 0.23f;
        this.CurrentTheme = theme;
        this.BackdropImage = theme.Background is StaticBackground staticBackground ? staticBackground.BackdropImage : default;
        this.BackdropEmbed = theme.Background is EmbedBackground embeddedBackground ? embeddedBackground.EmbedCode : default;
        this.IsLightMode = lightMode;
        this.AccentBaseColor = accentColor;
        this.AccentBaseColorHex = accentColor.Hex;
        this.NeutralBaseColor = backgroundColor;
        this.NeutralBaseColorHex = backgroundColor.Hex;
        this.BaseLayerLuminance = baseLayerLuminance;
        this.UIScale = themeOptions.ApplicationScale;
        this.XXSmallFontSize = XXSmallFontSizeValue * this.UIScale;
        this.XSmallFontSize = XSmallFontSizeValue * this.UIScale;
        this.SmallFontSize = SmallFontSizeValue * this.UIScale;
        this.MediumFontSize = MediumFontSizeValue * this.UIScale;
        this.LargeFontSize = LargeFontSizeValue * this.UIScale;
        this.XLargeFontSize = XLargeFontSizeValue * this.UIScale;
        this.XXLargeFontSize = XXLargeFontSizeValue * this.UIScale;
        this.ThemeChanged?.Invoke(this, theme);
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
