using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

//TODO: Remove duplicates and inconsistencies
//TODO: Add Phropecies and EOTN npcs
public sealed class Npc
{
    public static readonly Npc Unknown = new() { Id = 0, Name = "Unknown" };
    public static readonly Npc ZaishenRepresentative = new() { Id = 111, Name = "Zaishen Representative", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_Representative" };
    public static readonly Npc CanthanAmbassador = new() { Id = 216, Name = "Canthan Ambassador", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Ambassador" };
    public static readonly Npc PriestofBalthazar = new() { Id = 218, Name = "Priest of Balthazar", WikiUrl = "https://wiki.guildwars.com/wiki/Priest_of_Balthazar" };
    public static readonly Npc Tolkano = new() { Id = 219, Name = "Tolkano", WikiUrl = "https://wiki.guildwars.com/wiki/Tolkano" };
    public static readonly Npc XunlaiAgentHonlo = new() { Id = 220, Name = "Xunlai Agent Honlo", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Agent_Honlo" };
    public static readonly Npc XunlaiAgent = new() { Id = 221, Name = "Xunlai Agent", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Agent" };
    public static readonly Npc Hayate = new() { Id = 223, Name = "Hayate", WikiUrl = "https://wiki.guildwars.com/wiki/Hayate" };
    public static readonly Npc Rabbit = new() { Id = 1396, Name = "Rabbit", WikiUrl = "https://wiki.guildwars.com/wiki/Rabbit" };
    public static readonly Npc DeckhandJakob = new() { Id = 1950, Name = "Deckhand Jakob", WikiUrl = "https://wiki.guildwars.com/wiki/Deckhand_Jakob" };
    public static readonly Npc Bryan = new() { Id = 1955, Name = "Bryan", WikiUrl = "https://wiki.guildwars.com/wiki/Bryan" };
    public static readonly Npc CaptainAnder = new() { Id = 1955, Name = "Captain Ander", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Ander" };
    public static readonly Npc Diane = new() { Id = 1956, Name = "Diane", WikiUrl = "https://wiki.guildwars.com/wiki/Diane" };
    public static readonly Npc Armian = new() { Id = 1966, Name = "Armian", WikiUrl = "https://wiki.guildwars.com/wiki/Armian" };
    public static readonly Npc Durmand = new() { Id = 1987, Name = "Durmand", WikiUrl = "https://wiki.guildwars.com/wiki/Durmand" };
    public static readonly Npc CaptainJoran = new() { Id = 2012, Name = "Captain Joran", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Joran" };
    public static readonly Npc Ludor = new() { Id = 2033, Name = "Ludor", WikiUrl = "https://wiki.guildwars.com/wiki/Ludor" };
    public static readonly Npc Tuomas = new() { Id = 2062, Name = "Tuomas", WikiUrl = "https://wiki.guildwars.com/wiki/Tuomas" };
    public static readonly Npc MarinerKevan = new() { Id = 2066, Name = "Mariner Kevan", WikiUrl = "https://wiki.guildwars.com/wiki/Mariner_Kevan" };
    public static readonly Npc KurzickElementalistRecruit = new() { Id = 2789, Name = "Kurzick Elementalist Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Elementalist_Recruit" };
    public static readonly Npc FireGolem = new() { Id = 2791, Name = "Fire Golem", WikiUrl = "https://wiki.guildwars.com/wiki/Fire_Golem" };
    public static readonly Npc Crane = new() { Id = 2949, Name = "Crane", WikiUrl = "https://wiki.guildwars.com/wiki/Crane" };
    public static readonly Npc Tiger = new() { Id = 2950, Name = "Tiger", WikiUrl = "https://wiki.guildwars.com/wiki/Tiger" };
    public static readonly Npc Lurker = new() { Id = 2951, Name = "Lurker", WikiUrl = "https://wiki.guildwars.com/wiki/Lurker" };
    public static readonly Npc ReefLurker = new() { Id = 2952, Name = "Reef Lurker", WikiUrl = "https://wiki.guildwars.com/wiki/Reef_Lurker" };
    public static readonly Npc WhiteTiger = new() { Id = 2953, Name = "White Tiger", WikiUrl = "https://wiki.guildwars.com/wiki/White_Tiger" };
    public static readonly Npc Phoenix = new() { Id = 2954, Name = "Phoenix", WikiUrl = "https://wiki.guildwars.com/wiki/Phoenix" };
    public static readonly Npc ElderReefLurker = new() { Id = 2955, Name = "Elder Reef Lurker", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Reef_Lurker" };
    public static readonly Npc Hector = new() { Id = 2955, Name = "Hector", WikiUrl = "https://wiki.guildwars.com/wiki/Hector" };
    public static readonly Npc PetElderReefLurker = new() { Id = 2955, Name = "Pet - Elder Reef Lurker", WikiUrl = "https://wiki.guildwars.com/wiki/Pet_-_Elder_Reef_Lurker" };
    public static readonly Npc BlackMoa = new() { Id = 2957, Name = "Black Moa", WikiUrl = "https://wiki.guildwars.com/wiki/Black_Moa" };
    public static readonly Npc ElderWhiteTiger = new() { Id = 2960, Name = "Elder White Tiger", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_White_Tiger" };
    public static readonly Npc ElderCrane = new() { Id = 2965, Name = "Elder Crane", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Crane" };
    public static readonly Npc ElderTiger = new() { Id = 2970, Name = "Elder Tiger", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Tiger" };
    public static readonly Npc ElderReefLurker1 = new() { Id = 2975, Name = "Elder Reef Lurker", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Reef_Lurker" };
    public static readonly Npc Klaus = new() { Id = 2989, Name = "Klaus", WikiUrl = "https://wiki.guildwars.com/wiki/Klaus" };
    public static readonly Npc KurzickMesmerRecruit = new() { Id = 2989, Name = "Kurzick Mesmer Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Mesmer_Recruit" };
    public static readonly Npc KurzickMonkRecruit = new() { Id = 2989, Name = "Kurzick Monk Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Monk_Recruit" };
    public static readonly Npc KurzickRangerRecruit = new() { Id = 2989, Name = "Kurzick Ranger Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Ranger_Recruit" };
    public static readonly Npc KurzickWarriorRecruit = new() { Id = 2989, Name = "Kurzick Warrior Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Warrior_Recruit" };
    public static readonly Npc Leiber = new() { Id = 2989, Name = "Leiber", WikiUrl = "https://wiki.guildwars.com/wiki/Leiber" };
    public static readonly Npc Anya = new() { Id = 2990, Name = "Anya", WikiUrl = "https://wiki.guildwars.com/wiki/Anya" };
    public static readonly Npc ExRedemptorBerta = new() { Id = 2990, Name = "Ex-Redemptor Berta", WikiUrl = "https://wiki.guildwars.com/wiki/Ex-Redemptor_Berta" };
    public static readonly Npc KurzickAssassinRecruit = new() { Id = 2990, Name = "Kurzick Assassin Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Assassin_Recruit" };
    public static readonly Npc KurzickNecromancerRecruit = new() { Id = 2990, Name = "Kurzick Necromancer Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Necromancer_Recruit" };
    public static readonly Npc KurzickRitualistRecruit = new() { Id = 2990, Name = "Kurzick Ritualist Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Ritualist_Recruit" };
    public static readonly Npc LuxonElementalistRecruit = new() { Id = 2993, Name = "Luxon Elementalist Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Elementalist_Recruit" };
    public static readonly Npc LuxonNecromancerRecruit = new() { Id = 2993, Name = "Luxon Necromancer Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Necromancer_Recruit" };
    public static readonly Npc LuxonRangerRecruit = new() { Id = 2993, Name = "Luxon Ranger Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Ranger_Recruit" };
    public static readonly Npc LuxonWarriorRecruit = new() { Id = 2993, Name = "Luxon Warrior Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Warrior_Recruit" };
    public static readonly Npc LuxonAssassinRecruit = new() { Id = 2994, Name = "Luxon Assassin Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Assassin_Recruit" };
    public static readonly Npc LuxonMesmerRecruit = new() { Id = 2994, Name = "Luxon Mesmer Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Mesmer_Recruit" };
    public static readonly Npc LuxonMonkRecruit = new() { Id = 2994, Name = "Luxon Monk Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Monk_Recruit" };
    public static readonly Npc LuxonRitualistRecruit = new() { Id = 2994, Name = "Luxon Ritualist Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Ritualist_Recruit" };
    public static readonly Npc CountDurheim = new() { Id = 2998, Name = "Count Durheim", WikiUrl = "https://wiki.guildwars.com/wiki/Count_Durheim" };
    public static readonly Npc BaronessVasburg = new() { Id = 2999, Name = "Baroness Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Baroness_Vasburg" };
    public static readonly Npc ElderNagaWarrior = new() { Id = 3001, Name = "Elder Naga Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Naga_Warrior" };
    public static readonly Npc ElderNagaArcher = new() { Id = 3002, Name = "Elder Naga Archer", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Naga_Archer" };
    public static readonly Npc ElderNagaRitualist = new() { Id = 3003, Name = "Elder Naga Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Naga_Ritualist" };
    public static readonly Npc LuxonPriest = new() { Id = 3024, Name = "Luxon Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Priest" };
    public static readonly Npc OrangeCommander = new() { Id = 3025, Name = "Orange Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Orange_Commander" };
    public static readonly Npc PurpleCommander = new() { Id = 3025, Name = "Purple Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Purple_Commander" };
    public static readonly Npc MasterArchitectGunther = new() { Id = 3026, Name = "Master Architect Gunther", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Architect_Gunther" };
    public static readonly Npc GatekeeperPoletski = new() { Id = 3027, Name = "Gatekeeper Poletski", WikiUrl = "https://wiki.guildwars.com/wiki/Gatekeeper_Poletski" };
    public static readonly Npc GatekeeperRadik = new() { Id = 3027, Name = "Gatekeeper Radik", WikiUrl = "https://wiki.guildwars.com/wiki/Gatekeeper_Radik" };
    public static readonly Npc KurzickJuggernaut = new() { Id = 3028, Name = "Kurzick Juggernaut", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Juggernaut" };
    public static readonly Npc KurzickNecromancer = new() { Id = 3030, Name = "Kurzick Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Necromancer" };
    public static readonly Npc SiegeTurtle = new() { Id = 3031, Name = "Siege Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Siege_Turtle" };
    public static readonly Npc LuxonWarrior = new() { Id = 3032, Name = "Luxon Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Warrior" };
    public static readonly Npc StoneJudge = new() { Id = 3035, Name = "Stone Judge", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Judge" };
    public static readonly Npc Danika = new() { Id = 3036, Name = "Danika", WikiUrl = "https://wiki.guildwars.com/wiki/Danika" };
    public static readonly Npc Danika1 = new() { Id = 3037, Name = "Danika", WikiUrl = "https://wiki.guildwars.com/wiki/Danika" };
    public static readonly Npc RaitahnNem = new() { Id = 3038, Name = "Raitahn Nem", WikiUrl = "https://wiki.guildwars.com/wiki/Raitahn_Nem" };
    public static readonly Npc SoonKim = new() { Id = 3039, Name = "Soon Kim", WikiUrl = "https://wiki.guildwars.com/wiki/Soon_Kim" };
    public static readonly Npc LiYun = new() { Id = 3040, Name = "Li Yun", WikiUrl = "https://wiki.guildwars.com/wiki/Li_Yun" };
    public static readonly Npc MonkStudent = new() { Id = 3041, Name = "Monk Student", WikiUrl = "https://wiki.guildwars.com/wiki/Monk_Student" };
    public static readonly Npc WarriorStudent = new() { Id = 3042, Name = "Warrior Student", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior_Student" };
    public static readonly Npc Hakaru = new() { Id = 3043, Name = "Hakaru", WikiUrl = "https://wiki.guildwars.com/wiki/Hakaru" };
    public static readonly Npc Hanjo = new() { Id = 3044, Name = "Hanjo", WikiUrl = "https://wiki.guildwars.com/wiki/Hanjo" };
    public static readonly Npc Siyan = new() { Id = 3045, Name = "Siyan", WikiUrl = "https://wiki.guildwars.com/wiki/Siyan" };
    public static readonly Npc Huan = new() { Id = 3046, Name = "Huan", WikiUrl = "https://wiki.guildwars.com/wiki/Huan" };
    public static readonly Npc Kam = new() { Id = 3047, Name = "Kam", WikiUrl = "https://wiki.guildwars.com/wiki/Kam" };
    public static readonly Npc Miju = new() { Id = 3048, Name = "Miju", WikiUrl = "https://wiki.guildwars.com/wiki/Miju" };
    public static readonly Npc Ako = new() { Id = 3049, Name = "Ako", WikiUrl = "https://wiki.guildwars.com/wiki/Ako" };
    public static readonly Npc Huan1 = new() { Id = 3050, Name = "Huan", WikiUrl = "https://wiki.guildwars.com/wiki/Huan" };
    public static readonly Npc JatoroMusagi = new() { Id = 3051, Name = "Jatoro Musagi", WikiUrl = "https://wiki.guildwars.com/wiki/Jatoro_Musagi" };
    public static readonly Npc YijoTahn = new() { Id = 3052, Name = "Yijo Tahn", WikiUrl = "https://wiki.guildwars.com/wiki/Yijo_Tahn" };
    public static readonly Npc GullHookbeak = new() { Id = 3054, Name = "Gull Hookbeak", WikiUrl = "https://wiki.guildwars.com/wiki/Gull_Hookbeak" };
    public static readonly Npc YrrgSnagtooth = new() { Id = 3055, Name = "Yrrg Snagtooth", WikiUrl = "https://wiki.guildwars.com/wiki/Yrrg_Snagtooth" };
    public static readonly Npc GruutSnowfoot = new() { Id = 3056, Name = "Gruut Snowfoot", WikiUrl = "https://wiki.guildwars.com/wiki/Gruut_Snowfoot" };
    public static readonly Npc TrrokStrongjaw = new() { Id = 3056, Name = "Trrok Strongjaw", WikiUrl = "https://wiki.guildwars.com/wiki/Trrok_Strongjaw" };
    public static readonly Npc YorrtStrongjaw = new() { Id = 3056, Name = "Yorrt Strongjaw", WikiUrl = "https://wiki.guildwars.com/wiki/Yorrt_Strongjaw" };
    public static readonly Npc TheBogBeastofBokku = new() { Id = 3057, Name = "The Bog Beast of Bokku", WikiUrl = "https://wiki.guildwars.com/wiki/The_Bog_Beast_of_Bokku" };
    public static readonly Npc BoneHorror = new() { Id = 3058, Name = "Bone Horror", WikiUrl = "https://wiki.guildwars.com/wiki/Bone_Horror" };
    public static readonly Npc WildMinion = new() { Id = 3058, Name = "Wild Minion", WikiUrl = "https://wiki.guildwars.com/wiki/Wild_Minion" };
    public static readonly Npc YijoTahn1 = new() { Id = 3059, Name = "Yijo Tahn", WikiUrl = "https://wiki.guildwars.com/wiki/Yijo_Tahn" };
    public static readonly Npc Ludo = new() { Id = 3060, Name = "Ludo", WikiUrl = "https://wiki.guildwars.com/wiki/Ludo" };
    public static readonly Npc Mai = new() { Id = 3061, Name = "Mai", WikiUrl = "https://wiki.guildwars.com/wiki/Mai" };
    public static readonly Npc Kisai = new() { Id = 3062, Name = "Kisai", WikiUrl = "https://wiki.guildwars.com/wiki/Kisai" };
    public static readonly Npc Taya = new() { Id = 3063, Name = "Taya", WikiUrl = "https://wiki.guildwars.com/wiki/Taya" };
    public static readonly Npc Lukas = new() { Id = 3064, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc InstructorNg = new() { Id = 3065, Name = "Instructor Ng", WikiUrl = "https://wiki.guildwars.com/wiki/Instructor_Ng" };
    public static readonly Npc MasterTogo = new() { Id = 3066, Name = "Master Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo" };
    public static readonly Npc DrinkmasterTahnu = new() { Id = 3067, Name = "Drinkmaster Tahnu", WikiUrl = "https://wiki.guildwars.com/wiki/Drinkmaster_Tahnu" };
    public static readonly Npc Turimachus = new() { Id = 3068, Name = "Turimachus", WikiUrl = "https://wiki.guildwars.com/wiki/Turimachus" };
    public static readonly Npc BattlePriestCalibos = new() { Id = 3069, Name = "Battle Priest Calibos", WikiUrl = "https://wiki.guildwars.com/wiki/Battle_Priest_Calibos" };
    public static readonly Npc LuxonAdept = new() { Id = 3071, Name = "Luxon Adept", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Adept" };
    public static readonly Npc HeadmasterVhang = new() { Id = 3073, Name = "Headmaster Vhang", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Vhang" };
    public static readonly Npc MasterTogo1 = new() { Id = 3074, Name = "Master Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo" };
    public static readonly Npc MinistersGuard = new() { Id = 3075, Name = "Minister's Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Minister's_Guard" };
    public static readonly Npc ZaishenScout = new() { Id = 3075, Name = "Zaishen Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_Scout" };
    public static readonly Npc MasterTogo2 = new() { Id = 3077, Name = "Master Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo" };
    public static readonly Npc YijoTahn2 = new() { Id = 3078, Name = "Yijo Tahn", WikiUrl = "https://wiki.guildwars.com/wiki/Yijo_Tahn" };
    public static readonly Npc MerchantOrek = new() { Id = 3080, Name = "Merchant Orek", WikiUrl = "https://wiki.guildwars.com/wiki/Merchant_Orek" };
    public static readonly Npc Merchant = new() { Id = 3081, Name = "Merchant", WikiUrl = "https://wiki.guildwars.com/wiki/Merchant" };
    public static readonly Npc KurzickIllusionist = new() { Id = 3082, Name = "Kurzick Illusionist", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Illusionist" };
    public static readonly Npc LuxonWizard = new() { Id = 3083, Name = "Luxon Wizard", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Wizard" };
    public static readonly Npc KurzickThunder = new() { Id = 3084, Name = "Kurzick Thunder", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Thunder" };
    public static readonly Npc LuxonStormCaller = new() { Id = 3085, Name = "Luxon Storm Caller", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Storm_Caller" };
    public static readonly Npc KurzickFarShot = new() { Id = 3086, Name = "Kurzick Far Shot", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Far_Shot" };
    public static readonly Npc LuxonLongbow = new() { Id = 3087, Name = "Luxon Longbow", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Longbow" };
    public static readonly Npc KommandantDurheim = new() { Id = 3088, Name = "Kommandant Durheim", WikiUrl = "https://wiki.guildwars.com/wiki/Kommandant_Durheim" };
    public static readonly Npc KurzickBaseDefender = new() { Id = 3088, Name = "Kurzick Base Defender", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Base_Defender" };
    public static readonly Npc Erek = new() { Id = 3089, Name = "Erek", WikiUrl = "https://wiki.guildwars.com/wiki/Erek" };
    public static readonly Npc LuxonBaseDefender = new() { Id = 3089, Name = "Luxon Base Defender", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Base_Defender" };
    public static readonly Npc ShadyLuxon = new() { Id = 3090, Name = "Shady Luxon", WikiUrl = "https://wiki.guildwars.com/wiki/Shady_Luxon" };
    public static readonly Npc SuspiciousKurzick = new() { Id = 3091, Name = "Suspicious Kurzick", WikiUrl = "https://wiki.guildwars.com/wiki/Suspicious_Kurzick" };
    public static readonly Npc HeiTsu = new() { Id = 3092, Name = "Hei Tsu", WikiUrl = "https://wiki.guildwars.com/wiki/Hei_Tsu" };
    public static readonly Npc ZhuHanuku = new() { Id = 3093, Name = "Zhu Hanuku", WikiUrl = "https://wiki.guildwars.com/wiki/Zhu_Hanuku" };
    public static readonly Npc Mai1 = new() { Id = 3095, Name = "Mai", WikiUrl = "https://wiki.guildwars.com/wiki/Mai" };
    public static readonly Npc Kisai1 = new() { Id = 3096, Name = "Kisai", WikiUrl = "https://wiki.guildwars.com/wiki/Kisai" };
    public static readonly Npc Yuun = new() { Id = 3097, Name = "Yuun", WikiUrl = "https://wiki.guildwars.com/wiki/Yuun" };
    public static readonly Npc Aeson = new() { Id = 3098, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Lukas1 = new() { Id = 3099, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc Argo = new() { Id = 3100, Name = "Argo", WikiUrl = "https://wiki.guildwars.com/wiki/Argo" };
    public static readonly Npc Cynn = new() { Id = 3101, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc Danika2 = new() { Id = 3102, Name = "Danika", WikiUrl = "https://wiki.guildwars.com/wiki/Danika" };
    public static readonly Npc MasterTogo3 = new() { Id = 3103, Name = "Master Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo" };
    public static readonly Npc MasterTogo4 = new() { Id = 3104, Name = "Master Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo" };
    public static readonly Npc BrotherMhenlo = new() { Id = 3105, Name = "Brother Mhenlo", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Mhenlo" };
    public static readonly Npc Nika = new() { Id = 3106, Name = "Nika", WikiUrl = "https://wiki.guildwars.com/wiki/Nika" };
    public static readonly Npc Panaku = new() { Id = 3107, Name = "Panaku", WikiUrl = "https://wiki.guildwars.com/wiki/Panaku" };
    public static readonly Npc TalonSilverwing = new() { Id = 3108, Name = "Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Talon_Silverwing" };
    public static readonly Npc CelestialHorror = new() { Id = 3111, Name = "Celestial Horror", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Horror" };
    public static readonly Npc MasterTogo5 = new() { Id = 3113, Name = "Master Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo" };
    public static readonly Npc Jaizhanju = new() { Id = 3114, Name = "Jaizhanju", WikiUrl = "https://wiki.guildwars.com/wiki/Jaizhanju" };
    public static readonly Npc Teinai = new() { Id = 3114, Name = "Teinai", WikiUrl = "https://wiki.guildwars.com/wiki/Teinai" };
    public static readonly Npc Karei = new() { Id = 3115, Name = "Karei", WikiUrl = "https://wiki.guildwars.com/wiki/Karei" };
    public static readonly Npc Naku = new() { Id = 3115, Name = "Naku", WikiUrl = "https://wiki.guildwars.com/wiki/Naku" };
    public static readonly Npc MasterTogo6 = new() { Id = 3116, Name = "Master Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo" };
    public static readonly Npc Togo = new() { Id = 3116, Name = "Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Togo" };
    public static readonly Npc BrotherMhenlo1 = new() { Id = 3117, Name = "Brother Mhenlo", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Mhenlo" };
    public static readonly Npc ShadowBladeAssassin = new() { Id = 3118, Name = "Shadow Blade Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_Blade_Assassin" };
    public static readonly Npc Kiishen = new() { Id = 3119, Name = "Kiishen", WikiUrl = "https://wiki.guildwars.com/wiki/Kiishen" };
    public static readonly Npc ObsidianFlameAssassin = new() { Id = 3119, Name = "Obsidian Flame Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Obsidian_Flame_Assassin" };
    public static readonly Npc KeFeng = new() { Id = 3120, Name = "Ke Feng", WikiUrl = "https://wiki.guildwars.com/wiki/Ke_Feng" };
    public static readonly Npc Taya1 = new() { Id = 3121, Name = "Taya", WikiUrl = "https://wiki.guildwars.com/wiki/Taya" };
    public static readonly Npc ProfessorGai = new() { Id = 3122, Name = "Professor Gai", WikiUrl = "https://wiki.guildwars.com/wiki/Professor_Gai" };
    public static readonly Npc SisterTai = new() { Id = 3123, Name = "Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Tai" };
    public static readonly Npc MeiLing = new() { Id = 3124, Name = "Mei Ling", WikiUrl = "https://wiki.guildwars.com/wiki/Mei_Ling" };
    public static readonly Npc LoSha = new() { Id = 3125, Name = "Lo Sha", WikiUrl = "https://wiki.guildwars.com/wiki/Lo_Sha" };
    public static readonly Npc Su = new() { Id = 3126, Name = "Su", WikiUrl = "https://wiki.guildwars.com/wiki/Su" };
    public static readonly Npc TenguCutter = new() { Id = 3127, Name = "Tengu Cutter", WikiUrl = "https://wiki.guildwars.com/wiki/Tengu_Cutter" };
    public static readonly Npc Lukas2 = new() { Id = 3128, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc ErysVasburg = new() { Id = 3129, Name = "Erys Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Erys_Vasburg" };
    public static readonly Npc Aeson1 = new() { Id = 3130, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Daemen = new() { Id = 3131, Name = "Daemen", WikiUrl = "https://wiki.guildwars.com/wiki/Daemen" };
    public static readonly Npc Aurora = new() { Id = 3132, Name = "Aurora", WikiUrl = "https://wiki.guildwars.com/wiki/Aurora" };
    public static readonly Npc Hala = new() { Id = 3133, Name = "Hala", WikiUrl = "https://wiki.guildwars.com/wiki/Hala" };
    public static readonly Npc SeaguardGita = new() { Id = 3134, Name = "Seaguard Gita", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Gita" };
    public static readonly Npc SeaguardEli = new() { Id = 3135, Name = "Seaguard Eli", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Eli" };
    public static readonly Npc BrotherMhenlo2 = new() { Id = 3136, Name = "Brother Mhenlo", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Mhenlo" };
    public static readonly Npc Nika1 = new() { Id = 3137, Name = "Nika", WikiUrl = "https://wiki.guildwars.com/wiki/Nika" };
    public static readonly Npc Jamei = new() { Id = 3138, Name = "Jamei", WikiUrl = "https://wiki.guildwars.com/wiki/Jamei" };
    public static readonly Npc Chiyo = new() { Id = 3139, Name = "Chiyo", WikiUrl = "https://wiki.guildwars.com/wiki/Chiyo" };
    public static readonly Npc Emi = new() { Id = 3140, Name = "Emi", WikiUrl = "https://wiki.guildwars.com/wiki/Emi" };
    public static readonly Npc Devona = new() { Id = 3141, Name = "Devona", WikiUrl = "https://wiki.guildwars.com/wiki/Devona" };
    public static readonly Npc Cynn1 = new() { Id = 3142, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc Aidan = new() { Id = 3143, Name = "Aidan", WikiUrl = "https://wiki.guildwars.com/wiki/Aidan" };
    public static readonly Npc Eve = new() { Id = 3144, Name = "Eve", WikiUrl = "https://wiki.guildwars.com/wiki/Eve" };
    public static readonly Npc KaiYing = new() { Id = 3145, Name = "Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Kai_Ying" };
    public static readonly Npc Zho = new() { Id = 3146, Name = "Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Zho" };
    public static readonly Npc Panaku1 = new() { Id = 3147, Name = "Panaku", WikiUrl = "https://wiki.guildwars.com/wiki/Panaku" };
    public static readonly Npc HeadmasterVhang1 = new() { Id = 3148, Name = "Headmaster Vhang", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Vhang" };
    public static readonly Npc Mai2 = new() { Id = 3149, Name = "Mai", WikiUrl = "https://wiki.guildwars.com/wiki/Mai" };
    public static readonly Npc Kisai2 = new() { Id = 3150, Name = "Kisai", WikiUrl = "https://wiki.guildwars.com/wiki/Kisai" };
    public static readonly Npc Yuun1 = new() { Id = 3151, Name = "Yuun", WikiUrl = "https://wiki.guildwars.com/wiki/Yuun" };
    public static readonly Npc TalonSilverwing1 = new() { Id = 3152, Name = "Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Talon_Silverwing" };
    public static readonly Npc Bakghu = new() { Id = 3153, Name = "Bakghu", WikiUrl = "https://wiki.guildwars.com/wiki/Bakghu" };
    public static readonly Npc Ohtah = new() { Id = 3153, Name = "Ohtah", WikiUrl = "https://wiki.guildwars.com/wiki/Ohtah" };
    public static readonly Npc Kegai = new() { Id = 3154, Name = "Kegai", WikiUrl = "https://wiki.guildwars.com/wiki/Kegai" };
    public static readonly Npc Hannai = new() { Id = 3155, Name = "Hannai", WikiUrl = "https://wiki.guildwars.com/wiki/Hannai" };
    public static readonly Npc Arani = new() { Id = 3156, Name = "Arani", WikiUrl = "https://wiki.guildwars.com/wiki/Arani" };
    public static readonly Npc Caly = new() { Id = 3157, Name = "Caly", WikiUrl = "https://wiki.guildwars.com/wiki/Caly" };
    public static readonly Npc Ting = new() { Id = 3158, Name = "Ting", WikiUrl = "https://wiki.guildwars.com/wiki/Ting" };
    public static readonly Npc Khim = new() { Id = 3159, Name = "Khim", WikiUrl = "https://wiki.guildwars.com/wiki/Khim" };
    public static readonly Npc Rion = new() { Id = 3160, Name = "Rion", WikiUrl = "https://wiki.guildwars.com/wiki/Rion" };
    public static readonly Npc Sao = new() { Id = 3161, Name = "Sao", WikiUrl = "https://wiki.guildwars.com/wiki/Sao" };
    public static readonly Npc TheAfflictedHuu = new() { Id = 3162, Name = "The Afflicted Huu", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Huu" };
    public static readonly Npc TheAfflictedMeeka = new() { Id = 3163, Name = "The Afflicted Meeka", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Meeka" };
    public static readonly Npc TheAfflictedXi = new() { Id = 3164, Name = "The Afflicted Xi", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Xi" };
    public static readonly Npc TheAfflictedRasa = new() { Id = 3165, Name = "The Afflicted Rasa", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Rasa" };
    public static readonly Npc TheAfflictedCho = new() { Id = 3166, Name = "The Afflicted Cho", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Cho" };
    public static readonly Npc TheAfflictedTamaya = new() { Id = 3167, Name = "The Afflicted Tamaya", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Tamaya" };
    public static readonly Npc TheAfflictedSusei = new() { Id = 3168, Name = "The Afflicted Susei", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Susei" };
    public static readonly Npc TheAfflictedMei = new() { Id = 3169, Name = "The Afflicted Mei", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Mei" };
    public static readonly Npc GuardsmanChienpo = new() { Id = 3170, Name = "Guardsman Chienpo", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Chienpo" };
    public static readonly Npc GuardsmanPing = new() { Id = 3170, Name = "Guardsman Ping", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Ping" };
    public static readonly Npc ImperialGuardsmanLinro = new() { Id = 3170, Name = "Imperial Guardsman Linro", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guardsman_Linro" };
    public static readonly Npc ImperialAgentHanjo = new() { Id = 3171, Name = "Imperial Agent Hanjo", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Agent_Hanjo" };
    public static readonly Npc ImperialGuardsmanKintae = new() { Id = 3171, Name = "Imperial Guardsman Kintae", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guardsman_Kintae" };
    public static readonly Npc MinistryGuard = new() { Id = 3171, Name = "Ministry Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Ministry_Guard" };
    public static readonly Npc ShaeWong = new() { Id = 3171, Name = "Shae Wong", WikiUrl = "https://wiki.guildwars.com/wiki/Shae_Wong" };
    public static readonly Npc Kakumei = new() { Id = 3173, Name = "Kakumei", WikiUrl = "https://wiki.guildwars.com/wiki/Kakumei" };
    public static readonly Npc IzaYoi = new() { Id = 3174, Name = "Iza Yoi", WikiUrl = "https://wiki.guildwars.com/wiki/Iza_Yoi" };
    public static readonly Npc Ryoko = new() { Id = 3174, Name = "Ryoko", WikiUrl = "https://wiki.guildwars.com/wiki/Ryoko" };
    public static readonly Npc Suki = new() { Id = 3174, Name = "Suki", WikiUrl = "https://wiki.guildwars.com/wiki/Suki" };
    public static readonly Npc Natsuko = new() { Id = 3175, Name = "Natsuko", WikiUrl = "https://wiki.guildwars.com/wiki/Natsuko" };
    public static readonly Npc Ryukichi = new() { Id = 3176, Name = "Ryukichi", WikiUrl = "https://wiki.guildwars.com/wiki/Ryukichi" };
    public static readonly Npc Manzo = new() { Id = 3177, Name = "Manzo", WikiUrl = "https://wiki.guildwars.com/wiki/Manzo" };
    public static readonly Npc Kumiko = new() { Id = 3178, Name = "Kumiko", WikiUrl = "https://wiki.guildwars.com/wiki/Kumiko" };
    public static readonly Npc XueYi = new() { Id = 3179, Name = "Xue Yi", WikiUrl = "https://wiki.guildwars.com/wiki/Xue_Yi" };
    public static readonly Npc XueFang = new() { Id = 3180, Name = "Xue Fang", WikiUrl = "https://wiki.guildwars.com/wiki/Xue_Fang" };
    public static readonly Npc Yutake = new() { Id = 3181, Name = "Yutake", WikiUrl = "https://wiki.guildwars.com/wiki/Yutake" };
    public static readonly Npc JiazhenLiMaterial = new() { Id = 3182, Name = "Jiazhen Li Material", WikiUrl = "https://wiki.guildwars.com/wiki/Jiazhen_Li_Material" };
    public static readonly Npc ZhouPakScroll = new() { Id = 3183, Name = "Zhou Pak Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Zhou_Pak_Scroll" };
    public static readonly Npc Mitsunari = new() { Id = 3184, Name = "Mitsunari", WikiUrl = "https://wiki.guildwars.com/wiki/Mitsunari" };
    public static readonly Npc GuildmasterLuan = new() { Id = 3185, Name = "Guildmaster Luan", WikiUrl = "https://wiki.guildwars.com/wiki/Guildmaster_Luan" };
    public static readonly Npc MinisterJaisan = new() { Id = 3185, Name = "Minister Jaisan", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Jaisan" };
    public static readonly Npc PoFang = new() { Id = 3185, Name = "Po Fang", WikiUrl = "https://wiki.guildwars.com/wiki/Po_Fang" };
    public static readonly Npc Michiko = new() { Id = 3186, Name = "Michiko", WikiUrl = "https://wiki.guildwars.com/wiki/Michiko" };
    public static readonly Npc Akoto = new() { Id = 3187, Name = "Akoto", WikiUrl = "https://wiki.guildwars.com/wiki/Akoto" };
    public static readonly Npc MinisterNai = new() { Id = 3187, Name = "Minister Nai", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Nai" };
    public static readonly Npc MinisterZal = new() { Id = 3187, Name = "Minister Zal", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Zal" };
    public static readonly Npc JinSiyan = new() { Id = 3188, Name = "Jin Siyan", WikiUrl = "https://wiki.guildwars.com/wiki/Jin_Siyan" };
    public static readonly Npc BarkeepMehoro = new() { Id = 3189, Name = "Barkeep Mehoro", WikiUrl = "https://wiki.guildwars.com/wiki/Barkeep_Mehoro" };
    public static readonly Npc Bujo = new() { Id = 3190, Name = "Bujo", WikiUrl = "https://wiki.guildwars.com/wiki/Bujo" };
    public static readonly Npc EmperorKisu = new() { Id = 3192, Name = "Emperor Kisu", WikiUrl = "https://wiki.guildwars.com/wiki/Emperor_Kisu" };
    public static readonly Npc Kitah = new() { Id = 3203, Name = "Kitah", WikiUrl = "https://wiki.guildwars.com/wiki/Kitah" };
    public static readonly Npc Zojun = new() { Id = 3208, Name = "Zojun", WikiUrl = "https://wiki.guildwars.com/wiki/Zojun" };
    public static readonly Npc Kaolai = new() { Id = 3209, Name = "Kaolai", WikiUrl = "https://wiki.guildwars.com/wiki/Kaolai" };
    public static readonly Npc MasterTogo7 = new() { Id = 3211, Name = "Master Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo" };
    public static readonly Npc Togo1 = new() { Id = 3211, Name = "Togo", WikiUrl = "https://wiki.guildwars.com/wiki/Togo" };
    public static readonly Npc BrotherMhenlo3 = new() { Id = 3212, Name = "Brother Mhenlo", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Mhenlo" };
    public static readonly Npc Nika2 = new() { Id = 3214, Name = "Nika", WikiUrl = "https://wiki.guildwars.com/wiki/Nika" };
    public static readonly Npc Jamei1 = new() { Id = 3215, Name = "Jamei", WikiUrl = "https://wiki.guildwars.com/wiki/Jamei" };
    public static readonly Npc Chiyo1 = new() { Id = 3216, Name = "Chiyo", WikiUrl = "https://wiki.guildwars.com/wiki/Chiyo" };
    public static readonly Npc Emi1 = new() { Id = 3217, Name = "Emi", WikiUrl = "https://wiki.guildwars.com/wiki/Emi" };
    public static readonly Npc Cynn2 = new() { Id = 3219, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc TalonSilverwing2 = new() { Id = 3222, Name = "Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Talon_Silverwing" };
    public static readonly Npc TalonSilverwing3 = new() { Id = 3223, Name = "Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Talon_Silverwing" };
    public static readonly Npc Bauyun = new() { Id = 3224, Name = "Bauyun", WikiUrl = "https://wiki.guildwars.com/wiki/Bauyun" };
    public static readonly Npc CampGuardPongtu = new() { Id = 3224, Name = "Camp Guard Pongtu", WikiUrl = "https://wiki.guildwars.com/wiki/Camp_Guard_Pongtu" };
    public static readonly Npc CanthanGuard = new() { Id = 3224, Name = "Canthan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard" };
    public static readonly Npc CaptainLoFah = new() { Id = 3224, Name = "Captain Lo Fah", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Lo_Fah" };
    public static readonly Npc DockhandQuangqnai = new() { Id = 3224, Name = "Dockhand Quangqnai", WikiUrl = "https://wiki.guildwars.com/wiki/Dockhand_Quangqnai" };
    public static readonly Npc GuardCaptainVassi = new() { Id = 3224, Name = "Guard Captain Vassi", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Captain_Vassi" };
    public static readonly Npc GuardLaonan = new() { Id = 3224, Name = "Guard Laonan", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Laonan" };
    public static readonly Npc GuardRaabo = new() { Id = 3224, Name = "Guard Raabo", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Raabo" };
    public static readonly Npc GuardTsukaro = new() { Id = 3224, Name = "Guard Tsukaro", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Tsukaro" };
    public static readonly Npc GuardsmanAyoki = new() { Id = 3224, Name = "Guardsman Ayoki", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Ayoki" };
    public static readonly Npc GuardsmanChienpo1 = new() { Id = 3224, Name = "Guardsman Chienpo", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Chienpo" };
    public static readonly Npc GuardsmanChow = new() { Id = 3224, Name = "Guardsman Chow", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Chow" };
    public static readonly Npc GuardsmanKayao = new() { Id = 3224, Name = "Guardsman Kayao", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Kayao" };
    public static readonly Npc GuardsmanKikuchiyo = new() { Id = 3224, Name = "Guardsman Kikuchiyo", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Kikuchiyo" };
    public static readonly Npc GuardsmanMakuruyo = new() { Id = 3224, Name = "Guardsman Makuruyo", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Makuruyo" };
    public static readonly Npc GuardsmanPah = new() { Id = 3224, Name = "Guardsman Pah", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Pah" };
    public static readonly Npc GuardsmanPei = new() { Id = 3224, Name = "Guardsman Pei", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Pei" };
    public static readonly Npc GuardsmanPing1 = new() { Id = 3224, Name = "Guardsman Ping", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Ping" };
    public static readonly Npc GuardsmanTang = new() { Id = 3224, Name = "Guardsman Tang", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Tang" };
    public static readonly Npc GuardsmanZui = new() { Id = 3224, Name = "Guardsman Zui", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Zui" };
    public static readonly Npc ImperialGuardKozoko = new() { Id = 3224, Name = "Imperial Guard Kozoko", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guard_Kozoko" };
    public static readonly Npc ImperialGuardsmanLinro1 = new() { Id = 3224, Name = "Imperial Guardsman Linro", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guardsman_Linro" };
    public static readonly Npc ImperialGuardsmanYang = new() { Id = 3224, Name = "Imperial Guardsman Yang", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guardsman_Yang" };
    public static readonly Npc Laris = new() { Id = 3224, Name = "Laris", WikiUrl = "https://wiki.guildwars.com/wiki/Laris" };
    public static readonly Npc MonasteryQuartermaster = new() { Id = 3224, Name = "Monastery Quartermaster", WikiUrl = "https://wiki.guildwars.com/wiki/Monastery_Quartermaster" };
    public static readonly Npc OffDutyGuard = new() { Id = 3224, Name = "Off-Duty Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Off-Duty_Guard" };
    public static readonly Npc PetraBrauer = new() { Id = 3224, Name = "Petra Brauer", WikiUrl = "https://wiki.guildwars.com/wiki/Petra_Brauer" };
    public static readonly Npc RoyalGuard = new() { Id = 3224, Name = "Royal Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Guard" };
    public static readonly Npc SeungKim = new() { Id = 3224, Name = "Seung Kim", WikiUrl = "https://wiki.guildwars.com/wiki/Seung_Kim" };
    public static readonly Npc ShenXiGuardRequisition = new() { Id = 3224, Name = "Shen Xi Guard Requisition", WikiUrl = "https://wiki.guildwars.com/wiki/Shen_Xi_Guard_Requisition" };
    public static readonly Npc TempleGuardBai = new() { Id = 3224, Name = "Temple Guard Bai", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_Guard_Bai" };
    public static readonly Npc Vargus = new() { Id = 3224, Name = "Vargus", WikiUrl = "https://wiki.guildwars.com/wiki/Vargus" };
    public static readonly Npc CanthanGuard1 = new() { Id = 3225, Name = "Canthan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard" };
    public static readonly Npc GuardsmanPo = new() { Id = 3225, Name = "Guardsman Po", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Po" };
    public static readonly Npc MinistersGuard1 = new() { Id = 3225, Name = "Minister's Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Minister's_Guard" };
    public static readonly Npc ScoutShenfai = new() { Id = 3225, Name = "Scout Shenfai", WikiUrl = "https://wiki.guildwars.com/wiki/Scout_Shenfai" };
    public static readonly Npc Suhkaro = new() { Id = 3225, Name = "Suhkaro", WikiUrl = "https://wiki.guildwars.com/wiki/Suhkaro" };
    public static readonly Npc CanthanBodyguard = new() { Id = 3227, Name = "Canthan Bodyguard", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Bodyguard" };
    public static readonly Npc CanthanGuard2 = new() { Id = 3227, Name = "Canthan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard" };
    public static readonly Npc CanthanGuardCaptain = new() { Id = 3227, Name = "Canthan Guard Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard_Captain" };
    public static readonly Npc CaptainSei = new() { Id = 3227, Name = "Captain Sei", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Sei" };
    public static readonly Npc CaptainZinghu = new() { Id = 3227, Name = "Captain Zinghu", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Zinghu" };
    public static readonly Npc CommanderJafai = new() { Id = 3227, Name = "Commander Jafai", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Jafai" };
    public static readonly Npc EmperorsVoice = new() { Id = 3227, Name = "Emperor's Voice", WikiUrl = "https://wiki.guildwars.com/wiki/Emperor's_Voice" };
    public static readonly Npc GuardLaeFao = new() { Id = 3227, Name = "Guard Lae Fao", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Lae_Fao" };
    public static readonly Npc GuardsmanKenji = new() { Id = 3227, Name = "Guardsman Kenji", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Kenji" };
    public static readonly Npc GuardsmanKinri = new() { Id = 3227, Name = "Guardsman Kinri", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Kinri" };
    public static readonly Npc GuardsmanZingpah = new() { Id = 3227, Name = "Guardsman Zingpah", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Zingpah" };
    public static readonly Npc ImperialAgentHanjo1 = new() { Id = 3227, Name = "Imperial Agent Hanjo", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Agent_Hanjo" };
    public static readonly Npc ImperialGuardRantoh = new() { Id = 3227, Name = "Imperial Guard Rantoh", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guard_Rantoh" };
    public static readonly Npc ImperialGuardsmanHanzing = new() { Id = 3227, Name = "Imperial Guardsman Hanzing", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guardsman_Hanzing" };
    public static readonly Npc ImperialGuardsmanKintae1 = new() { Id = 3227, Name = "Imperial Guardsman Kintae", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guardsman_Kintae" };
    public static readonly Npc ImperialGuardsmanTingjo = new() { Id = 3227, Name = "Imperial Guardsman Tingjo", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guardsman_Tingjo" };
    public static readonly Npc MagistrateWakai = new() { Id = 3227, Name = "Magistrate Wakai", WikiUrl = "https://wiki.guildwars.com/wiki/Magistrate_Wakai" };
    public static readonly Npc MinistryGuard1 = new() { Id = 3227, Name = "Ministry Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Ministry_Guard" };
    public static readonly Npc RaitahnNem1 = new() { Id = 3227, Name = "Raitahn Nem", WikiUrl = "https://wiki.guildwars.com/wiki/Raitahn_Nem" };
    public static readonly Npc ShaeWong1 = new() { Id = 3227, Name = "Shae Wong", WikiUrl = "https://wiki.guildwars.com/wiki/Shae_Wong" };
    public static readonly Npc TheEmperorsBlade = new() { Id = 3227, Name = "The Emperor's Blade", WikiUrl = "https://wiki.guildwars.com/wiki/The_Emperor's_Blade" };
    public static readonly Npc CanthanGuardCaptain1 = new() { Id = 3228, Name = "Canthan Guard Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard_Captain" };
    public static readonly Npc MagistrateRaisung = new() { Id = 3228, Name = "Magistrate Raisung", WikiUrl = "https://wiki.guildwars.com/wiki/Magistrate_Raisung" };
    public static readonly Npc PalaceGuardTsungkim = new() { Id = 3228, Name = "Palace Guard Tsungkim", WikiUrl = "https://wiki.guildwars.com/wiki/Palace_Guard_Tsungkim" };
    public static readonly Npc RoyalGuardKazuya = new() { Id = 3228, Name = "Royal Guard Kazuya", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Guard_Kazuya" };
    public static readonly Npc CanthanGuard3 = new() { Id = 3230, Name = "Canthan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard" };
    public static readonly Npc CorpseofCanthanGuard = new() { Id = 3230, Name = "Corpse of Canthan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Corpse_of_Canthan_Guard" };
    public static readonly Npc AzumeGuardRequisition = new() { Id = 3231, Name = "Azume Guard Requisition", WikiUrl = "https://wiki.guildwars.com/wiki/Azume_Guard_Requisition" };
    public static readonly Npc CanthanGuard4 = new() { Id = 3231, Name = "Canthan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard" };
    public static readonly Npc GuardsmanKeiko = new() { Id = 3231, Name = "Guardsman Keiko", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Keiko" };
    public static readonly Npc ImperialGuide = new() { Id = 3231, Name = "Imperial Guide", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guide" };
    public static readonly Npc ImperialQuartermaster = new() { Id = 3231, Name = "Imperial Quartermaster", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Quartermaster" };
    public static readonly Npc Kujin = new() { Id = 3231, Name = "Kujin", WikiUrl = "https://wiki.guildwars.com/wiki/Kujin" };
    public static readonly Npc MonasteryQuartermaster1 = new() { Id = 3231, Name = "Monastery Quartermaster", WikiUrl = "https://wiki.guildwars.com/wiki/Monastery_Quartermaster" };
    public static readonly Npc ReiMing = new() { Id = 3231, Name = "Rei Ming", WikiUrl = "https://wiki.guildwars.com/wiki/Rei_Ming" };
    public static readonly Npc RoyalGuard1 = new() { Id = 3231, Name = "Royal Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Guard" };
    public static readonly Npc ZanLeiGuardRequisition = new() { Id = 3231, Name = "Zan Lei Guard Requisition", WikiUrl = "https://wiki.guildwars.com/wiki/Zan_Lei_Guard_Requisition" };
    public static readonly Npc CanthanGuard5 = new() { Id = 3233, Name = "Canthan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard" };
    public static readonly Npc CanthanChild = new() { Id = 3234, Name = "Canthan Child", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Child" };
    public static readonly Npc LostBoy = new() { Id = 3234, Name = "Lost Boy", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Boy" };
    public static readonly Npc Nei = new() { Id = 3234, Name = "Nei", WikiUrl = "https://wiki.guildwars.com/wiki/Nei" };
    public static readonly Npc CanthanChild1 = new() { Id = 3235, Name = "Canthan Child", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Child" };
    public static readonly Npc Aiko = new() { Id = 3236, Name = "Aiko", WikiUrl = "https://wiki.guildwars.com/wiki/Aiko" };
    public static readonly Npc CanthanChild2 = new() { Id = 3236, Name = "Canthan Child", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Child" };
    public static readonly Npc Kimi = new() { Id = 3236, Name = "Kimi", WikiUrl = "https://wiki.guildwars.com/wiki/Kimi" };
    public static readonly Npc Mitah = new() { Id = 3236, Name = "Mitah", WikiUrl = "https://wiki.guildwars.com/wiki/Mitah" };
    public static readonly Npc MusakoTuro = new() { Id = 3236, Name = "Musako Tu'ro", WikiUrl = "https://wiki.guildwars.com/wiki/Musako_Tu'ro" };
    public static readonly Npc Akoto1 = new() { Id = 3237, Name = "Akoto", WikiUrl = "https://wiki.guildwars.com/wiki/Akoto" };
    public static readonly Npc AttendantNashu = new() { Id = 3237, Name = "Attendant Nashu", WikiUrl = "https://wiki.guildwars.com/wiki/Attendant_Nashu" };
    public static readonly Npc Axlan = new() { Id = 3237, Name = "Axlan", WikiUrl = "https://wiki.guildwars.com/wiki/Axlan" };
    public static readonly Npc CanthanNoble = new() { Id = 3237, Name = "Canthan Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Noble" };
    public static readonly Npc CaptainQuimang = new() { Id = 3237, Name = "Captain Quimang", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Quimang" };
    public static readonly Npc ChenPoChin = new() { Id = 3237, Name = "Chen Po Chin", WikiUrl = "https://wiki.guildwars.com/wiki/Chen_Po_Chin" };
    public static readonly Npc EmperorsHand = new() { Id = 3237, Name = "Emperor's Hand", WikiUrl = "https://wiki.guildwars.com/wiki/Emperor's_Hand" };
    public static readonly Npc FirstMateXiang = new() { Id = 3237, Name = "First Mate Xiang", WikiUrl = "https://wiki.guildwars.com/wiki/First_Mate_Xiang" };
    public static readonly Npc ImperialChefYileng = new() { Id = 3237, Name = "Imperial Chef Yileng", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Chef_Yileng" };
    public static readonly Npc JuniorMinisterJejiang = new() { Id = 3237, Name = "Junior Minister Jejiang", WikiUrl = "https://wiki.guildwars.com/wiki/Junior_Minister_Jejiang" };
    public static readonly Npc LoudKou = new() { Id = 3237, Name = "Loud Kou", WikiUrl = "https://wiki.guildwars.com/wiki/Loud_Kou" };
    public static readonly Npc MessengerGosuh = new() { Id = 3237, Name = "Messenger Gosuh", WikiUrl = "https://wiki.guildwars.com/wiki/Messenger_Gosuh" };
    public static readonly Npc MinisterBaasong = new() { Id = 3237, Name = "Minister Baasong", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Baasong" };
    public static readonly Npc MinisterCho = new() { Id = 3237, Name = "Minister Cho", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Cho" };
    public static readonly Npc MinisterKhannai = new() { Id = 3237, Name = "Minister Khannai", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Khannai" };
    public static readonly Npc MinisterNai1 = new() { Id = 3237, Name = "Minister Nai", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Nai" };
    public static readonly Npc MinisterofMaintenanceRaiugyon = new() { Id = 3237, Name = "Minister of Maintenance Raiugyon", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_of_Maintenance_Raiugyon" };
    public static readonly Npc MinisterOnghsang = new() { Id = 3237, Name = "Minister Onghsang", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Onghsang" };
    public static readonly Npc MinisterZal1 = new() { Id = 3237, Name = "Minister Zal", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Zal" };
    public static readonly Npc NoblemanKagita = new() { Id = 3237, Name = "Nobleman Kagita", WikiUrl = "https://wiki.guildwars.com/wiki/Nobleman_Kagita" };
    public static readonly Npc OfficerChitaro = new() { Id = 3237, Name = "Officer Chitaro", WikiUrl = "https://wiki.guildwars.com/wiki/Officer_Chitaro" };
    public static readonly Npc OracleofTime = new() { Id = 3237, Name = "Oracle of Time", WikiUrl = "https://wiki.guildwars.com/wiki/Oracle_of_Time" };
    public static readonly Npc Saito = new() { Id = 3237, Name = "Saito", WikiUrl = "https://wiki.guildwars.com/wiki/Saito" };
    public static readonly Npc Sakai = new() { Id = 3237, Name = "Sakai", WikiUrl = "https://wiki.guildwars.com/wiki/Sakai" };
    public static readonly Npc YanleiBruun = new() { Id = 3237, Name = "Yanlei Bruun", WikiUrl = "https://wiki.guildwars.com/wiki/Yanlei_Bruun" };
    public static readonly Npc Yokuni = new() { Id = 3237, Name = "Yokuni", WikiUrl = "https://wiki.guildwars.com/wiki/Yokuni" };
    public static readonly Npc Akane = new() { Id = 3238, Name = "Akane", WikiUrl = "https://wiki.guildwars.com/wiki/Akane" };
    public static readonly Npc AttendantChien = new() { Id = 3238, Name = "Attendant Chien", WikiUrl = "https://wiki.guildwars.com/wiki/Attendant_Chien" };
    public static readonly Npc AttendantHara = new() { Id = 3238, Name = "Attendant Hara", WikiUrl = "https://wiki.guildwars.com/wiki/Attendant_Hara" };
    public static readonly Npc AttendantYoko = new() { Id = 3238, Name = "Attendant Yoko", WikiUrl = "https://wiki.guildwars.com/wiki/Attendant_Yoko" };
    public static readonly Npc CanthanNoble1 = new() { Id = 3238, Name = "Canthan Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Noble" };
    public static readonly Npc RanTi = new() { Id = 3238, Name = "Ran Ti", WikiUrl = "https://wiki.guildwars.com/wiki/Ran_Ti" };
    public static readonly Npc ZhaoDi = new() { Id = 3238, Name = "Zhao Di", WikiUrl = "https://wiki.guildwars.com/wiki/Zhao_Di" };
    public static readonly Npc Budo = new() { Id = 3239, Name = "Budo", WikiUrl = "https://wiki.guildwars.com/wiki/Budo" };
    public static readonly Npc CanthanNoble2 = new() { Id = 3239, Name = "Canthan Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Noble" };
    public static readonly Npc GuildmasterLuan1 = new() { Id = 3239, Name = "Guildmaster Luan", WikiUrl = "https://wiki.guildwars.com/wiki/Guildmaster_Luan" };
    public static readonly Npc ImperialHerald = new() { Id = 3239, Name = "Imperial Herald", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Herald" };
    public static readonly Npc Kyuzo = new() { Id = 3239, Name = "Kyuzo", WikiUrl = "https://wiki.guildwars.com/wiki/Kyuzo" };
    public static readonly Npc Masaharu = new() { Id = 3239, Name = "Masaharu", WikiUrl = "https://wiki.guildwars.com/wiki/Masaharu" };
    public static readonly Npc MinisterJaisan1 = new() { Id = 3239, Name = "Minister Jaisan", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Jaisan" };
    public static readonly Npc MinisterTahlen = new() { Id = 3239, Name = "Minister Tahlen", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Tahlen" };
    public static readonly Npc PoFang1 = new() { Id = 3239, Name = "Po Fang", WikiUrl = "https://wiki.guildwars.com/wiki/Po_Fang" };
    public static readonly Npc SekaitheMapmaker = new() { Id = 3239, Name = "Sekai the Mapmaker", WikiUrl = "https://wiki.guildwars.com/wiki/Sekai_the_Mapmaker" };
    public static readonly Npc TahwajZing = new() { Id = 3239, Name = "Tahwaj Zing", WikiUrl = "https://wiki.guildwars.com/wiki/Tahwaj_Zing" };
    public static readonly Npc XiGai = new() { Id = 3239, Name = "Xi Gai", WikiUrl = "https://wiki.guildwars.com/wiki/Xi_Gai" };
    public static readonly Npc XuFengxia = new() { Id = 3239, Name = "Xu Fengxia", WikiUrl = "https://wiki.guildwars.com/wiki/Xu_Fengxia" };
    public static readonly Npc XuweiDiyi = new() { Id = 3239, Name = "Xuwei Diyi", WikiUrl = "https://wiki.guildwars.com/wiki/Xuwei_Diyi" };
    public static readonly Npc Yiwong = new() { Id = 3239, Name = "Yiwong", WikiUrl = "https://wiki.guildwars.com/wiki/Yiwong" };
    public static readonly Npc ZiinyingmaoKaga = new() { Id = 3239, Name = "Ziinyingmao Kaga", WikiUrl = "https://wiki.guildwars.com/wiki/Ziinyingmao_Kaga" };
    public static readonly Npc AdeptBishu = new() { Id = 3240, Name = "Adept Bishu", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_Bishu" };
    public static readonly Npc CanthanNoble3 = new() { Id = 3240, Name = "Canthan Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Noble" };
    public static readonly Npc HaiLae = new() { Id = 3240, Name = "Hai Lae", WikiUrl = "https://wiki.guildwars.com/wiki/Hai_Lae" };
    public static readonly Npc Junsu = new() { Id = 3240, Name = "Junsu", WikiUrl = "https://wiki.guildwars.com/wiki/Junsu" };
    public static readonly Npc LadyMukeiMusagi = new() { Id = 3240, Name = "Lady Mukei Musagi", WikiUrl = "https://wiki.guildwars.com/wiki/Lady_Mukei_Musagi" };
    public static readonly Npc Michiko1 = new() { Id = 3240, Name = "Michiko", WikiUrl = "https://wiki.guildwars.com/wiki/Michiko" };
    public static readonly Npc MinisterTao = new() { Id = 3240, Name = "Minister Tao", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Tao" };
    public static readonly Npc Baosen = new() { Id = 3243, Name = "Baosen", WikiUrl = "https://wiki.guildwars.com/wiki/Baosen" };
    public static readonly Npc BarkeepMehoro1 = new() { Id = 3243, Name = "Barkeep Mehoro", WikiUrl = "https://wiki.guildwars.com/wiki/Barkeep_Mehoro" };
    public static readonly Npc CanthanPeasant = new() { Id = 3243, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanRefugee = new() { Id = 3243, Name = "Canthan Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Refugee" };
    public static readonly Npc HomelessCanthan = new() { Id = 3243, Name = "Homeless Canthan", WikiUrl = "https://wiki.guildwars.com/wiki/Homeless_Canthan" };
    public static readonly Npc InformantTahzen = new() { Id = 3243, Name = "Informant Tahzen", WikiUrl = "https://wiki.guildwars.com/wiki/Informant_Tahzen" };
    public static readonly Npc JeLing = new() { Id = 3243, Name = "Je Ling", WikiUrl = "https://wiki.guildwars.com/wiki/Je_Ling" };
    public static readonly Npc Leijo = new() { Id = 3243, Name = "Leijo", WikiUrl = "https://wiki.guildwars.com/wiki/Leijo" };
    public static readonly Npc MarikKuri = new() { Id = 3243, Name = "Marik Kuri", WikiUrl = "https://wiki.guildwars.com/wiki/Marik_Kuri" };
    public static readonly Npc MillerQuang = new() { Id = 3243, Name = "Miller Quang", WikiUrl = "https://wiki.guildwars.com/wiki/Miller_Quang" };
    public static readonly Npc Nakai = new() { Id = 3243, Name = "Nakai", WikiUrl = "https://wiki.guildwars.com/wiki/Nakai" };
    public static readonly Npc Paoko = new() { Id = 3243, Name = "Paoko", WikiUrl = "https://wiki.guildwars.com/wiki/Paoko" };
    public static readonly Npc Raiyi = new() { Id = 3243, Name = "Raiyi", WikiUrl = "https://wiki.guildwars.com/wiki/Raiyi" };
    public static readonly Npc Refugee = new() { Id = 3243, Name = "Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Refugee" };
    public static readonly Npc SahnlaetheTamer = new() { Id = 3243, Name = "Sahnlae the Tamer", WikiUrl = "https://wiki.guildwars.com/wiki/Sahnlae_the_Tamer" };
    public static readonly Npc Shenzun = new() { Id = 3243, Name = "Shenzun", WikiUrl = "https://wiki.guildwars.com/wiki/Shenzun" };
    public static readonly Npc YanZal = new() { Id = 3243, Name = "Yan Zal", WikiUrl = "https://wiki.guildwars.com/wiki/Yan_Zal" };
    public static readonly Npc Yikaro = new() { Id = 3243, Name = "Yikaro", WikiUrl = "https://wiki.guildwars.com/wiki/Yikaro" };
    public static readonly Npc Baasong = new() { Id = 3244, Name = "Baasong", WikiUrl = "https://wiki.guildwars.com/wiki/Baasong" };
    public static readonly Npc BaeKwon = new() { Id = 3244, Name = "Bae Kwon", WikiUrl = "https://wiki.guildwars.com/wiki/Bae_Kwon" };
    public static readonly Npc Bujo1 = new() { Id = 3244, Name = "Bujo", WikiUrl = "https://wiki.guildwars.com/wiki/Bujo" };
    public static readonly Npc CanthanPeasant1 = new() { Id = 3244, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanRefugee1 = new() { Id = 3244, Name = "Canthan Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Refugee" };
    public static readonly Npc CorpseofThiefMang = new() { Id = 3244, Name = "Corpse of Thief Mang", WikiUrl = "https://wiki.guildwars.com/wiki/Corpse_of_Thief_Mang" };
    public static readonly Npc DaiWaeng = new() { Id = 3244, Name = "Dai Waeng", WikiUrl = "https://wiki.guildwars.com/wiki/Dai_Waeng" };
    public static readonly Npc FishmongerBihzun = new() { Id = 3244, Name = "Fishmonger Bihzun", WikiUrl = "https://wiki.guildwars.com/wiki/Fishmonger_Bihzun" };
    public static readonly Npc Gorobei = new() { Id = 3244, Name = "Gorobei", WikiUrl = "https://wiki.guildwars.com/wiki/Gorobei" };
    public static readonly Npc HoDim = new() { Id = 3244, Name = "Ho Dim", WikiUrl = "https://wiki.guildwars.com/wiki/Ho_Dim" };
    public static readonly Npc LosaiHapatu = new() { Id = 3244, Name = "Losai Hapatu", WikiUrl = "https://wiki.guildwars.com/wiki/Losai_Hapatu" };
    public static readonly Npc Morokam = new() { Id = 3244, Name = "Morokam", WikiUrl = "https://wiki.guildwars.com/wiki/Morokam" };
    public static readonly Npc Refugee1 = new() { Id = 3244, Name = "Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Refugee" };
    public static readonly Npc SenjoWah = new() { Id = 3244, Name = "Senjo Wah", WikiUrl = "https://wiki.guildwars.com/wiki/Senjo_Wah" };
    public static readonly Npc Sunzu = new() { Id = 3244, Name = "Sunzu", WikiUrl = "https://wiki.guildwars.com/wiki/Sunzu" };
    public static readonly Npc Taojo = new() { Id = 3244, Name = "Taojo", WikiUrl = "https://wiki.guildwars.com/wiki/Taojo" };
    public static readonly Npc Teizhan = new() { Id = 3244, Name = "Teizhan", WikiUrl = "https://wiki.guildwars.com/wiki/Teizhan" };
    public static readonly Npc CorpseofCanthanPeasant = new() { Id = 3245, Name = "Corpse of Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Corpse_of_Canthan_Peasant" };
    public static readonly Npc CanthanPeasant2 = new() { Id = 3247, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc Lintao = new() { Id = 3247, Name = "Lintao", WikiUrl = "https://wiki.guildwars.com/wiki/Lintao" };
    public static readonly Npc PoorBeggar = new() { Id = 3247, Name = "Poor Beggar", WikiUrl = "https://wiki.guildwars.com/wiki/Poor_Beggar" };
    public static readonly Npc ZumotheBeggar = new() { Id = 3247, Name = "Zumo the Beggar", WikiUrl = "https://wiki.guildwars.com/wiki/Zumo_the_Beggar" };
    public static readonly Npc CanthanPeasant3 = new() { Id = 3248, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc RansujuntheProphet = new() { Id = 3248, Name = "Ransujun the Prophet", WikiUrl = "https://wiki.guildwars.com/wiki/Ransujun_the_Prophet" };
    public static readonly Npc SongSanNok = new() { Id = 3248, Name = "Song San Nok", WikiUrl = "https://wiki.guildwars.com/wiki/Song_San_Nok" };
    public static readonly Npc CanthanPeasant4 = new() { Id = 3249, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanPeasant5 = new() { Id = 3250, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc FarmerXengJo = new() { Id = 3250, Name = "Farmer Xeng Jo", WikiUrl = "https://wiki.guildwars.com/wiki/Farmer_Xeng_Jo" };
    public static readonly Npc FarmerZinhao = new() { Id = 3250, Name = "Farmer Zinhao", WikiUrl = "https://wiki.guildwars.com/wiki/Farmer_Zinhao" };
    public static readonly Npc NeingtheTanner = new() { Id = 3250, Name = "Neing the Tanner", WikiUrl = "https://wiki.guildwars.com/wiki/Neing_the_Tanner" };
    public static readonly Npc Yanjo = new() { Id = 3250, Name = "Yanjo", WikiUrl = "https://wiki.guildwars.com/wiki/Yanjo" };
    public static readonly Npc CanthanPeasant6 = new() { Id = 3251, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc HaoZhang = new() { Id = 3251, Name = "Hao Zhang", WikiUrl = "https://wiki.guildwars.com/wiki/Hao_Zhang" };
    public static readonly Npc YuimotheMime = new() { Id = 3251, Name = "Yuimo the Mime", WikiUrl = "https://wiki.guildwars.com/wiki/Yuimo_the_Mime" };
    public static readonly Npc Akemi = new() { Id = 3252, Name = "Akemi", WikiUrl = "https://wiki.guildwars.com/wiki/Akemi" };
    public static readonly Npc CanthanPeasant7 = new() { Id = 3252, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanRefugee2 = new() { Id = 3252, Name = "Canthan Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Refugee" };
    public static readonly Npc ChibichitheTamer = new() { Id = 3252, Name = "Chibichi the Tamer", WikiUrl = "https://wiki.guildwars.com/wiki/Chibichi_the_Tamer" };
    public static readonly Npc DebiroKuri = new() { Id = 3252, Name = "Debiro Kuri", WikiUrl = "https://wiki.guildwars.com/wiki/Debiro_Kuri" };
    public static readonly Npc Faozun = new() { Id = 3252, Name = "Faozun", WikiUrl = "https://wiki.guildwars.com/wiki/Faozun" };
    public static readonly Npc HerderTsiyingju = new() { Id = 3252, Name = "Herder Tsiyingju", WikiUrl = "https://wiki.guildwars.com/wiki/Herder_Tsiyingju" };
    public static readonly Npc HomelessCanthan1 = new() { Id = 3252, Name = "Homeless Canthan", WikiUrl = "https://wiki.guildwars.com/wiki/Homeless_Canthan" };
    public static readonly Npc JajeNiya = new() { Id = 3252, Name = "Jaje Niya", WikiUrl = "https://wiki.guildwars.com/wiki/Jaje_Niya" };
    public static readonly Npc Jia = new() { Id = 3252, Name = "Jia", WikiUrl = "https://wiki.guildwars.com/wiki/Jia" };
    public static readonly Npc JinSiyan1 = new() { Id = 3252, Name = "Jin Siyan", WikiUrl = "https://wiki.guildwars.com/wiki/Jin_Siyan" };
    public static readonly Npc Kaya = new() { Id = 3252, Name = "Kaya", WikiUrl = "https://wiki.guildwars.com/wiki/Kaya" };
    public static readonly Npc Ling = new() { Id = 3252, Name = "Ling", WikiUrl = "https://wiki.guildwars.com/wiki/Ling" };
    public static readonly Npc Mangjo = new() { Id = 3252, Name = "Mangjo", WikiUrl = "https://wiki.guildwars.com/wiki/Mangjo" };
    public static readonly Npc Refugee2 = new() { Id = 3252, Name = "Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Refugee" };
    public static readonly Npc YuLae = new() { Id = 3252, Name = "Yu Lae", WikiUrl = "https://wiki.guildwars.com/wiki/Yu_Lae" };
    public static readonly Npc CorpseofCanthanPeasant1 = new() { Id = 3253, Name = "Corpse of Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Corpse_of_Canthan_Peasant" };
    public static readonly Npc CanthanPeasant8 = new() { Id = 3254, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc Jiangpo = new() { Id = 3254, Name = "Jiangpo", WikiUrl = "https://wiki.guildwars.com/wiki/Jiangpo" };
    public static readonly Npc Qian = new() { Id = 3254, Name = "Qian", WikiUrl = "https://wiki.guildwars.com/wiki/Qian" };
    public static readonly Npc TanrieoTuro = new() { Id = 3254, Name = "Tanrieo Tu'ro", WikiUrl = "https://wiki.guildwars.com/wiki/Tanrieo_Tu'ro" };
    public static readonly Npc ZinLao = new() { Id = 3254, Name = "Zin Lao", WikiUrl = "https://wiki.guildwars.com/wiki/Zin_Lao" };
    public static readonly Npc CanthanPeasant9 = new() { Id = 3255, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc JungZeng = new() { Id = 3257, Name = "Jung Zeng", WikiUrl = "https://wiki.guildwars.com/wiki/Jung_Zeng" };
    public static readonly Npc TahboPaa = new() { Id = 3258, Name = "Tahbo Paa", WikiUrl = "https://wiki.guildwars.com/wiki/Tahbo_Paa" };
    public static readonly Npc MiFai = new() { Id = 3260, Name = "Mi Fai", WikiUrl = "https://wiki.guildwars.com/wiki/Mi_Fai" };
    public static readonly Npc CanthanPeasant10 = new() { Id = 3262, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc Paomu = new() { Id = 3262, Name = "Paomu", WikiUrl = "https://wiki.guildwars.com/wiki/Paomu" };
    public static readonly Npc VillagerHijai = new() { Id = 3262, Name = "Villager Hijai", WikiUrl = "https://wiki.guildwars.com/wiki/Villager_Hijai" };
    public static readonly Npc Yazoying = new() { Id = 3262, Name = "Yazoying", WikiUrl = "https://wiki.guildwars.com/wiki/Yazoying" };
    public static readonly Npc AdeptNai = new() { Id = 3263, Name = "Adept Nai", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_Nai" };
    public static readonly Npc AdeptofBone = new() { Id = 3263, Name = "Adept of Bone", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Bone" };
    public static readonly Npc AdeptofIllusion = new() { Id = 3263, Name = "Adept of Illusion", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Illusion" };
    public static readonly Npc AdeptofLight = new() { Id = 3263, Name = "Adept of Light", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Light" };
    public static readonly Npc AdeptofNature = new() { Id = 3263, Name = "Adept of Nature", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Nature" };
    public static readonly Npc AdeptofScythes = new() { Id = 3263, Name = "Adept of Scythes", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Scythes" };
    public static readonly Npc AdeptofShadows = new() { Id = 3263, Name = "Adept of Shadows", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Shadows" };
    public static readonly Npc AdeptofSpears = new() { Id = 3263, Name = "Adept of Spears", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Spears" };
    public static readonly Npc AdeptofSpirits = new() { Id = 3263, Name = "Adept of Spirits", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Spirits" };
    public static readonly Npc AdeptofSteel = new() { Id = 3263, Name = "Adept of Steel", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Steel" };
    public static readonly Npc AdeptoftheElements = new() { Id = 3263, Name = "Adept of the Elements", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_the_Elements" };
    public static readonly Npc AdeptTahn = new() { Id = 3263, Name = "Adept Tahn", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_Tahn" };
    public static readonly Npc BihahtheScribe = new() { Id = 3263, Name = "Bihah the Scribe", WikiUrl = "https://wiki.guildwars.com/wiki/Bihah_the_Scribe" };
    public static readonly Npc BrotherHanjui = new() { Id = 3263, Name = "Brother Hanjui", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Hanjui" };
    public static readonly Npc BrotherKhaiJhong = new() { Id = 3263, Name = "Brother Khai Jhong", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Khai_Jhong" };
    public static readonly Npc BrotherSitai = new() { Id = 3263, Name = "Brother Sitai", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Sitai" };
    public static readonly Npc BrotherTosai = new() { Id = 3263, Name = "Brother Tosai", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Tosai" };
    public static readonly Npc Resongkai = new() { Id = 3263, Name = "Resongkai", WikiUrl = "https://wiki.guildwars.com/wiki/Resongkai" };
    public static readonly Npc TempleAcolyte = new() { Id = 3263, Name = "Temple Acolyte", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_Acolyte" };
    public static readonly Npc Tombo = new() { Id = 3263, Name = "Tombo", WikiUrl = "https://wiki.guildwars.com/wiki/Tombo" };
    public static readonly Npc AdeptKai = new() { Id = 3264, Name = "Adept Kai", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_Kai" };
    public static readonly Npc Shinichiro = new() { Id = 3265, Name = "Shinichiro", WikiUrl = "https://wiki.guildwars.com/wiki/Shinichiro" };
    public static readonly Npc Tsungfa = new() { Id = 3265, Name = "Tsungfa", WikiUrl = "https://wiki.guildwars.com/wiki/Tsungfa" };
    public static readonly Npc XueYi1 = new() { Id = 3265, Name = "Xue Yi", WikiUrl = "https://wiki.guildwars.com/wiki/Xue_Yi" };
    public static readonly Npc DaeChung = new() { Id = 3266, Name = "Dae Chung", WikiUrl = "https://wiki.guildwars.com/wiki/Dae_Chung" };
    public static readonly Npc Kainu = new() { Id = 3266, Name = "Kainu", WikiUrl = "https://wiki.guildwars.com/wiki/Kainu" };
    public static readonly Npc Kumiko1 = new() { Id = 3266, Name = "Kumiko", WikiUrl = "https://wiki.guildwars.com/wiki/Kumiko" };
    public static readonly Npc SisterChoiJu = new() { Id = 3266, Name = "Sister Choi Ju", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Choi_Ju" };
    public static readonly Npc TakaraTaji = new() { Id = 3266, Name = "Takara Taji", WikiUrl = "https://wiki.guildwars.com/wiki/Takara_Taji" };
    public static readonly Npc Yunndae = new() { Id = 3266, Name = "Yunndae", WikiUrl = "https://wiki.guildwars.com/wiki/Yunndae" };
    public static readonly Npc AkinBroadcrest = new() { Id = 3267, Name = "Akin Broadcrest", WikiUrl = "https://wiki.guildwars.com/wiki/Akin_Broadcrest" };
    public static readonly Npc PustEmberclaw = new() { Id = 3267, Name = "Pust Emberclaw", WikiUrl = "https://wiki.guildwars.com/wiki/Pust_Emberclaw" };
    public static readonly Npc RoostEverclaw = new() { Id = 3267, Name = "Roost Everclaw", WikiUrl = "https://wiki.guildwars.com/wiki/Roost_Everclaw" };
    public static readonly Npc FuguiGe = new() { Id = 3268, Name = "Fugui Ge", WikiUrl = "https://wiki.guildwars.com/wiki/Fugui_Ge" };
    public static readonly Npc Kakumei1 = new() { Id = 3268, Name = "Kakumei", WikiUrl = "https://wiki.guildwars.com/wiki/Kakumei" };
    public static readonly Npc Kambei = new() { Id = 3268, Name = "Kambei", WikiUrl = "https://wiki.guildwars.com/wiki/Kambei" };
    public static readonly Npc Koumei = new() { Id = 3268, Name = "Koumei", WikiUrl = "https://wiki.guildwars.com/wiki/Koumei" };
    public static readonly Npc NuLeng = new() { Id = 3268, Name = "Nu Leng", WikiUrl = "https://wiki.guildwars.com/wiki/Nu_Leng" };
    public static readonly Npc Seiji = new() { Id = 3268, Name = "Seiji", WikiUrl = "https://wiki.guildwars.com/wiki/Seiji" };
    public static readonly Npc VoldotheExotic = new() { Id = 3268, Name = "Voldo the Exotic", WikiUrl = "https://wiki.guildwars.com/wiki/Voldo_the_Exotic" };
    public static readonly Npc Lain = new() { Id = 3269, Name = "Lain", WikiUrl = "https://wiki.guildwars.com/wiki/Lain" };
    public static readonly Npc Maeko = new() { Id = 3269, Name = "Maeko", WikiUrl = "https://wiki.guildwars.com/wiki/Maeko" };
    public static readonly Npc Maiya = new() { Id = 3269, Name = "Maiya", WikiUrl = "https://wiki.guildwars.com/wiki/Maiya" };
    public static readonly Npc MoonAhn = new() { Id = 3269, Name = "Moon Ahn", WikiUrl = "https://wiki.guildwars.com/wiki/Moon_Ahn" };
    public static readonly Npc Oroku = new() { Id = 3269, Name = "Oroku", WikiUrl = "https://wiki.guildwars.com/wiki/Oroku" };
    public static readonly Npc Ryoko1 = new() { Id = 3269, Name = "Ryoko", WikiUrl = "https://wiki.guildwars.com/wiki/Ryoko" };
    public static readonly Npc Suki1 = new() { Id = 3269, Name = "Suki", WikiUrl = "https://wiki.guildwars.com/wiki/Suki" };
    public static readonly Npc WeiQi = new() { Id = 3269, Name = "Wei Qi", WikiUrl = "https://wiki.guildwars.com/wiki/Wei_Qi" };
    public static readonly Npc XunlaiGiftGiverGurubei = new() { Id = 3269, Name = "Xunlai Gift-Giver Gurubei", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Gift-Giver_Gurubei" };
    public static readonly Npc FireworksMaster = new() { Id = 3270, Name = "Fireworks Master", WikiUrl = "https://wiki.guildwars.com/wiki/Fireworks_Master" };
    public static readonly Npc DoctorJungsRemediesandPotions = new() { Id = 3271, Name = "Doctor Jung's Remedies and Potions", WikiUrl = "https://wiki.guildwars.com/wiki/Doctor_Jung's_Remedies_and_Potions" };
    public static readonly Npc ErMing = new() { Id = 3271, Name = "Er Ming", WikiUrl = "https://wiki.guildwars.com/wiki/Er_Ming" };
    public static readonly Npc GiHahn = new() { Id = 3271, Name = "Gi Hahn", WikiUrl = "https://wiki.guildwars.com/wiki/Gi_Hahn" };
    public static readonly Npc Golgo = new() { Id = 3271, Name = "Golgo", WikiUrl = "https://wiki.guildwars.com/wiki/Golgo" };
    public static readonly Npc GongMei = new() { Id = 3271, Name = "Gong Mei", WikiUrl = "https://wiki.guildwars.com/wiki/Gong_Mei" };
    public static readonly Npc Masahiko = new() { Id = 3271, Name = "Masahiko", WikiUrl = "https://wiki.guildwars.com/wiki/Masahiko" };
    public static readonly Npc Morimoto = new() { Id = 3271, Name = "Morimoto", WikiUrl = "https://wiki.guildwars.com/wiki/Morimoto" };
    public static readonly Npc NaijusRemediesandPotions = new() { Id = 3271, Name = "Naiju's Remedies and Potions", WikiUrl = "https://wiki.guildwars.com/wiki/Naiju's_Remedies_and_Potions" };
    public static readonly Npc PeLingsRemediesandPotions = new() { Id = 3271, Name = "Pe Ling's Remedies and Potions", WikiUrl = "https://wiki.guildwars.com/wiki/Pe_Ling's_Remedies_and_Potions" };
    public static readonly Npc RaiKazuRemediesandPotions = new() { Id = 3271, Name = "Rai Kazu Remedies and Potions", WikiUrl = "https://wiki.guildwars.com/wiki/Rai_Kazu_Remedies_and_Potions" };
    public static readonly Npc XangsRemediesandPotions = new() { Id = 3271, Name = "Xang's Remedies and Potions", WikiUrl = "https://wiki.guildwars.com/wiki/Xang's_Remedies_and_Potions" };
    public static readonly Npc Hasung = new() { Id = 3272, Name = "Hasung", WikiUrl = "https://wiki.guildwars.com/wiki/Hasung" };
    public static readonly Npc Motiro = new() { Id = 3272, Name = "Motiro", WikiUrl = "https://wiki.guildwars.com/wiki/Motiro" };
    public static readonly Npc Ryukichi1 = new() { Id = 3272, Name = "Ryukichi", WikiUrl = "https://wiki.guildwars.com/wiki/Ryukichi" };
    public static readonly Npc ChikyuShujin = new() { Id = 3273, Name = "Chikyu Shujin", WikiUrl = "https://wiki.guildwars.com/wiki/Chikyu_Shujin" };
    public static readonly Npc HoJun = new() { Id = 3273, Name = "Ho Jun", WikiUrl = "https://wiki.guildwars.com/wiki/Ho_Jun" };
    public static readonly Npc Inugami = new() { Id = 3273, Name = "Inugami", WikiUrl = "https://wiki.guildwars.com/wiki/Inugami" };
    public static readonly Npc CrawRazorbeak = new() { Id = 3274, Name = "Craw Razorbeak", WikiUrl = "https://wiki.guildwars.com/wiki/Craw_Razorbeak" };
    public static readonly Npc MelkBrightfeather = new() { Id = 3274, Name = "Melk Brightfeather", WikiUrl = "https://wiki.guildwars.com/wiki/Melk_Brightfeather" };
    public static readonly Npc ZenSwiftwing = new() { Id = 3274, Name = "Zen Swiftwing", WikiUrl = "https://wiki.guildwars.com/wiki/Zen_Swiftwing" };
    public static readonly Npc Aiko1 = new() { Id = 3275, Name = "Aiko", WikiUrl = "https://wiki.guildwars.com/wiki/Aiko" };
    public static readonly Npc Asako = new() { Id = 3275, Name = "Asako", WikiUrl = "https://wiki.guildwars.com/wiki/Asako" };
    public static readonly Npc ChifaoTan = new() { Id = 3275, Name = "Chifao Tan", WikiUrl = "https://wiki.guildwars.com/wiki/Chifao_Tan" };
    public static readonly Npc Haruko = new() { Id = 3275, Name = "Haruko", WikiUrl = "https://wiki.guildwars.com/wiki/Haruko" };
    public static readonly Npc Huxeng = new() { Id = 3275, Name = "Huxeng", WikiUrl = "https://wiki.guildwars.com/wiki/Huxeng" };
    public static readonly Npc Jichow = new() { Id = 3275, Name = "Jichow", WikiUrl = "https://wiki.guildwars.com/wiki/Jichow" };
    public static readonly Npc KaiyaJaja = new() { Id = 3275, Name = "Kaiya Jaja", WikiUrl = "https://wiki.guildwars.com/wiki/Kaiya_Jaja" };
    public static readonly Npc Kokiri = new() { Id = 3275, Name = "Kokiri", WikiUrl = "https://wiki.guildwars.com/wiki/Kokiri" };
    public static readonly Npc Lei = new() { Id = 3275, Name = "Lei", WikiUrl = "https://wiki.guildwars.com/wiki/Lei" };
    public static readonly Npc LoYing = new() { Id = 3275, Name = "Lo Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Lo_Ying" };
    public static readonly Npc Nanako = new() { Id = 3275, Name = "Nanako", WikiUrl = "https://wiki.guildwars.com/wiki/Nanako" };
    public static readonly Npc Natsuko1 = new() { Id = 3275, Name = "Natsuko", WikiUrl = "https://wiki.guildwars.com/wiki/Natsuko" };
    public static readonly Npc Sen = new() { Id = 3275, Name = "Sen", WikiUrl = "https://wiki.guildwars.com/wiki/Sen" };
    public static readonly Npc Suki2 = new() { Id = 3275, Name = "Suki", WikiUrl = "https://wiki.guildwars.com/wiki/Suki" };
    public static readonly Npc Buhraa = new() { Id = 3276, Name = "Buhraa", WikiUrl = "https://wiki.guildwars.com/wiki/Buhraa" };
    public static readonly Npc Hiroyuki = new() { Id = 3276, Name = "Hiroyuki", WikiUrl = "https://wiki.guildwars.com/wiki/Hiroyuki" };
    public static readonly Npc HongleiSun = new() { Id = 3276, Name = "Honglei Sun", WikiUrl = "https://wiki.guildwars.com/wiki/Honglei_Sun" };
    public static readonly Npc LiuGong = new() { Id = 3276, Name = "Liu Gong", WikiUrl = "https://wiki.guildwars.com/wiki/Liu_Gong" };
    public static readonly Npc Manzo1 = new() { Id = 3276, Name = "Manzo", WikiUrl = "https://wiki.guildwars.com/wiki/Manzo" };
    public static readonly Npc Nago = new() { Id = 3276, Name = "Nago", WikiUrl = "https://wiki.guildwars.com/wiki/Nago" };
    public static readonly Npc Sheco = new() { Id = 3276, Name = "Sheco", WikiUrl = "https://wiki.guildwars.com/wiki/Sheco" };
    public static readonly Npc Shichiroji = new() { Id = 3276, Name = "Shichiroji", WikiUrl = "https://wiki.guildwars.com/wiki/Shichiroji" };
    public static readonly Npc TeipaoTahliwaj = new() { Id = 3276, Name = "Teipao Tahliwaj", WikiUrl = "https://wiki.guildwars.com/wiki/Teipao_Tahliwaj" };
    public static readonly Npc Tsukare = new() { Id = 3276, Name = "Tsukare", WikiUrl = "https://wiki.guildwars.com/wiki/Tsukare" };
    public static readonly Npc Zingyao = new() { Id = 3276, Name = "Zingyao", WikiUrl = "https://wiki.guildwars.com/wiki/Zingyao" };
    public static readonly Npc HaiJeling = new() { Id = 3277, Name = "Hai Jeling", WikiUrl = "https://wiki.guildwars.com/wiki/Hai_Jeling" };
    public static readonly Npc Taura = new() { Id = 3277, Name = "Taura", WikiUrl = "https://wiki.guildwars.com/wiki/Taura" };
    public static readonly Npc Ziyi = new() { Id = 3277, Name = "Ziyi", WikiUrl = "https://wiki.guildwars.com/wiki/Ziyi" };
    public static readonly Npc Senji = new() { Id = 3278, Name = "Senji", WikiUrl = "https://wiki.guildwars.com/wiki/Senji" };
    public static readonly Npc Vizu = new() { Id = 3279, Name = "Vizu", WikiUrl = "https://wiki.guildwars.com/wiki/Vizu" };
    public static readonly Npc Suun = new() { Id = 3280, Name = "Suun", WikiUrl = "https://wiki.guildwars.com/wiki/Suun" };
    public static readonly Npc Yohei = new() { Id = 3281, Name = "Yohei", WikiUrl = "https://wiki.guildwars.com/wiki/Yohei" };
    public static readonly Npc Yumiko = new() { Id = 3283, Name = "Yumiko", WikiUrl = "https://wiki.guildwars.com/wiki/Yumiko" };
    public static readonly Npc Akina = new() { Id = 3284, Name = "Akina", WikiUrl = "https://wiki.guildwars.com/wiki/Akina" };
    public static readonly Npc Oki = new() { Id = 3284, Name = "Oki", WikiUrl = "https://wiki.guildwars.com/wiki/Oki" };
    public static readonly Npc XueFang1 = new() { Id = 3284, Name = "Xue Fang", WikiUrl = "https://wiki.guildwars.com/wiki/Xue_Fang" };
    public static readonly Npc SihungLung = new() { Id = 3285, Name = "Sihung Lung", WikiUrl = "https://wiki.guildwars.com/wiki/Sihung_Lung" };
    public static readonly Npc Yimou = new() { Id = 3285, Name = "Yimou", WikiUrl = "https://wiki.guildwars.com/wiki/Yimou" };
    public static readonly Npc Yutake1 = new() { Id = 3285, Name = "Yutake", WikiUrl = "https://wiki.guildwars.com/wiki/Yutake" };
    public static readonly Npc SanLing = new() { Id = 3286, Name = "San Ling", WikiUrl = "https://wiki.guildwars.com/wiki/San_Ling" };
    public static readonly Npc KyoukoMaterial = new() { Id = 3287, Name = "Kyouko Material", WikiUrl = "https://wiki.guildwars.com/wiki/Kyouko_Material" };
    public static readonly Npc YongYiMaterial = new() { Id = 3287, Name = "Yong Yi Material", WikiUrl = "https://wiki.guildwars.com/wiki/Yong_Yi_Material" };
    public static readonly Npc JiazhenLiMaterial1 = new() { Id = 3288, Name = "Jiazhen Li Material", WikiUrl = "https://wiki.guildwars.com/wiki/Jiazhen_Li_Material" };
    public static readonly Npc SonglianMaterial = new() { Id = 3288, Name = "Songlian Material", WikiUrl = "https://wiki.guildwars.com/wiki/Songlian_Material" };
    public static readonly Npc ZhouPakScroll1 = new() { Id = 3289, Name = "Zhou Pak Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Zhou_Pak_Scroll" };
    public static readonly Npc Mitsunari1 = new() { Id = 3291, Name = "Mitsunari", WikiUrl = "https://wiki.guildwars.com/wiki/Mitsunari" };
    public static readonly Npc Okamoto = new() { Id = 3291, Name = "Okamoto", WikiUrl = "https://wiki.guildwars.com/wiki/Okamoto" };
    public static readonly Npc Yukito = new() { Id = 3291, Name = "Yukito", WikiUrl = "https://wiki.guildwars.com/wiki/Yukito" };
    public static readonly Npc Panaku2 = new() { Id = 3294, Name = "Panaku", WikiUrl = "https://wiki.guildwars.com/wiki/Panaku" };
    public static readonly Npc Panaku3 = new() { Id = 3295, Name = "Panaku", WikiUrl = "https://wiki.guildwars.com/wiki/Panaku" };
    public static readonly Npc VunYing = new() { Id = 3296, Name = "Vun Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Vun_Ying" };
    public static readonly Npc Jinzo = new() { Id = 3297, Name = "Jinzo", WikiUrl = "https://wiki.guildwars.com/wiki/Jinzo" };
    public static readonly Npc Jinzo1 = new() { Id = 3298, Name = "Jinzo", WikiUrl = "https://wiki.guildwars.com/wiki/Jinzo" };
    public static readonly Npc Jinzo2 = new() { Id = 3299, Name = "Jinzo", WikiUrl = "https://wiki.guildwars.com/wiki/Jinzo" };
    public static readonly Npc MeiLing1 = new() { Id = 3300, Name = "Mei Ling", WikiUrl = "https://wiki.guildwars.com/wiki/Mei_Ling" };
    public static readonly Npc LoSha1 = new() { Id = 3302, Name = "Lo Sha", WikiUrl = "https://wiki.guildwars.com/wiki/Lo_Sha" };
    public static readonly Npc Su1 = new() { Id = 3305, Name = "Su", WikiUrl = "https://wiki.guildwars.com/wiki/Su" };
    public static readonly Npc RengKu = new() { Id = 3306, Name = "Reng Ku", WikiUrl = "https://wiki.guildwars.com/wiki/Reng_Ku" };
    public static readonly Npc Ronsu = new() { Id = 3307, Name = "Ronsu", WikiUrl = "https://wiki.guildwars.com/wiki/Ronsu" };
    public static readonly Npc Ronsu1 = new() { Id = 3308, Name = "Ronsu", WikiUrl = "https://wiki.guildwars.com/wiki/Ronsu" };
    public static readonly Npc KaiYing1 = new() { Id = 3309, Name = "Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Kai_Ying" };
    public static readonly Npc KaiYing2 = new() { Id = 3310, Name = "Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Kai_Ying" };
    public static readonly Npc SisterTai1 = new() { Id = 3311, Name = "Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Tai" };
    public static readonly Npc SisterTai2 = new() { Id = 3312, Name = "Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Tai" };
    public static readonly Npc BrotherPeWan = new() { Id = 3313, Name = "Brother Pe Wan", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Pe_Wan" };
    public static readonly Npc BrotherPeWan1 = new() { Id = 3314, Name = "Brother Pe Wan", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Pe_Wan" };
    public static readonly Npc BrotherPeWan2 = new() { Id = 3315, Name = "Brother Pe Wan", WikiUrl = "https://wiki.guildwars.com/wiki/Brother_Pe_Wan" };
    public static readonly Npc WengGha = new() { Id = 3316, Name = "Weng Gha", WikiUrl = "https://wiki.guildwars.com/wiki/Weng_Gha" };
    public static readonly Npc WengGha1 = new() { Id = 3317, Name = "Weng Gha", WikiUrl = "https://wiki.guildwars.com/wiki/Weng_Gha" };
    public static readonly Npc SoarHonorclaw = new() { Id = 3319, Name = "Soar Honorclaw", WikiUrl = "https://wiki.guildwars.com/wiki/Soar_Honorclaw" };
    public static readonly Npc Sujun = new() { Id = 3321, Name = "Sujun", WikiUrl = "https://wiki.guildwars.com/wiki/Sujun" };
    public static readonly Npc Sujun1 = new() { Id = 3322, Name = "Sujun", WikiUrl = "https://wiki.guildwars.com/wiki/Sujun" };
    public static readonly Npc Zho1 = new() { Id = 3324, Name = "Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Zho" };
    public static readonly Npc Zho2 = new() { Id = 3325, Name = "Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Zho" };
    public static readonly Npc ProfessorGai1 = new() { Id = 3326, Name = "Professor Gai", WikiUrl = "https://wiki.guildwars.com/wiki/Professor_Gai" };
    public static readonly Npc ProfessorGai2 = new() { Id = 3327, Name = "Professor Gai", WikiUrl = "https://wiki.guildwars.com/wiki/Professor_Gai" };
    public static readonly Npc AngtheEphemeral = new() { Id = 3328, Name = "Ang the Ephemeral", WikiUrl = "https://wiki.guildwars.com/wiki/Ang_the_Ephemeral" };
    public static readonly Npc HeadmasterLee = new() { Id = 3329, Name = "Headmaster Lee", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Lee" };
    public static readonly Npc HeadmasterKaa = new() { Id = 3330, Name = "Headmaster Kaa", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Kaa" };
    public static readonly Npc HeadmasterKuju = new() { Id = 3331, Name = "Headmaster Kuju", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Kuju" };
    public static readonly Npc HeadmasterVhang2 = new() { Id = 3332, Name = "Headmaster Vhang", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Vhang" };
    public static readonly Npc HeadmasterAmara = new() { Id = 3333, Name = "Headmaster Amara", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Amara" };
    public static readonly Npc HeadmasterZhan = new() { Id = 3334, Name = "Headmaster Zhan", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Zhan" };
    public static readonly Npc HeadmasterGreico = new() { Id = 3335, Name = "Headmaster Greico", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Greico" };
    public static readonly Npc HeadmasterQuin = new() { Id = 3336, Name = "Headmaster Quin", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Quin" };
    public static readonly Npc GeneralKaimerVasburg = new() { Id = 3344, Name = "General Kaimer Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/General_Kaimer_Vasburg" };
    public static readonly Npc KurzickJuggernaut1 = new() { Id = 3345, Name = "Kurzick Juggernaut", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Juggernaut" };
    public static readonly Npc KurzickAssassin = new() { Id = 3346, Name = "Kurzick Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Assassin" };
    public static readonly Npc KurzickScout = new() { Id = 3346, Name = "Kurzick Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Scout" };
    public static readonly Npc KurzickMesmer = new() { Id = 3347, Name = "Kurzick Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Mesmer" };
    public static readonly Npc KurzickNecromancer1 = new() { Id = 3348, Name = "Kurzick Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Necromancer" };
    public static readonly Npc KurzickElementalist = new() { Id = 3349, Name = "Kurzick Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Elementalist" };
    public static readonly Npc KurzickMonk = new() { Id = 3350, Name = "Kurzick Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Monk" };
    public static readonly Npc KurzickWarrior = new() { Id = 3351, Name = "Kurzick Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Warrior" };
    public static readonly Npc KurzickRanger = new() { Id = 3352, Name = "Kurzick Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Ranger" };
    public static readonly Npc KurzickRitualist = new() { Id = 3353, Name = "Kurzick Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Ritualist" };
    public static readonly Npc HouseDurheimDuelist = new() { Id = 3356, Name = "House Durheim Duelist", WikiUrl = "https://wiki.guildwars.com/wiki/House_Durheim_Duelist" };
    public static readonly Npc HouseDurheimDuelist1 = new() { Id = 3357, Name = "House Durheim Duelist", WikiUrl = "https://wiki.guildwars.com/wiki/House_Durheim_Duelist" };
    public static readonly Npc HouseDurheimDuelist2 = new() { Id = 3358, Name = "House Durheim Duelist", WikiUrl = "https://wiki.guildwars.com/wiki/House_Durheim_Duelist" };
    public static readonly Npc HouseDurheimDuelist3 = new() { Id = 3359, Name = "House Durheim Duelist", WikiUrl = "https://wiki.guildwars.com/wiki/House_Durheim_Duelist" };
    public static readonly Npc GreenCarrierJuggernaut = new() { Id = 3363, Name = "Green Carrier Juggernaut", WikiUrl = "https://wiki.guildwars.com/wiki/Green_Carrier_Juggernaut" };
    public static readonly Npc GuardsmanOldrich = new() { Id = 3363, Name = "Guardsman Oldrich", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Oldrich" };
    public static readonly Npc PurpleCarrierJuggernaut = new() { Id = 3363, Name = "Purple Carrier Juggernaut", WikiUrl = "https://wiki.guildwars.com/wiki/Purple_Carrier_Juggernaut" };
    public static readonly Npc YellowCarrierJuggernaut = new() { Id = 3363, Name = "Yellow Carrier Juggernaut", WikiUrl = "https://wiki.guildwars.com/wiki/Yellow_Carrier_Juggernaut" };
    public static readonly Npc EliteJuggernautBerta = new() { Id = 3364, Name = "Elite Juggernaut Berta", WikiUrl = "https://wiki.guildwars.com/wiki/Elite_Juggernaut_Berta" };
    public static readonly Npc EliteJuggernautKlaus = new() { Id = 3364, Name = "Elite Juggernaut Klaus", WikiUrl = "https://wiki.guildwars.com/wiki/Elite_Juggernaut_Klaus" };
    public static readonly Npc EliteJuggernautLeiber = new() { Id = 3364, Name = "Elite Juggernaut Leiber", WikiUrl = "https://wiki.guildwars.com/wiki/Elite_Juggernaut_Leiber" };
    public static readonly Npc KurzickAssassin1 = new() { Id = 3365, Name = "Kurzick Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Assassin" };
    public static readonly Npc KurzickMesmer1 = new() { Id = 3366, Name = "Kurzick Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Mesmer" };
    public static readonly Npc KurzickElementalist1 = new() { Id = 3367, Name = "Kurzick Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Elementalist" };
    public static readonly Npc KurzickWarrior1 = new() { Id = 3369, Name = "Kurzick Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Warrior" };
    public static readonly Npc KurzickRanger1 = new() { Id = 3370, Name = "Kurzick Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Ranger" };
    public static readonly Npc KurzickSoldier = new() { Id = 3373, Name = "Kurzick Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Soldier" };
    public static readonly Npc KurzickSoldier1 = new() { Id = 3374, Name = "Kurzick Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Soldier" };
    public static readonly Npc TerrikzuHeltzer = new() { Id = 3374, Name = "Terrik zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Terrik_zu_Heltzer" };
    public static readonly Npc TreeSinger = new() { Id = 3374, Name = "Tree Singer", WikiUrl = "https://wiki.guildwars.com/wiki/Tree_Singer" };
    public static readonly Npc RaldzuHeltzer = new() { Id = 3377, Name = "Rald zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Rald_zu_Heltzer" };
    public static readonly Npc ErikLutgardis = new() { Id = 3378, Name = "Erik Lutgardis", WikiUrl = "https://wiki.guildwars.com/wiki/Erik_Lutgardis" };
    public static readonly Npc JohannzuHeltzer = new() { Id = 3378, Name = "Johann zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Johann_zu_Heltzer" };
    public static readonly Npc KommandantDurheim1 = new() { Id = 3378, Name = "Kommandant Durheim", WikiUrl = "https://wiki.guildwars.com/wiki/Kommandant_Durheim" };
    public static readonly Npc KonradVasburg = new() { Id = 3378, Name = "Konrad Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Konrad_Vasburg" };
    public static readonly Npc RuprechtBrauer = new() { Id = 3378, Name = "Ruprecht Brauer", WikiUrl = "https://wiki.guildwars.com/wiki/Ruprecht_Brauer" };
    public static readonly Npc RutgerzuHeltzer = new() { Id = 3378, Name = "Rutger zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Rutger_zu_Heltzer" };
    public static readonly Npc StoneSingerDalf = new() { Id = 3379, Name = "Stone Singer Dalf", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Singer_Dalf" };
    public static readonly Npc StoneSingerJarek = new() { Id = 3379, Name = "Stone Singer Jarek", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Singer_Jarek" };
    public static readonly Npc StoneSingerKasia = new() { Id = 3379, Name = "Stone Singer Kasia", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Singer_Kasia" };
    public static readonly Npc StoneSingerLotte = new() { Id = 3379, Name = "Stone Singer Lotte", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Singer_Lotte" };
    public static readonly Npc StoneSingerMinka = new() { Id = 3379, Name = "Stone Singer Minka", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Singer_Minka" };
    public static readonly Npc StoneSingerOswalt = new() { Id = 3379, Name = "Stone Singer Oswalt", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Singer_Oswalt" };
    public static readonly Npc StoneSingerTreff = new() { Id = 3379, Name = "Stone Singer Treff", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Singer_Treff" };
    public static readonly Npc StoneSingerWira = new() { Id = 3379, Name = "Stone Singer Wira", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Singer_Wira" };
    public static readonly Npc DenosMakaluum = new() { Id = 3380, Name = "Denos Makaluum", WikiUrl = "https://wiki.guildwars.com/wiki/Denos_Makaluum" };
    public static readonly Npc DmitriScharkoff = new() { Id = 3380, Name = "Dmitri Scharkoff", WikiUrl = "https://wiki.guildwars.com/wiki/Dmitri_Scharkoff" };
    public static readonly Npc GatekeeperBecker = new() { Id = 3380, Name = "Gatekeeper Becker", WikiUrl = "https://wiki.guildwars.com/wiki/Gatekeeper_Becker" };
    public static readonly Npc GatekeeperDedrick = new() { Id = 3380, Name = "Gatekeeper Dedrick", WikiUrl = "https://wiki.guildwars.com/wiki/Gatekeeper_Dedrick" };
    public static readonly Npc GuardKarsten = new() { Id = 3380, Name = "Guard Karsten", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Karsten" };
    public static readonly Npc GuardUwe = new() { Id = 3380, Name = "Guard Uwe", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Uwe" };
    public static readonly Npc KurzickGuard = new() { Id = 3380, Name = "Kurzick Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Guard" };
    public static readonly Npc KurzickQuartermaster = new() { Id = 3380, Name = "Kurzick Quartermaster", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Quartermaster" };
    public static readonly Npc LiftOperatorShulz = new() { Id = 3380, Name = "Lift Operator Shulz", WikiUrl = "https://wiki.guildwars.com/wiki/Lift_Operator_Shulz" };
    public static readonly Npc RecruiterSigmund = new() { Id = 3380, Name = "Recruiter Sigmund", WikiUrl = "https://wiki.guildwars.com/wiki/Recruiter_Sigmund" };
    public static readonly Npc ScoutmasterArne = new() { Id = 3380, Name = "Scoutmaster Arne", WikiUrl = "https://wiki.guildwars.com/wiki/Scoutmaster_Arne" };
    public static readonly Npc YuriVasburg = new() { Id = 3380, Name = "Yuri Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Yuri_Vasburg" };
    public static readonly Npc GuardCaptainMirkoz = new() { Id = 3381, Name = "Guard Captain Mirkoz", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Captain_Mirkoz" };
    public static readonly Npc SergeantGeinrich = new() { Id = 3381, Name = "Sergeant Geinrich", WikiUrl = "https://wiki.guildwars.com/wiki/Sergeant_Geinrich" };
    public static readonly Npc SergeantRodrik = new() { Id = 3381, Name = "Sergeant Rodrik", WikiUrl = "https://wiki.guildwars.com/wiki/Sergeant_Rodrik" };
    public static readonly Npc WarCaptainWomak = new() { Id = 3381, Name = "War Captain Womak", WikiUrl = "https://wiki.guildwars.com/wiki/War_Captain_Womak" };
    public static readonly Npc ForestermasterVasha = new() { Id = 3384, Name = "Forestermaster Vasha", WikiUrl = "https://wiki.guildwars.com/wiki/Forestermaster_Vasha" };
    public static readonly Npc AlshomGhislaun = new() { Id = 3388, Name = "Alshom Ghislaun", WikiUrl = "https://wiki.guildwars.com/wiki/Alshom_Ghislaun" };
    public static readonly Npc ArtorBobaniKiroz = new() { Id = 3388, Name = "Artor Bobani Kiroz", WikiUrl = "https://wiki.guildwars.com/wiki/Artor_Bobani_Kiroz" };
    public static readonly Npc HistorianOfHouses = new() { Id = 3388, Name = "Historian Of Houses", WikiUrl = "https://wiki.guildwars.com/wiki/Historian_Of_Houses" };
    public static readonly Npc JaunStumi = new() { Id = 3388, Name = "Jaun Stumi", WikiUrl = "https://wiki.guildwars.com/wiki/Jaun_Stumi" };
    public static readonly Npc JonnTertehl = new() { Id = 3388, Name = "Jonn Tertehl", WikiUrl = "https://wiki.guildwars.com/wiki/Jonn_Tertehl" };
    public static readonly Npc JustanWeiss = new() { Id = 3388, Name = "Justan Weiss", WikiUrl = "https://wiki.guildwars.com/wiki/Justan_Weiss" };
    public static readonly Npc KurzickPeasant = new() { Id = 3388, Name = "Kurzick Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Peasant" };
    public static readonly Npc KurzickTraveler = new() { Id = 3388, Name = "Kurzick Traveler", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Traveler" };
    public static readonly Npc ChefArmand = new() { Id = 3389, Name = "Chef Armand", WikiUrl = "https://wiki.guildwars.com/wiki/Chef_Armand" };
    public static readonly Npc DyeMasterFranjek = new() { Id = 3389, Name = "Dye Master Franjek", WikiUrl = "https://wiki.guildwars.com/wiki/Dye_Master_Franjek" };
    public static readonly Npc Gorani = new() { Id = 3389, Name = "Gorani", WikiUrl = "https://wiki.guildwars.com/wiki/Gorani" };
    public static readonly Npc KurzickTraveler1 = new() { Id = 3389, Name = "Kurzick Traveler", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Traveler" };
    public static readonly Npc LoremasterZin = new() { Id = 3389, Name = "Loremaster Zin", WikiUrl = "https://wiki.guildwars.com/wiki/Loremaster_Zin" };
    public static readonly Npc PeltsmanJiri = new() { Id = 3389, Name = "Peltsman Jiri", WikiUrl = "https://wiki.guildwars.com/wiki/Peltsman_Jiri" };
    public static readonly Npc VasiliLutgardis = new() { Id = 3389, Name = "Vasili Lutgardis", WikiUrl = "https://wiki.guildwars.com/wiki/Vasili_Lutgardis" };
    public static readonly Npc WilhelmJoseph = new() { Id = 3389, Name = "Wilhelm Joseph", WikiUrl = "https://wiki.guildwars.com/wiki/Wilhelm_Joseph" };
    public static readonly Npc GerlindaKorbauch = new() { Id = 3390, Name = "Gerlinda Korbauch", WikiUrl = "https://wiki.guildwars.com/wiki/Gerlinda_Korbauch" };
    public static readonly Npc KurzickPeasant1 = new() { Id = 3390, Name = "Kurzick Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Peasant" };
    public static readonly Npc HealerSilja = new() { Id = 3391, Name = "Healer Silja", WikiUrl = "https://wiki.guildwars.com/wiki/Healer_Silja" };
    public static readonly Npc Jun = new() { Id = 3391, Name = "Jun", WikiUrl = "https://wiki.guildwars.com/wiki/Jun" };
    public static readonly Npc KurzickPeasant2 = new() { Id = 3391, Name = "Kurzick Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Peasant" };
    public static readonly Npc KurzickTraveler2 = new() { Id = 3391, Name = "Kurzick Traveler", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Traveler" };
    public static readonly Npc AndruPitrak = new() { Id = 3392, Name = "Andru Pitrak", WikiUrl = "https://wiki.guildwars.com/wiki/Andru_Pitrak" };
    public static readonly Npc CountDurheim1 = new() { Id = 3392, Name = "Count Durheim", WikiUrl = "https://wiki.guildwars.com/wiki/Count_Durheim" };
    public static readonly Npc DeaconFredek = new() { Id = 3392, Name = "Deacon Fredek", WikiUrl = "https://wiki.guildwars.com/wiki/Deacon_Fredek" };
    public static readonly Npc DunmelGorhopf = new() { Id = 3392, Name = "Dunmel Gorhopf", WikiUrl = "https://wiki.guildwars.com/wiki/Dunmel_Gorhopf" };
    public static readonly Npc Friedrich = new() { Id = 3392, Name = "Friedrich", WikiUrl = "https://wiki.guildwars.com/wiki/Friedrich" };
    public static readonly Npc KurzickArchitect = new() { Id = 3392, Name = "Kurzick Architect", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Architect" };
    public static readonly Npc KurzickGatekeeper = new() { Id = 3392, Name = "Kurzick Gatekeeper", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Gatekeeper" };
    public static readonly Npc Meinrad = new() { Id = 3392, Name = "Meinrad", WikiUrl = "https://wiki.guildwars.com/wiki/Meinrad" };
    public static readonly Npc Mirko = new() { Id = 3392, Name = "Mirko", WikiUrl = "https://wiki.guildwars.com/wiki/Mirko" };
    public static readonly Npc RedemptorIszar = new() { Id = 3392, Name = "Redemptor Iszar", WikiUrl = "https://wiki.guildwars.com/wiki/Redemptor_Iszar" };
    public static readonly Npc SelikzuHeltzer = new() { Id = 3392, Name = "Selik zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Selik_zu_Heltzer" };
    public static readonly Npc SupplymasterKonrad = new() { Id = 3392, Name = "Supplymaster Konrad", WikiUrl = "https://wiki.guildwars.com/wiki/Supplymaster_Konrad" };
    public static readonly Npc ZarektheTamer = new() { Id = 3392, Name = "Zarek the Tamer", WikiUrl = "https://wiki.guildwars.com/wiki/Zarek_the_Tamer" };
    public static readonly Npc AdeilzuHeltzer = new() { Id = 3393, Name = "Adeil zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Adeil_zu_Heltzer" };
    public static readonly Npc CountArchekBrauer = new() { Id = 3393, Name = "Count Archek Brauer", WikiUrl = "https://wiki.guildwars.com/wiki/Count_Archek_Brauer" };
    public static readonly Npc DuelMasterVaughn = new() { Id = 3393, Name = "Duel Master Vaughn", WikiUrl = "https://wiki.guildwars.com/wiki/Duel_Master_Vaughn" };
    public static readonly Npc DukeHoltzLutgardis = new() { Id = 3393, Name = "Duke Holtz Lutgardis", WikiUrl = "https://wiki.guildwars.com/wiki/Duke_Holtz_Lutgardis" };
    public static readonly Npc Englebert = new() { Id = 3393, Name = "Englebert", WikiUrl = "https://wiki.guildwars.com/wiki/Englebert" };
    public static readonly Npc KurzickNoble = new() { Id = 3393, Name = "Kurzick Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Noble" };
    public static readonly Npc QuartermasterMikhail = new() { Id = 3393, Name = "Quartermaster Mikhail", WikiUrl = "https://wiki.guildwars.com/wiki/Quartermaster_Mikhail" };
    public static readonly Npc RedemptorKurcheck = new() { Id = 3393, Name = "Redemptor Kurcheck", WikiUrl = "https://wiki.guildwars.com/wiki/Redemptor_Kurcheck" };
    public static readonly Npc Ruben = new() { Id = 3393, Name = "Ruben", WikiUrl = "https://wiki.guildwars.com/wiki/Ruben" };
    public static readonly Npc Vash = new() { Id = 3393, Name = "Vash", WikiUrl = "https://wiki.guildwars.com/wiki/Vash" };
    public static readonly Npc Vernados = new() { Id = 3393, Name = "Vernados", WikiUrl = "https://wiki.guildwars.com/wiki/Vernados" };
    public static readonly Npc BaronessAttiaVasburg = new() { Id = 3394, Name = "Baroness Attia Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Baroness_Attia_Vasburg" };
    public static readonly Npc BrunazuHeltzer = new() { Id = 3394, Name = "Bruna zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Bruna_zu_Heltzer" };
    public static readonly Npc CountessSandraDurheim = new() { Id = 3394, Name = "Countess Sandra Durheim", WikiUrl = "https://wiki.guildwars.com/wiki/Countess_Sandra_Durheim" };
    public static readonly Npc KurzickNoble1 = new() { Id = 3394, Name = "Kurzick Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Noble" };
    public static readonly Npc MasterArchitectWright = new() { Id = 3394, Name = "Master Architect Wright", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Architect_Wright" };
    public static readonly Npc NadettazuHeltzer = new() { Id = 3394, Name = "Nadetta zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Nadetta_zu_Heltzer" };
    public static readonly Npc Zytka = new() { Id = 3394, Name = "Zytka", WikiUrl = "https://wiki.guildwars.com/wiki/Zytka" };
    public static readonly Npc Eileen = new() { Id = 3395, Name = "Eileen", WikiUrl = "https://wiki.guildwars.com/wiki/Eileen" };
    public static readonly Npc KurzickAmbassador = new() { Id = 3395, Name = "Kurzick Ambassador", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Ambassador" };
    public static readonly Npc KurzickGatekeeper1 = new() { Id = 3395, Name = "Kurzick Gatekeeper", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Gatekeeper" };
    public static readonly Npc KurzickNoble2 = new() { Id = 3395, Name = "Kurzick Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Noble" };
    public static readonly Npc AcolyteHanz = new() { Id = 3396, Name = "Acolyte Hanz", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Hanz" };
    public static readonly Npc AcolyteJorg = new() { Id = 3396, Name = "Acolyte Jorg", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Jorg" };
    public static readonly Npc BukDirayne = new() { Id = 3396, Name = "Buk Dirayne", WikiUrl = "https://wiki.guildwars.com/wiki/Buk_Dirayne" };
    public static readonly Npc KurzickPriest = new() { Id = 3396, Name = "Kurzick Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Priest" };
    public static readonly Npc RedemptorKarl = new() { Id = 3396, Name = "Redemptor Karl", WikiUrl = "https://wiki.guildwars.com/wiki/Redemptor_Karl" };
    public static readonly Npc ScholarAndrej = new() { Id = 3396, Name = "Scholar Andrej", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Andrej" };
    public static readonly Npc Ulrich = new() { Id = 3396, Name = "Ulrich", WikiUrl = "https://wiki.guildwars.com/wiki/Ulrich" };
    public static readonly Npc Giygas = new() { Id = 3399, Name = "Giygas", WikiUrl = "https://wiki.guildwars.com/wiki/Giygas" };
    public static readonly Npc Morbach = new() { Id = 3399, Name = "Morbach", WikiUrl = "https://wiki.guildwars.com/wiki/Morbach" };
    public static readonly Npc Bannan = new() { Id = 3401, Name = "Bannan", WikiUrl = "https://wiki.guildwars.com/wiki/Bannan" };
    public static readonly Npc EdmundGruca = new() { Id = 3401, Name = "Edmund Gruca", WikiUrl = "https://wiki.guildwars.com/wiki/Edmund_Gruca" };
    public static readonly Npc Oles = new() { Id = 3401, Name = "Oles", WikiUrl = "https://wiki.guildwars.com/wiki/Oles" };
    public static readonly Npc Wendell = new() { Id = 3401, Name = "Wendell", WikiUrl = "https://wiki.guildwars.com/wiki/Wendell" };
    public static readonly Npc Wulfgar = new() { Id = 3401, Name = "Wulfgar", WikiUrl = "https://wiki.guildwars.com/wiki/Wulfgar" };
    public static readonly Npc Benman = new() { Id = 3402, Name = "Benman", WikiUrl = "https://wiki.guildwars.com/wiki/Benman" };
    public static readonly Npc Jurgen = new() { Id = 3402, Name = "Jurgen", WikiUrl = "https://wiki.guildwars.com/wiki/Jurgen" };
    public static readonly Npc Ludwik = new() { Id = 3402, Name = "Ludwik", WikiUrl = "https://wiki.guildwars.com/wiki/Ludwik" };
    public static readonly Npc Wilbur = new() { Id = 3402, Name = "Wilbur", WikiUrl = "https://wiki.guildwars.com/wiki/Wilbur" };
    public static readonly Npc Adele = new() { Id = 3403, Name = "Adele", WikiUrl = "https://wiki.guildwars.com/wiki/Adele" };
    public static readonly Npc Aiiane = new() { Id = 3403, Name = "Aiiane", WikiUrl = "https://wiki.guildwars.com/wiki/Aiiane" };
    public static readonly Npc Bathilda = new() { Id = 3403, Name = "Bathilda", WikiUrl = "https://wiki.guildwars.com/wiki/Bathilda" };
    public static readonly Npc Helene = new() { Id = 3403, Name = "Helene", WikiUrl = "https://wiki.guildwars.com/wiki/Helene" };
    public static readonly Npc Johanna = new() { Id = 3403, Name = "Johanna", WikiUrl = "https://wiki.guildwars.com/wiki/Johanna" };
    public static readonly Npc Linsle = new() { Id = 3403, Name = "Linsle", WikiUrl = "https://wiki.guildwars.com/wiki/Linsle" };
    public static readonly Npc Nadina = new() { Id = 3403, Name = "Nadina", WikiUrl = "https://wiki.guildwars.com/wiki/Nadina" };
    public static readonly Npc Rasia = new() { Id = 3403, Name = "Rasia", WikiUrl = "https://wiki.guildwars.com/wiki/Rasia" };
    public static readonly Npc KurzickMerchant = new() { Id = 3404, Name = "Kurzick Merchant", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Merchant" };
    public static readonly Npc CecylBromka = new() { Id = 3405, Name = "Cecyl Bromka", WikiUrl = "https://wiki.guildwars.com/wiki/Cecyl_Bromka" };
    public static readonly Npc MastersmithRutger = new() { Id = 3405, Name = "Mastersmith Rutger", WikiUrl = "https://wiki.guildwars.com/wiki/Mastersmith_Rutger" };
    public static readonly Npc Gertrud = new() { Id = 3406, Name = "Gertrud", WikiUrl = "https://wiki.guildwars.com/wiki/Gertrud" };
    public static readonly Npc Mathilde = new() { Id = 3406, Name = "Mathilde", WikiUrl = "https://wiki.guildwars.com/wiki/Mathilde" };
    public static readonly Npc Aleksy = new() { Id = 3407, Name = "Aleksy", WikiUrl = "https://wiki.guildwars.com/wiki/Aleksy" };
    public static readonly Npc KristoffirRoi = new() { Id = 3407, Name = "Kristoffir Roi", WikiUrl = "https://wiki.guildwars.com/wiki/Kristoffir_Roi" };
    public static readonly Npc Zefiryna = new() { Id = 3408, Name = "Zefiryna", WikiUrl = "https://wiki.guildwars.com/wiki/Zefiryna" };
    public static readonly Npc KurzickBureaucrat = new() { Id = 3410, Name = "Kurzick Bureaucrat", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Bureaucrat" };
    public static readonly Npc Danika3 = new() { Id = 3411, Name = "Danika", WikiUrl = "https://wiki.guildwars.com/wiki/Danika" };
    public static readonly Npc BaronMirekVasburg = new() { Id = 3412, Name = "Baron Mirek Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Baron_Mirek_Vasburg" };
    public static readonly Npc BaronVasburg = new() { Id = 3412, Name = "Baron Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Baron_Vasburg" };
    public static readonly Npc CountzuHeltzer = new() { Id = 3413, Name = "Count zu Heltzer", WikiUrl = "https://wiki.guildwars.com/wiki/Count_zu_Heltzer" };
    public static readonly Npc Gretchen = new() { Id = 3415, Name = "Gretchen", WikiUrl = "https://wiki.guildwars.com/wiki/Gretchen" };
    public static readonly Npc RyszardMaterial = new() { Id = 3416, Name = "Ryszard Material", WikiUrl = "https://wiki.guildwars.com/wiki/Ryszard_Material" };
    public static readonly Npc LeopoldScroll = new() { Id = 3418, Name = "Leopold Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Leopold_Scroll" };
    public static readonly Npc AnastaciaScroll = new() { Id = 3419, Name = "Anastacia Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Anastacia_Scroll" };
    public static readonly Npc Skye = new() { Id = 3421, Name = "Skye", WikiUrl = "https://wiki.guildwars.com/wiki/Skye" };
    public static readonly Npc KurzickPriest1 = new() { Id = 3422, Name = "Kurzick Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Priest" };
    public static readonly Npc KurzickMerchant1 = new() { Id = 3423, Name = "Kurzick Merchant", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Merchant" };
    public static readonly Npc KurzickGuard1 = new() { Id = 3425, Name = "Kurzick Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Guard" };
    public static readonly Npc KurzickGuard2 = new() { Id = 3426, Name = "Kurzick Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Guard" };
    public static readonly Npc KurzickGuard3 = new() { Id = 3427, Name = "Kurzick Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Guard" };
    public static readonly Npc KurzickRefugee = new() { Id = 3428, Name = "Kurzick Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Refugee" };
    public static readonly Npc KurzickRefugee1 = new() { Id = 3429, Name = "Kurzick Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Refugee" };
    public static readonly Npc Mai3 = new() { Id = 3430, Name = "Mai", WikiUrl = "https://wiki.guildwars.com/wiki/Mai" };
    public static readonly Npc Kisai3 = new() { Id = 3431, Name = "Kisai", WikiUrl = "https://wiki.guildwars.com/wiki/Kisai" };
    public static readonly Npc Taya2 = new() { Id = 3432, Name = "Taya", WikiUrl = "https://wiki.guildwars.com/wiki/Taya" };
    public static readonly Npc Lukas3 = new() { Id = 3433, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc Yuun2 = new() { Id = 3434, Name = "Yuun", WikiUrl = "https://wiki.guildwars.com/wiki/Yuun" };
    public static readonly Npc Aeson2 = new() { Id = 3435, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Mai4 = new() { Id = 3436, Name = "Mai", WikiUrl = "https://wiki.guildwars.com/wiki/Mai" };
    public static readonly Npc Kisai4 = new() { Id = 3437, Name = "Kisai", WikiUrl = "https://wiki.guildwars.com/wiki/Kisai" };
    public static readonly Npc Taya3 = new() { Id = 3438, Name = "Taya", WikiUrl = "https://wiki.guildwars.com/wiki/Taya" };
    public static readonly Npc Lukas4 = new() { Id = 3439, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc Yuun3 = new() { Id = 3440, Name = "Yuun", WikiUrl = "https://wiki.guildwars.com/wiki/Yuun" };
    public static readonly Npc Aeson3 = new() { Id = 3441, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Mai5 = new() { Id = 3442, Name = "Mai", WikiUrl = "https://wiki.guildwars.com/wiki/Mai" };
    public static readonly Npc Kisai5 = new() { Id = 3443, Name = "Kisai", WikiUrl = "https://wiki.guildwars.com/wiki/Kisai" };
    public static readonly Npc Taya4 = new() { Id = 3444, Name = "Taya", WikiUrl = "https://wiki.guildwars.com/wiki/Taya" };
    public static readonly Npc Lukas5 = new() { Id = 3445, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc Yuun4 = new() { Id = 3446, Name = "Yuun", WikiUrl = "https://wiki.guildwars.com/wiki/Yuun" };
    public static readonly Npc Aeson4 = new() { Id = 3447, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Mai6 = new() { Id = 3448, Name = "Mai", WikiUrl = "https://wiki.guildwars.com/wiki/Mai" };
    public static readonly Npc Kisai6 = new() { Id = 3449, Name = "Kisai", WikiUrl = "https://wiki.guildwars.com/wiki/Kisai" };
    public static readonly Npc Taya5 = new() { Id = 3450, Name = "Taya", WikiUrl = "https://wiki.guildwars.com/wiki/Taya" };
    public static readonly Npc Lukas6 = new() { Id = 3451, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc Yuun5 = new() { Id = 3452, Name = "Yuun", WikiUrl = "https://wiki.guildwars.com/wiki/Yuun" };
    public static readonly Npc Aeson5 = new() { Id = 3453, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Panaku4 = new() { Id = 3462, Name = "Panaku", WikiUrl = "https://wiki.guildwars.com/wiki/Panaku" };
    public static readonly Npc LoSha2 = new() { Id = 3463, Name = "Lo Sha", WikiUrl = "https://wiki.guildwars.com/wiki/Lo_Sha" };
    public static readonly Npc Su2 = new() { Id = 3464, Name = "Su", WikiUrl = "https://wiki.guildwars.com/wiki/Su" };
    public static readonly Npc KaiYing3 = new() { Id = 3465, Name = "Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Kai_Ying" };
    public static readonly Npc SisterTai3 = new() { Id = 3466, Name = "Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Tai" };
    public static readonly Npc TalonSilverwing4 = new() { Id = 3467, Name = "Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Talon_Silverwing" };
    public static readonly Npc Zho3 = new() { Id = 3468, Name = "Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Zho" };
    public static readonly Npc ProfessorGai3 = new() { Id = 3469, Name = "Professor Gai", WikiUrl = "https://wiki.guildwars.com/wiki/Professor_Gai" };
    public static readonly Npc ErysVasburg1 = new() { Id = 3470, Name = "Erys Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Erys_Vasburg" };
    public static readonly Npc Brutus = new() { Id = 3471, Name = "Brutus", WikiUrl = "https://wiki.guildwars.com/wiki/Brutus" };
    public static readonly Npc Sheena = new() { Id = 3472, Name = "Sheena", WikiUrl = "https://wiki.guildwars.com/wiki/Sheena" };
    public static readonly Npc Danika4 = new() { Id = 3473, Name = "Danika", WikiUrl = "https://wiki.guildwars.com/wiki/Danika" };
    public static readonly Npc RedemptorKarl1 = new() { Id = 3474, Name = "Redemptor Karl", WikiUrl = "https://wiki.guildwars.com/wiki/Redemptor_Karl" };
    public static readonly Npc Lukas7 = new() { Id = 3475, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc SeaguardHala = new() { Id = 3476, Name = "Seaguard Hala", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Hala" };
    public static readonly Npc Argo1 = new() { Id = 3477, Name = "Argo", WikiUrl = "https://wiki.guildwars.com/wiki/Argo" };
    public static readonly Npc SeaguardGita1 = new() { Id = 3478, Name = "Seaguard Gita", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Gita" };
    public static readonly Npc SeaguardEli1 = new() { Id = 3479, Name = "Seaguard Eli", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Eli" };
    public static readonly Npc Daeman = new() { Id = 3480, Name = "Daeman", WikiUrl = "https://wiki.guildwars.com/wiki/Daeman" };
    public static readonly Npc Aurora1 = new() { Id = 3481, Name = "Aurora", WikiUrl = "https://wiki.guildwars.com/wiki/Aurora" };
    public static readonly Npc Aeson6 = new() { Id = 3482, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Emi2 = new() { Id = 3483, Name = "Emi", WikiUrl = "https://wiki.guildwars.com/wiki/Emi" };
    public static readonly Npc Eve1 = new() { Id = 3484, Name = "Eve", WikiUrl = "https://wiki.guildwars.com/wiki/Eve" };
    public static readonly Npc Cynn3 = new() { Id = 3485, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc HeadmasterVhang3 = new() { Id = 3486, Name = "Headmaster Vhang", WikiUrl = "https://wiki.guildwars.com/wiki/Headmaster_Vhang" };
    public static readonly Npc Jamei2 = new() { Id = 3487, Name = "Jamei", WikiUrl = "https://wiki.guildwars.com/wiki/Jamei" };
    public static readonly Npc Devona1 = new() { Id = 3488, Name = "Devona", WikiUrl = "https://wiki.guildwars.com/wiki/Devona" };
    public static readonly Npc Aidan1 = new() { Id = 3489, Name = "Aidan", WikiUrl = "https://wiki.guildwars.com/wiki/Aidan" };
    public static readonly Npc Aidan2 = new() { Id = 3489, Name = "Aidan", WikiUrl = "https://wiki.guildwars.com/wiki/Aidan" };
    public static readonly Npc Chiyo2 = new() { Id = 3490, Name = "Chiyo", WikiUrl = "https://wiki.guildwars.com/wiki/Chiyo" };
    public static readonly Npc Panaku5 = new() { Id = 3491, Name = "Panaku", WikiUrl = "https://wiki.guildwars.com/wiki/Panaku" };
    public static readonly Npc Nika3 = new() { Id = 3492, Name = "Nika", WikiUrl = "https://wiki.guildwars.com/wiki/Nika" };
    public static readonly Npc LoSha3 = new() { Id = 3493, Name = "Lo Sha", WikiUrl = "https://wiki.guildwars.com/wiki/Lo_Sha" };
    public static readonly Npc Su3 = new() { Id = 3494, Name = "Su", WikiUrl = "https://wiki.guildwars.com/wiki/Su" };
    public static readonly Npc KaiYing4 = new() { Id = 3495, Name = "Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Kai_Ying" };
    public static readonly Npc SisterTai4 = new() { Id = 3496, Name = "Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Tai" };
    public static readonly Npc TalonSilverwing5 = new() { Id = 3497, Name = "Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Talon_Silverwing" };
    public static readonly Npc Zho4 = new() { Id = 3498, Name = "Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Zho" };
    public static readonly Npc ProfessorGai4 = new() { Id = 3499, Name = "Professor Gai", WikiUrl = "https://wiki.guildwars.com/wiki/Professor_Gai" };
    public static readonly Npc ErysVasburg2 = new() { Id = 3500, Name = "Erys Vasburg", WikiUrl = "https://wiki.guildwars.com/wiki/Erys_Vasburg" };
    public static readonly Npc Brutus1 = new() { Id = 3501, Name = "Brutus", WikiUrl = "https://wiki.guildwars.com/wiki/Brutus" };
    public static readonly Npc Sheena1 = new() { Id = 3502, Name = "Sheena", WikiUrl = "https://wiki.guildwars.com/wiki/Sheena" };
    public static readonly Npc RedemptorKarl2 = new() { Id = 3503, Name = "Redemptor Karl", WikiUrl = "https://wiki.guildwars.com/wiki/Redemptor_Karl" };
    public static readonly Npc Lukas8 = new() { Id = 3504, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc Devona2 = new() { Id = 3505, Name = "Devona", WikiUrl = "https://wiki.guildwars.com/wiki/Devona" };
    public static readonly Npc SeaguardHala1 = new() { Id = 3506, Name = "Seaguard Hala", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Hala" };
    public static readonly Npc KaiYing5 = new() { Id = 3507, Name = "Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Kai_Ying" };
    public static readonly Npc SisterTai5 = new() { Id = 3508, Name = "Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Tai" };
    public static readonly Npc Daeman1 = new() { Id = 3509, Name = "Daeman", WikiUrl = "https://wiki.guildwars.com/wiki/Daeman" };
    public static readonly Npc Zho5 = new() { Id = 3510, Name = "Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Zho" };
    public static readonly Npc Aeson7 = new() { Id = 3511, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Panaku6 = new() { Id = 3512, Name = "Panaku", WikiUrl = "https://wiki.guildwars.com/wiki/Panaku" };
    public static readonly Npc LoSha4 = new() { Id = 3513, Name = "Lo Sha", WikiUrl = "https://wiki.guildwars.com/wiki/Lo_Sha" };
    public static readonly Npc Su4 = new() { Id = 3514, Name = "Su", WikiUrl = "https://wiki.guildwars.com/wiki/Su" };
    public static readonly Npc KaiYing6 = new() { Id = 3515, Name = "Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Kai_Ying" };
    public static readonly Npc SisterTai6 = new() { Id = 3516, Name = "Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Tai" };
    public static readonly Npc TalonSilverwing6 = new() { Id = 3517, Name = "Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Talon_Silverwing" };
    public static readonly Npc Zho6 = new() { Id = 3518, Name = "Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Zho" };
    public static readonly Npc ProfessorGai5 = new() { Id = 3519, Name = "Professor Gai", WikiUrl = "https://wiki.guildwars.com/wiki/Professor_Gai" };
    public static readonly Npc Emi3 = new() { Id = 3520, Name = "Emi", WikiUrl = "https://wiki.guildwars.com/wiki/Emi" };
    public static readonly Npc Eve2 = new() { Id = 3521, Name = "Eve", WikiUrl = "https://wiki.guildwars.com/wiki/Eve" };
    public static readonly Npc Cynn4 = new() { Id = 3522, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc Jamei3 = new() { Id = 3523, Name = "Jamei", WikiUrl = "https://wiki.guildwars.com/wiki/Jamei" };
    public static readonly Npc Devona3 = new() { Id = 3524, Name = "Devona", WikiUrl = "https://wiki.guildwars.com/wiki/Devona" };
    public static readonly Npc Aidan3 = new() { Id = 3525, Name = "Aidan", WikiUrl = "https://wiki.guildwars.com/wiki/Aidan" };
    public static readonly Npc Chiyo3 = new() { Id = 3526, Name = "Chiyo", WikiUrl = "https://wiki.guildwars.com/wiki/Chiyo" };
    public static readonly Npc Vhang = new() { Id = 3527, Name = "Vhang", WikiUrl = "https://wiki.guildwars.com/wiki/Vhang" };
    public static readonly Npc Danika5 = new() { Id = 3529, Name = "Danika", WikiUrl = "https://wiki.guildwars.com/wiki/Danika" };
    public static readonly Npc RedemptorKarl3 = new() { Id = 3530, Name = "Redemptor Karl", WikiUrl = "https://wiki.guildwars.com/wiki/Redemptor_Karl" };
    public static readonly Npc Cynn5 = new() { Id = 3531, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc Aidan4 = new() { Id = 3532, Name = "Aidan", WikiUrl = "https://wiki.guildwars.com/wiki/Aidan" };
    public static readonly Npc Nika4 = new() { Id = 3533, Name = "Nika", WikiUrl = "https://wiki.guildwars.com/wiki/Nika" };
    public static readonly Npc Lukas9 = new() { Id = 3534, Name = "Lukas", WikiUrl = "https://wiki.guildwars.com/wiki/Lukas" };
    public static readonly Npc Sheena2 = new() { Id = 3535, Name = "Sheena", WikiUrl = "https://wiki.guildwars.com/wiki/Sheena" };
    public static readonly Npc Brutus2 = new() { Id = 3536, Name = "Brutus", WikiUrl = "https://wiki.guildwars.com/wiki/Brutus" };
    public static readonly Npc TalonSilverwing7 = new() { Id = 3537, Name = "Talon Silverwing", WikiUrl = "https://wiki.guildwars.com/wiki/Talon_Silverwing" };
    public static readonly Npc SisterTai7 = new() { Id = 3538, Name = "Sister Tai", WikiUrl = "https://wiki.guildwars.com/wiki/Sister_Tai" };
    public static readonly Npc Zho7 = new() { Id = 3539, Name = "Zho", WikiUrl = "https://wiki.guildwars.com/wiki/Zho" };
    public static readonly Npc KaiYing7 = new() { Id = 3540, Name = "Kai Ying", WikiUrl = "https://wiki.guildwars.com/wiki/Kai_Ying" };
    public static readonly Npc Aeson8 = new() { Id = 3541, Name = "Aeson", WikiUrl = "https://wiki.guildwars.com/wiki/Aeson" };
    public static readonly Npc Aurora2 = new() { Id = 3542, Name = "Aurora", WikiUrl = "https://wiki.guildwars.com/wiki/Aurora" };
    public static readonly Npc Daemen1 = new() { Id = 3543, Name = "Daemen", WikiUrl = "https://wiki.guildwars.com/wiki/Daemen" };
    public static readonly Npc Argo2 = new() { Id = 3544, Name = "Argo", WikiUrl = "https://wiki.guildwars.com/wiki/Argo" };
    public static readonly Npc HouseDurheimDuelist4 = new() { Id = 3554, Name = "House Durheim Duelist", WikiUrl = "https://wiki.guildwars.com/wiki/House_Durheim_Duelist" };
    public static readonly Npc SeaguardHala2 = new() { Id = 3557, Name = "Seaguard Hala", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Hala" };
    public static readonly Npc Argo3 = new() { Id = 3559, Name = "Argo", WikiUrl = "https://wiki.guildwars.com/wiki/Argo" };
    public static readonly Npc SeaguardGita2 = new() { Id = 3560, Name = "Seaguard Gita", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Gita" };
    public static readonly Npc SeaguardEli2 = new() { Id = 3561, Name = "Seaguard Eli", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Eli" };
    public static readonly Npc Aurora3 = new() { Id = 3562, Name = "Aurora", WikiUrl = "https://wiki.guildwars.com/wiki/Aurora" };
    public static readonly Npc Daeman2 = new() { Id = 3563, Name = "Daeman", WikiUrl = "https://wiki.guildwars.com/wiki/Daeman" };
    public static readonly Npc SiegeTurtle1 = new() { Id = 3564, Name = "Siege Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Siege_Turtle" };
    public static readonly Npc LuxonAssassin = new() { Id = 3565, Name = "Luxon Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Assassin" };
    public static readonly Npc LuxonScout = new() { Id = 3565, Name = "Luxon Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Scout" };
    public static readonly Npc LuxonMesmer = new() { Id = 3566, Name = "Luxon Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Mesmer" };
    public static readonly Npc LuxonNecromancer = new() { Id = 3567, Name = "Luxon Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Necromancer" };
    public static readonly Npc LuxonElementalist = new() { Id = 3568, Name = "Luxon Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Elementalist" };
    public static readonly Npc LuxonMonk = new() { Id = 3569, Name = "Luxon Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Monk" };
    public static readonly Npc LuxonWarrior1 = new() { Id = 3570, Name = "Luxon Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Warrior" };
    public static readonly Npc KurzickWarrior2 = new() { Id = 3571, Name = "Kurzick Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Warrior" };
    public static readonly Npc LuxonRanger = new() { Id = 3571, Name = "Luxon Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Ranger" };
    public static readonly Npc LuxonSoldier = new() { Id = 3571, Name = "Luxon Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Soldier" };
    public static readonly Npc LuxonRitualist = new() { Id = 3572, Name = "Luxon Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Ritualist" };
    public static readonly Npc ConvictedCriminal = new() { Id = 3574, Name = "Convicted Criminal", WikiUrl = "https://wiki.guildwars.com/wiki/Convicted_Criminal" };
    public static readonly Npc ConvictedCriminal1 = new() { Id = 3575, Name = "Convicted Criminal", WikiUrl = "https://wiki.guildwars.com/wiki/Convicted_Criminal" };
    public static readonly Npc ConvictedCriminal2 = new() { Id = 3576, Name = "Convicted Criminal", WikiUrl = "https://wiki.guildwars.com/wiki/Convicted_Criminal" };
    public static readonly Npc ConvictedCriminal3 = new() { Id = 3577, Name = "Convicted Criminal", WikiUrl = "https://wiki.guildwars.com/wiki/Convicted_Criminal" };
    public static readonly Npc ConvictedCriminal4 = new() { Id = 3578, Name = "Convicted Criminal", WikiUrl = "https://wiki.guildwars.com/wiki/Convicted_Criminal" };
    public static readonly Npc ConvictedCriminal5 = new() { Id = 3579, Name = "Convicted Criminal", WikiUrl = "https://wiki.guildwars.com/wiki/Convicted_Criminal" };
    public static readonly Npc GiantTurtle = new() { Id = 3581, Name = "Giant Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Giant_Turtle" };
    public static readonly Npc GreenHaulerTurtle = new() { Id = 3581, Name = "Green Hauler Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Green_Hauler_Turtle" };
    public static readonly Npc PurpleHaulerTurtle = new() { Id = 3581, Name = "Purple Hauler Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Purple_Hauler_Turtle" };
    public static readonly Npc YellowHaulerTurtle = new() { Id = 3581, Name = "Yellow Hauler Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Yellow_Hauler_Turtle" };
    public static readonly Npc SiegeTurtle2 = new() { Id = 3582, Name = "Siege Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Siege_Turtle" };
    public static readonly Npc YoungTurtle = new() { Id = 3583, Name = "Young Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Young_Turtle" };
    public static readonly Npc Arion = new() { Id = 3585, Name = "Arion", WikiUrl = "https://wiki.guildwars.com/wiki/Arion" };
    public static readonly Npc Alpheus = new() { Id = 3586, Name = "Alpheus", WikiUrl = "https://wiki.guildwars.com/wiki/Alpheus" };
    public static readonly Npc Cyril = new() { Id = 3586, Name = "Cyril", WikiUrl = "https://wiki.guildwars.com/wiki/Cyril" };
    public static readonly Npc ElderCasta = new() { Id = 3586, Name = "Elder Casta", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Casta" };
    public static readonly Npc ElderCleo = new() { Id = 3586, Name = "Elder Cleo", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Cleo" };
    public static readonly Npc Fern = new() { Id = 3586, Name = "Fern", WikiUrl = "https://wiki.guildwars.com/wiki/Fern" };
    public static readonly Npc Kaj = new() { Id = 3586, Name = "Kaj", WikiUrl = "https://wiki.guildwars.com/wiki/Kaj" };
    public static readonly Npc Linus = new() { Id = 3586, Name = "Linus", WikiUrl = "https://wiki.guildwars.com/wiki/Linus" };
    public static readonly Npc LuxonSoldier1 = new() { Id = 3586, Name = "Luxon Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Soldier" };
    public static readonly Npc Remick = new() { Id = 3586, Name = "Remick", WikiUrl = "https://wiki.guildwars.com/wiki/Remick" };
    public static readonly Npc LuxonMonk1 = new() { Id = 3588, Name = "Luxon Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Monk" };
    public static readonly Npc CallistatheTamer = new() { Id = 3591, Name = "Callista the Tamer", WikiUrl = "https://wiki.guildwars.com/wiki/Callista_the_Tamer" };
    public static readonly Npc Endre = new() { Id = 3591, Name = "Endre", WikiUrl = "https://wiki.guildwars.com/wiki/Endre" };
    public static readonly Npc LuxonSoldier2 = new() { Id = 3591, Name = "Luxon Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Soldier" };
    public static readonly Npc Hylas = new() { Id = 3593, Name = "Hylas", WikiUrl = "https://wiki.guildwars.com/wiki/Hylas" };
    public static readonly Npc LuxonGuard = new() { Id = 3593, Name = "Luxon Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Guard" };
    public static readonly Npc ScoutmasterAerios = new() { Id = 3593, Name = "Scoutmaster Aerios", WikiUrl = "https://wiki.guildwars.com/wiki/Scoutmaster_Aerios" };
    public static readonly Npc LuxonGuard1 = new() { Id = 3594, Name = "Luxon Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Guard" };
    public static readonly Npc LuxonQuartermaster = new() { Id = 3594, Name = "Luxon Quartermaster", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Quartermaster" };
    public static readonly Npc ReaverTammox = new() { Id = 3594, Name = "Reaver Tammox", WikiUrl = "https://wiki.guildwars.com/wiki/Reaver_Tammox" };
    public static readonly Npc SeaguardLykaios = new() { Id = 3594, Name = "Seaguard Lykaios", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Lykaios" };
    public static readonly Npc Attis = new() { Id = 3596, Name = "Attis", WikiUrl = "https://wiki.guildwars.com/wiki/Attis" };
    public static readonly Npc Erek1 = new() { Id = 3596, Name = "Erek", WikiUrl = "https://wiki.guildwars.com/wiki/Erek" };
    public static readonly Npc HatcheryKeeperGratian = new() { Id = 3596, Name = "Hatchery Keeper Gratian", WikiUrl = "https://wiki.guildwars.com/wiki/Hatchery_Keeper_Gratian" };
    public static readonly Npc Leon = new() { Id = 3596, Name = "Leon", WikiUrl = "https://wiki.guildwars.com/wiki/Leon" };
    public static readonly Npc LuxonCommander = new() { Id = 3596, Name = "Luxon Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Commander" };
    public static readonly Npc ScoutmasterTheron = new() { Id = 3596, Name = "Scoutmaster Theron", WikiUrl = "https://wiki.guildwars.com/wiki/Scoutmaster_Theron" };
    public static readonly Npc SupplymasterKeleos = new() { Id = 3596, Name = "Supplymaster Keleos", WikiUrl = "https://wiki.guildwars.com/wiki/Supplymaster_Keleos" };
    public static readonly Npc WatchmanBencis = new() { Id = 3596, Name = "Watchman Bencis", WikiUrl = "https://wiki.guildwars.com/wiki/Watchman_Bencis" };
    public static readonly Npc CaptainLexis = new() { Id = 3599, Name = "Captain Lexis", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Lexis" };
    public static readonly Npc Jo = new() { Id = 3599, Name = "Jo", WikiUrl = "https://wiki.guildwars.com/wiki/Jo" };
    public static readonly Npc LuxonGuard2 = new() { Id = 3599, Name = "Luxon Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Guard" };
    public static readonly Npc RecruiterLysandra = new() { Id = 3599, Name = "Recruiter Lysandra", WikiUrl = "https://wiki.guildwars.com/wiki/Recruiter_Lysandra" };
    public static readonly Npc LuxonChild = new() { Id = 3601, Name = "Luxon Child", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Child" };
    public static readonly Npc LuxonChild1 = new() { Id = 3602, Name = "Luxon Child", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Child" };
    public static readonly Npc ElderOxis = new() { Id = 3603, Name = "Elder Oxis", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Oxis" };
    public static readonly Npc Erasmus = new() { Id = 3603, Name = "Erasmus", WikiUrl = "https://wiki.guildwars.com/wiki/Erasmus" };
    public static readonly Npc Eurayle = new() { Id = 3603, Name = "Eurayle", WikiUrl = "https://wiki.guildwars.com/wiki/Eurayle" };
    public static readonly Npc Ossian = new() { Id = 3603, Name = "Ossian", WikiUrl = "https://wiki.guildwars.com/wiki/Ossian" };
    public static readonly Npc Perlo = new() { Id = 3603, Name = "Perlo", WikiUrl = "https://wiki.guildwars.com/wiki/Perlo" };
    public static readonly Npc Pheobus = new() { Id = 3603, Name = "Pheobus", WikiUrl = "https://wiki.guildwars.com/wiki/Pheobus" };
    public static readonly Npc ZaviarMerkanah = new() { Id = 3603, Name = "Zaviar Merkanah", WikiUrl = "https://wiki.guildwars.com/wiki/Zaviar_Merkanah" };
    public static readonly Npc Zenos = new() { Id = 3603, Name = "Zenos", WikiUrl = "https://wiki.guildwars.com/wiki/Zenos" };
    public static readonly Npc Alexei = new() { Id = 3604, Name = "Alexei", WikiUrl = "https://wiki.guildwars.com/wiki/Alexei" };
    public static readonly Npc ArenaMasterPortus = new() { Id = 3604, Name = "Arena Master Portus", WikiUrl = "https://wiki.guildwars.com/wiki/Arena_Master_Portus" };
    public static readonly Npc Linus1 = new() { Id = 3604, Name = "Linus", WikiUrl = "https://wiki.guildwars.com/wiki/Linus" };
    public static readonly Npc LuxonAmbassador = new() { Id = 3604, Name = "Luxon Ambassador", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Ambassador" };
    public static readonly Npc LuxonNoble = new() { Id = 3604, Name = "Luxon Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Noble" };
    public static readonly Npc LuxonSentry = new() { Id = 3604, Name = "Luxon Sentry", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Sentry" };
    public static readonly Npc Nicodemus = new() { Id = 3604, Name = "Nicodemus", WikiUrl = "https://wiki.guildwars.com/wiki/Nicodemus" };
    public static readonly Npc Petras = new() { Id = 3604, Name = "Petras", WikiUrl = "https://wiki.guildwars.com/wiki/Petras" };
    public static readonly Npc Callula = new() { Id = 3605, Name = "Callula", WikiUrl = "https://wiki.guildwars.com/wiki/Callula" };
    public static readonly Npc CaptainElora = new() { Id = 3605, Name = "Captain Elora", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Elora" };
    public static readonly Npc Enelora = new() { Id = 3605, Name = "Enelora", WikiUrl = "https://wiki.guildwars.com/wiki/Enelora" };
    public static readonly Npc Leitha = new() { Id = 3605, Name = "Leitha", WikiUrl = "https://wiki.guildwars.com/wiki/Leitha" };
    public static readonly Npc LuxonNoble1 = new() { Id = 3605, Name = "Luxon Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Noble" };
    public static readonly Npc LuxonSentry1 = new() { Id = 3605, Name = "Luxon Sentry", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Sentry" };
    public static readonly Npc Zenaida = new() { Id = 3605, Name = "Zenaida", WikiUrl = "https://wiki.guildwars.com/wiki/Zenaida" };
    public static readonly Npc CaptainJuno = new() { Id = 3606, Name = "Captain Juno", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Juno" };
    public static readonly Npc ElderCasta1 = new() { Id = 3606, Name = "Elder Casta", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Casta" };
    public static readonly Npc ElderCleo1 = new() { Id = 3606, Name = "Elder Cleo", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Cleo" };
    public static readonly Npc ElderPomona = new() { Id = 3606, Name = "Elder Pomona", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Pomona" };
    public static readonly Npc Kamilla = new() { Id = 3606, Name = "Kamilla", WikiUrl = "https://wiki.guildwars.com/wiki/Kamilla" };
    public static readonly Npc LuxonNoble2 = new() { Id = 3606, Name = "Luxon Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Noble" };
    public static readonly Npc Rayea = new() { Id = 3606, Name = "Rayea", WikiUrl = "https://wiki.guildwars.com/wiki/Rayea" };
    public static readonly Npc Alpheus1 = new() { Id = 3607, Name = "Alpheus", WikiUrl = "https://wiki.guildwars.com/wiki/Alpheus" };
    public static readonly Npc AronSeaholm = new() { Id = 3607, Name = "Aron Seaholm", WikiUrl = "https://wiki.guildwars.com/wiki/Aron_Seaholm" };
    public static readonly Npc DauvMerishahl = new() { Id = 3607, Name = "Dauv Merishahl", WikiUrl = "https://wiki.guildwars.com/wiki/Dauv_Merishahl" };
    public static readonly Npc Eurus = new() { Id = 3607, Name = "Eurus", WikiUrl = "https://wiki.guildwars.com/wiki/Eurus" };
    public static readonly Npc Farrer = new() { Id = 3607, Name = "Farrer", WikiUrl = "https://wiki.guildwars.com/wiki/Farrer" };
    public static readonly Npc InfortunatosMaxeles = new() { Id = 3607, Name = "Infortunatos Maxeles", WikiUrl = "https://wiki.guildwars.com/wiki/Infortunatos_Maxeles" };
    public static readonly Npc JahnPitz = new() { Id = 3607, Name = "Jahn Pitz", WikiUrl = "https://wiki.guildwars.com/wiki/Jahn_Pitz" };
    public static readonly Npc KleoDesmos = new() { Id = 3607, Name = "Kleo Desmos", WikiUrl = "https://wiki.guildwars.com/wiki/Kleo_Desmos" };
    public static readonly Npc LuxonArenaFan = new() { Id = 3607, Name = "Luxon Arena Fan", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Arena_Fan" };
    public static readonly Npc LuxonPeasant = new() { Id = 3607, Name = "Luxon Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Peasant" };
    public static readonly Npc LuxonTraveler = new() { Id = 3607, Name = "Luxon Traveler", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Traveler" };
    public static readonly Npc Pietro = new() { Id = 3607, Name = "Pietro", WikiUrl = "https://wiki.guildwars.com/wiki/Pietro" };
    public static readonly Npc Rasmus = new() { Id = 3607, Name = "Rasmus", WikiUrl = "https://wiki.guildwars.com/wiki/Rasmus" };
    public static readonly Npc GreatReaverRixor = new() { Id = 3608, Name = "Great Reaver Rixor", WikiUrl = "https://wiki.guildwars.com/wiki/Great_Reaver_Rixor" };
    public static readonly Npc Kaj1 = new() { Id = 3608, Name = "Kaj", WikiUrl = "https://wiki.guildwars.com/wiki/Kaj" };
    public static readonly Npc Keotah = new() { Id = 3608, Name = "Keotah", WikiUrl = "https://wiki.guildwars.com/wiki/Keotah" };
    public static readonly Npc LuxonArenaFan1 = new() { Id = 3608, Name = "Luxon Arena Fan", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Arena_Fan" };
    public static readonly Npc LuxonPeasant1 = new() { Id = 3608, Name = "Luxon Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Peasant" };
    public static readonly Npc Maddison = new() { Id = 3608, Name = "Maddison", WikiUrl = "https://wiki.guildwars.com/wiki/Maddison" };
    public static readonly Npc Nendros = new() { Id = 3608, Name = "Nendros", WikiUrl = "https://wiki.guildwars.com/wiki/Nendros" };
    public static readonly Npc Rixonon = new() { Id = 3608, Name = "Rixonon", WikiUrl = "https://wiki.guildwars.com/wiki/Rixonon" };
    public static readonly Npc Axton = new() { Id = 3609, Name = "Axton", WikiUrl = "https://wiki.guildwars.com/wiki/Axton" };
    public static readonly Npc Cyril1 = new() { Id = 3609, Name = "Cyril", WikiUrl = "https://wiki.guildwars.com/wiki/Cyril" };
    public static readonly Npc Delphinus = new() { Id = 3609, Name = "Delphinus", WikiUrl = "https://wiki.guildwars.com/wiki/Delphinus" };
    public static readonly Npc KristoTrilios = new() { Id = 3609, Name = "Kristo Trilios", WikiUrl = "https://wiki.guildwars.com/wiki/Kristo_Trilios" };
    public static readonly Npc LuxonArenaFan2 = new() { Id = 3609, Name = "Luxon Arena Fan", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Arena_Fan" };
    public static readonly Npc LuxonPeasant2 = new() { Id = 3609, Name = "Luxon Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Peasant" };
    public static readonly Npc LuxonTraveler1 = new() { Id = 3609, Name = "Luxon Traveler", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Traveler" };
    public static readonly Npc MagistrateKin = new() { Id = 3609, Name = "Magistrate Kin", WikiUrl = "https://wiki.guildwars.com/wiki/Magistrate_Kin" };
    public static readonly Npc SamtiKohlreg = new() { Id = 3609, Name = "Samti Kohlreg", WikiUrl = "https://wiki.guildwars.com/wiki/Samti_Kohlreg" };
    public static readonly Npc CatrineEmbolom = new() { Id = 3610, Name = "Catrine Embolom", WikiUrl = "https://wiki.guildwars.com/wiki/Catrine_Embolom" };
    public static readonly Npc Fern1 = new() { Id = 3610, Name = "Fern", WikiUrl = "https://wiki.guildwars.com/wiki/Fern" };
    public static readonly Npc HistorianofClans = new() { Id = 3610, Name = "Historian of Clans", WikiUrl = "https://wiki.guildwars.com/wiki/Historian_of_Clans" };
    public static readonly Npc LoremasterSitai = new() { Id = 3610, Name = "Loremaster Sitai", WikiUrl = "https://wiki.guildwars.com/wiki/Loremaster_Sitai" };
    public static readonly Npc LosiHapatu = new() { Id = 3610, Name = "Losi Hapatu", WikiUrl = "https://wiki.guildwars.com/wiki/Losi_Hapatu" };
    public static readonly Npc LuxonArenaFan3 = new() { Id = 3610, Name = "Luxon Arena Fan", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Arena_Fan" };
    public static readonly Npc LuxonPeasant3 = new() { Id = 3610, Name = "Luxon Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Peasant" };
    public static readonly Npc LuxonTraveler2 = new() { Id = 3610, Name = "Luxon Traveler", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Traveler" };
    public static readonly Npc Amina = new() { Id = 3611, Name = "Amina", WikiUrl = "https://wiki.guildwars.com/wiki/Amina" };
    public static readonly Npc Luci = new() { Id = 3611, Name = "Luci", WikiUrl = "https://wiki.guildwars.com/wiki/Luci" };
    public static readonly Npc LuxonArenaFan4 = new() { Id = 3611, Name = "Luxon Arena Fan", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Arena_Fan" };
    public static readonly Npc LuxonPeasant4 = new() { Id = 3611, Name = "Luxon Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Peasant" };
    public static readonly Npc Tarena = new() { Id = 3611, Name = "Tarena", WikiUrl = "https://wiki.guildwars.com/wiki/Tarena" };
    public static readonly Npc ElderEzio = new() { Id = 3612, Name = "Elder Ezio", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Ezio" };
    public static readonly Npc JefriWhylir = new() { Id = 3612, Name = "Jefri Whylir", WikiUrl = "https://wiki.guildwars.com/wiki/Jefri_Whylir" };
    public static readonly Npc Remick1 = new() { Id = 3612, Name = "Remick", WikiUrl = "https://wiki.guildwars.com/wiki/Remick" };
    public static readonly Npc Mikolas = new() { Id = 3615, Name = "Mikolas", WikiUrl = "https://wiki.guildwars.com/wiki/Mikolas" };
    public static readonly Npc Tateos = new() { Id = 3615, Name = "Tateos", WikiUrl = "https://wiki.guildwars.com/wiki/Tateos" };
    public static readonly Npc ActaeonHali = new() { Id = 3617, Name = "Actaeon Hali", WikiUrl = "https://wiki.guildwars.com/wiki/Actaeon_Hali" };
    public static readonly Npc Ardelia = new() { Id = 3617, Name = "Ardelia", WikiUrl = "https://wiki.guildwars.com/wiki/Ardelia" };
    public static readonly Npc Bastien = new() { Id = 3617, Name = "Bastien", WikiUrl = "https://wiki.guildwars.com/wiki/Bastien" };
    public static readonly Npc Cyrus = new() { Id = 3617, Name = "Cyrus", WikiUrl = "https://wiki.guildwars.com/wiki/Cyrus" };
    public static readonly Npc Demetrius = new() { Id = 3617, Name = "Demetrius", WikiUrl = "https://wiki.guildwars.com/wiki/Demetrius" };
    public static readonly Npc Kratos = new() { Id = 3617, Name = "Kratos", WikiUrl = "https://wiki.guildwars.com/wiki/Kratos" };
    public static readonly Npc Martinus = new() { Id = 3617, Name = "Martinus", WikiUrl = "https://wiki.guildwars.com/wiki/Martinus" };
    public static readonly Npc Phineus = new() { Id = 3617, Name = "Phineus", WikiUrl = "https://wiki.guildwars.com/wiki/Phineus" };
    public static readonly Npc Taryn = new() { Id = 3617, Name = "Taryn", WikiUrl = "https://wiki.guildwars.com/wiki/Taryn" };
    public static readonly Npc Vasileios = new() { Id = 3617, Name = "Vasileios", WikiUrl = "https://wiki.guildwars.com/wiki/Vasileios" };
    public static readonly Npc Alaris = new() { Id = 3618, Name = "Alaris", WikiUrl = "https://wiki.guildwars.com/wiki/Alaris" };
    public static readonly Npc Alphemynie = new() { Id = 3618, Name = "Alphemynie", WikiUrl = "https://wiki.guildwars.com/wiki/Alphemynie" };
    public static readonly Npc Anja = new() { Id = 3618, Name = "Anja", WikiUrl = "https://wiki.guildwars.com/wiki/Anja" };
    public static readonly Npc Caitlin = new() { Id = 3618, Name = "Caitlin", WikiUrl = "https://wiki.guildwars.com/wiki/Caitlin" };
    public static readonly Npc Demi = new() { Id = 3618, Name = "Demi", WikiUrl = "https://wiki.guildwars.com/wiki/Demi" };
    public static readonly Npc Elysia = new() { Id = 3618, Name = "Elysia", WikiUrl = "https://wiki.guildwars.com/wiki/Elysia" };
    public static readonly Npc Ionessa = new() { Id = 3618, Name = "Ionessa", WikiUrl = "https://wiki.guildwars.com/wiki/Ionessa" };
    public static readonly Npc Jacinthe = new() { Id = 3618, Name = "Jacinthe", WikiUrl = "https://wiki.guildwars.com/wiki/Jacinthe" };
    public static readonly Npc Norah = new() { Id = 3618, Name = "Norah", WikiUrl = "https://wiki.guildwars.com/wiki/Norah" };
    public static readonly Npc LuxonMerchant = new() { Id = 3619, Name = "Luxon Merchant", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Merchant" };
    public static readonly Npc Astrok = new() { Id = 3620, Name = "Astrok", WikiUrl = "https://wiki.guildwars.com/wiki/Astrok" };
    public static readonly Npc Telamon = new() { Id = 3620, Name = "Telamon", WikiUrl = "https://wiki.guildwars.com/wiki/Telamon" };
    public static readonly Npc Theodosus = new() { Id = 3620, Name = "Theodosus", WikiUrl = "https://wiki.guildwars.com/wiki/Theodosus" };
    public static readonly Npc Autumn = new() { Id = 3621, Name = "Autumn", WikiUrl = "https://wiki.guildwars.com/wiki/Autumn" };
    public static readonly Npc Zofie = new() { Id = 3623, Name = "Zofie", WikiUrl = "https://wiki.guildwars.com/wiki/Zofie" };
    public static readonly Npc Gratian = new() { Id = 3624, Name = "Gratian", WikiUrl = "https://wiki.guildwars.com/wiki/Gratian" };
    public static readonly Npc DweiaMaterial = new() { Id = 3627, Name = "Dweia Material", WikiUrl = "https://wiki.guildwars.com/wiki/Dweia_Material" };
    public static readonly Npc Katharine = new() { Id = 3629, Name = "Katharine", WikiUrl = "https://wiki.guildwars.com/wiki/Katharine" };
    public static readonly Npc PhoebeScroll = new() { Id = 3629, Name = "Phoebe Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Phoebe_Scroll" };
    public static readonly Npc Symeon = new() { Id = 3630, Name = "Symeon", WikiUrl = "https://wiki.guildwars.com/wiki/Symeon" };
    public static readonly Npc LuxonMerchant1 = new() { Id = 3632, Name = "Luxon Merchant", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Merchant" };
    public static readonly Npc LuxonGuard3 = new() { Id = 3634, Name = "Luxon Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Guard" };
    public static readonly Npc LuxonGuard4 = new() { Id = 3635, Name = "Luxon Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Guard" };
    public static readonly Npc LuxonGuard5 = new() { Id = 3636, Name = "Luxon Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Guard" };
    public static readonly Npc LuxonPriest1 = new() { Id = 3637, Name = "Luxon Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Priest" };
    public static readonly Npc LuxonScavenger = new() { Id = 3639, Name = "Luxon Scavenger", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Scavenger" };
    public static readonly Npc Aurora4 = new() { Id = 3640, Name = "Aurora", WikiUrl = "https://wiki.guildwars.com/wiki/Aurora" };
    public static readonly Npc Daeman3 = new() { Id = 3641, Name = "Daeman", WikiUrl = "https://wiki.guildwars.com/wiki/Daeman" };
    public static readonly Npc Argo4 = new() { Id = 3642, Name = "Argo", WikiUrl = "https://wiki.guildwars.com/wiki/Argo" };
    public static readonly Npc SeaguardGita3 = new() { Id = 3643, Name = "Seaguard Gita", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Gita" };
    public static readonly Npc SeaguardEli3 = new() { Id = 3644, Name = "Seaguard Eli", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Eli" };
    public static readonly Npc SeaguardHala3 = new() { Id = 3645, Name = "Seaguard Hala", WikiUrl = "https://wiki.guildwars.com/wiki/Seaguard_Hala" };
    public static readonly Npc QuartermasterHector = new() { Id = 3646, Name = "Quartermaster Hector", WikiUrl = "https://wiki.guildwars.com/wiki/Quartermaster_Hector" };
    public static readonly Npc ElderRhea = new() { Id = 3647, Name = "Elder Rhea", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Rhea" };
    public static readonly Npc XisniDreamHaunt = new() { Id = 3648, Name = "Xisni Dream Haunt", WikiUrl = "https://wiki.guildwars.com/wiki/Xisni_Dream_Haunt" };
    public static readonly Npc RahseWindcatcher = new() { Id = 3649, Name = "Rahse Windcatcher", WikiUrl = "https://wiki.guildwars.com/wiki/Rahse_Windcatcher" };
    public static readonly Npc KaswaWebstrider = new() { Id = 3650, Name = "Kaswa Webstrider", WikiUrl = "https://wiki.guildwars.com/wiki/Kaswa_Webstrider" };
    public static readonly Npc RotFoulbelly = new() { Id = 3651, Name = "Rot Foulbelly", WikiUrl = "https://wiki.guildwars.com/wiki/Rot_Foulbelly" };
    public static readonly Npc TheTimeEater = new() { Id = 3653, Name = "The Time Eater", WikiUrl = "https://wiki.guildwars.com/wiki/The_Time_Eater" };
    public static readonly Npc TheScarEater = new() { Id = 3654, Name = "The Scar Eater", WikiUrl = "https://wiki.guildwars.com/wiki/The_Scar_Eater" };
    public static readonly Npc ThePainEater = new() { Id = 3655, Name = "The Pain Eater", WikiUrl = "https://wiki.guildwars.com/wiki/The_Pain_Eater" };
    public static readonly Npc TheSkillEater = new() { Id = 3656, Name = "The Skill Eater", WikiUrl = "https://wiki.guildwars.com/wiki/The_Skill_Eater" };
    public static readonly Npc BazzrDustwing = new() { Id = 3657, Name = "Bazzr Dustwing", WikiUrl = "https://wiki.guildwars.com/wiki/Bazzr_Dustwing" };
    public static readonly Npc ChkkrLocustLord = new() { Id = 3658, Name = "Chkkr Locust Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Chkkr_Locust_Lord" };
    public static readonly Npc BezzrWingstorm = new() { Id = 3659, Name = "Bezzr Wingstorm", WikiUrl = "https://wiki.guildwars.com/wiki/Bezzr_Wingstorm" };
    public static readonly Npc BazzrIcewing = new() { Id = 3660, Name = "Bazzr Icewing", WikiUrl = "https://wiki.guildwars.com/wiki/Bazzr_Icewing" };
    public static readonly Npc BizzrIronshell = new() { Id = 3661, Name = "Bizzr Ironshell", WikiUrl = "https://wiki.guildwars.com/wiki/Bizzr_Ironshell" };
    public static readonly Npc ByzzrWingmender = new() { Id = 3662, Name = "Byzzr Wingmender", WikiUrl = "https://wiki.guildwars.com/wiki/Byzzr_Wingmender" };
    public static readonly Npc ChkkrIronclaw = new() { Id = 3663, Name = "Chkkr Ironclaw", WikiUrl = "https://wiki.guildwars.com/wiki/Chkkr_Ironclaw" };
    public static readonly Npc ChkkrThousandTail = new() { Id = 3664, Name = "Chkkr Thousand Tail", WikiUrl = "https://wiki.guildwars.com/wiki/Chkkr_Thousand_Tail" };
    public static readonly Npc ChkkrBrightclaw = new() { Id = 3665, Name = "Chkkr Brightclaw", WikiUrl = "https://wiki.guildwars.com/wiki/Chkkr_Brightclaw" };
    public static readonly Npc KonrruTaintedStone = new() { Id = 3666, Name = "Konrru Tainted Stone", WikiUrl = "https://wiki.guildwars.com/wiki/Konrru_Tainted_Stone" };
    public static readonly Npc NandetGlassWeaver = new() { Id = 3667, Name = "Nandet Glass Weaver", WikiUrl = "https://wiki.guildwars.com/wiki/Nandet_Glass_Weaver" };
    public static readonly Npc Maximole = new() { Id = 3668, Name = "Maximole", WikiUrl = "https://wiki.guildwars.com/wiki/Maximole" };
    public static readonly Npc UrkaltheAmbusher = new() { Id = 3668, Name = "Urkal the Ambusher", WikiUrl = "https://wiki.guildwars.com/wiki/Urkal_the_Ambusher" };
    public static readonly Npc MugraSwiftspell = new() { Id = 3669, Name = "Mugra Swiftspell", WikiUrl = "https://wiki.guildwars.com/wiki/Mugra_Swiftspell" };
    public static readonly Npc HarggPlaguebinder = new() { Id = 3670, Name = "Hargg Plaguebinder", WikiUrl = "https://wiki.guildwars.com/wiki/Hargg_Plaguebinder" };
    public static readonly Npc TarlokEvermind = new() { Id = 3671, Name = "Tarlok Evermind", WikiUrl = "https://wiki.guildwars.com/wiki/Tarlok_Evermind" };
    public static readonly Npc MungriMagicbox = new() { Id = 3672, Name = "Mungri Magicbox", WikiUrl = "https://wiki.guildwars.com/wiki/Mungri_Magicbox" };
    public static readonly Npc TarnentheBully = new() { Id = 3673, Name = "Tarnen the Bully", WikiUrl = "https://wiki.guildwars.com/wiki/Tarnen_the_Bully" };
    public static readonly Npc NundakTheArcher = new() { Id = 3674, Name = "Nundak The Archer", WikiUrl = "https://wiki.guildwars.com/wiki/Nundak_The_Archer" };
    public static readonly Npc WaggSpiritspeak = new() { Id = 3675, Name = "Wagg Spiritspeak", WikiUrl = "https://wiki.guildwars.com/wiki/Wagg_Spiritspeak" };
    public static readonly Npc DeeprootSorrow = new() { Id = 3676, Name = "Deeproot Sorrow", WikiUrl = "https://wiki.guildwars.com/wiki/Deeproot_Sorrow" };
    public static readonly Npc DarkrootEntrop = new() { Id = 3677, Name = "Darkroot Entrop", WikiUrl = "https://wiki.guildwars.com/wiki/Darkroot_Entrop" };
    public static readonly Npc StrongrootTanglebranch = new() { Id = 3678, Name = "Strongroot Tanglebranch", WikiUrl = "https://wiki.guildwars.com/wiki/Strongroot_Tanglebranch" };
    public static readonly Npc SpiritrootMossbeard = new() { Id = 3679, Name = "Spiritroot Mossbeard", WikiUrl = "https://wiki.guildwars.com/wiki/Spiritroot_Mossbeard" };
    public static readonly Npc WiserootShatterstone = new() { Id = 3680, Name = "Wiseroot Shatterstone", WikiUrl = "https://wiki.guildwars.com/wiki/Wiseroot_Shatterstone" };
    public static readonly Npc BrambleEverthorn = new() { Id = 3681, Name = "Bramble Everthorn", WikiUrl = "https://wiki.guildwars.com/wiki/Bramble_Everthorn" };
    public static readonly Npc FrothStonereap = new() { Id = 3682, Name = "Froth Stonereap", WikiUrl = "https://wiki.guildwars.com/wiki/Froth_Stonereap" };
    public static readonly Npc AzukhanStonewrath = new() { Id = 3683, Name = "Azukhan Stonewrath", WikiUrl = "https://wiki.guildwars.com/wiki/Azukhan_Stonewrath" };
    public static readonly Npc EnnsaStoneweaver = new() { Id = 3684, Name = "Ennsa Stoneweaver", WikiUrl = "https://wiki.guildwars.com/wiki/Ennsa_Stoneweaver" };
    public static readonly Npc DornStonebreaker = new() { Id = 3685, Name = "Dorn Stonebreaker", WikiUrl = "https://wiki.guildwars.com/wiki/Dorn_Stonebreaker" };
    public static readonly Npc CrawStonereap = new() { Id = 3686, Name = "Craw Stonereap", WikiUrl = "https://wiki.guildwars.com/wiki/Craw_Stonereap" };
    public static readonly Npc ZarnasStonewrath = new() { Id = 3687, Name = "Zarnas Stonewrath", WikiUrl = "https://wiki.guildwars.com/wiki/Zarnas_Stonewrath" };
    public static readonly Npc MerilStoneweaver = new() { Id = 3688, Name = "Meril Stoneweaver", WikiUrl = "https://wiki.guildwars.com/wiki/Meril_Stoneweaver" };
    public static readonly Npc MahrStonebreaker = new() { Id = 3689, Name = "Mahr Stonebreaker", WikiUrl = "https://wiki.guildwars.com/wiki/Mahr_Stonebreaker" };
    public static readonly Npc KyrilOathwarden = new() { Id = 3690, Name = "Kyril Oathwarden", WikiUrl = "https://wiki.guildwars.com/wiki/Kyril_Oathwarden" };
    public static readonly Npc SunreachWarmaker = new() { Id = 3691, Name = "Sunreach Warmaker", WikiUrl = "https://wiki.guildwars.com/wiki/Sunreach_Warmaker" };
    public static readonly Npc InallaySplintercall = new() { Id = 3692, Name = "Inallay Splintercall", WikiUrl = "https://wiki.guildwars.com/wiki/Inallay_Splintercall" };
    public static readonly Npc FalaharnMistwarden = new() { Id = 3693, Name = "Falaharn Mistwarden", WikiUrl = "https://wiki.guildwars.com/wiki/Falaharn_Mistwarden" };
    public static readonly Npc WardenofSaprophytes = new() { Id = 3693, Name = "Warden of Saprophytes", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_Saprophytes" };
    public static readonly Npc MilefaunMindflayer = new() { Id = 3694, Name = "Milefaun Mindflayer", WikiUrl = "https://wiki.guildwars.com/wiki/Milefaun_Mindflayer" };
    public static readonly Npc FoalcrestDarkwish = new() { Id = 3695, Name = "Foalcrest Darkwish", WikiUrl = "https://wiki.guildwars.com/wiki/Foalcrest_Darkwish" };
    public static readonly Npc ArborEarthcall = new() { Id = 3696, Name = "Arbor Earthcall", WikiUrl = "https://wiki.guildwars.com/wiki/Arbor_Earthcall" };
    public static readonly Npc JayneForestlight = new() { Id = 3697, Name = "Jayne Forestlight", WikiUrl = "https://wiki.guildwars.com/wiki/Jayne_Forestlight" };
    public static readonly Npc TembarrTreefall = new() { Id = 3698, Name = "Tembarr Treefall", WikiUrl = "https://wiki.guildwars.com/wiki/Tembarr_Treefall" };
    public static readonly Npc RyverMossplanter = new() { Id = 3699, Name = "Ryver Mossplanter", WikiUrl = "https://wiki.guildwars.com/wiki/Ryver_Mossplanter" };
    public static readonly Npc SalkeFurFriend = new() { Id = 3700, Name = "Salke Fur Friend", WikiUrl = "https://wiki.guildwars.com/wiki/Salke_Fur_Friend" };
    public static readonly Npc FlowerSpiritgarden = new() { Id = 3701, Name = "Flower Spiritgarden", WikiUrl = "https://wiki.guildwars.com/wiki/Flower_Spiritgarden" };
    public static readonly Npc DarkFang = new() { Id = 3702, Name = "Dark Fang", WikiUrl = "https://wiki.guildwars.com/wiki/Dark_Fang" };
    public static readonly Npc TheAncient = new() { Id = 3703, Name = "The Ancient", WikiUrl = "https://wiki.guildwars.com/wiki/The_Ancient" };
    public static readonly Npc BloodDrinker = new() { Id = 3704, Name = "Blood Drinker", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_Drinker" };
    public static readonly Npc RhythmDrinker = new() { Id = 3704, Name = "Rhythm Drinker", WikiUrl = "https://wiki.guildwars.com/wiki/Rhythm_Drinker" };
    public static readonly Npc DiscordWallow = new() { Id = 3705, Name = "Discord Wallow", WikiUrl = "https://wiki.guildwars.com/wiki/Discord_Wallow" };
    public static readonly Npc FungalWallow = new() { Id = 3705, Name = "Fungal Wallow", WikiUrl = "https://wiki.guildwars.com/wiki/Fungal_Wallow" };
    public static readonly Npc Oni = new() { Id = 3706, Name = "Oni", WikiUrl = "https://wiki.guildwars.com/wiki/Oni" };
    public static readonly Npc MantisHunter = new() { Id = 3707, Name = "Mantis Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Mantis_Hunter" };
    public static readonly Npc MantisDreamweaver = new() { Id = 3708, Name = "Mantis Dreamweaver", WikiUrl = "https://wiki.guildwars.com/wiki/Mantis_Dreamweaver" };
    public static readonly Npc MantisStormcaller = new() { Id = 3709, Name = "Mantis Stormcaller", WikiUrl = "https://wiki.guildwars.com/wiki/Mantis_Stormcaller" };
    public static readonly Npc MantisMender = new() { Id = 3710, Name = "Mantis Mender", WikiUrl = "https://wiki.guildwars.com/wiki/Mantis_Mender" };
    public static readonly Npc MelodicGaki = new() { Id = 3711, Name = "Melodic Gaki", WikiUrl = "https://wiki.guildwars.com/wiki/Melodic_Gaki" };
    public static readonly Npc PainHungryGaki = new() { Id = 3711, Name = "Pain Hungry Gaki", WikiUrl = "https://wiki.guildwars.com/wiki/Pain_Hungry_Gaki" };
    public static readonly Npc SkillHungryGaki = new() { Id = 3712, Name = "Skill Hungry Gaki", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hungry_Gaki" };
    public static readonly Npc StoneScaleKirin = new() { Id = 3713, Name = "Stone Scale Kirin", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Scale_Kirin" };
    public static readonly Npc DredgeGutter = new() { Id = 3714, Name = "Dredge Gutter", WikiUrl = "https://wiki.guildwars.com/wiki/Dredge_Gutter" };
    public static readonly Npc SlyDredge = new() { Id = 3714, Name = "Sly Dredge", WikiUrl = "https://wiki.guildwars.com/wiki/Sly_Dredge" };
    public static readonly Npc DredgeGardener = new() { Id = 3715, Name = "Dredge Gardener", WikiUrl = "https://wiki.guildwars.com/wiki/Dredge_Gardener" };
    public static readonly Npc DredgeGuardian = new() { Id = 3716, Name = "Dredge Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Dredge_Guardian" };
    public static readonly Npc DredgeGatherer = new() { Id = 3717, Name = "Dredge Gatherer", WikiUrl = "https://wiki.guildwars.com/wiki/Dredge_Gatherer" };
    public static readonly Npc DragonMoss = new() { Id = 3718, Name = "Dragon Moss", WikiUrl = "https://wiki.guildwars.com/wiki/Dragon_Moss" };
    public static readonly Npc Undergrowth = new() { Id = 3719, Name = "Undergrowth", WikiUrl = "https://wiki.guildwars.com/wiki/Undergrowth" };
    public static readonly Npc StoneRain = new() { Id = 3720, Name = "Stone Rain", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Rain" };
    public static readonly Npc ZuHeltzerGuardian = new() { Id = 3720, Name = "Zu Heltzer Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Zu_Heltzer_Guardian" };
    public static readonly Npc StoneReaper = new() { Id = 3721, Name = "Stone Reaper", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Reaper" };
    public static readonly Npc StoneSoul = new() { Id = 3722, Name = "Stone Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Soul" };
    public static readonly Npc StoneCrusher = new() { Id = 3723, Name = "Stone Crusher", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Crusher" };
    public static readonly Npc WardenoftheSpirit = new() { Id = 3724, Name = "Warden of the Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Spirit" };
    public static readonly Npc WardenoftheMind = new() { Id = 3725, Name = "Warden of the Mind", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Mind" };
    public static readonly Npc WardenofEarth = new() { Id = 3726, Name = "Warden of Earth", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_Earth" };
    public static readonly Npc WardenoftheTree = new() { Id = 3727, Name = "Warden of the Tree", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Tree" };
    public static readonly Npc WardenofForests = new() { Id = 3728, Name = "Warden of Forests", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_Forests" };
    public static readonly Npc WardenoftheTrunk = new() { Id = 3729, Name = "Warden of the Trunk", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Trunk" };
    public static readonly Npc WardenoftheBranch = new() { Id = 3730, Name = "Warden of the Branch", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Branch" };
    public static readonly Npc WardenofWinds = new() { Id = 3731, Name = "Warden of Winds", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_Winds" };
    public static readonly Npc WardenoftheLeaf = new() { Id = 3732, Name = "Warden of the Leaf", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Leaf" };
    public static readonly Npc WardenoftheSummer = new() { Id = 3733, Name = "Warden of the Summer", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Summer" };
    public static readonly Npc WardenofSeasons = new() { Id = 3734, Name = "Warden of Seasons", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_Seasons" };
    public static readonly Npc WardenoftheSpring = new() { Id = 3735, Name = "Warden of the Spring", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Spring" };
    public static readonly Npc Moleneaux = new() { Id = 3737, Name = "Moleneaux", WikiUrl = "https://wiki.guildwars.com/wiki/Moleneaux" };
    public static readonly Npc GreaterBloodDrinker = new() { Id = 3739, Name = "Greater Blood Drinker", WikiUrl = "https://wiki.guildwars.com/wiki/Greater_Blood_Drinker" };
    public static readonly Npc ThoughtStealer = new() { Id = 3740, Name = "Thought Stealer", WikiUrl = "https://wiki.guildwars.com/wiki/Thought_Stealer" };
    public static readonly Npc HoppingVampire = new() { Id = 3741, Name = "Hopping Vampire", WikiUrl = "https://wiki.guildwars.com/wiki/Hopping_Vampire" };
    public static readonly Npc MaddenedDredge = new() { Id = 3742, Name = "Maddened Dredge", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Dredge" };
    public static readonly Npc MaddenedDredgeSavage = new() { Id = 3743, Name = "Maddened Dredge Savage", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Dredge_Savage" };
    public static readonly Npc MaddenedDredgeSeer = new() { Id = 3744, Name = "Maddened Dredge Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Dredge_Seer" };
    public static readonly Npc TwistedBark = new() { Id = 3745, Name = "Twisted Bark", WikiUrl = "https://wiki.guildwars.com/wiki/Twisted_Bark" };
    public static readonly Npc UprootedMalice = new() { Id = 3746, Name = "Uprooted Malice", WikiUrl = "https://wiki.guildwars.com/wiki/Uprooted_Malice" };
    public static readonly Npc BroodingThorns = new() { Id = 3747, Name = "Brooding Thorns", WikiUrl = "https://wiki.guildwars.com/wiki/Brooding_Thorns" };
    public static readonly Npc BurningBrush = new() { Id = 3748, Name = "Burning Brush", WikiUrl = "https://wiki.guildwars.com/wiki/Burning_Brush" };
    public static readonly Npc ThornWolf = new() { Id = 3749, Name = "Thorn Wolf", WikiUrl = "https://wiki.guildwars.com/wiki/Thorn_Wolf" };
    public static readonly Npc GuardianSerpent = new() { Id = 3753, Name = "Guardian Serpent", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_Serpent" };
    public static readonly Npc MaddenedThornWarden = new() { Id = 3754, Name = "Maddened Thorn Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Thorn_Warden" };
    public static readonly Npc MaddenedSongWarden = new() { Id = 3755, Name = "Maddened Song Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Song_Warden" };
    public static readonly Npc MaddenedMindWarden = new() { Id = 3756, Name = "Maddened Mind Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Mind_Warden" };
    public static readonly Npc MaddenedEarthWarden = new() { Id = 3757, Name = "Maddened Earth Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Earth_Warden" };
    public static readonly Npc MaddenedForestsWarden = new() { Id = 3758, Name = "Maddened Forests Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Forests_Warden" };
    public static readonly Npc MaddenedWindWarden = new() { Id = 3759, Name = "Maddened Wind Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Wind_Warden" };
    public static readonly Npc MaddenedSpiritWarden = new() { Id = 3760, Name = "Maddened Spirit Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Spirit_Warden" };
    public static readonly Npc SickenedLynx = new() { Id = 3761, Name = "Sickened Lynx", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Lynx" };
    public static readonly Npc SickenedMoa = new() { Id = 3762, Name = "Sickened Moa", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Moa" };
    public static readonly Npc SickenedStalker = new() { Id = 3763, Name = "Sickened Stalker", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Stalker" };
    public static readonly Npc SickenedWolf = new() { Id = 3764, Name = "Sickened Wolf", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Wolf" };
    public static readonly Npc SickenedWarthog = new() { Id = 3765, Name = "Sickened Warthog", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Warthog" };
    public static readonly Npc SickenedBear = new() { Id = 3766, Name = "Sickened Bear", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Bear" };
    public static readonly Npc SickenedGuard = new() { Id = 3767, Name = "Sickened Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Guard" };
    public static readonly Npc SickenedServant = new() { Id = 3768, Name = "Sickened Servant", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Servant" };
    public static readonly Npc SickenedScribe = new() { Id = 3769, Name = "Sickened Scribe", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Scribe" };
    public static readonly Npc SickenedGuard1 = new() { Id = 3770, Name = "Sickened Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Guard" };
    public static readonly Npc SickenedGuardsmanTahnjo = new() { Id = 3770, Name = "Sickened Guardsman Tahnjo", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Guardsman_Tahnjo" };
    public static readonly Npc SickenedGuard2 = new() { Id = 3771, Name = "Sickened Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Guard" };
    public static readonly Npc SickenedGuard3 = new() { Id = 3772, Name = "Sickened Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Guard" };
    public static readonly Npc SickenedPeasant = new() { Id = 3773, Name = "Sickened Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Peasant" };
    public static readonly Npc SickenedPeasant1 = new() { Id = 3774, Name = "Sickened Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Sickened_Peasant" };
    public static readonly Npc TheAfflictedSenku = new() { Id = 3775, Name = "The Afflicted Senku", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Senku" };
    public static readonly Npc AfflictedYijo = new() { Id = 3776, Name = "Afflicted Yijo", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Yijo" };
    public static readonly Npc TheAfflictedKana = new() { Id = 3777, Name = "The Afflicted Kana", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Kana" };
    public static readonly Npc AfflictedFarmerXengJo = new() { Id = 3779, Name = "Afflicted Farmer Xeng Jo", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Farmer_Xeng_Jo" };
    public static readonly Npc TheAfflictedXai = new() { Id = 3779, Name = "The Afflicted Xai", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Xai" };
    public static readonly Npc DiseasedMinister = new() { Id = 3780, Name = "Diseased Minister", WikiUrl = "https://wiki.guildwars.com/wiki/Diseased_Minister" };
    public static readonly Npc AfflictedHorror = new() { Id = 3781, Name = "Afflicted Horror", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Horror" };
    public static readonly Npc TheAfflictedSoonKim = new() { Id = 3782, Name = "The Afflicted Soon Kim", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Soon_Kim" };
    public static readonly Npc TheAfflictedLiYun = new() { Id = 3783, Name = "The Afflicted Li Yun", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Li_Yun" };
    public static readonly Npc TheAfflictedThu = new() { Id = 3784, Name = "The Afflicted Thu", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Thu" };
    public static readonly Npc TheAfflictedKam = new() { Id = 3785, Name = "The Afflicted Kam", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Kam" };
    public static readonly Npc TheAfflictedMiju = new() { Id = 3786, Name = "The Afflicted Miju", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Miju" };
    public static readonly Npc AfflictedGuardsmanChun = new() { Id = 3787, Name = "Afflicted Guardsman Chun", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Guardsman_Chun" };
    public static readonly Npc TheAfflictedAko = new() { Id = 3787, Name = "The Afflicted Ako", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Ako" };
    public static readonly Npc TheAfflictedHuan = new() { Id = 3788, Name = "The Afflicted Huan", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Huan" };
    public static readonly Npc TheAfflictedHakaru = new() { Id = 3789, Name = "The Afflicted Hakaru", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Hakaru" };
    public static readonly Npc TheAfflictedSoonKim1 = new() { Id = 3790, Name = "The Afflicted Soon Kim", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Soon_Kim" };
    public static readonly Npc TheAfflictedLiYun1 = new() { Id = 3791, Name = "The Afflicted Li Yun", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Li_Yun" };
    public static readonly Npc TheAfflictedHuan1 = new() { Id = 3792, Name = "The Afflicted Huan", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Huan" };
    public static readonly Npc TheAfflictedKam1 = new() { Id = 3793, Name = "The Afflicted Kam", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Kam" };
    public static readonly Npc TheAfflictedMiju1 = new() { Id = 3794, Name = "The Afflicted Miju", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Miju" };
    public static readonly Npc TheAfflictedAko1 = new() { Id = 3795, Name = "The Afflicted Ako", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Ako" };
    public static readonly Npc TheAfflictedHuan2 = new() { Id = 3796, Name = "The Afflicted Huan", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Huan" };
    public static readonly Npc TheAfflictedHakaru1 = new() { Id = 3797, Name = "The Afflicted Hakaru", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Hakaru" };
    public static readonly Npc TheAfflictedSenku1 = new() { Id = 3798, Name = "The Afflicted Senku", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Senku" };
    public static readonly Npc TheAfflictedHsinJun = new() { Id = 3799, Name = "The Afflicted Hsin Jun", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Hsin_Jun" };
    public static readonly Npc TheAfflictedLau = new() { Id = 3800, Name = "The Afflicted Lau", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Lau" };
    public static readonly Npc TheAfflictedShen = new() { Id = 3801, Name = "The Afflicted Shen", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Shen" };
    public static readonly Npc TheAfflictedJingme = new() { Id = 3802, Name = "The Afflicted Jingme", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Jingme" };
    public static readonly Npc TheAfflictedMaaka = new() { Id = 3803, Name = "The Afflicted Maaka", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Maaka" };
    public static readonly Npc TheAfflictedPana = new() { Id = 3804, Name = "The Afflicted Pana", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Pana" };
    public static readonly Npc TheAfflictedXenxo = new() { Id = 3805, Name = "The Afflicted Xenxo", WikiUrl = "https://wiki.guildwars.com/wiki/The_Afflicted_Xenxo" };
    public static readonly Npc CanthanPeasant11 = new() { Id = 3806, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanPeasant12 = new() { Id = 3807, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanPeasant13 = new() { Id = 3808, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanPeasant14 = new() { Id = 3809, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanPeasant15 = new() { Id = 3810, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanPeasant16 = new() { Id = 3811, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanPeasant17 = new() { Id = 3812, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc CanthanPeasant18 = new() { Id = 3813, Name = "Canthan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Peasant" };
    public static readonly Npc AfflictedAssassin = new() { Id = 3814, Name = "Afflicted Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Assassin" };
    public static readonly Npc AfflictedAssassin1 = new() { Id = 3815, Name = "Afflicted Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Assassin" };
    public static readonly Npc AfflictedAssassin2 = new() { Id = 3816, Name = "Afflicted Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Assassin" };
    public static readonly Npc AfflictedAssassin3 = new() { Id = 3817, Name = "Afflicted Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Assassin" };
    public static readonly Npc AfflictedAssassin4 = new() { Id = 3818, Name = "Afflicted Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Assassin" };
    public static readonly Npc AfflictedAssassin5 = new() { Id = 3819, Name = "Afflicted Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Assassin" };
    public static readonly Npc AfflictedCreature = new() { Id = 3819, Name = "Afflicted Creature", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Creature" };
    public static readonly Npc AfflictedAssassin6 = new() { Id = 3820, Name = "Afflicted Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Assassin" };
    public static readonly Npc AfflictedBull = new() { Id = 3821, Name = "Afflicted Bull", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Bull" };
    public static readonly Npc AfflictedMesmer = new() { Id = 3822, Name = "Afflicted Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Mesmer" };
    public static readonly Npc AfflictedMesmer1 = new() { Id = 3823, Name = "Afflicted Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Mesmer" };
    public static readonly Npc AfflictedMesmer2 = new() { Id = 3824, Name = "Afflicted Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Mesmer" };
    public static readonly Npc AfflictedMesmer3 = new() { Id = 3825, Name = "Afflicted Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Mesmer" };
    public static readonly Npc AfflictedMesmer4 = new() { Id = 3826, Name = "Afflicted Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Mesmer" };
    public static readonly Npc AfflictedMesmer5 = new() { Id = 3827, Name = "Afflicted Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Mesmer" };
    public static readonly Npc AfflictedMesmer6 = new() { Id = 3828, Name = "Afflicted Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Mesmer" };
    public static readonly Npc AfflictedNecromancer = new() { Id = 3829, Name = "Afflicted Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Necromancer" };
    public static readonly Npc AfflictedNecromancer1 = new() { Id = 3830, Name = "Afflicted Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Necromancer" };
    public static readonly Npc AfflictedNecromancer2 = new() { Id = 3831, Name = "Afflicted Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Necromancer" };
    public static readonly Npc AfflictedNecromancer3 = new() { Id = 3832, Name = "Afflicted Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Necromancer" };
    public static readonly Npc AfflictedNecromancer4 = new() { Id = 3833, Name = "Afflicted Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Necromancer" };
    public static readonly Npc AfflictedNecromancer5 = new() { Id = 3834, Name = "Afflicted Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Necromancer" };
    public static readonly Npc AfflictedNecromancer6 = new() { Id = 3835, Name = "Afflicted Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Necromancer" };
    public static readonly Npc AfflictedElementalist = new() { Id = 3836, Name = "Afflicted Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Elementalist" };
    public static readonly Npc AfflictedElementalist1 = new() { Id = 3837, Name = "Afflicted Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Elementalist" };
    public static readonly Npc AfflictedElementalist2 = new() { Id = 3838, Name = "Afflicted Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Elementalist" };
    public static readonly Npc AfflictedElementalist3 = new() { Id = 3839, Name = "Afflicted Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Elementalist" };
    public static readonly Npc AfflictedElementalist4 = new() { Id = 3840, Name = "Afflicted Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Elementalist" };
    public static readonly Npc AfflictedElementalist5 = new() { Id = 3841, Name = "Afflicted Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Elementalist" };
    public static readonly Npc AfflictedElementalist6 = new() { Id = 3842, Name = "Afflicted Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Elementalist" };
    public static readonly Npc AfflictedMonk = new() { Id = 3843, Name = "Afflicted Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Monk" };
    public static readonly Npc AfflictedMonk1 = new() { Id = 3844, Name = "Afflicted Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Monk" };
    public static readonly Npc AfflictedMonk2 = new() { Id = 3845, Name = "Afflicted Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Monk" };
    public static readonly Npc AfflictedMonk3 = new() { Id = 3846, Name = "Afflicted Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Monk" };
    public static readonly Npc AfflictedMonk4 = new() { Id = 3848, Name = "Afflicted Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Monk" };
    public static readonly Npc AfflictedMonk5 = new() { Id = 3849, Name = "Afflicted Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Monk" };
    public static readonly Npc AfflictedWarrior = new() { Id = 3850, Name = "Afflicted Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Warrior" };
    public static readonly Npc AfflictedWarrior1 = new() { Id = 3851, Name = "Afflicted Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Warrior" };
    public static readonly Npc AfflictedTukan = new() { Id = 3852, Name = "Afflicted Tukan", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Tukan" };
    public static readonly Npc AfflictedWarrior2 = new() { Id = 3852, Name = "Afflicted Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Warrior" };
    public static readonly Npc AfflictedWarrior3 = new() { Id = 3853, Name = "Afflicted Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Warrior" };
    public static readonly Npc AfflictedWarrior4 = new() { Id = 3854, Name = "Afflicted Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Warrior" };
    public static readonly Npc AfflictedWarrior5 = new() { Id = 3855, Name = "Afflicted Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Warrior" };
    public static readonly Npc AfflictedWarrior6 = new() { Id = 3856, Name = "Afflicted Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Warrior" };
    public static readonly Npc AfflictedRanger = new() { Id = 3857, Name = "Afflicted Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ranger" };
    public static readonly Npc AfflictedRanger1 = new() { Id = 3858, Name = "Afflicted Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ranger" };
    public static readonly Npc AfflictedRanger2 = new() { Id = 3859, Name = "Afflicted Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ranger" };
    public static readonly Npc AfflictedRanger3 = new() { Id = 3860, Name = "Afflicted Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ranger" };
    public static readonly Npc AfflictedRanger4 = new() { Id = 3861, Name = "Afflicted Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ranger" };
    public static readonly Npc AfflictedRanger5 = new() { Id = 3862, Name = "Afflicted Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ranger" };
    public static readonly Npc AfflictedRanger6 = new() { Id = 3863, Name = "Afflicted Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ranger" };
    public static readonly Npc AfflictedRavager = new() { Id = 3864, Name = "Afflicted Ravager", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ravager" };
    public static readonly Npc AfflictedRitualist = new() { Id = 3865, Name = "Afflicted Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ritualist" };
    public static readonly Npc AfflictedRitualist1 = new() { Id = 3866, Name = "Afflicted Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ritualist" };
    public static readonly Npc AfflictedRitualist2 = new() { Id = 3867, Name = "Afflicted Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ritualist" };
    public static readonly Npc AfflictedRitualist3 = new() { Id = 3868, Name = "Afflicted Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ritualist" };
    public static readonly Npc AfflictedRitualist4 = new() { Id = 3870, Name = "Afflicted Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ritualist" };
    public static readonly Npc AfflictedRitualist5 = new() { Id = 3871, Name = "Afflicted Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ritualist" };
    public static readonly Npc SanhanTallEarth = new() { Id = 3872, Name = "Sanhan Tall Earth", WikiUrl = "https://wiki.guildwars.com/wiki/Sanhan_Tall_Earth" };
    public static readonly Npc HanjuuTowerfist = new() { Id = 3873, Name = "Hanjuu Towerfist", WikiUrl = "https://wiki.guildwars.com/wiki/Hanjuu_Towerfist" };
    public static readonly Npc AhvhaSankii = new() { Id = 3874, Name = "Ahvha Sankii", WikiUrl = "https://wiki.guildwars.com/wiki/Ahvha_Sankii" };
    public static readonly Npc PeitheSkullBlade = new() { Id = 3875, Name = "Pei the Skull Blade", WikiUrl = "https://wiki.guildwars.com/wiki/Pei_the_Skull_Blade" };
    public static readonly Npc SulmengtheSkullStaff = new() { Id = 3876, Name = "Sulmeng the Skull Staff", WikiUrl = "https://wiki.guildwars.com/wiki/Sulmeng_the_Skull_Staff" };
    public static readonly Npc YingkotheSkullClaw = new() { Id = 3877, Name = "Yingko the Skull Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Yingko_the_Skull_Claw" };
    public static readonly Npc AuritheSkullWand = new() { Id = 3878, Name = "Auri the Skull Wand", WikiUrl = "https://wiki.guildwars.com/wiki/Auri_the_Skull_Wand" };
    public static readonly Npc FengtheSkullSymbol = new() { Id = 3879, Name = "Feng the Skull Symbol", WikiUrl = "https://wiki.guildwars.com/wiki/Feng_the_Skull_Symbol" };
    public static readonly Npc CaptainQuimang1 = new() { Id = 3880, Name = "Captain Quimang", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Quimang" };
    public static readonly Npc ChoWeitheSkullAxe = new() { Id = 3880, Name = "Cho Wei the Skull Axe", WikiUrl = "https://wiki.guildwars.com/wiki/Cho_Wei_the_Skull_Axe" };
    public static readonly Npc JintheSkullBow = new() { Id = 3881, Name = "Jin the Skull Bow", WikiUrl = "https://wiki.guildwars.com/wiki/Jin_the_Skull_Bow" };
    public static readonly Npc MikitheSkullSpirit = new() { Id = 3882, Name = "Miki the Skull Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Miki_the_Skull_Spirit" };
    public static readonly Npc TahkayunTsi = new() { Id = 3883, Name = "Tahkayun Tsi", WikiUrl = "https://wiki.guildwars.com/wiki/Tahkayun_Tsi" };
    public static readonly Npc YunlaiDeathkeeper = new() { Id = 3884, Name = "Yunlai Deathkeeper", WikiUrl = "https://wiki.guildwars.com/wiki/Yunlai_Deathkeeper" };
    public static readonly Npc ZiinjuuLifeCrawler = new() { Id = 3885, Name = "Ziinjuu Life Crawler", WikiUrl = "https://wiki.guildwars.com/wiki/Ziinjuu_Life_Crawler" };
    public static readonly Npc TinDaoKaineng = new() { Id = 3886, Name = "Tin Dao Kaineng", WikiUrl = "https://wiki.guildwars.com/wiki/Tin_Dao_Kaineng" };
    public static readonly Npc BaozoEvilbranch = new() { Id = 3887, Name = "Baozo Evilbranch", WikiUrl = "https://wiki.guildwars.com/wiki/Baozo_Evilbranch" };
    public static readonly Npc WaterBuffalo = new() { Id = 3888, Name = "Water Buffalo", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Buffalo" };
    public static readonly Npc Oni1 = new() { Id = 3889, Name = "Oni", WikiUrl = "https://wiki.guildwars.com/wiki/Oni" };
    public static readonly Npc LesserOni = new() { Id = 3890, Name = "Lesser Oni", WikiUrl = "https://wiki.guildwars.com/wiki/Lesser_Oni" };
    public static readonly Npc BonesnapTurtle = new() { Id = 3891, Name = "Bonesnap Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Bonesnap_Turtle" };
    public static readonly Npc RavenousDrake = new() { Id = 3891, Name = "Ravenous Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Ravenous_Drake" };
    public static readonly Npc CrimsonSkullEtherFiend = new() { Id = 3892, Name = "Crimson Skull Ether Fiend", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Ether_Fiend" };
    public static readonly Npc CrimsonSkullMentalist = new() { Id = 3894, Name = "Crimson Skull Mentalist", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Mentalist" };
    public static readonly Npc CrimsonSkullMesmer = new() { Id = 3895, Name = "Crimson Skull Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Mesmer" };
    public static readonly Npc CrimsonSkullHealer = new() { Id = 3896, Name = "Crimson Skull Healer", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Healer" };
    public static readonly Npc CrimsonSkullPriest = new() { Id = 3898, Name = "Crimson Skull Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Priest" };
    public static readonly Npc CrimsonSkullMender = new() { Id = 3899, Name = "Crimson Skull Mender", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Mender" };
    public static readonly Npc CrimsonSkullHunter = new() { Id = 3900, Name = "Crimson Skull Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Hunter" };
    public static readonly Npc CrimsonSkullLongbow = new() { Id = 3902, Name = "Crimson Skull Longbow", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Longbow" };
    public static readonly Npc CrimsonSkullRaider = new() { Id = 3903, Name = "Crimson Skull Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Raider" };
    public static readonly Npc CropThief = new() { Id = 3903, Name = "Crop Thief", WikiUrl = "https://wiki.guildwars.com/wiki/Crop_Thief" };
    public static readonly Npc CrimsonSkullSeer = new() { Id = 3904, Name = "Crimson Skull Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Seer" };
    public static readonly Npc DaoWeng = new() { Id = 3904, Name = "Dao Weng", WikiUrl = "https://wiki.guildwars.com/wiki/Dao_Weng" };
    public static readonly Npc CrimsonSkullSpiritLord = new() { Id = 3906, Name = "Crimson Skull Spirit Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Spirit_Lord" };
    public static readonly Npc CrimsonSkullRitualist = new() { Id = 3907, Name = "Crimson Skull Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Ritualist" };
    public static readonly Npc DragonLilly = new() { Id = 3908, Name = "Dragon Lilly", WikiUrl = "https://wiki.guildwars.com/wiki/Dragon_Lilly" };
    public static readonly Npc GraspingRoot = new() { Id = 3909, Name = "Grasping Root", WikiUrl = "https://wiki.guildwars.com/wiki/Grasping_Root" };
    public static readonly Npc MantidDrone = new() { Id = 3910, Name = "Mantid Drone", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Drone" };
    public static readonly Npc Mantid = new() { Id = 3911, Name = "Mantid", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid" };
    public static readonly Npc MantidParasite = new() { Id = 3913, Name = "Mantid Parasite", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Parasite" };
    public static readonly Npc MantidDarkwing = new() { Id = 3914, Name = "Mantid Darkwing", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Darkwing" };
    public static readonly Npc MantidDroneHatchling = new() { Id = 3917, Name = "Mantid Drone Hatchling", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Drone_Hatchling" };
    public static readonly Npc MantidDroneHatchling1 = new() { Id = 3918, Name = "Mantid Drone Hatchling", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Drone_Hatchling" };
    public static readonly Npc MantidHatchling = new() { Id = 3919, Name = "Mantid Hatchling", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Hatchling" };
    public static readonly Npc MantidMonitor = new() { Id = 3920, Name = "Mantid Monitor", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Monitor" };
    public static readonly Npc Mantid1 = new() { Id = 3921, Name = "Mantid", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid" };
    public static readonly Npc MantidGazer = new() { Id = 3923, Name = "Mantid Gazer", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Gazer" };
    public static readonly Npc QueenMantid = new() { Id = 3924, Name = "Queen Mantid", WikiUrl = "https://wiki.guildwars.com/wiki/Queen_Mantid" };
    public static readonly Npc MantidGlitterfang = new() { Id = 3925, Name = "Mantid Glitterfang", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Glitterfang" };
    public static readonly Npc MantidQueen = new() { Id = 3927, Name = "Mantid Queen", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Queen" };
    public static readonly Npc MantidMonitorHatchling = new() { Id = 3929, Name = "Mantid Monitor Hatchling", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Monitor_Hatchling" };
    public static readonly Npc NagaScout = new() { Id = 3930, Name = "Naga Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Scout" };
    public static readonly Npc NagaWarrior = new() { Id = 3931, Name = "Naga Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Warrior" };
    public static readonly Npc NagaWelp = new() { Id = 3932, Name = "Naga Welp", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Welp" };
    public static readonly Npc NagaWizard = new() { Id = 3933, Name = "Naga Wizard", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Wizard" };
    public static readonly Npc NagaRaincaller = new() { Id = 3934, Name = "Naga Raincaller", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Raincaller" };
    public static readonly Npc NagaSpellblade = new() { Id = 3935, Name = "Naga Spellblade", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Spellblade" };
    public static readonly Npc NagaWitch = new() { Id = 3936, Name = "Naga Witch", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Witch" };
    public static readonly Npc NagaBoneCollector = new() { Id = 3937, Name = "Naga Bone Collector", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Bone_Collector" };
    public static readonly Npc XsshsssZsss = new() { Id = 3937, Name = "Xsshsss Zsss", WikiUrl = "https://wiki.guildwars.com/wiki/Xsshsss_Zsss" };
    public static readonly Npc NagaSibyl = new() { Id = 3938, Name = "Naga Sibyl", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Sibyl" };
    public static readonly Npc SensaliAssassin = new() { Id = 3939, Name = "Sensali Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Sensali_Assassin" };
    public static readonly Npc SensaliClaw = new() { Id = 3940, Name = "Sensali Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Sensali_Claw" };
    public static readonly Npc SensaliBlood = new() { Id = 3941, Name = "Sensali Blood", WikiUrl = "https://wiki.guildwars.com/wiki/Sensali_Blood" };
    public static readonly Npc SensaliDarkFeather = new() { Id = 3942, Name = "Sensali DarkFeather", WikiUrl = "https://wiki.guildwars.com/wiki/Sensali_DarkFeather" };
    public static readonly Npc SensaliFighter = new() { Id = 3943, Name = "Sensali Fighter", WikiUrl = "https://wiki.guildwars.com/wiki/Sensali_Fighter" };
    public static readonly Npc SwiftHonorclaw = new() { Id = 3943, Name = "Swift Honorclaw", WikiUrl = "https://wiki.guildwars.com/wiki/Swift_Honorclaw" };
    public static readonly Npc GrowRazorbeak = new() { Id = 3944, Name = "Grow Razorbeak", WikiUrl = "https://wiki.guildwars.com/wiki/Grow_Razorbeak" };
    public static readonly Npc SensaliCutter = new() { Id = 3944, Name = "Sensali Cutter", WikiUrl = "https://wiki.guildwars.com/wiki/Sensali_Cutter" };
    public static readonly Npc HungryKappa = new() { Id = 3945, Name = "Hungry Kappa", WikiUrl = "https://wiki.guildwars.com/wiki/Hungry_Kappa" };
    public static readonly Npc Kappa = new() { Id = 3945, Name = "Kappa", WikiUrl = "https://wiki.guildwars.com/wiki/Kappa" };
    public static readonly Npc Kappa1 = new() { Id = 3946, Name = "Kappa", WikiUrl = "https://wiki.guildwars.com/wiki/Kappa" };
    public static readonly Npc Kappa2 = new() { Id = 3947, Name = "Kappa", WikiUrl = "https://wiki.guildwars.com/wiki/Kappa" };
    public static readonly Npc WildYeti = new() { Id = 3948, Name = "Wild Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Wild_Yeti" };
    public static readonly Npc WulkCragfist = new() { Id = 3949, Name = "Wulk Cragfist", WikiUrl = "https://wiki.guildwars.com/wiki/Wulk_Cragfist" };
    public static readonly Npc MountainYeti = new() { Id = 3950, Name = "Mountain Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Mountain_Yeti" };
    public static readonly Npc LonghairYeti = new() { Id = 3951, Name = "Longhair Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Longhair_Yeti" };
    public static readonly Npc RedYeti = new() { Id = 3952, Name = "Red Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Red_Yeti" };
    public static readonly Npc CrimsonSkullHunter1 = new() { Id = 3953, Name = "Crimson Skull Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Crimson_Skull_Hunter" };
    public static readonly Npc CelestialKirin = new() { Id = 3954, Name = "Celestial Kirin", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Kirin" };
    public static readonly Npc Zunraa = new() { Id = 3954, Name = "Zunraa", WikiUrl = "https://wiki.guildwars.com/wiki/Zunraa" };
    public static readonly Npc JadeTornKirin = new() { Id = 3955, Name = "Jade Torn Kirin", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Torn_Kirin" };
    public static readonly Npc CorruptedZunraa = new() { Id = 3956, Name = "Corrupted Zunraa", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Zunraa" };
    public static readonly Npc QuillSongfeather = new() { Id = 3957, Name = "Quill Songfeather", WikiUrl = "https://wiki.guildwars.com/wiki/Quill_Songfeather" };
    public static readonly Npc TenguAssassin = new() { Id = 3957, Name = "Tengu Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Tengu_Assassin" };
    public static readonly Npc TenguShrill = new() { Id = 3958, Name = "Tengu Shrill", WikiUrl = "https://wiki.guildwars.com/wiki/Tengu_Shrill" };
    public static readonly Npc TenguCutter1 = new() { Id = 3959, Name = "Tengu Cutter", WikiUrl = "https://wiki.guildwars.com/wiki/Tengu_Cutter" };
    public static readonly Npc Kuunavang = new() { Id = 3960, Name = "Kuunavang", WikiUrl = "https://wiki.guildwars.com/wiki/Kuunavang" };
    public static readonly Npc EnragedKuunavang = new() { Id = 3961, Name = "Enraged Kuunavang", WikiUrl = "https://wiki.guildwars.com/wiki/Enraged_Kuunavang" };
    public static readonly Npc Kuunavang1 = new() { Id = 3961, Name = "Kuunavang", WikiUrl = "https://wiki.guildwars.com/wiki/Kuunavang" };
    public static readonly Npc CorruptedSpore = new() { Id = 3963, Name = "Corrupted Spore", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Spore" };
    public static readonly Npc RazorfangHazeclaw = new() { Id = 3964, Name = "Razorfang Hazeclaw", WikiUrl = "https://wiki.guildwars.com/wiki/Razorfang_Hazeclaw" };
    public static readonly Npc RazorfinFleshrend = new() { Id = 3965, Name = "Razorfin Fleshrend", WikiUrl = "https://wiki.guildwars.com/wiki/Razorfin_Fleshrend" };
    public static readonly Npc RazortongueFrothspit = new() { Id = 3966, Name = "Razortongue Frothspit", WikiUrl = "https://wiki.guildwars.com/wiki/Razortongue_Frothspit" };
    public static readonly Npc KaySeyStormray = new() { Id = 3967, Name = "KaySey Stormray", WikiUrl = "https://wiki.guildwars.com/wiki/KaySey_Stormray" };
    public static readonly Npc RazorjawLongspine = new() { Id = 3968, Name = "Razorjaw Longspine", WikiUrl = "https://wiki.guildwars.com/wiki/Razorjaw_Longspine" };
    public static readonly Npc WhisperingRitualLord = new() { Id = 3969, Name = "Whispering Ritual Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Whispering_Ritual_Lord" };
    public static readonly Npc WhykSteelshell = new() { Id = 3971, Name = "Whyk Steelshell", WikiUrl = "https://wiki.guildwars.com/wiki/Whyk_Steelshell" };
    public static readonly Npc MohbyWindbeak = new() { Id = 3972, Name = "Mohby Windbeak", WikiUrl = "https://wiki.guildwars.com/wiki/Mohby_Windbeak" };
    public static readonly Npc ReefclawRagebound = new() { Id = 3973, Name = "Reefclaw Ragebound", WikiUrl = "https://wiki.guildwars.com/wiki/Reefclaw_Ragebound" };
    public static readonly Npc FocusofHanaku = new() { Id = 3974, Name = "Focus of Hanaku", WikiUrl = "https://wiki.guildwars.com/wiki/Focus_of_Hanaku" };
    public static readonly Npc ShroudedOni = new() { Id = 3975, Name = "Shrouded Oni", WikiUrl = "https://wiki.guildwars.com/wiki/Shrouded_Oni" };
    public static readonly Npc SourbeakRotshell = new() { Id = 3976, Name = "Sourbeak Rotshell", WikiUrl = "https://wiki.guildwars.com/wiki/Sourbeak_Rotshell" };
    public static readonly Npc MiellaLightwing = new() { Id = 3977, Name = "Miella Lightwing", WikiUrl = "https://wiki.guildwars.com/wiki/Miella_Lightwing" };
    public static readonly Npc SnapjawWindshell = new() { Id = 3978, Name = "Snapjaw Windshell", WikiUrl = "https://wiki.guildwars.com/wiki/Snapjaw_Windshell" };
    public static readonly Npc KunvieFirewing = new() { Id = 3979, Name = "Kunvie Firewing", WikiUrl = "https://wiki.guildwars.com/wiki/Kunvie_Firewing" };
    public static readonly Npc LukrkerFoulfist = new() { Id = 3980, Name = "Lukrker Foulfist", WikiUrl = "https://wiki.guildwars.com/wiki/Lukrker_Foulfist" };
    public static readonly Npc BahnbaShockfoot = new() { Id = 3981, Name = "Bahnba Shockfoot", WikiUrl = "https://wiki.guildwars.com/wiki/Bahnba_Shockfoot" };
    public static readonly Npc HukhrahEarthslove = new() { Id = 3982, Name = "Hukhrah Earthslove", WikiUrl = "https://wiki.guildwars.com/wiki/Hukhrah_Earthslove" };
    public static readonly Npc ArrahhshMountainclub = new() { Id = 3983, Name = "Arrahhsh Mountainclub", WikiUrl = "https://wiki.guildwars.com/wiki/Arrahhsh_Mountainclub" };
    public static readonly Npc ChehbabaRoottripper = new() { Id = 3984, Name = "Chehbaba Roottripper", WikiUrl = "https://wiki.guildwars.com/wiki/Chehbaba_Roottripper" };
    public static readonly Npc TomtonSpiriteater = new() { Id = 3985, Name = "Tomton Spiriteater", WikiUrl = "https://wiki.guildwars.com/wiki/Tomton_Spiriteater" };
    public static readonly Npc AriusDarkApostle = new() { Id = 3986, Name = "Arius Dark Apostle", WikiUrl = "https://wiki.guildwars.com/wiki/Arius_Dark_Apostle" };
    public static readonly Npc TaloustheMad = new() { Id = 3987, Name = "Talous the Mad", WikiUrl = "https://wiki.guildwars.com/wiki/Talous_the_Mad" };
    public static readonly Npc CultistMilthuran = new() { Id = 3988, Name = "Cultist Milthuran", WikiUrl = "https://wiki.guildwars.com/wiki/Cultist_Milthuran" };
    public static readonly Npc AmadisWindoftheSea = new() { Id = 3989, Name = "Amadis Wind of the Sea", WikiUrl = "https://wiki.guildwars.com/wiki/Amadis_Wind_of_the_Sea" };
    public static readonly Npc IncetolDevoutofDepths = new() { Id = 3990, Name = "Incetol Devout of Depths", WikiUrl = "https://wiki.guildwars.com/wiki/Incetol_Devout_of_Depths" };
    public static readonly Npc JacquiTheReaver = new() { Id = 3991, Name = "Jacqui The Reaver", WikiUrl = "https://wiki.guildwars.com/wiki/Jacqui_The_Reaver" };
    public static readonly Npc LorelleJadeCutter = new() { Id = 3992, Name = "Lorelle Jade Cutter", WikiUrl = "https://wiki.guildwars.com/wiki/Lorelle_Jade_Cutter" };
    public static readonly Npc DelictheVengeanceSeeker = new() { Id = 3993, Name = "Delic the Vengeance Seeker", WikiUrl = "https://wiki.guildwars.com/wiki/Delic_the_Vengeance_Seeker" };
    public static readonly Npc GeofferPainBringer = new() { Id = 3994, Name = "Geoffer Pain Bringer", WikiUrl = "https://wiki.guildwars.com/wiki/Geoffer_Pain_Bringer" };
    public static readonly Npc KayalitheBrave = new() { Id = 3995, Name = "Kayali the Brave", WikiUrl = "https://wiki.guildwars.com/wiki/Kayali_the_Brave" };
    public static readonly Npc SentasitheJadeMaul = new() { Id = 3996, Name = "Sentasi the Jade Maul", WikiUrl = "https://wiki.guildwars.com/wiki/Sentasi_the_Jade_Maul" };
    public static readonly Npc ChazekPlagueHerder = new() { Id = 3997, Name = "Chazek Plague Herder", WikiUrl = "https://wiki.guildwars.com/wiki/Chazek_Plague_Herder" };
    public static readonly Npc CultistRajazan = new() { Id = 3998, Name = "Cultist Rajazan", WikiUrl = "https://wiki.guildwars.com/wiki/Cultist_Rajazan" };
    public static readonly Npc MerkiTheReaver = new() { Id = 3999, Name = "Merki The Reaver", WikiUrl = "https://wiki.guildwars.com/wiki/Merki_The_Reaver" };
    public static readonly Npc MilodestustheWrangler = new() { Id = 4000, Name = "Milodestus the Wrangler", WikiUrl = "https://wiki.guildwars.com/wiki/Milodestus_the_Wrangler" };
    public static readonly Npc KenriiSeaSorrow = new() { Id = 4001, Name = "Kenrii Sea Sorrow", WikiUrl = "https://wiki.guildwars.com/wiki/Kenrii_Sea_Sorrow" };
    public static readonly Npc SsareshRattler = new() { Id = 4002, Name = "Ssaresh Rattler", WikiUrl = "https://wiki.guildwars.com/wiki/Ssaresh_Rattler" };
    public static readonly Npc SiskaScalewand = new() { Id = 4003, Name = "Siska Scalewand", WikiUrl = "https://wiki.guildwars.com/wiki/Siska_Scalewand" };
    public static readonly Npc SesskWoeSpreader = new() { Id = 4004, Name = "Sessk Woe Spreader", WikiUrl = "https://wiki.guildwars.com/wiki/Sessk_Woe_Spreader" };
    public static readonly Npc XirissStickleback = new() { Id = 4004, Name = "Xiriss Stickleback", WikiUrl = "https://wiki.guildwars.com/wiki/Xiriss_Stickleback" };
    public static readonly Npc SarssStormscale = new() { Id = 4005, Name = "Sarss Stormscale", WikiUrl = "https://wiki.guildwars.com/wiki/Sarss_Stormscale" };
    public static readonly Npc SsunsBlessedofDwayna = new() { Id = 4006, Name = "Ssuns Blessed of Dwayna", WikiUrl = "https://wiki.guildwars.com/wiki/Ssuns_Blessed_of_Dwayna" };
    public static readonly Npc SskaiDragonsBirth = new() { Id = 4007, Name = "Sskai Dragon's Birth", WikiUrl = "https://wiki.guildwars.com/wiki/Sskai_Dragon's_Birth" };
    public static readonly Npc StsouSwiftscale = new() { Id = 4008, Name = "Stsou Swiftscale", WikiUrl = "https://wiki.guildwars.com/wiki/Stsou_Swiftscale" };
    public static readonly Npc SsynCoiledGrasp = new() { Id = 4009, Name = "Ssyn Coiled Grasp", WikiUrl = "https://wiki.guildwars.com/wiki/Ssyn_Coiled_Grasp" };
    public static readonly Npc ScourgewindElderGuardian = new() { Id = 4010, Name = "Scourgewind Elder Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Scourgewind_Elder_Guardian" };
    public static readonly Npc SoulwhisperElderGuardian = new() { Id = 4011, Name = "Soulwhisper Elder Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Soulwhisper_Elder_Guardian" };
    public static readonly Npc SeacrashElderGuardian = new() { Id = 4012, Name = "Seacrash Elder Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Seacrash_Elder_Guardian" };
    public static readonly Npc WavecrestStonebreak = new() { Id = 4013, Name = "Wavecrest Stonebreak", WikiUrl = "https://wiki.guildwars.com/wiki/Wavecrest_Stonebreak" };
    public static readonly Npc ScuttleFish = new() { Id = 4014, Name = "Scuttle Fish", WikiUrl = "https://wiki.guildwars.com/wiki/Scuttle_Fish" };
    public static readonly Npc CreepingCarp = new() { Id = 4016, Name = "Creeping Carp", WikiUrl = "https://wiki.guildwars.com/wiki/Creeping_Carp" };
    public static readonly Npc Irukandji = new() { Id = 4017, Name = "Irukandji", WikiUrl = "https://wiki.guildwars.com/wiki/Irukandji" };
    public static readonly Npc Yeti = new() { Id = 4018, Name = "Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Yeti" };
    public static readonly Npc Yeti1 = new() { Id = 4019, Name = "Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Yeti" };
    public static readonly Npc Yeti2 = new() { Id = 4020, Name = "Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Yeti" };
    public static readonly Npc RotWallow = new() { Id = 4021, Name = "Rot Wallow", WikiUrl = "https://wiki.guildwars.com/wiki/Rot_Wallow" };
    public static readonly Npc LeviathanEye = new() { Id = 4022, Name = "Leviathan Eye", WikiUrl = "https://wiki.guildwars.com/wiki/Leviathan_Eye" };
    public static readonly Npc LeviathanClaw = new() { Id = 4023, Name = "Leviathan Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Leviathan_Claw" };
    public static readonly Npc TheImpossibleSeaMonster = new() { Id = 4023, Name = "The Impossible Sea Monster", WikiUrl = "https://wiki.guildwars.com/wiki/The_Impossible_Sea_Monster" };
    public static readonly Npc LeviathanMouth = new() { Id = 4024, Name = "Leviathan Mouth", WikiUrl = "https://wiki.guildwars.com/wiki/Leviathan_Mouth" };
    public static readonly Npc KrakenSpawn = new() { Id = 4025, Name = "Kraken Spawn", WikiUrl = "https://wiki.guildwars.com/wiki/Kraken_Spawn" };
    public static readonly Npc Oni2 = new() { Id = 4026, Name = "Oni", WikiUrl = "https://wiki.guildwars.com/wiki/Oni" };
    public static readonly Npc Oni3 = new() { Id = 4027, Name = "Oni", WikiUrl = "https://wiki.guildwars.com/wiki/Oni" };
    public static readonly Npc SaltsprayDragon = new() { Id = 4028, Name = "Saltspray Dragon", WikiUrl = "https://wiki.guildwars.com/wiki/Saltspray_Dragon" };
    public static readonly Npc RockhideDragon = new() { Id = 4030, Name = "Rockhide Dragon", WikiUrl = "https://wiki.guildwars.com/wiki/Rockhide_Dragon" };
    public static readonly Npc OutcastAssassin = new() { Id = 4031, Name = "Outcast Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Assassin" };
    public static readonly Npc OutcastDeathhand = new() { Id = 4032, Name = "Outcast Deathhand", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Deathhand" };
    public static readonly Npc OutcastWarrior = new() { Id = 4033, Name = "Outcast Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Warrior" };
    public static readonly Npc OutcastRitualist = new() { Id = 4035, Name = "Outcast Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Ritualist" };
    public static readonly Npc Kappa3 = new() { Id = 4036, Name = "Kappa", WikiUrl = "https://wiki.guildwars.com/wiki/Kappa" };
    public static readonly Npc JadeTornKirin1 = new() { Id = 4037, Name = "Jade Torn Kirin", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Torn_Kirin" };
    public static readonly Npc NagaWarrior1 = new() { Id = 4038, Name = "Naga Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Warrior" };
    public static readonly Npc NagaArcher = new() { Id = 4039, Name = "Naga Archer", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Archer" };
    public static readonly Npc NagaRitualist = new() { Id = 4040, Name = "Naga Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Ritualist" };
    public static readonly Npc IslandGuardian = new() { Id = 4041, Name = "Island Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Island_Guardian" };
    public static readonly Npc LuxonEnchanter = new() { Id = 4043, Name = "Luxon Enchanter", WikiUrl = "https://wiki.guildwars.com/wiki/Luxon_Enchanter" };
    public static readonly Npc DarkenedIrukandji = new() { Id = 4044, Name = "Darkened Irukandji", WikiUrl = "https://wiki.guildwars.com/wiki/Darkened_Irukandji" };
    public static readonly Npc ShadowofKanaxai = new() { Id = 4045, Name = "Shadow of Kanaxai", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_of_Kanaxai" };
    public static readonly Npc OutcastAssassin1 = new() { Id = 4046, Name = "Outcast Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Assassin" };
    public static readonly Npc OutcastDeathhand1 = new() { Id = 4047, Name = "Outcast Deathhand", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Deathhand" };
    public static readonly Npc OutcastSpellstorm = new() { Id = 4048, Name = "Outcast Spellstorm", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Spellstorm" };
    public static readonly Npc OutcastRaider = new() { Id = 4049, Name = "Outcast Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Raider" };
    public static readonly Npc DeepNightmare = new() { Id = 4050, Name = "Deep Nightmare", WikiUrl = "https://wiki.guildwars.com/wiki/Deep_Nightmare" };
    public static readonly Npc TearsofDwayna = new() { Id = 4051, Name = "Tears of Dwayna", WikiUrl = "https://wiki.guildwars.com/wiki/Tears_of_Dwayna" };
    public static readonly Npc SenkaiLordofthe1000DaggersGuild = new() { Id = 4052, Name = "Senkai Lord of the 1000 Daggers Guild", WikiUrl = "https://wiki.guildwars.com/wiki/Senkai_Lord_of_the_1000_Daggers_Guild" };
    public static readonly Npc OnioftheDeep = new() { Id = 4053, Name = "Oni of the Deep", WikiUrl = "https://wiki.guildwars.com/wiki/Oni_of_the_Deep" };
    public static readonly Npc Kanaxai = new() { Id = 4055, Name = "Kanaxai", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai" };
    public static readonly Npc KanaxaiAspectofDeath = new() { Id = 4056, Name = "Kanaxai Aspect of Death", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Death" };
    public static readonly Npc KanaxaiAspectofDecay = new() { Id = 4056, Name = "Kanaxai Aspect of Decay", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Decay" };
    public static readonly Npc KanaxaiAspectofDepletion = new() { Id = 4056, Name = "Kanaxai Aspect of Depletion", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Depletion" };
    public static readonly Npc KanaxaiAspectofExposure = new() { Id = 4056, Name = "Kanaxai Aspect of Exposure", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Exposure" };
    public static readonly Npc KanaxaiAspectofFailure = new() { Id = 4056, Name = "Kanaxai Aspect of Failure", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Failure" };
    public static readonly Npc KanaxaiAspectofFear = new() { Id = 4056, Name = "Kanaxai Aspect of Fear", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Fear" };
    public static readonly Npc KanaxaiAspectofLethargy = new() { Id = 4056, Name = "Kanaxai Aspect of Lethargy", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Lethargy" };
    public static readonly Npc KanaxaiAspectofPain = new() { Id = 4056, Name = "Kanaxai Aspect of Pain", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Pain" };
    public static readonly Npc KanaxaiAspectofScorpions = new() { Id = 4056, Name = "Kanaxai Aspect of Scorpions", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Scorpions" };
    public static readonly Npc KanaxaiAspectofShadows = new() { Id = 4056, Name = "Kanaxai Aspect of Shadows", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Shadows" };
    public static readonly Npc KanaxaiAspectofSoothing = new() { Id = 4056, Name = "Kanaxai Aspect of Soothing", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Soothing" };
    public static readonly Npc KanaxaiAspectofSurrender = new() { Id = 4056, Name = "Kanaxai Aspect of Surrender", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Surrender" };
    public static readonly Npc KanaxaiAspectofTorment = new() { Id = 4056, Name = "Kanaxai Aspect of Torment", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai_Aspect_of_Torment" };
    public static readonly Npc FreezingNightmare = new() { Id = 4057, Name = "Freezing Nightmare", WikiUrl = "https://wiki.guildwars.com/wiki/Freezing_Nightmare" };
    public static readonly Npc SappingNightmare = new() { Id = 4059, Name = "Sapping Nightmare", WikiUrl = "https://wiki.guildwars.com/wiki/Sapping_Nightmare" };
    public static readonly Npc ScourgeManta = new() { Id = 4061, Name = "Scourge Manta", WikiUrl = "https://wiki.guildwars.com/wiki/Scourge_Manta" };
    public static readonly Npc BlessedManta = new() { Id = 4062, Name = "Blessed Manta", WikiUrl = "https://wiki.guildwars.com/wiki/Blessed_Manta" };
    public static readonly Npc RipperCarp = new() { Id = 4063, Name = "Ripper Carp", WikiUrl = "https://wiki.guildwars.com/wiki/Ripper_Carp" };
    public static readonly Npc DarkenedIrukandji1 = new() { Id = 4064, Name = "Darkened Irukandji", WikiUrl = "https://wiki.guildwars.com/wiki/Darkened_Irukandji" };
    public static readonly Npc RebornIrukandji = new() { Id = 4065, Name = "Reborn Irukandji", WikiUrl = "https://wiki.guildwars.com/wiki/Reborn_Irukandji" };
    public static readonly Npc TheLeviathan = new() { Id = 4066, Name = "The Leviathan", WikiUrl = "https://wiki.guildwars.com/wiki/The_Leviathan" };
    public static readonly Npc LeviathanMind = new() { Id = 4067, Name = "Leviathan Mind", WikiUrl = "https://wiki.guildwars.com/wiki/Leviathan_Mind" };
    public static readonly Npc LeviathanArm = new() { Id = 4068, Name = "Leviathan Arm", WikiUrl = "https://wiki.guildwars.com/wiki/Leviathan_Arm" };
    public static readonly Npc LeviathanHead = new() { Id = 4069, Name = "Leviathan Head", WikiUrl = "https://wiki.guildwars.com/wiki/Leviathan_Head" };
    public static readonly Npc OutcastAssassin2 = new() { Id = 4070, Name = "Outcast Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Assassin" };
    public static readonly Npc OutcastDeathhand2 = new() { Id = 4071, Name = "Outcast Deathhand", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Deathhand" };
    public static readonly Npc OutcastSpellstorm1 = new() { Id = 4072, Name = "Outcast Spellstorm", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Spellstorm" };
    public static readonly Npc OutcastReaver = new() { Id = 4073, Name = "Outcast Reaver", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Reaver" };
    public static readonly Npc OutcastRaider1 = new() { Id = 4074, Name = "Outcast Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Raider" };
    public static readonly Npc OutcastRitualist1 = new() { Id = 4075, Name = "Outcast Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Outcast_Ritualist" };
    public static readonly Npc RitualistsConstruct = new() { Id = 4076, Name = "Ritualist's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Ritualist's_Construct" };
    public static readonly Npc AssassinsConstruct = new() { Id = 4077, Name = "Assassin's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Assassin's_Construct" };
    public static readonly Npc BoundVizu = new() { Id = 4078, Name = "Bound Vizu", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Vizu" };
    public static readonly Npc BoundKitah = new() { Id = 4079, Name = "Bound Kitah", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Kitah" };
    public static readonly Npc BoundNaku = new() { Id = 4080, Name = "Bound Naku", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Naku" };
    public static readonly Npc BoundTeinai = new() { Id = 4081, Name = "Bound Teinai", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Teinai" };
    public static readonly Npc BoundKarei = new() { Id = 4082, Name = "Bound Karei", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Karei" };
    public static readonly Npc BoundJaizhanju = new() { Id = 4083, Name = "Bound Jaizhanju", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Jaizhanju" };
    public static readonly Npc BoundZojun = new() { Id = 4084, Name = "Bound Zojun", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Zojun" };
    public static readonly Npc BoundKaolai = new() { Id = 4085, Name = "Bound Kaolai", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Kaolai" };
    public static readonly Npc AssassinsConstruct1 = new() { Id = 4086, Name = "Assassin's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Assassin's_Construct" };
    public static readonly Npc MesmersConstruct = new() { Id = 4087, Name = "Mesmer's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Mesmer's_Construct" };
    public static readonly Npc NecromancersConstruct = new() { Id = 4088, Name = "Necromancer's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Necromancer's_Construct" };
    public static readonly Npc ElementalsConstruct = new() { Id = 4089, Name = "Elemental's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Elemental's_Construct" };
    public static readonly Npc MonksConstruct = new() { Id = 4090, Name = "Monk's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Monk's_Construct" };
    public static readonly Npc WarriorsConstruct = new() { Id = 4091, Name = "Warrior's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior's_Construct" };
    public static readonly Npc RangersConstruct = new() { Id = 4092, Name = "Ranger's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Ranger's_Construct" };
    public static readonly Npc RitualistsConstruct1 = new() { Id = 4093, Name = "Ritualist's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Ritualist's_Construct" };
    public static readonly Npc MesmersConstruct1 = new() { Id = 4094, Name = "Mesmer's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Mesmer's_Construct" };
    public static readonly Npc NecromancersConstruct1 = new() { Id = 4095, Name = "Necromancer's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Necromancer's_Construct" };
    public static readonly Npc ElementalsConstruct1 = new() { Id = 4096, Name = "Elemental's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Elemental's_Construct" };
    public static readonly Npc MonksConstruct1 = new() { Id = 4097, Name = "Monk's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Monk's_Construct" };
    public static readonly Npc WarriorsConstruct1 = new() { Id = 4098, Name = "Warrior's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Warrior's_Construct" };
    public static readonly Npc RangersConstruct1 = new() { Id = 4099, Name = "Ranger's Construct", WikiUrl = "https://wiki.guildwars.com/wiki/Ranger's_Construct" };
    public static readonly Npc SilentAncientOnata = new() { Id = 4100, Name = "Silent Ancient Onata", WikiUrl = "https://wiki.guildwars.com/wiki/Silent_Ancient_Onata" };
    public static readonly Npc ArcaneAncientPhi = new() { Id = 4101, Name = "Arcane Ancient Phi", WikiUrl = "https://wiki.guildwars.com/wiki/Arcane_Ancient_Phi" };
    public static readonly Npc DoomedAncientKkraz = new() { Id = 4102, Name = "Doomed Ancient Kkraz", WikiUrl = "https://wiki.guildwars.com/wiki/Doomed_Ancient_Kkraz" };
    public static readonly Npc StarAncientKoosun = new() { Id = 4103, Name = "Star Ancient Koosun", WikiUrl = "https://wiki.guildwars.com/wiki/Star_Ancient_Koosun" };
    public static readonly Npc UntouchedAncientKy = new() { Id = 4104, Name = "Untouched Ancient Ky", WikiUrl = "https://wiki.guildwars.com/wiki/Untouched_Ancient_Ky" };
    public static readonly Npc SwordAncientKai = new() { Id = 4105, Name = "Sword Ancient Kai", WikiUrl = "https://wiki.guildwars.com/wiki/Sword_Ancient_Kai" };
    public static readonly Npc FamishedAncientBrrne = new() { Id = 4106, Name = "Famished Ancient Brrne", WikiUrl = "https://wiki.guildwars.com/wiki/Famished_Ancient_Brrne" };
    public static readonly Npc DefiantAncientSseer = new() { Id = 4107, Name = "Defiant Ancient Sseer", WikiUrl = "https://wiki.guildwars.com/wiki/Defiant_Ancient_Sseer" };
    public static readonly Npc SilentAncientOnata1 = new() { Id = 4108, Name = "Silent Ancient Onata", WikiUrl = "https://wiki.guildwars.com/wiki/Silent_Ancient_Onata" };
    public static readonly Npc ArcaneAncientPhi1 = new() { Id = 4109, Name = "Arcane Ancient Phi", WikiUrl = "https://wiki.guildwars.com/wiki/Arcane_Ancient_Phi" };
    public static readonly Npc DoomedAncientKkraz1 = new() { Id = 4110, Name = "Doomed Ancient Kkraz", WikiUrl = "https://wiki.guildwars.com/wiki/Doomed_Ancient_Kkraz" };
    public static readonly Npc StarAncientKoosun1 = new() { Id = 4111, Name = "Star Ancient Koosun", WikiUrl = "https://wiki.guildwars.com/wiki/Star_Ancient_Koosun" };
    public static readonly Npc UntouchedAncientKy1 = new() { Id = 4112, Name = "Untouched Ancient Ky", WikiUrl = "https://wiki.guildwars.com/wiki/Untouched_Ancient_Ky" };
    public static readonly Npc SwordAncientKai1 = new() { Id = 4113, Name = "Sword Ancient Kai", WikiUrl = "https://wiki.guildwars.com/wiki/Sword_Ancient_Kai" };
    public static readonly Npc FamishedAncientBrrne1 = new() { Id = 4114, Name = "Famished Ancient Brrne", WikiUrl = "https://wiki.guildwars.com/wiki/Famished_Ancient_Brrne" };
    public static readonly Npc DefiantAncientSseer1 = new() { Id = 4115, Name = "Defiant Ancient Sseer", WikiUrl = "https://wiki.guildwars.com/wiki/Defiant_Ancient_Sseer" };
    public static readonly Npc ShiroTagachi = new() { Id = 4116, Name = "Shiro Tagachi", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro_Tagachi" };
    public static readonly Npc ShiroTagachi1 = new() { Id = 4119, Name = "Shiro Tagachi", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro_Tagachi" };
    public static readonly Npc ShirokenAssassin = new() { Id = 4121, Name = "Shiro'ken Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Assassin" };
    public static readonly Npc ShirokenMesmer = new() { Id = 4122, Name = "Shiro'ken Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Mesmer" };
    public static readonly Npc ShirokenNecromancer = new() { Id = 4123, Name = "Shiro'ken Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Necromancer" };
    public static readonly Npc ShirokenElementalist = new() { Id = 4124, Name = "Shiro'ken Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Elementalist" };
    public static readonly Npc ShirokenMonk = new() { Id = 4125, Name = "Shiro'ken Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Monk" };
    public static readonly Npc ShirokenWarrior = new() { Id = 4126, Name = "Shiro'ken Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Warrior" };
    public static readonly Npc ShirokenRanger = new() { Id = 4127, Name = "Shiro'ken Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Ranger" };
    public static readonly Npc ShirokenRitualist = new() { Id = 4128, Name = "Shiro'ken Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Ritualist" };
    public static readonly Npc SpiritofPortals = new() { Id = 4129, Name = "Spirit of Portals", WikiUrl = "https://wiki.guildwars.com/wiki/Spirit_of_Portals" };
    public static readonly Npc BoundMesmer = new() { Id = 4131, Name = "Bound Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Mesmer" };
    public static readonly Npc BoundNecromancer = new() { Id = 4132, Name = "Bound Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Necromancer" };
    public static readonly Npc BoundElementalist = new() { Id = 4133, Name = "Bound Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Elementalist" };
    public static readonly Npc BoundMonk = new() { Id = 4134, Name = "Bound Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Monk" };
    public static readonly Npc BoundWarrior = new() { Id = 4135, Name = "Bound Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Warrior" };
    public static readonly Npc BoundRanger = new() { Id = 4136, Name = "Bound Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Ranger" };
    public static readonly Npc BoundRitualist = new() { Id = 4137, Name = "Bound Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Ritualist" };
    public static readonly Npc SpiritofTheMists = new() { Id = 4139, Name = "Spirit of The Mists", WikiUrl = "https://wiki.guildwars.com/wiki/Spirit_of_The_Mists" };
    public static readonly Npc SpiritofTheMists1 = new() { Id = 4140, Name = "Spirit of The Mists", WikiUrl = "https://wiki.guildwars.com/wiki/Spirit_of_The_Mists" };
    public static readonly Npc SpiritofTheMists2 = new() { Id = 4141, Name = "Spirit of The Mists", WikiUrl = "https://wiki.guildwars.com/wiki/Spirit_of_The_Mists" };
    public static readonly Npc Tahmu = new() { Id = 4142, Name = "Tahmu", WikiUrl = "https://wiki.guildwars.com/wiki/Tahmu" };
    public static readonly Npc HaiJii = new() { Id = 4143, Name = "Hai Jii", WikiUrl = "https://wiki.guildwars.com/wiki/Hai_Jii" };
    public static readonly Npc KaijunDon = new() { Id = 4144, Name = "Kaijun Don", WikiUrl = "https://wiki.guildwars.com/wiki/Kaijun_Don" };
    public static readonly Npc Kuonghsang = new() { Id = 4145, Name = "Kuonghsang", WikiUrl = "https://wiki.guildwars.com/wiki/Kuonghsang" };
    public static readonly Npc ShreaderSharptongue = new() { Id = 4146, Name = "Shreader Sharptongue", WikiUrl = "https://wiki.guildwars.com/wiki/Shreader_Sharptongue" };
    public static readonly Npc ZiinfaunLifeforce = new() { Id = 4147, Name = "Ziinfaun Lifeforce", WikiUrl = "https://wiki.guildwars.com/wiki/Ziinfaun_Lifeforce" };
    public static readonly Npc BaubaoWavewrath = new() { Id = 4148, Name = "Baubao Wavewrath", WikiUrl = "https://wiki.guildwars.com/wiki/Baubao_Wavewrath" };
    public static readonly Npc QuansongSpiritspeak = new() { Id = 4149, Name = "Quansong Spiritspeak", WikiUrl = "https://wiki.guildwars.com/wiki/Quansong_Spiritspeak" };
    public static readonly Npc EssenceofDragon = new() { Id = 4152, Name = "Essence of Dragon", WikiUrl = "https://wiki.guildwars.com/wiki/Essence_of_Dragon" };
    public static readonly Npc EssenceofPhoenix = new() { Id = 4153, Name = "Essence of Phoenix", WikiUrl = "https://wiki.guildwars.com/wiki/Essence_of_Phoenix" };
    public static readonly Npc EssenceofKirin = new() { Id = 4154, Name = "Essence of Kirin", WikiUrl = "https://wiki.guildwars.com/wiki/Essence_of_Kirin" };
    public static readonly Npc EssenceofTurtle = new() { Id = 4155, Name = "Essence of Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Essence_of_Turtle" };
    public static readonly Npc Vermin = new() { Id = 4156, Name = "Vermin", WikiUrl = "https://wiki.guildwars.com/wiki/Vermin" };
    public static readonly Npc StarSentinel = new() { Id = 4157, Name = "Star Sentinel", WikiUrl = "https://wiki.guildwars.com/wiki/Star_Sentinel" };
    public static readonly Npc SoarWindfeather = new() { Id = 4158, Name = "Soar Windfeather", WikiUrl = "https://wiki.guildwars.com/wiki/Soar_Windfeather" };
    public static readonly Npc StarBlade = new() { Id = 4158, Name = "Star Blade", WikiUrl = "https://wiki.guildwars.com/wiki/Star_Blade" };
    public static readonly Npc StarLight = new() { Id = 4159, Name = "Star Light", WikiUrl = "https://wiki.guildwars.com/wiki/Star_Light" };
    public static readonly Npc ClawTallfeather = new() { Id = 4160, Name = "Claw Tallfeather", WikiUrl = "https://wiki.guildwars.com/wiki/Claw_Tallfeather" };
    public static readonly Npc Kappa4 = new() { Id = 4163, Name = "Kappa", WikiUrl = "https://wiki.guildwars.com/wiki/Kappa" };
    public static readonly Npc MantidDrone1 = new() { Id = 4164, Name = "Mantid Drone", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Drone" };
    public static readonly Npc MantidMonitor1 = new() { Id = 4165, Name = "Mantid Monitor", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Monitor" };
    public static readonly Npc TempleGuardian = new() { Id = 4166, Name = "Temple Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_Guardian" };
    public static readonly Npc TempleGuardian1 = new() { Id = 4167, Name = "Temple Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_Guardian" };
    public static readonly Npc IlidusoftheEmptyPalm = new() { Id = 4168, Name = "Ilidus of the Empty Palm", WikiUrl = "https://wiki.guildwars.com/wiki/Ilidus_of_the_Empty_Palm" };
    public static readonly Npc XuekaotheDeceptive = new() { Id = 4170, Name = "Xuekao the Deceptive", WikiUrl = "https://wiki.guildwars.com/wiki/Xuekao_the_Deceptive" };
    public static readonly Npc LouoftheKnives = new() { Id = 4171, Name = "Lou of the Knives", WikiUrl = "https://wiki.guildwars.com/wiki/Lou_of_the_Knives" };
    public static readonly Npc Waeng = new() { Id = 4171, Name = "Waeng", WikiUrl = "https://wiki.guildwars.com/wiki/Waeng" };
    public static readonly Npc MinaShatterStorm = new() { Id = 4172, Name = "Mina Shatter Storm", WikiUrl = "https://wiki.guildwars.com/wiki/Mina_Shatter_Storm" };
    public static readonly Npc ChantheDragonsBlood = new() { Id = 4173, Name = "Chan the Dragon's Blood", WikiUrl = "https://wiki.guildwars.com/wiki/Chan_the_Dragon's_Blood" };
    public static readonly Npc ChungtheAttuned = new() { Id = 4174, Name = "Chung the Attuned", WikiUrl = "https://wiki.guildwars.com/wiki/Chung_the_Attuned" };
    public static readonly Npc AmFahLeader = new() { Id = 4175, Name = "Am Fah Leader", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Leader" };
    public static readonly Npc RientheMartyr = new() { Id = 4175, Name = "Rien the Martyr", WikiUrl = "https://wiki.guildwars.com/wiki/Rien_the_Martyr" };
    public static readonly Npc SuntheQuivering = new() { Id = 4176, Name = "Sun the Quivering", WikiUrl = "https://wiki.guildwars.com/wiki/Sun_the_Quivering" };
    public static readonly Npc MeynsangtheSadistic = new() { Id = 4177, Name = "Meynsang the Sadistic", WikiUrl = "https://wiki.guildwars.com/wiki/Meynsang_the_Sadistic" };
    public static readonly Npc ChoSpiritEmpath = new() { Id = 4178, Name = "Cho Spirit Empath", WikiUrl = "https://wiki.guildwars.com/wiki/Cho_Spirit_Empath" };
    public static readonly Npc KenshiSteelhand = new() { Id = 4179, Name = "Kenshi Steelhand", WikiUrl = "https://wiki.guildwars.com/wiki/Kenshi_Steelhand" };
    public static readonly Npc JinthePurifier = new() { Id = 4180, Name = "Jin the Purifier", WikiUrl = "https://wiki.guildwars.com/wiki/Jin_the_Purifier" };
    public static readonly Npc GhialtheBoneDancer = new() { Id = 4181, Name = "Ghial the Bone Dancer", WikiUrl = "https://wiki.guildwars.com/wiki/Ghial_the_Bone_Dancer" };
    public static readonly Npc LianDragonsPetal = new() { Id = 4182, Name = "Lian Dragon's Petal", WikiUrl = "https://wiki.guildwars.com/wiki/Lian_Dragon's_Petal" };
    public static readonly Npc Quufu = new() { Id = 4183, Name = "Quufu", WikiUrl = "https://wiki.guildwars.com/wiki/Quufu" };
    public static readonly Npc ShentheMagistrate = new() { Id = 4183, Name = "Shen the Magistrate", WikiUrl = "https://wiki.guildwars.com/wiki/Shen_the_Magistrate" };
    public static readonly Npc WingThreeBlade = new() { Id = 4184, Name = "Wing Three Blade", WikiUrl = "https://wiki.guildwars.com/wiki/Wing_Three_Blade" };
    public static readonly Npc RoyenBeastkeeper = new() { Id = 4185, Name = "Royen Beastkeeper", WikiUrl = "https://wiki.guildwars.com/wiki/Royen_Beastkeeper" };
    public static readonly Npc OrosenTranquilAcolyte = new() { Id = 4186, Name = "Orosen Tranquil Acolyte", WikiUrl = "https://wiki.guildwars.com/wiki/Orosen_Tranquil_Acolyte" };
    public static readonly Npc AmFahAssassin = new() { Id = 4187, Name = "Am Fah Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Assassin" };
    public static readonly Npc AmFahBandit = new() { Id = 4187, Name = "Am Fah Bandit", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Bandit" };
    public static readonly Npc LouoftheKnives1 = new() { Id = 4187, Name = "Lou of the Knives", WikiUrl = "https://wiki.guildwars.com/wiki/Lou_of_the_Knives" };
    public static readonly Npc ObsidianFlameAssassin1 = new() { Id = 4187, Name = "Obsidian Flame Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Obsidian_Flame_Assassin" };
    public static readonly Npc Ratsu = new() { Id = 4187, Name = "Ratsu", WikiUrl = "https://wiki.guildwars.com/wiki/Ratsu" };
    public static readonly Npc TaeXang = new() { Id = 4187, Name = "Tae Xang", WikiUrl = "https://wiki.guildwars.com/wiki/Tae_Xang" };
    public static readonly Npc Yanlen = new() { Id = 4187, Name = "Yanlen", WikiUrl = "https://wiki.guildwars.com/wiki/Yanlen" };
    public static readonly Npc AmFahNecromancer = new() { Id = 4188, Name = "Am Fah Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Necromancer" };
    public static readonly Npc AmFahHealer = new() { Id = 4189, Name = "Am Fah Healer", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Healer" };
    public static readonly Npc AmFahMarksman = new() { Id = 4190, Name = "Am Fah Marksman", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Marksman" };
    public static readonly Npc JadeBrotherhoodMesmer = new() { Id = 4191, Name = "Jade Brotherhood Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Brotherhood_Mesmer" };
    public static readonly Npc ChongPoi = new() { Id = 4192, Name = "Chong Poi", WikiUrl = "https://wiki.guildwars.com/wiki/Chong_Poi" };
    public static readonly Npc JadeBrotherhoodMage = new() { Id = 4192, Name = "Jade Brotherhood Mage", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Brotherhood_Mage" };
    public static readonly Npc JadeBrotherhoodKnight = new() { Id = 4193, Name = "Jade Brotherhood Knight", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Brotherhood_Knight" };
    public static readonly Npc Noqui = new() { Id = 4193, Name = "Noqui", WikiUrl = "https://wiki.guildwars.com/wiki/Noqui" };
    public static readonly Npc JadeBrotherhoodRitualist = new() { Id = 4194, Name = "Jade Brotherhood Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Brotherhood_Ritualist" };
    public static readonly Npc JadeBrotherhoodMage1 = new() { Id = 4195, Name = "Jade Brotherhood Mage", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Brotherhood_Mage" };
    public static readonly Npc JadeBrotherhoodRitualist1 = new() { Id = 4196, Name = "Jade Brotherhood Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Brotherhood_Ritualist" };
    public static readonly Npc AmFahAssassin1 = new() { Id = 4201, Name = "Am Fah Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Assassin" };
    public static readonly Npc AmFahNecromancer1 = new() { Id = 4202, Name = "Am Fah Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Necromancer" };
    public static readonly Npc AmFahHealer1 = new() { Id = 4203, Name = "Am Fah Healer", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Healer" };
    public static readonly Npc AmFahMarksman1 = new() { Id = 4204, Name = "Am Fah Marksman", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Marksman" };
    public static readonly Npc ShackledSpirit = new() { Id = 4207, Name = "Shackled Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Shackled_Spirit" };
    public static readonly Npc Kormir = new() { Id = 4859, Name = "Kormir", WikiUrl = "https://wiki.guildwars.com/wiki/Kormir" };
    public static readonly Npc XunlaiChest = new() { Id = 4997, Name = "Xunlai Chest", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Chest" };
    public static readonly Npc ArchivistofWhispers = new() { Id = 5214, Name = "Archivist of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Archivist_of_Whispers" };
    public static readonly Npc ScytheofChaos = new() { Id = 5411, Name = "Scythe of Chaos", WikiUrl = "https://wiki.guildwars.com/wiki/Scythe_of_Chaos" };
    public static readonly Npc WrathfulStorm = new() { Id = 5412, Name = "Wrathful Storm", WikiUrl = "https://wiki.guildwars.com/wiki/Wrathful_Storm" };
    public static readonly Npc TormentClaw = new() { Id = 5413, Name = "Torment Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Torment_Claw" };
    public static readonly Npc GraspofInsanity = new() { Id = 5414, Name = "Grasp of Insanity", WikiUrl = "https://wiki.guildwars.com/wiki/Grasp_of_Insanity" };
    public static readonly Npc Chasm = new() { Id = 5862, Name = "Chasm", WikiUrl = "https://wiki.guildwars.com/wiki/Chasm" };
    public static readonly Npc Vekk = new() { Id = 5910, Name = "Vekk", WikiUrl = "https://wiki.guildwars.com/wiki/Vekk" };
    public static readonly Npc OgdenStonehealer = new() { Id = 5929, Name = "Ogden Stonehealer", WikiUrl = "https://wiki.guildwars.com/wiki/Ogden_Stonehealer" };
    public static readonly Npc DwarvenDemolitionist = new() { Id = 6179, Name = "Dwarven Demolitionist", WikiUrl = "https://wiki.guildwars.com/wiki/Dwarven_Demolitionist" };
    public static readonly Npc Borvorel = new() { Id = 6190, Name = "Borvorel", WikiUrl = "https://wiki.guildwars.com/wiki/Borvorel" };
    public static readonly Npc BurolIronfist = new() { Id = 6211, Name = "Burol Ironfist", WikiUrl = "https://wiki.guildwars.com/wiki/Burol_Ironfist" };
    public static readonly Npc Kodan = new() { Id = 6213, Name = "Kodan", WikiUrl = "https://wiki.guildwars.com/wiki/Kodan" };
    public static readonly Npc MOX = new() { Id = 7535, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc Zenjal = new() { Id = 7744, Name = "Zenjal", WikiUrl = "https://wiki.guildwars.com/wiki/Zenjal" };
    public static readonly Npc ForemantheCrier = new() { Id = 7970, Name = "Foreman the Crier", WikiUrl = "https://wiki.guildwars.com/wiki/Foreman_the_Crier" };
    public static readonly Npc InitiateZeiRi = new() { Id = 8574, Name = "Initiate Zei Ri", WikiUrl = "https://wiki.guildwars.com/wiki/Initiate_Zei_Ri" };
    public static readonly Npc ZhaKu = new() { Id = 8576, Name = "Zha Ku", WikiUrl = "https://wiki.guildwars.com/wiki/Zha_Ku" };
    public static readonly Npc InitiateTsuriai = new() { Id = 8577, Name = "Initiate Tsuriai", WikiUrl = "https://wiki.guildwars.com/wiki/Initiate_Tsuriai" };
    public static readonly Npc HeraldofPurity = new() { Id = 8578, Name = "Herald of Purity", WikiUrl = "https://wiki.guildwars.com/wiki/Herald_of_Purity" };
    public static readonly Npc Setsu = new() { Id = 8578, Name = "Setsu", WikiUrl = "https://wiki.guildwars.com/wiki/Setsu" };
    public static readonly Npc ZuJintheQuick = new() { Id = 8679, Name = "Zu Jin the Quick", WikiUrl = "https://wiki.guildwars.com/wiki/Zu_Jin_the_Quick" };
    public static readonly Npc FahYutheShadowsEye = new() { Id = 8680, Name = "Fah Yu the Shadow's Eye", WikiUrl = "https://wiki.guildwars.com/wiki/Fah_Yu_the_Shadow's_Eye" };
    public static readonly Npc UrisTongofAsh = new() { Id = 8681, Name = "Uris Tong of Ash", WikiUrl = "https://wiki.guildwars.com/wiki/Uris_Tong_of_Ash" };
    public static readonly Npc AmFahWarrior = new() { Id = 8684, Name = "Am Fah Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Warrior" };
    public static readonly Npc AmFahRanger = new() { Id = 8685, Name = "Am Fah Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Ranger" };
    public static readonly Npc AmFahElementalist = new() { Id = 8687, Name = "Am Fah Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Elementalist" };
    public static readonly Npc AmFahMonk = new() { Id = 8688, Name = "Am Fah Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Monk" };
    public static readonly Npc AmFahNecromancer2 = new() { Id = 8689, Name = "Am Fah Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Necromancer" };
    public static readonly Npc AmFahRitualist = new() { Id = 8690, Name = "Am Fah Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Ritualist" };
    public static readonly Npc AmFahAssassin2 = new() { Id = 8691, Name = "Am Fah Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Am_Fah_Assassin" };
    public static readonly Npc LeiJeng = new() { Id = 8736, Name = "Lei Jeng", WikiUrl = "https://wiki.guildwars.com/wiki/Lei_Jeng" };
    public static readonly Npc GuardsmanQaoLin = new() { Id = 8874, Name = "Guardsman Qao Lin", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Qao_Lin" };
    public static readonly Npc AfflictedWarrior7 = new() { Id = -1, Name = "Afflicted Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Warrior" };
    public static readonly Npc BoundAssassinSanctum = new() { Id = -1, Name = "Bound Assassin Sanctum", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Assassin_Sanctum" };
    public static readonly Npc MantisHunter1 = new() { Id = -1, Name = "Mantis Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Mantis_Hunter" };
    public static readonly Npc Oni4 = new() { Id = -1, Name = "Oni", WikiUrl = "https://wiki.guildwars.com/wiki/Oni" };
    public static readonly Npc MantisStormcaller1 = new() { Id = -1, Name = "Mantis Stormcaller", WikiUrl = "https://wiki.guildwars.com/wiki/Mantis_Stormcaller" };
    public static readonly Npc KurzickScoutthe = new() { Id = -1, Name = "Kurzick Scout the", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Scout_the" };
    public static readonly Npc AfflictedMesmerDaijun = new() { Id = -1, Name = "Afflicted Mesmer Daijun", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Mesmer_Daijun" };
    public static readonly Npc MantisDreamweaver1 = new() { Id = -1, Name = "Mantis Dreamweaver", WikiUrl = "https://wiki.guildwars.com/wiki/Mantis_Dreamweaver" };
    public static readonly Npc MantidQueenDaijun = new() { Id = -1, Name = "Mantid Queen Daijun", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Queen_Daijun" };
    public static readonly Npc AfflictedMonkDaijun = new() { Id = -1, Name = "Afflicted Monk Daijun", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Monk_Daijun" };
    public static readonly Npc MantisMender1 = new() { Id = -1, Name = "Mantis Mender", WikiUrl = "https://wiki.guildwars.com/wiki/Mantis_Mender" };
    public static readonly Npc GreaterSerpentWarren = new() { Id = -1, Name = "Greater Serpent Warren", WikiUrl = "https://wiki.guildwars.com/wiki/Greater_Serpent_Warren" };
    public static readonly Npc MantidDestroyerDaijun = new() { Id = -1, Name = "Mantid Destroyer Daijun", WikiUrl = "https://wiki.guildwars.com/wiki/Mantid_Destroyer_Daijun" };
    public static readonly Npc FungalWallowVeil = new() { Id = -1, Name = "Fungal Wallow Veil", WikiUrl = "https://wiki.guildwars.com/wiki/Fungal_Wallow_Veil" };
    public static readonly Npc GateGuardPalace = new() { Id = -1, Name = "Gate Guard Palace", WikiUrl = "https://wiki.guildwars.com/wiki/Gate_Guard_Palace" };
    public static readonly Npc AfflictedRitualistChosEstate = new() { Id = -1, Name = "Afflicted Ritualist Cho's Estate", WikiUrl = "https://wiki.guildwars.com/wiki/Afflicted_Ritualist_Cho's_Estate" };
    public static readonly Npc KurzickMineCleanserAspenwood = new() { Id = -1, Name = "Kurzick Mine Cleanser Aspenwood", WikiUrl = "https://wiki.guildwars.com/wiki/Kurzick_Mine_Cleanser_Aspenwood" };
    public static readonly Npc MasterTogoTemple = new() { Id = -1, Name = "Master Togo Temple", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Togo_Temple" };
    public static readonly Npc Irukandji1 = new() { Id = -1, Name = "Irukandji", WikiUrl = "https://wiki.guildwars.com/wiki/Irukandji" };
    public static readonly Npc NagaRitualistSeabed = new() { Id = -1, Name = "Naga Ritualist Seabed", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Ritualist_Seabed" };
    public static readonly Npc NagaWitch1 = new() { Id = -1, Name = "Naga Witch", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Witch" };
    public static readonly Npc NagaWitch2 = new() { Id = -1, Name = "Naga Witch", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Witch" };
    public static readonly Npc ExplosiveGrowthWarren = new() { Id = -1, Name = "Explosive Growth Warren", WikiUrl = "https://wiki.guildwars.com/wiki/Explosive_Growth_Warren" };
    public static readonly Npc EurayleCenter = new() { Id = -1, Name = "Eurayle Center", WikiUrl = "https://wiki.guildwars.com/wiki/Eurayle_Center" };
    public static readonly Npc MarketMerchant = new() { Id = -1, Name = "Market Merchant", WikiUrl = "https://wiki.guildwars.com/wiki/Market_Merchant" };
    public static readonly Npc VashCenter = new() { Id = -1, Name = "Vash Center", WikiUrl = "https://wiki.guildwars.com/wiki/Vash_Center" };
    public static readonly Npc Vling = new() { Id = -1, Name = "Vling", WikiUrl = "https://wiki.guildwars.com/wiki/Vling" };
    public static readonly Npc Zenku = new() { Id = -1, Name = "Zenku", WikiUrl = "https://wiki.guildwars.com/wiki/Zenku" };
    public static readonly Npc FarmerDonlai = new() { Id = -1, Name = "Farmer Donlai", WikiUrl = "https://wiki.guildwars.com/wiki/Farmer_Donlai" };
    public static readonly Npc PetElderCrabSeabed = new() { Id = -1, Name = "Pet – Elder Crab Seabed", WikiUrl = "https://wiki.guildwars.com/wiki/Pet_-_Elder_Crab_Seabed" };
    public static readonly Npc TigerHatchery = new() { Id = -1, Name = "Tiger Hatchery", WikiUrl = "https://wiki.guildwars.com/wiki/Tiger_Hatchery" };
    public static readonly Npc UrgozWarren = new() { Id = -1, Name = "Urgoz Warren", WikiUrl = "https://wiki.guildwars.com/wiki/Urgoz_Warren" };
    public static readonly Npc WardenoftheTrunk1 = new() { Id = -1, Name = "Warden of the Trunk", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_the_Trunk" };
    public static readonly Npc Undergrowth1 = new() { Id = -1, Name = "Undergrowth", WikiUrl = "https://wiki.guildwars.com/wiki/Undergrowth" };
    public static readonly Npc LeviathanClaw1 = new() { Id = -1, Name = "Leviathan Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Leviathan_Claw" };
    public static readonly Npc CanthanGuard6 = new() { Id = -1, Name = "Canthan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Guard" };
    public static readonly Npc InscribedWall = new() { Id = 15, Name = "Inscribed Wall", WikiUrl = "https://wiki.guildwars.com/wiki/Inscribed_Wall" };
    public static readonly Npc FieldGeneralHayao = new() { Id = 107, Name = "Field General Hayao", WikiUrl = "https://wiki.guildwars.com/wiki/Field_General_Hayao" };
    public static readonly Npc GeneralYurukaro = new() { Id = 110, Name = "General Yurukaro", WikiUrl = "https://wiki.guildwars.com/wiki/General_Yurukaro" };
    public static readonly Npc CanthanAmbassador1 = new() { Id = 216, Name = "Canthan Ambassador", WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_Ambassador" };
    public static readonly Npc Garemmof = new() { Id = 218, Name = "Garemm of", WikiUrl = "https://wiki.guildwars.com/wiki/Garemm_of" };
    public static readonly Npc XunlaiAgentJueh = new() { Id = 220, Name = "Xunlai Agent Jueh", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Agent_Jueh" };
    public static readonly Npc XunlaiAgentMomo = new() { Id = 220, Name = "Xunlai Agent Momo", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Agent_Momo" };
    public static readonly Npc XunlaiAgent1 = new() { Id = 221, Name = "Xunlai Agent", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Agent" };
    public static readonly Npc Alfred = new() { Id = 224, Name = "Alfred", WikiUrl = "https://wiki.guildwars.com/wiki/Alfred" };
    public static readonly Npc Susahn = new() { Id = 1202, Name = "Susahn", WikiUrl = "https://wiki.guildwars.com/wiki/Susahn" };
    public static readonly Npc Warthog = new() { Id = 1343, Name = "Warthog", WikiUrl = "https://wiki.guildwars.com/wiki/Warthog" };
    public static readonly Npc ImpressiveWarthog = new() { Id = 1391, Name = "Impressive Warthog", WikiUrl = "https://wiki.guildwars.com/wiki/Impressive_Warthog" };
    public static readonly Npc RiverDrake = new() { Id = 1407, Name = "River Drake", WikiUrl = "https://wiki.guildwars.com/wiki/River_Drake" };
    public static readonly Npc LightningDrake = new() { Id = 1708, Name = "Lightning Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Lightning_Drake" };
    public static readonly Npc GrandDrake = new() { Id = 1709, Name = "Grand Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Grand_Drake" };
    public static readonly Npc SandDrake = new() { Id = 1789, Name = "Sand Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Sand_Drake" };
    public static readonly Npc ForgottenDefender = new() { Id = 1857, Name = "Forgotten Defender", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Defender" };
    public static readonly Npc ForgottenIllusionist = new() { Id = 1857, Name = "Forgotten Illusionist", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Illusionist" };
    public static readonly Npc ForgottenCursebearer = new() { Id = 1858, Name = "Forgotten Cursebearer", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Cursebearer" };
    public static readonly Npc ForgottenDefender1 = new() { Id = 1858, Name = "Forgotten Defender", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Defender" };
    public static readonly Npc ForgottenArcanist = new() { Id = 1859, Name = "Forgotten Arcanist", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Arcanist" };
    public static readonly Npc ForgottenDefender2 = new() { Id = 1859, Name = "Forgotten Defender", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Defender" };
    public static readonly Npc ForgottenSage = new() { Id = 1860, Name = "Forgotten Sage", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Sage" };
    public static readonly Npc KeeperHalyssi = new() { Id = 1870, Name = "Keeper Halyssi", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_Halyssi" };
    public static readonly Npc KeeperJinyssa = new() { Id = 1870, Name = "Keeper Jinyssa", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_Jinyssa" };
    public static readonly Npc KeeperKauniss = new() { Id = 1870, Name = "Keeper Kauniss", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_Kauniss" };
    public static readonly Npc KeeperZeliss = new() { Id = 1870, Name = "Keeper Zeliss", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_Zeliss" };
    public static readonly Npc RunicOracle = new() { Id = 1870, Name = "Runic Oracle", WikiUrl = "https://wiki.guildwars.com/wiki/Runic_Oracle" };
    public static readonly Npc Olias = new() { Id = 1934, Name = "Olias", WikiUrl = "https://wiki.guildwars.com/wiki/Olias" };
    public static readonly Npc LyssasMuse = new() { Id = 1940, Name = "Lyssa’s Muse", WikiUrl = "https://wiki.guildwars.com/wiki/Lyssa's_Muse" };
    public static readonly Npc VoiceofGrenth = new() { Id = 1941, Name = "Voice of Grenth", WikiUrl = "https://wiki.guildwars.com/wiki/Voice_of_Grenth" };
    public static readonly Npc AvatarofDwayna = new() { Id = 1942, Name = "Avatar of Dwayna", WikiUrl = "https://wiki.guildwars.com/wiki/Avatar_of_Dwayna" };
    public static readonly Npc ChampionofBalthazar = new() { Id = 1943, Name = "Champion of Balthazar", WikiUrl = "https://wiki.guildwars.com/wiki/Champion_of_Balthazar" };
    public static readonly Npc MelandrusWatcher = new() { Id = 1944, Name = "Melandru’s Watcher", WikiUrl = "https://wiki.guildwars.com/wiki/Melandru's_Watcher" };
    public static readonly Npc Diane1 = new() { Id = 1956, Name = "Diane", WikiUrl = "https://wiki.guildwars.com/wiki/Diane" };
    public static readonly Npc DerasTenderlin = new() { Id = 1960, Name = "Deras Tenderlin", WikiUrl = "https://wiki.guildwars.com/wiki/Deras_Tenderlin" };
    public static readonly Npc Durmand1 = new() { Id = 1987, Name = "Durmand", WikiUrl = "https://wiki.guildwars.com/wiki/Durmand" };
    public static readonly Npc Tuomas1 = new() { Id = 2062, Name = "Tuomas", WikiUrl = "https://wiki.guildwars.com/wiki/Tuomas" };
    public static readonly Npc TerrorwebDryder = new() { Id = 2317, Name = "Terrorweb Dryder", WikiUrl = "https://wiki.guildwars.com/wiki/Terrorweb_Dryder" };
    public static readonly Npc LostSoul = new() { Id = 2352, Name = "Lost Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Soul" };
    public static readonly Npc LostSoul1 = new() { Id = 2353, Name = "Lost Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Soul" };
    public static readonly Npc LostSoul2 = new() { Id = 2354, Name = "Lost Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Soul" };
    public static readonly Npc LostSoul3 = new() { Id = 2355, Name = "Lost Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Soul" };
    public static readonly Npc ElderCrocodile = new() { Id = 2464, Name = "Elder Crocodile", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Crocodile" };
    public static readonly Npc RisenAshenHulk = new() { Id = 2665, Name = "Risen Ashen Hulk", WikiUrl = "https://wiki.guildwars.com/wiki/Risen_Ashen_Hulk" };
    public static readonly Npc HandoftheTitans = new() { Id = 2667, Name = "Hand of the Titans", WikiUrl = "https://wiki.guildwars.com/wiki/Hand_of_the_Titans" };
    public static readonly Npc FistoftheTitans = new() { Id = 2668, Name = "Fist of the Titans", WikiUrl = "https://wiki.guildwars.com/wiki/Fist_of_the_Titans" };
    public static readonly Npc ArmageddonLord = new() { Id = 2670, Name = "Armageddon Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Armageddon_Lord" };
    public static readonly Npc WindBornTitan = new() { Id = 2671, Name = "Wind Born Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Wind_Born_Titan" };
    public static readonly Npc EarthBornTitan = new() { Id = 2672, Name = "Earth Born Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Earth_Born_Titan" };
    public static readonly Npc WaterBornTitan = new() { Id = 2673, Name = "Water Born Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Born_Titan" };
    public static readonly Npc WildGrowth = new() { Id = 2674, Name = "Wild Growth", WikiUrl = "https://wiki.guildwars.com/wiki/Wild_Growth" };
    public static readonly Npc RottingTitan = new() { Id = 2675, Name = "Rotting Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Rotting_Titan" };
    public static readonly Npc PortalWraith = new() { Id = 2742, Name = "Portal Wraith", WikiUrl = "https://wiki.guildwars.com/wiki/Portal_Wraith" };
    public static readonly Npc ObsidianFurnaceDrake = new() { Id = 2798, Name = "Obsidian Furnace Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Obsidian_Furnace_Drake" };
    public static readonly Npc JatoroMusagi1 = new() { Id = 3051, Name = "Jatoro Musagi", WikiUrl = "https://wiki.guildwars.com/wiki/Jatoro_Musagi" };
    public static readonly Npc ZaishenScout1 = new() { Id = 3075, Name = "Zaishen Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_Scout" };
    public static readonly Npc Mhenlo = new() { Id = 3117, Name = "Mhenlo", WikiUrl = "https://wiki.guildwars.com/wiki/Mhenlo" };
    public static readonly Npc FunwaShento = new() { Id = 3237, Name = "Funwa Shento", WikiUrl = "https://wiki.guildwars.com/wiki/Funwa_Shento" };
    public static readonly Npc Seung = new() { Id = 3240, Name = "Seung", WikiUrl = "https://wiki.guildwars.com/wiki/Seung" };
    public static readonly Npc Akemi1 = new() { Id = 3252, Name = "Akemi", WikiUrl = "https://wiki.guildwars.com/wiki/Akemi" };
    public static readonly Npc Zenmai = new() { Id = 3548, Name = "Zenmai", WikiUrl = "https://wiki.guildwars.com/wiki/Zenmai" };
    public static readonly Npc BoundHaoLi = new() { Id = 4078, Name = "Bound Hao Li", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Hao_Li" };
    public static readonly Npc BoundKaichen = new() { Id = 4081, Name = "Bound Kaichen", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Kaichen" };
    public static readonly Npc BoundTiendi = new() { Id = 4083, Name = "Bound Tiendi", WikiUrl = "https://wiki.guildwars.com/wiki/Bound_Tiendi" };
    public static readonly Npc ShirokenAssassin1 = new() { Id = 4121, Name = "Shiro'ken Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Assassin" };
    public static readonly Npc ShirokenMesmer1 = new() { Id = 4122, Name = "Shiro'ken Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Mesmer" };
    public static readonly Npc ShirokenNecromancer1 = new() { Id = 4123, Name = "Shiro'ken Necromancer", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Necromancer" };
    public static readonly Npc ShirokenElementalist1 = new() { Id = 4124, Name = "Shiro'ken Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Elementalist" };
    public static readonly Npc ShirokenMonk1 = new() { Id = 4125, Name = "Shiro'ken Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Monk" };
    public static readonly Npc ShirokenWarrior1 = new() { Id = 4126, Name = "Shiro'ken Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Warrior" };
    public static readonly Npc ShirokenRanger1 = new() { Id = 4127, Name = "Shiro'ken Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Ranger" };
    public static readonly Npc ShirokenRitualist1 = new() { Id = 4128, Name = "Shiro'ken Ritualist", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Ritualist" };
    public static readonly Npc Hyena = new() { Id = 4237, Name = "Hyena", WikiUrl = "https://wiki.guildwars.com/wiki/Hyena" };
    public static readonly Npc Flamingo = new() { Id = 4238, Name = "Flamingo", WikiUrl = "https://wiki.guildwars.com/wiki/Flamingo" };
    public static readonly Npc TroublesomeFlamingo = new() { Id = 4238, Name = "Troublesome Flamingo", WikiUrl = "https://wiki.guildwars.com/wiki/Troublesome_Flamingo" };
    public static readonly Npc Lioness = new() { Id = 4239, Name = "Lioness", WikiUrl = "https://wiki.guildwars.com/wiki/Lioness" };
    public static readonly Npc Lion = new() { Id = 4240, Name = "Lion", WikiUrl = "https://wiki.guildwars.com/wiki/Lion" };
    public static readonly Npc JahaiRat = new() { Id = 4241, Name = "Jahai Rat", WikiUrl = "https://wiki.guildwars.com/wiki/Jahai_Rat" };
    public static readonly Npc Crocodile = new() { Id = 4242, Name = "Crocodile", WikiUrl = "https://wiki.guildwars.com/wiki/Crocodile" };
    public static readonly Npc ElderHyena = new() { Id = 4254, Name = "Elder Hyena", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Hyena" };
    public static readonly Npc ElderCrocodile1 = new() { Id = 4264, Name = "Elder Crocodile", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Crocodile" };
    public static readonly Npc PetElderCrocodile = new() { Id = 4264, Name = "Pet - Elder Crocodile", WikiUrl = "https://wiki.guildwars.com/wiki/Pet_-_Elder_Crocodile" };
    public static readonly Npc AggressiveLion = new() { Id = 4272, Name = "Aggressive Lion", WikiUrl = "https://wiki.guildwars.com/wiki/Aggressive_Lion" };
    public static readonly Npc AggressiveLioness = new() { Id = 4277, Name = "Aggressive Lioness", WikiUrl = "https://wiki.guildwars.com/wiki/Aggressive_Lioness" };
    public static readonly Npc GreaterInfestation = new() { Id = 4282, Name = "Greater Infestation", WikiUrl = "https://wiki.guildwars.com/wiki/Greater_Infestation" };
    public static readonly Npc Infestation = new() { Id = 4283, Name = "Infestation", WikiUrl = "https://wiki.guildwars.com/wiki/Infestation" };
    public static readonly Npc MaddenedSpirit = new() { Id = 4284, Name = "Maddened Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Maddened_Spirit" };
    public static readonly Npc LostSoul4 = new() { Id = 4285, Name = "Lost Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Soul" };
    public static readonly Npc UndeadSoldier = new() { Id = 4286, Name = "Undead Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Undead_Soldier" };
    public static readonly Npc SolitaryColossus = new() { Id = 4287, Name = "Solitary Colossus", WikiUrl = "https://wiki.guildwars.com/wiki/Solitary_Colossus" };
    public static readonly Npc AchortheBladed = new() { Id = 4289, Name = "Achor the Bladed", WikiUrl = "https://wiki.guildwars.com/wiki/Achor_the_Bladed" };
    public static readonly Npc VahtheCrafty = new() { Id = 4290, Name = "Vah the Crafty", WikiUrl = "https://wiki.guildwars.com/wiki/Vah_the_Crafty" };
    public static readonly Npc HajkorMysticFlame = new() { Id = 4291, Name = "Hajkor Mystic Flame", WikiUrl = "https://wiki.guildwars.com/wiki/Hajkor_Mystic_Flame" };
    public static readonly Npc RendabiDeatheater = new() { Id = 4292, Name = "Rendabi Deatheater", WikiUrl = "https://wiki.guildwars.com/wiki/Rendabi_Deatheater" };
    public static readonly Npc WiolitheInfectious = new() { Id = 4293, Name = "Wioli the Infectious", WikiUrl = "https://wiki.guildwars.com/wiki/Wioli_the_Infectious" };
    public static readonly Npc AjamahnServantoftheSands = new() { Id = 4294, Name = "Ajamahn Servant of the Sands", WikiUrl = "https://wiki.guildwars.com/wiki/Ajamahn_Servant_of_the_Sands" };
    public static readonly Npc JedehtheMighty = new() { Id = 4295, Name = "Jedeh the Mighty", WikiUrl = "https://wiki.guildwars.com/wiki/Jedeh_the_Mighty" };
    public static readonly Npc UhiwitheSmoky = new() { Id = 4296, Name = "Uhiwi the Smoky", WikiUrl = "https://wiki.guildwars.com/wiki/Uhiwi_the_Smoky" };
    public static readonly Npc DunshekthePurifier = new() { Id = 4297, Name = "Dunshek the Purifier", WikiUrl = "https://wiki.guildwars.com/wiki/Dunshek_the_Purifier" };
    public static readonly Npc DroajamMageoftheSands = new() { Id = 4298, Name = "Droajam Mage of the Sands", WikiUrl = "https://wiki.guildwars.com/wiki/Droajam_Mage_of_the_Sands" };
    public static readonly Npc DuneSpider = new() { Id = 4299, Name = "Dune Spider", WikiUrl = "https://wiki.guildwars.com/wiki/Dune_Spider" };
    public static readonly Npc RavenousMandragor = new() { Id = 4301, Name = "Ravenous Mandragor", WikiUrl = "https://wiki.guildwars.com/wiki/Ravenous_Mandragor" };
    public static readonly Npc MandragorSandDevil = new() { Id = 4302, Name = "Mandragor Sand Devil", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Sand_Devil" };
    public static readonly Npc MandragorTerror = new() { Id = 4303, Name = "Mandragor Terror", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Terror" };
    public static readonly Npc RavenousMandragor1 = new() { Id = 4304, Name = "Ravenous Mandragor", WikiUrl = "https://wiki.guildwars.com/wiki/Ravenous_Mandragor" };
    public static readonly Npc RubyDjinn = new() { Id = 4307, Name = "Ruby Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Ruby_Djinn" };
    public static readonly Npc SapphireDjinn = new() { Id = 4308, Name = "Sapphire Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Sapphire_Djinn" };
    public static readonly Npc SadisticGiant = new() { Id = 4309, Name = "Sadistic Giant", WikiUrl = "https://wiki.guildwars.com/wiki/Sadistic_Giant" };
    public static readonly Npc NomadGiant = new() { Id = 4310, Name = "Nomad Giant", WikiUrl = "https://wiki.guildwars.com/wiki/Nomad_Giant" };
    public static readonly Npc GravenMonolith = new() { Id = 4311, Name = "Graven Monolith", WikiUrl = "https://wiki.guildwars.com/wiki/Graven_Monolith" };
    public static readonly Npc GravenMonolith1 = new() { Id = 4312, Name = "Graven Monolith", WikiUrl = "https://wiki.guildwars.com/wiki/Graven_Monolith" };
    public static readonly Npc GravenMonolith2 = new() { Id = 4313, Name = "Graven Monolith", WikiUrl = "https://wiki.guildwars.com/wiki/Graven_Monolith" };
    public static readonly Npc DuneBeetleQueen = new() { Id = 4314, Name = "Dune Beetle Queen", WikiUrl = "https://wiki.guildwars.com/wiki/Dune_Beetle_Queen" };
    public static readonly Npc BladedDuneTermite = new() { Id = 4315, Name = "Bladed Dune Termite", WikiUrl = "https://wiki.guildwars.com/wiki/Bladed_Dune_Termite" };
    public static readonly Npc InfectiousDementia = new() { Id = 4315, Name = "Infectious Dementia", WikiUrl = "https://wiki.guildwars.com/wiki/Infectious_Dementia" };
    public static readonly Npc DuneBeetleLance = new() { Id = 4316, Name = "Dune Beetle Lance", WikiUrl = "https://wiki.guildwars.com/wiki/Dune_Beetle_Lance" };
    public static readonly Npc ShamblingMesa = new() { Id = 4317, Name = "Shambling Mesa", WikiUrl = "https://wiki.guildwars.com/wiki/Shambling_Mesa" };
    public static readonly Npc SandstormCrag = new() { Id = 4318, Name = "Sandstorm Crag", WikiUrl = "https://wiki.guildwars.com/wiki/Sandstorm_Crag" };
    public static readonly Npc DesertWurm = new() { Id = 4319, Name = "Desert Wurm", WikiUrl = "https://wiki.guildwars.com/wiki/Desert_Wurm" };
    public static readonly Npc EmejuLonglegs = new() { Id = 4322, Name = "Emeju Longlegs", WikiUrl = "https://wiki.guildwars.com/wiki/Emeju_Longlegs" };
    public static readonly Npc KesheltheVoracious = new() { Id = 4323, Name = "Keshel the Voracious", WikiUrl = "https://wiki.guildwars.com/wiki/Keshel_the_Voracious" };
    public static readonly Npc NajabLifedrinker = new() { Id = 4324, Name = "Najab Lifedrinker", WikiUrl = "https://wiki.guildwars.com/wiki/Najab_Lifedrinker" };
    public static readonly Npc AjamdukHunteroftheSands = new() { Id = 4325, Name = "Ajamduk Hunter of the Sands", WikiUrl = "https://wiki.guildwars.com/wiki/Ajamduk_Hunter_of_the_Sands" };
    public static readonly Npc TaromRockbreaker = new() { Id = 4326, Name = "Tarom Rockbreaker", WikiUrl = "https://wiki.guildwars.com/wiki/Tarom_Rockbreaker" };
    public static readonly Npc EhyalLongtooth = new() { Id = 4327, Name = "Ehyal Longtooth", WikiUrl = "https://wiki.guildwars.com/wiki/Ehyal_Longtooth" };
    public static readonly Npc HaiossBlessedWind = new() { Id = 4328, Name = "Haioss Blessed Wind", WikiUrl = "https://wiki.guildwars.com/wiki/Haioss_Blessed_Wind" };
    public static readonly Npc ChinehSoaringLight = new() { Id = 4329, Name = "Chineh Soaring Light", WikiUrl = "https://wiki.guildwars.com/wiki/Chineh_Soaring_Light" };
    public static readonly Npc GedossWindcutter = new() { Id = 4330, Name = "Gedoss Windcutter", WikiUrl = "https://wiki.guildwars.com/wiki/Gedoss_Windcutter" };
    public static readonly Npc Apocrypha = new() { Id = 4331, Name = "Apocrypha", WikiUrl = "https://wiki.guildwars.com/wiki/Apocrypha" };
    public static readonly Npc HassinSoftskin = new() { Id = 4332, Name = "Hassin Softskin", WikiUrl = "https://wiki.guildwars.com/wiki/Hassin_Softskin" };
    public static readonly Npc BroodMotherKalwameh = new() { Id = 4333, Name = "Brood Mother Kalwameh", WikiUrl = "https://wiki.guildwars.com/wiki/Brood_Mother_Kalwameh" };
    public static readonly Npc SunehStormbringer = new() { Id = 4334, Name = "Suneh Stormbringer", WikiUrl = "https://wiki.guildwars.com/wiki/Suneh_Stormbringer" };
    public static readonly Npc ModossDarkwind = new() { Id = 4335, Name = "Modoss Darkwind", WikiUrl = "https://wiki.guildwars.com/wiki/Modoss_Darkwind" };
    public static readonly Npc RahtiFlowerofDread = new() { Id = 4336, Name = "Rahti Flower of Dread", WikiUrl = "https://wiki.guildwars.com/wiki/Rahti_Flower_of_Dread" };
    public static readonly Npc BloodbackMorrob = new() { Id = 4337, Name = "Bloodback Morrob", WikiUrl = "https://wiki.guildwars.com/wiki/Bloodback_Morrob" };
    public static readonly Npc LonolunWaterwalker = new() { Id = 4338, Name = "Lonolun Waterwalker", WikiUrl = "https://wiki.guildwars.com/wiki/Lonolun_Waterwalker" };
    public static readonly Npc BehbatheHardheaded = new() { Id = 4339, Name = "Behba the Hardheaded", WikiUrl = "https://wiki.guildwars.com/wiki/Behba_the_Hardheaded" };
    public static readonly Npc DreadLordOnrah = new() { Id = 4340, Name = "Dread Lord Onrah", WikiUrl = "https://wiki.guildwars.com/wiki/Dread_Lord_Onrah" };
    public static readonly Npc StalkingNephilia = new() { Id = 4341, Name = "Stalking Nephilia", WikiUrl = "https://wiki.guildwars.com/wiki/Stalking_Nephilia" };
    public static readonly Npc StalkingNephilia1 = new() { Id = 4342, Name = "Stalking Nephilia", WikiUrl = "https://wiki.guildwars.com/wiki/Stalking_Nephilia" };
    public static readonly Npc StalkingNephilia2 = new() { Id = 4343, Name = "Stalking Nephilia", WikiUrl = "https://wiki.guildwars.com/wiki/Stalking_Nephilia" };
    public static readonly Npc LadyoftheDead = new() { Id = 4345, Name = "Lady of the Dead", WikiUrl = "https://wiki.guildwars.com/wiki/Lady_of_the_Dead" };
    public static readonly Npc WaterDjinn = new() { Id = 4345, Name = "Water Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Djinn" };
    public static readonly Npc WaterDjinn1 = new() { Id = 4347, Name = "Water Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Djinn" };
    public static readonly Npc RinkhalMonitor = new() { Id = 4350, Name = "Rinkhal Monitor", WikiUrl = "https://wiki.guildwars.com/wiki/Rinkhal_Monitor" };
    public static readonly Npc IrontoothDrake = new() { Id = 4352, Name = "Irontooth Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Irontooth_Drake" };
    public static readonly Npc IrontoothDrake1 = new() { Id = 4353, Name = "Irontooth Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Irontooth_Drake" };
    public static readonly Npc SkreeTalon = new() { Id = 4354, Name = "Skree Talon", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Talon" };
    public static readonly Npc SkreeWarbler = new() { Id = 4355, Name = "Skree Warbler", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Warbler" };
    public static readonly Npc SkreeHatchling = new() { Id = 4356, Name = "Skree Hatchling", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Hatchling" };
    public static readonly Npc SkreeFledgeling = new() { Id = 4357, Name = "Skree Fledgeling", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Fledgeling" };
    public static readonly Npc SkreeGriffon = new() { Id = 4358, Name = "Skree Griffon", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Griffon" };
    public static readonly Npc HarpyMother = new() { Id = 4359, Name = "Harpy Mother", WikiUrl = "https://wiki.guildwars.com/wiki/Harpy_Mother" };
    public static readonly Npc SkreeTalon1 = new() { Id = 4359, Name = "Skree Talon", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Talon" };
    public static readonly Npc HarpyMother1 = new() { Id = 4360, Name = "Harpy Mother", WikiUrl = "https://wiki.guildwars.com/wiki/Harpy_Mother" };
    public static readonly Npc SkreeHunter = new() { Id = 4360, Name = "Skree Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Hunter" };
    public static readonly Npc HarpyMother2 = new() { Id = 4361, Name = "Harpy Mother", WikiUrl = "https://wiki.guildwars.com/wiki/Harpy_Mother" };
    public static readonly Npc SkreeWarbler1 = new() { Id = 4361, Name = "Skree Warbler", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Warbler" };
    public static readonly Npc RidgebackScale = new() { Id = 4362, Name = "Ridgeback Scale", WikiUrl = "https://wiki.guildwars.com/wiki/Ridgeback_Scale" };
    public static readonly Npc SkaleLasher = new() { Id = 4363, Name = "Skale Lasher", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_Lasher" };
    public static readonly Npc SkaleBlighter = new() { Id = 4364, Name = "Skale Blighter", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_Blighter" };
    public static readonly Npc RidgebackScale1 = new() { Id = 4365, Name = "Ridgeback Scale", WikiUrl = "https://wiki.guildwars.com/wiki/Ridgeback_Scale" };
    public static readonly Npc SkaleLasher1 = new() { Id = 4366, Name = "Skale Lasher", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_Lasher" };
    public static readonly Npc FrigidSkale = new() { Id = 4367, Name = "Frigid Skale", WikiUrl = "https://wiki.guildwars.com/wiki/Frigid_Skale" };
    public static readonly Npc HungrySkale = new() { Id = 4367, Name = "Hungry Skale", WikiUrl = "https://wiki.guildwars.com/wiki/Hungry_Skale" };
    public static readonly Npc SkaleBlighter1 = new() { Id = 4368, Name = "Skale Blighter", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_Blighter" };
    public static readonly Npc RidgebackSkale = new() { Id = 4369, Name = "Ridgeback Skale", WikiUrl = "https://wiki.guildwars.com/wiki/Ridgeback_Skale" };
    public static readonly Npc SkaleBlighter2 = new() { Id = 4372, Name = "Skale Blighter", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_Blighter" };
    public static readonly Npc RidgebackScale2 = new() { Id = 4373, Name = "Ridgeback Scale", WikiUrl = "https://wiki.guildwars.com/wiki/Ridgeback_Scale" };
    public static readonly Npc SkaleLasher2 = new() { Id = 4374, Name = "Skale Lasher", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_Lasher" };
    public static readonly Npc FierceSkale = new() { Id = 4375, Name = "Fierce Skale", WikiUrl = "https://wiki.guildwars.com/wiki/Fierce_Skale" };
    public static readonly Npc FrigidSkale1 = new() { Id = 4375, Name = "Frigid Skale", WikiUrl = "https://wiki.guildwars.com/wiki/Frigid_Skale" };
    public static readonly Npc JuvenileBladedTermite = new() { Id = 4376, Name = "Juvenile Bladed Termite", WikiUrl = "https://wiki.guildwars.com/wiki/Juvenile_Bladed_Termite" };
    public static readonly Npc GrubLance = new() { Id = 4377, Name = "Grub Lance", WikiUrl = "https://wiki.guildwars.com/wiki/Grub_Lance" };
    public static readonly Npc BladedTermite = new() { Id = 4378, Name = "Bladed Termite", WikiUrl = "https://wiki.guildwars.com/wiki/Bladed_Termite" };
    public static readonly Npc PreyingLance = new() { Id = 4379, Name = "Preying Lance", WikiUrl = "https://wiki.guildwars.com/wiki/Preying_Lance" };
    public static readonly Npc GrubLance1 = new() { Id = 4380, Name = "Grub Lance", WikiUrl = "https://wiki.guildwars.com/wiki/Grub_Lance" };
    public static readonly Npc BladedTermite1 = new() { Id = 4381, Name = "Bladed Termite", WikiUrl = "https://wiki.guildwars.com/wiki/Bladed_Termite" };
    public static readonly Npc PreyingLance1 = new() { Id = 4382, Name = "Preying Lance", WikiUrl = "https://wiki.guildwars.com/wiki/Preying_Lance" };
    public static readonly Npc StormseedJacaranda = new() { Id = 4383, Name = "Stormseed Jacaranda", WikiUrl = "https://wiki.guildwars.com/wiki/Stormseed_Jacaranda" };
    public static readonly Npc FangedIboga = new() { Id = 4384, Name = "Fanged Iboga", WikiUrl = "https://wiki.guildwars.com/wiki/Fanged_Iboga" };
    public static readonly Npc KillerIboga = new() { Id = 4384, Name = "Killer Iboga", WikiUrl = "https://wiki.guildwars.com/wiki/Killer_Iboga" };
    public static readonly Npc StormseedJacaranda1 = new() { Id = 4385, Name = "Stormseed Jacaranda", WikiUrl = "https://wiki.guildwars.com/wiki/Stormseed_Jacaranda" };
    public static readonly Npc FangedIboga1 = new() { Id = 4386, Name = "Fanged Iboga", WikiUrl = "https://wiki.guildwars.com/wiki/Fanged_Iboga" };
    public static readonly Npc GreatFireFlower = new() { Id = 4387, Name = "Great Fire Flower", WikiUrl = "https://wiki.guildwars.com/wiki/Great_Fire_Flower" };
    public static readonly Npc StormseedJacaranda2 = new() { Id = 4387, Name = "Stormseed Jacaranda", WikiUrl = "https://wiki.guildwars.com/wiki/Stormseed_Jacaranda" };
    public static readonly Npc BeautifulIboga = new() { Id = 4388, Name = "Beautiful Iboga", WikiUrl = "https://wiki.guildwars.com/wiki/Beautiful_Iboga" };
    public static readonly Npc FangedIboga2 = new() { Id = 4388, Name = "Fanged Iboga", WikiUrl = "https://wiki.guildwars.com/wiki/Fanged_Iboga" };
    public static readonly Npc MandragorImp = new() { Id = 4391, Name = "Mandragor Imp", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Imp" };
    public static readonly Npc MandragorSlither = new() { Id = 4392, Name = "Mandragor Slither", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Slither" };
    public static readonly Npc MandragorImp1 = new() { Id = 4393, Name = "Mandragor Imp", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Imp" };
    public static readonly Npc StonefleshMandragor = new() { Id = 4394, Name = "Stoneflesh Mandragor", WikiUrl = "https://wiki.guildwars.com/wiki/Stoneflesh_Mandragor" };
    public static readonly Npc MandragorSlither1 = new() { Id = 4395, Name = "Mandragor Slither", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Slither" };
    public static readonly Npc MandragorImp2 = new() { Id = 4396, Name = "Mandragor Imp", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Imp" };
    public static readonly Npc StonefleshMandragor1 = new() { Id = 4397, Name = "Stoneflesh Mandragor", WikiUrl = "https://wiki.guildwars.com/wiki/Stoneflesh_Mandragor" };
    public static readonly Npc Mandragor = new() { Id = 4398, Name = "Mandragor", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor" };
    public static readonly Npc MandragorSlither2 = new() { Id = 4398, Name = "Mandragor Slither", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Slither" };
    public static readonly Npc ImpressiveWarthog1 = new() { Id = 4399, Name = "Impressive Warthog", WikiUrl = "https://wiki.guildwars.com/wiki/Impressive_Warthog" };
    public static readonly Npc GraspofChaos = new() { Id = 4400, Name = "Grasp of Chaos", WikiUrl = "https://wiki.guildwars.com/wiki/Grasp_of_Chaos" };
    public static readonly Npc ScytheofChaos1 = new() { Id = 4401, Name = "Scythe of Chaos", WikiUrl = "https://wiki.guildwars.com/wiki/Scythe_of_Chaos" };
    public static readonly Npc Dehjah = new() { Id = 4402, Name = "Dehjah", WikiUrl = "https://wiki.guildwars.com/wiki/Dehjah" };
    public static readonly Npc KinyaKela = new() { Id = 4402, Name = "Kinya Kela", WikiUrl = "https://wiki.guildwars.com/wiki/Kinya_Kela" };
    public static readonly Npc BalthazarsEternal = new() { Id = 4403, Name = "Balthazar’s Eternal", WikiUrl = "https://wiki.guildwars.com/wiki/Balthazar's_Eternal" };
    public static readonly Npc GhostlyGriffon = new() { Id = 4404, Name = "Ghostly Griffon", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Griffon" };
    public static readonly Npc TombWraith = new() { Id = 4405, Name = "Tomb Wraith", WikiUrl = "https://wiki.guildwars.com/wiki/Tomb_Wraith" };
    public static readonly Npc MoavuKaal = new() { Id = 4406, Name = "Moa’vu’Kaal", WikiUrl = "https://wiki.guildwars.com/wiki/Moa'vu'Kaal" };
    public static readonly Npc SkaleLordJurpa = new() { Id = 4407, Name = "Skale Lord Jurpa", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_Lord_Jurpa" };
    public static readonly Npc RazorbackSkale = new() { Id = 4408, Name = "Razorback Skale", WikiUrl = "https://wiki.guildwars.com/wiki/Razorback_Skale" };
    public static readonly Npc HamouUkaiou = new() { Id = 4409, Name = "Hamou Ukaiou", WikiUrl = "https://wiki.guildwars.com/wiki/Hamou_Ukaiou" };
    public static readonly Npc BoklonBlackwater = new() { Id = 4410, Name = "Boklon Blackwater", WikiUrl = "https://wiki.guildwars.com/wiki/Boklon_Blackwater" };
    public static readonly Npc SkreeHatchling1 = new() { Id = 4413, Name = "Skree Hatchling", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Hatchling" };
    public static readonly Npc IbogaRoot = new() { Id = 4414, Name = "Iboga Root", WikiUrl = "https://wiki.guildwars.com/wiki/Iboga_Root" };
    public static readonly Npc TombGuardian = new() { Id = 4415, Name = "Tomb Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Tomb_Guardian" };
    public static readonly Npc GlugKlugg = new() { Id = 4416, Name = "Glug Klugg", WikiUrl = "https://wiki.guildwars.com/wiki/Glug_Klugg" };
    public static readonly Npc MandragorQueen = new() { Id = 4417, Name = "Mandragor Queen", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Queen" };
    public static readonly Npc Suitof15Armor = new() { Id = 4418, Name = "Suit of 15 Armor", WikiUrl = "https://wiki.guildwars.com/wiki/Suit_of_15_Armor" };
    public static readonly Npc Suitof35Armor = new() { Id = 4419, Name = "Suit of 35 Armor", WikiUrl = "https://wiki.guildwars.com/wiki/Suit_of_35_Armor" };
    public static readonly Npc Suitof55Armor = new() { Id = 4420, Name = "Suit of 55 Armor", WikiUrl = "https://wiki.guildwars.com/wiki/Suit_of_55_Armor" };
    public static readonly Npc Adjacent = new() { Id = 4421, Name = "Adjacent", WikiUrl = "https://wiki.guildwars.com/wiki/Adjacent" };
    public static readonly Npc IntheArea = new() { Id = 4421, Name = "In the Area", WikiUrl = "https://wiki.guildwars.com/wiki/In_the_Area" };
    public static readonly Npc LongbowTarget = new() { Id = 4421, Name = "Longbow Target", WikiUrl = "https://wiki.guildwars.com/wiki/Longbow_Target" };
    public static readonly Npc Nearby = new() { Id = 4421, Name = "Nearby", WikiUrl = "https://wiki.guildwars.com/wiki/Nearby" };
    public static readonly Npc PracticeTarget = new() { Id = 4421, Name = "Practice Target", WikiUrl = "https://wiki.guildwars.com/wiki/Practice_Target" };
    public static readonly Npc RecurveTarget = new() { Id = 4421, Name = "Recurve Target", WikiUrl = "https://wiki.guildwars.com/wiki/Recurve_Target" };
    public static readonly Npc ShortBowTarget = new() { Id = 4421, Name = "Short Bow Target", WikiUrl = "https://wiki.guildwars.com/wiki/Short_Bow_Target" };
    public static readonly Npc Norgu = new() { Id = 4425, Name = "Norgu", WikiUrl = "https://wiki.guildwars.com/wiki/Norgu" };
    public static readonly Npc Goren = new() { Id = 4430, Name = "Goren", WikiUrl = "https://wiki.guildwars.com/wiki/Goren" };
    public static readonly Npc Goren1 = new() { Id = 4431, Name = "Goren", WikiUrl = "https://wiki.guildwars.com/wiki/Goren" };
    public static readonly Npc ZhedShadowhoof = new() { Id = 4436, Name = "Zhed Shadowhoof", WikiUrl = "https://wiki.guildwars.com/wiki/Zhed_Shadowhoof" };
    public static readonly Npc GeneralMorgahn = new() { Id = 4442, Name = "General Morgahn", WikiUrl = "https://wiki.guildwars.com/wiki/General_Morgahn" };
    public static readonly Npc Margrid = new() { Id = 4447, Name = "Margrid", WikiUrl = "https://wiki.guildwars.com/wiki/Margrid" };
    public static readonly Npc MargridtheSly = new() { Id = 4447, Name = "Margrid the Sly", WikiUrl = "https://wiki.guildwars.com/wiki/Margrid_the_Sly" };
    public static readonly Npc Tahlkora = new() { Id = 4455, Name = "Tahlkora", WikiUrl = "https://wiki.guildwars.com/wiki/Tahlkora" };
    public static readonly Npc MasterofWhispers = new() { Id = 4460, Name = "Master of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Master_of_Whispers" };
    public static readonly Npc AcolyteJin = new() { Id = 4465, Name = "Acolyte Jin", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Jin" };
    public static readonly Npc Koss = new() { Id = 4468, Name = "Koss", WikiUrl = "https://wiki.guildwars.com/wiki/Koss" };
    public static readonly Npc Koss1 = new() { Id = 4471, Name = "Koss", WikiUrl = "https://wiki.guildwars.com/wiki/Koss" };
    public static readonly Npc Koss2 = new() { Id = 4473, Name = "Koss", WikiUrl = "https://wiki.guildwars.com/wiki/Koss" };
    public static readonly Npc Dunkoro = new() { Id = 4479, Name = "Dunkoro", WikiUrl = "https://wiki.guildwars.com/wiki/Dunkoro" };
    public static readonly Npc Acolyte = new() { Id = 4486, Name = "Acolyte", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte" };
    public static readonly Npc AcolyteSousuke = new() { Id = 4486, Name = "Acolyte Sousuke", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Sousuke" };
    public static readonly Npc Melonni = new() { Id = 4492, Name = "Melonni", WikiUrl = "https://wiki.guildwars.com/wiki/Melonni" };
    public static readonly Npc Razah = new() { Id = 4498, Name = "Razah", WikiUrl = "https://wiki.guildwars.com/wiki/Razah" };
    public static readonly Npc Kihm = new() { Id = 4527, Name = "Kihm", WikiUrl = "https://wiki.guildwars.com/wiki/Kihm" };
    public static readonly Npc Herta = new() { Id = 4528, Name = "Herta", WikiUrl = "https://wiki.guildwars.com/wiki/Herta" };
    public static readonly Npc Gehraz = new() { Id = 4529, Name = "Gehraz", WikiUrl = "https://wiki.guildwars.com/wiki/Gehraz" };
    public static readonly Npc Sogolon = new() { Id = 4530, Name = "Sogolon", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon" };
    public static readonly Npc Kihm1 = new() { Id = 4531, Name = "Kihm", WikiUrl = "https://wiki.guildwars.com/wiki/Kihm" };
    public static readonly Npc Odurra = new() { Id = 4532, Name = "Odurra", WikiUrl = "https://wiki.guildwars.com/wiki/Odurra" };
    public static readonly Npc Herta1 = new() { Id = 4533, Name = "Herta", WikiUrl = "https://wiki.guildwars.com/wiki/Herta" };
    public static readonly Npc Timera = new() { Id = 4534, Name = "Timera", WikiUrl = "https://wiki.guildwars.com/wiki/Timera" };
    public static readonly Npc Abasi = new() { Id = 4535, Name = "Abasi", WikiUrl = "https://wiki.guildwars.com/wiki/Abasi" };
    public static readonly Npc Gehraz1 = new() { Id = 4536, Name = "Gehraz", WikiUrl = "https://wiki.guildwars.com/wiki/Gehraz" };
    public static readonly Npc Sogolon1 = new() { Id = 4537, Name = "Sogolon", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon" };
    public static readonly Npc Kihm2 = new() { Id = 4538, Name = "Kihm", WikiUrl = "https://wiki.guildwars.com/wiki/Kihm" };
    public static readonly Npc Odurra1 = new() { Id = 4539, Name = "Odurra", WikiUrl = "https://wiki.guildwars.com/wiki/Odurra" };
    public static readonly Npc Herta2 = new() { Id = 4540, Name = "Herta", WikiUrl = "https://wiki.guildwars.com/wiki/Herta" };
    public static readonly Npc Timera1 = new() { Id = 4541, Name = "Timera", WikiUrl = "https://wiki.guildwars.com/wiki/Timera" };
    public static readonly Npc Abasi1 = new() { Id = 4542, Name = "Abasi", WikiUrl = "https://wiki.guildwars.com/wiki/Abasi" };
    public static readonly Npc Gehraz2 = new() { Id = 4543, Name = "Gehraz", WikiUrl = "https://wiki.guildwars.com/wiki/Gehraz" };
    public static readonly Npc Sogolon2 = new() { Id = 4544, Name = "Sogolon", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon" };
    public static readonly Npc Kihm3 = new() { Id = 4545, Name = "Kihm", WikiUrl = "https://wiki.guildwars.com/wiki/Kihm" };
    public static readonly Npc Odurra2 = new() { Id = 4546, Name = "Odurra", WikiUrl = "https://wiki.guildwars.com/wiki/Odurra" };
    public static readonly Npc Herta3 = new() { Id = 4547, Name = "Herta", WikiUrl = "https://wiki.guildwars.com/wiki/Herta" };
    public static readonly Npc Timera2 = new() { Id = 4548, Name = "Timera", WikiUrl = "https://wiki.guildwars.com/wiki/Timera" };
    public static readonly Npc Abasi2 = new() { Id = 4549, Name = "Abasi", WikiUrl = "https://wiki.guildwars.com/wiki/Abasi" };
    public static readonly Npc Gehraz3 = new() { Id = 4550, Name = "Gehraz", WikiUrl = "https://wiki.guildwars.com/wiki/Gehraz" };
    public static readonly Npc Sogolon3 = new() { Id = 4551, Name = "Sogolon", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon" };
    public static readonly Npc Kihm4 = new() { Id = 4552, Name = "Kihm", WikiUrl = "https://wiki.guildwars.com/wiki/Kihm" };
    public static readonly Npc Odurra3 = new() { Id = 4553, Name = "Odurra", WikiUrl = "https://wiki.guildwars.com/wiki/Odurra" };
    public static readonly Npc Herta4 = new() { Id = 4554, Name = "Herta", WikiUrl = "https://wiki.guildwars.com/wiki/Herta" };
    public static readonly Npc Timera3 = new() { Id = 4555, Name = "Timera", WikiUrl = "https://wiki.guildwars.com/wiki/Timera" };
    public static readonly Npc Abasi3 = new() { Id = 4556, Name = "Abasi", WikiUrl = "https://wiki.guildwars.com/wiki/Abasi" };
    public static readonly Npc Gehraz4 = new() { Id = 4557, Name = "Gehraz", WikiUrl = "https://wiki.guildwars.com/wiki/Gehraz" };
    public static readonly Npc Sogolon4 = new() { Id = 4558, Name = "Sogolon", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon" };
    public static readonly Npc Odurra4 = new() { Id = 4559, Name = "Odurra", WikiUrl = "https://wiki.guildwars.com/wiki/Odurra" };
    public static readonly Npc Eve3 = new() { Id = 4560, Name = "Eve", WikiUrl = "https://wiki.guildwars.com/wiki/Eve" };
    public static readonly Npc Cynn6 = new() { Id = 4561, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc Herta5 = new() { Id = 4562, Name = "Herta", WikiUrl = "https://wiki.guildwars.com/wiki/Herta" };
    public static readonly Npc Kihm5 = new() { Id = 4564, Name = "Kihm", WikiUrl = "https://wiki.guildwars.com/wiki/Kihm" };
    public static readonly Npc Devona4 = new() { Id = 4565, Name = "Devona", WikiUrl = "https://wiki.guildwars.com/wiki/Devona" };
    public static readonly Npc Aidan5 = new() { Id = 4566, Name = "Aidan", WikiUrl = "https://wiki.guildwars.com/wiki/Aidan" };
    public static readonly Npc Gehraz5 = new() { Id = 4567, Name = "Gehraz", WikiUrl = "https://wiki.guildwars.com/wiki/Gehraz" };
    public static readonly Npc Sogolon5 = new() { Id = 4568, Name = "Sogolon", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon" };
    public static readonly Npc Odurra5 = new() { Id = 4569, Name = "Odurra", WikiUrl = "https://wiki.guildwars.com/wiki/Odurra" };
    public static readonly Npc Eve4 = new() { Id = 4570, Name = "Eve", WikiUrl = "https://wiki.guildwars.com/wiki/Eve" };
    public static readonly Npc Cynn7 = new() { Id = 4571, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc Herta6 = new() { Id = 4572, Name = "Herta", WikiUrl = "https://wiki.guildwars.com/wiki/Herta" };
    public static readonly Npc Mhenlo1 = new() { Id = 4573, Name = "Mhenlo", WikiUrl = "https://wiki.guildwars.com/wiki/Mhenlo" };
    public static readonly Npc Kihm6 = new() { Id = 4574, Name = "Kihm", WikiUrl = "https://wiki.guildwars.com/wiki/Kihm" };
    public static readonly Npc Devona5 = new() { Id = 4575, Name = "Devona", WikiUrl = "https://wiki.guildwars.com/wiki/Devona" };
    public static readonly Npc Aidan6 = new() { Id = 4576, Name = "Aidan", WikiUrl = "https://wiki.guildwars.com/wiki/Aidan" };
    public static readonly Npc Gehraz6 = new() { Id = 4577, Name = "Gehraz", WikiUrl = "https://wiki.guildwars.com/wiki/Gehraz" };
    public static readonly Npc Sogolon6 = new() { Id = 4578, Name = "Sogolon", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon" };
    public static readonly Npc Odurra6 = new() { Id = 4579, Name = "Odurra", WikiUrl = "https://wiki.guildwars.com/wiki/Odurra" };
    public static readonly Npc Eve5 = new() { Id = 4580, Name = "Eve", WikiUrl = "https://wiki.guildwars.com/wiki/Eve" };
    public static readonly Npc Cynn8 = new() { Id = 4581, Name = "Cynn", WikiUrl = "https://wiki.guildwars.com/wiki/Cynn" };
    public static readonly Npc Herta7 = new() { Id = 4582, Name = "Herta", WikiUrl = "https://wiki.guildwars.com/wiki/Herta" };
    public static readonly Npc Mhenlo2 = new() { Id = 4583, Name = "Mhenlo", WikiUrl = "https://wiki.guildwars.com/wiki/Mhenlo" };
    public static readonly Npc Devona6 = new() { Id = 4585, Name = "Devona", WikiUrl = "https://wiki.guildwars.com/wiki/Devona" };
    public static readonly Npc Aidan7 = new() { Id = 4586, Name = "Aidan", WikiUrl = "https://wiki.guildwars.com/wiki/Aidan" };
    public static readonly Npc Gehraz7 = new() { Id = 4587, Name = "Gehraz", WikiUrl = "https://wiki.guildwars.com/wiki/Gehraz" };
    public static readonly Npc Sogolon7 = new() { Id = 4588, Name = "Sogolon", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon" };
    public static readonly Npc Shiny = new() { Id = 4589, Name = "Shiny", WikiUrl = "https://wiki.guildwars.com/wiki/Shiny" };
    public static readonly Npc ThunderofAhdashim = new() { Id = 4590, Name = "Thunder of Ahdashim", WikiUrl = "https://wiki.guildwars.com/wiki/Thunder_of_Ahdashim" };
    public static readonly Npc DivineGuardian = new() { Id = 4591, Name = "Divine Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Divine_Guardian" };
    public static readonly Npc EternalGuardianofLanguor = new() { Id = 4591, Name = "Eternal Guardian of Languor", WikiUrl = "https://wiki.guildwars.com/wiki/Eternal_Guardian_of_Languor" };
    public static readonly Npc EternalGuardianofLethargy = new() { Id = 4591, Name = "Eternal Guardian of Lethargy", WikiUrl = "https://wiki.guildwars.com/wiki/Eternal_Guardian_of_Lethargy" };
    public static readonly Npc EternalGuardianofSuffering = new() { Id = 4591, Name = "Eternal Guardian of Suffering", WikiUrl = "https://wiki.guildwars.com/wiki/Eternal_Guardian_of_Suffering" };
    public static readonly Npc KeyofAhdashim = new() { Id = 4591, Name = "Key of Ahdashim", WikiUrl = "https://wiki.guildwars.com/wiki/Key_of_Ahdashim" };
    public static readonly Npc LockofAhdashim = new() { Id = 4591, Name = "Lock of Ahdashim", WikiUrl = "https://wiki.guildwars.com/wiki/Lock_of_Ahdashim" };
    public static readonly Npc KormabBurningHeart = new() { Id = 4596, Name = "Kormab Burning Heart", WikiUrl = "https://wiki.guildwars.com/wiki/Kormab_Burning_Heart" };
    public static readonly Npc HajokEarthguardian = new() { Id = 4597, Name = "Hajok Earthguardian", WikiUrl = "https://wiki.guildwars.com/wiki/Hajok_Earthguardian" };
    public static readonly Npc ShakorFirespear = new() { Id = 4598, Name = "Shakor Firespear", WikiUrl = "https://wiki.guildwars.com/wiki/Shakor_Firespear" };
    public static readonly Npc DjinnOverseer = new() { Id = 4599, Name = "Djinn Overseer", WikiUrl = "https://wiki.guildwars.com/wiki/Djinn_Overseer" };
    public static readonly Npc FireLord = new() { Id = 4600, Name = "Fire Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Fire_Lord" };
    public static readonly Npc WaterLord = new() { Id = 4601, Name = "Water Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Lord" };
    public static readonly Npc Render = new() { Id = 4603, Name = "Render", WikiUrl = "https://wiki.guildwars.com/wiki/Render" };
    public static readonly Npc Scratcher = new() { Id = 4603, Name = "Scratcher", WikiUrl = "https://wiki.guildwars.com/wiki/Scratcher" };
    public static readonly Npc Screecher = new() { Id = 4603, Name = "Screecher", WikiUrl = "https://wiki.guildwars.com/wiki/Screecher" };
    public static readonly Npc BrokkRipsnort = new() { Id = 4604, Name = "Brokk Ripsnort", WikiUrl = "https://wiki.guildwars.com/wiki/Brokk_Ripsnort" };
    public static readonly Npc HarrkFacestab = new() { Id = 4604, Name = "Harrk Facestab", WikiUrl = "https://wiki.guildwars.com/wiki/Harrk_Facestab" };
    public static readonly Npc BlueTongueHeket = new() { Id = 4613, Name = "Blue Tongue Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Blue_Tongue_Heket" };
    public static readonly Npc BloodCowlHeket = new() { Id = 4614, Name = "Blood Cowl Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_Cowl_Heket" };
    public static readonly Npc StoneaxeHeket = new() { Id = 4615, Name = "Stoneaxe Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Stoneaxe_Heket" };
    public static readonly Npc BeastSwornHeket = new() { Id = 4616, Name = "Beast Sworn Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Beast_Sworn_Heket" };
    public static readonly Npc RiktundtheVicious = new() { Id = 4620, Name = "Riktund the Vicious", WikiUrl = "https://wiki.guildwars.com/wiki/Riktund_the_Vicious" };
    public static readonly Npc PehnsedtheLoudmouth = new() { Id = 4621, Name = "Pehnsed the Loudmouth", WikiUrl = "https://wiki.guildwars.com/wiki/Pehnsed_the_Loudmouth" };
    public static readonly Npc JisholDarksong = new() { Id = 4622, Name = "Jishol Darksong", WikiUrl = "https://wiki.guildwars.com/wiki/Jishol_Darksong" };
    public static readonly Npc MarobehSharptail = new() { Id = 4623, Name = "Marobeh Sharptail", WikiUrl = "https://wiki.guildwars.com/wiki/Marobeh_Sharptail" };
    public static readonly Npc EshekibehLongneck = new() { Id = 4624, Name = "Eshekibeh Longneck", WikiUrl = "https://wiki.guildwars.com/wiki/Eshekibeh_Longneck" };
    public static readonly Npc KorshektheImmolated = new() { Id = 4625, Name = "Korshek the Immolated", WikiUrl = "https://wiki.guildwars.com/wiki/Korshek_the_Immolated" };
    public static readonly Npc ChurahmSpiritWarrior = new() { Id = 4626, Name = "Churahm Spirit Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Churahm_Spirit_Warrior" };
    public static readonly Npc KormabBurningHeart1 = new() { Id = 4627, Name = "Kormab Burning Heart", WikiUrl = "https://wiki.guildwars.com/wiki/Kormab_Burning_Heart" };
    public static readonly Npc KorrubFlameofDreams = new() { Id = 4628, Name = "Korrub Flame of Dreams", WikiUrl = "https://wiki.guildwars.com/wiki/Korrub_Flame_of_Dreams" };
    public static readonly Npc LeilonTranquilWater = new() { Id = 4629, Name = "Leilon Tranquil Water", WikiUrl = "https://wiki.guildwars.com/wiki/Leilon_Tranquil_Water" };
    public static readonly Npc ShakorFirespear1 = new() { Id = 4630, Name = "Shakor Firespear", WikiUrl = "https://wiki.guildwars.com/wiki/Shakor_Firespear" };
    public static readonly Npc HojanukunMindstealer = new() { Id = 4631, Name = "Hojanukun Mindstealer", WikiUrl = "https://wiki.guildwars.com/wiki/Hojanukun_Mindstealer" };
    public static readonly Npc YammironEtherLord = new() { Id = 4632, Name = "Yammiron Ether Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Yammiron_Ether_Lord" };
    public static readonly Npc YammirvuEtherGuardian = new() { Id = 4633, Name = "Yammirvu Ether Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Yammirvu_Ether_Guardian" };
    public static readonly Npc MakdehtheAggravating = new() { Id = 4634, Name = "Makdeh the Aggravating", WikiUrl = "https://wiki.guildwars.com/wiki/Makdeh_the_Aggravating" };
    public static readonly Npc HanchorTrueblade = new() { Id = 4635, Name = "Hanchor Trueblade", WikiUrl = "https://wiki.guildwars.com/wiki/Hanchor_Trueblade" };
    public static readonly Npc NehpektheRemorseless = new() { Id = 4636, Name = "Nehpek the Remorseless", WikiUrl = "https://wiki.guildwars.com/wiki/Nehpek_the_Remorseless" };
    public static readonly Npc ShakJarintheJusticebringer = new() { Id = 4637, Name = "Shak-Jarin the Justicebringer", WikiUrl = "https://wiki.guildwars.com/wiki/Shak-Jarin_the_Justicebringer" };
    public static readonly Npc JosinqtheWhisperer = new() { Id = 4638, Name = "Josinq the Whisperer", WikiUrl = "https://wiki.guildwars.com/wiki/Josinq_the_Whisperer" };
    public static readonly Npc BanshehGathererofBranches = new() { Id = 4639, Name = "Bansheh Gatherer of Branches", WikiUrl = "https://wiki.guildwars.com/wiki/Bansheh_Gatherer_of_Branches" };
    public static readonly Npc MotehThundershooter = new() { Id = 4640, Name = "Moteh Thundershooter", WikiUrl = "https://wiki.guildwars.com/wiki/Moteh_Thundershooter" };
    public static readonly Npc KunantheLoudmouth = new() { Id = 4641, Name = "Kunan the Loudmouth", WikiUrl = "https://wiki.guildwars.com/wiki/Kunan_the_Loudmouth" };
    public static readonly Npc HajokEarthguardian1 = new() { Id = 4642, Name = "Hajok Earthguardian", WikiUrl = "https://wiki.guildwars.com/wiki/Hajok_Earthguardian" };
    public static readonly Npc TenezeltheQuick = new() { Id = 4643, Name = "Tenezel the Quick", WikiUrl = "https://wiki.guildwars.com/wiki/Tenezel_the_Quick" };
    public static readonly Npc TenshekRoundbody = new() { Id = 4644, Name = "Tenshek Roundbody", WikiUrl = "https://wiki.guildwars.com/wiki/Tenshek_Roundbody" };
    public static readonly Npc ToshauSharpspear = new() { Id = 4645, Name = "Toshau Sharpspear", WikiUrl = "https://wiki.guildwars.com/wiki/Toshau_Sharpspear" };
    public static readonly Npc ShezelSlowreaper = new() { Id = 4646, Name = "Shezel Slowreaper", WikiUrl = "https://wiki.guildwars.com/wiki/Shezel_Slowreaper" };
    public static readonly Npc SetikorFireflower = new() { Id = 4647, Name = "Setikor Fireflower", WikiUrl = "https://wiki.guildwars.com/wiki/Setikor_Fireflower" };
    public static readonly Npc BanorGreenbranch = new() { Id = 4648, Name = "Banor Greenbranch", WikiUrl = "https://wiki.guildwars.com/wiki/Banor_Greenbranch" };
    public static readonly Npc YameshMindclouder = new() { Id = 4649, Name = "Yamesh Mindclouder", WikiUrl = "https://wiki.guildwars.com/wiki/Yamesh_Mindclouder" };
    public static readonly Npc MabahHeardheart = new() { Id = 4650, Name = "Mabah Heardheart", WikiUrl = "https://wiki.guildwars.com/wiki/Mabah_Heardheart" };
    public static readonly Npc HahanFaithfulProtector = new() { Id = 4651, Name = "Hahan Faithful Protector", WikiUrl = "https://wiki.guildwars.com/wiki/Hahan_Faithful_Protector" };
    public static readonly Npc BoltenLargebelly = new() { Id = 4652, Name = "Bolten Largebelly", WikiUrl = "https://wiki.guildwars.com/wiki/Bolten_Largebelly" };
    public static readonly Npc JarimiyatheUnmerciful = new() { Id = 4653, Name = "Jarimiya the Unmerciful", WikiUrl = "https://wiki.guildwars.com/wiki/Jarimiya_the_Unmerciful" };
    public static readonly Npc ChumabthePrideful = new() { Id = 4654, Name = "Chumab the Prideful", WikiUrl = "https://wiki.guildwars.com/wiki/Chumab_the_Prideful" };
    public static readonly Npc BohdalztheFurious = new() { Id = 4655, Name = "Bohdalz the Furious", WikiUrl = "https://wiki.guildwars.com/wiki/Bohdalz_the_Furious" };
    public static readonly Npc ElderSkreeGriffin = new() { Id = 4656, Name = "Elder Skree Griffin", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Skree_Griffin" };
    public static readonly Npc ElderSkreeRaider = new() { Id = 4657, Name = "Elder Skree Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Skree_Raider" };
    public static readonly Npc ElderSkreeTracker = new() { Id = 4658, Name = "Elder Skree Tracker", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Skree_Tracker" };
    public static readonly Npc ElderSkreeSinger = new() { Id = 4659, Name = "Elder Skree Singer", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Skree_Singer" };
    public static readonly Npc CobaltMokele = new() { Id = 4660, Name = "Cobalt Mokele", WikiUrl = "https://wiki.guildwars.com/wiki/Cobalt_Mokele" };
    public static readonly Npc CobaltShrieker = new() { Id = 4661, Name = "Cobalt Shrieker", WikiUrl = "https://wiki.guildwars.com/wiki/Cobalt_Shrieker" };
    public static readonly Npc CobaltScabara = new() { Id = 4662, Name = "Cobalt Scabara", WikiUrl = "https://wiki.guildwars.com/wiki/Cobalt_Scabara" };
    public static readonly Npc BehemothGravebane = new() { Id = 4663, Name = "Behemoth Gravebane", WikiUrl = "https://wiki.guildwars.com/wiki/Behemoth_Gravebane" };
    public static readonly Npc BehemothGravebane1 = new() { Id = 4664, Name = "Behemoth Gravebane", WikiUrl = "https://wiki.guildwars.com/wiki/Behemoth_Gravebane" };
    public static readonly Npc ScytheclawBehemoth = new() { Id = 4665, Name = "Scytheclaw Behemoth", WikiUrl = "https://wiki.guildwars.com/wiki/Scytheclaw_Behemoth" };
    public static readonly Npc RubyDjinn1 = new() { Id = 4666, Name = "Ruby Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Ruby_Djinn" };
    public static readonly Npc RubyofAhdashim = new() { Id = 4666, Name = "Ruby of Ahdashim", WikiUrl = "https://wiki.guildwars.com/wiki/Ruby_of_Ahdashim" };
    public static readonly Npc DiamondDjinn = new() { Id = 4667, Name = "Diamond Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Diamond_Djinn" };
    public static readonly Npc DiamondofAhdashim = new() { Id = 4667, Name = "Diamond of Ahdashim", WikiUrl = "https://wiki.guildwars.com/wiki/Diamond_of_Ahdashim" };
    public static readonly Npc SapphireDjinn1 = new() { Id = 4669, Name = "Sapphire Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Sapphire_Djinn" };
    public static readonly Npc SapphireofAhdashim = new() { Id = 4669, Name = "Sapphire of Ahdashim", WikiUrl = "https://wiki.guildwars.com/wiki/Sapphire_of_Ahdashim" };
    public static readonly Npc SapphireDjinn2 = new() { Id = 4670, Name = "Sapphire Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Sapphire_Djinn" };
    public static readonly Npc RoaringEther = new() { Id = 4671, Name = "Roaring Ether", WikiUrl = "https://wiki.guildwars.com/wiki/Roaring_Ether" };
    public static readonly Npc RoaringEther1 = new() { Id = 4672, Name = "Roaring Ether", WikiUrl = "https://wiki.guildwars.com/wiki/Roaring_Ether" };
    public static readonly Npc SkreeGriffin = new() { Id = 4673, Name = "Skree Griffin", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Griffin" };
    public static readonly Npc SkreeRaider = new() { Id = 4674, Name = "Skree Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Raider" };
    public static readonly Npc SkreeTracker = new() { Id = 4675, Name = "Skree Tracker", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Tracker" };
    public static readonly Npc SkreeSinger = new() { Id = 4676, Name = "Skree Singer", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Singer" };
    public static readonly Npc RainBeetle = new() { Id = 4681, Name = "Rain Beetle", WikiUrl = "https://wiki.guildwars.com/wiki/Rain_Beetle" };
    public static readonly Npc RainBeetle1 = new() { Id = 4682, Name = "Rain Beetle", WikiUrl = "https://wiki.guildwars.com/wiki/Rain_Beetle" };
    public static readonly Npc RockBeetle = new() { Id = 4683, Name = "Rock Beetle", WikiUrl = "https://wiki.guildwars.com/wiki/Rock_Beetle" };
    public static readonly Npc BullTrainerGiant = new() { Id = 4684, Name = "Bull Trainer Giant", WikiUrl = "https://wiki.guildwars.com/wiki/Bull_Trainer_Giant" };
    public static readonly Npc HuntingMinotaur = new() { Id = 4685, Name = "Hunting Minotaur", WikiUrl = "https://wiki.guildwars.com/wiki/Hunting_Minotaur" };
    public static readonly Npc MirageIboga = new() { Id = 4686, Name = "Mirage Iboga", WikiUrl = "https://wiki.guildwars.com/wiki/Mirage_Iboga" };
    public static readonly Npc StormJacaranda = new() { Id = 4687, Name = "Storm Jacaranda", WikiUrl = "https://wiki.guildwars.com/wiki/Storm_Jacaranda" };
    public static readonly Npc EnchantedBrambles = new() { Id = 4688, Name = "Enchanted Brambles", WikiUrl = "https://wiki.guildwars.com/wiki/Enchanted_Brambles" };
    public static readonly Npc WhistlingThornbrush = new() { Id = 4689, Name = "Whistling Thornbrush", WikiUrl = "https://wiki.guildwars.com/wiki/Whistling_Thornbrush" };
    public static readonly Npc BloodCowlHeket1 = new() { Id = 4690, Name = "Blood Cowl Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_Cowl_Heket" };
    public static readonly Npc BlueTongueHeket1 = new() { Id = 4691, Name = "Blue Tongue Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Blue_Tongue_Heket" };
    public static readonly Npc StoneaxeHeket1 = new() { Id = 4692, Name = "Stoneaxe Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Stoneaxe_Heket" };
    public static readonly Npc BeastSwornHeket1 = new() { Id = 4693, Name = "Beast Sworn Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Beast_Sworn_Heket" };
    public static readonly Npc Chormar = new() { Id = 4694, Name = "Chormar", WikiUrl = "https://wiki.guildwars.com/wiki/Chormar" };
    public static readonly Npc Danton = new() { Id = 4694, Name = "Danton", WikiUrl = "https://wiki.guildwars.com/wiki/Danton" };
    public static readonly Npc Elonbel = new() { Id = 4694, Name = "Elonbel", WikiUrl = "https://wiki.guildwars.com/wiki/Elonbel" };
    public static readonly Npc Fahrik = new() { Id = 4694, Name = "Fahrik", WikiUrl = "https://wiki.guildwars.com/wiki/Fahrik" };
    public static readonly Npc GuardKovu = new() { Id = 4694, Name = "Guard Kovu", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Kovu" };
    public static readonly Npc GuardsmanGafai = new() { Id = 4694, Name = "Guardsman Gafai", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Gafai" };
    public static readonly Npc GuardsmanGahrik = new() { Id = 4694, Name = "Guardsman Gahrik", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Gahrik" };
    public static readonly Npc GuardsmanOnageh = new() { Id = 4694, Name = "Guardsman Onageh", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Onageh" };
    public static readonly Npc Gundok = new() { Id = 4694, Name = "Gundok", WikiUrl = "https://wiki.guildwars.com/wiki/Gundok" };
    public static readonly Npc Hoanjo = new() { Id = 4694, Name = "Hoanjo", WikiUrl = "https://wiki.guildwars.com/wiki/Hoanjo" };
    public static readonly Npc Jibehr = new() { Id = 4694, Name = "Jibehr", WikiUrl = "https://wiki.guildwars.com/wiki/Jibehr" };
    public static readonly Npc Lohfihau = new() { Id = 4694, Name = "Lohfihau", WikiUrl = "https://wiki.guildwars.com/wiki/Lohfihau" };
    public static readonly Npc Lunmor = new() { Id = 4694, Name = "Lunmor", WikiUrl = "https://wiki.guildwars.com/wiki/Lunmor" };
    public static readonly Npc Nermak = new() { Id = 4694, Name = "Nermak", WikiUrl = "https://wiki.guildwars.com/wiki/Nermak" };
    public static readonly Npc Sedai = new() { Id = 4694, Name = "Sedai", WikiUrl = "https://wiki.guildwars.com/wiki/Sedai" };
    public static readonly Npc Shaudok = new() { Id = 4694, Name = "Shaudok", WikiUrl = "https://wiki.guildwars.com/wiki/Shaudok" };
    public static readonly Npc ShoreWatcher = new() { Id = 4694, Name = "Shore Watcher", WikiUrl = "https://wiki.guildwars.com/wiki/Shore_Watcher" };
    public static readonly Npc Shorewatcher = new() { Id = 4694, Name = "Shorewatcher", WikiUrl = "https://wiki.guildwars.com/wiki/Shorewatcher" };
    public static readonly Npc Sunspear = new() { Id = 4694, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Tohsedi = new() { Id = 4694, Name = "Tohsedi", WikiUrl = "https://wiki.guildwars.com/wiki/Tohsedi" };
    public static readonly Npc Yamamank = new() { Id = 4694, Name = "Yamamank", WikiUrl = "https://wiki.guildwars.com/wiki/Yamamank" };
    public static readonly Npc Ahbri = new() { Id = 4696, Name = "Ahbri", WikiUrl = "https://wiki.guildwars.com/wiki/Ahbri" };
    public static readonly Npc Behchu = new() { Id = 4696, Name = "Behchu", WikiUrl = "https://wiki.guildwars.com/wiki/Behchu" };
    public static readonly Npc CaptainAhkenchu = new() { Id = 4696, Name = "Captain Ahkenchu", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Ahkenchu" };
    public static readonly Npc CastellanPuuba = new() { Id = 4696, Name = "Castellan Puuba", WikiUrl = "https://wiki.guildwars.com/wiki/Castellan_Puuba" };
    public static readonly Npc FirstSpearDehvad = new() { Id = 4696, Name = "First Spear Dehvad", WikiUrl = "https://wiki.guildwars.com/wiki/First_Spear_Dehvad" };
    public static readonly Npc FirstSpearJahdugar = new() { Id = 4696, Name = "First Spear Jahdugar", WikiUrl = "https://wiki.guildwars.com/wiki/First_Spear_Jahdugar" };
    public static readonly Npc Kohanu = new() { Id = 4696, Name = "Kohanu", WikiUrl = "https://wiki.guildwars.com/wiki/Kohanu" };
    public static readonly Npc Pikin = new() { Id = 4696, Name = "Pikin", WikiUrl = "https://wiki.guildwars.com/wiki/Pikin" };
    public static readonly Npc Ronkhet = new() { Id = 4696, Name = "Ronkhet", WikiUrl = "https://wiki.guildwars.com/wiki/Ronkhet" };
    public static readonly Npc RundukRank = new() { Id = 4696, Name = "Runduk Rank", WikiUrl = "https://wiki.guildwars.com/wiki/Runduk_Rank" };
    public static readonly Npc Shiloh = new() { Id = 4696, Name = "Shiloh", WikiUrl = "https://wiki.guildwars.com/wiki/Shiloh" };
    public static readonly Npc Tahristahn = new() { Id = 4696, Name = "Tahristahn", WikiUrl = "https://wiki.guildwars.com/wiki/Tahristahn" };
    public static readonly Npc Tohn = new() { Id = 4696, Name = "Tohn", WikiUrl = "https://wiki.guildwars.com/wiki/Tohn" };
    public static readonly Npc Shaurom = new() { Id = 4697, Name = "Shaurom", WikiUrl = "https://wiki.guildwars.com/wiki/Shaurom" };
    public static readonly Npc Dzajo = new() { Id = 4698, Name = "Dzajo", WikiUrl = "https://wiki.guildwars.com/wiki/Dzajo" };
    public static readonly Npc Ehiyah = new() { Id = 4698, Name = "Ehiyah", WikiUrl = "https://wiki.guildwars.com/wiki/Ehiyah" };
    public static readonly Npc Nundho = new() { Id = 4698, Name = "Nundho", WikiUrl = "https://wiki.guildwars.com/wiki/Nundho" };
    public static readonly Npc CorpseofShoreWatcher = new() { Id = 4700, Name = "Corpse of Shore Watcher", WikiUrl = "https://wiki.guildwars.com/wiki/Corpse_of_Shore_Watcher" };
    public static readonly Npc ShoreWatcher1 = new() { Id = 4700, Name = "Shore Watcher", WikiUrl = "https://wiki.guildwars.com/wiki/Shore_Watcher" };
    public static readonly Npc Bendah = new() { Id = 4701, Name = "Bendah", WikiUrl = "https://wiki.guildwars.com/wiki/Bendah" };
    public static readonly Npc SecondSpearBinah = new() { Id = 4701, Name = "Second Spear Binah", WikiUrl = "https://wiki.guildwars.com/wiki/Second_Spear_Binah" };
    public static readonly Npc Shorewatcher1 = new() { Id = 4701, Name = "Shorewatcher", WikiUrl = "https://wiki.guildwars.com/wiki/Shorewatcher" };
    public static readonly Npc Sunspear1 = new() { Id = 4701, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Ashigun = new() { Id = 4703, Name = "Ashigun", WikiUrl = "https://wiki.guildwars.com/wiki/Ashigun" };
    public static readonly Npc Dalzbeh = new() { Id = 4703, Name = "Dalzbeh", WikiUrl = "https://wiki.guildwars.com/wiki/Dalzbeh" };
    public static readonly Npc Hlengiwe = new() { Id = 4703, Name = "Hlengiwe", WikiUrl = "https://wiki.guildwars.com/wiki/Hlengiwe" };
    public static readonly Npc MahksSon = new() { Id = 4703, Name = "Mahk's Son", WikiUrl = "https://wiki.guildwars.com/wiki/Mahk's_Son" };
    public static readonly Npc Mau = new() { Id = 4703, Name = "Mau", WikiUrl = "https://wiki.guildwars.com/wiki/Mau" };
    public static readonly Npc Mauban = new() { Id = 4703, Name = "Mauban", WikiUrl = "https://wiki.guildwars.com/wiki/Mauban" };
    public static readonly Npc Nehduvad = new() { Id = 4703, Name = "Nehduvad", WikiUrl = "https://wiki.guildwars.com/wiki/Nehduvad" };
    public static readonly Npc Paldinam = new() { Id = 4703, Name = "Paldinam", WikiUrl = "https://wiki.guildwars.com/wiki/Paldinam" };
    public static readonly Npc Pobehr = new() { Id = 4703, Name = "Pobehr", WikiUrl = "https://wiki.guildwars.com/wiki/Pobehr" };
    public static readonly Npc Yahyakun = new() { Id = 4703, Name = "Yahyakun", WikiUrl = "https://wiki.guildwars.com/wiki/Yahyakun" };
    public static readonly Npc YoungChild = new() { Id = 4703, Name = "Young Child", WikiUrl = "https://wiki.guildwars.com/wiki/Young_Child" };
    public static readonly Npc Adina = new() { Id = 4704, Name = "Adina", WikiUrl = "https://wiki.guildwars.com/wiki/Adina" };
    public static readonly Npc Chitundu = new() { Id = 4704, Name = "Chitundu", WikiUrl = "https://wiki.guildwars.com/wiki/Chitundu" };
    public static readonly Npc Poti = new() { Id = 4704, Name = "Poti", WikiUrl = "https://wiki.guildwars.com/wiki/Poti" };
    public static readonly Npc YoungChild1 = new() { Id = 4704, Name = "Young Child", WikiUrl = "https://wiki.guildwars.com/wiki/Young_Child" };
    public static readonly Npc Adhitok = new() { Id = 4705, Name = "Adhitok", WikiUrl = "https://wiki.guildwars.com/wiki/Adhitok" };
    public static readonly Npc Architect = new() { Id = 4705, Name = "Architect", WikiUrl = "https://wiki.guildwars.com/wiki/Architect" };
    public static readonly Npc ChefVolon = new() { Id = 4705, Name = "Chef Volon", WikiUrl = "https://wiki.guildwars.com/wiki/Chef_Volon" };
    public static readonly Npc ClerkArlon = new() { Id = 4705, Name = "Clerk Arlon", WikiUrl = "https://wiki.guildwars.com/wiki/Clerk_Arlon" };
    public static readonly Npc DockmasterAhlaro = new() { Id = 4705, Name = "Dockmaster Ahlaro", WikiUrl = "https://wiki.guildwars.com/wiki/Dockmaster_Ahlaro" };
    public static readonly Npc ElderBelin = new() { Id = 4705, Name = "Elder Belin", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Belin" };
    public static readonly Npc HealerZenwa = new() { Id = 4705, Name = "Healer Zenwa", WikiUrl = "https://wiki.guildwars.com/wiki/Healer_Zenwa" };
    public static readonly Npc HistorianLaharo = new() { Id = 4705, Name = "Historian Laharo", WikiUrl = "https://wiki.guildwars.com/wiki/Historian_Laharo" };
    public static readonly Npc Horat = new() { Id = 4705, Name = "Horat", WikiUrl = "https://wiki.guildwars.com/wiki/Horat" };
    public static readonly Npc IstaniNoble = new() { Id = 4705, Name = "Istani Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Noble" };
    public static readonly Npc Iyemab = new() { Id = 4705, Name = "Iyemab", WikiUrl = "https://wiki.guildwars.com/wiki/Iyemab" };
    public static readonly Npc Jidun = new() { Id = 4705, Name = "Jidun", WikiUrl = "https://wiki.guildwars.com/wiki/Jidun" };
    public static readonly Npc Kehtu = new() { Id = 4705, Name = "Kehtu", WikiUrl = "https://wiki.guildwars.com/wiki/Kehtu" };
    public static readonly Npc Mofuun = new() { Id = 4705, Name = "Mofuun", WikiUrl = "https://wiki.guildwars.com/wiki/Mofuun" };
    public static readonly Npc RolandleMoisson = new() { Id = 4705, Name = "Roland le Moisson", WikiUrl = "https://wiki.guildwars.com/wiki/Roland_le_Moisson" };
    public static readonly Npc ScholarChago = new() { Id = 4705, Name = "Scholar Chago", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Chago" };
    public static readonly Npc ScholarKayanu = new() { Id = 4705, Name = "Scholar Kayanu", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Kayanu" };
    public static readonly Npc TacticianHaj = new() { Id = 4705, Name = "Tactician Haj", WikiUrl = "https://wiki.guildwars.com/wiki/Tactician_Haj" };
    public static readonly Npc VabbiTradeOfficial = new() { Id = 4705, Name = "Vabbi Trade Official", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Trade_Official" };
    public static readonly Npc Gamyuka = new() { Id = 4706, Name = "Gamyuka", WikiUrl = "https://wiki.guildwars.com/wiki/Gamyuka" };
    public static readonly Npc IstaniNoble1 = new() { Id = 4706, Name = "Istani Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Noble" };
    public static readonly Npc Kormir1 = new() { Id = 4706, Name = "Kormir", WikiUrl = "https://wiki.guildwars.com/wiki/Kormir" };
    public static readonly Npc Midauha = new() { Id = 4706, Name = "Midauha", WikiUrl = "https://wiki.guildwars.com/wiki/Midauha" };
    public static readonly Npc Paljab = new() { Id = 4706, Name = "Paljab", WikiUrl = "https://wiki.guildwars.com/wiki/Paljab" };
    public static readonly Npc QuarrymasterBohanna = new() { Id = 4706, Name = "Quarrymaster Bohanna", WikiUrl = "https://wiki.guildwars.com/wiki/Quarrymaster_Bohanna" };
    public static readonly Npc SkyScholar = new() { Id = 4706, Name = "Sky Scholar", WikiUrl = "https://wiki.guildwars.com/wiki/Sky_Scholar" };
    public static readonly Npc Sushah = new() { Id = 4706, Name = "Sushah", WikiUrl = "https://wiki.guildwars.com/wiki/Sushah" };
    public static readonly Npc Timahr = new() { Id = 4706, Name = "Timahr", WikiUrl = "https://wiki.guildwars.com/wiki/Timahr" };
    public static readonly Npc Tugani = new() { Id = 4706, Name = "Tugani", WikiUrl = "https://wiki.guildwars.com/wiki/Tugani" };
    public static readonly Npc VabbiTradeOfficial1 = new() { Id = 4706, Name = "Vabbi Trade Official", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Trade_Official" };
    public static readonly Npc Zukesha = new() { Id = 4706, Name = "Zukesha", WikiUrl = "https://wiki.guildwars.com/wiki/Zukesha" };
    public static readonly Npc AilonsehDejarin = new() { Id = 4707, Name = "Ailonseh Dejarin", WikiUrl = "https://wiki.guildwars.com/wiki/Ailonseh_Dejarin" };
    public static readonly Npc ChefPanjoh = new() { Id = 4708, Name = "Chef Panjoh", WikiUrl = "https://wiki.guildwars.com/wiki/Chef_Panjoh" };
    public static readonly Npc Dajwa = new() { Id = 4708, Name = "Dajwa", WikiUrl = "https://wiki.guildwars.com/wiki/Dajwa" };
    public static readonly Npc DigmasterGatah = new() { Id = 4708, Name = "Digmaster Gatah", WikiUrl = "https://wiki.guildwars.com/wiki/Digmaster_Gatah" };
    public static readonly Npc Dungrud = new() { Id = 4708, Name = "Dungrud", WikiUrl = "https://wiki.guildwars.com/wiki/Dungrud" };
    public static readonly Npc ElderJurdu = new() { Id = 4708, Name = "Elder Jurdu", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Jurdu" };
    public static readonly Npc Elonyam = new() { Id = 4708, Name = "Elonyam", WikiUrl = "https://wiki.guildwars.com/wiki/Elonyam" };
    public static readonly Npc FarmerMuenda = new() { Id = 4708, Name = "Farmer Muenda", WikiUrl = "https://wiki.guildwars.com/wiki/Farmer_Muenda" };
    public static readonly Npc Hamar = new() { Id = 4708, Name = "Hamar", WikiUrl = "https://wiki.guildwars.com/wiki/Hamar" };
    public static readonly Npc Hatuk = new() { Id = 4708, Name = "Hatuk", WikiUrl = "https://wiki.guildwars.com/wiki/Hatuk" };
    public static readonly Npc IstaniCommoner = new() { Id = 4708, Name = "Istani Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Commoner" };
    public static readonly Npc IstaniMiner = new() { Id = 4708, Name = "Istani Miner", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Miner" };
    public static readonly Npc IstaniPeasant = new() { Id = 4708, Name = "Istani Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Peasant" };
    public static readonly Npc Kanyama = new() { Id = 4708, Name = "Kanyama", WikiUrl = "https://wiki.guildwars.com/wiki/Kanyama" };
    public static readonly Npc LendutheFisherman = new() { Id = 4708, Name = "Lendu the Fisherman", WikiUrl = "https://wiki.guildwars.com/wiki/Lendu_the_Fisherman" };
    public static readonly Npc Mahk = new() { Id = 4708, Name = "Mahk", WikiUrl = "https://wiki.guildwars.com/wiki/Mahk" };
    public static readonly Npc Mahzu = new() { Id = 4708, Name = "Mahzu", WikiUrl = "https://wiki.guildwars.com/wiki/Mahzu" };
    public static readonly Npc Morik = new() { Id = 4708, Name = "Morik", WikiUrl = "https://wiki.guildwars.com/wiki/Morik" };
    public static readonly Npc Mufinbo = new() { Id = 4708, Name = "Mufinbo", WikiUrl = "https://wiki.guildwars.com/wiki/Mufinbo" };
    public static readonly Npc Munashe = new() { Id = 4708, Name = "Munashe", WikiUrl = "https://wiki.guildwars.com/wiki/Munashe" };
    public static readonly Npc Pentehlez = new() { Id = 4708, Name = "Pentehlez", WikiUrl = "https://wiki.guildwars.com/wiki/Pentehlez" };
    public static readonly Npc Poturi = new() { Id = 4708, Name = "Poturi", WikiUrl = "https://wiki.guildwars.com/wiki/Poturi" };
    public static readonly Npc Rohtu = new() { Id = 4708, Name = "Rohtu", WikiUrl = "https://wiki.guildwars.com/wiki/Rohtu" };
    public static readonly Npc Uhisheh = new() { Id = 4708, Name = "Uhisheh", WikiUrl = "https://wiki.guildwars.com/wiki/Uhisheh" };
    public static readonly Npc Vadben = new() { Id = 4708, Name = "Vadben", WikiUrl = "https://wiki.guildwars.com/wiki/Vadben" };
    public static readonly Npc WeaverTamapi = new() { Id = 4708, Name = "Weaver Tamapi", WikiUrl = "https://wiki.guildwars.com/wiki/Weaver_Tamapi" };
    public static readonly Npc WoundedVillager = new() { Id = 4708, Name = "Wounded Villager", WikiUrl = "https://wiki.guildwars.com/wiki/Wounded_Villager" };
    public static readonly Npc Yajide = new() { Id = 4708, Name = "Yajide", WikiUrl = "https://wiki.guildwars.com/wiki/Yajide" };
    public static readonly Npc Ajambo = new() { Id = 4709, Name = "Ajambo", WikiUrl = "https://wiki.guildwars.com/wiki/Ajambo" };
    public static readonly Npc Ando = new() { Id = 4709, Name = "Ando", WikiUrl = "https://wiki.guildwars.com/wiki/Ando" };
    public static readonly Npc Apida = new() { Id = 4709, Name = "Apida", WikiUrl = "https://wiki.guildwars.com/wiki/Apida" };
    public static readonly Npc Architect1 = new() { Id = 4709, Name = "Architect", WikiUrl = "https://wiki.guildwars.com/wiki/Architect" };
    public static readonly Npc BeastmasterYapono = new() { Id = 4709, Name = "Beastmaster Yapono", WikiUrl = "https://wiki.guildwars.com/wiki/Beastmaster_Yapono" };
    public static readonly Npc Behrdos = new() { Id = 4709, Name = "Behrdos", WikiUrl = "https://wiki.guildwars.com/wiki/Behrdos" };
    public static readonly Npc IstaniCommoner1 = new() { Id = 4709, Name = "Istani Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Commoner" };
    public static readonly Npc MasterEngineerJakumba = new() { Id = 4709, Name = "Master Engineer Jakumba", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Engineer_Jakumba" };
    public static readonly Npc Mirmahk = new() { Id = 4709, Name = "Mirmahk", WikiUrl = "https://wiki.guildwars.com/wiki/Mirmahk" };
    public static readonly Npc NecromancerMusseh = new() { Id = 4709, Name = "Necromancer Musseh", WikiUrl = "https://wiki.guildwars.com/wiki/Necromancer_Musseh" };
    public static readonly Npc Rahb = new() { Id = 4709, Name = "Rahb", WikiUrl = "https://wiki.guildwars.com/wiki/Rahb" };
    public static readonly Npc Rohtu1 = new() { Id = 4709, Name = "Rohtu", WikiUrl = "https://wiki.guildwars.com/wiki/Rohtu" };
    public static readonly Npc StonecutterGed = new() { Id = 4709, Name = "Stonecutter Ged", WikiUrl = "https://wiki.guildwars.com/wiki/Stonecutter_Ged" };
    public static readonly Npc SuspiciousHermit = new() { Id = 4709, Name = "Suspicious Hermit", WikiUrl = "https://wiki.guildwars.com/wiki/Suspicious_Hermit" };
    public static readonly Npc CorpseofIstaniPeasant = new() { Id = 4710, Name = "Corpse of Istani Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Corpse_of_Istani_Peasant" };
    public static readonly Npc ChefLonbahn = new() { Id = 4711, Name = "Chef Lonbahn", WikiUrl = "https://wiki.guildwars.com/wiki/Chef_Lonbahn" };
    public static readonly Npc Dahni = new() { Id = 4711, Name = "Dahni", WikiUrl = "https://wiki.guildwars.com/wiki/Dahni" };
    public static readonly Npc IstaniCommoner2 = new() { Id = 4711, Name = "Istani Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Commoner" };
    public static readonly Npc Kailona = new() { Id = 4711, Name = "Kailona", WikiUrl = "https://wiki.guildwars.com/wiki/Kailona" };
    public static readonly Npc Kihawa = new() { Id = 4711, Name = "Kihawa", WikiUrl = "https://wiki.guildwars.com/wiki/Kihawa" };
    public static readonly Npc MahksWife = new() { Id = 4711, Name = "Mahk's Wife", WikiUrl = "https://wiki.guildwars.com/wiki/Mahk's_Wife" };
    public static readonly Npc Motur = new() { Id = 4711, Name = "Motur", WikiUrl = "https://wiki.guildwars.com/wiki/Motur" };
    public static readonly Npc MourningVillager = new() { Id = 4711, Name = "Mourning Villager", WikiUrl = "https://wiki.guildwars.com/wiki/Mourning_Villager" };
    public static readonly Npc Nenah = new() { Id = 4711, Name = "Nenah", WikiUrl = "https://wiki.guildwars.com/wiki/Nenah" };
    public static readonly Npc Nertu = new() { Id = 4711, Name = "Nertu", WikiUrl = "https://wiki.guildwars.com/wiki/Nertu" };
    public static readonly Npc ObserverJahfoh = new() { Id = 4711, Name = "Observer Jahfoh", WikiUrl = "https://wiki.guildwars.com/wiki/Observer_Jahfoh" };
    public static readonly Npc Pehrub = new() { Id = 4711, Name = "Pehrub", WikiUrl = "https://wiki.guildwars.com/wiki/Pehrub" };
    public static readonly Npc Pelei = new() { Id = 4711, Name = "Pelei", WikiUrl = "https://wiki.guildwars.com/wiki/Pelei" };
    public static readonly Npc Sehti = new() { Id = 4711, Name = "Sehti", WikiUrl = "https://wiki.guildwars.com/wiki/Sehti" };
    public static readonly Npc Sehyal = new() { Id = 4711, Name = "Sehyal", WikiUrl = "https://wiki.guildwars.com/wiki/Sehyal" };
    public static readonly Npc Shakashi = new() { Id = 4711, Name = "Shakashi", WikiUrl = "https://wiki.guildwars.com/wiki/Shakashi" };
    public static readonly Npc Sholmara = new() { Id = 4711, Name = "Sholmara", WikiUrl = "https://wiki.guildwars.com/wiki/Sholmara" };
    public static readonly Npc Sinni = new() { Id = 4711, Name = "Sinni", WikiUrl = "https://wiki.guildwars.com/wiki/Sinni" };
    public static readonly Npc Talmehinu = new() { Id = 4711, Name = "Talmehinu", WikiUrl = "https://wiki.guildwars.com/wiki/Talmehinu" };
    public static readonly Npc WoundedVillager1 = new() { Id = 4711, Name = "Wounded Villager", WikiUrl = "https://wiki.guildwars.com/wiki/Wounded_Villager" };
    public static readonly Npc Yahya = new() { Id = 4711, Name = "Yahya", WikiUrl = "https://wiki.guildwars.com/wiki/Yahya" };
    public static readonly Npc Bahlya = new() { Id = 4712, Name = "Bahlya", WikiUrl = "https://wiki.guildwars.com/wiki/Bahlya" };
    public static readonly Npc Dajwa1 = new() { Id = 4714, Name = "Dajwa", WikiUrl = "https://wiki.guildwars.com/wiki/Dajwa" };
    public static readonly Npc FarmerFoneng = new() { Id = 4714, Name = "Farmer Foneng", WikiUrl = "https://wiki.guildwars.com/wiki/Farmer_Foneng" };
    public static readonly Npc IstaniCommoner3 = new() { Id = 4714, Name = "Istani Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Commoner" };
    public static readonly Npc MinerTonabanza = new() { Id = 4714, Name = "Miner Tonabanza", WikiUrl = "https://wiki.guildwars.com/wiki/Miner_Tonabanza" };
    public static readonly Npc Miresh = new() { Id = 4714, Name = "Miresh", WikiUrl = "https://wiki.guildwars.com/wiki/Miresh" };
    public static readonly Npc Poturi1 = new() { Id = 4714, Name = "Poturi", WikiUrl = "https://wiki.guildwars.com/wiki/Poturi" };
    public static readonly Npc Rahlon = new() { Id = 4714, Name = "Rahlon", WikiUrl = "https://wiki.guildwars.com/wiki/Rahlon" };
    public static readonly Npc Damak = new() { Id = 4715, Name = "Damak", WikiUrl = "https://wiki.guildwars.com/wiki/Damak" };
    public static readonly Npc XunlaiGiftGiverGimmek = new() { Id = 4715, Name = "Xunlai Gift-Giver Gimmek", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Gift-Giver_Gimmek" };
    public static readonly Npc CaptainBolduhr = new() { Id = 4716, Name = "Captain Bolduhr", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Bolduhr" };
    public static readonly Npc Issti = new() { Id = 4716, Name = "Issti", WikiUrl = "https://wiki.guildwars.com/wiki/Issti" };
    public static readonly Npc Matunbo = new() { Id = 4716, Name = "Matunbo", WikiUrl = "https://wiki.guildwars.com/wiki/Matunbo" };
    public static readonly Npc Velig = new() { Id = 4716, Name = "Velig", WikiUrl = "https://wiki.guildwars.com/wiki/Velig" };
    public static readonly Npc Jakatu = new() { Id = 4717, Name = "Jakatu", WikiUrl = "https://wiki.guildwars.com/wiki/Jakatu" };
    public static readonly Npc Kehtokweh = new() { Id = 4718, Name = "Kehtokweh", WikiUrl = "https://wiki.guildwars.com/wiki/Kehtokweh" };
    public static readonly Npc Pasu = new() { Id = 4718, Name = "Pasu", WikiUrl = "https://wiki.guildwars.com/wiki/Pasu" };
    public static readonly Npc Vatundo = new() { Id = 4718, Name = "Vatundo", WikiUrl = "https://wiki.guildwars.com/wiki/Vatundo" };
    public static readonly Npc Mehinu = new() { Id = 4719, Name = "Mehinu", WikiUrl = "https://wiki.guildwars.com/wiki/Mehinu" };
    public static readonly Npc Sulee = new() { Id = 4719, Name = "Sulee", WikiUrl = "https://wiki.guildwars.com/wiki/Sulee" };
    public static readonly Npc Arissi = new() { Id = 4720, Name = "Arissi", WikiUrl = "https://wiki.guildwars.com/wiki/Arissi" };
    public static readonly Npc Habab = new() { Id = 4720, Name = "Habab", WikiUrl = "https://wiki.guildwars.com/wiki/Habab" };
    public static readonly Npc Jasuh = new() { Id = 4720, Name = "Jasuh", WikiUrl = "https://wiki.guildwars.com/wiki/Jasuh" };
    public static readonly Npc Kargah = new() { Id = 4720, Name = "Kargah", WikiUrl = "https://wiki.guildwars.com/wiki/Kargah" };
    public static readonly Npc Kupekanu = new() { Id = 4720, Name = "Kupekanu", WikiUrl = "https://wiki.guildwars.com/wiki/Kupekanu" };
    public static readonly Npc Lobutu = new() { Id = 4720, Name = "Lobutu", WikiUrl = "https://wiki.guildwars.com/wiki/Lobutu" };
    public static readonly Npc Lokai = new() { Id = 4720, Name = "Lokai", WikiUrl = "https://wiki.guildwars.com/wiki/Lokai" };
    public static readonly Npc Lomar = new() { Id = 4720, Name = "Lomar", WikiUrl = "https://wiki.guildwars.com/wiki/Lomar" };
    public static readonly Npc Matuu = new() { Id = 4720, Name = "Matuu", WikiUrl = "https://wiki.guildwars.com/wiki/Matuu" };
    public static readonly Npc Shatam = new() { Id = 4720, Name = "Shatam", WikiUrl = "https://wiki.guildwars.com/wiki/Shatam" };
    public static readonly Npc VabbianCommoner = new() { Id = 4720, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc Alekaya = new() { Id = 4721, Name = "Alekaya", WikiUrl = "https://wiki.guildwars.com/wiki/Alekaya" };
    public static readonly Npc Answa = new() { Id = 4721, Name = "Answa", WikiUrl = "https://wiki.guildwars.com/wiki/Answa" };
    public static readonly Npc Tesserai = new() { Id = 4721, Name = "Tesserai", WikiUrl = "https://wiki.guildwars.com/wiki/Tesserai" };
    public static readonly Npc VabbianCommoner1 = new() { Id = 4721, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc Yedah = new() { Id = 4721, Name = "Yedah", WikiUrl = "https://wiki.guildwars.com/wiki/Yedah" };
    public static readonly Npc Chorben = new() { Id = 4722, Name = "Chorben", WikiUrl = "https://wiki.guildwars.com/wiki/Chorben" };
    public static readonly Npc Dahleht = new() { Id = 4722, Name = "Dahleht", WikiUrl = "https://wiki.guildwars.com/wiki/Dahleht" };
    public static readonly Npc Farzi = new() { Id = 4722, Name = "Farzi", WikiUrl = "https://wiki.guildwars.com/wiki/Farzi" };
    public static readonly Npc Fahri = new() { Id = 4723, Name = "Fahri", WikiUrl = "https://wiki.guildwars.com/wiki/Fahri" };
    public static readonly Npc Saara = new() { Id = 4723, Name = "Saara", WikiUrl = "https://wiki.guildwars.com/wiki/Saara" };
    public static readonly Npc Barlom = new() { Id = 4725, Name = "Barlom", WikiUrl = "https://wiki.guildwars.com/wiki/Barlom" };
    public static readonly Npc Guul = new() { Id = 4726, Name = "Guul", WikiUrl = "https://wiki.guildwars.com/wiki/Guul" };
    public static readonly Npc Kahlim = new() { Id = 4726, Name = "Kahlim", WikiUrl = "https://wiki.guildwars.com/wiki/Kahlim" };
    public static readonly Npc Nasera = new() { Id = 4727, Name = "Nasera", WikiUrl = "https://wiki.guildwars.com/wiki/Nasera" };
    public static readonly Npc DashehMaterial = new() { Id = 4728, Name = "Dasheh Material", WikiUrl = "https://wiki.guildwars.com/wiki/Dasheh_Material" };
    public static readonly Npc DuujaMaterial = new() { Id = 4729, Name = "Duuja Material", WikiUrl = "https://wiki.guildwars.com/wiki/Duuja_Material" };
    public static readonly Npc NehgoyoMaterial = new() { Id = 4729, Name = "Nehgoyo Material", WikiUrl = "https://wiki.guildwars.com/wiki/Nehgoyo_Material" };
    public static readonly Npc Goldai = new() { Id = 4732, Name = "Goldai", WikiUrl = "https://wiki.guildwars.com/wiki/Goldai" };
    public static readonly Npc Nela = new() { Id = 4733, Name = "Nela", WikiUrl = "https://wiki.guildwars.com/wiki/Nela" };
    public static readonly Npc Kenohir = new() { Id = 4734, Name = "Kenohir", WikiUrl = "https://wiki.guildwars.com/wiki/Kenohir" };
    public static readonly Npc Kenohir1 = new() { Id = 4734, Name = "Kenohir", WikiUrl = "https://wiki.guildwars.com/wiki/Kenohir" };
    public static readonly Npc Zuwarah = new() { Id = 4735, Name = "Zuwarah", WikiUrl = "https://wiki.guildwars.com/wiki/Zuwarah" };
    public static readonly Npc Zuwarah1 = new() { Id = 4735, Name = "Zuwarah", WikiUrl = "https://wiki.guildwars.com/wiki/Zuwarah" };
    public static readonly Npc Shausha = new() { Id = 4736, Name = "Shausha", WikiUrl = "https://wiki.guildwars.com/wiki/Shausha" };
    public static readonly Npc Shausha1 = new() { Id = 4736, Name = "Shausha", WikiUrl = "https://wiki.guildwars.com/wiki/Shausha" };
    public static readonly Npc Ahamid = new() { Id = 4737, Name = "Ahamid", WikiUrl = "https://wiki.guildwars.com/wiki/Ahamid" };
    public static readonly Npc Ahamid1 = new() { Id = 4737, Name = "Ahamid", WikiUrl = "https://wiki.guildwars.com/wiki/Ahamid" };
    public static readonly Npc Dalzid = new() { Id = 4738, Name = "Dalzid", WikiUrl = "https://wiki.guildwars.com/wiki/Dalzid" };
    public static readonly Npc Marlani = new() { Id = 4739, Name = "Marlani", WikiUrl = "https://wiki.guildwars.com/wiki/Marlani" };
    public static readonly Npc Suti = new() { Id = 4740, Name = "Suti", WikiUrl = "https://wiki.guildwars.com/wiki/Suti" };
    public static readonly Npc Bomahz = new() { Id = 4741, Name = "Bomahz", WikiUrl = "https://wiki.guildwars.com/wiki/Bomahz" };
    public static readonly Npc Radhiya = new() { Id = 4742, Name = "Radhiya", WikiUrl = "https://wiki.guildwars.com/wiki/Radhiya" };
    public static readonly Npc Ekundayo = new() { Id = 4743, Name = "Ekundayo", WikiUrl = "https://wiki.guildwars.com/wiki/Ekundayo" };
    public static readonly Npc Rafiki = new() { Id = 4744, Name = "Rafiki", WikiUrl = "https://wiki.guildwars.com/wiki/Rafiki" };
    public static readonly Npc Selah = new() { Id = 4745, Name = "Selah", WikiUrl = "https://wiki.guildwars.com/wiki/Selah" };
    public static readonly Npc Tahembi = new() { Id = 4746, Name = "Tahembi", WikiUrl = "https://wiki.guildwars.com/wiki/Tahembi" };
    public static readonly Npc Nalah = new() { Id = 4747, Name = "Nalah", WikiUrl = "https://wiki.guildwars.com/wiki/Nalah" };
    public static readonly Npc Kadirah = new() { Id = 4748, Name = "Kadirah", WikiUrl = "https://wiki.guildwars.com/wiki/Kadirah" };
    public static readonly Npc Sekou = new() { Id = 4749, Name = "Sekou", WikiUrl = "https://wiki.guildwars.com/wiki/Sekou" };
    public static readonly Npc Faraji = new() { Id = 4750, Name = "Faraji", WikiUrl = "https://wiki.guildwars.com/wiki/Faraji" };
    public static readonly Npc Safiya = new() { Id = 4751, Name = "Safiya", WikiUrl = "https://wiki.guildwars.com/wiki/Safiya" };
    public static readonly Npc Wanjala = new() { Id = 4752, Name = "Wanjala", WikiUrl = "https://wiki.guildwars.com/wiki/Wanjala" };
    public static readonly Npc Nahjiri = new() { Id = 4753, Name = "Nahjiri", WikiUrl = "https://wiki.guildwars.com/wiki/Nahjiri" };
    public static readonly Npc Bomani = new() { Id = 4754, Name = "Bomani", WikiUrl = "https://wiki.guildwars.com/wiki/Bomani" };
    public static readonly Npc Dume = new() { Id = 4755, Name = "Dume", WikiUrl = "https://wiki.guildwars.com/wiki/Dume" };
    public static readonly Npc Sodan = new() { Id = 4756, Name = "Sodan", WikiUrl = "https://wiki.guildwars.com/wiki/Sodan" };
    public static readonly Npc Mara = new() { Id = 4757, Name = "Mara", WikiUrl = "https://wiki.guildwars.com/wiki/Mara" };
    public static readonly Npc Isokeh = new() { Id = 4758, Name = "Isokeh", WikiUrl = "https://wiki.guildwars.com/wiki/Isokeh" };
    public static readonly Npc Rakanja = new() { Id = 4759, Name = "Rakanja", WikiUrl = "https://wiki.guildwars.com/wiki/Rakanja" };
    public static readonly Npc Lisha = new() { Id = 4760, Name = "Lisha", WikiUrl = "https://wiki.guildwars.com/wiki/Lisha" };
    public static readonly Npc Zahwena = new() { Id = 4761, Name = "Zahwena", WikiUrl = "https://wiki.guildwars.com/wiki/Zahwena" };
    public static readonly Npc Nagozi = new() { Id = 4762, Name = "Nagozi", WikiUrl = "https://wiki.guildwars.com/wiki/Nagozi" };
    public static readonly Npc Arayah = new() { Id = 4763, Name = "Arayah", WikiUrl = "https://wiki.guildwars.com/wiki/Arayah" };
    public static readonly Npc Mothusi = new() { Id = 4764, Name = "Mothusi", WikiUrl = "https://wiki.guildwars.com/wiki/Mothusi" };
    public static readonly Npc Farisah = new() { Id = 4765, Name = "Farisah", WikiUrl = "https://wiki.guildwars.com/wiki/Farisah" };
    public static readonly Npc Sunspear2 = new() { Id = 4766, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Hagon = new() { Id = 4767, Name = "Hagon", WikiUrl = "https://wiki.guildwars.com/wiki/Hagon" };
    public static readonly Npc Kohjo = new() { Id = 4767, Name = "Kohjo", WikiUrl = "https://wiki.guildwars.com/wiki/Kohjo" };
    public static readonly Npc Lahna = new() { Id = 4768, Name = "Lahna", WikiUrl = "https://wiki.guildwars.com/wiki/Lahna" };
    public static readonly Npc ScoutKahra = new() { Id = 4768, Name = "Scout Kahra", WikiUrl = "https://wiki.guildwars.com/wiki/Scout_Kahra" };
    public static readonly Npc Sunspear3 = new() { Id = 4768, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc SunspearElementalist = new() { Id = 4768, Name = "Sunspear Elementalist", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Elementalist" };
    public static readonly Npc SunspearQuartermaster = new() { Id = 4768, Name = "Sunspear Quartermaster", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Quartermaster" };
    public static readonly Npc Hero = new() { Id = 4769, Name = "Hero", WikiUrl = "https://wiki.guildwars.com/wiki/Hero" };
    public static readonly Npc Kushau = new() { Id = 4769, Name = "Kushau", WikiUrl = "https://wiki.guildwars.com/wiki/Kushau" };
    public static readonly Npc Lokuto = new() { Id = 4769, Name = "Lokuto", WikiUrl = "https://wiki.guildwars.com/wiki/Lokuto" };
    public static readonly Npc Sunspear4 = new() { Id = 4769, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Dinja = new() { Id = 4770, Name = "Dinja", WikiUrl = "https://wiki.guildwars.com/wiki/Dinja" };
    public static readonly Npc Risa = new() { Id = 4770, Name = "Risa", WikiUrl = "https://wiki.guildwars.com/wiki/Risa" };
    public static readonly Npc Shorewatcher2 = new() { Id = 4770, Name = "Shorewatcher", WikiUrl = "https://wiki.guildwars.com/wiki/Shorewatcher" };
    public static readonly Npc Sunspear5 = new() { Id = 4770, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc SunspearWarrior = new() { Id = 4770, Name = "Sunspear Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Warrior" };
    public static readonly Npc Yahir = new() { Id = 4770, Name = "Yahir", WikiUrl = "https://wiki.guildwars.com/wiki/Yahir" };
    public static readonly Npc SunspearWarrior1 = new() { Id = 4771, Name = "Sunspear Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Warrior" };
    public static readonly Npc Kina = new() { Id = 4772, Name = "Kina", WikiUrl = "https://wiki.guildwars.com/wiki/Kina" };
    public static readonly Npc Sunspear6 = new() { Id = 4772, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc SunspearRanger = new() { Id = 4772, Name = "Sunspear Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Ranger" };
    public static readonly Npc SunspearScout = new() { Id = 4772, Name = "Sunspear Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Scout" };
    public static readonly Npc Kanessa = new() { Id = 4773, Name = "Kanessa", WikiUrl = "https://wiki.guildwars.com/wiki/Kanessa" };
    public static readonly Npc CaptainGudur = new() { Id = 4774, Name = "Captain Gudur", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Gudur" };
    public static readonly Npc Korsu = new() { Id = 4774, Name = "Korsu", WikiUrl = "https://wiki.guildwars.com/wiki/Korsu" };
    public static readonly Npc Libeh = new() { Id = 4774, Name = "Libeh", WikiUrl = "https://wiki.guildwars.com/wiki/Libeh" };
    public static readonly Npc RaidmarshalMehdara = new() { Id = 4774, Name = "Raidmarshal Mehdara", WikiUrl = "https://wiki.guildwars.com/wiki/Raidmarshal_Mehdara" };
    public static readonly Npc Rohmen = new() { Id = 4774, Name = "Rohmen", WikiUrl = "https://wiki.guildwars.com/wiki/Rohmen" };
    public static readonly Npc SpearmarshalBendro = new() { Id = 4774, Name = "Spearmarshal Bendro", WikiUrl = "https://wiki.guildwars.com/wiki/Spearmarshal_Bendro" };
    public static readonly Npc Sunspear7 = new() { Id = 4774, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc SunspearParagon = new() { Id = 4774, Name = "Sunspear Paragon", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Paragon" };
    public static readonly Npc SunspearQuartermaster1 = new() { Id = 4774, Name = "Sunspear Quartermaster", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Quartermaster" };
    public static readonly Npc SunspearScout1 = new() { Id = 4774, Name = "Sunspear Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Scout" };
    public static readonly Npc Sunspear8 = new() { Id = 4775, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Sunspear9 = new() { Id = 4777, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Hawan = new() { Id = 4778, Name = "Hawan", WikiUrl = "https://wiki.guildwars.com/wiki/Hawan" };
    public static readonly Npc Sunspear10 = new() { Id = 4778, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Sunspear11 = new() { Id = 4779, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Sunspear12 = new() { Id = 4780, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Sunspear13 = new() { Id = 4781, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Sunspear14 = new() { Id = 4782, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc Churrlan = new() { Id = 4784, Name = "Churrlan", WikiUrl = "https://wiki.guildwars.com/wiki/Churrlan" };
    public static readonly Npc CapturedSunspear = new() { Id = 4786, Name = "Captured Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Captured_Sunspear" };
    public static readonly Npc CapturedSunspear1 = new() { Id = 4788, Name = "Captured Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Captured_Sunspear" };
    public static readonly Npc SunspearPrisoner = new() { Id = 4789, Name = "Sunspear Prisoner", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Prisoner" };
    public static readonly Npc CapturedSunspear2 = new() { Id = 4790, Name = "Captured Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Captured_Sunspear" };
    public static readonly Npc SunspearPrisoner1 = new() { Id = 4790, Name = "Sunspear Prisoner", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Prisoner" };
    public static readonly Npc CapturedSunspear3 = new() { Id = 4791, Name = "Captured Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Captured_Sunspear" };
    public static readonly Npc SunspearPrisoner2 = new() { Id = 4793, Name = "Sunspear Prisoner", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Prisoner" };
    public static readonly Npc SunspearRecruit = new() { Id = 4797, Name = "Sunspear Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Recruit" };
    public static readonly Npc Sunspear15 = new() { Id = 4798, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc SunspearRecruit1 = new() { Id = 4798, Name = "Sunspear Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Recruit" };
    public static readonly Npc Sunspear16 = new() { Id = 4799, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc CaptainVahndah = new() { Id = 4801, Name = "Captain Vahndah", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Vahndah" };
    public static readonly Npc SunspearRecruit2 = new() { Id = 4801, Name = "Sunspear Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Recruit" };
    public static readonly Npc SunspearRecruit3 = new() { Id = 4805, Name = "Sunspear Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Recruit" };
    public static readonly Npc SunspearRecruit4 = new() { Id = 4806, Name = "Sunspear Recruit", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Recruit" };
    public static readonly Npc Sunspear17 = new() { Id = 4807, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear" };
    public static readonly Npc SunspearRefugee = new() { Id = 4807, Name = "Sunspear Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Refugee" };
    public static readonly Npc CommanderSuha = new() { Id = 4808, Name = "Commander Suha", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Suha" };
    public static readonly Npc EliteScoutZusoh = new() { Id = 4808, Name = "Elite Scout Zusoh", WikiUrl = "https://wiki.guildwars.com/wiki/Elite_Scout_Zusoh" };
    public static readonly Npc SunspearModiki = new() { Id = 4808, Name = "Sunspear Modiki", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Modiki" };
    public static readonly Npc SunspearRefugee1 = new() { Id = 4808, Name = "Sunspear Refugee", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Refugee" };
    public static readonly Npc SunspearWarrior2 = new() { Id = 4808, Name = "Sunspear Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Warrior" };
    public static readonly Npc Vadah = new() { Id = 4808, Name = "Vadah", WikiUrl = "https://wiki.guildwars.com/wiki/Vadah" };
    public static readonly Npc ElderSuhl = new() { Id = 4809, Name = "Elder Suhl", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Suhl" };
    public static readonly Npc Nerashi = new() { Id = 4810, Name = "Nerashi", WikiUrl = "https://wiki.guildwars.com/wiki/Nerashi" };
    public static readonly Npc ElderOlunideh = new() { Id = 4812, Name = "Elder Olunideh", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Olunideh" };
    public static readonly Npc Ahtok = new() { Id = 4813, Name = "Ahtok", WikiUrl = "https://wiki.guildwars.com/wiki/Ahtok" };
    public static readonly Npc Nehdukah = new() { Id = 4814, Name = "Nehdukah", WikiUrl = "https://wiki.guildwars.com/wiki/Nehdukah" };
    public static readonly Npc Rojis = new() { Id = 4815, Name = "Rojis", WikiUrl = "https://wiki.guildwars.com/wiki/Rojis" };
    public static readonly Npc Lonai = new() { Id = 4816, Name = "Lonai", WikiUrl = "https://wiki.guildwars.com/wiki/Lonai" };
    public static readonly Npc Lonai1 = new() { Id = 4817, Name = "Lonai", WikiUrl = "https://wiki.guildwars.com/wiki/Lonai" };
    public static readonly Npc Tahon = new() { Id = 4821, Name = "Tahon", WikiUrl = "https://wiki.guildwars.com/wiki/Tahon" };
    public static readonly Npc IstaniCultist = new() { Id = 4822, Name = "Istani Cultist", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Cultist" };
    public static readonly Npc CastellanPuuba1 = new() { Id = 4823, Name = "Castellan Puuba", WikiUrl = "https://wiki.guildwars.com/wiki/Castellan_Puuba" };
    public static readonly Npc SahlahjartheDead = new() { Id = 4824, Name = "Sahlahjar the Dead", WikiUrl = "https://wiki.guildwars.com/wiki/Sahlahjar_the_Dead" };
    public static readonly Npc Digger = new() { Id = 4825, Name = "Digger", WikiUrl = "https://wiki.guildwars.com/wiki/Digger" };
    public static readonly Npc Dengo = new() { Id = 4826, Name = "Dengo", WikiUrl = "https://wiki.guildwars.com/wiki/Dengo" };
    public static readonly Npc Bahbukar = new() { Id = 4830, Name = "Bahbukar", WikiUrl = "https://wiki.guildwars.com/wiki/Bahbukar" };
    public static readonly Npc SahrehJelon = new() { Id = 4831, Name = "Sahreh Jelon", WikiUrl = "https://wiki.guildwars.com/wiki/Sahreh_Jelon" };
    public static readonly Npc Behron = new() { Id = 4832, Name = "Behron", WikiUrl = "https://wiki.guildwars.com/wiki/Behron" };
    public static readonly Npc DalzJelon = new() { Id = 4833, Name = "Dalz Jelon", WikiUrl = "https://wiki.guildwars.com/wiki/Dalz_Jelon" };
    public static readonly Npc JedurWahmeh = new() { Id = 4834, Name = "Jedur Wahmeh", WikiUrl = "https://wiki.guildwars.com/wiki/Jedur_Wahmeh" };
    public static readonly Npc MiselaWahmeh = new() { Id = 4835, Name = "Misela Wahmeh", WikiUrl = "https://wiki.guildwars.com/wiki/Misela_Wahmeh" };
    public static readonly Npc MutuWahmeh = new() { Id = 4836, Name = "Mutu Wahmeh", WikiUrl = "https://wiki.guildwars.com/wiki/Mutu_Wahmeh" };
    public static readonly Npc Jerek = new() { Id = 4837, Name = "Jerek", WikiUrl = "https://wiki.guildwars.com/wiki/Jerek" };
    public static readonly Npc EvilMine = new() { Id = 4838, Name = "Evil Mine", WikiUrl = "https://wiki.guildwars.com/wiki/Evil_Mine" };
    public static readonly Npc IstaniCultist1 = new() { Id = 4839, Name = "Istani Cultist", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Cultist" };
    public static readonly Npc Sailor = new() { Id = 4841, Name = "Sailor", WikiUrl = "https://wiki.guildwars.com/wiki/Sailor" };
    public static readonly Npc IstaniPeasant1 = new() { Id = 4846, Name = "Istani Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Peasant" };
    public static readonly Npc IstaniPeasant2 = new() { Id = 4847, Name = "Istani Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Peasant" };
    public static readonly Npc IstaniPeasant3 = new() { Id = 4848, Name = "Istani Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Peasant" };
    public static readonly Npc SunspearEvacuee = new() { Id = 4849, Name = "Sunspear Evacuee", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Evacuee" };
    public static readonly Npc SunspearEvacuee1 = new() { Id = 4850, Name = "Sunspear Evacuee", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Evacuee" };
    public static readonly Npc TheGreatZehtuka = new() { Id = 4852, Name = "The Great Zehtuka", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Zehtuka" };
    public static readonly Npc NecromancerLahse = new() { Id = 4854, Name = "Necromancer Lahse", WikiUrl = "https://wiki.guildwars.com/wiki/Necromancer_Lahse" };
    public static readonly Npc AvatarofKormir = new() { Id = 4858, Name = "Avatar of Kormir", WikiUrl = "https://wiki.guildwars.com/wiki/Avatar_of_Kormir" };
    public static readonly Npc SeerofTruth = new() { Id = 4858, Name = "Seer of Truth", WikiUrl = "https://wiki.guildwars.com/wiki/Seer_of_Truth" };
    public static readonly Npc Kormir2 = new() { Id = 4859, Name = "Kormir", WikiUrl = "https://wiki.guildwars.com/wiki/Kormir" };
    public static readonly Npc Kormir3 = new() { Id = 4860, Name = "Kormir", WikiUrl = "https://wiki.guildwars.com/wiki/Kormir" };
    public static readonly Npc Kormir4 = new() { Id = 4861, Name = "Kormir", WikiUrl = "https://wiki.guildwars.com/wiki/Kormir" };
    public static readonly Npc ZelnehlunFastfoot = new() { Id = 4862, Name = "Zelnehlun Fastfoot", WikiUrl = "https://wiki.guildwars.com/wiki/Zelnehlun_Fastfoot" };
    public static readonly Npc DabinehDeathbringer = new() { Id = 4863, Name = "Dabineh Deathbringer", WikiUrl = "https://wiki.guildwars.com/wiki/Dabineh_Deathbringer" };
    public static readonly Npc WandalztheAngry = new() { Id = 4864, Name = "Wandalz the Angry", WikiUrl = "https://wiki.guildwars.com/wiki/Wandalz_the_Angry" };
    public static readonly Npc OnwanLordoftheNtouka = new() { Id = 4865, Name = "Onwan Lord of the Ntouka", WikiUrl = "https://wiki.guildwars.com/wiki/Onwan_Lord_of_the_Ntouka" };
    public static readonly Npc OlunossWindwalker = new() { Id = 4866, Name = "Olunoss Windwalker", WikiUrl = "https://wiki.guildwars.com/wiki/Olunoss_Windwalker" };
    public static readonly Npc ChiossenSoothingBreeze = new() { Id = 4867, Name = "Chiossen Soothing Breeze", WikiUrl = "https://wiki.guildwars.com/wiki/Chiossen_Soothing_Breeze" };
    public static readonly Npc ShelbohtheRavenous = new() { Id = 4868, Name = "Shelboh the Ravenous", WikiUrl = "https://wiki.guildwars.com/wiki/Shelboh_the_Ravenous" };
    public static readonly Npc JoknangEarthturner = new() { Id = 4869, Name = "Joknang Earthturner", WikiUrl = "https://wiki.guildwars.com/wiki/Joknang_Earthturner" };
    public static readonly Npc EnadiztheHardheaded = new() { Id = 4870, Name = "Enadiz the Hardheaded", WikiUrl = "https://wiki.guildwars.com/wiki/Enadiz_the_Hardheaded" };
    public static readonly Npc MahtoSharptooth = new() { Id = 4871, Name = "Mahto Sharptooth", WikiUrl = "https://wiki.guildwars.com/wiki/Mahto_Sharptooth" };
    public static readonly Npc SehlonBeautifulWater = new() { Id = 4872, Name = "Sehlon Beautiful Water", WikiUrl = "https://wiki.guildwars.com/wiki/Sehlon_Beautiful_Water" };
    public static readonly Npc KorrLivingFlame = new() { Id = 4873, Name = "Korr Living Flame", WikiUrl = "https://wiki.guildwars.com/wiki/Korr_Living_Flame" };
    public static readonly Npc LuntoSharpfoot = new() { Id = 4874, Name = "Lunto Sharpfoot", WikiUrl = "https://wiki.guildwars.com/wiki/Lunto_Sharpfoot" };
    public static readonly Npc ChidehkirLightoftheBlind = new() { Id = 4875, Name = "Chidehkir Light of the Blind", WikiUrl = "https://wiki.guildwars.com/wiki/Chidehkir_Light_of_the_Blind" };
    public static readonly Npc ChortheBladed = new() { Id = 4876, Name = "Chor the Bladed", WikiUrl = "https://wiki.guildwars.com/wiki/Chor_the_Bladed" };
    public static readonly Npc EshauLongspear = new() { Id = 4877, Name = "Eshau Longspear", WikiUrl = "https://wiki.guildwars.com/wiki/Eshau_Longspear" };
    public static readonly Npc EshimMindclouder = new() { Id = 4878, Name = "Eshim Mindclouder", WikiUrl = "https://wiki.guildwars.com/wiki/Eshim_Mindclouder" };
    public static readonly Npc BirnehSkybringer = new() { Id = 4879, Name = "Birneh Skybringer", WikiUrl = "https://wiki.guildwars.com/wiki/Birneh_Skybringer" };
    public static readonly Npc ModtiDarkflower = new() { Id = 4880, Name = "Modti Darkflower", WikiUrl = "https://wiki.guildwars.com/wiki/Modti_Darkflower" };
    public static readonly Npc NeolitheContagious = new() { Id = 4881, Name = "Neoli the Contagious", WikiUrl = "https://wiki.guildwars.com/wiki/Neoli_the_Contagious" };
    public static readonly Npc BubahlIcehands = new() { Id = 4882, Name = "Bubahl Icehands", WikiUrl = "https://wiki.guildwars.com/wiki/Bubahl_Icehands" };
    public static readonly Npc JernehNightbringer = new() { Id = 4883, Name = "Jerneh Nightbringer", WikiUrl = "https://wiki.guildwars.com/wiki/Jerneh_Nightbringer" };
    public static readonly Npc ArmindtheBalancer = new() { Id = 4884, Name = "Armind the Balancer", WikiUrl = "https://wiki.guildwars.com/wiki/Armind_the_Balancer" };
    public static readonly Npc BuhonIcelord = new() { Id = 4885, Name = "Buhon Icelord", WikiUrl = "https://wiki.guildwars.com/wiki/Buhon_Icelord" };
    public static readonly Npc RobahHardback = new() { Id = 4886, Name = "Robah Hardback", WikiUrl = "https://wiki.guildwars.com/wiki/Robah_Hardback" };
    public static readonly Npc ChurrtatheRock = new() { Id = 4887, Name = "Churrta the Rock", WikiUrl = "https://wiki.guildwars.com/wiki/Churrta_the_Rock" };
    public static readonly Npc TundosstheDestroyer = new() { Id = 4888, Name = "Tundoss the Destroyer", WikiUrl = "https://wiki.guildwars.com/wiki/Tundoss_the_Destroyer" };
    public static readonly Npc PodalturtheAngry = new() { Id = 4889, Name = "Podaltur the Angry", WikiUrl = "https://wiki.guildwars.com/wiki/Podaltur_the_Angry" };
    public static readonly Npc TerobRoundback = new() { Id = 4890, Name = "Terob Roundback", WikiUrl = "https://wiki.guildwars.com/wiki/Terob_Roundback" };
    public static readonly Npc DzabelLandGuardian = new() { Id = 4891, Name = "Dzabel Land Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Dzabel_Land_Guardian" };
    public static readonly Npc TheDrought = new() { Id = 4892, Name = "The Drought", WikiUrl = "https://wiki.guildwars.com/wiki/The_Drought" };
    public static readonly Npc KehmaktheTranquil = new() { Id = 4894, Name = "Kehmak the Tranquil", WikiUrl = "https://wiki.guildwars.com/wiki/Kehmak_the_Tranquil" };
    public static readonly Npc RanshekCarrionEator = new() { Id = 4895, Name = "Ranshek Carrion Eator", WikiUrl = "https://wiki.guildwars.com/wiki/Ranshek_Carrion_Eator" };
    public static readonly Npc YakunTrueshot = new() { Id = 4896, Name = "Yakun Trueshot", WikiUrl = "https://wiki.guildwars.com/wiki/Yakun_Trueshot" };
    public static readonly Npc InfectiousNightmare = new() { Id = 4897, Name = "Infectious Nightmare", WikiUrl = "https://wiki.guildwars.com/wiki/Infectious_Nightmare" };
    public static readonly Npc SeborhinPest = new() { Id = 4898, Name = "Seborhin Pest", WikiUrl = "https://wiki.guildwars.com/wiki/Seborhin_Pest" };
    public static readonly Npc VeldtNephila = new() { Id = 4898, Name = "Veldt Nephila", WikiUrl = "https://wiki.guildwars.com/wiki/Veldt_Nephila" };
    public static readonly Npc VeldtNephila1 = new() { Id = 4899, Name = "Veldt Nephila", WikiUrl = "https://wiki.guildwars.com/wiki/Veldt_Nephila" };
    public static readonly Npc CrackedMesa = new() { Id = 4900, Name = "Cracked Mesa", WikiUrl = "https://wiki.guildwars.com/wiki/Cracked_Mesa" };
    public static readonly Npc StoneShardCrag = new() { Id = 4901, Name = "Stone Shard Crag", WikiUrl = "https://wiki.guildwars.com/wiki/Stone_Shard_Crag" };
    public static readonly Npc ImmolatedDjinn = new() { Id = 4902, Name = "Immolated Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Immolated_Djinn" };
    public static readonly Npc WaterDjinn2 = new() { Id = 4903, Name = "Water Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Djinn" };
    public static readonly Npc MaelstromDjinn = new() { Id = 4904, Name = "Maelstrom Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Maelstrom_Djinn" };
    public static readonly Npc BloodCowlHeket2 = new() { Id = 4905, Name = "Blood Cowl Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Blood_Cowl_Heket" };
    public static readonly Npc BlueTongueHeket2 = new() { Id = 4906, Name = "Blue Tongue Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Blue_Tongue_Heket" };
    public static readonly Npc StoneaxeHeket2 = new() { Id = 4907, Name = "Stoneaxe Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Stoneaxe_Heket" };
    public static readonly Npc BeastSwornHeket2 = new() { Id = 4908, Name = "Beast Sworn Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Beast_Sworn_Heket" };
    public static readonly Npc CrestedNtoukaBird = new() { Id = 4909, Name = "Crested Ntouka Bird", WikiUrl = "https://wiki.guildwars.com/wiki/Crested_Ntouka_Bird" };
    public static readonly Npc NtoukaBird = new() { Id = 4910, Name = "Ntouka Bird", WikiUrl = "https://wiki.guildwars.com/wiki/Ntouka_Bird" };
    public static readonly Npc RampagingNtouka = new() { Id = 4911, Name = "Rampaging Ntouka", WikiUrl = "https://wiki.guildwars.com/wiki/Rampaging_Ntouka" };
    public static readonly Npc TuskedHowler = new() { Id = 4912, Name = "Tusked Howler", WikiUrl = "https://wiki.guildwars.com/wiki/Tusked_Howler" };
    public static readonly Npc TuskedHunter = new() { Id = 4913, Name = "Tusked Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Tusked_Hunter" };
    public static readonly Npc SteelfangDrake = new() { Id = 4914, Name = "Steelfang Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Steelfang_Drake" };
    public static readonly Npc KuskaleBlighter = new() { Id = 4915, Name = "Kuskale Blighter", WikiUrl = "https://wiki.guildwars.com/wiki/Kuskale_Blighter" };
    public static readonly Npc RidgebackKuskale = new() { Id = 4916, Name = "Ridgeback Kuskale", WikiUrl = "https://wiki.guildwars.com/wiki/Ridgeback_Kuskale" };
    public static readonly Npc KuskaleLasher = new() { Id = 4917, Name = "Kuskale Lasher", WikiUrl = "https://wiki.guildwars.com/wiki/Kuskale_Lasher" };
    public static readonly Npc FrigidKuskale = new() { Id = 4918, Name = "Frigid Kuskale", WikiUrl = "https://wiki.guildwars.com/wiki/Frigid_Kuskale" };
    public static readonly Npc SeborhinPest1 = new() { Id = 4919, Name = "Seborhin Pest", WikiUrl = "https://wiki.guildwars.com/wiki/Seborhin_Pest" };
    public static readonly Npc VeldtBeetleQueen = new() { Id = 4919, Name = "Veldt Beetle Queen", WikiUrl = "https://wiki.guildwars.com/wiki/Veldt_Beetle_Queen" };
    public static readonly Npc BladedVeldtTermite = new() { Id = 4920, Name = "Bladed Veldt Termite", WikiUrl = "https://wiki.guildwars.com/wiki/Bladed_Veldt_Termite" };
    public static readonly Npc InfectiousDementia1 = new() { Id = 4920, Name = "Infectious Dementia", WikiUrl = "https://wiki.guildwars.com/wiki/Infectious_Dementia" };
    public static readonly Npc SeborhinPest2 = new() { Id = 4920, Name = "Seborhin Pest", WikiUrl = "https://wiki.guildwars.com/wiki/Seborhin_Pest" };
    public static readonly Npc BladedVeldtTermite1 = new() { Id = 4921, Name = "Bladed Veldt Termite", WikiUrl = "https://wiki.guildwars.com/wiki/Bladed_Veldt_Termite" };
    public static readonly Npc SeborhinPest3 = new() { Id = 4922, Name = "Seborhin Pest", WikiUrl = "https://wiki.guildwars.com/wiki/Seborhin_Pest" };
    public static readonly Npc VeldtBeetleLance = new() { Id = 4922, Name = "Veldt Beetle Lance", WikiUrl = "https://wiki.guildwars.com/wiki/Veldt_Beetle_Lance" };
    public static readonly Npc StormfaceJacaranda = new() { Id = 4923, Name = "Stormface Jacaranda", WikiUrl = "https://wiki.guildwars.com/wiki/Stormface_Jacaranda" };
    public static readonly Npc StormforceJacaranda = new() { Id = 4923, Name = "Stormforce Jacaranda", WikiUrl = "https://wiki.guildwars.com/wiki/Stormforce_Jacaranda" };
    public static readonly Npc ViciousSeedling = new() { Id = 4923, Name = "Vicious Seedling", WikiUrl = "https://wiki.guildwars.com/wiki/Vicious_Seedling" };
    public static readonly Npc MurmuringThornbrush = new() { Id = 4924, Name = "Murmuring Thornbrush", WikiUrl = "https://wiki.guildwars.com/wiki/Murmuring_Thornbrush" };
    public static readonly Npc ViciousSeedling1 = new() { Id = 4924, Name = "Vicious Seedling", WikiUrl = "https://wiki.guildwars.com/wiki/Vicious_Seedling" };
    public static readonly Npc HarojFiremane = new() { Id = 4925, Name = "Haroj Firemane", WikiUrl = "https://wiki.guildwars.com/wiki/Haroj_Firemane" };
    public static readonly Npc MirageIboga1 = new() { Id = 4925, Name = "Mirage Iboga", WikiUrl = "https://wiki.guildwars.com/wiki/Mirage_Iboga" };
    public static readonly Npc ViciousSeedling2 = new() { Id = 4925, Name = "Vicious Seedling", WikiUrl = "https://wiki.guildwars.com/wiki/Vicious_Seedling" };
    public static readonly Npc CorruptedNature = new() { Id = 4926, Name = "Corrupted Nature", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Nature" };
    public static readonly Npc MandragorImp3 = new() { Id = 4926, Name = "Mandragor Imp", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Imp" };
    public static readonly Npc CorruptedNature1 = new() { Id = 4927, Name = "Corrupted Nature", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Nature" };
    public static readonly Npc StonefleshMandragor2 = new() { Id = 4927, Name = "Stoneflesh Mandragor", WikiUrl = "https://wiki.guildwars.com/wiki/Stoneflesh_Mandragor" };
    public static readonly Npc CorruptedNature2 = new() { Id = 4928, Name = "Corrupted Nature", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Nature" };
    public static readonly Npc MandragorSlither3 = new() { Id = 4928, Name = "Mandragor Slither", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Slither" };
    public static readonly Npc Droughtling = new() { Id = 4930, Name = "Droughtling", WikiUrl = "https://wiki.guildwars.com/wiki/Droughtling" };
    public static readonly Npc FerociousDrake = new() { Id = 4931, Name = "Ferocious Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Ferocious_Drake" };
    public static readonly Npc Toma = new() { Id = 4932, Name = "Toma", WikiUrl = "https://wiki.guildwars.com/wiki/Toma" };
    public static readonly Npc Cow = new() { Id = 4933, Name = "Cow", WikiUrl = "https://wiki.guildwars.com/wiki/Cow" };
    public static readonly Npc Yartu = new() { Id = 4934, Name = "Yartu", WikiUrl = "https://wiki.guildwars.com/wiki/Yartu" };
    public static readonly Npc CorruptedRoot = new() { Id = 4935, Name = "Corrupted Root", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Root" };
    public static readonly Npc AncestralBud = new() { Id = 4936, Name = "Ancestral Bud", WikiUrl = "https://wiki.guildwars.com/wiki/Ancestral_Bud" };
    public static readonly Npc CorruptedFlower = new() { Id = 4936, Name = "Corrupted Flower", WikiUrl = "https://wiki.guildwars.com/wiki/Corrupted_Flower" };
    public static readonly Npc SevadsKeeper = new() { Id = 4938, Name = "Sevad’s Keeper", WikiUrl = "https://wiki.guildwars.com/wiki/Sevad's_Keeper" };
    public static readonly Npc TormentedLand = new() { Id = 4939, Name = "Tormented Land", WikiUrl = "https://wiki.guildwars.com/wiki/Tormented_Land" };
    public static readonly Npc EarthenAbomination = new() { Id = 4940, Name = "Earthen Abomination", WikiUrl = "https://wiki.guildwars.com/wiki/Earthen_Abomination" };
    public static readonly Npc SlortNilbog = new() { Id = 4941, Name = "Slort Nilbog", WikiUrl = "https://wiki.guildwars.com/wiki/Slort_Nilbog" };
    public static readonly Npc ZephyrHedger = new() { Id = 4942, Name = "Zephyr Hedger", WikiUrl = "https://wiki.guildwars.com/wiki/Zephyr_Hedger" };
    public static readonly Npc Centaur = new() { Id = 4944, Name = "Centaur", WikiUrl = "https://wiki.guildwars.com/wiki/Centaur" };
    public static readonly Npc DirahTraptail = new() { Id = 4944, Name = "Dirah Traptail", WikiUrl = "https://wiki.guildwars.com/wiki/Dirah_Traptail" };
    public static readonly Npc GrifEbonmane = new() { Id = 4944, Name = "Grif Ebonmane", WikiUrl = "https://wiki.guildwars.com/wiki/Grif_Ebonmane" };
    public static readonly Npc HarojFiremane1 = new() { Id = 4944, Name = "Haroj Firemane", WikiUrl = "https://wiki.guildwars.com/wiki/Haroj_Firemane" };
    public static readonly Npc KolSwordshanks = new() { Id = 4944, Name = "Kol Swordshanks", WikiUrl = "https://wiki.guildwars.com/wiki/Kol_Swordshanks" };
    public static readonly Npc LaphLongmane = new() { Id = 4944, Name = "Laph Longmane", WikiUrl = "https://wiki.guildwars.com/wiki/Laph_Longmane" };
    public static readonly Npc VeldrunnerFighter = new() { Id = 4944, Name = "Veldrunner Fighter", WikiUrl = "https://wiki.guildwars.com/wiki/Veldrunner_Fighter" };
    public static readonly Npc YeraSwiftsight = new() { Id = 4944, Name = "Yera Swiftsight", WikiUrl = "https://wiki.guildwars.com/wiki/Yera_Swiftsight" };
    public static readonly Npc VeldrunnerCentaur = new() { Id = 4945, Name = "Veldrunner Centaur", WikiUrl = "https://wiki.guildwars.com/wiki/Veldrunner_Centaur" };
    public static readonly Npc MirzaVeldrunner = new() { Id = 4946, Name = "Mirza Veldrunner", WikiUrl = "https://wiki.guildwars.com/wiki/Mirza_Veldrunner" };
    public static readonly Npc ShiroTagachi2 = new() { Id = 4948, Name = "Shiro Tagachi", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro_Tagachi" };
    public static readonly Npc UndeadLich = new() { Id = 4949, Name = "Undead Lich", WikiUrl = "https://wiki.guildwars.com/wiki/Undead_Lich" };
    public static readonly Npc ShakahmtheSummoner = new() { Id = 4950, Name = "Shakahm the Summoner", WikiUrl = "https://wiki.guildwars.com/wiki/Shakahm_the_Summoner" };
    public static readonly Npc FaveoAggredior = new() { Id = 4951, Name = "Faveo Aggredior", WikiUrl = "https://wiki.guildwars.com/wiki/Faveo_Aggredior" };
    public static readonly Npc SaevioProelium = new() { Id = 4952, Name = "Saevio Proelium", WikiUrl = "https://wiki.guildwars.com/wiki/Saevio_Proelium" };
    public static readonly Npc CreoVulnero = new() { Id = 4953, Name = "Creo Vulnero", WikiUrl = "https://wiki.guildwars.com/wiki/Creo_Vulnero" };
    public static readonly Npc ExuroFlatus = new() { Id = 4954, Name = "Exuro Flatus", WikiUrl = "https://wiki.guildwars.com/wiki/Exuro_Flatus" };
    public static readonly Npc ScytheofChaos2 = new() { Id = 4955, Name = "Scythe of Chaos", WikiUrl = "https://wiki.guildwars.com/wiki/Scythe_of_Chaos" };
    public static readonly Npc GraspofInsanity1 = new() { Id = 4956, Name = "Grasp of Insanity", WikiUrl = "https://wiki.guildwars.com/wiki/Grasp_of_Insanity" };
    public static readonly Npc WrathfulStorm1 = new() { Id = 4957, Name = "Wrathful Storm", WikiUrl = "https://wiki.guildwars.com/wiki/Wrathful_Storm" };
    public static readonly Npc ShadowMesmer = new() { Id = 4958, Name = "Shadow Mesmer", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_Mesmer" };
    public static readonly Npc ShadowElemental = new() { Id = 4959, Name = "Shadow Elemental", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_Elemental" };
    public static readonly Npc ShadowMonk = new() { Id = 4960, Name = "Shadow Monk", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_Monk" };
    public static readonly Npc ShadowWarrior = new() { Id = 4961, Name = "Shadow Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_Warrior" };
    public static readonly Npc ShadowRanger = new() { Id = 4962, Name = "Shadow Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_Ranger" };
    public static readonly Npc ShadowBeast = new() { Id = 4963, Name = "Shadow Beast", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_Beast" };
    public static readonly Npc FleshGlutton = new() { Id = 4964, Name = "Flesh Glutton", WikiUrl = "https://wiki.guildwars.com/wiki/Flesh_Glutton" };
    public static readonly Npc TitanAbomination = new() { Id = 4964, Name = "Titan Abomination", WikiUrl = "https://wiki.guildwars.com/wiki/Titan_Abomination" };
    public static readonly Npc PainTitan = new() { Id = 4965, Name = "Pain Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Pain_Titan" };
    public static readonly Npc MadnessTitan = new() { Id = 4966, Name = "Madness Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Madness_Titan" };
    public static readonly Npc ShirokenAssassin2 = new() { Id = 4967, Name = "Shiro'ken Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Assassin" };
    public static readonly Npc ScorchEmberspire = new() { Id = 4968, Name = "Scorch Emberspire", WikiUrl = "https://wiki.guildwars.com/wiki/Scorch_Emberspire" };
    public static readonly Npc Rukkassa = new() { Id = 4969, Name = "Rukkassa", WikiUrl = "https://wiki.guildwars.com/wiki/Rukkassa" };
    public static readonly Npc KeeperJinyssa1 = new() { Id = 4970, Name = "Keeper Jinyssa", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_Jinyssa" };
    public static readonly Npc KeeperKauniss1 = new() { Id = 4970, Name = "Keeper Kauniss", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_Kauniss" };
    public static readonly Npc Terick = new() { Id = 4972, Name = "Terick", WikiUrl = "https://wiki.guildwars.com/wiki/Terick" };
    public static readonly Npc FortuneTeller = new() { Id = 4973, Name = "Fortune Teller", WikiUrl = "https://wiki.guildwars.com/wiki/Fortune_Teller" };
    public static readonly Npc IgnisCruor = new() { Id = 4974, Name = "Ignis Cruor", WikiUrl = "https://wiki.guildwars.com/wiki/Ignis_Cruor" };
    public static readonly Npc SecurisPhasmatis = new() { Id = 4975, Name = "Securis Phasmatis", WikiUrl = "https://wiki.guildwars.com/wiki/Securis_Phasmatis" };
    public static readonly Npc LetumContineo = new() { Id = 4976, Name = "Letum Contineo", WikiUrl = "https://wiki.guildwars.com/wiki/Letum_Contineo" };
    public static readonly Npc TheHunter = new() { Id = 4977, Name = "The Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hunter" };
    public static readonly Npc Dunbri = new() { Id = 4979, Name = "Dunbri", WikiUrl = "https://wiki.guildwars.com/wiki/Dunbri" };
    public static readonly Npc ImperialCaptainShiWang = new() { Id = 4979, Name = "Imperial Captain Shi Wang", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Captain_Shi_Wang" };
    public static readonly Npc ImperialGuardHaoLi = new() { Id = 4979, Name = "Imperial Guard Hao Li", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guard_Hao_Li" };
    public static readonly Npc ImperialGuardKaichen = new() { Id = 4979, Name = "Imperial Guard Kaichen", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guard_Kaichen" };
    public static readonly Npc Jarindok = new() { Id = 4979, Name = "Jarindok", WikiUrl = "https://wiki.guildwars.com/wiki/Jarindok" };
    public static readonly Npc JoyousSoul = new() { Id = 4979, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc Kaelen = new() { Id = 4979, Name = "Kaelen", WikiUrl = "https://wiki.guildwars.com/wiki/Kaelen" };
    public static readonly Npc Keshsek = new() { Id = 4979, Name = "Keshsek", WikiUrl = "https://wiki.guildwars.com/wiki/Keshsek" };
    public static readonly Npc MadSoul = new() { Id = 4979, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc OrrianSpiritGildran = new() { Id = 4979, Name = "Orrian Spirit Gildran", WikiUrl = "https://wiki.guildwars.com/wiki/Orrian_Spirit_Gildran" };
    public static readonly Npc OrrianSpiritMinos = new() { Id = 4979, Name = "Orrian Spirit Minos", WikiUrl = "https://wiki.guildwars.com/wiki/Orrian_Spirit_Minos" };
    public static readonly Npc Pehai = new() { Id = 4979, Name = "Pehai", WikiUrl = "https://wiki.guildwars.com/wiki/Pehai" };
    public static readonly Npc Rahmor = new() { Id = 4979, Name = "Rahmor", WikiUrl = "https://wiki.guildwars.com/wiki/Rahmor" };
    public static readonly Npc Rochor = new() { Id = 4979, Name = "Rochor", WikiUrl = "https://wiki.guildwars.com/wiki/Rochor" };
    public static readonly Npc Talneng = new() { Id = 4979, Name = "Talneng", WikiUrl = "https://wiki.guildwars.com/wiki/Talneng" };
    public static readonly Npc TormentedSoul = new() { Id = 4979, Name = "Tormented Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Tormented_Soul" };
    public static readonly Npc Tuor = new() { Id = 4979, Name = "Tuor", WikiUrl = "https://wiki.guildwars.com/wiki/Tuor" };
    public static readonly Npc ImperialGuardTiendi = new() { Id = 4980, Name = "Imperial Guard Tiendi", WikiUrl = "https://wiki.guildwars.com/wiki/Imperial_Guard_Tiendi" };
    public static readonly Npc JoyousSoul1 = new() { Id = 4980, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc Laninahk = new() { Id = 4980, Name = "Laninahk", WikiUrl = "https://wiki.guildwars.com/wiki/Laninahk" };
    public static readonly Npc MadSoul1 = new() { Id = 4980, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc Neersi = new() { Id = 4980, Name = "Neersi", WikiUrl = "https://wiki.guildwars.com/wiki/Neersi" };
    public static readonly Npc OrrianSpiritKandril = new() { Id = 4980, Name = "Orrian Spirit Kandril", WikiUrl = "https://wiki.guildwars.com/wiki/Orrian_Spirit_Kandril" };
    public static readonly Npc TormentedSoul1 = new() { Id = 4980, Name = "Tormented Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Tormented_Soul" };
    public static readonly Npc MadSoul2 = new() { Id = 4981, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc SecretKeeper = new() { Id = 4981, Name = "Secret Keeper", WikiUrl = "https://wiki.guildwars.com/wiki/Secret_Keeper" };
    public static readonly Npc SpiritofTruth = new() { Id = 4981, Name = "Spirit of Truth", WikiUrl = "https://wiki.guildwars.com/wiki/Spirit_of_Truth" };
    public static readonly Npc Churrazek = new() { Id = 4982, Name = "Churrazek", WikiUrl = "https://wiki.guildwars.com/wiki/Churrazek" };
    public static readonly Npc MadSoul3 = new() { Id = 4982, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc SecretKeeper1 = new() { Id = 4982, Name = "Secret Keeper", WikiUrl = "https://wiki.guildwars.com/wiki/Secret_Keeper" };
    public static readonly Npc Jutuk = new() { Id = 4984, Name = "Jutuk", WikiUrl = "https://wiki.guildwars.com/wiki/Jutuk" };
    public static readonly Npc TormentedSoul2 = new() { Id = 4985, Name = "Tormented Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Tormented_Soul" };
    public static readonly Npc Gunli = new() { Id = 4988, Name = "Gunli", WikiUrl = "https://wiki.guildwars.com/wiki/Gunli" };
    public static readonly Npc Mehpekanu = new() { Id = 4988, Name = "Mehpekanu", WikiUrl = "https://wiki.guildwars.com/wiki/Mehpekanu" };
    public static readonly Npc Fahsik = new() { Id = 4989, Name = "Fahsik", WikiUrl = "https://wiki.guildwars.com/wiki/Fahsik" };
    public static readonly Npc Makaum = new() { Id = 4989, Name = "Makaum", WikiUrl = "https://wiki.guildwars.com/wiki/Makaum" };
    public static readonly Npc StorageChest = new() { Id = 4997, Name = "Storage Chest", WikiUrl = "https://wiki.guildwars.com/wiki/Storage_Chest" };
    public static readonly Npc XunlaiChest1 = new() { Id = 4997, Name = "Xunlai Chest", WikiUrl = "https://wiki.guildwars.com/wiki/Xunlai_Chest" };
    public static readonly Npc ArchivistZisthus = new() { Id = 4998, Name = "Archivist Zisthus", WikiUrl = "https://wiki.guildwars.com/wiki/Archivist_Zisthus" };
    public static readonly Npc AurusTrevess = new() { Id = 4998, Name = "Aurus Trevess", WikiUrl = "https://wiki.guildwars.com/wiki/Aurus_Trevess" };
    public static readonly Npc CaptainJerazh = new() { Id = 4998, Name = "Captain Jerazh", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Jerazh" };
    public static readonly Npc CaptainSulahresh = new() { Id = 4998, Name = "Captain Sulahresh", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Sulahresh" };
    public static readonly Npc ChaplainPhyratyss = new() { Id = 4998, Name = "Chaplain Phyratyss", WikiUrl = "https://wiki.guildwars.com/wiki/Chaplain_Phyratyss" };
    public static readonly Npc CommanderThurnis = new() { Id = 4998, Name = "Commander Thurnis", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Thurnis" };
    public static readonly Npc ForgottenGuardian = new() { Id = 4998, Name = "Forgotten Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Guardian" };
    public static readonly Npc ForgottenKeeper = new() { Id = 4998, Name = "Forgotten Keeper", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Keeper" };
    public static readonly Npc ForgottenWarden = new() { Id = 4998, Name = "Forgotten Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Forgotten_Warden" };
    public static readonly Npc HighPriestZhellix = new() { Id = 4998, Name = "High Priest Zhellix", WikiUrl = "https://wiki.guildwars.com/wiki/High_Priest_Zhellix" };
    public static readonly Npc Hoju = new() { Id = 4998, Name = "Hoju", WikiUrl = "https://wiki.guildwars.com/wiki/Hoju" };
    public static readonly Npc KeeperofArmor = new() { Id = 4998, Name = "Keeper of Armor", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Armor" };
    public static readonly Npc KeeperofArms = new() { Id = 4998, Name = "Keeper of Arms", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Arms" };
    public static readonly Npc KeeperofBone = new() { Id = 4998, Name = "Keeper of Bone", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Bone" };
    public static readonly Npc KeeperofIllusion = new() { Id = 4998, Name = "Keeper of Illusion", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Illusion" };
    public static readonly Npc KeeperofLight = new() { Id = 4998, Name = "Keeper of Light", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Light" };
    public static readonly Npc KeeperofNature = new() { Id = 4998, Name = "Keeper of Nature", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Nature" };
    public static readonly Npc KeeperofSecrets = new() { Id = 4998, Name = "Keeper of Secrets", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Secrets" };
    public static readonly Npc KeeperofShadows = new() { Id = 4998, Name = "Keeper of Shadows", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Shadows" };
    public static readonly Npc KeeperofSpirits = new() { Id = 4998, Name = "Keeper of Spirits", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Spirits" };
    public static readonly Npc KeeperofSteel = new() { Id = 4998, Name = "Keeper of Steel", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Steel" };
    public static readonly Npc KeeperoftheElements = new() { Id = 4998, Name = "Keeper of the Elements", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_the_Elements" };
    public static readonly Npc KeeperoftheScythe = new() { Id = 4998, Name = "Keeper of the Scythe", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_the_Scythe" };
    public static readonly Npc KeeperoftheSpear = new() { Id = 4998, Name = "Keeper of the Spear", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_the_Spear" };
    public static readonly Npc KeeperShafoss = new() { Id = 4998, Name = "Keeper Shafoss", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_Shafoss" };
    public static readonly Npc KeeperSharissh = new() { Id = 4998, Name = "Keeper Sharissh", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_Sharissh" };
    public static readonly Npc Raukus = new() { Id = 4998, Name = "Raukus", WikiUrl = "https://wiki.guildwars.com/wiki/Raukus" };
    public static readonly Npc RelkysstheBroken = new() { Id = 4998, Name = "Relkyss the Broken", WikiUrl = "https://wiki.guildwars.com/wiki/Relkyss_the_Broken" };
    public static readonly Npc RunahsSilus = new() { Id = 4998, Name = "Runahs Silus", WikiUrl = "https://wiki.guildwars.com/wiki/Runahs_Silus" };
    public static readonly Npc Tekliss = new() { Id = 4998, Name = "Tekliss", WikiUrl = "https://wiki.guildwars.com/wiki/Tekliss" };
    public static readonly Npc Virashak = new() { Id = 4998, Name = "Virashak", WikiUrl = "https://wiki.guildwars.com/wiki/Virashak" };
    public static readonly Npc VisshRakissh = new() { Id = 4998, Name = "Vissh Rakissh", WikiUrl = "https://wiki.guildwars.com/wiki/Vissh_Rakissh" };
    public static readonly Npc Volatiss = new() { Id = 4998, Name = "Volatiss", WikiUrl = "https://wiki.guildwars.com/wiki/Volatiss" };
    public static readonly Npc Zendeht = new() { Id = 4998, Name = "Zendeht", WikiUrl = "https://wiki.guildwars.com/wiki/Zendeht" };
    public static readonly Npc Vialee = new() { Id = 4999, Name = "Vialee", WikiUrl = "https://wiki.guildwars.com/wiki/Vialee" };
    public static readonly Npc WanderingSoul = new() { Id = 4999, Name = "Wandering Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Wandering_Soul" };
    public static readonly Npc DynasticSpirit = new() { Id = 5001, Name = "Dynastic Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Dynastic_Spirit" };
    public static readonly Npc JoyousSoul2 = new() { Id = 5001, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc MadSoul4 = new() { Id = 5001, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc AscensionPilgrim = new() { Id = 5002, Name = "Ascension Pilgrim", WikiUrl = "https://wiki.guildwars.com/wiki/Ascension_Pilgrim" };
    public static readonly Npc DynasticSpirit1 = new() { Id = 5002, Name = "Dynastic Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Dynastic_Spirit" };
    public static readonly Npc JoyousSoul3 = new() { Id = 5002, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc LostSoulMission = new() { Id = 5002, Name = "Lost Soul Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Lost_Soul_Mission" };
    public static readonly Npc MadSoul5 = new() { Id = 5002, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc TormentedSoul3 = new() { Id = 5002, Name = "Tormented Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Tormented_Soul" };
    public static readonly Npc AscensionPilgrim1 = new() { Id = 5003, Name = "Ascension Pilgrim", WikiUrl = "https://wiki.guildwars.com/wiki/Ascension_Pilgrim" };
    public static readonly Npc JoyousSoul4 = new() { Id = 5003, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc MadSoul6 = new() { Id = 5003, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc Wanzel = new() { Id = 5003, Name = "Wanzel", WikiUrl = "https://wiki.guildwars.com/wiki/Wanzel" };
    public static readonly Npc JoyousSoul5 = new() { Id = 5004, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc AscensionPilgrim2 = new() { Id = 5005, Name = "Ascension Pilgrim", WikiUrl = "https://wiki.guildwars.com/wiki/Ascension_Pilgrim" };
    public static readonly Npc JoyousSoul6 = new() { Id = 5005, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc AscensionPilgrim3 = new() { Id = 5006, Name = "Ascension Pilgrim", WikiUrl = "https://wiki.guildwars.com/wiki/Ascension_Pilgrim" };
    public static readonly Npc JoyousSoul7 = new() { Id = 5006, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc MadSoul7 = new() { Id = 5006, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc JoyousSoul8 = new() { Id = 5007, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc AscensionPilgrim4 = new() { Id = 5008, Name = "Ascension Pilgrim", WikiUrl = "https://wiki.guildwars.com/wiki/Ascension_Pilgrim" };
    public static readonly Npc DynasticSpirit2 = new() { Id = 5008, Name = "Dynastic Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Dynastic_Spirit" };
    public static readonly Npc JoyousSoul9 = new() { Id = 5008, Name = "Joyous Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Joyous_Soul" };
    public static readonly Npc MadSoul8 = new() { Id = 5008, Name = "Mad Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_Soul" };
    public static readonly Npc TormentedSoul4 = new() { Id = 5008, Name = "Tormented Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Tormented_Soul" };
    public static readonly Npc CaptainYithlis = new() { Id = 5009, Name = "Captain Yithlis", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Yithlis" };
    public static readonly Npc GarfazSteelfur = new() { Id = 5010, Name = "Garfaz Steelfur", WikiUrl = "https://wiki.guildwars.com/wiki/Garfaz_Steelfur" };
    public static readonly Npc Igraine = new() { Id = 5011, Name = "Igraine", WikiUrl = "https://wiki.guildwars.com/wiki/Igraine" };
    public static readonly Npc TheLost = new() { Id = 5012, Name = "The Lost", WikiUrl = "https://wiki.guildwars.com/wiki/The_Lost" };
    public static readonly Npc ScoutAhktum = new() { Id = 5013, Name = "Scout Ahktum", WikiUrl = "https://wiki.guildwars.com/wiki/Scout_Ahktum" };
    public static readonly Npc Thenemi = new() { Id = 5014, Name = "Thenemi", WikiUrl = "https://wiki.guildwars.com/wiki/Thenemi" };
    public static readonly Npc LieutenantSilmok = new() { Id = 5015, Name = "Lieutenant Silmok", WikiUrl = "https://wiki.guildwars.com/wiki/Lieutenant_Silmok" };
    public static readonly Npc CommanderWerishakul = new() { Id = 5016, Name = "Commander Werishakul", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Werishakul" };
    public static readonly Npc EnsignCharehli = new() { Id = 5017, Name = "Ensign Charehli", WikiUrl = "https://wiki.guildwars.com/wiki/Ensign_Charehli" };
    public static readonly Npc EnsignLumi = new() { Id = 5018, Name = "Ensign Lumi", WikiUrl = "https://wiki.guildwars.com/wiki/Ensign_Lumi" };
    public static readonly Npc BosunMohrti = new() { Id = 5020, Name = "Bosun Mohrti", WikiUrl = "https://wiki.guildwars.com/wiki/Bosun_Mohrti" };
    public static readonly Npc AdmiralChiggen = new() { Id = 5021, Name = "Admiral Chiggen", WikiUrl = "https://wiki.guildwars.com/wiki/Admiral_Chiggen" };
    public static readonly Npc MidshipmanMorolah = new() { Id = 5022, Name = "Midshipman Morolah", WikiUrl = "https://wiki.guildwars.com/wiki/Midshipman_Morolah" };
    public static readonly Npc CommanderSehden = new() { Id = 5023, Name = "Commander Sehden", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Sehden" };
    public static readonly Npc RisehtheHarmless = new() { Id = 5024, Name = "Riseh the Harmless", WikiUrl = "https://wiki.guildwars.com/wiki/Riseh_the_Harmless" };
    public static readonly Npc CommanderWahli = new() { Id = 5025, Name = "Commander Wahli", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Wahli" };
    public static readonly Npc CaptainMhedi = new() { Id = 5026, Name = "Captain Mhedi", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Mhedi" };
    public static readonly Npc CaptainAlsin = new() { Id = 5027, Name = "Captain Alsin", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Alsin" };
    public static readonly Npc LieutenantShagu = new() { Id = 5028, Name = "Lieutenant Shagu", WikiUrl = "https://wiki.guildwars.com/wiki/Lieutenant_Shagu" };
    public static readonly Npc AdmiralKaya = new() { Id = 5029, Name = "Admiral Kaya", WikiUrl = "https://wiki.guildwars.com/wiki/Admiral_Kaya" };
    public static readonly Npc ArredsCrew = new() { Id = 5030, Name = "Arred’s Crew", WikiUrl = "https://wiki.guildwars.com/wiki/Arred's_Crew" };
    public static readonly Npc CorsairWizard = new() { Id = 5030, Name = "Corsair Wizard", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wizard" };
    public static readonly Npc Wianuyo = new() { Id = 5030, Name = "Wianuyo", WikiUrl = "https://wiki.guildwars.com/wiki/Wianuyo" };
    public static readonly Npc ArredsCrew1 = new() { Id = 5031, Name = "Arred’s Crew", WikiUrl = "https://wiki.guildwars.com/wiki/Arred's_Crew" };
    public static readonly Npc CorsairBlackhand = new() { Id = 5031, Name = "Corsair Blackhand", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Blackhand" };
    public static readonly Npc CorsairSeaman = new() { Id = 5031, Name = "Corsair Seaman", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Seaman" };
    public static readonly Npc SmugglerBlackhand = new() { Id = 5031, Name = "Smuggler Blackhand", WikiUrl = "https://wiki.guildwars.com/wiki/Smuggler_Blackhand" };
    public static readonly Npc CorsairCook = new() { Id = 5032, Name = "Corsair Cook", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Cook" };
    public static readonly Npc SmugglerWizard = new() { Id = 5032, Name = "Smuggler Wizard", WikiUrl = "https://wiki.guildwars.com/wiki/Smuggler_Wizard" };
    public static readonly Npc ArredsCrew2 = new() { Id = 5033, Name = "Arred’s Crew", WikiUrl = "https://wiki.guildwars.com/wiki/Arred's_Crew" };
    public static readonly Npc CorsairBosun = new() { Id = 5033, Name = "Corsair Bosun", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Bosun" };
    public static readonly Npc CorsairSeaman1 = new() { Id = 5033, Name = "Corsair Seaman", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Seaman" };
    public static readonly Npc SmugglerMedic = new() { Id = 5033, Name = "Smuggler Medic", WikiUrl = "https://wiki.guildwars.com/wiki/Smuggler_Medic" };
    public static readonly Npc ArredsCrew3 = new() { Id = 5034, Name = "Arred’s Crew", WikiUrl = "https://wiki.guildwars.com/wiki/Arred's_Crew" };
    public static readonly Npc CorsairCutthroat = new() { Id = 5034, Name = "Corsair Cutthroat", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Cutthroat" };
    public static readonly Npc CorsairSeaman2 = new() { Id = 5034, Name = "Corsair Seaman", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Seaman" };
    public static readonly Npc SmugglerThug = new() { Id = 5034, Name = "Smuggler Thug", WikiUrl = "https://wiki.guildwars.com/wiki/Smuggler_Thug" };
    public static readonly Npc SneakyCorsair = new() { Id = 5034, Name = "Sneaky Corsair", WikiUrl = "https://wiki.guildwars.com/wiki/Sneaky_Corsair" };
    public static readonly Npc ArredsCrew4 = new() { Id = 5037, Name = "Arred’s Crew", WikiUrl = "https://wiki.guildwars.com/wiki/Arred's_Crew" };
    public static readonly Npc CorsairRaider = new() { Id = 5037, Name = "Corsair Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Raider" };
    public static readonly Npc CorsairSeaman3 = new() { Id = 5037, Name = "Corsair Seaman", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Seaman" };
    public static readonly Npc SmugglerRaider = new() { Id = 5037, Name = "Smuggler Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Smuggler_Raider" };
    public static readonly Npc ArredsCrew5 = new() { Id = 5038, Name = "Arred’s Crew", WikiUrl = "https://wiki.guildwars.com/wiki/Arred's_Crew" };
    public static readonly Npc CorsairBerserker = new() { Id = 5038, Name = "Corsair Berserker", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Berserker" };
    public static readonly Npc CorsairSeaman4 = new() { Id = 5038, Name = "Corsair Seaman", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Seaman" };
    public static readonly Npc SmugglerBerserker = new() { Id = 5038, Name = "Smuggler Berserker", WikiUrl = "https://wiki.guildwars.com/wiki/Smuggler_Berserker" };
    public static readonly Npc ArredsCrew6 = new() { Id = 5039, Name = "Arred’s Crew", WikiUrl = "https://wiki.guildwars.com/wiki/Arred's_Crew" };
    public static readonly Npc CorsairCommandant = new() { Id = 5039, Name = "Corsair Commandant", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commandant" };
    public static readonly Npc CorsairSeaman5 = new() { Id = 5039, Name = "Corsair Seaman", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Seaman" };
    public static readonly Npc IronfistsEnvoy = new() { Id = 5039, Name = "Ironfist’s Envoy", WikiUrl = "https://wiki.guildwars.com/wiki/Ironfist's_Envoy" };
    public static readonly Npc CorsairMindReader = new() { Id = 5040, Name = "Corsair Mind Reader", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Mind_Reader" };
    public static readonly Npc CorsairTorturer = new() { Id = 5041, Name = "Corsair Torturer", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Torturer" };
    public static readonly Npc CorsairWindMaster = new() { Id = 5042, Name = "Corsair Wind Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wind_Master" };
    public static readonly Npc CorsairDoctor = new() { Id = 5043, Name = "Corsair Doctor", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Doctor" };
    public static readonly Npc CorsairWeaponsMaster = new() { Id = 5044, Name = "Corsair Weapons Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Weapons_Master" };
    public static readonly Npc CorsairLieutenant = new() { Id = 5045, Name = "Corsair Lieutenant", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Lieutenant" };
    public static readonly Npc CorsairGrappler = new() { Id = 5046, Name = "Corsair Grappler", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Grappler" };
    public static readonly Npc CorsairAdmiral = new() { Id = 5047, Name = "Corsair Admiral", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Admiral" };
    public static readonly Npc CorsairSeer = new() { Id = 5048, Name = "Corsair Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Seer" };
    public static readonly Npc CorsairFlogger = new() { Id = 5049, Name = "Corsair Flogger", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Flogger" };
    public static readonly Npc CorsairReefFinder = new() { Id = 5050, Name = "Corsair Reef Finder", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Reef_Finder" };
    public static readonly Npc CorsairMedic = new() { Id = 5051, Name = "Corsair Medic", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Medic" };
    public static readonly Npc CorsairThug = new() { Id = 5052, Name = "Corsair Thug", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Thug" };
    public static readonly Npc CorsairLookout = new() { Id = 5053, Name = "Corsair Lookout", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Lookout" };
    public static readonly Npc CorsairMarauder = new() { Id = 5054, Name = "Corsair Marauder", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Marauder" };
    public static readonly Npc CorsairCaptain = new() { Id = 5055, Name = "Corsair Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Captain" };
    public static readonly Npc CorsairWizard1 = new() { Id = 5056, Name = "Corsair Wizard", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wizard" };
    public static readonly Npc CorsairBlackhand1 = new() { Id = 5057, Name = "Corsair Blackhand", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Blackhand" };
    public static readonly Npc CorsairThug1 = new() { Id = 5057, Name = "Corsair Thug", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Thug" };
    public static readonly Npc CorsairCook1 = new() { Id = 5058, Name = "Corsair Cook", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Cook" };
    public static readonly Npc CorsairBosun1 = new() { Id = 5059, Name = "Corsair Bosun", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Bosun" };
    public static readonly Npc CorsairCutthroat1 = new() { Id = 5060, Name = "Corsair Cutthroat", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Cutthroat" };
    public static readonly Npc CorsairRaider1 = new() { Id = 5061, Name = "Corsair Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Raider" };
    public static readonly Npc CorsairBerserker1 = new() { Id = 5062, Name = "Corsair Berserker", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Berserker" };
    public static readonly Npc CorsairCommandant1 = new() { Id = 5063, Name = "Corsair Commandant", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commandant" };
    public static readonly Npc SuubaytheGreedy = new() { Id = 5063, Name = "Suubay the Greedy", WikiUrl = "https://wiki.guildwars.com/wiki/Suubay_the_Greedy" };
    public static readonly Npc WoundedCorsair = new() { Id = 5063, Name = "Wounded Corsair", WikiUrl = "https://wiki.guildwars.com/wiki/Wounded_Corsair" };
    public static readonly Npc CorsairMage = new() { Id = 5064, Name = "Corsair Mage", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Mage" };
    public static readonly Npc CorsairHealer = new() { Id = 5065, Name = "Corsair Healer", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Healer" };
    public static readonly Npc CorsairBuccaneer = new() { Id = 5066, Name = "Corsair Buccaneer", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Buccaneer" };
    public static readonly Npc CorsairSpotter = new() { Id = 5067, Name = "Corsair Spotter", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Spotter" };
    public static readonly Npc CorsairWizard2 = new() { Id = 5068, Name = "Corsair Wizard", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wizard" };
    public static readonly Npc JackofTruths = new() { Id = 5068, Name = "Jack of Truths", WikiUrl = "https://wiki.guildwars.com/wiki/Jack_of_Truths" };
    public static readonly Npc CorsairBlackhand2 = new() { Id = 5069, Name = "Corsair Blackhand", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Blackhand" };
    public static readonly Npc GatekeeperKahno = new() { Id = 5069, Name = "Gatekeeper Kahno", WikiUrl = "https://wiki.guildwars.com/wiki/Gatekeeper_Kahno" };
    public static readonly Npc Suwash = new() { Id = 5069, Name = "Suwash", WikiUrl = "https://wiki.guildwars.com/wiki/Suwash" };
    public static readonly Npc CorsairCook2 = new() { Id = 5070, Name = "Corsair Cook", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Cook" };
    public static readonly Npc UnluckySimon = new() { Id = 5070, Name = "Unlucky Simon", WikiUrl = "https://wiki.guildwars.com/wiki/Unlucky_Simon" };
    public static readonly Npc CorsairBosun2 = new() { Id = 5071, Name = "Corsair Bosun", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Bosun" };
    public static readonly Npc Dzaga = new() { Id = 5071, Name = "Dzaga", WikiUrl = "https://wiki.guildwars.com/wiki/Dzaga" };
    public static readonly Npc Smuggler = new() { Id = 5071, Name = "Smuggler", WikiUrl = "https://wiki.guildwars.com/wiki/Smuggler" };
    public static readonly Npc CongutheRed = new() { Id = 5072, Name = "Congu the Red", WikiUrl = "https://wiki.guildwars.com/wiki/Congu_the_Red" };
    public static readonly Npc CorsairCutthroat2 = new() { Id = 5072, Name = "Corsair Cutthroat", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Cutthroat" };
    public static readonly Npc FahijtheSmiles = new() { Id = 5072, Name = "Fahij the Smiles", WikiUrl = "https://wiki.guildwars.com/wiki/Fahij_the_Smiles" };
    public static readonly Npc FirstMateGunanu = new() { Id = 5072, Name = "First Mate Gunanu", WikiUrl = "https://wiki.guildwars.com/wiki/First_Mate_Gunanu" };
    public static readonly Npc Jehrono = new() { Id = 5072, Name = "Jehrono", WikiUrl = "https://wiki.guildwars.com/wiki/Jehrono" };
    public static readonly Npc JehronoMaterial = new() { Id = 5072, Name = "Jehrono Material", WikiUrl = "https://wiki.guildwars.com/wiki/Jehrono_Material" };
    public static readonly Npc SavageNunbe = new() { Id = 5072, Name = "Savage Nunbe", WikiUrl = "https://wiki.guildwars.com/wiki/Savage_Nunbe" };
    public static readonly Npc CorsairRaider2 = new() { Id = 5073, Name = "Corsair Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Raider" };
    public static readonly Npc IronhookHube = new() { Id = 5073, Name = "Ironhook Hube", WikiUrl = "https://wiki.guildwars.com/wiki/Ironhook_Hube" };
    public static readonly Npc Sakku = new() { Id = 5073, Name = "Sakku", WikiUrl = "https://wiki.guildwars.com/wiki/Sakku" };
    public static readonly Npc Smuggler1 = new() { Id = 5073, Name = "Smuggler", WikiUrl = "https://wiki.guildwars.com/wiki/Smuggler" };
    public static readonly Npc CorsairBerserker2 = new() { Id = 5074, Name = "Corsair Berserker", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Berserker" };
    public static readonly Npc OneEyedRugger = new() { Id = 5074, Name = "One-Eyed Rugger", WikiUrl = "https://wiki.guildwars.com/wiki/One-Eyed_Rugger" };
    public static readonly Npc CaptainBohseda = new() { Id = 5075, Name = "Captain Bohseda", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Bohseda" };
    public static readonly Npc CaptainMindhebeh = new() { Id = 5075, Name = "Captain Mindhebeh", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Mindhebeh" };
    public static readonly Npc CorsairCommandant2 = new() { Id = 5075, Name = "Corsair Commandant", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commandant" };
    public static readonly Npc HasahktheClever = new() { Id = 5075, Name = "Hasahk the Clever", WikiUrl = "https://wiki.guildwars.com/wiki/Hasahk_the_Clever" };
    public static readonly Npc HasahktheCleverMaterial = new() { Id = 5075, Name = "Hasahk the Clever Material", WikiUrl = "https://wiki.guildwars.com/wiki/Hasahk_the_Clever_Material" };
    public static readonly Npc HoguntheUnpredictable = new() { Id = 5075, Name = "Hogun the Unpredictable", WikiUrl = "https://wiki.guildwars.com/wiki/Hogun_the_Unpredictable" };
    public static readonly Npc RuthlessSevad = new() { Id = 5075, Name = "Ruthless Sevad", WikiUrl = "https://wiki.guildwars.com/wiki/Ruthless_Sevad" };
    public static readonly Npc CorsairSpy = new() { Id = 5076, Name = "Corsair Spy", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Spy" };
    public static readonly Npc CommanderWerishakul1 = new() { Id = 5078, Name = "Commander Werishakul", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Werishakul" };
    public static readonly Npc EnsignCharehli1 = new() { Id = 5079, Name = "Ensign Charehli", WikiUrl = "https://wiki.guildwars.com/wiki/Ensign_Charehli" };
    public static readonly Npc EnsignLumi1 = new() { Id = 5080, Name = "Ensign Lumi", WikiUrl = "https://wiki.guildwars.com/wiki/Ensign_Lumi" };
    public static readonly Npc ArredtheUnready = new() { Id = 5081, Name = "Arred the Unready", WikiUrl = "https://wiki.guildwars.com/wiki/Arred_the_Unready" };
    public static readonly Npc Ironfist = new() { Id = 5083, Name = "Ironfist", WikiUrl = "https://wiki.guildwars.com/wiki/Ironfist" };
    public static readonly Npc ShiftyLem = new() { Id = 5084, Name = "Shifty Lem", WikiUrl = "https://wiki.guildwars.com/wiki/Shifty_Lem" };
    public static readonly Npc JerobNoSpine = new() { Id = 5085, Name = "Jerob No-Spine", WikiUrl = "https://wiki.guildwars.com/wiki/Jerob_No-Spine" };
    public static readonly Npc ShahaitheCunning = new() { Id = 5086, Name = "Shahai the Cunning", WikiUrl = "https://wiki.guildwars.com/wiki/Shahai_the_Cunning" };
    public static readonly Npc CorsairRunner = new() { Id = 5087, Name = "Corsair Runner", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Runner" };
    public static readonly Npc KahlitheStiched = new() { Id = 5089, Name = "Kahli the Stiched", WikiUrl = "https://wiki.guildwars.com/wiki/Kahli_the_Stiched" };
    public static readonly Npc TheDarkBlade = new() { Id = 5090, Name = "The Dark Blade", WikiUrl = "https://wiki.guildwars.com/wiki/The_Dark_Blade" };
    public static readonly Npc CaptainBloodFarid = new() { Id = 5091, Name = "Captain Blood Farid", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Blood_Farid" };
    public static readonly Npc CursedSalihm = new() { Id = 5092, Name = "Cursed Salihm", WikiUrl = "https://wiki.guildwars.com/wiki/Cursed_Salihm" };
    public static readonly Npc SunspearVolunteer = new() { Id = 5093, Name = "Sunspear Volunteer", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Volunteer" };
    public static readonly Npc SunspearVolunteer1 = new() { Id = 5094, Name = "Sunspear Volunteer", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Volunteer" };
    public static readonly Npc MidshipmanBeraidun = new() { Id = 5096, Name = "Midshipman Beraidun", WikiUrl = "https://wiki.guildwars.com/wiki/Midshipman_Beraidun" };
    public static readonly Npc CaptainShehnahr = new() { Id = 5097, Name = "Captain Shehnahr", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Shehnahr" };
    public static readonly Npc CommanderBahreht = new() { Id = 5098, Name = "Commander Bahreht", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Bahreht" };
    public static readonly Npc EnsignJahan = new() { Id = 5099, Name = "Ensign Jahan", WikiUrl = "https://wiki.guildwars.com/wiki/Ensign_Jahan" };
    public static readonly Npc LieutenantMahrik = new() { Id = 5100, Name = "Lieutenant Mahrik", WikiUrl = "https://wiki.guildwars.com/wiki/Lieutenant_Mahrik" };
    public static readonly Npc CorsairGuildLord = new() { Id = 5101, Name = "Corsair Guild Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Guild_Lord" };
    public static readonly Npc CorsairGuildLord1 = new() { Id = 5102, Name = "Corsair Guild Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Guild_Lord" };
    public static readonly Npc CorsairWindMaster1 = new() { Id = 5105, Name = "Corsair Wind Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wind_Master" };
    public static readonly Npc CorsairWindMaster2 = new() { Id = 5106, Name = "Corsair Wind Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wind_Master" };
    public static readonly Npc CorsairWeaponsMaster1 = new() { Id = 5108, Name = "Corsair Weapons Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Weapons_Master" };
    public static readonly Npc CorsairWeaponsMaster2 = new() { Id = 5109, Name = "Corsair Weapons Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Weapons_Master" };
    public static readonly Npc CorsairLieutenant1 = new() { Id = 5111, Name = "Corsair Lieutenant", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Lieutenant" };
    public static readonly Npc CorsairWeaponsMaster3 = new() { Id = 5114, Name = "Corsair Weapons Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Weapons_Master" };
    public static readonly Npc CorsairTorturer1 = new() { Id = 5115, Name = "Corsair Torturer", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Torturer" };
    public static readonly Npc CorsairLieutenant2 = new() { Id = 5116, Name = "Corsair Lieutenant", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Lieutenant" };
    public static readonly Npc CorsairWindMaster3 = new() { Id = 5117, Name = "Corsair Wind Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wind_Master" };
    public static readonly Npc CorsairTorturer2 = new() { Id = 5118, Name = "Corsair Torturer", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Torturer" };
    public static readonly Npc CorsairDoctor1 = new() { Id = 5119, Name = "Corsair Doctor", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Doctor" };
    public static readonly Npc CorsairWeaponsMaster4 = new() { Id = 5120, Name = "Corsair Weapons Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Weapons_Master" };
    public static readonly Npc CorsairAdmiral1 = new() { Id = 5121, Name = "Corsair Admiral", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Admiral" };
    public static readonly Npc CorsairMindReader1 = new() { Id = 5122, Name = "Corsair Mind Reader", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Mind_Reader" };
    public static readonly Npc CorsairTorturer3 = new() { Id = 5123, Name = "Corsair Torturer", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Torturer" };
    public static readonly Npc CorsairWindMaster4 = new() { Id = 5124, Name = "Corsair Wind Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wind_Master" };
    public static readonly Npc CorsairDoctor2 = new() { Id = 5125, Name = "Corsair Doctor", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Doctor" };
    public static readonly Npc CorsairWindMaster5 = new() { Id = 5126, Name = "Corsair Wind Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wind_Master" };
    public static readonly Npc CorsairWeaponsMaster5 = new() { Id = 5127, Name = "Corsair Weapons Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Weapons_Master" };
    public static readonly Npc CorsairLieutenant3 = new() { Id = 5128, Name = "Corsair Lieutenant", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Lieutenant" };
    public static readonly Npc CorsairCommander = new() { Id = 5129, Name = "Corsair Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commander" };
    public static readonly Npc CorsairCommander1 = new() { Id = 5130, Name = "Corsair Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commander" };
    public static readonly Npc CorsairCommander2 = new() { Id = 5131, Name = "Corsair Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commander" };
    public static readonly Npc CorsairCommander3 = new() { Id = 5132, Name = "Corsair Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commander" };
    public static readonly Npc CorsairCommander4 = new() { Id = 5133, Name = "Corsair Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commander" };
    public static readonly Npc CorsairCommander5 = new() { Id = 5134, Name = "Corsair Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commander" };
    public static readonly Npc CorsairCommander6 = new() { Id = 5135, Name = "Corsair Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commander" };
    public static readonly Npc CorsairCommander7 = new() { Id = 5136, Name = "Corsair Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commander" };
    public static readonly Npc CorsairGhost = new() { Id = 5137, Name = "Corsair Ghost", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Ghost" };
    public static readonly Npc Abaddon = new() { Id = 5138, Name = "Abaddon", WikiUrl = "https://wiki.guildwars.com/wiki/Abaddon" };
    public static readonly Npc LordJadoth = new() { Id = 5140, Name = "Lord Jadoth", WikiUrl = "https://wiki.guildwars.com/wiki/Lord_Jadoth" };
    public static readonly Npc StygianLord = new() { Id = 5141, Name = "Stygian Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Lord" };
    public static readonly Npc StygianLord1 = new() { Id = 5142, Name = "Stygian Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Lord" };
    public static readonly Npc StygianLord2 = new() { Id = 5143, Name = "Stygian Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Lord" };
    public static readonly Npc StygianLord3 = new() { Id = 5144, Name = "Stygian Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Lord" };
    public static readonly Npc TheFury = new() { Id = 5145, Name = "The Fury", WikiUrl = "https://wiki.guildwars.com/wiki/The_Fury" };
    public static readonly Npc TheBlackBeastofArrgh = new() { Id = 5146, Name = "The Black Beast of Arrgh", WikiUrl = "https://wiki.guildwars.com/wiki/The_Black_Beast_of_Arrgh" };
    public static readonly Npc TheGreaterDarkness = new() { Id = 5147, Name = "The Greater Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/The_Greater_Darkness" };
    public static readonly Npc TheDarkness = new() { Id = 5148, Name = "The Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/The_Darkness" };
    public static readonly Npc ChillofDarkness = new() { Id = 5150, Name = "Chill of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Chill_of_Darkness" };
    public static readonly Npc FuryTitan = new() { Id = 5154, Name = "Fury Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Fury_Titan" };
    public static readonly Npc ShaunurtheDivine = new() { Id = 5155, Name = "Shaunur the Divine", WikiUrl = "https://wiki.guildwars.com/wiki/Shaunur_the_Divine" };
    public static readonly Npc TurepMakerofOrphans = new() { Id = 5156, Name = "Turep Maker of Orphans", WikiUrl = "https://wiki.guildwars.com/wiki/Turep_Maker_of_Orphans" };
    public static readonly Npc GuardianofKomalie = new() { Id = 5157, Name = "Guardian of Komalie", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_of_Komalie" };
    public static readonly Npc GuardianofKomalie1 = new() { Id = 5158, Name = "Guardian of Komalie", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_of_Komalie" };
    public static readonly Npc StygianUnderlord = new() { Id = 5160, Name = "Stygian Underlord", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Underlord" };
    public static readonly Npc MargoniteAnurKaya = new() { Id = 5162, Name = "Margonite Anur Kaya", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Kaya" };
    public static readonly Npc MargoniteAnurDabi = new() { Id = 5163, Name = "Margonite Anur Dabi", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Dabi" };
    public static readonly Npc MargoniteAnurSu = new() { Id = 5164, Name = "Margonite Anur Su", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Su" };
    public static readonly Npc MargoniteAnurKi = new() { Id = 5165, Name = "Margonite Anur Ki", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Ki" };
    public static readonly Npc MargoniteAnurVu = new() { Id = 5166, Name = "Margonite Anur Vu", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Vu" };
    public static readonly Npc MargoniteAnurTuk = new() { Id = 5167, Name = "Margonite Anur Tuk", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Tuk" };
    public static readonly Npc MargoniteAnurRuk = new() { Id = 5168, Name = "Margonite Anur Ruk", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Ruk" };
    public static readonly Npc MargoniteAnurRund = new() { Id = 5169, Name = "Margonite Anur Rund", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Rund" };
    public static readonly Npc MargoniteAnurMank = new() { Id = 5170, Name = "Margonite Anur Mank", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Anur_Mank" };
    public static readonly Npc StygianHunger = new() { Id = 5171, Name = "Stygian Hunger", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Hunger" };
    public static readonly Npc StygianBrute = new() { Id = 5172, Name = "Stygian Brute", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Brute" };
    public static readonly Npc StygianGolem = new() { Id = 5173, Name = "Stygian Golem", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Golem" };
    public static readonly Npc StygianHorror = new() { Id = 5174, Name = "Stygian Horror", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Horror" };
    public static readonly Npc StygianFiend = new() { Id = 5175, Name = "Stygian Fiend", WikiUrl = "https://wiki.guildwars.com/wiki/Stygian_Fiend" };
    public static readonly Npc MindTormentor = new() { Id = 5176, Name = "Mind Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Mind_Tormentor" };
    public static readonly Npc SoulTormentor = new() { Id = 5177, Name = "Soul Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Soul_Tormentor" };
    public static readonly Npc WaterTormentor = new() { Id = 5178, Name = "Water Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Tormentor" };
    public static readonly Npc HeartTormentor = new() { Id = 5179, Name = "Heart Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_Tormentor" };
    public static readonly Npc FleshTormentor = new() { Id = 5180, Name = "Flesh Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Flesh_Tormentor" };
    public static readonly Npc SpiritTormentor = new() { Id = 5181, Name = "Spirit Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Spirit_Tormentor" };
    public static readonly Npc EarthTormentor = new() { Id = 5182, Name = "Earth Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Earth_Tormentor" };
    public static readonly Npc SanityTormentor = new() { Id = 5183, Name = "Sanity Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Sanity_Tormentor" };
    public static readonly Npc ThoughtofDarkness = new() { Id = 5184, Name = "Thought of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Thought_of_Darkness" };
    public static readonly Npc ChillofDarkness1 = new() { Id = 5185, Name = "Chill of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Chill_of_Darkness" };
    public static readonly Npc ScourgeofDarkness = new() { Id = 5186, Name = "Scourge of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Scourge_of_Darkness" };
    public static readonly Npc ClawofDarkness = new() { Id = 5187, Name = "Claw of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Claw_of_Darkness" };
    public static readonly Npc WindofDarkness = new() { Id = 5188, Name = "Wind of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Wind_of_Darkness" };
    public static readonly Npc CurseofDarkness = new() { Id = 5189, Name = "Curse of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Curse_of_Darkness" };
    public static readonly Npc Abyssal = new() { Id = 5190, Name = "Abyssal", WikiUrl = "https://wiki.guildwars.com/wiki/Abyssal" };
    public static readonly Npc MiseryTitan = new() { Id = 5191, Name = "Misery Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Misery_Titan" };
    public static readonly Npc RageTitan = new() { Id = 5192, Name = "Rage Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Rage_Titan" };
    public static readonly Npc DementiaTitan = new() { Id = 5193, Name = "Dementia Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Dementia_Titan" };
    public static readonly Npc AnguishTitan = new() { Id = 5194, Name = "Anguish Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Anguish_Titan" };
    public static readonly Npc DespairTitan = new() { Id = 5195, Name = "Despair Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Despair_Titan" };
    public static readonly Npc RageTitan1 = new() { Id = 5197, Name = "Rage Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Rage_Titan" };
    public static readonly Npc DementiaTitan1 = new() { Id = 5198, Name = "Dementia Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Dementia_Titan" };
    public static readonly Npc DespairTitan1 = new() { Id = 5199, Name = "Despair Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Despair_Titan" };
    public static readonly Npc FuryTitan1 = new() { Id = 5199, Name = "Fury Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Fury_Titan" };
    public static readonly Npc MindTormentor1 = new() { Id = 5200, Name = "Mind Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Mind_Tormentor" };
    public static readonly Npc SoulTormentor1 = new() { Id = 5201, Name = "Soul Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Soul_Tormentor" };
    public static readonly Npc WaterTormentor1 = new() { Id = 5202, Name = "Water Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Tormentor" };
    public static readonly Npc HeartTormentor1 = new() { Id = 5203, Name = "Heart Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Heart_Tormentor" };
    public static readonly Npc FleshTormentor1 = new() { Id = 5204, Name = "Flesh Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Flesh_Tormentor" };
    public static readonly Npc SpiritTormentor1 = new() { Id = 5205, Name = "Spirit Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Spirit_Tormentor" };
    public static readonly Npc EarthTormentor1 = new() { Id = 5206, Name = "Earth Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Earth_Tormentor" };
    public static readonly Npc SanityTormentor1 = new() { Id = 5208, Name = "Sanity Tormentor", WikiUrl = "https://wiki.guildwars.com/wiki/Sanity_Tormentor" };
    public static readonly Npc TormentClaw1 = new() { Id = 5209, Name = "Torment Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Torment_Claw" };
    public static readonly Npc SmotheringTendrils = new() { Id = 5210, Name = "Smothering Tendrils", WikiUrl = "https://wiki.guildwars.com/wiki/Smothering_Tendrils" };
    public static readonly Npc TorturewebDryder = new() { Id = 5211, Name = "Tortureweb Dryder", WikiUrl = "https://wiki.guildwars.com/wiki/Tortureweb_Dryder" };
    public static readonly Npc GreaterDreamRider = new() { Id = 5212, Name = "Greater Dream Rider", WikiUrl = "https://wiki.guildwars.com/wiki/Greater_Dream_Rider" };
    public static readonly Npc GreaterDreamRider1 = new() { Id = 5213, Name = "Greater Dream Rider", WikiUrl = "https://wiki.guildwars.com/wiki/Greater_Dream_Rider" };
    public static readonly Npc AdeptofWhispers = new() { Id = 5214, Name = "Adept of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Whispers" };
    public static readonly Npc ArchivistofWhispers1 = new() { Id = 5214, Name = "Archivist of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Archivist_of_Whispers" };
    public static readonly Npc ArmorsmithofWhispers = new() { Id = 5214, Name = "Armorsmith of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Armorsmith_of_Whispers" };
    public static readonly Npc DeaconofWhispers = new() { Id = 5214, Name = "Deacon of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Deacon_of_Whispers" };
    public static readonly Npc KeeperofWhispers = new() { Id = 5214, Name = "Keeper of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Keeper_of_Whispers" };
    public static readonly Npc OrderofWhispers = new() { Id = 5214, Name = "Order of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Order_of_Whispers" };
    public static readonly Npc VoiceofWhispers = new() { Id = 5214, Name = "Voice of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Voice_of_Whispers" };
    public static readonly Npc WhispersInformant = new() { Id = 5214, Name = "Whispers Informant", WikiUrl = "https://wiki.guildwars.com/wiki/Whispers_Informant" };
    public static readonly Npc CaptainValkyss = new() { Id = 5217, Name = "Captain Valkyss", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Valkyss" };
    public static readonly Npc GeneralYendzarsh = new() { Id = 5217, Name = "General Yendzarsh", WikiUrl = "https://wiki.guildwars.com/wiki/General_Yendzarsh" };
    public static readonly Npc Silzesh = new() { Id = 5217, Name = "Silzesh", WikiUrl = "https://wiki.guildwars.com/wiki/Silzesh" };
    public static readonly Npc BindingGuardian = new() { Id = 5218, Name = "Binding Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Binding_Guardian" };
    public static readonly Npc LieutenantVanahk = new() { Id = 5219, Name = "Lieutenant Vanahk", WikiUrl = "https://wiki.guildwars.com/wiki/Lieutenant_Vanahk" };
    public static readonly Npc CorporalSuli = new() { Id = 5220, Name = "Corporal Suli", WikiUrl = "https://wiki.guildwars.com/wiki/Corporal_Suli" };
    public static readonly Npc CommanderSadiBelai = new() { Id = 5221, Name = "Commander Sadi-Belai", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Sadi-Belai" };
    public static readonly Npc AdmiralKantoh = new() { Id = 5222, Name = "Admiral Kantoh", WikiUrl = "https://wiki.guildwars.com/wiki/Admiral_Kantoh" };
    public static readonly Npc CorporalArgon = new() { Id = 5223, Name = "Corporal Argon", WikiUrl = "https://wiki.guildwars.com/wiki/Corporal_Argon" };
    public static readonly Npc MajorJeahr = new() { Id = 5224, Name = "Major Jeahr", WikiUrl = "https://wiki.guildwars.com/wiki/Major_Jeahr" };
    public static readonly Npc ColonelChaklin = new() { Id = 5225, Name = "Colonel Chaklin", WikiUrl = "https://wiki.guildwars.com/wiki/Colonel_Chaklin" };
    public static readonly Npc ColonelCusto = new() { Id = 5226, Name = "Colonel Custo", WikiUrl = "https://wiki.guildwars.com/wiki/Colonel_Custo" };
    public static readonly Npc CorporalLuluh = new() { Id = 5227, Name = "Corporal Luluh", WikiUrl = "https://wiki.guildwars.com/wiki/Corporal_Luluh" };
    public static readonly Npc CaptainChichor = new() { Id = 5228, Name = "Captain Chichor", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Chichor" };
    public static readonly Npc LieutenantKayin = new() { Id = 5229, Name = "Lieutenant Kayin", WikiUrl = "https://wiki.guildwars.com/wiki/Lieutenant_Kayin" };
    public static readonly Npc CommanderNoss = new() { Id = 5230, Name = "Commander Noss", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Noss" };
    public static readonly Npc SergeantBehnwa = new() { Id = 5231, Name = "Sergeant Behnwa", WikiUrl = "https://wiki.guildwars.com/wiki/Sergeant_Behnwa" };
    public static readonly Npc AcolyteofBalthazar = new() { Id = 5232, Name = "Acolyte of Balthazar", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_of_Balthazar" };
    public static readonly Npc AcolyteofDwayna = new() { Id = 5233, Name = "Acolyte of Dwayna", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_of_Dwayna" };
    public static readonly Npc AcolyteofGrenth = new() { Id = 5234, Name = "Acolyte of Grenth", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_of_Grenth" };
    public static readonly Npc AcolyteofLyssa = new() { Id = 5235, Name = "Acolyte of Lyssa", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_of_Lyssa" };
    public static readonly Npc AcolyteofMelandru = new() { Id = 5236, Name = "Acolyte of Melandru", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_of_Melandru" };
    public static readonly Npc GeneralKahyet = new() { Id = 5237, Name = "General Kahyet", WikiUrl = "https://wiki.guildwars.com/wiki/General_Kahyet" };
    public static readonly Npc TaskmasterVanahk = new() { Id = 5238, Name = "Taskmaster Vanahk", WikiUrl = "https://wiki.guildwars.com/wiki/Taskmaster_Vanahk" };
    public static readonly Npc KournanPriest = new() { Id = 5239, Name = "Kournan Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Priest" };
    public static readonly Npc TaskmasterSadiBelai = new() { Id = 5239, Name = "Taskmaster Sadi-Belai", WikiUrl = "https://wiki.guildwars.com/wiki/Taskmaster_Sadi-Belai" };
    public static readonly Npc TaskmasterSuli = new() { Id = 5240, Name = "Taskmaster Suli", WikiUrl = "https://wiki.guildwars.com/wiki/Taskmaster_Suli" };
    public static readonly Npc OverseerBoktek = new() { Id = 5241, Name = "Overseer Boktek", WikiUrl = "https://wiki.guildwars.com/wiki/Overseer_Boktek" };
    public static readonly Npc OverseerHaubeh = new() { Id = 5242, Name = "Overseer Haubeh", WikiUrl = "https://wiki.guildwars.com/wiki/Overseer_Haubeh" };
    public static readonly Npc CaptainMwende = new() { Id = 5243, Name = "Captain Mwende", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Mwende" };
    public static readonly Npc CaptainKavaka = new() { Id = 5244, Name = "Captain Kavaka", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Kavaka" };
    public static readonly Npc CaptainDenduru = new() { Id = 5245, Name = "Captain Denduru", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Denduru" };
    public static readonly Npc CaptainLumanda = new() { Id = 5246, Name = "Captain Lumanda", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Lumanda" };
    public static readonly Npc CaptainKuruk = new() { Id = 5247, Name = "Captain Kuruk", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Kuruk" };
    public static readonly Npc CommanderKubeh = new() { Id = 5258, Name = "Commander Kubeh", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Kubeh" };
    public static readonly Npc LieutenantNali = new() { Id = 5259, Name = "Lieutenant Nali", WikiUrl = "https://wiki.guildwars.com/wiki/Lieutenant_Nali" };
    public static readonly Npc CaptainNebo = new() { Id = 5260, Name = "Captain Nebo", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Nebo" };
    public static readonly Npc ColonelKajo = new() { Id = 5262, Name = "Colonel Kajo", WikiUrl = "https://wiki.guildwars.com/wiki/Colonel_Kajo" };
    public static readonly Npc KournanEliteScribe = new() { Id = 5263, Name = "Kournan Elite Scribe", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Elite_Scribe" };
    public static readonly Npc KournanEliteGuard = new() { Id = 5264, Name = "Kournan Elite Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Elite_Guard" };
    public static readonly Npc KournanEliteZealot = new() { Id = 5265, Name = "Kournan Elite Zealot", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Elite_Zealot" };
    public static readonly Npc KournanEliteSpear = new() { Id = 5266, Name = "Kournan Elite Spear", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Elite_Spear" };
    public static readonly Npc CaptainBesuz = new() { Id = 5267, Name = "Captain Besuz", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Besuz" };
    public static readonly Npc Chirah = new() { Id = 5268, Name = "Chirah", WikiUrl = "https://wiki.guildwars.com/wiki/Chirah" };
    public static readonly Npc Wekesha = new() { Id = 5271, Name = "Wekesha", WikiUrl = "https://wiki.guildwars.com/wiki/Wekesha" };
    public static readonly Npc KournanTaskmaster = new() { Id = 5272, Name = "Kournan Taskmaster", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Taskmaster" };
    public static readonly Npc KournanSiegeEngineer = new() { Id = 5273, Name = "Kournan Siege Engineer", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Siege_Engineer" };
    public static readonly Npc Dehjah1 = new() { Id = 5274, Name = "Dehjah", WikiUrl = "https://wiki.guildwars.com/wiki/Dehjah" };
    public static readonly Npc CaretakerPalmor = new() { Id = 5275, Name = "Caretaker Palmor", WikiUrl = "https://wiki.guildwars.com/wiki/Caretaker_Palmor" };
    public static readonly Npc Kwaju = new() { Id = 5276, Name = "Kwaju", WikiUrl = "https://wiki.guildwars.com/wiki/Kwaju" };
    public static readonly Npc FreyatheFirm = new() { Id = 5278, Name = "Freya the Firm", WikiUrl = "https://wiki.guildwars.com/wiki/Freya_the_Firm" };
    public static readonly Npc KournanSpotter = new() { Id = 5278, Name = "Kournan Spotter", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Spotter" };
    public static readonly Npc MerkletheMuscular = new() { Id = 5278, Name = "Merkle the Muscular", WikiUrl = "https://wiki.guildwars.com/wiki/Merkle_the_Muscular" };
    public static readonly Npc VaughntheVenerable = new() { Id = 5278, Name = "Vaughn the Venerable", WikiUrl = "https://wiki.guildwars.com/wiki/Vaughn_the_Venerable" };
    public static readonly Npc ElderJonah = new() { Id = 5282, Name = "Elder Jonah", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Jonah" };
    public static readonly Npc KournanGuard = new() { Id = 5284, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc GuardLinko = new() { Id = 5285, Name = "Guard Linko", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Linko" };
    public static readonly Npc KournanGuard1 = new() { Id = 5285, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc ProphetVaresh = new() { Id = 5288, Name = "Prophet Varesh", WikiUrl = "https://wiki.guildwars.com/wiki/Prophet_Varesh" };
    public static readonly Npc CommanderVaresh = new() { Id = 5289, Name = "Commander Varesh", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Varesh" };
    public static readonly Npc VareshOssa = new() { Id = 5290, Name = "Varesh Ossa", WikiUrl = "https://wiki.guildwars.com/wiki/Varesh_Ossa" };
    public static readonly Npc Ahchin = new() { Id = 5297, Name = "Ahchin", WikiUrl = "https://wiki.guildwars.com/wiki/Ahchin" };
    public static readonly Npc KournanEngineer = new() { Id = 5301, Name = "Kournan Engineer", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Engineer" };
    public static readonly Npc KournanPriest1 = new() { Id = 5303, Name = "Kournan Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Priest" };
    public static readonly Npc AgentofWhispers = new() { Id = 5305, Name = "Agent of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Agent_of_Whispers" };
    public static readonly Npc Haibir = new() { Id = 5305, Name = "Haibir", WikiUrl = "https://wiki.guildwars.com/wiki/Haibir" };
    public static readonly Npc Kehtur = new() { Id = 5305, Name = "Kehtur", WikiUrl = "https://wiki.guildwars.com/wiki/Kehtur" };
    public static readonly Npc KournanGuard2 = new() { Id = 5305, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanScout = new() { Id = 5305, Name = "Kournan Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Scout" };
    public static readonly Npc KournanSoldier = new() { Id = 5305, Name = "Kournan Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Soldier" };
    public static readonly Npc KournanBowman = new() { Id = 5309, Name = "Kournan Bowman", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Bowman" };
    public static readonly Npc KournanCommander = new() { Id = 5313, Name = "Kournan Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Commander" };
    public static readonly Npc KournanPhalanx = new() { Id = 5313, Name = "Kournan Phalanx", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Phalanx" };
    public static readonly Npc ShipmentGuardCaptain = new() { Id = 5313, Name = "Shipment Guard Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Shipment_Guard_Captain" };
    public static readonly Npc KournanSeer = new() { Id = 5315, Name = "Kournan Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Seer" };
    public static readonly Npc KournanSeer1 = new() { Id = 5317, Name = "Kournan Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Seer" };
    public static readonly Npc KournanOppressor = new() { Id = 5319, Name = "Kournan Oppressor", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Oppressor" };
    public static readonly Npc KournanOppressor1 = new() { Id = 5321, Name = "Kournan Oppressor", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Oppressor" };
    public static readonly Npc KournanCommander1 = new() { Id = 5323, Name = "Kournan Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Commander" };
    public static readonly Npc KournanScribe = new() { Id = 5323, Name = "Kournan Scribe", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Scribe" };
    public static readonly Npc KournanScribe1 = new() { Id = 5325, Name = "Kournan Scribe", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Scribe" };
    public static readonly Npc KournanPriest2 = new() { Id = 5327, Name = "Kournan Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Priest" };
    public static readonly Npc KournanPriest3 = new() { Id = 5329, Name = "Kournan Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Priest" };
    public static readonly Npc KournanGuard3 = new() { Id = 5331, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanGuard4 = new() { Id = 5332, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanSoldier1 = new() { Id = 5332, Name = "Kournan Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Soldier" };
    public static readonly Npc KournanGuard5 = new() { Id = 5333, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanGuard6 = new() { Id = 5335, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanGuard7 = new() { Id = 5336, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanGuard8 = new() { Id = 5338, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanBowman1 = new() { Id = 5339, Name = "Kournan Bowman", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Bowman" };
    public static readonly Npc KournanBowman2 = new() { Id = 5341, Name = "Kournan Bowman", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Bowman" };
    public static readonly Npc KournanZealot = new() { Id = 5343, Name = "Kournan Zealot", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Zealot" };
    public static readonly Npc KournanZealot1 = new() { Id = 5345, Name = "Kournan Zealot", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Zealot" };
    public static readonly Npc GuardPostCommander = new() { Id = 5347, Name = "Guard Post Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Post_Commander" };
    public static readonly Npc KournanCaptain = new() { Id = 5347, Name = "Kournan Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Captain" };
    public static readonly Npc KournanPhalanx1 = new() { Id = 5347, Name = "Kournan Phalanx", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Phalanx" };
    public static readonly Npc KournanPhalanx2 = new() { Id = 5348, Name = "Kournan Phalanx", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Phalanx" };
    public static readonly Npc KournanFieldCommander = new() { Id = 5349, Name = "Kournan Field Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Field_Commander" };
    public static readonly Npc KournanPhalanx3 = new() { Id = 5350, Name = "Kournan Phalanx", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Phalanx" };
    public static readonly Npc KournanGuard9 = new() { Id = 5352, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanChild = new() { Id = 5358, Name = "Kournan Child", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Child" };
    public static readonly Npc Nerwa = new() { Id = 5358, Name = "Nerwa", WikiUrl = "https://wiki.guildwars.com/wiki/Nerwa" };
    public static readonly Npc KournanChild1 = new() { Id = 5359, Name = "Kournan Child", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Child" };
    public static readonly Npc Bahldasareh = new() { Id = 5360, Name = "Bahldasareh", WikiUrl = "https://wiki.guildwars.com/wiki/Bahldasareh" };
    public static readonly Npc EstateGuardRikesh = new() { Id = 5360, Name = "Estate Guard Rikesh", WikiUrl = "https://wiki.guildwars.com/wiki/Estate_Guard_Rikesh" };
    public static readonly Npc JailerGahanni = new() { Id = 5360, Name = "Jailer Gahanni", WikiUrl = "https://wiki.guildwars.com/wiki/Jailer_Gahanni" };
    public static readonly Npc KournanGuard10 = new() { Id = 5360, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanSoldier2 = new() { Id = 5360, Name = "Kournan Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Soldier" };
    public static readonly Npc Maho = new() { Id = 5360, Name = "Maho", WikiUrl = "https://wiki.guildwars.com/wiki/Maho" };
    public static readonly Npc KournanGuard11 = new() { Id = 5361, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanSoldier3 = new() { Id = 5361, Name = "Kournan Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Soldier" };
    public static readonly Npc CaptainMehhan = new() { Id = 5362, Name = "Captain Mehhan", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Mehhan" };
    public static readonly Npc CaptainNahnkos = new() { Id = 5362, Name = "Captain Nahnkos", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Nahnkos" };
    public static readonly Npc GuardCaptainKahturin = new() { Id = 5362, Name = "Guard Captain Kahturin", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Captain_Kahturin" };
    public static readonly Npc KournanCaptain1 = new() { Id = 5362, Name = "Kournan Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Captain" };
    public static readonly Npc PrisonKeeperShelkesh = new() { Id = 5362, Name = "Prison Keeper Shelkesh", WikiUrl = "https://wiki.guildwars.com/wiki/Prison_Keeper_Shelkesh" };
    public static readonly Npc SunspearCaptainMission = new() { Id = 5362, Name = "Sunspear Captain Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Captain_Mission" };
    public static readonly Npc VaultMasterEijah = new() { Id = 5362, Name = "Vault Master Eijah", WikiUrl = "https://wiki.guildwars.com/wiki/Vault_Master_Eijah" };
    public static readonly Npc CounselorRahburt = new() { Id = 5366, Name = "Counselor Rahburt", WikiUrl = "https://wiki.guildwars.com/wiki/Counselor_Rahburt" };
    public static readonly Npc KournanGuard12 = new() { Id = 5366, Name = "Kournan Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Guard" };
    public static readonly Npc KournanSoldier4 = new() { Id = 5366, Name = "Kournan Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Soldier" };
    public static readonly Npc EmissaryDajmir = new() { Id = 5369, Name = "Emissary Dajmir", WikiUrl = "https://wiki.guildwars.com/wiki/Emissary_Dajmir" };
    public static readonly Npc GerahladMahkfahlan = new() { Id = 5369, Name = "Gerahlad Mahkfahlan", WikiUrl = "https://wiki.guildwars.com/wiki/Gerahlad_Mahkfahlan" };
    public static readonly Npc Iasha = new() { Id = 5369, Name = "Iasha", WikiUrl = "https://wiki.guildwars.com/wiki/Iasha" };
    public static readonly Npc KournanNoble = new() { Id = 5369, Name = "Kournan Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Noble" };
    public static readonly Npc Medando = new() { Id = 5369, Name = "Medando", WikiUrl = "https://wiki.guildwars.com/wiki/Medando" };
    public static readonly Npc StefantheSonorous = new() { Id = 5369, Name = "Stefan the Sonorous", WikiUrl = "https://wiki.guildwars.com/wiki/Stefan_the_Sonorous" };
    public static readonly Npc Vohwash = new() { Id = 5369, Name = "Vohwash", WikiUrl = "https://wiki.guildwars.com/wiki/Vohwash" };
    public static readonly Npc Wedende = new() { Id = 5369, Name = "Wedende", WikiUrl = "https://wiki.guildwars.com/wiki/Wedende" };
    public static readonly Npc ZudashDejarin = new() { Id = 5369, Name = "Zudash Dejarin", WikiUrl = "https://wiki.guildwars.com/wiki/Zudash_Dejarin" };
    public static readonly Npc KournanNoble1 = new() { Id = 5370, Name = "Kournan Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Noble" };
    public static readonly Npc Varesh = new() { Id = 5370, Name = "Varesh", WikiUrl = "https://wiki.guildwars.com/wiki/Varesh" };
    public static readonly Npc EngineerTosi = new() { Id = 5371, Name = "Engineer Tosi", WikiUrl = "https://wiki.guildwars.com/wiki/Engineer_Tosi" };
    public static readonly Npc FarmerGorkan = new() { Id = 5371, Name = "Farmer Gorkan", WikiUrl = "https://wiki.guildwars.com/wiki/Farmer_Gorkan" };
    public static readonly Npc Fumeai = new() { Id = 5371, Name = "Fumeai", WikiUrl = "https://wiki.guildwars.com/wiki/Fumeai" };
    public static readonly Npc HerbalistMakala = new() { Id = 5371, Name = "Herbalist Makala", WikiUrl = "https://wiki.guildwars.com/wiki/Herbalist_Makala" };
    public static readonly Npc HerbalistUmjabwe = new() { Id = 5371, Name = "Herbalist Umjabwe", WikiUrl = "https://wiki.guildwars.com/wiki/Herbalist_Umjabwe" };
    public static readonly Npc KournanPeasant = new() { Id = 5371, Name = "Kournan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Peasant" };
    public static readonly Npc Mubata = new() { Id = 5371, Name = "Mubata", WikiUrl = "https://wiki.guildwars.com/wiki/Mubata" };
    public static readonly Npc Mubata1 = new() { Id = 5371, Name = "Mubata", WikiUrl = "https://wiki.guildwars.com/wiki/Mubata" };
    public static readonly Npc Nihanu = new() { Id = 5371, Name = "Nihanu", WikiUrl = "https://wiki.guildwars.com/wiki/Nihanu" };
    public static readonly Npc SukohtJahrevit = new() { Id = 5371, Name = "Sukoht Jahrevit", WikiUrl = "https://wiki.guildwars.com/wiki/Sukoht_Jahrevit" };
    public static readonly Npc BehnotuSupehnahn = new() { Id = 5372, Name = "Behnotu Supehnahn", WikiUrl = "https://wiki.guildwars.com/wiki/Behnotu_Supehnahn" };
    public static readonly Npc DockmasterDimedeh = new() { Id = 5372, Name = "Dockmaster Dimedeh", WikiUrl = "https://wiki.guildwars.com/wiki/Dockmaster_Dimedeh" };
    public static readonly Npc FarmerOrjok = new() { Id = 5372, Name = "Farmer Orjok", WikiUrl = "https://wiki.guildwars.com/wiki/Farmer_Orjok" };
    public static readonly Npc HerdsmanZekanu = new() { Id = 5372, Name = "Herdsman Zekanu", WikiUrl = "https://wiki.guildwars.com/wiki/Herdsman_Zekanu" };
    public static readonly Npc KournanPeasant1 = new() { Id = 5372, Name = "Kournan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Peasant" };
    public static readonly Npc GuardsmanBahsi = new() { Id = 5373, Name = "Guardsman Bahsi", WikiUrl = "https://wiki.guildwars.com/wiki/Guardsman_Bahsi" };
    public static readonly Npc HerdsmanMehnosi = new() { Id = 5373, Name = "Herdsman Mehnosi", WikiUrl = "https://wiki.guildwars.com/wiki/Herdsman_Mehnosi" };
    public static readonly Npc Kahan = new() { Id = 5373, Name = "Kahan", WikiUrl = "https://wiki.guildwars.com/wiki/Kahan" };
    public static readonly Npc KournanPeasant2 = new() { Id = 5373, Name = "Kournan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Peasant" };
    public static readonly Npc DreamerRaja = new() { Id = 5374, Name = "Dreamer Raja", WikiUrl = "https://wiki.guildwars.com/wiki/Dreamer_Raja" };
    public static readonly Npc ElderIsma = new() { Id = 5374, Name = "Elder Isma", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Isma" };
    public static readonly Npc Janeera = new() { Id = 5374, Name = "Janeera", WikiUrl = "https://wiki.guildwars.com/wiki/Janeera" };
    public static readonly Npc KournanPeasant3 = new() { Id = 5374, Name = "Kournan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Peasant" };
    public static readonly Npc Kwesi = new() { Id = 5374, Name = "Kwesi", WikiUrl = "https://wiki.guildwars.com/wiki/Kwesi" };
    public static readonly Npc Ossjo = new() { Id = 5374, Name = "Ossjo", WikiUrl = "https://wiki.guildwars.com/wiki/Ossjo" };
    public static readonly Npc TohmahsSukobehr = new() { Id = 5374, Name = "Tohmahs Sukobehr", WikiUrl = "https://wiki.guildwars.com/wiki/Tohmahs_Sukobehr" };
    public static readonly Npc Helenah = new() { Id = 5375, Name = "Helenah", WikiUrl = "https://wiki.guildwars.com/wiki/Helenah" };
    public static readonly Npc Kazleht = new() { Id = 5375, Name = "Kazleht", WikiUrl = "https://wiki.guildwars.com/wiki/Kazleht" };
    public static readonly Npc KournanPeasant4 = new() { Id = 5375, Name = "Kournan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Peasant" };
    public static readonly Npc Pehtigam = new() { Id = 5375, Name = "Pehtigam", WikiUrl = "https://wiki.guildwars.com/wiki/Pehtigam" };
    public static readonly Npc DreamerHahla = new() { Id = 5376, Name = "Dreamer Hahla", WikiUrl = "https://wiki.guildwars.com/wiki/Dreamer_Hahla" };
    public static readonly Npc Dzawan = new() { Id = 5376, Name = "Dzawan", WikiUrl = "https://wiki.guildwars.com/wiki/Dzawan" };
    public static readonly Npc Dzawan1 = new() { Id = 5376, Name = "Dzawan", WikiUrl = "https://wiki.guildwars.com/wiki/Dzawan" };
    public static readonly Npc KournanPeasant5 = new() { Id = 5376, Name = "Kournan Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Peasant" };
    public static readonly Npc Chuno = new() { Id = 5378, Name = "Chuno", WikiUrl = "https://wiki.guildwars.com/wiki/Chuno" };
    public static readonly Npc Sinbi = new() { Id = 5378, Name = "Sinbi", WikiUrl = "https://wiki.guildwars.com/wiki/Sinbi" };
    public static readonly Npc Sinbi1 = new() { Id = 5378, Name = "Sinbi", WikiUrl = "https://wiki.guildwars.com/wiki/Sinbi" };
    public static readonly Npc KournanNoble2 = new() { Id = 5379, Name = "Kournan Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Noble" };
    public static readonly Npc WanderingPriest = new() { Id = 5379, Name = "Wandering Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Wandering_Priest" };
    public static readonly Npc KournanNoble3 = new() { Id = 5380, Name = "Kournan Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Noble" };
    public static readonly Npc WanderingPriest1 = new() { Id = 5380, Name = "Wandering Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Wandering_Priest" };
    public static readonly Npc Burreh = new() { Id = 5382, Name = "Burreh", WikiUrl = "https://wiki.guildwars.com/wiki/Burreh" };
    public static readonly Npc Itenda = new() { Id = 5383, Name = "Itenda", WikiUrl = "https://wiki.guildwars.com/wiki/Itenda" };
    public static readonly Npc Jormar = new() { Id = 5383, Name = "Jormar", WikiUrl = "https://wiki.guildwars.com/wiki/Jormar" };
    public static readonly Npc Lormeh = new() { Id = 5383, Name = "Lormeh", WikiUrl = "https://wiki.guildwars.com/wiki/Lormeh" };
    public static readonly Npc Enbe = new() { Id = 5384, Name = "Enbe", WikiUrl = "https://wiki.guildwars.com/wiki/Enbe" };
    public static readonly Npc Ashnod = new() { Id = 5385, Name = "Ashnod", WikiUrl = "https://wiki.guildwars.com/wiki/Ashnod" };
    public static readonly Npc Dulon = new() { Id = 5385, Name = "Dulon", WikiUrl = "https://wiki.guildwars.com/wiki/Dulon" };
    public static readonly Npc Farim = new() { Id = 5385, Name = "Farim", WikiUrl = "https://wiki.guildwars.com/wiki/Farim" };
    public static readonly Npc Kahdeh = new() { Id = 5385, Name = "Kahdeh", WikiUrl = "https://wiki.guildwars.com/wiki/Kahdeh" };
    public static readonly Npc Lorossa = new() { Id = 5385, Name = "Lorossa", WikiUrl = "https://wiki.guildwars.com/wiki/Lorossa" };
    public static readonly Npc Nehtumbah = new() { Id = 5385, Name = "Nehtumbah", WikiUrl = "https://wiki.guildwars.com/wiki/Nehtumbah" };
    public static readonly Npc Riwar = new() { Id = 5385, Name = "Riwar", WikiUrl = "https://wiki.guildwars.com/wiki/Riwar" };
    public static readonly Npc Sahnbur = new() { Id = 5385, Name = "Sahnbur", WikiUrl = "https://wiki.guildwars.com/wiki/Sahnbur" };
    public static readonly Npc Totando = new() { Id = 5385, Name = "Totando", WikiUrl = "https://wiki.guildwars.com/wiki/Totando" };
    public static readonly Npc Turshi = new() { Id = 5385, Name = "Turshi", WikiUrl = "https://wiki.guildwars.com/wiki/Turshi" };
    public static readonly Npc Yerin = new() { Id = 5385, Name = "Yerin", WikiUrl = "https://wiki.guildwars.com/wiki/Yerin" };
    public static readonly Npc Ahleri = new() { Id = 5386, Name = "Ahleri", WikiUrl = "https://wiki.guildwars.com/wiki/Ahleri" };
    public static readonly Npc Dijahpo = new() { Id = 5386, Name = "Dijahpo", WikiUrl = "https://wiki.guildwars.com/wiki/Dijahpo" };
    public static readonly Npc Judila = new() { Id = 5386, Name = "Judila", WikiUrl = "https://wiki.guildwars.com/wiki/Judila" };
    public static readonly Npc Jurani = new() { Id = 5386, Name = "Jurani", WikiUrl = "https://wiki.guildwars.com/wiki/Jurani" };
    public static readonly Npc Koulaba = new() { Id = 5386, Name = "Koulaba", WikiUrl = "https://wiki.guildwars.com/wiki/Koulaba" };
    public static readonly Npc Lumesah = new() { Id = 5386, Name = "Lumesah", WikiUrl = "https://wiki.guildwars.com/wiki/Lumesah" };
    public static readonly Npc Perdahn = new() { Id = 5386, Name = "Perdahn", WikiUrl = "https://wiki.guildwars.com/wiki/Perdahn" };
    public static readonly Npc Ralaja = new() { Id = 5386, Name = "Ralaja", WikiUrl = "https://wiki.guildwars.com/wiki/Ralaja" };
    public static readonly Npc Raleva = new() { Id = 5386, Name = "Raleva", WikiUrl = "https://wiki.guildwars.com/wiki/Raleva" };
    public static readonly Npc Hadusi = new() { Id = 5387, Name = "Hadusi", WikiUrl = "https://wiki.guildwars.com/wiki/Hadusi" };
    public static readonly Npc Hahbe = new() { Id = 5387, Name = "Hahbe", WikiUrl = "https://wiki.guildwars.com/wiki/Hahbe" };
    public static readonly Npc Sende = new() { Id = 5387, Name = "Sende", WikiUrl = "https://wiki.guildwars.com/wiki/Sende" };
    public static readonly Npc Bolereh = new() { Id = 5388, Name = "Bolereh", WikiUrl = "https://wiki.guildwars.com/wiki/Bolereh" };
    public static readonly Npc Tehshan = new() { Id = 5388, Name = "Tehshan", WikiUrl = "https://wiki.guildwars.com/wiki/Tehshan" };
    public static readonly Npc Ahkessa = new() { Id = 5389, Name = "Ahkessa", WikiUrl = "https://wiki.guildwars.com/wiki/Ahkessa" };
    public static readonly Npc Benera = new() { Id = 5389, Name = "Benera", WikiUrl = "https://wiki.guildwars.com/wiki/Benera" };
    public static readonly Npc Benera1 = new() { Id = 5389, Name = "Benera", WikiUrl = "https://wiki.guildwars.com/wiki/Benera" };
    public static readonly Npc Odahn = new() { Id = 5389, Name = "Odahn", WikiUrl = "https://wiki.guildwars.com/wiki/Odahn" };
    public static readonly Npc Pohgri = new() { Id = 5389, Name = "Pohgri", WikiUrl = "https://wiki.guildwars.com/wiki/Pohgri" };
    public static readonly Npc Lutinu = new() { Id = 5390, Name = "Lutinu", WikiUrl = "https://wiki.guildwars.com/wiki/Lutinu" };
    public static readonly Npc Yahtu = new() { Id = 5391, Name = "Yahtu", WikiUrl = "https://wiki.guildwars.com/wiki/Yahtu" };
    public static readonly Npc Nehbusa = new() { Id = 5392, Name = "Nehbusa", WikiUrl = "https://wiki.guildwars.com/wiki/Nehbusa" };
    public static readonly Npc Sulumba = new() { Id = 5392, Name = "Sulumba", WikiUrl = "https://wiki.guildwars.com/wiki/Sulumba" };
    public static readonly Npc OrramMaterial = new() { Id = 5393, Name = "Orram Material", WikiUrl = "https://wiki.guildwars.com/wiki/Orram_Material" };
    public static readonly Npc FeriiMaterial = new() { Id = 5394, Name = "Ferii Material", WikiUrl = "https://wiki.guildwars.com/wiki/Ferii_Material" };
    public static readonly Npc StashehMaterial = new() { Id = 5394, Name = "Stasheh Material", WikiUrl = "https://wiki.guildwars.com/wiki/Stasheh_Material" };
    public static readonly Npc Megundeh = new() { Id = 5395, Name = "Megundeh", WikiUrl = "https://wiki.guildwars.com/wiki/Megundeh" };
    public static readonly Npc Megundeh1 = new() { Id = 5395, Name = "Megundeh", WikiUrl = "https://wiki.guildwars.com/wiki/Megundeh" };
    public static readonly Npc Fahpo = new() { Id = 5396, Name = "Fahpo", WikiUrl = "https://wiki.guildwars.com/wiki/Fahpo" };
    public static readonly Npc Shanka = new() { Id = 5396, Name = "Shanka", WikiUrl = "https://wiki.guildwars.com/wiki/Shanka" };
    public static readonly Npc Kerendu = new() { Id = 5397, Name = "Kerendu", WikiUrl = "https://wiki.guildwars.com/wiki/Kerendu" };
    public static readonly Npc KohnScroll = new() { Id = 5397, Name = "Kohn Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Kohn_Scroll" };
    public static readonly Npc HarfehlaScroll = new() { Id = 5398, Name = "Harfehla Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Harfehla_Scroll" };
    public static readonly Npc HarbingerofNightfall = new() { Id = 5399, Name = "Harbinger of Nightfall", WikiUrl = "https://wiki.guildwars.com/wiki/Harbinger_of_Nightfall" };
    public static readonly Npc Harbinger = new() { Id = 5400, Name = "Harbinger", WikiUrl = "https://wiki.guildwars.com/wiki/Harbinger" };
    public static readonly Npc TheHunger = new() { Id = 5401, Name = "The Hunger", WikiUrl = "https://wiki.guildwars.com/wiki/The_Hunger" };
    public static readonly Npc GeneralBayel = new() { Id = 5402, Name = "General Bayel", WikiUrl = "https://wiki.guildwars.com/wiki/General_Bayel" };
    public static readonly Npc HarbingerofNightfall1 = new() { Id = 5403, Name = "Harbinger of Nightfall", WikiUrl = "https://wiki.guildwars.com/wiki/Harbinger_of_Nightfall" };
    public static readonly Npc HarbingerofTwilight = new() { Id = 5404, Name = "Harbinger of Twilight", WikiUrl = "https://wiki.guildwars.com/wiki/Harbinger_of_Twilight" };
    public static readonly Npc HarbingerofTwilight1 = new() { Id = 5405, Name = "Harbinger of Twilight", WikiUrl = "https://wiki.guildwars.com/wiki/Harbinger_of_Twilight" };
    public static readonly Npc EmissaryofDhuum = new() { Id = 5406, Name = "Emissary of Dhuum", WikiUrl = "https://wiki.guildwars.com/wiki/Emissary_of_Dhuum" };
    public static readonly Npc ShadowofFear = new() { Id = 5406, Name = "Shadow of Fear", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_of_Fear" };
    public static readonly Npc BladeofCorruption = new() { Id = 5407, Name = "Blade of Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Blade_of_Corruption" };
    public static readonly Npc EmissaryofDhuum1 = new() { Id = 5408, Name = "Emissary of Dhuum", WikiUrl = "https://wiki.guildwars.com/wiki/Emissary_of_Dhuum" };
    public static readonly Npc DemonSpawn = new() { Id = 5419, Name = "Demon Spawn", WikiUrl = "https://wiki.guildwars.com/wiki/Demon_Spawn" };
    public static readonly Npc DemonicFortuneTeller = new() { Id = 5420, Name = "Demonic Fortune Teller", WikiUrl = "https://wiki.guildwars.com/wiki/Demonic_Fortune_Teller" };
    public static readonly Npc EmissaryofDhuum2 = new() { Id = 5421, Name = "Emissary of Dhuum", WikiUrl = "https://wiki.guildwars.com/wiki/Emissary_of_Dhuum" };
    public static readonly Npc Razakel = new() { Id = 5422, Name = "Razakel", WikiUrl = "https://wiki.guildwars.com/wiki/Razakel" };
    public static readonly Npc TorturewebDryder1 = new() { Id = 5423, Name = "Tortureweb Dryder", WikiUrl = "https://wiki.guildwars.com/wiki/Tortureweb_Dryder" };
    public static readonly Npc ShepherdofDementia = new() { Id = 5425, Name = "Shepherd of Dementia", WikiUrl = "https://wiki.guildwars.com/wiki/Shepherd_of_Dementia" };
    public static readonly Npc OnslaughtofTerror = new() { Id = 5426, Name = "Onslaught of Terror", WikiUrl = "https://wiki.guildwars.com/wiki/Onslaught_of_Terror" };
    public static readonly Npc OathofProfanity = new() { Id = 5427, Name = "Oath of Profanity", WikiUrl = "https://wiki.guildwars.com/wiki/Oath_of_Profanity" };
    public static readonly Npc StormofAnguish = new() { Id = 5432, Name = "Storm of Anguish", WikiUrl = "https://wiki.guildwars.com/wiki/Storm_of_Anguish" };
    public static readonly Npc FlameofFervor = new() { Id = 5433, Name = "Flame of Fervor", WikiUrl = "https://wiki.guildwars.com/wiki/Flame_of_Fervor" };
    public static readonly Npc SeedofSuffering = new() { Id = 5434, Name = "Seed of Suffering", WikiUrl = "https://wiki.guildwars.com/wiki/Seed_of_Suffering" };
    public static readonly Npc ShriekerofDread = new() { Id = 5435, Name = "Shrieker of Dread", WikiUrl = "https://wiki.guildwars.com/wiki/Shrieker_of_Dread" };
    public static readonly Npc VisionofDespair = new() { Id = 5436, Name = "Vision of Despair", WikiUrl = "https://wiki.guildwars.com/wiki/Vision_of_Despair" };
    public static readonly Npc BringerofDeceit = new() { Id = 5437, Name = "Bringer of Deceit", WikiUrl = "https://wiki.guildwars.com/wiki/Bringer_of_Deceit" };
    public static readonly Npc BearerofMisfortune = new() { Id = 5438, Name = "Bearer of Misfortune", WikiUrl = "https://wiki.guildwars.com/wiki/Bearer_of_Misfortune" };
    public static readonly Npc HeraldofNightmares = new() { Id = 5439, Name = "Herald of Nightmares", WikiUrl = "https://wiki.guildwars.com/wiki/Herald_of_Nightmares" };
    public static readonly Npc ShadowofFear1 = new() { Id = 5440, Name = "Shadow of Fear", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_of_Fear" };
    public static readonly Npc RainofTerror = new() { Id = 5441, Name = "Rain of Terror", WikiUrl = "https://wiki.guildwars.com/wiki/Rain_of_Terror" };
    public static readonly Npc WordofMadness = new() { Id = 5442, Name = "Word of Madness", WikiUrl = "https://wiki.guildwars.com/wiki/Word_of_Madness" };
    public static readonly Npc BladeofCorruption1 = new() { Id = 5443, Name = "Blade of Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Blade_of_Corruption" };
    public static readonly Npc ArmofInsanity = new() { Id = 5444, Name = "Arm of Insanity", WikiUrl = "https://wiki.guildwars.com/wiki/Arm_of_Insanity" };
    public static readonly Npc ScytheofChaos3 = new() { Id = 5445, Name = "Scythe of Chaos", WikiUrl = "https://wiki.guildwars.com/wiki/Scythe_of_Chaos" };
    public static readonly Npc SpearofTorment = new() { Id = 5446, Name = "Spear of Torment", WikiUrl = "https://wiki.guildwars.com/wiki/Spear_of_Torment" };
    public static readonly Npc HeraldofNightmares1 = new() { Id = 5447, Name = "Herald of Nightmares", WikiUrl = "https://wiki.guildwars.com/wiki/Herald_of_Nightmares" };
    public static readonly Npc ShadowofFear2 = new() { Id = 5448, Name = "Shadow of Fear", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_of_Fear" };
    public static readonly Npc RainofTerror1 = new() { Id = 5449, Name = "Rain of Terror", WikiUrl = "https://wiki.guildwars.com/wiki/Rain_of_Terror" };
    public static readonly Npc WordofMadness1 = new() { Id = 5450, Name = "Word of Madness", WikiUrl = "https://wiki.guildwars.com/wiki/Word_of_Madness" };
    public static readonly Npc BladeofCorruption2 = new() { Id = 5451, Name = "Blade of Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Blade_of_Corruption" };
    public static readonly Npc ArmofInsanity1 = new() { Id = 5452, Name = "Arm of Insanity", WikiUrl = "https://wiki.guildwars.com/wiki/Arm_of_Insanity" };
    public static readonly Npc ScytheofChaos4 = new() { Id = 5453, Name = "Scythe of Chaos", WikiUrl = "https://wiki.guildwars.com/wiki/Scythe_of_Chaos" };
    public static readonly Npc SpearofTorment1 = new() { Id = 5454, Name = "Spear of Torment", WikiUrl = "https://wiki.guildwars.com/wiki/Spear_of_Torment" };
    public static readonly Npc TormentClaw2 = new() { Id = 5456, Name = "Torment Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Torment_Claw" };
    public static readonly Npc TormentClaw3 = new() { Id = 5457, Name = "Torment Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Torment_Claw" };
    public static readonly Npc TormentClaw4 = new() { Id = 5458, Name = "Torment Claw", WikiUrl = "https://wiki.guildwars.com/wiki/Torment_Claw" };
    public static readonly Npc PreceptorZunark = new() { Id = 5459, Name = "Preceptor Zunark", WikiUrl = "https://wiki.guildwars.com/wiki/Preceptor_Zunark" };
    public static readonly Npc RebirtherJirath = new() { Id = 5461, Name = "Rebirther Jirath", WikiUrl = "https://wiki.guildwars.com/wiki/Rebirther_Jirath" };
    public static readonly Npc ScribeWensal = new() { Id = 5462, Name = "Scribe Wensal", WikiUrl = "https://wiki.guildwars.com/wiki/Scribe_Wensal" };
    public static readonly Npc PriestZeinZuu = new() { Id = 5463, Name = "Priest Zein Zuu", WikiUrl = "https://wiki.guildwars.com/wiki/Priest_Zein_Zuu" };
    public static readonly Npc ZealotSheoli = new() { Id = 5466, Name = "Zealot Sheoli", WikiUrl = "https://wiki.guildwars.com/wiki/Zealot_Sheoli" };
    public static readonly Npc BattlelordTurgar = new() { Id = 5467, Name = "Battlelord Turgar", WikiUrl = "https://wiki.guildwars.com/wiki/Battlelord_Turgar" };
    public static readonly Npc CommanderChutal = new() { Id = 5467, Name = "Commander Chutal", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Chutal" };
    public static readonly Npc MargoniteCleric = new() { Id = 5467, Name = "Margonite Cleric", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Cleric" };
    public static readonly Npc CuratorKali = new() { Id = 5468, Name = "Curator Kali", WikiUrl = "https://wiki.guildwars.com/wiki/Curator_Kali" };
    public static readonly Npc GeneralTirraj = new() { Id = 5468, Name = "General Tirraj", WikiUrl = "https://wiki.guildwars.com/wiki/General_Tirraj" };
    public static readonly Npc GeneralNimtak = new() { Id = 5469, Name = "General Nimtak", WikiUrl = "https://wiki.guildwars.com/wiki/General_Nimtak" };
    public static readonly Npc ChampionPuran = new() { Id = 5470, Name = "Champion Puran", WikiUrl = "https://wiki.guildwars.com/wiki/Champion_Puran" };
    public static readonly Npc GeneralDoriah = new() { Id = 5470, Name = "General Doriah", WikiUrl = "https://wiki.guildwars.com/wiki/General_Doriah" };
    public static readonly Npc GeneralKumtash = new() { Id = 5471, Name = "General Kumtash", WikiUrl = "https://wiki.guildwars.com/wiki/General_Kumtash" };
    public static readonly Npc TormentWeaver = new() { Id = 5473, Name = "Torment Weaver", WikiUrl = "https://wiki.guildwars.com/wiki/Torment_Weaver" };
    public static readonly Npc ZealousAmarantha = new() { Id = 5474, Name = "Zealous Amarantha", WikiUrl = "https://wiki.guildwars.com/wiki/Zealous_Amarantha" };
    public static readonly Npc AbaddonsAdjutant = new() { Id = 5479, Name = "Abaddon's Adjutant", WikiUrl = "https://wiki.guildwars.com/wiki/Abaddon's_Adjutant" };
    public static readonly Npc MargoniteHighPriest = new() { Id = 5480, Name = "Margonite High Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_High_Priest" };
    public static readonly Npc MargonitePatriarch = new() { Id = 5481, Name = "Margonite Patriarch", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Patriarch" };
    public static readonly Npc HordeofDarkness = new() { Id = 5482, Name = "Horde of Darkness", WikiUrl = "https://wiki.guildwars.com/wiki/Horde_of_Darkness" };
    public static readonly Npc UnboundEnergy = new() { Id = 5485, Name = "Unbound Energy", WikiUrl = "https://wiki.guildwars.com/wiki/Unbound_Energy" };
    public static readonly Npc TheBlasphemy = new() { Id = 5486, Name = "The Blasphemy", WikiUrl = "https://wiki.guildwars.com/wiki/The_Blasphemy" };
    public static readonly Npc Apostate = new() { Id = 5487, Name = "Apostate", WikiUrl = "https://wiki.guildwars.com/wiki/Apostate" };
    public static readonly Npc HausehtheDefiler = new() { Id = 5488, Name = "Hauseh the Defiler", WikiUrl = "https://wiki.guildwars.com/wiki/Hauseh_the_Defiler" };
    public static readonly Npc TanmahktheArcane = new() { Id = 5489, Name = "Tanmahk the Arcane", WikiUrl = "https://wiki.guildwars.com/wiki/Tanmahk_the_Arcane" };
    public static readonly Npc DupektheMighty = new() { Id = 5490, Name = "Dupek the Mighty", WikiUrl = "https://wiki.guildwars.com/wiki/Dupek_the_Mighty" };
    public static readonly Npc MargoniteWarlock = new() { Id = 5492, Name = "Margonite Warlock", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Warlock" };
    public static readonly Npc MargoniteSorcerer = new() { Id = 5493, Name = "Margonite Sorcerer", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Sorcerer" };
    public static readonly Npc MargoniteCleric1 = new() { Id = 5494, Name = "Margonite Cleric", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Cleric" };
    public static readonly Npc MargoniteExecutioner = new() { Id = 5495, Name = "Margonite Executioner", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Executioner" };
    public static readonly Npc MargoniteBowmaster = new() { Id = 5496, Name = "Margonite Bowmaster", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Bowmaster" };
    public static readonly Npc MargoniteReaper = new() { Id = 5497, Name = "Margonite Reaper", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Reaper" };
    public static readonly Npc MargonitePortalMage = new() { Id = 5499, Name = "Margonite Portal Mage", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Portal_Mage" };
    public static readonly Npc RualStealerofHope = new() { Id = 5509, Name = "Rual Stealer of Hope", WikiUrl = "https://wiki.guildwars.com/wiki/Rual_Stealer_of_Hope" };
    public static readonly Npc TureksintheDelegator = new() { Id = 5510, Name = "Tureksin the Delegator", WikiUrl = "https://wiki.guildwars.com/wiki/Tureksin_the_Delegator" };
    public static readonly Npc SaushalitheFrustrating = new() { Id = 5511, Name = "Saushali the Frustrating", WikiUrl = "https://wiki.guildwars.com/wiki/Saushali_the_Frustrating" };
    public static readonly Npc TaintheCorrupter = new() { Id = 5512, Name = "Tain the Corrupter", WikiUrl = "https://wiki.guildwars.com/wiki/Tain_the_Corrupter" };
    public static readonly Npc LushivahrtheInvoker = new() { Id = 5513, Name = "Lushivahr the Invoker", WikiUrl = "https://wiki.guildwars.com/wiki/Lushivahr_the_Invoker" };
    public static readonly Npc BriahntheChosen = new() { Id = 5514, Name = "Briahn the Chosen", WikiUrl = "https://wiki.guildwars.com/wiki/Briahn_the_Chosen" };
    public static readonly Npc GrabthartheOverbearing = new() { Id = 5515, Name = "Grabthar the Overbearing", WikiUrl = "https://wiki.guildwars.com/wiki/Grabthar_the_Overbearing" };
    public static readonly Npc ChimortheLightblooded = new() { Id = 5516, Name = "Chimor the Lightblooded", WikiUrl = "https://wiki.guildwars.com/wiki/Chimor_the_Lightblooded" };
    public static readonly Npc FahralontheZealous = new() { Id = 5518, Name = "Fahralon the Zealous", WikiUrl = "https://wiki.guildwars.com/wiki/Fahralon_the_Zealous" };
    public static readonly Npc WieshurtheInspiring = new() { Id = 5519, Name = "Wieshur the Inspiring", WikiUrl = "https://wiki.guildwars.com/wiki/Wieshur_the_Inspiring" };
    public static readonly Npc HautohthePilferer = new() { Id = 5520, Name = "Hautoh the Pilferer", WikiUrl = "https://wiki.guildwars.com/wiki/Hautoh_the_Pilferer" };
    public static readonly Npc MargoniteSeer = new() { Id = 5521, Name = "Margonite Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Seer" };
    public static readonly Npc MargoniteWarlock1 = new() { Id = 5522, Name = "Margonite Warlock", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Warlock" };
    public static readonly Npc MargoniteCleric2 = new() { Id = 5524, Name = "Margonite Cleric", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Cleric" };
    public static readonly Npc MargoniteExecutioner1 = new() { Id = 5525, Name = "Margonite Executioner", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Executioner" };
    public static readonly Npc MargoniteBowmaster1 = new() { Id = 5526, Name = "Margonite Bowmaster", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Bowmaster" };
    public static readonly Npc MargoniteScout = new() { Id = 5529, Name = "Margonite Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Scout" };
    public static readonly Npc MargoniteSeer1 = new() { Id = 5529, Name = "Margonite Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Seer" };
    public static readonly Npc MargoniteSouleater = new() { Id = 5529, Name = "Margonite Souleater", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Souleater" };
    public static readonly Npc MargoniteSouleater1 = new() { Id = 5530, Name = "Margonite Souleater", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Souleater" };
    public static readonly Npc MargoniteWarlock2 = new() { Id = 5530, Name = "Margonite Warlock", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Warlock" };
    public static readonly Npc MargoniteSorcerer1 = new() { Id = 5531, Name = "Margonite Sorcerer", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Sorcerer" };
    public static readonly Npc MargoniteSouleater2 = new() { Id = 5531, Name = "Margonite Souleater", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Souleater" };
    public static readonly Npc MargoniteCleric3 = new() { Id = 5532, Name = "Margonite Cleric", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Cleric" };
    public static readonly Npc MargoniteScout1 = new() { Id = 5532, Name = "Margonite Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Scout" };
    public static readonly Npc MargoniteSouleater3 = new() { Id = 5532, Name = "Margonite Souleater", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Souleater" };
    public static readonly Npc MargoniteExecutioner2 = new() { Id = 5533, Name = "Margonite Executioner", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Executioner" };
    public static readonly Npc MargoniteScout2 = new() { Id = 5533, Name = "Margonite Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Scout" };
    public static readonly Npc MargoniteSouleater4 = new() { Id = 5533, Name = "Margonite Souleater", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Souleater" };
    public static readonly Npc MargoniteStalker = new() { Id = 5533, Name = "Margonite Stalker", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Stalker" };
    public static readonly Npc MargoniteBowmaster2 = new() { Id = 5534, Name = "Margonite Bowmaster", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Bowmaster" };
    public static readonly Npc MargoniteScout3 = new() { Id = 5534, Name = "Margonite Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Scout" };
    public static readonly Npc MargoniteSouleater5 = new() { Id = 5534, Name = "Margonite Souleater", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Souleater" };
    public static readonly Npc MargoniteReaper1 = new() { Id = 5535, Name = "Margonite Reaper", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Reaper" };
    public static readonly Npc MargoniteScout4 = new() { Id = 5535, Name = "Margonite Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Scout" };
    public static readonly Npc MargoniteSouleater6 = new() { Id = 5535, Name = "Margonite Souleater", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Souleater" };
    public static readonly Npc MargoniteAscendant = new() { Id = 5536, Name = "Margonite Ascendant", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Ascendant" };
    public static readonly Npc MargoniteScout5 = new() { Id = 5536, Name = "Margonite Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Scout" };
    public static readonly Npc MargoniteSouleater7 = new() { Id = 5536, Name = "Margonite Souleater", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Souleater" };
    public static readonly Npc AncientUndead = new() { Id = 5537, Name = "Ancient Undead", WikiUrl = "https://wiki.guildwars.com/wiki/Ancient_Undead" };
    public static readonly Npc RestlessDead = new() { Id = 5537, Name = "Restless Dead", WikiUrl = "https://wiki.guildwars.com/wiki/Restless_Dead" };
    public static readonly Npc AncientUndead1 = new() { Id = 5538, Name = "Ancient Undead", WikiUrl = "https://wiki.guildwars.com/wiki/Ancient_Undead" };
    public static readonly Npc RelentlessCorpse = new() { Id = 5538, Name = "Relentless Corpse", WikiUrl = "https://wiki.guildwars.com/wiki/Relentless_Corpse" };
    public static readonly Npc AncientUndead2 = new() { Id = 5539, Name = "Ancient Undead", WikiUrl = "https://wiki.guildwars.com/wiki/Ancient_Undead" };
    public static readonly Npc AncientUndead3 = new() { Id = 5540, Name = "Ancient Undead", WikiUrl = "https://wiki.guildwars.com/wiki/Ancient_Undead" };
    public static readonly Npc DarehktheQuick = new() { Id = 5541, Name = "Darehk the Quick", WikiUrl = "https://wiki.guildwars.com/wiki/Darehk_the_Quick" };
    public static readonly Npc GhostlySunspear = new() { Id = 5541, Name = "Ghostly Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Sunspear" };
    public static readonly Npc GhostlySunspearCommander = new() { Id = 5541, Name = "Ghostly Sunspear Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Sunspear_Commander" };
    public static readonly Npc Kahdash = new() { Id = 5542, Name = "Kahdash", WikiUrl = "https://wiki.guildwars.com/wiki/Kahdash" };
    public static readonly Npc GhostlyScout = new() { Id = 5543, Name = "Ghostly Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Scout" };
    public static readonly Npc SunspearGhost = new() { Id = 5543, Name = "Sunspear Ghost", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Ghost" };
    public static readonly Npc GhostlyScout1 = new() { Id = 5544, Name = "Ghostly Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Scout" };
    public static readonly Npc SunspearGhost1 = new() { Id = 5544, Name = "Sunspear Ghost", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Ghost" };
    public static readonly Npc TorturedSunspear = new() { Id = 5544, Name = "Tortured Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Tortured_Sunspear" };
    public static readonly Npc FirstSpearDanah = new() { Id = 5545, Name = "First Spear Danah", WikiUrl = "https://wiki.guildwars.com/wiki/First_Spear_Danah" };
    public static readonly Npc FirstSpearJanah = new() { Id = 5545, Name = "First Spear Janah", WikiUrl = "https://wiki.guildwars.com/wiki/First_Spear_Janah" };
    public static readonly Npc SogolontheProtector = new() { Id = 5546, Name = "Sogolon the Protector", WikiUrl = "https://wiki.guildwars.com/wiki/Sogolon_the_Protector" };
    public static readonly Npc SpearmarshallKojolin = new() { Id = 5546, Name = "Spearmarshall Kojolin", WikiUrl = "https://wiki.guildwars.com/wiki/Spearmarshall_Kojolin" };
    public static readonly Npc ConciliatorKomah = new() { Id = 5547, Name = "Conciliator Komah", WikiUrl = "https://wiki.guildwars.com/wiki/Conciliator_Komah" };
    public static readonly Npc KojolintheConciliator = new() { Id = 5547, Name = "Kojolin the Conciliator", WikiUrl = "https://wiki.guildwars.com/wiki/Kojolin_the_Conciliator" };
    public static readonly Npc ShekosstheStony = new() { Id = 5548, Name = "Shekoss the Stony", WikiUrl = "https://wiki.guildwars.com/wiki/Shekoss_the_Stony" };
    public static readonly Npc MekirthePrismatic = new() { Id = 5549, Name = "Mekir the Prismatic", WikiUrl = "https://wiki.guildwars.com/wiki/Mekir_the_Prismatic" };
    public static readonly Npc ArdehtheQuick = new() { Id = 5550, Name = "Ardeh the Quick", WikiUrl = "https://wiki.guildwars.com/wiki/Ardeh_the_Quick" };
    public static readonly Npc EshwetheInsane = new() { Id = 5551, Name = "Eshwe the Insane", WikiUrl = "https://wiki.guildwars.com/wiki/Eshwe_the_Insane" };
    public static readonly Npc KehttheFierce = new() { Id = 5552, Name = "Keht the Fierce", WikiUrl = "https://wiki.guildwars.com/wiki/Keht_the_Fierce" };
    public static readonly Npc ArnehtheVigorous = new() { Id = 5553, Name = "Arneh the Vigorous", WikiUrl = "https://wiki.guildwars.com/wiki/Arneh_the_Vigorous" };
    public static readonly Npc HamlentheFallen = new() { Id = 5554, Name = "Hamlen the Fallen", WikiUrl = "https://wiki.guildwars.com/wiki/Hamlen_the_Fallen" };
    public static readonly Npc KoahmtheWeary = new() { Id = 5555, Name = "Koahm the Weary", WikiUrl = "https://wiki.guildwars.com/wiki/Koahm_the_Weary" };
    public static readonly Npc FondalztheSpiteful = new() { Id = 5556, Name = "Fondalz the Spiteful", WikiUrl = "https://wiki.guildwars.com/wiki/Fondalz_the_Spiteful" };
    public static readonly Npc NehjabtheParched = new() { Id = 5557, Name = "Nehjab the Parched", WikiUrl = "https://wiki.guildwars.com/wiki/Nehjab_the_Parched" };
    public static readonly Npc ShelkehtheHungry = new() { Id = 5558, Name = "Shelkeh the Hungry", WikiUrl = "https://wiki.guildwars.com/wiki/Shelkeh_the_Hungry" };
    public static readonly Npc AmireshthePious = new() { Id = 5559, Name = "Amiresh the Pious", WikiUrl = "https://wiki.guildwars.com/wiki/Amiresh_the_Pious" };
    public static readonly Npc ChurkehtheDefiant = new() { Id = 5560, Name = "Churkeh the Defiant", WikiUrl = "https://wiki.guildwars.com/wiki/Churkeh_the_Defiant" };
    public static readonly Npc BohdabitheDestructive = new() { Id = 5561, Name = "Bohdabi the Destructive", WikiUrl = "https://wiki.guildwars.com/wiki/Bohdabi_the_Destructive" };
    public static readonly Npc VahlentheSilent = new() { Id = 5562, Name = "Vahlen the Silent", WikiUrl = "https://wiki.guildwars.com/wiki/Vahlen_the_Silent" };
    public static readonly Npc AlemtheUnclean = new() { Id = 5563, Name = "Alem the Unclean", WikiUrl = "https://wiki.guildwars.com/wiki/Alem_the_Unclean" };
    public static readonly Npc Aijunundu = new() { Id = 5564, Name = "Aijunundu", WikiUrl = "https://wiki.guildwars.com/wiki/Aijunundu" };
    public static readonly Npc QueenAijundu = new() { Id = 5564, Name = "Queen Aijundu", WikiUrl = "https://wiki.guildwars.com/wiki/Queen_Aijundu" };
    public static readonly Npc AmindtheBitter = new() { Id = 5565, Name = "Amind the Bitter", WikiUrl = "https://wiki.guildwars.com/wiki/Amind_the_Bitter" };
    public static readonly Npc NehmaktheUnpleasant = new() { Id = 5566, Name = "Nehmak the Unpleasant", WikiUrl = "https://wiki.guildwars.com/wiki/Nehmak_the_Unpleasant" };
    public static readonly Npc AvahtheCrafty = new() { Id = 5567, Name = "Avah the Crafty", WikiUrl = "https://wiki.guildwars.com/wiki/Avah_the_Crafty" };
    public static readonly Npc ChundutheMeek = new() { Id = 5568, Name = "Chundu the Meek", WikiUrl = "https://wiki.guildwars.com/wiki/Chundu_the_Meek" };
    public static readonly Npc ChakehtheLonely = new() { Id = 5569, Name = "Chakeh the Lonely", WikiUrl = "https://wiki.guildwars.com/wiki/Chakeh_the_Lonely" };
    public static readonly Npc AwakenedThoughtLeech = new() { Id = 5570, Name = "Awakened Thought Leech", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Thought_Leech" };
    public static readonly Npc AwakenedDefiler = new() { Id = 5571, Name = "Awakened Defiler", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Defiler" };
    public static readonly Npc CarvenEffigy = new() { Id = 5572, Name = "Carven Effigy", WikiUrl = "https://wiki.guildwars.com/wiki/Carven_Effigy" };
    public static readonly Npc AwakenedAcolyte = new() { Id = 5573, Name = "Awakened Acolyte", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Acolyte" };
    public static readonly Npc AwakenedBlademaster = new() { Id = 5574, Name = "Awakened Blademaster", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Blademaster" };
    public static readonly Npc AwakenedGrayGiant = new() { Id = 5575, Name = "Awakened Gray Giant", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Gray_Giant" };
    public static readonly Npc AwakenedHead = new() { Id = 5576, Name = "Awakened Head", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Head" };
    public static readonly Npc AwakenedDuneCarver = new() { Id = 5578, Name = "Awakened Dune Carver", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Dune_Carver" };
    public static readonly Npc AwakenedCavalier = new() { Id = 5579, Name = "Awakened Cavalier", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Cavalier" };
    public static readonly Npc RebelliousGeneral = new() { Id = 5579, Name = "Rebellious General", WikiUrl = "https://wiki.guildwars.com/wiki/Rebellious_General" };
    public static readonly Npc SpiritofSeborhin = new() { Id = 5580, Name = "Spirit of Seborhin", WikiUrl = "https://wiki.guildwars.com/wiki/Spirit_of_Seborhin" };
    public static readonly Npc Thrall = new() { Id = 5582, Name = "Thrall", WikiUrl = "https://wiki.guildwars.com/wiki/Thrall" };
    public static readonly Npc OfficerLohru = new() { Id = 5583, Name = "Officer Lohru", WikiUrl = "https://wiki.guildwars.com/wiki/Officer_Lohru" };
    public static readonly Npc Thrall1 = new() { Id = 5583, Name = "Thrall", WikiUrl = "https://wiki.guildwars.com/wiki/Thrall" };
    public static readonly Npc Thrall2 = new() { Id = 5584, Name = "Thrall", WikiUrl = "https://wiki.guildwars.com/wiki/Thrall" };
    public static readonly Npc Thrall3 = new() { Id = 5585, Name = "Thrall", WikiUrl = "https://wiki.guildwars.com/wiki/Thrall" };
    public static readonly Npc Thrall4 = new() { Id = 5586, Name = "Thrall", WikiUrl = "https://wiki.guildwars.com/wiki/Thrall" };
    public static readonly Npc Thrall5 = new() { Id = 5587, Name = "Thrall", WikiUrl = "https://wiki.guildwars.com/wiki/Thrall" };
    public static readonly Npc Fahrankelon = new() { Id = 5588, Name = "Fahrankelon", WikiUrl = "https://wiki.guildwars.com/wiki/Fahrankelon" };
    public static readonly Npc Thrall6 = new() { Id = 5588, Name = "Thrall", WikiUrl = "https://wiki.guildwars.com/wiki/Thrall" };
    public static readonly Npc SlaveSpirit = new() { Id = 5589, Name = "Slave Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Slave_Spirit" };
    public static readonly Npc SlaveSpirit1 = new() { Id = 5590, Name = "Slave Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Slave_Spirit" };
    public static readonly Npc ScoutAhtok = new() { Id = 5591, Name = "Scout Ahtok", WikiUrl = "https://wiki.guildwars.com/wiki/Scout_Ahtok" };
    public static readonly Npc PrimevalKingJahnus = new() { Id = 5592, Name = "Primeval King Jahnus", WikiUrl = "https://wiki.guildwars.com/wiki/Primeval_King_Jahnus" };
    public static readonly Npc KarehOssa = new() { Id = 5593, Name = "Kareh Ossa", WikiUrl = "https://wiki.guildwars.com/wiki/Kareh_Ossa" };
    public static readonly Npc RoyalGuard2 = new() { Id = 5594, Name = "Royal Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Guard" };
    public static readonly Npc JununduYoung = new() { Id = 5595, Name = "Junundu Young", WikiUrl = "https://wiki.guildwars.com/wiki/Junundu_Young" };
    public static readonly Npc CommanderMosek = new() { Id = 5597, Name = "Commander Mosek", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Mosek" };
    public static readonly Npc CommanderGiturh = new() { Id = 5598, Name = "Commander Giturh", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Giturh" };
    public static readonly Npc CommanderLohgor = new() { Id = 5599, Name = "Commander Lohgor", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Lohgor" };
    public static readonly Npc CommanderYamji = new() { Id = 5600, Name = "Commander Yamji", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Yamji" };
    public static readonly Npc InfantrymanHareh = new() { Id = 5601, Name = "Infantryman Hareh", WikiUrl = "https://wiki.guildwars.com/wiki/Infantryman_Hareh" };
    public static readonly Npc GeneralFarrund = new() { Id = 5602, Name = "General Farrund", WikiUrl = "https://wiki.guildwars.com/wiki/General_Farrund" };
    public static readonly Npc PalawaJoko = new() { Id = 5603, Name = "Palawa Joko", WikiUrl = "https://wiki.guildwars.com/wiki/Palawa_Joko" };
    public static readonly Npc ElderSiegeWurm = new() { Id = 5606, Name = "Elder Siege Wurm", WikiUrl = "https://wiki.guildwars.com/wiki/Elder_Siege_Wurm" };
    public static readonly Npc Buuran = new() { Id = 5607, Name = "Buuran", WikiUrl = "https://wiki.guildwars.com/wiki/Buuran" };
    public static readonly Npc Chessa = new() { Id = 5607, Name = "Chessa", WikiUrl = "https://wiki.guildwars.com/wiki/Chessa" };
    public static readonly Npc Chinwe = new() { Id = 5607, Name = "Chinwe", WikiUrl = "https://wiki.guildwars.com/wiki/Chinwe" };
    public static readonly Npc Dayoesh = new() { Id = 5607, Name = "Dayoesh", WikiUrl = "https://wiki.guildwars.com/wiki/Dayoesh" };
    public static readonly Npc DynasticSpirit3 = new() { Id = 5607, Name = "Dynastic Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Dynastic_Spirit" };
    public static readonly Npc Ganji = new() { Id = 5607, Name = "Ganji", WikiUrl = "https://wiki.guildwars.com/wiki/Ganji" };
    public static readonly Npc Ghost = new() { Id = 5607, Name = "Ghost", WikiUrl = "https://wiki.guildwars.com/wiki/Ghost" };
    public static readonly Npc Johe = new() { Id = 5607, Name = "Johe", WikiUrl = "https://wiki.guildwars.com/wiki/Johe" };
    public static readonly Npc Juba = new() { Id = 5607, Name = "Juba", WikiUrl = "https://wiki.guildwars.com/wiki/Juba" };
    public static readonly Npc Kane = new() { Id = 5607, Name = "Kane", WikiUrl = "https://wiki.guildwars.com/wiki/Kane" };
    public static readonly Npc Krahm = new() { Id = 5607, Name = "Krahm", WikiUrl = "https://wiki.guildwars.com/wiki/Krahm" };
    public static readonly Npc Larano = new() { Id = 5607, Name = "Larano", WikiUrl = "https://wiki.guildwars.com/wiki/Larano" };
    public static readonly Npc MyrishtheSlave = new() { Id = 5607, Name = "Myrish the Slave", WikiUrl = "https://wiki.guildwars.com/wiki/Myrish_the_Slave" };
    public static readonly Npc Nikun = new() { Id = 5607, Name = "Nikun", WikiUrl = "https://wiki.guildwars.com/wiki/Nikun" };
    public static readonly Npc TormentedSoul5 = new() { Id = 5607, Name = "Tormented Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Tormented_Soul" };
    public static readonly Npc Charen = new() { Id = 5608, Name = "Charen", WikiUrl = "https://wiki.guildwars.com/wiki/Charen" };
    public static readonly Npc FreedSoul = new() { Id = 5608, Name = "Freed Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Freed_Soul" };
    public static readonly Npc Jehner = new() { Id = 5608, Name = "Jehner", WikiUrl = "https://wiki.guildwars.com/wiki/Jehner" };
    public static readonly Npc Kane1 = new() { Id = 5608, Name = "Kane", WikiUrl = "https://wiki.guildwars.com/wiki/Kane" };
    public static readonly Npc Allura = new() { Id = 5609, Name = "Allura", WikiUrl = "https://wiki.guildwars.com/wiki/Allura" };
    public static readonly Npc DynasticSpirit4 = new() { Id = 5609, Name = "Dynastic Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Dynastic_Spirit" };
    public static readonly Npc Ghost1 = new() { Id = 5609, Name = "Ghost", WikiUrl = "https://wiki.guildwars.com/wiki/Ghost" };
    public static readonly Npc GhostofSehwanu = new() { Id = 5609, Name = "Ghost of Sehwanu", WikiUrl = "https://wiki.guildwars.com/wiki/Ghost_of_Sehwanu" };
    public static readonly Npc Godaj = new() { Id = 5609, Name = "Godaj", WikiUrl = "https://wiki.guildwars.com/wiki/Godaj" };
    public static readonly Npc Shandara = new() { Id = 5609, Name = "Shandara", WikiUrl = "https://wiki.guildwars.com/wiki/Shandara" };
    public static readonly Npc Shanrah = new() { Id = 5609, Name = "Shanrah", WikiUrl = "https://wiki.guildwars.com/wiki/Shanrah" };
    public static readonly Npc TormentedSoul6 = new() { Id = 5609, Name = "Tormented Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Tormented_Soul" };
    public static readonly Npc FreedSoul1 = new() { Id = 5610, Name = "Freed Soul", WikiUrl = "https://wiki.guildwars.com/wiki/Freed_Soul" };
    public static readonly Npc CuratorRuras = new() { Id = 5611, Name = "Curator Ruras", WikiUrl = "https://wiki.guildwars.com/wiki/Curator_Ruras" };
    public static readonly Npc Dau = new() { Id = 5611, Name = "Dau", WikiUrl = "https://wiki.guildwars.com/wiki/Dau" };
    public static readonly Npc DynasticSpirit5 = new() { Id = 5611, Name = "Dynastic Spirit", WikiUrl = "https://wiki.guildwars.com/wiki/Dynastic_Spirit" };
    public static readonly Npc GhostlyPriest = new() { Id = 5611, Name = "Ghostly Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Priest" };
    public static readonly Npc Jehner1 = new() { Id = 5611, Name = "Jehner", WikiUrl = "https://wiki.guildwars.com/wiki/Jehner" };
    public static readonly Npc Shahin = new() { Id = 5611, Name = "Shahin", WikiUrl = "https://wiki.guildwars.com/wiki/Shahin" };
    public static readonly Npc Khendi = new() { Id = 5612, Name = "Khendi", WikiUrl = "https://wiki.guildwars.com/wiki/Khendi" };
    public static readonly Npc Ahn = new() { Id = 5613, Name = "Ahn", WikiUrl = "https://wiki.guildwars.com/wiki/Ahn" };
    public static readonly Npc Aisu = new() { Id = 5613, Name = "Aisu", WikiUrl = "https://wiki.guildwars.com/wiki/Aisu" };
    public static readonly Npc Kenmu = new() { Id = 5613, Name = "Kenmu", WikiUrl = "https://wiki.guildwars.com/wiki/Kenmu" };
    public static readonly Npc Padu = new() { Id = 5613, Name = "Padu", WikiUrl = "https://wiki.guildwars.com/wiki/Padu" };
    public static readonly Npc Ruricu = new() { Id = 5613, Name = "Ruricu", WikiUrl = "https://wiki.guildwars.com/wiki/Ruricu" };
    public static readonly Npc Tatau = new() { Id = 5613, Name = "Tatau", WikiUrl = "https://wiki.guildwars.com/wiki/Tatau" };
    public static readonly Npc Urlen = new() { Id = 5613, Name = "Urlen", WikiUrl = "https://wiki.guildwars.com/wiki/Urlen" };
    public static readonly Npc Yovel = new() { Id = 5613, Name = "Yovel", WikiUrl = "https://wiki.guildwars.com/wiki/Yovel" };
    public static readonly Npc Dahn = new() { Id = 5614, Name = "Dahn", WikiUrl = "https://wiki.guildwars.com/wiki/Dahn" };
    public static readonly Npc Yovel1 = new() { Id = 5614, Name = "Yovel", WikiUrl = "https://wiki.guildwars.com/wiki/Yovel" };
    public static readonly Npc Palmod = new() { Id = 5619, Name = "Palmod", WikiUrl = "https://wiki.guildwars.com/wiki/Palmod" };
    public static readonly Npc ZellMaterial = new() { Id = 5619, Name = "Zell Material", WikiUrl = "https://wiki.guildwars.com/wiki/Zell_Material" };
    public static readonly Npc Awata = new() { Id = 5620, Name = "Awata", WikiUrl = "https://wiki.guildwars.com/wiki/Awata" };
    public static readonly Npc BonesmithRokel = new() { Id = 5621, Name = "Bonesmith Rokel", WikiUrl = "https://wiki.guildwars.com/wiki/Bonesmith_Rokel" };
    public static readonly Npc Fanri = new() { Id = 5621, Name = "Fanri", WikiUrl = "https://wiki.guildwars.com/wiki/Fanri" };
    public static readonly Npc Zenbu = new() { Id = 5621, Name = "Zenbu", WikiUrl = "https://wiki.guildwars.com/wiki/Zenbu" };
    public static readonly Npc PriestKehmtut = new() { Id = 5622, Name = "Priest Kehmtut", WikiUrl = "https://wiki.guildwars.com/wiki/Priest_Kehmtut" };
    public static readonly Npc Jasek = new() { Id = 5624, Name = "Jasek", WikiUrl = "https://wiki.guildwars.com/wiki/Jasek" };
    public static readonly Npc Ehndu = new() { Id = 5626, Name = "Ehndu", WikiUrl = "https://wiki.guildwars.com/wiki/Ehndu" };
    public static readonly Npc AxshaiScroll = new() { Id = 5627, Name = "Axshai Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Axshai_Scroll" };
    public static readonly Npc LomehScroll = new() { Id = 5627, Name = "Lomeh Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Lomeh_Scroll" };
    public static readonly Npc AhndurizBohim = new() { Id = 5628, Name = "Ahnduriz Bohim", WikiUrl = "https://wiki.guildwars.com/wiki/Ahnduriz_Bohim" };
    public static readonly Npc Guard = new() { Id = 5628, Name = "Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Guard" };
    public static readonly Npc JohanGarham = new() { Id = 5628, Name = "Johan Garham", WikiUrl = "https://wiki.guildwars.com/wiki/Johan_Garham" };
    public static readonly Npc JonossNodahlon = new() { Id = 5628, Name = "Jonoss Nodahlon", WikiUrl = "https://wiki.guildwars.com/wiki/Jonoss_Nodahlon" };
    public static readonly Npc Kuwame = new() { Id = 5628, Name = "Kuwame", WikiUrl = "https://wiki.guildwars.com/wiki/Kuwame" };
    public static readonly Npc MikahlHintohn = new() { Id = 5628, Name = "Mikahl Hintohn", WikiUrl = "https://wiki.guildwars.com/wiki/Mikahl_Hintohn" };
    public static readonly Npc SergeantBokkun = new() { Id = 5628, Name = "Sergeant Bokkun", WikiUrl = "https://wiki.guildwars.com/wiki/Sergeant_Bokkun" };
    public static readonly Npc SergeantBolrob = new() { Id = 5628, Name = "Sergeant Bolrob", WikiUrl = "https://wiki.guildwars.com/wiki/Sergeant_Bolrob" };
    public static readonly Npc Siktur = new() { Id = 5628, Name = "Siktur", WikiUrl = "https://wiki.guildwars.com/wiki/Siktur" };
    public static readonly Npc VabbiGuard = new() { Id = 5628, Name = "Vabbi Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Guard" };
    public static readonly Npc VabbianGuard = new() { Id = 5628, Name = "Vabbian Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Guard" };
    public static readonly Npc VabbianScout = new() { Id = 5628, Name = "Vabbian Scout", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Scout" };
    public static readonly Npc VabbiGuard1 = new() { Id = 5629, Name = "Vabbi Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Guard" };
    public static readonly Npc RescueGuard = new() { Id = 5630, Name = "Rescue Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Rescue_Guard" };
    public static readonly Npc VabbianGuard1 = new() { Id = 5630, Name = "Vabbian Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Guard" };
    public static readonly Npc ButohtheBold = new() { Id = 5631, Name = "Butoh the Bold", WikiUrl = "https://wiki.guildwars.com/wiki/Butoh_the_Bold" };
    public static readonly Npc CaptainJafahni = new() { Id = 5631, Name = "Captain Jafahni", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Jafahni" };
    public static readonly Npc GahridDareshlar = new() { Id = 5631, Name = "Gahrid Dareshlar", WikiUrl = "https://wiki.guildwars.com/wiki/Gahrid_Dareshlar" };
    public static readonly Npc MahkJenshan = new() { Id = 5631, Name = "Mahk Jenshan", WikiUrl = "https://wiki.guildwars.com/wiki/Mahk_Jenshan" };
    public static readonly Npc PalaceGuard = new() { Id = 5631, Name = "Palace Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Palace_Guard" };
    public static readonly Npc SeborhinProtectorZuor = new() { Id = 5631, Name = "Seborhin Protector Zuor", WikiUrl = "https://wiki.guildwars.com/wiki/Seborhin_Protector_Zuor" };
    public static readonly Npc PalaceGuard1 = new() { Id = 5632, Name = "Palace Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Palace_Guard" };
    public static readonly Npc JennurKurahHatohn = new() { Id = 5633, Name = "Jennur Kurah Hatohn", WikiUrl = "https://wiki.guildwars.com/wiki/Jennur_Kurah_Hatohn" };
    public static readonly Npc LieutenantMurunda = new() { Id = 5633, Name = "Lieutenant Murunda", WikiUrl = "https://wiki.guildwars.com/wiki/Lieutenant_Murunda" };
    public static readonly Npc CommanderTanmod = new() { Id = 5635, Name = "Commander Tanmod", WikiUrl = "https://wiki.guildwars.com/wiki/Commander_Tanmod" };
    public static readonly Npc GeneralPoruk = new() { Id = 5635, Name = "General Poruk", WikiUrl = "https://wiki.guildwars.com/wiki/General_Poruk" };
    public static readonly Npc GuardCaptain = new() { Id = 5635, Name = "Guard Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Guard_Captain" };
    public static readonly Npc RoyalGuardBunda = new() { Id = 5635, Name = "Royal Guard Bunda", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Guard_Bunda" };
    public static readonly Npc RoyalGuardZendeh = new() { Id = 5635, Name = "Royal Guard Zendeh", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Guard_Zendeh" };
    public static readonly Npc VabbiGuardCaptain = new() { Id = 5635, Name = "Vabbi Guard Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Guard_Captain" };
    public static readonly Npc VabbianGuard2 = new() { Id = 5635, Name = "Vabbian Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Guard" };
    public static readonly Npc VabbiGuardCaptain1 = new() { Id = 5636, Name = "Vabbi Guard Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Guard_Captain" };
    public static readonly Npc CaptainPortir = new() { Id = 5637, Name = "Captain Portir", WikiUrl = "https://wiki.guildwars.com/wiki/Captain_Portir" };
    public static readonly Npc LieutenantMurunda1 = new() { Id = 5637, Name = "Lieutenant Murunda", WikiUrl = "https://wiki.guildwars.com/wiki/Lieutenant_Murunda" };
    public static readonly Npc VabbiGuardCaptain2 = new() { Id = 5637, Name = "Vabbi Guard Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Guard_Captain" };
    public static readonly Npc VabbianGuard3 = new() { Id = 5637, Name = "Vabbian Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Guard" };
    public static readonly Npc VabbiGuardCaptain3 = new() { Id = 5638, Name = "Vabbi Guard Captain", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Guard_Captain" };
    public static readonly Npc KahturinHovohden = new() { Id = 5641, Name = "Kahturin Hovohden", WikiUrl = "https://wiki.guildwars.com/wiki/Kahturin_Hovohden" };
    public static readonly Npc LohizHoput = new() { Id = 5641, Name = "Lohiz Hoput", WikiUrl = "https://wiki.guildwars.com/wiki/Lohiz_Hoput" };
    public static readonly Npc ScoutDehra = new() { Id = 5641, Name = "Scout Dehra", WikiUrl = "https://wiki.guildwars.com/wiki/Scout_Dehra" };
    public static readonly Npc VabbiGuard2 = new() { Id = 5641, Name = "Vabbi Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Guard" };
    public static readonly Npc VabbianGuard4 = new() { Id = 5641, Name = "Vabbian Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Guard" };
    public static readonly Npc VabbiGuard3 = new() { Id = 5642, Name = "Vabbi Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Guard" };
    public static readonly Npc VabbianGuard5 = new() { Id = 5643, Name = "Vabbian Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Guard" };
    public static readonly Npc Ahoj = new() { Id = 5644, Name = "Ahoj", WikiUrl = "https://wiki.guildwars.com/wiki/Ahoj" };
    public static readonly Npc Dende = new() { Id = 5644, Name = "Dende", WikiUrl = "https://wiki.guildwars.com/wiki/Dende" };
    public static readonly Npc VabbianChild = new() { Id = 5644, Name = "Vabbian Child", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Child" };
    public static readonly Npc VabbianCommoner2 = new() { Id = 5644, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc YoungChild2 = new() { Id = 5644, Name = "Young Child", WikiUrl = "https://wiki.guildwars.com/wiki/Young_Child" };
    public static readonly Npc Charmah = new() { Id = 5645, Name = "Charmah", WikiUrl = "https://wiki.guildwars.com/wiki/Charmah" };
    public static readonly Npc Mina = new() { Id = 5645, Name = "Mina", WikiUrl = "https://wiki.guildwars.com/wiki/Mina" };
    public static readonly Npc VabbiChild = new() { Id = 5645, Name = "Vabbi Child", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Child" };
    public static readonly Npc VabbianChild1 = new() { Id = 5645, Name = "Vabbian Child", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Child" };
    public static readonly Npc VabbianCommoner3 = new() { Id = 5645, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc YoungChild3 = new() { Id = 5645, Name = "Young Child", WikiUrl = "https://wiki.guildwars.com/wiki/Young_Child" };
    public static readonly Npc Bahskir = new() { Id = 5646, Name = "Bahskir", WikiUrl = "https://wiki.guildwars.com/wiki/Bahskir" };
    public static readonly Npc Baram = new() { Id = 5646, Name = "Baram", WikiUrl = "https://wiki.guildwars.com/wiki/Baram" };
    public static readonly Npc Behruseh = new() { Id = 5646, Name = "Behruseh", WikiUrl = "https://wiki.guildwars.com/wiki/Behruseh" };
    public static readonly Npc BrihanChahu = new() { Id = 5646, Name = "Brihan Chahu", WikiUrl = "https://wiki.guildwars.com/wiki/Brihan_Chahu" };
    public static readonly Npc DehvitNotigunahm = new() { Id = 5646, Name = "Dehvit Notigunahm", WikiUrl = "https://wiki.guildwars.com/wiki/Dehvit_Notigunahm" };
    public static readonly Npc Gahnlar = new() { Id = 5646, Name = "Gahnlar", WikiUrl = "https://wiki.guildwars.com/wiki/Gahnlar" };
    public static readonly Npc HervahntheVexing = new() { Id = 5646, Name = "Hervahn the Vexing", WikiUrl = "https://wiki.guildwars.com/wiki/Hervahn_the_Vexing" };
    public static readonly Npc HorticulturistHinon = new() { Id = 5646, Name = "Horticulturist Hinon", WikiUrl = "https://wiki.guildwars.com/wiki/Horticulturist_Hinon" };
    public static readonly Npc JahmehBahkmasitur = new() { Id = 5646, Name = "Jahmeh Bahkmasitur", WikiUrl = "https://wiki.guildwars.com/wiki/Jahmeh_Bahkmasitur" };
    public static readonly Npc JiorijWahlahz = new() { Id = 5646, Name = "Jiorij Wahlahz", WikiUrl = "https://wiki.guildwars.com/wiki/Jiorij_Wahlahz" };
    public static readonly Npc KaristendBehrisfahr = new() { Id = 5646, Name = "Karistend Behrisfahr", WikiUrl = "https://wiki.guildwars.com/wiki/Karistend_Behrisfahr" };
    public static readonly Npc Kurmauzeh = new() { Id = 5646, Name = "Kurmauzeh", WikiUrl = "https://wiki.guildwars.com/wiki/Kurmauzeh" };
    public static readonly Npc Kwayama = new() { Id = 5646, Name = "Kwayama", WikiUrl = "https://wiki.guildwars.com/wiki/Kwayama" };
    public static readonly Npc LibrarianEhrahtimos = new() { Id = 5646, Name = "Librarian Ehrahtimos", WikiUrl = "https://wiki.guildwars.com/wiki/Librarian_Ehrahtimos" };
    public static readonly Npc LibraryEnvoyChukeht = new() { Id = 5646, Name = "Library Envoy Chukeht", WikiUrl = "https://wiki.guildwars.com/wiki/Library_Envoy_Chukeht" };
    public static readonly Npc MasterLibrarianAntohneoss = new() { Id = 5646, Name = "Master Librarian Antohneoss", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Librarian_Antohneoss" };
    public static readonly Npc NehtohnEhmbahrsin = new() { Id = 5646, Name = "Nehtohn Ehmbahrsin", WikiUrl = "https://wiki.guildwars.com/wiki/Nehtohn_Ehmbahrsin" };
    public static readonly Npc NoblemanEmrah = new() { Id = 5646, Name = "Nobleman Emrah", WikiUrl = "https://wiki.guildwars.com/wiki/Nobleman_Emrah" };
    public static readonly Npc Norgu1 = new() { Id = 5646, Name = "Norgu", WikiUrl = "https://wiki.guildwars.com/wiki/Norgu" };
    public static readonly Npc Ordash = new() { Id = 5646, Name = "Ordash", WikiUrl = "https://wiki.guildwars.com/wiki/Ordash" };
    public static readonly Npc Peyema = new() { Id = 5646, Name = "Peyema", WikiUrl = "https://wiki.guildwars.com/wiki/Peyema" };
    public static readonly Npc Resh = new() { Id = 5646, Name = "Resh", WikiUrl = "https://wiki.guildwars.com/wiki/Resh" };
    public static readonly Npc RoyalFinanceMinisterOluda = new() { Id = 5646, Name = "Royal Finance Minister Oluda", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Finance_Minister_Oluda" };
    public static readonly Npc RoyalFoodTasterRendu = new() { Id = 5646, Name = "Royal Food Taster Rendu", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Food_Taster_Rendu" };
    public static readonly Npc Sholhij = new() { Id = 5646, Name = "Sholhij", WikiUrl = "https://wiki.guildwars.com/wiki/Sholhij" };
    public static readonly Npc Shosa = new() { Id = 5646, Name = "Shosa", WikiUrl = "https://wiki.guildwars.com/wiki/Shosa" };
    public static readonly Npc TahravohrGarifiz = new() { Id = 5646, Name = "Tahravohr Garifiz", WikiUrl = "https://wiki.guildwars.com/wiki/Tahravohr_Garifiz" };
    public static readonly Npc TheatreManagerDaudi = new() { Id = 5646, Name = "Theatre Manager Daudi", WikiUrl = "https://wiki.guildwars.com/wiki/Theatre_Manager_Daudi" };
    public static readonly Npc TohnehJonesh = new() { Id = 5646, Name = "Tohneh Jonesh", WikiUrl = "https://wiki.guildwars.com/wiki/Tohneh_Jonesh" };
    public static readonly Npc VabbiNoble = new() { Id = 5646, Name = "Vabbi Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Noble" };
    public static readonly Npc VabbianNoble = new() { Id = 5646, Name = "Vabbian Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Noble" };
    public static readonly Npc VaughntheVenerable1 = new() { Id = 5646, Name = "Vaughn the Venerable", WikiUrl = "https://wiki.guildwars.com/wiki/Vaughn_the_Venerable" };
    public static readonly Npc Wekekuda = new() { Id = 5646, Name = "Wekekuda", WikiUrl = "https://wiki.guildwars.com/wiki/Wekekuda" };
    public static readonly Npc ZeraitheLearner = new() { Id = 5646, Name = "Zerai the Learner", WikiUrl = "https://wiki.guildwars.com/wiki/Zerai_the_Learner" };
    public static readonly Npc Endah = new() { Id = 5648, Name = "Endah", WikiUrl = "https://wiki.guildwars.com/wiki/Endah" };
    public static readonly Npc EventPlannerKazsha = new() { Id = 5648, Name = "Event Planner Kazsha", WikiUrl = "https://wiki.guildwars.com/wiki/Event_Planner_Kazsha" };
    public static readonly Npc Jekunda = new() { Id = 5648, Name = "Jekunda", WikiUrl = "https://wiki.guildwars.com/wiki/Jekunda" };
    public static readonly Npc KahlinIdehjir = new() { Id = 5648, Name = "Kahlin Idehjir", WikiUrl = "https://wiki.guildwars.com/wiki/Kahlin_Idehjir" };
    public static readonly Npc KaturinEhmbolem = new() { Id = 5648, Name = "Katurin Ehmbolem", WikiUrl = "https://wiki.guildwars.com/wiki/Katurin_Ehmbolem" };
    public static readonly Npc LibrarianKahlidahri = new() { Id = 5648, Name = "Librarian Kahlidahri", WikiUrl = "https://wiki.guildwars.com/wiki/Librarian_Kahlidahri" };
    public static readonly Npc PehniRahnad = new() { Id = 5648, Name = "Pehni Rahnad", WikiUrl = "https://wiki.guildwars.com/wiki/Pehni_Rahnad" };
    public static readonly Npc PrincessLeifah = new() { Id = 5648, Name = "Princess Leifah", WikiUrl = "https://wiki.guildwars.com/wiki/Princess_Leifah" };
    public static readonly Npc Ridara = new() { Id = 5648, Name = "Ridara", WikiUrl = "https://wiki.guildwars.com/wiki/Ridara" };
    public static readonly Npc Sinvahn = new() { Id = 5648, Name = "Sinvahn", WikiUrl = "https://wiki.guildwars.com/wiki/Sinvahn" };
    public static readonly Npc VabbiNoble1 = new() { Id = 5648, Name = "Vabbi Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Noble" };
    public static readonly Npc VabbianGypsy = new() { Id = 5648, Name = "Vabbian Gypsy", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Gypsy" };
    public static readonly Npc VabbianNoble1 = new() { Id = 5648, Name = "Vabbian Noble", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Noble" };
    public static readonly Npc Yuli = new() { Id = 5648, Name = "Yuli", WikiUrl = "https://wiki.guildwars.com/wiki/Yuli" };
    public static readonly Npc AttendanttothePrince = new() { Id = 5649, Name = "Attendant to the Prince", WikiUrl = "https://wiki.guildwars.com/wiki/Attendant_to_the_Prince" };
    public static readonly Npc HorticulturistHinon1 = new() { Id = 5649, Name = "Horticulturist Hinon", WikiUrl = "https://wiki.guildwars.com/wiki/Horticulturist_Hinon" };
    public static readonly Npc LarrinBahlakbahn = new() { Id = 5649, Name = "Larrin Bahlakbahn", WikiUrl = "https://wiki.guildwars.com/wiki/Larrin_Bahlakbahn" };
    public static readonly Npc LibrarianGahmirLenon = new() { Id = 5649, Name = "Librarian Gahmir Lenon", WikiUrl = "https://wiki.guildwars.com/wiki/Librarian_Gahmir_Lenon" };
    public static readonly Npc LibrarianMularuk = new() { Id = 5649, Name = "Librarian Mularuk", WikiUrl = "https://wiki.guildwars.com/wiki/Librarian_Mularuk" };
    public static readonly Npc LibrarianNichitheRanger = new() { Id = 5649, Name = "Librarian Nichi the Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Librarian_Nichi_the_Ranger" };
    public static readonly Npc MasterGardenerKobahl = new() { Id = 5649, Name = "Master Gardener Kobahl", WikiUrl = "https://wiki.guildwars.com/wiki/Master_Gardener_Kobahl" };
    public static readonly Npc PahulJohansohn = new() { Id = 5649, Name = "Pahul Johansohn", WikiUrl = "https://wiki.guildwars.com/wiki/Pahul_Johansohn" };
    public static readonly Npc RoyalFoodTasterRendu1 = new() { Id = 5649, Name = "Royal Food Taster Rendu", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Food_Taster_Rendu" };
    public static readonly Npc VabbianCommoner4 = new() { Id = 5649, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc VabbianGypsy1 = new() { Id = 5649, Name = "Vabbian Gypsy", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Gypsy" };
    public static readonly Npc ArchivistMijir = new() { Id = 5650, Name = "Archivist Mijir", WikiUrl = "https://wiki.guildwars.com/wiki/Archivist_Mijir" };
    public static readonly Npc LibrarianChitohn = new() { Id = 5650, Name = "Librarian Chitohn", WikiUrl = "https://wiki.guildwars.com/wiki/Librarian_Chitohn" };
    public static readonly Npc MahrekChuri = new() { Id = 5650, Name = "Mahrek Churi", WikiUrl = "https://wiki.guildwars.com/wiki/Mahrek_Churi" };
    public static readonly Npc RoyalChefHatundo = new() { Id = 5650, Name = "Royal Chef Hatundo", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Chef_Hatundo" };
    public static readonly Npc Sakutila = new() { Id = 5650, Name = "Sakutila", WikiUrl = "https://wiki.guildwars.com/wiki/Sakutila" };
    public static readonly Npc VabbianCommoner5 = new() { Id = 5650, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc VabbianGypsy2 = new() { Id = 5650, Name = "Vabbian Gypsy", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Gypsy" };
    public static readonly Npc AhdariDohchimahn = new() { Id = 5652, Name = "Ahdari Dohchimahn", WikiUrl = "https://wiki.guildwars.com/wiki/Ahdari_Dohchimahn" };
    public static readonly Npc JenRohsin = new() { Id = 5652, Name = "Jen Rohsin", WikiUrl = "https://wiki.guildwars.com/wiki/Jen_Rohsin" };
    public static readonly Npc Jermahzeh = new() { Id = 5652, Name = "Jermahzeh", WikiUrl = "https://wiki.guildwars.com/wiki/Jermahzeh" };
    public static readonly Npc Lahati = new() { Id = 5652, Name = "Lahati", WikiUrl = "https://wiki.guildwars.com/wiki/Lahati" };
    public static readonly Npc LibrarianKahnu = new() { Id = 5652, Name = "Librarian Kahnu", WikiUrl = "https://wiki.guildwars.com/wiki/Librarian_Kahnu" };
    public static readonly Npc Nuwisha = new() { Id = 5652, Name = "Nuwisha", WikiUrl = "https://wiki.guildwars.com/wiki/Nuwisha" };
    public static readonly Npc VabbianCommoner6 = new() { Id = 5652, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc VabbianGypsy3 = new() { Id = 5652, Name = "Vabbian Gypsy", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Gypsy" };
    public static readonly Npc Kalanda = new() { Id = 5653, Name = "Kalanda", WikiUrl = "https://wiki.guildwars.com/wiki/Kalanda" };
    public static readonly Npc Rahvu = new() { Id = 5653, Name = "Rahvu", WikiUrl = "https://wiki.guildwars.com/wiki/Rahvu" };
    public static readonly Npc VabbianCommoner7 = new() { Id = 5653, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc Abaddon1 = new() { Id = 5655, Name = "Abaddon", WikiUrl = "https://wiki.guildwars.com/wiki/Abaddon" };
    public static readonly Npc AssistantLenahn = new() { Id = 5655, Name = "Assistant Lenahn", WikiUrl = "https://wiki.guildwars.com/wiki/Assistant_Lenahn" };
    public static readonly Npc Belanu = new() { Id = 5655, Name = "Belanu", WikiUrl = "https://wiki.guildwars.com/wiki/Belanu" };
    public static readonly Npc BelguntheQuarryMaster = new() { Id = 5655, Name = "Belgun the Quarry Master", WikiUrl = "https://wiki.guildwars.com/wiki/Belgun_the_Quarry_Master" };
    public static readonly Npc Dahwan = new() { Id = 5655, Name = "Dahwan", WikiUrl = "https://wiki.guildwars.com/wiki/Dahwan" };
    public static readonly Npc HedgeWizardMabai = new() { Id = 5655, Name = "Hedge Wizard Mabai", WikiUrl = "https://wiki.guildwars.com/wiki/Hedge_Wizard_Mabai" };
    public static readonly Npc JahkKihngah = new() { Id = 5655, Name = "Jahk Kihngah", WikiUrl = "https://wiki.guildwars.com/wiki/Jahk_Kihngah" };
    public static readonly Npc Korvus = new() { Id = 5655, Name = "Korvus", WikiUrl = "https://wiki.guildwars.com/wiki/Korvus" };
    public static readonly Npc KuridehtheMad = new() { Id = 5655, Name = "Kurideh the Mad", WikiUrl = "https://wiki.guildwars.com/wiki/Kurideh_the_Mad" };
    public static readonly Npc Kurli = new() { Id = 5655, Name = "Kurli", WikiUrl = "https://wiki.guildwars.com/wiki/Kurli" };
    public static readonly Npc Lahri = new() { Id = 5655, Name = "Lahri", WikiUrl = "https://wiki.guildwars.com/wiki/Lahri" };
    public static readonly Npc LumotheMime = new() { Id = 5655, Name = "Lumo the Mime", WikiUrl = "https://wiki.guildwars.com/wiki/Lumo_the_Mime" };
    public static readonly Npc Moh = new() { Id = 5655, Name = "Moh", WikiUrl = "https://wiki.guildwars.com/wiki/Moh" };
    public static readonly Npc Musician = new() { Id = 5655, Name = "Musician", WikiUrl = "https://wiki.guildwars.com/wiki/Musician" };
    public static readonly Npc OlatheFantastical = new() { Id = 5655, Name = "Ola the Fantastical", WikiUrl = "https://wiki.guildwars.com/wiki/Ola_the_Fantastical" };
    public static readonly Npc Olujime = new() { Id = 5655, Name = "Olujime", WikiUrl = "https://wiki.guildwars.com/wiki/Olujime" };
    public static readonly Npc RoyalServant = new() { Id = 5655, Name = "Royal Servant", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Servant" };
    public static readonly Npc Shashi = new() { Id = 5655, Name = "Shashi", WikiUrl = "https://wiki.guildwars.com/wiki/Shashi" };
    public static readonly Npc VabbiPeasant = new() { Id = 5655, Name = "Vabbi Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Peasant" };
    public static readonly Npc VabbianCommoner8 = new() { Id = 5655, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc VabbianMiner = new() { Id = 5655, Name = "Vabbian Miner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Miner" };
    public static readonly Npc Zardok = new() { Id = 5655, Name = "Zardok", WikiUrl = "https://wiki.guildwars.com/wiki/Zardok" };
    public static readonly Npc AssistantJenahn = new() { Id = 5657, Name = "Assistant Jenahn", WikiUrl = "https://wiki.guildwars.com/wiki/Assistant_Jenahn" };
    public static readonly Npc DuelMasterLumbo = new() { Id = 5657, Name = "Duel Master Lumbo", WikiUrl = "https://wiki.guildwars.com/wiki/Duel_Master_Lumbo" };
    public static readonly Npc GardenerTidak = new() { Id = 5657, Name = "Gardener Tidak", WikiUrl = "https://wiki.guildwars.com/wiki/Gardener_Tidak" };
    public static readonly Npc Hamri = new() { Id = 5657, Name = "Hamri", WikiUrl = "https://wiki.guildwars.com/wiki/Hamri" };
    public static readonly Npc JohinPiht = new() { Id = 5657, Name = "Johin Piht", WikiUrl = "https://wiki.guildwars.com/wiki/Johin_Piht" };
    public static readonly Npc Kamveh = new() { Id = 5657, Name = "Kamveh", WikiUrl = "https://wiki.guildwars.com/wiki/Kamveh" };
    public static readonly Npc Kenyatta = new() { Id = 5657, Name = "Kenyatta", WikiUrl = "https://wiki.guildwars.com/wiki/Kenyatta" };
    public static readonly Npc Musician1 = new() { Id = 5657, Name = "Musician", WikiUrl = "https://wiki.guildwars.com/wiki/Musician" };
    public static readonly Npc Narrator = new() { Id = 5657, Name = "Narrator", WikiUrl = "https://wiki.guildwars.com/wiki/Narrator" };
    public static readonly Npc RoyalServant1 = new() { Id = 5657, Name = "Royal Servant", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Servant" };
    public static readonly Npc Talgun = new() { Id = 5657, Name = "Talgun", WikiUrl = "https://wiki.guildwars.com/wiki/Talgun" };
    public static readonly Npc VabbiPeasant1 = new() { Id = 5657, Name = "Vabbi Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Peasant" };
    public static readonly Npc VabbianCommoner9 = new() { Id = 5657, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc VabbianMiner1 = new() { Id = 5657, Name = "Vabbian Miner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Miner" };
    public static readonly Npc ZilotheDrunkard = new() { Id = 5657, Name = "Zilo the Drunkard", WikiUrl = "https://wiki.guildwars.com/wiki/Zilo_the_Drunkard" };
    public static readonly Npc Amadi = new() { Id = 5658, Name = "Amadi", WikiUrl = "https://wiki.guildwars.com/wiki/Amadi" };
    public static readonly Npc Awadur = new() { Id = 5658, Name = "Awadur", WikiUrl = "https://wiki.guildwars.com/wiki/Awadur" };
    public static readonly Npc Bahlbahs = new() { Id = 5658, Name = "Bahlbahs", WikiUrl = "https://wiki.guildwars.com/wiki/Bahlbahs" };
    public static readonly Npc Charmamani = new() { Id = 5658, Name = "Charmamani", WikiUrl = "https://wiki.guildwars.com/wiki/Charmamani" };
    public static readonly Npc Hermehinu = new() { Id = 5658, Name = "Hermehinu", WikiUrl = "https://wiki.guildwars.com/wiki/Hermehinu" };
    public static readonly Npc Kaya1 = new() { Id = 5658, Name = "Kaya", WikiUrl = "https://wiki.guildwars.com/wiki/Kaya" };
    public static readonly Npc Kimab = new() { Id = 5658, Name = "Kimab", WikiUrl = "https://wiki.guildwars.com/wiki/Kimab" };
    public static readonly Npc Musician2 = new() { Id = 5658, Name = "Musician", WikiUrl = "https://wiki.guildwars.com/wiki/Musician" };
    public static readonly Npc PriestessHaila = new() { Id = 5658, Name = "Priestess Haila", WikiUrl = "https://wiki.guildwars.com/wiki/Priestess_Haila" };
    public static readonly Npc RoyalServant2 = new() { Id = 5658, Name = "Royal Servant", WikiUrl = "https://wiki.guildwars.com/wiki/Royal_Servant" };
    public static readonly Npc VabbiPeasant2 = new() { Id = 5658, Name = "Vabbi Peasant", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbi_Peasant" };
    public static readonly Npc VabbianCommoner10 = new() { Id = 5658, Name = "Vabbian Commoner", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Commoner" };
    public static readonly Npc VabbianGypsy4 = new() { Id = 5658, Name = "Vabbian Gypsy", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Gypsy" };
    public static readonly Npc ArchivistLeiton = new() { Id = 5660, Name = "Archivist Leiton", WikiUrl = "https://wiki.guildwars.com/wiki/Archivist_Leiton" };
    public static readonly Npc Haitend = new() { Id = 5660, Name = "Haitend", WikiUrl = "https://wiki.guildwars.com/wiki/Haitend" };
    public static readonly Npc HeadPriestVahmani = new() { Id = 5660, Name = "Head Priest Vahmani", WikiUrl = "https://wiki.guildwars.com/wiki/Head_Priest_Vahmani" };
    public static readonly Npc Kachok = new() { Id = 5660, Name = "Kachok", WikiUrl = "https://wiki.guildwars.com/wiki/Kachok" };
    public static readonly Npc MasterofCeremonies = new() { Id = 5660, Name = "Master of Ceremonies", WikiUrl = "https://wiki.guildwars.com/wiki/Master_of_Ceremonies" };
    public static readonly Npc PriestJeshek = new() { Id = 5660, Name = "Priest Jeshek", WikiUrl = "https://wiki.guildwars.com/wiki/Priest_Jeshek" };
    public static readonly Npc PriestTuwahan = new() { Id = 5660, Name = "Priest Tuwahan", WikiUrl = "https://wiki.guildwars.com/wiki/Priest_Tuwahan" };
    public static readonly Npc RecordsKeeperPalin = new() { Id = 5660, Name = "Records Keeper Palin", WikiUrl = "https://wiki.guildwars.com/wiki/Records_Keeper_Palin" };
    public static readonly Npc VabbianPriest = new() { Id = 5660, Name = "Vabbian Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Priest" };
    public static readonly Npc Larmor = new() { Id = 5661, Name = "Larmor", WikiUrl = "https://wiki.guildwars.com/wiki/Larmor" };
    public static readonly Npc Nerwar = new() { Id = 5661, Name = "Nerwar", WikiUrl = "https://wiki.guildwars.com/wiki/Nerwar" };
    public static readonly Npc Lilita = new() { Id = 5662, Name = "Lilita", WikiUrl = "https://wiki.guildwars.com/wiki/Lilita" };
    public static readonly Npc Pakasah = new() { Id = 5662, Name = "Pakasah", WikiUrl = "https://wiki.guildwars.com/wiki/Pakasah" };
    public static readonly Npc Mateneh = new() { Id = 5663, Name = "Mateneh", WikiUrl = "https://wiki.guildwars.com/wiki/Mateneh" };
    public static readonly Npc Buuh = new() { Id = 5665, Name = "Buuh", WikiUrl = "https://wiki.guildwars.com/wiki/Buuh" };
    public static readonly Npc Dazah = new() { Id = 5665, Name = "Dazah", WikiUrl = "https://wiki.guildwars.com/wiki/Dazah" };
    public static readonly Npc Deshwa = new() { Id = 5665, Name = "Deshwa", WikiUrl = "https://wiki.guildwars.com/wiki/Deshwa" };
    public static readonly Npc Jinte = new() { Id = 5665, Name = "Jinte", WikiUrl = "https://wiki.guildwars.com/wiki/Jinte" };
    public static readonly Npc Magis = new() { Id = 5665, Name = "Magis", WikiUrl = "https://wiki.guildwars.com/wiki/Magis" };
    public static readonly Npc Noben = new() { Id = 5665, Name = "Noben", WikiUrl = "https://wiki.guildwars.com/wiki/Noben" };
    public static readonly Npc Ohapeh = new() { Id = 5665, Name = "Ohapeh", WikiUrl = "https://wiki.guildwars.com/wiki/Ohapeh" };
    public static readonly Npc Papo = new() { Id = 5665, Name = "Papo", WikiUrl = "https://wiki.guildwars.com/wiki/Papo" };
    public static readonly Npc Peshwan = new() { Id = 5665, Name = "Peshwan", WikiUrl = "https://wiki.guildwars.com/wiki/Peshwan" };
    public static readonly Npc Zojan = new() { Id = 5665, Name = "Zojan", WikiUrl = "https://wiki.guildwars.com/wiki/Zojan" };
    public static readonly Npc Ahjii = new() { Id = 5666, Name = "Ahjii", WikiUrl = "https://wiki.guildwars.com/wiki/Ahjii" };
    public static readonly Npc Itai = new() { Id = 5666, Name = "Itai", WikiUrl = "https://wiki.guildwars.com/wiki/Itai" };
    public static readonly Npc Jahnbur = new() { Id = 5666, Name = "Jahnbur", WikiUrl = "https://wiki.guildwars.com/wiki/Jahnbur" };
    public static readonly Npc Merassi = new() { Id = 5666, Name = "Merassi", WikiUrl = "https://wiki.guildwars.com/wiki/Merassi" };
    public static readonly Npc Sahwan = new() { Id = 5666, Name = "Sahwan", WikiUrl = "https://wiki.guildwars.com/wiki/Sahwan" };
    public static readonly Npc Shanta = new() { Id = 5666, Name = "Shanta", WikiUrl = "https://wiki.guildwars.com/wiki/Shanta" };
    public static readonly Npc Turissa = new() { Id = 5666, Name = "Turissa", WikiUrl = "https://wiki.guildwars.com/wiki/Turissa" };
    public static readonly Npc Yilai = new() { Id = 5666, Name = "Yilai", WikiUrl = "https://wiki.guildwars.com/wiki/Yilai" };
    public static readonly Npc Aksah = new() { Id = 5667, Name = "Aksah", WikiUrl = "https://wiki.guildwars.com/wiki/Aksah" };
    public static readonly Npc Rahmdah = new() { Id = 5667, Name = "Rahmdah", WikiUrl = "https://wiki.guildwars.com/wiki/Rahmdah" };
    public static readonly Npc Wisseh = new() { Id = 5667, Name = "Wisseh", WikiUrl = "https://wiki.guildwars.com/wiki/Wisseh" };
    public static readonly Npc Asiri = new() { Id = 5668, Name = "Asiri", WikiUrl = "https://wiki.guildwars.com/wiki/Asiri" };
    public static readonly Npc Kehjim = new() { Id = 5668, Name = "Kehjim", WikiUrl = "https://wiki.guildwars.com/wiki/Kehjim" };
    public static readonly Npc Amurte = new() { Id = 5669, Name = "Amurte", WikiUrl = "https://wiki.guildwars.com/wiki/Amurte" };
    public static readonly Npc Rahd = new() { Id = 5669, Name = "Rahd", WikiUrl = "https://wiki.guildwars.com/wiki/Rahd" };
    public static readonly Npc Narjisuh = new() { Id = 5670, Name = "Narjisuh", WikiUrl = "https://wiki.guildwars.com/wiki/Narjisuh" };
    public static readonly Npc Shahler = new() { Id = 5670, Name = "Shahler", WikiUrl = "https://wiki.guildwars.com/wiki/Shahler" };
    public static readonly Npc Bodahn = new() { Id = 5671, Name = "Bodahn", WikiUrl = "https://wiki.guildwars.com/wiki/Bodahn" };
    public static readonly Npc Harib = new() { Id = 5671, Name = "Harib", WikiUrl = "https://wiki.guildwars.com/wiki/Harib" };
    public static readonly Npc Urij = new() { Id = 5671, Name = "Urij", WikiUrl = "https://wiki.guildwars.com/wiki/Urij" };
    public static readonly Npc Naksem = new() { Id = 5672, Name = "Naksem", WikiUrl = "https://wiki.guildwars.com/wiki/Naksem" };
    public static readonly Npc KolonaMaterial = new() { Id = 5673, Name = "Kolona Material", WikiUrl = "https://wiki.guildwars.com/wiki/Kolona_Material" };
    public static readonly Npc LurlaiMaterial = new() { Id = 5673, Name = "Lurlai Material", WikiUrl = "https://wiki.guildwars.com/wiki/Lurlai_Material" };
    public static readonly Npc PerashMaterial = new() { Id = 5673, Name = "Perash Material", WikiUrl = "https://wiki.guildwars.com/wiki/Perash_Material" };
    public static readonly Npc ZatahMaterial = new() { Id = 5674, Name = "Zatah Material", WikiUrl = "https://wiki.guildwars.com/wiki/Zatah_Material" };
    public static readonly Npc IreshScroll = new() { Id = 5675, Name = "Iresh Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Iresh_Scroll" };
    public static readonly Npc TotarScroll = new() { Id = 5675, Name = "Totar Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Totar_Scroll" };
    public static readonly Npc WehwahScroll = new() { Id = 5675, Name = "Wehwah Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Wehwah_Scroll" };
    public static readonly Npc LajariScroll = new() { Id = 5676, Name = "Lajari Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Lajari_Scroll" };
    public static readonly Npc OuridaScroll = new() { Id = 5676, Name = "Ourida Scroll", WikiUrl = "https://wiki.guildwars.com/wiki/Ourida_Scroll" };
    public static readonly Npc Hesham = new() { Id = 5677, Name = "Hesham", WikiUrl = "https://wiki.guildwars.com/wiki/Hesham" };
    public static readonly Npc Karmu = new() { Id = 5677, Name = "Karmu", WikiUrl = "https://wiki.guildwars.com/wiki/Karmu" };
    public static readonly Npc Fahnesh = new() { Id = 5678, Name = "Fahnesh", WikiUrl = "https://wiki.guildwars.com/wiki/Fahnesh" };
    public static readonly Npc Lahi = new() { Id = 5678, Name = "Lahi", WikiUrl = "https://wiki.guildwars.com/wiki/Lahi" };
    public static readonly Npc GuardianofWhispers = new() { Id = 5679, Name = "Guardian of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_of_Whispers" };
    public static readonly Npc Mureh = new() { Id = 5679, Name = "Mureh", WikiUrl = "https://wiki.guildwars.com/wiki/Mureh" };
    public static readonly Npc SeekerofWhispers = new() { Id = 5679, Name = "Seeker of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Seeker_of_Whispers" };
    public static readonly Npc SourceofWhispers = new() { Id = 5679, Name = "Source of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Source_of_Whispers" };
    public static readonly Npc WanderingScribe = new() { Id = 5679, Name = "Wandering Scribe", WikiUrl = "https://wiki.guildwars.com/wiki/Wandering_Scribe" };
    public static readonly Npc WardenofWhispers = new() { Id = 5679, Name = "Warden of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_Whispers" };
    public static readonly Npc WhispersAcolyte = new() { Id = 5679, Name = "Whispers Acolyte", WikiUrl = "https://wiki.guildwars.com/wiki/Whispers_Acolyte" };
    public static readonly Npc WhispersAdept = new() { Id = 5679, Name = "Whispers Adept", WikiUrl = "https://wiki.guildwars.com/wiki/Whispers_Adept" };
    public static readonly Npc WhispersCrusaderMission = new() { Id = 5679, Name = "Whispers Crusader Mission", WikiUrl = "https://wiki.guildwars.com/wiki/Whispers_Crusader_Mission" };
    public static readonly Npc WhispersInformant1 = new() { Id = 5679, Name = "Whispers Informant", WikiUrl = "https://wiki.guildwars.com/wiki/Whispers_Informant" };
    public static readonly Npc WitnessAhtok = new() { Id = 5679, Name = "Witness Ahtok", WikiUrl = "https://wiki.guildwars.com/wiki/Witness_Ahtok" };
    public static readonly Npc Merkod = new() { Id = 5683, Name = "Merkod", WikiUrl = "https://wiki.guildwars.com/wiki/Merkod" };
    public static readonly Npc Nadara = new() { Id = 5684, Name = "Nadara", WikiUrl = "https://wiki.guildwars.com/wiki/Nadara" };
    public static readonly Npc Kehanni = new() { Id = 5685, Name = "Kehanni", WikiUrl = "https://wiki.guildwars.com/wiki/Kehanni" };
    public static readonly Npc PrinceBokkatheMagnificent = new() { Id = 5687, Name = "Prince Bokka the Magnificent", WikiUrl = "https://wiki.guildwars.com/wiki/Prince_Bokka_the_Magnificent" };
    public static readonly Npc PrinceAhmturtheMighty = new() { Id = 5688, Name = "Prince Ahmtur the Mighty", WikiUrl = "https://wiki.guildwars.com/wiki/Prince_Ahmtur_the_Mighty" };
    public static readonly Npc PrinceMehtutheWise = new() { Id = 5689, Name = "Prince Mehtu the Wise", WikiUrl = "https://wiki.guildwars.com/wiki/Prince_Mehtu_the_Wise" };
    public static readonly Npc AdeptofWhispers1 = new() { Id = 5690, Name = "Adept of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Adept_of_Whispers" };
    public static readonly Npc DiscipleofSecrets = new() { Id = 5690, Name = "Disciple of Secrets", WikiUrl = "https://wiki.guildwars.com/wiki/Disciple_of_Secrets" };
    public static readonly Npc WardenofWhispers1 = new() { Id = 5690, Name = "Warden of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Warden_of_Whispers" };
    public static readonly Npc Vidaj = new() { Id = 5691, Name = "Vidaj", WikiUrl = "https://wiki.guildwars.com/wiki/Vidaj" };
    public static readonly Npc HanbahdtheAnchorite = new() { Id = 5692, Name = "Hanbahd the Anchorite", WikiUrl = "https://wiki.guildwars.com/wiki/Hanbahd_the_Anchorite" };
    public static readonly Npc TheGreatZehtuka1 = new() { Id = 5693, Name = "The Great Zehtuka", WikiUrl = "https://wiki.guildwars.com/wiki/The_Great_Zehtuka" };
    public static readonly Npc PalawaJoko1 = new() { Id = 5695, Name = "Palawa Joko", WikiUrl = "https://wiki.guildwars.com/wiki/Palawa_Joko" };
    public static readonly Npc GardenerPohsin = new() { Id = 5697, Name = "Gardener Pohsin", WikiUrl = "https://wiki.guildwars.com/wiki/Gardener_Pohsin" };
    public static readonly Npc ScholarAiki = new() { Id = 5698, Name = "Scholar Aiki", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Aiki" };
    public static readonly Npc ScholarGahesh = new() { Id = 5698, Name = "Scholar Gahesh", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Gahesh" };
    public static readonly Npc Diji = new() { Id = 5699, Name = "Diji", WikiUrl = "https://wiki.guildwars.com/wiki/Diji" };
    public static readonly Npc ScholarBelzar = new() { Id = 5699, Name = "Scholar Belzar", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Belzar" };
    public static readonly Npc ScholarDakkun = new() { Id = 5699, Name = "Scholar Dakkun", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Dakkun" };
    public static readonly Npc ScholarKoben = new() { Id = 5699, Name = "Scholar Koben", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Koben" };
    public static readonly Npc ScholarZelkun = new() { Id = 5700, Name = "Scholar Zelkun", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Zelkun" };
    public static readonly Npc ScholarKammab = new() { Id = 5701, Name = "Scholar Kammab", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Kammab" };
    public static readonly Npc ScholarMehdok = new() { Id = 5701, Name = "Scholar Mehdok", WikiUrl = "https://wiki.guildwars.com/wiki/Scholar_Mehdok" };
    public static readonly Npc LordYamatheVengeful = new() { Id = 5702, Name = "Lord Yama the Vengeful", WikiUrl = "https://wiki.guildwars.com/wiki/Lord_Yama_the_Vengeful" };
    public static readonly Npc MessengerofLyssa = new() { Id = 5704, Name = "Messenger of Lyssa", WikiUrl = "https://wiki.guildwars.com/wiki/Messenger_of_Lyssa" };
    public static readonly Npc HorticulturistHinon2 = new() { Id = 5706, Name = "Horticulturist Hinon", WikiUrl = "https://wiki.guildwars.com/wiki/Horticulturist_Hinon" };
    public static readonly Npc ErnaldtheExact = new() { Id = 5708, Name = "Ernald the Exact", WikiUrl = "https://wiki.guildwars.com/wiki/Ernald_the_Exact" };
    public static readonly Npc Fissure = new() { Id = 5862, Name = "Fissure", WikiUrl = "https://wiki.guildwars.com/wiki/Fissure" };
    public static readonly Npc Vekk1 = new() { Id = 5910, Name = "Vekk", WikiUrl = "https://wiki.guildwars.com/wiki/Vekk" };
    public static readonly Npc OgdenStonehealer1 = new() { Id = 5929, Name = "Ogden Stonehealer", WikiUrl = "https://wiki.guildwars.com/wiki/Ogden_Stonehealer" };
    public static readonly Npc DestroyerofSinew = new() { Id = 6090, Name = "Destroyer of Sinew", WikiUrl = "https://wiki.guildwars.com/wiki/Destroyer_of_Sinew" };
    public static readonly Npc DestroyerofBones = new() { Id = 6091, Name = "Destroyer of Bones", WikiUrl = "https://wiki.guildwars.com/wiki/Destroyer_of_Bones" };
    public static readonly Npc DestroyerofFlesh = new() { Id = 6092, Name = "Destroyer of Flesh", WikiUrl = "https://wiki.guildwars.com/wiki/Destroyer_of_Flesh" };
    public static readonly Npc DwarvenDemolitionist1 = new() { Id = 6179, Name = "Dwarven Demolitionist", WikiUrl = "https://wiki.guildwars.com/wiki/Dwarven_Demolitionist" };
    public static readonly Npc Borvorel1 = new() { Id = 6190, Name = "Borvorel", WikiUrl = "https://wiki.guildwars.com/wiki/Borvorel" };
    public static readonly Npc BurolIronfist1 = new() { Id = 6211, Name = "Burol Ironfist", WikiUrl = "https://wiki.guildwars.com/wiki/Burol_Ironfist" };
    public static readonly Npc Kodan1 = new() { Id = 6213, Name = "Kodan", WikiUrl = "https://wiki.guildwars.com/wiki/Kodan" };
    public static readonly Npc AdmiralJakman = new() { Id = 7175, Name = "Admiral Jakman", WikiUrl = "https://wiki.guildwars.com/wiki/Admiral_Jakman" };
    public static readonly Npc CorsairWizard3 = new() { Id = 7176, Name = "Corsair Wizard", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wizard" };
    public static readonly Npc CorsairWindMaster6 = new() { Id = 7178, Name = "Corsair Wind Master", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Wind_Master" };
    public static readonly Npc CorsairCutthroat3 = new() { Id = 7180, Name = "Corsair Cutthroat", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Cutthroat" };
    public static readonly Npc CorsairRaider3 = new() { Id = 7181, Name = "Corsair Raider", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Raider" };
    public static readonly Npc CorsairBerserker3 = new() { Id = 7182, Name = "Corsair Berserker", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Berserker" };
    public static readonly Npc CorsairCommandant3 = new() { Id = 7183, Name = "Corsair Commandant", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Commandant" };
    public static readonly Npc AwakenedThoughtLeech1 = new() { Id = 7185, Name = "Awakened Thought Leech", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Thought_Leech" };
    public static readonly Npc AwakenedDefiler1 = new() { Id = 7186, Name = "Awakened Defiler", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Defiler" };
    public static readonly Npc AcolyteofJoko = new() { Id = 7187, Name = "Acolyte of Joko", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_of_Joko" };
    public static readonly Npc CarvenEffigy1 = new() { Id = 7188, Name = "Carven Effigy", WikiUrl = "https://wiki.guildwars.com/wiki/Carven_Effigy" };
    public static readonly Npc AwakenedAcolyte1 = new() { Id = 7189, Name = "Awakened Acolyte", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Acolyte" };
    public static readonly Npc AwakenedBlademaster1 = new() { Id = 7190, Name = "Awakened Blademaster", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Blademaster" };
    public static readonly Npc AwakenedGrayGiant1 = new() { Id = 7191, Name = "Awakened Gray Giant", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Gray_Giant" };
    public static readonly Npc AwakenedDuneCarver1 = new() { Id = 7192, Name = "Awakened Dune Carver", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Dune_Carver" };
    public static readonly Npc AwakenedTrooper = new() { Id = 7193, Name = "Awakened Trooper", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Trooper" };
    public static readonly Npc PalawaJoko2 = new() { Id = 7194, Name = "Palawa Joko", WikiUrl = "https://wiki.guildwars.com/wiki/Palawa_Joko" };
    public static readonly Npc JokosEliteBodyguard = new() { Id = 7195, Name = "Joko's Elite Bodyguard", WikiUrl = "https://wiki.guildwars.com/wiki/Joko's_Elite_Bodyguard" };
    public static readonly Npc EliteKournanTrooper = new() { Id = 7196, Name = "Elite Kournan Trooper", WikiUrl = "https://wiki.guildwars.com/wiki/Elite_Kournan_Trooper" };
    public static readonly Npc VabbianRefugeeSoldier = new() { Id = 7197, Name = "Vabbian Refugee Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Refugee_Soldier" };
    public static readonly Npc VabbianRefugeeSoldier1 = new() { Id = 7198, Name = "Vabbian Refugee Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Refugee_Soldier" };
    public static readonly Npc VabbianRefugeeSoldier2 = new() { Id = 7199, Name = "Vabbian Refugee Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Refugee_Soldier" };
    public static readonly Npc VabbianRefugeeCommander = new() { Id = 7200, Name = "Vabbian Refugee Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Refugee_Commander" };
    public static readonly Npc VabbianRefugeeSoldier3 = new() { Id = 7201, Name = "Vabbian Refugee Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Refugee_Soldier" };
    public static readonly Npc VabbianRefugeeSoldier4 = new() { Id = 7202, Name = "Vabbian Refugee Soldier", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Refugee_Soldier" };
    public static readonly Npc SunspearCommander = new() { Id = 7203, Name = "Sunspear Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Commander" };
    public static readonly Npc SunspearVolunteer2 = new() { Id = 7204, Name = "Sunspear Volunteer", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Volunteer" };
    public static readonly Npc SunspearVolunteer3 = new() { Id = 7205, Name = "Sunspear Volunteer", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_Volunteer" };
    public static readonly Npc AwakenedDefiler2 = new() { Id = 7206, Name = "Awakened Defiler", WikiUrl = "https://wiki.guildwars.com/wiki/Awakened_Defiler" };
    public static readonly Npc MOX1 = new() { Id = 7535, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc NicholastheTraveler = new() { Id = 7658, Name = "Nicholas the Traveler", WikiUrl = "https://wiki.guildwars.com/wiki/Nicholas_the_Traveler" };
    public static readonly Npc ProfessorYakkington = new() { Id = 7659, Name = "Professor Yakkington", WikiUrl = "https://wiki.guildwars.com/wiki/Professor_Yakkington" };
    public static readonly Npc Zenjal1 = new() { Id = 7744, Name = "Zenjal", WikiUrl = "https://wiki.guildwars.com/wiki/Zenjal" };
    public static readonly Npc ForemantheCrier1 = new() { Id = 7970, Name = "Foreman the Crier", WikiUrl = "https://wiki.guildwars.com/wiki/Foreman_the_Crier" };
    public static readonly Npc BeastSwornHeket3 = new() { Id = -1, Name = "Beast Sworn Heket", WikiUrl = "https://wiki.guildwars.com/wiki/Beast_Sworn_Heket" };
    public static readonly Npc CorsairBerserker4 = new() { Id = -1, Name = "Corsair Berserker", WikiUrl = "https://wiki.guildwars.com/wiki/Corsair_Berserker" };
    public static readonly Npc ImmolatedDjinn1 = new() { Id = -1, Name = "Immolated Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Immolated_Djinn" };
    public static readonly Npc Lioness1 = new() { Id = -1, Name = "Lioness", WikiUrl = "https://wiki.guildwars.com/wiki/Lioness" };
    public static readonly Npc DesertWurm1 = new() { Id = -1, Name = "Desert Wurm", WikiUrl = "https://wiki.guildwars.com/wiki/Desert_Wurm" };
    public static readonly Npc RestlessDead1 = new() { Id = -1, Name = "Restless Dead", WikiUrl = "https://wiki.guildwars.com/wiki/Restless_Dead" };
    public static readonly Npc BitGolem = new() { Id = -1, Name = "Bit Golem", WikiUrl = "https://wiki.guildwars.com/wiki/Bit_Golem" };
    public static readonly Npc BreakerBitGolem = new() { Id = -1, Name = "Breaker Bit Golem", WikiUrl = "https://wiki.guildwars.com/wiki/Breaker_Bit_Golem" };
    public static readonly Npc FireBitGolem = new() { Id = -1, Name = "Fire Bit Golem", WikiUrl = "https://wiki.guildwars.com/wiki/Fire_Bit_Golem" };
    public static readonly Npc NOX = new() { Id = -1, Name = "N.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/N.O.X." };
    public static readonly Npc PhantomBitGolem = new() { Id = -1, Name = "Phantom Bit Golem", WikiUrl = "https://wiki.guildwars.com/wiki/Phantom_Bit_Golem" };
    public static readonly Npc PrinceBokka = new() { Id = -1, Name = "Prince Bokka", WikiUrl = "https://wiki.guildwars.com/wiki/Prince_Bokka" };
    public static readonly Npc RectifierBitGolem = new() { Id = -1, Name = "Rectifier Bit Golem", WikiUrl = "https://wiki.guildwars.com/wiki/Rectifier_Bit_Golem" };
    public static readonly Npc ThunderBitGolem = new() { Id = -1, Name = "Thunder Bit Golem", WikiUrl = "https://wiki.guildwars.com/wiki/Thunder_Bit_Golem" };
    public static readonly Npc VabbianActor = new() { Id = -1, Name = "Vabbian Actor", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Actor" };
    public static readonly Npc VabbianActress = new() { Id = -1, Name = "Vabbian Actress", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Actress" };
    public static readonly Npc VabbianStagehand = new() { Id = -1, Name = "Vabbian Stagehand", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Stagehand" };
    public static readonly Npc VabbianTheaterManager = new() { Id = -1, Name = "Vabbian Theater Manager", WikiUrl = "https://wiki.guildwars.com/wiki/Vabbian_Theater_Manager" };
    public static readonly Npc IstaniFarmer = new() { Id = -1, Name = "Istani Farmer", WikiUrl = "https://wiki.guildwars.com/wiki/Istani_Farmer" };
    public static readonly Npc KournanPriest4 = new() { Id = -1, Name = "Kournan Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Priest" };
    public static readonly Npc KournanScribe2 = new() { Id = -1, Name = "Kournan Scribe", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Scribe" };
    public static readonly Npc BahltekTheFirst = new() { Id = -1, Name = "Bahltek The First", WikiUrl = "https://wiki.guildwars.com/wiki/Bahltek_The_First" };
    public static readonly Npc FrigidSkaleTheFirst = new() { Id = -1, Name = "Frigid Skale The First", WikiUrl = "https://wiki.guildwars.com/wiki/Frigid_Skale_The_First" };
    public static readonly Npc SkaleBlighterTheFirst = new() { Id = -1, Name = "Skale Blighter The First", WikiUrl = "https://wiki.guildwars.com/wiki/Skale_Blighter_The_First" };
    public static readonly Npc SkreeTalonTheFirst = new() { Id = -1, Name = "Skree Talon The First", WikiUrl = "https://wiki.guildwars.com/wiki/Skree_Talon_The_First" };
    public static readonly Npc Lion1 = new() { Id = -1, Name = "Lion", WikiUrl = "https://wiki.guildwars.com/wiki/Lion" };
    public static readonly Npc MargoniteSorcerer2 = new() { Id = -1, Name = "Margonite Sorcerer", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Sorcerer" };
    public static readonly Npc MargoniteWarlock3 = new() { Id = -1, Name = "Margonite Warlock", WikiUrl = "https://wiki.guildwars.com/wiki/Margonite_Warlock" };
    public static readonly Npc AnyDjinnand = new() { Id = -1, Name = "Any Djinn and", WikiUrl = "https://wiki.guildwars.com/wiki/Any_Djinn_and" };
    public static readonly Npc LockofAhdashimand = new() { Id = -1, Name = "Lock of Ahdashim and", WikiUrl = "https://wiki.guildwars.com/wiki/Lock_of_Ahdashim_and" };
    public static readonly Npc EnsignLumi2 = new() { Id = -1, Name = "Ensign Lumi", WikiUrl = "https://wiki.guildwars.com/wiki/Ensign_Lumi" };
    public static readonly Npc ArmofInsanity2 = new() { Id = -1, Name = "Arm of Insanity", WikiUrl = "https://wiki.guildwars.com/wiki/Arm_of_Insanity" };
    public static readonly Npc BladeofCorruption3 = new() { Id = -1, Name = "Blade of Corruption", WikiUrl = "https://wiki.guildwars.com/wiki/Blade_of_Corruption" };
    public static readonly Npc ShadowofFear3 = new() { Id = -1, Name = "Shadow of Fear", WikiUrl = "https://wiki.guildwars.com/wiki/Shadow_of_Fear" };
    public static readonly Npc KournanEliteZealotCrossroads = new() { Id = -1, Name = "Kournan Elite Zealot Crossroads", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Elite_Zealot_Crossroads" };
    public static readonly Npc KournanOppressor2 = new() { Id = -1, Name = "Kournan Oppressor", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Oppressor" };
    public static readonly Npc KournanScribe3 = new() { Id = -1, Name = "Kournan Scribe", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Scribe" };
    public static readonly Npc KournanSeer2 = new() { Id = -1, Name = "Kournan Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Seer" };
    public static readonly Npc Sheltah = new() { Id = -1, Name = "Sheltah", WikiUrl = "https://wiki.guildwars.com/wiki/Sheltah" };
    public static readonly Npc HeraldofNightmares2 = new() { Id = -1, Name = "Herald of Nightmares", WikiUrl = "https://wiki.guildwars.com/wiki/Herald_of_Nightmares" };
    public static readonly Npc PainTitan1 = new() { Id = -1, Name = "Pain Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Pain_Titan" };
    public static readonly Npc UnsuspectingKournanGuardMeNo = new() { Id = -1, Name = "Unsuspecting Kournan Guard Me No", WikiUrl = "https://wiki.guildwars.com/wiki/Unsuspecting_Kournan_Guard_Me_No" };
    public static readonly Npc KournanEliteGuardPassage = new() { Id = -1, Name = "Kournan Elite Guard Passage", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Elite_Guard_Passage" };
    public static readonly Npc KournanEliteScribePassage = new() { Id = -1, Name = "Kournan Elite Scribe Passage", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Elite_Scribe_Passage" };
    public static readonly Npc KournanEliteSpearPassage = new() { Id = -1, Name = "Kournan Elite Spear Passage", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Elite_Spear_Passage" };
    public static readonly Npc KournanPhalanx4 = new() { Id = -1, Name = "Kournan Phalanx", WikiUrl = "https://wiki.guildwars.com/wiki/Kournan_Phalanx" };
    public static readonly Npc MandragorSlither4 = new() { Id = -1, Name = "Mandragor Slither", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Slither" };
    public static readonly Npc ScytheofChaosAlkali = new() { Id = -1, Name = "Scythe of Chaos Alkali", WikiUrl = "https://wiki.guildwars.com/wiki/Scythe_of_Chaos_Alkali" };
    public static readonly Npc MOX2 = new() { Id = 525, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc Olias1 = new() { Id = 1931, Name = "Olias", WikiUrl = "https://wiki.guildwars.com/wiki/Olias" };
    public static readonly Npc Olias2 = new() { Id = 1932, Name = "Olias", WikiUrl = "https://wiki.guildwars.com/wiki/Olias" };
    public static readonly Npc Olias3 = new() { Id = 1933, Name = "Olias", WikiUrl = "https://wiki.guildwars.com/wiki/Olias" };
    public static readonly Npc Zenmai1 = new() { Id = 3545, Name = "Zenmai", WikiUrl = "https://wiki.guildwars.com/wiki/Zenmai" };
    public static readonly Npc Zenmai2 = new() { Id = 3546, Name = "Zenmai", WikiUrl = "https://wiki.guildwars.com/wiki/Zenmai" };
    public static readonly Npc Zenmai3 = new() { Id = 3547, Name = "Zenmai", WikiUrl = "https://wiki.guildwars.com/wiki/Zenmai" };
    public static readonly Npc Norgu2 = new() { Id = 4422, Name = "Norgu", WikiUrl = "https://wiki.guildwars.com/wiki/Norgu" };
    public static readonly Npc Norgu3 = new() { Id = 4423, Name = "Norgu", WikiUrl = "https://wiki.guildwars.com/wiki/Norgu" };
    public static readonly Npc Norgu4 = new() { Id = 4424, Name = "Norgu", WikiUrl = "https://wiki.guildwars.com/wiki/Norgu" };
    public static readonly Npc Goren2 = new() { Id = 4427, Name = "Goren", WikiUrl = "https://wiki.guildwars.com/wiki/Goren" };
    public static readonly Npc Goren3 = new() { Id = 4428, Name = "Goren", WikiUrl = "https://wiki.guildwars.com/wiki/Goren" };
    public static readonly Npc Goren4 = new() { Id = 4429, Name = "Goren", WikiUrl = "https://wiki.guildwars.com/wiki/Goren" };
    public static readonly Npc ZhedShadowhoof1 = new() { Id = 4433, Name = "Zhed Shadowhoof", WikiUrl = "https://wiki.guildwars.com/wiki/Zhed_Shadowhoof" };
    public static readonly Npc ZhedShadowhoof2 = new() { Id = 4434, Name = "Zhed Shadowhoof", WikiUrl = "https://wiki.guildwars.com/wiki/Zhed_Shadowhoof" };
    public static readonly Npc ZhedShadowhoof3 = new() { Id = 4435, Name = "Zhed Shadowhoof", WikiUrl = "https://wiki.guildwars.com/wiki/Zhed_Shadowhoof" };
    public static readonly Npc GeneralMorgahn1 = new() { Id = 4439, Name = "General Morgahn", WikiUrl = "https://wiki.guildwars.com/wiki/General_Morgahn" };
    public static readonly Npc GeneralMorgahn2 = new() { Id = 4440, Name = "General Morgahn", WikiUrl = "https://wiki.guildwars.com/wiki/General_Morgahn" };
    public static readonly Npc GeneralMorgahn3 = new() { Id = 4441, Name = "General Morgahn", WikiUrl = "https://wiki.guildwars.com/wiki/General_Morgahn" };
    public static readonly Npc MargridtheSly1 = new() { Id = 4444, Name = "Margrid the Sly", WikiUrl = "https://wiki.guildwars.com/wiki/Margrid_the_Sly" };
    public static readonly Npc MargridtheSly2 = new() { Id = 4445, Name = "Margrid the Sly", WikiUrl = "https://wiki.guildwars.com/wiki/Margrid_the_Sly" };
    public static readonly Npc MargridtheSly3 = new() { Id = 4446, Name = "Margrid the Sly", WikiUrl = "https://wiki.guildwars.com/wiki/Margrid_the_Sly" };
    public static readonly Npc Tahlkora1 = new() { Id = 4452, Name = "Tahlkora", WikiUrl = "https://wiki.guildwars.com/wiki/Tahlkora" };
    public static readonly Npc Tahlkora2 = new() { Id = 4453, Name = "Tahlkora", WikiUrl = "https://wiki.guildwars.com/wiki/Tahlkora" };
    public static readonly Npc Tahlkora3 = new() { Id = 4454, Name = "Tahlkora", WikiUrl = "https://wiki.guildwars.com/wiki/Tahlkora" };
    public static readonly Npc MasterofWhispers1 = new() { Id = 4457, Name = "Master of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Master_of_Whispers" };
    public static readonly Npc MasterofWhispers2 = new() { Id = 4458, Name = "Master of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Master_of_Whispers" };
    public static readonly Npc MasterofWhispers3 = new() { Id = 4459, Name = "Master of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Master_of_Whispers" };
    public static readonly Npc AcolyteJin1 = new() { Id = 4462, Name = "Acolyte Jin", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Jin" };
    public static readonly Npc AcolyteJin2 = new() { Id = 4463, Name = "Acolyte Jin", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Jin" };
    public static readonly Npc AcolyteJin3 = new() { Id = 4464, Name = "Acolyte Jin", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Jin" };
    public static readonly Npc Koss3 = new() { Id = 4468, Name = "Koss", WikiUrl = "https://wiki.guildwars.com/wiki/Koss" };
    public static readonly Npc Koss4 = new() { Id = 4469, Name = "Koss", WikiUrl = "https://wiki.guildwars.com/wiki/Koss" };
    public static readonly Npc Koss5 = new() { Id = 4470, Name = "Koss", WikiUrl = "https://wiki.guildwars.com/wiki/Koss" };
    public static readonly Npc Dunkoro1 = new() { Id = 4476, Name = "Dunkoro", WikiUrl = "https://wiki.guildwars.com/wiki/Dunkoro" };
    public static readonly Npc Dunkoro2 = new() { Id = 4477, Name = "Dunkoro", WikiUrl = "https://wiki.guildwars.com/wiki/Dunkoro" };
    public static readonly Npc Dunkoro3 = new() { Id = 4478, Name = "Dunkoro", WikiUrl = "https://wiki.guildwars.com/wiki/Dunkoro" };
    public static readonly Npc AcolyteSousuke1 = new() { Id = 4483, Name = "Acolyte Sousuke", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Sousuke" };
    public static readonly Npc AcolyteSousuke2 = new() { Id = 4484, Name = "Acolyte Sousuke", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Sousuke" };
    public static readonly Npc AcolyteSousuke3 = new() { Id = 4485, Name = "Acolyte Sousuke", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Sousuke" };
    public static readonly Npc Melonni1 = new() { Id = 4489, Name = "Melonni", WikiUrl = "https://wiki.guildwars.com/wiki/Melonni" };
    public static readonly Npc Melonni2 = new() { Id = 4490, Name = "Melonni", WikiUrl = "https://wiki.guildwars.com/wiki/Melonni" };
    public static readonly Npc Melonni3 = new() { Id = 4491, Name = "Melonni", WikiUrl = "https://wiki.guildwars.com/wiki/Melonni" };
    public static readonly Npc Razah1 = new() { Id = 4495, Name = "Razah", WikiUrl = "https://wiki.guildwars.com/wiki/Razah" };
    public static readonly Npc Razah2 = new() { Id = 4503, Name = "Razah", WikiUrl = "https://wiki.guildwars.com/wiki/Razah" };
    public static readonly Npc Razah3 = new() { Id = 4504, Name = "Razah", WikiUrl = "https://wiki.guildwars.com/wiki/Razah" };
    public static readonly Npc Razah4 = new() { Id = 4505, Name = "Razah", WikiUrl = "https://wiki.guildwars.com/wiki/Razah" };
    public static readonly Npc Vekk2 = new() { Id = 5905, Name = "Vekk", WikiUrl = "https://wiki.guildwars.com/wiki/Vekk" };
    public static readonly Npc Vekk3 = new() { Id = 5906, Name = "Vekk", WikiUrl = "https://wiki.guildwars.com/wiki/Vekk" };
    public static readonly Npc Vekk4 = new() { Id = 5907, Name = "Vekk", WikiUrl = "https://wiki.guildwars.com/wiki/Vekk" };
    public static readonly Npc PyreFierceshot = new() { Id = 5915, Name = "Pyre Fierceshot", WikiUrl = "https://wiki.guildwars.com/wiki/Pyre_Fierceshot" };
    public static readonly Npc PyreFierceshot1 = new() { Id = 5916, Name = "Pyre Fierceshot", WikiUrl = "https://wiki.guildwars.com/wiki/Pyre_Fierceshot" };
    public static readonly Npc PyreFierceshot2 = new() { Id = 5917, Name = "Pyre Fierceshot", WikiUrl = "https://wiki.guildwars.com/wiki/Pyre_Fierceshot" };
    public static readonly Npc OgdenStonehealer2 = new() { Id = 5924, Name = "Ogden Stonehealer", WikiUrl = "https://wiki.guildwars.com/wiki/Ogden_Stonehealer" };
    public static readonly Npc OgdenStonehealer3 = new() { Id = 5925, Name = "Ogden Stonehealer", WikiUrl = "https://wiki.guildwars.com/wiki/Ogden_Stonehealer" };
    public static readonly Npc OgdenStonehealer4 = new() { Id = 5926, Name = "Ogden Stonehealer", WikiUrl = "https://wiki.guildwars.com/wiki/Ogden_Stonehealer" };
    public static readonly Npc Anton = new() { Id = 5933, Name = "Anton", WikiUrl = "https://wiki.guildwars.com/wiki/Anton" };
    public static readonly Npc Anton1 = new() { Id = 5934, Name = "Anton", WikiUrl = "https://wiki.guildwars.com/wiki/Anton" };
    public static readonly Npc Anton2 = new() { Id = 5935, Name = "Anton", WikiUrl = "https://wiki.guildwars.com/wiki/Anton" };
    public static readonly Npc Livia = new() { Id = 5940, Name = "Livia", WikiUrl = "https://wiki.guildwars.com/wiki/Livia" };
    public static readonly Npc Livia1 = new() { Id = 5941, Name = "Livia", WikiUrl = "https://wiki.guildwars.com/wiki/Livia" };
    public static readonly Npc Livia2 = new() { Id = 5942, Name = "Livia", WikiUrl = "https://wiki.guildwars.com/wiki/Livia" };
    public static readonly Npc Hayda = new() { Id = 5948, Name = "Hayda", WikiUrl = "https://wiki.guildwars.com/wiki/Hayda" };
    public static readonly Npc Hayda1 = new() { Id = 5949, Name = "Hayda", WikiUrl = "https://wiki.guildwars.com/wiki/Hayda" };
    public static readonly Npc Hayda2 = new() { Id = 5950, Name = "Hayda", WikiUrl = "https://wiki.guildwars.com/wiki/Hayda" };
    public static readonly Npc Kahmu = new() { Id = 5955, Name = "Kahmu", WikiUrl = "https://wiki.guildwars.com/wiki/Kahmu" };
    public static readonly Npc Kahmu1 = new() { Id = 5956, Name = "Kahmu", WikiUrl = "https://wiki.guildwars.com/wiki/Kahmu" };
    public static readonly Npc Kahmu2 = new() { Id = 5957, Name = "Kahmu", WikiUrl = "https://wiki.guildwars.com/wiki/Kahmu" };
    public static readonly Npc Gwen = new() { Id = 5962, Name = "Gwen", WikiUrl = "https://wiki.guildwars.com/wiki/Gwen" };
    public static readonly Npc Gwen1 = new() { Id = 5963, Name = "Gwen", WikiUrl = "https://wiki.guildwars.com/wiki/Gwen" };
    public static readonly Npc Gwen2 = new() { Id = 5964, Name = "Gwen", WikiUrl = "https://wiki.guildwars.com/wiki/Gwen" };
    public static readonly Npc Xandra = new() { Id = 5970, Name = "Xandra", WikiUrl = "https://wiki.guildwars.com/wiki/Xandra" };
    public static readonly Npc Xandra1 = new() { Id = 5971, Name = "Xandra", WikiUrl = "https://wiki.guildwars.com/wiki/Xandra" };
    public static readonly Npc Xandra2 = new() { Id = 5972, Name = "Xandra", WikiUrl = "https://wiki.guildwars.com/wiki/Xandra" };
    public static readonly Npc Jora = new() { Id = 5975, Name = "Jora", WikiUrl = "https://wiki.guildwars.com/wiki/Jora" };
    public static readonly Npc Jora1 = new() { Id = 5976, Name = "Jora", WikiUrl = "https://wiki.guildwars.com/wiki/Jora" };
    public static readonly Npc Jora2 = new() { Id = 5977, Name = "Jora", WikiUrl = "https://wiki.guildwars.com/wiki/Jora" };
    public static readonly Npc KeiranThackeray = new() { Id = 8380, Name = "Keiran Thackeray", WikiUrl = "https://wiki.guildwars.com/wiki/Keiran_Thackeray" };
    public static readonly Npc Miku = new() { Id = 8958, Name = "Miku", WikiUrl = "https://wiki.guildwars.com/wiki/Miku" };
    public static readonly Npc ZeiRi = new() { Id = 8962, Name = "Zei Ri", WikiUrl = "https://wiki.guildwars.com/wiki/Zei_Ri" };
    public static readonly Npc CharrShaman = new() { Id = 230, Name = "Charr Shaman", WikiUrl = "https://wiki.guildwars.com/wiki/Charr_Shaman" };
    public static readonly Npc BoneDragon = new() { Id = 231, Name = "Bone Dragon", WikiUrl = "https://wiki.guildwars.com/wiki/Bone_Dragon" };
    public static readonly Npc PrinceRurik = new() { Id = 232, Name = "Prince Rurik", WikiUrl = "https://wiki.guildwars.com/wiki/Prince_Rurik" };
    public static readonly Npc Shiro = new() { Id = 233, Name = "Shiro", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro" };
    public static readonly Npc BurningTitan = new() { Id = 234, Name = "Burning Titan", WikiUrl = "https://wiki.guildwars.com/wiki/Burning_Titan" };
    public static readonly Npc Kirin = new() { Id = 235, Name = "Kirin", WikiUrl = "https://wiki.guildwars.com/wiki/Kirin" };
    public static readonly Npc NecridHorseman = new() { Id = 236, Name = "Necrid Horseman", WikiUrl = "https://wiki.guildwars.com/wiki/Necrid_Horseman" };
    public static readonly Npc JadeArmor = new() { Id = 237, Name = "Jade Armor", WikiUrl = "https://wiki.guildwars.com/wiki/Jade_Armor" };
    public static readonly Npc Hydra = new() { Id = 238, Name = "Hydra", WikiUrl = "https://wiki.guildwars.com/wiki/Hydra" };
    public static readonly Npc FungalWallow1 = new() { Id = 239, Name = "Fungal Wallow", WikiUrl = "https://wiki.guildwars.com/wiki/Fungal_Wallow" };
    public static readonly Npc SiegeTurtle3 = new() { Id = 240, Name = "Siege Turtle", WikiUrl = "https://wiki.guildwars.com/wiki/Siege_Turtle" };
    public static readonly Npc TempleGuardian2 = new() { Id = 241, Name = "Temple Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Temple_Guardian" };
    public static readonly Npc JungleTroll = new() { Id = 242, Name = "Jungle Troll", WikiUrl = "https://wiki.guildwars.com/wiki/Jungle_Troll" };
    public static readonly Npc WhiptailDevourer = new() { Id = 243, Name = "Whiptail Devourer", WikiUrl = "https://wiki.guildwars.com/wiki/Whiptail_Devourer" };
    public static readonly Npc Gwen3 = new() { Id = 244, Name = "Gwen", WikiUrl = "https://wiki.guildwars.com/wiki/Gwen" };
    public static readonly Npc GwenDoll = new() { Id = 245, Name = "Gwen Doll", WikiUrl = "https://wiki.guildwars.com/wiki/Gwen_Doll" };
    public static readonly Npc WaterDjinn3 = new() { Id = 246, Name = "Water Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Water_Djinn" };
    public static readonly Npc Lich = new() { Id = 247, Name = "Lich", WikiUrl = "https://wiki.guildwars.com/wiki/Lich" };
    public static readonly Npc Elf = new() { Id = 248, Name = "Elf", WikiUrl = "https://wiki.guildwars.com/wiki/Elf" };
    public static readonly Npc PalawaJoko3 = new() { Id = 249, Name = "Palawa Joko", WikiUrl = "https://wiki.guildwars.com/wiki/Palawa_Joko" };
    public static readonly Npc Koss6 = new() { Id = 250, Name = "Koss", WikiUrl = "https://wiki.guildwars.com/wiki/Koss" };
    public static readonly Npc MandragorImp4 = new() { Id = 251, Name = "Mandragor Imp", WikiUrl = "https://wiki.guildwars.com/wiki/Mandragor_Imp" };
    public static readonly Npc HeketWarrior = new() { Id = 252, Name = "Heket Warrior", WikiUrl = "https://wiki.guildwars.com/wiki/Heket_Warrior" };
    public static readonly Npc HarpyRanger = new() { Id = 253, Name = "Harpy Ranger", WikiUrl = "https://wiki.guildwars.com/wiki/Harpy_Ranger" };
    public static readonly Npc Juggernaut = new() { Id = 254, Name = "Juggernaut", WikiUrl = "https://wiki.guildwars.com/wiki/Juggernaut" };
    public static readonly Npc WindRider = new() { Id = 255, Name = "Wind Rider", WikiUrl = "https://wiki.guildwars.com/wiki/Wind_Rider" };
    public static readonly Npc FireImp = new() { Id = 256, Name = "Fire Imp", WikiUrl = "https://wiki.guildwars.com/wiki/Fire_Imp" };
    public static readonly Npc Aaxte = new() { Id = 257, Name = "Aaxte", WikiUrl = "https://wiki.guildwars.com/wiki/Aaxte" };
    public static readonly Npc ThornWolf1 = new() { Id = 258, Name = "Thorn Wolf", WikiUrl = "https://wiki.guildwars.com/wiki/Thorn_Wolf" };
    public static readonly Npc Abyssal1 = new() { Id = 259, Name = "Abyssal", WikiUrl = "https://wiki.guildwars.com/wiki/Abyssal" };
    public static readonly Npc BlackBeastofAaaaarrrrrrggghhh = new() { Id = 260, Name = "Black Beast of Aaaaarrrrrrggghhh", WikiUrl = "https://wiki.guildwars.com/wiki/Black_Beast_of_Aaaaarrrrrrggghhh_(summon)" };
    public static readonly Npc Freezie = new() { Id = 261, Name = "Freezie", WikiUrl = "https://wiki.guildwars.com/wiki/Freezie" };
    public static readonly Npc Irukandji2 = new() { Id = 262, Name = "Irukandji", WikiUrl = "https://wiki.guildwars.com/wiki/Irukandji" };
    public static readonly Npc MadKingThorn = new() { Id = 263, Name = "Mad King Thorn", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_King_Thorn" };
    public static readonly Npc ForestMinotaur = new() { Id = 264, Name = "Forest Minotaur", WikiUrl = "https://wiki.guildwars.com/wiki/Forest_Minotaur" };
    public static readonly Npc Mursaat = new() { Id = 265, Name = "Mursaat", WikiUrl = "https://wiki.guildwars.com/wiki/Mursaat" };
    public static readonly Npc Nornbear = new() { Id = 266, Name = "Nornbear", WikiUrl = "https://wiki.guildwars.com/wiki/Nornbear" };
    public static readonly Npc Ooze = new() { Id = 267, Name = "Ooze", WikiUrl = "https://wiki.guildwars.com/wiki/Ooze" };
    public static readonly Npc Raptor = new() { Id = 268, Name = "Raptor", WikiUrl = "https://wiki.guildwars.com/wiki/Raptor" };
    public static readonly Npc RoaringEther2 = new() { Id = 269, Name = "Roaring Ether", WikiUrl = "https://wiki.guildwars.com/wiki/Roaring_Ether" };
    public static readonly Npc CloudtouchedSimian = new() { Id = 270, Name = "Cloudtouched Simian", WikiUrl = "https://wiki.guildwars.com/wiki/Cloudtouched_Simian" };
    public static readonly Npc CaveSpider = new() { Id = 271, Name = "Cave Spider", WikiUrl = "https://wiki.guildwars.com/wiki/Cave_Spider" };
    public static readonly Npc WhiteRabbit = new() { Id = 272, Name = "White Rabbit", WikiUrl = "https://wiki.guildwars.com/wiki/White_Rabbit" };
    public static readonly Npc WordofMadness2 = new() { Id = 273, Name = "Word of Madness", WikiUrl = "https://wiki.guildwars.com/wiki/Word_of_Madness" };
    public static readonly Npc DredgeBrute = new() { Id = 274, Name = "Dredge Brute", WikiUrl = "https://wiki.guildwars.com/wiki/Dredge_Brute" };
    public static readonly Npc TerrorwebDryder1 = new() { Id = 275, Name = "Terrorweb Dryder", WikiUrl = "https://wiki.guildwars.com/wiki/Terrorweb_Dryder" };
    public static readonly Npc Abomination = new() { Id = 276, Name = "Abomination", WikiUrl = "https://wiki.guildwars.com/wiki/Abomination" };
    public static readonly Npc KraitNeoss = new() { Id = 277, Name = "Krait Neoss", WikiUrl = "https://wiki.guildwars.com/wiki/Krait_Neoss" };
    public static readonly Npc DesertGriffon = new() { Id = 278, Name = "Desert Griffon", WikiUrl = "https://wiki.guildwars.com/wiki/Desert_Griffon" };
    public static readonly Npc Kveldulf = new() { Id = 279, Name = "Kveldulf", WikiUrl = "https://wiki.guildwars.com/wiki/Kveldulf" };
    public static readonly Npc QuetzalSly = new() { Id = 280, Name = "Quetzal Sly", WikiUrl = "https://wiki.guildwars.com/wiki/Quetzal_Sly" };
    public static readonly Npc Jora3 = new() { Id = 281, Name = "Jora", WikiUrl = "https://wiki.guildwars.com/wiki/Jora" };
    public static readonly Npc FlowstoneElemental = new() { Id = 282, Name = "Flowstone Elemental", WikiUrl = "https://wiki.guildwars.com/wiki/Flowstone_Elemental" };
    public static readonly Npc Nian = new() { Id = 283, Name = "Nian", WikiUrl = "https://wiki.guildwars.com/wiki/Nian" };
    public static readonly Npc DagnarStonepate = new() { Id = 284, Name = "Dagnar Stonepate", WikiUrl = "https://wiki.guildwars.com/wiki/Dagnar_Stonepate" };
    public static readonly Npc FlameDjinn = new() { Id = 285, Name = "Flame Djinn", WikiUrl = "https://wiki.guildwars.com/wiki/Flame_Djinn" };
    public static readonly Npc EyeofJanthir = new() { Id = 286, Name = "Eye of Janthir", WikiUrl = "https://wiki.guildwars.com/wiki/Eye_of_Janthir" };
    public static readonly Npc Seer = new() { Id = 287, Name = "Seer", WikiUrl = "https://wiki.guildwars.com/wiki/Seer" };
    public static readonly Npc SiegeDevourer = new() { Id = 288, Name = "Siege Devourer", WikiUrl = "https://wiki.guildwars.com/wiki/Siege_Devourer" };
    public static readonly Npc ShardWolf = new() { Id = 289, Name = "Shard Wolf", WikiUrl = "https://wiki.guildwars.com/wiki/Shard_Wolf" };
    public static readonly Npc FireDrake = new() { Id = 290, Name = "Fire Drake", WikiUrl = "https://wiki.guildwars.com/wiki/Fire_Drake" };
    public static readonly Npc SummitGiantHerder = new() { Id = 291, Name = "Summit Giant Herder", WikiUrl = "https://wiki.guildwars.com/wiki/Summit_Giant_Herder" };
    public static readonly Npc OphilNahualli = new() { Id = 292, Name = "Ophil Nahualli", WikiUrl = "https://wiki.guildwars.com/wiki/Ophil_Nahualli" };
    public static readonly Npc CobaltScabara1 = new() { Id = 293, Name = "Cobalt Scabara", WikiUrl = "https://wiki.guildwars.com/wiki/Cobalt_Scabara" };
    public static readonly Npc ScourgeManta1 = new() { Id = 294, Name = "Scourge Manta", WikiUrl = "https://wiki.guildwars.com/wiki/Scourge_Manta" };
    public static readonly Npc Ventari = new() { Id = 295, Name = "Ventari", WikiUrl = "https://wiki.guildwars.com/wiki/Ventari" };
    public static readonly Npc Oola = new() { Id = 296, Name = "Oola", WikiUrl = "https://wiki.guildwars.com/wiki/Oola" };
    public static readonly Npc CandysmithMarley = new() { Id = 297, Name = "Candysmith Marley", WikiUrl = "https://wiki.guildwars.com/wiki/Candysmith_Marley" };
    public static readonly Npc ZhuHanuku1 = new() { Id = 298, Name = "Zhu Hanuku", WikiUrl = "https://wiki.guildwars.com/wiki/Zhu_Hanuku" };
    public static readonly Npc KingAdelbern = new() { Id = 299, Name = "King Adelbern", WikiUrl = "https://wiki.guildwars.com/wiki/King_Adelbern" };
    public static readonly Npc MOX3 = new() { Id = 300, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc MOX4 = new() { Id = 301, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc MOX5 = new() { Id = 302, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc MOX6 = new() { Id = 303, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc MOX7 = new() { Id = 304, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc MOX8 = new() { Id = 305, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X." };
    public static readonly Npc BrownRabbit = new() { Id = 306, Name = "Brown Rabbit", WikiUrl = "https://wiki.guildwars.com/wiki/Brown_Rabbit" };
    public static readonly Npc Yakkington = new() { Id = 307, Name = "Yakkington", WikiUrl = "https://wiki.guildwars.com/wiki/Yakkington" };
    public static readonly Npc Kuunavang2 = new() { Id = 309, Name = "Kuunavang", WikiUrl = "https://wiki.guildwars.com/wiki/Kuunavang" };
    public static readonly Npc GrayGiant = new() { Id = 310, Name = "Gray Giant", WikiUrl = "https://wiki.guildwars.com/wiki/Gray_Giant" };
    public static readonly Npc Asura = new() { Id = 311, Name = "Asura", WikiUrl = "https://wiki.guildwars.com/wiki/Asura" };
    public static readonly Npc DestroyerofFlesh1 = new() { Id = 312, Name = "Destroyer of Flesh", WikiUrl = "https://wiki.guildwars.com/wiki/Destroyer_of_Flesh" };
    public static readonly Npc PolarBear = new() { Id = 313, Name = "Polar Bear", WikiUrl = "https://wiki.guildwars.com/wiki/Polar_Bear" };
    public static readonly Npc VareshOssa1 = new() { Id = 314, Name = "Varesh Ossa", WikiUrl = "https://wiki.guildwars.com/wiki/Varesh_Ossa" };
    public static readonly Npc Mallyx = new() { Id = 315, Name = "Mallyx", WikiUrl = "https://wiki.guildwars.com/wiki/Mallyx" };
    public static readonly Npc Ceratadon = new() { Id = 316, Name = "Ceratadon", WikiUrl = "https://wiki.guildwars.com/wiki/Ceratadon" };
    public static readonly Npc Kanaxai1 = new() { Id = 317, Name = "Kanaxai", WikiUrl = "https://wiki.guildwars.com/wiki/Kanaxai" };
    public static readonly Npc Panda = new() { Id = 318, Name = "Panda", WikiUrl = "https://wiki.guildwars.com/wiki/Panda" };
    public static readonly Npc IslandGuardian1 = new() { Id = 319, Name = "Island Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Island_Guardian" };
    public static readonly Npc NagaRaincaller1 = new() { Id = 320, Name = "Naga Raincaller", WikiUrl = "https://wiki.guildwars.com/wiki/Naga_Raincaller" };
    public static readonly Npc LonghairYeti1 = new() { Id = 321, Name = "Longhair Yeti", WikiUrl = "https://wiki.guildwars.com/wiki/Longhair_Yeti" };
    public static readonly Npc Oni5 = new() { Id = 322, Name = "Oni", WikiUrl = "https://wiki.guildwars.com/wiki/Oni" };
    public static readonly Npc ShirokenAssassin3 = new() { Id = 323, Name = "Shiro’ken Assassin", WikiUrl = "https://wiki.guildwars.com/wiki/Shiro'ken_Assassin" };
    public static readonly Npc Vizu1 = new() { Id = 324, Name = "Vizu", WikiUrl = "https://wiki.guildwars.com/wiki/Vizu" };
    public static readonly Npc ZhedShadowhoof4 = new() { Id = 325, Name = "Zhed Shadowhoof", WikiUrl = "https://wiki.guildwars.com/wiki/Zhed_Shadowhoof" };
    public static readonly Npc Grawl = new() { Id = 326, Name = "Grawl", WikiUrl = "https://wiki.guildwars.com/wiki/Grawl" };
    public static readonly Npc GhostlyHero = new() { Id = 327, Name = "Ghostly Hero", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Hero" };
    public static readonly Npc Pig = new() { Id = 328, Name = "Pig", WikiUrl = "https://wiki.guildwars.com/wiki/Pig" };
    public static readonly Npc GreasedLightning = new() { Id = 329, Name = "Greased Lightning", WikiUrl = "https://wiki.guildwars.com/wiki/Greased_Lightning" };
    public static readonly Npc WorldFamousRacingBeetle = new() { Id = 330, Name = "World-Famous Racing Beetle", WikiUrl = "https://wiki.guildwars.com/wiki/World-Famous_Racing_Beetle" };
    public static readonly Npc CelestialPig = new() { Id = 331, Name = "Celestial Pig", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Pig" };
    public static readonly Npc CelestialRat = new() { Id = 332, Name = "Celestial Rat", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Rat" };
    public static readonly Npc CelestialOx = new() { Id = 333, Name = "Celestial Ox", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Ox" };
    public static readonly Npc CelestialTiger = new() { Id = 334, Name = "Celestial Tiger", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Tiger" };
    public static readonly Npc CelestialRabbit = new() { Id = 335, Name = "Celestial Rabbit", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Rabbit" };
    public static readonly Npc CelestialDragon = new() { Id = 336, Name = "Celestial Dragon", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Dragon" };
    public static readonly Npc CelestialSnake = new() { Id = 337, Name = "Celestial Snake", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Snake" };
    public static readonly Npc CelestialHorse = new() { Id = 338, Name = "Celestial Horse", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Horse" };
    public static readonly Npc CelestialSheep = new() { Id = 339, Name = "Celestial Sheep", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Sheep" };
    public static readonly Npc CelestialMonkey = new() { Id = 340, Name = "Celestial Monkey", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Monkey" };
    public static readonly Npc CelestialRooster = new() { Id = 341, Name = "Celestial Rooster", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Rooster" };
    public static readonly Npc CelestialDog = new() { Id = 342, Name = "Celestial Dog", WikiUrl = "https://wiki.guildwars.com/wiki/Celestial_Dog" };
    public static readonly Npc BlackMoaChick = new() { Id = 343, Name = "Black Moa Chick", WikiUrl = "https://wiki.guildwars.com/wiki/Black_Moa_Chick" };
    public static readonly Npc Dhuum = new() { Id = 344, Name = "Dhuum", WikiUrl = "https://wiki.guildwars.com/wiki/Dhuum" };
    public static readonly Npc MadKingsGuard = new() { Id = 345, Name = "Mad King’s Guard", WikiUrl = "https://wiki.guildwars.com/wiki/Mad_King's_Guard" };
    public static readonly Npc SmiteCrawler = new() { Id = 346, Name = "Smite Crawler", WikiUrl = "https://wiki.guildwars.com/wiki/Smite_Crawler" };
    public static readonly Npc GuildLord = new() { Id = 347, Name = "Guild Lord", WikiUrl = "https://wiki.guildwars.com/wiki/Guild_Lord" };
    public static readonly Npc HighPriestZhang = new() { Id = 348, Name = "High Priest Zhang", WikiUrl = "https://wiki.guildwars.com/wiki/High_Priest_Zhang" };
    public static readonly Npc GhostlyPriest1 = new() { Id = 349, Name = "Ghostly Priest", WikiUrl = "https://wiki.guildwars.com/wiki/Ghostly_Priest" };
    public static readonly Npc RiftWarden = new() { Id = 350, Name = "Rift Warden", WikiUrl = "https://wiki.guildwars.com/wiki/Rift_Warden" };
    public static readonly Npc Legionnaire = new() { Id = 7980, Name = "Legionnaire", WikiUrl = "https://wiki.guildwars.com/wiki/Legionnaire" };
    public static readonly Npc ConfessorDorian = new() { Id = 8289, Name = "Confessor Dorian", WikiUrl = "https://wiki.guildwars.com/wiki/Confessor_Dorian" };
    public static readonly Npc PrincessSalma = new() { Id = 8294, Name = "Princess Salma", WikiUrl = "https://wiki.guildwars.com/wiki/Princess_Salma" };
    public static readonly Npc Livia3 = new() { Id = 8295, Name = "Livia", WikiUrl = "https://wiki.guildwars.com/wiki/Livia" };
    public static readonly Npc Evennia = new() { Id = 8296, Name = "Evennia", WikiUrl = "https://wiki.guildwars.com/wiki/Evennia" };
    public static readonly Npc ConfessorIsaish = new() { Id = 8297, Name = "Confessor Isaish", WikiUrl = "https://wiki.guildwars.com/wiki/Confessor_Isaish" };
    public static readonly Npc PeacekeeperEnforcer = new() { Id = 8299, Name = "Peacekeeper Enforcer", WikiUrl = "https://wiki.guildwars.com/wiki/Peacekeeper_Enforcer" };
    public static readonly Npc MinisterReiko = new() { Id = 8983, Name = "Minister Reiko", WikiUrl = "https://wiki.guildwars.com/wiki/Minister_Reiko" };
    public static readonly Npc EcclesiateXunRao = new() { Id = 8984, Name = "Ecclesiate Xun Rao", WikiUrl = "https://wiki.guildwars.com/wiki/Ecclesiate_Xun_Rao" };
    public static readonly Npc TheFrog = new() { Id = -1, Name = "The Frog", WikiUrl = "https://wiki.guildwars.com/wiki/The_Frog" };
    public static readonly Npc TheFrog1 = new() { Id = -1, Name = "The Frog", WikiUrl = "https://wiki.guildwars.com/wiki/The_Frog" };
    public static readonly Npc TheFrog2 = new() { Id = -1, Name = "The Frog", WikiUrl = "https://wiki.guildwars.com/wiki/The_Frog" };


    public static List<Npc> Npcs { get; } = new()
    {
        Unknown,
        ZaishenRepresentative,
        CanthanAmbassador,
        PriestofBalthazar,
        Tolkano,
        XunlaiAgentHonlo,
        XunlaiAgent,
        Hayate,
        Rabbit,
        DeckhandJakob,
        Bryan,
        CaptainAnder,
        Diane,
        Armian,
        Durmand,
        CaptainJoran,
        Ludor,
        Tuomas,
        MarinerKevan,
        KurzickElementalistRecruit,
        FireGolem,
        Crane,
        Tiger,
        Lurker,
        ReefLurker,
        WhiteTiger,
        Phoenix,
        ElderReefLurker,
        Hector,
        PetElderReefLurker,
        BlackMoa,
        ElderWhiteTiger,
        ElderCrane,
        ElderTiger,
        ElderReefLurker1,
        Klaus,
        KurzickMesmerRecruit,
        KurzickMonkRecruit,
        KurzickRangerRecruit,
        KurzickWarriorRecruit,
        Leiber,
        Anya,
        ExRedemptorBerta,
        KurzickAssassinRecruit,
        KurzickNecromancerRecruit,
        KurzickRitualistRecruit,
        LuxonElementalistRecruit,
        LuxonNecromancerRecruit,
        LuxonRangerRecruit,
        LuxonWarriorRecruit,
        LuxonAssassinRecruit,
        LuxonMesmerRecruit,
        LuxonMonkRecruit,
        LuxonRitualistRecruit,
        CountDurheim,
        BaronessVasburg,
        ElderNagaWarrior,
        ElderNagaArcher,
        ElderNagaRitualist,
        LuxonPriest,
        OrangeCommander,
        PurpleCommander,
        MasterArchitectGunther,
        GatekeeperPoletski,
        GatekeeperRadik,
        KurzickJuggernaut,
        KurzickNecromancer,
        SiegeTurtle,
        LuxonWarrior,
        StoneJudge,
        Danika,
        Danika1,
        RaitahnNem,
        SoonKim,
        LiYun,
        MonkStudent,
        WarriorStudent,
        Hakaru,
        Hanjo,
        Siyan,
        Huan,
        Kam,
        Miju,
        Ako,
        Huan1,
        JatoroMusagi,
        YijoTahn,
        GullHookbeak,
        YrrgSnagtooth,
        GruutSnowfoot,
        TrrokStrongjaw,
        YorrtStrongjaw,
        TheBogBeastofBokku,
        BoneHorror,
        WildMinion,
        YijoTahn1,
        Ludo,
        Mai,
        Kisai,
        Taya,
        Lukas,
        InstructorNg,
        MasterTogo,
        DrinkmasterTahnu,
        Turimachus,
        BattlePriestCalibos,
        LuxonAdept,
        HeadmasterVhang,
        MasterTogo1,
        MinistersGuard,
        ZaishenScout,
        MasterTogo2,
        YijoTahn2,
        MerchantOrek,
        Merchant,
        KurzickIllusionist,
        LuxonWizard,
        KurzickThunder,
        LuxonStormCaller,
        KurzickFarShot,
        LuxonLongbow,
        KommandantDurheim,
        KurzickBaseDefender,
        Erek,
        LuxonBaseDefender,
        ShadyLuxon,
        SuspiciousKurzick,
        HeiTsu,
        ZhuHanuku,
        Mai1,
        Kisai1,
        Yuun,
        Aeson,
        Lukas1,
        Argo,
        Cynn,
        Danika2,
        MasterTogo3,
        MasterTogo4,
        BrotherMhenlo,
        Nika,
        Panaku,
        TalonSilverwing,
        CelestialHorror,
        MasterTogo5,
        Jaizhanju,
        Teinai,
        Karei,
        Naku,
        MasterTogo6,
        Togo,
        BrotherMhenlo1,
        ShadowBladeAssassin,
        Kiishen,
        ObsidianFlameAssassin,
        KeFeng,
        Taya1,
        ProfessorGai,
        SisterTai,
        MeiLing,
        LoSha,
        Su,
        TenguCutter,
        Lukas2,
        ErysVasburg,
        Aeson1,
        Daemen,
        Aurora,
        Hala,
        SeaguardGita,
        SeaguardEli,
        BrotherMhenlo2,
        Nika1,
        Jamei,
        Chiyo,
        Emi,
        Devona,
        Cynn1,
        Aidan,
        Eve,
        KaiYing,
        Zho,
        Panaku1,
        HeadmasterVhang1,
        Mai2,
        Kisai2,
        Yuun1,
        TalonSilverwing1,
        Bakghu,
        Ohtah,
        Kegai,
        Hannai,
        Arani,
        Caly,
        Ting,
        Khim,
        Rion,
        Sao,
        TheAfflictedHuu,
        TheAfflictedMeeka,
        TheAfflictedXi,
        TheAfflictedRasa,
        TheAfflictedCho,
        TheAfflictedTamaya,
        TheAfflictedSusei,
        TheAfflictedMei,
        GuardsmanChienpo,
        GuardsmanPing,
        ImperialGuardsmanLinro,
        ImperialAgentHanjo,
        ImperialGuardsmanKintae,
        MinistryGuard,
        ShaeWong,
        Kakumei,
        IzaYoi,
        Ryoko,
        Suki,
        Natsuko,
        Ryukichi,
        Manzo,
        Kumiko,
        XueYi,
        XueFang,
        Yutake,
        JiazhenLiMaterial,
        ZhouPakScroll,
        Mitsunari,
        GuildmasterLuan,
        MinisterJaisan,
        PoFang,
        Michiko,
        Akoto,
        MinisterNai,
        MinisterZal,
        JinSiyan,
        BarkeepMehoro,
        Bujo,
        EmperorKisu,
        Kitah,
        Zojun,
        Kaolai,
        MasterTogo7,
        Togo1,
        BrotherMhenlo3,
        Nika2,
        Jamei1,
        Chiyo1,
        Emi1,
        Cynn2,
        TalonSilverwing2,
        TalonSilverwing3,
        Bauyun,
        CampGuardPongtu,
        CanthanGuard,
        CaptainLoFah,
        DockhandQuangqnai,
        GuardCaptainVassi,
        GuardLaonan,
        GuardRaabo,
        GuardTsukaro,
        GuardsmanAyoki,
        GuardsmanChienpo1,
        GuardsmanChow,
        GuardsmanKayao,
        GuardsmanKikuchiyo,
        GuardsmanMakuruyo,
        GuardsmanPah,
        GuardsmanPei,
        GuardsmanPing1,
        GuardsmanTang,
        GuardsmanZui,
        ImperialGuardKozoko,
        ImperialGuardsmanLinro1,
        ImperialGuardsmanYang,
        Laris,
        MonasteryQuartermaster,
        OffDutyGuard,
        PetraBrauer,
        RoyalGuard,
        SeungKim,
        ShenXiGuardRequisition,
        TempleGuardBai,
        Vargus,
        CanthanGuard1,
        GuardsmanPo,
        MinistersGuard1,
        ScoutShenfai,
        Suhkaro,
        CanthanBodyguard,
        CanthanGuard2,
        CanthanGuardCaptain,
        CaptainSei,
        CaptainZinghu,
        CommanderJafai,
        EmperorsVoice,
        GuardLaeFao,
        GuardsmanKenji,
        GuardsmanKinri,
        GuardsmanZingpah,
        ImperialAgentHanjo1,
        ImperialGuardRantoh,
        ImperialGuardsmanHanzing,
        ImperialGuardsmanKintae1,
        ImperialGuardsmanTingjo,
        MagistrateWakai,
        MinistryGuard1,
        RaitahnNem1,
        ShaeWong1,
        TheEmperorsBlade,
        CanthanGuardCaptain1,
        MagistrateRaisung,
        PalaceGuardTsungkim,
        RoyalGuardKazuya,
        CanthanGuard3,
        CorpseofCanthanGuard,
        AzumeGuardRequisition,
        CanthanGuard4,
        GuardsmanKeiko,
        ImperialGuide,
        ImperialQuartermaster,
        Kujin,
        MonasteryQuartermaster1,
        ReiMing,
        RoyalGuard1,
        ZanLeiGuardRequisition,
        CanthanGuard5,
        CanthanChild,
        LostBoy,
        Nei,
        CanthanChild1,
        Aiko,
        CanthanChild2,
        Kimi,
        Mitah,
        MusakoTuro,
        Akoto1,
        AttendantNashu,
        Axlan,
        CanthanNoble,
        CaptainQuimang,
        ChenPoChin,
        EmperorsHand,
        FirstMateXiang,
        ImperialChefYileng,
        JuniorMinisterJejiang,
        LoudKou,
        MessengerGosuh,
        MinisterBaasong,
        MinisterCho,
        MinisterKhannai,
        MinisterNai1,
        MinisterofMaintenanceRaiugyon,
        MinisterOnghsang,
        MinisterZal1,
        NoblemanKagita,
        OfficerChitaro,
        OracleofTime,
        Saito,
        Sakai,
        YanleiBruun,
        Yokuni,
        Akane,
        AttendantChien,
        AttendantHara,
        AttendantYoko,
        CanthanNoble1,
        RanTi,
        ZhaoDi,
        Budo,
        CanthanNoble2,
        GuildmasterLuan1,
        ImperialHerald,
        Kyuzo,
        Masaharu,
        MinisterJaisan1,
        MinisterTahlen,
        PoFang1,
        SekaitheMapmaker,
        TahwajZing,
        XiGai,
        XuFengxia,
        XuweiDiyi,
        Yiwong,
        ZiinyingmaoKaga,
        AdeptBishu,
        CanthanNoble3,
        HaiLae,
        Junsu,
        LadyMukeiMusagi,
        Michiko1,
        MinisterTao,
        Baosen,
        BarkeepMehoro1,
        CanthanPeasant,
        CanthanRefugee,
        HomelessCanthan,
        InformantTahzen,
        JeLing,
        Leijo,
        MarikKuri,
        MillerQuang,
        Nakai,
        Paoko,
        Raiyi,
        Refugee,
        SahnlaetheTamer,
        Shenzun,
        YanZal,
        Yikaro,
        Baasong,
        BaeKwon,
        Bujo1,
        CanthanPeasant1,
        CanthanRefugee1,
        CorpseofThiefMang,
        DaiWaeng,
        FishmongerBihzun,
        Gorobei,
        HoDim,
        LosaiHapatu,
        Morokam,
        Refugee1,
        SenjoWah,
        Sunzu,
        Taojo,
        Teizhan,
        CorpseofCanthanPeasant,
        CanthanPeasant2,
        Lintao,
        PoorBeggar,
        ZumotheBeggar,
        CanthanPeasant3,
        RansujuntheProphet,
        SongSanNok,
        CanthanPeasant4,
        CanthanPeasant5,
        FarmerXengJo,
        FarmerZinhao,
        NeingtheTanner,
        Yanjo,
        CanthanPeasant6,
        HaoZhang,
        YuimotheMime,
        Akemi,
        CanthanPeasant7,
        CanthanRefugee2,
        ChibichitheTamer,
        DebiroKuri,
        Faozun,
        HerderTsiyingju,
        HomelessCanthan1,
        JajeNiya,
        Jia,
        JinSiyan1,
        Kaya,
        Ling,
        Mangjo,
        Refugee2,
        YuLae,
        CorpseofCanthanPeasant1,
        CanthanPeasant8,
        Jiangpo,
        Qian,
        TanrieoTuro,
        ZinLao,
        CanthanPeasant9,
        JungZeng,
        TahboPaa,
        MiFai,
        CanthanPeasant10,
        Paomu,
        VillagerHijai,
        Yazoying,
        AdeptNai,
        AdeptofBone,
        AdeptofIllusion,
        AdeptofLight,
        AdeptofNature,
        AdeptofScythes,
        AdeptofShadows,
        AdeptofSpears,
        AdeptofSpirits,
        AdeptofSteel,
        AdeptoftheElements,
        AdeptTahn,
        BihahtheScribe,
        BrotherHanjui,
        BrotherKhaiJhong,
        BrotherSitai,
        BrotherTosai,
        Resongkai,
        TempleAcolyte,
        Tombo,
        AdeptKai,
        Shinichiro,
        Tsungfa,
        XueYi1,
        DaeChung,
        Kainu,
        Kumiko1,
        SisterChoiJu,
        TakaraTaji,
        Yunndae,
        AkinBroadcrest,
        PustEmberclaw,
        RoostEverclaw,
        FuguiGe,
        Kakumei1,
        Kambei,
        Koumei,
        NuLeng,
        Seiji,
        VoldotheExotic,
        Lain,
        Maeko,
        Maiya,
        MoonAhn,
        Oroku,
        Ryoko1,
        Suki1,
        WeiQi,
        XunlaiGiftGiverGurubei,
        FireworksMaster,
        DoctorJungsRemediesandPotions,
        ErMing,
        GiHahn,
        Golgo,
        GongMei,
        Masahiko,
        Morimoto,
        NaijusRemediesandPotions,
        PeLingsRemediesandPotions,
        RaiKazuRemediesandPotions,
        XangsRemediesandPotions,
        Hasung,
        Motiro,
        Ryukichi1,
        ChikyuShujin,
        HoJun,
        Inugami,
        CrawRazorbeak,
        MelkBrightfeather,
        ZenSwiftwing,
        Aiko1,
        Asako,
        ChifaoTan,
        Haruko,
        Huxeng,
        Jichow,
        KaiyaJaja,
        Kokiri,
        Lei,
        LoYing,
        Nanako,
        Natsuko1,
        Sen,
        Suki2,
        Buhraa,
        Hiroyuki,
        HongleiSun,
        LiuGong,
        Manzo1,
        Nago,
        Sheco,
        Shichiroji,
        TeipaoTahliwaj,
        Tsukare,
        Zingyao,
        HaiJeling,
        Taura,
        Ziyi,
        Senji,
        Vizu,
        Suun,
        Yohei,
        Yumiko,
        Akina,
        Oki,
        XueFang1,
        SihungLung,
        Yimou,
        Yutake1,
        SanLing,
        KyoukoMaterial,
        YongYiMaterial,
        JiazhenLiMaterial1,
        SonglianMaterial,
        ZhouPakScroll1,
        Mitsunari1,
        Okamoto,
        Yukito,
        Panaku2,
        Panaku3,
        VunYing,
        Jinzo,
        Jinzo1,
        Jinzo2,
        MeiLing1,
        LoSha1,
        Su1,
        RengKu,
        Ronsu,
        Ronsu1,
        KaiYing1,
        KaiYing2,
        SisterTai1,
        SisterTai2,
        BrotherPeWan,
        BrotherPeWan1,
        BrotherPeWan2,
        WengGha,
        WengGha1,
        SoarHonorclaw,
        Sujun,
        Sujun1,
        Zho1,
        Zho2,
        ProfessorGai1,
        ProfessorGai2,
        AngtheEphemeral,
        HeadmasterLee,
        HeadmasterKaa,
        HeadmasterKuju,
        HeadmasterVhang2,
        HeadmasterAmara,
        HeadmasterZhan,
        HeadmasterGreico,
        HeadmasterQuin,
        GeneralKaimerVasburg,
        KurzickJuggernaut1,
        KurzickAssassin,
        KurzickScout,
        KurzickMesmer,
        KurzickNecromancer1,
        KurzickElementalist,
        KurzickMonk,
        KurzickWarrior,
        KurzickRanger,
        KurzickRitualist,
        HouseDurheimDuelist,
        HouseDurheimDuelist1,
        HouseDurheimDuelist2,
        HouseDurheimDuelist3,
        GreenCarrierJuggernaut,
        GuardsmanOldrich,
        PurpleCarrierJuggernaut,
        YellowCarrierJuggernaut,
        EliteJuggernautBerta,
        EliteJuggernautKlaus,
        EliteJuggernautLeiber,
        KurzickAssassin1,
        KurzickMesmer1,
        KurzickElementalist1,
        KurzickWarrior1,
        KurzickRanger1,
        KurzickSoldier,
        KurzickSoldier1,
        TerrikzuHeltzer,
        TreeSinger,
        RaldzuHeltzer,
        ErikLutgardis,
        JohannzuHeltzer,
        KommandantDurheim1,
        KonradVasburg,
        RuprechtBrauer,
        RutgerzuHeltzer,
        StoneSingerDalf,
        StoneSingerJarek,
        StoneSingerKasia,
        StoneSingerLotte,
        StoneSingerMinka,
        StoneSingerOswalt,
        StoneSingerTreff,
        StoneSingerWira,
        DenosMakaluum,
        DmitriScharkoff,
        GatekeeperBecker,
        GatekeeperDedrick,
        GuardKarsten,
        GuardUwe,
        KurzickGuard,
        KurzickQuartermaster,
        LiftOperatorShulz,
        RecruiterSigmund,
        ScoutmasterArne,
        YuriVasburg,
        GuardCaptainMirkoz,
        SergeantGeinrich,
        SergeantRodrik,
        WarCaptainWomak,
        ForestermasterVasha,
        AlshomGhislaun,
        ArtorBobaniKiroz,
        HistorianOfHouses,
        JaunStumi,
        JonnTertehl,
        JustanWeiss,
        KurzickPeasant,
        KurzickTraveler,
        ChefArmand,
        DyeMasterFranjek,
        Gorani,
        KurzickTraveler1,
        LoremasterZin,
        PeltsmanJiri,
        VasiliLutgardis,
        WilhelmJoseph,
        GerlindaKorbauch,
        KurzickPeasant1,
        HealerSilja,
        Jun,
        KurzickPeasant2,
        KurzickTraveler2,
        AndruPitrak,
        CountDurheim1,
        DeaconFredek,
        DunmelGorhopf,
        Friedrich,
        KurzickArchitect,
        KurzickGatekeeper,
        Meinrad,
        Mirko,
        RedemptorIszar,
        SelikzuHeltzer,
        SupplymasterKonrad,
        ZarektheTamer,
        AdeilzuHeltzer,
        CountArchekBrauer,
        DuelMasterVaughn,
        DukeHoltzLutgardis,
        Englebert,
        KurzickNoble,
        QuartermasterMikhail,
        RedemptorKurcheck,
        Ruben,
        Vash,
        Vernados,
        BaronessAttiaVasburg,
        BrunazuHeltzer,
        CountessSandraDurheim,
        KurzickNoble1,
        MasterArchitectWright,
        NadettazuHeltzer,
        Zytka,
        Eileen,
        KurzickAmbassador,
        KurzickGatekeeper1,
        KurzickNoble2,
        AcolyteHanz,
        AcolyteJorg,
        BukDirayne,
        KurzickPriest,
        RedemptorKarl,
        ScholarAndrej,
        Ulrich,
        Giygas,
        Morbach,
        Bannan,
        EdmundGruca,
        Oles,
        Wendell,
        Wulfgar,
        Benman,
        Jurgen,
        Ludwik,
        Wilbur,
        Adele,
        Aiiane,
        Bathilda,
        Helene,
        Johanna,
        Linsle,
        Nadina,
        Rasia,
        KurzickMerchant,
        CecylBromka,
        MastersmithRutger,
        Gertrud,
        Mathilde,
        Aleksy,
        KristoffirRoi,
        Zefiryna,
        KurzickBureaucrat,
        Danika3,
        BaronMirekVasburg,
        BaronVasburg,
        CountzuHeltzer,
        Gretchen,
        RyszardMaterial,
        LeopoldScroll,
        AnastaciaScroll,
        Skye,
        KurzickPriest1,
        KurzickMerchant1,
        KurzickGuard1,
        KurzickGuard2,
        KurzickGuard3,
        KurzickRefugee,
        KurzickRefugee1,
        Mai3,
        Kisai3,
        Taya2,
        Lukas3,
        Yuun2,
        Aeson2,
        Mai4,
        Kisai4,
        Taya3,
        Lukas4,
        Yuun3,
        Aeson3,
        Mai5,
        Kisai5,
        Taya4,
        Lukas5,
        Yuun4,
        Aeson4,
        Mai6,
        Kisai6,
        Taya5,
        Lukas6,
        Yuun5,
        Aeson5,
        Panaku4,
        LoSha2,
        Su2,
        KaiYing3,
        SisterTai3,
        TalonSilverwing4,
        Zho3,
        ProfessorGai3,
        ErysVasburg1,
        Brutus,
        Sheena,
        Danika4,
        RedemptorKarl1,
        Lukas7,
        SeaguardHala,
        Argo1,
        SeaguardGita1,
        SeaguardEli1,
        Daeman,
        Aurora1,
        Aeson6,
        Emi2,
        Eve1,
        Cynn3,
        HeadmasterVhang3,
        Jamei2,
        Devona1,
        Aidan1,
        Aidan2,
        Chiyo2,
        Panaku5,
        Nika3,
        LoSha3,
        Su3,
        KaiYing4,
        SisterTai4,
        TalonSilverwing5,
        Zho4,
        ProfessorGai4,
        ErysVasburg2,
        Brutus1,
        Sheena1,
        RedemptorKarl2,
        Lukas8,
        Devona2,
        SeaguardHala1,
        KaiYing5,
        SisterTai5,
        Daeman1,
        Zho5,
        Aeson7,
        Panaku6,
        LoSha4,
        Su4,
        KaiYing6,
        SisterTai6,
        TalonSilverwing6,
        Zho6,
        ProfessorGai5,
        Emi3,
        Eve2,
        Cynn4,
        Jamei3,
        Devona3,
        Aidan3,
        Chiyo3,
        Vhang,
        Danika5,
        RedemptorKarl3,
        Cynn5,
        Aidan4,
        Nika4,
        Lukas9,
        Sheena2,
        Brutus2,
        TalonSilverwing7,
        SisterTai7,
        Zho7,
        KaiYing7,
        Aeson8,
        Aurora2,
        Daemen1,
        Argo2,
        HouseDurheimDuelist4,
        SeaguardHala2,
        Argo3,
        SeaguardGita2,
        SeaguardEli2,
        Aurora3,
        Daeman2,
        SiegeTurtle1,
        LuxonAssassin,
        LuxonScout,
        LuxonMesmer,
        LuxonNecromancer,
        LuxonElementalist,
        LuxonMonk,
        LuxonWarrior1,
        KurzickWarrior2,
        LuxonRanger,
        LuxonSoldier,
        LuxonRitualist,
        ConvictedCriminal,
        ConvictedCriminal1,
        ConvictedCriminal2,
        ConvictedCriminal3,
        ConvictedCriminal4,
        ConvictedCriminal5,
        GiantTurtle,
        GreenHaulerTurtle,
        PurpleHaulerTurtle,
        YellowHaulerTurtle,
        SiegeTurtle2,
        YoungTurtle,
        Arion,
        Alpheus,
        Cyril,
        ElderCasta,
        ElderCleo,
        Fern,
        Kaj,
        Linus,
        LuxonSoldier1,
        Remick,
        LuxonMonk1,
        CallistatheTamer,
        Endre,
        LuxonSoldier2,
        Hylas,
        LuxonGuard,
        ScoutmasterAerios,
        LuxonGuard1,
        LuxonQuartermaster,
        ReaverTammox,
        SeaguardLykaios,
        Attis,
        Erek1,
        HatcheryKeeperGratian,
        Leon,
        LuxonCommander,
        ScoutmasterTheron,
        SupplymasterKeleos,
        WatchmanBencis,
        CaptainLexis,
        Jo,
        LuxonGuard2,
        RecruiterLysandra,
        LuxonChild,
        LuxonChild1,
        ElderOxis,
        Erasmus,
        Eurayle,
        Ossian,
        Perlo,
        Pheobus,
        ZaviarMerkanah,
        Zenos,
        Alexei,
        ArenaMasterPortus,
        Linus1,
        LuxonAmbassador,
        LuxonNoble,
        LuxonSentry,
        Nicodemus,
        Petras,
        Callula,
        CaptainElora,
        Enelora,
        Leitha,
        LuxonNoble1,
        LuxonSentry1,
        Zenaida,
        CaptainJuno,
        ElderCasta1,
        ElderCleo1,
        ElderPomona,
        Kamilla,
        LuxonNoble2,
        Rayea,
        Alpheus1,
        AronSeaholm,
        DauvMerishahl,
        Eurus,
        Farrer,
        InfortunatosMaxeles,
        JahnPitz,
        KleoDesmos,
        LuxonArenaFan,
        LuxonPeasant,
        LuxonTraveler,
        Pietro,
        Rasmus,
        GreatReaverRixor,
        Kaj1,
        Keotah,
        LuxonArenaFan1,
        LuxonPeasant1,
        Maddison,
        Nendros,
        Rixonon,
        Axton,
        Cyril1,
        Delphinus,
        KristoTrilios,
        LuxonArenaFan2,
        LuxonPeasant2,
        LuxonTraveler1,
        MagistrateKin,
        SamtiKohlreg,
        CatrineEmbolom,
        Fern1,
        HistorianofClans,
        LoremasterSitai,
        LosiHapatu,
        LuxonArenaFan3,
        LuxonPeasant3,
        LuxonTraveler2,
        Amina,
        Luci,
        LuxonArenaFan4,
        LuxonPeasant4,
        Tarena,
        ElderEzio,
        JefriWhylir,
        Remick1,
        Mikolas,
        Tateos,
        ActaeonHali,
        Ardelia,
        Bastien,
        Cyrus,
        Demetrius,
        Kratos,
        Martinus,
        Phineus,
        Taryn,
        Vasileios,
        Alaris,
        Alphemynie,
        Anja,
        Caitlin,
        Demi,
        Elysia,
        Ionessa,
        Jacinthe,
        Norah,
        LuxonMerchant,
        Astrok,
        Telamon,
        Theodosus,
        Autumn,
        Zofie,
        Gratian,
        DweiaMaterial,
        Katharine,
        PhoebeScroll,
        Symeon,
        LuxonMerchant1,
        LuxonGuard3,
        LuxonGuard4,
        LuxonGuard5,
        LuxonPriest1,
        LuxonScavenger,
        Aurora4,
        Daeman3,
        Argo4,
        SeaguardGita3,
        SeaguardEli3,
        SeaguardHala3,
        QuartermasterHector,
        ElderRhea,
        XisniDreamHaunt,
        RahseWindcatcher,
        KaswaWebstrider,
        RotFoulbelly,
        TheTimeEater,
        TheScarEater,
        ThePainEater,
        TheSkillEater,
        BazzrDustwing,
        ChkkrLocustLord,
        BezzrWingstorm,
        BazzrIcewing,
        BizzrIronshell,
        ByzzrWingmender,
        ChkkrIronclaw,
        ChkkrThousandTail,
        ChkkrBrightclaw,
        KonrruTaintedStone,
        NandetGlassWeaver,
        Maximole,
        UrkaltheAmbusher,
        MugraSwiftspell,
        HarggPlaguebinder,
        TarlokEvermind,
        MungriMagicbox,
        TarnentheBully,
        NundakTheArcher,
        WaggSpiritspeak,
        DeeprootSorrow,
        DarkrootEntrop,
        StrongrootTanglebranch,
        SpiritrootMossbeard,
        WiserootShatterstone,
        BrambleEverthorn,
        FrothStonereap,
        AzukhanStonewrath,
        EnnsaStoneweaver,
        DornStonebreaker,
        CrawStonereap,
        ZarnasStonewrath,
        MerilStoneweaver,
        MahrStonebreaker,
        KyrilOathwarden,
        SunreachWarmaker,
        InallaySplintercall,
        FalaharnMistwarden,
        WardenofSaprophytes,
        MilefaunMindflayer,
        FoalcrestDarkwish,
        ArborEarthcall,
        JayneForestlight,
        TembarrTreefall,
        RyverMossplanter,
        SalkeFurFriend,
        FlowerSpiritgarden,
        DarkFang,
        TheAncient,
        BloodDrinker,
        RhythmDrinker,
        DiscordWallow,
        FungalWallow,
        Oni,
        MantisHunter,
        MantisDreamweaver,
        MantisStormcaller,
        MantisMender,
        MelodicGaki,
        PainHungryGaki,
        SkillHungryGaki,
        StoneScaleKirin,
        DredgeGutter,
        SlyDredge,
        DredgeGardener,
        DredgeGuardian,
        DredgeGatherer,
        DragonMoss,
        Undergrowth,
        StoneRain,
        ZuHeltzerGuardian,
        StoneReaper,
        StoneSoul,
        StoneCrusher,
        WardenoftheSpirit,
        WardenoftheMind,
        WardenofEarth,
        WardenoftheTree,
        WardenofForests,
        WardenoftheTrunk,
        WardenoftheBranch,
        WardenofWinds,
        WardenoftheLeaf,
        WardenoftheSummer,
        WardenofSeasons,
        WardenoftheSpring,
        Moleneaux,
        GreaterBloodDrinker,
        ThoughtStealer,
        HoppingVampire,
        MaddenedDredge,
        MaddenedDredgeSavage,
        MaddenedDredgeSeer,
        TwistedBark,
        UprootedMalice,
        BroodingThorns,
        BurningBrush,
        ThornWolf,
        GuardianSerpent,
        MaddenedThornWarden,
        MaddenedSongWarden,
        MaddenedMindWarden,
        MaddenedEarthWarden,
        MaddenedForestsWarden,
        MaddenedWindWarden,
        MaddenedSpiritWarden,
        SickenedLynx,
        SickenedMoa,
        SickenedStalker,
        SickenedWolf,
        SickenedWarthog,
        SickenedBear,
        SickenedGuard,
        SickenedServant,
        SickenedScribe,
        SickenedGuard1,
        SickenedGuardsmanTahnjo,
        SickenedGuard2,
        SickenedGuard3,
        SickenedPeasant,
        SickenedPeasant1,
        TheAfflictedSenku,
        AfflictedYijo,
        TheAfflictedKana,
        AfflictedFarmerXengJo,
        TheAfflictedXai,
        DiseasedMinister,
        AfflictedHorror,
        TheAfflictedSoonKim,
        TheAfflictedLiYun,
        TheAfflictedThu,
        TheAfflictedKam,
        TheAfflictedMiju,
        AfflictedGuardsmanChun,
        TheAfflictedAko,
        TheAfflictedHuan,
        TheAfflictedHakaru,
        TheAfflictedSoonKim1,
        TheAfflictedLiYun1,
        TheAfflictedHuan1,
        TheAfflictedKam1,
        TheAfflictedMiju1,
        TheAfflictedAko1,
        TheAfflictedHuan2,
        TheAfflictedHakaru1,
        TheAfflictedSenku1,
        TheAfflictedHsinJun,
        TheAfflictedLau,
        TheAfflictedShen,
        TheAfflictedJingme,
        TheAfflictedMaaka,
        TheAfflictedPana,
        TheAfflictedXenxo,
        CanthanPeasant11,
        CanthanPeasant12,
        CanthanPeasant13,
        CanthanPeasant14,
        CanthanPeasant15,
        CanthanPeasant16,
        CanthanPeasant17,
        CanthanPeasant18,
        AfflictedAssassin,
        AfflictedAssassin1,
        AfflictedAssassin2,
        AfflictedAssassin3,
        AfflictedAssassin4,
        AfflictedAssassin5,
        AfflictedCreature,
        AfflictedAssassin6,
        AfflictedBull,
        AfflictedMesmer,
        AfflictedMesmer1,
        AfflictedMesmer2,
        AfflictedMesmer3,
        AfflictedMesmer4,
        AfflictedMesmer5,
        AfflictedMesmer6,
        AfflictedNecromancer,
        AfflictedNecromancer1,
        AfflictedNecromancer2,
        AfflictedNecromancer3,
        AfflictedNecromancer4,
        AfflictedNecromancer5,
        AfflictedNecromancer6,
        AfflictedElementalist,
        AfflictedElementalist1,
        AfflictedElementalist2,
        AfflictedElementalist3,
        AfflictedElementalist4,
        AfflictedElementalist5,
        AfflictedElementalist6,
        AfflictedMonk,
        AfflictedMonk1,
        AfflictedMonk2,
        AfflictedMonk3,
        AfflictedMonk4,
        AfflictedMonk5,
        AfflictedWarrior,
        AfflictedWarrior1,
        AfflictedTukan,
        AfflictedWarrior2,
        AfflictedWarrior3,
        AfflictedWarrior4,
        AfflictedWarrior5,
        AfflictedWarrior6,
        AfflictedRanger,
        AfflictedRanger1,
        AfflictedRanger2,
        AfflictedRanger3,
        AfflictedRanger4,
        AfflictedRanger5,
        AfflictedRanger6,
        AfflictedRavager,
        AfflictedRitualist,
        AfflictedRitualist1,
        AfflictedRitualist2,
        AfflictedRitualist3,
        AfflictedRitualist4,
        AfflictedRitualist5,
        SanhanTallEarth,
        HanjuuTowerfist,
        AhvhaSankii,
        PeitheSkullBlade,
        SulmengtheSkullStaff,
        YingkotheSkullClaw,
        AuritheSkullWand,
        FengtheSkullSymbol,
        CaptainQuimang1,
        ChoWeitheSkullAxe,
        JintheSkullBow,
        MikitheSkullSpirit,
        TahkayunTsi,
        YunlaiDeathkeeper,
        ZiinjuuLifeCrawler,
        TinDaoKaineng,
        BaozoEvilbranch,
        WaterBuffalo,
        Oni1,
        LesserOni,
        BonesnapTurtle,
        RavenousDrake,
        CrimsonSkullEtherFiend,
        CrimsonSkullMentalist,
        CrimsonSkullMesmer,
        CrimsonSkullHealer,
        CrimsonSkullPriest,
        CrimsonSkullMender,
        CrimsonSkullHunter,
        CrimsonSkullLongbow,
        CrimsonSkullRaider,
        CropThief,
        CrimsonSkullSeer,
        DaoWeng,
        CrimsonSkullSpiritLord,
        CrimsonSkullRitualist,
        DragonLilly,
        GraspingRoot,
        MantidDrone,
        Mantid,
        MantidParasite,
        MantidDarkwing,
        MantidDroneHatchling,
        MantidDroneHatchling1,
        MantidHatchling,
        MantidMonitor,
        Mantid1,
        MantidGazer,
        QueenMantid,
        MantidGlitterfang,
        MantidQueen,
        MantidMonitorHatchling,
        NagaScout,
        NagaWarrior,
        NagaWelp,
        NagaWizard,
        NagaRaincaller,
        NagaSpellblade,
        NagaWitch,
        NagaBoneCollector,
        XsshsssZsss,
        NagaSibyl,
        SensaliAssassin,
        SensaliClaw,
        SensaliBlood,
        SensaliDarkFeather,
        SensaliFighter,
        SwiftHonorclaw,
        GrowRazorbeak,
        SensaliCutter,
        HungryKappa,
        Kappa,
        Kappa1,
        Kappa2,
        WildYeti,
        WulkCragfist,
        MountainYeti,
        LonghairYeti,
        RedYeti,
        CrimsonSkullHunter1,
        CelestialKirin,
        Zunraa,
        JadeTornKirin,
        CorruptedZunraa,
        QuillSongfeather,
        TenguAssassin,
        TenguShrill,
        TenguCutter1,
        Kuunavang,
        EnragedKuunavang,
        Kuunavang1,
        CorruptedSpore,
        RazorfangHazeclaw,
        RazorfinFleshrend,
        RazortongueFrothspit,
        KaySeyStormray,
        RazorjawLongspine,
        WhisperingRitualLord,
        WhykSteelshell,
        MohbyWindbeak,
        ReefclawRagebound,
        FocusofHanaku,
        ShroudedOni,
        SourbeakRotshell,
        MiellaLightwing,
        SnapjawWindshell,
        KunvieFirewing,
        LukrkerFoulfist,
        BahnbaShockfoot,
        HukhrahEarthslove,
        ArrahhshMountainclub,
        ChehbabaRoottripper,
        TomtonSpiriteater,
        AriusDarkApostle,
        TaloustheMad,
        CultistMilthuran,
        AmadisWindoftheSea,
        IncetolDevoutofDepths,
        JacquiTheReaver,
        LorelleJadeCutter,
        DelictheVengeanceSeeker,
        GeofferPainBringer,
        KayalitheBrave,
        SentasitheJadeMaul,
        ChazekPlagueHerder,
        CultistRajazan,
        MerkiTheReaver,
        MilodestustheWrangler,
        KenriiSeaSorrow,
        SsareshRattler,
        SiskaScalewand,
        SesskWoeSpreader,
        XirissStickleback,
        SarssStormscale,
        SsunsBlessedofDwayna,
        SskaiDragonsBirth,
        StsouSwiftscale,
        SsynCoiledGrasp,
        ScourgewindElderGuardian,
        SoulwhisperElderGuardian,
        SeacrashElderGuardian,
        WavecrestStonebreak,
        ScuttleFish,
        CreepingCarp,
        Irukandji,
        Yeti,
        Yeti1,
        Yeti2,
        RotWallow,
        LeviathanEye,
        LeviathanClaw,
        TheImpossibleSeaMonster,
        LeviathanMouth,
        KrakenSpawn,
        Oni2,
        Oni3,
        SaltsprayDragon,
        RockhideDragon,
        OutcastAssassin,
        OutcastDeathhand,
        OutcastWarrior,
        OutcastRitualist,
        Kappa3,
        JadeTornKirin1,
        NagaWarrior1,
        NagaArcher,
        NagaRitualist,
        IslandGuardian,
        LuxonEnchanter,
        DarkenedIrukandji,
        ShadowofKanaxai,
        OutcastAssassin1,
        OutcastDeathhand1,
        OutcastSpellstorm,
        OutcastRaider,
        DeepNightmare,
        TearsofDwayna,
        SenkaiLordofthe1000DaggersGuild,
        OnioftheDeep,
        Kanaxai,
        KanaxaiAspectofDeath,
        KanaxaiAspectofDecay,
        KanaxaiAspectofDepletion,
        KanaxaiAspectofExposure,
        KanaxaiAspectofFailure,
        KanaxaiAspectofFear,
        KanaxaiAspectofLethargy,
        KanaxaiAspectofPain,
        KanaxaiAspectofScorpions,
        KanaxaiAspectofShadows,
        KanaxaiAspectofSoothing,
        KanaxaiAspectofSurrender,
        KanaxaiAspectofTorment,
        FreezingNightmare,
        SappingNightmare,
        ScourgeManta,
        BlessedManta,
        RipperCarp,
        DarkenedIrukandji1,
        RebornIrukandji,
        TheLeviathan,
        LeviathanMind,
        LeviathanArm,
        LeviathanHead,
        OutcastAssassin2,
        OutcastDeathhand2,
        OutcastSpellstorm1,
        OutcastReaver,
        OutcastRaider1,
        OutcastRitualist1,
        RitualistsConstruct,
        AssassinsConstruct,
        BoundVizu,
        BoundKitah,
        BoundNaku,
        BoundTeinai,
        BoundKarei,
        BoundJaizhanju,
        BoundZojun,
        BoundKaolai,
        AssassinsConstruct1,
        MesmersConstruct,
        NecromancersConstruct,
        ElementalsConstruct,
        MonksConstruct,
        WarriorsConstruct,
        RangersConstruct,
        RitualistsConstruct1,
        MesmersConstruct1,
        NecromancersConstruct1,
        ElementalsConstruct1,
        MonksConstruct1,
        WarriorsConstruct1,
        RangersConstruct1,
        SilentAncientOnata,
        ArcaneAncientPhi,
        DoomedAncientKkraz,
        StarAncientKoosun,
        UntouchedAncientKy,
        SwordAncientKai,
        FamishedAncientBrrne,
        DefiantAncientSseer,
        SilentAncientOnata1,
        ArcaneAncientPhi1,
        DoomedAncientKkraz1,
        StarAncientKoosun1,
        UntouchedAncientKy1,
        SwordAncientKai1,
        FamishedAncientBrrne1,
        DefiantAncientSseer1,
        ShiroTagachi,
        ShiroTagachi1,
        ShirokenAssassin,
        ShirokenMesmer,
        ShirokenNecromancer,
        ShirokenElementalist,
        ShirokenMonk,
        ShirokenWarrior,
        ShirokenRanger,
        ShirokenRitualist,
        SpiritofPortals,
        BoundMesmer,
        BoundNecromancer,
        BoundElementalist,
        BoundMonk,
        BoundWarrior,
        BoundRanger,
        BoundRitualist,
        SpiritofTheMists,
        SpiritofTheMists1,
        SpiritofTheMists2,
        Tahmu,
        HaiJii,
        KaijunDon,
        Kuonghsang,
        ShreaderSharptongue,
        ZiinfaunLifeforce,
        BaubaoWavewrath,
        QuansongSpiritspeak,
        EssenceofDragon,
        EssenceofPhoenix,
        EssenceofKirin,
        EssenceofTurtle,
        Vermin,
        StarSentinel,
        SoarWindfeather,
        StarBlade,
        StarLight,
        ClawTallfeather,
        Kappa4,
        MantidDrone1,
        MantidMonitor1,
        TempleGuardian,
        TempleGuardian1,
        IlidusoftheEmptyPalm,
        XuekaotheDeceptive,
        LouoftheKnives,
        Waeng,
        MinaShatterStorm,
        ChantheDragonsBlood,
        ChungtheAttuned,
        AmFahLeader,
        RientheMartyr,
        SuntheQuivering,
        MeynsangtheSadistic,
        ChoSpiritEmpath,
        KenshiSteelhand,
        JinthePurifier,
        GhialtheBoneDancer,
        LianDragonsPetal,
        Quufu,
        ShentheMagistrate,
        WingThreeBlade,
        RoyenBeastkeeper,
        OrosenTranquilAcolyte,
        AmFahAssassin,
        AmFahBandit,
        LouoftheKnives1,
        ObsidianFlameAssassin1,
        Ratsu,
        TaeXang,
        Yanlen,
        AmFahNecromancer,
        AmFahHealer,
        AmFahMarksman,
        JadeBrotherhoodMesmer,
        ChongPoi,
        JadeBrotherhoodMage,
        JadeBrotherhoodKnight,
        Noqui,
        JadeBrotherhoodRitualist,
        JadeBrotherhoodMage1,
        JadeBrotherhoodRitualist1,
        AmFahAssassin1,
        AmFahNecromancer1,
        AmFahHealer1,
        AmFahMarksman1,
        ShackledSpirit,
        Kormir,
        XunlaiChest,
        ArchivistofWhispers,
        ScytheofChaos,
        WrathfulStorm,
        TormentClaw,
        GraspofInsanity,
        Chasm,
        Vekk,
        OgdenStonehealer,
        DwarvenDemolitionist,
        Borvorel,
        BurolIronfist,
        Kodan,
        MOX,
        Zenjal,
        ForemantheCrier,
        InitiateZeiRi,
        ZhaKu,
        InitiateTsuriai,
        HeraldofPurity,
        Setsu,
        ZuJintheQuick,
        FahYutheShadowsEye,
        UrisTongofAsh,
        AmFahWarrior,
        AmFahRanger,
        AmFahElementalist,
        AmFahMonk,
        AmFahNecromancer2,
        AmFahRitualist,
        AmFahAssassin2,
        LeiJeng,
        GuardsmanQaoLin,
        AfflictedWarrior7,
        BoundAssassinSanctum,
        MantisHunter1,
        Oni4,
        MantisStormcaller1,
        KurzickScoutthe,
        AfflictedMesmerDaijun,
        MantisDreamweaver1,
        MantidQueenDaijun,
        AfflictedMonkDaijun,
        MantisMender1,
        GreaterSerpentWarren,
        MantidDestroyerDaijun,
        FungalWallowVeil,
        GateGuardPalace,
        AfflictedRitualistChosEstate,
        KurzickMineCleanserAspenwood,
        MasterTogoTemple,
        Irukandji1,
        NagaRitualistSeabed,
        NagaWitch1,
        NagaWitch2,
        ExplosiveGrowthWarren,
        EurayleCenter,
        MarketMerchant,
        VashCenter,
        Vling,
        Zenku,
        FarmerDonlai,
        PetElderCrabSeabed,
        TigerHatchery,
        UrgozWarren,
        WardenoftheTrunk1,
        Undergrowth1,
        LeviathanClaw1,
        CanthanGuard6,
        InscribedWall,
        FieldGeneralHayao,
        GeneralYurukaro,
        CanthanAmbassador1,
        Garemmof,
        XunlaiAgentJueh,
        XunlaiAgentMomo,
        XunlaiAgent1,
        Alfred,
        Susahn,
        Warthog,
        ImpressiveWarthog,
        RiverDrake,
        LightningDrake,
        GrandDrake,
        SandDrake,
        ForgottenDefender,
        ForgottenIllusionist,
        ForgottenCursebearer,
        ForgottenDefender1,
        ForgottenArcanist,
        ForgottenDefender2,
        ForgottenSage,
        KeeperHalyssi,
        KeeperJinyssa,
        KeeperKauniss,
        KeeperZeliss,
        RunicOracle,
        Olias,
        LyssasMuse,
        VoiceofGrenth,
        AvatarofDwayna,
        ChampionofBalthazar,
        MelandrusWatcher,
        Diane1,
        DerasTenderlin,
        Durmand1,
        Tuomas1,
        TerrorwebDryder,
        LostSoul,
        LostSoul1,
        LostSoul2,
        LostSoul3,
        ElderCrocodile,
        RisenAshenHulk,
        HandoftheTitans,
        FistoftheTitans,
        ArmageddonLord,
        WindBornTitan,
        EarthBornTitan,
        WaterBornTitan,
        WildGrowth,
        RottingTitan,
        PortalWraith,
        ObsidianFurnaceDrake,
        JatoroMusagi1,
        ZaishenScout1,
        Mhenlo,
        FunwaShento,
        Seung,
        Akemi1,
        Zenmai,
        BoundHaoLi,
        BoundKaichen,
        BoundTiendi,
        ShirokenAssassin1,
        ShirokenMesmer1,
        ShirokenNecromancer1,
        ShirokenElementalist1,
        ShirokenMonk1,
        ShirokenWarrior1,
        ShirokenRanger1,
        ShirokenRitualist1,
        Hyena,
        Flamingo,
        TroublesomeFlamingo,
        Lioness,
        Lion,
        JahaiRat,
        Crocodile,
        ElderHyena,
        ElderCrocodile1,
        PetElderCrocodile,
        AggressiveLion,
        AggressiveLioness,
        GreaterInfestation,
        Infestation,
        MaddenedSpirit,
        LostSoul4,
        UndeadSoldier,
        SolitaryColossus,
        AchortheBladed,
        VahtheCrafty,
        HajkorMysticFlame,
        RendabiDeatheater,
        WiolitheInfectious,
        AjamahnServantoftheSands,
        JedehtheMighty,
        UhiwitheSmoky,
        DunshekthePurifier,
        DroajamMageoftheSands,
        DuneSpider,
        RavenousMandragor,
        MandragorSandDevil,
        MandragorTerror,
        RavenousMandragor1,
        RubyDjinn,
        SapphireDjinn,
        SadisticGiant,
        NomadGiant,
        GravenMonolith,
        GravenMonolith1,
        GravenMonolith2,
        DuneBeetleQueen,
        BladedDuneTermite,
        InfectiousDementia,
        DuneBeetleLance,
        ShamblingMesa,
        SandstormCrag,
        DesertWurm,
        EmejuLonglegs,
        KesheltheVoracious,
        NajabLifedrinker,
        AjamdukHunteroftheSands,
        TaromRockbreaker,
        EhyalLongtooth,
        HaiossBlessedWind,
        ChinehSoaringLight,
        GedossWindcutter,
        Apocrypha,
        HassinSoftskin,
        BroodMotherKalwameh,
        SunehStormbringer,
        ModossDarkwind,
        RahtiFlowerofDread,
        BloodbackMorrob,
        LonolunWaterwalker,
        BehbatheHardheaded,
        DreadLordOnrah,
        StalkingNephilia,
        StalkingNephilia1,
        StalkingNephilia2,
        LadyoftheDead,
        WaterDjinn,
        WaterDjinn1,
        RinkhalMonitor,
        IrontoothDrake,
        IrontoothDrake1,
        SkreeTalon,
        SkreeWarbler,
        SkreeHatchling,
        SkreeFledgeling,
        SkreeGriffon,
        HarpyMother,
        SkreeTalon1,
        HarpyMother1,
        SkreeHunter,
        HarpyMother2,
        SkreeWarbler1,
        RidgebackScale,
        SkaleLasher,
        SkaleBlighter,
        RidgebackScale1,
        SkaleLasher1,
        FrigidSkale,
        HungrySkale,
        SkaleBlighter1,
        RidgebackSkale,
        SkaleBlighter2,
        RidgebackScale2,
        SkaleLasher2,
        FierceSkale,
        FrigidSkale1,
        JuvenileBladedTermite,
        GrubLance,
        BladedTermite,
        PreyingLance,
        GrubLance1,
        BladedTermite1,
        PreyingLance1,
        StormseedJacaranda,
        FangedIboga,
        KillerIboga,
        StormseedJacaranda1,
        FangedIboga1,
        GreatFireFlower,
        StormseedJacaranda2,
        BeautifulIboga,
        FangedIboga2,
        MandragorImp,
        MandragorSlither,
        MandragorImp1,
        StonefleshMandragor,
        MandragorSlither1,
        MandragorImp2,
        StonefleshMandragor1,
        Mandragor,
        MandragorSlither2,
        ImpressiveWarthog1,
        GraspofChaos,
        ScytheofChaos1,
        Dehjah,
        KinyaKela,
        BalthazarsEternal,
        GhostlyGriffon,
        TombWraith,
        MoavuKaal,
        SkaleLordJurpa,
        RazorbackSkale,
        HamouUkaiou,
        BoklonBlackwater,
        SkreeHatchling1,
        IbogaRoot,
        TombGuardian,
        GlugKlugg,
        MandragorQueen,
        Suitof15Armor,
        Suitof35Armor,
        Suitof55Armor,
        Adjacent,
        IntheArea,
        LongbowTarget,
        Nearby,
        PracticeTarget,
        RecurveTarget,
        ShortBowTarget,
        Norgu,
        Goren,
        Goren1,
        ZhedShadowhoof,
        GeneralMorgahn,
        Margrid,
        MargridtheSly,
        Tahlkora,
        MasterofWhispers,
        AcolyteJin,
        Koss,
        Koss1,
        Koss2,
        Dunkoro,
        Acolyte,
        AcolyteSousuke,
        Melonni,
        Razah,
        Kihm,
        Herta,
        Gehraz,
        Sogolon,
        Kihm1,
        Odurra,
        Herta1,
        Timera,
        Abasi,
        Gehraz1,
        Sogolon1,
        Kihm2,
        Odurra1,
        Herta2,
        Timera1,
        Abasi1,
        Gehraz2,
        Sogolon2,
        Kihm3,
        Odurra2,
        Herta3,
        Timera2,
        Abasi2,
        Gehraz3,
        Sogolon3,
        Kihm4,
        Odurra3,
        Herta4,
        Timera3,
        Abasi3,
        Gehraz4,
        Sogolon4,
        Odurra4,
        Eve3,
        Cynn6,
        Herta5,
        Kihm5,
        Devona4,
        Aidan5,
        Gehraz5,
        Sogolon5,
        Odurra5,
        Eve4,
        Cynn7,
        Herta6,
        Mhenlo1,
        Kihm6,
        Devona5,
        Aidan6,
        Gehraz6,
        Sogolon6,
        Odurra6,
        Eve5,
        Cynn8,
        Herta7,
        Mhenlo2,
        Devona6,
        Aidan7,
        Gehraz7,
        Sogolon7,
        Shiny,
        ThunderofAhdashim,
        DivineGuardian,
        EternalGuardianofLanguor,
        EternalGuardianofLethargy,
        EternalGuardianofSuffering,
        KeyofAhdashim,
        LockofAhdashim,
        KormabBurningHeart,
        HajokEarthguardian,
        ShakorFirespear,
        DjinnOverseer,
        FireLord,
        WaterLord,
        Render,
        Scratcher,
        Screecher,
        BrokkRipsnort,
        HarrkFacestab,
        BlueTongueHeket,
        BloodCowlHeket,
        StoneaxeHeket,
        BeastSwornHeket,
        RiktundtheVicious,
        PehnsedtheLoudmouth,
        JisholDarksong,
        MarobehSharptail,
        EshekibehLongneck,
        KorshektheImmolated,
        ChurahmSpiritWarrior,
        KormabBurningHeart1,
        KorrubFlameofDreams,
        LeilonTranquilWater,
        ShakorFirespear1,
        HojanukunMindstealer,
        YammironEtherLord,
        YammirvuEtherGuardian,
        MakdehtheAggravating,
        HanchorTrueblade,
        NehpektheRemorseless,
        ShakJarintheJusticebringer,
        JosinqtheWhisperer,
        BanshehGathererofBranches,
        MotehThundershooter,
        KunantheLoudmouth,
        HajokEarthguardian1,
        TenezeltheQuick,
        TenshekRoundbody,
        ToshauSharpspear,
        ShezelSlowreaper,
        SetikorFireflower,
        BanorGreenbranch,
        YameshMindclouder,
        MabahHeardheart,
        HahanFaithfulProtector,
        BoltenLargebelly,
        JarimiyatheUnmerciful,
        ChumabthePrideful,
        BohdalztheFurious,
        ElderSkreeGriffin,
        ElderSkreeRaider,
        ElderSkreeTracker,
        ElderSkreeSinger,
        CobaltMokele,
        CobaltShrieker,
        CobaltScabara,
        BehemothGravebane,
        BehemothGravebane1,
        ScytheclawBehemoth,
        RubyDjinn1,
        RubyofAhdashim,
        DiamondDjinn,
        DiamondofAhdashim,
        SapphireDjinn1,
        SapphireofAhdashim,
        SapphireDjinn2,
        RoaringEther,
        RoaringEther1,
        SkreeGriffin,
        SkreeRaider,
        SkreeTracker,
        SkreeSinger,
        RainBeetle,
        RainBeetle1,
        RockBeetle,
        BullTrainerGiant,
        HuntingMinotaur,
        MirageIboga,
        StormJacaranda,
        EnchantedBrambles,
        WhistlingThornbrush,
        BloodCowlHeket1,
        BlueTongueHeket1,
        StoneaxeHeket1,
        BeastSwornHeket1,
        Chormar,
        Danton,
        Elonbel,
        Fahrik,
        GuardKovu,
        GuardsmanGafai,
        GuardsmanGahrik,
        GuardsmanOnageh,
        Gundok,
        Hoanjo,
        Jibehr,
        Lohfihau,
        Lunmor,
        Nermak,
        Sedai,
        Shaudok,
        ShoreWatcher,
        Shorewatcher,
        Sunspear,
        Tohsedi,
        Yamamank,
        Ahbri,
        Behchu,
        CaptainAhkenchu,
        CastellanPuuba,
        FirstSpearDehvad,
        FirstSpearJahdugar,
        Kohanu,
        Pikin,
        Ronkhet,
        RundukRank,
        Shiloh,
        Tahristahn,
        Tohn,
        Shaurom,
        Dzajo,
        Ehiyah,
        Nundho,
        CorpseofShoreWatcher,
        ShoreWatcher1,
        Bendah,
        SecondSpearBinah,
        Shorewatcher1,
        Sunspear1,
        Ashigun,
        Dalzbeh,
        Hlengiwe,
        MahksSon,
        Mau,
        Mauban,
        Nehduvad,
        Paldinam,
        Pobehr,
        Yahyakun,
        YoungChild,
        Adina,
        Chitundu,
        Poti,
        YoungChild1,
        Adhitok,
        Architect,
        ChefVolon,
        ClerkArlon,
        DockmasterAhlaro,
        ElderBelin,
        HealerZenwa,
        HistorianLaharo,
        Horat,
        IstaniNoble,
        Iyemab,
        Jidun,
        Kehtu,
        Mofuun,
        RolandleMoisson,
        ScholarChago,
        ScholarKayanu,
        TacticianHaj,
        VabbiTradeOfficial,
        Gamyuka,
        IstaniNoble1,
        Kormir1,
        Midauha,
        Paljab,
        QuarrymasterBohanna,
        SkyScholar,
        Sushah,
        Timahr,
        Tugani,
        VabbiTradeOfficial1,
        Zukesha,
        AilonsehDejarin,
        ChefPanjoh,
        Dajwa,
        DigmasterGatah,
        Dungrud,
        ElderJurdu,
        Elonyam,
        FarmerMuenda,
        Hamar,
        Hatuk,
        IstaniCommoner,
        IstaniMiner,
        IstaniPeasant,
        Kanyama,
        LendutheFisherman,
        Mahk,
        Mahzu,
        Morik,
        Mufinbo,
        Munashe,
        Pentehlez,
        Poturi,
        Rohtu,
        Uhisheh,
        Vadben,
        WeaverTamapi,
        WoundedVillager,
        Yajide,
        Ajambo,
        Ando,
        Apida,
        Architect1,
        BeastmasterYapono,
        Behrdos,
        IstaniCommoner1,
        MasterEngineerJakumba,
        Mirmahk,
        NecromancerMusseh,
        Rahb,
        Rohtu1,
        StonecutterGed,
        SuspiciousHermit,
        CorpseofIstaniPeasant,
        ChefLonbahn,
        Dahni,
        IstaniCommoner2,
        Kailona,
        Kihawa,
        MahksWife,
        Motur,
        MourningVillager,
        Nenah,
        Nertu,
        ObserverJahfoh,
        Pehrub,
        Pelei,
        Sehti,
        Sehyal,
        Shakashi,
        Sholmara,
        Sinni,
        Talmehinu,
        WoundedVillager1,
        Yahya,
        Bahlya,
        Dajwa1,
        FarmerFoneng,
        IstaniCommoner3,
        MinerTonabanza,
        Miresh,
        Poturi1,
        Rahlon,
        Damak,
        XunlaiGiftGiverGimmek,
        CaptainBolduhr,
        Issti,
        Matunbo,
        Velig,
        Jakatu,
        Kehtokweh,
        Pasu,
        Vatundo,
        Mehinu,
        Sulee,
        Arissi,
        Habab,
        Jasuh,
        Kargah,
        Kupekanu,
        Lobutu,
        Lokai,
        Lomar,
        Matuu,
        Shatam,
        VabbianCommoner,
        Alekaya,
        Answa,
        Tesserai,
        VabbianCommoner1,
        Yedah,
        Chorben,
        Dahleht,
        Farzi,
        Fahri,
        Saara,
        Barlom,
        Guul,
        Kahlim,
        Nasera,
        DashehMaterial,
        DuujaMaterial,
        NehgoyoMaterial,
        Goldai,
        Nela,
        Kenohir,
        Kenohir1,
        Zuwarah,
        Zuwarah1,
        Shausha,
        Shausha1,
        Ahamid,
        Ahamid1,
        Dalzid,
        Marlani,
        Suti,
        Bomahz,
        Radhiya,
        Ekundayo,
        Rafiki,
        Selah,
        Tahembi,
        Nalah,
        Kadirah,
        Sekou,
        Faraji,
        Safiya,
        Wanjala,
        Nahjiri,
        Bomani,
        Dume,
        Sodan,
        Mara,
        Isokeh,
        Rakanja,
        Lisha,
        Zahwena,
        Nagozi,
        Arayah,
        Mothusi,
        Farisah,
        Sunspear2,
        Hagon,
        Kohjo,
        Lahna,
        ScoutKahra,
        Sunspear3,
        SunspearElementalist,
        SunspearQuartermaster,
        Hero,
        Kushau,
        Lokuto,
        Sunspear4,
        Dinja,
        Risa,
        Shorewatcher2,
        Sunspear5,
        SunspearWarrior,
        Yahir,
        SunspearWarrior1,
        Kina,
        Sunspear6,
        SunspearRanger,
        SunspearScout,
        Kanessa,
        CaptainGudur,
        Korsu,
        Libeh,
        RaidmarshalMehdara,
        Rohmen,
        SpearmarshalBendro,
        Sunspear7,
        SunspearParagon,
        SunspearQuartermaster1,
        SunspearScout1,
        Sunspear8,
        Sunspear9,
        Hawan,
        Sunspear10,
        Sunspear11,
        Sunspear12,
        Sunspear13,
        Sunspear14,
        Churrlan,
        CapturedSunspear,
        CapturedSunspear1,
        SunspearPrisoner,
        CapturedSunspear2,
        SunspearPrisoner1,
        CapturedSunspear3,
        SunspearPrisoner2,
        SunspearRecruit,
        Sunspear15,
        SunspearRecruit1,
        Sunspear16,
        CaptainVahndah,
        SunspearRecruit2,
        SunspearRecruit3,
        SunspearRecruit4,
        Sunspear17,
        SunspearRefugee,
        CommanderSuha,
        EliteScoutZusoh,
        SunspearModiki,
        SunspearRefugee1,
        SunspearWarrior2,
        Vadah,
        ElderSuhl,
        Nerashi,
        ElderOlunideh,
        Ahtok,
        Nehdukah,
        Rojis,
        Lonai,
        Lonai1,
        Tahon,
        IstaniCultist,
        CastellanPuuba1,
        SahlahjartheDead,
        Digger,
        Dengo,
        Bahbukar,
        SahrehJelon,
        Behron,
        DalzJelon,
        JedurWahmeh,
        MiselaWahmeh,
        MutuWahmeh,
        Jerek,
        EvilMine,
        IstaniCultist1,
        Sailor,
        IstaniPeasant1,
        IstaniPeasant2,
        IstaniPeasant3,
        SunspearEvacuee,
        SunspearEvacuee1,
        TheGreatZehtuka,
        NecromancerLahse,
        AvatarofKormir,
        SeerofTruth,
        Kormir2,
        Kormir3,
        Kormir4,
        ZelnehlunFastfoot,
        DabinehDeathbringer,
        WandalztheAngry,
        OnwanLordoftheNtouka,
        OlunossWindwalker,
        ChiossenSoothingBreeze,
        ShelbohtheRavenous,
        JoknangEarthturner,
        EnadiztheHardheaded,
        MahtoSharptooth,
        SehlonBeautifulWater,
        KorrLivingFlame,
        LuntoSharpfoot,
        ChidehkirLightoftheBlind,
        ChortheBladed,
        EshauLongspear,
        EshimMindclouder,
        BirnehSkybringer,
        ModtiDarkflower,
        NeolitheContagious,
        BubahlIcehands,
        JernehNightbringer,
        ArmindtheBalancer,
        BuhonIcelord,
        RobahHardback,
        ChurrtatheRock,
        TundosstheDestroyer,
        PodalturtheAngry,
        TerobRoundback,
        DzabelLandGuardian,
        TheDrought,
        KehmaktheTranquil,
        RanshekCarrionEator,
        YakunTrueshot,
        InfectiousNightmare,
        SeborhinPest,
        VeldtNephila,
        VeldtNephila1,
        CrackedMesa,
        StoneShardCrag,
        ImmolatedDjinn,
        WaterDjinn2,
        MaelstromDjinn,
        BloodCowlHeket2,
        BlueTongueHeket2,
        StoneaxeHeket2,
        BeastSwornHeket2,
        CrestedNtoukaBird,
        NtoukaBird,
        RampagingNtouka,
        TuskedHowler,
        TuskedHunter,
        SteelfangDrake,
        KuskaleBlighter,
        RidgebackKuskale,
        KuskaleLasher,
        FrigidKuskale,
        SeborhinPest1,
        VeldtBeetleQueen,
        BladedVeldtTermite,
        InfectiousDementia1,
        SeborhinPest2,
        BladedVeldtTermite1,
        SeborhinPest3,
        VeldtBeetleLance,
        StormfaceJacaranda,
        StormforceJacaranda,
        ViciousSeedling,
        MurmuringThornbrush,
        ViciousSeedling1,
        HarojFiremane,
        MirageIboga1,
        ViciousSeedling2,
        CorruptedNature,
        MandragorImp3,
        CorruptedNature1,
        StonefleshMandragor2,
        CorruptedNature2,
        MandragorSlither3,
        Droughtling,
        FerociousDrake,
        Toma,
        Cow,
        Yartu,
        CorruptedRoot,
        AncestralBud,
        CorruptedFlower,
        SevadsKeeper,
        TormentedLand,
        EarthenAbomination,
        SlortNilbog,
        ZephyrHedger,
        Centaur,
        DirahTraptail,
        GrifEbonmane,
        HarojFiremane1,
        KolSwordshanks,
        LaphLongmane,
        VeldrunnerFighter,
        YeraSwiftsight,
        VeldrunnerCentaur,
        MirzaVeldrunner,
        ShiroTagachi2,
        UndeadLich,
        ShakahmtheSummoner,
        FaveoAggredior,
        SaevioProelium,
        CreoVulnero,
        ExuroFlatus,
        ScytheofChaos2,
        GraspofInsanity1,
        WrathfulStorm1,
        ShadowMesmer,
        ShadowElemental,
        ShadowMonk,
        ShadowWarrior,
        ShadowRanger,
        ShadowBeast,
        FleshGlutton,
        TitanAbomination,
        PainTitan,
        MadnessTitan,
        ShirokenAssassin2,
        ScorchEmberspire,
        Rukkassa,
        KeeperJinyssa1,
        KeeperKauniss1,
        Terick,
        FortuneTeller,
        IgnisCruor,
        SecurisPhasmatis,
        LetumContineo,
        TheHunter,
        Dunbri,
        ImperialCaptainShiWang,
        ImperialGuardHaoLi,
        ImperialGuardKaichen,
        Jarindok,
        JoyousSoul,
        Kaelen,
        Keshsek,
        MadSoul,
        OrrianSpiritGildran,
        OrrianSpiritMinos,
        Pehai,
        Rahmor,
        Rochor,
        Talneng,
        TormentedSoul,
        Tuor,
        ImperialGuardTiendi,
        JoyousSoul1,
        Laninahk,
        MadSoul1,
        Neersi,
        OrrianSpiritKandril,
        TormentedSoul1,
        MadSoul2,
        SecretKeeper,
        SpiritofTruth,
        Churrazek,
        MadSoul3,
        SecretKeeper1,
        Jutuk,
        TormentedSoul2,
        Gunli,
        Mehpekanu,
        Fahsik,
        Makaum,
        StorageChest,
        XunlaiChest1,
        ArchivistZisthus,
        AurusTrevess,
        CaptainJerazh,
        CaptainSulahresh,
        ChaplainPhyratyss,
        CommanderThurnis,
        ForgottenGuardian,
        ForgottenKeeper,
        ForgottenWarden,
        HighPriestZhellix,
        Hoju,
        KeeperofArmor,
        KeeperofArms,
        KeeperofBone,
        KeeperofIllusion,
        KeeperofLight,
        KeeperofNature,
        KeeperofSecrets,
        KeeperofShadows,
        KeeperofSpirits,
        KeeperofSteel,
        KeeperoftheElements,
        KeeperoftheScythe,
        KeeperoftheSpear,
        KeeperShafoss,
        KeeperSharissh,
        Raukus,
        RelkysstheBroken,
        RunahsSilus,
        Tekliss,
        Virashak,
        VisshRakissh,
        Volatiss,
        Zendeht,
        Vialee,
        WanderingSoul,
        DynasticSpirit,
        JoyousSoul2,
        MadSoul4,
        AscensionPilgrim,
        DynasticSpirit1,
        JoyousSoul3,
        LostSoulMission,
        MadSoul5,
        TormentedSoul3,
        AscensionPilgrim1,
        JoyousSoul4,
        MadSoul6,
        Wanzel,
        JoyousSoul5,
        AscensionPilgrim2,
        JoyousSoul6,
        AscensionPilgrim3,
        JoyousSoul7,
        MadSoul7,
        JoyousSoul8,
        AscensionPilgrim4,
        DynasticSpirit2,
        JoyousSoul9,
        MadSoul8,
        TormentedSoul4,
        CaptainYithlis,
        GarfazSteelfur,
        Igraine,
        TheLost,
        ScoutAhktum,
        Thenemi,
        LieutenantSilmok,
        CommanderWerishakul,
        EnsignCharehli,
        EnsignLumi,
        BosunMohrti,
        AdmiralChiggen,
        MidshipmanMorolah,
        CommanderSehden,
        RisehtheHarmless,
        CommanderWahli,
        CaptainMhedi,
        CaptainAlsin,
        LieutenantShagu,
        AdmiralKaya,
        ArredsCrew,
        CorsairWizard,
        Wianuyo,
        ArredsCrew1,
        CorsairBlackhand,
        CorsairSeaman,
        SmugglerBlackhand,
        CorsairCook,
        SmugglerWizard,
        ArredsCrew2,
        CorsairBosun,
        CorsairSeaman1,
        SmugglerMedic,
        ArredsCrew3,
        CorsairCutthroat,
        CorsairSeaman2,
        SmugglerThug,
        SneakyCorsair,
        ArredsCrew4,
        CorsairRaider,
        CorsairSeaman3,
        SmugglerRaider,
        ArredsCrew5,
        CorsairBerserker,
        CorsairSeaman4,
        SmugglerBerserker,
        ArredsCrew6,
        CorsairCommandant,
        CorsairSeaman5,
        IronfistsEnvoy,
        CorsairMindReader,
        CorsairTorturer,
        CorsairWindMaster,
        CorsairDoctor,
        CorsairWeaponsMaster,
        CorsairLieutenant,
        CorsairGrappler,
        CorsairAdmiral,
        CorsairSeer,
        CorsairFlogger,
        CorsairReefFinder,
        CorsairMedic,
        CorsairThug,
        CorsairLookout,
        CorsairMarauder,
        CorsairCaptain,
        CorsairWizard1,
        CorsairBlackhand1,
        CorsairThug1,
        CorsairCook1,
        CorsairBosun1,
        CorsairCutthroat1,
        CorsairRaider1,
        CorsairBerserker1,
        CorsairCommandant1,
        SuubaytheGreedy,
        WoundedCorsair,
        CorsairMage,
        CorsairHealer,
        CorsairBuccaneer,
        CorsairSpotter,
        CorsairWizard2,
        JackofTruths,
        CorsairBlackhand2,
        GatekeeperKahno,
        Suwash,
        CorsairCook2,
        UnluckySimon,
        CorsairBosun2,
        Dzaga,
        Smuggler,
        CongutheRed,
        CorsairCutthroat2,
        FahijtheSmiles,
        FirstMateGunanu,
        Jehrono,
        JehronoMaterial,
        SavageNunbe,
        CorsairRaider2,
        IronhookHube,
        Sakku,
        Smuggler1,
        CorsairBerserker2,
        OneEyedRugger,
        CaptainBohseda,
        CaptainMindhebeh,
        CorsairCommandant2,
        HasahktheClever,
        HasahktheCleverMaterial,
        HoguntheUnpredictable,
        RuthlessSevad,
        CorsairSpy,
        CommanderWerishakul1,
        EnsignCharehli1,
        EnsignLumi1,
        ArredtheUnready,
        Ironfist,
        ShiftyLem,
        JerobNoSpine,
        ShahaitheCunning,
        CorsairRunner,
        KahlitheStiched,
        TheDarkBlade,
        CaptainBloodFarid,
        CursedSalihm,
        SunspearVolunteer,
        SunspearVolunteer1,
        MidshipmanBeraidun,
        CaptainShehnahr,
        CommanderBahreht,
        EnsignJahan,
        LieutenantMahrik,
        CorsairGuildLord,
        CorsairGuildLord1,
        CorsairWindMaster1,
        CorsairWindMaster2,
        CorsairWeaponsMaster1,
        CorsairWeaponsMaster2,
        CorsairLieutenant1,
        CorsairWeaponsMaster3,
        CorsairTorturer1,
        CorsairLieutenant2,
        CorsairWindMaster3,
        CorsairTorturer2,
        CorsairDoctor1,
        CorsairWeaponsMaster4,
        CorsairAdmiral1,
        CorsairMindReader1,
        CorsairTorturer3,
        CorsairWindMaster4,
        CorsairDoctor2,
        CorsairWindMaster5,
        CorsairWeaponsMaster5,
        CorsairLieutenant3,
        CorsairCommander,
        CorsairCommander1,
        CorsairCommander2,
        CorsairCommander3,
        CorsairCommander4,
        CorsairCommander5,
        CorsairCommander6,
        CorsairCommander7,
        CorsairGhost,
        Abaddon,
        LordJadoth,
        StygianLord,
        StygianLord1,
        StygianLord2,
        StygianLord3,
        TheFury,
        TheBlackBeastofArrgh,
        TheGreaterDarkness,
        TheDarkness,
        ChillofDarkness,
        FuryTitan,
        ShaunurtheDivine,
        TurepMakerofOrphans,
        GuardianofKomalie,
        GuardianofKomalie1,
        StygianUnderlord,
        MargoniteAnurKaya,
        MargoniteAnurDabi,
        MargoniteAnurSu,
        MargoniteAnurKi,
        MargoniteAnurVu,
        MargoniteAnurTuk,
        MargoniteAnurRuk,
        MargoniteAnurRund,
        MargoniteAnurMank,
        StygianHunger,
        StygianBrute,
        StygianGolem,
        StygianHorror,
        StygianFiend,
        MindTormentor,
        SoulTormentor,
        WaterTormentor,
        HeartTormentor,
        FleshTormentor,
        SpiritTormentor,
        EarthTormentor,
        SanityTormentor,
        ThoughtofDarkness,
        ChillofDarkness1,
        ScourgeofDarkness,
        ClawofDarkness,
        WindofDarkness,
        CurseofDarkness,
        Abyssal,
        MiseryTitan,
        RageTitan,
        DementiaTitan,
        AnguishTitan,
        DespairTitan,
        RageTitan1,
        DementiaTitan1,
        DespairTitan1,
        FuryTitan1,
        MindTormentor1,
        SoulTormentor1,
        WaterTormentor1,
        HeartTormentor1,
        FleshTormentor1,
        SpiritTormentor1,
        EarthTormentor1,
        SanityTormentor1,
        TormentClaw1,
        SmotheringTendrils,
        TorturewebDryder,
        GreaterDreamRider,
        GreaterDreamRider1,
        AdeptofWhispers,
        ArchivistofWhispers1,
        ArmorsmithofWhispers,
        DeaconofWhispers,
        KeeperofWhispers,
        OrderofWhispers,
        VoiceofWhispers,
        WhispersInformant,
        CaptainValkyss,
        GeneralYendzarsh,
        Silzesh,
        BindingGuardian,
        LieutenantVanahk,
        CorporalSuli,
        CommanderSadiBelai,
        AdmiralKantoh,
        CorporalArgon,
        MajorJeahr,
        ColonelChaklin,
        ColonelCusto,
        CorporalLuluh,
        CaptainChichor,
        LieutenantKayin,
        CommanderNoss,
        SergeantBehnwa,
        AcolyteofBalthazar,
        AcolyteofDwayna,
        AcolyteofGrenth,
        AcolyteofLyssa,
        AcolyteofMelandru,
        GeneralKahyet,
        TaskmasterVanahk,
        KournanPriest,
        TaskmasterSadiBelai,
        TaskmasterSuli,
        OverseerBoktek,
        OverseerHaubeh,
        CaptainMwende,
        CaptainKavaka,
        CaptainDenduru,
        CaptainLumanda,
        CaptainKuruk,
        CommanderKubeh,
        LieutenantNali,
        CaptainNebo,
        ColonelKajo,
        KournanEliteScribe,
        KournanEliteGuard,
        KournanEliteZealot,
        KournanEliteSpear,
        CaptainBesuz,
        Chirah,
        Wekesha,
        KournanTaskmaster,
        KournanSiegeEngineer,
        Dehjah1,
        CaretakerPalmor,
        Kwaju,
        FreyatheFirm,
        KournanSpotter,
        MerkletheMuscular,
        VaughntheVenerable,
        ElderJonah,
        KournanGuard,
        GuardLinko,
        KournanGuard1,
        ProphetVaresh,
        CommanderVaresh,
        VareshOssa,
        Ahchin,
        KournanEngineer,
        KournanPriest1,
        AgentofWhispers,
        Haibir,
        Kehtur,
        KournanGuard2,
        KournanScout,
        KournanSoldier,
        KournanBowman,
        KournanCommander,
        KournanPhalanx,
        ShipmentGuardCaptain,
        KournanSeer,
        KournanSeer1,
        KournanOppressor,
        KournanOppressor1,
        KournanCommander1,
        KournanScribe,
        KournanScribe1,
        KournanPriest2,
        KournanPriest3,
        KournanGuard3,
        KournanGuard4,
        KournanSoldier1,
        KournanGuard5,
        KournanGuard6,
        KournanGuard7,
        KournanGuard8,
        KournanBowman1,
        KournanBowman2,
        KournanZealot,
        KournanZealot1,
        GuardPostCommander,
        KournanCaptain,
        KournanPhalanx1,
        KournanPhalanx2,
        KournanFieldCommander,
        KournanPhalanx3,
        KournanGuard9,
        KournanChild,
        Nerwa,
        KournanChild1,
        Bahldasareh,
        EstateGuardRikesh,
        JailerGahanni,
        KournanGuard10,
        KournanSoldier2,
        Maho,
        KournanGuard11,
        KournanSoldier3,
        CaptainMehhan,
        CaptainNahnkos,
        GuardCaptainKahturin,
        KournanCaptain1,
        PrisonKeeperShelkesh,
        SunspearCaptainMission,
        VaultMasterEijah,
        CounselorRahburt,
        KournanGuard12,
        KournanSoldier4,
        EmissaryDajmir,
        GerahladMahkfahlan,
        Iasha,
        KournanNoble,
        Medando,
        StefantheSonorous,
        Vohwash,
        Wedende,
        ZudashDejarin,
        KournanNoble1,
        Varesh,
        EngineerTosi,
        FarmerGorkan,
        Fumeai,
        HerbalistMakala,
        HerbalistUmjabwe,
        KournanPeasant,
        Mubata,
        Mubata1,
        Nihanu,
        SukohtJahrevit,
        BehnotuSupehnahn,
        DockmasterDimedeh,
        FarmerOrjok,
        HerdsmanZekanu,
        KournanPeasant1,
        GuardsmanBahsi,
        HerdsmanMehnosi,
        Kahan,
        KournanPeasant2,
        DreamerRaja,
        ElderIsma,
        Janeera,
        KournanPeasant3,
        Kwesi,
        Ossjo,
        TohmahsSukobehr,
        Helenah,
        Kazleht,
        KournanPeasant4,
        Pehtigam,
        DreamerHahla,
        Dzawan,
        Dzawan1,
        KournanPeasant5,
        Chuno,
        Sinbi,
        Sinbi1,
        KournanNoble2,
        WanderingPriest,
        KournanNoble3,
        WanderingPriest1,
        Burreh,
        Itenda,
        Jormar,
        Lormeh,
        Enbe,
        Ashnod,
        Dulon,
        Farim,
        Kahdeh,
        Lorossa,
        Nehtumbah,
        Riwar,
        Sahnbur,
        Totando,
        Turshi,
        Yerin,
        Ahleri,
        Dijahpo,
        Judila,
        Jurani,
        Koulaba,
        Lumesah,
        Perdahn,
        Ralaja,
        Raleva,
        Hadusi,
        Hahbe,
        Sende,
        Bolereh,
        Tehshan,
        Ahkessa,
        Benera,
        Benera1,
        Odahn,
        Pohgri,
        Lutinu,
        Yahtu,
        Nehbusa,
        Sulumba,
        OrramMaterial,
        FeriiMaterial,
        StashehMaterial,
        Megundeh,
        Megundeh1,
        Fahpo,
        Shanka,
        Kerendu,
        KohnScroll,
        HarfehlaScroll,
        HarbingerofNightfall,
        Harbinger,
        TheHunger,
        GeneralBayel,
        HarbingerofNightfall1,
        HarbingerofTwilight,
        HarbingerofTwilight1,
        EmissaryofDhuum,
        ShadowofFear,
        BladeofCorruption,
        EmissaryofDhuum1,
        DemonSpawn,
        DemonicFortuneTeller,
        EmissaryofDhuum2,
        Razakel,
        TorturewebDryder1,
        ShepherdofDementia,
        OnslaughtofTerror,
        OathofProfanity,
        StormofAnguish,
        FlameofFervor,
        SeedofSuffering,
        ShriekerofDread,
        VisionofDespair,
        BringerofDeceit,
        BearerofMisfortune,
        HeraldofNightmares,
        ShadowofFear1,
        RainofTerror,
        WordofMadness,
        BladeofCorruption1,
        ArmofInsanity,
        ScytheofChaos3,
        SpearofTorment,
        HeraldofNightmares1,
        ShadowofFear2,
        RainofTerror1,
        WordofMadness1,
        BladeofCorruption2,
        ArmofInsanity1,
        ScytheofChaos4,
        SpearofTorment1,
        TormentClaw2,
        TormentClaw3,
        TormentClaw4,
        PreceptorZunark,
        RebirtherJirath,
        ScribeWensal,
        PriestZeinZuu,
        ZealotSheoli,
        BattlelordTurgar,
        CommanderChutal,
        MargoniteCleric,
        CuratorKali,
        GeneralTirraj,
        GeneralNimtak,
        ChampionPuran,
        GeneralDoriah,
        GeneralKumtash,
        TormentWeaver,
        ZealousAmarantha,
        AbaddonsAdjutant,
        MargoniteHighPriest,
        MargonitePatriarch,
        HordeofDarkness,
        UnboundEnergy,
        TheBlasphemy,
        Apostate,
        HausehtheDefiler,
        TanmahktheArcane,
        DupektheMighty,
        MargoniteWarlock,
        MargoniteSorcerer,
        MargoniteCleric1,
        MargoniteExecutioner,
        MargoniteBowmaster,
        MargoniteReaper,
        MargonitePortalMage,
        RualStealerofHope,
        TureksintheDelegator,
        SaushalitheFrustrating,
        TaintheCorrupter,
        LushivahrtheInvoker,
        BriahntheChosen,
        GrabthartheOverbearing,
        ChimortheLightblooded,
        FahralontheZealous,
        WieshurtheInspiring,
        HautohthePilferer,
        MargoniteSeer,
        MargoniteWarlock1,
        MargoniteCleric2,
        MargoniteExecutioner1,
        MargoniteBowmaster1,
        MargoniteScout,
        MargoniteSeer1,
        MargoniteSouleater,
        MargoniteSouleater1,
        MargoniteWarlock2,
        MargoniteSorcerer1,
        MargoniteSouleater2,
        MargoniteCleric3,
        MargoniteScout1,
        MargoniteSouleater3,
        MargoniteExecutioner2,
        MargoniteScout2,
        MargoniteSouleater4,
        MargoniteStalker,
        MargoniteBowmaster2,
        MargoniteScout3,
        MargoniteSouleater5,
        MargoniteReaper1,
        MargoniteScout4,
        MargoniteSouleater6,
        MargoniteAscendant,
        MargoniteScout5,
        MargoniteSouleater7,
        AncientUndead,
        RestlessDead,
        AncientUndead1,
        RelentlessCorpse,
        AncientUndead2,
        AncientUndead3,
        DarehktheQuick,
        GhostlySunspear,
        GhostlySunspearCommander,
        Kahdash,
        GhostlyScout,
        SunspearGhost,
        GhostlyScout1,
        SunspearGhost1,
        TorturedSunspear,
        FirstSpearDanah,
        FirstSpearJanah,
        SogolontheProtector,
        SpearmarshallKojolin,
        ConciliatorKomah,
        KojolintheConciliator,
        ShekosstheStony,
        MekirthePrismatic,
        ArdehtheQuick,
        EshwetheInsane,
        KehttheFierce,
        ArnehtheVigorous,
        HamlentheFallen,
        KoahmtheWeary,
        FondalztheSpiteful,
        NehjabtheParched,
        ShelkehtheHungry,
        AmireshthePious,
        ChurkehtheDefiant,
        BohdabitheDestructive,
        VahlentheSilent,
        AlemtheUnclean,
        Aijunundu,
        QueenAijundu,
        AmindtheBitter,
        NehmaktheUnpleasant,
        AvahtheCrafty,
        ChundutheMeek,
        ChakehtheLonely,
        AwakenedThoughtLeech,
        AwakenedDefiler,
        CarvenEffigy,
        AwakenedAcolyte,
        AwakenedBlademaster,
        AwakenedGrayGiant,
        AwakenedHead,
        AwakenedDuneCarver,
        AwakenedCavalier,
        RebelliousGeneral,
        SpiritofSeborhin,
        Thrall,
        OfficerLohru,
        Thrall1,
        Thrall2,
        Thrall3,
        Thrall4,
        Thrall5,
        Fahrankelon,
        Thrall6,
        SlaveSpirit,
        SlaveSpirit1,
        ScoutAhtok,
        PrimevalKingJahnus,
        KarehOssa,
        RoyalGuard2,
        JununduYoung,
        CommanderMosek,
        CommanderGiturh,
        CommanderLohgor,
        CommanderYamji,
        InfantrymanHareh,
        GeneralFarrund,
        PalawaJoko,
        ElderSiegeWurm,
        Buuran,
        Chessa,
        Chinwe,
        Dayoesh,
        DynasticSpirit3,
        Ganji,
        Ghost,
        Johe,
        Juba,
        Kane,
        Krahm,
        Larano,
        MyrishtheSlave,
        Nikun,
        TormentedSoul5,
        Charen,
        FreedSoul,
        Jehner,
        Kane1,
        Allura,
        DynasticSpirit4,
        Ghost1,
        GhostofSehwanu,
        Godaj,
        Shandara,
        Shanrah,
        TormentedSoul6,
        FreedSoul1,
        CuratorRuras,
        Dau,
        DynasticSpirit5,
        GhostlyPriest,
        Jehner1,
        Shahin,
        Khendi,
        Ahn,
        Aisu,
        Kenmu,
        Padu,
        Ruricu,
        Tatau,
        Urlen,
        Yovel,
        Dahn,
        Yovel1,
        Palmod,
        ZellMaterial,
        Awata,
        BonesmithRokel,
        Fanri,
        Zenbu,
        PriestKehmtut,
        Jasek,
        Ehndu,
        AxshaiScroll,
        LomehScroll,
        AhndurizBohim,
        Guard,
        JohanGarham,
        JonossNodahlon,
        Kuwame,
        MikahlHintohn,
        SergeantBokkun,
        SergeantBolrob,
        Siktur,
        VabbiGuard,
        VabbianGuard,
        VabbianScout,
        VabbiGuard1,
        RescueGuard,
        VabbianGuard1,
        ButohtheBold,
        CaptainJafahni,
        GahridDareshlar,
        MahkJenshan,
        PalaceGuard,
        SeborhinProtectorZuor,
        PalaceGuard1,
        JennurKurahHatohn,
        LieutenantMurunda,
        CommanderTanmod,
        GeneralPoruk,
        GuardCaptain,
        RoyalGuardBunda,
        RoyalGuardZendeh,
        VabbiGuardCaptain,
        VabbianGuard2,
        VabbiGuardCaptain1,
        CaptainPortir,
        LieutenantMurunda1,
        VabbiGuardCaptain2,
        VabbianGuard3,
        VabbiGuardCaptain3,
        KahturinHovohden,
        LohizHoput,
        ScoutDehra,
        VabbiGuard2,
        VabbianGuard4,
        VabbiGuard3,
        VabbianGuard5,
        Ahoj,
        Dende,
        VabbianChild,
        VabbianCommoner2,
        YoungChild2,
        Charmah,
        Mina,
        VabbiChild,
        VabbianChild1,
        VabbianCommoner3,
        YoungChild3,
        Bahskir,
        Baram,
        Behruseh,
        BrihanChahu,
        DehvitNotigunahm,
        Gahnlar,
        HervahntheVexing,
        HorticulturistHinon,
        JahmehBahkmasitur,
        JiorijWahlahz,
        KaristendBehrisfahr,
        Kurmauzeh,
        Kwayama,
        LibrarianEhrahtimos,
        LibraryEnvoyChukeht,
        MasterLibrarianAntohneoss,
        NehtohnEhmbahrsin,
        NoblemanEmrah,
        Norgu1,
        Ordash,
        Peyema,
        Resh,
        RoyalFinanceMinisterOluda,
        RoyalFoodTasterRendu,
        Sholhij,
        Shosa,
        TahravohrGarifiz,
        TheatreManagerDaudi,
        TohnehJonesh,
        VabbiNoble,
        VabbianNoble,
        VaughntheVenerable1,
        Wekekuda,
        ZeraitheLearner,
        Endah,
        EventPlannerKazsha,
        Jekunda,
        KahlinIdehjir,
        KaturinEhmbolem,
        LibrarianKahlidahri,
        PehniRahnad,
        PrincessLeifah,
        Ridara,
        Sinvahn,
        VabbiNoble1,
        VabbianGypsy,
        VabbianNoble1,
        Yuli,
        AttendanttothePrince,
        HorticulturistHinon1,
        LarrinBahlakbahn,
        LibrarianGahmirLenon,
        LibrarianMularuk,
        LibrarianNichitheRanger,
        MasterGardenerKobahl,
        PahulJohansohn,
        RoyalFoodTasterRendu1,
        VabbianCommoner4,
        VabbianGypsy1,
        ArchivistMijir,
        LibrarianChitohn,
        MahrekChuri,
        RoyalChefHatundo,
        Sakutila,
        VabbianCommoner5,
        VabbianGypsy2,
        AhdariDohchimahn,
        JenRohsin,
        Jermahzeh,
        Lahati,
        LibrarianKahnu,
        Nuwisha,
        VabbianCommoner6,
        VabbianGypsy3,
        Kalanda,
        Rahvu,
        VabbianCommoner7,
        Abaddon1,
        AssistantLenahn,
        Belanu,
        BelguntheQuarryMaster,
        Dahwan,
        HedgeWizardMabai,
        JahkKihngah,
        Korvus,
        KuridehtheMad,
        Kurli,
        Lahri,
        LumotheMime,
        Moh,
        Musician,
        OlatheFantastical,
        Olujime,
        RoyalServant,
        Shashi,
        VabbiPeasant,
        VabbianCommoner8,
        VabbianMiner,
        Zardok,
        AssistantJenahn,
        DuelMasterLumbo,
        GardenerTidak,
        Hamri,
        JohinPiht,
        Kamveh,
        Kenyatta,
        Musician1,
        Narrator,
        RoyalServant1,
        Talgun,
        VabbiPeasant1,
        VabbianCommoner9,
        VabbianMiner1,
        ZilotheDrunkard,
        Amadi,
        Awadur,
        Bahlbahs,
        Charmamani,
        Hermehinu,
        Kaya1,
        Kimab,
        Musician2,
        PriestessHaila,
        RoyalServant2,
        VabbiPeasant2,
        VabbianCommoner10,
        VabbianGypsy4,
        ArchivistLeiton,
        Haitend,
        HeadPriestVahmani,
        Kachok,
        MasterofCeremonies,
        PriestJeshek,
        PriestTuwahan,
        RecordsKeeperPalin,
        VabbianPriest,
        Larmor,
        Nerwar,
        Lilita,
        Pakasah,
        Mateneh,
        Buuh,
        Dazah,
        Deshwa,
        Jinte,
        Magis,
        Noben,
        Ohapeh,
        Papo,
        Peshwan,
        Zojan,
        Ahjii,
        Itai,
        Jahnbur,
        Merassi,
        Sahwan,
        Shanta,
        Turissa,
        Yilai,
        Aksah,
        Rahmdah,
        Wisseh,
        Asiri,
        Kehjim,
        Amurte,
        Rahd,
        Narjisuh,
        Shahler,
        Bodahn,
        Harib,
        Urij,
        Naksem,
        KolonaMaterial,
        LurlaiMaterial,
        PerashMaterial,
        ZatahMaterial,
        IreshScroll,
        TotarScroll,
        WehwahScroll,
        LajariScroll,
        OuridaScroll,
        Hesham,
        Karmu,
        Fahnesh,
        Lahi,
        GuardianofWhispers,
        Mureh,
        SeekerofWhispers,
        SourceofWhispers,
        WanderingScribe,
        WardenofWhispers,
        WhispersAcolyte,
        WhispersAdept,
        WhispersCrusaderMission,
        WhispersInformant1,
        WitnessAhtok,
        Merkod,
        Nadara,
        Kehanni,
        PrinceBokkatheMagnificent,
        PrinceAhmturtheMighty,
        PrinceMehtutheWise,
        AdeptofWhispers1,
        DiscipleofSecrets,
        WardenofWhispers1,
        Vidaj,
        HanbahdtheAnchorite,
        TheGreatZehtuka1,
        PalawaJoko1,
        GardenerPohsin,
        ScholarAiki,
        ScholarGahesh,
        Diji,
        ScholarBelzar,
        ScholarDakkun,
        ScholarKoben,
        ScholarZelkun,
        ScholarKammab,
        ScholarMehdok,
        LordYamatheVengeful,
        MessengerofLyssa,
        HorticulturistHinon2,
        ErnaldtheExact,
        Fissure,
        Vekk1,
        OgdenStonehealer1,
        DestroyerofSinew,
        DestroyerofBones,
        DestroyerofFlesh,
        DwarvenDemolitionist1,
        Borvorel1,
        BurolIronfist1,
        Kodan1,
        AdmiralJakman,
        CorsairWizard3,
        CorsairWindMaster6,
        CorsairCutthroat3,
        CorsairRaider3,
        CorsairBerserker3,
        CorsairCommandant3,
        AwakenedThoughtLeech1,
        AwakenedDefiler1,
        AcolyteofJoko,
        CarvenEffigy1,
        AwakenedAcolyte1,
        AwakenedBlademaster1,
        AwakenedGrayGiant1,
        AwakenedDuneCarver1,
        AwakenedTrooper,
        PalawaJoko2,
        JokosEliteBodyguard,
        EliteKournanTrooper,
        VabbianRefugeeSoldier,
        VabbianRefugeeSoldier1,
        VabbianRefugeeSoldier2,
        VabbianRefugeeCommander,
        VabbianRefugeeSoldier3,
        VabbianRefugeeSoldier4,
        SunspearCommander,
        SunspearVolunteer2,
        SunspearVolunteer3,
        AwakenedDefiler2,
        MOX1,
        NicholastheTraveler,
        ProfessorYakkington,
        Zenjal1,
        ForemantheCrier1,
        BeastSwornHeket3,
        CorsairBerserker4,
        ImmolatedDjinn1,
        Lioness1,
        DesertWurm1,
        RestlessDead1,
        BitGolem,
        BreakerBitGolem,
        FireBitGolem,
        NOX,
        PhantomBitGolem,
        PrinceBokka,
        RectifierBitGolem,
        ThunderBitGolem,
        VabbianActor,
        VabbianActress,
        VabbianStagehand,
        VabbianTheaterManager,
        IstaniFarmer,
        KournanPriest4,
        KournanScribe2,
        BahltekTheFirst,
        FrigidSkaleTheFirst,
        SkaleBlighterTheFirst,
        SkreeTalonTheFirst,
        Lion1,
        MargoniteSorcerer2,
        MargoniteWarlock3,
        AnyDjinnand,
        LockofAhdashimand,
        EnsignLumi2,
        ArmofInsanity2,
        BladeofCorruption3,
        ShadowofFear3,
        KournanEliteZealotCrossroads,
        KournanOppressor2,
        KournanScribe3,
        KournanSeer2,
        Sheltah,
        HeraldofNightmares2,
        PainTitan1,
        UnsuspectingKournanGuardMeNo,
        KournanEliteGuardPassage,
        KournanEliteScribePassage,
        KournanEliteSpearPassage,
        KournanPhalanx4,
        MandragorSlither4,
        ScytheofChaosAlkali,
        MOX2,
        Olias1,
        Olias2,
        Olias3,
        Zenmai1,
        Zenmai2,
        Zenmai3,
        Norgu2,
        Norgu3,
        Norgu4,
        Goren2,
        Goren3,
        Goren4,
        ZhedShadowhoof1,
        ZhedShadowhoof2,
        ZhedShadowhoof3,
        GeneralMorgahn1,
        GeneralMorgahn2,
        GeneralMorgahn3,
        MargridtheSly1,
        MargridtheSly2,
        MargridtheSly3,
        Tahlkora1,
        Tahlkora2,
        Tahlkora3,
        MasterofWhispers1,
        MasterofWhispers2,
        MasterofWhispers3,
        AcolyteJin1,
        AcolyteJin2,
        AcolyteJin3,
        Koss3,
        Koss4,
        Koss5,
        Dunkoro1,
        Dunkoro2,
        Dunkoro3,
        AcolyteSousuke1,
        AcolyteSousuke2,
        AcolyteSousuke3,
        Melonni1,
        Melonni2,
        Melonni3,
        Razah1,
        Razah2,
        Razah3,
        Razah4,
        Vekk2,
        Vekk3,
        Vekk4,
        PyreFierceshot,
        PyreFierceshot1,
        PyreFierceshot2,
        OgdenStonehealer2,
        OgdenStonehealer3,
        OgdenStonehealer4,
        Anton,
        Anton1,
        Anton2,
        Livia,
        Livia1,
        Livia2,
        Hayda,
        Hayda1,
        Hayda2,
        Kahmu,
        Kahmu1,
        Kahmu2,
        Gwen,
        Gwen1,
        Gwen2,
        Xandra,
        Xandra1,
        Xandra2,
        Jora,
        Jora1,
        Jora2,
        KeiranThackeray,
        Miku,
        ZeiRi,
        CharrShaman,
        BoneDragon,
        PrinceRurik,
        Shiro,
        BurningTitan,
        Kirin,
        NecridHorseman,
        JadeArmor,
        Hydra,
        FungalWallow1,
        SiegeTurtle3,
        TempleGuardian2,
        JungleTroll,
        WhiptailDevourer,
        Gwen3,
        GwenDoll,
        WaterDjinn3,
        Lich,
        Elf,
        PalawaJoko3,
        Koss6,
        MandragorImp4,
        HeketWarrior,
        HarpyRanger,
        Juggernaut,
        WindRider,
        FireImp,
        Aaxte,
        ThornWolf1,
        Abyssal1,
        BlackBeastofAaaaarrrrrrggghhh,
        Freezie,
        Irukandji2,
        MadKingThorn,
        ForestMinotaur,
        Mursaat,
        Nornbear,
        Ooze,
        Raptor,
        RoaringEther2,
        CloudtouchedSimian,
        CaveSpider,
        WhiteRabbit,
        WordofMadness2,
        DredgeBrute,
        TerrorwebDryder1,
        Abomination,
        KraitNeoss,
        DesertGriffon,
        Kveldulf,
        QuetzalSly,
        Jora3,
        FlowstoneElemental,
        Nian,
        DagnarStonepate,
        FlameDjinn,
        EyeofJanthir,
        Seer,
        SiegeDevourer,
        ShardWolf,
        FireDrake,
        SummitGiantHerder,
        OphilNahualli,
        CobaltScabara1,
        ScourgeManta1,
        Ventari,
        Oola,
        CandysmithMarley,
        ZhuHanuku1,
        KingAdelbern,
        MOX3,
        MOX4,
        MOX5,
        MOX6,
        MOX7,
        MOX8,
        BrownRabbit,
        Yakkington,
        Kuunavang2,
        GrayGiant,
        Asura,
        DestroyerofFlesh1,
        PolarBear,
        VareshOssa1,
        Mallyx,
        Ceratadon,
        Kanaxai1,
        Panda,
        IslandGuardian1,
        NagaRaincaller1,
        LonghairYeti1,
        Oni5,
        ShirokenAssassin3,
        Vizu1,
        ZhedShadowhoof4,
        Grawl,
        GhostlyHero,
        Pig,
        GreasedLightning,
        WorldFamousRacingBeetle,
        CelestialPig,
        CelestialRat,
        CelestialOx,
        CelestialTiger,
        CelestialRabbit,
        CelestialDragon,
        CelestialSnake,
        CelestialHorse,
        CelestialSheep,
        CelestialMonkey,
        CelestialRooster,
        CelestialDog,
        BlackMoaChick,
        Dhuum,
        MadKingsGuard,
        SmiteCrawler,
        GuildLord,
        HighPriestZhang,
        GhostlyPriest1,
        RiftWarden,
        Legionnaire,
        ConfessorDorian,
        PrincessSalma,
        Livia3,
        Evennia,
        ConfessorIsaish,
        PeacekeeperEnforcer,
        MinisterReiko,
        EcclesiateXunRao,
        TheFrog,
        TheFrog1,
        TheFrog2
    };

    public static bool TryParse(int id, out Npc npc)
    {
        npc = Npcs.Where(n => n.Id == id).FirstOrDefault()!;
        if (npc is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Npc npc)
    {
        npc = Npcs.Where(n => n.Name == name).FirstOrDefault()!;
        if (npc is null)
        {
            return false;
        }

        return true;
    }
    public static Npc Parse(int id)
    {
        if (TryParse(id, out var npc) is false)
        {
            throw new InvalidOperationException($"Could not find an npc with id {id}");
        }

        return npc;
    }
    public static Npc Parse(string name)
    {
        if (TryParse(name, out var npc) is false)
        {
            throw new InvalidOperationException($"Could not find an npc with name {name}");
        }

        return npc;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WikiUrl { get; set; } = string.Empty;

    private Npc()
    {
    }
}
