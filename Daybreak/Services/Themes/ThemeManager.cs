using ControlzEx.Theming;
using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Utils;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;
using System.Windows;
using System.Windows.Extensions.Services;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace Daybreak.Services.Themes;
internal sealed class ThemeManager : IThemeManager, IApplicationLifetimeService
{
    private const string LightThemeValue = "Light";
    private const string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes";
    private const string PersonalizeKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string AppsUseLightThemeValue = "AppsUseLightTheme";
    private const string DarkMode = "Dark";
    private const string LightMode = "Light";

    private static readonly Uri LightThemeUri =
        new Uri("pack://application:,,,/Wpf.Ui;component/Resources/Theme/Light.xaml", UriKind.Absolute);
    private static readonly Uri DarkThemeUri =
        new Uri("pack://application:,,,/Wpf.Ui;component/Resources/Theme/Dark.xaml", UriKind.Absolute);

    private readonly ILiveOptions<ThemeOptions> themeOptions;

    public ThemeManager(
        IOptionsUpdateHook optionsUpdateHook,
        ILiveOptions<ThemeOptions> themeOptions)
    {
        this.themeOptions = themeOptions.ThrowIfNull();
        optionsUpdateHook.ThrowIfNull().RegisterHook<ThemeOptions>(this.UpdateTheme);
        this.SetAndReturnCurrentTheme();
    }

    public Color GetForegroundColor()
    {
        var themeManager = ControlzEx.Theming.ThemeManager.Current;
        (var mode, var theme) = this.GetCurrentModeAndTheme();
        var definedTheme = themeManager.GetTheme(mode, theme);
        var mahAppsResources = definedTheme?.Resources.MergedDictionaries.First().MergedDictionaries.First();
        return (Color)mahAppsResources!["MahApps.Colors.ThemeForeground"];
    }

    public object? GetCurrentTheme()
    {
        return ControlzEx.Theming.ThemeManager.Current.DetectTheme(Launcher.Instance) ?? throw new InvalidOperationException("Unable to retrieve theme");
    }

    public void OnClosing()
    {
    }

    public void OnStartup()
    {
    }

    private void UpdateTheme()
    {
        this.SetAndReturnCurrentTheme();
    }

    private Theme SetAndReturnCurrentTheme()
    {
        var themeManager = ControlzEx.Theming.ThemeManager.Current;
        (var mode, var theme) = this.GetCurrentModeAndTheme();
        var definedTheme = themeManager.GetTheme(mode, theme);
        var mahAppsResources = definedTheme?.Resources.MergedDictionaries.First().MergedDictionaries.First()!;
        var definedBackgroundBrush = mahAppsResources["MahApps.Brushes.ThemeBackground"].Cast<SolidColorBrush>();
        var foregroundColor = mahAppsResources["MahApps.Colors.ThemeForeground"].Cast<Color>();
        var foregroundColorToBlend = Color.FromArgb(80, foregroundColor.R, foregroundColor.G, foregroundColor.B);
        var backgroundColorToBlend = Color.FromArgb(200, definedBackgroundBrush.Color.R, definedBackgroundBrush.Color.G, definedBackgroundBrush.Color.B);
        mahAppsResources["Daybreak.Brushes.Background"] = new SolidColorBrush(backgroundColorToBlend);
        mahAppsResources["Daybreak.Brushes.Kurzick"] = new SolidColorBrush(ColorPalette.Blue);
        mahAppsResources["Daybreak.Brushes.Luxon"] = new SolidColorBrush(ColorPalette.Orange);
        mahAppsResources["Daybreak.Brushes.Imperial"] = new SolidColorBrush(ColorPalette.Purple);
        mahAppsResources["Daybreak.Brushes.Balthazar"] = new SolidColorBrush(ColorPalette.Yellow);
        mahAppsResources["Daybreak.Brushes.Experience"] = new SolidColorBrush(ColorPalette.Green);
        mahAppsResources["Daybreak.Brushes.Health"] = new SolidColorBrush(ColorPalette.Red);
        mahAppsResources["Daybreak.Brushes.Energy"] = new SolidColorBrush(ColorPalette.Teal);
        mahAppsResources["Daybreak.Brushes.Rank"] = new SolidColorBrush(ColorPalette.Amber);
        mahAppsResources["Daybreak.Brushes.Vanquish"] = new SolidColorBrush(ColorPalette.Indigo);
        foreach(var member in typeof(ColorPalette).GetMembers().OfType<FieldInfo>().Where(m => m.FieldType == typeof(Color)))
        {
            var color = member.GetValue(default)!.Cast<Color>();
            var blendedColor = color.AlphaBlend(foregroundColorToBlend);
            mahAppsResources[$"Daybreak.Brushes.{member.Name}"] = new SolidColorBrush(color);
            mahAppsResources[$"Daybreak.BlendedBrushes.{member.Name}"] = new SolidColorBrush(blendedColor);
        }

        ApplicationThemeManager.ApplySystemTheme(true);
        ChangeTheme(themeManager, mode, theme);
        return definedTheme!;
    }

    private (string Mode, string Theme) GetCurrentModeAndTheme()
    {
        var mode = this.themeOptions.Value.DarkMode ? DarkMode : LightMode;
        if (this.themeOptions.Value.SystemSynchronization)
        {
            mode = IsWindowsLight() ? LightMode : DarkMode;
        }

        var theme = this.themeOptions.Value.Theme.ToString();
        return (mode, theme);
    }

    private static void ChangeTheme(ControlzEx.Theming.ThemeManager themeManager, string mode, string theme)
    {
        if (mode == LightMode)
        {
            themeManager.ChangeTheme(Launcher.Instance, DarkMode, theme);
        }

        themeManager.ChangeTheme(Launcher.Instance, mode, theme);
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
