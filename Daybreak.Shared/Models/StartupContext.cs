using Daybreak.Shared.Models.Async;
using System.ComponentModel;

namespace Daybreak.Shared.Models;
public sealed class StartupContext : INotifyPropertyChanged
{
    public ProgressUpdate ProgressUpdate
    {
        get => field;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ProgressUpdate)));
        }
    } = new ProgressUpdate(0, string.Empty);

    public event PropertyChangedEventHandler? PropertyChanged;
}
