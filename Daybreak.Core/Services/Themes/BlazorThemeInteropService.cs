using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.ColorPalette;
using Daybreak.Shared.Models.Themes;
using Daybreak.Shared.Services.Themes;
using Daybreak.Themes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Extensions;

namespace Daybreak.Services.Themes;

public class BlazorThemeInteropService(
    IOptionsMonitor<ThemeOptions> themeOptions,
    ISystemThemeDetector systemThemeDetector,
    ILogger<BlazorThemeInteropService> logger) : IThemeManager, IHostedService
{
    private const double XXSmallFontSizeValue = 0.56;
    private const double XSmallFontSizeValue = 0.625;
    private const double SmallFontSizeValue = 0.8125;
    private const double MediumFontSizeValue = 1.0;
    private const double LargeFontSizeValue = 1.125;
    private const double XLargeFontSizeValue = 1.5;
    private const double XXLargeFontSizeValue = 2.0;

    private readonly IOptionsMonitor<ThemeOptions> themeOptions = themeOptions;
    private readonly ISystemThemeDetector systemThemeDetector = systemThemeDetector;
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

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.themeOptions.OnChange(this.OnThemeUpdated);
        this.OnThemeUpdated(this.themeOptions.CurrentValue, default);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void ReapplyTheme()
    {
        this.OnThemeUpdated(this.themeOptions.CurrentValue, default);
    }

    private void OnThemeUpdated(ThemeOptions themeOptions, string? _)
    {
        var theme = themeOptions.ApplicationTheme ?? CoreThemes.Daybreak;
        var accentColor = theme.AccentColor;
        var lightMode = theme.Mode is Theme.LightDarkMode.SystemSynchronized 
            ? this.systemThemeDetector.IsLightTheme()
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

}
