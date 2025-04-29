using Daybreak.Models.LaunchConfigurations;
using System.ComponentModel;

namespace Daybreak.Models;
public sealed class LauncherViewContext : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public LaunchConfigurationWithCredentials? Configuration { get; init; }

    public bool IsSelected
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsSelected)));
        }
    } = false;

    public bool CanLaunch
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CanLaunch)));
        }
    } = false;

    public bool CanKill
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CanKill)));
        }
    } = false;
}
