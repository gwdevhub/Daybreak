namespace Daybreak.Shared.Models.LaunchConfigurations;

public sealed class LaunchConfigurationWithCredentials : IEquatable<LaunchConfigurationWithCredentials>
{
    public string? Identifier { get; init; }
    public string? Name { get; set; }
    public string? ExecutablePath { get; set; }
    public string? Arguments { get; set; }
    public bool SteamSupport { get; set; } = true;
    public LoginCredentials? Credentials { get; set; }

    public bool Equals(LaunchConfigurationWithCredentials? other)
    {
        if (other is null)
        {
            return false;
        }

        return this?.Identifier?.Equals(other?.Identifier) is true &&
            this?.ExecutablePath?.Equals(other?.ExecutablePath) is not false && //ExecutablePath can be null, so in that case the comparison returns null
            this?.Credentials?.Equals(other?.Credentials) is true &&
            this?.Name?.Equals(other?.Name) is true &&
            this?.SteamSupport == other?.SteamSupport;
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as LaunchConfigurationWithCredentials);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.ExecutablePath, this.Credentials?.GetHashCode());
    }

    public static bool operator ==(LaunchConfigurationWithCredentials l1, LaunchConfigurationWithCredentials l2)
    {
        return l1?.Equals(l2) is true;
    }

    public static bool operator !=(LaunchConfigurationWithCredentials l1, LaunchConfigurationWithCredentials l2)
    {
        return l1?.Equals(l2) is not true;
    }
}
