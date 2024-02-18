namespace Daybreak.Services.Scanner.Models;
public sealed class TitleInfoPayload
{
    public uint CurrentPoints { get; set; }
    public uint PointsNeededNextRank { get; set; }
    public uint TitleId { get; set; }
    public uint TitleTierId { get; set; }
    public uint CurrentTier { get; set; }
    public string? TitleName { get; set; }
    public bool IsPercentageBased { get; set; }
}
