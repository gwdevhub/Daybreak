namespace Daybreak.Shared.Services.Themes;
public interface IThemeManager
{
    event EventHandler? ThemeChanged;
    bool IsLightMode { get; }
    string AccentBaseColor { get; }
    string NeutralBaseColor { get; }
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
