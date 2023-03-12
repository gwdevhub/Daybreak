﻿using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public class PlayerInformation
{
    public int Id { get; init; }
    public int Level { get; init; }
    public Position? Position { get; init; }
    public Profession? PrimaryProfession { get; init; }
    public Profession? SecondaryProfession { get; init; }
    public List<Profession>? UnlockedProfession { get; init; }
    public Build? CurrentBuild { get; init; }
    public float CurrentHealth { get; init; }
    public float MaxHealth { get; init; }
    public float CurrentEnergy { get; init; }
    public float MaxEnergy { get; init; }
    public float HealthRegen { get; init; }
    public float EnergyRegen { get; init; }
}
