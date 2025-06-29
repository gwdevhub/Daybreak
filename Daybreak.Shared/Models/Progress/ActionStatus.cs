using System.ComponentModel;

namespace Daybreak.Shared.Models.Progress;

public abstract class ActionStatus : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public LoadStatus CurrentStep
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentStep)));
        }
    } = default!;
}
