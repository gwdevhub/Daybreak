using Daybreak.Models.Guildwars;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Builds;
public sealed class TeamBuildEntry : BuildEntryBase, IEquatable<TeamBuildEntry>
{
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
}
