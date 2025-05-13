using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct PartyContext
{
    [FieldOffset(0x0040)]
    public readonly GuildwarsPointerArray<PartyInfoContext> Parties;

    [FieldOffset(0x0054)]
    public readonly GuildwarsPointer<PartyInfoContext> PlayerParty;

    [FieldOffset(0x00C0)]
    public readonly GenericGuildwarsArray PartySearchPointers;
}
