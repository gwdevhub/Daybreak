namespace Daybreak.Models.Interop;

public readonly struct GuildwarsPointer<T>
{
    public readonly uint Address;

    public bool IsValid()
    {
        return this.Address != 0x0;
    }
}
