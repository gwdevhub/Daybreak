using System;

namespace Daybreak.Models.Interop;

public readonly struct TitleContext
{
    public readonly uint Props;

    public readonly uint CurrentPoints;

    public readonly uint CurrentTitleTierIndex;

    public readonly uint PointsNeededForCurrentRank;

    public readonly uint NextTitleTierIndex;

    public readonly uint PointsNeededForNextRank;

    public readonly uint MaxTitleRank;

    public readonly uint MaxTitleTierIndex;

    public readonly int H0020;

    public readonly int H0024;

    public bool IsPercentage => (this.Props & 1U) != 0;

    public bool HasTiers => (this.Props & 3U) == 2;
}
