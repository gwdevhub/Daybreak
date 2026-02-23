using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[Flags]
public enum TitleProps : uint
{
    None = 0,
    PercentageBased = 1,
    HasTiers = 2
    // Note: The HasTiers check uses (props & 3) == 2, which means bit 1 is set but bit 0 is not
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x2C)]
[GWCAEquivalent("Title")]
public readonly struct TitleContext
{
    [FieldOffset(0x0000)]
    public readonly TitleProps Props;
    [FieldOffset(0x0004)]
    public readonly uint CurrentPoints;
    [FieldOffset(0x0008)]
    public readonly uint CurrentTitleTierIndex;
    [FieldOffset(0x000C)]
    public readonly uint PointsNeededForCurrentRank;
    [FieldOffset(0x0014)]
    public readonly uint NextTitleTierIndex;
    [FieldOffset(0x0018)]
    public readonly uint PointsNeededForNextRank;
    [FieldOffset(0x001C)]
    public readonly uint MaxTitleRank;
    [FieldOffset(0x0020)]
    public readonly uint MaxTitleTierIndex;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC)]
[GWCAEquivalent("TitleTier")]
public readonly unsafe struct TitleTier
{
    public readonly TitleProps Props;
    public readonly uint TierNumber;
    public readonly char* TierNameEncoded;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("TitleClientData")]
public readonly struct TitleClientData
{
    public readonly uint TitleId;
    public readonly uint NameId;
}
