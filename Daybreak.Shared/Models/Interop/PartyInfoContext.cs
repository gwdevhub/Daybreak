using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct PartyInfoContext
{
    [FieldOffset(0x0000)]
    public readonly uint PartyId;

    [FieldOffset(0x0004)]
    public readonly GuildwarsArray<PlayerPartyMember> Players;

    [FieldOffset(0x0014)]
    public readonly GuildwarsArray<HenchmanPartyMember> Henchmen;

    [FieldOffset(0x0024)]
    public readonly GuildwarsArray<HeroPartyMember> Heroes;

    [FieldOffset(0x0034)]
    public readonly GuildwarsArray<uint> Others;

    [FieldOffset(0x0080)]
    private readonly uint H0080;
}
