using Daybreak.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

[JsonConverter(typeof(AttributeJsonConverter))]
public sealed class Attribute
{
    public static readonly Attribute FastCasting = new() { Name = "Fast Casting", Id = 0, Profession = Profession.Mesmer };
    public static readonly Attribute IllusionMagic = new() { Name = "Illusion Magic", Id = 1, Profession = Profession.Mesmer };
    public static readonly Attribute DominationMagic = new() { Name = "Domination Magic", Id = 2, Profession = Profession.Mesmer };
    public static readonly Attribute InspirationMagic = new() { Name = "Inspiration Magic", Id = 3, Profession = Profession.Mesmer };
    public static readonly Attribute BloodMagic = new() { Name = "Blood Magic", Id = 4, Profession = Profession.Necromancer };
    public static readonly Attribute DeathMagic = new() { Name = "Death Magic", Id = 5, Profession = Profession.Necromancer };
    public static readonly Attribute SoulReaping = new() { Name = "Soul Reaping", Id = 6, Profession = Profession.Necromancer };
    public static readonly Attribute Curses = new() { Name = "Curses", Id = 7, Profession = Profession.Necromancer };
    public static readonly Attribute AirMagic = new() { Name = "Air Magic", Id = 8, Profession = Profession.Elementalist };
    public static readonly Attribute EarthMagic = new() { Name = "Earth Magic", Id = 9, Profession = Profession.Elementalist };
    public static readonly Attribute FireMagic = new() { Name = "Fire Magic", Id = 10, Profession = Profession.Elementalist };
    public static readonly Attribute WaterMagic = new() { Name = "Water Magic", Id = 11, Profession = Profession.Elementalist };
    public static readonly Attribute EnergyStorage = new() { Name = "Energy Storage", Id = 12, Profession = Profession.Elementalist };
    public static readonly Attribute HealingPrayers = new() { Name = "Healing Prayers", Id = 13, Profession = Profession.Monk };
    public static readonly Attribute SmitingPrayers = new() { Name = "Smiting Prayers", Id = 14, Profession = Profession.Monk };
    public static readonly Attribute ProtectionPrayers = new() { Name = "Protection Prayers", Id = 15, Profession = Profession.Monk };
    public static readonly Attribute DivineFavor = new() { Name = "Divine Favor", Id = 16, Profession = Profession.Monk };
    public static readonly Attribute Strength = new() { Name = "Strength", Id = 17, Profession = Profession.Warrior };
    public static readonly Attribute AxeMastery = new() { Name = "Axe Mastery", Id = 18, Profession = Profession.Warrior };
    public static readonly Attribute HammerMastery = new() { Name = "Hammer Mastery", Id = 19, Profession = Profession.Warrior };
    public static readonly Attribute Swordsmanship = new() { Name = "Swordsmanship", Id = 20, Profession = Profession.Warrior };
    public static readonly Attribute Tactics = new() { Name = "Tactics", Id = 21, Profession = Profession.Warrior };
    public static readonly Attribute BeastMastery = new() { Name = "Beast Mastery", Id = 22, Profession = Profession.Ranger };
    public static readonly Attribute Expertise = new() { Name = "Expertise", Id = 23, Profession = Profession.Ranger };
    public static readonly Attribute WildernessSurvival = new() { Name = "Wilderness Survival", Id = 24, Profession = Profession.Ranger };
    public static readonly Attribute Marksmanship = new() { Name = "Marksmanship", Id = 25, Profession = Profession.Ranger };
    public static readonly Attribute DaggerMastery = new() { Name = "Dagger Mastery", Id = 29, Profession = Profession.Assassin };
    public static readonly Attribute DeadlyArts = new() { Name = "Deadly Arts", Id = 30, Profession = Profession.Assassin };
    public static readonly Attribute ShadowArts = new() { Name = "Shadow Arts", Id = 31, Profession = Profession.Assassin };
    public static readonly Attribute Communing = new() { Name = "Communing", Id = 32, Profession = Profession.Ritualist };
    public static readonly Attribute RestorationMagic = new() { Name = "Restoration Magic", Id = 33, Profession = Profession.Ritualist };
    public static readonly Attribute ChannelingMagic = new() { Name = "Channeling Magic", Id = 34, Profession = Profession.Ritualist };
    public static readonly Attribute CriticalStrikes = new() { Name = "Critical Strikes", Id = 35, Profession = Profession.Assassin };
    public static readonly Attribute SpawningPower = new() { Name = "Spawning Power", Id = 36, Profession = Profession.Ritualist };
    public static readonly Attribute SpearMastery = new() { Name = "Spear Mastery", Id = 37, Profession = Profession.Paragon };
    public static readonly Attribute Command = new() { Name = "Command", Id = 38, Profession = Profession.Paragon };
    public static readonly Attribute Motivation = new() { Name = "Motivation", Id = 39, Profession = Profession.Paragon };
    public static readonly Attribute Leadership = new() { Name = "Leadership", Id = 40, Profession = Profession.Paragon };
    public static readonly Attribute ScytheMastery = new() { Name = "Scythe Mastery", Id = 41, Profession = Profession.Dervish };
    public static readonly Attribute WindPrayers = new() { Name = "Wind Prayers", Id = 42, Profession = Profession.Dervish };
    public static readonly Attribute EarthPrayers = new() { Name = "Earth Prayers", Id = 43, Profession = Profession.Dervish };
    public static readonly Attribute Mysticism = new() { Name = "Mysticism", Id = 44, Profession = Profession.Dervish };
    public static readonly IEnumerable<Attribute> Attributes = new List<Attribute>
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
    public override string ToString() => this.Name ?? string.Empty;
    private Attribute()
    {
    }
}
