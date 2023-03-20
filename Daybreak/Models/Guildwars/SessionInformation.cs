namespace Daybreak.Models.Guildwars;

public readonly struct SessionInformation
{
    public uint FoesKilled { get; init; }
    public uint FoesToKill { get; init; }
    public Map? CurrentMap { get; init; }
    public uint InstanceTimer { get; init; }
}
