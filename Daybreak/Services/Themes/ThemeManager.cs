using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Models;
using Daybreak.Services.Options;
using Daybreak.Utils;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Reflection;
using System.Windows.Extensions.Services;
using System.Windows.Media;

namespace Daybreak.Services.Themes;
public sealed class ThemeManager : IThemeManager, IApplicationLifetimeService
{
    private const string BackgroundKey = "Daybreak.Brushes.Background";
    private const string MahAppsBackgroundKey = "MahApps.Brushes.ThemeBackground";
    private const string DarkMode = "Dark";
    private const string LightMode = "Light";

    private readonly ILiveOptions<ThemeOptions> themeOptions;

    public ThemeManager(
        IOptionsUpdateHook optionsUpdateHook,
        ILiveOptions<ThemeOptions> themeOptions)
    {
        this.themeOptions = themeOptions.ThrowIfNull();
        optionsUpdateHook.ThrowIfNull().RegisterHook<ThemeOptions>(this.UpdateTheme);
    }

    public Color GetForegroundColor()
    {
        var themeManager = ControlzEx.Theming.ThemeManager.Current;
        var mode = this.themeOptions.Value.DarkMode ? DarkMode : LightMode;
        var theme = this.themeOptions.Value.Theme.ToString();
        var definedTheme = themeManager.GetTheme(mode, theme);
        var mahAppsResources = definedTheme?.Resources.MergedDictionaries.First().MergedDictionaries.First();
        return (Color)mahAppsResources!["MahApps.Colors.ThemeForeground"];
    }

    public void OnClosing()
    {
    }

    public void OnStartup()
    {
        this.SetCurrentTheme();
    }

    private void UpdateTheme()
    {
        this.SetCurrentTheme();
    }

    private void SetCurrentTheme()
    {
        var themeManager = ControlzEx.Theming.ThemeManager.Current;
        var mode = this.themeOptions.Value.DarkMode ? DarkMode : LightMode;
        var theme = this.themeOptions.Value.Theme.ToString();
        var definedTheme = themeManager.GetTheme(mode, theme);
        var mahAppsResources = definedTheme?.Resources.MergedDictionaries.First().MergedDictionaries.First()!;
        var definedBackgroundBrush = mahAppsResources["MahApps.Brushes.ThemeBackground"].Cast<SolidColorBrush>();
        var foregroundColor = mahAppsResources["MahApps.Colors.ThemeForeground"].Cast<Color>();
        var foregroundColorToBlend = Color.FromArgb(80, foregroundColor.R, foregroundColor.G, foregroundColor.B);
        mahAppsResources["Daybreak.Brushes.Background"] = new SolidColorBrush(definedBackgroundBrush!.Color) { Opacity = 0.8 };
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

        themeManager.ChangeTheme(Launcher.Instance, mode, theme);
    }
}
