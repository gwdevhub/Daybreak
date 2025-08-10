using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Themes;
using Daybreak.Themes;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Launch;
/// <summary>
/// Interaction logic for SplashWindow.xaml
/// </summary>
public partial class SplashWindow : Window
{
    private readonly StartupStatus startupStatus;

    [GenerateDependencyProperty]
    private string splashText = string.Empty;

    public SplashWindow(
        IOptions<ThemeOptions> options,
        StartupStatus startupStatus)
    {
        this.startupStatus = startupStatus.ThrowIfNull();
        this.startupStatus.PropertyChanged += this.StartupStatus_PropertyChanged;
        this.InitializeComponent();

        var theme = options.Value.ApplicationTheme ?? CoreThemes.Daybreak;

        this.Foreground = theme.Mode is Theme.LightDarkMode.Dark ?
            new SolidColorBrush(Colors.White) :
            new SolidColorBrush(Colors.Black);
        this.Background = theme.Mode is Theme.LightDarkMode.Dark ?
            new SolidColorBrush(ColorPalette.BackgroundColor.Gray210.Color) :
            new SolidColorBrush(ColorPalette.BackgroundColor.Gray40.Color);
        this.SplashText = this.startupStatus.CurrentStep.Description;
    }

    private void StartupStatus_PropertyChanged(object? _, PropertyChangedEventArgs __)
    {
        this.Dispatcher.InvokeAsync(() => this.SplashText = this.startupStatus.CurrentStep.Description);
    }
}
