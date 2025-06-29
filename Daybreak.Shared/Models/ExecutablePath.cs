using System.ComponentModel;

namespace Daybreak.Shared.Models;

public sealed class ExecutablePath : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Path
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Path)));
        }
    } = string.Empty;
}
