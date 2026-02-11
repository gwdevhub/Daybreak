namespace Daybreak.Shared.Models.LaunchConfigurations;

public sealed class LaunchConfigurationWithCredentials : IEquatable<LaunchConfigurationWithCredentials>
{
    public string? Identifier { get; init; }
    public string? Name { get; set; }
    public string? ExecutablePath { get; set; }
    public string? Arguments { get; set; }
    public bool SteamSupport { get; set; } = true;
    public LoginCredentials? Credentials { get; set; }
    public List<string>? EnabledMods { get; set; }
    public bool CustomModLoadoutEnabled { get; set; }

    public bool Equals(LaunchConfigurationWithCredentials? other)
    {
        if (other is null)
        {
            return false;
        }

        return
            ((this.Identifier is null && other.Identifier is null) ||
            (this.Identifier is not null && other.Identifier is not null && string.Equals(this.Identifier, other.Identifier, StringComparison.Ordinal))) &&

            ((this.ExecutablePath is null && other.ExecutablePath is null) ||
            (this.ExecutablePath is not null && other.ExecutablePath is not null && string.Equals(this.ExecutablePath, other.ExecutablePath, StringComparison.Ordinal))) &&

            ((this.Name is null && other.Name is null) ||
            (this.Name is not null && other.Name is not null && string.Equals(this.Name, other.Name, StringComparison.Ordinal))) &&

            this.Credentials?.Equals(other.Credentials) is true &&
            this.SteamSupport == other.SteamSupport &&

            this.EnabledMods?.SequenceEqual(other.EnabledMods ?? []) is true &&
            this.CustomModLoadoutEnabled == other.CustomModLoadoutEnabled;
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
