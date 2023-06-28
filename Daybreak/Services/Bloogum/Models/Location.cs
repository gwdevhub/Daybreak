using Daybreak.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Services.Bloogum.Models;

public sealed class Location
{
    public static readonly Location AscalonPreSearing = new(
        Region.PresearingAscalon,
        "pre",
        new List<Category>
        {
                new Category(Map.AscalonCityPresearing, "openingcutscene", 5),
                new Category(Map.AscalonCityPresearing, "ascaloncity", 3),
                new Category(Map.LakesideCounty, "lakesidecounty", 15),
                new Category(Map.AshfordAbbeyOutpost, "ashfordabbey", 1),
                new Category(Map.TheCatacombs, "thecatacombs", 13),
                new Category(Map.GreenHillsCounty, "greenhillscounty", 14),
                new Category(Map.TheBarradinEstateOutpost, "thebarradinestate", 3),
                new Category(Map.WizardsFolly, "wizardsfolly", 20),
                new Category(Map.RegentValleyPreSearing, "regentvalley", 31),
                new Category(Map.FortRanikPreSearingOutpost, "fortranik", 1),
                new Category(Map.TheNorthlands, "thenorthlands", 18),
                new Category(Map.AscalonAcademyPvPBattleMission, "ascalonacademy", 10)
        });
    public static readonly Location Ascalon = new(
        Region.Ascalon,
        "ascalon",
        new List<Category>
        {
                new Category(Map.AscalonCityOutpost, "ascaloncity", 16),
                new Category(Map.AscalonCityWintersdayOutpost, "ascaloncity", 16),
                new Category(Map.OldAscalon, "oldascalon", 13),
                new Category(Map.RegentValley, "regentvalley", 9),
                new Category(Map.PockmarkFlats, "pockmarkflats", 12),
                new Category(Map.EasternFrontier, "easternfrontier", 6),
                new Category(Map.TheBreach, "thebreach", 6),
                new Category(Map.DiessaLowlands, "diessalowlands", 5),
                new Category(Map.DragonsGullet, "dragonsgullet", 4),
                new Category(Map.AscalonFoothills, "ascalonfoothills", 2),
                new Category(Map.TheGreatNorthernWall, "thegreatnorthernwall", 6),
                new Category(Map.FortRanik, "fortranik", 11),
                new Category(Map.RuinsOfSurmia, "ruinsofsurmia", 8),
                new Category(Map.NolaniAcademy, "nolaniacademy", 18)
        });
    public static readonly Location NorthernShiverpeaks = new(
        Region.ShiverpeakMountains,
        "nshiverpeaks",
        new List<Category>
        {
                new Category(Map.TravelersVale, "travelersvale", 20),
                new Category(Map.YaksBendOutpost, "yaksbend", 7),
                new Category(Map.BorlisPass, "borlispass", 37),
                new Category(Map.IronHorseMine, "ironhorsemine", 18),
                new Category(Map.TheFrostGate, "thefrostgate", 18),
                new Category(Map.AnvilRock, "anvilrock", 21),
                new Category(Map.IceToothCaveOutpost, "icetoothcave", 2),
                new Category(Map.DeldrimorBowl, "deldrimorbowl", 26),
                new Category(Map.BeaconsPerchOutpost, "beaconsperch", 8),
                new Category(Map.GriffonsMouth, "griffonsmouth", 7)
        });
    public static readonly Location Kryta = new(
        Region.Kryta,
        "kryta",
        new List<Category>
        {
                new Category(Map.ScoundrelsRise, "scoundrelsrise", 5),
                new Category(Map.GatesOfKryta, "gatesofkryta", 29),
                new Category(Map.LionsGate, "lionsgate", 2),
                new Category(Map.LionsArchOutpost, "lionsarch", 44),
                new Category(Map.WarinKrytaLionsArchKeep, "lionsarchkeep", 9),
                new Category(Map.NorthKrytaProvince, "northkrytaprovince", 42),
                new Category(Map.DAlessioSeaboard, "dallesioseaboard", 39),
                new Category(Map.DAlessioArenaMission, "dallesioseaboard", 39),
                new Category(Map.DAlessioArenaMission2, "dallesioseaboard", 39),
                new Category(Map.DAlessioArenaMission3, "dallesioseaboard", 39),
                new Category(Map.NeboTerrace, "neboterrace", 15),
                new Category(Map.BergenHotSpringsOutpost, "bergenhotsprings", 8),
                new Category(Map.CursedLands, "cursedlands", 1),
                new Category(Map.BeetletunOutpost, "beetletun", 6),
                new Category(Map.WatchtowerCoast, "watchtowercoast", 14),
                new Category(Map.DivinityCoast, "divinitycoast", 34),
                new Category(Map.TempleOfTheAges, "templeofages", 5),
                new Category(Map.TempleOfTheAgesROX, "templeofages", 5),
                new Category(Map.TheBlackCurtain, "theblackcurtain", 25),
                new Category(Map.KessexPeak, "kessexpeak", 11),
                new Category(Map.TalmarkWilderness, "talmarkwilderness", 30),
                new Category(Map.MajestysRest, "majestysrest", 14),
                new Category(Map.TearsOfTheFallen, "tearsofthefallen", 14),
                new Category(Map.TwinSerpentLakes, "twinserpentlakes", 16),
                new Category(Map.StingrayStrand, "stingraystrand", 15),
                new Category(Map.FishermensHavenOutpost, "fishermenshaven", 4),
                new Category(Map.RiversideProvince, "riversideprovince", 31),
                new Category(Map.WarinKrytaRiversideProvince, "riversideprovince", 31),
                new Category(Map.SanctumCay, "sanctumcay", 21),
                new Category(Map.MajestysRest, "majestysrest", 14)
        });
    public static readonly Location MaguumaJungle = new(
        Region.MaguumaJungle,
        "maguuma",
        new List<Category>
        {
                new Category(Map.DruidsOverlookOutpost, "druidsoverlook", 1),
                new Category(Map.SageLands, "sagelands", 27),
                new Category(Map.TheWilds, "thewilds", 19),
                new Category(Map.MamnoonLagoon, "mamnoonlagoon", 17),
                new Category(Map.QuarrelFallsOutpost, "quarrelfalls", 2),
                new Category(Map.Silverwood, "silverwood", 29),
                new Category(Map.BloodstoneFen, "bloodstonefen", 24),
                new Category(Map.BloodstoneFenQuest, "bloodstonefen", 24),
                new Category(Map.EttinsBack, "ettinsback", 21),
                new Category(Map.VentarisRefugeOutpost, "ventarisrefuge", 1),
                new Category(Map.ReedBog, "reedbog", 12),
                new Category(Map.TheFalls, "thefalls", 22),
                new Category(Map.DryTop, "drytop", 5),
                new Category(Map.TangleRoot, "tangleroot", 20),
                new Category(Map.MaguumaStadeOutpost, "maguumastade", 1),
                new Category(Map.AuroraGlade, "auroraglade", 12),
                new Category(Map.HengeOfDenraviOutpost, "hengeofdenravi", 12)
        });
    public static readonly Location CrystalDesert = new(
        Region.CrystalDesert,
        "crystaldesert",
        new List<Category>
        {
                new Category(Map.TheAmnoonOasisOutpost, "amnoonoasis", 6),
                new Category(Map.ProphetsPath, "prophetspath", 24),
                new Category(Map.HeroesAudienceOutpost, "heroesaudience", 3),
                new Category(Map.SaltFlats, "seaflats", 22),
                new Category(Map.SeekersPassageOutpost, "seekerspassage", 2),
                new Category(Map.DivinersAscent, "divinersascent", 21),
                new Category(Map.ElonaReach, "elonareach", 23),
                new Category(Map.SkywardReach, "skywardreach", 34),
                new Category(Map.DestinysGorgeOutpost, "destinysgorge", 4),
                new Category(Map.TheScar, "thescar", 25),
                new Category(Map.ThirstyRiver, "thirstyriver", 24),
                new Category(Map.TheAridSea, "thearidsea", 40),
                new Category(Map.VultureDrifts, "vulturedrifts", 40),
                new Category(Map.DunesOfDespair, "dunesofdespair", 36),
                new Category(Map.AuguryRockMission, "auguryrock", 25),
                new Category(Map.AuguryRockOutpost, "auguryrock", 25),
                new Category(Map.TombOfThePrimevalKings, "tomboftheprimevalkings", 17),
                new Category(Map.TombOfThePrimevalKingsHalloweenOutpost, "tomboftheprimevalkings", 17),
                new Category(Map.TheDragonsLair, "thedragonslair", 60)
        });
    public static readonly Location SouthernShiverpeaks = new(
        Region.ShiverpeakMountains,
        "sshiverpeaks",
        new List<Category>
        {
                new Category(Map.DroknarsForgeOutpost, "droknarsforge", 40),
                new Category(Map.DroknarsForgeCinematic, "droknarsforge", 40),
                new Category(Map.DroknarsForgeHalloweenOutpost, "droknarsforge", 40),
                new Category(Map.DroknarsForgeWintersdayOutpost, "droknarsforge", 40),
                new Category(Map.WitmansFolly, "witmansfolly", 48),
                new Category(Map.PortSledgeOutpost, "portsledge", 7),
                new Category(Map.TalusChute, "taluschute", 54),
                new Category(Map.IceCavesofSorrow, "icecavesofsorrow", 50),
                new Category(Map.CampRankorOutpost, "camprankor", 2),
                new Category(Map.SnakeDance, "snakedance", 23),
                new Category(Map.DreadnoughtsDrift, "dreadnoughtsdrift", 6),
                new Category(Map.LornarsPass, "lornarspass", 33),
                new Category(Map.DeldrimorWarCampOutpost, "deldrimorwarcamp", 3),
                new Category(Map.GrenthsFootprint, "grenthsfootprint", 26),
                new Category(Map.SpearheadPeak, "spearheadpeak", 37),
                new Category(Map.TheGraniteCitadelOutpost, "thegranitecitadel", 11),
                new Category(Map.TascasDemise, "tascasdemise", 11),
                new Category(Map.MineralSprings, "mineralsprings", 41),
                new Category(Map.Icedome, "icedome", 10),
                new Category(Map.CopperhammerMinesOutpost, "copperhammermines", 2),
                new Category(Map.FrozenForest, "frozenforest", 49),
                new Category(Map.IronMinesofMoladune, "ironminesofmoladune", 46),
                new Category(Map.IceFloe, "icefloe", 54),
                new Category(Map.MarhansGrottoOutpost, "marhansgrotto", 3),
                new Category(Map.ThunderheadKeep, "thunderheadkeep", 57)
        });
    public static readonly Location RingOfFireIslandChain = new(
        Region.RingOfFireIslands,
        "fireisles",
        new List<Category>
        {
                new Category(Map.EmberLightCampOutpost, "emberlightcamp", 3),
                new Category(Map.PerditionRock, "perditionrock", 37),
                new Category(Map.RingOfFire, "ringoffire", 36),
                new Category(Map.AbaddonsMouth, "abaddonsmouth", 47),
                new Category(Map.HellsPrecipice, "hellsprecipice", 38)
        });
    public static readonly Location FarShiverpeaks = new(
        Region.FarShiverpeaks,
        "fshiverpeaks",
        new List<Category>
        {
                new Category(Map.BorealStationOutpost, "borealstation", 2),
                new Category(Map.IceCliffChasms, "icecliffchasms", 38),
                new Category(Map.EyeOfTheNorthOutpost, "eyeofthenorth", 6),
                new Category(Map.EyeOfTheNorthOutpostWintersdayOutpost, "eyeofthenorth", 6),
                new Category(Map.HallOfMonuments, "hallofmonuments", 2),
                new Category(Map.GunnarsHoldOutpost, "gunnarshold", 9),
                new Category(Map.NorrhartDomains, "norrhartdomains", 48),
                new Category(Map.OlafsteadCinematic, "olafstead", 3),
                new Category(Map.OlafsteadOutpost, "olafstead", 3),
                new Category(Map.VarajarFells, "varajarfells", 51),
                new Category(Map.VarajarFellsunknown, "varajarfells", 51),
                new Category(Map.SifhallaOutpost, "sifhalla", 8),
                new Category(Map.DrakkarLake, "drakkarlake", 50),
                new Category(Map.JagaMoraine, "jagamoraine", 47),
                new Category(Map.BjoraMarches, "bjoramarches", 56),
                new Category(Map.LongeyesLedgeOutpost, "longeyesledge", 4)
        });
    public static readonly Location CharrHomelands = new(
        Region.CharrHomelands,
        "charr",
        new List<Category>
        {
                new Category(Map.GrothmarWardowns, "grothmarwardowns", 64),
                new Category(Map.DoomloreShrineOutpost, "doomloreshrine", 5),
                new Category(Map.DaladaUplands, "daladauplands", 61),
                new Category(Map.SacnothValley, "sacnothvalley", 60)
        });
    public static readonly Location TarnishedCoast = new(
        Region.TarnishedCoast,
        "tarnishedcoast",
        new List<Category>
        {
                new Category(Map.VerdantCascades, "verdantcascades", 52),
                new Category(Map.UmbralGrottoOutpost, "umbralgrotto", 1),
                new Category(Map.GaddsEncampmentOutpost, "gaddsencampment", 6),
                new Category(Map.SparkflySwamp, "sparkflyswamp", 37),
                new Category(Map.VloxsFalls, "vloxsfalls", 4),
                new Category(Map.ArborBay, "arborbay", 52),
                new Category(Map.AlcaziaTangle, "alcaziatangle", 41),
                new Category(Map.TarnishedHavenOutpost, "tarnishedhaven", 3),
                new Category(Map.RivenEarth, "rivenearth", 50),
                new Category(Map.RataSumOutpost, "ratasum", 6),
                new Category(Map.MagusStones, "magusstones", 37),
                new Category(Map.PolymockColiseum, "polymockcoliseum", 4)
        });
    public static readonly Location DepthsOfTyria = new(
        Region.DepthsOfTyria,
        "dungeons",
        new List<Category>
        {
                new Category(Map.TheFissureofWoe, "fissureofwoe", 14),
                new Category(Map.CatacombsofKathandraxLevel1, "catacombsofkathandrax", 47),
                new Category(Map.CatacombsofKathandraxLevel2, "catacombsofkathandrax", 47),
                new Category(Map.CatacombsofKathandraxLevel3, "catacombsofkathandrax", 47),
                new Category(Map.RragarsMenagerieLevel1, "rragarsmenagerie", 50),
                new Category(Map.RragarsMenagerieLevel2, "rragarsmenagerie", 50),
                new Category(Map.RragarsMenagerieLevel3, "rragarsmenagerie", 50),
                new Category(Map.CathedralofFlamesLevel1, "cathedralofflame", 56),
                new Category(Map.CathedralofFlamesLevel2, "cathedralofflame", 56),
                new Category(Map.CathedralofFlamesLevel3, "cathedralofflame", 56),
                new Category(Map.OozePit, "oozepit", 52),
                new Category(Map.OozePitMission, "oozepit", 52),
                new Category(Map.DarkrimeDelvesLevel1, "darkrimedelves", 58),
                new Category(Map.DarkrimeDelvesLevel2, "darkrimedelves", 58),
                new Category(Map.DarkrimeDelvesLevel3, "darkrimedelves", 58),
                new Category(Map.FrostmawsBurrowsLevel1, "frostmawsburrows", 54),
                new Category(Map.FrostmawsBurrowsLevel2, "frostmawsburrows", 54),
                new Category(Map.FrostmawsBurrowsLevel3, "frostmawsburrows", 54),
                new Category(Map.FrostmawsBurrowsLevel4, "frostmawsburrows", 54),
                new Category(Map.FrostmawsBurrowsLevel5, "frostmawsburrows", 54),
                new Category(Map.SepulchreOfDragrimmarLevel1, "sepulchreofdragrimmar", 35),
                new Category(Map.SepulchreOfDragrimmarLevel2, "sepulchreofdragrimmar", 35),
                new Category(Map.RavensPointLevel1, "ravenspoint", 64),
                new Category(Map.RavensPointLevel2, "ravenspoint", 64),
                new Category(Map.RavensPointLevel3, "ravenspoint", 64),
                new Category(Map.VloxenExcavationsLevel1, "vloxenexcavations", 52),
                new Category(Map.VloxenExcavationsLevel2, "vloxenexcavations", 52),
                new Category(Map.VloxenExcavationsLevel3, "vloxenexcavations", 52),
                new Category(Map.BogrootGrowthsLevel1, "bogrootgrowths", 47),
                new Category(Map.BogrootGrowthsLevel2, "bogrootgrowths", 47),
                new Category(Map.BloodstoneCavesLevel1, "bloodstonecaves", 20),
                new Category(Map.BloodstoneCavesLevel2, "bloodstonecaves", 20),
                new Category(Map.BloodstoneCavesLevel3, "bloodstonecaves", 20),
                new Category(Map.ShardsOfOrrLevel1, "shardsoforr", 40),
                new Category(Map.ShardsOfOrrLevel2, "shardsoforr", 40),
                new Category(Map.ShardsOfOrrLevel3, "shardsoforr", 40),
                new Category(Map.OolasLabLevel1, "oolaslab", 46),
                new Category(Map.OolasLabLevel2, "oolaslab", 46),
                new Category(Map.OolasLabLevel3, "oolaslab", 46),
                new Category(Map.ArachnisHauntLevel1, "arachnishaunt", 34),
                new Category(Map.ArachnisHauntLevel2, "arachnishaunt", 34),
                new Category(Map.SlaversExileLevel1, "slaversexile", 59),
                new Category(Map.SlaversExileLevel2, "slaversexile", 59),
                new Category(Map.SlaversExileLevel3, "slaversexile", 59),
                new Category(Map.SlaversExileLevel4, "slaversexile", 59),
                new Category(Map.SlaversExileLevel5, "slaversexile", 59),
                new Category(Map.FronisIrontoesLairMission, "fronisirontoeslair", 11),
                new Category(Map.SecretLairOftheSnowmen, "secretlairofthesnowmen", 16),
                new Category(Map.SecretLairOftheSnowmen2, "secretlairofthesnowmen", 16),
                new Category(Map.SecretLairOftheSnowmen3, "secretlairofthesnowmen", 16),
                new Category(Map.HeartOftheShiverpeaksLevel1, "heartoftheshiverpeaks", 40),
                new Category(Map.HeartOftheShiverpeaksLevel2, "heartoftheshiverpeaks", 40),
                new Category(Map.HeartOftheShiverpeaksLevel3, "heartoftheshiverpeaks", 40)
        });
    public static readonly Location ShingJeaIsland = new(
        Region.ShingJeaIsland,
        "shingjea",
        new List<Category>
        {
                new Category(Map.MonasteryOverlook1, "monasteryoverlook", 12),
                new Category(Map.MonasteryOverlook2, "monasteryoverlook", 12),
                new Category(Map.ShingJeaMonasteryCanthanNewYearOutpost, "shingjeamonastery", 15),
                new Category(Map.ShingJeaMonasteryDragonFestivalOutpost, "shingjeamonastery", 15),
                new Category(Map.ShingJeaMonasteryMission, "shingjeamonastery", 15),
                new Category(Map.ShingJeaMonasteryOutpost, "shingjeamonastery", 15),
                new Category(Map.ShingJeaMonasteryRaidOnShingJeaMonastery, "shingjeamonastery", 15),
                new Category(Map.SunquaVale, "sunquavale", 40),
                new Category(Map.TsumeiVillageMission, "tsumeivillage", 3),
                new Category(Map.TsumeiVillageMission2, "tsumeivillage", 3),
                new Category(Map.TsumeiVillageOutpost, "tsumeivillage", 3),
                new Category(Map.TsumeiVillageWindsOfChangeATreatysATreaty, "tsumeivillage", 3),
                new Category(Map.PanjiangPeninsula, "panjiangpeninsula", 47),
                new Category(Map.RanMusuGardensOutpost, "ranmusugardens", 4),
                new Category(Map.KinyaProvince, "kinyaprovince", 29),
                new Category(Map.MinisterChosEstateExplorable, "ministerchosestate", 26),
                new Category(Map.MinisterChosEstateMission2, "ministerchosestate", 26),
                new Category(Map.MinisterChosEstateOutpostMission, "ministerchosestate", 26),
                new Category(Map.LinnokCourtyard, "linnokcourtyard", 1),
                new Category(Map.SaoshangTrail, "saoshangtrail", 3),
                new Category(Map.SeitungHarborAreaDeadlyCargo, "seitungharbor", 8),
                new Category(Map.SeitungHarborMission, "seitungharbor", 8),
                new Category(Map.SeitungHarborMission2, "seitungharbor", 8),
                new Category(Map.SeitungHarborOutpost, "seitungharbor", 8),
                new Category(Map.JayaBluffs, "jayabluffs", 24),
                new Category(Map.JayaBluffsMission, "jayabluffs", 24),
                new Category(Map.ZenDaijunExplorable, "zendaijun", 24),
                new Category(Map.ZenDaijunOutpostMission, "zendaijun", 24),
                new Category(Map.HaijuLagoon, "haijulagoon", 38),
                new Category(Map.HaijuLagoonMission, "haijulagoon", 38),
                new Category(Map.ShingJeaArena, "shingjeaarena", 2),
                new Category(Map.ShingJeaArenaMission, "shingjeaarena", 2)
        });
    public static readonly Location KainengCity = new(
        Region.KainengCity,
        "kaineng",
        new List<Category>
        {
                new Category(Map.KainengCenterCanthanNewYearOutpost, "kainengcenter", 30),
                new Category(Map.KainengCenterOutpost, "kainengcenter", 30),
                new Category(Map.KainengCenterSunspearsInCantha, "kainengcenter", 30),
                new Category(Map.KainengCenterWindsOfChangeAChanceEncounter, "kainengcenter", 30),
                new Category(Map.KainengCenterWindsOfChangeRaidonKainengCenter, "kainengcenter", 30),
                new Category(Map.BejunkanPier, "bejunkanpier", 3),
                new Category(Map.BukdekByway, "bukdekbyway", 28),
                new Category(Map.BukdekBywayWindsOfChangeCanthaCourierCrisis, "bukdekbyway", 28),
                new Category(Map.TheMarketplaceOutpost, "themarketplace", 5),
                new Category(Map.TheMarketplaceAreaTrackingtheCorruption, "themarketplace", 5),
                new Category(Map.KainengDocks, "kainengdocks", 5),
                new Category(Map.WajjunBazaar, "wajjunbazaar", 30),
                new Category(Map.WajjunBazaarPOX, "wajjunbazaar", 30),
                new Category(Map.WajjunBazaarWindsOfChangeMinistryOfOppression, "wajjunbazaar", 30),
                new Category(Map.WajjunBazaarWindsOfChangeViolenceInTheStreets, "wajjunbazaar", 30),
                new Category(Map.SenjisCornerOutpost, "senjiscorner", 1),
                new Category(Map.XaquangSkyway, "xaquangskyway", 10),
                new Category(Map.DragonsThroat, "dragonsthroat", 3),
                new Category(Map.DragonsThroatAreaWhatWaitsInShadow, "dragonsthroat", 3),
                new Category(Map.NahpuiQuarterExplorable, "nahpuiquarter", 22),
                new Category(Map.NahpuiQuarterOutpostMission, "nahpuiquarter", 22),
                new Category(Map.ShadowsPassage, "shadowspassage", 2),
                new Category(Map.ShadowsPassageWindsofChangeCallingAllThugs, "shadowspassage", 2),
                new Category(Map.ShenzunTunnels, "shenzuntunnels", 13),
                new Category(Map.TahnnakaiTempleExplorable, "tahnnakaitemple", 13),
                new Category(Map.TahnnakaiTempleOutpostMission, "tahnnakaitemple", 13),
                new Category(Map.TahnnakaiTempleWindsOfChangeTheRescueAttempt, "tahnnakaitemple", 13),
                new Category(Map.ZinKuCorridorOutpost, "zinkucorridor", 5),
                new Category(Map.VizunahSquareForeignQuarterOutpost, "vizunahsquare", 12),
                new Category(Map.VizunahSquareLocalQuarterOutpost, "vizunahsquare", 12),
                new Category(Map.VizunahSquareMission, "vizunahsquare", 12),
                new Category(Map.TheUndercity, "theundercity", 2),
                new Category(Map.SunjiangDistrictExplorable, "sunjiangdistrict", 8),
                new Category(Map.SunjiangDistrictOutpostMission, "sunjiangdistrict", 8),
                new Category(Map.MaatuKeepOutpost, "maatukeep", 1),
                new Category(Map.PongmeiValley, "pongmeivalley", 13),
                new Category(Map.RaisuPavilion, "raisupavilion", 5),
                new Category(Map.RaisuPalace, "raisupalace", 37),
                new Category(Map.RaisuPalaceOutpostMission, "raisupalace", 37),
                new Category(Map.ImperialSanctumOutpostMission, "imperialsanctum", 9),
                new Category(Map.DivinePath, "divinepath", 5)
        });
    public static readonly Location EchovaldForest = new(
        Region.EchovaldForest,
        "echovald",
        new List<Category>
        {
                new Category(Map.TanglewoodCopseOutpost, "tanglewoodcopse", 2),
                new Category(Map.ArborstoneExplorable, "arborstone", 37),
                new Category(Map.ArborstoneOutpostMission, "arborstone", 37),
                new Category(Map.AltrummRuins, "altrummruins", 9),
                new Category(Map.AltrummRuinsFindingJinnai, "altrummruins", 9),
                new Category(Map.HouseZuHeltzerOutpost, "housezuheltzer", 6),
                new Category(Map.Ferndale, "ferndale", 25),
                new Category(Map.AspenwoodGateKurzickOutpost, "aspenwoodgate", 1),
                new Category(Map.FortAspenwoodKurzickOutpost, "fortaspenwood", 11),
                new Category(Map.FortAspenwoodMission, "fortaspenwood", 11),
                new Category(Map.SaintAnjekasShrineOutpost, "saintanjekasshrine", 1),
                new Category(Map.DrazachThicket, "drazachthicket", 24),
                new Category(Map.LutgardisConservatoryOutpost, "lutgardisconservatory", 2),
                new Category(Map.BrauerAcademyOutpost, "braueracademy", 3),
                new Category(Map.MelandrusHope, "melandrushope", 28),
                new Category(Map.JadeFlatsKurzickOutpost, "jadeflats", 2),
                new Category(Map.TheJadeQuarryKurzickOutpost, "thejadequarry", 2),
                new Category(Map.TheEternalGrove, "theeternalgrove", 31),
                new Category(Map.TheEternalGroveOutpostMission, "theeternalgrove", 31),
                new Category(Map.VasburgArmoryOutpost, "vasburgarmory", 4),
                new Category(Map.MorostavTrail, "morostavtrail", 17),
                new Category(Map.DurheimArchivesOutpost, "durheimarchives", 3),
                new Category(Map.MourningVeilFalls, "mourningveilfalls", 31),
                new Category(Map.AmatzBasin, "amatzbasin", 13),
                new Category(Map.UnwakingWatersKurzickOutpost, "unwakingwaters", 4),
                new Category(Map.UrgozsWarren, "urgozswarren", 13)
        });
    public static readonly Location JadeSea = new(
        Region.TheJadeSea,
        "jadesea",
        new List<Category>
        {
                new Category(Map.BoreasSeabedExplorable, "boreasseabed", 42),
                new Category(Map.BoreasSeabedOutpostMission, "boreasseabed", 42),
                new Category(Map.ZosShivrosChannel, "zosshivroschannel", 9),
                new Category(Map.CavalonOutpost, "cavalon", 7),
                new Category(Map.Archipelagos, "archipelagos", 29),
                new Category(Map.BreakerHollowOutpost, "breakerhollow", 1),
                new Category(Map.MountQinkai, "mountqinkai", 24),
                new Category(Map.AspenwoodGateLuxonOutpost, "aspenwoodgate", 2),
                new Category(Map.FortAspenwoodLuxonOutpost, "fortaspenwood", 3),
                new Category(Map.JadeFlatsLuxonOutpost, "jadeflats", 1),
                new Category(Map.TheJadeQuarryLuxonOutpost, "thejadequarry", 8),
                new Category(Map.TheJadeQuarryMission, "thejadequarry", 8),
                new Category(Map.MaishangHills, "maishanghills", 26),
                new Category(Map.BaiPaasuReachOutpost, "baipaasureach", 1),
                new Category(Map.EredonTerraceOutpost, "eredonterrace", 4),
                new Category(Map.GyalaHatchery, "gyalahatchery", 36),
                new Category(Map.GyalaHatcheryOutpostMission, "gyalahatchery", 36),
                new Category(Map.LeviathanPitsOutpost, "leviathanpits", 3),
                new Category(Map.SilentSurf, "silentsurf", 27),
                new Category(Map.SeafarersRestOutpost, "seafarersrest", 2),
                new Category(Map.RheasCrater, "rheascrater", 28),
                new Category(Map.TheAuriosMines, "theauriosmines", 10),
                new Category(Map.UnwakingWatersLuxonOutpost, "unwakingwaters", 14),
                new Category(Map.UnwakingWatersMission, "unwakingwaters", 14),
                new Category(Map.HarvestTempleOutpost, "harvesttemple", 2),
                new Category(Map.TheDeep, "thedeep", 18)
        });
    public static readonly Location Istan = new(
        Region.Istan,
        "istan",
        new List<Category>
        {
                new Category(Map.IslandOfShehkah, "islandofshehkah", 12),
                new Category(Map.ChahbekVillage, "chahbekvillage", 1),
                new Category(Map.ChurrhirFields, "churrhirfields", 6),
                new Category(Map.KamadanJewelOfIstanCanthanNewYearOutpost, "kamadan", 16),
                new Category(Map.KamadanJewelOfIstanExplorable, "kamadan", 16),
                new Category(Map.KamadanJewelOfIstanHalloweenOutpost, "kamadan", 16),
                new Category(Map.KamadanJewelOfIstanOutpost, "kamadan", 16),
                new Category(Map.KamadanJewelOfIstanWintersdayOutpost, "kamadan", 16),
                new Category(Map.KamadanMission, "kamadan", 16),
                new Category(Map.SunDocks, "sundocks", 2),
                new Category(Map.SunspearArena, "sunspeararena", 1),
                new Category(Map.SunspearArenaMission, "sunspeararena", 1),
                new Category(Map.PlainsOfJarin, "plainsofjarin", 27),
                new Category(Map.SunspearGreatHallOutpost, "sunspeargreathall", 1),
                new Category(Map.TheAstralariumOutpost, "theastralarium", 7),
                new Category(Map.ChampionsDawnOutpost, "championsdawn", 2),
                new Category(Map.CliffsOfDohjok, "cliffsofdohjok", 26),
                new Category(Map.ZehlonReach, "zehlonreach", 46),
                new Category(Map.JokanurDiggings, "jokanurdiggings", 3),
                new Category(Map.FahranurMission, "fahranurthefirstcity", 29),
                new Category(Map.FahranurTheFirstCity, "fahranurthefirstcity", 29),
                new Category(Map.BlacktideDen, "blacktideden", 3),
                new Category(Map.LahtendaBog, "lahtendabog", 24),
                new Category(Map.BeknurHarbor, "beknurharbor", 3),
                new Category(Map.BeknurHarborOutpost, "beknurharbor", 3),
                new Category(Map.IssnurIsles, "issnurisles", 35),
                new Category(Map.KodlonuHamletOutpost, "kodlonuhamlet", 9),
                new Category(Map.MehtaniKeys, "mehtanikeys", 32),
                new Category(Map.Consulate, "consulate", 1),
                new Category(Map.ConsulateDocks, "consulatedocks", 2)
        });
    public static readonly Location Kourna = new(
        Region.Kourna,
        "kourna",
        new List<Category>
        {
                new Category(Map.YohlonHavenOutpost, "yohlonhaven", 4),
                new Category(Map.ArkjokWard, "arkjokward", 49),
                new Category(Map.SunspearSanctuaryOutpost, "sunspearsanctuary", 8),
                new Category(Map.CommandPost, "commandpost", 4),
                new Category(Map.SunwardMarches, "sunwardmarches", 47),
                new Category(Map.VentaCemetery, "ventacemetery", 2),
                new Category(Map.MargaCoast, "margacoast", 57),
                new Category(Map.DajkahInlet, "dajkahinlet", 5),
                new Category(Map.JahaiBluffs, "jahaibluffs", 37),
                new Category(Map.KodonurCrossroads, "kodonurcrossroads", 3),
                new Category(Map.DejarinEstate, "dejarinestate", 45),
                new Category(Map.PogahnPassage, "pogahnpassage", 2),
                new Category(Map.GandaraTheMoonFortress, "gandarathemoonfortress", 36),
                new Category(Map.CampHojanuOutpost, "camphojanu", 3),
                new Category(Map.BarbarousShore, "barbarousshore", 38),
                new Category(Map.RilohnRefuge, "rilohnrefuge", 1),
                new Category(Map.TheFloodplainOfMahnkelon, "thefloodplainofmahnkelon", 20),
                new Category(Map.ModdokCrevice, "moddokcrevice", 3),
                new Category(Map.BahdokCaverns, "bahdokcaverns", 3),
                new Category(Map.WehhanTerracesOutpost, "wehhanterraces", 6),
                new Category(Map.NunduBay, "nundubay", 37),
                new Category(Map.TuraisProcession, "turaisprocession", 35)
        });
    public static readonly Location Vabbi = new(
        Region.Vabbi,
        "vabbi",
        new List<Category>
        {
                new Category(Map.YatendiCanyons, "yatendicanyons", 24),
                new Category(Map.ChantryOfSecretsOutpost, "chantryofsecrets", 4),
                new Category(Map.VehtendiValley, "vehtendivalley", 43),
                new Category(Map.YahnurMarketOutpost, "yahnurmarket", 6),
                new Category(Map.ForumHighlands, "forumhighlands", 52),
                new Category(Map.TiharkOrchard, "tiharkorchard", 6),
                new Category(Map.ResplendentMakuun, "resplendentmakuun", 46),
                new Category(Map.ResplendentMakuun2, "resplendentmakuun", 46),
                new Category(Map.BokkaAmphitheatre, "bokkaamphitheatre", 5),
                new Category(Map.BokkaAmphitheatreNOX, "bokkaamphitheatre", 5),
                new Category(Map.HonurHillOutpost, "honurhill", 3),
                new Category(Map.TheKodashBazaarOutpost, "thekodashbazaar", 18),
                new Category(Map.TheMirrorOfLyss, "themirroroflyss", 33),
                new Category(Map.DzagonurBastion, "dzagonurbastion", 4),
                new Category(Map.WildernessOfBahdza, "wildernessofbahdza", 32),
                new Category(Map.DashaVestibule, "dashavestibule", 2),
                new Category(Map.TheHiddenCityOfAhdashim, "thehiddencityofadashim", 36),
                new Category(Map.MihanuTownshipOutpost, "mihanutownship", 3),
                new Category(Map.HoldingsOfChokhin, "holdingsofchokhin", 27),
                new Category(Map.GardenOfSeborhin, "gardenofseborhin", 34),
                new Category(Map.GrandCourtOfSebelkeh, "grandcourtofsebelkeh", 9),
                new Category(Map.JennursHorde, "jennurshorde", 4),
                new Category(Map.VehjinMines, "vehjinmines", 29),
                new Category(Map.BasaltGrottoOutpost, "basaltgrotto", 2)
        });
    public static readonly Location TheDesolation = new(
        Region.TheDesolation,
        "desolation",
        new List<Category>
        {
                new Category(Map.GateOfDesolation, "gateofdesolation", 6),
                new Category(Map.TheSulfurousWastes, "thesulfurouswastes", 30),
                new Category(Map.RemainsOfSahlahja, "remainsofsahlahja", 4),
                new Category(Map.RemainsOfSahlahja, "dynastictombs", 4),
                new Category(Map.JokosDomain, "jokosdomain", 34),
                new Category(Map.TheShatteredRavines, "theshatteredravines", 31),
                new Category(Map.LairOfTheForgottenOutpost, "lairoftheforgotten", 6),
                new Category(Map.PoisonedOutcrops, "poisonedoutcrops", 33),
                new Category(Map.BonePalaceOutpost, "bonepalace", 4),
                new Category(Map.TheAlkaliPan, "thealkalipan", 24),
                new Category(Map.CrystalOverlook, "crystaloverlook", 32),
                new Category(Map.RuinsOfMorah, "ruinsofmorah", 2),
                new Category(Map.TheRupturedHeart, "therupturedheart", 18),
                new Category(Map.TheMouthOfTormentOutpost, "themouthoftorment", 5)
        });
    public static readonly Location GateOfTorment = new(
        Region.RealmOfTorment,
        "torment",
        new List<Category>
        {
                new Category(Map.GateOfTormentOutpost, "gateoftorment", 15),
                new Category(Map.NightfallenJahai, "nightfallenjahai", 39),
                new Category(Map.GateOftheNightfallenLandsOutpost, "gateofthenightfallenlands", 4),
                new Category(Map.NightfallenGarden, "nightfallengarden", 31),
                new Category(Map.GateOfPain, "gateofpain", 4),
                new Category(Map.DomainOfPain, "domainofpain", 30),
                new Category(Map.GateOfFearOutpost, "gateoffear", 3),
                new Category(Map.DomainOfFear, "domainoffear", 28),
                new Category(Map.GateOfSecretsOutpost, "gateofsecrets", 6),
                new Category(Map.DomainOfSecrets, "domainofsecrets", 32),
                new Category(Map.GateOfMadness, "gateofmadness", 2),
                new Category(Map.DepthsOfMadness, "depthsofmadness", 46),
                new Category(Map.HeartOfAbaddon, "heartofabaddon", 11),
                new Category(Map.AbaddonsGate, "abaddonsgate", 10),
                new Category(Map.ThroneOfSecrets, "throneofsecrets", 9),
                new Category(Map.DomainOfAnguish, "gateofanguish", 4)
        });
    public static readonly Location BattleIsles = new(
        Region.TheBattleIsles,
        "battleisles",
        new List<Category>
        {
                new Category(Map.GreatTempleOfBalthazarOutpost, "greattempleofbalthazar", 6),
                new Category(Map.IsleOfTheNameless, "isleofthenameless", 22),
                new Category(Map.IsleOfTheNamelessPvP, "isleofthenameless", 22),
                new Category(Map.ZaishenMenagerieOutpost, "zaishenmenagerie", 3),
                new Category(Map.ZaishenMenagerieGrounds, "zaishenmenageriegrounds", 34),
                new Category(Map.HeroesAscentOutpost, "heroesascent", 7),
                new Category(Map.CodexArenaOutpost, "codexarena", 7),
                new Category(Map.RandomArenasOutpost, "randomarenas", 8),
                new Category(Map.ZaishenChallengeOutpost, "zaishenchallenge", 10),
                new Category(Map.ZaishenEliteOutpost, "zaishenelite", 5),
                new Category(Map.EmbarkBeach, "embarkbeach", 12)
        });
    public static readonly Location GuildHalls = new(
        Region.TheBattleIsles,
        "gh",
        new List<Category>
        {
                new Category(Map.WarriorsIsle, "warriorsisle", 20),
                new Category(Map.WarriorsIsleMission, "warriorsisle", 20),
                new Category(Map.WarriorsIsleOutpost, "warriorsisle", 20),
                new Category(Map.WizardsIsle, "wizardsisle", 21),
                new Category(Map.WizardsIsleMission, "wizardsisle", 21),
                new Category(Map.WizardsIsleOutpost, "wizardsisle", 21),
                new Category(Map.IsleOfTheDeadGuildHall, "isleofthedead", 10),
                new Category(Map.IsleOfTheDeadGuildHallMission, "isleofthedead", 10),
                new Category(Map.IsleOfTheDeadGuildHallOutpost, "isleofthedead", 10),
                new Category(Map.FrozenIsle, "frozenisle", 14),
                new Category(Map.FrozenIsleMission, "frozenisle", 14),
                new Category(Map.FrozenIsleOutpost, "frozenisle", 14),
                new Category(Map.HuntersIsle, "huntersisle", 14),
                new Category(Map.HuntersIsleMission, "huntersisle", 14),
                new Category(Map.HuntersIsleOutpost, "huntersisle", 14),
                new Category(Map.DruidsIsle, "druidsisle", 7),
                new Category(Map.DruidsIsleMission, "druidsisle", 7),
                new Category(Map.DruidsIsleOutpost, "druidsisle", 7),
                new Category(Map.NomadsIsle, "nomadsisle", 12),
                new Category(Map.NomadsIsleMission, "nomadsisle", 12),
                new Category(Map.NomadsIsleOutpost, "nomadsisle", 12),
                new Category(Map.BurningIsle, "burningisle", 15),
                new Category(Map.BurningIsleMission, "burningisle", 15),
                new Category(Map.BurningIsleOutpost, "burningisle", 15),
                new Category(Map.IsleOfMeditation, "isleofmeditation", 16),
                new Category(Map.IsleOfMeditationMission, "isleofmeditation", 16),
                new Category(Map.IsleOfMeditationOutpost, "isleofmeditation", 16),
                new Category(Map.IsleOfJade, "isleofjade", 8),
                new Category(Map.IsleOfJadeMission, "isleofjade", 8),
                new Category(Map.IsleOfJadeOutpost, "isleofjade", 8),
                new Category(Map.IsleOfWeepingStone, "isleofweepingstone", 17),
                new Category(Map.IsleOfWeepingStoneMission, "isleofweepingstone", 17),
                new Category(Map.IsleOfWeepingStoneOutpost, "isleofweepingstone", 17),
                new Category(Map.ImperialIsle, "imperialisle", 13),
                new Category(Map.ImperialIsleMission, "imperialisle", 13),
                new Category(Map.ImperialIsleOutpost, "imperialisle", 13),
                new Category(Map.UnchartedIsle, "uncharteredisle", 14),
                new Category(Map.UnchartedIsleMission, "uncharteredisle", 14),
                new Category(Map.UnchartedIsleOutpost, "uncharteredisle", 14),
                new Category(Map.CorruptedIsle, "corruptedisle", 8),
                new Category(Map.CorruptedIsleMission, "corruptedisle", 8),
                new Category(Map.CorruptedIsleOutpost, "corruptedisle", 8),
                new Category(Map.IsleOfSolitude, "isleofsolitude", 11),
                new Category(Map.IsleOfSolitudeMission, "isleofsolitude", 11),
                new Category(Map.IsleOfSolitudeOutpost, "isleofsolitude", 11),
                new Category(Map.IsleOfWurms, "isleofwurms", 12),
                new Category(Map.IsleOfWurmsMission, "isleofwurms", 12),
                new Category(Map.IsleOfWurmsOutpost, "isleofwurms", 12)
        });

    public static List<Location> Locations { get; } = new List<Location>
    {
        AscalonPreSearing,
        Ascalon,
        NorthernShiverpeaks,
        Kryta,
        MaguumaJungle,
        CrystalDesert,
        SouthernShiverpeaks,
        RingOfFireIslandChain,
        FarShiverpeaks,
        CharrHomelands,
        TarnishedCoast,
        DepthsOfTyria,
        ShingJeaIsland,
        KainengCity,
        EchovaldForest,
        JadeSea,
        Istan,
        Kourna,
        Vabbi,
        TheDesolation,
        GateOfTorment,
        BattleIsles,
        GuildHalls
    };

    public Region Region { get; }
    public string LocationName { get; }
    public List<Category> Categories { get; } = new();
    private Location(Region region, string locationName, List<Category> categories)
    {
        this.Region = region;
        this.LocationName = locationName;
        this.Categories = categories;
    }
}
