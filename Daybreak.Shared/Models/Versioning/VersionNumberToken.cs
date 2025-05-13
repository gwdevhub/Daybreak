namespace Daybreak.Models.Versioning;

public sealed class VersionNumberToken : VersionToken
{
    public int Number { get; }
    internal VersionNumberToken(int number)
    {
        this.Number = number;
    }

    protected override string Stringify()
    {
        return this.Number.ToString();
    }

    public override bool Equals(object? obj)
    {
        return this.Number.Equals(obj);
    }

    public override int GetHashCode()
    {
        return this.Number.GetHashCode();
    }

    public override string ToString()
    {
        return this.Number.ToString();
    }
}
