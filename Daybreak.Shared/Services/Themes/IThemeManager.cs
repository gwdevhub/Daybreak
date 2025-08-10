namespace Daybreak.Shared.Services.Themes;
public interface IThemeManager
{
    event EventHandler? ThemeChanged;
    bool IsLightMode { get; }
    string AccentBaseColor { get; }
    string NeutralBaseColor { get; }
    float BaseLayerLuminance { get; }
    string BackdropImage { get; }
}
