using MemoryPack;

namespace Daybreak.Shared.Models.Api;

/// <summary>
/// State of the main player data in the game.
/// This will be serialized to a <see cref="System.ReadOnlySpan{System.Byte}"/> when being sent over the WebSocket.
/// </summary>
[MemoryPackable]
public partial record MainPlayerState(
    uint CurrentExperience,
    uint Level,
    uint CurrentLuxon,
    uint CurrentKurzick,
    uint CurrentImperial,
    uint CurrentBalthazar,
    uint MaxLuxon,
    uint MaxKurzick,
    uint MaxImperial,
    uint MaxBalthazar,
    uint TotalLuxon,
    uint TotalKurzick,
    uint TotalImperial,
    uint TotalBalthazar,
    float CurrentHp,
    uint MaxHp,
    float CurrentEnergy,
    uint MaxEnergy,
    uint PrimaryProfession,
    uint SecondaryProfession,
    float PosX,
    float PosY)
{
}
