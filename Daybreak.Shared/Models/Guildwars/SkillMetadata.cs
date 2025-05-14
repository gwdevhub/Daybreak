namespace Daybreak.Shared.Models.Guildwars;

public sealed class SkillMetadata
{
    public Skill? Skill { get; init; }
    public uint Adrenaline1 { get; init; }
    public uint Adrenaline2 { get; init; }
    public uint Recharge { get; init; }
    public uint Id { get; init; }
    public uint Event { get; init; }
}
