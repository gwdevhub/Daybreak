using Daybreak.Shared.Models.Guildwars;
using System.ComponentModel;

namespace Daybreak.Shared.Models.Builds;

public sealed class TeamBuildEntry : IBuildEntry, INotifyPropertyChanged, IEquatable<TeamBuildEntry>
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string? Name
    {
        get;
        set
        {
            field = value;
            this.OnPropertyChanged(nameof(this.Name));
        }
    }

    public string? PreviousName { get; set; }

    public string? SourceUrl
    {
        get;
        set
        {
            field = value;
            this.OnPropertyChanged(nameof(this.SourceUrl));
        }
    }

    public DateTime CreationTime
    {
        get;
        set
        {
            field = value;
            this.OnPropertyChanged(nameof(this.CreationTime));
        }
    }

    public int? ToolboxBuildId
    {
        get;
        set
        {
            field = value;
            this.OnPropertyChanged(nameof(this.ToolboxBuildId));
        }
    }

    public bool IsToolboxBuild
    {
        get;
        set
        {
            field = value;
            this.OnPropertyChanged(nameof(this.IsToolboxBuild));
        }
    }

    public List<PartyCompositionMetadataEntry>? PartyComposition
    {
        get;
        set
        {
            field = value;
            this.OnPropertyChanged(nameof(this.PartyComposition));
        }
    }

    public List<SingleBuildEntry> Builds
    {
        get;
        set
        {
            field = value;
            this.OnPropertyChanged(nameof(this.Builds));
        }
    } = [];

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

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as TeamBuildEntry);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Name, this.PreviousName, this.SourceUrl, this.Builds.Select(c => c.GetHashCode()));
    }

    private void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
