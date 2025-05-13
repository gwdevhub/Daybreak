using System;
using System.Diagnostics;

namespace Daybreak.Models.LaunchConfigurations;

public sealed record GuildWarsApplicationLaunchContext : IEquatable<GuildWarsApplicationLaunchContext>
{
    public LaunchConfigurationWithCredentials LaunchConfiguration { get; init; } = default!;
    public Process GuildWarsProcess { get; init; } = default!;
    public uint ProcessId { get; init; } = default!;

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
