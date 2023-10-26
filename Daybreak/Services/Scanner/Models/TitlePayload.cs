namespace Daybreak.Services.Scanner.Models;

internal class TitlePayload
{
    public uint CurrentPoints { get; set; }
    public uint Id { get; set; }
    public bool IsPercentage { get; set; }
    public uint MaxTierNumber { get; set; }
    public uint PointsForCurrentRank { get; set; }
    public uint PointsForNextRank { get; set; }
    public uint TierNumber { get; set; }
}
