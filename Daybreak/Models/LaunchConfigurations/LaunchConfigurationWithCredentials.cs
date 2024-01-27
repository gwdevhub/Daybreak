using System;

namespace Daybreak.Models.LaunchConfigurations;

public sealed class LaunchConfigurationWithCredentials : IEquatable<LaunchConfigurationWithCredentials>
{
    public string? Identifier { get; init; }
    public string? ExecutablePath { get; set; }
    public LoginCredentials? Credentials { get; set; }

    public bool Equals(LaunchConfigurationWithCredentials? other)
    {
        if (other is null)
        {
            return false;
        }

        return this?.Identifier?.Equals(other?.Identifier) is true &&
            this?.ExecutablePath?.Equals(other?.ExecutablePath) is true &&
            this?.Credentials?.Equals(other?.Credentials) is true;
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
