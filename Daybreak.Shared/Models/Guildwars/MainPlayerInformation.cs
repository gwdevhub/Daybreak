﻿using System.Collections.Generic;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class MainPlayerInformation
{
    public string? Name { get; init; }
    public uint Timer { get; init; }
    public TitleInformation? TitleInformation { get; init; }
    public int Id { get; init; }
    public int Level { get; init; }
    public Position? Position { get; set; }
    public Profession? PrimaryProfession { get; init; }
    public Profession? SecondaryProfession { get; init; }
    public List<Profession>? UnlockedProfession { get; init; }
    public Build? CurrentBuild { get; init; }
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; init; }
    public float CurrentEnergy { get; set; }
    public float MaxEnergy { get; init; }
    public float HealthRegen { get; init; }
    public float EnergyRegen { get; init; }
    public float RotationAngle { get; set; }
    public bool HardModeUnlocked { get; init; }
    public uint Experience { get; init; }
    public uint Morale { get; init; }
    public Quest? Quest { get; init; }
    public List<QuestMetadata>? QuestLog { get; init; }
}
