using System.Core.Extensions;

namespace Daybreak.Shared.Models.Versioning;

public sealed class VersionStringToken : VersionToken
{
    public string Token { get; }
    internal VersionStringToken(string token)
    {
        this.Token = token.ThrowIfNull();
    }

    protected override string Stringify()
    {
        return this.Token;
    }

    public override bool Equals(object? obj)
    {
        return this.Token.Equals(obj);
    }

    public override int GetHashCode()
    {
        return this.Token.GetHashCode();
    }

    public override string ToString()
    {
        return this.Token.ToString();
    }
}
