﻿using Daybreak.Shared.Converters;
using Newtonsoft.Json;

namespace Daybreak.Shared.Models.Guildwars;

[JsonConverter(typeof(RegionJsonConverter))]
public sealed class Region : IWikiEntity
{
    public static readonly Region Kryta = new()
    { 
        Id = 0,
        Name = "Kryta",
        WikiUrl = "https://wiki.guildwars.com/wiki/Kryta",
        Maps =
        [
            Map.LionsArchCanthanNewYearOutpost,
            Map.LionsArchHalloweenOutpost,
            Map.LionsArchOutpost,
            Map.LionsArchSunspearsinKryta,
            Map.LionsArchWintersdayOutpost,
            Map.BeetletunOutpost,
            Map.BergenHotSpringsOutpost,
            Map.FishermensHavenOutpost,
            Map.TempleOfTheAges,
            Map.TempleOfTheAgesROX,
            Map.DAlessioSeaboard,
            Map.DivinityCoast,
            Map.GatesOfKryta,
            Map.RiversideProvince,
            Map.WarinKrytaRiversideProvince,
            Map.SanctumCay,
            Map.CursedLands,
            Map.KessexPeak,
            Map.LionsGate,
            Map.MajestysRest,
            Map.NeboTerrace,
            Map.NorthKrytaProvince,
            Map.ScoundrelsRise,
            Map.StingrayStrand,
            Map.TalmarkWilderness,
            Map.TearsOfTheFallen,
            Map.TheBlackCurtain,
            Map.TwinSerpentLakes,
            Map.WatchtowerCoast
        ]
    };
    public static readonly Region MaguumaJungle = new()
    {
        Id = 1,
        Name = "Maguuma Jungle",
        WikiUrl = "https://wiki.guildwars.com/wiki/Maguuma_Jungle",
        Maps =
        [
            Map.HengeOfDenraviOutpost,
            Map.DruidsOverlookOutpost,
            Map.MaguumaStadeOutpost,
            Map.QuarrelFallsOutpost,
            Map.VentarisRefugeOutpost,
            Map.AuroraGlade,
            Map.BloodstoneFen,
            Map.BloodstoneFenQuest,
            Map.TheWilds,
            Map.DryTop,
            Map.EttinsBack,
            Map.MajestysRest,
            Map.MamnoonLagoon,
            Map.ReedBog,
            Map.SageLands,
            Map.Silverwood,
            Map.TangleRoot,
            Map.TheFalls
        ]
    };
    public static readonly Region Ascalon = new()
    { 
        Id = 2,
        Name = "Ascalon",
        WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon",
        Maps =
        [
            Map.AscalonCityOutpost,
            Map.AscalonCityWintersdayOutpost,
            Map.FrontierGateOutpost,
            Map.EasternFrontier,
            Map.GrendichCourthouseOutpost,
            Map.SpecialOpsGrendichCourthouse,
            Map.PikenSquareOutpost,
            Map.SardelacSanitariumOutpost,
            Map.SerenityTempleOutpost,
            Map.FortRanik,
            Map.RuinsOfSurmia,
            Map.NolaniAcademy,
            Map.TheGreatNorthernWall,
            Map.AscalonArena,
            Map.AscalonArenaMission,
            Map.AscalonFoothills,
            Map.DiessaLowlands,
            Map.DragonsGullet,
            Map.EasternFrontier,
            Map.FlameTempleCorridor,
            Map.OldAscalon,
            Map.PockmarkFlats,
            Map.RegentValley,
            Map.TheBreach
        ]
    };
    public static readonly Region ShiverpeakMountains = new()
    { 
        Id = 3,
        Name = "Shiverpeak Mountains",
        WikiUrl = "https://wiki.guildwars.com/wiki/Shiverpeak_Mountains",
        Maps =
        [
            Map.BeaconsPerchOutpost,
            Map.IceToothCaveOutpost,
            Map.YaksBendOutpost,
            Map.BorlisPass,
            Map.TheFrostGate,
            Map.ShiverpeakArena,
            Map.ShiverpeakArenaMission,
            Map.ShiverpeakArenaMission2,
            Map.ShiverpeakArenaMission3,
            Map.AnvilRock,
            Map.DeldrimorBowl,
            Map.GriffonsMouth,
            Map.IronHorseMine,
            Map.TravelersVale,
            Map.DroknarsForgeOutpost,
            Map.DroknarsForgeWintersdayOutpost,
            Map.DroknarsForgeHalloweenOutpost,
            Map.DroknarsForgeCinematic,
            Map.CampRankorOutpost,
            Map.CopperhammerMinesOutpost,
            Map.DeldrimorWarCampOutpost,
            Map.MarhansGrottoOutpost,
            Map.PortSledgeOutpost,
            Map.TheGraniteCitadelOutpost,
            Map.IceCavesofSorrow,
            Map.IronMinesofMoladune,
            Map.ThunderheadKeep,
            Map.DreadnoughtsDrift,
            Map.FrozenForest,
            Map.GrenthsFootprint,
            Map.IceFloe,
            Map.Icedome,
            Map.LornarsPass,
            Map.MineralSprings,
            Map.SnakeDance,
            Map.SorrowsFurnace,
            Map.SpearheadPeak,
            Map.TalusChute,
            Map.TascasDemise,
            Map.WitmansFolly
        ]
    };
    public static readonly Region HeroesAscent = new()
    { 
        Id = 4,
        Name = "Heroes' Ascent",
        WikiUrl = "https://wiki.guildwars.com/wiki/Heroes%27_Ascent",
        Maps =
        [
            Map.BurialMoundsMission,
            Map.FetidRiverMission,
            Map.TheUnderworldArenaMission,
            Map.UnholyTemplesMission,
            Map.ForgottenShrinesMission,
            Map.GoldenGatesMission,
            Map.TheCourtyardArenaMission,
            Map.TheVaultMission,
            Map.TheHallOfHeroesArenaMission,
            Map.BrokenTowerMission,
            Map.SacredTemplesMission,
            Map.ScarredEarth,
            Map.ScarredEarth2
        ]
    };
    public static readonly Region CrystalDesert = new()
    { 
        Id = 5, 
        Name = "Crystal Desert", 
        WikiUrl = "https://wiki.guildwars.com/wiki/Crystal_Desert",
        Maps =
        [
            Map.TheAmnoonOasisOutpost,
            Map.DestinysGorgeOutpost,
            Map.HeroesAudienceOutpost,
            Map.SeekersPassageOutpost,
            Map.TombOfThePrimevalKings,
            Map.TombOfThePrimevalKingsHalloweenOutpost,
            Map.AuguryRockMission,
            Map.AuguryRockOutpost,
            Map.DunesOfDespair,
            Map.ElonaReach,
            Map.ThirstyRiver,
            Map.TheDragonsLair,
            Map.DivinersAscent,
            Map.ProphetsPath,
            Map.SaltFlats,
            Map.SkywardReach,
            Map.TheAridSea,
            Map.TheScar,
            Map.VultureDrifts
        ]
    };
    public static readonly Region RingOfFireIslands = new()
    {
        Id = 6,
        Name = "Ring of Fire Islands",
        WikiUrl = "https://wiki.guildwars.com/wiki/Ring_of_Fire_Islands",
        Maps =
        [
            Map.EmberLightCampOutpost,
            Map.AbaddonsMouth,
            Map.HellsPrecipice,
            Map.RingOfFire,
            Map.PerditionRock,
            Map.TheFissureofWoe
        ]
    };
    public static readonly Region PresearingAscalon = new()
    {
        Id = 7,
        Name = "Pre Searing Ascalon",
        WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_(pre-Searing)",
        Maps =
        [
            Map.AscalonCityPresearing,
            Map.AshfordAbbeyOutpost,
            Map.AshfordCatacombs1070AE,
            Map.FoiblesFairOutpost,
            Map.FortRanikPreSearingOutpost,
            Map.TheBarradinEstateOutpost,
            Map.AscalonAcademyPvPBattleMission,
            Map.GreenHillsCounty,
            Map.LakesideCounty,
            Map.LakesideCounty1070AE,
            Map.RegentValleyPreSearing,
            Map.TheCatacombs,
            Map.TheNorthlands,
            Map.WizardsFolly
        ]
    };
    public static readonly Region KainengCity = new()
    { 
        Id = 8,
        Name = "Kaineng City",
        WikiUrl = "https://wiki.guildwars.com/wiki/Kaineng_City",
        Maps =
        [
            Map.KainengCenterOutpost,
            Map.KainengCenterCanthanNewYearOutpost,
            Map.KainengCenterSunspearsInCantha,
            Map.KainengCenterWindsOfChangeAChanceEncounter,
            Map.KainengCenterWindsOfChangeRaidonKainengCenter,
            Map.MaatuKeepOutpost,
            Map.SenjisCornerOutpost,
            Map.TheMarketplaceOutpost,
            Map.TheMarketplaceAreaTrackingtheCorruption,
            Map.ZinKuCorridorOutpost,
            Map.DragonsThroat,
            Map.DragonsThroatAreaWhatWaitsInShadow,
            Map.ImperialSanctumOutpostMission,
            Map.NahpuiQuarterOutpostMission,
            Map.NahpuiQuarterExplorable,
            Map.RaisuPalace,
            Map.RaisuPalaceOutpostMission,
            Map.SunjiangDistrictExplorable,
            Map.SunjiangDistrictOutpostMission,
            Map.TahnnakaiTempleOutpostMission,
            Map.TahnnakaiTempleExplorable,
            Map.TahnnakaiTempleWindsOfChangeTheRescueAttempt,
            Map.VizunahSquareMission,
            Map.VizunahSquareForeignQuarterOutpost,
            Map.VizunahSquareLocalQuarterOutpost,
            Map.BejunkanPier,
            Map.BukdekByway,
            Map.BukdekBywayWindsOfChangeCanthaCourierCrisis,
            Map.DivinePath,
            Map.KainengDocks,
            Map.PongmeiValley,
            Map.RaisuPavilion,
            Map.ShadowsPassage,
            Map.ShenzunTunnels,
            Map.TheUndercity,
            Map.WajjunBazaar,
            Map.WajjunBazaarPOX,
            Map.WajjunBazaarWindsOfChangeMinistryOfOppression,
            Map.WajjunBazaarWindsOfChangeViolenceInTheStreets,
            Map.XaquangSkyway
        ]
    };
    public static readonly Region EchovaldForest = new()
    {
        Id = 9,
        Name = "Echovald Forest",
        WikiUrl = "https://wiki.guildwars.com/wiki/Echovald_Forest",
        Maps =
        [
            Map.HouseZuHeltzerOutpost,
            Map.AspenwoodGateKurzickOutpost,
            Map.BrauerAcademyOutpost,
            Map.DurheimArchivesOutpost,
            Map.JadeFlatsKurzickOutpost,
            Map.LutgardisConservatoryOutpost,
            Map.SaintAnjekasShrineOutpost,
            Map.TanglewoodCopseOutpost,
            Map.VasburgArmoryOutpost,
            Map.AltrummRuins,
            Map.AltrummRuinsFindingJinnai,
            Map.AmatzBasin,
            Map.ArborstoneOutpostMission,
            Map.ArborstoneExplorable,
            Map.FortAspenwoodKurzickOutpost,
            Map.FortAspenwoodMission,
            Map.TheEternalGrove,
            Map.TheEternalGroveOutpostMission,
            Map.TheJadeQuarryKurzickOutpost,
            Map.UnwakingWatersKurzickOutpost,
            Map.UrgozsWarren,
            Map.DrazachThicket,
            Map.Ferndale,
            Map.MelandrusHope,
            Map.MorostavTrail,
            Map.MourningVeilFalls
        ]
    };
    public static readonly Region TheJadeSea = new()
    {
        Id = 10,
        Name = "The Jade Sea",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Sea",
        Maps =
        [
            Map.CavalonOutpost,
            Map.AspenwoodGateLuxonOutpost,
            Map.BaiPaasuReachOutpost,
            Map.BreakerHollowOutpost,
            Map.EredonTerraceOutpost,
            Map.HarvestTempleOutpost,
            Map.JadeFlatsLuxonOutpost,
            Map.LeviathanPitsOutpost,
            Map.SeafarersRestOutpost,
            Map.BoreasSeabedExplorable,
            Map.BoreasSeabedOutpostMission,
            Map.FortAspenwoodLuxonOutpost,
            Map.GyalaHatchery,
            Map.GyalaHatcheryOutpostMission,
            Map.TheAuriosMines,
            Map.TheDeep,
            Map.TheJadeQuarryLuxonOutpost,
            Map.TheJadeQuarryMission,
            Map.UnwakingWaters,
            Map.UnwakingWatersLuxonOutpost,
            Map.UnwakingWatersMission,
            Map.ZosShivrosChannel,
            Map.Archipelagos,
            Map.MaishangHills,
            Map.MountQinkai,
            Map.RheasCrater,
            Map.SilentSurf
        ]
    };
    public static readonly Region ShingJeaIsland = new()
    {
        Id = 11,
        Name = "Shing Jea Island",
        WikiUrl = "https://wiki.guildwars.com/wiki/Shing_Jea_Island",
        Maps =
        [
            Map.ShingJeaArena,
            Map.ShingJeaArenaMission,
            Map.ShingJeaMonasteryCanthanNewYearOutpost,
            Map.ShingJeaMonasteryDragonFestivalOutpost,
            Map.ShingJeaMonasteryMission,
            Map.ShingJeaMonasteryOutpost,
            Map.ShingJeaMonasteryRaidOnShingJeaMonastery,
            Map.RanMusuGardensOutpost,
            Map.SeitungHarborAreaDeadlyCargo,
            Map.SeitungHarborMission,
            Map.SeitungHarborMission2,
            Map.SeitungHarborOutpost,
            Map.TsumeiVillageMission,
            Map.TsumeiVillageMission2,
            Map.TsumeiVillageOutpost,
            Map.TsumeiVillageWindsOfChangeATreatysATreaty,
            Map.MinisterChosEstateExplorable,
            Map.MinisterChosEstateMission2,
            Map.MinisterChosEstateOutpostMission,
            Map.ZenDaijunExplorable,
            Map.ZenDaijunOutpostMission,
            Map.HaijuLagoon,
            Map.HaijuLagoonMission,
            Map.JayaBluffs,
            Map.JayaBluffsMission,
            Map.KinyaProvince,
            Map.LinnokCourtyard,
            Map.MonasteryOverlook1,
            Map.MonasteryOverlook2,
            Map.PanjiangPeninsula,
            Map.SaoshangTrail,
            Map.SunquaVale
        ]
    };
    public static readonly Region Kourna = new()
    {
        Id = 12,
        Name = "Kourna",
        WikiUrl = "https://wiki.guildwars.com/wiki/Kourna",
        Maps =
        [
            Map.SunspearSanctuaryOutpost,
            Map.CampHojanuOutpost,
            Map.WehhanTerracesOutpost,
            Map.YohlonHavenOutpost,
            Map.DajkahInlet,
            Map.KodonurCrossroads,
            Map.ModdokCrevice,
            Map.NunduBay,
            Map.PogahnPassage,
            Map.RilohnRefuge,
            Map.VentaCemetery,
            Map.ArkjokWard,
            Map.BahdokCaverns,
            Map.BarbarousShore,
            Map.CommandPost,
            Map.DejarinEstate,
            Map.GandaraTheMoonFortress,
            Map.JahaiBluffs,
            Map.MargaCoast,
            Map.SunwardMarches,
            Map.TheFloodplainOfMahnkelon,
            Map.TuraisProcession,
            Map.NightfallenCoast
        ]
    };
    public static readonly Region Vabbi = new()
    {
        Id = 13,
        Name = "Vabbi",
        WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi",
        Maps =
        [
            Map.TheKodashBazaarOutpost,
            Map.BasaltGrottoOutpost,
            Map.ChantryOfSecretsOutpost,
            Map.HonurHillOutpost,
            Map.MihanuTownshipOutpost,
            Map.YahnurMarketOutpost,
            Map.DashaVestibule,
            Map.DzagonurBastion,
            Map.GrandCourtOfSebelkeh,
            Map.JennursHorde,
            Map.TiharkOrchard,
            Map.BokkaAmphitheatre,
            Map.BokkaAmphitheatreNOX,
            Map.ForumHighlands,
            Map.GardenOfSeborhin,
            Map.HoldingsOfChokhin,
            Map.ResplendentMakuun,
            Map.ResplendentMakuun2,
            Map.TheHiddenCityOfAhdashim,
            Map.TheMirrorOfLyss,
            Map.VehjinMines,
            Map.VehtendiValley,
            Map.WildernessOfBahdza,
            Map.YatendiCanyons
        ]
    };
    public static readonly Region TheDesolation = new()
    {
        Id = 14,
        Name = "TheDesolation",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Desolation",
        Maps =
        [
            Map.BonePalaceOutpost,
            Map.LairOfTheForgottenOutpost,
            Map.TheMouthOfTormentOutpost,
            Map.GateOfDesolation,
            Map.RuinsOfMorah,
            Map.RemainsOfSahlahja,
            Map.CrystalOverlook,
            Map.JokosDomain,
            Map.PoisonedOutcrops,
            Map.TheAlkaliPan,
            Map.TheRupturedHeart,
            Map.TheShatteredRavines,
            Map.TheSulfurousWastes
        ]
    };
    public static readonly Region Istan = new()
    {
        Id = 15,
        Name = "Istan",
        WikiUrl = "https://wiki.guildwars.com/wiki/Istan",
        Maps =
        [
            Map.KamadanJewelOfIstanCanthanNewYearOutpost,
            Map.KamadanJewelOfIstanExplorable,
            Map.KamadanJewelOfIstanHalloweenOutpost,
            Map.KamadanJewelOfIstanOutpost,
            Map.KamadanJewelOfIstanWintersdayOutpost,
            Map.KamadanMission,
            Map.BeknurHarbor,
            Map.BeknurHarborOutpost,
            Map.ChampionsDawnOutpost,
            Map.KodlonuHamletOutpost,
            Map.SunspearArena,
            Map.SunspearArenaMission,
            Map.SunspearGreatHallOutpost,
            Map.TheAstralariumOutpost,
            Map.BlacktideDen,
            Map.ChahbekVillage,
            Map.Consulate,
            Map.ConsulateDocks,
            Map.JokanurDiggings,
            Map.ChurrhirFields,
            Map.CliffsOfDohjok,
            Map.FahranurMission,
            Map.FahranurTheFirstCity,
            Map.IslandOfShehkah,
            Map.IssnurIsles,
            Map.LahtendaBog,
            Map.MehtaniKeys,
            Map.PlainsOfJarin,
            Map.SunDocks,
            Map.ZehlonReach
        ]
    };
    public static readonly Region RealmOfTorment = new()
    {
        Id = 16,
        Name = "Realm of Torment",
        WikiUrl = "https://wiki.guildwars.com/wiki/Realm_of_Torment",
        Maps =
        [
            Map.DomainOfAnguish,
            Map.GateOfTormentOutpost,
            Map.GateOfFearOutpost,
            Map.GateOfSecretsOutpost,
            Map.GateOftheNightfallenLandsOutpost,
            Map.AbaddonsGate,
            Map.GateOfMadness,
            Map.GateOfPain,
            Map.TheShadowNexus,
            Map.TheEbonyCitadelOfMallyxMission,
            Map.DepthsOfMadness,
            Map.DomainOfFear,
            Map.DomainOfPain,
            Map.DomainOfSecrets,
            Map.HeartOfAbaddon,
            Map.NightfallenGarden,
            Map.NightfallenJahai,
            Map.ThroneOfSecrets
        ]
    };
    public static readonly Region TarnishedCoast = new()
    {
        Id = 17,
        Name = "Tarnished Coast",
        WikiUrl = "https://wiki.guildwars.com/wiki/Tarnished_Coast",
        Maps =
        [
            Map.RataSumOutpost,
            Map.GaddsEncampmentOutpost,
            Map.TarnishedHavenOutpost,
            Map.UmbralGrottoOutpost,
            Map.VloxsFalls,
            Map.FindingTheBloodstoneMission,
            Map.FindingTheBloodstoneLevel1,
            Map.FindingTheBloodstoneLevel2,
            Map.FindingTheBloodstoneLevel3,
            Map.TheElusiveGolemancerMission,
            Map.TheElusiveGolemancerLevel1,
            Map.TheElusiveGolemancerLevel2,
            Map.TheElusiveGolemancerLevel3,
            Map.GeniusOperatedLivingEnchantedManifestation,
            Map.GeniusOperatedLivingEnchantedManifestationMission,
            Map.ArborBay,
            Map.AlcaziaTangle,
            Map.MagusStones,
            Map.PolymockColiseum,
            Map.RivenEarth,
            Map.SparkflySwamp,
            Map.VerdantCascades
        ]
    };
    public static readonly Region DepthsOfTyria = new()
    {
        Id = 18,
        Name = "Depths Of Tyria",
        WikiUrl = "https://wiki.guildwars.com/wiki/Depths_of_Tyria",
        Maps =
        [
            Map.CentralTransferChamberOutpost,
            Map.DestructionsDepthsMission,
            Map.DestructionsDepthsLevel1,
            Map.DestructionsDepthsLevel2,
            Map.DestructionsDepthsLevel3,
            Map.ATimeForHeroes,
            Map.ATimeForHeroesMission,
            Map.BattledepthsLevel1,
            Map.BattledepthsLevel2,
            Map.BattledepthsLevel3,
            Map.BeneathLionsArch,
            Map.CavernsBelowKamadan,
            Map.TunnelsBelowCantha,
            Map.WarinKrytaTheMausoleum,
            Map.ArachnisHauntLevel1,
            Map.ArachnisHauntLevel2,
            Map.BloodstoneCavesLevel1,
            Map.BloodstoneCavesLevel2,
            Map.BloodstoneCavesLevel3,
            Map.BogrootGrowthsLevel1,
            Map.BogrootGrowthsLevel2,
            Map.CatacombsofKathandraxLevel1,
            Map.CatacombsofKathandraxLevel2,
            Map.CatacombsofKathandraxLevel3,
            Map.CathedralofFlamesLevel1,
            Map.CathedralofFlamesLevel2,
            Map.CathedralofFlamesLevel3,
            Map.DarkrimeDelvesLevel1,
            Map.DarkrimeDelvesLevel2,
            Map.DarkrimeDelvesLevel3,
            Map.FronisIrontoesLairMission,
            Map.HeartOftheShiverpeaksLevel1,
            Map.HeartOftheShiverpeaksLevel2,
            Map.HeartOftheShiverpeaksLevel3,
            Map.OolasLabLevel1,
            Map.OolasLabLevel2,
            Map.OolasLabLevel3,
            Map.OozePit,
            Map.OozePitMission,
            Map.RavensPointLevel1,
            Map.RavensPointLevel2,
            Map.RavensPointLevel3,
            Map.RragarsMenagerieLevel1,
            Map.RragarsMenagerieLevel2,
            Map.RragarsMenagerieLevel3,
            Map.SecretLairOftheSnowmen,
            Map.SecretLairOftheSnowmen2,
            Map.SecretLairOftheSnowmen3,
            Map.SepulchreOfDragrimmarLevel1,
            Map.SepulchreOfDragrimmarLevel2,
            Map.ShardsOfOrrLevel1,
            Map.ShardsOfOrrLevel2,
            Map.ShardsOfOrrLevel3,
            Map.SlaversExileLevel1,
            Map.SlaversExileLevel2,
            Map.SlaversExileLevel3,
            Map.SlaversExileLevel4,
            Map.SlaversExileLevel5,
            Map.VloxenExcavationsLevel1,
            Map.VloxenExcavationsLevel2,
            Map.VloxenExcavationsLevel3
        ]
    };
    public static readonly Region FarShiverpeaks = new()
    {
        Id = 19,
        Name = "Far Shiverpeaks",
        WikiUrl = "https://wiki.guildwars.com/wiki/Far_Shiverpeaks",
        Maps =
        [
            Map.GunnarsHoldOutpost,
            Map.BorealStationOutpost,
            Map.EyeOfTheNorthOutpost,
            Map.EyeOfTheNorthOutpostWintersdayOutpost,
            Map.LongeyesLedgeOutpost,
            Map.OlafsteadOutpost,
            Map.OlafsteadCinematic,
            Map.SifhallaOutpost,
            Map.CurseOfTheNornbear,
            Map.CurseOfTheNornbearMission,
            Map.CinematicCaveNornCursed,
            Map.AGateTooFarMission,
            Map.AGateTooFarLevel1,
            Map.AGateTooFarLevel2,
            Map.AGateTooFarLevel3,
            Map.BloodWashesBlood,
            Map.BloodWashesBloodMission,
            Map.TheNornFightingTournament,
            Map.BjoraMarches,
            Map.DrakkarLake,
            Map.Epilogue,
            Map.HallOfMonuments,
            Map.IceCliffChasms,
            Map.JagaMoraine,
            Map.NorrhartDomains,
            Map.PolymockGlacier,
            Map.VarajarFells
        ]
    };
    public static readonly Region CharrHomelands = new()
    {
        Id = 20,
        Name = "Charr Homelands",
        WikiUrl = "https://wiki.guildwars.com/wiki/Charr_Homelands",
        Maps =
        [
            Map.DoomloreShrineOutpost,
            Map.AgainstTheCharr,
            Map.AgainstTheCharrMission,
            Map.WarbandOfBrothersMission,
            Map.WarbandOfBrothersLevel1,
            Map.WarbandOfBrothersLevel2,
            Map.WarbandOfBrothersLevel3,
            Map.AssaultOnTheStronghold,
            Map.AssaultOnTheStrongholdMission,
            Map.DaladaUplands,
            Map.GrothmarWardowns,
            Map.PolymockCrossing,
            Map.SacnothValley
        ]
    };
    public static readonly Region TheBattleIsles = new()
    {
        Id = 21,
        Name = "The Battle Isles",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_Isles",
        Maps =
        [
            Map.GreatTempleOfBalthazarOutpost,
            Map.IsleOfTheDeadGuildHall,
            Map.IsleOfTheDeadGuildHallMission,
            Map.IsleOfTheDeadGuildHallOutpost,
            Map.BurningIsle,
            Map.BurningIsleMission,
            Map.BurningIsleOutpost,
            Map.DruidsIsle,
            Map.DruidsIsleMission,
            Map.DruidsIsleOutpost,
            Map.FrozenIsle,
            Map.FrozenIsleMission,
            Map.FrozenIsleOutpost,
            Map.HuntersIsle,
            Map.HuntersIsleMission,
            Map.HuntersIsleOutpost,
            Map.NomadsIsle,
            Map.NomadsIsleMission,
            Map.NomadsIsleOutpost,
            Map.WarriorsIsle,
            Map.WarriorsIsleMission,
            Map.WarriorsIsleOutpost,
            Map.WizardsIsle,
            Map.WizardsIsleMission,
            Map.WizardsIsleOutpost,
            Map.ImperialIsle,
            Map.ImperialIsleMission,
            Map.ImperialIsleOutpost,
            Map.IsleOfJade,
            Map.IsleOfJadeMission,
            Map.IsleOfJadeOutpost,
            Map.IsleOfMeditation,
            Map.IsleOfMeditationMission,
            Map.IsleOfMeditationOutpost,
            Map.IsleOfWeepingStone,
            Map.IsleOfWeepingStoneMission,
            Map.IsleOfWeepingStoneOutpost,
            Map.CorruptedIsle,
            Map.CorruptedIsleMission,
            Map.CorruptedIsleOutpost,
            Map.IsleOfSolitude,
            Map.IsleOfSolitudeMission,
            Map.IsleOfSolitudeOutpost,
            Map.IsleOfWurms,
            Map.IsleOfWurmsMission,
            Map.IsleOfWurmsOutpost,
            Map.UnchartedIsle,
            Map.UnchartedIsleMission,
            Map.UnchartedIsleOutpost
        ]
    };
    public static readonly Region TheBattleOfJahai = new()
    {
        Id = 22,
        Name = "The Battle Of Jahai",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_of_Jahai",
        Maps =
        [
            Map.TheBattleOfJahai
        ]
    };
    public static readonly Region TheFlightNorth = new()
    {
        Id = 23,
        Name = "The Flight North",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Flight_North",
        Maps =
        [
            Map.TheFlightNorth
        ]
    };
    public static readonly Region TheTenguAccords = new()
    {
        Id = 24,
        Name = "The Tengu Accords",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Tengu_Accords",
        Maps =
        [
            Map.TheTenguAccords
        ]
    };
    public static readonly Region TheRiseOfTheWhiteMantle = new()
    {
        Id = 25,
        Name = "The Rise Of The White Mantle",
        WikiUrl = "https://wiki.guildwars.com/wiki/The_Rise_of_the_White_Mantle",
        Maps =
        [
            Map.TheRiseOfTheWhiteMantle
        ]
    };
    public static readonly Region Swat = new() { Id = 26 };
    public static readonly Region DevRegion = new() { Id = 27 };

    public static IReadOnlyList<Region> Regions { get; } =
    [
        Kryta,
        MaguumaJungle,
        Ascalon,
        ShiverpeakMountains,
        HeroesAscent,
        CrystalDesert,
        RingOfFireIslands,
        PresearingAscalon,
        KainengCity,
        EchovaldForest,
        TheJadeSea,
        Kourna,
        Vabbi,
        TheDesolation,
        Istan,
        RealmOfTorment,
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
    ];

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
    public IReadOnlyList<Map>? Maps { get; init; }
}
