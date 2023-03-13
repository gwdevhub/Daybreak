namespace Daybreak.Models.Guildwars;

public readonly struct LivingEntity
{
    public int Id { get; init; }

    public Position? Position { get; init; }

    public Profession? PrimaryProfession { get; init; }
    
    public Profession? SecondaryProfession { get; init; }

    public int Level { get; init; }

    public LivingEntityState State { get; init; }
    
    public LivingEntityAllegiance Allegiance { get; init; }
}
