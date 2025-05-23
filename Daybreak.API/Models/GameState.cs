namespace Daybreak.API.Models;

public sealed class GameState
{
    public required uint? CurrentMap { get; init; }
    public required string Email { get; init; }
    public required string CharacterName { get; init; }
    public required uint CurrentExperience { get; init; }
    public required uint Level { get; init; }
    public required uint CurrentLuxon { get; init; }
    public required uint CurrentKurzick { get; init; }
    public required uint CurrentImperial { get; init; }
    public required uint CurrentBalthazar { get; init; }
    public required uint MaxLuxon { get; init; }
    public required uint MaxKurzick { get; init; }
    public required uint MaxImperial { get; init; }
    public required uint MaxBalthazar { get; init; }
    public required uint TotalLuxon { get; init; }
    public required uint TotalKurzick { get; init; }
    public required uint TotalImperial { get; init; }
    public required uint TotalBalthazar { get; init; }
}
