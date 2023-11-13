using Daybreak.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Services.Screenshots.Models;
internal sealed class Event
{
    public string? Name { get; init; }
    public List<Map>? ValidLocations { get; init; }

    private Event()
    {
    }

    public static readonly Event Wintersday = new()
    {
        Name = "Wintersday",
        ValidLocations = new List<Map>
        {
            Map.AscalonCityWintersdayOutpost,
            Map.DroknarsForgeWintersdayOutpost,
            Map.EyeOfTheNorthOutpostWintersdayOutpost,
            Map.KamadanJewelOfIstanWintersdayOutpost,
            Map.TheGreatSnowballFightoftheGodsFightinginaWinterWonderland,
            Map.TravelersVale,
            Map.BorlisPass,
            Map.IronHorseMine,
            Map.TheFrostGate,
            Map.AnvilRock,
            Map.IceToothCaveOutpost,
            Map.DeldrimorBowl,
            Map.BeaconsPerchOutpost,
            Map.GriffonsMouth,
            Map.DroknarsForgeOutpost,
            Map.WitmansFolly,
            Map.PortSledgeOutpost,
            Map.TalusChute,
            Map.IceCavesofSorrow,
            Map.CampRankorOutpost,
            Map.SnakeDance,
            Map.DreadnoughtsDrift,
            Map.LornarsPass,
            Map.GrenthsFootprint,
            Map.SpearheadPeak,
            Map.TheGraniteCitadelOutpost,
            Map.TascasDemise,
            Map.MineralSprings,
            Map.Icedome,
            Map.CopperhammerMinesOutpost,
            Map.FrozenForest,
            Map.IronMinesofMoladune,
            Map.IceFloe,
            Map.MarhansGrottoOutpost,
            Map.ThunderheadKeep,
            Map.IceCliffChasms,
            Map.GunnarsHoldOutpost,
            Map.NorrhartDomains,
            Map.OlafsteadOutpost,
            Map.VarajarFells,
            Map.SifhallaOutpost,
            Map.DrakkarLake,
            Map.JagaMoraine,
            Map.BjoraMarches
        }
    };
}
