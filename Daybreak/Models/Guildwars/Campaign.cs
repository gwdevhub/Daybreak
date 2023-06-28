using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

public sealed class Campaign
{
    public static Campaign Core { get; } = new()
    {
        Id = 0,
        Name = "Core",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Core",
        Continents = new List<Continent>
        {
            Continent.TheBattleIsles,
            Continent.TheMists
        }
    };

    public static Campaign Prophecies { get; } = new()
    {
        Id = 1,
        Name = "Prophecies",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Prophecies",
        Continents = new List<Continent>
        {
            Continent.Tyria
        }
    };

    public static Campaign Factions { get; } = new()
    {
        Id = 2,
        Name = "Factions",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Factions",
        Continents = new List<Continent>
        {
            Continent.Cantha
        }
    };

    public static Campaign Nightfall { get; } = new()
    {
        Id = 3,
        Name = "Nightfall",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Nightfall",
        Continents = new List<Continent>
        {
            Continent.Elona,
            Continent.RealmOfTorment
        }
    };

    public static Campaign EyeOfTheNorth { get; } = new()
    {
        Id = 4,
        Name = "Eye of the North",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Eye_of_the_North",
        Continents = new List<Continent>
        {
            Continent.Tyria
        }
    };

    public static Campaign BonusMissionPack { get; } = new()
    {
        Id = 5,
        Name = "Bonus Mission Pack",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Bonus_Mission_Pack",
        Continents = new List<Continent>
        {
            Continent.Tyria,
            Continent.Elona,
            Continent.Cantha
        }
    };

    public static IReadOnlyList<Campaign> Campaigns { get; } = new List<Campaign>
    {
        Core,
        Prophecies,
        Factions,
        Nightfall,
        EyeOfTheNorth,
        BonusMissionPack
    };

    public static bool TryParse(int id, out Campaign campaign)
    {
        campaign = Campaigns.Where(campaign => campaign.Id == id).FirstOrDefault()!;
        if (campaign is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Campaign campaign)
    {
        campaign = Campaigns.Where(region => region.Name == name).FirstOrDefault()!;
        if (campaign is null)
        {
            return false;
        }

        return true;
    }
    public static Campaign Parse(int id)
    {
        if (TryParse(id, out var campaign) is false)
        {
            throw new InvalidOperationException($"Could not find a campaign with id {id}");
        }

        return campaign;
    }
    public static Campaign Parse(string name)
    {
        if (TryParse(name, out var region) is false)
        {
            throw new InvalidOperationException($"Could not find a campaign with name {name}");
        }

        return region;
    }

    private Campaign()
    {
    }

    public int Id { get; init; }
    public string? Name { get; init; }
    public string? WikiUrl { get; init; }
    public IReadOnlyList<Continent>? Continents { get; init; }
}
