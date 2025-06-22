namespace Daybreak.Shared.Models.Api;
public sealed record TitleInfo(uint Id, bool IsPercentage, uint CurrentPoints, uint PointsForCurrentRank, uint PointsForNextRank, uint TierNumber, uint MaxTierNumber)
{
}
