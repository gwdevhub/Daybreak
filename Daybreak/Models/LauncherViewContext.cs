using Daybreak.Models.LaunchConfigurations;
using System.ComponentModel;

namespace Daybreak.Models;
public sealed class LauncherViewContext : INotifyPropertyChanged
{
    private bool canLaunch = false;

    public event PropertyChangedEventHandler? PropertyChanged;

    public LaunchConfigurationWithCredentials? Configuration { get; init; }
    public bool CanLaunch
    {
        get => this.canLaunch;
        set
        {
            this.canLaunch = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CanLaunch)));
        }
    }
}
