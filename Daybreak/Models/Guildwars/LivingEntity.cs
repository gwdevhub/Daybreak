namespace Daybreak.Models.Guildwars;

public sealed class LivingEntity : IEntity
{
    public int Id { get; init; }

    public uint Timer { get; init; }

    public uint? ModelType { get; init; }

    public Npc? NpcDefinition { get; init; }

    public Position? Position { get; init; }

    public Profession? PrimaryProfession { get; init; }
    
    public Profession? SecondaryProfession { get; init; }

    public int Level { get; init; }

    public LivingEntityState State { get; init; }
    
    public LivingEntityAllegiance Allegiance { get; init; }
}
