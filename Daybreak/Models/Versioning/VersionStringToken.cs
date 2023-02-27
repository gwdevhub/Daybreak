namespace Daybreak.Models.Versioning;

public sealed class VersionStringToken : VersionToken
{
    public string Token { get; }
    internal VersionStringToken(string token)
    {
        this.Token = token;
    }

    protected override string Stringify()
    {
        return this.Token;
    }
}
