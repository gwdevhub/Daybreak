using Daybreak.Models.Guildwars;

namespace Daybreak.Models;

public sealed class GameData
{
    public string? Email { get; init; }
    public string? CharacterName { get; init; }
    public Quest? Quest { get; init; }
    public bool HardModeUnlocked { get; init; }
    public uint Experience { get; init; }
    public uint CurrentKurzickPoints { get; init; }
    public uint TotalKurzickPoints { get; init; }
    public uint CurrentLuxonPoints { get; init; }
    public uint TotalLuxonPoints { get; init; }
    public uint CurrentImperialPoints { get; init; }
    public uint TotalImperialPoints { get; init; }
    public uint Level { get; init; }
    public uint Morale { get; init; }
    public uint CurrentBalthazarPoints { get; init; }
    public uint TotalBalthazarPoints { get; init; }
    public uint CurrentSkillPoints { get; init; }
    public uint TotalSkillPoints { get; init; }
    public uint MaxKurzickPoints { get; init; }
    public uint MaxLuxonPoints { get; init; }
    public uint MaxImperialPoints { get; init; }
    public uint MaxBalthazarPoints { get; init; }
    public uint FoesKilled { get; init; }
    public uint FoesToKill { get; init; }
}
