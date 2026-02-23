using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
[GWCAEquivalent("GameContext")]
public readonly unsafe struct GameContext
{
    [FieldOffset(0x0008)]
    public readonly AgentGameContext* AgentGameContext;

    [FieldOffset(0x0014)]
    public readonly MapContext* MapContext;

    [FieldOffset(0x0018)]
    public readonly TextParserContext* TextParserContext;

    [FieldOffset(0x0028)]
    public readonly AccountGameContext* AccountContext;

    [FieldOffset(0x002C)]
    public readonly WorldContext* WorldContext;

    [FieldOffset(0x003C)]
    public readonly GuildContext* GuildContext;

    [FieldOffset(0x0040)]
    public readonly ItemContext* ItemContext;

    [FieldOffset(0x0044)]
    public readonly CharContext* CharContext;

    [FieldOffset(0x004C)]
    public readonly PartyContext* PartyContext;
}
