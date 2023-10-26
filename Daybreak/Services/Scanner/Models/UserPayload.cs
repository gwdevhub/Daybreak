namespace Daybreak.Services.Scanner.Models;

internal sealed class UserPayload
{
    public string? Email { get; set; }
    public uint CurrentKurzickPoints { get; set; }
    public uint CurrentLuxonPoints { get; set; }
    public uint CurrentImperialPoints { get; set; }
    public uint CurrentBalthazarPoints { get; set; }
    public uint CurrentSkillPoints { get; set; }
    public uint TotalKurzickPoints { get; set; }
    public uint TotalLuxonPoints { get; set; }
    public uint TotalImperialPoints { get; set; }
    public uint TotalBalthazarPoints { get; set; }
    public uint TotalSkillPoints { get; set; }
    public uint MaxKurzickPoints { get; set; }
    public uint MaxLuxonPoints { get; set; }
    public uint MaxImperialPoints { get; set; }
    public uint MaxBalthazarPoints { get; set; }
}
