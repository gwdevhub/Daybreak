using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Extensions;
using System.Linq;

namespace Daybreak.Models.Builds;
public sealed class TeamBuildEntry : IBuildEntry, INotifyPropertyChanged, IEquatable<TeamBuildEntry>
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

    public bool Equals(TeamBuildEntry? other)
    {
        return this.Name == other?.Name &&
            this.PreviousName == other?.PreviousName &&
            this.SourceUrl == other?.SourceUrl &&
            this.Builds.Select((thisBuild, index) =>
            {
                var otherBuild = other?.Builds.Skip(index).FirstOrDefault();
                return thisBuild.Primary == otherBuild?.Primary &&
                    thisBuild.Secondary == otherBuild?.Secondary &&
                    thisBuild.Skills.SequenceEqual(otherBuild.Skills);
            }).All(result => result);
    }
}
