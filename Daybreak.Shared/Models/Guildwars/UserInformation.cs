namespace Daybreak.Shared.Models.Guildwars;

public sealed class UserInformation
{
    public string? Email { get; init; }
    public uint CurrentKurzickPoints { get; init; }
    public uint TotalKurzickPoints { get; init; }
    public uint CurrentLuxonPoints { get; init; }
    public uint TotalLuxonPoints { get; init; }
    public uint CurrentImperialPoints { get; init; }
    public uint TotalImperialPoints { get; init; }
    public uint CurrentBalthazarPoints { get; init; }
    public uint TotalBalthazarPoints { get; init; }
    public uint CurrentSkillPoints { get; init; }
    public uint TotalSkillPoints { get; init; }
    public uint MaxKurzickPoints { get; init; }
    public uint MaxLuxonPoints { get; init; }
    public uint MaxImperialPoints { get; init; }
    public uint MaxBalthazarPoints { get; init; }
}
