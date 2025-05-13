namespace Daybreak.Models.Guildwars;

public sealed class SessionInformation
{
    public uint FoesKilled { get; init; }
    public uint FoesToKill { get; init; }
    public Map? CurrentMap { get; init; }
    public uint InstanceTimer { get; init; }
    public InstanceType InstanceType { get; init; }
}
