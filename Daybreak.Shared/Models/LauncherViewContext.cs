using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.Mods;
using System.Collections.Generic;
using System.ComponentModel;

namespace Daybreak.Shared.Models;
public sealed class LauncherViewContext : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public LaunchConfigurationWithCredentials? Configuration { get; init; }

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

    public bool CanAttach
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CanAttach)));
        }
    } = false;

    public bool GameRunning
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GameRunning)));
        }
    } = false;

    public bool ShowJustName
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ShowJustName)));
        }
    } = false;

    public GuildWarsApplicationLaunchContext? AppContext
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AppContext)));
        }
    }

    public ScopedApiContext? ApiContext
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ApiContext)));
        }
    }

    public IEnumerable<IModService>? ModsToReapply
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ModsToReapply)));
        }
    }
}
