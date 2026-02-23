using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[Flags]
public enum PlayerContextFlags : uint
{
    None = 0,
    PvP = 0x800
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x50)]
[GWCAEquivalent("PlayerContext")]
public readonly unsafe struct PlayerContext
{
    [FieldOffset(0x0000)]
    public readonly int AgentId;

    [FieldOffset(0x0014)]
    public readonly PlayerContextFlags Flags;

    [FieldOffset(0x0018)]
    public readonly uint PrimaryProfession;

    [FieldOffset(0x001C)]
    public readonly uint SecondaryProfession;

    [FieldOffset(0x0024)]
    public readonly char* NameEncoded;

    [FieldOffset(0x0028)]
    public readonly char* Name;

    [FieldOffset(0x002C)]
    public readonly uint PartyLeaderPlayerNumber;

    [FieldOffset(0x0030)]
    public readonly uint ActiveTitleTier;

    [FieldOffset(0x0034)]
    public readonly uint ReforgedOrDhuumsFlags;

    [FieldOffset(0x0038)]
    public readonly uint PlayerNumber;

    [FieldOffset(0x003C)]
    public readonly uint PartySize;
}
