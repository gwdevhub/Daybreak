namespace Daybreak.API.Interop.GuildWars;

public readonly unsafe struct WrappedPointer<T>
    where T : unmanaged
{
    public readonly T* Pointer;
}
