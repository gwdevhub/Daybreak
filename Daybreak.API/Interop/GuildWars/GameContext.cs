using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly unsafe struct GameContext
{
    [FieldOffset(0x002C)]
    public readonly WorldContext* WorldContext;

    [FieldOffset(0x003C)]
    public readonly GuildContext* GuildContext;

    [FieldOffset(0x0044)]
    public readonly CharContext* CharContext;

    [FieldOffset(0x004C)]
    public readonly PartyContext* PartyContext;
}
