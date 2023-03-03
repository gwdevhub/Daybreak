using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

public sealed class Profession
{
    public static Profession None { get; } = new() { Name = "None", Id = 0 };
    public static Profession Warrior { get; } = new() { Name = "Warrior", Id = 1, PrimaryAttribute = Attribute.Strength, Attributes = new List<Attribute> { Attribute.AxeMastery, Attribute.HammerMastery, Attribute.Swordsmanship, Attribute.Tactics } };
    public static Profession Ranger { get; } = new() { Name = "Ranger", Id = 2, PrimaryAttribute = Attribute.Expertise, Attributes = new List<Attribute> { Attribute.BeastMastery, Attribute.Marksmanship, Attribute.WildernessSurvival } };
    public static Profession Monk { get; } = new() { Name = "Monk", Id = 3, PrimaryAttribute = Attribute.DivineFavor, Attributes = new List<Attribute> { Attribute.HealingPrayers, Attribute.SmitingPrayers, Attribute.ProtectionPrayers } };
    public static Profession Necromancer { get; } = new() { Name = "Necromancer", Id = 4, PrimaryAttribute = Attribute.SoulReaping, Attributes = new List<Attribute> { Attribute.Curses, Attribute.BloodMagic, Attribute.DeathMagic } };
    public static Profession Mesmer { get; } = new() { Name = "Mesmer", Id = 5, PrimaryAttribute = Attribute.FastCasting, Attributes = new List<Attribute> { Attribute.DominationMagic, Attribute.IllusionMagic, Attribute.InspirationMagic } };
    public static Profession Elementalist { get; } = new() { Name = "Elementalist", Id = 6, PrimaryAttribute = Attribute.EnergyStorage, Attributes = new List<Attribute> { Attribute.AirMagic, Attribute.EarthMagic, Attribute.FireMagic, Attribute.WaterMagic } };
    public static Profession Assassin { get; } = new() { Name = "Assassin", Id = 7, PrimaryAttribute = Attribute.CriticalStrikes, Attributes = new List<Attribute> { Attribute.DaggerMastery, Attribute.DeadlyArts, Attribute.ShadowArts } };
    public static Profession Ritualist { get; } = new() { Name = "Ritualist", Id = 8, PrimaryAttribute = Attribute.SpawningPower, Attributes = new List<Attribute> { Attribute.ChannelingMagic, Attribute.Communing, Attribute.RestorationMagic } };
    public static Profession Paragon { get; } = new() { Name = "Paragon", Id = 9, PrimaryAttribute = Attribute.Leadership, Attributes = new List<Attribute> { Attribute.Command, Attribute.Motivation, Attribute.SpearMastery } };
    public static Profession Dervish { get; } = new() { Name = "Dervish", Id = 10, PrimaryAttribute = Attribute.Mysticism, Attributes = new List<Attribute> { Attribute.EarthPrayers, Attribute.ScytheMastery, Attribute.WindPrayers } };
    public static IEnumerable<Profession> Professions { get; } = new List<Profession>
    {
        None,
        Warrior,
        Ranger,
        Monk,
        Necromancer,
        Mesmer,
        Elementalist,
        Assassin,
        Ritualist,
        Paragon,
        Dervish
    };
    public static bool TryParse(int id, out Profession profession)
    {
        profession = Professions.Where(prof => prof.Id == id).FirstOrDefault()!;
        if (profession is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Profession profession)
    {
        profession = Professions.Where(prof => prof.Name == name).FirstOrDefault()!;
        if (profession is null)
        {
            return false;
        }

        return true;
    }
    public static Profession Parse(int id)
    {
        if (TryParse(id, out var profession) is false)
        {
            throw new InvalidOperationException($"Could not find a profession with id {id}");
        }

        return profession;
    }
    public static Profession Parse(string name)
    {
        if (TryParse(name, out var profession) is false)
        {
            throw new InvalidOperationException($"Could not find a profession with name {name}");
        }

        return profession;
    }

    public string? Name { get; private set; }
    public int Id { get; set; }
    public Attribute? PrimaryAttribute { get; private set; }
    public List<Attribute> Attributes { get; private set; } = new List<Attribute>();
    private Profession()
    {
    }
}
