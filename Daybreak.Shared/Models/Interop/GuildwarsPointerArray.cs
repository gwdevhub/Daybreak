using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Sequential)]
public readonly struct GuildwarsPointerArray<T>
{
    public readonly GuildwarsPointer<uint> Buffer;

    public readonly uint Capacity;

    public readonly uint Size;

    public readonly uint Param;
}
