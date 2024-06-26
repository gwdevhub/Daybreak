﻿namespace Daybreak.Models.Guildwars;

public sealed class LivingEntity : IEntity
{
    public int Id { get; init; }

    public uint Timer { get; init; }

    public uint? ModelType { get; init; }

    public Npc? NpcDefinition { get; init; }

    public Position? Position { get; set; }

    public Profession? PrimaryProfession { get; init; }
    
    public Profession? SecondaryProfession { get; init; }

    public int Level { get; init; }

    public LivingEntityState State { get; set; }
    
    public LivingEntityAllegiance Allegiance { get; init; }

    public float Health { get; set; }

    public float Energy { get; set; }

    public float RotationAngle { get; set; }
}
