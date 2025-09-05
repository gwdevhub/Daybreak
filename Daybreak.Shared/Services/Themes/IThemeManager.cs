using static Daybreak.Shared.Models.ColorPalette;

namespace Daybreak.Shared.Services.Themes;
public interface IThemeManager
{
    event EventHandler? ThemeChanged;
    bool IsLightMode { get; }
    AccentColor AccentBaseColor { get; }
    BackgroundColor NeutralBaseColor { get; }
    string AccentBaseColorHex { get; }
    string NeutralBaseColorHex { get; }
    float BaseLayerLuminance { get; }
    string BackdropImage { get; }
    double UIScale { get; }
    double XXSmallFontSize { get; }
    double XSmallFontSize { get; }
    double SmallFontSize { get; }
    double MediumFontSize { get; }
    double LargeFontSize { get; }
    double XLargeFontSize { get; }
    double XXLargeFontSize { get; }
}
