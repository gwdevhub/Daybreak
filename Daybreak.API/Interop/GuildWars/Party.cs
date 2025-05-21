using System.Extensions;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[Flags]
public enum PlayerPartyMemberState : uint
{
    None = 0,
    Connected = 1,
    Ticked = 2
}

public enum PartySearchType
{
    PartySearchType_Hunting = 0,
    PartySearchType_Mission = 1,
    PartySearchType_Quest = 2,
    PartySearchType_Trade = 3,
    PartySearchType_Guild = 4,
};

[Flags]
public enum PartyFlags : uint
{
    None = 0,
    HardMode = 0x10,  // Bit 4
    Defeated = 0x20,  // Bit 5
    PartyLeader = 0x80   // Bit 7 (0x7th position)
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0xC)]
public readonly unsafe struct PartyMoraleContext
{
    [FieldOffset(0x000C)]
    public readonly PartyMemberMoraleInfo* MoraleInfo;
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly struct PartyMemberMoraleInfo
{
    [FieldOffset(0x0000)]
    public readonly uint AgentId;
    [FieldOffset(0x0014)]
    public readonly uint Morale;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x58)]
public readonly unsafe struct PartyContext
{
    [FieldOffset(0x0014)]
    public readonly PartyFlags Flags;
    [FieldOffset(0x0040)]
    public readonly GuildWarsArray<WrappedPointer<PartyInfo>> Parties;
    [FieldOffset(0x0054)]
    public readonly PartyInfo* PlayerParty;
    [FieldOffset(0x00C0)]
    public readonly GuildWarsArray<WrappedPointer<PartySearch>> PartySearches;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC)]
public readonly struct PlayerPartyMember
{
    public readonly uint LogingNumber;
    public readonly uint CalledTargetId;
    public readonly PlayerPartyMemberState State;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x18)]
public readonly struct HeroPartyMember
{
    [FieldOffset(0x0000)]
    public readonly uint AgentId;
    [FieldOffset(0x0004)]
    public readonly uint OwnerPlayerId;
    [FieldOffset(0x0008)]
    public readonly uint HeroId;
    [FieldOffset(0x0014)]
    public readonly uint Level;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x34)]
public readonly struct HenchmanPartyMember
{
    [FieldOffset(0x0000)]
    public readonly uint AgentId;
    [FieldOffset(0x002C)]
    public readonly Profession Profession;
    [FieldOffset(0x0030)]
    public readonly uint Level;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x84)]
public readonly struct PartyInfo
{
    [FieldOffset(0x0000)]
    public readonly uint PartyId;
    [FieldOffset(0x0004)]
    public readonly GuildWarsArray<PlayerPartyMember> Players;
    [FieldOffset(0x0014)]
    public readonly GuildWarsArray<HenchmanPartyMember> Henchmen;
    [FieldOffset(0x0024)]
    public readonly GuildWarsArray<HeroPartyMember> Heroes;
    [FieldOffset(0x0034)]
    public readonly GuildWarsArray<uint> Others;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct PartySearch
{
    public readonly uint PartySearchId;
    public readonly PartySearchType PartySearchType;
    public readonly uint HardMode;
    public readonly uint District;
    public readonly uint Language;
    public readonly uint PartySize;
    public readonly uint HeroCount;
    public readonly Array32Char Message;
    public readonly Array20Char PartyLeader;
    public readonly Profession Primary;
    public readonly Profession Secondary;
    public readonly uint Level;
    public readonly uint Timestamp;
}
