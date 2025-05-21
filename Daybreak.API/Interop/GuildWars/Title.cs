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

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 40)]
public readonly struct TitleContext
{
    public readonly TitleProps Props;
    public readonly uint CurrentPoints;
    public readonly uint CurrentTitleTierIndex;
    public readonly uint PointsNeededForCurrentRank;
    public readonly uint NextTitleTierIndex;
    public readonly uint PointsNeededForNextRank;
    public readonly uint MaxTitleRank;
    public readonly uint MaxTitleTierIndex;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC)]
public readonly unsafe struct TitleTier
{
    public readonly TitleProps Props;
    public readonly uint TierNumber;
    public readonly char* TierNameEncoded;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct TitleClientData
{
    public readonly uint TitleId;
    public readonly uint NameId;
}
