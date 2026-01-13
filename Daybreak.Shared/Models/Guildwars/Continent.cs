using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class Continent
{
    public static Continent Tyria { get; } = new Continent
    {
        Id = 0,
        Name = "Tyria",
        WikiUrl = "https://wiki.guildwars.com/wiki/Tyria",
        Regions =
        [
            Region.Ascalon,
            Region.PresearingAscalon,
            Region.CrystalDesert,
            Region.Kryta,
            Region.MaguumaJungle,
            Region.RingOfFireIslands,
            Region.ShiverpeakMountains,
            Region.CharrHomelands,
            Region.DepthsOfTyria,
            Region.FarShiverpeaks,
            Region.TarnishedCoast,
            Region.TheFlightNorth,
            Region.TheRiseOfTheWhiteMantle
        ]
    };

    public static Continent TheMists { get; } = new Continent
    {
        Id = 1,
        Name = "The Mists",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Mists",
        Regions =
        [
            Region.HeroesAscent
        ]
    };

    public static Continent Cantha { get; } = new Continent
    {
        Id = 2,
        Name = "Cantha",
        WikiUrl = "https://wiki.guildwars.com/wiki/Cantha",
        Regions =
        [
            Region.ShingJeaIsland,
            Region.KainengCity,
            Region.EchovaldForest,
            Region.TheJadeSea,
            Region.TheTenguAccords
        ]
    };

    public static Continent TheBattleIsles { get; } = new Continent
    {
        Id = 3,
        Name = "The Battle Isles",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_Isles",
        Regions =
        [
            Region.TheBattleIsles,
        ]
    };

    public static Continent Elona { get; } = new Continent
    {
        Id = 4,
        Name = "Elona",
        WikiUrl = "https://wiki.guildwars.com/wiki/Elona",
        Regions =
        [
            Region.Istan,
            Region.Kourna,
            Region.Vabbi,
            Region.TheDesolation,
            Region.TheBattleOfJahai
        ]
    };

    public static Continent RealmOfTorment { get; } = new Continent
    {
        Id = 5,
        Name = "Realm of Torment",
        WikiUrl = "https://wiki.guildwars.com/wiki/Realm_of_Torment",
        Regions =
        [
            Region.RealmOfTorment
        ]
    };

    public static IReadOnlyList<Continent> Continents { get; } =
    [
        Tyria,
        TheMists,
        Cantha,
        TheBattleIsles,
        Elona,
        RealmOfTorment
    ];

    public static bool TryParse(int id, out Continent continent)
    {
        continent = Continents.Where(continent => continent.Id == id).FirstOrDefault()!;
        if (continent is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Continent continent)
    {
        continent = Continents.Where(continent => continent.Name == name).FirstOrDefault()!;
        if (continent is null)
        {
            return false;
        }

        return true;
    }
    public static Continent Parse(int id)
    {
        if (TryParse(id, out var continent) is false)
        {
            throw new InvalidOperationException($"Could not find a continent with id {id}");
        }

        return continent;
    }
    public static Continent Parse(string name)
    {
        if (TryParse(name, out var continent) is false)
        {
            throw new InvalidOperationException($"Could not find a continent with name {name}");
        }

        return continent;
    }

    private Continent()
    {
    }

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("wikiUrl")]
    public string? WikiUrl { get; init; }

    [JsonPropertyName("regions")]
    public IReadOnlyList<Region>? Regions { get; init; }
}
