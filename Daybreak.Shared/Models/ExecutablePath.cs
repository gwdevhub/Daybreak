using System.ComponentModel;

namespace Daybreak.Models;

public sealed class ExecutablePath : INotifyPropertyChanged
{
    private string path = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Path
    {
        get => this.path;
        set
        {
            this.path = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Path)));
        }
    }
}
