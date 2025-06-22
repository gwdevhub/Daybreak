using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Models.FocusView;
public sealed class TitleInformationComponentContext
{
    public required Title Title { get; init; }
    public required bool IsPercentage { get; init; }
    public required uint CurrentPoints { get; init; }
    public required uint PointsForCurrentRank { get; init; }
    public required uint PointsForNextRank { get; init; }
    public required uint TierNumber { get; init; }
    public required uint MaxTierNumber { get; init; }
}
