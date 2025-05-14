using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct PlayerContext
{
    [FieldOffset(0x0000)]
    public readonly int AgentId;

    [FieldOffset(0x0014)]
    public readonly uint PartyFlags;

    [FieldOffset(0x0018)]
    public readonly uint PrimaryProfession;

    [FieldOffset(0x001C)]
    public readonly uint SecondaryProfession;

    [FieldOffset(0x0028)]
    public readonly GuildwarsPointer<string> NamePointer;

    [FieldOffset(0x002C)]
    public readonly uint PartyLeaderPlayerNumber;

    [FieldOffset(0x0030)]
    public readonly uint ActiveTitleTier;

    [FieldOffset(0x0034)]
    public readonly uint PlayerNumber;

    [FieldOffset(0x0038)]
    public readonly uint PartySize;

    [FieldOffset(0x003C)]
    //Ignore this field. Added so that the struct will have the proper size and be marshaled properly into the array.
    private readonly GuildwarsArray<uint> HH3C;

    public bool Pvp => (this.PartyFlags & 0x800) != 0;
}
