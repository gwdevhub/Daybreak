using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

public sealed class Attribute
{
    public static Attribute FastCasting { get; } = new() { Name = "Fast Casting", Id = 0, Profession = Profession.Mesmer };
    public static Attribute IllusionMagic { get; } = new() { Name = "Illusion Magic", Id = 1, Profession = Profession.Mesmer };
    public static Attribute DominationMagic { get; } = new() { Name = "Domination Magic", Id = 2, Profession = Profession.Mesmer };
    public static Attribute InspirationMagic { get; } = new() { Name = "Inspiration Magic", Id = 3, Profession = Profession.Mesmer };
    public static Attribute BloodMagic { get; } = new() { Name = "Blood Magic", Id = 4, Profession = Profession.Necromancer };
    public static Attribute DeathMagic { get; } = new() { Name = "Death Magic", Id = 5, Profession = Profession.Necromancer };
    public static Attribute SoulReaping { get; } = new() { Name = "Soul Reaping", Id = 6, Profession = Profession.Necromancer };
    public static Attribute Curses { get; } = new() { Name = "Curses", Id = 7, Profession = Profession.Necromancer };
    public static Attribute AirMagic { get; } = new() { Name = "Air Magic", Id = 8, Profession = Profession.Elementalist };
    public static Attribute EarthMagic { get; } = new() { Name = "Earth Magic", Id = 9, Profession = Profession.Elementalist };
    public static Attribute FireMagic { get; } = new() { Name = "Fire Magic", Id = 10, Profession = Profession.Elementalist };
    public static Attribute WaterMagic { get; } = new() { Name = "Water Magic", Id = 11, Profession = Profession.Elementalist };
    public static Attribute EnergyStorage { get; } = new() { Name = "Energy Storage", Id = 12, Profession = Profession.Elementalist };
    public static Attribute HealingPrayers { get; } = new() { Name = "Healing Prayers", Id = 13, Profession = Profession.Monk };
    public static Attribute SmitingPrayers { get; } = new() { Name = "Smiting Prayers", Id = 14, Profession = Profession.Monk };
    public static Attribute ProtectionPrayers { get; } = new() { Name = "Protection Prayers", Id = 15, Profession = Profession.Monk };
    public static Attribute DivineFavor { get; } = new() { Name = "Divine Favor", Id = 16, Profession = Profession.Monk };
    public static Attribute Strength { get; } = new() { Name = "Strength", Id = 17, Profession = Profession.Warrior };
    public static Attribute AxeMastery { get; } = new() { Name = "Axe Mastery", Id = 18, Profession = Profession.Warrior };
    public static Attribute HammerMastery { get; } = new() { Name = "Hammer Mastery", Id = 19, Profession = Profession.Warrior };
    public static Attribute Swordsmanship { get; } = new() { Name = "Swordsmanship", Id = 20, Profession = Profession.Warrior };
    public static Attribute Tactics { get; } = new() { Name = "Tactics", Id = 21, Profession = Profession.Warrior };
    public static Attribute BeastMastery { get; } = new() { Name = "Beast Mastery", Id = 22, Profession = Profession.Ranger };
    public static Attribute Expertise { get; } = new() { Name = "Expertise", Id = 23, Profession = Profession.Ranger };
    public static Attribute WildernessSurvival { get; } = new() { Name = "Wilderness Survival", Id = 24, Profession = Profession.Ranger };
    public static Attribute Marksmanship { get; } = new() { Name = "Marksmanship", Id = 25, Profession = Profession.Ranger };
    public static Attribute DaggerMastery { get; } = new() { Name = "Dagger Mastery", Id = 29, Profession = Profession.Assassin };
    public static Attribute DeadlyArts { get; } = new() { Name = "Deadly Arts", Id = 30, Profession = Profession.Assassin };
    public static Attribute ShadowArts { get; } = new() { Name = "Shadow Arts", Id = 31, Profession = Profession.Assassin };
    public static Attribute Communing { get; } = new() { Name = "Communing", Id = 32, Profession = Profession.Ritualist };
    public static Attribute RestorationMagic { get; } = new() { Name = "Restoration Magic", Id = 33, Profession = Profession.Ritualist };
    public static Attribute ChannelingMagic { get; } = new() { Name = "Channeling Magic", Id = 34, Profession = Profession.Ritualist };
    public static Attribute CriticalStrikes { get; } = new() { Name = "Critical Strikes", Id = 35, Profession = Profession.Assassin };
    public static Attribute SpawningPower { get; } = new() { Name = "Spawning Power", Id = 36, Profession = Profession.Ritualist };
    public static Attribute SpearMastery { get; } = new() { Name = "Spear Mastery", Id = 37, Profession = Profession.Paragon };
    public static Attribute Command { get; } = new() { Name = "Command", Id = 38, Profession = Profession.Paragon };
    public static Attribute Motivation { get; } = new() { Name = "Motivation", Id = 39, Profession = Profession.Paragon };
    public static Attribute Leadership { get; } = new() { Name = "Leadership", Id = 40, Profession = Profession.Paragon };
    public static Attribute ScytheMastery { get; } = new() { Name = "Scythe Mastery", Id = 41, Profession = Profession.Dervish };
    public static Attribute WindPrayers { get; } = new() { Name = "Wind Prayers", Id = 42, Profession = Profession.Dervish };
    public static Attribute EarthPrayers { get; } = new() { Name = "Earth Prayers", Id = 43, Profession = Profession.Dervish };
    public static Attribute Mysticism { get; } = new() { Name = "Mysticism", Id = 44, Profession = Profession.Dervish };
    public static IEnumerable<Attribute> Attributes { get; } = new List<Attribute>
    {
        FastCasting,
        IllusionMagic,
        DominationMagic,
        InspirationMagic,
        BloodMagic,
        DeathMagic,
        SoulReaping,
        Curses,
        AirMagic,
        EarthMagic,
        FireMagic,
        WaterMagic,
        EnergyStorage,
        HealingPrayers,
        SmitingPrayers,
        ProtectionPrayers,
        DivineFavor,
        Strength,
        AxeMastery,
        HammerMastery,
        Swordsmanship,
        Tactics,
        BeastMastery,
        Expertise,
        WildernessSurvival,
        Marksmanship,
        DaggerMastery,
        DeadlyArts,
        ShadowArts,
        Communing,
        RestorationMagic,
        ChannelingMagic,
        CriticalStrikes,
        SpawningPower,
        SpearMastery,
        Command,
        Motivation,
        Leadership,
        ScytheMastery,
        EarthPrayers,
        WindPrayers,
        EarthMagic,
        Mysticism
    };

    public static bool TryParse(int id, out Attribute attribute)
    {
        attribute = Attributes.Where(attr => attr.Id == id).FirstOrDefault()!;
        if (attribute is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Attribute attribute)
    {
        attribute = Attributes.Where(attr => attr.Name == name).FirstOrDefault()!;
        if (attribute is null)
        {
            return false;
        }

        return true;
    }
    public static Attribute Parse(int id)
    {
        if (TryParse(id, out var attribute) is false)
        {
            throw new InvalidOperationException($"Could not find an attribute with id {id}");
        }

        return attribute;
    }
    public static Attribute Parse(string name)
    {
        if (TryParse(name, out var attribute) is false)
        {
            throw new InvalidOperationException($"Could not find an attribute with name {name}");
        }

        return attribute;
    }

    public int Id { get; private set; }
    public string? Name { get; private set; }
    public Profession? Profession { get; private set; }
    private Attribute()
    {
    }
}
