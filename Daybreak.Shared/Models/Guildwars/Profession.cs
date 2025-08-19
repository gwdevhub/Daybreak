using Daybreak.Shared.Converters;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Daybreak.Shared.Models.Guildwars;

[JsonConverter(typeof(ProfessionJsonConverter))]
[TypeConverter(typeof(ProfessionTypeConverter))]
public sealed class Profession : IWikiEntity
{
    public static readonly Profession None = new() { Name = "None", Id = 0, Alias = "Any" };
    public static readonly Profession Warrior = new()
    { 
        Name = "Warrior",
        Id = 1,
        Alias = "W",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:W/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Warrior",
        PrimaryAttribute = Attribute.Strength,
        Attributes = [Attribute.AxeMastery, Attribute.HammerMastery, Attribute.Swordsmanship, Attribute.Tactics]
    };
    public static readonly Profession Ranger = new()
    {
        Name = "Ranger",
        Id = 2,
        Alias = "R",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:R/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Ranger",
        PrimaryAttribute = Attribute.Expertise,
        Attributes = [Attribute.BeastMastery, Attribute.Marksmanship, Attribute.WildernessSurvival]
    };
    public static readonly Profession Monk = new()
    {
        Name = "Monk",
        Id = 3,
        Alias = "Mo",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:Mo/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Monk",
        PrimaryAttribute = Attribute.DivineFavor,
        Attributes = [Attribute.HealingPrayers, Attribute.SmitingPrayers, Attribute.ProtectionPrayers]
    };
    public static readonly Profession Necromancer = new()
    { 
        Name = "Necromancer",
        Alias = "N",
        Id = 4,
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:N/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Necromancer",
        PrimaryAttribute = Attribute.SoulReaping,
        Attributes = [Attribute.Curses, Attribute.BloodMagic, Attribute.DeathMagic]
    };
    public static readonly Profession Mesmer = new() 
    { 
        Name = "Mesmer",
        Id = 5,
        Alias = "Me",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:Me/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Mesmer",
        PrimaryAttribute = Attribute.FastCasting, 
        Attributes = [Attribute.DominationMagic, Attribute.IllusionMagic, Attribute.InspirationMagic]
    };
    public static readonly Profession Elementalist = new()
    { 
        Name = "Elementalist",
        Id = 6,
        Alias = "E",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:E/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Elementalist",
        PrimaryAttribute = Attribute.EnergyStorage,
        Attributes = [Attribute.AirMagic, Attribute.EarthMagic, Attribute.FireMagic, Attribute.WaterMagic]
    };
    public static readonly Profession Assassin = new()
    { 
        Name = "Assassin",
        Id = 7,
        Alias = "A",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:A/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Assassin",
        PrimaryAttribute = Attribute.CriticalStrikes,
        Attributes = [Attribute.DaggerMastery, Attribute.DeadlyArts, Attribute.ShadowArts]
    };
    public static readonly Profession Ritualist = new()
    { 
        Name = "Ritualist",
        Id = 8,
        Alias = "Rt",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:Rt/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Ritualist",
        PrimaryAttribute = Attribute.SpawningPower,
        Attributes = [Attribute.ChannelingMagic, Attribute.Communing, Attribute.RestorationMagic]
    };
    public static readonly Profession Paragon = new()
    {
        Name = "Paragon",
        Id = 9,
        Alias = "P",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:P/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Paragon",
        PrimaryAttribute = Attribute.Leadership,
        Attributes = [Attribute.Command, Attribute.Motivation, Attribute.SpearMastery]
    };
    public static readonly Profession Dervish = new()
    { 
        Name = "Dervish",
        Id = 10,
        Alias = "D",
        BuildsUrl = "https://gwpvx.fandom.com/wiki/Special:PrefixIndex/Build:D/",
        WikiUrl = "https://wiki.guildwars.com/wiki/Dervish",
        PrimaryAttribute = Attribute.Mysticism,
        Attributes = [Attribute.EarthPrayers, Attribute.ScytheMastery, Attribute.WindPrayers]
    };
    public static readonly IEnumerable<Profession> Professions =
    [
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
    ];
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

    public string? WikiUrl { get; init; } = string.Empty;
    public string? BuildsUrl { get; init; }
    public string? Alias { get; init; }
    public string? Name { get; init; }
    public int Id { get; set; }
    public Attribute? PrimaryAttribute { get; private set; }
    public List<Attribute> Attributes { get; private set; } = [];
    private Profession()
    {
    }

    public override string ToString()
    {
        return this.Name!;
    }
}
