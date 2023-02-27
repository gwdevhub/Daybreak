using Daybreak.Models.Guildwars;
using System.ComponentModel;

namespace Daybreak.Models.Builds;

public sealed class BuildEntry : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string? name;
    private Build? build;

    public string? PreviousName { get; set; }
    public string? Name
    {
        get => this.name;
        set
        {
            this.name = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Name)));
        }
    }
    public Build? Build
    {
        get => this.build;
        set
        {
            this.build = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Build)));
        }
    }
}
