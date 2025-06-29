using System.ComponentModel;

namespace Daybreak.Shared.Models.Progress;

public sealed class StartupStatus : INotifyPropertyChanged
{
    public static readonly StartupStep Starting = new("Starting Daybreak");
    public static readonly StartupStep Finished = new("Opening Daybreak window");
    public static StartupStep Custom(string description) => new(description);

    public event PropertyChangedEventHandler? PropertyChanged;

    public StartupStep CurrentStep
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentStep)));
        }
    } = Starting;

    public class StartupStep(string description) : LoadStatus(description)
    {
    }
}
