using Daybreak.Shared.Converters;
using Newtonsoft.Json;

namespace Daybreak.Shared.Models.Guildwars;

[JsonConverter(typeof(QuestJsonConverter))]
public sealed class Quest : IWikiEntity
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? WikiUrl { get; init; }

    public static readonly Quest TheAscalonSettlement = new() { Id = 0, Name = "The Ascalon Settlement", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ascalon_Settlement" };
    public static readonly Quest TheVillainyofGalrath = new() { Id = 1, Name = "The Villainy of Galrath", WikiUrl = "https://wiki.guildwars.com/wiki/The_Villainy_of_Galrath" };
    public static readonly Quest BanditTrouble = new() { Id = 2, Name = "Bandit Trouble", WikiUrl = "https://wiki.guildwars.com/wiki/Bandit_Trouble" };
    public static readonly Quest AGiftforJalisIronhammer = new() { Id = 3, Name = "A Gift for Jalis Ironhammer", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gift_for_Jalis_Ironhammer" };
    public static readonly Quest ToKrytaJourneysEnd = new() { Id = 4, Name = "To Kryta Journeys End", WikiUrl = "https://wiki.guildwars.com/wiki/To_Kryta:_Journey%27s_End" };
    public static readonly Quest GravenImages = new() { Id = 5, Name = "Graven Images", WikiUrl = "https://wiki.guildwars.com/wiki/Graven_Images" };
    public static readonly Quest TheHotSpringsMurders = new() { Id = 6, Name = "The Hot Springs Murders", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hot_Springs_Murders" };
    public static readonly Quest TheLastHog = new() { Id = 7, Name = "The Last Hog", WikiUrl = "https://wiki.guildwars.com/wiki/The_Last_Hog" };
    public static readonly Quest TheLostPrincess = new() { Id = 8, Name = "The Lost Princess", WikiUrl = "https://wiki.guildwars.com/wiki/The_Lost_Princess" };
    public static readonly Quest DutiesofaLionguard = new() { Id = 9, Name = "Duties of a Lionguard", WikiUrl = "https://wiki.guildwars.com/wiki/Duties_of_a_Lionguard" };
    public static readonly Quest TheRoyalPapers = new() { Id = 10, Name = "The Royal Papers", WikiUrl = "https://wiki.guildwars.com/wiki/The_Royal_Papers" };
    public static readonly Quest AJourneyofRedemption = new() { Id = 11, Name = "A Journey of Redemption", WikiUrl = "https://wiki.guildwars.com/wiki/A_Journey_of_Redemption" };
    public static readonly Quest BlanketsfortheSettlers = new() { Id = 12, Name = "Blankets for the Settlers", WikiUrl = "https://wiki.guildwars.com/wiki/Blankets_for_the_Settlers" };
    public static readonly Quest OrrianExcavation = new() { Id = 13, Name = "Orrian Excavation", WikiUrl = "https://wiki.guildwars.com/wiki/Orrian_Excavation" };
    public static readonly Quest MalaquiresTest = new() { Id = 14, Name = "Malaquires Test", WikiUrl = "https://wiki.guildwars.com/wiki/Malaquire%27s_Test" };
    public static readonly Quest ReversingtheSkales = new() { Id = 15, Name = "Reversing the Skales", WikiUrl = "https://wiki.guildwars.com/wiki/Reversing_the_Skales" };
    public static readonly Quest TheUndeadHordes = new() { Id = 16, Name = "The Undead Hordes", WikiUrl = "https://wiki.guildwars.com/wiki/The_Undead_Hordes" };
    public static readonly Quest LagaansOrdeal = new() { Id = 17, Name = "Lagaans Ordeal", WikiUrl = "https://wiki.guildwars.com/wiki/Lagaan%27s_Ordeal" };
    public static readonly Quest LagaansGratitude = new() { Id = 18, Name = "Lagaans Gratitude", WikiUrl = "https://wiki.guildwars.com/wiki/Lagaan%27s_Gratitude" };
    public static readonly Quest ReporttotheWhiteMantle = new() { Id = 19, Name = "Report to the White Mantle", WikiUrl = "https://wiki.guildwars.com/wiki/Report_to_the_White_Mantle" };
    public static readonly Quest MerchantsPlea = new() { Id = 20, Name = "Merchants Plea", WikiUrl = "https://wiki.guildwars.com/wiki/Merchant%27s_Plea" };
    public static readonly Quest ABelatedBetrothal = new() { Id = 21, Name = "A Belated Betrothal", WikiUrl = "https://wiki.guildwars.com/wiki/A_Belated_Betrothal" };
    public static readonly Quest AncientSecrets = new() { Id = 22, Name = "Ancient Secrets", WikiUrl = "https://wiki.guildwars.com/wiki/Ancient_Secrets" };
    public static readonly Quest TheForgottenOnes = new() { Id = 23, Name = "The Forgotten Ones", WikiUrl = "https://wiki.guildwars.com/wiki/The_Forgotten_Ones" };
    public static readonly Quest ForgottenWisdom = new() { Id = 24, Name = "Forgotten Wisdom", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Wisdom" };
    public static readonly Quest GhostlyVengeance = new() { Id = 25, Name = "Ghostly Vengeance", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Vengeance" };
    public static readonly Quest IntotheUnknown = new() { Id = 26, Name = "Into the Unknown", WikiUrl = "https://wiki.guildwars.com/wiki/Into_the_Unknown" };
    public static readonly Quest TheMisplacedSword = new() { Id = 27, Name = "The Misplaced Sword", WikiUrl = "https://wiki.guildwars.com/wiki/The_Misplaced_Sword" };
    public static readonly Quest SandsOfSouls = new() { Id = 28, Name = "Sands Of Souls", WikiUrl = "https://wiki.guildwars.com/wiki/Sands_Of_Souls" };
    public static readonly Quest TheMesmersPath = new() { Id = 29, Name = "The Mesmers Path", WikiUrl = "https://wiki.guildwars.com/wiki/The_Mesmer%27s_Path" };
    public static readonly Quest TheNecromancersPath = new() { Id = 30, Name = "The Necromancers Path", WikiUrl = "https://wiki.guildwars.com/wiki/The_Necromancer%27s_Path" };
    public static readonly Quest TheElementalistsPath = new() { Id = 31, Name = "The Elementalists Path", WikiUrl = "https://wiki.guildwars.com/wiki/The_Elementalist%27s_Path" };
    public static readonly Quest TheMonksPath = new() { Id = 32, Name = "The Monks Path", WikiUrl = "https://wiki.guildwars.com/wiki/The_Monk%27s_Path" };
    public static readonly Quest TheWarriorsPath = new() { Id = 33, Name = "The Warriors Path", WikiUrl = "https://wiki.guildwars.com/wiki/The_Warrior%27s_Path" };
    public static readonly Quest TheRangersPath = new() { Id = 34, Name = "The Rangers Path", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ranger%27s_Path" };
    public static readonly Quest WarPreparations = new() { Id = 35, Name = "War Preparations", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations" };
    public static readonly Quest TheHuntersHorn = new() { Id = 36, Name = "The Hunters Horn", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hunter%27s_Horn" };
    public static readonly Quest TheSupremacyofAir = new() { Id = 37, Name = "The Supremacy of Air", WikiUrl = "https://wiki.guildwars.com/wiki/The_Supremacy_of_Air" };
    public static readonly Quest RitesofRemembrance = new() { Id = 38, Name = "Rites of Remembrance", WikiUrl = "https://wiki.guildwars.com/wiki/Rites_of_Remembrance" };
    public static readonly Quest LittleThomsBigCloak = new() { Id = 39, Name = "Little Thoms Big Cloak", WikiUrl = "https://wiki.guildwars.com/wiki/Little_Thom%27s_Big_Cloak" };
    public static readonly Quest TheVineyardProblem = new() { Id = 40, Name = "The Vineyard Problem", WikiUrl = "https://wiki.guildwars.com/wiki/The_Vineyard_Problem" };
    public static readonly Quest BanditRaid = new() { Id = 41, Name = "Bandit Raid", WikiUrl = "https://wiki.guildwars.com/wiki/Bandit_Raid" };
    public static readonly Quest ATestofMarksmanship = new() { Id = 42, Name = "A Test of Marksmanship", WikiUrl = "https://wiki.guildwars.com/wiki/A_Test_of_Marksmanship" };
    public static readonly Quest ThePowerofBlood = new() { Id = 43, Name = "The Power of Blood", WikiUrl = "https://wiki.guildwars.com/wiki/The_Power_of_Blood" };
    public static readonly Quest TheRoguesReplacement = new() { Id = 44, Name = "The Rogues Replacement", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rogue%27s_Replacement" };
    public static readonly Quest CharrintheCatacombs = new() { Id = 45, Name = "Charr in the Catacombs", WikiUrl = "https://wiki.guildwars.com/wiki/Charr_in_the_Catacombs" };
    public static readonly Quest CharrattheGate = new() { Id = 46, Name = "Charr at the Gate", WikiUrl = "https://wiki.guildwars.com/wiki/Charr_at_the_Gate" };
    public static readonly Quest TheAccursedPath = new() { Id = 47, Name = "The Accursed Path", WikiUrl = "https://wiki.guildwars.com/wiki/The_Accursed_Path" };
    public static readonly Quest ThePoisonDevourer = new() { Id = 48, Name = "The Poison Devourer", WikiUrl = "https://wiki.guildwars.com/wiki/The_Poison_Devourer" };
    public static readonly Quest DominationMagic = new() { Id = 49, Name = "Domination Magic (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Domination_Magic_(quest)" };
    public static readonly Quest TheTrueKing = new() { Id = 50, Name = "The True King", WikiUrl = "https://wiki.guildwars.com/wiki/The_True_King" };
    public static readonly Quest TheEggHunter = new() { Id = 51, Name = "The Egg Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/The_Egg_Hunter" };
    public static readonly Quest TheWormProblem = new() { Id = 52, Name = "The Worm Problem", WikiUrl = "https://wiki.guildwars.com/wiki/The_Worm_Problem" };
    public static readonly Quest ThePrizeMoaBird = new() { Id = 53, Name = "The Prize Moa Bird", WikiUrl = "https://wiki.guildwars.com/wiki/The_Prize_Moa_Bird" };
    public static readonly Quest FurtherAdventures = new() { Id = 54, Name = "Further Adventures", WikiUrl = "https://wiki.guildwars.com/wiki/Further_Adventures" };
    public static readonly Quest TheWaywardWizard = new() { Id = 55, Name = "The Wayward Wizard", WikiUrl = "https://wiki.guildwars.com/wiki/The_Wayward_Wizard" };
    public static readonly Quest AdventurewithanAlly = new() { Id = 56, Name = "Adventure with an Ally", WikiUrl = "https://wiki.guildwars.com/wiki/Adventure_with_an_Ally" };
    public static readonly Quest TheOrchard = new() { Id = 57, Name = "The Orchard", WikiUrl = "https://wiki.guildwars.com/wiki/The_Orchard" };
    public static readonly Quest AGiftforAlthea = new() { Id = 58, Name = "A Gift for Althea", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gift_for_Althea" };
    public static readonly Quest GwensFlute = new() { Id = 59, Name = "Gwens Flute (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Gwen%27s_Flute_(quest)" };
    public static readonly Quest WarriorsChallenge = new() { Id = 60, Name = "Warriors Challenge", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior%27s_Challenge" };
    public static readonly Quest TitheforAshfordAbbey = new() { Id = 61, Name = "Tithe for Ashford Abbey", WikiUrl = "https://wiki.guildwars.com/wiki/Tithe_for_Ashford_Abbey" };
    public static readonly Quest UnsettlingRumors = new() { Id = 62, Name = "Unsettling Rumors", WikiUrl = "https://wiki.guildwars.com/wiki/Unsettling_Rumors" };
    public static readonly Quest ANewMesmerTrainer = new() { Id = 63, Name = "A New Mesmer Trainer", WikiUrl = "https://wiki.guildwars.com/wiki/A_New_Mesmer_Trainer" };
    public static readonly Quest ANewNecromancerTrainer = new() { Id = 64, Name = "A New Necromancer Trainer", WikiUrl = "https://wiki.guildwars.com/wiki/A_New_Necromancer_Trainer" };
    public static readonly Quest ANewElementalistTrainer = new() { Id = 65, Name = "A New Elementalist Trainer", WikiUrl = "https://wiki.guildwars.com/wiki/A_New_Elementalist_Trainer" };
    public static readonly Quest ANewMonkTrainer = new() { Id = 66, Name = "A New Monk Trainer", WikiUrl = "https://wiki.guildwars.com/wiki/A_New_Monk_Trainer" };
    public static readonly Quest ANewWarriorTrainer = new() { Id = 67, Name = "A New Warrior Trainer", WikiUrl = "https://wiki.guildwars.com/wiki/A_New_Warrior_Trainer" };
    public static readonly Quest ANewRangerTrainer = new() { Id = 68, Name = "A New Ranger Trainer", WikiUrl = "https://wiki.guildwars.com/wiki/A_New_Ranger_Trainer" };
    public static readonly Quest AcrosstheWall = new() { Id = 69, Name = "Across the Wall", WikiUrl = "https://wiki.guildwars.com/wiki/Across_the_Wall" };
    public static readonly Quest PoorTenant = new() { Id = 70, Name = "Poor Tenant", WikiUrl = "https://wiki.guildwars.com/wiki/Poor_Tenant" };
    public static readonly Quest AMesmersBurden = new() { Id = 71, Name = "A Mesmers Burden", WikiUrl = "https://wiki.guildwars.com/wiki/A_Mesmer%27s_Burden" };
    public static readonly Quest TheNecromancersNovice = new() { Id = 72, Name = "The Necromancers Novice", WikiUrl = "https://wiki.guildwars.com/wiki/The_Necromancer%27s_Novice" };
    public static readonly Quest TheElementalistExperiment = new() { Id = 73, Name = "The Elementalist Experiment", WikiUrl = "https://wiki.guildwars.com/wiki/The_Elementalist_Experiment" };
    public static readonly Quest AMonksMission = new() { Id = 74, Name = "A Monks Mission", WikiUrl = "https://wiki.guildwars.com/wiki/A_Monk%27s_Mission" };
    public static readonly Quest GrawlInvasion = new() { Id = 75, Name = "Grawl Invasion", WikiUrl = "https://wiki.guildwars.com/wiki/Grawl_Invasion" };
    public static readonly Quest TheRangersCompanion = new() { Id = 76, Name = "The Rangers Companion", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ranger%27s_Companion" };
    public static readonly Quest ProtectionPrayers = new() { Id = 77, Name = "Protection Prayers (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Protection_Prayers_(quest)" };
    public static readonly Quest OppositiontotheKing = new() { Id = 78, Name = "Opposition to the King", WikiUrl = "https://wiki.guildwars.com/wiki/Opposition_to_the_King" };
    public static readonly Quest ASecondProfession = new() { Id = 79, Name = "A Second Profession", WikiUrl = "https://wiki.guildwars.com/wiki/A_Second_Profession" };
    public static readonly Quest MessagefromaFriend = new() { Id = 80, Name = "Message from a Friend", WikiUrl = "https://wiki.guildwars.com/wiki/Message_from_a_Friend" };
    public static readonly Quest MesmerTest = new() { Id = 81, Name = "Mesmer Test", WikiUrl = "https://wiki.guildwars.com/wiki/Mesmer_Test" };
    public static readonly Quest NecromancerTest = new() { Id = 82, Name = "Necromancer Test", WikiUrl = "https://wiki.guildwars.com/wiki/Necromancer_Test" };
    public static readonly Quest ElementalistTest = new() { Id = 83, Name = "Elementalist Test", WikiUrl = "https://wiki.guildwars.com/wiki/Elementalist_Test" };
    public static readonly Quest MonkTest = new() { Id = 84, Name = "Monk Test", WikiUrl = "https://wiki.guildwars.com/wiki/Monk_Test" };
    public static readonly Quest WarriorTest = new() { Id = 85, Name = "Warrior Test", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior_Test" };
    public static readonly Quest RangerTest = new() { Id = 86, Name = "Ranger Test", WikiUrl = "https://wiki.guildwars.com/wiki/Ranger_Test" };
    public static readonly Quest TheBlessingsofBalthazar = new() { Id = 87, Name = "The Blessings of Balthazar", WikiUrl = "https://wiki.guildwars.com/wiki/The_Blessings_of_Balthazar" };
    public static readonly Quest UnnaturalGrowths = new() { Id = 88, Name = "Unnatural Growths", WikiUrl = "https://wiki.guildwars.com/wiki/Unnatural_Growths" };
    public static readonly Quest ThePathtoGlory = new() { Id = 89, Name = "The Path to Glory", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Glory" };
    public static readonly Quest TroubleintheWoods = new() { Id = 90, Name = "Trouble in the Woods", WikiUrl = "https://wiki.guildwars.com/wiki/Trouble_in_the_Woods" };
    public static readonly Quest WhiteMantleWrathAHelpingHand = new() { Id = 91, Name = "White Mantle Wrath A Helping Hand", WikiUrl = "https://wiki.guildwars.com/wiki/White_Mantle_Wrath:_A_Helping_Hand" };
    public static readonly Quest UrgentWarning = new() { Id = 92, Name = "Urgent Warning", WikiUrl = "https://wiki.guildwars.com/wiki/Urgent_Warning" };
    public static readonly Quest BloodAndSmoke = new() { Id = 93, Name = "Blood And Smoke", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_And_Smoke" };
    public static readonly Quest DroppingEaves = new() { Id = 94, Name = "Dropping Eaves", WikiUrl = "https://wiki.guildwars.com/wiki/Dropping_Eaves" };
    public static readonly Quest ABrothersFury = new() { Id = 95, Name = "A Brothers Fury", WikiUrl = "https://wiki.guildwars.com/wiki/A_Brother%27s_Fury" };
    public static readonly Quest EyeForProfit = new() { Id = 96, Name = "Eye For Profit", WikiUrl = "https://wiki.guildwars.com/wiki/Eye_For_Profit" };
    public static readonly Quest MysteriousMessage = new() { Id = 97, Name = "Mysterious Message (Prophecies quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Mysterious_Message_(Prophecies_quest)" };
    public static readonly Quest ThePriceofSteel = new() { Id = 98, Name = "The Price of Steel", WikiUrl = "https://wiki.guildwars.com/wiki/The_Price_of_Steel" };
    public static readonly Quest WhiteMantleWrathDemagoguesVanguard = new() { Id = 99, Name = "White Mantle Wrath Demagogues Vanguard", WikiUrl = "https://wiki.guildwars.com/wiki/White_Mantle_Wrath:_Demagogue%27s_Vanguard" };
    public static readonly Quest PassageThroughTheDarkRiver = new() { Id = 100, Name = "Passage Through The Dark River", WikiUrl = "https://wiki.guildwars.com/wiki/Passage_Through_The_Dark_River" };
    public static readonly Quest CleartheChamber = new() { Id = 101, Name = "Clear the Chamber", WikiUrl = "https://wiki.guildwars.com/wiki/Clear_the_Chamber" };
    public static readonly Quest ServantsofGrenth = new() { Id = 102, Name = "Servants of Grenth", WikiUrl = "https://wiki.guildwars.com/wiki/Servants_of_Grenth" };
    public static readonly Quest UnwantedGuests = new() { Id = 103, Name = "Unwanted Guests", WikiUrl = "https://wiki.guildwars.com/wiki/Unwanted_Guests" };
    public static readonly Quest DemonAssassin = new() { Id = 104, Name = "Demon Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Demon_Assassin" };
    public static readonly Quest ImprisonedSpirits = new() { Id = 105, Name = "Imprisoned Spirits", WikiUrl = "https://wiki.guildwars.com/wiki/Imprisoned_Spirits" };
    public static readonly Quest TheFourHorsemen = new() { Id = 106, Name = "The Four Horsemen", WikiUrl = "https://wiki.guildwars.com/wiki/The_Four_Horsemen" };
    public static readonly Quest TerrorwebQueen = new() { Id = 107, Name = "Terrorweb Queen", WikiUrl = "https://wiki.guildwars.com/wiki/Terrorweb_Queen" };
    public static readonly Quest EscortofSouls = new() { Id = 108, Name = "Escort of Souls", WikiUrl = "https://wiki.guildwars.com/wiki/Escort_of_Souls" };
    public static readonly Quest RestoringGrenthsMonuments = new() { Id = 109, Name = "Restoring Grenths Monuments", WikiUrl = "https://wiki.guildwars.com/wiki/Restoring_Grenth%27s_Monuments" };
    public static readonly Quest WrathfulSpirits = new() { Id = 110, Name = "Wrathful Spirits", WikiUrl = "https://wiki.guildwars.com/wiki/Wrathful_Spirits" };
    public static readonly Quest CharrReinforcements = new() { Id = 111, Name = "Charr Reinforcements", WikiUrl = "https://wiki.guildwars.com/wiki/Charr_Reinforcements" };
    public static readonly Quest SymonsHistoryofAscalon = new() { Id = 112, Name = "Symons History of Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Symon%27s_History_of_Ascalon" };
    public static readonly Quest ExperimentalElixir = new() { Id = 113, Name = "Experimental Elixir", WikiUrl = "https://wiki.guildwars.com/wiki/Experimental_Elixir" };
    public static readonly Quest TryingTimes = new() { Id = 114, Name = "Trying Times", WikiUrl = "https://wiki.guildwars.com/wiki/Trying_Times" };
    public static readonly Quest UnnaturalCreatures = new() { Id = 115, Name = "Unnatural Creatures", WikiUrl = "https://wiki.guildwars.com/wiki/Unnatural_Creatures" };
    public static readonly Quest TheCharrPatrol = new() { Id = 116, Name = "The Charr Patrol", WikiUrl = "https://wiki.guildwars.com/wiki/The_Charr_Patrol" };
    public static readonly Quest TheCharrStagingArea = new() { Id = 117, Name = "The Charr Staging Area", WikiUrl = "https://wiki.guildwars.com/wiki/The_Charr_Staging_Area" };
    public static readonly Quest ACureforRalena = new() { Id = 118, Name = "A Cure for Ralena", WikiUrl = "https://wiki.guildwars.com/wiki/A_Cure_for_Ralena" };
    public static readonly Quest CasualtyReport = new() { Id = 119, Name = "Casualty Report", WikiUrl = "https://wiki.guildwars.com/wiki/Casualty_Report" };
    public static readonly Quest DeathintheRuins = new() { Id = 120, Name = "Death in the Ruins", WikiUrl = "https://wiki.guildwars.com/wiki/Death_in_the_Ruins" };
    public static readonly Quest FallenSoldiers = new() { Id = 121, Name = "Fallen Soldiers", WikiUrl = "https://wiki.guildwars.com/wiki/Fallen_Soldiers" };
    public static readonly Quest OberansRage = new() { Id = 122, Name = "Oberans Rage", WikiUrl = "https://wiki.guildwars.com/wiki/Oberan%27s_Rage" };
    public static readonly Quest AltheasAshes = new() { Id = 123, Name = "Altheas Ashes", WikiUrl = "https://wiki.guildwars.com/wiki/Althea%27s_Ashes" };
    public static readonly Quest HammerandAnvil = new() { Id = 124, Name = "Hammer and Anvil", WikiUrl = "https://wiki.guildwars.com/wiki/Hammer_and_Anvil" };
    public static readonly Quest TheGeomancersTest = new() { Id = 125, Name = "The Geomancers Test", WikiUrl = "https://wiki.guildwars.com/wiki/The_Geomancer%27s_Test" };
    public static readonly Quest TheWayoftheGeomancer = new() { Id = 126, Name = "The Way of the Geomancer", WikiUrl = "https://wiki.guildwars.com/wiki/The_Way_of_the_Geomancer" };
    public static readonly Quest ShalevsTask = new() { Id = 127, Name = "Shalevs Task", WikiUrl = "https://wiki.guildwars.com/wiki/Shalev%27s_Task" };
    public static readonly Quest ElementalKnowledge = new() { Id = 128, Name = "Elemental Knowledge", WikiUrl = "https://wiki.guildwars.com/wiki/Elemental_Knowledge" };
    public static readonly Quest TheDukesDaughter = new() { Id = 129, Name = "The Dukes Daughter", WikiUrl = "https://wiki.guildwars.com/wiki/The_Duke%27s_Daughter" };
    public static readonly Quest BarradinsAdvance = new() { Id = 130, Name = "Barradins Advance", WikiUrl = "https://wiki.guildwars.com/wiki/Barradin%27s_Advance" };
    public static readonly Quest FiresintheEast = new() { Id = 131, Name = "Fires in the East", WikiUrl = "https://wiki.guildwars.com/wiki/Fires_in_the_East" };
    public static readonly Quest FiresintheNorth = new() { Id = 132, Name = "Fires in the North", WikiUrl = "https://wiki.guildwars.com/wiki/Fires_in_the_North" };
    public static readonly Quest FrontierGateFugitives = new() { Id = 133, Name = "Frontier Gate Fugitives", WikiUrl = "https://wiki.guildwars.com/wiki/Frontier_Gate_Fugitives" };
    public static readonly Quest BountyforChieftainMurg = new() { Id = 134, Name = "Bounty for Chieftain Murg", WikiUrl = "https://wiki.guildwars.com/wiki/Bounty_for_Chieftain_Murg" };
    public static readonly Quest HelpingthePeopleofAscalon = new() { Id = 135, Name = "Helping the People of Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Helping_the_People_of_Ascalon" };
    public static readonly Quest CitiesofAscalon = new() { Id = 136, Name = "Cities of Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Cities_of_Ascalon" };
    public static readonly Quest CountingtheFallen = new() { Id = 137, Name = "Counting the Fallen", WikiUrl = "https://wiki.guildwars.com/wiki/Counting_the_Fallen" };
    public static readonly Quest ScavengersinOldAscalon = new() { Id = 138, Name = "Scavengers in Old Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Scavengers_in_Old_Ascalon" };
    public static readonly Quest GarfazzBloodfang = new() { Id = 139, Name = "Garfazz Bloodfang (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Garfazz_Bloodfang_(quest)" };
    public static readonly Quest TheKingsMessage = new() { Id = 140, Name = "The Kings Message", WikiUrl = "https://wiki.guildwars.com/wiki/The_King%27s_Message" };
    public static readonly Quest InMemoryofPaulus = new() { Id = 141, Name = "In Memory of Paulus", WikiUrl = "https://wiki.guildwars.com/wiki/In_Memory_of_Paulus" };
    public static readonly Quest ProtectingAscalon = new() { Id = 142, Name = "Protecting Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Protecting_Ascalon" };
    public static readonly Quest TheLostMaster = new() { Id = 143, Name = "The Lost Master", WikiUrl = "https://wiki.guildwars.com/wiki/The_Lost_Master" };
    public static readonly Quest TheAmbassadorsQuandary = new() { Id = 144, Name = "The Ambassadors Quandary", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ambassador%27s_Quandary" };
    public static readonly Quest RastinsRitual = new() { Id = 145, Name = "Rastins Ritual", WikiUrl = "https://wiki.guildwars.com/wiki/Rastin%27s_Ritual" };
    public static readonly Quest AMissionofPeace = new() { Id = 146, Name = "A Mission of Peace", WikiUrl = "https://wiki.guildwars.com/wiki/A_Mission_of_Peace" };
    public static readonly Quest TheTroublesomeArtifact = new() { Id = 147, Name = "The Troublesome Artifact", WikiUrl = "https://wiki.guildwars.com/wiki/The_Troublesome_Artifact" };
    public static readonly Quest BarradinsStand = new() { Id = 148, Name = "Barradins Stand", WikiUrl = "https://wiki.guildwars.com/wiki/Barradin%27s_Stand" };
    public static readonly Quest ArmyLife = new() { Id = 149, Name = "Army Life", WikiUrl = "https://wiki.guildwars.com/wiki/Army_Life" };
    public static readonly Quest IntotheBreach = new() { Id = 150, Name = "Into the Breach", WikiUrl = "https://wiki.guildwars.com/wiki/Into_the_Breach" };
    public static readonly Quest MilitaryMatters = new() { Id = 151, Name = "Military Matters", WikiUrl = "https://wiki.guildwars.com/wiki/Military_Matters" };
    public static readonly Quest TheRedCloakedDeserter = new() { Id = 152, Name = "The Red-Cloaked Deserter", WikiUrl = "https://wiki.guildwars.com/wiki/The_Red-Cloaked_Deserter" };
    public static readonly Quest TheSiegeofPikenSquare = new() { Id = 153, Name = "The Siege of Piken Square", WikiUrl = "https://wiki.guildwars.com/wiki/The_Siege_of_Piken_Square" };
    public static readonly Quest ReplacementHealers = new() { Id = 154, Name = "Replacement Healers", WikiUrl = "https://wiki.guildwars.com/wiki/Replacement_Healers" };
    public static readonly Quest RogennsDuel = new() { Id = 155, Name = "Rogenns Duel", WikiUrl = "https://wiki.guildwars.com/wiki/Rogenn%27s_Duel" };
    public static readonly Quest RegentValleyDefense = new() { Id = 156, Name = "Regent Valley Defense", WikiUrl = "https://wiki.guildwars.com/wiki/Regent_Valley_Defense" };
    public static readonly Quest MesmerizingtheEnemy = new() { Id = 157, Name = "Mesmerizing the Enemy", WikiUrl = "https://wiki.guildwars.com/wiki/Mesmerizing_the_Enemy" };
    public static readonly Quest TheStolenArtifact = new() { Id = 158, Name = "The Stolen Artifact", WikiUrl = "https://wiki.guildwars.com/wiki/The_Stolen_Artifact" };
    public static readonly Quest ScorchedEarth = new() { Id = 159, Name = "Scorched Earth (Prophecies quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Scorched_Earth_(Prophecies_quest)" };
    public static readonly Quest SowingSeeds = new() { Id = 160, Name = "Sowing Seeds", WikiUrl = "https://wiki.guildwars.com/wiki/Sowing_Seeds" };
    public static readonly Quest GargoyleTrouble = new() { Id = 161, Name = "Gargoyle Trouble", WikiUrl = "https://wiki.guildwars.com/wiki/Gargoyle_Trouble" };
    public static readonly Quest EndangeredSpecies = new() { Id = 162, Name = "Endangered Species", WikiUrl = "https://wiki.guildwars.com/wiki/Endangered_Species" };
    public static readonly Quest TheMissingMelandruRelic = new() { Id = 163, Name = "The Missing Melandru Relic", WikiUrl = "https://wiki.guildwars.com/wiki/The_Missing_Melandru_Relic" };
    public static readonly Quest SuppliesfortheDuke = new() { Id = 164, Name = "Supplies for the Duke", WikiUrl = "https://wiki.guildwars.com/wiki/Supplies_for_the_Duke" };
    public static readonly Quest RuinsofSurmia = new() { Id = 165, Name = "Ruins of Surmia (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Ruins_of_Surmia_(quest)" };
    public static readonly Quest VanguardEquipment = new() { Id = 166, Name = "Vanguard Equipment", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Equipment" };
    public static readonly Quest DeliveraMessagetoMyWife = new() { Id = 167, Name = "Deliver a Message to My Wife", WikiUrl = "https://wiki.guildwars.com/wiki/Deliver_a_Message_to_My_Wife" };
    public static readonly Quest RecruitsforHollis = new() { Id = 168, Name = "Recruits for Hollis", WikiUrl = "https://wiki.guildwars.com/wiki/Recruits_for_Hollis" };
    public static readonly Quest TheKrytanAmbassador = new() { Id = 169, Name = "The Krytan Ambassador", WikiUrl = "https://wiki.guildwars.com/wiki/The_Krytan_Ambassador" };
    public static readonly Quest CollectCharrArmorSet = new() { Id = 170, Name = "Collect Charr Armor Set", WikiUrl = "https://wiki.guildwars.com/wiki/Collect_Charr_Armor_Set" };
    public static readonly Quest DefendtheWall = new() { Id = 171, Name = "Defend the Wall", WikiUrl = "https://wiki.guildwars.com/wiki/Defend_the_Wall" };
    public static readonly Quest SlayRotscale = new() { Id = 172, Name = "Slay Rotscale", WikiUrl = "https://wiki.guildwars.com/wiki/Slay_Rotscale" };
    public static readonly Quest SlayStankReekfoul = new() { Id = 173, Name = "Slay Stank Reekfoul", WikiUrl = "https://wiki.guildwars.com/wiki/Slay_Stank_Reekfoul" };
    public static readonly Quest RepelCharr = new() { Id = 174, Name = "Repel Charr", WikiUrl = "https://wiki.guildwars.com/wiki/Repel_Charr" };
    public static readonly Quest ScoutCharr = new() { Id = 175, Name = "Scout Charr", WikiUrl = "https://wiki.guildwars.com/wiki/Scout_Charr" };
    public static readonly Quest CollectGargoyleFangs = new() { Id = 176, Name = "Collect Gargoyle Fangs", WikiUrl = "https://wiki.guildwars.com/wiki/Collect_Gargoyle_Fangs" };
    public static readonly Quest DefendDroknarsForge = new() { Id = 177, Name = "Defend Droknars Forge", WikiUrl = "https://wiki.guildwars.com/wiki/Defend_Droknar%27s_Forge" };
    public static readonly Quest TheRoadtoBorlisPass = new() { Id = 178, Name = "The Road to Borlis Pass", WikiUrl = "https://wiki.guildwars.com/wiki/The_Road_to_Borlis_Pass" };
    public static readonly Quest ToKrytaTheIceCave = new() { Id = 179, Name = "To Kryta The Ice Cave", WikiUrl = "https://wiki.guildwars.com/wiki/To_Kryta:_The_Ice_Cave" };
    public static readonly Quest TheStoneSummitChampion = new() { Id = 180, Name = "The Stone Summit Champion", WikiUrl = "https://wiki.guildwars.com/wiki/The_Stone_Summit_Champion" };
    public static readonly Quest RenegadeNecromancer = new() { Id = 181, Name = "Renegade Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Renegade_Necromancer" };
    public static readonly Quest TheDeserters = new() { Id = 182, Name = "The Deserters", WikiUrl = "https://wiki.guildwars.com/wiki/The_Deserters" };
    public static readonly Quest AHeartofIce = new() { Id = 183, Name = "A Heart of Ice", WikiUrl = "https://wiki.guildwars.com/wiki/A_Heart_of_Ice" };
    public static readonly Quest TheMissingArtisan = new() { Id = 184, Name = "The Missing Artisan", WikiUrl = "https://wiki.guildwars.com/wiki/The_Missing_Artisan" };
    public static readonly Quest HelpingtheDwarves = new() { Id = 185, Name = "Helping the Dwarves", WikiUrl = "https://wiki.guildwars.com/wiki/Helping_the_Dwarves" };
    public static readonly Quest HungryDevourer = new() { Id = 186, Name = "Hungry Devourer", WikiUrl = "https://wiki.guildwars.com/wiki/Hungry_Devourer" };
    public static readonly Quest OswaltsEpitaph = new() { Id = 187, Name = "Oswalts Epitaph", WikiUrl = "https://wiki.guildwars.com/wiki/Oswalt%27s_Epitaph" };
    public static readonly Quest SecuringtheVale = new() { Id = 188, Name = "Securing the Vale", WikiUrl = "https://wiki.guildwars.com/wiki/Securing_the_Vale" };
    public static readonly Quest MinaarsTrouble = new() { Id = 189, Name = "Minaars Trouble", WikiUrl = "https://wiki.guildwars.com/wiki/Minaar%27s_Trouble" };
    public static readonly Quest IronHorseWarMachine = new() { Id = 190, Name = "Iron Horse War Machine", WikiUrl = "https://wiki.guildwars.com/wiki/Iron_Horse_War_Machine" };
    public static readonly Quest MinaarsWorry = new() { Id = 191, Name = "Minaars Worry", WikiUrl = "https://wiki.guildwars.com/wiki/Minaar%27s_Worry" };
    public static readonly Quest StoneSummitBeastmasters = new() { Id = 192, Name = "Stone Summit Beastmasters", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Summit_Beastmasters" };
    public static readonly Quest SlayFrostbite = new() { Id = 193, Name = "Slay Frostbite", WikiUrl = "https://wiki.guildwars.com/wiki/Slay_Frostbite" };
    public static readonly Quest TheHerosJourney = new() { Id = 194, Name = "The Heros Journey", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hero%27s_Journey" };
    public static readonly Quest TheHerosChallenge = new() { Id = 195, Name = "The Heros Challenge", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hero%27s_Challenge" };
    public static readonly Quest SeekingTheSeer = new() { Id = 196, Name = "Seeking The Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Seeking_The_Seer" };
    public static readonly Quest ShiverpeakStragglers = new() { Id = 197, Name = "Shiverpeak Stragglers", WikiUrl = "https://wiki.guildwars.com/wiki/Shiverpeak_Stragglers" };
    public static readonly Quest TheWayisBlocked = new() { Id = 198, Name = "The Way is Blocked", WikiUrl = "https://wiki.guildwars.com/wiki/The_Way_is_Blocked" };
    public static readonly Quest ToKrytaRefugees = new() { Id = 199, Name = "To Kryta Refugees", WikiUrl = "https://wiki.guildwars.com/wiki/To_Kryta:_Refugees" };
    public static readonly Quest TheWaywardMonk = new() { Id = 200, Name = "The Wayward Monk", WikiUrl = "https://wiki.guildwars.com/wiki/The_Wayward_Monk" };
    public static readonly Quest IntotheBreach1 = new() { Id = 201, Name = "Into the Breach", WikiUrl = "https://wiki.guildwars.com/wiki/Into_the_Breach" };
    public static readonly Quest DefendtheTempleofWar = new() { Id = 202, Name = "Defend the Temple of War", WikiUrl = "https://wiki.guildwars.com/wiki/Defend_the_Temple_of_War" };
    public static readonly Quest ArmyofDarkness = new() { Id = 203, Name = "Army of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Army_of_Darkness" };
    public static readonly Quest TheWailingLord = new() { Id = 204, Name = "The Wailing Lord", WikiUrl = "https://wiki.guildwars.com/wiki/The_Wailing_Lord" };
    public static readonly Quest AGiftofGriffons = new() { Id = 205, Name = "A Gift of Griffons", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gift_of_Griffons" };
    public static readonly Quest SlavesofMenzies = new() { Id = 206, Name = "Slaves of Menzies", WikiUrl = "https://wiki.guildwars.com/wiki/Slaves_of_Menzies" };
    public static readonly Quest RestoretheTempleofWar = new() { Id = 207, Name = "Restore the Temple of War", WikiUrl = "https://wiki.guildwars.com/wiki/Restore_the_Temple_of_War" };
    public static readonly Quest TheHunt = new() { Id = 208, Name = "The Hunt", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hunt" };
    public static readonly Quest TheEternalForgemaster = new() { Id = 209, Name = "The Eternal Forgemaster", WikiUrl = "https://wiki.guildwars.com/wiki/The_Eternal_Forgemaster" };
    public static readonly Quest ChampionsofOrr = new() { Id = 210, Name = "Champions of Orr", WikiUrl = "https://wiki.guildwars.com/wiki/Champions_of_Orr" };
    public static readonly Quest TowerofStrength = new() { Id = 211, Name = "Tower of Strength", WikiUrl = "https://wiki.guildwars.com/wiki/Tower_of_Strength" };
    public static readonly Quest TowerofCourage = new() { Id = 212, Name = "Tower of Courage", WikiUrl = "https://wiki.guildwars.com/wiki/Tower_of_Courage" };
    public static readonly Quest FinalBlow = new() { Id = 213, Name = "Final Blow", WikiUrl = "https://wiki.guildwars.com/wiki/Final_Blow" };
    public static readonly Quest WisdomoftheDruids = new() { Id = 214, Name = "Wisdom of the Druids", WikiUrl = "https://wiki.guildwars.com/wiki/Wisdom_of_the_Druids" };
    public static readonly Quest FamilyTies = new() { Id = 215, Name = "Family Ties (Prophecies quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Family_Ties_(Prophecies_quest)" };
    public static readonly Quest TheWeaverofNebo = new() { Id = 216, Name = "The Weaver of Nebo", WikiUrl = "https://wiki.guildwars.com/wiki/The_Weaver_of_Nebo" };
    public static readonly Quest WarPreparations1 = new() { Id = 217, Name = "War Preparations", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations" };
    public static readonly Quest WarPreparations2 = new() { Id = 218, Name = "War Preparations", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations" };
    public static readonly Quest WarPreparations3 = new() { Id = 219, Name = "War Preparations", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations" };
    public static readonly Quest WarPreparations4 = new() { Id = 220, Name = "War Preparations", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations" };
    public static readonly Quest WarPreparations5 = new() { Id = 221, Name = "War Preparations", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations" };
    public static readonly Quest WarPreparations6 = new() { Id = 222, Name = "War Preparations", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations" };
    public static readonly Quest TheFalseGods = new() { Id = 223, Name = "The False Gods", WikiUrl = "https://wiki.guildwars.com/wiki/The_False_Gods" };
    public static readonly Quest KhobaytheBetrayer = new() { Id = 224, Name = "Khobay the Betrayer", WikiUrl = "https://wiki.guildwars.com/wiki/Khobay_the_Betrayer" };
    public static readonly Quest CaravaninTrouble = new() { Id = 225, Name = "Caravan in Trouble", WikiUrl = "https://wiki.guildwars.com/wiki/Caravan_in_Trouble" };
    public static readonly Quest DefendNorthKrytaProvince = new() { Id = 226, Name = "Defend North Kryta Province", WikiUrl = "https://wiki.guildwars.com/wiki/Defend_North_Kryta_Province" };
    public static readonly Quest DefendDenravi = new() { Id = 227, Name = "Defend Denravi", WikiUrl = "https://wiki.guildwars.com/wiki/Defend_Denravi" };
    public static readonly Quest TheLastDayDawns = new() { Id = 228, Name = "The Last Day Dawns", WikiUrl = "https://wiki.guildwars.com/wiki/The_Last_Day_Dawns" };
    public static readonly Quest TheTitanSource = new() { Id = 229, Name = "The Titan Source", WikiUrl = "https://wiki.guildwars.com/wiki/The_Titan_Source" };
    public static readonly Quest ColdOne = new() { Id = 230, Name = "Cold One", WikiUrl = "https://wiki.guildwars.com/wiki/Cold_One" };
    public static readonly Quest TheForgeHeart = new() { Id = 231, Name = "The Forge Heart", WikiUrl = "https://wiki.guildwars.com/wiki/The_Forge_Heart" };
    public static readonly Quest WrenchesInTheGears = new() { Id = 232, Name = "Wrenches In The Gears", WikiUrl = "https://wiki.guildwars.com/wiki/Wrenches_In_The_Gears" };
    public static readonly Quest UnspeakableUnknowable = new() { Id = 233, Name = "Unspeakable, Unknowable", WikiUrl = "https://wiki.guildwars.com/wiki/Unspeakable,_Unknowable" };
    public static readonly Quest ToSorrowsFurnace = new() { Id = 234, Name = "To Sorrows Furnace", WikiUrl = "https://wiki.guildwars.com/wiki/To_Sorrow%27s_Furnace" };
    public static readonly Quest NobleIntentions = new() { Id = 235, Name = "Noble Intentions", WikiUrl = "https://wiki.guildwars.com/wiki/Noble_Intentions" };
    public static readonly Quest NobleIntentionsPlanB = new() { Id = 236, Name = "Noble Intentions Plan B", WikiUrl = "https://wiki.guildwars.com/wiki/Noble_Intentions_Plan_B" };
    public static readonly Quest KilroyStonekin = new() { Id = 237, Name = "Kilroy Stonekin (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Kilroy_Stonekin_(quest)" };
    public static readonly Quest SummitSlaves = new() { Id = 238, Name = "Summit Slaves", WikiUrl = "https://wiki.guildwars.com/wiki/Summit_Slaves" };
    public static readonly Quest GatheringResources = new() { Id = 239, Name = "Gathering Resources", WikiUrl = "https://wiki.guildwars.com/wiki/Gathering_Resources" };
    public static readonly Quest UnrulySlaves = new() { Id = 240, Name = "Unruly Slaves", WikiUrl = "https://wiki.guildwars.com/wiki/Unruly_Slaves" };
    public static readonly Quest FireintheHole = new() { Id = 241, Name = "Fire in the Hole", WikiUrl = "https://wiki.guildwars.com/wiki/Fire_in_the_Hole" };
    public static readonly Quest TascasReprise = new() { Id = 242, Name = "Tascas Reprise", WikiUrl = "https://wiki.guildwars.com/wiki/Tasca%27s_Reprise" };
    public static readonly Quest SubversiveIdeas = new() { Id = 243, Name = "Subversive Ideas", WikiUrl = "https://wiki.guildwars.com/wiki/Subversive_Ideas" };
    public static readonly Quest PoliticalRamifications = new() { Id = 244, Name = "Political Ramifications", WikiUrl = "https://wiki.guildwars.com/wiki/Political_Ramifications" };
    public static readonly Quest TheFinalAssault = new() { Id = 245, Name = "The Final Assault", WikiUrl = "https://wiki.guildwars.com/wiki/The_Final_Assault" };
    public static readonly Quest SeekOutHeadmasterLee = new() { Id = 246, Name = "Seek Out Headmaster Lee", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_Out_Headmaster_Lee" };
    public static readonly Quest SeekOutHeadmasterKaa = new() { Id = 247, Name = "Seek Out Headmaster Kaa", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_Out_Headmaster_Kaa" };
    public static readonly Quest SeekOutHeadmasterKuju = new() { Id = 248, Name = "Seek Out Headmaster Kuju", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_Out_Headmaster_Kuju" };
    public static readonly Quest SeekOutHeadmasterVhang = new() { Id = 249, Name = "Seek Out Headmaster Vhang", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_Out_Headmaster_Vhang" };
    public static readonly Quest SeekOutHeadmasterZhan = new() { Id = 250, Name = "Seek Out Headmaster Zhan", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_Out_Headmaster_Zhan" };
    public static readonly Quest SeekOutHeadmasterAmara = new() { Id = 251, Name = "Seek Out Headmaster Amara", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_Out_Headmaster_Amara" };
    public static readonly Quest SeekOutHeadmasterGreico = new() { Id = 252, Name = "Seek Out Headmaster Greico", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_Out_Headmaster_Greico" };
    public static readonly Quest SeekOutHeadmasterQuin = new() { Id = 253, Name = "Seek Out Headmaster Quin", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_Out_Headmaster_Quin" };
    public static readonly Quest LocateJinzo = new() { Id = 254, Name = "Locate Jinzo", WikiUrl = "https://wiki.guildwars.com/wiki/Locate_Jinzo" };
    public static readonly Quest LocateMeiLing = new() { Id = 255, Name = "Locate Mei Ling", WikiUrl = "https://wiki.guildwars.com/wiki/Locate_Mei_Ling" };
    public static readonly Quest LocateRengKu = new() { Id = 256, Name = "Locate Reng Ku", WikiUrl = "https://wiki.guildwars.com/wiki/Locate_Reng_Ku" };
    public static readonly Quest LocateRonsu = new() { Id = 257, Name = "Locate Ronsu", WikiUrl = "https://wiki.guildwars.com/wiki/Locate_Ronsu" };
    public static readonly Quest LocateSisterTai = new() { Id = 258, Name = "Locate Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Locate_Sister_Tai" };
    public static readonly Quest LocateTalonSilverwing = new() { Id = 259, Name = "Locate Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Locate_Talon_Silverwing" };
    public static readonly Quest LocateSujun = new() { Id = 260, Name = "Locate Sujun", WikiUrl = "https://wiki.guildwars.com/wiki/Locate_Sujun" };
    public static readonly Quest LocateProfessorGai = new() { Id = 261, Name = "Locate Professor Gai", WikiUrl = "https://wiki.guildwars.com/wiki/Locate_Professor_Gai" };
    public static readonly Quest TrackDownPanaku = new() { Id = 262, Name = "Track Down Panaku", WikiUrl = "https://wiki.guildwars.com/wiki/Track_Down_Panaku" };
    public static readonly Quest TrackDownLoSha = new() { Id = 263, Name = "Track Down Lo Sha", WikiUrl = "https://wiki.guildwars.com/wiki/Track_Down_Lo_Sha" };
    public static readonly Quest TrackDownSu = new() { Id = 264, Name = "Track Down Su", WikiUrl = "https://wiki.guildwars.com/wiki/Track_Down_Su" };
    public static readonly Quest TrackDownKaiYing = new() { Id = 265, Name = "Track Down Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Track_Down_Kai_Ying" };
    public static readonly Quest TrackDownBrotherPeWan = new() { Id = 266, Name = "Track Down Brother Pe Wan", WikiUrl = "https://wiki.guildwars.com/wiki/Track_Down_Brother_Pe_Wan" };
    public static readonly Quest TrackDownWengGha = new() { Id = 267, Name = "Track Down Weng Gha", WikiUrl = "https://wiki.guildwars.com/wiki/Track_Down_Weng_Gha" };
    public static readonly Quest TrackDownZho = new() { Id = 268, Name = "Track Down Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Track_Down_Zho" };
    public static readonly Quest TrackDownAngtheEphemeral = new() { Id = 269, Name = "Track Down Ang the Ephemeral", WikiUrl = "https://wiki.guildwars.com/wiki/Track_Down_Ang_the_Ephemeral" };
    public static readonly Quest OpeningStrike = new() { Id = 270, Name = "Opening Strike", WikiUrl = "https://wiki.guildwars.com/wiki/Opening_Strike" };
    public static readonly Quest CleansingtheShrine = new() { Id = 271, Name = "Cleansing the Shrine", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_the_Shrine" };
    public static readonly Quest LittleCreatures = new() { Id = 272, Name = "Little Creatures", WikiUrl = "https://wiki.guildwars.com/wiki/Little_Creatures" };
    public static readonly Quest SparkofInterest = new() { Id = 273, Name = "Spark of Interest", WikiUrl = "https://wiki.guildwars.com/wiki/Spark_of_Interest" };
    public static readonly Quest StaleMate = new() { Id = 274, Name = "Stale Mate", WikiUrl = "https://wiki.guildwars.com/wiki/Stale_Mate" };
    public static readonly Quest TalonsDuel = new() { Id = 275, Name = "Talons Duel", WikiUrl = "https://wiki.guildwars.com/wiki/Talon%27s_Duel" };
    public static readonly Quest FreetheFur = new() { Id = 276, Name = "Free the Fur", WikiUrl = "https://wiki.guildwars.com/wiki/Free_the_Fur" };
    public static readonly Quest MinionsGoneWild = new() { Id = 277, Name = "Minions Gone Wild", WikiUrl = "https://wiki.guildwars.com/wiki/Minions_Gone_Wild" };
    public static readonly Quest DualStrike = new() { Id = 278, Name = "Dual Strike", WikiUrl = "https://wiki.guildwars.com/wiki/Dual_Strike" };
    public static readonly Quest LoShasGift = new() { Id = 279, Name = "Lo Shas Gift", WikiUrl = "https://wiki.guildwars.com/wiki/Lo_Sha%27s_Gift" };
    public static readonly Quest ReapingtheNaga = new() { Id = 280, Name = "Reaping the Naga", WikiUrl = "https://wiki.guildwars.com/wiki/Reaping_the_Naga" };
    public static readonly Quest SparringElements = new() { Id = 281, Name = "Sparring Elements", WikiUrl = "https://wiki.guildwars.com/wiki/Sparring_Elements" };
    public static readonly Quest SeekingaCure = new() { Id = 282, Name = "Seeking a Cure", WikiUrl = "https://wiki.guildwars.com/wiki/Seeking_a_Cure" };
    public static readonly Quest TheCropThieves = new() { Id = 283, Name = "The Crop Thieves", WikiUrl = "https://wiki.guildwars.com/wiki/The_Crop_Thieves" };
    public static readonly Quest TheYetiHunt = new() { Id = 284, Name = "The Yeti Hunt", WikiUrl = "https://wiki.guildwars.com/wiki/The_Yeti_Hunt" };
    public static readonly Quest ShackledSpirits = new() { Id = 285, Name = "Shackled Spirits", WikiUrl = "https://wiki.guildwars.com/wiki/Shackled_Spirits" };
    public static readonly Quest AnAudiencewithMasterTogoAssassin = new() { Id = 286, Name = "An Audience with Master Togo (assassin)", WikiUrl = "https://wiki.guildwars.com/wiki/An_Audience_with_Master_Togo_(assassin)" };
    public static readonly Quest AnAudiencewithMasterTogoMesmer = new() { Id = 287, Name = "An Audience with Master Togo (mesmer)", WikiUrl = "https://wiki.guildwars.com/wiki/An_Audience_with_Master_Togo_(mesmer)" };
    public static readonly Quest AnAudiencewithMasterTogoNecromancer = new() { Id = 288, Name = "An Audience with Master Togo (necromancer)", WikiUrl = "https://wiki.guildwars.com/wiki/An_Audience_with_Master_Togo_(necromancer)" };
    public static readonly Quest AnAudiencewithMasterTogoElementalist = new() { Id = 289, Name = "An Audience with Master Togo (elementalist)", WikiUrl = "https://wiki.guildwars.com/wiki/An_Audience_with_Master_Togo_(elementalist)" };
    public static readonly Quest AnAudiencewithMasterTogoMonk = new() { Id = 290, Name = "An Audience with Master Togo (monk)", WikiUrl = "https://wiki.guildwars.com/wiki/An_Audience_with_Master_Togo_(monk)" };
    public static readonly Quest AnAudiencewithMasterTogoWarrior = new() { Id = 291, Name = "An Audience with Master Togo (warrior)", WikiUrl = "https://wiki.guildwars.com/wiki/An_Audience_with_Master_Togo_(warrior)" };
    public static readonly Quest AnAudiencewithMasterTogoRanger = new() { Id = 292, Name = "An Audience with Master Togo (ranger)", WikiUrl = "https://wiki.guildwars.com/wiki/An_Audience_with_Master_Togo_(ranger)" };
    public static readonly Quest AnAudiencewithMasterTogoRitualist = new() { Id = 293, Name = "An Audience with Master Togo (ritualist)", WikiUrl = "https://wiki.guildwars.com/wiki/An_Audience_with_Master_Togo_(ritualist)" };
    public static readonly Quest SpeakwithHeadmasterLeeAssassin = new() { Id = 294, Name = "Speak with Headmaster Lee (Assassin)", WikiUrl = "https://wiki.guildwars.com/wiki/Speak_with_Headmaster_Lee_(Assassin)" };
    public static readonly Quest SpeakwithHeadmasterKaaMesmer = new() { Id = 295, Name = "Speak with Headmaster Kaa (Mesmer)", WikiUrl = "https://wiki.guildwars.com/wiki/Speak_with_Headmaster_Kaa_(Mesmer)" };
    public static readonly Quest SpeakwithHeadmasterKujuNecromancer = new() { Id = 296, Name = "Speak with Headmaster Kuju (Necromancer)", WikiUrl = "https://wiki.guildwars.com/wiki/Speak_with_Headmaster_Kuju_(Necromancer)" };
    public static readonly Quest SpeakwithHeadmasterVhangElementalist = new() { Id = 297, Name = "Speak with Headmaster Vhang (Elementalist)", WikiUrl = "https://wiki.guildwars.com/wiki/Speak_with_Headmaster_Vhang_(Elementalist)" };
    public static readonly Quest SpeakwithHeadmasterAmaraMonk = new() { Id = 298, Name = "Speak with Headmaster Amara (Monk)", WikiUrl = "https://wiki.guildwars.com/wiki/Speak_with_Headmaster_Amara_(Monk)" };
    public static readonly Quest SpeakwithHeadmasterZhanWarrior = new() { Id = 299, Name = "Speak with Headmaster Zhan (Warrior)", WikiUrl = "https://wiki.guildwars.com/wiki/Speak_with_Headmaster_Zhan_(Warrior)" };
    public static readonly Quest SpeakwithHeadmasterGreicoRanger = new() { Id = 300, Name = "Speak with Headmaster Greico (Ranger)", WikiUrl = "https://wiki.guildwars.com/wiki/Speak_with_Headmaster_Greico_(Ranger)" };
    public static readonly Quest SpeakwithHeadmasterQuinRitualist = new() { Id = 301, Name = "Speak with Headmaster Quin (Ritualist)", WikiUrl = "https://wiki.guildwars.com/wiki/Speak_with_Headmaster_Quin_(Ritualist)" };
    public static readonly Quest DefenseAgainstHexes = new() { Id = 302, Name = "Defense Against Hexes", WikiUrl = "https://wiki.guildwars.com/wiki/Defense_Against_Hexes" };
    public static readonly Quest Disruption = new() { Id = 303, Name = "Disruption", WikiUrl = "https://wiki.guildwars.com/wiki/Disruption" };
    public static readonly Quest SkillChaining = new() { Id = 304, Name = "Skill Chaining", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Chaining" };
    public static readonly Quest SnaringCourse = new() { Id = 305, Name = "Snaring Course", WikiUrl = "https://wiki.guildwars.com/wiki/Snaring_Course" };
    public static readonly Quest DisenchantmentCourse = new() { Id = 306, Name = "Disenchantment Course", WikiUrl = "https://wiki.guildwars.com/wiki/Disenchantment_Course" };
    public static readonly Quest ConditionRemoval = new() { Id = 307, Name = "Condition Removal", WikiUrl = "https://wiki.guildwars.com/wiki/Condition_Removal" };
    public static readonly Quest AdvancedDefenseTechniques = new() { Id = 308, Name = "Advanced Defense Techniques", WikiUrl = "https://wiki.guildwars.com/wiki/Advanced_Defense_Techniques" };
    public static readonly Quest ElementalistInsignia = new() { Id = 309, Name = "Elementalist Insignia", WikiUrl = "https://wiki.guildwars.com/wiki/Elementalist_Insignia" };
    public static readonly Quest WarriorInsignia = new() { Id = 310, Name = "Warrior Insignia", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior_Insignia" };
    public static readonly Quest MesmerInsignia = new() { Id = 311, Name = "Mesmer Insignia", WikiUrl = "https://wiki.guildwars.com/wiki/Mesmer_Insignia" };
    public static readonly Quest MonkInsignia = new() { Id = 312, Name = "Monk Insignia", WikiUrl = "https://wiki.guildwars.com/wiki/Monk_Insignia" };
    public static readonly Quest NecromancerInsignia = new() { Id = 313, Name = "Necromancer Insignia", WikiUrl = "https://wiki.guildwars.com/wiki/Necromancer_Insignia" };
    public static readonly Quest RangerInsignia = new() { Id = 314, Name = "Ranger Insignia", WikiUrl = "https://wiki.guildwars.com/wiki/Ranger_Insignia" };
    public static readonly Quest RitualistInsignia = new() { Id = 315, Name = "Ritualist Insignia", WikiUrl = "https://wiki.guildwars.com/wiki/Ritualist_Insignia" };
    public static readonly Quest AssassinInsignia = new() { Id = 316, Name = "Assassin Insignia", WikiUrl = "https://wiki.guildwars.com/wiki/Assassin_Insignia" };
    public static readonly Quest ChooseYourSecondaryProfessionFactions = new() { Id = 317, Name = "Choose Your Secondary Profession (Factions quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Choose_Your_Secondary_Profession_(Factions_quest)" };
    public static readonly Quest AFormalIntroduction = new() { Id = 318, Name = "A Formal Introduction", WikiUrl = "https://wiki.guildwars.com/wiki/A_Formal_Introduction" };
    public static readonly Quest TheSickenedVillage = new() { Id = 319, Name = "The Sickened Village", WikiUrl = "https://wiki.guildwars.com/wiki/The_Sickened_Village" };
    public static readonly Quest TheRedFrog = new() { Id = 320, Name = "The Red Frog", WikiUrl = "https://wiki.guildwars.com/wiki/The_Red_Frog" };
    public static readonly Quest TheKaguchiBrothers = new() { Id = 321, Name = "The Kaguchi Brothers", WikiUrl = "https://wiki.guildwars.com/wiki/The_Kaguchi_Brothers" };
    public static readonly Quest TheBoxofIllusions = new() { Id = 322, Name = "The Box of Illusions", WikiUrl = "https://wiki.guildwars.com/wiki/The_Box_of_Illusions" };
    public static readonly Quest OldFriendsDarkTimes = new() { Id = 323, Name = "Old Friends, Dark Times", WikiUrl = "https://wiki.guildwars.com/wiki/Old_Friends,_Dark_Times" };
    public static readonly Quest PowerSurge = new() { Id = 324, Name = "Power Surge", WikiUrl = "https://wiki.guildwars.com/wiki/Power_Surge" };
    public static readonly Quest CleansingtheSteel = new() { Id = 325, Name = "Cleansing the Steel", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_the_Steel" };
    public static readonly Quest TheStoneoftheElements = new() { Id = 326, Name = "The Stone of the Elements", WikiUrl = "https://wiki.guildwars.com/wiki/The_Stone_of_the_Elements" };
    public static readonly Quest AssassinationAttempt = new() { Id = 327, Name = "Assassination Attempt", WikiUrl = "https://wiki.guildwars.com/wiki/Assassination_Attempt" };
    public static readonly Quest LittleLostBear = new() { Id = 328, Name = "Little Lost Bear", WikiUrl = "https://wiki.guildwars.com/wiki/Little_Lost_Bear" };
    public static readonly Quest ImperialAssistance = new() { Id = 329, Name = "Imperial Assistance", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Assistance" };
    public static readonly Quest MedicalEmergency = new() { Id = 330, Name = "Medical Emergency", WikiUrl = "https://wiki.guildwars.com/wiki/Medical_Emergency" };
    public static readonly Quest RedTape = new() { Id = 331, Name = "Red Tape", WikiUrl = "https://wiki.guildwars.com/wiki/Red_Tape" };
    public static readonly Quest AssisttheGuards = new() { Id = 332, Name = "Assist the Guards", WikiUrl = "https://wiki.guildwars.com/wiki/Assist_the_Guards" };
    public static readonly Quest StraighttotheTop = new() { Id = 333, Name = "Straight to the Top", WikiUrl = "https://wiki.guildwars.com/wiki/Straight_to_the_Top" };
    public static readonly Quest DrinkfromtheChaliceofCorruption = new() { Id = 334, Name = "Drink from the Chalice of Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Drink_from_the_Chalice_of_Corruption" };
    public static readonly Quest RefusetoDrink = new() { Id = 335, Name = "Refuse to Drink", WikiUrl = "https://wiki.guildwars.com/wiki/Refuse_to_Drink" };
    public static readonly Quest TheSearchforaCure = new() { Id = 336, Name = "The Search for a Cure", WikiUrl = "https://wiki.guildwars.com/wiki/The_Search_for_a_Cure" };
    public static readonly Quest SeekoutBrotherTosai = new() { Id = 337, Name = "Seek out Brother Tosai", WikiUrl = "https://wiki.guildwars.com/wiki/Seek_out_Brother_Tosai" };
    public static readonly Quest AnEndtoSuffering = new() { Id = 338, Name = "An End to Suffering", WikiUrl = "https://wiki.guildwars.com/wiki/An_End_to_Suffering" };
    public static readonly Quest WarningtheTengu = new() { Id = 339, Name = "Warning the Tengu", WikiUrl = "https://wiki.guildwars.com/wiki/Warning_the_Tengu" };
    public static readonly Quest TheThreatGrows = new() { Id = 340, Name = "The Threat Grows", WikiUrl = "https://wiki.guildwars.com/wiki/The_Threat_Grows" };
    public static readonly Quest JourneytotheMaster = new() { Id = 341, Name = "Journey to the Master", WikiUrl = "https://wiki.guildwars.com/wiki/Journey_to_the_Master" };
    public static readonly Quest TheRoadLessTraveled = new() { Id = 342, Name = "The Road Less Traveled", WikiUrl = "https://wiki.guildwars.com/wiki/The_Road_Less_Traveled" };
    public static readonly Quest LookingForTrouble = new() { Id = 343, Name = "Looking For Trouble", WikiUrl = "https://wiki.guildwars.com/wiki/Looking_For_Trouble" };
    public static readonly Quest TotheRescueFactions = new() { Id = 344, Name = "To the Rescue (Factions quest)", WikiUrl = "https://wiki.guildwars.com/wiki/To_the_Rescue_(Factions_quest)" };
    public static readonly Quest ToZenDaijun = new() { Id = 345, Name = "To Zen Daijun", WikiUrl = "https://wiki.guildwars.com/wiki/To_Zen_Daijun" };
    public static readonly Quest LostTreasure = new() { Id = 346, Name = "Lost Treasure", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Treasure" };
    public static readonly Quest MantidHatchlings = new() { Id = 347, Name = "Mantid Hatchlings", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Hatchlings" };
    public static readonly Quest AnUnwelcomeGuest = new() { Id = 348, Name = "An Unwelcome Guest", WikiUrl = "https://wiki.guildwars.com/wiki/An_Unwelcome_Guest" };
    public static readonly Quest AMastersBurden = new() { Id = 349, Name = "A Masters Burden", WikiUrl = "https://wiki.guildwars.com/wiki/A_Master%27s_Burden" };
    public static readonly Quest ToTahnnakaiTemple = new() { Id = 350, Name = "To Tahnnakai Temple", WikiUrl = "https://wiki.guildwars.com/wiki/To_Tahnnakai_Temple" };
    public static readonly Quest IntotheWhirlpool = new() { Id = 351, Name = "Into the Whirlpool", WikiUrl = "https://wiki.guildwars.com/wiki/Into_the_Whirlpool" };
    public static readonly Quest JourneytoHousezuHeltzer = new() { Id = 352, Name = "Journey to House zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Journey_to_House_zu_Heltzer" };
    public static readonly Quest JourneytoCavalon = new() { Id = 353, Name = "Journey to Cavalon", WikiUrl = "https://wiki.guildwars.com/wiki/Journey_to_Cavalon" };
    public static readonly Quest NagaOil = new() { Id = 354, Name = "Naga Oil", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Oil" };
    public static readonly Quest StreetJustice = new() { Id = 355, Name = "Street Justice", WikiUrl = "https://wiki.guildwars.com/wiki/Street_Justice" };
    public static readonly Quest FindingTheOracle = new() { Id = 356, Name = "Finding The Oracle", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_The_Oracle" };
    public static readonly Quest ClosertotheStars = new() { Id = 357, Name = "Closer to the Stars", WikiUrl = "https://wiki.guildwars.com/wiki/Closer_to_the_Stars" };
    public static readonly Quest AppearanceoftheNaga = new() { Id = 358, Name = "Appearance of the Naga", WikiUrl = "https://wiki.guildwars.com/wiki/Appearance_of_the_Naga" };
    public static readonly Quest TheRiteofValor = new() { Id = 359, Name = "The Rite of Valor", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rite_of_Valor" };
    public static readonly Quest DeathwithHonor = new() { Id = 360, Name = "Death with Honor", WikiUrl = "https://wiki.guildwars.com/wiki/Death_with_Honor" };
    public static readonly Quest TheNamelessMasters = new() { Id = 361, Name = "The Nameless Masters", WikiUrl = "https://wiki.guildwars.com/wiki/The_Nameless_Masters" };
    public static readonly Quest ThePathoftheZaishenElite = new() { Id = 362, Name = "The Path of the Zaishen Elite", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_of_the_Zaishen_Elite" };
    public static readonly Quest TheTeamTrials = new() { Id = 363, Name = "The Team Trials", WikiUrl = "https://wiki.guildwars.com/wiki/The_Team_Trials" };
    public static readonly Quest KurzickSupplyLines = new() { Id = 364, Name = "Kurzick Supply Lines", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Supply_Lines" };
    public static readonly Quest ScoutingFerndale = new() { Id = 365, Name = "Scouting Ferndale", WikiUrl = "https://wiki.guildwars.com/wiki/Scouting_Ferndale" };
    public static readonly Quest ScouttheCoast = new() { Id = 366, Name = "Scout the Coast", WikiUrl = "https://wiki.guildwars.com/wiki/Scout_the_Coast" };
    public static readonly Quest SecuringEchovaldForest = new() { Id = 367, Name = "Securing Echovald Forest", WikiUrl = "https://wiki.guildwars.com/wiki/Securing_Echovald_Forest" };
    public static readonly Quest DueloftheHouses = new() { Id = 368, Name = "Duel of the Houses", WikiUrl = "https://wiki.guildwars.com/wiki/Duel_of_the_Houses" };
    public static readonly Quest TheJadeArena = new() { Id = 369, Name = "The Jade Arena", WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Arena" };
    public static readonly Quest IntheArmyLuxon = new() { Id = 370, Name = "In the Army (Luxon)", WikiUrl = "https://wiki.guildwars.com/wiki/In_the_Army_(Luxon)" };
    public static readonly Quest ScoutingMaishangHills = new() { Id = 371, Name = "Scouting Maishang Hills", WikiUrl = "https://wiki.guildwars.com/wiki/Scouting_Maishang_Hills" };
    public static readonly Quest LuxonSupplyLines = new() { Id = 372, Name = "Luxon Supply Lines", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Supply_Lines" };
    public static readonly Quest IntheArmyNowKurzick = new() { Id = 373, Name = "In the Army Now (Kurzick)", WikiUrl = "https://wiki.guildwars.com/wiki/In_the_Army_Now_(Kurzick)" };
    public static readonly Quest BefriendingtheKurzicks = new() { Id = 374, Name = "Befriending the Kurzicks", WikiUrl = "https://wiki.guildwars.com/wiki/Befriending_the_Kurzicks" };
    public static readonly Quest BefriendingtheLuxons = new() { Id = 375, Name = "Befriending the Luxons", WikiUrl = "https://wiki.guildwars.com/wiki/Befriending_the_Luxons" };
    public static readonly Quest MayhemintheMarket = new() { Id = 376, Name = "Mayhem in the Market", WikiUrl = "https://wiki.guildwars.com/wiki/Mayhem_in_the_Market" };
    public static readonly Quest WelcometoCantha = new() { Id = 377, Name = "Welcome to Cantha", WikiUrl = "https://wiki.guildwars.com/wiki/Welcome_to_Cantha" };
    public static readonly Quest TheDefendersoftheForest = new() { Id = 378, Name = "The Defenders of the Forest", WikiUrl = "https://wiki.guildwars.com/wiki/The_Defenders_of_the_Forest" };
    public static readonly Quest AMeetingWiththeEmperor = new() { Id = 379, Name = "A Meeting With the Emperor", WikiUrl = "https://wiki.guildwars.com/wiki/A_Meeting_With_the_Emperor" };
    public static readonly Quest TheCountsDaughter = new() { Id = 380, Name = "The Counts Daughter", WikiUrl = "https://wiki.guildwars.com/wiki/The_Count%27s_Daughter" };
    public static readonly Quest StolenEggs = new() { Id = 381, Name = "Stolen Eggs", WikiUrl = "https://wiki.guildwars.com/wiki/Stolen_Eggs" };
    public static readonly Quest TheConvocation = new() { Id = 382, Name = "The Convocation", WikiUrl = "https://wiki.guildwars.com/wiki/The_Convocation" };
    public static readonly Quest JourneytotheWhirlpool = new() { Id = 383, Name = "Journey to the Whirlpool", WikiUrl = "https://wiki.guildwars.com/wiki/Journey_to_the_Whirlpool" };
    public static readonly Quest TakingBackthePalace = new() { Id = 384, Name = "Taking Back the Palace", WikiUrl = "https://wiki.guildwars.com/wiki/Taking_Back_the_Palace" };
    public static readonly Quest QuimangsLastStand = new() { Id = 385, Name = "Quimangs Last Stand", WikiUrl = "https://wiki.guildwars.com/wiki/Quimang%27s_Last_Stand" };
    public static readonly Quest TheSiegeatTsumeiVillage = new() { Id = 386, Name = "The Siege at Tsumei Village", WikiUrl = "https://wiki.guildwars.com/wiki/The_Siege_at_Tsumei_Village" };
    public static readonly Quest TogosUltimatum = new() { Id = 387, Name = "Togos Ultimatum", WikiUrl = "https://wiki.guildwars.com/wiki/Togo%27s_Ultimatum" };
    public static readonly Quest RevengeoftheYeti = new() { Id = 388, Name = "Revenge of the Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Revenge_of_the_Yeti" };
    public static readonly Quest TheAgriculturist = new() { Id = 389, Name = "The Agriculturist", WikiUrl = "https://wiki.guildwars.com/wiki/The_Agriculturist" };
    public static readonly Quest TheCapturedSon = new() { Id = 390, Name = "The Captured Son", WikiUrl = "https://wiki.guildwars.com/wiki/The_Captured_Son" };
    public static readonly Quest TheNagaSource = new() { Id = 391, Name = "The Naga Source", WikiUrl = "https://wiki.guildwars.com/wiki/The_Naga_Source" };
    public static readonly Quest SentimentalTreasures = new() { Id = 392, Name = "Sentimental Treasures", WikiUrl = "https://wiki.guildwars.com/wiki/Sentimental_Treasures" };
    public static readonly Quest SkintheSnake = new() { Id = 393, Name = "Skin the Snake", WikiUrl = "https://wiki.guildwars.com/wiki/Skin_the_Snake" };
    public static readonly Quest PestControl = new() { Id = 394, Name = "Pest Control", WikiUrl = "https://wiki.guildwars.com/wiki/Pest_Control" };
    public static readonly Quest TheThievingNanny = new() { Id = 395, Name = "The Thieving Nanny", WikiUrl = "https://wiki.guildwars.com/wiki/The_Thieving_Nanny" };
    public static readonly Quest CityUnderAttack = new() { Id = 396, Name = "City Under Attack", WikiUrl = "https://wiki.guildwars.com/wiki/City_Under_Attack" };
    public static readonly Quest BattleintheSewers = new() { Id = 397, Name = "Battle in the Sewers", WikiUrl = "https://wiki.guildwars.com/wiki/Battle_in_the_Sewers" };
    public static readonly Quest TheAncientForest = new() { Id = 398, Name = "The Ancient Forest", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ancient_Forest" };
    public static readonly Quest WickedWardens = new() { Id = 399, Name = "Wicked Wardens", WikiUrl = "https://wiki.guildwars.com/wiki/Wicked_Wardens" };
    public static readonly Quest SongandStone = new() { Id = 400, Name = "Song and Stone", WikiUrl = "https://wiki.guildwars.com/wiki/Song_and_Stone" };
    public static readonly Quest ANewEscort = new() { Id = 401, Name = "A New Escort", WikiUrl = "https://wiki.guildwars.com/wiki/A_New_Escort" };
    public static readonly Quest TheExperimentalWeaponsmith = new() { Id = 402, Name = "The Experimental Weaponsmith", WikiUrl = "https://wiki.guildwars.com/wiki/The_Experimental_Weaponsmith" };
    public static readonly Quest MelodicGakiFlute = new() { Id = 403, Name = "Melodic Gaki Flute", WikiUrl = "https://wiki.guildwars.com/wiki/Melodic_Gaki_Flute" };
    public static readonly Quest DiscordWallowLyre = new() { Id = 404, Name = "Discord Wallow Lyre", WikiUrl = "https://wiki.guildwars.com/wiki/Discord_Wallow_Lyre" };
    public static readonly Quest RhythmDrinkerDrum = new() { Id = 405, Name = "Rhythm Drinker Drum", WikiUrl = "https://wiki.guildwars.com/wiki/Rhythm_Drinker_Drum" };
    public static readonly Quest EnvoyoftheDredge = new() { Id = 406, Name = "Envoy of the Dredge", WikiUrl = "https://wiki.guildwars.com/wiki/Envoy_of_the_Dredge" };
    public static readonly Quest TempleoftheDredge = new() { Id = 407, Name = "Temple of the Dredge", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_of_the_Dredge" };
    public static readonly Quest RevoltoftheDredge = new() { Id = 408, Name = "Revolt of the Dredge", WikiUrl = "https://wiki.guildwars.com/wiki/Revolt_of_the_Dredge" };
    public static readonly Quest TheHalcyonJob = new() { Id = 409, Name = "The Halcyon Job", WikiUrl = "https://wiki.guildwars.com/wiki/The_Halcyon_Job" };
    public static readonly Quest AttacktheKurzicks = new() { Id = 410, Name = "Attack the Kurzicks!", WikiUrl = "https://wiki.guildwars.com/wiki/Attack_the_Kurzicks!" };
    public static readonly Quest ProtecttheHalcyon = new() { Id = 411, Name = "Protect the Halcyon", WikiUrl = "https://wiki.guildwars.com/wiki/Protect_the_Halcyon" };
    public static readonly Quest OutcastsintheQuarry = new() { Id = 412, Name = "Outcasts in the Quarry", WikiUrl = "https://wiki.guildwars.com/wiki/Outcasts_in_the_Quarry" };
    public static readonly Quest ChallengeofStrength = new() { Id = 413, Name = "Challenge of Strength", WikiUrl = "https://wiki.guildwars.com/wiki/Challenge_of_Strength" };
    public static readonly Quest ArtisticEndeavors = new() { Id = 414, Name = "Artistic Endeavors", WikiUrl = "https://wiki.guildwars.com/wiki/Artistic_Endeavors" };
    public static readonly Quest TheBeakofDarkness = new() { Id = 415, Name = "The Beak of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/The_Beak_of_Darkness" };
    public static readonly Quest WardensOntheMarch = new() { Id = 416, Name = "Wardens On the March", WikiUrl = "https://wiki.guildwars.com/wiki/Wardens_On_the_March" };
    public static readonly Quest InvokingtheSaints = new() { Id = 417, Name = "Invoking the Saints", WikiUrl = "https://wiki.guildwars.com/wiki/Invoking_the_Saints" };
    public static readonly Quest IfItWerentforBadLuck = new() { Id = 418, Name = "If It Werent for Bad Luck....", WikiUrl = "https://wiki.guildwars.com/wiki/If_It_Weren%27t_for_Bad_Luck...." };
    public static readonly Quest NightRaiders = new() { Id = 419, Name = "Night Raiders", WikiUrl = "https://wiki.guildwars.com/wiki/Night_Raiders" };
    public static readonly Quest MessageonaDragonScale = new() { Id = 420, Name = "Message on a Dragon Scale", WikiUrl = "https://wiki.guildwars.com/wiki/Message_on_a_Dragon_Scale" };
    public static readonly Quest MessagesMessagesEverywhere = new() { Id = 421, Name = "Messages, Messages Everywhere", WikiUrl = "https://wiki.guildwars.com/wiki/Messages,_Messages_Everywhere" };
    public static readonly Quest TheZenosSquad = new() { Id = 422, Name = "The Zenos Squad", WikiUrl = "https://wiki.guildwars.com/wiki/The_Zenos_Squad" };
    public static readonly Quest TheImpossibleSeaMonster = new() { Id = 423, Name = "The Impossible Sea Monster", WikiUrl = "https://wiki.guildwars.com/wiki/The_Impossible_Sea_Monster" };
    public static readonly Quest MinistersTest = new() { Id = 424, Name = "Ministers Test", WikiUrl = "https://wiki.guildwars.com/wiki/Minister%27s_Test" };
    public static readonly Quest CapturingtheOrrianTome = new() { Id = 425, Name = "Capturing the Orrian Tome", WikiUrl = "https://wiki.guildwars.com/wiki/Capturing_the_Orrian_Tome" };
    public static readonly Quest ATastyMorsel = new() { Id = 426, Name = "A Tasty Morsel", WikiUrl = "https://wiki.guildwars.com/wiki/A_Tasty_Morsel" };
    public static readonly Quest WaywardGuide = new() { Id = 427, Name = "Wayward Guide", WikiUrl = "https://wiki.guildwars.com/wiki/Wayward_Guide" };
    public static readonly Quest ANewGuide = new() { Id = 428, Name = "A New Guide", WikiUrl = "https://wiki.guildwars.com/wiki/A_New_Guide" };
    public static readonly Quest GettingEven = new() { Id = 429, Name = "Getting Even", WikiUrl = "https://wiki.guildwars.com/wiki/Getting_Even" };
    public static readonly Quest SticksandStones = new() { Id = 430, Name = "Sticks and Stones", WikiUrl = "https://wiki.guildwars.com/wiki/Sticks_and_Stones" };
    public static readonly Quest ForgottenRetribution = new() { Id = 431, Name = "Forgotten Retribution", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Retribution" };
    public static readonly Quest DefendFortAspenwood = new() { Id = 432, Name = "Defend Fort Aspenwood", WikiUrl = "https://wiki.guildwars.com/wiki/Defend_Fort_Aspenwood" };
    public static readonly Quest TheJadeQuarryKurzick = new() { Id = 433, Name = "The Jade Quarry (Kurzick quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Quarry_(Kurzick_quest)" };
    public static readonly Quest FortAspenwood = new() { Id = 434, Name = "Fort Aspenwood (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Aspenwood_(quest)" };
    public static readonly Quest TheJadeQuarryLuxon = new() { Id = 435, Name = "The Jade Quarry (Luxon quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Quarry_(Luxon_quest)" };
    public static readonly Quest MhenlosRequest = new() { Id = 436, Name = "Mhenlos Request", WikiUrl = "https://wiki.guildwars.com/wiki/Mhenlo%27s_Request" };
    public static readonly Quest TheBogBeastofBokku = new() { Id = 437, Name = "The Bog Beast of Bokku", WikiUrl = "https://wiki.guildwars.com/wiki/The_Bog_Beast_of_Bokku" };
    public static readonly Quest ABeltPouch = new() { Id = 438, Name = "A Belt Pouch", WikiUrl = "https://wiki.guildwars.com/wiki/A_Belt_Pouch" };
    public static readonly Quest CashCrops = new() { Id = 439, Name = "Cash Crops", WikiUrl = "https://wiki.guildwars.com/wiki/Cash_Crops" };
    public static readonly Quest FormingaParty = new() { Id = 440, Name = "Forming a Party", WikiUrl = "https://wiki.guildwars.com/wiki/Forming_a_Party" };
    public static readonly Quest StudyBuddy = new() { Id = 441, Name = "Study Buddy", WikiUrl = "https://wiki.guildwars.com/wiki/Study_Buddy" };
    public static readonly Quest TheDragonHunter = new() { Id = 442, Name = "The Dragon Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/The_Dragon_Hunter" };
    public static readonly Quest MoreCowbell = new() { Id = 443, Name = "More Cowbell", WikiUrl = "https://wiki.guildwars.com/wiki/More_Cowbell" };
    public static readonly Quest ALetterHome = new() { Id = 444, Name = "A Letter Home", WikiUrl = "https://wiki.guildwars.com/wiki/A_Letter_Home" };
    public static readonly Quest ReturnoftheYeti = new() { Id = 445, Name = "Return of the Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Return_of_the_Yeti" };
    public static readonly Quest TheChallenge = new() { Id = 446, Name = "The Challenge", WikiUrl = "https://wiki.guildwars.com/wiki/The_Challenge" };
    public static readonly Quest Captured = new() { Id = 447, Name = "Captured", WikiUrl = "https://wiki.guildwars.com/wiki/Captured" };
    public static readonly Quest TheEmperorinPeril = new() { Id = 448, Name = "The Emperor in Peril", WikiUrl = "https://wiki.guildwars.com/wiki/The_Emperor_in_Peril" };
    public static readonly Quest ThePlotThickens = new() { Id = 449, Name = "The Plot Thickens", WikiUrl = "https://wiki.guildwars.com/wiki/The_Plot_Thickens" };
    public static readonly Quest ItEndsHere = new() { Id = 450, Name = "It Ends Here", WikiUrl = "https://wiki.guildwars.com/wiki/It_Ends_Here" };
    public static readonly Quest MastersofCorruption = new() { Id = 451, Name = "Masters of Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Masters_of_Corruption" };
    public static readonly Quest TheDrunkenMaster = new() { Id = 452, Name = "The Drunken Master", WikiUrl = "https://wiki.guildwars.com/wiki/The_Drunken_Master" };
    public static readonly Quest EliminatetheAmFah = new() { Id = 453, Name = "Eliminate the Am Fah", WikiUrl = "https://wiki.guildwars.com/wiki/Eliminate_the_Am_Fah" };
    public static readonly Quest EliminatetheJadeBrotherhood = new() { Id = 454, Name = "Eliminate the Jade Brotherhood", WikiUrl = "https://wiki.guildwars.com/wiki/Eliminate_the_Jade_Brotherhood" };
    public static readonly Quest ProblemSalving = new() { Id = 455, Name = "Problem Salving", WikiUrl = "https://wiki.guildwars.com/wiki/Problem_Salving" };
    public static readonly Quest GoingHome = new() { Id = 456, Name = "Going Home", WikiUrl = "https://wiki.guildwars.com/wiki/Going_Home" };
    public static readonly Quest MissingDaughter = new() { Id = 457, Name = "Missing Daughter", WikiUrl = "https://wiki.guildwars.com/wiki/Missing_Daughter" };
    public static readonly Quest TooHighaPrice = new() { Id = 458, Name = "Too High a Price", WikiUrl = "https://wiki.guildwars.com/wiki/Too_High_a_Price" };
    public static readonly Quest TheXunlaiAgent = new() { Id = 459, Name = "The Xunlai Agent", WikiUrl = "https://wiki.guildwars.com/wiki/The_Xunlai_Agent" };
    public static readonly Quest LuxuryGoods = new() { Id = 460, Name = "Luxury Goods", WikiUrl = "https://wiki.guildwars.com/wiki/Luxury_Goods" };
    public static readonly Quest ChartingtheForests = new() { Id = 461, Name = "Charting the Forests", WikiUrl = "https://wiki.guildwars.com/wiki/Charting_the_Forests" };
    public static readonly Quest ChartingtheJadeSea = new() { Id = 462, Name = "Charting the Jade Sea", WikiUrl = "https://wiki.guildwars.com/wiki/Charting_the_Jade_Sea" };
    public static readonly Quest StemmingtheTide = new() { Id = 463, Name = "Stemming the Tide", WikiUrl = "https://wiki.guildwars.com/wiki/Stemming_the_Tide" };
    public static readonly Quest TheMissingCorpses = new() { Id = 464, Name = "The Missing Corpses", WikiUrl = "https://wiki.guildwars.com/wiki/The_Missing_Corpses" };
    public static readonly Quest TheMissingLink = new() { Id = 465, Name = "The Missing Link", WikiUrl = "https://wiki.guildwars.com/wiki/The_Missing_Link" };
    public static readonly Quest TheAfflictedGuard = new() { Id = 466, Name = "The Afflicted Guard", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Guard" };
    public static readonly Quest SeekingShelter = new() { Id = 467, Name = "Seeking Shelter", WikiUrl = "https://wiki.guildwars.com/wiki/Seeking_Shelter" };
    public static readonly Quest HousingforRefugees = new() { Id = 468, Name = "Housing for Refugees", WikiUrl = "https://wiki.guildwars.com/wiki/Housing_for_Refugees" };
    public static readonly Quest AMonstrousRevelation = new() { Id = 469, Name = "A Monstrous Revelation", WikiUrl = "https://wiki.guildwars.com/wiki/A_Monstrous_Revelation" };
    public static readonly Quest TheShadowBlades = new() { Id = 470, Name = "The Shadow Blades", WikiUrl = "https://wiki.guildwars.com/wiki/The_Shadow_Blades" };
    public static readonly Quest LambstotheSlaughter = new() { Id = 471, Name = "Lambs to the Slaughter", WikiUrl = "https://wiki.guildwars.com/wiki/Lambs_to_the_Slaughter" };
    public static readonly Quest SeaofGreenCanopyofStone = new() { Id = 472, Name = "Sea of Green, Canopy of Stone", WikiUrl = "https://wiki.guildwars.com/wiki/Sea_of_Green,_Canopy_of_Stone" };
    public static readonly Quest FeedtheHungry = new() { Id = 473, Name = "Feed the Hungry", WikiUrl = "https://wiki.guildwars.com/wiki/Feed_the_Hungry" };
    public static readonly Quest PassingtheBuck = new() { Id = 474, Name = "Passing the Buck", WikiUrl = "https://wiki.guildwars.com/wiki/Passing_the_Buck" };
    public static readonly Quest RazetheRoost = new() { Id = 475, Name = "Raze the Roost", WikiUrl = "https://wiki.guildwars.com/wiki/Raze_the_Roost" };
    public static readonly Quest LastoftheBreed = new() { Id = 476, Name = "Last of the Breed", WikiUrl = "https://wiki.guildwars.com/wiki/Last_of_the_Breed" };
    public static readonly Quest RefusetheKing = new() { Id = 477, Name = "Refuse the King", WikiUrl = "https://wiki.guildwars.com/wiki/Refuse_the_King" };
    public static readonly Quest TheGrowingThreat = new() { Id = 478, Name = "The Growing Threat", WikiUrl = "https://wiki.guildwars.com/wiki/The_Growing_Threat" };
    public static readonly Quest ChaosinKryta = new() { Id = 479, Name = "Chaos in Kryta", WikiUrl = "https://wiki.guildwars.com/wiki/Chaos_in_Kryta" };
    public static readonly Quest VanishingSpirits = new() { Id = 480, Name = "Vanishing Spirits", WikiUrl = "https://wiki.guildwars.com/wiki/Vanishing_Spirits" };
    public static readonly Quest Pilgrimage = new() { Id = 481, Name = "Pilgrimage", WikiUrl = "https://wiki.guildwars.com/wiki/Pilgrimage" };
    public static readonly Quest RequiemforaBrain = new() { Id = 482, Name = "Requiem for a Brain", WikiUrl = "https://wiki.guildwars.com/wiki/Requiem_for_a_Brain" };
    public static readonly Quest UnderNewManagement = new() { Id = 483, Name = "Under New Management", WikiUrl = "https://wiki.guildwars.com/wiki/Under_New_Management" };
    public static readonly Quest DestroytheUngratefulSlaves = new() { Id = 484, Name = "Destroy the Ungrateful Slaves", WikiUrl = "https://wiki.guildwars.com/wiki/Destroy_the_Ungrateful_Slaves" };
    public static readonly Quest AShowofForce = new() { Id = 485, Name = "A Show of Force", WikiUrl = "https://wiki.guildwars.com/wiki/A_Show_of_Force" };
    public static readonly Quest TempleoftheMonoliths = new() { Id = 486, Name = "Temple of the Monoliths", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_of_the_Monoliths" };
    public static readonly Quest ReturnoftheUndeadKing = new() { Id = 487, Name = "Return of the Undead King", WikiUrl = "https://wiki.guildwars.com/wiki/Return_of_the_Undead_King" };
    public static readonly Quest StrangeBedfellows = new() { Id = 488, Name = "Strange Bedfellows", WikiUrl = "https://wiki.guildwars.com/wiki/Strange_Bedfellows" };
    public static readonly Quest SheHungers = new() { Id = 489, Name = "She Hungers", WikiUrl = "https://wiki.guildwars.com/wiki/She_Hungers" };
    public static readonly Quest TheCouncilisCalled = new() { Id = 490, Name = "The Council is Called", WikiUrl = "https://wiki.guildwars.com/wiki/The_Council_is_Called" };
    public static readonly Quest DesperateMeasures = new() { Id = 491, Name = "Desperate Measures", WikiUrl = "https://wiki.guildwars.com/wiki/Desperate_Measures" };
    public static readonly Quest TheSearchforSurvivors = new() { Id = 492, Name = "The Search for Survivors", WikiUrl = "https://wiki.guildwars.com/wiki/The_Search_for_Survivors" };
    public static readonly Quest ASoundofAncientHorns = new() { Id = 493, Name = "A Sound of Ancient Horns", WikiUrl = "https://wiki.guildwars.com/wiki/A_Sound_of_Ancient_Horns" };
    public static readonly Quest GardenChores = new() { Id = 494, Name = "Garden Chores", WikiUrl = "https://wiki.guildwars.com/wiki/Garden_Chores" };
    public static readonly Quest WhichWayDidHeGo = new() { Id = 495, Name = "Which Way Did He Go%3F", WikiUrl = "https://wiki.guildwars.com/wiki/Which_Way_Did_He_Go%3F" };
    public static readonly Quest OneMansDream = new() { Id = 496, Name = "One Mans Dream", WikiUrl = "https://wiki.guildwars.com/wiki/One_Man%27s_Dream" };
    public static readonly Quest PuzzlingParchment = new() { Id = 497, Name = "Puzzling Parchment", WikiUrl = "https://wiki.guildwars.com/wiki/Puzzling_Parchment" };
    public static readonly Quest ABriefIntroduction = new() { Id = 498, Name = "A Brief Introduction", WikiUrl = "https://wiki.guildwars.com/wiki/A_Brief_Introduction" };
    public static readonly Quest PlagueofFrogs = new() { Id = 499, Name = "Plague of Frogs", WikiUrl = "https://wiki.guildwars.com/wiki/Plague_of_Frogs" };
    public static readonly Quest BetweenaRock = new() { Id = 500, Name = "Between a Rock...", WikiUrl = "https://wiki.guildwars.com/wiki/Between_a_Rock..." };
    public static readonly Quest BotanicalResearch = new() { Id = 501, Name = "Botanical Research", WikiUrl = "https://wiki.guildwars.com/wiki/Botanical_Research" };
    public static readonly Quest RallyThePrinces = new() { Id = 502, Name = "Rally The Princes", WikiUrl = "https://wiki.guildwars.com/wiki/Rally_The_Princes" };
    public static readonly Quest AllsWellThatEndsWell = new() { Id = 503, Name = "Alls Well That Ends Well", WikiUrl = "https://wiki.guildwars.com/wiki/All%27s_Well_That_Ends_Well" };
    public static readonly Quest WarningKehanni = new() { Id = 504, Name = "Warning Kehanni", WikiUrl = "https://wiki.guildwars.com/wiki/Warning_Kehanni" };
    public static readonly Quest CallingtheOrder = new() { Id = 505, Name = "Calling the Order", WikiUrl = "https://wiki.guildwars.com/wiki/Calling_the_Order" };
    public static readonly Quest GreedandRegret = new() { Id = 506, Name = "Greed and Regret", WikiUrl = "https://wiki.guildwars.com/wiki/Greed_and_Regret" };
    public static readonly Quest PledgeoftheMerchantPrinces = new() { Id = 507, Name = "Pledge of the Merchant Princes", WikiUrl = "https://wiki.guildwars.com/wiki/Pledge_of_the_Merchant_Princes" };
    public static readonly Quest AnOldMansPast = new() { Id = 508, Name = "An Old Mans Past", WikiUrl = "https://wiki.guildwars.com/wiki/An_Old_Man%27s_Past" };
    public static readonly Quest AnOldMansShame = new() { Id = 509, Name = "An Old Mans Shame", WikiUrl = "https://wiki.guildwars.com/wiki/An_Old_Man%27s_Shame" };
    public static readonly Quest TotheRescueNightfall = new() { Id = 510, Name = "To the Rescue (Nightfall quest)", WikiUrl = "https://wiki.guildwars.com/wiki/To_the_Rescue_(Nightfall_quest)" };
    public static readonly Quest CofferofJoko = new() { Id = 511, Name = "Coffer of Joko", WikiUrl = "https://wiki.guildwars.com/wiki/Coffer_of_Joko" };
    public static readonly Quest DataMining = new() { Id = 512, Name = "Data Mining", WikiUrl = "https://wiki.guildwars.com/wiki/Data_Mining" };
    public static readonly Quest TheMadnessofProphecy = new() { Id = 513, Name = "The Madness of Prophecy", WikiUrl = "https://wiki.guildwars.com/wiki/The_Madness_of_Prophecy" };
    public static readonly Quest ProtecttheLearned = new() { Id = 514, Name = "Protect the Learned", WikiUrl = "https://wiki.guildwars.com/wiki/Protect_the_Learned" };
    public static readonly Quest ReclaimingtheTemple = new() { Id = 515, Name = "Reclaiming the Temple", WikiUrl = "https://wiki.guildwars.com/wiki/Reclaiming_the_Temple" };
    public static readonly Quest TheSearchforEnlightenment = new() { Id = 516, Name = "The Search for Enlightenment", WikiUrl = "https://wiki.guildwars.com/wiki/The_Search_for_Enlightenment" };
    public static readonly Quest SummertimeforBokka = new() { Id = 517, Name = "Summertime for Bokka", WikiUrl = "https://wiki.guildwars.com/wiki/Summertime_for_Bokka" };
    public static readonly Quest InDefenseofTheatre = new() { Id = 518, Name = "In Defense of Theatre", WikiUrl = "https://wiki.guildwars.com/wiki/In_Defense_of_Theatre" };
    public static readonly Quest WorstPerformanceEver = new() { Id = 519, Name = "Worst. Performance. Ever.", WikiUrl = "https://wiki.guildwars.com/wiki/Worst._Performance._Ever." };
    public static readonly Quest TheShowMustGoOn = new() { Id = 520, Name = "The Show Must Go On", WikiUrl = "https://wiki.guildwars.com/wiki/The_Show_Must_Go_On" };
    public static readonly Quest ValleyoftheRifts = new() { Id = 521, Name = "Valley of the Rifts", WikiUrl = "https://wiki.guildwars.com/wiki/Valley_of_the_Rifts" };
    public static readonly Quest PopulationControl = new() { Id = 522, Name = "Population Control", WikiUrl = "https://wiki.guildwars.com/wiki/Population_Control" };
    public static readonly Quest DestroytheHarpies = new() { Id = 523, Name = "Destroy the Harpies", WikiUrl = "https://wiki.guildwars.com/wiki/Destroy_the_Harpies" };
    public static readonly Quest GuardRescue = new() { Id = 524, Name = "Guard Rescue", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Rescue" };
    public static readonly Quest GiftoftheDjinn = new() { Id = 525, Name = "Gift of the Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Gift_of_the_Djinn" };
    public static readonly Quest Interception = new() { Id = 526, Name = "Interception", WikiUrl = "https://wiki.guildwars.com/wiki/Interception" };
    public static readonly Quest OldFriends = new() { Id = 527, Name = "Old Friends", WikiUrl = "https://wiki.guildwars.com/wiki/Old_Friends" };
    public static readonly Quest ForYourEarsOnly = new() { Id = 528, Name = "For Your Ears Only", WikiUrl = "https://wiki.guildwars.com/wiki/For_Your_Ears_Only" };
    public static readonly Quest TheScourgeofVabbi = new() { Id = 529, Name = "The Scourge of Vabbi", WikiUrl = "https://wiki.guildwars.com/wiki/The_Scourge_of_Vabbi" };
    public static readonly Quest TheHangingGardener = new() { Id = 530, Name = "The Hanging Gardener", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hanging_Gardener" };
    public static readonly Quest ScorchedEarthNightfall = new() { Id = 531, Name = "Scorched Earth (Nightfall quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Scorched_Earth_(Nightfall_quest)" };
    public static readonly Quest Extinction = new() { Id = 532, Name = "Extinction", WikiUrl = "https://wiki.guildwars.com/wiki/Extinction" };
    public static readonly Quest InsatiableAppetite = new() { Id = 533, Name = "Insatiable Appetite", WikiUrl = "https://wiki.guildwars.com/wiki/Insatiable_Appetite" };
    public static readonly Quest BuildingtheBasePrisonersofWar = new() { Id = 534, Name = "Building the Base Prisoners of War", WikiUrl = "https://wiki.guildwars.com/wiki/Building_the_Base:_Prisoners_of_War" };
    public static readonly Quest BuildingtheBaseTheInterrogation = new() { Id = 535, Name = "Building the Base The Interrogation", WikiUrl = "https://wiki.guildwars.com/wiki/Building_the_Base:_The_Interrogation" };
    public static readonly Quest BuildingtheBaseTheMeeting = new() { Id = 536, Name = "Building the Base The Meeting", WikiUrl = "https://wiki.guildwars.com/wiki/Building_the_Base:_The_Meeting" };
    public static readonly Quest TheToolsofWar = new() { Id = 537, Name = "The Tools of War", WikiUrl = "https://wiki.guildwars.com/wiki/The_Tools_of_War" };
    public static readonly Quest FeedingFrenzy = new() { Id = 538, Name = "Feeding Frenzy", WikiUrl = "https://wiki.guildwars.com/wiki/Feeding_Frenzy" };
    public static readonly Quest YouCanRun = new() { Id = 539, Name = "You Can Run...", WikiUrl = "https://wiki.guildwars.com/wiki/You_Can_Run..." };
    public static readonly Quest CentaurConcerns = new() { Id = 540, Name = "Centaur Concerns", WikiUrl = "https://wiki.guildwars.com/wiki/Centaur_Concerns" };
    public static readonly Quest MirzasLastStand = new() { Id = 541, Name = "Mirzas Last Stand", WikiUrl = "https://wiki.guildwars.com/wiki/Mirza%27s_Last_Stand" };
    public static readonly Quest BattleofTuraisProcession = new() { Id = 542, Name = "Battle of Turais Procession", WikiUrl = "https://wiki.guildwars.com/wiki/Battle_of_Turai%27s_Procession" };
    public static readonly Quest AQuestionofMorality = new() { Id = 543, Name = "A Question of Morality", WikiUrl = "https://wiki.guildwars.com/wiki/A_Question_of_Morality" };
    public static readonly Quest ABushelofTrouble = new() { Id = 544, Name = "A Bushel of Trouble", WikiUrl = "https://wiki.guildwars.com/wiki/A_Bushel_of_Trouble" };
    public static readonly Quest TheGreatZehtuka = new() { Id = 545, Name = "The Great Zehtuka (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Zehtuka_(quest)" };
    public static readonly Quest Eavesdropping = new() { Id = 546, Name = "Eavesdropping", WikiUrl = "https://wiki.guildwars.com/wiki/Eavesdropping" };
    public static readonly Quest ALittleRecon = new() { Id = 547, Name = "A Little Recon", WikiUrl = "https://wiki.guildwars.com/wiki/A_Little_Recon" };
    public static readonly Quest Hunted = new() { Id = 548, Name = "Hunted!", WikiUrl = "https://wiki.guildwars.com/wiki/Hunted!" };
    public static readonly Quest TheGreatEscape = new() { Id = 549, Name = "The Great Escape", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Escape" };
    public static readonly Quest AndaHeroShallLeadThem = new() { Id = 550, Name = "And a Hero Shall Lead Them", WikiUrl = "https://wiki.guildwars.com/wiki/And_a_Hero_Shall_Lead_Them" };
    public static readonly Quest ToVabbi = new() { Id = 551, Name = "To Vabbi!", WikiUrl = "https://wiki.guildwars.com/wiki/To_Vabbi!" };
    public static readonly Quest CentaurBlackmail = new() { Id = 552, Name = "Centaur Blackmail", WikiUrl = "https://wiki.guildwars.com/wiki/Centaur_Blackmail" };
    public static readonly Quest MysteriousMessageNightfall = new() { Id = 553, Name = "Mysterious Message (Nightfall quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Mysterious_Message_(Nightfall_quest)" };
    public static readonly Quest SecretsintheShadow = new() { Id = 554, Name = "Secrets in the Shadow", WikiUrl = "https://wiki.guildwars.com/wiki/Secrets_in_the_Shadow" };
    public static readonly Quest ToKillaDemon = new() { Id = 555, Name = "To Kill a Demon", WikiUrl = "https://wiki.guildwars.com/wiki/To_Kill_a_Demon" };
    public static readonly Quest ForaPrice = new() { Id = 556, Name = "For a Price", WikiUrl = "https://wiki.guildwars.com/wiki/For_a_Price" };
    public static readonly Quest NoMeNoKormir = new() { Id = 557, Name = "No Me, No Kormir", WikiUrl = "https://wiki.guildwars.com/wiki/No_Me,_No_Kormir" };
    public static readonly Quest AThorninVareshsSide = new() { Id = 558, Name = "A Thorn in Vareshs Side", WikiUrl = "https://wiki.guildwars.com/wiki/A_Thorn_in_Varesh%27s_Side" };
    public static readonly Quest TenderinganOffer = new() { Id = 559, Name = "Tendering an Offer", WikiUrl = "https://wiki.guildwars.com/wiki/Tendering_an_Offer" };
    public static readonly Quest EstatePlanning = new() { Id = 560, Name = "Estate Planning", WikiUrl = "https://wiki.guildwars.com/wiki/Estate_Planning" };
    public static readonly Quest FamilyTiesNightfall = new() { Id = 561, Name = "Family Ties (Nightfall quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Family_Ties_(Nightfall_quest)" };
    public static readonly Quest AMessageforJaneera = new() { Id = 562, Name = "A Message for Janeera", WikiUrl = "https://wiki.guildwars.com/wiki/A_Message_for_Janeera" };
    public static readonly Quest StrangeAllies = new() { Id = 563, Name = "Strange Allies", WikiUrl = "https://wiki.guildwars.com/wiki/Strange_Allies" };
    public static readonly Quest TheFoolhardyFather = new() { Id = 564, Name = "The Foolhardy Father", WikiUrl = "https://wiki.guildwars.com/wiki/The_Foolhardy_Father" };
    public static readonly Quest TheYoungLadyVanishes = new() { Id = 565, Name = "The Young Lady Vanishes", WikiUrl = "https://wiki.guildwars.com/wiki/The_Young_Lady_Vanishes" };
    public static readonly Quest OldWomanRiver = new() { Id = 566, Name = "Old Woman River", WikiUrl = "https://wiki.guildwars.com/wiki/Old_Woman_River" };
    public static readonly Quest AFoolsLuck = new() { Id = 567, Name = "A Fools Luck", WikiUrl = "https://wiki.guildwars.com/wiki/A_Fool%27s_Luck" };
    public static readonly Quest HerdstotheSlaughter = new() { Id = 568, Name = "Herds to the Slaughter", WikiUrl = "https://wiki.guildwars.com/wiki/Herds_to_the_Slaughter" };
    public static readonly Quest KosssElixir = new() { Id = 569, Name = "Kosss Elixir", WikiUrl = "https://wiki.guildwars.com/wiki/Koss%27s_Elixir" };
    public static readonly Quest DrakeinaCage = new() { Id = 570, Name = "Drake in a Cage", WikiUrl = "https://wiki.guildwars.com/wiki/Drake_in_a_Cage" };
    public static readonly Quest TroubledLands = new() { Id = 571, Name = "Troubled Lands", WikiUrl = "https://wiki.guildwars.com/wiki/Troubled_Lands" };
    public static readonly Quest AncestralAnguish = new() { Id = 572, Name = "Ancestral Anguish", WikiUrl = "https://wiki.guildwars.com/wiki/Ancestral_Anguish" };
    public static readonly Quest TotalCorruption = new() { Id = 573, Name = "Total Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Total_Corruption" };
    public static readonly Quest FishinaBarrel = new() { Id = 574, Name = "Fish in a Barrel", WikiUrl = "https://wiki.guildwars.com/wiki/Fish_in_a_Barrel" };
    public static readonly Quest MelonnisMeditations = new() { Id = 575, Name = "Melonnis Meditations", WikiUrl = "https://wiki.guildwars.com/wiki/Melonni%27s_Meditations" };
    public static readonly Quest WeirdWaters = new() { Id = 576, Name = "Weird Waters", WikiUrl = "https://wiki.guildwars.com/wiki/Weird_Waters" };
    public static readonly Quest APrescriptionforConscription = new() { Id = 577, Name = "A Prescription for Conscription", WikiUrl = "https://wiki.guildwars.com/wiki/A_Prescription_for_Conscription" };
    public static readonly Quest UndeadDefenders = new() { Id = 578, Name = "Undead Defenders", WikiUrl = "https://wiki.guildwars.com/wiki/Undead_Defenders" };
    public static readonly Quest TheColdTouchofthePast = new() { Id = 579, Name = "The Cold Touch of the Past", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cold_Touch_of_the_Past" };
    public static readonly Quest RaisinganArmy = new() { Id = 580, Name = "Raising an Army", WikiUrl = "https://wiki.guildwars.com/wiki/Raising_an_Army" };
    public static readonly Quest HeartorMindGardeninDanger = new() { Id = 581, Name = "Heart or Mind Garden in Danger", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_or_Mind:_Garden_in_Danger" };
    public static readonly Quest TheHallowedPoint = new() { Id = 582, Name = "The Hallowed Point", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hallowed_Point" };
    public static readonly Quest ADealsaDeal = new() { Id = 583, Name = "A Deals a Deal", WikiUrl = "https://wiki.guildwars.com/wiki/A_Deal%27s_a_Deal" };
    public static readonly Quest HordeofDarkness = new() { Id = 584, Name = "Horde of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Horde_of_Darkness" };
    public static readonly Quest FamilySoul = new() { Id = 585, Name = "Family Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Family_Soul" };
    public static readonly Quest HeartorMindRonjokinDanger = new() { Id = 586, Name = "Heart or Mind Ronjok in Danger", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_or_Mind:_Ronjok_in_Danger" };
    public static readonly Quest AssaultonBeknurHarbor = new() { Id = 587, Name = "Assault on Beknur Harbor", WikiUrl = "https://wiki.guildwars.com/wiki/Assault_on_Beknur_Harbor" };
    public static readonly Quest MoavuKaalAwakened = new() { Id = 588, Name = "MoavuKaal, Awakened", WikiUrl = "https://wiki.guildwars.com/wiki/Moa%27vu%27Kaal,_Awakened" };
    public static readonly Quest TheCyclonePalace = new() { Id = 589, Name = "The Cyclone Palace", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cyclone_Palace" };
    public static readonly Quest DownontheBayou = new() { Id = 590, Name = "Down on the Bayou", WikiUrl = "https://wiki.guildwars.com/wiki/Down_on_the_Bayou" };
    public static readonly Quest DoubleDogDare = new() { Id = 591, Name = "Double Dog Dare", WikiUrl = "https://wiki.guildwars.com/wiki/Double_Dog_Dare" };
    public static readonly Quest AMessageHome = new() { Id = 592, Name = "A Message Home", WikiUrl = "https://wiki.guildwars.com/wiki/A_Message_Home" };
    public static readonly Quest OneGoodTurnDeservesAnother = new() { Id = 593, Name = "One Good Turn Deserves Another", WikiUrl = "https://wiki.guildwars.com/wiki/One_Good_Turn_Deserves_Another" };
    public static readonly Quest WhatDoYouDowithaDrunkenShauben = new() { Id = 594, Name = "What Do You Do with a Drunken Shauben%3F", WikiUrl = "https://wiki.guildwars.com/wiki/What_Do_You_Do_with_a_Drunken_Shauben%3F" };
    public static readonly Quest SecretPassage = new() { Id = 595, Name = "Secret Passage", WikiUrl = "https://wiki.guildwars.com/wiki/Secret_Passage" };
    public static readonly Quest ChooseYourSecondaryProfessionNightfall = new() { Id = 596, Name = "Choose Your Secondary Profession (Nightfall quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Choose_Your_Secondary_Profession_(Nightfall_quest)" };
    public static readonly Quest CorsairVengeance = new() { Id = 597, Name = "Corsair Vengeance", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Vengeance" };
    public static readonly Quest DefendersChoice = new() { Id = 598, Name = "Defenders Choice", WikiUrl = "https://wiki.guildwars.com/wiki/Defender%27s_Choice" };
    public static readonly Quest TheLoneRaider = new() { Id = 599, Name = "The Lone Raider", WikiUrl = "https://wiki.guildwars.com/wiki/The_Lone_Raider" };
    public static readonly Quest PrimaryTraining = new() { Id = 600, Name = "Primary Training", WikiUrl = "https://wiki.guildwars.com/wiki/Primary_Training" };
    public static readonly Quest SecondaryTraining = new() { Id = 601, Name = "Secondary Training", WikiUrl = "https://wiki.guildwars.com/wiki/Secondary_Training" };
    public static readonly Quest AHiddenThreat = new() { Id = 602, Name = "A Hidden Threat", WikiUrl = "https://wiki.guildwars.com/wiki/A_Hidden_Threat" };
    public static readonly Quest WanderedOffAgain = new() { Id = 603, Name = "Wandered Off Again", WikiUrl = "https://wiki.guildwars.com/wiki/Wandered_Off_Again" };
    public static readonly Quest AngeroftheStoneFace = new() { Id = 604, Name = "Anger of the Stone Face", WikiUrl = "https://wiki.guildwars.com/wiki/Anger_of_the_Stone_Face" };
    public static readonly Quest TheCultoftheStoneFace = new() { Id = 605, Name = "The Cult of the Stone Face", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cult_of_the_Stone_Face" };
    public static readonly Quest StoneFacedOrders = new() { Id = 606, Name = "Stone-Faced Orders", WikiUrl = "https://wiki.guildwars.com/wiki/Stone-Faced_Orders" };
    public static readonly Quest ArmoredTransport = new() { Id = 607, Name = "Armored Transport", WikiUrl = "https://wiki.guildwars.com/wiki/Armored_Transport" };
    public static readonly Quest QualitySteel = new() { Id = 608, Name = "Quality Steel", WikiUrl = "https://wiki.guildwars.com/wiki/Quality_Steel" };
    public static readonly Quest MaterialGirl = new() { Id = 609, Name = "Material Girl", WikiUrl = "https://wiki.guildwars.com/wiki/Material_Girl" };
    public static readonly Quest SuwashthePirate = new() { Id = 610, Name = "Suwash the Pirate", WikiUrl = "https://wiki.guildwars.com/wiki/Suwash_the_Pirate" };
    public static readonly Quest CatchoftheDay = new() { Id = 611, Name = "Catch of the Day", WikiUrl = "https://wiki.guildwars.com/wiki/Catch_of_the_Day" };
    public static readonly Quest Flamingoinggoinggone = new() { Id = 612, Name = "Flamingo-ing... going... gone.", WikiUrl = "https://wiki.guildwars.com/wiki/Flamingo-ing..._going..._gone." };
    public static readonly Quest ALeapofFaith = new() { Id = 613, Name = "A Leap of Faith", WikiUrl = "https://wiki.guildwars.com/wiki/A_Leap_of_Faith" };
    public static readonly Quest ZaishenElite = new() { Id = 614, Name = "Zaishen Elite (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_Elite_(quest)" };
    public static readonly Quest StudentJin = new() { Id = 615, Name = "Student Jin", WikiUrl = "https://wiki.guildwars.com/wiki/Student_Jin" };
    public static readonly Quest StudentSousuke = new() { Id = 616, Name = "Student Sousuke", WikiUrl = "https://wiki.guildwars.com/wiki/Student_Sousuke" };
    public static readonly Quest FeastofBokka = new() { Id = 617, Name = "Feast of Bokka", WikiUrl = "https://wiki.guildwars.com/wiki/Feast_of_Bokka" };
    public static readonly Quest AGhostlyRequest = new() { Id = 618, Name = "A Ghostly Request", WikiUrl = "https://wiki.guildwars.com/wiki/A_Ghostly_Request" };
    public static readonly Quest GhostsintheGraveyard = new() { Id = 619, Name = "Ghosts in the Graveyard", WikiUrl = "https://wiki.guildwars.com/wiki/Ghosts_in_the_Graveyard" };
    public static readonly Quest SkreeHatchlingSeason = new() { Id = 620, Name = "Skree Hatchling Season", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Hatchling_Season" };
    public static readonly Quest AFriendinNeed = new() { Id = 621, Name = "A Friend in Need", WikiUrl = "https://wiki.guildwars.com/wiki/A_Friend_in_Need" };
    public static readonly Quest AFathersFate = new() { Id = 622, Name = "A Fathers Fate", WikiUrl = "https://wiki.guildwars.com/wiki/A_Father%27s_Fate" };
    public static readonly Quest SecondBorn = new() { Id = 623, Name = "Second Born", WikiUrl = "https://wiki.guildwars.com/wiki/Second_Born" };
    public static readonly Quest FirstBorn = new() { Id = 624, Name = "First Born", WikiUrl = "https://wiki.guildwars.com/wiki/First_Born" };
    public static readonly Quest ThirdBorn = new() { Id = 625, Name = "Third Born", WikiUrl = "https://wiki.guildwars.com/wiki/Third_Born" };
    public static readonly Quest IdentityTheft = new() { Id = 626, Name = "Identity Theft", WikiUrl = "https://wiki.guildwars.com/wiki/Identity_Theft" };
    public static readonly Quest NeedMoreStuff = new() { Id = 627, Name = "Need More Stuff", WikiUrl = "https://wiki.guildwars.com/wiki/Need_More_Stuff" };
    public static readonly Quest ToDyeFor = new() { Id = 628, Name = "To Dye For", WikiUrl = "https://wiki.guildwars.com/wiki/To_Dye_For" };
    public static readonly Quest MissingShipment = new() { Id = 629, Name = "Missing Shipment", WikiUrl = "https://wiki.guildwars.com/wiki/Missing_Shipment" };
    public static readonly Quest BlowOutSale = new() { Id = 630, Name = "Blow Out Sale!", WikiUrl = "https://wiki.guildwars.com/wiki/Blow_Out_Sale!" };
    public static readonly Quest ScholarlyAffairs = new() { Id = 631, Name = "Scholarly Affairs", WikiUrl = "https://wiki.guildwars.com/wiki/Scholarly_Affairs" };
    public static readonly Quest LeavingaLegacy = new() { Id = 632, Name = "Leaving a Legacy", WikiUrl = "https://wiki.guildwars.com/wiki/Leaving_a_Legacy" };
    public static readonly Quest TheHonorableGeneral = new() { Id = 633, Name = "The Honorable General", WikiUrl = "https://wiki.guildwars.com/wiki/The_Honorable_General" };
    public static readonly Quest SignsandPortents = new() { Id = 634, Name = "Signs and Portents", WikiUrl = "https://wiki.guildwars.com/wiki/Signs_and_Portents" };
    public static readonly Quest IsleoftheDead = new() { Id = 635, Name = "Isle of the Dead (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Isle_of_the_Dead_(quest)" };
    public static readonly Quest BadTideRising = new() { Id = 636, Name = "Bad Tide Rising", WikiUrl = "https://wiki.guildwars.com/wiki/Bad_Tide_Rising" };
    public static readonly Quest SpecialDelivery = new() { Id = 637, Name = "Special Delivery", WikiUrl = "https://wiki.guildwars.com/wiki/Special_Delivery" };
    public static readonly Quest BigNewsSmallPackage = new() { Id = 638, Name = "Big News, Small Package", WikiUrl = "https://wiki.guildwars.com/wiki/Big_News,_Small_Package" };
    public static readonly Quest FollowingtheTrail = new() { Id = 639, Name = "Following the Trail", WikiUrl = "https://wiki.guildwars.com/wiki/Following_the_Trail" };
    public static readonly Quest TheIronTruth = new() { Id = 640, Name = "The Iron Truth", WikiUrl = "https://wiki.guildwars.com/wiki/The_Iron_Truth" };
    public static readonly Quest TrialbyFire = new() { Id = 641, Name = "Trial by Fire", WikiUrl = "https://wiki.guildwars.com/wiki/Trial_by_Fire" };
    public static readonly Quest WarPreparationsRecruitTraining = new() { Id = 642, Name = "War Preparations (Recruit Training)", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations_(Recruit_Training)" };
    public static readonly Quest WarPreparationsWindandWater = new() { Id = 643, Name = "War Preparations (Wind and Water)", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations_(Wind_and_Water)" };
    public static readonly Quest WarPreparationsGhostReconnaissance = new() { Id = 644, Name = "War Preparations (Ghost Reconnaissance)", WikiUrl = "https://wiki.guildwars.com/wiki/War_Preparations_(Ghost_Reconnaissance)" };
    public static readonly Quest TheTimeisNigh = new() { Id = 645, Name = "The Time is Nigh", WikiUrl = "https://wiki.guildwars.com/wiki/The_Time_is_Nigh" };
    public static readonly Quest ALooseCannon = new() { Id = 646, Name = "A Loose Cannon", WikiUrl = "https://wiki.guildwars.com/wiki/A_Loose_Cannon" };
    public static readonly Quest DiamondintheRough = new() { Id = 647, Name = "Diamond in the Rough", WikiUrl = "https://wiki.guildwars.com/wiki/Diamond_in_the_Rough" };
    public static readonly Quest MapTravelInventor = new() { Id = 648, Name = "Map-Travel Inventor", WikiUrl = "https://wiki.guildwars.com/wiki/Map-Travel_Inventor" };
    public static readonly Quest HoningyourSkills = new() { Id = 649, Name = "Honing your Skills", WikiUrl = "https://wiki.guildwars.com/wiki/Honing_your_Skills" };
    public static readonly Quest VoicesintheNight = new() { Id = 650, Name = "Voices in the Night", WikiUrl = "https://wiki.guildwars.com/wiki/Voices_in_the_Night" };
    public static readonly Quest CorsairInvasion = new() { Id = 651, Name = "Corsair Invasion", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Invasion" };
    public static readonly Quest APeacefulSolution = new() { Id = 652, Name = "A Peaceful Solution", WikiUrl = "https://wiki.guildwars.com/wiki/A_Peaceful_Solution" };
    public static readonly Quest HogHunt = new() { Id = 653, Name = "Hog Hunt", WikiUrl = "https://wiki.guildwars.com/wiki/Hog_Hunt" };
    public static readonly Quest ADelayedDelivery = new() { Id = 654, Name = "A Delayed Delivery", WikiUrl = "https://wiki.guildwars.com/wiki/A_Delayed_Delivery" };
    public static readonly Quest ATroublingTheory = new() { Id = 655, Name = "A Troubling Theory", WikiUrl = "https://wiki.guildwars.com/wiki/A_Troubling_Theory" };
    public static readonly Quest AMysteriousMissive = new() { Id = 656, Name = "A Mysterious Missive", WikiUrl = "https://wiki.guildwars.com/wiki/A_Mysterious_Missive" };
    public static readonly Quest APerplexingPlague = new() { Id = 657, Name = "A Perplexing Plague", WikiUrl = "https://wiki.guildwars.com/wiki/A_Perplexing_Plague" };
    public static readonly Quest AStolenSpore = new() { Id = 658, Name = "A Stolen Spore", WikiUrl = "https://wiki.guildwars.com/wiki/A_Stolen_Spore" };
    public static readonly Quest ProofofCourage = new() { Id = 659, Name = "Proof of Courage", WikiUrl = "https://wiki.guildwars.com/wiki/Proof_of_Courage" };
    public static readonly Quest QuarryQuandry = new() { Id = 660, Name = "Quarry Quandry", WikiUrl = "https://wiki.guildwars.com/wiki/Quarry_Quandry" };
    public static readonly Quest QueenoftheQuarry = new() { Id = 661, Name = "Queen of the Quarry", WikiUrl = "https://wiki.guildwars.com/wiki/Queen_of_the_Quarry" };
    public static readonly Quest AStickyOperation = new() { Id = 662, Name = "A Sticky Operation", WikiUrl = "https://wiki.guildwars.com/wiki/A_Sticky_Operation" };
    public static readonly Quest RisingSuns = new() { Id = 663, Name = "Rising Suns", WikiUrl = "https://wiki.guildwars.com/wiki/Rising_Suns" };
    public static readonly Quest ReenlistRojis = new() { Id = 664, Name = "Re-enlist Rojis", WikiUrl = "https://wiki.guildwars.com/wiki/Re-enlist_Rojis" };
    public static readonly Quest SkaleandMagicCompass = new() { Id = 665, Name = "Skale and Magic Compass", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_and_Magic_Compass" };
    public static readonly Quest ToSeetheSights = new() { Id = 666, Name = "To See the Sights", WikiUrl = "https://wiki.guildwars.com/wiki/To_See_the_Sights" };
    public static readonly Quest ToAskForMore = new() { Id = 667, Name = "To Ask For More", WikiUrl = "https://wiki.guildwars.com/wiki/To_Ask_For_More" };
    public static readonly Quest MoneyontheSide = new() { Id = 668, Name = "Money on the Side", WikiUrl = "https://wiki.guildwars.com/wiki/Money_on_the_Side" };
    public static readonly Quest FuryofaGrievingHeart = new() { Id = 669, Name = "Fury of a Grieving Heart", WikiUrl = "https://wiki.guildwars.com/wiki/Fury_of_a_Grieving_Heart" };
    public static readonly Quest Cryptology = new() { Id = 670, Name = "Crypt-ology", WikiUrl = "https://wiki.guildwars.com/wiki/Crypt-ology" };
    public static readonly Quest StolenSupplies = new() { Id = 671, Name = "Stolen Supplies", WikiUrl = "https://wiki.guildwars.com/wiki/Stolen_Supplies" };
    public static readonly Quest LoyaltiltheEnd = new() { Id = 672, Name = "Loyal til the End", WikiUrl = "https://wiki.guildwars.com/wiki/Loyal_%27til_the_End" };
    public static readonly Quest APersonalVault = new() { Id = 673, Name = "A Personal Vault", WikiUrl = "https://wiki.guildwars.com/wiki/A_Personal_Vault" };
    public static readonly Quest TradeRelations = new() { Id = 674, Name = "Trade Relations", WikiUrl = "https://wiki.guildwars.com/wiki/Trade_Relations" };
    public static readonly Quest RallytheRecruitsTutorial = new() { Id = 675, Name = "Rally the Recruits (Tutorial)", WikiUrl = "https://wiki.guildwars.com/wiki/Rally_the_Recruits_(Tutorial)" };
    public static readonly Quest IntoChahbekVillage = new() { Id = 676, Name = "Into Chahbek Village", WikiUrl = "https://wiki.guildwars.com/wiki/Into_Chahbek_Village" };
    public static readonly Quest TaketheShortcutSkipTutorial = new() { Id = 677, Name = "Take the Shortcut (Skip Tutorial)", WikiUrl = "https://wiki.guildwars.com/wiki/Take_the_Shortcut_(Skip_Tutorial)" };
    public static readonly Quest ADecayedMonument = new() { Id = 678, Name = "A Decayed Monument", WikiUrl = "https://wiki.guildwars.com/wiki/A_Decayed_Monument" };
    public static readonly Quest GorensStuffPart1 = new() { Id = 679, Name = "Gorens Stuff Part 1", WikiUrl = "https://wiki.guildwars.com/wiki/Goren%27s_Stuff:_Part_1" };
    public static readonly Quest AttackattheKodash = new() { Id = 680, Name = "Attack at the Kodash", WikiUrl = "https://wiki.guildwars.com/wiki/Attack_at_the_Kodash" };
    public static readonly Quest MelonniGoesRecruiting = new() { Id = 682, Name = "Melonni Goes Recruiting", WikiUrl = "https://wiki.guildwars.com/wiki/Melonni_Goes_Recruiting" };
    public static readonly Quest SecuringChampionsDawn = new() { Id = 683, Name = "Securing Champions Dawn", WikiUrl = "https://wiki.guildwars.com/wiki/Securing_Champion%27s_Dawn" };
    public static readonly Quest TheApostate = new() { Id = 685, Name = "The Apostate", WikiUrl = "https://wiki.guildwars.com/wiki/The_Apostate" };
    //TODO: public static readonly Quest BreakingtheBroken = new() { Id = 686, Name = "Breaking the Broken", WikiUrl = "https://wiki.guildwars.com/wiki/Breaking_the_Broken" }; -- Commented out due to collision
    public static readonly Quest AFlickeringFlame = new() { Id = 687, Name = "A Flickering Flame", WikiUrl = "https://wiki.guildwars.com/wiki/A_Flickering_Flame" };
    public static readonly Quest DismembertheTitans = new() { Id = 688, Name = "Dismember the Titans", WikiUrl = "https://wiki.guildwars.com/wiki/Dismember_the_Titans" };
    public static readonly Quest CoverYourTracks = new() { Id = 689, Name = "Cover Your Tracks", WikiUrl = "https://wiki.guildwars.com/wiki/Cover_Your_Tracks" };
    public static readonly Quest DarkGateway = new() { Id = 690, Name = "Dark Gateway", WikiUrl = "https://wiki.guildwars.com/wiki/Dark_Gateway" };
    public static readonly Quest TheyOnlyComeOutatNight = new() { Id = 691, Name = "They Only Come Out at Night", WikiUrl = "https://wiki.guildwars.com/wiki/They_Only_Come_Out_at_Night" };
    public static readonly Quest InvasionFromWithin = new() { Id = 692, Name = "Invasion From Within", WikiUrl = "https://wiki.guildwars.com/wiki/Invasion_From_Within" };
    public static readonly Quest DrinkoftheGods = new() { Id = 693, Name = "Drink of the Gods", WikiUrl = "https://wiki.guildwars.com/wiki/Drink_of_the_Gods" };
    public static readonly Quest TheTroubledKeeper = new() { Id = 694, Name = "The Troubled Keeper", WikiUrl = "https://wiki.guildwars.com/wiki/The_Troubled_Keeper" };
    public static readonly Quest OpenSeason = new() { Id = 695, Name = "Open Season", WikiUrl = "https://wiki.guildwars.com/wiki/Open_Season" };
    public static readonly Quest GoodDemonHunting = new() { Id = 696, Name = "Good Demon Hunting", WikiUrl = "https://wiki.guildwars.com/wiki/Good_Demon_Hunting" };
    public static readonly Quest HoldingtheLine = new() { Id = 697, Name = "Holding the Line", WikiUrl = "https://wiki.guildwars.com/wiki/Holding_the_Line" };
    public static readonly Quest AFleshyOperation = new() { Id = 698, Name = "A Fleshy Operation", WikiUrl = "https://wiki.guildwars.com/wiki/A_Fleshy_Operation" };
    public static readonly Quest BadFortune = new() { Id = 699, Name = "Bad Fortune", WikiUrl = "https://wiki.guildwars.com/wiki/Bad_Fortune" };
    public static readonly Quest KnowThineEnemy = new() { Id = 700, Name = "Know Thine Enemy", WikiUrl = "https://wiki.guildwars.com/wiki/Know_Thine_Enemy" };
    public static readonly Quest UnchartedTerritory = new() { Id = 701, Name = "Uncharted Territory", WikiUrl = "https://wiki.guildwars.com/wiki/Uncharted_Territory" };
    public static readonly Quest KormirsCrusade = new() { Id = 702, Name = "Kormirs Crusade", WikiUrl = "https://wiki.guildwars.com/wiki/Kormir%27s_Crusade" };
    public static readonly Quest AllAloneintheDarkness = new() { Id = 703, Name = "All Alone in the Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/All_Alone_in_the_Darkness" };
    public static readonly Quest PassingtheLuck = new() { Id = 704, Name = "Passing the Luck", WikiUrl = "https://wiki.guildwars.com/wiki/Passing_the_Luck" };
    public static readonly Quest BlueprintoftheFall = new() { Id = 705, Name = "Blueprint of the Fall", WikiUrl = "https://wiki.guildwars.com/wiki/Blueprint_of_the_Fall" };
    public static readonly Quest EscapefromtheTorment = new() { Id = 706, Name = "Escape from the Torment", WikiUrl = "https://wiki.guildwars.com/wiki/Escape_from_the_Torment" };
    public static readonly Quest FadedMemory = new() { Id = 707, Name = "Faded Memory", WikiUrl = "https://wiki.guildwars.com/wiki/Faded_Memory" };
    public static readonly Quest AHistoryofViolence = new() { Id = 708, Name = "A History of Violence", WikiUrl = "https://wiki.guildwars.com/wiki/A_History_of_Violence" };
    public static readonly Quest QuiztheRecruits = new() { Id = 709, Name = "Quiz the Recruits", WikiUrl = "https://wiki.guildwars.com/wiki/Quiz_the_Recruits" };
    public static readonly Quest KossGoesRecruiting = new() { Id = 710, Name = "Koss Goes Recruiting", WikiUrl = "https://wiki.guildwars.com/wiki/Koss_Goes_Recruiting" };
    public static readonly Quest NeverFightAlone = new() { Id = 711, Name = "Never Fight Alone", WikiUrl = "https://wiki.guildwars.com/wiki/Never_Fight_Alone" };
    public static readonly Quest CommandTraining = new() { Id = 712, Name = "Command Training", WikiUrl = "https://wiki.guildwars.com/wiki/Command_Training" };
    public static readonly Quest GorensStuffPart2 = new() { Id = 713, Name = "Gorens Stuff Part 2", WikiUrl = "https://wiki.guildwars.com/wiki/Goren%27s_Stuff:_Part_2" };
    public static readonly Quest TheToysStory = new() { Id = 714, Name = "The Toys Story", WikiUrl = "https://wiki.guildwars.com/wiki/The_Toy%27s_Story" };
    public static readonly Quest RisingintheRanksMasterSergeant = new() { Id = 715, Name = "Rising in the Ranks Master Sergeant", WikiUrl = "https://wiki.guildwars.com/wiki/Rising_in_the_Ranks:_Master_Sergeant" };
    public static readonly Quest RisingintheRanksFirstSpear = new() { Id = 716, Name = "Rising in the Ranks First Spear", WikiUrl = "https://wiki.guildwars.com/wiki/Rising_in_the_Ranks:_First_Spear" };
    public static readonly Quest MiselatheMiddleChild = new() { Id = 717, Name = "Misela, the Middle Child", WikiUrl = "https://wiki.guildwars.com/wiki/Misela,_the_Middle_Child" };
    public static readonly Quest MututheOldestChild = new() { Id = 718, Name = "Mutu, the Oldest Child", WikiUrl = "https://wiki.guildwars.com/wiki/Mutu,_the_Oldest_Child" };
    public static readonly Quest JedurtheYoungestChild = new() { Id = 719, Name = "Jedur, the Youngest Child", WikiUrl = "https://wiki.guildwars.com/wiki/Jedur,_the_Youngest_Child" };
    public static readonly Quest NorgusNightfall = new() { Id = 720, Name = "Norgus Nightfall", WikiUrl = "https://wiki.guildwars.com/wiki/Norgu%27s_Nightfall" };
    public static readonly Quest TheRoleofaLifetime = new() { Id = 721, Name = "The Role of a Lifetime", WikiUrl = "https://wiki.guildwars.com/wiki/The_Role_of_a_Lifetime" };
    public static readonly Quest BrainsorBrawn = new() { Id = 722, Name = "Brains or Brawn", WikiUrl = "https://wiki.guildwars.com/wiki/Brains_or_Brawn" };
    public static readonly Quest SunspearsinKryta = new() { Id = 723, Name = "Sunspears in Kryta", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspears_in_Kryta" };
    public static readonly Quest SunspearsinCantha = new() { Id = 724, Name = "Sunspears in Cantha", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspears_in_Cantha" };
    public static readonly Quest TerrorinTyria = new() { Id = 725, Name = "Terror in Tyria", WikiUrl = "https://wiki.guildwars.com/wiki/Terror_in_Tyria" };
    public static readonly Quest PlagueinCantha = new() { Id = 726, Name = "Plague in Cantha", WikiUrl = "https://wiki.guildwars.com/wiki/Plague_in_Cantha" };
    public static readonly Quest TheCommandPost = new() { Id = 727, Name = "The Command Post", WikiUrl = "https://wiki.guildwars.com/wiki/The_Command_Post" };
    public static readonly Quest TheDejarinEstate = new() { Id = 728, Name = "The Dejarin Estate", WikiUrl = "https://wiki.guildwars.com/wiki/The_Dejarin_Estate" };
    public static readonly Quest GainNorgu = new() { Id = 729, Name = "Gain Norgu", WikiUrl = "https://wiki.guildwars.com/wiki/Gain_Norgu" };
    public static readonly Quest GainGoren = new() { Id = 730, Name = "Gain Goren", WikiUrl = "https://wiki.guildwars.com/wiki/Gain_Goren" };
    public static readonly Quest GainMargrid = new() { Id = 731, Name = "Gain Margrid", WikiUrl = "https://wiki.guildwars.com/wiki/Gain_Margrid" };
    public static readonly Quest GainMasterofWhispers = new() { Id = 732, Name = "Gain Master of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Gain_Master_of_Whispers" };
    public static readonly Quest GainJin = new() { Id = 733, Name = "Gain Jin", WikiUrl = "https://wiki.guildwars.com/wiki/Gain_Jin" };
    public static readonly Quest GainSousuke = new() { Id = 734, Name = "Gain Sousuke", WikiUrl = "https://wiki.guildwars.com/wiki/Gain_Sousuke" };
    public static readonly Quest GainOlias = new() { Id = 735, Name = "Gain Olias", WikiUrl = "https://wiki.guildwars.com/wiki/Gain_Olias" };
    public static readonly Quest GainZenmai = new() { Id = 736, Name = "Gain Zenmai", WikiUrl = "https://wiki.guildwars.com/wiki/Gain_Zenmai" };
    public static readonly Quest BattlePreparations = new() { Id = 737, Name = "Battle Preparations", WikiUrl = "https://wiki.guildwars.com/wiki/Battle_Preparations" };
    public static readonly Quest TheTimeisNigh1 = new() { Id = 738, Name = "The Time is Nigh", WikiUrl = "https://wiki.guildwars.com/wiki/The_Time_is_Nigh" };
    public static readonly Quest DrakesonthePlain = new() { Id = 739, Name = "Drakes on the Plain", WikiUrl = "https://wiki.guildwars.com/wiki/Drakes_on_the_Plain" };
    public static readonly Quest CapturingtheSignetofCapture = new() { Id = 740, Name = "Capturing the Signet of Capture", WikiUrl = "https://wiki.guildwars.com/wiki/Capturing_the_Signet_of_Capture" };
    public static readonly Quest ALandofHeroes = new() { Id = 741, Name = "A Land of Heroes", WikiUrl = "https://wiki.guildwars.com/wiki/A_Land_of_Heroes" };
    public static readonly Quest BreachingtheStygianVeil = new() { Id = 742, Name = "Breaching the Stygian Veil", WikiUrl = "https://wiki.guildwars.com/wiki/Breaching_the_Stygian_Veil" };
    public static readonly Quest FoundryBreakout = new() { Id = 743, Name = "Foundry Breakout", WikiUrl = "https://wiki.guildwars.com/wiki/Foundry_Breakout" };
    public static readonly Quest TheFoundryofFailedCreations = new() { Id = 744, Name = "The Foundry of Failed Creations (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Foundry_of_Failed_Creations_(quest)" };
    public static readonly Quest TheOddbodies = new() { Id = 745, Name = "The Oddbodies (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Oddbodies_(quest)" };
    public static readonly Quest TheLastPatrol = new() { Id = 746, Name = "The Last Patrol", WikiUrl = "https://wiki.guildwars.com/wiki/The_Last_Patrol" };
    public static readonly Quest TheMissingPatrol = new() { Id = 747, Name = "The Missing Patrol", WikiUrl = "https://wiki.guildwars.com/wiki/The_Missing_Patrol" };
    public static readonly Quest IntotheFire = new() { Id = 748, Name = "Into the Fire", WikiUrl = "https://wiki.guildwars.com/wiki/Into_the_Fire" };
    public static readonly Quest DeathbringerCompany = new() { Id = 749, Name = "Deathbringer Company", WikiUrl = "https://wiki.guildwars.com/wiki/Deathbringer_Company" };
    public static readonly Quest FindingaPurpose = new() { Id = 750, Name = "Finding a Purpose", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_a_Purpose" };
    public static readonly Quest TheCityofTorcqua = new() { Id = 751, Name = "The City of Torcqua", WikiUrl = "https://wiki.guildwars.com/wiki/The_City_of_Torc%27qua" };
    public static readonly Quest TheRiftsBetweenUs = new() { Id = 752, Name = "The Rifts Between Us", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rifts_Between_Us" };
    public static readonly Quest TotheRescue = new() { Id = 753, Name = "To the Rescue!", WikiUrl = "https://wiki.guildwars.com/wiki/To_the_Rescue!" };
    public static readonly Quest MallyxtheUnyielding = new() { Id = 754, Name = "Mallyx the Unyielding (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Mallyx_the_Unyielding_(quest)" };
    public static readonly Quest BroodWars = new() { Id = 755, Name = "Brood Wars", WikiUrl = "https://wiki.guildwars.com/wiki/Brood_Wars" };
    public static readonly Quest HowtheGrentchesStoleWintersday = new() { Id = 756, Name = "How the Grentches Stole Wintersday", WikiUrl = "https://wiki.guildwars.com/wiki/How_the_Grentches_Stole_Wintersday" };
    public static readonly Quest SpreadingtheWintersdaySpirit = new() { Id = 757, Name = "Spreading the Wintersday Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Spreading_the_Wintersday_Spirit" };
    public static readonly Quest WhiteMist = new() { Id = 758, Name = "White Mist", WikiUrl = "https://wiki.guildwars.com/wiki/White_Mist" };
    public static readonly Quest TheGiftofGiving = new() { Id = 759, Name = "The Gift of Giving", WikiUrl = "https://wiki.guildwars.com/wiki/The_Gift_of_Giving" };
    public static readonly Quest SavetheReindeer = new() { Id = 760, Name = "Save the Reindeer", WikiUrl = "https://wiki.guildwars.com/wiki/Save_the_Reindeer" };
    public static readonly Quest FindtheStolenPresents = new() { Id = 761, Name = "Find the Stolen Presents", WikiUrl = "https://wiki.guildwars.com/wiki/Find_the_Stolen_Presents" };
    public static readonly Quest AVeryGrentchieWintersday = new() { Id = 762, Name = "A Very Grentchie Wintersday", WikiUrl = "https://wiki.guildwars.com/wiki/A_Very_Grentchie_Wintersday" };
    public static readonly Quest TheGreatestSnowmanEverMade = new() { Id = 763, Name = "The Greatest Snowman Ever Made", WikiUrl = "https://wiki.guildwars.com/wiki/The_Greatest_Snowman_Ever_Made" };
    public static readonly Quest YoureaMeanOneMrGrenth = new() { Id = 764, Name = "Youre a Mean One, Mr. Grenth", WikiUrl = "https://wiki.guildwars.com/wiki/You%27re_a_Mean_One,_Mr._Grenth" };
    public static readonly Quest InGrenthsDefense = new() { Id = 765, Name = "In Grenths Defense", WikiUrl = "https://wiki.guildwars.com/wiki/In_Grenth%27s_Defense" };
    public static readonly Quest ArmyWife = new() { Id = 766, Name = "Army Wife", WikiUrl = "https://wiki.guildwars.com/wiki/Army_Wife" };
    public static readonly Quest ImpressingtheGirl = new() { Id = 767, Name = "Impressing the Girl", WikiUrl = "https://wiki.guildwars.com/wiki/Impressing_the_Girl" };
    public static readonly Quest TossingtheBouquet = new() { Id = 768, Name = "Tossing the Bouquet", WikiUrl = "https://wiki.guildwars.com/wiki/Tossing_the_Bouquet" };
    public static readonly Quest MakingFriends = new() { Id = 769, Name = "Making Friends", WikiUrl = "https://wiki.guildwars.com/wiki/Making_Friends" };
    public static readonly Quest MissingSuitor = new() { Id = 770, Name = "Missing Suitor", WikiUrl = "https://wiki.guildwars.com/wiki/Missing_Suitor" };
    public static readonly Quest AvadontheRun = new() { Id = 771, Name = "Avad on the Run", WikiUrl = "https://wiki.guildwars.com/wiki/Avad_on_the_Run" };
    public static readonly Quest TheEternalDebate = new() { Id = 772, Name = "The Eternal Debate", WikiUrl = "https://wiki.guildwars.com/wiki/The_Eternal_Debate" };
    public static readonly Quest TheContest = new() { Id = 773, Name = "The Contest", WikiUrl = "https://wiki.guildwars.com/wiki/The_Contest" };
    public static readonly Quest Consolation = new() { Id = 774, Name = "Consolation", WikiUrl = "https://wiki.guildwars.com/wiki/Consolation" };
    public static readonly Quest ABurningDesire = new() { Id = 775, Name = "A Burning Desire", WikiUrl = "https://wiki.guildwars.com/wiki/A_Burning_Desire" };
    public static readonly Quest TheBigBang = new() { Id = 776, Name = "The Big Bang", WikiUrl = "https://wiki.guildwars.com/wiki/The_Big_Bang" };
    public static readonly Quest DouseYourEnthusiasm = new() { Id = 777, Name = "Douse Your Enthusiasm", WikiUrl = "https://wiki.guildwars.com/wiki/Douse_Your_Enthusiasm" };
    public static readonly Quest HopelessRomantic = new() { Id = 778, Name = "Hopeless Romantic", WikiUrl = "https://wiki.guildwars.com/wiki/Hopeless_Romantic" };
    public static readonly Quest FireintheSky = new() { Id = 779, Name = "Fire in the Sky", WikiUrl = "https://wiki.guildwars.com/wiki/Fire_in_the_Sky" };
    public static readonly Quest TheKnightsWhoSayNian = new() { Id = 780, Name = "The Knights Who Say Nian", WikiUrl = "https://wiki.guildwars.com/wiki/The_Knights_Who_Say_Nian" };
    public static readonly Quest JustMyLuck = new() { Id = 781, Name = "Just My Luck", WikiUrl = "https://wiki.guildwars.com/wiki/Just_My_Luck" };
    public static readonly Quest AllforOneandOneforJustice = new() { Id = 782, Name = "All for One and One for Justice", WikiUrl = "https://wiki.guildwars.com/wiki/All_for_One_and_One_for_Justice" };
    public static readonly Quest ChasingZenmai = new() { Id = 783, Name = "Chasing Zenmai", WikiUrl = "https://wiki.guildwars.com/wiki/Chasing_Zenmai" };
    public static readonly Quest OutofKourna = new() { Id = 784, Name = "Out of Kourna", WikiUrl = "https://wiki.guildwars.com/wiki/Out_of_Kourna" };
    public static readonly Quest CountingtheFallen1 = new() { Id = 785, Name = "Counting the Fallen", WikiUrl = "https://wiki.guildwars.com/wiki/Counting_the_Fallen" };
    public static readonly Quest CommissioningaMemorial = new() { Id = 786, Name = "Commissioning a Memorial", WikiUrl = "https://wiki.guildwars.com/wiki/Commissioning_a_Memorial" };
    public static readonly Quest AncientHistory = new() { Id = 787, Name = "Ancient History", WikiUrl = "https://wiki.guildwars.com/wiki/Ancient_History" };
    public static readonly Quest FindingaPurpose1 = new() { Id = 788, Name = "Finding a Purpose", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_a_Purpose" };
    public static readonly Quest MoneyBackGuarantee = new() { Id = 789, Name = "Money Back Guarantee", WikiUrl = "https://wiki.guildwars.com/wiki/Money_Back_Guarantee" };
    public static readonly Quest AgainsttheCharr = new() { Id = 790, Name = "Against the Charr", WikiUrl = "https://wiki.guildwars.com/wiki/Against_the_Charr" };
    public static readonly Quest WarbandofBrothers = new() { Id = 791, Name = "Warband of Brothers", WikiUrl = "https://wiki.guildwars.com/wiki/Warband_of_Brothers" };
    public static readonly Quest SearchfortheEbonVanguard = new() { Id = 792, Name = "Search for the Ebon Vanguard", WikiUrl = "https://wiki.guildwars.com/wiki/Search_for_the_Ebon_Vanguard" };
    public static readonly Quest TheMissingVanguard = new() { Id = 793, Name = "The Missing Vanguard", WikiUrl = "https://wiki.guildwars.com/wiki/The_Missing_Vanguard" };
    public static readonly Quest ScrambledReinforcements = new() { Id = 794, Name = "Scrambled Reinforcements", WikiUrl = "https://wiki.guildwars.com/wiki/Scrambled_Reinforcements" };
    public static readonly Quest TheRampagingYetis = new() { Id = 795, Name = "The Rampaging Yetis", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rampaging_Yetis" };
    public static readonly Quest TheShrineofMaat = new() { Id = 796, Name = "The Shrine of Maat", WikiUrl = "https://wiki.guildwars.com/wiki/The_Shrine_of_Maat" };
    public static readonly Quest AStrangeRequest = new() { Id = 797, Name = "A Strange Request", WikiUrl = "https://wiki.guildwars.com/wiki/A_Strange_Request" };
    public static readonly Quest DarknessatKaitan = new() { Id = 798, Name = "Darkness at Kaitan", WikiUrl = "https://wiki.guildwars.com/wiki/Darkness_at_Kaitan" };
    public static readonly Quest FlamesoftheBearSpirit = new() { Id = 800, Name = "Flames of the Bear Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Flames_of_the_Bear_Spirit" };
    public static readonly Quest TempleoftheDamned = new() { Id = 801, Name = "Temple of the Damned", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_of_the_Damned" };
    public static readonly Quest GiriffsWar = new() { Id = 802, Name = "Giriffs War", WikiUrl = "https://wiki.guildwars.com/wiki/Giriff%27s_War" };
    public static readonly Quest VeiledThreat = new() { Id = 803, Name = "Veiled Threat", WikiUrl = "https://wiki.guildwars.com/wiki/Veiled_Threat" };
    public static readonly Quest LostSouls = new() { Id = 804, Name = "Lost Souls", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Souls" };
    public static readonly Quest KathandraxsCrusher = new() { Id = 805, Name = "Kathandraxs Crusher", WikiUrl = "https://wiki.guildwars.com/wiki/Kathandrax%27s_Crusher" };
    public static readonly Quest CurseoftheNornbear = new() { Id = 806, Name = "Curse of the Nornbear", WikiUrl = "https://wiki.guildwars.com/wiki/Curse_of_the_Nornbear" };
    public static readonly Quest TheElusiveGolemancer = new() { Id = 807, Name = "The Elusive Golemancer", WikiUrl = "https://wiki.guildwars.com/wiki/The_Elusive_Golemancer" };
    public static readonly Quest TrackingtheNornbear = new() { Id = 808, Name = "Tracking the Nornbear", WikiUrl = "https://wiki.guildwars.com/wiki/Tracking_the_Nornbear" };
    public static readonly Quest TheAnvilofDragrimmar = new() { Id = 809, Name = "The Anvil of Dragrimmar", WikiUrl = "https://wiki.guildwars.com/wiki/The_Anvil_of_Dragrimmar" };
    public static readonly Quest ColdVengeance = new() { Id = 810, Name = "Cold Vengeance", WikiUrl = "https://wiki.guildwars.com/wiki/Cold_Vengeance" };
    public static readonly Quest TheMisanthropicJotunPrinciple = new() { Id = 811, Name = "The Misanthropic Jotun Principle", WikiUrl = "https://wiki.guildwars.com/wiki/The_Misanthropic_Jotun_Principle" };
    public static readonly Quest LabSpace = new() { Id = 812, Name = "Lab Space", WikiUrl = "https://wiki.guildwars.com/wiki/Lab_Space" };
    public static readonly Quest AGateTooFar = new() { Id = 813, Name = "A Gate Too Far", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gate_Too_Far" };
    public static readonly Quest VisionoftheRavenSpirit = new() { Id = 814, Name = "Vision of the Raven Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Vision_of_the_Raven_Spirit" };
    public static readonly Quest GeniusOperatedLivingEnchantedManifestation = new() { Id = 815, Name = "Genius Operated Living Enchanted Manifestation", WikiUrl = "https://wiki.guildwars.com/wiki/Genius_Operated_Living_Enchanted_Manifestation" };
    public static readonly Quest DestructionsDepths = new() { Id = 816, Name = "Destructions Depths", WikiUrl = "https://wiki.guildwars.com/wiki/Destruction%27s_Depths" };
    public static readonly Quest HeartoftheShiverpeaks = new() { Id = 817, Name = "Heart of the Shiverpeaks (quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_of_the_Shiverpeaks_(quest)" };
    public static readonly Quest FindingtheBloodstone = new() { Id = 818, Name = "Finding the Bloodstone", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_the_Bloodstone" };
    public static readonly Quest FindingGadd = new() { Id = 819, Name = "Finding Gadd", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_Gadd" };
    public static readonly Quest ALittleHelp = new() { Id = 820, Name = "A Little Help", WikiUrl = "https://wiki.guildwars.com/wiki/A_Little_Help" };
    public static readonly Quest IFeeltheEarthMoveUnderCanthasFeet = new() { Id = 821, Name = "I Feel the Earth Move Under Canthas Feet", WikiUrl = "https://wiki.guildwars.com/wiki/I_Feel_the_Earth_Move_Under_Cantha%27s_Feet" };
    public static readonly Quest HoleofIstan = new() { Id = 822, Name = "Hole of Istan", WikiUrl = "https://wiki.guildwars.com/wiki/Hole_of_Istan" };
    public static readonly Quest WhatLiesBeneath = new() { Id = 823, Name = "What Lies Beneath", WikiUrl = "https://wiki.guildwars.com/wiki/What_Lies_Beneath" };
    public static readonly Quest TheBeginningoftheEnd = new() { Id = 824, Name = "The Beginning of the End", WikiUrl = "https://wiki.guildwars.com/wiki/The_Beginning_of_the_End" };
    public static readonly Quest TekkssWar = new() { Id = 825, Name = "Tekkss War", WikiUrl = "https://wiki.guildwars.com/wiki/Tekks%27s_War" };
    public static readonly Quest WorthyDeedsDoneDirtCheap = new() { Id = 826, Name = "Worthy Deeds (Done Dirt Cheap)", WikiUrl = "https://wiki.guildwars.com/wiki/Worthy_Deeds_(Done_Dirt_Cheap)" };
    public static readonly Quest LittleWorkshopofHorrors = new() { Id = 827, Name = "Little Workshop of Horrors", WikiUrl = "https://wiki.guildwars.com/wiki/Little_Workshop_of_Horrors" };
    public static readonly Quest DredgingtheDepths = new() { Id = 828, Name = "Dredging the Depths", WikiUrl = "https://wiki.guildwars.com/wiki/Dredging_the_Depths" };
    public static readonly Quest WatchitJiggle = new() { Id = 829, Name = "Watch it Jiggle", WikiUrl = "https://wiki.guildwars.com/wiki/Watch_it_Jiggle" };
    public static readonly Quest AnythingYouCanDo = new() { Id = 830, Name = "Anything You Can Do", WikiUrl = "https://wiki.guildwars.com/wiki/Anything_You_Can_Do" };
    public static readonly Quest KraksCavalry = new() { Id = 831, Name = "Kraks Cavalry", WikiUrl = "https://wiki.guildwars.com/wiki/Krak%27s_Cavalry" };
    public static readonly Quest CrystalMethod = new() { Id = 832, Name = "Crystal Method", WikiUrl = "https://wiki.guildwars.com/wiki/Crystal_Method" };
    public static readonly Quest TheBladesEssence = new() { Id = 833, Name = "The Blades Essence", WikiUrl = "https://wiki.guildwars.com/wiki/The_Blade%27s_Essence" };
    public static readonly Quest TheArrowsPoint = new() { Id = 834, Name = "The Arrows Point", WikiUrl = "https://wiki.guildwars.com/wiki/The_Arrow%27s_Point" };
    public static readonly Quest DefendingtheBreach = new() { Id = 835, Name = "Defending the Breach", WikiUrl = "https://wiki.guildwars.com/wiki/Defending_the_Breach" };
    public static readonly Quest ColdasIce = new() { Id = 836, Name = "Cold as Ice", WikiUrl = "https://wiki.guildwars.com/wiki/Cold_as_Ice" };
    public static readonly Quest IntheServiceofRevenge = new() { Id = 837, Name = "In the Service of Revenge", WikiUrl = "https://wiki.guildwars.com/wiki/In_the_Service_of_Revenge" };
    public static readonly Quest Truthseeker = new() { Id = 838, Name = "Truthseeker", WikiUrl = "https://wiki.guildwars.com/wiki/Truthseeker" };
    public static readonly Quest AHuntersPride = new() { Id = 839, Name = "A Hunters Pride", WikiUrl = "https://wiki.guildwars.com/wiki/A_Hunter%27s_Pride" };
    public static readonly Quest ShadowsintheNight = new() { Id = 840, Name = "Shadows in the Night", WikiUrl = "https://wiki.guildwars.com/wiki/Shadows_in_the_Night" };
    public static readonly Quest LeaderofthePack = new() { Id = 841, Name = "Leader of the Pack", WikiUrl = "https://wiki.guildwars.com/wiki/Leader_of_the_Pack" };
    public static readonly Quest Round1Fight = new() { Id = 842, Name = "Round 1 Fight!", WikiUrl = "https://wiki.guildwars.com/wiki/Round_1:_Fight!" };
    public static readonly Quest Round2Fight = new() { Id = 843, Name = "Round 2 Fight!", WikiUrl = "https://wiki.guildwars.com/wiki/Round_2:_Fight!" };
    public static readonly Quest Round3Fight = new() { Id = 844, Name = "Round 3 Fight!", WikiUrl = "https://wiki.guildwars.com/wiki/Round_3:_Fight!" };
    public static readonly Quest Round4Fight = new() { Id = 845, Name = "Round 4 Fight!", WikiUrl = "https://wiki.guildwars.com/wiki/Round_4:_Fight!" };
    public static readonly Quest Round5Fight = new() { Id = 846, Name = "Round 5 Fight!", WikiUrl = "https://wiki.guildwars.com/wiki/Round_5:_Fight!" };
    public static readonly Quest FinalRoundFight = new() { Id = 847, Name = "Final Round Fight!", WikiUrl = "https://wiki.guildwars.com/wiki/Final_Round:_Fight!" };
    public static readonly Quest HeroTutorial = new() { Id = 848, Name = "Hero Tutorial", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_Tutorial" };
    public static readonly Quest BearClubforWomen = new() { Id = 849, Name = "Bear Club for Women", WikiUrl = "https://wiki.guildwars.com/wiki/Bear_Club_for_Women" };
    public static readonly Quest TheGreatNornAlemoot = new() { Id = 850, Name = "The Great Norn Alemoot", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Norn_Alemoot" };
    public static readonly Quest TheGreatNornAlemoot1 = new() { Id = 851, Name = "The Great Norn Alemoot", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Norn_Alemoot" };
    public static readonly Quest PrenuptialDisagreementFemale = new() { Id = 852, Name = "Prenuptial Disagreement (female)", WikiUrl = "https://wiki.guildwars.com/wiki/Prenuptial_Disagreement_(female)" };
    public static readonly Quest PrenuptialDisagreementMale = new() { Id = 853, Name = "Prenuptial Disagreement (male)", WikiUrl = "https://wiki.guildwars.com/wiki/Prenuptial_Disagreement_(male)" };
    public static readonly Quest CharrInvaders = new() { Id = 854, Name = "Charr Invaders", WikiUrl = "https://wiki.guildwars.com/wiki/Charr_Invaders" };
    public static readonly Quest BearClubforMen = new() { Id = 855, Name = "Bear Club for Men", WikiUrl = "https://wiki.guildwars.com/wiki/Bear_Club_for_Men" };
    public static readonly Quest KilroyStonekinsPunchOutExtravaganza = new() { Id = 856, Name = "Kilroy Stonekins Punch-Out Extravaganza!", WikiUrl = "https://wiki.guildwars.com/wiki/Kilroy_Stonekin%27s_Punch-Out_Extravaganza!" };
    public static readonly Quest TheThrowdowninaNornTown = new() { Id = 857, Name = "The Throwdown in a Norn Town", WikiUrl = "https://wiki.guildwars.com/wiki/The_Throwdown_in_a_Norn_Town" };
    public static readonly Quest PunchtheClown = new() { Id = 858, Name = "Punch the Clown", WikiUrl = "https://wiki.guildwars.com/wiki/Punch_the_Clown" };
    public static readonly Quest DestructiveResearch = new() { Id = 859, Name = "Destructive Research", WikiUrl = "https://wiki.guildwars.com/wiki/Destructive_Research" };
    public static readonly Quest TheDestroyerChallenge = new() { Id = 860, Name = "The Destroyer Challenge", WikiUrl = "https://wiki.guildwars.com/wiki/The_Destroyer_Challenge" };
    public static readonly Quest FallingOut = new() { Id = 861, Name = "Falling Out", WikiUrl = "https://wiki.guildwars.com/wiki/Falling_Out" };
    public static readonly Quest ForgottenRelics = new() { Id = 862, Name = "Forgotten Relics", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Relics" };
    public static readonly Quest TheSmellofTitanintheMorning = new() { Id = 864, Name = "The Smell of Titan in the Morning", WikiUrl = "https://wiki.guildwars.com/wiki/The_Smell_of_Titan_in_the_Morning" };
    public static readonly Quest BeVeryVeryQuiet = new() { Id = 865, Name = "Be Very, Very Quiet...", WikiUrl = "https://wiki.guildwars.com/wiki/Be_Very,_Very_Quiet..." };
    public static readonly Quest PlanA = new() { Id = 866, Name = "Plan A", WikiUrl = "https://wiki.guildwars.com/wiki/Plan_A" };
    public static readonly Quest FinalRoundChampionshipEditionFight = new() { Id = 867, Name = "Final Round, Championship Edition Fight!", WikiUrl = "https://wiki.guildwars.com/wiki/Final_Round,_Championship_Edition:_Fight!" };
    public static readonly Quest ManoaNorno = new() { Id = 868, Name = "Mano a Norn-o", WikiUrl = "https://wiki.guildwars.com/wiki/Mano_a_Norn-o" };
    public static readonly Quest ATimeforHeroes = new() { Id = 869, Name = "A Time for Heroes", WikiUrl = "https://wiki.guildwars.com/wiki/A_Time_for_Heroes" };
    public static readonly Quest TheAssassinsRevenge = new() { Id = 870, Name = "The Assassins Revenge", WikiUrl = "https://wiki.guildwars.com/wiki/The_Assassin%27s_Revenge" };
    public static readonly Quest TheImplodingPast = new() { Id = 871, Name = "The Imploding Past", WikiUrl = "https://wiki.guildwars.com/wiki/The_Imploding_Past" };
    public static readonly Quest FailuretoCommunicate = new() { Id = 872, Name = "Failure to Communicate", WikiUrl = "https://wiki.guildwars.com/wiki/Failure_to_Communicate" };
    public static readonly Quest ServiceInDefenseoftheEye = new() { Id = 873, Name = "Service In Defense of the Eye", WikiUrl = "https://wiki.guildwars.com/wiki/Service:_In_Defense_of_the_Eye" };
    public static readonly Quest ServicePracticeDummy = new() { Id = 874, Name = "Service Practice, Dummy", WikiUrl = "https://wiki.guildwars.com/wiki/Service:_Practice,_Dummy" };
    public static readonly Quest PolymockDefeatPlurgg = new() { Id = 875, Name = "Polymock Defeat Plurgg", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock:_Defeat_Plurgg" };
    public static readonly Quest PolymockDefeatFonk = new() { Id = 876, Name = "Polymock Defeat Fonk", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock:_Defeat_Fonk" };
    public static readonly Quest PolymockDefeatDuneTeardrinker = new() { Id = 877, Name = "Polymock Defeat Dune Teardrinker", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock:_Defeat_Dune_Teardrinker" };
    public static readonly Quest PolymockDefeatGrulhammerSilverfist = new() { Id = 878, Name = "Polymock Defeat Grulhammer Silverfist", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock:_Defeat_Grulhammer_Silverfist" };
    public static readonly Quest PolymockDefeatNecromancerVolumandus = new() { Id = 879, Name = "Polymock Defeat Necromancer Volumandus", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock:_Defeat_Necromancer_Volumandus" };
    public static readonly Quest PolymockDefeatMasterHoff = new() { Id = 880, Name = "Polymock Defeat Master Hoff", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock:_Defeat_Master_Hoff" };
    public static readonly Quest PolymockDefeatBlarp = new() { Id = 881, Name = "Polymock Defeat Blarp", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock:_Defeat_Blarp" };
    public static readonly Quest PolymockDefeatYulma = new() { Id = 882, Name = "Polymock Defeat Yulma", WikiUrl = "https://wiki.guildwars.com/wiki/Polymock:_Defeat_Yulma" };
    public static readonly Quest SingleUglyGrawlSeeksSameforMindlessDestructioninAscalon = new() { Id = 883, Name = "Single Ugly Grawl Seeks Same for Mindless Destruction in Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Single_Ugly_Grawl_Seeks_Same_for_Mindless_Destruction_in_Ascalon" };
    public static readonly Quest TheHuntingoftheCharr = new() { Id = 884, Name = "The Hunting of the Charr", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hunting_of_the_Charr" };
    public static readonly Quest Frogstomp = new() { Id = 885, Name = "Frogstomp", WikiUrl = "https://wiki.guildwars.com/wiki/Frogstomp" };
    public static readonly Quest GivePeaceaChance = new() { Id = 886, Name = "Give Peace a Chance", WikiUrl = "https://wiki.guildwars.com/wiki/Give_Peace_a_Chance" };
    public static readonly Quest TheCipherofBalthazar = new() { Id = 887, Name = "The Cipher of Balthazar", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cipher_of_Balthazar" };
    public static readonly Quest TheCipherofDwayna = new() { Id = 888, Name = "The Cipher of Dwayna", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cipher_of_Dwayna" };
    public static readonly Quest TheCipherofGrenth = new() { Id = 889, Name = "The Cipher of Grenth", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cipher_of_Grenth" };
    public static readonly Quest TheCipherofKormir = new() { Id = 890, Name = "The Cipher of Kormir", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cipher_of_Kormir" };
    public static readonly Quest TheCipherofLyssa = new() { Id = 891, Name = "The Cipher of Lyssa", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cipher_of_Lyssa" };
    public static readonly Quest TheCipherofMelandru = new() { Id = 892, Name = "The Cipher of Melandru", WikiUrl = "https://wiki.guildwars.com/wiki/The_Cipher_of_Melandru" };
    public static readonly Quest ThePathtoRevelations = new() { Id = 893, Name = "The Path to Revelations", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Revelations" };
    public static readonly Quest TheBigUnfriendlyJotun = new() { Id = 894, Name = "The Big Unfriendly Jotun", WikiUrl = "https://wiki.guildwars.com/wiki/The_Big_Unfriendly_Jotun" };
    public static readonly Quest ForbiddenFruit = new() { Id = 895, Name = "Forbidden Fruit", WikiUrl = "https://wiki.guildwars.com/wiki/Forbidden_Fruit" };
    public static readonly Quest OBraveNewWorld = new() { Id = 897, Name = "O Brave New World", WikiUrl = "https://wiki.guildwars.com/wiki/O_Brave_New_World" };
    public static readonly Quest LostTreasureofKingHundar = new() { Id = 898, Name = "Lost Treasure of King Hundar", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Treasure_of_King_Hundar" };
    public static readonly Quest DeeprunnersMap = new() { Id = 899, Name = "Deeprunners Map", WikiUrl = "https://wiki.guildwars.com/wiki/Deeprunner%27s_Map" };
    public static readonly Quest AttackonJalissCamp = new() { Id = 900, Name = "Attack on Jaliss Camp", WikiUrl = "https://wiki.guildwars.com/wiki/Attack_on_Jalis%27s_Camp" };
    public static readonly Quest TheAsuraTrap = new() { Id = 901, Name = "The Asura Trap", WikiUrl = "https://wiki.guildwars.com/wiki/The_Asura_Trap" };
    public static readonly Quest MothstoaFlame = new() { Id = 902, Name = "Moths to a Flame", WikiUrl = "https://wiki.guildwars.com/wiki/Moths_to_a_Flame" };
    public static readonly Quest InsidiousRemnants = new() { Id = 903, Name = "Insidious Remnants", WikiUrl = "https://wiki.guildwars.com/wiki/Insidious_Remnants" };
    public static readonly Quest TurningthePage = new() { Id = 904, Name = "Turning the Page", WikiUrl = "https://wiki.guildwars.com/wiki/Turning_the_Page" };
    public static readonly Quest NorthernAllies = new() { Id = 905, Name = "Northern Allies", WikiUrl = "https://wiki.guildwars.com/wiki/Northern_Allies" };
    public static readonly Quest AssaultontheStronghold = new() { Id = 906, Name = "Assault on the Stronghold", WikiUrl = "https://wiki.guildwars.com/wiki/Assault_on_the_Stronghold" };
    public static readonly Quest BloodWashesBlood = new() { Id = 907, Name = "Blood Washes Blood", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_Washes_Blood" };
    public static readonly Quest TheDawnofRebellion = new() { Id = 908, Name = "The Dawn of Rebellion", WikiUrl = "https://wiki.guildwars.com/wiki/The_Dawn_of_Rebellion" };
    public static readonly Quest WhatMustBeDone = new() { Id = 909, Name = "What Must Be Done", WikiUrl = "https://wiki.guildwars.com/wiki/What_Must_Be_Done" };
    public static readonly Quest Nornhood = new() { Id = 910, Name = "Nornhood", WikiUrl = "https://wiki.guildwars.com/wiki/Nornhood" };
    public static readonly Quest TheJusticiarsEnd = new() { Id = 911, Name = "The Justiciars End", WikiUrl = "https://wiki.guildwars.com/wiki/The_Justiciar%27s_End" };
    public static readonly Quest Haunted = new() { Id = 912, Name = "Haunted", WikiUrl = "https://wiki.guildwars.com/wiki/Haunted" };
    public static readonly Quest AgainsttheDestroyers = new() { Id = 913, Name = "Against the Destroyers", WikiUrl = "https://wiki.guildwars.com/wiki/Against_the_Destroyers" };
    public static readonly Quest FireandPain = new() { Id = 914, Name = "Fire and Pain", WikiUrl = "https://wiki.guildwars.com/wiki/Fire_and_Pain" };
    public static readonly Quest TheKnowledgeableAsura = new() { Id = 915, Name = "The Knowledgeable Asura", WikiUrl = "https://wiki.guildwars.com/wiki/The_Knowledgeable_Asura" };
    public static readonly Quest ThenandNowHereandThere = new() { Id = 916, Name = "Then and Now, Here and There", WikiUrl = "https://wiki.guildwars.com/wiki/Then_and_Now,_Here_and_There" };
    public static readonly Quest TheLastHierophant = new() { Id = 917, Name = "The Last Hierophant", WikiUrl = "https://wiki.guildwars.com/wiki/The_Last_Hierophant" };
    public static readonly Quest KilroysPunchoutTournament = new() { Id = 918, Name = "Kilroys Punchout Tournament", WikiUrl = "https://wiki.guildwars.com/wiki/Kilroy%27s_Punchout_Tournament" };
    public static readonly Quest SpecialOpsDragonsGullet = new() { Id = 919, Name = "Special Ops Dragons Gullet", WikiUrl = "https://wiki.guildwars.com/wiki/Special_Ops:_Dragon%27s_Gullet" };
    public static readonly Quest SpecialOpsFlameTempleCorridor = new() { Id = 920, Name = "Special Ops Flame Temple Corridor", WikiUrl = "https://wiki.guildwars.com/wiki/Special_Ops:_Flame_Temple_Corridor" };
    public static readonly Quest SpecialOpsGrendichCourthouse = new() { Id = 921, Name = "Special Ops Grendich Courthouse", WikiUrl = "https://wiki.guildwars.com/wiki/Special_Ops:_Grendich_Courthouse" };
    public static readonly Quest TheTenguAccords = new() { Id = 922, Name = "The Tengu Accords", WikiUrl = "https://wiki.guildwars.com/wiki/The_Tengu_Accords" };
    public static readonly Quest TheBattleofJahai = new() { Id = 923, Name = "The Battle of Jahai", WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_of_Jahai" };
    public static readonly Quest TheFlightNorth = new() { Id = 924, Name = "The Flight North", WikiUrl = "https://wiki.guildwars.com/wiki/The_Flight_North" };
    public static readonly Quest TheRiseoftheWhiteMantle = new() { Id = 925, Name = "The Rise of the White Mantle", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rise_of_the_White_Mantle" };
    public static readonly Quest StraighttotheHeart = new() { Id = 926, Name = "Straight to the Heart", WikiUrl = "https://wiki.guildwars.com/wiki/Straight_to_the_Heart" };
    public static readonly Quest TheStrengthofSnow = new() { Id = 927, Name = "The Strength of Snow", WikiUrl = "https://wiki.guildwars.com/wiki/The_Strength_of_Snow" };
    public static readonly Quest DeactivatingPOX = new() { Id = 928, Name = "Deactivating P.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/Deactivating_P.O.X." };
    public static readonly Quest DeactivatingNOX = new() { Id = 929, Name = "Deactivating N.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/Deactivating_N.O.X." };
    public static readonly Quest DeactivatingROX = new() { Id = 930, Name = "Deactivating R.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/Deactivating_R.O.X." };
    public static readonly Quest ZinnsTask = new() { Id = 931, Name = "Zinns Task", WikiUrl = "https://wiki.guildwars.com/wiki/Zinn%27s_Task" };
    public static readonly Quest TheThreeWiseNorn = new() { Id = 932, Name = "The Three Wise Norn", WikiUrl = "https://wiki.guildwars.com/wiki/The_Three_Wise_Norn" };
    public static readonly Quest CharrbroiledPlans = new() { Id = 933, Name = "Charr-broiled Plans", WikiUrl = "https://wiki.guildwars.com/wiki/Charr-broiled_Plans" };
    public static readonly Quest SnowballDominance = new() { Id = 934, Name = "Snowball Dominance", WikiUrl = "https://wiki.guildwars.com/wiki/Snowball_Dominance" };
    public static readonly Quest WintersdayCheer = new() { Id = 935, Name = "Wintersday Cheer", WikiUrl = "https://wiki.guildwars.com/wiki/Wintersday_Cheer" };
    public static readonly Quest TheGreatNorthernWallZaishenQuest = new() { Id = 936, Name = "The Great Northern Wall (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Northern_Wall_(Zaishen_quest)" };
    public static readonly Quest FortRanikZaishenQuest = new() { Id = 937, Name = "Fort Ranik (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Ranik_(Zaishen_quest)" };
    public static readonly Quest RuinsofSurmiaZaishenQuest = new() { Id = 938, Name = "Ruins of Surmia (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Ruins_of_Surmia_(Zaishen_quest)" };
    public static readonly Quest NolaniAcademyZaishenQuest = new() { Id = 939, Name = "Nolani Academy (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Nolani_Academy_(Zaishen_quest)" };
    public static readonly Quest BorlisPassZaishenQuest = new() { Id = 940, Name = "Borlis Pass (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Borlis_Pass_(Zaishen_quest)" };
    public static readonly Quest TheFrostGateZaishenQuest = new() { Id = 941, Name = "The Frost Gate (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Frost_Gate_(Zaishen_quest)" };
    public static readonly Quest GatesofKrytaZaishenQuest = new() { Id = 942, Name = "Gates of Kryta (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Gates_of_Kryta_(Zaishen_quest)" };
    public static readonly Quest DAlessioSeaboardZaishenQuest = new() { Id = 943, Name = "DAlessio Seaboard (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/D%27Alessio_Seaboard_(Zaishen_quest)" };
    public static readonly Quest DivinityCoastZaishenQuest = new() { Id = 944, Name = "Divinity Coast (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Divinity_Coast_(Zaishen_quest)" };
    public static readonly Quest TheWildsZaishenQuest = new() { Id = 945, Name = "The Wilds (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Wilds_(Zaishen_quest)" };
    public static readonly Quest BloodstoneFenZaishenQuest = new() { Id = 946, Name = "Bloodstone Fen (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Bloodstone_Fen_(Zaishen_quest)" };
    public static readonly Quest AuroraGladeZaishenQuest = new() { Id = 947, Name = "Aurora Glade (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Aurora_Glade_(Zaishen_quest)" };
    public static readonly Quest RiversideProvinceZaishenQuest = new() { Id = 948, Name = "Riverside Province (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Riverside_Province_(Zaishen_quest)" };
    public static readonly Quest SanctumCayZaishenQuest = new() { Id = 949, Name = "Sanctum Cay (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Sanctum_Cay_(Zaishen_quest)" };
    public static readonly Quest DunesofDespairZaishenQuest = new() { Id = 950, Name = "Dunes of Despair (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Dunes_of_Despair_(Zaishen_quest)" };
    public static readonly Quest ThirstyRiverZaishenQuest = new() { Id = 951, Name = "Thirsty River (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Thirsty_River_(Zaishen_quest)" };
    public static readonly Quest ElonaReachZaishenQuest = new() { Id = 952, Name = "Elona Reach (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Elona_Reach_(Zaishen_quest)" };
    public static readonly Quest AuguryRockZaishenQuest = new() { Id = 953, Name = "Augury Rock (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Augury_Rock_(Zaishen_quest)" };
    public static readonly Quest TheDragonsLairZaishenQuest = new() { Id = 954, Name = "The Dragons Lair (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Dragon%27s_Lair_(Zaishen_quest)" };
    public static readonly Quest IceCavesofSorrowZaishenQuest = new() { Id = 955, Name = "Ice Caves of Sorrow (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Ice_Caves_of_Sorrow_(Zaishen_quest)" };
    public static readonly Quest IronMinesofMoladuneZaishenQuest = new() { Id = 956, Name = "Iron Mines of Moladune (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Iron_Mines_of_Moladune_(Zaishen_quest)" };
    public static readonly Quest ThunderheadKeepZaishenQuest = new() { Id = 957, Name = "Thunderhead Keep (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Thunderhead_Keep_(Zaishen_quest)" };
    public static readonly Quest RingofFireZaishenQuest = new() { Id = 958, Name = "Ring of Fire (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Ring_of_Fire_(Zaishen_quest)" };
    public static readonly Quest AbaddonsMouthZaishenQuest = new() { Id = 959, Name = "Abaddons Mouth (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Abaddon%27s_Mouth_(Zaishen_quest)" };
    public static readonly Quest HellsPrecipiceZaishenQuest = new() { Id = 960, Name = "Hells Precipice (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Hell%27s_Precipice_(Zaishen_quest)" };
    public static readonly Quest ZenDaijunZaishenQuest = new() { Id = 961, Name = "Zen Daijun (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Zen_Daijun_(Zaishen_quest)" };
    public static readonly Quest VizunahSquareZaishenQuest = new() { Id = 962, Name = "Vizunah Square (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Vizunah_Square_(Zaishen_quest)" };
    public static readonly Quest NahpuiQuarterZaishenQuest = new() { Id = 963, Name = "Nahpui Quarter (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Nahpui_Quarter_(Zaishen_quest)" };
    public static readonly Quest TahnnakaiTempleZaishenQuest = new() { Id = 964, Name = "Tahnnakai Temple (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Tahnnakai_Temple_(Zaishen_quest)" };
    public static readonly Quest ArborstoneZaishenQuest = new() { Id = 965, Name = "Arborstone (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Arborstone_(Zaishen_quest)" };
    public static readonly Quest BoreasSeabedZaishenQuest = new() { Id = 966, Name = "Boreas Seabed (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Boreas_Seabed_(Zaishen_quest)" };
    public static readonly Quest SunjiangDistrictZaishenQuest = new() { Id = 967, Name = "Sunjiang District (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Sunjiang_District_(Zaishen_quest)" };
    public static readonly Quest TheEternalGroveZaishenQuest = new() { Id = 968, Name = "The Eternal Grove (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Eternal_Grove_(Zaishen_quest)" };
    public static readonly Quest UnwakingWatersZaishenQuest = new() { Id = 969, Name = "Unwaking Waters (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Unwaking_Waters_(Zaishen_quest)" };
    public static readonly Quest GyalaHatcheryZaishenQuest = new() { Id = 970, Name = "Gyala Hatchery (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Gyala_Hatchery_(Zaishen_quest)" };
    public static readonly Quest RaisuPalaceZaishenQuest = new() { Id = 971, Name = "Raisu Palace (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Raisu_Palace_(Zaishen_quest)" };
    public static readonly Quest ImperialSanctumZaishenQuest = new() { Id = 972, Name = "Imperial Sanctum (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Sanctum_(Zaishen_quest)" };
    public static readonly Quest ChahbekVillageZaishenQuest = new() { Id = 978, Name = "Chahbek Village (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Chahbek_Village_(Zaishen_quest)" };
    public static readonly Quest JokanurDiggingsZaishenQuest = new() { Id = 979, Name = "Jokanur Diggings (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Jokanur_Diggings_(Zaishen_quest)" };
    public static readonly Quest BlacktideDenZaishenQuest = new() { Id = 980, Name = "Blacktide Den (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Blacktide_Den_(Zaishen_quest)" };
    public static readonly Quest ConsulateDocksZaishenQuest = new() { Id = 981, Name = "Consulate Docks (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Consulate_Docks_(Zaishen_quest)" };
    public static readonly Quest VentaCemeteryZaishenQuest = new() { Id = 982, Name = "Venta Cemetery (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Venta_Cemetery_(Zaishen_quest)" };
    public static readonly Quest KodonurCrossroadsZaishenQuest = new() { Id = 983, Name = "Kodonur Crossroads (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Kodonur_Crossroads_(Zaishen_quest)" };
    public static readonly Quest RilohnRefugeZaishenQuest = new() { Id = 984, Name = "Rilohn Refuge (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Rilohn_Refuge_(Zaishen_quest)" };
    public static readonly Quest ModdokCreviceZaishenQuest = new() { Id = 985, Name = "Moddok Crevice (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Moddok_Crevice_(Zaishen_quest)" };
    public static readonly Quest TiharkOrchardZaishenQuest = new() { Id = 986, Name = "Tihark Orchard (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Tihark_Orchard_(Zaishen_quest)" };
    public static readonly Quest DzagonurBastionZaishenQuest = new() { Id = 987, Name = "Dzagonur Bastion (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Dzagonur_Bastion_(Zaishen_quest)" };
    public static readonly Quest DashaVestibuleZaishenQuest = new() { Id = 988, Name = "Dasha Vestibule (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Dasha_Vestibule_(Zaishen_quest)" };
    public static readonly Quest GrandCourtofSebelkehZaishenQuest = new() { Id = 989, Name = "Grand Court of Sebelkeh (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Grand_Court_of_Sebelkeh_(Zaishen_quest)" };
    public static readonly Quest JennursHordeZaishenQuest = new() { Id = 990, Name = "Jennurs Horde (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Jennur%27s_Horde_(Zaishen_quest)" };
    public static readonly Quest NunduBayZaishenQuest = new() { Id = 991, Name = "Nundu Bay (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Nundu_Bay_(Zaishen_quest)" };
    public static readonly Quest GateofDesolationZaishenQuest = new() { Id = 992, Name = "Gate of Desolation (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Desolation_(Zaishen_quest)" };
    public static readonly Quest RuinsofMorahZaishenQuest = new() { Id = 993, Name = "Ruins of Morah (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Ruins_of_Morah_(Zaishen_quest)" };
    public static readonly Quest GateofPainZaishenQuest = new() { Id = 994, Name = "Gate of Pain (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Pain_(Zaishen_quest)" };
    public static readonly Quest GateofMadnessZaishenQuest = new() { Id = 995, Name = "Gate of Madness (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_of_Madness_(Zaishen_quest)" };
    public static readonly Quest AbaddonsGateZaishenQuest = new() { Id = 996, Name = "Abaddons Gate (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Abaddon%27s_Gate_(Zaishen_quest)" };
    public static readonly Quest FindingtheBloodstoneZaishenQuest = new() { Id = 1000, Name = "Finding the Bloodstone (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_the_Bloodstone_(Zaishen_quest)" };
    public static readonly Quest TheElusiveGolemancerZaishenQuest = new() { Id = 1001, Name = "The Elusive Golemancer (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Elusive_Golemancer_(Zaishen_quest)" };
    public static readonly Quest GOLEMZaishenQuest = new() { Id = 1002, Name = "G.O.L.E.M. (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/G.O.L.E.M._(Zaishen_quest)" };
    public static readonly Quest AgainsttheCharrZaishenQuest = new() { Id = 1003, Name = "Against the Charr (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Against_the_Charr_(Zaishen_quest)" };
    public static readonly Quest WarbandofBrothersZaishenQuest = new() { Id = 1004, Name = "Warband of Brothers (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Warband_of_Brothers_(Zaishen_quest)" };
    public static readonly Quest AssaultontheStrongholdZaishenQuest = new() { Id = 1005, Name = "Assault on the Stronghold (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Assault_on_the_Stronghold_(Zaishen_quest)" };
    public static readonly Quest CurseoftheNornbearZaishenQuest = new() { Id = 1006, Name = "Curse of the Nornbear (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Curse_of_the_Nornbear_(Zaishen_quest)" };
    public static readonly Quest BloodWashesBloodZaishenQuest = new() { Id = 1007, Name = "Blood Washes Blood (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_Washes_Blood_(Zaishen_quest)" };
    public static readonly Quest AGateTooFarZaishenQuest = new() { Id = 1008, Name = "A Gate Too Far (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/A_Gate_Too_Far_(Zaishen_quest)" };
    public static readonly Quest DestructionsDepthsZaishenQuest = new() { Id = 1009, Name = "Destructions Depths (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Destruction%27s_Depths_(Zaishen_quest)" };
    public static readonly Quest ATimeforHeroesZaishenQuest = new() { Id = 1010, Name = "A Time for Heroes (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/A_Time_for_Heroes_(Zaishen_quest)" };
    public static readonly Quest VerataZaishenQuest = new() { Id = 1016, Name = "Verata (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Verata_(Zaishen_quest)" };
    public static readonly Quest RotscaleZaishenQuest = new() { Id = 1017, Name = "Rotscale (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Rotscale_(Zaishen_quest)" };
    public static readonly Quest TheIronForgemanZaishenQuest = new() { Id = 1019, Name = "The Iron Forgeman (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Iron_Forgeman_(Zaishen_quest)" };
    public static readonly Quest TheDarknessesZaishenQuest = new() { Id = 1020, Name = "The Darknesses (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Darknesses_(Zaishen_quest)" };
    public static readonly Quest KepkhetMarrowfeastZaishenQuest = new() { Id = 1021, Name = "Kepkhet Marrowfeast (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Kepkhet_Marrowfeast_(Zaishen_quest)" };
    public static readonly Quest HarnandMaxineColdstoneZaishenQuest = new() { Id = 1022, Name = "Harn and Maxine Coldstone (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Harn_and_Maxine_Coldstone_(Zaishen_quest)" };
    public static readonly Quest KanaxaiZaishenQuest = new() { Id = 1024, Name = "Kanaxai (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_(Zaishen_quest)" };
    public static readonly Quest UrgozZaishenQuest = new() { Id = 1025, Name = "Urgoz (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Urgoz_(Zaishen_quest)" };
    public static readonly Quest ChungtheAttunedZaishenQuest = new() { Id = 1026, Name = "Chung, the Attuned (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Chung,_the_Attuned_(Zaishen_quest)" };
    public static readonly Quest RoyenBeastkeeperZaishenQuest = new() { Id = 1027, Name = "Royen Beastkeeper (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Royen_Beastkeeper_(Zaishen_quest)" };
    public static readonly Quest KunvieFirewingZaishenQuest = new() { Id = 1028, Name = "Kunvie Firewing (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Kunvie_Firewing_(Zaishen_quest)" };
    public static readonly Quest MungriMagicboxZaishenQuest = new() { Id = 1029, Name = "Mungri Magicbox (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Mungri_Magicbox_(Zaishen_quest)" };
    public static readonly Quest ArborEarthcallZaishenQuest = new() { Id = 1030, Name = "Arbor Earthcall (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Arbor_Earthcall_(Zaishen_quest)" };
    public static readonly Quest MohbyWindbeakZaishenQuest = new() { Id = 1031, Name = "Mohby Windbeak (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Mohby_Windbeak_(Zaishen_quest)" };
    public static readonly Quest SsunsBlessedofDwaynaZaishenQuest = new() { Id = 1032, Name = "Ssuns, Blessed of Dwayna (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Ssuns,_Blessed_of_Dwayna_(Zaishen_quest)" };
    public static readonly Quest GhialtheBoneDancerZaishenQuest = new() { Id = 1033, Name = "Ghial the Bone Dancer (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Ghial_the_Bone_Dancer_(Zaishen_quest)" };
    public static readonly Quest QuansongSpiritspeakZaishenQuest = new() { Id = 1034, Name = "Quansong Spiritspeak (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Quansong_Spiritspeak_(Zaishen_quest)" };
    public static readonly Quest BaubaoWavewrathZaishenQuest = new() { Id = 1035, Name = "Baubao Wavewrath (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Baubao_Wavewrath_(Zaishen_quest)" };
    public static readonly Quest JarimiyatheUnmercifulZaishenQuest = new() { Id = 1036, Name = "Jarimiya the Unmerciful (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Jarimiya_the_Unmerciful_(Zaishen_quest)" };
    public static readonly Quest CommanderWahliZaishenQuest = new() { Id = 1037, Name = "Commander Wahli (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Wahli_(Zaishen_quest)" };
    public static readonly Quest DroajamMageoftheSandsZaishenQuest = new() { Id = 1038, Name = "Droajam, Mage of the Sands (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Droajam,_Mage_of_the_Sands_(Zaishen_quest)" };
    public static readonly Quest JedehtheMightyZaishenQuest = new() { Id = 1039, Name = "Jedeh the Mighty (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Jedeh_the_Mighty_(Zaishen_quest)" };
    public static readonly Quest KorshektheImmolatedZaishenQuest = new() { Id = 1040, Name = "Korshek the Immolated (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Korshek_the_Immolated_(Zaishen_quest)" };
    public static readonly Quest AdmiralKantohZaishenQuest = new() { Id = 1041, Name = "Admiral Kantoh (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Admiral_Kantoh_(Zaishen_quest)" };
    public static readonly Quest TheBlackBeastofArrghZaishenQuest = new() { Id = 1043, Name = "The Black Beast of Arrgh (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Black_Beast_of_Arrgh_(Zaishen_quest)" };
    public static readonly Quest LordJadothZaishenQuest = new() { Id = 1044, Name = "Lord Jadoth (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Lord_Jadoth_(Zaishen_quest)" };
    public static readonly Quest TheStygianUnderlordsZaishenQuest = new() { Id = 1045, Name = "The Stygian Underlords (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Stygian_Underlords_(Zaishen_quest)" };
    public static readonly Quest TheStygianLordsZaishenQuest = new() { Id = 1046, Name = "The Stygian Lords (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Stygian_Lords_(Zaishen_quest)" };
    public static readonly Quest TheGreaterDarknessZaishenQuest = new() { Id = 1047, Name = "The Greater Darkness (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Greater_Darkness_(Zaishen_quest)" };
    public static readonly Quest IlsundurLordofFireZaishenQuest = new() { Id = 1048, Name = "Ilsundur, Lord of Fire (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Ilsundur,_Lord_of_Fire_(Zaishen_quest)" };
    public static readonly Quest RragarManeaterZaishenQuest = new() { Id = 1049, Name = "Rragar Maneater (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Rragar_Maneater_(Zaishen_quest)" };
    public static readonly Quest MurakaiLadyoftheNightZaishenQuest = new() { Id = 1050, Name = "Murakai, Lady of the Night (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Murakai,_Lady_of_the_Night_(Zaishen_quest)" };
    public static readonly Quest PrismaticOozeZaishenQuest = new() { Id = 1051, Name = "Prismatic Ooze (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Prismatic_Ooze_(Zaishen_quest)" };
    public static readonly Quest HavokSoulwailZaishenQuest = new() { Id = 1052, Name = "Havok Soulwail (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Havok_Soulwail_(Zaishen_quest)" };
    public static readonly Quest FrostmawtheKinslayerZaishenQuest = new() { Id = 1053, Name = "Frostmaw the Kinslayer (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Frostmaw_the_Kinslayer_(Zaishen_quest)" };
    public static readonly Quest RemnantofAntiquitiesZaishenQuest = new() { Id = 1054, Name = "Remnant of Antiquities (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Remnant_of_Antiquities_(Zaishen_quest)" };
    public static readonly Quest PlagueofDestructionZaishenQuest = new() { Id = 1055, Name = "Plague of Destruction (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Plague_of_Destruction_(Zaishen_quest)" };
    public static readonly Quest ZoldarktheUnholyZaishenQuest = new() { Id = 1056, Name = "Zoldark the Unholy (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Zoldark_the_Unholy_(Zaishen_quest)" };
    public static readonly Quest KhabuusZaishenQuest = new() { Id = 1057, Name = "Khabuus (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Khabuus_(Zaishen_quest)" };
    public static readonly Quest ZhimMonnsZaishenQuest = new() { Id = 1058, Name = "Zhim Monns (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Z%27him_Monns_(Zaishen_quest)" };
    public static readonly Quest EldritchEttinZaishenQuest = new() { Id = 1059, Name = "Eldritch Ettin (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Eldritch_Ettin_(Zaishen_quest)" };
    public static readonly Quest FendiNinZaishenQuest = new() { Id = 1060, Name = "Fendi Nin (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fendi_Nin_(Zaishen_quest)" };
    public static readonly Quest TPSRegulatorGolemZaishenQuest = new() { Id = 1061, Name = "TPS Regulator Golem (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/TPS_Regulator_Golem_(Zaishen_quest)" };
    public static readonly Quest ArachniZaishenQuest = new() { Id = 1062, Name = "Arachni (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Arachni_(Zaishen_quest)" };
    public static readonly Quest ForgewightZaishenQuest = new() { Id = 1063, Name = "Forgewight (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Forgewight_(Zaishen_quest)" };
    public static readonly Quest SelvetarmZaishenQuest = new() { Id = 1064, Name = "Selvetarm (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Selvetarm_(Zaishen_quest)" };
    public static readonly Quest JusticiarThommisZaishenQuest = new() { Id = 1065, Name = "Justiciar Thommis (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Justiciar_Thommis_(Zaishen_quest)" };
    public static readonly Quest RandStormweaverZaishenQuest = new() { Id = 1066, Name = "Rand Stormweaver (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Rand_Stormweaver_(Zaishen_quest)" };
    public static readonly Quest DuncantheBlackZaishenQuest = new() { Id = 1067, Name = "Duncan the Black (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Duncan_the_Black_(Zaishen_quest)" };
    public static readonly Quest FronisIrontoeZaishenQuest = new() { Id = 1068, Name = "Fronis Irontoe (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fronis_Irontoe_(Zaishen_quest)" };
    public static readonly Quest MagmusZaishenQuest = new() { Id = 1070, Name = "Magmus (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Magmus_(Zaishen_quest)" };
    public static readonly Quest MolotovRocktailZaishenQuest = new() { Id = 1071, Name = "Molotov Rocktail (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Molotov_Rocktail_(Zaishen_quest)" };
    public static readonly Quest NulfastuEarthboundZaishenQuest = new() { Id = 1072, Name = "Nulfastu, Earthbound (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Nulfastu,_Earthbound_(Zaishen_quest)" };
    public static readonly Quest PywatttheSwiftZaishenQuest = new() { Id = 1074, Name = "Pywatt the Swift (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Pywatt_the_Swift_(Zaishen_quest)" };
    public static readonly Quest JoffstheMitigatorZaishenQuest = new() { Id = 1075, Name = "Joffs the Mitigator (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Joffs_the_Mitigator_(Zaishen_quest)" };
    public static readonly Quest MobrinLordoftheMarshZaishenQuest = new() { Id = 1076, Name = "Mobrin, Lord of the Marsh (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Mobrin,_Lord_of_the_Marsh_(Zaishen_quest)" };
    public static readonly Quest BorrguusBlisterbarkZaishenQuest = new() { Id = 1077, Name = "Borrguus Blisterbark (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Borrguus_Blisterbark_(Zaishen_quest)" };
    public static readonly Quest FozzyYeoryiosZaishenQuest = new() { Id = 1078, Name = "Fozzy Yeoryios (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fozzy_Yeoryios_(Zaishen_quest)" };
    public static readonly Quest MyishLadyoftheLakeZaishenQuest = new() { Id = 1079, Name = "Myish, Lady of the Lake (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Myish,_Lady_of_the_Lake_(Zaishen_quest)" };
    public static readonly Quest FenrirZaishenQuest = new() { Id = 1080, Name = "Fenrir (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fenrir_(Zaishen_quest)" };
    public static readonly Quest VengefulAatxeZaishenQuest = new() { Id = 1081, Name = "Vengeful Aatxe (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Vengeful_Aatxe_(Zaishen_quest)" };
    public static readonly Quest TheFourHorsemenZaishenQuest = new() { Id = 1082, Name = "The Four Horsemen (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Four_Horsemen_(Zaishen_quest)" };
    public static readonly Quest ChargedBlacknessZaishenQuest = new() { Id = 1083, Name = "Charged Blackness (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Charged_Blackness_(Zaishen_quest)" };
    public static readonly Quest DragonLichZaishenQuest = new() { Id = 1084, Name = "Dragon Lich (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Dragon_Lich_(Zaishen_quest)" };
    public static readonly Quest PriestofMenziesZaishenQuest = new() { Id = 1085, Name = "Priest of Menzies (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Priest_of_Menzies_(Zaishen_quest)" };
    public static readonly Quest LordKhobayZaishenQuest = new() { Id = 1086, Name = "Lord Khobay (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Lord_Khobay_(Zaishen_quest)" };
    public static readonly Quest RandomArenaZaishenQuest = new() { Id = 1087, Name = "Random Arena (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Random_Arena_(Zaishen_quest)" };
    public static readonly Quest HeroBattlesZaishenQuest = new() { Id = 1089, Name = "Hero Battles (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_Battles_(Zaishen_quest)" };
    public static readonly Quest FortAspenwoodZaishenQuest = new() { Id = 1090, Name = "Fort Aspenwood (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Aspenwood_(Zaishen_quest)" };
    public static readonly Quest JadeQuarryZaishenQuest = new() { Id = 1091, Name = "Jade Quarry (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Quarry_(Zaishen_quest)" };
    public static readonly Quest AllianceBattlesZaishenQuest = new() { Id = 1092, Name = "Alliance Battles (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Alliance_Battles_(Zaishen_quest)" };
    public static readonly Quest GuildVersusGuildZaishenQuest = new() { Id = 1093, Name = "Guild Versus Guild (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Versus_Guild_(Zaishen_quest)" };
    public static readonly Quest HeroesAscentZaishenQuest = new() { Id = 1094, Name = "Heroes Ascent (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes%27_Ascent_(Zaishen_quest)" };
    public static readonly Quest RandomArenaZaishenQuest1 = new() { Id = 1095, Name = "Random Arena (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Random_Arena_(Zaishen_quest)" };
    public static readonly Quest HeroBattles = new() { Id = 1097, Name = "Hero Battles", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_Battles" };
    public static readonly Quest FortAspenwoodZaishenQuest1 = new() { Id = 1098, Name = "Fort Aspenwood (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Aspenwood_(Zaishen_quest)" };
    public static readonly Quest TheJadeQuarry = new() { Id = 1099, Name = "The Jade Quarry", WikiUrl = "https://wiki.guildwars.com/wiki/The_Jade_Quarry" };
    public static readonly Quest AllianceBattlesZaishenQuest1 = new() { Id = 1100, Name = "Alliance Battles (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Alliance_Battles_(Zaishen_quest)" };
    public static readonly Quest GuildVersusGuildZaishenQuest1 = new() { Id = 1101, Name = "Guild Versus Guild (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Versus_Guild_(Zaishen_quest)" };
    public static readonly Quest HeroesAscentZaishenQuest1 = new() { Id = 1102, Name = "Heroes Ascent (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes%27_Ascent_(Zaishen_quest)" };
    public static readonly Quest RandomArenaZaishenQuest2 = new() { Id = 1103, Name = "Random Arena (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Random_Arena_(Zaishen_quest)" };
    public static readonly Quest FortAspenwoodZaishenQuest2 = new() { Id = 1106, Name = "Fort Aspenwood (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Fort_Aspenwood_(Zaishen_quest)" };
    public static readonly Quest JadeQuarryZaishenQuest1 = new() { Id = 1107, Name = "Jade Quarry (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Quarry_(Zaishen_quest)" };
    public static readonly Quest GuildVersusGuildZaishenQuest2 = new() { Id = 1109, Name = "Guild Versus Guild (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Versus_Guild_(Zaishen_quest)" };
    public static readonly Quest RandomArenaZaishenQuest3 = new() { Id = 1111, Name = "Random Arena (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Random_Arena_(Zaishen_quest)" };
    public static readonly Quest TeamArenaZaishenQuest = new() { Id = 1112, Name = "Team Arena (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Team_Arena_(Zaishen_quest)" };
    public static readonly Quest HeroBattlesZaishenQuest1 = new() { Id = 1113, Name = "Hero Battles (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_Battles_(Zaishen_quest)" };
    public static readonly Quest JadeQuarryZaishenQuest2 = new() { Id = 1115, Name = "Jade Quarry (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Quarry_(Zaishen_quest)" };
    public static readonly Quest AllianceBattlesZaishenQuest2 = new() { Id = 1116, Name = "Alliance Battles (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Alliance_Battles_(Zaishen_quest)" };
    public static readonly Quest GuildVersusGuildZaishenQuest3 = new() { Id = 1117, Name = "Guild Versus Guild (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Versus_Guild_(Zaishen_quest)" };
    public static readonly Quest HeroesAscentZaishenQuest2 = new() { Id = 1118, Name = "Heroes Ascent (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Heroes%27_Ascent_(Zaishen_quest)" };
    public static readonly Quest MinisterChosEstateZaishenQuest = new() { Id = 1119, Name = "Minister Chos Estate (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Cho%27s_Estate_(Zaishen_quest)" };
    public static readonly Quest SomethingWickedThisWayComes = new() { Id = 1120, Name = "Something Wicked This Way Comes", WikiUrl = "https://wiki.guildwars.com/wiki/Something_Wicked_This_Way_Comes" };
    public static readonly Quest DontFeartheReapers = new() { Id = 1121, Name = "Dont Fear the Reapers", WikiUrl = "https://wiki.guildwars.com/wiki/Don%27t_Fear_the_Reapers" };
    public static readonly Quest StemmingtheSkeletalTide = new() { Id = 1122, Name = "Stemming the Skeletal Tide", WikiUrl = "https://wiki.guildwars.com/wiki/Stemming_the_Skeletal_Tide" };
    public static readonly Quest EveryBitHelps = new() { Id = 1123, Name = "Every Bit Helps", WikiUrl = "https://wiki.guildwars.com/wiki/Every_Bit_Helps" };
    public static readonly Quest TheWaitingGame = new() { Id = 1124, Name = "The Waiting Game", WikiUrl = "https://wiki.guildwars.com/wiki/The_Waiting_Game" };
    public static readonly Quest CodexArenaZaishenQuest = new() { Id = 1125, Name = "Codex Arena (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Codex_Arena_(Zaishen_quest)" };
    public static readonly Quest CodexArenaZaishenQuest1 = new() { Id = 1126, Name = "Codex Arena (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Codex_Arena_(Zaishen_quest)" };
    public static readonly Quest CodexArenaZaishenQuest2 = new() { Id = 1128, Name = "Codex Arena (Zaishen quest)", WikiUrl = "https://wiki.guildwars.com/wiki/Codex_Arena_(Zaishen_quest)" };
    public static readonly Quest TheNightmanCometh = new() { Id = 1129, Name = "The Nightman Cometh", WikiUrl = "https://wiki.guildwars.com/wiki/The_Nightman_Cometh" };
    public static readonly Quest WantedInquisitorLashona = new() { Id = 1130, Name = "Wanted Inquisitor Lashona", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Inquisitor_Lashona" };
    public static readonly Quest WantedInquisitorLovisa = new() { Id = 1131, Name = "Wanted Inquisitor Lovisa", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Inquisitor_Lovisa" };
    public static readonly Quest WantedInquisitorBauer = new() { Id = 1132, Name = "Wanted Inquisitor Bauer", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Inquisitor_Bauer" };
    public static readonly Quest WantedJusticiarKasandra = new() { Id = 1133, Name = "Wanted Justiciar Kasandra", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Justiciar_Kasandra" };
    public static readonly Quest WantedJusticiarAmilyn = new() { Id = 1134, Name = "Wanted Justiciar Amilyn", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Justiciar_Amilyn" };
    public static readonly Quest WantedJusticiarSevaan = new() { Id = 1135, Name = "Wanted Justiciar Sevaan", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Justiciar_Sevaan" };
    public static readonly Quest WantedValistheRampant = new() { Id = 1136, Name = "Wanted Valis the Rampant", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Valis_the_Rampant" };
    public static readonly Quest WantedMaximiliantheMeticulous = new() { Id = 1137, Name = "Wanted Maximilian the Meticulous", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Maximilian_the_Meticulous" };
    public static readonly Quest WantedSarniatheRedHanded = new() { Id = 1138, Name = "Wanted Sarnia the Red-Handed", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Sarnia_the_Red-Handed" };
    public static readonly Quest WantedDestortheTruthSeeker = new() { Id = 1139, Name = "Wanted Destor the Truth Seeker", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Destor_the_Truth_Seeker" };
    public static readonly Quest WantedSelenastheBlunt = new() { Id = 1140, Name = "Wanted Selenas the Blunt", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Selenas_the_Blunt" };
    public static readonly Quest WantedBarthimustheProvident = new() { Id = 1141, Name = "Wanted Barthimus the Provident", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Barthimus_the_Provident" };
    public static readonly Quest WantedCerris = new() { Id = 1142, Name = "Wanted Cerris", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Cerris" };
    public static readonly Quest WantedCarnaktheHungry = new() { Id = 1143, Name = "Wanted Carnak the Hungry", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Carnak_the_Hungry" };
    public static readonly Quest WantedInsatiableVakar = new() { Id = 1144, Name = "Wanted Insatiable Vakar", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Insatiable_Vakar" };
    public static readonly Quest WantedAmalektheUnmerciful = new() { Id = 1145, Name = "Wanted Amalek the Unmerciful", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Amalek_the_Unmerciful" };
    public static readonly Quest WantedJohtheHostile = new() { Id = 1146, Name = "Wanted Joh the Hostile", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Joh_the_Hostile" };
    public static readonly Quest WantedGrevestheOverbearing = new() { Id = 1147, Name = "Wanted Greves the Overbearing", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Greves_the_Overbearing" };
    public static readonly Quest WantedCalamitous = new() { Id = 1148, Name = "Wanted Calamitous", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Calamitous" };
    public static readonly Quest WantedLevtheCondemned = new() { Id = 1149, Name = "Wanted Lev the Condemned", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Lev_the_Condemned" };
    public static readonly Quest WantedVesstheDisputant = new() { Id = 1150, Name = "Wanted Vess the Disputant", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Vess_the_Disputant" };
    public static readonly Quest WantedJusticiarKimii = new() { Id = 1151, Name = "Wanted Justiciar Kimii", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Justiciar_Kimii" };
    public static readonly Quest WantedZalntheJaded = new() { Id = 1152, Name = "Wanted Zaln the Jaded", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Zaln_the_Jaded" };
    public static readonly Quest WantedJusticiarMarron = new() { Id = 1153, Name = "Wanted Justiciar Marron", WikiUrl = "https://wiki.guildwars.com/wiki/Wanted:_Justiciar_Marron" };
    public static readonly Quest RiversideAssassination = new() { Id = 1154, Name = "Riverside Assassination", WikiUrl = "https://wiki.guildwars.com/wiki/Riverside_Assassination" };
    public static readonly Quest ALittleHelpFromAbove = new() { Id = 1155, Name = "A Little Help From Above", WikiUrl = "https://wiki.guildwars.com/wiki/A_Little_Help_From_Above" };
    public static readonly Quest TempleoftheIntolerable = new() { Id = 1156, Name = "Temple of the Intolerable", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_of_the_Intolerable" };
    public static readonly Quest MusteringaResponse = new() { Id = 1157, Name = "Mustering a Response", WikiUrl = "https://wiki.guildwars.com/wiki/Mustering_a_Response" };
    public static readonly Quest TheBattleforLionsArch = new() { Id = 1158, Name = "The Battle for Lions Arch", WikiUrl = "https://wiki.guildwars.com/wiki/The_Battle_for_Lion%27s_Arch" };
    public static readonly Quest HeirloomsoftheMadKing = new() { Id = 1159, Name = "Heirlooms of the Mad King", WikiUrl = "https://wiki.guildwars.com/wiki/Heirlooms_of_the_Mad_King" };
    public static readonly Quest CommandeeringaMortalVessel = new() { Id = 1160, Name = "Commandeering a Mortal Vessel", WikiUrl = "https://wiki.guildwars.com/wiki/Commandeering_a_Mortal_Vessel" };
    public static readonly Quest AnIngeniousPlan = new() { Id = 1161, Name = "An Ingenious Plan", WikiUrl = "https://wiki.guildwars.com/wiki/An_Ingenious_Plan" };
    public static readonly Quest TilDeathDoUsPart = new() { Id = 1162, Name = "Til Death Do Us Part...", WikiUrl = "https://wiki.guildwars.com/wiki/Til_Death_Do_Us_Part..." };
    public static readonly Quest OpentheFloodGatesofDeath = new() { Id = 1163, Name = "Open the Flood Gates of Death", WikiUrl = "https://wiki.guildwars.com/wiki/Open_the_Flood_Gates_of_Death" };
    public static readonly Quest TheKillingJoke = new() { Id = 1164, Name = "The Killing Joke", WikiUrl = "https://wiki.guildwars.com/wiki/The_Killing_Joke" };
    public static readonly Quest NornCatering = new() { Id = 1165, Name = "Norn Catering", WikiUrl = "https://wiki.guildwars.com/wiki/Norn_Catering" };
    public static readonly Quest TheArrowhead = new() { Id = 1166, Name = "The Arrowhead", WikiUrl = "https://wiki.guildwars.com/wiki/The_Arrowhead" };
    public static readonly Quest TheTarnishedEmblem = new() { Id = 1167, Name = "The Tarnished Emblem", WikiUrl = "https://wiki.guildwars.com/wiki/The_Tarnished_Emblem" };
    public static readonly Quest TheBrokenSword = new() { Id = 1168, Name = "The Broken Sword", WikiUrl = "https://wiki.guildwars.com/wiki/The_Broken_Sword" };
    public static readonly Quest TheMantlesGuise = new() { Id = 1169, Name = "The Mantles Guise", WikiUrl = "https://wiki.guildwars.com/wiki/The_Mantle%27s_Guise" };
    public static readonly Quest AuspiciousBeginnings = new() { Id = 1170, Name = "Auspicious Beginnings", WikiUrl = "https://wiki.guildwars.com/wiki/Auspicious_Beginnings" };
    public static readonly Quest AVengeanceofBlades = new() { Id = 1171, Name = "A Vengeance of Blades", WikiUrl = "https://wiki.guildwars.com/wiki/A_Vengeance_of_Blades" };
    public static readonly Quest ShadowsintheJungle = new() { Id = 1172, Name = "Shadows in the Jungle", WikiUrl = "https://wiki.guildwars.com/wiki/Shadows_in_the_Jungle" };
    public static readonly Quest Rise = new() { Id = 1173, Name = "Rise", WikiUrl = "https://wiki.guildwars.com/wiki/Rise" };
    public static readonly Quest Reunion = new() { Id = 1174, Name = "Reunion", WikiUrl = "https://wiki.guildwars.com/wiki/Reunion" };
    public static readonly Quest TheWedding = new() { Id = 1175, Name = "The Wedding", WikiUrl = "https://wiki.guildwars.com/wiki/The_Wedding" };
    public static readonly Quest TheWarinKryta = new() { Id = 1176, Name = "The War in Kryta", WikiUrl = "https://wiki.guildwars.com/wiki/The_War_in_Kryta" };
    public static readonly Quest AsuranAllies = new() { Id = 1177, Name = "Asuran Allies", WikiUrl = "https://wiki.guildwars.com/wiki/Asuran_Allies" };
    public static readonly Quest EbonVanguardAllies = new() { Id = 1178, Name = "Ebon Vanguard Allies", WikiUrl = "https://wiki.guildwars.com/wiki/Ebon_Vanguard_Allies" };
    public static readonly Quest OperationCrushSpirits = new() { Id = 1179, Name = "Operation Crush Spirits", WikiUrl = "https://wiki.guildwars.com/wiki/Operation:_Crush_Spirits" };
    public static readonly Quest FightinginaWinterWonderland = new() { Id = 1180, Name = "Fighting in a Winter Wonderland", WikiUrl = "https://wiki.guildwars.com/wiki/Fighting_in_a_Winter_Wonderland" };
    public static readonly Quest VanguardBountyBlazefiendGriefblade = new() { Id = 1182, Name = "Vanguard Bounty Blazefiend Griefblade", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Bounty:_Blazefiend_Griefblade" };
    public static readonly Quest VanguardBountyCountessNadya = new() { Id = 1183, Name = "Vanguard Bounty Countess Nadya", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Bounty:_Countess_Nadya" };
    public static readonly Quest VanguardBountyUtiniWupwup = new() { Id = 1184, Name = "Vanguard Bounty Utini Wupwup", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Bounty:_Utini_Wupwup" };
    public static readonly Quest VanguardRescueFarmerHamnet = new() { Id = 1185, Name = "Vanguard Rescue Farmer Hamnet", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Rescue:_Farmer_Hamnet" };
    public static readonly Quest VanguardRescueFootmanTate = new() { Id = 1186, Name = "Vanguard Rescue Footman Tate", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Rescue:_Footman_Tate" };
    public static readonly Quest VanguardRescueSavetheAscalonianNoble = new() { Id = 1187, Name = "Vanguard Rescue Save the Ascalonian Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Rescue:_Save_the_Ascalonian_Noble" };
    public static readonly Quest VanguardAnnihilationCharr = new() { Id = 1188, Name = "Vanguard Annihilation Charr", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Annihilation:_Charr" };
    public static readonly Quest VanguardAnnihilationBandits = new() { Id = 1189, Name = "Vanguard Annihilation Bandits", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Annihilation:_Bandits" };
    public static readonly Quest VanguardAnnihilationUndead = new() { Id = 1190, Name = "Vanguard Annihilation Undead", WikiUrl = "https://wiki.guildwars.com/wiki/Vanguard_Annihilation:_Undead" };
    public static readonly Quest AnvilRockZaishenVanquish = new() { Id = 1191, Name = "Anvil Rock (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Anvil_Rock_(Zaishen_vanquish)" };
    public static readonly Quest ArborstoneZaishenVanquish = new() { Id = 1192, Name = "Arborstone (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Arborstone_(Zaishen_vanquish)" };
    public static readonly Quest WitmansFollyZaishenVanquish = new() { Id = 1193, Name = "Witmans Folly (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Witman%27s_Folly_(Zaishen_vanquish)" };
    public static readonly Quest ArkjokWardZaishenVanquish = new() { Id = 1194, Name = "Arkjok Ward (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Arkjok_Ward_(Zaishen_vanquish)" };
    public static readonly Quest AscalonFoothillsZaishenVanquish = new() { Id = 1195, Name = "Ascalon Foothills (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Ascalon_Foothills_(Zaishen_vanquish)" };
    public static readonly Quest BahdokCavernsZaishenVanquish = new() { Id = 1196, Name = "Bahdok Caverns (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Bahdok_Caverns_(Zaishen_vanquish)" };
    public static readonly Quest CursedLandsZaishenVanquish = new() { Id = 1197, Name = "Cursed Lands (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Cursed_Lands_(Zaishen_vanquish)" };
    public static readonly Quest AlcaziaTangleZaishenVanquish = new() { Id = 1198, Name = "Alcazia Tangle (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Alcazia_Tangle_(Zaishen_vanquish)" };
    public static readonly Quest ArchipelagosZaishenVanquish = new() { Id = 1199, Name = "Archipelagos (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Archipelagos_(Zaishen_vanquish)" };
    public static readonly Quest EasternFrontierZaishenVanquish = new() { Id = 1200, Name = "Eastern Frontier (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Eastern_Frontier_(Zaishen_vanquish)" };
    public static readonly Quest DejarinEstateZaishenVanquish = new() { Id = 1201, Name = "Dejarin Estate (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Dejarin_Estate_(Zaishen_vanquish)" };
    public static readonly Quest WatchtowerCoastZaishenVanquish = new() { Id = 1202, Name = "Watchtower Coast (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Watchtower_Coast_(Zaishen_vanquish)" };
    public static readonly Quest ArborBayZaishenVanquish = new() { Id = 1203, Name = "Arbor Bay (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Arbor_Bay_(Zaishen_vanquish)" };
    public static readonly Quest BarbarousShoreZaishenVanquish = new() { Id = 1204, Name = "Barbarous Shore (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Barbarous_Shore_(Zaishen_vanquish)" };
    public static readonly Quest DeldrimorBowlZaishenVanquish = new() { Id = 1205, Name = "Deldrimor Bowl (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Deldrimor_Bowl_(Zaishen_vanquish)" };
    public static readonly Quest BoreasSeabedZaishenVanquish = new() { Id = 1206, Name = "Boreas Seabed (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Boreas_Seabed_(Zaishen_vanquish)" };
    public static readonly Quest CliffsofDohjokZaishenVanquish = new() { Id = 1207, Name = "Cliffs of Dohjok (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Cliffs_of_Dohjok_(Zaishen_vanquish)" };
    public static readonly Quest DiessaLowlandsZaishenVanquish = new() { Id = 1208, Name = "Diessa Lowlands (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Diessa_Lowlands_(Zaishen_vanquish)" };
    public static readonly Quest BukdekBywayZaishenVanquish = new() { Id = 1209, Name = "Bukdek Byway (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Bukdek_Byway_(Zaishen_vanquish)" };
    public static readonly Quest BjoraMarchesZaishenVanquish = new() { Id = 1210, Name = "Bjora Marches (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Bjora_Marches_(Zaishen_vanquish)" };
    public static readonly Quest CrystalOverlookZaishenVanquish = new() { Id = 1211, Name = "Crystal Overlook (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Crystal_Overlook_(Zaishen_vanquish)" };
    public static readonly Quest DivinersAscentZaishenVanquish = new() { Id = 1212, Name = "Diviners Ascent (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Diviner%27s_Ascent_(Zaishen_vanquish)" };
    public static readonly Quest DaladaUplandsZaishenVanquish = new() { Id = 1213, Name = "Dalada Uplands (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Dalada_Uplands_(Zaishen_vanquish)" };
    public static readonly Quest DrazachThicketZaishenVanquish = new() { Id = 1214, Name = "Drazach Thicket (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Drazach_Thicket_(Zaishen_vanquish)" };
    public static readonly Quest FahranurtheFirstCityZaishenVanquish = new() { Id = 1215, Name = "Fahranur, the First City (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Fahranur,_the_First_City_(Zaishen_vanquish)" };
    public static readonly Quest DragonsGulletZaishenVanquish = new() { Id = 1216, Name = "Dragons Gullet (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Dragon%27s_Gullet_(Zaishen_vanquish)" };
    public static readonly Quest FerndaleZaishenVanquish = new() { Id = 1217, Name = "Ferndale (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Ferndale_(Zaishen_vanquish)" };
    public static readonly Quest ForumHighlandsZaishenVanquish = new() { Id = 1218, Name = "Forum Highlands (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Forum_Highlands_(Zaishen_vanquish)" };
    public static readonly Quest DreadnoughtsDriftZaishenVanquish = new() { Id = 1219, Name = "Dreadnoughts Drift (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Dreadnought%27s_Drift_(Zaishen_vanquish)" };
    public static readonly Quest DrakkarLakeZaishenVanquish = new() { Id = 1220, Name = "Drakkar Lake (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Drakkar_Lake_(Zaishen_vanquish)" };
    public static readonly Quest DryTopZaishenVanquish = new() { Id = 1221, Name = "Dry Top (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Dry_Top_(Zaishen_vanquish)" };
    public static readonly Quest TearsoftheFallenZaishenVanquish = new() { Id = 1222, Name = "Tears of the Fallen (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Tears_of_the_Fallen_(Zaishen_vanquish)" };
    public static readonly Quest GyalaHatcheryZaishenVanquish = new() { Id = 1223, Name = "Gyala Hatchery (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Gyala_Hatchery_(Zaishen_vanquish)" };
    public static readonly Quest EttinsBackZaishenVanquish = new() { Id = 1224, Name = "Ettins Back (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Ettin%27s_Back_(Zaishen_vanquish)" };
    public static readonly Quest GandaratheMoonFortressZaishenVanquish = new() { Id = 1225, Name = "Gandara, the Moon Fortress (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Gandara,_the_Moon_Fortress_(Zaishen_vanquish)" };
    public static readonly Quest GrothmarWardownsZaishenVanquish = new() { Id = 1226, Name = "Grothmar Wardowns (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Grothmar_Wardowns_(Zaishen_vanquish)" };
    public static readonly Quest FlameTempleCorridorZaishenVanquish = new() { Id = 1227, Name = "Flame Temple Corridor (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Flame_Temple_Corridor_(Zaishen_vanquish)" };
    public static readonly Quest HaijuLagoonZaishenVanquish = new() { Id = 1228, Name = "Haiju Lagoon (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Haiju_Lagoon_(Zaishen_vanquish)" };
    public static readonly Quest FrozenForestZaishenVanquish = new() { Id = 1229, Name = "Frozen Forest (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Frozen_Forest_(Zaishen_vanquish)" };
    public static readonly Quest GardenofSeborhinZaishenVanquish = new() { Id = 1230, Name = "Garden of Seborhin (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Garden_of_Seborhin_(Zaishen_vanquish)" };
    public static readonly Quest GrenthsFootprintZaishenVanquish = new() { Id = 1231, Name = "Grenths Footprint (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Grenth%27s_Footprint_(Zaishen_vanquish)" };
    public static readonly Quest JayaBluffsZaishenVanquish = new() { Id = 1232, Name = "Jaya Bluffs (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Jaya_Bluffs_(Zaishen_vanquish)" };
    public static readonly Quest HoldingsofChokhinZaishenVanquish = new() { Id = 1233, Name = "Holdings of Chokhin (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Holdings_of_Chokhin_(Zaishen_vanquish)" };
    public static readonly Quest IceCliffChasmsZaishenVanquish = new() { Id = 1234, Name = "Ice Cliff Chasms (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Ice_Cliff_Chasms_(Zaishen_vanquish)" };
    public static readonly Quest GriffonsMouthZaishenVanquish = new() { Id = 1235, Name = "Griffons Mouth (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Griffon%27s_Mouth_(Zaishen_vanquish)" };
    public static readonly Quest KinyaProvinceZaishenVanquish = new() { Id = 1236, Name = "Kinya Province (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Kinya_Province_(Zaishen_vanquish)" };
    public static readonly Quest IssnurIslesZaishenVanquish = new() { Id = 1237, Name = "Issnur Isles (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Issnur_Isles_(Zaishen_vanquish)" };
    public static readonly Quest JagaMoraineZaishenVanquish = new() { Id = 1238, Name = "Jaga Moraine (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Jaga_Moraine_(Zaishen_vanquish)" };
    public static readonly Quest IceFloeZaishenVanquish = new() { Id = 1239, Name = "Ice Floe (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Ice_Floe_(Zaishen_vanquish)" };
    public static readonly Quest MaishangHillsZaishenVanquish = new() { Id = 1240, Name = "Maishang Hills (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Maishang_Hills_(Zaishen_vanquish)" };
    public static readonly Quest JahaiBluffsZaishenVanquish = new() { Id = 1241, Name = "Jahai Bluffs (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Jahai_Bluffs_(Zaishen_vanquish)" };
    public static readonly Quest RivenEarthZaishenVanquish = new() { Id = 1242, Name = "Riven Earth (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Riven_Earth_(Zaishen_vanquish)" };
    public static readonly Quest IcedomeZaishenVanquish = new() { Id = 1243, Name = "Icedome (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Icedome_(Zaishen_vanquish)" };
    public static readonly Quest MinisterChosEstateZaishenVanquish = new() { Id = 1244, Name = "Minister Chos Estate (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Cho%27s_Estate_(Zaishen_vanquish)" };
    public static readonly Quest MehtaniKeysZaishenVanquish = new() { Id = 1245, Name = "Mehtani Keys (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Mehtani_Keys_(Zaishen_vanquish)" };
    public static readonly Quest SacnothValleyZaishenVanquish = new() { Id = 1246, Name = "Sacnoth Valley (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Sacnoth_Valley_(Zaishen_vanquish)" };
    public static readonly Quest IronHorseMineZaishenVanquish = new() { Id = 1247, Name = "Iron Horse Mine (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Iron_Horse_Mine_(Zaishen_vanquish)" };
    public static readonly Quest MorostavTrailZaishenVanquish = new() { Id = 1248, Name = "Morostav Trail (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Morostav_Trail_(Zaishen_vanquish)" };
    public static readonly Quest PlainsofJarinZaishenVanquish = new() { Id = 1249, Name = "Plains of Jarin (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Plains_of_Jarin_(Zaishen_vanquish)" };
    public static readonly Quest SparkflySwampZaishenVanquish = new() { Id = 1250, Name = "Sparkfly Swamp (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Sparkfly_Swamp_(Zaishen_vanquish)" };
    public static readonly Quest KessexPeakZaishenVanquish = new() { Id = 1251, Name = "Kessex Peak (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Kessex_Peak_(Zaishen_vanquish)" };
    public static readonly Quest MourningVeilFallsZaishenVanquish = new() { Id = 1252, Name = "Mourning Veil Falls (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Mourning_Veil_Falls_(Zaishen_vanquish)" };
    public static readonly Quest TheAlkaliPanZaishenVanquish = new() { Id = 1253, Name = "The Alkali Pan (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Alkali_Pan_(Zaishen_vanquish)" };
    public static readonly Quest VarajarFellsZaishenVanquish = new() { Id = 1254, Name = "Varajar Fells (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Varajar_Fells_(Zaishen_vanquish)" };
    public static readonly Quest LornarsPassZaishenVanquish = new() { Id = 1255, Name = "Lornars Pass (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Lornar%27s_Pass_(Zaishen_vanquish)" };
    public static readonly Quest PongmeiValleyZaishenVanquish = new() { Id = 1256, Name = "Pongmei Valley (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Pongmei_Valley_(Zaishen_vanquish)" };
    public static readonly Quest TheFloodplainofMahnkelonZaishenVanquish = new() { Id = 1257, Name = "The Floodplain of Mahnkelon (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Floodplain_of_Mahnkelon_(Zaishen_vanquish)" };
    public static readonly Quest VerdantCascadesZaishenVanquish = new() { Id = 1258, Name = "Verdant Cascades (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Verdant_Cascades_(Zaishen_vanquish)" };
    public static readonly Quest MajestysRestZaishenVanquish = new() { Id = 1259, Name = "Majestys Rest (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Majesty%27s_Rest_(Zaishen_vanquish)" };
    public static readonly Quest RaisuPalaceZaishenVanquish = new() { Id = 1260, Name = "Raisu Palace (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Raisu_Palace_(Zaishen_vanquish)" };
    public static readonly Quest TheHiddenCityofAhdashimZaishenVanquish = new() { Id = 1261, Name = "The Hidden City of Ahdashim (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hidden_City_of_Ahdashim_(Zaishen_vanquish)" };
    public static readonly Quest RheasCraterZaishenVanquish = new() { Id = 1262, Name = "Rheas Crater (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Rhea%27s_Crater_(Zaishen_vanquish)" };
    public static readonly Quest MamnoonLagoonZaishenVanquish = new() { Id = 1263, Name = "Mamnoon Lagoon (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Mamnoon_Lagoon_(Zaishen_vanquish)" };
    public static readonly Quest ShadowsPassageZaishenVanquish = new() { Id = 1264, Name = "Shadows Passage (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow%27s_Passage_(Zaishen_vanquish)" };
    public static readonly Quest TheMirrorofLyssZaishenVanquish = new() { Id = 1265, Name = "The Mirror of Lyss (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Mirror_of_Lyss_(Zaishen_vanquish)" };
    public static readonly Quest SaoshangTrailZaishenVanquish = new() { Id = 1266, Name = "Saoshang Trail (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Saoshang_Trail_(Zaishen_vanquish)" };
    public static readonly Quest NeboTerraceZaishenVanquish = new() { Id = 1267, Name = "Nebo Terrace (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Nebo_Terrace_(Zaishen_vanquish)" };
    public static readonly Quest ShenzunTunnelsZaishenVanquish = new() { Id = 1268, Name = "Shenzun Tunnels (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Shenzun_Tunnels_(Zaishen_vanquish)" };
    public static readonly Quest TheRupturedHeartZaishenVanquish = new() { Id = 1269, Name = "The Ruptured Heart (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ruptured_Heart_(Zaishen_vanquish)" };
    public static readonly Quest SaltFlatsZaishenVanquish = new() { Id = 1270, Name = "Salt Flats (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Salt_Flats_(Zaishen_vanquish)" };
    public static readonly Quest NorthKrytaProvinceZaishenVanquish = new() { Id = 1271, Name = "North Kryta Province (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/North_Kryta_Province_(Zaishen_vanquish)" };
    public static readonly Quest SilentSurfZaishenVanquish = new() { Id = 1272, Name = "Silent Surf (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Silent_Surf_(Zaishen_vanquish)" };
    public static readonly Quest TheShatteredRavinesZaishenVanquish = new() { Id = 1273, Name = "The Shattered Ravines (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Shattered_Ravines_(Zaishen_vanquish)" };
    public static readonly Quest ScoundrelsRiseZaishenVanquish = new() { Id = 1274, Name = "Scoundrels Rise (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Scoundrel%27s_Rise_(Zaishen_vanquish)" };
    public static readonly Quest OldAscalonZaishenVanquish = new() { Id = 1275, Name = "Old Ascalon (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Old_Ascalon_(Zaishen_vanquish)" };
    public static readonly Quest SunjiangDistrictZaishenVanquish = new() { Id = 1276, Name = "Sunjiang District (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Sunjiang_District_(Zaishen_vanquish)" };
    public static readonly Quest TheSulfurousWastesZaishenVanquish = new() { Id = 1277, Name = "The Sulfurous Wastes (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Sulfurous_Wastes_(Zaishen_vanquish)" };
    public static readonly Quest MagusStonesZaishenVanquish = new() { Id = 1278, Name = "Magus Stones (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Magus_Stones_(Zaishen_vanquish)" };
    public static readonly Quest PerditionRockZaishenVanquish = new() { Id = 1279, Name = "Perdition Rock (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Perdition_Rock_(Zaishen_vanquish)" };
    public static readonly Quest SunquaValeZaishenVanquish = new() { Id = 1280, Name = "Sunqua Vale (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Sunqua_Vale_(Zaishen_vanquish)" };
    public static readonly Quest TuraisProcessionZaishenVanquish = new() { Id = 1281, Name = "Turais Procession (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Turai%27s_Procession_(Zaishen_vanquish)" };
    public static readonly Quest NorrhartDomainsZaishenVanquish = new() { Id = 1282, Name = "Norrhart Domains (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Norrhart_Domains_(Zaishen_vanquish)" };
    public static readonly Quest PockmarkFlatsZaishenVanquish = new() { Id = 1283, Name = "Pockmark Flats (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Pockmark_Flats_(Zaishen_vanquish)" };
    public static readonly Quest TahnnakaiTempleZaishenVanquish = new() { Id = 1284, Name = "Tahnnakai Temple (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Tahnnakai_Temple_(Zaishen_vanquish)" };
    public static readonly Quest VehjinMinesZaishenVanquish = new() { Id = 1285, Name = "Vehjin Mines (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Vehjin_Mines_(Zaishen_vanquish)" };
    public static readonly Quest PoisonedOutcropsZaishenVanquish = new() { Id = 1286, Name = "Poisoned Outcrops (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Poisoned_Outcrops_(Zaishen_vanquish)" };
    public static readonly Quest ProphetsPathZaishenVanquish = new() { Id = 1287, Name = "Prophets Path (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Prophet%27s_Path_(Zaishen_vanquish)" };
    public static readonly Quest TheEternalGroveZaishenVanquish = new() { Id = 1288, Name = "The Eternal Grove (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Eternal_Grove_(Zaishen_vanquish)" };
    public static readonly Quest TascasDemiseZaishenVanquish = new() { Id = 1289, Name = "Tascas Demise (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Tasca%27s_Demise_(Zaishen_vanquish)" };
    public static readonly Quest ResplendentMakuunZaishenVanquish = new() { Id = 1290, Name = "Resplendent Makuun (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Resplendent_Makuun_(Zaishen_vanquish)" };
    public static readonly Quest ReedBogZaishenVanquish = new() { Id = 1291, Name = "Reed Bog (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Reed_Bog_(Zaishen_vanquish)" };
    public static readonly Quest UnwakingWatersZaishenVanquish = new() { Id = 1292, Name = "Unwaking Waters (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Unwaking_Waters_(Zaishen_vanquish)" };
    public static readonly Quest StingrayStrandZaishenVanquish = new() { Id = 1293, Name = "Stingray Strand (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Stingray_Strand_(Zaishen_vanquish)" };
    public static readonly Quest SunwardMarchesZaishenVanquish = new() { Id = 1294, Name = "Sunward Marches (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Sunward_Marches_(Zaishen_vanquish)" };
    public static readonly Quest RegentValleyZaishenVanquish = new() { Id = 1295, Name = "Regent Valley (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Regent_Valley_(Zaishen_vanquish)" };
    public static readonly Quest WajjunBazaarZaishenVanquish = new() { Id = 1296, Name = "Wajjun Bazaar (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Wajjun_Bazaar_(Zaishen_vanquish)" };
    public static readonly Quest YatendiCanyonsZaishenVanquish = new() { Id = 1297, Name = "Yatendi Canyons (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Yatendi_Canyons_(Zaishen_vanquish)" };
    public static readonly Quest TwinSerpentLakesZaishenVanquish = new() { Id = 1298, Name = "Twin Serpent Lakes (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Twin_Serpent_Lakes_(Zaishen_vanquish)" };
    public static readonly Quest SageLandsZaishenVanquish = new() { Id = 1299, Name = "Sage Lands (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Sage_Lands_(Zaishen_vanquish)" };
    public static readonly Quest XaquangSkywayZaishenVanquish = new() { Id = 1300, Name = "Xaquang Skyway (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Xaquang_Skyway_(Zaishen_vanquish)" };
    public static readonly Quest ZehlonReachZaishenVanquish = new() { Id = 1301, Name = "Zehlon Reach (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Zehlon_Reach_(Zaishen_vanquish)" };
    public static readonly Quest TangleRootZaishenVanquish = new() { Id = 1302, Name = "Tangle Root (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Tangle_Root_(Zaishen_vanquish)" };
    public static readonly Quest SilverwoodZaishenVanquish = new() { Id = 1303, Name = "Silverwood (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Silverwood_(Zaishen_vanquish)" };
    public static readonly Quest ZenDaijunZaishenVanquish = new() { Id = 1304, Name = "Zen Daijun (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Zen_Daijun_(Zaishen_vanquish)" };
    public static readonly Quest TheAridSeaZaishenVanquish = new() { Id = 1305, Name = "The Arid Sea (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Arid_Sea_(Zaishen_vanquish)" };
    public static readonly Quest NahpuiQuarterZaishenVanquish = new() { Id = 1306, Name = "Nahpui Quarter (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Nahpui_Quarter_(Zaishen_vanquish)" };
    public static readonly Quest SkywardReachZaishenVanquish = new() { Id = 1307, Name = "Skyward Reach (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Skyward_Reach_(Zaishen_vanquish)" };
    public static readonly Quest TheScarZaishenVanquish = new() { Id = 1308, Name = "The Scar (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Scar_(Zaishen_vanquish)" };
    public static readonly Quest TheBlackCurtainZaishenVanquish = new() { Id = 1309, Name = "The Black Curtain (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Black_Curtain_(Zaishen_vanquish)" };
    public static readonly Quest PanjiangPeninsulaZaishenVanquish = new() { Id = 1310, Name = "Panjiang Peninsula (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Panjiang_Peninsula_(Zaishen_vanquish)" };
    public static readonly Quest SnakeDanceZaishenVanquish = new() { Id = 1311, Name = "Snake Dance (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Snake_Dance_(Zaishen_vanquish)" };
    public static readonly Quest TravelersValeZaishenVanquish = new() { Id = 1312, Name = "Travelers Vale (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Traveler%27s_Vale_(Zaishen_vanquish)" };
    public static readonly Quest TheBreachZaishenVanquish = new() { Id = 1313, Name = "The Breach (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Breach_(Zaishen_vanquish)" };
    public static readonly Quest LahtendaBogZaishenVanquish = new() { Id = 1314, Name = "Lahtenda Bog (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Lahtenda_Bog_(Zaishen_vanquish)" };
    public static readonly Quest SpearheadPeakZaishenVanquish = new() { Id = 1315, Name = "Spearhead Peak (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Spearhead_Peak_(Zaishen_vanquish)" };
    public static readonly Quest MountQinkaiZaishenVanquish = new() { Id = 1316, Name = "Mount Qinkai (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Mount_Qinkai_(Zaishen_vanquish)" };
    public static readonly Quest MargaCoastZaishenVanquish = new() { Id = 1317, Name = "Marga Coast (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Marga_Coast_(Zaishen_vanquish)" };
    public static readonly Quest MelandrusHopeZaishenVanquish = new() { Id = 1318, Name = "Melandrus Hope (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Melandru%27s_Hope_(Zaishen_vanquish)" };
    public static readonly Quest TheFallsZaishenVanquish = new() { Id = 1319, Name = "The Falls (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Falls_(Zaishen_vanquish)" };
    public static readonly Quest JokosDomainZaishenVanquish = new() { Id = 1320, Name = "Jokos Domain (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Joko%27s_Domain_(Zaishen_vanquish)" };
    public static readonly Quest VultureDriftsZaishenVanquish = new() { Id = 1321, Name = "Vulture Drifts (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Vulture_Drifts_(Zaishen_vanquish)" };
    public static readonly Quest WildernessofBahdzaZaishenVanquish = new() { Id = 1322, Name = "Wilderness of Bahdza (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Wilderness_of_Bahdza_(Zaishen_vanquish)" };
    public static readonly Quest TalmarkWildernessZaishenVanquish = new() { Id = 1323, Name = "Talmark Wilderness (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Talmark_Wilderness_(Zaishen_vanquish)" };
    public static readonly Quest VehtendiValleyZaishenVanquish = new() { Id = 1324, Name = "Vehtendi Valley (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Vehtendi_Valley_(Zaishen_vanquish)" };
    public static readonly Quest TalusChuteZaishenVanquish = new() { Id = 1325, Name = "Talus Chute (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Talus_Chute_(Zaishen_vanquish)" };
    public static readonly Quest MineralSpringsZaishenVanquish = new() { Id = 1326, Name = "Mineral Springs (Zaishen vanquish)", WikiUrl = "https://wiki.guildwars.com/wiki/Mineral_Springs_(Zaishen_vanquish)" };
    public static readonly Quest ThePathtoCombatRandomArena = new() { Id = 1329, Name = "The Path to Combat Random Arena", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Combat:_Random_Arena" };
    public static readonly Quest ThePathtoVictoryRandomArenas = new() { Id = 1330, Name = "The Path to Victory Random Arenas", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Victory:_Random_Arenas" };
    public static readonly Quest ThePathtoCombatCodexArena = new() { Id = 1331, Name = "The Path to Combat Codex Arena", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Combat:_Codex_Arena" };
    public static readonly Quest ThePathtoVictoryCodexArena = new() { Id = 1332, Name = "The Path to Victory Codex Arena", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Victory:_Codex_Arena" };
    public static readonly Quest ThePathtoCombatGuildBattles = new() { Id = 1333, Name = "The Path to Combat Guild Battles", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Combat:_Guild_Battles" };
    public static readonly Quest ThePathtoVictoryGuildBattles = new() { Id = 1334, Name = "The Path to Victory Guild Battles", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Victory:_Guild_Battles" };
    public static readonly Quest ThePathtoCombatHeroesAscent = new() { Id = 1335, Name = "The Path to Combat Heroes Ascent", WikiUrl = "https://wiki.guildwars.com/wiki/The_Path_to_Combat:_Heroes%27_Ascent" };
    public static readonly Quest CleansingBukdekByway = new() { Id = 1342, Name = "Cleansing Bukdek Byway", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Bukdek_Byway" };
    public static readonly Quest CleansingBukdekBywayHardMode = new() { Id = 1343, Name = "Cleansing Bukdek Byway (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Bukdek_Byway_(Hard_mode)" };
    public static readonly Quest CleansingShadowsPassage = new() { Id = 1344, Name = "Cleansing Shadows Passage", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Shadow%27s_Passage" };
    public static readonly Quest CleansingShadowsPassageHardMode = new() { Id = 1345, Name = "Cleansing Shadows Passage (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Shadow%27s_Passage_(Hard_mode)" };
    public static readonly Quest CleansingShenzunTunnels = new() { Id = 1346, Name = "Cleansing Shenzun Tunnels", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Shenzun_Tunnels" };
    public static readonly Quest CleansingShenzunTunnelsHardMode = new() { Id = 1347, Name = "Cleansing Shenzun Tunnels (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Shenzun_Tunnels_(Hard_mode)" };
    public static readonly Quest CleansingtheUndercity = new() { Id = 1348, Name = "Cleansing the Undercity", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_the_Undercity" };
    public static readonly Quest CleansingtheUndercityHardMode = new() { Id = 1349, Name = "Cleansing the Undercity (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_the_Undercity_(Hard_mode)" };
    public static readonly Quest CleansingtheSunjiangDistrict = new() { Id = 1350, Name = "Cleansing the Sunjiang District", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_the_Sunjiang_District" };
    public static readonly Quest CleansingtheSunjiangDistrictHardMode = new() { Id = 1351, Name = "Cleansing the Sunjiang District (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_the_Sunjiang_District_(Hard_mode)" };
    public static readonly Quest CleansingPongmeiValley = new() { Id = 1352, Name = "Cleansing Pongmei Valley", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Pongmei_Valley" };
    public static readonly Quest CleansingPongmeiValleyHardMode = new() { Id = 1353, Name = "Cleansing Pongmei Valley (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Pongmei_Valley_(Hard_mode)" };
    public static readonly Quest RescueatMinisterChosEstate = new() { Id = 1354, Name = "Rescue at Minister Chos Estate", WikiUrl = "https://wiki.guildwars.com/wiki/Rescue_at_Minister_Cho%27s_Estate" };
    public static readonly Quest RescueatMinisterChosEstateHardMode = new() { Id = 1355, Name = "Rescue at Minister Chos Estate (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Rescue_at_Minister_Cho%27s_Estate_(Hard_mode)" };
    public static readonly Quest CleansingHaijuLagoon = new() { Id = 1356, Name = "Cleansing Haiju Lagoon", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Haiju_Lagoon" };
    public static readonly Quest CleansingHaijuLagoonHardMode = new() { Id = 1357, Name = "Cleansing Haiju Lagoon (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Haiju_Lagoon_(Hard_mode)" };
    public static readonly Quest CleansingZenDaijun = new() { Id = 1358, Name = "Cleansing Zen Daijun", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Zen_Daijun" };
    public static readonly Quest CleansingZenDaijunHardMode = new() { Id = 1359, Name = "Cleansing Zen Daijun (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Zen_Daijun_(Hard_mode)" };
    public static readonly Quest CleansingRheasCrater = new() { Id = 1360, Name = "Cleansing Rheas Crater", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Rhea%27s_Crater" };
    public static readonly Quest CleansingRheasCraterHardMode = new() { Id = 1361, Name = "Cleansing Rheas Crater (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Rhea%27s_Crater_(Hard_mode)" };
    public static readonly Quest CleansingtheSilentSurf = new() { Id = 1362, Name = "Cleansing the Silent Surf", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_the_Silent_Surf" };
    public static readonly Quest CleansingtheSilentSurfHardMode = new() { Id = 1363, Name = "Cleansing the Silent Surf (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_the_Silent_Surf_(Hard_mode)" };
    public static readonly Quest CleansingMorostavTrail = new() { Id = 1364, Name = "Cleansing Morostav Trail", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Morostav_Trail" };
    public static readonly Quest CleansingMorostavTrailHardMode = new() { Id = 1365, Name = "Cleansing Morostav Trail (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cleansing_Morostav_Trail_(Hard_mode)" };
    public static readonly Quest InterceptingtheAmFah = new() { Id = 1366, Name = "Intercepting the Am Fah", WikiUrl = "https://wiki.guildwars.com/wiki/Intercepting_the_Am_Fah" };
    public static readonly Quest InterceptingtheAmFahHardMode = new() { Id = 1367, Name = "Intercepting the Am Fah (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Intercepting_the_Am_Fah_(Hard_mode)" };
    public static readonly Quest TrackingtheCorruption = new() { Id = 1368, Name = "Tracking the Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Tracking_the_Corruption" };
    public static readonly Quest TrackingtheCorruptionHardMode = new() { Id = 1369, Name = "Tracking the Corruption (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Tracking_the_Corruption_(Hard_mode)" };
    public static readonly Quest ArchitectofCorruption = new() { Id = 1370, Name = "Architect of Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Architect_of_Corruption" };
    public static readonly Quest ArchitectofCorruptionHardMode = new() { Id = 1371, Name = "Architect of Corruption (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Architect_of_Corruption_(Hard_mode)" };
    public static readonly Quest TheGangsofKaineng = new() { Id = 1372, Name = "The Gangs of Kaineng", WikiUrl = "https://wiki.guildwars.com/wiki/The_Gangs_of_Kaineng" };
    public static readonly Quest TheGangsofKainengHardMode = new() { Id = 1373, Name = "The Gangs of Kaineng (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Gangs_of_Kaineng_(Hard_mode)" };
    public static readonly Quest WhatWaitsinShadow = new() { Id = 1374, Name = "What Waits in Shadow", WikiUrl = "https://wiki.guildwars.com/wiki/What_Waits_in_Shadow" };
    public static readonly Quest WhatWaitsinShadowHardMode = new() { Id = 1375, Name = "What Waits in Shadow (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/What_Waits_in_Shadow_(Hard_mode)" };
    public static readonly Quest AChanceEncounter = new() { Id = 1376, Name = "A Chance Encounter", WikiUrl = "https://wiki.guildwars.com/wiki/A_Chance_Encounter" };
    public static readonly Quest AChanceEncounterHardMode = new() { Id = 1377, Name = "A Chance Encounter (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/A_Chance_Encounter_(Hard_mode)" };
    public static readonly Quest CanthaCourierCrisis = new() { Id = 1378, Name = "Cantha Courier Crisis", WikiUrl = "https://wiki.guildwars.com/wiki/Cantha_Courier_Crisis" };
    public static readonly Quest CanthaCourierCrisisHardMode = new() { Id = 1379, Name = "Cantha Courier Crisis (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Cantha_Courier_Crisis_(Hard_mode)" };
    public static readonly Quest LightMyFire = new() { Id = 1380, Name = "Light My Fire", WikiUrl = "https://wiki.guildwars.com/wiki/Light_My_Fire" };
    public static readonly Quest LightMyFireHardMode = new() { Id = 1381, Name = "Light My Fire (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Light_My_Fire_(Hard_mode)" };
    public static readonly Quest WhenKappaAttack = new() { Id = 1382, Name = "When Kappa Attack", WikiUrl = "https://wiki.guildwars.com/wiki/When_Kappa_Attack" };
    public static readonly Quest WhenKappaAttackHardMode = new() { Id = 1383, Name = "When Kappa Attack (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/When_Kappa_Attack_(Hard_mode)" };
    public static readonly Quest AFavorReturned = new() { Id = 1384, Name = "A Favor Returned", WikiUrl = "https://wiki.guildwars.com/wiki/A_Favor_Returned" };
    public static readonly Quest DeadlyCargo = new() { Id = 1385, Name = "Deadly Cargo", WikiUrl = "https://wiki.guildwars.com/wiki/Deadly_Cargo" };
    public static readonly Quest TradingBlows = new() { Id = 1386, Name = "Trading Blows", WikiUrl = "https://wiki.guildwars.com/wiki/Trading_Blows" };
    public static readonly Quest EverClosertotheEdge = new() { Id = 1387, Name = "Ever Closer to the Edge", WikiUrl = "https://wiki.guildwars.com/wiki/Ever_Closer_to_the_Edge" };
    public static readonly Quest ViolenceintheStreets = new() { Id = 1388, Name = "Violence in the Streets", WikiUrl = "https://wiki.guildwars.com/wiki/Violence_in_the_Streets" };
    public static readonly Quest AFavorReturnedHardMode = new() { Id = 1389, Name = "A Favor Returned (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/A_Favor_Returned_(Hard_mode)" };
    public static readonly Quest DeadlyCargoHardMode = new() { Id = 1390, Name = "Deadly Cargo (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Deadly_Cargo_(Hard_mode)" };
    public static readonly Quest TradingBlowsHardMode = new() { Id = 1391, Name = "Trading Blows (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Trading_Blows_(Hard_mode)" };
    public static readonly Quest EverClosertotheEdgeHardMode = new() { Id = 1392, Name = "Ever Closer to the Edge (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Ever_Closer_to_the_Edge_(Hard_mode)" };
    public static readonly Quest ViolenceintheStreetsHardMode = new() { Id = 1393, Name = "Violence in the Streets (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Violence_in_the_Streets_(Hard_mode)" };
    public static readonly Quest TheRescueAttempt = new() { Id = 1394, Name = "The Rescue Attempt", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rescue_Attempt" };
    public static readonly Quest TheRescueAttemptHardMode = new() { Id = 1395, Name = "The Rescue Attempt (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Rescue_Attempt_(Hard_mode)" };
    public static readonly Quest WarningtheAngchu = new() { Id = 1396, Name = "Warning the Angchu", WikiUrl = "https://wiki.guildwars.com/wiki/Warning_the_Angchu" };
    public static readonly Quest FreeBirds = new() { Id = 1397, Name = "Free Birds", WikiUrl = "https://wiki.guildwars.com/wiki/Free_Birds" };
    public static readonly Quest HonorableCombat = new() { Id = 1398, Name = "Honorable Combat", WikiUrl = "https://wiki.guildwars.com/wiki/Honorable_Combat" };
    public static readonly Quest ATreatysaTreaty = new() { Id = 1399, Name = "A Treatys a Treaty", WikiUrl = "https://wiki.guildwars.com/wiki/A_Treaty%27s_a_Treaty" };
    public static readonly Quest WarningtheAngchuHardMode = new() { Id = 1400, Name = "Warning the Angchu (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Warning_the_Angchu_(Hard_mode)" };
    public static readonly Quest FreeBirdsHardMode = new() { Id = 1401, Name = "Free Birds (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Free_Birds_(Hard_mode)" };
    public static readonly Quest HonorableCombatHardMode = new() { Id = 1402, Name = "Honorable Combat (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Honorable_Combat_(Hard_mode)" };
    public static readonly Quest ATreatysaTreatyHardMode = new() { Id = 1403, Name = "A Treatys a Treaty (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/A_Treaty%27s_a_Treaty_(Hard_mode)" };
    public static readonly Quest FamilyMatters = new() { Id = 1404, Name = "Family Matters", WikiUrl = "https://wiki.guildwars.com/wiki/Family_Matters" };
    public static readonly Quest FamilyMattersHardMode = new() { Id = 1405, Name = "Family Matters (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Family_Matters_(Hard_mode)" };
    public static readonly Quest EvilResidents = new() { Id = 1406, Name = "Evil Residents", WikiUrl = "https://wiki.guildwars.com/wiki/Evil_Residents" };
    public static readonly Quest EvilResidentsCodeHarmonica = new() { Id = 1407, Name = "Evil Residents Code Harmonica", WikiUrl = "https://wiki.guildwars.com/wiki/Evil_Residents_Code:_Harmonica" };
    public static readonly Quest FindingJinnai = new() { Id = 1408, Name = "Finding Jinnai", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_Jinnai" };
    public static readonly Quest FindingJinnaiHardMode = new() { Id = 1409, Name = "Finding Jinnai (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Finding_Jinnai_(Hard_mode)" };
    public static readonly Quest CallingAllThugs = new() { Id = 1410, Name = "Calling All Thugs", WikiUrl = "https://wiki.guildwars.com/wiki/Calling_All_Thugs" };
    public static readonly Quest RaidonKainengCenter = new() { Id = 1411, Name = "Raid on Kaineng Center", WikiUrl = "https://wiki.guildwars.com/wiki/Raid_on_Kaineng_Center" };
    public static readonly Quest ThereGoestheNeighborhood = new() { Id = 1412, Name = "There Goes the Neighborhood", WikiUrl = "https://wiki.guildwars.com/wiki/There_Goes_the_Neighborhood" };
    public static readonly Quest MinistryofOppression = new() { Id = 1413, Name = "Ministry of Oppression", WikiUrl = "https://wiki.guildwars.com/wiki/Ministry_of_Oppression" };
    public static readonly Quest CallingAllThugsHardMode = new() { Id = 1414, Name = "Calling All Thugs (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Calling_All_Thugs_(Hard_mode)" };
    public static readonly Quest RaidonKainengCenterHardMode = new() { Id = 1415, Name = "Raid on Kaineng Center (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Raid_on_Kaineng_Center_(Hard_mode)" };
    public static readonly Quest ThereGoestheNeighborhoodHardMode = new() { Id = 1416, Name = "There Goes the Neighborhood (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/There_Goes_the_Neighborhood_(Hard_mode)" };
    public static readonly Quest MinistryofOppressionHardMode = new() { Id = 1417, Name = "Ministry of Oppression (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Ministry_of_Oppression_(Hard_mode)" };
    public static readonly Quest RaidonShingJeaMonastery = new() { Id = 1418, Name = "Raid on Shing Jea Monastery", WikiUrl = "https://wiki.guildwars.com/wiki/Raid_on_Shing_Jea_Monastery" };
    public static readonly Quest RaidonShingJeaMonasteryHardMode = new() { Id = 1419, Name = "Raid on Shing Jea Monastery (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Raid_on_Shing_Jea_Monastery_(Hard_mode)" };
    public static readonly Quest TheFinalConfrontation = new() { Id = 1420, Name = "The Final Confrontation", WikiUrl = "https://wiki.guildwars.com/wiki/The_Final_Confrontation" };
    public static readonly Quest TheFinalConfrontationHardMode = new() { Id = 1421, Name = "The Final Confrontation (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/The_Final_Confrontation_(Hard_mode)" };
    public static readonly Quest MemoriesofPurity = new() { Id = 1422, Name = "Memories of Purity", WikiUrl = "https://wiki.guildwars.com/wiki/Memories_of_Purity" };
    public static readonly Quest VassalStates = new() { Id = 1423, Name = "Vassal States", WikiUrl = "https://wiki.guildwars.com/wiki/Vassal_States" };
    public static readonly Quest VassalStatesHardMode = new() { Id = 1424, Name = "Vassal States (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Vassal_States_(Hard_mode)" };
    public static readonly Quest RedemptionfortheLost = new() { Id = 1425, Name = "Redemption for the Lost", WikiUrl = "https://wiki.guildwars.com/wiki/Redemption_for_the_Lost" };
    public static readonly Quest RedemptionfortheLostHardMode = new() { Id = 1426, Name = "Redemption for the Lost (Hard mode)", WikiUrl = "https://wiki.guildwars.com/wiki/Redemption_for_the_Lost_(Hard_mode)" };
    public static readonly Quest WayfarersReverieTyria = new() { Id = 1429, Name = "Wayfarers Reverie Tyria", WikiUrl = "https://wiki.guildwars.com/wiki/Wayfarer%27s_Reverie:_Tyria" };
    public static readonly Quest WayfarersReverieCantha = new() { Id = 1430, Name = "Wayfarers Reverie Cantha", WikiUrl = "https://wiki.guildwars.com/wiki/Wayfarer%27s_Reverie:_Cantha" };
    public static readonly Quest WayfarersReverieElona = new() { Id = 1431, Name = "Wayfarers Reverie Elona", WikiUrl = "https://wiki.guildwars.com/wiki/Wayfarer%27s_Reverie:_Elona" };
    public static readonly Quest WayfarersReverieTheFarNorth = new() { Id = 1432, Name = "Wayfarers Reverie The Far North", WikiUrl = "https://wiki.guildwars.com/wiki/Wayfarer%27s_Reverie:_The_Far_North" };
    public static readonly Quest MadLittlePony = new() { Id = 1433, Name = "Mad Little Pony", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Little_Pony" };
    public static readonly Quest BetterYouThanMe = new() { Id = 1434, Name = "Better You Than Me", WikiUrl = "https://wiki.guildwars.com/wiki/Better_You_Than_Me" };
    public static readonly Quest DoubleYourPleasureandRUN = new() { Id = 1435, Name = "Double Your Pleasure and RUN!", WikiUrl = "https://wiki.guildwars.com/wiki/Double_Your_Pleasure_and_RUN!" };
    public static readonly Quest AHumblingGift = new() { Id = 1436, Name = "A Humbling Gift", WikiUrl = "https://wiki.guildwars.com/wiki/A_Humbling_Gift" };
    public static readonly Quest ThePolarDepressed = new() { Id = 1437, Name = "The Polar Depressed", WikiUrl = "https://wiki.guildwars.com/wiki/The_Polar_Depressed" };
    public static readonly Quest TakeMySisterPlease = new() { Id = 1438, Name = "Take My Sister, Please", WikiUrl = "https://wiki.guildwars.com/wiki/Take_My_Sister,_Please" };
    public static readonly Quest PilgrimagetotheHallofHeroes = new() { Id = 1439, Name = "Pilgrimage to the Hall of Heroes", WikiUrl = "https://wiki.guildwars.com/wiki/Pilgrimage_to_the_Hall_of_Heroes" };
    public static readonly Quest CrossingTheDesolation = new() { Id = 686, Name = "Crossing The Desolation", WikiUrl = "https://wiki.guildwars.com/wiki/Crossing_the_Desolation" };

    public static IEnumerable<Quest> Quests { get; } = new List<Quest>
    {
        TheAscalonSettlement,
        TheVillainyofGalrath,
        BanditTrouble,
        AGiftforJalisIronhammer,
        ToKrytaJourneysEnd,
        GravenImages,
        TheHotSpringsMurders,
        TheLastHog,
        TheLostPrincess,
        DutiesofaLionguard,
        TheRoyalPapers,
        AJourneyofRedemption,
        BlanketsfortheSettlers,
        OrrianExcavation,
        MalaquiresTest,
        ReversingtheSkales,
        TheUndeadHordes,
        LagaansOrdeal,
        LagaansGratitude,
        ReporttotheWhiteMantle,
        MerchantsPlea,
        ABelatedBetrothal,
        AncientSecrets,
        TheForgottenOnes,
        ForgottenWisdom,
        GhostlyVengeance,
        IntotheUnknown,
        TheMisplacedSword,
        SandsOfSouls,
        TheMesmersPath,
        TheNecromancersPath,
        TheElementalistsPath,
        TheMonksPath,
        TheWarriorsPath,
        TheRangersPath,
        WarPreparations,
        TheHuntersHorn,
        TheSupremacyofAir,
        RitesofRemembrance,
        LittleThomsBigCloak,
        TheVineyardProblem,
        BanditRaid,
        ATestofMarksmanship,
        ThePowerofBlood,
        TheRoguesReplacement,
        CharrintheCatacombs,
        CharrattheGate,
        TheAccursedPath,
        ThePoisonDevourer,
        DominationMagic,
        TheTrueKing,
        TheEggHunter,
        TheWormProblem,
        ThePrizeMoaBird,
        FurtherAdventures,
        TheWaywardWizard,
        AdventurewithanAlly,
        TheOrchard,
        AGiftforAlthea,
        GwensFlute,
        WarriorsChallenge,
        TitheforAshfordAbbey,
        UnsettlingRumors,
        ANewMesmerTrainer,
        ANewNecromancerTrainer,
        ANewElementalistTrainer,
        ANewMonkTrainer,
        ANewWarriorTrainer,
        ANewRangerTrainer,
        AcrosstheWall,
        PoorTenant,
        AMesmersBurden,
        TheNecromancersNovice,
        TheElementalistExperiment,
        AMonksMission,
        GrawlInvasion,
        TheRangersCompanion,
        ProtectionPrayers,
        OppositiontotheKing,
        ASecondProfession,
        MessagefromaFriend,
        MesmerTest,
        NecromancerTest,
        ElementalistTest,
        MonkTest,
        WarriorTest,
        RangerTest,
        TheBlessingsofBalthazar,
        UnnaturalGrowths,
        ThePathtoGlory,
        TroubleintheWoods,
        WhiteMantleWrathAHelpingHand,
        UrgentWarning,
        BloodAndSmoke,
        DroppingEaves,
        ABrothersFury,
        EyeForProfit,
        MysteriousMessage,
        ThePriceofSteel,
        WhiteMantleWrathDemagoguesVanguard,
        PassageThroughTheDarkRiver,
        CleartheChamber,
        ServantsofGrenth,
        UnwantedGuests,
        DemonAssassin,
        ImprisonedSpirits,
        TheFourHorsemen,
        TerrorwebQueen,
        EscortofSouls,
        RestoringGrenthsMonuments,
        WrathfulSpirits,
        CharrReinforcements,
        SymonsHistoryofAscalon,
        ExperimentalElixir,
        TryingTimes,
        UnnaturalCreatures,
        TheCharrPatrol,
        TheCharrStagingArea,
        ACureforRalena,
        CasualtyReport,
        DeathintheRuins,
        FallenSoldiers,
        OberansRage,
        AltheasAshes,
        HammerandAnvil,
        TheGeomancersTest,
        TheWayoftheGeomancer,
        ShalevsTask,
        ElementalKnowledge,
        TheDukesDaughter,
        BarradinsAdvance,
        FiresintheEast,
        FiresintheNorth,
        FrontierGateFugitives,
        BountyforChieftainMurg,
        HelpingthePeopleofAscalon,
        CitiesofAscalon,
        CountingtheFallen,
        ScavengersinOldAscalon,
        GarfazzBloodfang,
        TheKingsMessage,
        InMemoryofPaulus,
        ProtectingAscalon,
        TheLostMaster,
        TheAmbassadorsQuandary,
        RastinsRitual,
        AMissionofPeace,
        TheTroublesomeArtifact,
        BarradinsStand,
        ArmyLife,
        IntotheBreach,
        MilitaryMatters,
        TheRedCloakedDeserter,
        TheSiegeofPikenSquare,
        ReplacementHealers,
        RogennsDuel,
        RegentValleyDefense,
        MesmerizingtheEnemy,
        TheStolenArtifact,
        ScorchedEarth,
        SowingSeeds,
        GargoyleTrouble,
        EndangeredSpecies,
        TheMissingMelandruRelic,
        SuppliesfortheDuke,
        RuinsofSurmia,
        VanguardEquipment,
        DeliveraMessagetoMyWife,
        RecruitsforHollis,
        TheKrytanAmbassador,
        CollectCharrArmorSet,
        DefendtheWall,
        SlayRotscale,
        SlayStankReekfoul,
        RepelCharr,
        ScoutCharr,
        CollectGargoyleFangs,
        DefendDroknarsForge,
        TheRoadtoBorlisPass,
        ToKrytaTheIceCave,
        TheStoneSummitChampion,
        RenegadeNecromancer,
        TheDeserters,
        AHeartofIce,
        TheMissingArtisan,
        HelpingtheDwarves,
        HungryDevourer,
        OswaltsEpitaph,
        SecuringtheVale,
        MinaarsTrouble,
        IronHorseWarMachine,
        MinaarsWorry,
        StoneSummitBeastmasters,
        SlayFrostbite,
        TheHerosJourney,
        TheHerosChallenge,
        SeekingTheSeer,
        ShiverpeakStragglers,
        TheWayisBlocked,
        ToKrytaRefugees,
        TheWaywardMonk,
        IntotheBreach1,
        DefendtheTempleofWar,
        ArmyofDarkness,
        TheWailingLord,
        AGiftofGriffons,
        SlavesofMenzies,
        RestoretheTempleofWar,
        TheHunt,
        TheEternalForgemaster,
        ChampionsofOrr,
        TowerofStrength,
        TowerofCourage,
        FinalBlow,
        WisdomoftheDruids,
        FamilyTies,
        TheWeaverofNebo,
        WarPreparations1,
        WarPreparations2,
        WarPreparations3,
        WarPreparations4,
        WarPreparations5,
        WarPreparations6,
        TheFalseGods,
        KhobaytheBetrayer,
        CaravaninTrouble,
        DefendNorthKrytaProvince,
        DefendDenravi,
        TheLastDayDawns,
        TheTitanSource,
        ColdOne,
        TheForgeHeart,
        WrenchesInTheGears,
        UnspeakableUnknowable,
        ToSorrowsFurnace,
        NobleIntentions,
        NobleIntentionsPlanB,
        KilroyStonekin,
        SummitSlaves,
        GatheringResources,
        UnrulySlaves,
        FireintheHole,
        TascasReprise,
        SubversiveIdeas,
        PoliticalRamifications,
        TheFinalAssault,
        SeekOutHeadmasterLee,
        SeekOutHeadmasterKaa,
        SeekOutHeadmasterKuju,
        SeekOutHeadmasterVhang,
        SeekOutHeadmasterZhan,
        SeekOutHeadmasterAmara,
        SeekOutHeadmasterGreico,
        SeekOutHeadmasterQuin,
        LocateJinzo,
        LocateMeiLing,
        LocateRengKu,
        LocateRonsu,
        LocateSisterTai,
        LocateTalonSilverwing,
        LocateSujun,
        LocateProfessorGai,
        TrackDownPanaku,
        TrackDownLoSha,
        TrackDownSu,
        TrackDownKaiYing,
        TrackDownBrotherPeWan,
        TrackDownWengGha,
        TrackDownZho,
        TrackDownAngtheEphemeral,
        OpeningStrike,
        CleansingtheShrine,
        LittleCreatures,
        SparkofInterest,
        StaleMate,
        TalonsDuel,
        FreetheFur,
        MinionsGoneWild,
        DualStrike,
        LoShasGift,
        ReapingtheNaga,
        SparringElements,
        SeekingaCure,
        TheCropThieves,
        TheYetiHunt,
        ShackledSpirits,
        AnAudiencewithMasterTogoAssassin,
        AnAudiencewithMasterTogoMesmer,
        AnAudiencewithMasterTogoNecromancer,
        AnAudiencewithMasterTogoElementalist,
        AnAudiencewithMasterTogoMonk,
        AnAudiencewithMasterTogoWarrior,
        AnAudiencewithMasterTogoRanger,
        AnAudiencewithMasterTogoRitualist,
        SpeakwithHeadmasterLeeAssassin,
        SpeakwithHeadmasterKaaMesmer,
        SpeakwithHeadmasterKujuNecromancer,
        SpeakwithHeadmasterVhangElementalist,
        SpeakwithHeadmasterAmaraMonk,
        SpeakwithHeadmasterZhanWarrior,
        SpeakwithHeadmasterGreicoRanger,
        SpeakwithHeadmasterQuinRitualist,
        DefenseAgainstHexes,
        Disruption,
        SkillChaining,
        SnaringCourse,
        DisenchantmentCourse,
        ConditionRemoval,
        AdvancedDefenseTechniques,
        ElementalistInsignia,
        WarriorInsignia,
        MesmerInsignia,
        MonkInsignia,
        NecromancerInsignia,
        RangerInsignia,
        RitualistInsignia,
        AssassinInsignia,
        ChooseYourSecondaryProfessionFactions,
        AFormalIntroduction,
        TheSickenedVillage,
        TheRedFrog,
        TheKaguchiBrothers,
        TheBoxofIllusions,
        OldFriendsDarkTimes,
        PowerSurge,
        CleansingtheSteel,
        TheStoneoftheElements,
        AssassinationAttempt,
        LittleLostBear,
        ImperialAssistance,
        MedicalEmergency,
        RedTape,
        AssisttheGuards,
        StraighttotheTop,
        DrinkfromtheChaliceofCorruption,
        RefusetoDrink,
        TheSearchforaCure,
        SeekoutBrotherTosai,
        AnEndtoSuffering,
        WarningtheTengu,
        TheThreatGrows,
        JourneytotheMaster,
        TheRoadLessTraveled,
        LookingForTrouble,
        TotheRescueFactions,
        ToZenDaijun,
        LostTreasure,
        MantidHatchlings,
        AnUnwelcomeGuest,
        AMastersBurden,
        ToTahnnakaiTemple,
        IntotheWhirlpool,
        JourneytoHousezuHeltzer,
        JourneytoCavalon,
        NagaOil,
        StreetJustice,
        FindingTheOracle,
        ClosertotheStars,
        AppearanceoftheNaga,
        TheRiteofValor,
        DeathwithHonor,
        TheNamelessMasters,
        ThePathoftheZaishenElite,
        TheTeamTrials,
        KurzickSupplyLines,
        ScoutingFerndale,
        ScouttheCoast,
        SecuringEchovaldForest,
        DueloftheHouses,
        TheJadeArena,
        IntheArmyLuxon,
        ScoutingMaishangHills,
        LuxonSupplyLines,
        IntheArmyNowKurzick,
        BefriendingtheKurzicks,
        BefriendingtheLuxons,
        MayhemintheMarket,
        WelcometoCantha,
        TheDefendersoftheForest,
        AMeetingWiththeEmperor,
        TheCountsDaughter,
        StolenEggs,
        TheConvocation,
        JourneytotheWhirlpool,
        TakingBackthePalace,
        QuimangsLastStand,
        TheSiegeatTsumeiVillage,
        TogosUltimatum,
        RevengeoftheYeti,
        TheAgriculturist,
        TheCapturedSon,
        TheNagaSource,
        SentimentalTreasures,
        SkintheSnake,
        PestControl,
        TheThievingNanny,
        CityUnderAttack,
        BattleintheSewers,
        TheAncientForest,
        WickedWardens,
        SongandStone,
        ANewEscort,
        TheExperimentalWeaponsmith,
        MelodicGakiFlute,
        DiscordWallowLyre,
        RhythmDrinkerDrum,
        EnvoyoftheDredge,
        TempleoftheDredge,
        RevoltoftheDredge,
        TheHalcyonJob,
        AttacktheKurzicks,
        ProtecttheHalcyon,
        OutcastsintheQuarry,
        ChallengeofStrength,
        ArtisticEndeavors,
        TheBeakofDarkness,
        WardensOntheMarch,
        InvokingtheSaints,
        IfItWerentforBadLuck,
        NightRaiders,
        MessageonaDragonScale,
        MessagesMessagesEverywhere,
        TheZenosSquad,
        TheImpossibleSeaMonster,
        MinistersTest,
        CapturingtheOrrianTome,
        ATastyMorsel,
        WaywardGuide,
        ANewGuide,
        GettingEven,
        SticksandStones,
        ForgottenRetribution,
        DefendFortAspenwood,
        TheJadeQuarryKurzick,
        FortAspenwood,
        TheJadeQuarryLuxon,
        MhenlosRequest,
        TheBogBeastofBokku,
        ABeltPouch,
        CashCrops,
        FormingaParty,
        StudyBuddy,
        TheDragonHunter,
        MoreCowbell,
        ALetterHome,
        ReturnoftheYeti,
        TheChallenge,
        Captured,
        TheEmperorinPeril,
        ThePlotThickens,
        ItEndsHere,
        MastersofCorruption,
        TheDrunkenMaster,
        EliminatetheAmFah,
        EliminatetheJadeBrotherhood,
        ProblemSalving,
        GoingHome,
        MissingDaughter,
        TooHighaPrice,
        TheXunlaiAgent,
        LuxuryGoods,
        ChartingtheForests,
        ChartingtheJadeSea,
        StemmingtheTide,
        TheMissingCorpses,
        TheMissingLink,
        TheAfflictedGuard,
        SeekingShelter,
        HousingforRefugees,
        AMonstrousRevelation,
        TheShadowBlades,
        LambstotheSlaughter,
        SeaofGreenCanopyofStone,
        FeedtheHungry,
        PassingtheBuck,
        RazetheRoost,
        LastoftheBreed,
        RefusetheKing,
        TheGrowingThreat,
        ChaosinKryta,
        VanishingSpirits,
        Pilgrimage,
        RequiemforaBrain,
        UnderNewManagement,
        DestroytheUngratefulSlaves,
        AShowofForce,
        TempleoftheMonoliths,
        ReturnoftheUndeadKing,
        StrangeBedfellows,
        SheHungers,
        TheCouncilisCalled,
        DesperateMeasures,
        TheSearchforSurvivors,
        ASoundofAncientHorns,
        GardenChores,
        WhichWayDidHeGo,
        OneMansDream,
        PuzzlingParchment,
        ABriefIntroduction,
        PlagueofFrogs,
        BetweenaRock,
        BotanicalResearch,
        RallyThePrinces,
        AllsWellThatEndsWell,
        WarningKehanni,
        CallingtheOrder,
        GreedandRegret,
        PledgeoftheMerchantPrinces,
        AnOldMansPast,
        AnOldMansShame,
        TotheRescueNightfall,
        CofferofJoko,
        DataMining,
        TheMadnessofProphecy,
        ProtecttheLearned,
        ReclaimingtheTemple,
        TheSearchforEnlightenment,
        SummertimeforBokka,
        InDefenseofTheatre,
        WorstPerformanceEver,
        TheShowMustGoOn,
        ValleyoftheRifts,
        PopulationControl,
        DestroytheHarpies,
        GuardRescue,
        GiftoftheDjinn,
        Interception,
        OldFriends,
        ForYourEarsOnly,
        TheScourgeofVabbi,
        TheHangingGardener,
        ScorchedEarthNightfall,
        Extinction,
        InsatiableAppetite,
        BuildingtheBasePrisonersofWar,
        BuildingtheBaseTheInterrogation,
        BuildingtheBaseTheMeeting,
        TheToolsofWar,
        FeedingFrenzy,
        YouCanRun,
        CentaurConcerns,
        MirzasLastStand,
        BattleofTuraisProcession,
        AQuestionofMorality,
        ABushelofTrouble,
        TheGreatZehtuka,
        Eavesdropping,
        ALittleRecon,
        Hunted,
        TheGreatEscape,
        AndaHeroShallLeadThem,
        ToVabbi,
        CentaurBlackmail,
        MysteriousMessageNightfall,
        SecretsintheShadow,
        ToKillaDemon,
        ForaPrice,
        NoMeNoKormir,
        AThorninVareshsSide,
        TenderinganOffer,
        EstatePlanning,
        FamilyTiesNightfall,
        AMessageforJaneera,
        StrangeAllies,
        TheFoolhardyFather,
        TheYoungLadyVanishes,
        OldWomanRiver,
        AFoolsLuck,
        HerdstotheSlaughter,
        KosssElixir,
        DrakeinaCage,
        TroubledLands,
        AncestralAnguish,
        TotalCorruption,
        FishinaBarrel,
        MelonnisMeditations,
        WeirdWaters,
        APrescriptionforConscription,
        UndeadDefenders,
        TheColdTouchofthePast,
        RaisinganArmy,
        HeartorMindGardeninDanger,
        TheHallowedPoint,
        ADealsaDeal,
        HordeofDarkness,
        FamilySoul,
        HeartorMindRonjokinDanger,
        AssaultonBeknurHarbor,
        MoavuKaalAwakened,
        TheCyclonePalace,
        DownontheBayou,
        DoubleDogDare,
        AMessageHome,
        OneGoodTurnDeservesAnother,
        WhatDoYouDowithaDrunkenShauben,
        SecretPassage,
        ChooseYourSecondaryProfessionNightfall,
        CorsairVengeance,
        DefendersChoice,
        TheLoneRaider,
        PrimaryTraining,
        SecondaryTraining,
        AHiddenThreat,
        WanderedOffAgain,
        AngeroftheStoneFace,
        TheCultoftheStoneFace,
        StoneFacedOrders,
        ArmoredTransport,
        QualitySteel,
        MaterialGirl,
        SuwashthePirate,
        CatchoftheDay,
        Flamingoinggoinggone,
        ALeapofFaith,
        ZaishenElite,
        StudentJin,
        StudentSousuke,
        FeastofBokka,
        AGhostlyRequest,
        GhostsintheGraveyard,
        SkreeHatchlingSeason,
        AFriendinNeed,
        AFathersFate,
        SecondBorn,
        FirstBorn,
        ThirdBorn,
        IdentityTheft,
        NeedMoreStuff,
        ToDyeFor,
        MissingShipment,
        BlowOutSale,
        ScholarlyAffairs,
        LeavingaLegacy,
        TheHonorableGeneral,
        SignsandPortents,
        IsleoftheDead,
        BadTideRising,
        SpecialDelivery,
        BigNewsSmallPackage,
        FollowingtheTrail,
        TheIronTruth,
        TrialbyFire,
        WarPreparationsRecruitTraining,
        WarPreparationsWindandWater,
        WarPreparationsGhostReconnaissance,
        TheTimeisNigh,
        ALooseCannon,
        DiamondintheRough,
        MapTravelInventor,
        HoningyourSkills,
        VoicesintheNight,
        CorsairInvasion,
        APeacefulSolution,
        HogHunt,
        ADelayedDelivery,
        ATroublingTheory,
        AMysteriousMissive,
        APerplexingPlague,
        AStolenSpore,
        ProofofCourage,
        QuarryQuandry,
        QueenoftheQuarry,
        AStickyOperation,
        RisingSuns,
        ReenlistRojis,
        SkaleandMagicCompass,
        ToSeetheSights,
        ToAskForMore,
        MoneyontheSide,
        FuryofaGrievingHeart,
        Cryptology,
        StolenSupplies,
        LoyaltiltheEnd,
        APersonalVault,
        TradeRelations,
        RallytheRecruitsTutorial,
        IntoChahbekVillage,
        TaketheShortcutSkipTutorial,
        ADecayedMonument,
        GorensStuffPart1,
        AttackattheKodash,
        MelonniGoesRecruiting,
        SecuringChampionsDawn,
        TheApostate,
        //TODO: BreakingtheBroken, -- Commented out due to collision
        AFlickeringFlame,
        DismembertheTitans,
        CoverYourTracks,
        DarkGateway,
        TheyOnlyComeOutatNight,
        InvasionFromWithin,
        DrinkoftheGods,
        TheTroubledKeeper,
        OpenSeason,
        GoodDemonHunting,
        HoldingtheLine,
        AFleshyOperation,
        BadFortune,
        KnowThineEnemy,
        UnchartedTerritory,
        KormirsCrusade,
        AllAloneintheDarkness,
        PassingtheLuck,
        BlueprintoftheFall,
        EscapefromtheTorment,
        FadedMemory,
        AHistoryofViolence,
        QuiztheRecruits,
        KossGoesRecruiting,
        NeverFightAlone,
        CommandTraining,
        GorensStuffPart2,
        TheToysStory,
        RisingintheRanksMasterSergeant,
        RisingintheRanksFirstSpear,
        MiselatheMiddleChild,
        MututheOldestChild,
        JedurtheYoungestChild,
        NorgusNightfall,
        TheRoleofaLifetime,
        BrainsorBrawn,
        SunspearsinKryta,
        SunspearsinCantha,
        TerrorinTyria,
        PlagueinCantha,
        TheCommandPost,
        TheDejarinEstate,
        GainNorgu,
        GainGoren,
        GainMargrid,
        GainMasterofWhispers,
        GainJin,
        GainSousuke,
        GainOlias,
        GainZenmai,
        BattlePreparations,
        TheTimeisNigh1,
        DrakesonthePlain,
        CapturingtheSignetofCapture,
        ALandofHeroes,
        BreachingtheStygianVeil,
        FoundryBreakout,
        TheFoundryofFailedCreations,
        TheOddbodies,
        TheLastPatrol,
        TheMissingPatrol,
        IntotheFire,
        DeathbringerCompany,
        FindingaPurpose,
        TheCityofTorcqua,
        TheRiftsBetweenUs,
        TotheRescue,
        MallyxtheUnyielding,
        BroodWars,
        HowtheGrentchesStoleWintersday,
        SpreadingtheWintersdaySpirit,
        WhiteMist,
        TheGiftofGiving,
        SavetheReindeer,
        FindtheStolenPresents,
        AVeryGrentchieWintersday,
        TheGreatestSnowmanEverMade,
        YoureaMeanOneMrGrenth,
        InGrenthsDefense,
        ArmyWife,
        ImpressingtheGirl,
        TossingtheBouquet,
        MakingFriends,
        MissingSuitor,
        AvadontheRun,
        TheEternalDebate,
        TheContest,
        Consolation,
        ABurningDesire,
        TheBigBang,
        DouseYourEnthusiasm,
        HopelessRomantic,
        FireintheSky,
        TheKnightsWhoSayNian,
        JustMyLuck,
        AllforOneandOneforJustice,
        ChasingZenmai,
        OutofKourna,
        CountingtheFallen1,
        CommissioningaMemorial,
        AncientHistory,
        FindingaPurpose1,
        MoneyBackGuarantee,
        AgainsttheCharr,
        WarbandofBrothers,
        SearchfortheEbonVanguard,
        TheMissingVanguard,
        ScrambledReinforcements,
        TheRampagingYetis,
        TheShrineofMaat,
        AStrangeRequest,
        DarknessatKaitan,
        FlamesoftheBearSpirit,
        TempleoftheDamned,
        GiriffsWar,
        VeiledThreat,
        LostSouls,
        KathandraxsCrusher,
        CurseoftheNornbear,
        TheElusiveGolemancer,
        TrackingtheNornbear,
        TheAnvilofDragrimmar,
        ColdVengeance,
        TheMisanthropicJotunPrinciple,
        LabSpace,
        AGateTooFar,
        VisionoftheRavenSpirit,
        GeniusOperatedLivingEnchantedManifestation,
        DestructionsDepths,
        HeartoftheShiverpeaks,
        FindingtheBloodstone,
        FindingGadd,
        ALittleHelp,
        IFeeltheEarthMoveUnderCanthasFeet,
        HoleofIstan,
        WhatLiesBeneath,
        TheBeginningoftheEnd,
        TekkssWar,
        WorthyDeedsDoneDirtCheap,
        LittleWorkshopofHorrors,
        DredgingtheDepths,
        WatchitJiggle,
        AnythingYouCanDo,
        KraksCavalry,
        CrystalMethod,
        TheBladesEssence,
        TheArrowsPoint,
        DefendingtheBreach,
        ColdasIce,
        IntheServiceofRevenge,
        Truthseeker,
        AHuntersPride,
        ShadowsintheNight,
        LeaderofthePack,
        Round1Fight,
        Round2Fight,
        Round3Fight,
        Round4Fight,
        Round5Fight,
        FinalRoundFight,
        HeroTutorial,
        BearClubforWomen,
        TheGreatNornAlemoot,
        TheGreatNornAlemoot1,
        PrenuptialDisagreementFemale,
        PrenuptialDisagreementMale,
        CharrInvaders,
        BearClubforMen,
        KilroyStonekinsPunchOutExtravaganza,
        TheThrowdowninaNornTown,
        PunchtheClown,
        DestructiveResearch,
        TheDestroyerChallenge,
        FallingOut,
        ForgottenRelics,
        TheSmellofTitanintheMorning,
        BeVeryVeryQuiet,
        PlanA,
        FinalRoundChampionshipEditionFight,
        ManoaNorno,
        ATimeforHeroes,
        TheAssassinsRevenge,
        TheImplodingPast,
        FailuretoCommunicate,
        ServiceInDefenseoftheEye,
        ServicePracticeDummy,
        PolymockDefeatPlurgg,
        PolymockDefeatFonk,
        PolymockDefeatDuneTeardrinker,
        PolymockDefeatGrulhammerSilverfist,
        PolymockDefeatNecromancerVolumandus,
        PolymockDefeatMasterHoff,
        PolymockDefeatBlarp,
        PolymockDefeatYulma,
        SingleUglyGrawlSeeksSameforMindlessDestructioninAscalon,
        TheHuntingoftheCharr,
        Frogstomp,
        GivePeaceaChance,
        TheCipherofBalthazar,
        TheCipherofDwayna,
        TheCipherofGrenth,
        TheCipherofKormir,
        TheCipherofLyssa,
        TheCipherofMelandru,
        ThePathtoRevelations,
        TheBigUnfriendlyJotun,
        ForbiddenFruit,
        OBraveNewWorld,
        LostTreasureofKingHundar,
        DeeprunnersMap,
        AttackonJalissCamp,
        TheAsuraTrap,
        MothstoaFlame,
        InsidiousRemnants,
        TurningthePage,
        NorthernAllies,
        AssaultontheStronghold,
        BloodWashesBlood,
        TheDawnofRebellion,
        WhatMustBeDone,
        Nornhood,
        TheJusticiarsEnd,
        Haunted,
        AgainsttheDestroyers,
        FireandPain,
        TheKnowledgeableAsura,
        ThenandNowHereandThere,
        TheLastHierophant,
        KilroysPunchoutTournament,
        SpecialOpsDragonsGullet,
        SpecialOpsFlameTempleCorridor,
        SpecialOpsGrendichCourthouse,
        TheTenguAccords,
        TheBattleofJahai,
        TheFlightNorth,
        TheRiseoftheWhiteMantle,
        StraighttotheHeart,
        TheStrengthofSnow,
        DeactivatingPOX,
        DeactivatingNOX,
        DeactivatingROX,
        ZinnsTask,
        TheThreeWiseNorn,
        CharrbroiledPlans,
        SnowballDominance,
        WintersdayCheer,
        TheGreatNorthernWallZaishenQuest,
        FortRanikZaishenQuest,
        RuinsofSurmiaZaishenQuest,
        NolaniAcademyZaishenQuest,
        BorlisPassZaishenQuest,
        TheFrostGateZaishenQuest,
        GatesofKrytaZaishenQuest,
        DAlessioSeaboardZaishenQuest,
        DivinityCoastZaishenQuest,
        TheWildsZaishenQuest,
        BloodstoneFenZaishenQuest,
        AuroraGladeZaishenQuest,
        RiversideProvinceZaishenQuest,
        SanctumCayZaishenQuest,
        DunesofDespairZaishenQuest,
        ThirstyRiverZaishenQuest,
        ElonaReachZaishenQuest,
        AuguryRockZaishenQuest,
        TheDragonsLairZaishenQuest,
        IceCavesofSorrowZaishenQuest,
        IronMinesofMoladuneZaishenQuest,
        ThunderheadKeepZaishenQuest,
        RingofFireZaishenQuest,
        AbaddonsMouthZaishenQuest,
        HellsPrecipiceZaishenQuest,
        ZenDaijunZaishenQuest,
        VizunahSquareZaishenQuest,
        NahpuiQuarterZaishenQuest,
        TahnnakaiTempleZaishenQuest,
        ArborstoneZaishenQuest,
        BoreasSeabedZaishenQuest,
        SunjiangDistrictZaishenQuest,
        TheEternalGroveZaishenQuest,
        UnwakingWatersZaishenQuest,
        GyalaHatcheryZaishenQuest,
        RaisuPalaceZaishenQuest,
        ImperialSanctumZaishenQuest,
        ChahbekVillageZaishenQuest,
        JokanurDiggingsZaishenQuest,
        BlacktideDenZaishenQuest,
        ConsulateDocksZaishenQuest,
        VentaCemeteryZaishenQuest,
        KodonurCrossroadsZaishenQuest,
        RilohnRefugeZaishenQuest,
        ModdokCreviceZaishenQuest,
        TiharkOrchardZaishenQuest,
        DzagonurBastionZaishenQuest,
        DashaVestibuleZaishenQuest,
        GrandCourtofSebelkehZaishenQuest,
        JennursHordeZaishenQuest,
        NunduBayZaishenQuest,
        GateofDesolationZaishenQuest,
        RuinsofMorahZaishenQuest,
        GateofPainZaishenQuest,
        GateofMadnessZaishenQuest,
        AbaddonsGateZaishenQuest,
        FindingtheBloodstoneZaishenQuest,
        TheElusiveGolemancerZaishenQuest,
        GOLEMZaishenQuest,
        AgainsttheCharrZaishenQuest,
        WarbandofBrothersZaishenQuest,
        AssaultontheStrongholdZaishenQuest,
        CurseoftheNornbearZaishenQuest,
        BloodWashesBloodZaishenQuest,
        AGateTooFarZaishenQuest,
        DestructionsDepthsZaishenQuest,
        ATimeforHeroesZaishenQuest,
        VerataZaishenQuest,
        RotscaleZaishenQuest,
        TheIronForgemanZaishenQuest,
        TheDarknessesZaishenQuest,
        KepkhetMarrowfeastZaishenQuest,
        HarnandMaxineColdstoneZaishenQuest,
        KanaxaiZaishenQuest,
        UrgozZaishenQuest,
        ChungtheAttunedZaishenQuest,
        RoyenBeastkeeperZaishenQuest,
        KunvieFirewingZaishenQuest,
        MungriMagicboxZaishenQuest,
        ArborEarthcallZaishenQuest,
        MohbyWindbeakZaishenQuest,
        SsunsBlessedofDwaynaZaishenQuest,
        GhialtheBoneDancerZaishenQuest,
        QuansongSpiritspeakZaishenQuest,
        BaubaoWavewrathZaishenQuest,
        JarimiyatheUnmercifulZaishenQuest,
        CommanderWahliZaishenQuest,
        DroajamMageoftheSandsZaishenQuest,
        JedehtheMightyZaishenQuest,
        KorshektheImmolatedZaishenQuest,
        AdmiralKantohZaishenQuest,
        TheBlackBeastofArrghZaishenQuest,
        LordJadothZaishenQuest,
        TheStygianUnderlordsZaishenQuest,
        TheStygianLordsZaishenQuest,
        TheGreaterDarknessZaishenQuest,
        IlsundurLordofFireZaishenQuest,
        RragarManeaterZaishenQuest,
        MurakaiLadyoftheNightZaishenQuest,
        PrismaticOozeZaishenQuest,
        HavokSoulwailZaishenQuest,
        FrostmawtheKinslayerZaishenQuest,
        RemnantofAntiquitiesZaishenQuest,
        PlagueofDestructionZaishenQuest,
        ZoldarktheUnholyZaishenQuest,
        KhabuusZaishenQuest,
        ZhimMonnsZaishenQuest,
        EldritchEttinZaishenQuest,
        FendiNinZaishenQuest,
        TPSRegulatorGolemZaishenQuest,
        ArachniZaishenQuest,
        ForgewightZaishenQuest,
        SelvetarmZaishenQuest,
        JusticiarThommisZaishenQuest,
        RandStormweaverZaishenQuest,
        DuncantheBlackZaishenQuest,
        FronisIrontoeZaishenQuest,
        MagmusZaishenQuest,
        MolotovRocktailZaishenQuest,
        NulfastuEarthboundZaishenQuest,
        PywatttheSwiftZaishenQuest,
        JoffstheMitigatorZaishenQuest,
        MobrinLordoftheMarshZaishenQuest,
        BorrguusBlisterbarkZaishenQuest,
        FozzyYeoryiosZaishenQuest,
        MyishLadyoftheLakeZaishenQuest,
        FenrirZaishenQuest,
        VengefulAatxeZaishenQuest,
        TheFourHorsemenZaishenQuest,
        ChargedBlacknessZaishenQuest,
        DragonLichZaishenQuest,
        PriestofMenziesZaishenQuest,
        LordKhobayZaishenQuest,
        RandomArenaZaishenQuest,
        HeroBattlesZaishenQuest,
        FortAspenwoodZaishenQuest,
        JadeQuarryZaishenQuest,
        AllianceBattlesZaishenQuest,
        GuildVersusGuildZaishenQuest,
        HeroesAscentZaishenQuest,
        RandomArenaZaishenQuest1,
        HeroBattles,
        FortAspenwoodZaishenQuest1,
        TheJadeQuarry,
        AllianceBattlesZaishenQuest1,
        GuildVersusGuildZaishenQuest1,
        HeroesAscentZaishenQuest1,
        RandomArenaZaishenQuest2,
        FortAspenwoodZaishenQuest2,
        JadeQuarryZaishenQuest1,
        GuildVersusGuildZaishenQuest2,
        RandomArenaZaishenQuest3,
        TeamArenaZaishenQuest,
        HeroBattlesZaishenQuest1,
        JadeQuarryZaishenQuest2,
        AllianceBattlesZaishenQuest2,
        GuildVersusGuildZaishenQuest3,
        HeroesAscentZaishenQuest2,
        MinisterChosEstateZaishenQuest,
        SomethingWickedThisWayComes,
        DontFeartheReapers,
        StemmingtheSkeletalTide,
        EveryBitHelps,
        TheWaitingGame,
        CodexArenaZaishenQuest,
        CodexArenaZaishenQuest1,
        CodexArenaZaishenQuest2,
        TheNightmanCometh,
        WantedInquisitorLashona,
        WantedInquisitorLovisa,
        WantedInquisitorBauer,
        WantedJusticiarKasandra,
        WantedJusticiarAmilyn,
        WantedJusticiarSevaan,
        WantedValistheRampant,
        WantedMaximiliantheMeticulous,
        WantedSarniatheRedHanded,
        WantedDestortheTruthSeeker,
        WantedSelenastheBlunt,
        WantedBarthimustheProvident,
        WantedCerris,
        WantedCarnaktheHungry,
        WantedInsatiableVakar,
        WantedAmalektheUnmerciful,
        WantedJohtheHostile,
        WantedGrevestheOverbearing,
        WantedCalamitous,
        WantedLevtheCondemned,
        WantedVesstheDisputant,
        WantedJusticiarKimii,
        WantedZalntheJaded,
        WantedJusticiarMarron,
        RiversideAssassination,
        ALittleHelpFromAbove,
        TempleoftheIntolerable,
        MusteringaResponse,
        TheBattleforLionsArch,
        HeirloomsoftheMadKing,
        CommandeeringaMortalVessel,
        AnIngeniousPlan,
        TilDeathDoUsPart,
        OpentheFloodGatesofDeath,
        TheKillingJoke,
        NornCatering,
        TheArrowhead,
        TheTarnishedEmblem,
        TheBrokenSword,
        TheMantlesGuise,
        AuspiciousBeginnings,
        AVengeanceofBlades,
        ShadowsintheJungle,
        Rise,
        Reunion,
        TheWedding,
        TheWarinKryta,
        AsuranAllies,
        EbonVanguardAllies,
        OperationCrushSpirits,
        FightinginaWinterWonderland,
        VanguardBountyBlazefiendGriefblade,
        VanguardBountyCountessNadya,
        VanguardBountyUtiniWupwup,
        VanguardRescueSavetheAscalonianNoble,
        VanguardRescueFarmerHamnet,
        VanguardRescueFootmanTate,
        VanguardAnnihilationBandits,
        VanguardAnnihilationUndead,
        VanguardAnnihilationCharr,
        AnvilRockZaishenVanquish,
        ArborstoneZaishenVanquish,
        WitmansFollyZaishenVanquish,
        ArkjokWardZaishenVanquish,
        AscalonFoothillsZaishenVanquish,
        BahdokCavernsZaishenVanquish,
        CursedLandsZaishenVanquish,
        AlcaziaTangleZaishenVanquish,
        ArchipelagosZaishenVanquish,
        EasternFrontierZaishenVanquish,
        DejarinEstateZaishenVanquish,
        WatchtowerCoastZaishenVanquish,
        ArborBayZaishenVanquish,
        BarbarousShoreZaishenVanquish,
        DeldrimorBowlZaishenVanquish,
        BoreasSeabedZaishenVanquish,
        CliffsofDohjokZaishenVanquish,
        DiessaLowlandsZaishenVanquish,
        BukdekBywayZaishenVanquish,
        BjoraMarchesZaishenVanquish,
        CrystalOverlookZaishenVanquish,
        DivinersAscentZaishenVanquish,
        DaladaUplandsZaishenVanquish,
        DrazachThicketZaishenVanquish,
        FahranurtheFirstCityZaishenVanquish,
        DragonsGulletZaishenVanquish,
        FerndaleZaishenVanquish,
        ForumHighlandsZaishenVanquish,
        DreadnoughtsDriftZaishenVanquish,
        DrakkarLakeZaishenVanquish,
        DryTopZaishenVanquish,
        TearsoftheFallenZaishenVanquish,
        GyalaHatcheryZaishenVanquish,
        EttinsBackZaishenVanquish,
        GandaratheMoonFortressZaishenVanquish,
        GrothmarWardownsZaishenVanquish,
        FlameTempleCorridorZaishenVanquish,
        HaijuLagoonZaishenVanquish,
        FrozenForestZaishenVanquish,
        GardenofSeborhinZaishenVanquish,
        GrenthsFootprintZaishenVanquish,
        JayaBluffsZaishenVanquish,
        HoldingsofChokhinZaishenVanquish,
        IceCliffChasmsZaishenVanquish,
        GriffonsMouthZaishenVanquish,
        KinyaProvinceZaishenVanquish,
        IssnurIslesZaishenVanquish,
        JagaMoraineZaishenVanquish,
        IceFloeZaishenVanquish,
        MaishangHillsZaishenVanquish,
        JahaiBluffsZaishenVanquish,
        RivenEarthZaishenVanquish,
        IcedomeZaishenVanquish,
        MinisterChosEstateZaishenVanquish,
        MehtaniKeysZaishenVanquish,
        SacnothValleyZaishenVanquish,
        IronHorseMineZaishenVanquish,
        MorostavTrailZaishenVanquish,
        PlainsofJarinZaishenVanquish,
        SparkflySwampZaishenVanquish,
        KessexPeakZaishenVanquish,
        MourningVeilFallsZaishenVanquish,
        TheAlkaliPanZaishenVanquish,
        VarajarFellsZaishenVanquish,
        LornarsPassZaishenVanquish,
        PongmeiValleyZaishenVanquish,
        TheFloodplainofMahnkelonZaishenVanquish,
        VerdantCascadesZaishenVanquish,
        MajestysRestZaishenVanquish,
        RaisuPalaceZaishenVanquish,
        TheHiddenCityofAhdashimZaishenVanquish,
        RheasCraterZaishenVanquish,
        MamnoonLagoonZaishenVanquish,
        ShadowsPassageZaishenVanquish,
        TheMirrorofLyssZaishenVanquish,
        SaoshangTrailZaishenVanquish,
        NeboTerraceZaishenVanquish,
        ShenzunTunnelsZaishenVanquish,
        TheRupturedHeartZaishenVanquish,
        SaltFlatsZaishenVanquish,
        NorthKrytaProvinceZaishenVanquish,
        SilentSurfZaishenVanquish,
        TheShatteredRavinesZaishenVanquish,
        ScoundrelsRiseZaishenVanquish,
        OldAscalonZaishenVanquish,
        SunjiangDistrictZaishenVanquish,
        TheSulfurousWastesZaishenVanquish,
        MagusStonesZaishenVanquish,
        PerditionRockZaishenVanquish,
        SunquaValeZaishenVanquish,
        TuraisProcessionZaishenVanquish,
        NorrhartDomainsZaishenVanquish,
        PockmarkFlatsZaishenVanquish,
        TahnnakaiTempleZaishenVanquish,
        VehjinMinesZaishenVanquish,
        PoisonedOutcropsZaishenVanquish,
        ProphetsPathZaishenVanquish,
        TheEternalGroveZaishenVanquish,
        TascasDemiseZaishenVanquish,
        ResplendentMakuunZaishenVanquish,
        ReedBogZaishenVanquish,
        UnwakingWatersZaishenVanquish,
        StingrayStrandZaishenVanquish,
        SunwardMarchesZaishenVanquish,
        RegentValleyZaishenVanquish,
        WajjunBazaarZaishenVanquish,
        YatendiCanyonsZaishenVanquish,
        TwinSerpentLakesZaishenVanquish,
        SageLandsZaishenVanquish,
        XaquangSkywayZaishenVanquish,
        ZehlonReachZaishenVanquish,
        TangleRootZaishenVanquish,
        SilverwoodZaishenVanquish,
        ZenDaijunZaishenVanquish,
        TheAridSeaZaishenVanquish,
        NahpuiQuarterZaishenVanquish,
        SkywardReachZaishenVanquish,
        TheScarZaishenVanquish,
        TheBlackCurtainZaishenVanquish,
        PanjiangPeninsulaZaishenVanquish,
        SnakeDanceZaishenVanquish,
        TravelersValeZaishenVanquish,
        TheBreachZaishenVanquish,
        LahtendaBogZaishenVanquish,
        SpearheadPeakZaishenVanquish,
        MountQinkaiZaishenVanquish,
        MargaCoastZaishenVanquish,
        MelandrusHopeZaishenVanquish,
        TheFallsZaishenVanquish,
        JokosDomainZaishenVanquish,
        VultureDriftsZaishenVanquish,
        WildernessofBahdzaZaishenVanquish,
        TalmarkWildernessZaishenVanquish,
        VehtendiValleyZaishenVanquish,
        TalusChuteZaishenVanquish,
        MineralSpringsZaishenVanquish,
        ThePathtoCombatRandomArena,
        ThePathtoVictoryRandomArenas,
        ThePathtoCombatCodexArena,
        ThePathtoVictoryCodexArena,
        ThePathtoCombatGuildBattles,
        ThePathtoVictoryGuildBattles,
        ThePathtoCombatHeroesAscent,
        CleansingBukdekByway,
        CleansingBukdekBywayHardMode,
        CleansingShadowsPassage,
        CleansingShadowsPassageHardMode,
        CleansingShenzunTunnels,
        CleansingShenzunTunnelsHardMode,
        CleansingtheUndercity,
        CleansingtheUndercityHardMode,
        CleansingtheSunjiangDistrict,
        CleansingtheSunjiangDistrictHardMode,
        CleansingPongmeiValley,
        CleansingPongmeiValleyHardMode,
        RescueatMinisterChosEstate,
        RescueatMinisterChosEstateHardMode,
        CleansingHaijuLagoon,
        CleansingHaijuLagoonHardMode,
        CleansingZenDaijun,
        CleansingZenDaijunHardMode,
        CleansingRheasCrater,
        CleansingRheasCraterHardMode,
        CleansingtheSilentSurf,
        CleansingtheSilentSurfHardMode,
        CleansingMorostavTrail,
        CleansingMorostavTrailHardMode,
        InterceptingtheAmFah,
        InterceptingtheAmFahHardMode,
        TrackingtheCorruption,
        TrackingtheCorruptionHardMode,
        ArchitectofCorruption,
        ArchitectofCorruptionHardMode,
        TheGangsofKaineng,
        TheGangsofKainengHardMode,
        WhatWaitsinShadow,
        WhatWaitsinShadowHardMode,
        AChanceEncounter,
        AChanceEncounterHardMode,
        CanthaCourierCrisis,
        CanthaCourierCrisisHardMode,
        LightMyFire,
        LightMyFireHardMode,
        WhenKappaAttack,
        WhenKappaAttackHardMode,
        AFavorReturned,
        DeadlyCargo,
        TradingBlows,
        EverClosertotheEdge,
        ViolenceintheStreets,
        AFavorReturnedHardMode,
        DeadlyCargoHardMode,
        TradingBlowsHardMode,
        EverClosertotheEdgeHardMode,
        ViolenceintheStreetsHardMode,
        TheRescueAttempt,
        TheRescueAttemptHardMode,
        WarningtheAngchu,
        FreeBirds,
        HonorableCombat,
        ATreatysaTreaty,
        WarningtheAngchuHardMode,
        FreeBirdsHardMode,
        HonorableCombatHardMode,
        ATreatysaTreatyHardMode,
        FamilyMatters,
        FamilyMattersHardMode,
        EvilResidents,
        EvilResidentsCodeHarmonica,
        FindingJinnai,
        FindingJinnaiHardMode,
        CallingAllThugs,
        RaidonKainengCenter,
        ThereGoestheNeighborhood,
        MinistryofOppression,
        CallingAllThugsHardMode,
        RaidonKainengCenterHardMode,
        ThereGoestheNeighborhoodHardMode,
        MinistryofOppressionHardMode,
        RaidonShingJeaMonastery,
        RaidonShingJeaMonasteryHardMode,
        TheFinalConfrontation,
        TheFinalConfrontationHardMode,
        MemoriesofPurity,
        VassalStates,
        VassalStatesHardMode,
        RedemptionfortheLost,
        RedemptionfortheLostHardMode,
        WayfarersReverieTyria,
        WayfarersReverieCantha,
        WayfarersReverieElona,
        WayfarersReverieTheFarNorth,
        MadLittlePony,
        BetterYouThanMe,
        DoubleYourPleasureandRUN,
        AHumblingGift,
        ThePolarDepressed,
        TakeMySisterPlease,
        PilgrimagetotheHallofHeroes,
        CrossingTheDesolation
    };

    public static bool TryParse(int id, out Quest quest)
    {
        quest = Quests.Where(quest => quest.Id == id).FirstOrDefault()!;
        if (quest is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Quest quest)
    {
        quest = Quests.Where(quest => quest.Name == name).FirstOrDefault()!;
        if (quest is null)
        {
            return false;
        }

        return true;
    }
    public static Quest Parse(int id)
    {
        if (TryParse(id, out var quest) is false)
        {
            throw new InvalidOperationException($"Could not find a quest with id {id}");
        }

        return quest;
    }
    public static Quest Parse(string name)
    {
        if (TryParse(name, out var quest) is false)
        {
            throw new InvalidOperationException($"Could not find a quest with name {name}");
        }
        
        return quest;
    }

    private Quest()
    {
    }
}
