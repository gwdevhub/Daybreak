using Daybreak.Models.LaunchConfigurations;
using System.ComponentModel;

namespace Daybreak.Models;
public sealed class LauncherViewContext : INotifyPropertyChanged
{
    private bool canLaunch = false;
    private bool canKill = false;

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

    public bool CanKill
    {
        get => this.canKill;
        set
        {
            this.canKill = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CanKill)));
        }
    }
}
