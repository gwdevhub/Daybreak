using MemoryPack;

namespace Daybreak.API.Models;

/// <summary>
/// State of the main player data in the game.
/// This will be serialized to a <see cref="ReadOnlySpan{Byte}"/> when being sent over the WebSocket.
/// </summary>
[MemoryPackable]
public partial class MainPlayerState
{
    public const int Size = 0x38;

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
