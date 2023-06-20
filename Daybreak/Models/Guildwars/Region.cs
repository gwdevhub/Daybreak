using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

public sealed class Region : IWikiEntity
{
    public static Region Kryta { get; } = new Region { Id = 0, Name = "Kryta", WikiUrl = "https://wiki.guildwars.com/wiki/Kryta" };
    public static Region MaguumaJungle { get; } = new Region { Id = 1, Name = "Maguuma Jungle", WikiUrl = "https://wiki.guildwars.com/wiki/Maguuma_Jungle" };
    public static Region Ascalon { get; } = new Region { Id = 2, Name = "Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon" };
    public static Region NorthernShiverpeaks { get; } = new Region { Id = 3, Name = "Northern Shiverpeaks", WikiUrl = "https://wiki.guildwars.com/wiki/Northern_Shiverpeaks" };
    public static Region HeroesAscent { get; } = new Region { Id = 4, Name = "Heroes' Ascent", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes%27_Ascent" };
    public static Region CrystalDesert { get; } = new Region { Id = 5, Name = "Crystal Desert", WikiUrl = "https://wiki.guildwars.com/wiki/Crystal_Desert" };
    public static Region FissureOfWoe { get; } = new Region { Id = 6, Name = "Fissure Of Woe", WikiUrl = "https://wiki.guildwars.com/wiki/The_Fissure_of_Woe" };
    public static Region PresearingAscalon { get; } = new Region { Id = 7, Name = "Pre Searing Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_(pre-Searing)" };
    public static Region KainengCity { get; } = new Region { Id = 8, Name = "Kaineng City", WikiUrl = "https://wiki.guildwars.com/wiki/Kaineng_City" };
    public static Region EchovaldForest { get; } = new Region { Id = 9, Name = "Echovald Forest", WikiUrl = "https://wiki.guildwars.com/wiki/Echovald_Forest" };
    public static Region TheJadeSea { get; } = new Region { Id = 10, Name = "The Jade Sea", WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Sea" };
    public static Region ShingJeaIsland { get; } = new Region { Id = 11, Name = "Shing Jea Island", WikiUrl = "https://wiki.guildwars.com/wiki/Shing_Jea_Island" };
    public static Region Kourna { get; } = new Region { Id = 12, Name = "Kourna", WikiUrl = "https://wiki.guildwars.com/wiki/Kourna" };
    public static Region Vabbi { get; } = new Region { Id = 13, Name = "Vabbi", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi" };
    public static Region TheDesolation { get; } = new Region { Id = 14, Name = "TheDesolation", WikiUrl = "https://wiki.guildwars.com/wiki/The_Desolation" };
    public static Region Istan { get; } = new Region { Id = 15, Name = "Istan", WikiUrl = "https://wiki.guildwars.com/wiki/Istan" };
    public static Region DomainOfAnguish { get; } = new Region { Id = 16, Name = "Domain Of Anguish", WikiUrl = "https://wiki.guildwars.com/wiki/Domain_of_Anguish" };
    public static Region TarnishedCoast { get; } = new Region { Id = 17, Name = "Tarnished Coast", WikiUrl = "https://wiki.guildwars.com/wiki/Tarnished_Coast" };
    public static Region DepthsOfTyria { get; } = new Region { Id = 18, Name = "Depths Of Tyria", WikiUrl = "https://wiki.guildwars.com/wiki/Depths_of_Tyria" };
    public static Region FarShiverpeaks { get; } = new Region { Id = 19, Name = "Far Shiverpeaks", WikiUrl = "https://wiki.guildwars.com/wiki/Far_Shiverpeaks" };
    public static Region CharrHomelands { get; } = new Region { Id = 20, Name = "Charr Homelands", WikiUrl = "https://wiki.guildwars.com/wiki/Charr_Homelands" };
    public static Region TheBattleIsles { get; } = new Region { Id = 21, Name = "The Battle Isles", WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_Isles" };
    public static Region TheBattleOfJahai { get; } = new Region { Id = 22, Name = "The Battle Of Jahai", WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_of_Jahai" };
    public static Region TheFlightNorth { get; } = new Region { Id = 23, Name = "The Flight North", WikiUrl = "https://wiki.guildwars.com/wiki/The_Flight_North" };
    public static Region TheTenguAccords { get; } = new Region { Id = 24, Name = "The Tengu Accords", WikiUrl = "https://wiki.guildwars.com/wiki/The_Tengu_Accords" };
    public static Region TheRiseOfTheWhiteMantle { get; } = new Region { Id = 25, Name = "The Rise Of The White Mantle", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rise_of_the_White_Mantle" };
    public static Region Swat { get; } = new Region { Id = 26 };
    public static Region DevRegion { get; } = new Region { Id = 27 };

    public static List<Region> Regions { get; } = new()
    {
        Kryta,
        MaguumaJungle,
        Ascalon,
        NorthernShiverpeaks,
        HeroesAscent,
        CrystalDesert,
        FissureOfWoe,
        PresearingAscalon,
        KainengCity,
        EchovaldForest,
        TheJadeSea,
        Kourna,
        Vabbi,
        TheDesolation,
        Istan,
        DomainOfAnguish,
        TarnishedCoast,
        DepthsOfTyria,
        FarShiverpeaks,
        CharrHomelands,
        TheBattleIsles,
        TheBattleOfJahai,
        TheFlightNorth,
        TheTenguAccords,
        TheRiseOfTheWhiteMantle,
        Swat,
        DevRegion
    };

    public static bool TryParse(int id, out Region region)
    {
        region = Regions.Where(region => region.Id == id).FirstOrDefault()!;
        if (region is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Region region)
    {
        region = Regions.Where(region => region.Name == name).FirstOrDefault()!;
        if (region is null)
        {
            return false;
        }

        return true;
    }
    public static Region Parse(int id)
    {
        if (TryParse(id, out var region) is false)
        {
            throw new InvalidOperationException($"Could not find a region with id {id}");
        }

        return region;
    }
    public static Region Parse(string name)
    {
        if (TryParse(name, out var region) is false)
        {
            throw new InvalidOperationException($"Could not find a region with name {name}");
        }

        return region;
    }

    private Region()
    {
    }
    
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? WikiUrl { get; init; }
}
