using System.ComponentModel;

namespace Daybreak.Shared.Models.Progress;

public abstract class ActionStatus : INotifyPropertyChanged
{
    private LoadStatus currentStep = default!;

    public event PropertyChangedEventHandler? PropertyChanged;

    public LoadStatus CurrentStep
    {
        get => this.currentStep;
        set
        {
            this.currentStep = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentStep)));
        }
    }
}
