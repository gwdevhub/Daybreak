using System.Extensions;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly unsafe struct GuildContext
{
    [FieldOffset(0x0034)]
    public readonly Array20Char PlayerName;

    [FieldOffset(0x0060)]
    public readonly uint PlayerBuildIndex;

    [FieldOffset(0x0078)]
    public readonly Array256Char Announcement;

    [FieldOffset(0x0287)]
    public readonly Array20Char AnnouncementAuthor;

    [FieldOffset(0x02A0)]
    public readonly uint PlayerGuildRank;

    [FieldOffset(0x02A8)]
    public readonly GuildWarsArray<TownAlliance> FactionOutpostGuilds;

    [FieldOffset(0x02B8)]
    public readonly uint KurzickTownCount;

    [FieldOffset(0x02BC)]
    public readonly uint LuxonTownCount;
    
    [FieldOffset(0x02F8)]
    public readonly GuildWarsArray<WrappedPointer<Guild>> Guilds;

    [FieldOffset(0x0358)]
    public readonly GuildWarsArray<WrappedPointer<GuildPlayer>> PlayerRoster;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct TownAlliance
{
    public readonly uint Rank;
    public readonly uint Allegiance;
    public readonly uint Faction;
    public readonly Array32Char Name;
    public readonly Array5Char Tag;
    public readonly CapeDesign Cape;
    public readonly uint MapId;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CapeDesign
{
    public readonly uint CapeBgColor;
    public readonly uint CapeDetailColor;
    public readonly uint CapeEmblemColor;
    public readonly uint CapeShape;
    public readonly uint CapeDetail;
    public readonly uint CapeEmblem;
    public readonly uint CapeTrim;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x174)]
public readonly unsafe struct GuildPlayer
{
    [FieldOffset(0x0004)]
    public readonly char* NamePtr;
    [FieldOffset(0x0008)]
    public readonly Array20Char InvitedName;
    [FieldOffset(0x0030)]
    public readonly Array20Char CurrentName;
    [FieldOffset(0x0050)]
    public readonly Array20Char InviterName;
    [FieldOffset(0x0080)]
    public readonly uint InviteTime;
    [FieldOffset(0x0084)]
    public readonly Array20Char PromoterName;
    [FieldOffset(0x00DC)]
    public readonly uint Offline;
    [FieldOffset(0x00E0)]
    public readonly uint MemberType;
    [FieldOffset(0x00E4)]
    public readonly uint Status;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0xAC)]
public readonly struct Guild
{
    [FieldOffset(0x0024)]
    public readonly uint Index;
    [FieldOffset(0x0028)]
    public readonly uint Rank;
    [FieldOffset(0x002C)]
    public readonly uint Features;
    [FieldOffset(0x0030)]
    public readonly Array32Char Name;
    [FieldOffset(0x0070)]
    public readonly uint Rating;
    [FieldOffset(0x0074)]
    public readonly uint Faction;
    [FieldOffset(0x0078)]
    public readonly uint FactionPoints;
    [FieldOffset(0x007C)]
    public readonly uint QualifierPoints;
    [FieldOffset(0x0080)]
    public readonly Array8Char Tag;
    [FieldOffset(0x0090)]
    public readonly CapeDesign Cape;
}
