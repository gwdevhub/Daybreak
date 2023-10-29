using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class PlayerInformation : IEntity
{
    public int Id { get; init; }
    public uint Timer { get; init; }
    public int Level { get; init; }
    public Position? Position { get; set; }
    public Profession? PrimaryProfession { get; init; }
    public Profession? SecondaryProfession { get; init; }
    public List<Profession>? UnlockedProfession { get; init; }
    public Build? CurrentBuild { get; init; }
    public Npc? NpcDefinition { get; init; }
    public uint ModelType { get; init; }
    public float CurrentHealth { get; init; }
    public float MaxHealth { get; init; }
    public float CurrentEnergy { get; init; }
    public float MaxEnergy { get; init; }
    public float HealthRegen { get; init; }
    public float EnergyRegen { get; init; }
}
