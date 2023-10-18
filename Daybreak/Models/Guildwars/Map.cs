using Daybreak.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

[JsonConverter(typeof(MapJsonConverter))]
public sealed class Map : IWikiEntity
{
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public string? WikiUrl { get; private set; }

    public static readonly Map None = new() { Id = 0, Name = "", WikiUrl = "https://wiki.guildwars.com/wiki/" };
    public static readonly Map GladiatorsArena = new() { Id = 1, Name = "Gladiator's Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Gladiator's_Arena" };
    public static readonly Map DEVTestArena1v1 = new() { Id = 2, Name = "DEV Test Arena (1v1)", WikiUrl = "" };
    public static readonly Map Testmap = new() { Id = 3, Name = "Test Map", WikiUrl = "" };
    public static readonly Map WarriorsIsleOutpost = new() { Id = 4, Name = "Warrior's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior's_Isle" };
    public static readonly Map HuntersIsleOutpost = new() { Id = 5, Name = "Hunter's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Hunter's_Isle" };
    public static readonly Map WizardsIsleOutpost = new() { Id = 6, Name = "Wizard's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Wizard's_Isle" };
    public static readonly Map WarriorsIsle = new() { Id = 7, Name = "Warrior's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior's_Isle" };
    public static readonly Map HuntersIsle = new() { Id = 8, Name = "Hunter's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Hunter's_Isle" };
    public static readonly Map WizardsIsle = new() { Id = 9, Name = "Wizard's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Wizard's_Isle" };
    public static readonly Map BloodstoneFen = new() { Id = 10, Name = "Bloodstone Fen", WikiUrl = "https://wiki.guildwars.com/wiki/Bloodstone_Fen" };
    public static readonly Map TheWilds = new() { Id = 11, Name = "The Wilds", WikiUrl = "https://wiki.guildwars.com/wiki/The_Wilds" };
    public static readonly Map AuroraGlade = new() { Id = 12, Name = "Aurora Glade", WikiUrl = "https://wiki.guildwars.com/wiki/Aurora_Glade" };
    public static readonly Map DiessaLowlands = new() { Id = 13, Name = "Diessa Lowlands", WikiUrl = "https://wiki.guildwars.com/wiki/Diessa_Lowlands" };
    public static readonly Map GatesOfKryta = new() { Id = 14, Name = "Gates of Kryta", WikiUrl = "https://wiki.guildwars.com/wiki/Gates_of_Kryta" };
    public static readonly Map DAlessioSeaboard = new() { Id = 15, Name = "D'Alessio Seaboard", WikiUrl = "https://wiki.guildwars.com/wiki/D'Alessio_Seaboard" };
    public static readonly Map DivinityCoast = new() { Id = 16, Name = "Divinity Coast", WikiUrl = "https://wiki.guildwars.com/wiki/Divinity_Coast" };
    public static readonly Map TalmarkWilderness = new() { Id = 17, Name = "Talmark Wilderness", WikiUrl = "https://wiki.guildwars.com/wiki/Talmark_Wilderness" };
    public static readonly Map TheBlackCurtain = new() { Id = 18, Name = "The Black Curtain", WikiUrl = "https://wiki.guildwars.com/wiki/The_Black_Curtain" };
    public static readonly Map SanctumCay = new() { Id = 19, Name = "Sanctum Cay", WikiUrl = "https://wiki.guildwars.com/wiki/Sanctum_Cay" };
    public static readonly Map DroknarsForgeOutpost = new() { Id = 20, Name = "Droknar's Forge", WikiUrl = "https://wiki.guildwars.com/wiki/Droknar's_Forge" };
    public static readonly Map TheFrostGate = new() { Id = 21, Name = "The Frost Gate", WikiUrl = "https://wiki.guildwars.com/wiki/The_Frost_Gate" };
    public static readonly Map IceCavesofSorrow = new() { Id = 22, Name = "Ice Caves of Sorrow", WikiUrl = "https://wiki.guildwars.com/wiki/Ice_Caves_of_Sorrow" };
    public static readonly Map ThunderheadKeep = new() { Id = 23, Name = "Thunderhead Keep", WikiUrl = "https://wiki.guildwars.com/wiki/Thunderhead_Keep" };
    public static readonly Map IronMinesofMoladune = new() { Id = 24, Name = "Iron Mines of Moladune", WikiUrl = "https://wiki.guildwars.com/wiki/Iron_Mines_of_Moladune" };
    public static readonly Map BorlisPass = new() { Id = 25, Name = "Borlis Pass", WikiUrl = "https://wiki.guildwars.com/wiki/Borlis_Pass" };
    public static readonly Map TalusChute = new() { Id = 26, Name = "Talus Chute", WikiUrl = "https://wiki.guildwars.com/wiki/Talus_Chute" };
    public static readonly Map GriffonsMouth = new() { Id = 27, Name = "Griffons Mouth", WikiUrl = "https://wiki.guildwars.com/wiki/Griffons_Mouth" };
    public static readonly Map TheGreatNorthernWall = new() { Id = 28, Name = "The Great Northern Wall", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Northern_Wall" };
    public static readonly Map FortRanik = new() { Id = 29, Name = "Fort Ranik", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Ranik" };
    public static readonly Map RuinsOfSurmia = new() { Id = 30, Name = "Ruins of Surmia", WikiUrl = "https://wiki.guildwars.com/wiki/Ruins_of_Surmia" };
    public static readonly Map XaquangSkyway = new() { Id = 31, Name = "Xaquang Skyway", WikiUrl = "https://wiki.guildwars.com/wiki/Xaquang_Skyway" };
    public static readonly Map NolaniAcademy = new() { Id = 32, Name = "Nolani Academy", WikiUrl = "https://wiki.guildwars.com/wiki/Nolani_Academy" };
    public static readonly Map OldAscalon = new() { Id = 33, Name = "Old Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Old_Ascalon" };
    public static readonly Map TheFissureofWoe = new() { Id = 34, Name = "The Fissure of Woe", WikiUrl = "https://wiki.guildwars.com/wiki/The_Fissure_of_Woe" };
    public static readonly Map EmberLightCampOutpost = new() { Id = 35, Name = "Ember Light Camp", WikiUrl = "https://wiki.guildwars.com/wiki/Ember_Light_Camp" };
    public static readonly Map GrendichCourthouseOutpost = new() { Id = 36, Name = "Grendich Courthouse", WikiUrl = "https://wiki.guildwars.com/wiki/Grendich_Courthouse" };
    public static readonly Map GlintsChallengeMission = new() { Id = 37, Name = "Glint's Challenge", WikiUrl = "https://wiki.guildwars.com/wiki/Glint's_Challenge" };
    public static readonly Map AuguryRockOutpost = new() { Id = 38, Name = "Augury Rock", WikiUrl = "https://wiki.guildwars.com/wiki/Augury_Rock" };
    public static readonly Map SardelacSanitariumOutpost = new() { Id = 39, Name = "Sardelac Sanitarium", WikiUrl = "https://wiki.guildwars.com/wiki/Sardelac_Sanitarium" };
    public static readonly Map PikenSquareOutpost = new() { Id = 40, Name = "Piken Square", WikiUrl = "https://wiki.guildwars.com/wiki/Piken_Square" };
    public static readonly Map SageLands = new() { Id = 41, Name = "Sage Lands", WikiUrl = "https://wiki.guildwars.com/wiki/Sage_Lands" };
    public static readonly Map MamnoonLagoon = new() { Id = 42, Name = "Mamnoon Lagoon", WikiUrl = "https://wiki.guildwars.com/wiki/Mamnoon_Lagoon" };
    public static readonly Map Silverwood = new() { Id = 43, Name = "Silverwood", WikiUrl = "https://wiki.guildwars.com/wiki/Silverwood" };
    public static readonly Map EttinsBack = new() { Id = 44, Name = "Ettin's Back", WikiUrl = "https://wiki.guildwars.com/wiki/Ettin's_Back" };
    public static readonly Map ReedBog = new() { Id = 45, Name = "Reed Bog", WikiUrl = "https://wiki.guildwars.com/wiki/Reed_Bog" };
    public static readonly Map TheFalls = new() { Id = 46, Name = "The Falls", WikiUrl = "https://wiki.guildwars.com/wiki/The_Falls" };
    public static readonly Map DryTop = new() { Id = 47, Name = "Dry Top", WikiUrl = "https://wiki.guildwars.com/wiki/Dry_Top" };
    public static readonly Map TangleRoot = new() { Id = 48, Name = "Tangle Root", WikiUrl = "https://wiki.guildwars.com/wiki/Tangle_Root" };
    public static readonly Map HengeOfDenraviOutpost = new() { Id = 49, Name = "Henge of Denravi", WikiUrl = "https://wiki.guildwars.com/wiki/Henge_of_Denravi" };
    public static readonly Map SenjisCornerOutpost = new() { Id = 50, Name = "Senji's Corner", WikiUrl = "https://wiki.guildwars.com/wiki/Senji's_Corner" };
    public static readonly Map BurningIsleOutpost = new() { Id = 52, Name = "Burning Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Burning_Isle" };
    public static readonly Map TearsOfTheFallen = new() { Id = 53, Name = "Tears of the Fallen", WikiUrl = "https://wiki.guildwars.com/wiki/Tears_of_the_Fallen" };
    public static readonly Map ScoundrelsRise = new() { Id = 54, Name = "Scoundrel's Rise", WikiUrl = "https://wiki.guildwars.com/wiki/Scoundrel's_Rise" };
    public static readonly Map LionsArchOutpost = new() { Id = 55, Name = "Lions Arch", WikiUrl = "https://wiki.guildwars.com/wiki/Lions_Arch" };
    public static readonly Map CursedLands = new() { Id = 56, Name = "Cursed Lands", WikiUrl = "https://wiki.guildwars.com/wiki/Cursed_Lands" };
    public static readonly Map BergenHotSpringsOutpost = new() { Id = 57, Name = "Bergen Hot Springs", WikiUrl = "https://wiki.guildwars.com/wiki/Bergen_Hot_Springs" };
    public static readonly Map NorthKrytaProvince = new() { Id = 58, Name = "North Kryta Province", WikiUrl = "https://wiki.guildwars.com/wiki/North_Kryta_Province" };
    public static readonly Map NeboTerrace = new() { Id = 59, Name = "Nebo Terrace", WikiUrl = "https://wiki.guildwars.com/wiki/Nebo_Terrace" };
    public static readonly Map MajestysRest = new() { Id = 60, Name = "Majesty's Rest", WikiUrl = "https://wiki.guildwars.com/wiki/Majesty's_Rest" };
    public static readonly Map TwinSerpentLakes = new() { Id = 61, Name = "Twin Serpent Lakes", WikiUrl = "https://wiki.guildwars.com/wiki/Twin_Serpent_Lakes" };
    public static readonly Map WatchtowerCoast = new() { Id = 62, Name = "Watchtower Coast", WikiUrl = "https://wiki.guildwars.com/wiki/Watchtower_Coast" };
    public static readonly Map StingrayStrand = new() { Id = 63, Name = "Stingray Strand", WikiUrl = "https://wiki.guildwars.com/wiki/Stingray_Strand" };
    public static readonly Map KessexPeak = new() { Id = 64, Name = "Kessex Peak", WikiUrl = "https://wiki.guildwars.com/wiki/Kessex_Peak" };
    public static readonly Map DAlessioArenaMission = new() { Id = 65, Name = "D'Alessio Arena", WikiUrl = "https://wiki.guildwars.com/wiki/D'Alessio_Arena" };
    public static readonly Map BurningIsle = new() { Id = 66, Name = "Burning Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Burning_Isle" };
    public static readonly Map FrozenIsle = new() { Id = 68, Name = "Frozen Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Frozen_Isle" };
    public static readonly Map NomadsIsle = new() { Id = 69, Name = "Nomads Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Nomads_Isle" };
    public static readonly Map DruidsIsle = new() { Id = 70, Name = "Druids Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Druids_Isle" };
    public static readonly Map IsleOfTheDeadGuildHall = new() { Id = 71, Name = "Isle of the Dead", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_the_Dead" };
    public static readonly Map TheUnderworld = new() { Id = 72, Name = "The Underworld", WikiUrl = "https://wiki.guildwars.com/wiki/The_Underworld" };
    public static readonly Map RiversideProvince = new() { Id = 73, Name = "Riverside Province", WikiUrl = "https://wiki.guildwars.com/wiki/Riverside_Province" };
    public static readonly Map TheHallOfHeroesArenaMission = new() { Id = 74, Name = "The Hall of Heroes", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hall_of_Heroes" };
    public static readonly Map BrokenTowerMission = new() { Id = 76, Name = "Broken Tower", WikiUrl = "https://wiki.guildwars.com/wiki/Broken_Tower" };
    public static readonly Map HouseZuHeltzerOutpost = new() { Id = 77, Name = "House zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/House_zu_Heltzer" };
    public static readonly Map TheCourtyardArenaMission = new() { Id = 78, Name = "The Courtyard", WikiUrl = "https://wiki.guildwars.com/wiki/The_Courtyard" };
    public static readonly Map UnholyTemplesMission = new() { Id = 79, Name = "Unholy Temples", WikiUrl = "https://wiki.guildwars.com/wiki/Unholy_Temples" };
    public static readonly Map BurialMoundsMission = new() { Id = 80, Name = "Burial Mounds", WikiUrl = "https://wiki.guildwars.com/wiki/Burial_Mounds" };
    public static readonly Map AscalonCityOutpost = new() { Id = 81, Name = "Ascalon City", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_City" };
    public static readonly Map TombOfThePrimevalKings = new() { Id = 82, Name = "Tomb of the Primeval Kings", WikiUrl = "https://wiki.guildwars.com/wiki/Tomb_of_the_Primeval_Kings" };
    public static readonly Map TheVaultMission = new() { Id = 83, Name = "The Vault", WikiUrl = "https://wiki.guildwars.com/wiki/The_Vault" };
    public static readonly Map TheUnderworldArenaMission = new() { Id = 84, Name = "The Underworld", WikiUrl = "https://wiki.guildwars.com/wiki/The_Underworld" };
    public static readonly Map AscalonArena = new() { Id = 85, Name = "Ascalon Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_Arena" };
    public static readonly Map SacredTemplesMission = new() { Id = 86, Name = "Sacred Temples", WikiUrl = "https://wiki.guildwars.com/wiki/Sacred_Temples" };
    public static readonly Map Icedome = new() { Id = 87, Name = "Icedome", WikiUrl = "https://wiki.guildwars.com/wiki/Icedome" };
    public static readonly Map IronHorseMine = new() { Id = 88, Name = "Iron Horse Mine", WikiUrl = "https://wiki.guildwars.com/wiki/Iron_Horse_Mine" };
    public static readonly Map AnvilRock = new() { Id = 89, Name = "Anvil Rock", WikiUrl = "https://wiki.guildwars.com/wiki/Anvil_Rock" };
    public static readonly Map LornarsPass = new() { Id = 90, Name = "Lornar's Pass", WikiUrl = "https://wiki.guildwars.com/wiki/Lornar's_Pass" };
    public static readonly Map SnakeDance = new() { Id = 91, Name = "Snake Dance", WikiUrl = "https://wiki.guildwars.com/wiki/Snake_Dance" };
    public static readonly Map TascasDemise = new() { Id = 92, Name = "Tasca's Demise", WikiUrl = "https://wiki.guildwars.com/wiki/Tasca's_Demise" };
    public static readonly Map SpearheadPeak = new() { Id = 93, Name = "Spearhead Peak", WikiUrl = "https://wiki.guildwars.com/wiki/Spearhead_Peak" };
    public static readonly Map IceFloe = new() { Id = 94, Name = "Ice Floe", WikiUrl = "https://wiki.guildwars.com/wiki/Ice_Floe" };
    public static readonly Map WitmansFolly = new() { Id = 95, Name = "Witman's Folly", WikiUrl = "https://wiki.guildwars.com/wiki/Witman's_Folly" };
    public static readonly Map MineralSprings = new() { Id = 96, Name = "Mineral Springs", WikiUrl = "https://wiki.guildwars.com/wiki/Mineral_Springs" };
    public static readonly Map DreadnoughtsDrift = new() { Id = 97, Name = "Dreadnought's Drift", WikiUrl = "https://wiki.guildwars.com/wiki/Dreadnought's_Drift" };
    public static readonly Map FrozenForest = new() { Id = 98, Name = "Frozen Forest", WikiUrl = "https://wiki.guildwars.com/wiki/Frozen_Forest" };
    public static readonly Map TravelersVale = new() { Id = 99, Name = "Traveler's Vale", WikiUrl = "https://wiki.guildwars.com/wiki/Traveler's_Vale" };
    public static readonly Map DeldrimorBowl = new() { Id = 100, Name = "Deldrimor Bowl", WikiUrl = "https://wiki.guildwars.com/wiki/Deldrimor_Bowl" };
    public static readonly Map RegentValley = new() { Id = 101, Name = "Regent Valley", WikiUrl = "https://wiki.guildwars.com/wiki/Regent_Valley" };
    public static readonly Map TheBreach = new() { Id = 102, Name = "The Breach", WikiUrl = "https://wiki.guildwars.com/wiki/The_Breach" };
    public static readonly Map AscalonFoothills = new() { Id = 103, Name = "Ascalon Foothills", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_Foothills" };
    public static readonly Map PockmarkFlats = new() { Id = 104, Name = "Pockmark Flats", WikiUrl = "https://wiki.guildwars.com/wiki/Pockmark_Flats" };
    public static readonly Map DragonsGullet = new() { Id = 105, Name = "Dragon's Gullet", WikiUrl = "https://wiki.guildwars.com/wiki/Dragon's_Gullet" };
    public static readonly Map FlameTempleCorridor = new() { Id = 106, Name = "Flame Temple Corridor", WikiUrl = "https://wiki.guildwars.com/wiki/Flame_Temple_Corridor" };
    public static readonly Map EasternFrontier = new() { Id = 107, Name = "Eastern Frontier", WikiUrl = "https://wiki.guildwars.com/wiki/Eastern_Frontier" };
    public static readonly Map TheScar = new() { Id = 108, Name = "The Scar", WikiUrl = "https://wiki.guildwars.com/wiki/The_Scar" };
    public static readonly Map TheAmnoonOasisOutpost = new() { Id = 109, Name = "The Amnoon Oasis", WikiUrl = "https://wiki.guildwars.com/wiki/The_Amnoon_Oasis" };
    public static readonly Map DivinersAscent = new() { Id = 110, Name = "Diviner's Ascent", WikiUrl = "https://wiki.guildwars.com/wiki/Diviner's_Ascent" };
    public static readonly Map VultureDrifts = new() { Id = 111, Name = "Vulture Drifts", WikiUrl = "https://wiki.guildwars.com/wiki/Vulture_Drifts" };
    public static readonly Map TheAridSea = new() { Id = 112, Name = "The Arid Sea", WikiUrl = "https://wiki.guildwars.com/wiki/The_Arid_Sea" };
    public static readonly Map ProphetsPath = new() { Id = 113, Name = "Prophet's Path", WikiUrl = "https://wiki.guildwars.com/wiki/Prophet's_Path" };
    public static readonly Map SaltFlats = new() { Id = 114, Name = "Salt Flats", WikiUrl = "https://wiki.guildwars.com/wiki/Salt_Flats" };
    public static readonly Map SkywardReach = new() { Id = 115, Name = "Skyward Reach", WikiUrl = "https://wiki.guildwars.com/wiki/Skyward_Reach" };
    public static readonly Map DunesOfDespair = new() { Id = 116, Name = "Dunes of Despair", WikiUrl = "https://wiki.guildwars.com/wiki/Dunes_of_Despair" };
    public static readonly Map ThirstyRiver = new() { Id = 117, Name = "Thirsty River", WikiUrl = "https://wiki.guildwars.com/wiki/Thirsty_River" };
    public static readonly Map ElonaReach = new() { Id = 118, Name = "Elona Reach", WikiUrl = "https://wiki.guildwars.com/wiki/Elona_Reach" };
    public static readonly Map AuguryRockMission = new() { Id = 119, Name = "Augury Rock", WikiUrl = "https://wiki.guildwars.com/wiki/Augury_Rock" };
    public static readonly Map TheDragonsLair = new() { Id = 120, Name = "The Dragons Lair", WikiUrl = "https://wiki.guildwars.com/wiki/The_Dragons_Lair" };
    public static readonly Map PerditionRock = new() { Id = 121, Name = "Perdition Rock", WikiUrl = "https://wiki.guildwars.com/wiki/Perdition_Rock" };
    public static readonly Map RingOfFire = new() { Id = 122, Name = "Ring of Fire", WikiUrl = "https://wiki.guildwars.com/wiki/Ring_of_Fire" };
    public static readonly Map AbaddonsMouth = new() { Id = 123, Name = "Abaddons Mouth", WikiUrl = "https://wiki.guildwars.com/wiki/Abaddons_Mouth" };
    public static readonly Map HellsPrecipice = new() { Id = 124, Name = "Hell's Precipice", WikiUrl = "https://wiki.guildwars.com/wiki/Hell's_Precipice" };
    public static readonly Map GoldenGatesMission = new() { Id = 125, Name = "Golden Gates", WikiUrl = "https://wiki.guildwars.com/wiki/Golden_Gates" };
    public static readonly Map ScarredEarth2 = new() { Id = 127, Name = "Scarred Earth", WikiUrl = "https://wiki.guildwars.com/wiki/Scarred_Earth" };
    public static readonly Map TheEternalGrove = new() { Id = 128, Name = "The Eternal Grove", WikiUrl = "https://wiki.guildwars.com/wiki/The_Eternal_Grove" };
    public static readonly Map LutgardisConservatoryOutpost = new() { Id = 129, Name = "Lutgardis Conservatory", WikiUrl = "https://wiki.guildwars.com/wiki/Lutgardis_Conservatory" };
    public static readonly Map VasburgArmoryOutpost = new() { Id = 130, Name = "Vasburg Armory", WikiUrl = "https://wiki.guildwars.com/wiki/Vasburg_Armory" };
    public static readonly Map SerenityTempleOutpost = new() { Id = 131, Name = "Serenity Temple", WikiUrl = "https://wiki.guildwars.com/wiki/Serenity_Temple" };
    public static readonly Map IceToothCaveOutpost = new() { Id = 132, Name = "Ice Tooth Cave", WikiUrl = "https://wiki.guildwars.com/wiki/Ice_Tooth_Cave" };
    public static readonly Map BeaconsPerchOutpost = new() { Id = 133, Name = "Beacon's Perch", WikiUrl = "https://wiki.guildwars.com/wiki/Beacon's_Perch" };
    public static readonly Map YaksBendOutpost = new() { Id = 134, Name = "Yaks Bend", WikiUrl = "https://wiki.guildwars.com/wiki/Yaks_Bend" };
    public static readonly Map FrontierGateOutpost = new() { Id = 135, Name = "Frontier Gate", WikiUrl = "https://wiki.guildwars.com/wiki/Frontier_Gate" };
    public static readonly Map BeetletunOutpost = new() { Id = 136, Name = "Beetletun", WikiUrl = "https://wiki.guildwars.com/wiki/Beetletun" };
    public static readonly Map FishermensHavenOutpost = new() { Id = 137, Name = "Fishermen's Haven", WikiUrl = "https://wiki.guildwars.com/wiki/Fishermen's_Haven" };
    public static readonly Map TempleOfTheAges = new() { Id = 138, Name = "Temple of the Ages", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_of_the_Ages" };
    public static readonly Map VentarisRefugeOutpost = new() { Id = 139, Name = "Ventari's Refuge", WikiUrl = "https://wiki.guildwars.com/wiki/Ventari's_Refuge" };
    public static readonly Map DruidsOverlookOutpost = new() { Id = 140, Name = "Druid's Overlook", WikiUrl = "https://wiki.guildwars.com/wiki/Druid's_Overlook" };
    public static readonly Map MaguumaStadeOutpost = new() { Id = 141, Name = "Maguuma Stade", WikiUrl = "https://wiki.guildwars.com/wiki/Maguuma_Stade" };
    public static readonly Map QuarrelFallsOutpost = new() { Id = 142, Name = "Quarrel Falls", WikiUrl = "https://wiki.guildwars.com/wiki/Quarrel_Falls" };
    public static readonly Map GyalaHatchery = new() { Id = 143, Name = "Gyala Hatchery", WikiUrl = "https://wiki.guildwars.com/wiki/Gyala_Hatchery" };
    public static readonly Map TheCatacombs = new() { Id = 145, Name = "The Catacombs", WikiUrl = "https://wiki.guildwars.com/wiki/The_Catacombs" };
    public static readonly Map LakesideCounty = new() { Id = 146, Name = "Lakeside County", WikiUrl = "https://wiki.guildwars.com/wiki/Lakeside_County" };
    public static readonly Map TheNorthlands = new() { Id = 147, Name = "The Northlands", WikiUrl = "https://wiki.guildwars.com/wiki/The_Northlands" };
    public static readonly Map AscalonCityPresearing = new() { Id = 148, Name = "Ascalon City", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_City" };
    public static readonly Map AscalonAcademy = new() { Id = 149, Name = "Ascalon Academy", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_Academy" };
    public static readonly Map AscalonAcademyPvPBattleMission = new() { Id = 150, Name = "Ascalon Academy", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_Academy" };
    public static readonly Map AscalonAcademyunk = new() { Id = 151, Name = "Ascalon Academy", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_Academy" };
    public static readonly Map HeroesAudienceOutpost = new() { Id = 152, Name = "Heroes' Audience", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes'_Audience" };
    public static readonly Map SeekersPassageOutpost = new() { Id = 153, Name = "Seeker's Passage", WikiUrl = "https://wiki.guildwars.com/wiki/Seeker's_Passage" };
    public static readonly Map DestinysGorgeOutpost = new() { Id = 154, Name = "Destiny's Gorge", WikiUrl = "https://wiki.guildwars.com/wiki/Destiny's_Gorge" };
    public static readonly Map CampRankorOutpost = new() { Id = 155, Name = "Camp Rankor", WikiUrl = "https://wiki.guildwars.com/wiki/Camp_Rankor" };
    public static readonly Map TheGraniteCitadelOutpost = new() { Id = 156, Name = "The Granite Citadel", WikiUrl = "https://wiki.guildwars.com/wiki/The_Granite_Citadel" };
    public static readonly Map MarhansGrottoOutpost = new() { Id = 157, Name = "Marhan's Grotto", WikiUrl = "https://wiki.guildwars.com/wiki/Marhan's_Grotto" };
    public static readonly Map PortSledgeOutpost = new() { Id = 158, Name = "Port Sledge", WikiUrl = "https://wiki.guildwars.com/wiki/Port_Sledge" };
    public static readonly Map CopperhammerMinesOutpost = new() { Id = 159, Name = "Copperhammer Mines", WikiUrl = "https://wiki.guildwars.com/wiki/Copperhammer_Mines" };
    public static readonly Map GreenHillsCounty = new() { Id = 160, Name = "Green Hills County", WikiUrl = "https://wiki.guildwars.com/wiki/Green_Hills_County" };
    public static readonly Map WizardsFolly = new() { Id = 161, Name = "Wizard's Folly", WikiUrl = "https://wiki.guildwars.com/wiki/Wizard's_Folly" };
    public static readonly Map RegentValleyPreSearing = new() { Id = 162, Name = "Regent Valley", WikiUrl = "https://wiki.guildwars.com/wiki/Regent_Valley" };
    public static readonly Map TheBarradinEstateOutpost = new() { Id = 163, Name = "The Barradin Estate", WikiUrl = "https://wiki.guildwars.com/wiki/The_Barradin_Estate" };
    public static readonly Map AshfordAbbeyOutpost = new() { Id = 164, Name = "Ashford Abbey", WikiUrl = "https://wiki.guildwars.com/wiki/Ashford_Abbey" };
    public static readonly Map FoiblesFairOutpost = new() { Id = 165, Name = "Foible's Fair", WikiUrl = "https://wiki.guildwars.com/wiki/Foible's_Fair" };
    public static readonly Map FortRanikPreSearingOutpost = new() { Id = 166, Name = "Fort Ranik", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Ranik" };
    public static readonly Map BurningIsleMission = new() { Id = 167, Name = "Burning Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Burning_Isle" };
    public static readonly Map DruidsIsleMission = new() { Id = 168, Name = "Druids Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Druids_Isle" };
    public static readonly Map FrozenIsleMission = new() { Id = 169, Name = "Frozen Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Frozen_Isle" };
    public static readonly Map WarriorsIsleMission = new() { Id = 171, Name = "Warrior's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior's_Isle" };
    public static readonly Map HuntersIsleMission = new() { Id = 172, Name = "Hunter's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Hunter's_Isle" };
    public static readonly Map WizardsIsleMission = new() { Id = 173, Name = "Wizard's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Wizard's_Isle" };
    public static readonly Map NomadsIsleMission = new() { Id = 174, Name = "Nomad's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Nomad's_Isle" };
    public static readonly Map IsleOfTheDeadGuildHallMission = new() { Id = 175, Name = "Isle of the Dead", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_the_Dead" };
    public static readonly Map FrozenIsleOutpost = new() { Id = 176, Name = "Frozen Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Frozen_Isle" };
    public static readonly Map NomadsIsleOutpost = new() { Id = 177, Name = "Nomad's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Nomad's_Isle" };
    public static readonly Map DruidsIsleOutpost = new() { Id = 178, Name = "Druid's Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Druid's_Isle" };
    public static readonly Map IsleOfTheDeadGuildHallOutpost = new() { Id = 179, Name = "Isle of the Dead", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_the_Dead" };
    public static readonly Map FortKogaMission = new() { Id = 180, Name = "Fort Koga", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Koga" };
    public static readonly Map ShiverpeakArena = new() { Id = 181, Name = "Shiverpeak Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Shiverpeak_Arena" };
    public static readonly Map AmnoonArenaMission = new() { Id = 182, Name = "Amnoon Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Amnoon_Arena" };
    public static readonly Map DeldrimorArenaMission = new() { Id = 183, Name = "Deldrimor Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Deldrimor_Arena" };
    public static readonly Map TheCragMission = new() { Id = 184, Name = "The Crag", WikiUrl = "https://wiki.guildwars.com/wiki/The_Crag" };
    public static readonly Map RandomArenasOutpost = new() { Id = 185, Name = "Random Arenas", WikiUrl = "https://wiki.guildwars.com/wiki/Random_Arenas" };
    public static readonly Map TeamArenasOutpost = new() { Id = 189, Name = "Team Arenas", WikiUrl = "https://wiki.guildwars.com/wiki/Team_Arenas" };
    public static readonly Map SorrowsFurnace = new() { Id = 190, Name = "Sorrow's Furnace", WikiUrl = "https://wiki.guildwars.com/wiki/Sorrow's_Furnace" };
    public static readonly Map GrenthsFootprint = new() { Id = 191, Name = "Grenth's Footprint", WikiUrl = "https://wiki.guildwars.com/wiki/Grenth's_Footprint" };
    public static readonly Map CavalonOutpost = new() { Id = 192, Name = "Cavalon", WikiUrl = "https://wiki.guildwars.com/wiki/Cavalon" };
    public static readonly Map KainengCenterOutpost = new() { Id = 194, Name = "Kaineng Center", WikiUrl = "https://wiki.guildwars.com/wiki/Kaineng_Center" };
    public static readonly Map DrazachThicket = new() { Id = 195, Name = "Drazach Thicket", WikiUrl = "https://wiki.guildwars.com/wiki/Drazach_Thicket" };
    public static readonly Map JayaBluffs = new() { Id = 196, Name = "Jaya Bluffs", WikiUrl = "https://wiki.guildwars.com/wiki/Jaya_Bluffs" };
    public static readonly Map ShenzunTunnels = new() { Id = 197, Name = "Shenzun Tunnels", WikiUrl = "https://wiki.guildwars.com/wiki/Shenzun_Tunnels" };
    public static readonly Map Archipelagos = new() { Id = 198, Name = "Archipelagos", WikiUrl = "https://wiki.guildwars.com/wiki/Archipelagos" };
    public static readonly Map MaishangHills = new() { Id = 199, Name = "Maishang Hills", WikiUrl = "https://wiki.guildwars.com/wiki/Maishang_Hills" };
    public static readonly Map MountQinkai = new() { Id = 200, Name = "Mount Qinkai", WikiUrl = "https://wiki.guildwars.com/wiki/Mount_Qinkai" };
    public static readonly Map MelandrusHope = new() { Id = 201, Name = "Melandru's Hope", WikiUrl = "https://wiki.guildwars.com/wiki/Melandru's_Hope" };
    public static readonly Map RheasCrater = new() { Id = 202, Name = "Rheas Crater", WikiUrl = "https://wiki.guildwars.com/wiki/Rheas_Crater" };
    public static readonly Map SilentSurf = new() { Id = 203, Name = "Silent Surf", WikiUrl = "https://wiki.guildwars.com/wiki/Silent_Surf" };
    public static readonly Map UnwakingWatersMission = new() { Id = 204, Name = "Unwaking Waters", WikiUrl = "https://wiki.guildwars.com/wiki/Unwaking_Waters" };
    public static readonly Map MorostavTrail = new() { Id = 205, Name = "Morostav Trail", WikiUrl = "https://wiki.guildwars.com/wiki/Morostav_Trail" };
    public static readonly Map DeldrimorWarCampOutpost = new() { Id = 206, Name = "Deldrimor War Camp", WikiUrl = "https://wiki.guildwars.com/wiki/Deldrimor_War_Camp" };
    public static readonly Map HeroesCryptMission = new() { Id = 207, Name = "Heroes' Crypt", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes'_Crypt" };
    public static readonly Map MourningVeilFalls = new() { Id = 209, Name = "Mourning Veil Falls", WikiUrl = "https://wiki.guildwars.com/wiki/Mourning_Veil_Falls" };
    public static readonly Map Ferndale = new() { Id = 210, Name = "Ferndale", WikiUrl = "https://wiki.guildwars.com/wiki/Ferndale" };
    public static readonly Map PongmeiValley = new() { Id = 211, Name = "Pongmei Valley", WikiUrl = "https://wiki.guildwars.com/wiki/Pongmei_Valley" };
    public static readonly Map MonasteryOverlook1 = new() { Id = 212, Name = "Monastery Overlook", WikiUrl = "https://wiki.guildwars.com/wiki/Monastery_Overlook" };
    public static readonly Map ZenDaijunOutpostMission = new() { Id = 213, Name = "Zen Daijun", WikiUrl = "https://wiki.guildwars.com/wiki/Zen_Daijun" };
    public static readonly Map MinisterChosEstateOutpostMission = new() { Id = 214, Name = "Minister Cho's Estate", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Cho's_Estate" };
    public static readonly Map VizunahSquareMission = new() { Id = 215, Name = "Vizunah Square", WikiUrl = "https://wiki.guildwars.com/wiki/Vizunah_Square" };
    public static readonly Map NahpuiQuarterOutpostMission = new() { Id = 216, Name = "Nahpui Quarter", WikiUrl = "https://wiki.guildwars.com/wiki/Nahpui_Quarter" };
    public static readonly Map TahnnakaiTempleOutpostMission = new() { Id = 217, Name = "Tahnnakai Temple", WikiUrl = "https://wiki.guildwars.com/wiki/Tahnnakai_Temple" };
    public static readonly Map ArborstoneOutpostMission = new() { Id = 218, Name = "Arborstone", WikiUrl = "https://wiki.guildwars.com/wiki/Arborstone" };
    public static readonly Map BoreasSeabedOutpostMission = new() { Id = 219, Name = "Boreas Seabed", WikiUrl = "https://wiki.guildwars.com/wiki/Boreas_Seabed" };
    public static readonly Map SunjiangDistrictOutpostMission = new() { Id = 220, Name = "Sunjiang District", WikiUrl = "https://wiki.guildwars.com/wiki/Sunjiang_District" };
    public static readonly Map FortAspenwoodMission = new() { Id = 221, Name = "Fort Aspenwood", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Aspenwood" };
    public static readonly Map TheEternalGroveOutpostMission = new() { Id = 222, Name = "The Eternal Grove", WikiUrl = "https://wiki.guildwars.com/wiki/The_Eternal_Grove" };
    public static readonly Map TheJadeQuarryMission = new() { Id = 223, Name = "The Jade Quarry", WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Quarry" };
    public static readonly Map GyalaHatcheryOutpostMission = new() { Id = 224, Name = "Gyala Hatchery", WikiUrl = "https://wiki.guildwars.com/wiki/Gyala_Hatchery" };
    public static readonly Map RaisuPalaceOutpostMission = new() { Id = 225, Name = "Raisu Palace", WikiUrl = "https://wiki.guildwars.com/wiki/Raisu_Palace" };
    public static readonly Map ImperialSanctumOutpostMission = new() { Id = 226, Name = "Imperial Sanctum", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Sanctum" };
    public static readonly Map UnwakingWaters = new() { Id = 227, Name = "Unwaking Waters", WikiUrl = "https://wiki.guildwars.com/wiki/Unwaking_Waters" };
    public static readonly Map GrenzFrontierMission = new() { Id = 228, Name = "Grenz Frontier", WikiUrl = "https://wiki.guildwars.com/wiki/Grenz_Frontier" };
    public static readonly Map AmatzBasin = new() { Id = 229, Name = "Amatz Basin", WikiUrl = "https://wiki.guildwars.com/wiki/Amatz_Basin" };
    public static readonly Map ShadowsPassage = new() { Id = 232, Name = "Shadow's Passage", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow's_Passage" };
    public static readonly Map RaisuPalace = new() { Id = 233, Name = "Raisu Palace", WikiUrl = "https://wiki.guildwars.com/wiki/Raisu_Palace" };
    public static readonly Map TheAuriosMines = new() { Id = 234, Name = "The Aurios Mines", WikiUrl = "https://wiki.guildwars.com/wiki/The_Aurios_Mines" };
    public static readonly Map PanjiangPeninsula = new() { Id = 235, Name = "Panjiang Peninsula", WikiUrl = "https://wiki.guildwars.com/wiki/Panjiang_Peninsula" };
    public static readonly Map KinyaProvince = new() { Id = 236, Name = "Kinya Province", WikiUrl = "https://wiki.guildwars.com/wiki/Kinya_Province" };
    public static readonly Map HaijuLagoon = new() { Id = 237, Name = "Haiju Lagoon", WikiUrl = "https://wiki.guildwars.com/wiki/Haiju_Lagoon" };
    public static readonly Map SunquaVale = new() { Id = 238, Name = "Sunqua Vale", WikiUrl = "https://wiki.guildwars.com/wiki/Sunqua_Vale" };
    public static readonly Map WajjunBazaar = new() { Id = 239, Name = "Wajjun Bazaar", WikiUrl = "https://wiki.guildwars.com/wiki/Wajjun_Bazaar" };
    public static readonly Map BukdekByway = new() { Id = 240, Name = "Bukdek Byway", WikiUrl = "https://wiki.guildwars.com/wiki/Bukdek_Byway" };
    public static readonly Map TheUndercity = new() { Id = 241, Name = "The Undercity", WikiUrl = "https://wiki.guildwars.com/wiki/The_Undercity" };
    public static readonly Map ShingJeaMonasteryOutpost = new() { Id = 242, Name = "Shing Jea Monastery", WikiUrl = "https://wiki.guildwars.com/wiki/Shing_Jea_Monastery" };
    public static readonly Map ShingJeaArena = new() { Id = 243, Name = "Shing Jea Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Shing_Jea_Arena" };
    public static readonly Map ArborstoneExplorable = new() { Id = 244, Name = "Arborstone", WikiUrl = "https://wiki.guildwars.com/wiki/Arborstone" };
    public static readonly Map MinisterChosEstateExplorable = new() { Id = 245, Name = "Minister Cho's Estate", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Cho's_Estate" };
    public static readonly Map ZenDaijunExplorable = new() { Id = 246, Name = "Zen Daijun", WikiUrl = "https://wiki.guildwars.com/wiki/Zen_Daijun" };
    public static readonly Map BoreasSeabedExplorable = new() { Id = 247, Name = "Boreas Seabed", WikiUrl = "https://wiki.guildwars.com/wiki/Boreas_Seabed" };
    public static readonly Map GreatTempleOfBalthazarOutpost = new() { Id = 248, Name = "Great Temple of Balthazar", WikiUrl = "https://wiki.guildwars.com/wiki/Great_Temple_of_Balthazar" };
    public static readonly Map TsumeiVillageOutpost = new() { Id = 249, Name = "Tsumei Village", WikiUrl = "https://wiki.guildwars.com/wiki/Tsumei_Village" };
    public static readonly Map SeitungHarborOutpost = new() { Id = 250, Name = "Seitung Harbor", WikiUrl = "https://wiki.guildwars.com/wiki/Seitung_Harbor" };
    public static readonly Map RanMusuGardensOutpost = new() { Id = 251, Name = "Ran Musu Gardens", WikiUrl = "https://wiki.guildwars.com/wiki/Ran_Musu_Gardens" };
    public static readonly Map LinnokCourtyard = new() { Id = 252, Name = "Linnok Courtyard", WikiUrl = "https://wiki.guildwars.com/wiki/Linnok_Courtyard" };
    public static readonly Map DwaynaVsGrenth = new() { Id = 253, Name = "Dwayna Vs Grenth", WikiUrl = "https://wiki.guildwars.com/wiki/Dwayna_Vs_Grenth" };
    public static readonly Map SunjiangDistrictExplorable = new() { Id = 254, Name = "Sunjiang District", WikiUrl = "https://wiki.guildwars.com/wiki/Sunjiang_District" };
    public static readonly Map NahpuiQuarterExplorable = new() { Id = 257, Name = "Nahpui Quarter", WikiUrl = "https://wiki.guildwars.com/wiki/Nahpui_Quarter" };
    public static readonly Map UrgozsWarren = new() { Id = 266, Name = "Urgozs Warren", WikiUrl = "https://wiki.guildwars.com/wiki/Urgozs_Warren" };
    public static readonly Map TahnnakaiTempleExplorable = new() { Id = 267, Name = "Tahnnakai Temple", WikiUrl = "https://wiki.guildwars.com/wiki/Tahnnakai_Temple" };
    public static readonly Map AltrummRuins = new() { Id = 270, Name = "Altrumm Ruins", WikiUrl = "https://wiki.guildwars.com/wiki/Altrumm_Ruins" };
    public static readonly Map ZosShivrosChannel = new() { Id = 273, Name = "Zos Shivros Channel", WikiUrl = "https://wiki.guildwars.com/wiki/Zos_Shivros_Channel" };
    public static readonly Map DragonsThroat = new() { Id = 274, Name = "Dragon's Throat", WikiUrl = "https://wiki.guildwars.com/wiki/Dragon's_Throat" };
    public static readonly Map IsleOfWeepingStoneOutpost = new() { Id = 275, Name = "Isle of Weeping Stone", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Weeping_Stone" };
    public static readonly Map IsleOfJadeOutpost = new() { Id = 276, Name = "Isle of Jade", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Jade" };
    public static readonly Map HarvestTempleOutpost = new() { Id = 277, Name = "Harvest Temple", WikiUrl = "https://wiki.guildwars.com/wiki/Harvest_Temple" };
    public static readonly Map BreakerHollowOutpost = new() { Id = 278, Name = "Breaker Hollow", WikiUrl = "https://wiki.guildwars.com/wiki/Breaker_Hollow" };
    public static readonly Map LeviathanPitsOutpost = new() { Id = 279, Name = "Leviathan Pits", WikiUrl = "https://wiki.guildwars.com/wiki/Leviathan_Pits" };
    public static readonly Map IsleOfTheNameless = new() { Id = 280, Name = "Isle of the Nameless", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_the_Nameless" };
    public static readonly Map ZaishenChallengeOutpost = new() { Id = 281, Name = "Zaishen Challenge", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_Challenge" };
    public static readonly Map ZaishenEliteOutpost = new() { Id = 282, Name = "Zaishen Elite", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_Elite" };
    public static readonly Map MaatuKeepOutpost = new() { Id = 283, Name = "Maatu Keep", WikiUrl = "https://wiki.guildwars.com/wiki/Maatu_Keep" };
    public static readonly Map ZinKuCorridorOutpost = new() { Id = 284, Name = "Zin Ku Corridor", WikiUrl = "https://wiki.guildwars.com/wiki/Zin_Ku_Corridor" };
    public static readonly Map MonasteryOverlook2 = new() { Id = 285, Name = "Monastery Overlook", WikiUrl = "https://wiki.guildwars.com/wiki/Monastery_Overlook" };
    public static readonly Map BrauerAcademyOutpost = new() { Id = 286, Name = "Brauer Academy", WikiUrl = "https://wiki.guildwars.com/wiki/Brauer_Academy" };
    public static readonly Map DurheimArchivesOutpost = new() { Id = 287, Name = "Durheim Archives", WikiUrl = "https://wiki.guildwars.com/wiki/Durheim_Archives" };
    public static readonly Map BaiPaasuReachOutpost = new() { Id = 288, Name = "Bai Paasu Reach", WikiUrl = "https://wiki.guildwars.com/wiki/Bai_Paasu_Reach" };
    public static readonly Map SeafarersRestOutpost = new() { Id = 289, Name = "Seafarer's Rest", WikiUrl = "https://wiki.guildwars.com/wiki/Seafarer's_Rest" };
    public static readonly Map BejunkanPier = new() { Id = 290, Name = "Bejunkan Pier", WikiUrl = "https://wiki.guildwars.com/wiki/Bejunkan_Pier" };
    public static readonly Map VizunahSquareLocalQuarterOutpost = new() { Id = 291, Name = "Vizunah Square (Local Quarter)", WikiUrl = "https://wiki.guildwars.com/wiki/Vizunah_Square_(Local_Quarter)" };
    public static readonly Map VizunahSquareForeignQuarterOutpost = new() { Id = 292, Name = "Vizunah Square (Foreign Quarter)", WikiUrl = "https://wiki.guildwars.com/wiki/Vizunah_Square_(Foreign_Quarter)" };
    public static readonly Map FortAspenwoodLuxonOutpost = new() { Id = 293, Name = "Fort Aspenwood (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Aspenwood_(Luxon)" };
    public static readonly Map FortAspenwoodKurzickOutpost = new() { Id = 294, Name = "Fort Aspenwood (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Aspenwood_(Kurzick)" };
    public static readonly Map TheJadeQuarryLuxonOutpost = new() { Id = 295, Name = "The Jade Quarry (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Quarry_(Luxon)" };
    public static readonly Map TheJadeQuarryKurzickOutpost = new() { Id = 296, Name = "The Jade Quarry (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Quarry_(Kurzick)" };
    public static readonly Map UnwakingWatersLuxonOutpost = new() { Id = 297, Name = "Unwaking Waters (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/Unwaking_Waters_(Luxon)" };
    public static readonly Map UnwakingWatersKurzickOutpost = new() { Id = 298, Name = "Unwaking Waters (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/Unwaking_Waters_(Kurzick)" };
    public static readonly Map EtnaranKeysMission = new() { Id = 299, Name = "Etnaran Keys", WikiUrl = "https://wiki.guildwars.com/wiki/Etnaran_Keys" };
    public static readonly Map RaisuPavilion = new() { Id = 301, Name = "Raisu Pavilion", WikiUrl = "https://wiki.guildwars.com/wiki/Raisu_Pavilion" };
    public static readonly Map KainengDocks = new() { Id = 302, Name = "Kaineng Docks", WikiUrl = "https://wiki.guildwars.com/wiki/Kaineng_Docks" };
    public static readonly Map TheMarketplaceOutpost = new() { Id = 303, Name = "The Marketplace", WikiUrl = "https://wiki.guildwars.com/wiki/The_Marketplace" };
    public static readonly Map TheDeep = new() { Id = 304, Name = "The Deep", WikiUrl = "https://wiki.guildwars.com/wiki/The_Deep" };
    public static readonly Map AscalonArenaMission = new() { Id = 308, Name = "Ascalon Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_Arena" };
    public static readonly Map AnnihilationMission = new() { Id = 309, Name = "Annihilation Training", WikiUrl = "https://wiki.guildwars.com/wiki/Training_Arena" };
    public static readonly Map KillCountTrainingMission = new() { Id = 310, Name = "Kill Count Training", WikiUrl = "https://wiki.guildwars.com/wiki/Training_Arena" };
    public static readonly Map PriestAnnihilationTraining = new() { Id = 311, Name = "Priest Annihilation Training", WikiUrl = "https://wiki.guildwars.com/wiki/Priest_Annihilation_Training" };
    public static readonly Map ObeliskAnnihilationTrainingMission = new() { Id = 312, Name = "Obelisk Annihilation Training", WikiUrl = "https://wiki.guildwars.com/wiki/Training_Arena" };
    public static readonly Map SaoshangTrail = new() { Id = 313, Name = "Saoshang Trail", WikiUrl = "https://wiki.guildwars.com/wiki/Saoshang_Trail" };
    public static readonly Map ShiverpeakArenaMission = new() { Id = 314, Name = "Shiverpeak Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Shiverpeak_Arena" };
    public static readonly Map DAlessioArenaMission3 = new() { Id = 315, Name = "D'Alessio Arena", WikiUrl = "https://wiki.guildwars.com/wiki/D'Alessio_Arena" };
    public static readonly Map AmnoonArenaMission3 = new() { Id = 319, Name = "Amnoon Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Amnoon_Arena" };
    public static readonly Map FortKogaMission3 = new() { Id = 320, Name = "Fort Koga", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Koga" };
    public static readonly Map HeroesCryptMission3 = new() { Id = 321, Name = "Heroes' Crypt", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes'_Crypt" };
    public static readonly Map ShiverpeakArenaMission3 = new() { Id = 322, Name = "Shiverpeak Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Shiverpeak_Arena" };
    public static readonly Map SaltsprayBeachLuxonOutpost = new() { Id = 323, Name = "Saltspray Beach (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/Saltspray_Beach_(Luxon)" };
    public static readonly Map SaltsprayBeachKurzickOutpost = new() { Id = 329, Name = "Saltspray Beach (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/Saltspray_Beach_(Kurzick)" };
    public static readonly Map HeroesAscentOutpost = new() { Id = 330, Name = "Heroes Ascent", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes_Ascent" };
    public static readonly Map GrenzFrontierLuxonOutpost = new() { Id = 331, Name = "Grenz Frontier (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/Grenz_Frontier_(Luxon)" };
    public static readonly Map GrenzFrontierKurzickOutpost = new() { Id = 332, Name = "Grenz Frontier (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/Grenz_Frontier_(Kurzick)" };
    public static readonly Map TheAncestralLandsLuxonOutpost = new() { Id = 333, Name = "The Ancestral Lands (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ancestral_Lands_(Luxon)" };
    public static readonly Map TheAncestralLandsKurzickOutpost = new() { Id = 334, Name = "The Ancestral Lands (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ancestral_Lands_(Kurzick)" };
    public static readonly Map EtnaranKeysLuxonOutpost = new() { Id = 335, Name = "Etnaran Keys (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/Etnaran_Keys_(Luxon)" };
    public static readonly Map EtnaranKeysKurzickOutpost = new() { Id = 336, Name = "Etnaran Keys (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/Etnaran_Keys_(Kurzick)" };
    public static readonly Map KaanaiCanyonLuxonOutpost = new() { Id = 337, Name = "Kaanai Canyon (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/Kaanai_Canyon_(Luxon)" };
    public static readonly Map KaanaiCanyonKurzickOutpost = new() { Id = 338, Name = "Kaanai Canyon (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/Kaanai_Canyon_(Kurzick)" };
    public static readonly Map DAlessioArenaMission2 = new() { Id = 339, Name = "D'Alessio Arena", WikiUrl = "https://wiki.guildwars.com/wiki/D'Alessio_Arena" };
    public static readonly Map AmnoonArenaMission2 = new() { Id = 340, Name = "Amnoon Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Amnoon_Arena" };
    public static readonly Map FortKogaMission2 = new() { Id = 341, Name = "Fort Koga", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Koga" };
    public static readonly Map HeroesCryptMission2 = new() { Id = 342, Name = "Heroes' Crypt", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes'_Crypt" };
    public static readonly Map ShiverpeakArenaMission2 = new() { Id = 343, Name = "Shiverpeak Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Shiverpeak_Arena" };
    public static readonly Map TheHallofHeroes = new() { Id = 344, Name = "The Hall of Heroes", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hall_of_Heroes" };
    public static readonly Map TheCourtyard = new() { Id = 345, Name = "The Courtyard", WikiUrl = "https://wiki.guildwars.com/wiki/The_Courtyard" };
    public static readonly Map ScarredEarth = new() { Id = 346, Name = "Scarred Earth", WikiUrl = "https://wiki.guildwars.com/wiki/Scarred_Earth" };
    public static readonly Map TheUnderworldPvP = new() { Id = 347, Name = "The Underworld", WikiUrl = "https://wiki.guildwars.com/wiki/The_Underworld" };
    public static readonly Map TanglewoodCopseOutpost = new() { Id = 348, Name = "Tanglewood Copse", WikiUrl = "https://wiki.guildwars.com/wiki/Tanglewood_Copse" };
    public static readonly Map SaintAnjekasShrineOutpost = new() { Id = 349, Name = "Saint Anjeka's Shrine", WikiUrl = "https://wiki.guildwars.com/wiki/Saint_Anjeka's_Shrine" };
    public static readonly Map EredonTerraceOutpost = new() { Id = 350, Name = "Eredon Terrace", WikiUrl = "https://wiki.guildwars.com/wiki/Eredon_Terrace" };
    public static readonly Map DivinePath = new() { Id = 351, Name = "Divine Path", WikiUrl = "https://wiki.guildwars.com/wiki/Divine_Path" };
    public static readonly Map BrawlersPitMission = new() { Id = 352, Name = "Brawler's Pit", WikiUrl = "https://wiki.guildwars.com/wiki/Brawler's_Pit" };
    public static readonly Map PetrifiedArenaMission = new() { Id = 353, Name = "Petrified Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Petrified_Arena" };
    public static readonly Map SeabedArenaMission = new() { Id = 354, Name = "Seabed Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Seabed_Arena" };
    public static readonly Map IsleOfWeepingStoneMission = new() { Id = 355, Name = "Isle of Weeping Stone", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Weeping_Stone" };
    public static readonly Map IsleOfJadeMission = new() { Id = 356, Name = "Isle of Jade", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Jade" };
    public static readonly Map ImperialIsleMission = new() { Id = 357, Name = "Imperial Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Isle" };
    public static readonly Map IsleOfMeditationMission = new() { Id = 358, Name = "Isle of Meditation", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Meditation" };
    public static readonly Map ImperialIsleOutpost = new() { Id = 359, Name = "Imperial Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Isle" };
    public static readonly Map IsleOfMeditationOutpost = new() { Id = 360, Name = "Isle of Meditation", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Meditation" };
    public static readonly Map IsleOfWeepingStone = new() { Id = 361, Name = "Isle of Weeping Stone", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Weeping_Stone" };
    public static readonly Map IsleOfJade = new() { Id = 362, Name = "Isle of Jade", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Jade" };
    public static readonly Map ImperialIsle = new() { Id = 363, Name = "Imperial Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Isle" };
    public static readonly Map IsleOfMeditation = new() { Id = 364, Name = "Isle of Meditation", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Meditation" };
    public static readonly Map ShingJeaArenaMission = new() { Id = 365, Name = "Shing Jea Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Shing_Jea_Arena" };
    public static readonly Map DragonArena = new() { Id = 367, Name = "Dragon Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Dragon_Arena" };
    public static readonly Map JahaiBluffs = new() { Id = 369, Name = "Jahai Bluffs", WikiUrl = "https://wiki.guildwars.com/wiki/Jahai_Bluffs" };
    public static readonly Map KamadanMission = new() { Id = 370, Name = "Kamadan", WikiUrl = "https://wiki.guildwars.com/wiki/Kamadan" };
    public static readonly Map MargaCoast = new() { Id = 371, Name = "Marga Coast", WikiUrl = "https://wiki.guildwars.com/wiki/Marga_Coast" };
    public static readonly Map FahranurMission = new() { Id = 372, Name = "Fahranur", WikiUrl = "https://wiki.guildwars.com/wiki/Fahranur" };
    public static readonly Map SunwardMarches = new() { Id = 373, Name = "Sunward Marches", WikiUrl = "https://wiki.guildwars.com/wiki/Sunward_Marches" };
    public static readonly Map BarbarousShore = new() { Id = 374, Name = "Vortex", WikiUrl = "https://wiki.guildwars.com/wiki/Vortex" };
    public static readonly Map CampHojanuOutpost = new() { Id = 376, Name = "Camp Hojanu", WikiUrl = "https://wiki.guildwars.com/wiki/Camp_Hojanu" };
    public static readonly Map BahdokCaverns = new() { Id = 377, Name = "Bahdok Caverns", WikiUrl = "https://wiki.guildwars.com/wiki/Bahdok_Caverns" };
    public static readonly Map WehhanTerracesOutpost = new() { Id = 378, Name = "Wehhan Terraces", WikiUrl = "https://wiki.guildwars.com/wiki/Wehhan_Terraces" };
    public static readonly Map DejarinEstate = new() { Id = 379, Name = "Dejarin Estate", WikiUrl = "https://wiki.guildwars.com/wiki/Dejarin_Estate" };
    public static readonly Map ArkjokWard = new() { Id = 380, Name = "Arkjok Ward", WikiUrl = "https://wiki.guildwars.com/wiki/Arkjok_Ward" };
    public static readonly Map YohlonHavenOutpost = new() { Id = 381, Name = "Yohlon Haven", WikiUrl = "https://wiki.guildwars.com/wiki/Yohlon_Haven" };
    public static readonly Map GandaraTheMoonFortress = new() { Id = 382, Name = "Gandara", WikiUrl = "https://wiki.guildwars.com/wiki/Gandara" };
    public static readonly Map TheFloodplainOfMahnkelon = new() { Id = 383, Name = "The Floodplain of Mahnkelon", WikiUrl = "https://wiki.guildwars.com/wiki/The_Floodplain_of_Mahnkelon" };
    public static readonly Map LionsArchSunspearsinKryta = new() { Id = 385, Name = "Lion's Arch", WikiUrl = "https://wiki.guildwars.com/wiki/Lion's_Arch" };
    public static readonly Map TuraisProcession = new() { Id = 386, Name = "Turai's Procession", WikiUrl = "https://wiki.guildwars.com/wiki/Turai's_Procession" };
    public static readonly Map SunspearSanctuaryOutpost = new() { Id = 387, Name = "Sunspear Sanctuary", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Sanctuary" };
    public static readonly Map AspenwoodGateKurzickOutpost = new() { Id = 388, Name = "Aspenwood Gate (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/Aspenwood_Gate_(Kurzick)" };
    public static readonly Map AspenwoodGateLuxonOutpost = new() { Id = 389, Name = "Aspenwood Gate (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/Aspenwood_Gate_(Luxon)" };
    public static readonly Map JadeFlatsKurzickOutpost = new() { Id = 390, Name = "Jade Flats (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Flats_(Kurzick)" };
    public static readonly Map JadeFlatsLuxonOutpost = new() { Id = 391, Name = "Jade Flats (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Flats_(Luxon)" };
    public static readonly Map YatendiCanyons = new() { Id = 392, Name = "Yatendi Canyons", WikiUrl = "https://wiki.guildwars.com/wiki/Yatendi_Canyons" };
    public static readonly Map ChantryOfSecretsOutpost = new() { Id = 393, Name = "Chantry of Secrets", WikiUrl = "https://wiki.guildwars.com/wiki/Chantry_of_Secrets" };
    public static readonly Map GardenOfSeborhin = new() { Id = 394, Name = "Garden of Seborhin", WikiUrl = "https://wiki.guildwars.com/wiki/Garden_of_Seborhin" };
    public static readonly Map HoldingsOfChokhin = new() { Id = 395, Name = "Holdings of Chokhin", WikiUrl = "https://wiki.guildwars.com/wiki/Holdings_of_Chokhin" };
    public static readonly Map MihanuTownshipOutpost = new() { Id = 396, Name = "Mihanu Township", WikiUrl = "https://wiki.guildwars.com/wiki/Mihanu_Township" };
    public static readonly Map VehjinMines = new() { Id = 397, Name = "Vehjin Mines", WikiUrl = "https://wiki.guildwars.com/wiki/Vehjin_Mines" };
    public static readonly Map BasaltGrottoOutpost = new() { Id = 398, Name = "Basalt Grotto", WikiUrl = "https://wiki.guildwars.com/wiki/Basalt_Grotto" };
    public static readonly Map ForumHighlands = new() { Id = 399, Name = "Forum Highlands", WikiUrl = "https://wiki.guildwars.com/wiki/Forum_Highlands" };
    public static readonly Map KainengCenterSunspearsInCantha = new() { Id = 400, Name = "Kaineng Center", WikiUrl = "https://wiki.guildwars.com/wiki/Kaineng_Center" };
    public static readonly Map ResplendentMakuun = new() { Id = 401, Name = "Resplendent Makuun", WikiUrl = "https://wiki.guildwars.com/wiki/Resplendent_Makuun" };
    public static readonly Map ResplendentMakuun2 = new() { Id = 402, Name = "Resplendent Makuun", WikiUrl = "https://wiki.guildwars.com/wiki/Resplendent_Makuun" };
    public static readonly Map HonurHillOutpost = new() { Id = 403, Name = "Honur Hill", WikiUrl = "https://wiki.guildwars.com/wiki/Honur_Hill" };
    public static readonly Map WildernessOfBahdza = new() { Id = 404, Name = "Wilderness of Bahdza", WikiUrl = "https://wiki.guildwars.com/wiki/Wilderness_of_Bahdza" };
    public static readonly Map VehtendiValley = new() { Id = 405, Name = "Vehtendi Valley", WikiUrl = "https://wiki.guildwars.com/wiki/Vehtendi_Valley" };
    public static readonly Map YahnurMarketOutpost = new() { Id = 407, Name = "Yahnur Market", WikiUrl = "https://wiki.guildwars.com/wiki/Yahnur_Market" };
    public static readonly Map TheHiddenCityOfAhdashim = new() { Id = 408, Name = "The Hidden City of Ahdashim", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hidden_City_of_Ahdashim" };
    public static readonly Map TheKodashBazaarOutpost = new() { Id = 414, Name = "The Kodash Bazaar", WikiUrl = "https://wiki.guildwars.com/wiki/The_Kodash_Bazaar" };
    public static readonly Map LionsGate = new() { Id = 415, Name = "Lion's Gate", WikiUrl = "https://wiki.guildwars.com/wiki/Lion's_Gate" };
    public static readonly Map TheMirrorOfLyss = new() { Id = 416, Name = "The Mirror of Lyss", WikiUrl = "https://wiki.guildwars.com/wiki/The_Mirror_of_Lyss" };
    public static readonly Map TheMirrorOfLyss2 = new() { Id = 419, Name = "The Mirror of Lyss", WikiUrl = "https://wiki.guildwars.com/wiki/The_Mirror_of_Lyss" };
    public static readonly Map SecuretheRefuge = new() { Id = 420, Name = "Secure the Refuge", WikiUrl = "https://wiki.guildwars.com/wiki/Secure_the_Refuge" };
    public static readonly Map VentaCemetery = new() { Id = 421, Name = "Venta Cemetery", WikiUrl = "https://wiki.guildwars.com/wiki/Venta_Cemetery" };
    public static readonly Map KamadanJewelOfIstanExplorable = new() { Id = 422, Name = "Kamadan", WikiUrl = "https://wiki.guildwars.com/wiki/Kamadan" };
    public static readonly Map TheTribunal = new() { Id = 423, Name = "The Tribunal", WikiUrl = "https://wiki.guildwars.com/wiki/The_Tribunal" };
    public static readonly Map KodonurCrossroads = new() { Id = 424, Name = "Kodonur Crossroads", WikiUrl = "https://wiki.guildwars.com/wiki/Kodonur_Crossroads" };
    public static readonly Map RilohnRefuge = new() { Id = 425, Name = "Rilohn Refuge", WikiUrl = "https://wiki.guildwars.com/wiki/Rilohn_Refuge" };
    public static readonly Map PogahnPassage = new() { Id = 426, Name = "Pogahn Passage", WikiUrl = "https://wiki.guildwars.com/wiki/Pogahn_Passage" };
    public static readonly Map ModdokCrevice = new() { Id = 427, Name = "Moddok Crevice", WikiUrl = "https://wiki.guildwars.com/wiki/Moddok_Crevice" };
    public static readonly Map TiharkOrchard = new() { Id = 428, Name = "Tihark Orchard", WikiUrl = "https://wiki.guildwars.com/wiki/Tihark_Orchard" };
    public static readonly Map Consulate = new() { Id = 429, Name = "Consulate", WikiUrl = "https://wiki.guildwars.com/wiki/Consulate" };
    public static readonly Map PlainsOfJarin = new() { Id = 430, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map SunspearGreatHallOutpost = new() { Id = 431, Name = "Sunspear Great Hall", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Great_Hall" };
    public static readonly Map CliffsOfDohjok = new() { Id = 432, Name = "Cliffs of Dohjok", WikiUrl = "https://wiki.guildwars.com/wiki/Cliffs_of_Dohjok" };
    public static readonly Map DzagonurBastion = new() { Id = 433, Name = "Dzagonur Bastion", WikiUrl = "https://wiki.guildwars.com/wiki/Dzagonur_Bastion" };
    public static readonly Map DashaVestibule = new() { Id = 434, Name = "Dasha Vestibule", WikiUrl = "https://wiki.guildwars.com/wiki/Dasha_Vestibule" };
    public static readonly Map GrandCourtOfSebelkeh = new() { Id = 435, Name = "Grand Court of Sebelkeh", WikiUrl = "https://wiki.guildwars.com/wiki/Grand_Court_of_Sebelkeh" };
    public static readonly Map CommandPost = new() { Id = 436, Name = "Command Post", WikiUrl = "https://wiki.guildwars.com/wiki/Command_Post" };
    public static readonly Map JokosDomain = new() { Id = 437, Name = "Joko's Domain", WikiUrl = "https://wiki.guildwars.com/wiki/Joko's_Domain" };
    public static readonly Map BonePalaceOutpost = new() { Id = 438, Name = "Bone Palace", WikiUrl = "https://wiki.guildwars.com/wiki/Bone_Palace" };
    public static readonly Map TheRupturedHeart = new() { Id = 439, Name = "The Ruptured Heart", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ruptured_Heart" };
    public static readonly Map TheMouthOfTormentOutpost = new() { Id = 440, Name = "The Mouth of Torment", WikiUrl = "https://wiki.guildwars.com/wiki/The_Mouth_of_Torment" };
    public static readonly Map TheShatteredRavines = new() { Id = 441, Name = "The Shattered Ravines", WikiUrl = "https://wiki.guildwars.com/wiki/The_Shattered_Ravines" };
    public static readonly Map LairOfTheForgottenOutpost = new() { Id = 442, Name = "Lair of the Forgotten", WikiUrl = "https://wiki.guildwars.com/wiki/Lair_of_the_Forgotten" };
    public static readonly Map PoisonedOutcrops = new() { Id = 443, Name = "Poisoned Outcrops", WikiUrl = "https://wiki.guildwars.com/wiki/Poisoned_Outcrops" };
    public static readonly Map TheSulfurousWastes = new() { Id = 444, Name = "The Sulfurous Wastes", WikiUrl = "https://wiki.guildwars.com/wiki/The_Sulfurous_Wastes" };
    public static readonly Map TheEbonyCitadelOfMallyxMission = new() { Id = 445, Name = "The Ebony Citadel of Mallyx", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ebony_Citadel_of_Mallyx" };
    public static readonly Map TheAlkaliPan = new() { Id = 446, Name = "The Alkali Pan", WikiUrl = "https://wiki.guildwars.com/wiki/The_Alkali_Pan" };
    public static readonly Map ALandofHeroes = new() { Id = 447, Name = "Cliffs of Dohjok", WikiUrl = "https://wiki.guildwars.com/wiki/Cliffs_of_Dohjok" };
    public static readonly Map CrystalOverlook = new() { Id = 448, Name = "Crystal Overlook", WikiUrl = "https://wiki.guildwars.com/wiki/Crystal_Overlook" };
    public static readonly Map KamadanJewelOfIstanOutpost = new() { Id = 449, Name = "Kamadan", WikiUrl = "https://wiki.guildwars.com/wiki/Kamadan" };
    public static readonly Map GateOfTormentOutpost = new() { Id = 450, Name = "Gate of Torment", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Torment" };
    public static readonly Map NightfallenGarden = new() { Id = 451, Name = "Nightfallen Garden", WikiUrl = "https://wiki.guildwars.com/wiki/Nightfallen_Garden" };
    public static readonly Map ChurrhirFields = new() { Id = 456, Name = "Churrhir Fields", WikiUrl = "https://wiki.guildwars.com/wiki/Churrhir_Fields" };
    public static readonly Map BeknurHarborOutpost = new() { Id = 457, Name = "Beknur Harbor", WikiUrl = "https://wiki.guildwars.com/wiki/Beknur_Harbor" };
    public static readonly Map TheUnderworld2 = new() { Id = 458, Name = "The Underworld", WikiUrl = "https://wiki.guildwars.com/wiki/The_Underworld" };
    public static readonly Map HeartOfAbaddon = new() { Id = 462, Name = "Heart of Abaddon", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_of_Abaddon" };
    public static readonly Map TheUnderworld3 = new() { Id = 463, Name = "The Underworld", WikiUrl = "https://wiki.guildwars.com/wiki/The_Underworld" };
    public static readonly Map NightfallenCoast = new() { Id = 464, Name = "Nundu Bay", WikiUrl = "https://wiki.guildwars.com/wiki/Nundu_Bay" };
    public static readonly Map NightfallenJahai = new() { Id = 465, Name = "Nightfallen Jahai", WikiUrl = "https://wiki.guildwars.com/wiki/Nightfallen_Jahai" };
    public static readonly Map DepthsOfMadness = new() { Id = 466, Name = "Depths of Madness", WikiUrl = "https://wiki.guildwars.com/wiki/Depths_of_Madness" };
    public static readonly Map RollerbeetleRacing = new() { Id = 467, Name = "Rollerbeetle Racing", WikiUrl = "https://wiki.guildwars.com/wiki/Rollerbeetle_Racing" };
    public static readonly Map DomainOfFear = new() { Id = 468, Name = "Domain of Fear", WikiUrl = "https://wiki.guildwars.com/wiki/Domain_of_Fear" };
    public static readonly Map GateOfFearOutpost = new() { Id = 469, Name = "Gate of Fear", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Fear" };
    public static readonly Map DomainOfPain = new() { Id = 470, Name = "Domain of Pain", WikiUrl = "https://wiki.guildwars.com/wiki/Domain_of_Pain" };
    public static readonly Map BloodstoneFenQuest = new() { Id = 471, Name = "Bloodstone Fen", WikiUrl = "https://wiki.guildwars.com/wiki/Bloodstone_Fen" };
    public static readonly Map DomainOfSecrets = new() { Id = 472, Name = "Domain of Secrets", WikiUrl = "https://wiki.guildwars.com/wiki/Domain_of_Secrets" };
    public static readonly Map GateOfSecretsOutpost = new() { Id = 473, Name = "Gate of Secrets", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Secrets" };
    public static readonly Map DomainOfAnguish = new() { Id = 474, Name = "Gate of Anguish", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Anguish" };
    public static readonly Map OozePitMission = new() { Id = 475, Name = "Oozez Pit", WikiUrl = "https://wiki.guildwars.com/wiki/Ooze_Pit" };
    public static readonly Map JennursHorde = new() { Id = 476, Name = "Jennur's Horde", WikiUrl = "https://wiki.guildwars.com/wiki/Jennur's_Horde" };
    public static readonly Map NunduBay = new() { Id = 477, Name = "Nundu Bay", WikiUrl = "https://wiki.guildwars.com/wiki/Nundu_Bay" };
    public static readonly Map GateOfDesolation = new() { Id = 478, Name = "Gate of Desolation", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Desolation" };
    public static readonly Map ChampionsDawnOutpost = new() { Id = 479, Name = "Champions Dawn", WikiUrl = "https://wiki.guildwars.com/wiki/Champions_Dawn" };
    public static readonly Map RuinsOfMorah = new() { Id = 480, Name = "Ruins of Morah", WikiUrl = "https://wiki.guildwars.com/wiki/Ruins_of_Morah" };
    public static readonly Map FahranurTheFirstCity = new() { Id = 481, Name = "Fahranur", WikiUrl = "https://wiki.guildwars.com/wiki/Fahranur" };
    public static readonly Map BjoraMarches = new() { Id = 482, Name = "Bjora Marches", WikiUrl = "https://wiki.guildwars.com/wiki/Bjora_Marches" };
    public static readonly Map ZehlonReach = new() { Id = 483, Name = "Zehlon Reach", WikiUrl = "https://wiki.guildwars.com/wiki/Zehlon_Reach" };
    public static readonly Map LahtendaBog = new() { Id = 484, Name = "Lahtenda Bog", WikiUrl = "https://wiki.guildwars.com/wiki/Lahtenda_Bog" };
    public static readonly Map ArborBay = new() { Id = 485, Name = "Arbor Bay", WikiUrl = "https://wiki.guildwars.com/wiki/Arbor_Bay" };
    public static readonly Map IssnurIsles = new() { Id = 486, Name = "Issnur Isles", WikiUrl = "https://wiki.guildwars.com/wiki/Issnur_Isles" };
    public static readonly Map BeknurHarbor = new() { Id = 487, Name = "Beknur Harbor", WikiUrl = "https://wiki.guildwars.com/wiki/Beknur_Harbor" };
    public static readonly Map MehtaniKeys = new() { Id = 488, Name = "Mehtani Keys", WikiUrl = "https://wiki.guildwars.com/wiki/Mehtani_Keys" };
    public static readonly Map KodlonuHamletOutpost = new() { Id = 489, Name = "Kodlonu Hamlet", WikiUrl = "https://wiki.guildwars.com/wiki/Kodlonu_Hamlet" };
    public static readonly Map IslandOfShehkah = new() { Id = 490, Name = "Island of Shehkah", WikiUrl = "https://wiki.guildwars.com/wiki/Island_of_Shehkah" };
    public static readonly Map JokanurDiggings = new() { Id = 491, Name = "Jokanur Diggings", WikiUrl = "https://wiki.guildwars.com/wiki/Jokanur_Diggings" };
    public static readonly Map BlacktideDen = new() { Id = 492, Name = "Blacktide Den", WikiUrl = "https://wiki.guildwars.com/wiki/Blacktide_Den" };
    public static readonly Map ConsulateDocks = new() { Id = 493, Name = "Consulate Docks", WikiUrl = "https://wiki.guildwars.com/wiki/Consulate_Docks" };
    public static readonly Map GateOfPain = new() { Id = 494, Name = "Gate of Pain", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Pain" };
    public static readonly Map GateOfMadness = new() { Id = 495, Name = "Gate of Madness", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Madness" };
    public static readonly Map AbaddonsGate = new() { Id = 496, Name = "Abaddons Gate", WikiUrl = "https://wiki.guildwars.com/wiki/Abaddons_Gate" };
    public static readonly Map SunspearArena = new() { Id = 497, Name = "Sunspear Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Arena" };
    public static readonly Map IceCliffChasms = new() { Id = 498, Name = "Ice Cliff Chasms", WikiUrl = "https://wiki.guildwars.com/wiki/Ice_Cliff_Chasms" };
    public static readonly Map BokkaAmphitheatre = new() { Id = 500, Name = "Bokka Amphitheatre", WikiUrl = "https://wiki.guildwars.com/wiki/Bokka_Amphitheatre" };
    public static readonly Map RivenEarth = new() { Id = 501, Name = "Riven Earth", WikiUrl = "https://wiki.guildwars.com/wiki/Riven_Earth" };
    public static readonly Map TheAstralariumOutpost = new() { Id = 502, Name = "The Astralarium", WikiUrl = "https://wiki.guildwars.com/wiki/The_Astralarium" };
    public static readonly Map ThroneOfSecrets = new() { Id = 503, Name = "Throne Of Secrets", WikiUrl = "https://wiki.guildwars.com/wiki/Throne_of_Secrets" };
    public static readonly Map ChurranuIslandArenaMission = new() { Id = 504, Name = "Churranu Island Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Churranu_Island_Arena" };
    public static readonly Map ShingJeaMonasteryMission = new() { Id = 505, Name = "Shing Jea Monastery", WikiUrl = "https://wiki.guildwars.com/wiki/Shing_Jea_Monastery" };
    public static readonly Map HaijuLagoonMission = new() { Id = 506, Name = "Haiju Lagoon", WikiUrl = "https://wiki.guildwars.com/wiki/Haiju_Lagoon" };
    public static readonly Map JayaBluffsMission = new() { Id = 507, Name = "Jaya Bluffs", WikiUrl = "https://wiki.guildwars.com/wiki/Jaya_Bluffs" };
    public static readonly Map SeitungHarborMission = new() { Id = 508, Name = "Seitung Harbor", WikiUrl = "https://wiki.guildwars.com/wiki/Seitung_Harbor" };
    public static readonly Map TsumeiVillageMission = new() { Id = 509, Name = "Tsumei Village", WikiUrl = "https://wiki.guildwars.com/wiki/Tsumei_Village" };
    public static readonly Map SeitungHarborMission2 = new() { Id = 510, Name = "Seitung Harbor", WikiUrl = "https://wiki.guildwars.com/wiki/Seitung_Harbor" };
    public static readonly Map TsumeiVillageMission2 = new() { Id = 511, Name = "Tsumei Village", WikiUrl = "https://wiki.guildwars.com/wiki/Tsumei_Village" };
    public static readonly Map DrakkarLake = new() { Id = 512, Name = "Drakkar Lake", WikiUrl = "https://wiki.guildwars.com/wiki/Drakkar_Lake" };
    public static readonly Map MinisterChosEstateMission2 = new() { Id = 514, Name = "Minister Cho's Estate", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Cho's_Estate" };
    public static readonly Map UnchartedIsleOutpost = new() { Id = 513, Name = "Uncharted Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Uncharted_Isle" };
    public static readonly Map IsleOfWurmsOutpost = new() { Id = 530, Name = "Isle of Wurms", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Wurms" };
    public static readonly Map UnchartedIsle = new() { Id = 531, Name = "Uncharted Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Uncharted_Isle" };
    public static readonly Map IsleOfWurms = new() { Id = 532, Name = "Isle of Wurms", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Wurms" };
    public static readonly Map UnchartedIsleMission = new() { Id = 533, Name = "Uncharted Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Uncharted_Isle" };
    public static readonly Map IsleOfWurmsMission = new() { Id = 534, Name = "Isle of Wurms", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Wurms" };
    public static readonly Map SunspearArenaMission = new() { Id = 535, Name = "Sunspear Arena", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Arena" };
    public static readonly Map CorruptedIsleOutpost = new() { Id = 537, Name = "Corrupted Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Isle" };
    public static readonly Map IsleOfSolitudeOutpost = new() { Id = 538, Name = "Isle of Solitude", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Solitude" };
    public static readonly Map CorruptedIsle = new() { Id = 539, Name = "Corrupted Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Isle" };
    public static readonly Map IsleOfSolitude = new() { Id = 540, Name = "Isle of Solitude", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Solitude" };
    public static readonly Map CorruptedIsleMission = new() { Id = 541, Name = "Corrupted Isle", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Isle" };
    public static readonly Map IsleOfSolitudeMission = new() { Id = 542, Name = "Isle of Solitude", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_Solitude" };
    public static readonly Map SunDocks = new() { Id = 543, Name = "Sun Docks", WikiUrl = "https://wiki.guildwars.com/wiki/Sun_Docks" };
    public static readonly Map ChahbekVillage = new() { Id = 544, Name = "Chahbek Village", WikiUrl = "https://wiki.guildwars.com/wiki/Chahbek_Village" };
    public static readonly Map RemainsOfSahlahja = new() { Id = 545, Name = "Remains of Sahlahja", WikiUrl = "https://wiki.guildwars.com/wiki/Remains_of_Sahlahja" };
    public static readonly Map JagaMoraine = new() { Id = 546, Name = "Jaga Moraine", WikiUrl = "https://wiki.guildwars.com/wiki/Jaga_Moraine" };
    public static readonly Map BombardmentMission = new() { Id = 547, Name = "Bombardment", WikiUrl = "https://wiki.guildwars.com/wiki/Bombardment" };
    public static readonly Map NorrhartDomains = new() { Id = 548, Name = "Norrhart Domains", WikiUrl = "https://wiki.guildwars.com/wiki/Norrhart_Domains" };
    public static readonly Map HeroBattlesOutpost = new() { Id = 549, Name = "Hero Battles", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_Battles" };
    public static readonly Map TheBeachheadMission = new() { Id = 550, Name = "Hero Battles", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_Battles" };
    public static readonly Map TheCrossingMission = new() { Id = 551, Name = "The Crossing", WikiUrl = "https://wiki.guildwars.com/wiki/The_Crossing" };
    public static readonly Map DesertSandsMission = new() { Id = 552, Name = "Desert Sands", WikiUrl = "https://wiki.guildwars.com/wiki/Desert_Sands" };
    public static readonly Map VarajarFells = new() { Id = 553, Name = "Varajar Fells", WikiUrl = "https://wiki.guildwars.com/wiki/Varajar_Fells" };
    public static readonly Map DajkahInlet = new() { Id = 554, Name = "Dajkah Inlet", WikiUrl = "https://wiki.guildwars.com/wiki/Dajkah_Inlet" };
    public static readonly Map TheShadowNexus = new() { Id = 555, Name = "The Shadow Nexus", WikiUrl = "https://wiki.guildwars.com/wiki/The_Shadow_Nexus" };
    public static readonly Map SparkflySwamp = new() { Id = 556, Name = "Sparkfly Swamp", WikiUrl = "https://wiki.guildwars.com/wiki/Sparkfly_Swamp" };
    public static readonly Map GateOftheNightfallenLandsOutpost = new() { Id = 559, Name = "Gate of the Nightfallen Lands", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_the_Nightfallen_Lands" };
    public static readonly Map CathedralofFlamesLevel1 = new() { Id = 560, Name = "Cathedral of Flames", WikiUrl = "https://wiki.guildwars.com/wiki/Cathedral_of_Flames" };
    public static readonly Map TheTroubledKeeper = new() { Id = 561, Name = "Gate of Torment", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Torment" };
    public static readonly Map VerdantCascades = new() { Id = 562, Name = "Verdant Cascades", WikiUrl = "https://wiki.guildwars.com/wiki/Verdant_Cascades" };
    public static readonly Map CathedralofFlamesLevel2 = new() { Id = 567, Name = "Cathedral of Flames: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Cathedral_of_Flames" };
    public static readonly Map CathedralofFlamesLevel3 = new() { Id = 568, Name = "Cathedral of Flames: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Cathedral_of_Flames" };
    public static readonly Map MagusStones = new() { Id = 569, Name = "Magus Stones", WikiUrl = "https://wiki.guildwars.com/wiki/Magus_Stones" };
    public static readonly Map CatacombsofKathandraxLevel1 = new() { Id = 570, Name = "Catacombs of Kathandrax", WikiUrl = "https://wiki.guildwars.com/wiki/Catacombs_of_Kathandrax" };
    public static readonly Map CatacombsofKathandraxLevel2 = new() { Id = 571, Name = "Catacombs of Kathandrax: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Catacombs_of_Kathandrax:_Level_2" };
    public static readonly Map AlcaziaTangle = new() { Id = 572, Name = "Alcazia Tangle", WikiUrl = "https://wiki.guildwars.com/wiki/Alcazia_Tangle" };
    public static readonly Map RragarsMenagerieLevel1 = new() { Id = 573, Name = "Rragars Menagerie", WikiUrl = "https://wiki.guildwars.com/wiki/Rragar's_Menagerie" };
    public static readonly Map RragarsMenagerieLevel2 = new() { Id = 574, Name = "Rragars Menagerie: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Rragar's_Menagerie" };
    public static readonly Map RragarsMenagerieLevel3 = new() { Id = 575, Name = "Rragars Menagerie: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Rragar's_Menagerie" };
    public static readonly Map OozePit = new() { Id = 576, Name = "Ooze Pit", WikiUrl = "https://wiki.guildwars.com/wiki/Ooze_Pit" };
    public static readonly Map SlaversExileLevel1 = new() { Id = 577, Name = "Slavers Exile", WikiUrl = "https://wiki.guildwars.com/wiki/Slavers_Exile" };
    public static readonly Map OolasLabLevel1 = new() { Id = 578, Name = "Oola's Lab", WikiUrl = "https://wiki.guildwars.com/wiki/Oola's_Lab" };
    public static readonly Map OolasLabLevel2 = new() { Id = 579, Name = "Oola's Lab: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Oola's_Lab:_Level_2" };
    public static readonly Map OolasLabLevel3 = new() { Id = 580, Name = "Oola's Lab: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Oola's_Lab:_Level_3" };
    public static readonly Map ShardsOfOrrLevel1 = new() { Id = 581, Name = "Shards of Orr", WikiUrl = "https://wiki.guildwars.com/wiki/Shards_of_Orr" };
    public static readonly Map ShardsOfOrrLevel2 = new() { Id = 582, Name = "Shards of Orr: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Shards_of_Orr:_Level_2" };
    public static readonly Map ShardsOfOrrLevel3 = new() { Id = 583, Name = "Shards of Orr: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Shards_of_Orr:_Level_3" };
    public static readonly Map ArachnisHauntLevel1 = new() { Id = 584, Name = "Arachni's Haunt", WikiUrl = "https://wiki.guildwars.com/wiki/Arachni's_Haunt" };
    public static readonly Map ArachnisHauntLevel2 = new() { Id = 585, Name = "Arachni's Haunt: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Arachni's_Haunt:_Level_2" };
    public static readonly Map FetidRiverMission = new() { Id = 586, Name = "Fetid River", WikiUrl = "https://wiki.guildwars.com/wiki/Fetid_River" };
    public static readonly Map ForgottenShrinesMission = new() { Id = 594, Name = "Forgotten Shrines", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Shrines" };
    public static readonly Map AntechamberMission = new() { Id = 597, Name = "The Antechamber", WikiUrl = "https://wiki.guildwars.com/wiki/The_Antechamber" };
    public static readonly Map VloxenExcavationsLevel1 = new() { Id = 599, Name = "Vloxen Excavations", WikiUrl = "https://wiki.guildwars.com/wiki/Vloxen_Excavations" };
    public static readonly Map VloxenExcavationsLevel2 = new() { Id = 605, Name = "Vloxen Excavations: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Vloxen_Excavations:_Level_2" };
    public static readonly Map VloxenExcavationsLevel3 = new() { Id = 606, Name = "Vloxen Excavations: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Vloxen_Excavations:_Level_3" };
    public static readonly Map HeartOftheShiverpeaksLevel1 = new() { Id = 607, Name = "Heart of the Shiverpeaks", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_of_the_Shiverpeaks" };
    public static readonly Map HeartOftheShiverpeaksLevel2 = new() { Id = 608, Name = "Heart of the Shiverpeaks: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_of_the_Shiverpeaks:_Level_2" };
    public static readonly Map HeartOftheShiverpeaksLevel3 = new() { Id = 609, Name = "Heart of the Shiverpeaks: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_of_the_Shiverpeaks:_Level_3" };
    public static readonly Map BloodstoneCavesLevel1 = new() { Id = 610, Name = "Bloodstone Caves", WikiUrl = "https://wiki.guildwars.com/wiki/Bloodstone_Caves" };
    public static readonly Map BloodstoneCavesLevel2 = new() { Id = 613, Name = "Bloodstone Caves: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Bloodstone_Caves:_Level_2" };
    public static readonly Map BloodstoneCavesLevel3 = new() { Id = 614, Name = "Bloodstone Caves: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Bloodstone_Caves:_Level_3" };
    public static readonly Map BogrootGrowthsLevel1 = new() { Id = 615, Name = "Bogroot Growths", WikiUrl = "https://wiki.guildwars.com/wiki/Bogroot_Growths" };
    public static readonly Map BogrootGrowthsLevel2 = new() { Id = 616, Name = "Bogroot Growths: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Bogroot_Growths:_Level_2" };
    public static readonly Map RavensPointLevel1 = new() { Id = 617, Name = "Raven's Point", WikiUrl = "https://wiki.guildwars.com/wiki/Raven's_Point" };
    public static readonly Map RavensPointLevel2 = new() { Id = 618, Name = "Raven's Point: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Raven's_Point:_Level_2" };
    public static readonly Map RavensPointLevel3 = new() { Id = 619, Name = "Raven's Point: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Raven's_Point:_Level_3" };
    public static readonly Map SlaversExileLevel2 = new() { Id = 620, Name = "Slaver's Exile", WikiUrl = "https://wiki.guildwars.com/wiki/Slaver's_Exile" };
    public static readonly Map SlaversExileLevel3 = new() { Id = 621, Name = "Slaver's Exile", WikiUrl = "https://wiki.guildwars.com/wiki/Slaver's_Exile" };
    public static readonly Map SlaversExileLevel4 = new() { Id = 622, Name = "Slaver's Exile", WikiUrl = "https://wiki.guildwars.com/wiki/Slaver's_Exile" };
    public static readonly Map SlaversExileLevel5 = new() { Id = 623, Name = "Slaver's Exile", WikiUrl = "https://wiki.guildwars.com/wiki/Slaver's_Exile" };
    public static readonly Map VloxsFalls = new() { Id = 624, Name = "Vlox's Falls", WikiUrl = "https://wiki.guildwars.com/wiki/Vlox's_Falls" };
    public static readonly Map BattledepthsLevel1 = new() { Id = 625, Name = "Battledepths", WikiUrl = "https://wiki.guildwars.com/wiki/Battledepths" };
    public static readonly Map BattledepthsLevel2 = new() { Id = 626, Name = "Battledepths: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Battledepths" };
    public static readonly Map BattledepthsLevel3 = new() { Id = 627, Name = "Battledepths: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Battledepths" };
    public static readonly Map SepulchreOfDragrimmarLevel1 = new() { Id = 628, Name = "Sepulchre of Dragrimmar", WikiUrl = "https://wiki.guildwars.com/wiki/Sepulchre_of_Dragrimmar" };
    public static readonly Map SepulchreOfDragrimmarLevel2 = new() { Id = 629, Name = "Sepulchre of Dragrimmar: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Sepulchre_of_Dragrimmar" };
    public static readonly Map FrostmawsBurrowsLevel1 = new() { Id = 630, Name = "Frostmaws Burrows", WikiUrl = "https://wiki.guildwars.com/wiki/Frostmaws_Burrows" };
    public static readonly Map FrostmawsBurrowsLevel2 = new() { Id = 631, Name = "Frostmaws Burrows: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Frostmaw's_Burrows" };
    public static readonly Map FrostmawsBurrowsLevel3 = new() { Id = 632, Name = "Frostmaws Burrows: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Frostmaw's_Burrows" };
    public static readonly Map FrostmawsBurrowsLevel4 = new() { Id = 633, Name = "Frostmaws Burrows: Level 4", WikiUrl = "https://wiki.guildwars.com/wiki/Frostmaw's_Burrows" };
    public static readonly Map FrostmawsBurrowsLevel5 = new() { Id = 634, Name = "Frostmaws Burrows: Level 5", WikiUrl = "https://wiki.guildwars.com/wiki/Frostmaw's_Burrows" };
    public static readonly Map DarkrimeDelvesLevel1 = new() { Id = 635, Name = "Darkrime Delves", WikiUrl = "https://wiki.guildwars.com/wiki/Darkrime_Delves" };
    public static readonly Map DarkrimeDelvesLevel2 = new() { Id = 636, Name = "Darkrime Delves: Level 2", WikiUrl = "https://wiki.guildwars.com/wiki/Darkrime_Delves" };
    public static readonly Map DarkrimeDelvesLevel3 = new() { Id = 637, Name = "Darkrime Delves: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Darkrime_Delves" };
    public static readonly Map GaddsEncampmentOutpost = new() { Id = 638, Name = "Gadd's Encampment", WikiUrl = "https://wiki.guildwars.com/wiki/Gadd's_Encampment" };
    public static readonly Map UmbralGrottoOutpost = new() { Id = 639, Name = "Umbral Grotto", WikiUrl = "https://wiki.guildwars.com/wiki/Umbral_Grotto" };
    public static readonly Map RataSumOutpost = new() { Id = 640, Name = "Rata Sum", WikiUrl = "https://wiki.guildwars.com/wiki/Rata_Sum" };
    public static readonly Map TarnishedHavenOutpost = new() { Id = 641, Name = "Tarnished Haven", WikiUrl = "https://wiki.guildwars.com/wiki/Tarnished_Haven" };
    public static readonly Map EyeOfTheNorthOutpost = new() { Id = 642, Name = "Eye of the North", WikiUrl = "https://wiki.guildwars.com/wiki/Eye_of_the_North" };
    public static readonly Map SifhallaOutpost = new() { Id = 643, Name = "Sifhalla", WikiUrl = "https://wiki.guildwars.com/wiki/Sifhalla" };
    public static readonly Map GunnarsHoldOutpost = new() { Id = 644, Name = "Gunnar's Hold", WikiUrl = "https://wiki.guildwars.com/wiki/Gunnar's_Hold" };
    public static readonly Map OlafsteadOutpost = new() { Id = 645, Name = "Olafstead", WikiUrl = "https://wiki.guildwars.com/wiki/Olafstead" };
    public static readonly Map HallOfMonuments = new() { Id = 646, Name = "Hall of Monuments", WikiUrl = "https://wiki.guildwars.com/wiki/Hall_of_Monuments" };
    public static readonly Map DaladaUplands = new() { Id = 647, Name = "Dalada Uplands", WikiUrl = "https://wiki.guildwars.com/wiki/Dalada_Uplands" };
    public static readonly Map DoomloreShrineOutpost = new() { Id = 648, Name = "Doomlore Shrine", WikiUrl = "https://wiki.guildwars.com/wiki/Doomlore_Shrine" };
    public static readonly Map GrothmarWardowns = new() { Id = 649, Name = "Grothmar Wardowns", WikiUrl = "https://wiki.guildwars.com/wiki/Grothmar_Wardowns" };
    public static readonly Map LongeyesLedgeOutpost = new() { Id = 650, Name = "Longeye's Ledge", WikiUrl = "https://wiki.guildwars.com/wiki/Longeye's_Ledge" };
    public static readonly Map SacnothValley = new() { Id = 651, Name = "Sacnoth Valley", WikiUrl = "https://wiki.guildwars.com/wiki/Sacnoth_Valley" };
    public static readonly Map CentralTransferChamberOutpost = new() { Id = 652, Name = "Central Transfer Chamber", WikiUrl = "https://wiki.guildwars.com/wiki/Central_Transfer_Chamber" };
    public static readonly Map CurseOfTheNornbear = new() { Id = 653, Name = "Curse of the Nornbear", WikiUrl = "https://wiki.guildwars.com/wiki/Curse_of_the_Nornbear" };
    public static readonly Map BloodWashesBlood = new() { Id = 654, Name = "Blood Washes Blood", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_Washes_Blood" };
    public static readonly Map AGateTooFarLevel1 = new() { Id = 655, Name = "A Gate Too Far", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gate_Too_Far" };
    public static readonly Map AGateTooFarLevel2 = new() { Id = 656, Name = "A Gate Too Far", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gate_Too_Far" };
    public static readonly Map AGateTooFarLevel3 = new() { Id = 657, Name = "A Gate Too Far", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gate_Too_Far" };
    public static readonly Map TheElusiveGolemancerLevel1 = new() { Id = 658, Name = "Oola's Laboratory", WikiUrl = "https://wiki.guildwars.com/wiki/Oola's_Laboratory" };
    public static readonly Map TheElusiveGolemancerLevel2 = new() { Id = 659, Name = "Oola's Laboratory", WikiUrl = "https://wiki.guildwars.com/wiki/Oola's_Laboratory" };
    public static readonly Map TheElusiveGolemancerLevel3 = new() { Id = 660, Name = "Oola's Laboratory", WikiUrl = "https://wiki.guildwars.com/wiki/Oola's_Laboratory" };
    public static readonly Map FindingTheBloodstoneLevel1 = new() { Id = 661, Name = "Finding The Bloodstone", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_the_Bloodstone" };
    public static readonly Map FindingTheBloodstoneLevel2 = new() { Id = 662, Name = "Finding The Bloodstone", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_the_Bloodstone" };
    public static readonly Map FindingTheBloodstoneLevel3 = new() { Id = 663, Name = "Finding The Bloodstone", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_the_Bloodstone" };
    public static readonly Map GeniusOperatedLivingEnchantedManifestation = new() { Id = 664, Name = "Genius Operated Living Enchanted Manifestation", WikiUrl = "https://wiki.guildwars.com/wiki/Genius_Operated_Living_Enchanted_Manifestation" };
    public static readonly Map AgainstTheCharr = new() { Id = 665, Name = "Against the Charr", WikiUrl = "https://wiki.guildwars.com/wiki/Against_the_Charr" };
    public static readonly Map WarbandOfBrothersLevel1 = new() { Id = 666, Name = "Warband of Brothers", WikiUrl = "https://wiki.guildwars.com/wiki/Warband_of_Brothers" };
    public static readonly Map WarbandOfBrothersLevel2 = new() { Id = 667, Name = "Warband of Brothers", WikiUrl = "https://wiki.guildwars.com/wiki/Warband_of_Brothers" };
    public static readonly Map WarbandOfBrothersLevel3 = new() { Id = 668, Name = "Warband of Brothers", WikiUrl = "https://wiki.guildwars.com/wiki/Warband_of_Brothers" };
    public static readonly Map AssaultOnTheStronghold = new() { Id = 669, Name = "Freeing the Vanguard", WikiUrl = "https://wiki.guildwars.com/wiki/Assault_on_the_Stronghold" };
    public static readonly Map DestructionsDepthsLevel1 = new() { Id = 670, Name = "Destruction's Depths", WikiUrl = "https://wiki.guildwars.com/wiki/Destruction's_Depths" };
    public static readonly Map DestructionsDepthsLevel2 = new() { Id = 671, Name = "Destruction's Depths", WikiUrl = "https://wiki.guildwars.com/wiki/Destruction's_Depths" };
    public static readonly Map DestructionsDepthsLevel3 = new() { Id = 672, Name = "Destruction's Depths", WikiUrl = "https://wiki.guildwars.com/wiki/Destruction's_Depths" };
    public static readonly Map ATimeForHeroes = new() { Id = 673, Name = "A Time for Heroes", WikiUrl = "https://wiki.guildwars.com/wiki/A_Time_for_Heroes" };
    public static readonly Map WarbandTraining = new() { Id = 674, Name = "Steppe Practice", WikiUrl = "https://wiki.guildwars.com/wiki/Warband_Training" };
    public static readonly Map BorealStationOutpost = new() { Id = 675, Name = "Boreal Station", WikiUrl = "https://wiki.guildwars.com/wiki/Boreal_Station" };
    public static readonly Map CatacombsofKathandraxLevel3 = new() { Id = 676, Name = "Catacombs of Kathandrax: Level 3", WikiUrl = "https://wiki.guildwars.com/wiki/Catacombs_of_Kathandrax:_Level_3" };
    public static readonly Map AttackoftheNornbear = new() { Id = 677, Name = "Mountain Holdout", WikiUrl = "https://wiki.guildwars.com/wiki/Attack_of_the_Nornbear" };
    public static readonly Map CinematicCaveNornCursed = new() { Id = 679, Name = "Cinematic Cave Norn Cursed", WikiUrl = "https://wiki.guildwars.com/wiki/Attack_of_the_Nornbear" };
    public static readonly Map CinematicSteppeInterrogation = new() { Id = 680, Name = "Cinematic Steppe Interrogation", WikiUrl = "https://wiki.guildwars.com/wiki/Warband_Training" };
    public static readonly Map CinematicInteriorResearch = new() { Id = 681, Name = "Cinematic Interior Research", WikiUrl = "" };
    public static readonly Map CinematicEyeVisionA = new() { Id = 682, Name = "Cinematic Eye Vision A", WikiUrl = "https://wiki.guildwars.com/wiki/Hall_of_Monuments" };
    public static readonly Map CinematicEyeVisionB = new() { Id = 683, Name = "Cinematic Eye Vision B", WikiUrl = "https://wiki.guildwars.com/wiki/Hall_of_Monuments" };
    public static readonly Map CinematicEyeVisionC = new() { Id = 684, Name = "Cinematic Eye Vision C", WikiUrl = "https://wiki.guildwars.com/wiki/Hall_of_Monuments" };
    public static readonly Map CinematicEyeVisionD = new() { Id = 685, Name = "Cinematic Eye Vision D", WikiUrl = "https://wiki.guildwars.com/wiki/Hall_of_Monuments" };
    public static readonly Map PolymockColiseum = new() { Id = 686, Name = "Polymock Coliseum", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock_Coliseum" };
    public static readonly Map PolymockGlacier = new() { Id = 687, Name = "Polymock Glacier", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock_Glacier" };
    public static readonly Map PolymockCrossing = new() { Id = 688, Name = "Polymock Crossing", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock_Crossing" };
    public static readonly Map CinematicMountainResolution = new() { Id = 689, Name = "Cinematic Mountain Resolution", WikiUrl = "" };
    public static readonly Map ColdasIce = new() { Id = 690, Name = "<Mountain Polar OP>", WikiUrl = "" };
    public static readonly Map BeneathLionsArch = new() { Id = 691, Name = "Beneath Lions Arch", WikiUrl = "" };
    public static readonly Map TunnelsBelowCantha = new() { Id = 692, Name = "Tunnels Below Cantha", WikiUrl = "https://wiki.guildwars.com/wiki/Tunnels_Below_Cantha" };
    public static readonly Map CavernsBelowKamadan = new() { Id = 693, Name = "Caverns Below Kamadan", WikiUrl = "https://wiki.guildwars.com/wiki/Caverns_Below_Kamadan" };
    public static readonly Map CinematicMountainDwarfs = new() { Id = 694, Name = "Cinematic Mountain Dwarfs", WikiUrl = "" };
    public static readonly Map ServiceInDefenseoftheEye = new() { Id = 695, Name = "The Eye of the North", WikiUrl = "https://wiki.guildwars.com/wiki/Eye_of_the_North_(outpost)" };
    public static readonly Map ManoaNorno = new() { Id = 696, Name = "The Eye of the North", WikiUrl = "https://wiki.guildwars.com/wiki/Eye_of_the_North_(outpost)" };
    public static readonly Map ServicePracticeDummy = new() { Id = 697, Name = "The Eye of the North", WikiUrl = "https://wiki.guildwars.com/wiki/Eye_of_the_North_(outpost)" };
    public static readonly Map HeroTutorial = new() { Id = 698, Name = "Hero Tutorial", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_Tutorial" };
    public static readonly Map TheNornFightingTournament = new() { Id = 699, Name = "The Norn Fighting Tournament", WikiUrl = "https://wiki.guildwars.com/wiki/The_Norn_Fighting_Tournament" };
    public static readonly Map SecretLairOftheSnowmen = new() { Id = 701, Name = "Hundar's Resplendent Treasure Vault", WikiUrl = "" };
    public static readonly Map NornBrawlingChampionship = new() { Id = 702, Name = "Norn Brawling Championship", WikiUrl = "https://wiki.guildwars.com/wiki/Norn_Brawling_Championship" };
    public static readonly Map KilroysPunchoutTraining = new() { Id = 703, Name = "Kilroys Punchout Training", WikiUrl = "" };
    public static readonly Map FronisIrontoesLairMission = new() { Id = 704, Name = "Fronis Irontoe's Lair", WikiUrl = "https://wiki.guildwars.com/wiki/Fronis_Irontoe's_Lair" };
    public static readonly Map TheJusticiarsEnd = new() { Id = 705, Name = "INTERIOR_WATCH_OP1_FINAL_BATTLE", WikiUrl = "" };
    public static readonly Map TheGreatNornAlemoot = new() { Id = 706, Name = "The Great Norn Alemoot", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Norn_Alemoot" };
    public static readonly Map VarajarFellsunknown = new() { Id = 708, Name = "(Mountain Traverse)", WikiUrl = "" };
    public static readonly Map Epilogue = new() { Id = 709, Name = "Destroyer Ending", WikiUrl = "" };
    public static readonly Map InsidiousRemnants = new() { Id = 711, Name = "Alcazia Tangle", WikiUrl = "https://wiki.guildwars.com/wiki/Alcazia_Tangle" };
    public static readonly Map AttackonJalissCamp = new() { Id = 712, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map CostumeBrawlOutpost = new() { Id = 718, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map WhitefuryRapidsMission = new() { Id = 722, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map KystenShoreMission = new() { Id = 723, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map DeepwayRuinsMission = new() { Id = 724, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map PlikkupWorksMission = new() { Id = 725, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map KilroysPunchoutTournament = new() { Id = 726, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map SpecialOpsFlameTempleCorridor = new() { Id = 727, Name = "Plains of Jarin", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin" };
    public static readonly Map SpecialOpsDragonsGullet = new() { Id = 728, Name = "(crash)", WikiUrl = "" };
    public static readonly Map SpecialOpsGrendichCourthouse = new() { Id = 729, Name = "Special Ops Grendich Courthouse", WikiUrl = "https://wiki.guildwars.com/wiki/Grendich_Courthouse" };
    public static readonly Map TheTenguAccords = new() { Id = 730, Name = "The Tengu Accords", WikiUrl = "https://wiki.guildwars.com/wiki/_The_Tengu_Accords" };
    public static readonly Map TheBattleOfJahai = new() { Id = 731, Name = "The Battle of Jahai", WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_of_Jahai" };
    public static readonly Map TheFlightNorth = new() { Id = 732, Name = "The Flight North", WikiUrl = "https://wiki.guildwars.com/wiki/_The_Flight_North" };
    public static readonly Map TheRiseOfTheWhiteMantle = new() { Id = 733, Name = "The Rise of the White Mantle", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rise_of_the_White_Mantle" };
    public static readonly Map FindingTheBloodstoneMission = new() { Id = 734, Name = "Finding the Bloodstone Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_the_Bloodstone" };
    public static readonly Map GeniusOperatedLivingEnchantedManifestationMission = new() { Id = 760, Name = "Genius Operated Living Enchanted Manifestation Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Genius_Operated_Living_Enchanted_Manifestation" };
    public static readonly Map AgainstTheCharrMission = new() { Id = 761, Name = "Against the Charr Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Against_the_Charr" };
    public static readonly Map WarbandOfBrothersMission = new() { Id = 762, Name = "Warband of brothers Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Warband_of_Brothers" };
    public static readonly Map AssaultOnTheStrongholdMission = new() { Id = 763, Name = "Assault on the Stronghold Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Assault_on_the_Stronghold" };
    public static readonly Map DestructionsDepthsMission = new() { Id = 764, Name = "Destructions Depths Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Destruction's_Depths" };
    public static readonly Map ATimeForHeroesMission = new() { Id = 765, Name = "A Time for Heroes Mission", WikiUrl = "https://wiki.guildwars.com/wiki/A_Time_for_Heroes" };
    public static readonly Map CurseOfTheNornbearMission = new() { Id = 766, Name = "Curse of the Nornbear Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Curse_of_the_Nornbear" };
    public static readonly Map BloodWashesBloodMission = new() { Id = 767, Name = "Blood Washes Blood Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_Washes_Blood" };
    public static readonly Map AGateTooFarMission = new() { Id = 768, Name = "A Gate Too Far Mission", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gate_Too_Far" };
    public static readonly Map TheElusiveGolemancerMission = new() { Id = 769, Name = "The Elusive Golemancer Mission", WikiUrl = "https://wiki.guildwars.com/wiki/The_Elusive_Golemancer" };
    public static readonly Map SecretLairOftheSnowmen2 = new() { Id = 770, Name = "Secret Lair of the Snowmen2", WikiUrl = "https://wiki.guildwars.com/wiki/Secret_Lair_of_the_Snowmen" };
    public static readonly Map SecretLairOftheSnowmen3 = new() { Id = 782, Name = "Secret Lair of the Snowmen3", WikiUrl = "https://wiki.guildwars.com/wiki/Secret_Lair_of_the_Snowmen" };
    public static readonly Map DroknarsForgeCinematic = new() { Id = 783, Name = "Droknars Forge (cinematic)", WikiUrl = "https://wiki.guildwars.com/wiki/Droknar's_Forge" };
    public static readonly Map IsleOfTheNamelessPvP = new() { Id = 784, Name = "Isle of the Nameless PvP", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_the_Nameless_(PvP)" };
    public static readonly Map TempleOfTheAgesROX = new() { Id = 785, Name = "Temple of the Ages ROX", WikiUrl = "https://wiki.guildwars.com/wiki/Deactivating_R.O.X." };
    public static readonly Map WajjunBazaarPOX = new() { Id = 789, Name = "Wajjun Bazaar POX", WikiUrl = "https://wiki.guildwars.com/wiki/Deactivating_P.O.X." };
    public static readonly Map BokkaAmphitheatreNOX = new() { Id = 790, Name = "Bokka Amphitheatre NOX", WikiUrl = "https://wiki.guildwars.com/wiki/Deactivating_N.O.X." };
    public static readonly Map SecretUndergroundLair = new() { Id = 791, Name = "Secret Underground Lair", WikiUrl = "https://wiki.guildwars.com/wiki/_Secret_Underground_Lair" };
    public static readonly Map GolemTutorialSimulation = new() { Id = 792, Name = "Golem Tutorial Simulation", WikiUrl = "https://wiki.guildwars.com/wiki/_Golem_Tutorial_Simulation" };
    public static readonly Map SnowballDominance = new() { Id = 793, Name = "Snowball Dominance", WikiUrl = "https://wiki.guildwars.com/wiki/_Snowball_Dominance" };
    public static readonly Map ZaishenMenagerieGrounds = new() { Id = 794, Name = "Zaishen Menagerie Grounds", WikiUrl = "https://wiki.guildwars.com/wiki/_Zaishen_Menagerie_Grounds" };
    public static readonly Map ZaishenMenagerieOutpost = new() { Id = 795, Name = "Zaishen Menagerie (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_Menagerie_(outpost)" };
    public static readonly Map CodexArenaOutpost = new() { Id = 796, Name = "Codex Arena (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Codex_Arena_(outpost)" };
    public static readonly Map TheUnderworldSomethingWickedThisWayComes = new() { Id = 797, Name = "The Underworld Something Wicked This Way Comes", WikiUrl = "https://wiki.guildwars.com/wiki/Something_Wicked_This_Way_Comes" };
    public static readonly Map TheUnderworldDontFeartheReapers = new() { Id = 807, Name = "The Underworld Don't Fear the Reapers", WikiUrl = "https://wiki.guildwars.com/wiki/Don't_Fear_the_Reapers" };
    public static readonly Map LionsArchHalloweenOutpost = new() { Id = 808, Name = "Lions Arch Halloween (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Lion's_Arch_(Halloween)" };
    public static readonly Map LionsArchWintersdayOutpost = new() { Id = 809, Name = "Lions Arch Wintersday (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Lion's_Arch_(Wintersday)" };
    public static readonly Map LionsArchCanthanNewYearOutpost = new() { Id = 810, Name = "Lions Arch Canthan New Year (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Lion's_Arch" };
    public static readonly Map AscalonCityWintersdayOutpost = new() { Id = 811, Name = "Ascalon City Wintersday (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_City_(Wintersday)" };
    public static readonly Map DroknarsForgeHalloweenOutpost = new() { Id = 812, Name = "Droknars Forge Halloween (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Droknar's_Forge_(Halloween)" };
    public static readonly Map DroknarsForgeWintersdayOutpost = new() { Id = 813, Name = "Droknars Forge Wintersday (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Droknar's_Forge_(Halloween)" };
    public static readonly Map TombOfThePrimevalKingsHalloweenOutpost = new() { Id = 814, Name = "Tomb of the Primeval Kings Halloween (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Tomb_of_the_Primeval_Kings_(Halloween)" };
    public static readonly Map ShingJeaMonasteryDragonFestivalOutpost = new() { Id = 815, Name = "Shing Jea Monastery Dragon Festival (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Shing_Jea_Monastery" };
    public static readonly Map ShingJeaMonasteryCanthanNewYearOutpost = new() { Id = 816, Name = "Shing Jea Monastery Canthan New Year (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Shing_Jea_Monastery" };
    public static readonly Map KainengCenterCanthanNewYearOutpost = new() { Id = 817, Name = "Kaineng Center Canthan New Year (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Kaineng_Center" };
    public static readonly Map KamadanJewelOfIstanHalloweenOutpost = new() { Id = 818, Name = "Kamadan Jewel of Istan Halloween (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Kamadan,_Jewel_of_Istan_(Halloween)" };
    public static readonly Map KamadanJewelOfIstanWintersdayOutpost = new() { Id = 819, Name = "Kamadan Jewel of Istan Wintersday (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Kamadan,_Jewel_of_Istan_(Wintersday)" };
    public static readonly Map KamadanJewelOfIstanCanthanNewYearOutpost = new() { Id = 820, Name = "Kamadan Jewel of Istan Canthan New Year (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Kamadan,_Jewel_of_Istan" };
    public static readonly Map EyeOfTheNorthOutpostWintersdayOutpost = new() { Id = 821, Name = "Eyeofthe North Outpost Wintersday (Outpost)", WikiUrl = "https://wiki.guildwars.com/wiki/Eye_of_the_North_(outpost)_(Wintersday)" };
    public static readonly Map WarinKrytaTalmarkWilderness = new() { Id = 822, Name = "War in Kryta Talmark Wilderness", WikiUrl = "https://wiki.guildwars.com/wiki/Talmark_Wilderness_(War_in_Kryta)" };
    public static readonly Map WarinKrytaTrialofZinn = new() { Id = 838, Name = "War in Kryta Trialof Zinn", WikiUrl = "https://wiki.guildwars.com/wiki/Trial_of_Zinn" };
    public static readonly Map WarinKrytaDivinityCoast = new() { Id = 839, Name = "War in Kryta Divinity Coast", WikiUrl = "https://wiki.guildwars.com/wiki/Divinity_Coast_(explorable_area)" };
    public static readonly Map WarinKrytaLionsArchKeep = new() { Id = 840, Name = "War in Kryta Lions Arch Keep", WikiUrl = "https://wiki.guildwars.com/wiki/Lion's_Arch_(War_in_Kryta)" };
    public static readonly Map WarinKrytaDAlessioSeaboard = new() { Id = 841, Name = "War in Kryta D'Alessio Seaboard", WikiUrl = "https://wiki.guildwars.com/wiki/D'Alessio_Seaboard_(explorable_area)" };
    public static readonly Map WarinKrytaTheBattleforLionsArch = new() { Id = 842, Name = "War in Kryta The Battle for Lions Arch", WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_for_Lion's_Arch" };
    public static readonly Map WarinKrytaRiversideProvince = new() { Id = 843, Name = "War in Kryta Riverside Province", WikiUrl = "https://wiki.guildwars.com/wiki/Riverside_Province_(explorable_area)" };
    public static readonly Map WarinKrytaLionsArch = new() { Id = 844, Name = "War in Kryta Lions Arch", WikiUrl = "https://wiki.guildwars.com/wiki/Lion's_Arch_(War_in_Kryta)" };
    public static readonly Map WarinKrytaTheMausoleum = new() { Id = 845, Name = "War in Kryta The Mausoleum", WikiUrl = "https://wiki.guildwars.com/wiki/The_Mausoleum" };
    public static readonly Map WarinKrytaRise = new() { Id = 846, Name = "War in Kryta Rise", WikiUrl = "https://wiki.guildwars.com/wiki/Rise" };
    public static readonly Map WarinKrytaShadowsintheJungle = new() { Id = 847, Name = "War in Kryta Shadows in the Jungle", WikiUrl = "https://wiki.guildwars.com/wiki/Shadows_in_the_Jungle" };
    public static readonly Map WarinKrytaAVengeanceofBlades = new() { Id = 848, Name = "War in Kryta A Vengeance of Blades", WikiUrl = "https://wiki.guildwars.com/wiki/A_Vengeance_of_Blades" };
    public static readonly Map WarinKrytaAuspiciousBeginnings = new() { Id = 849, Name = "War in Kryta Auspicious Beginnings", WikiUrl = "https://wiki.guildwars.com/wiki/Auspicious_Beginnings" };
    public static readonly Map OlafsteadCinematic = new() { Id = 850, Name = "Olafstead (cinematic)", WikiUrl = "https://wiki.guildwars.com/wiki/Olafstead" };
    public static readonly Map TheGreatSnowballFightoftheGodsOperationCrushSpirits = new() { Id = 855, Name = "The Great Snowball Fight of the Gods Operation Crush Spirits", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Snowball_Fight_of_the_Gods_(outpost)" };
    public static readonly Map TheGreatSnowballFightoftheGodsFightinginaWinterWonderland = new() { Id = 856, Name = "The Great Snowball Fight of the Gods Fighting in a Winter Wonderland", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Snowball_Fight_of_the_Gods" };
    public static readonly Map EmbarkBeach = new() { Id = 857, Name = "Embark Beach", WikiUrl = "https://wiki.guildwars.com/wiki/_Embark_Beach" };
    public static readonly Map DragonsThroatAreaWhatWaitsInShadow = new() { Id = 858, Name = "What Waits in Shadow", WikiUrl = "https://wiki.guildwars.com/wiki/What_Waits_in_Shadow" };
    public static readonly Map KainengCenterWindsOfChangeAChanceEncounter = new() { Id = 861, Name = "Winds of Change A Chance Encounter", WikiUrl = "https://wiki.guildwars.com/wiki/A_Chance_Encounter" };
    public static readonly Map TheMarketplaceAreaTrackingtheCorruption = new() { Id = 862, Name = "Tracking The Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Tracking_the_Corruption" };
    public static readonly Map BukdekBywayWindsOfChangeCanthaCourierCrisis = new() { Id = 863, Name = "Cantha Courier Crisis", WikiUrl = "https://wiki.guildwars.com/wiki/Cantha_Courier_Crisis" };
    public static readonly Map TsumeiVillageWindsOfChangeATreatysATreaty = new() { Id = 864, Name = "A Treaty's a Treaty", WikiUrl = "https://wiki.guildwars.com/wiki/A_Treaty's_a_Treaty" };
    public static readonly Map SeitungHarborAreaDeadlyCargo = new() { Id = 865, Name = "Deadly Cargo", WikiUrl = "https://wiki.guildwars.com/wiki/Deadly_Cargo" };
    public static readonly Map TahnnakaiTempleWindsOfChangeTheRescueAttempt = new() { Id = 866, Name = "The Rescue Attempt", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rescue_Attempt" };
    public static readonly Map WajjunBazaarWindsOfChangeViolenceInTheStreets = new() { Id = 867, Name = "Violence in the Streets", WikiUrl = "https://wiki.guildwars.com/wiki/Violence_in_the_Streets" };
    public static readonly Map ScarredPsycheMission = new() { Id = 868, Name = "Scarred Psyche Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Scarred_Psyche" };
    public static readonly Map ShadowsPassageWindsofChangeCallingAllThugs = new() { Id = 869, Name = "Calling All Thugs", WikiUrl = "https://wiki.guildwars.com/wiki/Calling_All_Thugs" };
    public static readonly Map AltrummRuinsFindingJinnai = new() { Id = 870, Name = "Finding Jinnai", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_Jinnai" };
    public static readonly Map ShingJeaMonasteryRaidOnShingJeaMonastery = new() { Id = 871, Name = "Raid on Shing Jea Monastery", WikiUrl = "https://wiki.guildwars.com/wiki/Raid_on_Shing_Jea_Monastery" };
    public static readonly Map KainengCenterWindsOfChangeRaidonKainengCenter = new() { Id = 872, Name = "Raid on Kaineng Center", WikiUrl = "https://wiki.guildwars.com/wiki/Raid_on_Kaineng_Center" };
    public static readonly Map WajjunBazaarWindsOfChangeMinistryOfOppression = new() { Id = 873, Name = "Ministry of Oppression", WikiUrl = "https://wiki.guildwars.com/wiki/Ministry_of_Oppression" };
    public static readonly Map TheFinalConfrontation = new() { Id = 874, Name = "The Final Confrontation", WikiUrl = "https://wiki.guildwars.com/wiki/_The_Final_Confrontation" };
    public static readonly Map LakesideCounty1070AE = new() { Id = 875, Name = "Lakeside County 1070AE", WikiUrl = "https://wiki.guildwars.com/wiki/Lakeside_County:_1070_AE" };
    public static readonly Map AshfordCatacombs1070AE = new() { Id = 876, Name = "Ashford Catacombs 1070AE", WikiUrl = "https://wiki.guildwars.com/wiki/Ashford_Catacombs:_1070_AE" };
    public static readonly Map Count = new() { Id = 877, Name = "Count", WikiUrl = "" };

    public static IEnumerable<Map> Maps { get; } = new List<Map>
    {
        None,
        GladiatorsArena,
        DEVTestArena1v1,
        Testmap,
        WarriorsIsleOutpost,
        HuntersIsleOutpost,
        WizardsIsleOutpost,
        WarriorsIsle,
        HuntersIsle,
        WizardsIsle,
        BloodstoneFen,
        TheWilds,
        AuroraGlade,
        DiessaLowlands,
        GatesOfKryta,
        DAlessioSeaboard,
        DivinityCoast,
        TalmarkWilderness,
        TheBlackCurtain,
        SanctumCay,
        DroknarsForgeOutpost,
        TheFrostGate,
        IceCavesofSorrow,
        ThunderheadKeep,
        IronMinesofMoladune,
        BorlisPass,
        TalusChute,
        GriffonsMouth,
        TheGreatNorthernWall,
        FortRanik,
        RuinsOfSurmia,
        XaquangSkyway,
        NolaniAcademy,
        OldAscalon,
        TheFissureofWoe,
        EmberLightCampOutpost,
        GrendichCourthouseOutpost,
        GlintsChallengeMission,
        AuguryRockOutpost,
        SardelacSanitariumOutpost,
        PikenSquareOutpost,
        SageLands,
        MamnoonLagoon,
        Silverwood,
        EttinsBack,
        ReedBog,
        TheFalls,
        DryTop,
        TangleRoot,
        HengeOfDenraviOutpost,
        SenjisCornerOutpost,
        BurningIsleOutpost,
        TearsOfTheFallen,
        ScoundrelsRise,
        LionsArchOutpost,
        CursedLands,
        BergenHotSpringsOutpost,
        NorthKrytaProvince,
        NeboTerrace,
        MajestysRest,
        TwinSerpentLakes,
        WatchtowerCoast,
        StingrayStrand,
        KessexPeak,
        DAlessioArenaMission,
        BurningIsle,
        FrozenIsle,
        NomadsIsle,
        DruidsIsle,
        IsleOfTheDeadGuildHall,
        TheUnderworld,
        RiversideProvince,
        TheHallOfHeroesArenaMission,
        BrokenTowerMission,
        HouseZuHeltzerOutpost,
        TheCourtyardArenaMission,
        UnholyTemplesMission,
        BurialMoundsMission,
        AscalonCityOutpost,
        TombOfThePrimevalKings,
        TheVaultMission,
        TheUnderworldArenaMission,
        AscalonArena,
        SacredTemplesMission,
        Icedome,
        IronHorseMine,
        AnvilRock,
        LornarsPass,
        SnakeDance,
        TascasDemise,
        SpearheadPeak,
        IceFloe,
        WitmansFolly,
        MineralSprings,
        DreadnoughtsDrift,
        FrozenForest,
        TravelersVale,
        DeldrimorBowl,
        RegentValley,
        TheBreach,
        AscalonFoothills,
        PockmarkFlats,
        DragonsGullet,
        FlameTempleCorridor,
        EasternFrontier,
        TheScar,
        TheAmnoonOasisOutpost,
        DivinersAscent,
        VultureDrifts,
        TheAridSea,
        ProphetsPath,
        SaltFlats,
        SkywardReach,
        DunesOfDespair,
        ThirstyRiver,
        ElonaReach,
        AuguryRockMission,
        TheDragonsLair,
        PerditionRock,
        RingOfFire,
        AbaddonsMouth,
        HellsPrecipice,
        GoldenGatesMission,
        ScarredEarth2,
        TheEternalGrove,
        LutgardisConservatoryOutpost,
        VasburgArmoryOutpost,
        SerenityTempleOutpost,
        IceToothCaveOutpost,
        BeaconsPerchOutpost,
        YaksBendOutpost,
        FrontierGateOutpost,
        BeetletunOutpost,
        FishermensHavenOutpost,
        TempleOfTheAges,
        VentarisRefugeOutpost,
        DruidsOverlookOutpost,
        MaguumaStadeOutpost,
        QuarrelFallsOutpost,
        GyalaHatchery,
        TheCatacombs,
        LakesideCounty,
        TheNorthlands,
        AscalonCityPresearing,
        AscalonAcademy,
        AscalonAcademyPvPBattleMission,
        AscalonAcademyunk,
        HeroesAudienceOutpost,
        SeekersPassageOutpost,
        DestinysGorgeOutpost,
        CampRankorOutpost,
        TheGraniteCitadelOutpost,
        MarhansGrottoOutpost,
        PortSledgeOutpost,
        CopperhammerMinesOutpost,
        GreenHillsCounty,
        WizardsFolly,
        RegentValleyPreSearing,
        TheBarradinEstateOutpost,
        AshfordAbbeyOutpost,
        FoiblesFairOutpost,
        FortRanikPreSearingOutpost,
        BurningIsleMission,
        DruidsIsleMission,
        FrozenIsleMission,
        WarriorsIsleMission,
        HuntersIsleMission,
        WizardsIsleMission,
        NomadsIsleMission,
        IsleOfTheDeadGuildHallMission,
        FrozenIsleOutpost,
        NomadsIsleOutpost,
        DruidsIsleOutpost,
        IsleOfTheDeadGuildHallOutpost,
        FortKogaMission,
        ShiverpeakArena,
        AmnoonArenaMission,
        DeldrimorArenaMission,
        TheCragMission,
        RandomArenasOutpost,
        TeamArenasOutpost,
        SorrowsFurnace,
        GrenthsFootprint,
        CavalonOutpost,
        KainengCenterOutpost,
        DrazachThicket,
        JayaBluffs,
        ShenzunTunnels,
        Archipelagos,
        MaishangHills,
        MountQinkai,
        MelandrusHope,
        RheasCrater,
        SilentSurf,
        UnwakingWatersMission,
        MorostavTrail,
        DeldrimorWarCampOutpost,
        HeroesCryptMission,
        MourningVeilFalls,
        Ferndale,
        PongmeiValley,
        MonasteryOverlook1,
        ZenDaijunOutpostMission,
        MinisterChosEstateOutpostMission,
        VizunahSquareMission,
        NahpuiQuarterOutpostMission,
        TahnnakaiTempleOutpostMission,
        ArborstoneOutpostMission,
        BoreasSeabedOutpostMission,
        SunjiangDistrictOutpostMission,
        FortAspenwoodMission,
        TheEternalGroveOutpostMission,
        TheJadeQuarryMission,
        GyalaHatcheryOutpostMission,
        RaisuPalaceOutpostMission,
        ImperialSanctumOutpostMission,
        UnwakingWaters,
        GrenzFrontierMission,
        AmatzBasin,
        ShadowsPassage,
        RaisuPalace,
        TheAuriosMines,
        PanjiangPeninsula,
        KinyaProvince,
        HaijuLagoon,
        SunquaVale,
        WajjunBazaar,
        BukdekByway,
        TheUndercity,
        ShingJeaMonasteryOutpost,
        ShingJeaArena,
        ArborstoneExplorable,
        MinisterChosEstateExplorable,
        ZenDaijunExplorable,
        BoreasSeabedExplorable,
        GreatTempleOfBalthazarOutpost,
        TsumeiVillageOutpost,
        SeitungHarborOutpost,
        RanMusuGardensOutpost,
        LinnokCourtyard,
        DwaynaVsGrenth,
        SunjiangDistrictExplorable,
        NahpuiQuarterExplorable,
        UrgozsWarren,
        TahnnakaiTempleExplorable,
        AltrummRuins,
        ZosShivrosChannel,
        DragonsThroat,
        IsleOfWeepingStoneOutpost,
        IsleOfJadeOutpost,
        HarvestTempleOutpost,
        BreakerHollowOutpost,
        LeviathanPitsOutpost,
        IsleOfTheNameless,
        ZaishenChallengeOutpost,
        ZaishenEliteOutpost,
        MaatuKeepOutpost,
        ZinKuCorridorOutpost,
        MonasteryOverlook2,
        BrauerAcademyOutpost,
        DurheimArchivesOutpost,
        BaiPaasuReachOutpost,
        SeafarersRestOutpost,
        BejunkanPier,
        VizunahSquareLocalQuarterOutpost,
        VizunahSquareForeignQuarterOutpost,
        FortAspenwoodLuxonOutpost,
        FortAspenwoodKurzickOutpost,
        TheJadeQuarryLuxonOutpost,
        TheJadeQuarryKurzickOutpost,
        UnwakingWatersLuxonOutpost,
        UnwakingWatersKurzickOutpost,
        EtnaranKeysMission,
        RaisuPavilion,
        KainengDocks,
        TheMarketplaceOutpost,
        TheDeep,
        AscalonArenaMission,
        AnnihilationMission,
        KillCountTrainingMission,
        PriestAnnihilationTraining,
        ObeliskAnnihilationTrainingMission,
        SaoshangTrail,
        ShiverpeakArenaMission,
        DAlessioArenaMission3,
        AmnoonArenaMission3,
        FortKogaMission3,
        HeroesCryptMission3,
        ShiverpeakArenaMission3,
        SaltsprayBeachLuxonOutpost,
        SaltsprayBeachKurzickOutpost,
        HeroesAscentOutpost,
        GrenzFrontierLuxonOutpost,
        GrenzFrontierKurzickOutpost,
        TheAncestralLandsLuxonOutpost,
        TheAncestralLandsKurzickOutpost,
        EtnaranKeysLuxonOutpost,
        EtnaranKeysKurzickOutpost,
        KaanaiCanyonLuxonOutpost,
        KaanaiCanyonKurzickOutpost,
        DAlessioArenaMission2,
        AmnoonArenaMission2,
        FortKogaMission2,
        HeroesCryptMission2,
        ShiverpeakArenaMission2,
        TheHallofHeroes,
        TheCourtyard,
        ScarredEarth,
        TheUnderworldPvP,
        TanglewoodCopseOutpost,
        SaintAnjekasShrineOutpost,
        EredonTerraceOutpost,
        DivinePath,
        BrawlersPitMission,
        PetrifiedArenaMission,
        SeabedArenaMission,
        IsleOfWeepingStoneMission,
        IsleOfJadeMission,
        ImperialIsleMission,
        IsleOfMeditationMission,
        ImperialIsleOutpost,
        IsleOfMeditationOutpost,
        IsleOfWeepingStone,
        IsleOfJade,
        ImperialIsle,
        IsleOfMeditation,
        ShingJeaArenaMission,
        DragonArena,
        JahaiBluffs,
        KamadanMission,
        MargaCoast,
        FahranurMission,
        SunwardMarches,
        BarbarousShore,
        CampHojanuOutpost,
        BahdokCaverns,
        WehhanTerracesOutpost,
        DejarinEstate,
        ArkjokWard,
        YohlonHavenOutpost,
        GandaraTheMoonFortress,
        TheFloodplainOfMahnkelon,
        LionsArchSunspearsinKryta,
        TuraisProcession,
        SunspearSanctuaryOutpost,
        AspenwoodGateKurzickOutpost,
        AspenwoodGateLuxonOutpost,
        JadeFlatsKurzickOutpost,
        JadeFlatsLuxonOutpost,
        YatendiCanyons,
        ChantryOfSecretsOutpost,
        GardenOfSeborhin,
        HoldingsOfChokhin,
        MihanuTownshipOutpost,
        VehjinMines,
        BasaltGrottoOutpost,
        ForumHighlands,
        KainengCenterSunspearsInCantha,
        ResplendentMakuun,
        ResplendentMakuun2,
        HonurHillOutpost,
        WildernessOfBahdza,
        VehtendiValley,
        YahnurMarketOutpost,
        TheHiddenCityOfAhdashim,
        TheKodashBazaarOutpost,
        LionsGate,
        TheMirrorOfLyss,
        TheMirrorOfLyss2,
        SecuretheRefuge,
        VentaCemetery,
        KamadanJewelOfIstanExplorable,
        TheTribunal,
        KodonurCrossroads,
        RilohnRefuge,
        PogahnPassage,
        ModdokCrevice,
        TiharkOrchard,
        Consulate,
        PlainsOfJarin,
        SunspearGreatHallOutpost,
        CliffsOfDohjok,
        DzagonurBastion,
        DashaVestibule,
        GrandCourtOfSebelkeh,
        CommandPost,
        JokosDomain,
        BonePalaceOutpost,
        TheRupturedHeart,
        TheMouthOfTormentOutpost,
        TheShatteredRavines,
        LairOfTheForgottenOutpost,
        PoisonedOutcrops,
        TheSulfurousWastes,
        TheEbonyCitadelOfMallyxMission,
        TheAlkaliPan,
        ALandofHeroes,
        CrystalOverlook,
        KamadanJewelOfIstanOutpost,
        GateOfTormentOutpost,
        NightfallenGarden,
        ChurrhirFields,
        BeknurHarborOutpost,
        TheUnderworld2,
        HeartOfAbaddon,
        TheUnderworld3,
        NightfallenCoast,
        NightfallenJahai,
        DepthsOfMadness,
        RollerbeetleRacing,
        DomainOfFear,
        GateOfFearOutpost,
        DomainOfPain,
        BloodstoneFenQuest,
        DomainOfSecrets,
        GateOfSecretsOutpost,
        DomainOfAnguish,
        OozePitMission,
        JennursHorde,
        NunduBay,
        GateOfDesolation,
        ChampionsDawnOutpost,
        RuinsOfMorah,
        FahranurTheFirstCity,
        BjoraMarches,
        ZehlonReach,
        LahtendaBog,
        ArborBay,
        IssnurIsles,
        BeknurHarbor,
        MehtaniKeys,
        KodlonuHamletOutpost,
        IslandOfShehkah,
        JokanurDiggings,
        BlacktideDen,
        ConsulateDocks,
        GateOfPain,
        GateOfMadness,
        AbaddonsGate,
        SunspearArena,
        IceCliffChasms,
        BokkaAmphitheatre,
        RivenEarth,
        TheAstralariumOutpost,
        ThroneOfSecrets,
        ChurranuIslandArenaMission,
        ShingJeaMonasteryMission,
        HaijuLagoonMission,
        JayaBluffsMission,
        SeitungHarborMission,
        TsumeiVillageMission,
        SeitungHarborMission2,
        TsumeiVillageMission2,
        DrakkarLake,
        MinisterChosEstateMission2,
        UnchartedIsleOutpost,
        IsleOfWurmsOutpost,
        UnchartedIsle,
        IsleOfWurms,
        UnchartedIsleMission,
        IsleOfWurmsMission,
        SunspearArenaMission,
        CorruptedIsleOutpost,
        IsleOfSolitudeOutpost,
        CorruptedIsle,
        IsleOfSolitude,
        CorruptedIsleMission,
        IsleOfSolitudeMission,
        SunDocks,
        ChahbekVillage,
        RemainsOfSahlahja,
        JagaMoraine,
        BombardmentMission,
        NorrhartDomains,
        HeroBattlesOutpost,
        TheBeachheadMission,
        TheCrossingMission,
        DesertSandsMission,
        VarajarFells,
        DajkahInlet,
        TheShadowNexus,
        SparkflySwamp,
        GateOftheNightfallenLandsOutpost,
        CathedralofFlamesLevel1,
        TheTroubledKeeper,
        VerdantCascades,
        CathedralofFlamesLevel2,
        CathedralofFlamesLevel3,
        MagusStones,
        CatacombsofKathandraxLevel1,
        CatacombsofKathandraxLevel2,
        AlcaziaTangle,
        RragarsMenagerieLevel1,
        RragarsMenagerieLevel2,
        RragarsMenagerieLevel3,
        OozePit,
        SlaversExileLevel1,
        OolasLabLevel1,
        OolasLabLevel2,
        OolasLabLevel3,
        ShardsOfOrrLevel1,
        ShardsOfOrrLevel2,
        ShardsOfOrrLevel3,
        ArachnisHauntLevel1,
        ArachnisHauntLevel2,
        FetidRiverMission,
        ForgottenShrinesMission,
        AntechamberMission,
        VloxenExcavationsLevel1,
        VloxenExcavationsLevel2,
        VloxenExcavationsLevel3,
        HeartOftheShiverpeaksLevel1,
        HeartOftheShiverpeaksLevel2,
        HeartOftheShiverpeaksLevel3,
        BloodstoneCavesLevel1,
        BloodstoneCavesLevel2,
        BloodstoneCavesLevel3,
        BogrootGrowthsLevel1,
        BogrootGrowthsLevel2,
        RavensPointLevel1,
        RavensPointLevel2,
        RavensPointLevel3,
        SlaversExileLevel2,
        SlaversExileLevel3,
        SlaversExileLevel4,
        SlaversExileLevel5,
        VloxsFalls,
        BattledepthsLevel1,
        BattledepthsLevel2,
        BattledepthsLevel3,
        SepulchreOfDragrimmarLevel1,
        SepulchreOfDragrimmarLevel2,
        FrostmawsBurrowsLevel1,
        FrostmawsBurrowsLevel2,
        FrostmawsBurrowsLevel3,
        FrostmawsBurrowsLevel4,
        FrostmawsBurrowsLevel5,
        DarkrimeDelvesLevel1,
        DarkrimeDelvesLevel2,
        DarkrimeDelvesLevel3,
        GaddsEncampmentOutpost,
        UmbralGrottoOutpost,
        RataSumOutpost,
        TarnishedHavenOutpost,
        EyeOfTheNorthOutpost,
        SifhallaOutpost,
        GunnarsHoldOutpost,
        OlafsteadOutpost,
        HallOfMonuments,
        DaladaUplands,
        DoomloreShrineOutpost,
        GrothmarWardowns,
        LongeyesLedgeOutpost,
        SacnothValley,
        CentralTransferChamberOutpost,
        CurseOfTheNornbear,
        BloodWashesBlood,
        AGateTooFarLevel1,
        AGateTooFarLevel2,
        AGateTooFarLevel3,
        TheElusiveGolemancerLevel1,
        TheElusiveGolemancerLevel2,
        TheElusiveGolemancerLevel3,
        FindingTheBloodstoneLevel1,
        FindingTheBloodstoneLevel2,
        FindingTheBloodstoneLevel3,
        GeniusOperatedLivingEnchantedManifestation,
        AgainstTheCharr,
        WarbandOfBrothersLevel1,
        WarbandOfBrothersLevel2,
        WarbandOfBrothersLevel3,
        AssaultOnTheStronghold,
        DestructionsDepthsLevel1,
        DestructionsDepthsLevel2,
        DestructionsDepthsLevel3,
        ATimeForHeroes,
        WarbandTraining,
        BorealStationOutpost,
        CatacombsofKathandraxLevel3,
        AttackoftheNornbear,
        CinematicCaveNornCursed,
        CinematicSteppeInterrogation,
        CinematicInteriorResearch,
        CinematicEyeVisionA,
        CinematicEyeVisionB,
        CinematicEyeVisionC,
        CinematicEyeVisionD,
        PolymockColiseum,
        PolymockGlacier,
        PolymockCrossing,
        CinematicMountainResolution,
        ColdasIce,
        BeneathLionsArch,
        TunnelsBelowCantha,
        CavernsBelowKamadan,
        CinematicMountainDwarfs,
        ServiceInDefenseoftheEye,
        ManoaNorno,
        ServicePracticeDummy,
        HeroTutorial,
        TheNornFightingTournament,
        SecretLairOftheSnowmen,
        NornBrawlingChampionship,
        KilroysPunchoutTraining,
        FronisIrontoesLairMission,
        TheJusticiarsEnd,
        TheGreatNornAlemoot,
        VarajarFellsunknown,
        Epilogue,
        InsidiousRemnants,
        AttackonJalissCamp,
        CostumeBrawlOutpost,
        WhitefuryRapidsMission,
        KystenShoreMission,
        DeepwayRuinsMission,
        PlikkupWorksMission,
        KilroysPunchoutTournament,
        SpecialOpsFlameTempleCorridor,
        SpecialOpsDragonsGullet,
        SpecialOpsGrendichCourthouse,
        TheTenguAccords,
        TheBattleOfJahai,
        TheFlightNorth,
        TheRiseOfTheWhiteMantle,
        FindingTheBloodstoneMission,
        GeniusOperatedLivingEnchantedManifestationMission,
        AgainstTheCharrMission,
        WarbandOfBrothersMission,
        AssaultOnTheStrongholdMission,
        DestructionsDepthsMission,
        ATimeForHeroesMission,
        CurseOfTheNornbearMission,
        BloodWashesBloodMission,
        AGateTooFarMission,
        TheElusiveGolemancerMission,
        SecretLairOftheSnowmen2,
        SecretLairOftheSnowmen3,
        DroknarsForgeCinematic,
        IsleOfTheNamelessPvP,
        TempleOfTheAgesROX,
        WajjunBazaarPOX,
        BokkaAmphitheatreNOX,
        SecretUndergroundLair,
        GolemTutorialSimulation,
        SnowballDominance,
        ZaishenMenagerieGrounds,
        ZaishenMenagerieOutpost,
        CodexArenaOutpost,
        TheUnderworldSomethingWickedThisWayComes,
        TheUnderworldDontFeartheReapers,
        LionsArchHalloweenOutpost,
        LionsArchWintersdayOutpost,
        LionsArchCanthanNewYearOutpost,
        AscalonCityWintersdayOutpost,
        DroknarsForgeHalloweenOutpost,
        DroknarsForgeWintersdayOutpost,
        TombOfThePrimevalKingsHalloweenOutpost,
        ShingJeaMonasteryDragonFestivalOutpost,
        ShingJeaMonasteryCanthanNewYearOutpost,
        KainengCenterCanthanNewYearOutpost,
        KamadanJewelOfIstanHalloweenOutpost,
        KamadanJewelOfIstanWintersdayOutpost,
        KamadanJewelOfIstanCanthanNewYearOutpost,
        EyeOfTheNorthOutpostWintersdayOutpost,
        WarinKrytaTalmarkWilderness,
        WarinKrytaTrialofZinn,
        WarinKrytaDivinityCoast,
        WarinKrytaLionsArchKeep,
        WarinKrytaDAlessioSeaboard,
        WarinKrytaTheBattleforLionsArch,
        WarinKrytaRiversideProvince,
        WarinKrytaLionsArch,
        WarinKrytaTheMausoleum,
        WarinKrytaRise,
        WarinKrytaShadowsintheJungle,
        WarinKrytaAVengeanceofBlades,
        WarinKrytaAuspiciousBeginnings,
        OlafsteadCinematic,
        TheGreatSnowballFightoftheGodsOperationCrushSpirits,
        TheGreatSnowballFightoftheGodsFightinginaWinterWonderland,
        EmbarkBeach,
        DragonsThroatAreaWhatWaitsInShadow,
        KainengCenterWindsOfChangeAChanceEncounter,
        TheMarketplaceAreaTrackingtheCorruption,
        BukdekBywayWindsOfChangeCanthaCourierCrisis,
        TsumeiVillageWindsOfChangeATreatysATreaty,
        SeitungHarborAreaDeadlyCargo,
        TahnnakaiTempleWindsOfChangeTheRescueAttempt,
        WajjunBazaarWindsOfChangeViolenceInTheStreets,
        ScarredPsycheMission,
        ShadowsPassageWindsofChangeCallingAllThugs,
        AltrummRuinsFindingJinnai,
        ShingJeaMonasteryRaidOnShingJeaMonastery,
        KainengCenterWindsOfChangeRaidonKainengCenter,
        WajjunBazaarWindsOfChangeMinistryOfOppression,
        TheFinalConfrontation,
        LakesideCounty1070AE,
        AshfordCatacombs1070AE,
        Count,
    };

    public static bool TryParse(int id, out Map map)
    {
        map = Maps.Where(map => map.Id == id).FirstOrDefault()!;
        if (map is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Map map)
    {
        map = Maps.Where(map => map.Name == name).FirstOrDefault()!;
        if (map is null)
        {
            return false;
        }

        return true;
    }
    public static Map Parse(int id)
    {
        if (TryParse(id, out var map) is false)
        {
            throw new InvalidOperationException($"Could not find a map with id {id}");
        }

        return map;
    }
    public static Map Parse(string name)
    {
        if (TryParse(name, out var map) is false)
        {
            throw new InvalidOperationException($"Could not find a map with name {name}");
        }

        return map;
    }

    private Map()
    {
    }
}
