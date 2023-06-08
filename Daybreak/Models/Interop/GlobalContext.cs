using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GlobalContext
{
    [FieldOffset(0x0008)]
    public readonly GuildwarsPointer<InstanceContext> InstanceContext;

    [FieldOffset(0x0014)]
    public readonly GuildwarsPointer<MapContext> MapContext;

    [FieldOffset(0x002C)]
    public readonly GuildwarsPointer<GameContext> GameContext;

    [FieldOffset(0x0040)]
    public readonly GuildwarsPointer<ItemContext> ItemContext;

    [FieldOffset(0x0044)]
    public readonly GuildwarsPointer<UserContext> UserContext;
}
