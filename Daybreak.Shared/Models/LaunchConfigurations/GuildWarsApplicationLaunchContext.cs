using System;
using System.Diagnostics;

namespace Daybreak.Shared.Models.LaunchConfigurations;

public sealed record GuildWarsApplicationLaunchContext : IEquatable<GuildWarsApplicationLaunchContext>
{
    public required LaunchConfigurationWithCredentials LaunchConfiguration { get; init; }
    public required Process GuildWarsProcess { get; init; }
    public required uint ProcessId { get; init; }

    public GuildWarsApplicationLaunchContext()
    {
    }

    public bool Equals(GuildWarsApplicationLaunchContext? other)
    {
        return this?.LaunchConfiguration.Equals(other?.LaunchConfiguration) is true &&
            this?.GuildWarsProcess.Id == other?.GuildWarsProcess.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.LaunchConfiguration, this.GuildWarsProcess.Id);
    }
}
