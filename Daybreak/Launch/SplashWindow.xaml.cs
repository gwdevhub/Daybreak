using Daybreak.Shared.Models.Progress;
using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Extensions;

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
        StartupStatus startupStatus)
    {
        this.startupStatus = startupStatus.ThrowIfNull();
        this.startupStatus.PropertyChanged += this.StartupStatus_PropertyChanged;
        this.InitializeComponent();

        this.SplashText = this.startupStatus.CurrentStep.Description;
    }

    private void StartupStatus_PropertyChanged(object? _, PropertyChangedEventArgs __)
    {
        this.Dispatcher.InvokeAsync(() => this.SplashText = this.startupStatus.CurrentStep.Description);
    }
}
