using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class Campaign
{
    public static Campaign None { get; } = new()
    {
        Id = -1,
        Name = "None"
    };

    public static Campaign Core { get; } = new()
    {
        Id = 0,
        Name = "Core",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Core",
        Continents =
        [
            Continent.TheBattleIsles,
            Continent.TheMists
        ]
    };

    public static Campaign Prophecies { get; } = new()
    {
        Id = 1,
        Name = "Prophecies",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Prophecies",
        Continents =
        [
            Continent.Tyria
        ]
    };

    public static Campaign Factions { get; } = new()
    {
        Id = 2,
        Name = "Factions",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Factions",
        Continents =
        [
            Continent.Cantha
        ]
    };

    public static Campaign Nightfall { get; } = new()
    {
        Id = 3,
        Name = "Nightfall",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Nightfall",
        Continents =
        [
            Continent.Elona,
            Continent.RealmOfTorment
        ]
    };

    public static Campaign EyeOfTheNorth { get; } = new()
    {
        Id = 4,
        Name = "Eye of the North",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Eye_of_the_North",
        Continents =
        [
            Continent.Tyria
        ]
    };

    public static Campaign BonusMissionPack { get; } = new()
    {
        Id = 5,
        Name = "Bonus Mission Pack",
        WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Wars_Bonus_Mission_Pack",
        Continents =
        [
            Continent.Tyria,
            Continent.Elona,
            Continent.Cantha
        ]
    };

    public static IReadOnlyList<Campaign> Campaigns { get; } =
    [
        None,
        Core,
        Prophecies,
        Factions,
        Nightfall,
        EyeOfTheNorth,
        BonusMissionPack
    ];

    public static bool TryParse(int id, [NotNullWhen(true)] out Campaign? campaign)
    {
        campaign = Campaigns.Where(campaign => campaign.Id == id).FirstOrDefault();
        if (campaign is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, [NotNullWhen(true)] out Campaign? campaign)
    {
        campaign = Campaigns.Where(region => region.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) is true).FirstOrDefault();
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

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("wikiUrl")]
    public string? WikiUrl { get; init; }

    [JsonPropertyName("continents")]
    public IReadOnlyList<Continent>? Continents { get; init; }
}
