using System.ComponentModel;

namespace Daybreak.Models.Progress;

public sealed class StartupStatus : INotifyPropertyChanged
{
    public static readonly StartupStep Starting = new("Starting Daybreak");
    public static readonly StartupStep Finished = new("Opening Daybreak window");
    public static StartupStep Custom(string description) => new(description);

    public event PropertyChangedEventHandler? PropertyChanged;

    private StartupStep currentStep = Starting;

    public StartupStep CurrentStep
    {
        get => this.currentStep;
        set
        {
            this.currentStep = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentStep)));
        }
    }

    public class StartupStep : LoadStatus
    {
        public StartupStep(string description) : base(description)
        {
        }
    }
}
