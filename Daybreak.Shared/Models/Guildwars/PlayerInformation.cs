using System.Collections.Generic;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class PlayerInformation
{
    public int Id { get; init; }
    public uint Timer { get; init; }
    public int Level { get; init; }
    public Profession? PrimaryProfession { get; init; }
    public Profession? SecondaryProfession { get; init; }
    public List<Profession>? UnlockedProfession { get; init; }
    public Build? CurrentBuild { get; init; }
    public Npc? NpcDefinition { get; init; }
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; init; }
    public float CurrentEnergy { get; set; }
    public float MaxEnergy { get; init; }
    public float HealthRegen { get; init; }
    public float EnergyRegen { get; init; }
    public float RotationAngle { get; set; }

    // These two currently don't work with the memory scanner
    public uint ModelType { get; init; }
    public Position? Position { get; set; }
}
