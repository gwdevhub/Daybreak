using System.Collections.Generic;
using System.ComponentModel;

namespace Daybreak.Models.Builds;
public sealed class TeamBuildEntry : IBuildEntry
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string? name;
    private string? sourceUrl;
    private List<SingleBuildEntry> builds = [];

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
    public string? SourceUrl
    {
        get => this.sourceUrl;
        set
        {
            this.sourceUrl = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SourceUrl)));
        }
    }
    public List<SingleBuildEntry> Builds
    {
        get => this.builds;
        set
        {
            this.builds = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Builds)));
        }
    }
}
