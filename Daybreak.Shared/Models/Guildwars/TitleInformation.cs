namespace Daybreak.Models.Guildwars;

public sealed class TitleInformation
{
    public bool IsValid => this.CurrentPoints != 0 && this.PointsForCurrentRank != 0 && this.PointsForNextRank != 0 && this.TierNumber != 0 && this.MaxTierNumber != 0;

    public bool? IsPercentage { get; init; }

    public uint? CurrentPoints { get; init; }

    public uint? PointsForCurrentRank { get; init; }

    public uint? PointsForNextRank { get; init; }
    
    public uint? TierNumber { get; init; }

    public uint? MaxTierNumber { get; init; }

    public Title? Title { get; init; }
}
