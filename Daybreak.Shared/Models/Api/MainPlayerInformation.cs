namespace Daybreak.Shared.Models.Api;

public sealed record MainPlayerInformation(
    string Uuid,
    string Email,
    string CharacterName,
    string AccountName,
    uint Wins,
    uint Losses,
    uint Rating, 
    uint QualifierPoints,
    uint Rank,
    uint TournamentRewardPoints)
{
}
