namespace Daybreak.Shared.Models.Interop;

public readonly struct GuildwarsPointer<T>
{
    public readonly uint Address;

    public bool IsValid()
    {
        return this.Address != 0x0;
    }

    public override string ToString()
    {
        return this.Address.ToString("X");
    }
}
