using Daybreak.Models.Progress;
using System.Core.Extensions;
using System.Windows;

namespace Daybreak.Launch;
/// <summary>
/// Interaction logic for SplashWindow.xaml
/// </summary>
public partial class SplashWindow : Window
{
    public StartupStatus StartupStatus { get; init; }

    public SplashWindow(
        StartupStatus startupStatus)
    {
        this.StartupStatus = startupStatus.ThrowIfNull();
        this.InitializeComponent();
    }
}
