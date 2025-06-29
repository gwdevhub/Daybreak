namespace Daybreak.Shared.Models;

public sealed class LoginCredentials : IEquatable<LoginCredentials>
{
    public string? Identifier { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public bool Equals(LoginCredentials? other)
    {
        return this?.Identifier?.Equals(other?.Identifier) is true &&
            this?.Username?.Equals(other?.Username) is true &&
            this?.Password?.Equals(other?.Password) is true;
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as LoginCredentials);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Identifier, this.Username, this.Password);
    }
}
