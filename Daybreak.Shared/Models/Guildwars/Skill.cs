using Daybreak.Shared.Converters;
using Newtonsoft.Json;

namespace Daybreak.Shared.Models.Guildwars;

[JsonConverter(typeof(SkillJsonConverter))]
public sealed class Skill
{
    public static readonly Skill NoSkill = new() { Id = 0, Name = "No Skill", Profession = Profession.None };
    public static readonly Skill ResurrectionSignet = new() { Id = 2, Name = "Resurrection Signet", Profession = Profession.None };
    public static readonly Skill SignetofCapture = new() { Id = 3, Name = "Signet of Capture", Profession = Profession.None };
    public static readonly Skill CycloneAxe = new() { Id = 330, Name = "Cyclone Axe", Profession = Profession.Warrior };
    public static readonly Skill AxeRake = new() { Id = 334, Name = "Axe Rake", Profession = Profession.Warrior };
    public static readonly Skill Cleave = new() { Id = 335, Name = "Cleave", Profession = Profession.Warrior };
    public static readonly Skill ExecutionersStrike = new() { Id = 336, Name = "Executioner's Strike", Profession = Profession.Warrior };
    public static readonly Skill Dismember = new() { Id = 337, Name = "Dismember", Profession = Profession.Warrior };
    public static readonly Skill Eviscerate = new() { Id = 338, Name = "Eviscerate", Profession = Profession.Warrior };
    public static readonly Skill PenetratingBlow = new() { Id = 339, Name = "Penetrating Blow", Profession = Profession.Warrior };
    public static readonly Skill DisruptingChop = new() { Id = 340, Name = "Disrupting Chop", Profession = Profession.Warrior };
    public static readonly Skill SwiftChop = new() { Id = 341, Name = "Swift Chop", Profession = Profession.Warrior };
    public static readonly Skill AxeTwist = new() { Id = 342, Name = "Axe Twist", Profession = Profession.Warrior };
    public static readonly Skill LaceratingChop = new() { Id = 849, Name = "Lacerating Chop", Profession = Profession.Warrior };
    public static readonly Skill WhirlingAxe = new() { Id = 888, Name = "Whirling Axe", Profession = Profession.Warrior };
    public static readonly Skill FuriousAxe = new() { Id = 904, Name = "Furious Axe", Profession = Profession.Warrior };
    public static readonly Skill TripleChop = new() { Id = 992, Name = "Triple Chop", Profession = Profession.Warrior };
    public static readonly Skill PenetratingChop = new() { Id = 1136, Name = "Penetrating Chop", Profession = Profession.Warrior };
    public static readonly Skill CriticalChop = new() { Id = 1402, Name = "Critical Chop", Profession = Profession.Warrior };
    public static readonly Skill AgonizingChop = new() { Id = 1403, Name = "Agonizing Chop", Profession = Profession.Warrior };
    public static readonly Skill Decapitate = new() { Id = 1696, Name = "Decapitate", Profession = Profession.Warrior };
    public static readonly Skill KeenChop = new() { Id = 2009, Name = "Keen Chop", Profession = Profession.Warrior };
    public static readonly Skill HammerBash = new() { Id = 331, Name = "Hammer Bash", Profession = Profession.Warrior };
    public static readonly Skill BellySmash = new() { Id = 350, Name = "Belly Smash", Profession = Profession.Warrior };
    public static readonly Skill MightyBlow = new() { Id = 351, Name = "Mighty Blow", Profession = Profession.Warrior };
    public static readonly Skill CrushingBlow = new() { Id = 352, Name = "Crushing Blow", Profession = Profession.Warrior };
    public static readonly Skill CrudeSwing = new() { Id = 353, Name = "Crude Swing", Profession = Profession.Warrior };
    public static readonly Skill EarthShaker = new() { Id = 354, Name = "Earth Shaker", Profession = Profession.Warrior };
    public static readonly Skill DevastatingHammer = new() { Id = 355, Name = "Devastating Hammer", Profession = Profession.Warrior };
    public static readonly Skill IrresistibleBlow = new() { Id = 356, Name = "Irresistible Blow", Profession = Profession.Warrior };
    public static readonly Skill CounterBlow = new() { Id = 357, Name = "Counter Blow", Profession = Profession.Warrior };
    public static readonly Skill Backbreaker = new() { Id = 358, Name = "Backbreaker", Profession = Profession.Warrior };
    public static readonly Skill HeavyBlow = new() { Id = 359, Name = "Heavy Blow", Profession = Profession.Warrior };
    public static readonly Skill StaggeringBlow = new() { Id = 360, Name = "Staggering Blow", Profession = Profession.Warrior };
    public static readonly Skill FierceBlow = new() { Id = 850, Name = "Fierce Blow", Profession = Profession.Warrior };
    public static readonly Skill ForcefulBlow = new() { Id = 889, Name = "Forceful Blow", Profession = Profession.Warrior };
    public static readonly Skill AuspiciousBlow = new() { Id = 905, Name = "Auspicious Blow", Profession = Profession.Warrior };
    public static readonly Skill EnragedSmash = new() { Id = 993, Name = "Enraged Smash", Profession = Profession.Warrior };
    public static readonly Skill RenewingSmash = new() { Id = 994, Name = "Renewing Smash", Profession = Profession.Warrior };
    public static readonly Skill YetiSmash = new() { Id = 1137, Name = "Yeti Smash", Profession = Profession.Warrior };
    public static readonly Skill MokeleSmash = new() { Id = 1409, Name = "Mokele Smash", Profession = Profession.Warrior };
    public static readonly Skill OverbearingSmash = new() { Id = 1410, Name = "Overbearing Smash", Profession = Profession.Warrior };
    public static readonly Skill MagehuntersSmash = new() { Id = 1697, Name = "Magehunter's Smash", Profession = Profession.Warrior };
    public static readonly Skill PulverizingSmash = new() { Id = 2008, Name = "Pulverizing Smash", Profession = Profession.Warrior };
    public static readonly Skill BattleRage = new() { Id = 317, Name = "Battle Rage", Profession = Profession.Warrior };
    public static readonly Skill DefyPain = new() { Id = 318, Name = "Defy Pain", Profession = Profession.Warrior };
    public static readonly Skill Rush = new() { Id = 319, Name = "Rush", Profession = Profession.Warrior };
    public static readonly Skill PowerAttack = new() { Id = 322, Name = "Power Attack", Profession = Profession.Warrior };
    public static readonly Skill ProtectorsStrike = new() { Id = 326, Name = "Protector's Strike", Profession = Profession.Warrior };
    public static readonly Skill GriffonsSweep = new() { Id = 327, Name = "Griffon's Sweep", Profession = Profession.Warrior };
    public static readonly Skill BullsStrike = new() { Id = 332, Name = "Bull's Strike", Profession = Profession.Warrior };
    public static readonly Skill IWillAvengeYou = new() { Id = 333, Name = "\"I Will Avenge You!\"", Profession = Profession.Warrior };
    public static readonly Skill EndurePain = new() { Id = 347, Name = "Endure Pain", Profession = Profession.Warrior };
    public static readonly Skill Sprint = new() { Id = 349, Name = "Sprint", Profession = Profession.Warrior };
    public static readonly Skill DolyakSignet = new() { Id = 361, Name = "Dolyak Signet", Profession = Profession.Warrior };
    public static readonly Skill WarriorsCunning = new() { Id = 362, Name = "Warrior's Cunning", Profession = Profession.Warrior };
    public static readonly Skill ShieldBash = new() { Id = 363, Name = "Shield Bash", Profession = Profession.Warrior };
    public static readonly Skill IWillSurvive = new() { Id = 368, Name = "\"I Will Survive!\"", Profession = Profession.Warrior };
    public static readonly Skill BerserkerStance = new() { Id = 370, Name = "Berserker Stance", Profession = Profession.Warrior };
    public static readonly Skill WarriorsEndurance = new() { Id = 374, Name = "Warrior's Endurance", Profession = Profession.Warrior };
    public static readonly Skill DwarvenBattleStance = new() { Id = 375, Name = "Dwarven Battle Stance", Profession = Profession.Warrior };
    public static readonly Skill BullsCharge = new() { Id = 379, Name = "Bull's Charge", Profession = Profession.Warrior };
    public static readonly Skill Flourish = new() { Id = 389, Name = "Flourish", Profession = Profession.Warrior };
    public static readonly Skill PrimalRage = new() { Id = 831, Name = "Primal Rage", Profession = Profession.Warrior };
    public static readonly Skill SignetofStrength = new() { Id = 944, Name = "Signet of Strength", Profession = Profession.Warrior };
    public static readonly Skill TigerStance = new() { Id = 995, Name = "Tiger Stance", Profession = Profession.Warrior };
    public static readonly Skill LeviathansSweep = new() { Id = 1134, Name = "Leviathan's Sweep", Profession = Profession.Warrior };
    public static readonly Skill YouWillDie = new() { Id = 1141, Name = "\"You Will Die!\"", Profession = Profession.Warrior };
    public static readonly Skill Flail = new() { Id = 1404, Name = "Flail", Profession = Profession.Warrior };
    public static readonly Skill ChargingStrike = new() { Id = 1405, Name = "Charging Strike", Profession = Profession.Warrior };
    public static readonly Skill Headbutt = new() { Id = 1406, Name = "Headbutt", Profession = Profession.Warrior };
    public static readonly Skill LionsComfort = new() { Id = 1407, Name = "Lion's Comfort", Profession = Profession.Warrior };
    public static readonly Skill RageoftheNtouka = new() { Id = 1408, Name = "Rage of the Ntouka", Profession = Profession.Warrior };
    public static readonly Skill SignetofStamina = new() { Id = 1411, Name = "Signet of Stamina", Profession = Profession.Warrior };
    public static readonly Skill BurstofAggression = new() { Id = 1413, Name = "Burst of Aggression", Profession = Profession.Warrior };
    public static readonly Skill EnragingCharge = new() { Id = 1414, Name = "Enraging Charge", Profession = Profession.Warrior };
    public static readonly Skill Counterattack = new() { Id = 1693, Name = "Counterattack", Profession = Profession.Warrior };
    public static readonly Skill MagehunterStrike = new() { Id = 1694, Name = "Magehunter Strike", Profession = Profession.Warrior };
    public static readonly Skill Disarm = new() { Id = 2066, Name = "Disarm", Profession = Profession.Warrior };
    public static readonly Skill IMeanttoDoThat = new() { Id = 2067, Name = "\"I Meant to Do That!\"", Profession = Profession.Warrior };
    public static readonly Skill BodyBlow = new() { Id = 2197, Name = "Body Blow", Profession = Profession.Warrior };
    public static readonly Skill Hamstring = new() { Id = 320, Name = "Hamstring", Profession = Profession.Warrior };
    public static readonly Skill PureStrike = new() { Id = 328, Name = "Pure Strike", Profession = Profession.Warrior };
    public static readonly Skill HundredBlades = new() { Id = 381, Name = "Hundred Blades", Profession = Profession.Warrior };
    public static readonly Skill SeverArtery = new() { Id = 382, Name = "Sever Artery", Profession = Profession.Warrior };
    public static readonly Skill GalrathSlash = new() { Id = 383, Name = "Galrath Slash", Profession = Profession.Warrior };
    public static readonly Skill Gash = new() { Id = 384, Name = "Gash", Profession = Profession.Warrior };
    public static readonly Skill FinalThrust = new() { Id = 385, Name = "Final Thrust", Profession = Profession.Warrior };
    public static readonly Skill SeekingBlade = new() { Id = 386, Name = "Seeking Blade", Profession = Profession.Warrior };
    public static readonly Skill SavageSlash = new() { Id = 390, Name = "Savage Slash", Profession = Profession.Warrior };
    public static readonly Skill SunandMoonSlash = new() { Id = 851, Name = "Sun and Moon Slash", Profession = Profession.Warrior };
    public static readonly Skill QuiveringBlade = new() { Id = 892, Name = "Quivering Blade", Profession = Profession.Warrior };
    public static readonly Skill DragonSlash = new() { Id = 907, Name = "Dragon Slash", Profession = Profession.Warrior };
    public static readonly Skill StandingSlash = new() { Id = 996, Name = "Standing Slash", Profession = Profession.Warrior };
    public static readonly Skill JaizhenjuStrike = new() { Id = 1135, Name = "Jaizhenju Strike", Profession = Profession.Warrior };
    public static readonly Skill SilverwingSlash = new() { Id = 1144, Name = "Silverwing Slash", Profession = Profession.Warrior };
    public static readonly Skill CripplingSlash = new() { Id = 1415, Name = "Crippling Slash", Profession = Profession.Warrior };
    public static readonly Skill BarbarousSlice = new() { Id = 1416, Name = "Barbarous Slice", Profession = Profession.Warrior };
    public static readonly Skill SteelfangSlash = new() { Id = 1702, Name = "Steelfang Slash", Profession = Profession.Warrior };
    public static readonly Skill KneeCutter = new() { Id = 2010, Name = "Knee Cutter", Profession = Profession.Warrior };
    public static readonly Skill HealingSignet = new() { Id = 1, Name = "Healing Signet", Profession = Profession.Warrior };
    public static readonly Skill TotheLimit = new() { Id = 316, Name = "\"To the Limit!\"", Profession = Profession.Warrior };
    public static readonly Skill DesperationBlow = new() { Id = 323, Name = "Desperation Blow", Profession = Profession.Warrior };
    public static readonly Skill ThrillofVictory = new() { Id = 324, Name = "Thrill of Victory", Profession = Profession.Warrior };
    public static readonly Skill DefensiveStance = new() { Id = 345, Name = "Defensive Stance", Profession = Profession.Warrior };
    public static readonly Skill WatchYourself = new() { Id = 348, Name = "\"Watch Yourself!\"", Profession = Profession.Warrior };
    public static readonly Skill Charge = new() { Id = 364, Name = "\"Charge!\"", Profession = Profession.Warrior };
    public static readonly Skill VictoryIsMine = new() { Id = 365, Name = "\"Victory Is Mine!\"", Profession = Profession.Warrior };
    public static readonly Skill FearMe = new() { Id = 366, Name = "\"Fear Me!\"", Profession = Profession.Warrior };
    public static readonly Skill ShieldsUp = new() { Id = 367, Name = "\"Shields Up!\"", Profession = Profession.Warrior };
    public static readonly Skill BalancedStance = new() { Id = 371, Name = "Balanced Stance", Profession = Profession.Warrior };
    public static readonly Skill GladiatorsDefense = new() { Id = 372, Name = "Gladiator's Defense", Profession = Profession.Warrior };
    public static readonly Skill DeflectArrows = new() { Id = 373, Name = "Deflect Arrows", Profession = Profession.Warrior };
    public static readonly Skill DisciplinedStance = new() { Id = 376, Name = "Disciplined Stance", Profession = Profession.Warrior };
    public static readonly Skill WaryStance = new() { Id = 377, Name = "Wary Stance", Profession = Profession.Warrior };
    public static readonly Skill ShieldStance = new() { Id = 378, Name = "Shield Stance", Profession = Profession.Warrior };
    public static readonly Skill BonettisDefense = new() { Id = 380, Name = "Bonetti's Defense", Profession = Profession.Warrior };
    public static readonly Skill Riposte = new() { Id = 387, Name = "Riposte", Profession = Profession.Warrior };
    public static readonly Skill DeadlyRiposte = new() { Id = 388, Name = "Deadly Riposte", Profession = Profession.Warrior };
    public static readonly Skill ProtectorsDefense = new() { Id = 810, Name = "Protector's Defense", Profession = Profession.Warrior };
    public static readonly Skill Retreat = new() { Id = 839, Name = "\"Retreat!\"", Profession = Profession.Warrior };
    public static readonly Skill NoneShallPass = new() { Id = 891, Name = "\"None Shall Pass!\"", Profession = Profession.Warrior };
    public static readonly Skill DrunkenBlow = new() { Id = 1133, Name = "Drunken Blow", Profession = Profession.Warrior };
    public static readonly Skill AuspiciousParry = new() { Id = 1142, Name = "Auspicious Parry", Profession = Profession.Warrior };
    public static readonly Skill Shove = new() { Id = 1146, Name = "Shove", Profession = Profession.Warrior };
    public static readonly Skill SoldiersStrike = new() { Id = 1695, Name = "Soldier's Strike", Profession = Profession.Warrior };
    public static readonly Skill SoldiersStance = new() { Id = 1698, Name = "Soldier's Stance", Profession = Profession.Warrior };
    public static readonly Skill SoldiersDefense = new() { Id = 1699, Name = "Soldier's Defense", Profession = Profession.Warrior };
    public static readonly Skill SteadyStance = new() { Id = 1701, Name = "Steady Stance", Profession = Profession.Warrior };
    public static readonly Skill SoldiersSpeed = new() { Id = 2196, Name = "Soldier's Speed", Profession = Profession.Warrior };
    public static readonly Skill WildBlow = new() { Id = 321, Name = "Wild Blow", Profession = Profession.Warrior };
    public static readonly Skill DistractingBlow = new() { Id = 325, Name = "Distracting Blow", Profession = Profession.Warrior };
    public static readonly Skill SkullCrack = new() { Id = 329, Name = "Skull Crack", Profession = Profession.Warrior };
    public static readonly Skill ForGreatJustice = new() { Id = 343, Name = "\"For Great Justice!\"", Profession = Profession.Warrior };
    public static readonly Skill Flurry = new() { Id = 344, Name = "Flurry", Profession = Profession.Warrior };
    public static readonly Skill Frenzy = new() { Id = 346, Name = "Frenzy", Profession = Profession.Warrior };
    public static readonly Skill Coward = new() { Id = 869, Name = "\"Coward!\"", Profession = Profession.Warrior };
    public static readonly Skill OnYourKnees = new() { Id = 906, Name = "\"On Your Knees!\"", Profession = Profession.Warrior };
    public static readonly Skill YoureAllAlone = new() { Id = 1412, Name = "\"You're All Alone!\"", Profession = Profession.Warrior };
    public static readonly Skill FrenziedDefense = new() { Id = 1700, Name = "Frenzied Defense", Profession = Profession.Warrior };
    public static readonly Skill Grapple = new() { Id = 2011, Name = "Grapple", Profession = Profession.Warrior };
    public static readonly Skill DistractingStrike = new() { Id = 2194, Name = "Distracting Strike", Profession = Profession.Warrior };
    public static readonly Skill SymbolicStrike = new() { Id = 2195, Name = "Symbolic Strike", Profession = Profession.Warrior };
    public static readonly Skill CharmAnimal = new() { Id = 411, Name = "Charm Animal", Profession = Profession.Ranger };
    public static readonly Skill CallofProtection = new() { Id = 412, Name = "Call of Protection", Profession = Profession.Ranger };
    public static readonly Skill CallofHaste = new() { Id = 415, Name = "Call of Haste", Profession = Profession.Ranger };
    public static readonly Skill ReviveAnimal = new() { Id = 422, Name = "Revive Animal", Profession = Profession.Ranger };
    public static readonly Skill SymbioticBond = new() { Id = 423, Name = "Symbiotic Bond", Profession = Profession.Ranger };
    public static readonly Skill ComfortAnimal = new() { Id = 436, Name = "Comfort Animal", Profession = Profession.Ranger };
    public static readonly Skill BestialPounce = new() { Id = 437, Name = "Bestial Pounce", Profession = Profession.Ranger };
    public static readonly Skill MaimingStrike = new() { Id = 438, Name = "Maiming Strike", Profession = Profession.Ranger };
    public static readonly Skill FeralLunge = new() { Id = 439, Name = "Feral Lunge", Profession = Profession.Ranger };
    public static readonly Skill ScavengerStrike = new() { Id = 440, Name = "Scavenger Strike", Profession = Profession.Ranger };
    public static readonly Skill MelandrusAssault = new() { Id = 441, Name = "Melandru's Assault", Profession = Profession.Ranger };
    public static readonly Skill FerociousStrike = new() { Id = 442, Name = "Ferocious Strike", Profession = Profession.Ranger };
    public static readonly Skill PredatorsPounce = new() { Id = 443, Name = "Predator's Pounce", Profession = Profession.Ranger };
    public static readonly Skill BrutalStrike = new() { Id = 444, Name = "Brutal Strike", Profession = Profession.Ranger };
    public static readonly Skill DisruptingLunge = new() { Id = 445, Name = "Disrupting Lunge", Profession = Profession.Ranger };
    public static readonly Skill OtyughsCry = new() { Id = 447, Name = "Otyugh's Cry", Profession = Profession.Ranger };
    public static readonly Skill TigersFury = new() { Id = 454, Name = "Tiger's Fury", Profession = Profession.Ranger };
    public static readonly Skill EdgeofExtinction = new() { Id = 464, Name = "Edge of Extinction", Profession = Profession.Ranger };
    public static readonly Skill FertileSeason = new() { Id = 467, Name = "Fertile Season", Profession = Profession.Ranger };
    public static readonly Skill Symbiosis = new() { Id = 468, Name = "Symbiosis", Profession = Profession.Ranger };
    public static readonly Skill PrimalEchoes = new() { Id = 469, Name = "Primal Echoes", Profession = Profession.Ranger };
    public static readonly Skill PredatorySeason = new() { Id = 470, Name = "Predatory Season", Profession = Profession.Ranger };
    public static readonly Skill EnergizingWind = new() { Id = 474, Name = "Energizing Wind", Profession = Profession.Ranger };
    public static readonly Skill RunasOne = new() { Id = 811, Name = "Run as One", Profession = Profession.Ranger };
    public static readonly Skill Lacerate = new() { Id = 961, Name = "Lacerate", Profession = Profession.Ranger };
    public static readonly Skill PredatoryBond = new() { Id = 1194, Name = "Predatory Bond", Profession = Profession.Ranger };
    public static readonly Skill HealasOne = new() { Id = 1195, Name = "Heal as One", Profession = Profession.Ranger };
    public static readonly Skill SavagePounce = new() { Id = 1201, Name = "Savage Pounce", Profession = Profession.Ranger };
    public static readonly Skill EnragedLunge = new() { Id = 1202, Name = "Enraged Lunge", Profession = Profession.Ranger };
    public static readonly Skill BestialMauling = new() { Id = 1203, Name = "Bestial Mauling", Profession = Profession.Ranger };
    public static readonly Skill PoisonousBite = new() { Id = 1205, Name = "Poisonous Bite", Profession = Profession.Ranger };
    public static readonly Skill Pounce = new() { Id = 1206, Name = "Pounce", Profession = Profession.Ranger };
    public static readonly Skill BestialFury = new() { Id = 1209, Name = "Bestial Fury", Profession = Profession.Ranger };
    public static readonly Skill VipersNest = new() { Id = 1211, Name = "Viper's Nest", Profession = Profession.Ranger };
    public static readonly Skill StrikeasOne = new() { Id = 1468, Name = "Strike as One", Profession = Profession.Ranger };
    public static readonly Skill Toxicity = new() { Id = 1472, Name = "Toxicity", Profession = Profession.Ranger };
    public static readonly Skill RampageasOne = new() { Id = 1721, Name = "Rampage as One", Profession = Profession.Ranger };
    public static readonly Skill HeketsRampage = new() { Id = 1728, Name = "Heket's Rampage", Profession = Profession.Ranger };
    public static readonly Skill Companionship = new() { Id = 2141, Name = "Companionship", Profession = Profession.Ranger };
    public static readonly Skill FeralAggression = new() { Id = 2142, Name = "Feral Aggression", Profession = Profession.Ranger };
    public static readonly Skill DistractingShot = new() { Id = 399, Name = "Distracting Shot", Profession = Profession.Ranger };
    public static readonly Skill OathShot = new() { Id = 405, Name = "Oath Shot", Profession = Profession.Ranger };
    public static readonly Skill PointBlankShot = new() { Id = 407, Name = "Point Blank Shot", Profession = Profession.Ranger };
    public static readonly Skill ThrowDirt = new() { Id = 424, Name = "Throw Dirt", Profession = Profession.Ranger };
    public static readonly Skill Dodge = new() { Id = 425, Name = "Dodge", Profession = Profession.Ranger };
    public static readonly Skill MarksmansWager = new() { Id = 430, Name = "Marksman's Wager", Profession = Profession.Ranger };
    public static readonly Skill Escape = new() { Id = 448, Name = "Escape", Profession = Profession.Ranger };
    public static readonly Skill PracticedStance = new() { Id = 449, Name = "Practiced Stance", Profession = Profession.Ranger };
    public static readonly Skill WhirlingDefense = new() { Id = 450, Name = "Whirling Defense", Profession = Profession.Ranger };
    public static readonly Skill LightningReflexes = new() { Id = 453, Name = "Lightning Reflexes", Profession = Profession.Ranger };
    public static readonly Skill TrappersFocus = new() { Id = 946, Name = "Trapper's Focus", Profession = Profession.Ranger };
    public static readonly Skill ZojunsShot = new() { Id = 1192, Name = "Zojun's Shot", Profession = Profession.Ranger };
    public static readonly Skill ZojunsHaste = new() { Id = 1196, Name = "Zojun's Haste", Profession = Profession.Ranger };
    public static readonly Skill GlassArrows = new() { Id = 1199, Name = "Glass Arrows", Profession = Profession.Ranger };
    public static readonly Skill ArchersSignet = new() { Id = 1200, Name = "Archer's Signet", Profession = Profession.Ranger };
    public static readonly Skill TrappersSpeed = new() { Id = 1475, Name = "Trapper's Speed", Profession = Profession.Ranger };
    public static readonly Skill ExpertsDexterity = new() { Id = 1724, Name = "Expert's Dexterity", Profession = Profession.Ranger };
    public static readonly Skill InfuriatingHeat = new() { Id = 1730, Name = "Infuriating Heat", Profession = Profession.Ranger };
    public static readonly Skill ExpertFocus = new() { Id = 2145, Name = "Expert Focus", Profession = Profession.Ranger };
    public static readonly Skill HuntersShot = new() { Id = 391, Name = "Hunter's Shot", Profession = Profession.Ranger };
    public static readonly Skill PinDown = new() { Id = 392, Name = "Pin Down", Profession = Profession.Ranger };
    public static readonly Skill CripplingShot = new() { Id = 393, Name = "Crippling Shot", Profession = Profession.Ranger };
    public static readonly Skill PowerShot = new() { Id = 394, Name = "Power Shot", Profession = Profession.Ranger };
    public static readonly Skill Barrage = new() { Id = 395, Name = "Barrage", Profession = Profession.Ranger };
    public static readonly Skill PenetratingAttack = new() { Id = 398, Name = "Penetrating Attack", Profession = Profession.Ranger };
    public static readonly Skill PrecisionShot = new() { Id = 400, Name = "Precision Shot", Profession = Profession.Ranger };
    public static readonly Skill DeterminedShot = new() { Id = 402, Name = "Determined Shot", Profession = Profession.Ranger };
    public static readonly Skill DebilitatingShot = new() { Id = 406, Name = "Debilitating Shot", Profession = Profession.Ranger };
    public static readonly Skill ConcussionShot = new() { Id = 408, Name = "Concussion Shot", Profession = Profession.Ranger };
    public static readonly Skill PunishingShot = new() { Id = 409, Name = "Punishing Shot", Profession = Profession.Ranger };
    public static readonly Skill SavageShot = new() { Id = 426, Name = "Savage Shot", Profession = Profession.Ranger };
    public static readonly Skill ReadtheWind = new() { Id = 432, Name = "Read the Wind", Profession = Profession.Ranger };
    public static readonly Skill FavorableWinds = new() { Id = 472, Name = "Favorable Winds", Profession = Profession.Ranger };
    public static readonly Skill SplinterShot = new() { Id = 852, Name = "Splinter Shot", Profession = Profession.Ranger };
    public static readonly Skill MelandrusShot = new() { Id = 853, Name = "Melandru's Shot", Profession = Profession.Ranger };
    public static readonly Skill SeekingArrows = new() { Id = 893, Name = "Seeking Arrows", Profession = Profession.Ranger };
    public static readonly Skill MaraudersShot = new() { Id = 908, Name = "Marauder's Shot", Profession = Profession.Ranger };
    public static readonly Skill FocusedShot = new() { Id = 909, Name = "Focused Shot", Profession = Profession.Ranger };
    public static readonly Skill SunderingAttack = new() { Id = 1191, Name = "Sundering Attack", Profession = Profession.Ranger };
    public static readonly Skill NeedlingShot = new() { Id = 1197, Name = "Needling Shot", Profession = Profession.Ranger };
    public static readonly Skill BroadHeadArrow = new() { Id = 1198, Name = "Broad Head Arrow", Profession = Profession.Ranger };
    public static readonly Skill PreparedShot = new() { Id = 1465, Name = "Prepared Shot", Profession = Profession.Ranger };
    public static readonly Skill BurningArrow = new() { Id = 1466, Name = "Burning Arrow", Profession = Profession.Ranger };
    public static readonly Skill ArcingShot = new() { Id = 1467, Name = "Arcing Shot", Profession = Profession.Ranger };
    public static readonly Skill Crossfire = new() { Id = 1469, Name = "Crossfire", Profession = Profession.Ranger };
    public static readonly Skill ScreamingShot = new() { Id = 1719, Name = "Screaming Shot", Profession = Profession.Ranger };
    public static readonly Skill KeenArrow = new() { Id = 1720, Name = "Keen Arrow", Profession = Profession.Ranger };
    public static readonly Skill DisruptingAccuracy = new() { Id = 1723, Name = "Disrupting Accuracy", Profession = Profession.Ranger };
    public static readonly Skill RapidFire = new() { Id = 2068, Name = "Rapid Fire", Profession = Profession.Ranger };
    public static readonly Skill SlothHuntersShot = new() { Id = 2069, Name = "Sloth Hunter's Shot", Profession = Profession.Ranger };
    public static readonly Skill DisruptingShot = new() { Id = 2143, Name = "Disrupting Shot", Profession = Profession.Ranger };
    public static readonly Skill Volley = new() { Id = 2144, Name = "Volley", Profession = Profession.Ranger };
    public static readonly Skill BodyShot = new() { Id = 2198, Name = "Body Shot", Profession = Profession.Ranger };
    public static readonly Skill PoisonArrow = new() { Id = 404, Name = "Poison Arrow", Profession = Profession.Ranger };
    public static readonly Skill IncendiaryArrows = new() { Id = 428, Name = "Incendiary Arrows", Profession = Profession.Ranger };
    public static readonly Skill MelandrusArrows = new() { Id = 429, Name = "Melandru's Arrows", Profession = Profession.Ranger };
    public static readonly Skill IgniteArrows = new() { Id = 431, Name = "Ignite Arrows", Profession = Profession.Ranger };
    public static readonly Skill KindleArrows = new() { Id = 433, Name = "Kindle Arrows", Profession = Profession.Ranger };
    public static readonly Skill ChokingGas = new() { Id = 434, Name = "Choking Gas", Profession = Profession.Ranger };
    public static readonly Skill ApplyPoison = new() { Id = 435, Name = "Apply Poison", Profession = Profession.Ranger };
    public static readonly Skill TrollUnguent = new() { Id = 446, Name = "Troll Unguent", Profession = Profession.Ranger };
    public static readonly Skill MelandrusResilience = new() { Id = 451, Name = "Melandru's Resilience", Profession = Profession.Ranger };
    public static readonly Skill DrydersDefenses = new() { Id = 452, Name = "Dryder's Defenses", Profession = Profession.Ranger };
    public static readonly Skill StormChaser = new() { Id = 455, Name = "Storm Chaser", Profession = Profession.Ranger };
    public static readonly Skill SerpentsQuickness = new() { Id = 456, Name = "Serpent's Quickness", Profession = Profession.Ranger };
    public static readonly Skill DustTrap = new() { Id = 457, Name = "Dust Trap", Profession = Profession.Ranger };
    public static readonly Skill BarbedTrap = new() { Id = 458, Name = "Barbed Trap", Profession = Profession.Ranger };
    public static readonly Skill FlameTrap = new() { Id = 459, Name = "Flame Trap", Profession = Profession.Ranger };
    public static readonly Skill HealingSpring = new() { Id = 460, Name = "Healing Spring", Profession = Profession.Ranger };
    public static readonly Skill SpikeTrap = new() { Id = 461, Name = "Spike Trap", Profession = Profession.Ranger };
    public static readonly Skill Winter = new() { Id = 462, Name = "Winter", Profession = Profession.Ranger };
    public static readonly Skill Winnowing = new() { Id = 463, Name = "Winnowing", Profession = Profession.Ranger };
    public static readonly Skill GreaterConflagration = new() { Id = 465, Name = "Greater Conflagration", Profession = Profession.Ranger };
    public static readonly Skill Conflagration = new() { Id = 466, Name = "Conflagration", Profession = Profession.Ranger };
    public static readonly Skill FrozenSoil = new() { Id = 471, Name = "Frozen Soil", Profession = Profession.Ranger };
    public static readonly Skill QuickeningZephyr = new() { Id = 475, Name = "Quickening Zephyr", Profession = Profession.Ranger };
    public static readonly Skill NaturesRenewal = new() { Id = 476, Name = "Nature's Renewal", Profession = Profession.Ranger };
    public static readonly Skill MuddyTerrain = new() { Id = 477, Name = "Muddy Terrain", Profession = Profession.Ranger };
    public static readonly Skill Snare = new() { Id = 854, Name = "Snare", Profession = Profession.Ranger };
    public static readonly Skill Pestilence = new() { Id = 870, Name = "Pestilence", Profession = Profession.Ranger };
    public static readonly Skill Brambles = new() { Id = 947, Name = "Brambles", Profession = Profession.Ranger };
    public static readonly Skill Famine = new() { Id = 997, Name = "Famine", Profession = Profession.Ranger };
    public static readonly Skill Equinox = new() { Id = 1212, Name = "Equinox", Profession = Profession.Ranger };
    public static readonly Skill Tranquility = new() { Id = 1213, Name = "Tranquility", Profession = Profession.Ranger };
    public static readonly Skill BarbedArrows = new() { Id = 1470, Name = "Barbed Arrows", Profession = Profession.Ranger };
    public static readonly Skill ScavengersFocus = new() { Id = 1471, Name = "Scavenger's Focus", Profession = Profession.Ranger };
    public static readonly Skill Quicksand = new() { Id = 1473, Name = "Quicksand", Profession = Profession.Ranger };
    public static readonly Skill Tripwire = new() { Id = 1476, Name = "Tripwire", Profession = Profession.Ranger };
    public static readonly Skill RoaringWinds = new() { Id = 1725, Name = "Roaring Winds", Profession = Profession.Ranger };
    public static readonly Skill NaturalStride = new() { Id = 1727, Name = "Natural Stride", Profession = Profession.Ranger };
    public static readonly Skill SmokeTrap = new() { Id = 1729, Name = "Smoke Trap", Profession = Profession.Ranger };
    public static readonly Skill PiercingTrap = new() { Id = 2140, Name = "Piercing Trap", Profession = Profession.Ranger };
    public static readonly Skill PoisonTipSignet = new() { Id = 2199, Name = "Poison Tip Signet", Profession = Profession.Ranger };
    public static readonly Skill DualShot = new() { Id = 396, Name = "Dual Shot", Profession = Profession.Ranger };
    public static readonly Skill QuickShot = new() { Id = 397, Name = "Quick Shot", Profession = Profession.Ranger };
    public static readonly Skill CalledShot = new() { Id = 403, Name = "Called Shot", Profession = Profession.Ranger };
    public static readonly Skill AntidoteSignet = new() { Id = 427, Name = "Antidote Signet", Profession = Profession.Ranger };
    public static readonly Skill StormsEmbrace = new() { Id = 1474, Name = "Storm's Embrace", Profession = Profession.Ranger };
    public static readonly Skill ForkedArrow = new() { Id = 1722, Name = "Forked Arrow", Profession = Profession.Ranger };
    public static readonly Skill MagebaneShot = new() { Id = 1726, Name = "Magebane Shot", Profession = Profession.Ranger };
    public static readonly Skill DivineIntervention = new() { Id = 246, Name = "Divine Intervention", Profession = Profession.Monk };
    public static readonly Skill WatchfulSpirit = new() { Id = 255, Name = "Watchful Spirit", Profession = Profession.Monk };
    public static readonly Skill BlessedAura = new() { Id = 256, Name = "Blessed Aura", Profession = Profession.Monk };
    public static readonly Skill PeaceandHarmony = new() { Id = 266, Name = "Peace and Harmony", Profession = Profession.Monk };
    public static readonly Skill UnyieldingAura = new() { Id = 268, Name = "Unyielding Aura", Profession = Profession.Monk };
    public static readonly Skill SpellBreaker = new() { Id = 273, Name = "Spell Breaker", Profession = Profession.Monk };
    public static readonly Skill DivineHealing = new() { Id = 279, Name = "Divine Healing", Profession = Profession.Monk };
    public static readonly Skill DivineBoon = new() { Id = 284, Name = "Divine Boon", Profession = Profession.Monk };
    public static readonly Skill SignetofDevotion = new() { Id = 293, Name = "Signet of Devotion", Profession = Profession.Monk };
    public static readonly Skill BlessedSignet = new() { Id = 297, Name = "Blessed Signet", Profession = Profession.Monk };
    public static readonly Skill ContemplationofPurity = new() { Id = 300, Name = "Contemplation of Purity", Profession = Profession.Monk };
    public static readonly Skill DivineSpirit = new() { Id = 310, Name = "Divine Spirit", Profession = Profession.Monk };
    public static readonly Skill BoonSignet = new() { Id = 847, Name = "Boon Signet", Profession = Profession.Monk };
    public static readonly Skill BlessedLight = new() { Id = 941, Name = "Blessed Light", Profession = Profession.Monk };
    public static readonly Skill WithdrawHexes = new() { Id = 942, Name = "Withdraw Hexes", Profession = Profession.Monk };
    public static readonly Skill SpellShield = new() { Id = 957, Name = "Spell Shield", Profession = Profession.Monk };
    public static readonly Skill ReleaseEnchantments = new() { Id = 960, Name = "Release Enchantments", Profession = Profession.Monk };
    public static readonly Skill DenyHexes = new() { Id = 991, Name = "Deny Hexes", Profession = Profession.Monk };
    public static readonly Skill HeavensDelight = new() { Id = 1117, Name = "Heaven's Delight", Profession = Profession.Monk };
    public static readonly Skill WatchfulHealing = new() { Id = 1392, Name = "Watchful Healing", Profession = Profession.Monk };
    public static readonly Skill HealersBoon = new() { Id = 1393, Name = "Healer's Boon", Profession = Profession.Monk };
    public static readonly Skill ScribesInsight = new() { Id = 1684, Name = "Scribe's Insight", Profession = Profession.Monk };
    public static readonly Skill HolyHaste = new() { Id = 1685, Name = "Holy Haste", Profession = Profession.Monk };
    public static readonly Skill SmitersBoon = new() { Id = 2005, Name = "Smiter's Boon", Profession = Profession.Monk };
    public static readonly Skill VigorousSpirit = new() { Id = 254, Name = "Vigorous Spirit", Profession = Profession.Monk };
    public static readonly Skill HealingSeed = new() { Id = 274, Name = "Healing Seed", Profession = Profession.Monk };
    public static readonly Skill HealArea = new() { Id = 280, Name = "Heal Area", Profession = Profession.Monk };
    public static readonly Skill OrisonofHealing = new() { Id = 281, Name = "Orison of Healing", Profession = Profession.Monk };
    public static readonly Skill WordofHealing = new() { Id = 282, Name = "Word of Healing", Profession = Profession.Monk };
    public static readonly Skill DwaynasKiss = new() { Id = 283, Name = "Dwayna's Kiss", Profession = Profession.Monk };
    public static readonly Skill HealingHands = new() { Id = 285, Name = "Healing Hands", Profession = Profession.Monk };
    public static readonly Skill HealOther = new() { Id = 286, Name = "Heal Other", Profession = Profession.Monk };
    public static readonly Skill HealParty = new() { Id = 287, Name = "Heal Party", Profession = Profession.Monk };
    public static readonly Skill HealingBreeze = new() { Id = 288, Name = "Healing Breeze", Profession = Profession.Monk };
    public static readonly Skill Mending = new() { Id = 290, Name = "Mending", Profession = Profession.Monk };
    public static readonly Skill LiveVicariously = new() { Id = 291, Name = "Live Vicariously", Profession = Profession.Monk };
    public static readonly Skill InfuseHealth = new() { Id = 292, Name = "Infuse Health", Profession = Profession.Monk };
    public static readonly Skill HealingTouch = new() { Id = 313, Name = "Healing Touch", Profession = Profession.Monk };
    public static readonly Skill RestoreLife = new() { Id = 314, Name = "Restore Life", Profession = Profession.Monk };
    public static readonly Skill DwaynasSorrow = new() { Id = 838, Name = "Dwayna's Sorrow", Profession = Profession.Monk };
    public static readonly Skill HealingLight = new() { Id = 867, Name = "Healing Light", Profession = Profession.Monk };
    public static readonly Skill RestfulBreeze = new() { Id = 886, Name = "Restful Breeze", Profession = Profession.Monk };
    public static readonly Skill SignetofRejuvenation = new() { Id = 887, Name = "Signet of Rejuvenation", Profession = Profession.Monk };
    public static readonly Skill HealingWhisper = new() { Id = 958, Name = "Healing Whisper", Profession = Profession.Monk };
    public static readonly Skill EtherealLight = new() { Id = 959, Name = "Ethereal Light", Profession = Profession.Monk };
    public static readonly Skill HealingBurst = new() { Id = 1118, Name = "Healing Burst", Profession = Profession.Monk };
    public static readonly Skill KareisHealingCircle = new() { Id = 1119, Name = "Karei's Healing Circle", Profession = Profession.Monk };
    public static readonly Skill JameisGaze = new() { Id = 1120, Name = "Jamei's Gaze", Profession = Profession.Monk };
    public static readonly Skill GiftofHealth = new() { Id = 1121, Name = "Gift of Health", Profession = Profession.Monk };
    public static readonly Skill ResurrectionChant = new() { Id = 1128, Name = "Resurrection Chant", Profession = Profession.Monk };
    public static readonly Skill HealingRing = new() { Id = 1262, Name = "Healing Ring", Profession = Profession.Monk };
    public static readonly Skill RenewLife = new() { Id = 1263, Name = "Renew Life", Profession = Profession.Monk };
    public static readonly Skill SupportiveSpirit = new() { Id = 1391, Name = "Supportive Spirit", Profession = Profession.Monk };
    public static readonly Skill HealersCovenant = new() { Id = 1394, Name = "Healer's Covenant", Profession = Profession.Monk };
    public static readonly Skill WordsofComfort = new() { Id = 1396, Name = "Words of Comfort", Profession = Profession.Monk };
    public static readonly Skill LightofDeliverance = new() { Id = 1397, Name = "Light of Deliverance", Profession = Profession.Monk };
    public static readonly Skill GlimmerofLight = new() { Id = 1686, Name = "Glimmer of Light", Profession = Profession.Monk };
    public static readonly Skill CureHex = new() { Id = 2003, Name = "Cure Hex", Profession = Profession.Monk };
    public static readonly Skill PatientSpirit = new() { Id = 2061, Name = "Patient Spirit", Profession = Profession.Monk };
    public static readonly Skill HealingRibbon = new() { Id = 2062, Name = "Healing Ribbon", Profession = Profession.Monk };
    public static readonly Skill SpotlessMind = new() { Id = 2064, Name = "Spotless Mind", Profession = Profession.Monk };
    public static readonly Skill SpotlessSoul = new() { Id = 2065, Name = "Spotless Soul", Profession = Profession.Monk };
    public static readonly Skill LifeBond = new() { Id = 241, Name = "Life Bond", Profession = Profession.Monk };
    public static readonly Skill LifeAttunement = new() { Id = 244, Name = "Life Attunement", Profession = Profession.Monk };
    public static readonly Skill ProtectiveSpirit = new() { Id = 245, Name = "Protective Spirit", Profession = Profession.Monk };
    public static readonly Skill Aegis = new() { Id = 257, Name = "Aegis", Profession = Profession.Monk };
    public static readonly Skill Guardian = new() { Id = 258, Name = "Guardian", Profession = Profession.Monk };
    public static readonly Skill ShieldofDeflection = new() { Id = 259, Name = "Shield of Deflection", Profession = Profession.Monk };
    public static readonly Skill AuraofFaith = new() { Id = 260, Name = "Aura of Faith", Profession = Profession.Monk };
    public static readonly Skill ShieldofRegeneration = new() { Id = 261, Name = "Shield of Regeneration", Profession = Profession.Monk };
    public static readonly Skill ProtectiveBond = new() { Id = 263, Name = "Protective Bond", Profession = Profession.Monk };
    public static readonly Skill Pacifism = new() { Id = 264, Name = "Pacifism", Profession = Profession.Monk };
    public static readonly Skill Amity = new() { Id = 265, Name = "Amity", Profession = Profession.Monk };
    public static readonly Skill MarkofProtection = new() { Id = 269, Name = "Mark of Protection", Profession = Profession.Monk };
    public static readonly Skill LifeBarrier = new() { Id = 270, Name = "Life Barrier", Profession = Profession.Monk };
    public static readonly Skill MendCondition = new() { Id = 275, Name = "Mend Condition", Profession = Profession.Monk };
    public static readonly Skill RestoreCondition = new() { Id = 276, Name = "Restore Condition", Profession = Profession.Monk };
    public static readonly Skill MendAilment = new() { Id = 277, Name = "Mend Ailment", Profession = Profession.Monk };
    public static readonly Skill VitalBlessing = new() { Id = 289, Name = "Vital Blessing", Profession = Profession.Monk };
    public static readonly Skill ShieldingHands = new() { Id = 299, Name = "Shielding Hands", Profession = Profession.Monk };
    public static readonly Skill ConvertHexes = new() { Id = 303, Name = "Convert Hexes", Profession = Profession.Monk };
    public static readonly Skill Rebirth = new() { Id = 306, Name = "Rebirth", Profession = Profession.Monk };
    public static readonly Skill ReversalofFortune = new() { Id = 307, Name = "Reversal of Fortune", Profession = Profession.Monk };
    public static readonly Skill DrawConditions = new() { Id = 311, Name = "Draw Conditions", Profession = Profession.Monk };
    public static readonly Skill ReverseHex = new() { Id = 848, Name = "Reverse Hex", Profession = Profession.Monk };
    public static readonly Skill ShieldGuardian = new() { Id = 885, Name = "Shield Guardian", Profession = Profession.Monk };
    public static readonly Skill Extinguish = new() { Id = 943, Name = "Extinguish", Profession = Profession.Monk };
    public static readonly Skill SpiritBond = new() { Id = 1114, Name = "Spirit Bond", Profession = Profession.Monk };
    public static readonly Skill AirofEnchantment = new() { Id = 1115, Name = "Air of Enchantment", Profession = Profession.Monk };
    public static readonly Skill LifeSheath = new() { Id = 1123, Name = "Life Sheath", Profession = Profession.Monk };
    public static readonly Skill ShieldofAbsorption = new() { Id = 1399, Name = "Shield of Absorption", Profession = Profession.Monk };
    public static readonly Skill MendingTouch = new() { Id = 1401, Name = "Mending Touch", Profession = Profession.Monk };
    public static readonly Skill PensiveGuardian = new() { Id = 1683, Name = "Pensive Guardian", Profession = Profession.Monk };
    public static readonly Skill ZealousBenediction = new() { Id = 1687, Name = "Zealous Benediction", Profession = Profession.Monk };
    public static readonly Skill DismissCondition = new() { Id = 1691, Name = "Dismiss Condition", Profession = Profession.Monk };
    public static readonly Skill DivertHexes = new() { Id = 1692, Name = "Divert Hexes", Profession = Profession.Monk };
    public static readonly Skill PurifyingVeil = new() { Id = 2007, Name = "Purifying Veil", Profession = Profession.Monk };
    public static readonly Skill AuraofStability = new() { Id = 2063, Name = "Aura of Stability", Profession = Profession.Monk };
    public static readonly Skill Smite = new() { Id = 240, Name = "Smite", Profession = Profession.Monk };
    public static readonly Skill BalthazarsSpirit = new() { Id = 242, Name = "Balthazar's Spirit", Profession = Profession.Monk };
    public static readonly Skill StrengthofHonor = new() { Id = 243, Name = "Strength of Honor", Profession = Profession.Monk };
    public static readonly Skill SymbolofWrath = new() { Id = 247, Name = "Symbol of Wrath", Profession = Profession.Monk };
    public static readonly Skill Retribution = new() { Id = 248, Name = "Retribution", Profession = Profession.Monk };
    public static readonly Skill HolyWrath = new() { Id = 249, Name = "Holy Wrath", Profession = Profession.Monk };
    public static readonly Skill ScourgeHealing = new() { Id = 251, Name = "Scourge Healing", Profession = Profession.Monk };
    public static readonly Skill Banish = new() { Id = 252, Name = "Banish", Profession = Profession.Monk };
    public static readonly Skill ScourgeSacrifice = new() { Id = 253, Name = "Scourge Sacrifice", Profession = Profession.Monk };
    public static readonly Skill ShieldofJudgment = new() { Id = 262, Name = "Shield of Judgment", Profession = Profession.Monk };
    public static readonly Skill JudgesInsight = new() { Id = 267, Name = "Judge's Insight", Profession = Profession.Monk };
    public static readonly Skill ZealotsFire = new() { Id = 271, Name = "Zealot's Fire", Profession = Profession.Monk };
    public static readonly Skill BalthazarsAura = new() { Id = 272, Name = "Balthazar's Aura", Profession = Profession.Monk };
    public static readonly Skill SignetofJudgment = new() { Id = 294, Name = "Signet of Judgment", Profession = Profession.Monk };
    public static readonly Skill BaneSignet = new() { Id = 296, Name = "Bane Signet", Profession = Profession.Monk };
    public static readonly Skill SmiteHex = new() { Id = 302, Name = "Smite Hex", Profession = Profession.Monk };
    public static readonly Skill HolyStrike = new() { Id = 312, Name = "Holy Strike", Profession = Profession.Monk };
    public static readonly Skill RayofJudgment = new() { Id = 830, Name = "Ray of Judgment", Profession = Profession.Monk };
    public static readonly Skill KirinsWrath = new() { Id = 1113, Name = "Kirin's Wrath", Profession = Profession.Monk };
    public static readonly Skill WordofCensure = new() { Id = 1129, Name = "Word of Censure", Profession = Profession.Monk };
    public static readonly Skill SpearofLight = new() { Id = 1130, Name = "Spear of Light", Profession = Profession.Monk };
    public static readonly Skill StonesoulStrike = new() { Id = 1131, Name = "Stonesoul Strike", Profession = Profession.Monk };
    public static readonly Skill SignetofRage = new() { Id = 1269, Name = "Signet of Rage", Profession = Profession.Monk };
    public static readonly Skill JudgesIntervention = new() { Id = 1390, Name = "Judge's Intervention", Profession = Profession.Monk };
    public static readonly Skill BalthazarsPendulum = new() { Id = 1395, Name = "Balthazar's Pendulum", Profession = Profession.Monk };
    public static readonly Skill ScourgeEnchantment = new() { Id = 1398, Name = "Scourge Enchantment", Profession = Profession.Monk };
    public static readonly Skill ReversalofDamage = new() { Id = 1400, Name = "Reversal of Damage", Profession = Profession.Monk };
    public static readonly Skill DefendersZeal = new() { Id = 1688, Name = "Defender's Zeal", Profession = Profession.Monk };
    public static readonly Skill SignetofMysticWrath = new() { Id = 1689, Name = "Signet of Mystic Wrath", Profession = Profession.Monk };
    public static readonly Skill SmiteCondition = new() { Id = 2004, Name = "Smite Condition", Profession = Profession.Monk };
    public static readonly Skill CastigationSignet = new() { Id = 2006, Name = "Castigation Signet", Profession = Profession.Monk };
    public static readonly Skill EssenceBond = new() { Id = 250, Name = "Essence Bond", Profession = Profession.Monk };
    public static readonly Skill PurgeConditions = new() { Id = 278, Name = "Purge Conditions", Profession = Profession.Monk };
    public static readonly Skill PurgeSignet = new() { Id = 295, Name = "Purge Signet", Profession = Profession.Monk };
    public static readonly Skill Martyr = new() { Id = 298, Name = "Martyr", Profession = Profession.Monk };
    public static readonly Skill RemoveHex = new() { Id = 301, Name = "Remove Hex", Profession = Profession.Monk };
    public static readonly Skill LightofDwayna = new() { Id = 304, Name = "Light of Dwayna", Profession = Profession.Monk };
    public static readonly Skill Resurrect = new() { Id = 305, Name = "Resurrect", Profession = Profession.Monk };
    public static readonly Skill Succor = new() { Id = 308, Name = "Succor", Profession = Profession.Monk };
    public static readonly Skill HolyVeil = new() { Id = 309, Name = "Holy Veil", Profession = Profession.Monk };
    public static readonly Skill Vengeance = new() { Id = 315, Name = "Vengeance", Profession = Profession.Monk };
    public static readonly Skill EmpathicRemoval = new() { Id = 1126, Name = "Empathic Removal", Profession = Profession.Monk };
    public static readonly Skill SignetofRemoval = new() { Id = 1690, Name = "Signet of Removal", Profession = Profession.Monk };
    public static readonly Skill WellofPower = new() { Id = 91, Name = "Well of Power", Profession = Profession.Necromancer };
    public static readonly Skill WellofBlood = new() { Id = 92, Name = "Well of Blood", Profession = Profession.Necromancer };
    public static readonly Skill ShadowStrike = new() { Id = 102, Name = "Shadow Strike", Profession = Profession.Necromancer };
    public static readonly Skill LifeSiphon = new() { Id = 109, Name = "Life Siphon", Profession = Profession.Necromancer };
    public static readonly Skill UnholyFeast = new() { Id = 110, Name = "Unholy Feast", Profession = Profession.Necromancer };
    public static readonly Skill AwakentheBlood = new() { Id = 111, Name = "Awaken the Blood", Profession = Profession.Necromancer };
    public static readonly Skill BloodRenewal = new() { Id = 115, Name = "Blood Renewal", Profession = Profession.Necromancer };
    public static readonly Skill BloodisPower = new() { Id = 119, Name = "Blood is Power", Profession = Profession.Necromancer };
    public static readonly Skill LifeTransfer = new() { Id = 126, Name = "Life Transfer", Profession = Profession.Necromancer };
    public static readonly Skill MarkofSubversion = new() { Id = 127, Name = "Mark of Subversion", Profession = Profession.Necromancer };
    public static readonly Skill SoulLeech = new() { Id = 128, Name = "Soul Leech", Profession = Profession.Necromancer };
    public static readonly Skill DemonicFlesh = new() { Id = 130, Name = "Demonic Flesh", Profession = Profession.Necromancer };
    public static readonly Skill BarbedSignet = new() { Id = 131, Name = "Barbed Signet", Profession = Profession.Necromancer };
    public static readonly Skill DarkPact = new() { Id = 133, Name = "Dark Pact", Profession = Profession.Necromancer };
    public static readonly Skill OrderofPain = new() { Id = 134, Name = "Order of Pain", Profession = Profession.Necromancer };
    public static readonly Skill DarkBond = new() { Id = 138, Name = "Dark Bond", Profession = Profession.Necromancer };
    public static readonly Skill StripEnchantment = new() { Id = 143, Name = "Strip Enchantment", Profession = Profession.Necromancer };
    public static readonly Skill SignetofAgony = new() { Id = 145, Name = "Signet of Agony", Profession = Profession.Necromancer };
    public static readonly Skill OfferingofBlood = new() { Id = 146, Name = "Offering of Blood", Profession = Profession.Necromancer };
    public static readonly Skill DarkFury = new() { Id = 147, Name = "Dark Fury", Profession = Profession.Necromancer };
    public static readonly Skill OrderoftheVampire = new() { Id = 148, Name = "Order of the Vampire", Profession = Profession.Necromancer };
    public static readonly Skill VampiricGaze = new() { Id = 153, Name = "Vampiric Gaze", Profession = Profession.Necromancer };
    public static readonly Skill VampiricTouch = new() { Id = 156, Name = "Vampiric Touch", Profession = Profession.Necromancer };
    public static readonly Skill BloodRitual = new() { Id = 157, Name = "Blood Ritual", Profession = Profession.Necromancer };
    public static readonly Skill TouchofAgony = new() { Id = 158, Name = "Touch of Agony", Profession = Profession.Necromancer };
    public static readonly Skill JaundicedGaze = new() { Id = 763, Name = "Jaundiced Gaze", Profession = Profession.Necromancer };
    public static readonly Skill CultistsFervor = new() { Id = 806, Name = "Cultist's Fervor", Profession = Profession.Necromancer };
    public static readonly Skill VampiricSpirit = new() { Id = 819, Name = "Vampiric Spirit", Profession = Profession.Necromancer };
    public static readonly Skill BloodBond = new() { Id = 835, Name = "Blood Bond", Profession = Profession.Necromancer };
    public static readonly Skill RavenousGaze = new() { Id = 862, Name = "Ravenous Gaze", Profession = Profession.Necromancer };
    public static readonly Skill OppressiveGaze = new() { Id = 864, Name = "Oppressive Gaze", Profession = Profession.Necromancer };
    public static readonly Skill BloodoftheAggressor = new() { Id = 902, Name = "Blood of the Aggressor", Profession = Profession.Necromancer };
    public static readonly Skill SpoilVictor = new() { Id = 1066, Name = "Spoil Victor", Profession = Profession.Necromancer };
    public static readonly Skill LifebaneStrike = new() { Id = 1067, Name = "Lifebane Strike", Profession = Profession.Necromancer };
    public static readonly Skill VampiricSwarm = new() { Id = 1075, Name = "Vampiric Swarm", Profession = Profession.Necromancer };
    public static readonly Skill BloodDrinker = new() { Id = 1076, Name = "Blood Drinker", Profession = Profession.Necromancer };
    public static readonly Skill VampiricBite = new() { Id = 1077, Name = "Vampiric Bite", Profession = Profession.Necromancer };
    public static readonly Skill WallowsBite = new() { Id = 1078, Name = "Wallow's Bite", Profession = Profession.Necromancer };
    public static readonly Skill MarkofFury = new() { Id = 1360, Name = "Mark of Fury", Profession = Profession.Necromancer };
    public static readonly Skill SignetofSuffering = new() { Id = 1364, Name = "Signet of Suffering", Profession = Profession.Necromancer };
    public static readonly Skill ParasiticBond = new() { Id = 99, Name = "Parasitic Bond", Profession = Profession.Necromancer };
    public static readonly Skill SoulBarbs = new() { Id = 100, Name = "Soul Barbs", Profession = Profession.Necromancer };
    public static readonly Skill Barbs = new() { Id = 101, Name = "Barbs", Profession = Profession.Necromancer };
    public static readonly Skill PriceofFailure = new() { Id = 103, Name = "Price of Failure", Profession = Profession.Necromancer };
    public static readonly Skill Suffering = new() { Id = 108, Name = "Suffering", Profession = Profession.Necromancer };
    public static readonly Skill DesecrateEnchantments = new() { Id = 112, Name = "Desecrate Enchantments", Profession = Profession.Necromancer };
    public static readonly Skill Enfeeble = new() { Id = 117, Name = "Enfeeble", Profession = Profession.Necromancer };
    public static readonly Skill EnfeeblingBlood = new() { Id = 118, Name = "Enfeebling Blood", Profession = Profession.Necromancer };
    public static readonly Skill SpitefulSpirit = new() { Id = 121, Name = "Spiteful Spirit", Profession = Profession.Necromancer };
    public static readonly Skill InsidiousParasite = new() { Id = 123, Name = "Insidious Parasite", Profession = Profession.Necromancer };
    public static readonly Skill SpinalShivers = new() { Id = 124, Name = "Spinal Shivers", Profession = Profession.Necromancer };
    public static readonly Skill Wither = new() { Id = 125, Name = "Wither", Profession = Profession.Necromancer };
    public static readonly Skill DefileFlesh = new() { Id = 129, Name = "Defile Flesh", Profession = Profession.Necromancer };
    public static readonly Skill PlagueSignet = new() { Id = 132, Name = "Plague Signet", Profession = Profession.Necromancer };
    public static readonly Skill Faintheartedness = new() { Id = 135, Name = "Faintheartedness", Profession = Profession.Necromancer };
    public static readonly Skill ShadowofFear = new() { Id = 136, Name = "Shadow of Fear", Profession = Profession.Necromancer };
    public static readonly Skill RigorMortis = new() { Id = 137, Name = "Rigor Mortis", Profession = Profession.Necromancer };
    public static readonly Skill Malaise = new() { Id = 140, Name = "Malaise", Profession = Profession.Necromancer };
    public static readonly Skill RendEnchantments = new() { Id = 141, Name = "Rend Enchantments", Profession = Profession.Necromancer };
    public static readonly Skill LingeringCurse = new() { Id = 142, Name = "Lingering Curse", Profession = Profession.Necromancer };
    public static readonly Skill Chilblains = new() { Id = 144, Name = "Chilblains", Profession = Profession.Necromancer };
    public static readonly Skill PlagueSending = new() { Id = 149, Name = "Plague Sending", Profession = Profession.Necromancer };
    public static readonly Skill MarkofPain = new() { Id = 150, Name = "Mark of Pain", Profession = Profession.Necromancer };
    public static readonly Skill FeastofCorruption = new() { Id = 151, Name = "Feast of Corruption", Profession = Profession.Necromancer };
    public static readonly Skill PlagueTouch = new() { Id = 154, Name = "Plague Touch", Profession = Profession.Necromancer };
    public static readonly Skill WeakenArmor = new() { Id = 159, Name = "Weaken Armor", Profession = Profession.Necromancer };
    public static readonly Skill WellofWeariness = new() { Id = 818, Name = "Well of Weariness", Profession = Profession.Necromancer };
    public static readonly Skill Depravity = new() { Id = 820, Name = "Depravity", Profession = Profession.Necromancer };
    public static readonly Skill WeakenKnees = new() { Id = 822, Name = "Weaken Knees", Profession = Profession.Necromancer };
    public static readonly Skill RecklessHaste = new() { Id = 834, Name = "Reckless Haste", Profession = Profession.Necromancer };
    public static readonly Skill PoisonedHeart = new() { Id = 840, Name = "Poisoned Heart", Profession = Profession.Necromancer };
    public static readonly Skill OrderofApostasy = new() { Id = 863, Name = "Order of Apostasy", Profession = Profession.Necromancer };
    public static readonly Skill VocalMinority = new() { Id = 883, Name = "Vocal Minority", Profession = Profession.Necromancer };
    public static readonly Skill SoulBind = new() { Id = 901, Name = "Soul Bind", Profession = Profession.Necromancer };
    public static readonly Skill EnvenomEnchantments = new() { Id = 936, Name = "Envenom Enchantments", Profession = Profession.Necromancer };
    public static readonly Skill RipEnchantment = new() { Id = 955, Name = "Rip Enchantment", Profession = Profession.Necromancer };
    public static readonly Skill DefileEnchantments = new() { Id = 1070, Name = "Defile Enchantments", Profession = Profession.Necromancer };
    public static readonly Skill ShiversofDread = new() { Id = 1071, Name = "Shivers of Dread", Profession = Profession.Necromancer };
    public static readonly Skill EnfeeblingTouch = new() { Id = 1079, Name = "Enfeebling Touch", Profession = Profession.Necromancer };
    public static readonly Skill Meekness = new() { Id = 1260, Name = "Meekness", Profession = Profession.Necromancer };
    public static readonly Skill UlcerousLungs = new() { Id = 1358, Name = "Ulcerous Lungs", Profession = Profession.Necromancer };
    public static readonly Skill PainofDisenchantment = new() { Id = 1359, Name = "Pain of Disenchantment", Profession = Profession.Necromancer };
    public static readonly Skill CorruptEnchantment = new() { Id = 1362, Name = "Corrupt Enchantment", Profession = Profession.Necromancer };
    public static readonly Skill WellofDarkness = new() { Id = 1366, Name = "Well of Darkness", Profession = Profession.Necromancer };
    public static readonly Skill WellofSilence = new() { Id = 1660, Name = "Well of Silence", Profession = Profession.Necromancer };
    public static readonly Skill Cacophony = new() { Id = 1998, Name = "Cacophony", Profession = Profession.Necromancer };
    public static readonly Skill DefileDefenses = new() { Id = 2188, Name = "Defile Defenses", Profession = Profession.Necromancer };
    public static readonly Skill WellofRuin = new() { Id = 2236, Name = "Well of Ruin", Profession = Profession.Necromancer };
    public static readonly Skill Atrophy = new() { Id = 2237, Name = "Atrophy", Profession = Profession.Necromancer };
    public static readonly Skill AnimateBoneHorror = new() { Id = 83, Name = "Animate Bone Horror", Profession = Profession.Necromancer };
    public static readonly Skill AnimateBoneFiend = new() { Id = 84, Name = "Animate Bone Fiend", Profession = Profession.Necromancer };
    public static readonly Skill AnimateBoneMinions = new() { Id = 85, Name = "Animate Bone Minions", Profession = Profession.Necromancer };
    public static readonly Skill VeratasGaze = new() { Id = 87, Name = "Verata's Gaze", Profession = Profession.Necromancer };
    public static readonly Skill VeratasAura = new() { Id = 88, Name = "Verata's Aura", Profession = Profession.Necromancer };
    public static readonly Skill DeathlyChill = new() { Id = 89, Name = "Deathly Chill", Profession = Profession.Necromancer };
    public static readonly Skill VeratasSacrifice = new() { Id = 90, Name = "Verata's Sacrifice", Profession = Profession.Necromancer };
    public static readonly Skill WellofSuffering = new() { Id = 93, Name = "Well of Suffering", Profession = Profession.Necromancer };
    public static readonly Skill WelloftheProfane = new() { Id = 94, Name = "Well of the Profane", Profession = Profession.Necromancer };
    public static readonly Skill PutridExplosion = new() { Id = 95, Name = "Putrid Explosion", Profession = Profession.Necromancer };
    public static readonly Skill SoulFeast = new() { Id = 96, Name = "Soul Feast", Profession = Profession.Necromancer };
    public static readonly Skill NecroticTraversal = new() { Id = 97, Name = "Necrotic Traversal", Profession = Profession.Necromancer };
    public static readonly Skill ConsumeCorpse = new() { Id = 98, Name = "Consume Corpse", Profession = Profession.Necromancer };
    public static readonly Skill DeathNova = new() { Id = 104, Name = "Death Nova", Profession = Profession.Necromancer };
    public static readonly Skill DeathlySwarm = new() { Id = 105, Name = "Deathly Swarm", Profession = Profession.Necromancer };
    public static readonly Skill RottingFlesh = new() { Id = 106, Name = "Rotting Flesh", Profession = Profession.Necromancer };
    public static readonly Skill Virulence = new() { Id = 107, Name = "Virulence", Profession = Profession.Necromancer };
    public static readonly Skill TaintedFlesh = new() { Id = 113, Name = "Tainted Flesh", Profession = Profession.Necromancer };
    public static readonly Skill AuraoftheLich = new() { Id = 114, Name = "Aura of the Lich", Profession = Profession.Necromancer };
    public static readonly Skill DarkAura = new() { Id = 116, Name = "Dark Aura", Profession = Profession.Necromancer };
    public static readonly Skill BloodoftheMaster = new() { Id = 120, Name = "Blood of the Master", Profession = Profession.Necromancer };
    public static readonly Skill MalignIntervention = new() { Id = 122, Name = "Malign Intervention", Profession = Profession.Necromancer };
    public static readonly Skill InfuseCondition = new() { Id = 139, Name = "Infuse Condition", Profession = Profession.Necromancer };
    public static readonly Skill TasteofDeath = new() { Id = 152, Name = "Taste of Death", Profession = Profession.Necromancer };
    public static readonly Skill VileTouch = new() { Id = 155, Name = "Vile Touch", Profession = Profession.Necromancer };
    public static readonly Skill AnimateVampiricHorror = new() { Id = 805, Name = "Animate Vampiric Horror", Profession = Profession.Necromancer };
    public static readonly Skill Discord = new() { Id = 817, Name = "Discord", Profession = Profession.Necromancer };
    public static readonly Skill VileMiasma = new() { Id = 828, Name = "Vile Miasma", Profession = Profession.Necromancer };
    public static readonly Skill AnimateFleshGolem = new() { Id = 832, Name = "Animate Flesh Golem", Profession = Profession.Necromancer };
    public static readonly Skill FetidGround = new() { Id = 841, Name = "Fetid Ground", Profession = Profession.Necromancer };
    public static readonly Skill RisingBile = new() { Id = 935, Name = "Rising Bile", Profession = Profession.Necromancer };
    public static readonly Skill BitterChill = new() { Id = 1068, Name = "Bitter Chill", Profession = Profession.Necromancer };
    public static readonly Skill TasteofPain = new() { Id = 1069, Name = "Taste of Pain", Profession = Profession.Necromancer };
    public static readonly Skill AnimateShamblingHorror = new() { Id = 1351, Name = "Animate Shambling Horror", Profession = Profession.Necromancer };
    public static readonly Skill OrderofUndeath = new() { Id = 1352, Name = "Order of Undeath", Profession = Profession.Necromancer };
    public static readonly Skill PutridFlesh = new() { Id = 1353, Name = "Putrid Flesh", Profession = Profession.Necromancer };
    public static readonly Skill FeastfortheDead = new() { Id = 1354, Name = "Feast for the Dead", Profession = Profession.Necromancer };
    public static readonly Skill JaggedBones = new() { Id = 1355, Name = "Jagged Bones", Profession = Profession.Necromancer };
    public static readonly Skill Contagion = new() { Id = 1356, Name = "Contagion", Profession = Profession.Necromancer };
    public static readonly Skill ToxicChill = new() { Id = 1659, Name = "Toxic Chill", Profession = Profession.Necromancer };
    public static readonly Skill WitheringAura = new() { Id = 1997, Name = "Withering Aura", Profession = Profession.Necromancer };
    public static readonly Skill PutridBile = new() { Id = 2058, Name = "Putrid Bile", Profession = Profession.Necromancer };
    public static readonly Skill WailofDoom = new() { Id = 764, Name = "Wail of Doom", Profession = Profession.Necromancer };
    public static readonly Skill ReapersMark = new() { Id = 808, Name = "Reaper's Mark", Profession = Profession.Necromancer };
    public static readonly Skill IcyVeins = new() { Id = 821, Name = "Icy Veins", Profession = Profession.Necromancer };
    public static readonly Skill SignetofSorrow = new() { Id = 1363, Name = "Signet of Sorrow", Profession = Profession.Necromancer };
    public static readonly Skill SignetofLostSouls = new() { Id = 1365, Name = "Signet of Lost Souls", Profession = Profession.Necromancer };
    public static readonly Skill FoulFeast = new() { Id = 2057, Name = "Foul Feast", Profession = Profession.Necromancer };
    public static readonly Skill HexersVigor = new() { Id = 2138, Name = "Hexer's Vigor", Profession = Profession.Necromancer };
    public static readonly Skill Masochism = new() { Id = 2139, Name = "Masochism", Profession = Profession.Necromancer };
    public static readonly Skill AngorodonsGaze = new() { Id = 2189, Name = "Angorodon's Gaze", Profession = Profession.Necromancer };
    public static readonly Skill GrenthsBalance = new() { Id = 86, Name = "Grenth's Balance", Profession = Profession.Necromancer };
    public static readonly Skill GazeofContempt = new() { Id = 766, Name = "Gaze of Contempt", Profession = Profession.Necromancer };
    public static readonly Skill PowerBlock = new() { Id = 5, Name = "Power Block", Profession = Profession.Mesmer };
    public static readonly Skill HexBreaker = new() { Id = 10, Name = "Hex Breaker", Profession = Profession.Mesmer };
    public static readonly Skill PowerSpike = new() { Id = 23, Name = "Power Spike", Profession = Profession.Mesmer };
    public static readonly Skill PowerLeak = new() { Id = 24, Name = "Power Leak", Profession = Profession.Mesmer };
    public static readonly Skill Empathy = new() { Id = 26, Name = "Empathy", Profession = Profession.Mesmer };
    public static readonly Skill ShatterDelusions = new() { Id = 27, Name = "Shatter Delusions", Profession = Profession.Mesmer };
    public static readonly Skill Backfire = new() { Id = 28, Name = "Backfire", Profession = Profession.Mesmer };
    public static readonly Skill Blackout = new() { Id = 29, Name = "Blackout", Profession = Profession.Mesmer };
    public static readonly Skill Diversion = new() { Id = 30, Name = "Diversion", Profession = Profession.Mesmer };
    public static readonly Skill Ignorance = new() { Id = 35, Name = "Ignorance", Profession = Profession.Mesmer };
    public static readonly Skill EnergySurge = new() { Id = 39, Name = "Energy Surge", Profession = Profession.Mesmer };
    public static readonly Skill EnergyBurn = new() { Id = 42, Name = "Energy Burn", Profession = Profession.Mesmer };
    public static readonly Skill Guilt = new() { Id = 46, Name = "Guilt", Profession = Profession.Mesmer };
    public static readonly Skill MindWrack = new() { Id = 49, Name = "Mind Wrack", Profession = Profession.Mesmer };
    public static readonly Skill WastrelsWorry = new() { Id = 50, Name = "Wastrel's Worry", Profession = Profession.Mesmer };
    public static readonly Skill Shame = new() { Id = 51, Name = "Shame", Profession = Profession.Mesmer };
    public static readonly Skill Panic = new() { Id = 52, Name = "Panic", Profession = Profession.Mesmer };
    public static readonly Skill CryofFrustration = new() { Id = 57, Name = "Cry of Frustration", Profession = Profession.Mesmer };
    public static readonly Skill SignetofWeariness = new() { Id = 59, Name = "Signet of Weariness", Profession = Profession.Mesmer };
    public static readonly Skill ShatterHex = new() { Id = 67, Name = "Shatter Hex", Profession = Profession.Mesmer };
    public static readonly Skill ShatterEnchantment = new() { Id = 69, Name = "Shatter Enchantment", Profession = Profession.Mesmer };
    public static readonly Skill ChaosStorm = new() { Id = 77, Name = "Chaos Storm", Profession = Profession.Mesmer };
    public static readonly Skill ArcaneThievery = new() { Id = 81, Name = "Arcane Thievery", Profession = Profession.Mesmer };
    public static readonly Skill SignetofDisruption = new() { Id = 860, Name = "Signet of Disruption", Profession = Profession.Mesmer };
    public static readonly Skill VisionsofRegret = new() { Id = 878, Name = "Visions of Regret", Profession = Profession.Mesmer };
    public static readonly Skill Overload = new() { Id = 898, Name = "Overload", Profession = Profession.Mesmer };
    public static readonly Skill Complicate = new() { Id = 932, Name = "Complicate", Profession = Profession.Mesmer };
    public static readonly Skill UnnaturalSignet = new() { Id = 934, Name = "Unnatural Signet", Profession = Profession.Mesmer };
    public static readonly Skill PowerFlux = new() { Id = 953, Name = "Power Flux", Profession = Profession.Mesmer };
    public static readonly Skill Mistrust = new() { Id = 979, Name = "Mistrust", Profession = Profession.Mesmer };
    public static readonly Skill PsychicDistraction = new() { Id = 1053, Name = "Psychic Distraction", Profession = Profession.Mesmer };
    public static readonly Skill ArcaneLarceny = new() { Id = 1062, Name = "Arcane Larceny", Profession = Profession.Mesmer };
    public static readonly Skill WastrelsDemise = new() { Id = 1335, Name = "Wastrel's Demise", Profession = Profession.Mesmer };
    public static readonly Skill SpiritualPain = new() { Id = 1336, Name = "Spiritual Pain", Profession = Profession.Mesmer };
    public static readonly Skill EnchantersConundrum = new() { Id = 1345, Name = "Enchanter's Conundrum", Profession = Profession.Mesmer };
    public static readonly Skill HexEaterVortex = new() { Id = 1348, Name = "Hex Eater Vortex", Profession = Profession.Mesmer };
    public static readonly Skill SimpleThievery = new() { Id = 1350, Name = "Simple Thievery", Profession = Profession.Mesmer };
    public static readonly Skill PriceofPride = new() { Id = 1655, Name = "Price of Pride", Profession = Profession.Mesmer };
    public static readonly Skill SignetofDistraction = new() { Id = 1992, Name = "Signet of Distraction", Profession = Profession.Mesmer };
    public static readonly Skill PowerLock = new() { Id = 1994, Name = "Power Lock", Profession = Profession.Mesmer };
    public static readonly Skill Aneurysm = new() { Id = 2055, Name = "Aneurysm", Profession = Profession.Mesmer };
    public static readonly Skill MantraofRecovery = new() { Id = 13, Name = "Mantra of Recovery", Profession = Profession.Mesmer };
    public static readonly Skill KeystoneSignet = new() { Id = 63, Name = "Keystone Signet", Profession = Profession.Mesmer };
    public static readonly Skill ArcaneLanguor = new() { Id = 804, Name = "Arcane Languor", Profession = Profession.Mesmer };
    public static readonly Skill StolenSpeed = new() { Id = 880, Name = "Stolen Speed", Profession = Profession.Mesmer };
    public static readonly Skill PowerReturn = new() { Id = 931, Name = "Power Return", Profession = Profession.Mesmer };
    public static readonly Skill PsychicInstability = new() { Id = 1057, Name = "Psychic Instability", Profession = Profession.Mesmer };
    public static readonly Skill PersistenceofMemory = new() { Id = 1338, Name = "Persistence of Memory", Profession = Profession.Mesmer };
    public static readonly Skill SymbolsofInspiration = new() { Id = 1339, Name = "Symbols of Inspiration", Profession = Profession.Mesmer };
    public static readonly Skill SymbolicCelerity = new() { Id = 1340, Name = "Symbolic Celerity", Profession = Profession.Mesmer };
    public static readonly Skill SymbolicPosture = new() { Id = 1658, Name = "Symbolic Posture", Profession = Profession.Mesmer };
    public static readonly Skill Distortion = new() { Id = 11, Name = "Distortion", Profession = Profession.Mesmer };
    public static readonly Skill Fragility = new() { Id = 19, Name = "Fragility", Profession = Profession.Mesmer };
    public static readonly Skill ConjurePhantasm = new() { Id = 31, Name = "Conjure Phantasm", Profession = Profession.Mesmer };
    public static readonly Skill IllusionofWeakness = new() { Id = 32, Name = "Illusion of Weakness", Profession = Profession.Mesmer };
    public static readonly Skill IllusionaryWeaponry = new() { Id = 33, Name = "Illusionary Weaponry", Profession = Profession.Mesmer };
    public static readonly Skill SympatheticVisage = new() { Id = 34, Name = "Sympathetic Visage", Profession = Profession.Mesmer };
    public static readonly Skill ArcaneConundrum = new() { Id = 36, Name = "Arcane Conundrum", Profession = Profession.Mesmer };
    public static readonly Skill IllusionofHaste = new() { Id = 37, Name = "Illusion of Haste", Profession = Profession.Mesmer };
    public static readonly Skill Clumsiness = new() { Id = 43, Name = "Clumsiness", Profession = Profession.Mesmer };
    public static readonly Skill PhantomPain = new() { Id = 44, Name = "Phantom Pain", Profession = Profession.Mesmer };
    public static readonly Skill EtherealBurden = new() { Id = 45, Name = "Ethereal Burden", Profession = Profession.Mesmer };
    public static readonly Skill Ineptitude = new() { Id = 47, Name = "Ineptitude", Profession = Profession.Mesmer };
    public static readonly Skill Migraine = new() { Id = 53, Name = "Migraine", Profession = Profession.Mesmer };
    public static readonly Skill CripplingAnguish = new() { Id = 54, Name = "Crippling Anguish", Profession = Profession.Mesmer };
    public static readonly Skill FeveredDreams = new() { Id = 55, Name = "Fevered Dreams", Profession = Profession.Mesmer };
    public static readonly Skill SoothingImages = new() { Id = 56, Name = "Soothing Images", Profession = Profession.Mesmer };
    public static readonly Skill ImaginedBurden = new() { Id = 76, Name = "Imagined Burden", Profession = Profession.Mesmer };
    public static readonly Skill ConjureNightmare = new() { Id = 859, Name = "Conjure Nightmare", Profession = Profession.Mesmer };
    public static readonly Skill IllusionofPain = new() { Id = 879, Name = "Illusion of Pain", Profession = Profession.Mesmer };
    public static readonly Skill ImagesofRemorse = new() { Id = 899, Name = "Images of Remorse", Profession = Profession.Mesmer };
    public static readonly Skill SharedBurden = new() { Id = 900, Name = "Shared Burden", Profession = Profession.Mesmer };
    public static readonly Skill AccumulatedPain = new() { Id = 1052, Name = "Accumulated Pain", Profession = Profession.Mesmer };
    public static readonly Skill AncestorsVisage = new() { Id = 1054, Name = "Ancestor's Visage", Profession = Profession.Mesmer };
    public static readonly Skill RecurringInsecurity = new() { Id = 1055, Name = "Recurring Insecurity", Profession = Profession.Mesmer };
    public static readonly Skill KitahsBurden = new() { Id = 1056, Name = "Kitah's Burden", Profession = Profession.Mesmer };
    public static readonly Skill Frustration = new() { Id = 1341, Name = "Frustration", Profession = Profession.Mesmer };
    public static readonly Skill SignetofIllusions = new() { Id = 1346, Name = "Signet of Illusions", Profession = Profession.Mesmer };
    public static readonly Skill AirofDisenchantment = new() { Id = 1656, Name = "Air of Disenchantment", Profession = Profession.Mesmer };
    public static readonly Skill SignetofClumsiness = new() { Id = 1657, Name = "Signet of Clumsiness", Profession = Profession.Mesmer };
    public static readonly Skill SumofAllFears = new() { Id = 1996, Name = "Sum of All Fears", Profession = Profession.Mesmer };
    public static readonly Skill CalculatedRisk = new() { Id = 2053, Name = "Calculated Risk", Profession = Profession.Mesmer };
    public static readonly Skill ShrinkingArmor = new() { Id = 2054, Name = "Shrinking Armor", Profession = Profession.Mesmer };
    public static readonly Skill WanderingEye = new() { Id = 2056, Name = "Wandering Eye", Profession = Profession.Mesmer };
    public static readonly Skill ConfusingImages = new() { Id = 2137, Name = "Confusing Images", Profession = Profession.Mesmer };
    public static readonly Skill MantraofEarth = new() { Id = 6, Name = "Mantra of Earth", Profession = Profession.Mesmer };
    public static readonly Skill MantraofFlame = new() { Id = 7, Name = "Mantra of Flame", Profession = Profession.Mesmer };
    public static readonly Skill MantraofFrost = new() { Id = 8, Name = "Mantra of Frost", Profession = Profession.Mesmer };
    public static readonly Skill MantraofLightning = new() { Id = 9, Name = "Mantra of Lightning", Profession = Profession.Mesmer };
    public static readonly Skill MantraofPersistence = new() { Id = 14, Name = "Mantra of Persistence", Profession = Profession.Mesmer };
    public static readonly Skill MantraofInscriptions = new() { Id = 15, Name = "Mantra of Inscriptions", Profession = Profession.Mesmer };
    public static readonly Skill MantraofConcentration = new() { Id = 16, Name = "Mantra of Concentration", Profession = Profession.Mesmer };
    public static readonly Skill MantraofResolve = new() { Id = 17, Name = "Mantra of Resolve", Profession = Profession.Mesmer };
    public static readonly Skill MantraofSignets = new() { Id = 18, Name = "Mantra of Signets", Profession = Profession.Mesmer };
    public static readonly Skill InspiredEnchantment = new() { Id = 21, Name = "Inspired Enchantment", Profession = Profession.Mesmer };
    public static readonly Skill InspiredHex = new() { Id = 22, Name = "Inspired Hex", Profession = Profession.Mesmer };
    public static readonly Skill PowerDrain = new() { Id = 25, Name = "Power Drain", Profession = Profession.Mesmer };
    public static readonly Skill Channeling = new() { Id = 38, Name = "Channeling", Profession = Profession.Mesmer };
    public static readonly Skill EtherFeast = new() { Id = 40, Name = "Ether Feast", Profession = Profession.Mesmer };
    public static readonly Skill EtherLord = new() { Id = 41, Name = "Ether Lord", Profession = Profession.Mesmer };
    public static readonly Skill SpiritofFailure = new() { Id = 48, Name = "Spirit of Failure", Profession = Profession.Mesmer };
    public static readonly Skill LeechSignet = new() { Id = 61, Name = "Leech Signet", Profession = Profession.Mesmer };
    public static readonly Skill SignetofHumility = new() { Id = 62, Name = "Signet of Humility", Profession = Profession.Mesmer };
    public static readonly Skill SpiritShackles = new() { Id = 66, Name = "Spirit Shackles", Profession = Profession.Mesmer };
    public static readonly Skill DrainEnchantment = new() { Id = 68, Name = "Drain Enchantment", Profession = Profession.Mesmer };
    public static readonly Skill ElementalResistance = new() { Id = 72, Name = "Elemental Resistance", Profession = Profession.Mesmer };
    public static readonly Skill PhysicalResistance = new() { Id = 73, Name = "Physical Resistance", Profession = Profession.Mesmer };
    public static readonly Skill EnergyDrain = new() { Id = 79, Name = "Energy Drain", Profession = Profession.Mesmer };
    public static readonly Skill EnergyTap = new() { Id = 80, Name = "Energy Tap", Profession = Profession.Mesmer };
    public static readonly Skill MantraofRecall = new() { Id = 82, Name = "Mantra of Recall", Profession = Profession.Mesmer };
    public static readonly Skill PowerLeech = new() { Id = 803, Name = "Power Leech", Profession = Profession.Mesmer };
    public static readonly Skill LyssasAura = new() { Id = 813, Name = "Lyssa's Aura", Profession = Profession.Mesmer };
    public static readonly Skill EtherSignet = new() { Id = 881, Name = "Ether Signet", Profession = Profession.Mesmer };
    public static readonly Skill AuspiciousIncantation = new() { Id = 930, Name = "Auspicious Incantation", Profession = Profession.Mesmer };
    public static readonly Skill RevealedEnchantment = new() { Id = 1048, Name = "Revealed Enchantment", Profession = Profession.Mesmer };
    public static readonly Skill RevealedHex = new() { Id = 1049, Name = "Revealed Hex", Profession = Profession.Mesmer };
    public static readonly Skill HexEaterSignet = new() { Id = 1059, Name = "Hex Eater Signet", Profession = Profession.Mesmer };
    public static readonly Skill Feedback = new() { Id = 1061, Name = "Feedback", Profession = Profession.Mesmer };
    public static readonly Skill ExtendConditions = new() { Id = 1333, Name = "Extend Conditions", Profession = Profession.Mesmer };
    public static readonly Skill DrainDelusions = new() { Id = 1337, Name = "Drain Delusions", Profession = Profession.Mesmer };
    public static readonly Skill Tease = new() { Id = 1342, Name = "Tease", Profession = Profession.Mesmer };
    public static readonly Skill EtherPhantom = new() { Id = 1343, Name = "Ether Phantom", Profession = Profession.Mesmer };
    public static readonly Skill DischargeEnchantment = new() { Id = 1347, Name = "Discharge Enchantment", Profession = Profession.Mesmer };
    public static readonly Skill SignetofRecall = new() { Id = 1993, Name = "Signet of Recall", Profession = Profession.Mesmer };
    public static readonly Skill WasteNotWantNot = new() { Id = 1995, Name = "Waste Not, Want Not", Profession = Profession.Mesmer };
    public static readonly Skill SignetofMidnight = new() { Id = 58, Name = "Signet of Midnight", Profession = Profession.Mesmer };
    public static readonly Skill ArcaneMimicry = new() { Id = 65, Name = "Arcane Mimicry", Profession = Profession.Mesmer };
    public static readonly Skill Echo = new() { Id = 74, Name = "Echo", Profession = Profession.Mesmer };
    public static readonly Skill ArcaneEcho = new() { Id = 75, Name = "Arcane Echo", Profession = Profession.Mesmer };
    public static readonly Skill Epidemic = new() { Id = 78, Name = "Epidemic", Profession = Profession.Mesmer };
    public static readonly Skill LyssasBalance = new() { Id = 877, Name = "Lyssa's Balance", Profession = Profession.Mesmer };
    public static readonly Skill SignetofDisenchantment = new() { Id = 882, Name = "Signet of Disenchantment", Profession = Profession.Mesmer };
    public static readonly Skill ShatterStorm = new() { Id = 933, Name = "Shatter Storm", Profession = Profession.Mesmer };
    public static readonly Skill ExpelHexes = new() { Id = 954, Name = "Expel Hexes", Profession = Profession.Mesmer };
    public static readonly Skill Hypochondria = new() { Id = 1334, Name = "Hypochondria", Profession = Profession.Mesmer };
    public static readonly Skill WebofDisruption = new() { Id = 1344, Name = "Web of Disruption", Profession = Profession.Mesmer };
    public static readonly Skill MirrorofDisenchantment = new() { Id = 1349, Name = "Mirror of Disenchantment", Profession = Profession.Mesmer };
    public static readonly Skill WindborneSpeed = new() { Id = 160, Name = "Windborne Speed", Profession = Profession.Elementalist };
    public static readonly Skill Gale = new() { Id = 162, Name = "Gale", Profession = Profession.Elementalist };
    public static readonly Skill Whirlwind = new() { Id = 163, Name = "Whirlwind", Profession = Profession.Elementalist };
    public static readonly Skill LightningSurge = new() { Id = 205, Name = "Lightning Surge", Profession = Profession.Elementalist };
    public static readonly Skill BlindingFlash = new() { Id = 220, Name = "Blinding Flash", Profession = Profession.Elementalist };
    public static readonly Skill ConjureLightning = new() { Id = 221, Name = "Conjure Lightning", Profession = Profession.Elementalist };
    public static readonly Skill LightningStrike = new() { Id = 222, Name = "Lightning Strike", Profession = Profession.Elementalist };
    public static readonly Skill ChainLightning = new() { Id = 223, Name = "Chain Lightning", Profession = Profession.Elementalist };
    public static readonly Skill EnervatingCharge = new() { Id = 224, Name = "Enervating Charge", Profession = Profession.Elementalist };
    public static readonly Skill AirAttunement = new() { Id = 225, Name = "Air Attunement", Profession = Profession.Elementalist };
    public static readonly Skill MindShock = new() { Id = 226, Name = "Mind Shock", Profession = Profession.Elementalist };
    public static readonly Skill GlimmeringMark = new() { Id = 227, Name = "Glimmering Mark", Profession = Profession.Elementalist };
    public static readonly Skill Thunderclap = new() { Id = 228, Name = "Thunderclap", Profession = Profession.Elementalist };
    public static readonly Skill LightningOrb = new() { Id = 229, Name = "Lightning Orb", Profession = Profession.Elementalist };
    public static readonly Skill LightningJavelin = new() { Id = 230, Name = "Lightning Javelin", Profession = Profession.Elementalist };
    public static readonly Skill Shock = new() { Id = 231, Name = "Shock", Profession = Profession.Elementalist };
    public static readonly Skill LightningTouch = new() { Id = 232, Name = "Lightning Touch", Profession = Profession.Elementalist };
    public static readonly Skill RidetheLightning = new() { Id = 836, Name = "Ride the Lightning", Profession = Profession.Elementalist };
    public static readonly Skill ArcLightning = new() { Id = 842, Name = "Arc Lightning", Profession = Profession.Elementalist };
    public static readonly Skill Gust = new() { Id = 843, Name = "Gust", Profession = Profession.Elementalist };
    public static readonly Skill LightningHammer = new() { Id = 865, Name = "Lightning Hammer", Profession = Profession.Elementalist };
    public static readonly Skill TeinaisWind = new() { Id = 1081, Name = "Teinai's Wind", Profession = Profession.Elementalist };
    public static readonly Skill ShockArrow = new() { Id = 1082, Name = "Shock Arrow", Profession = Profession.Elementalist };
    public static readonly Skill BlindingSurge = new() { Id = 1367, Name = "Blinding Surge", Profession = Profession.Elementalist };
    public static readonly Skill ChillingWinds = new() { Id = 1368, Name = "Chilling Winds", Profession = Profession.Elementalist };
    public static readonly Skill LightningBolt = new() { Id = 1369, Name = "Lightning Bolt", Profession = Profession.Elementalist };
    public static readonly Skill StormDjinnsHaste = new() { Id = 1370, Name = "Storm Djinn's Haste", Profession = Profession.Elementalist };
    public static readonly Skill InvokeLightning = new() { Id = 1664, Name = "Invoke Lightning", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofSwiftness = new() { Id = 2002, Name = "Glyph of Swiftness", Profession = Profession.Elementalist };
    public static readonly Skill ShellShock = new() { Id = 2059, Name = "Shell Shock", Profession = Profession.Elementalist };
    public static readonly Skill ArmorofEarth = new() { Id = 165, Name = "Armor of Earth", Profession = Profession.Elementalist };
    public static readonly Skill KineticArmor = new() { Id = 166, Name = "Kinetic Armor", Profession = Profession.Elementalist };
    public static readonly Skill Eruption = new() { Id = 167, Name = "Eruption", Profession = Profession.Elementalist };
    public static readonly Skill MagneticAura = new() { Id = 168, Name = "Magnetic Aura", Profession = Profession.Elementalist };
    public static readonly Skill EarthAttunement = new() { Id = 169, Name = "Earth Attunement", Profession = Profession.Elementalist };
    public static readonly Skill Earthquake = new() { Id = 170, Name = "Earthquake", Profession = Profession.Elementalist };
    public static readonly Skill Stoning = new() { Id = 171, Name = "Stoning", Profession = Profession.Elementalist };
    public static readonly Skill StoneDaggers = new() { Id = 172, Name = "Stone Daggers", Profession = Profession.Elementalist };
    public static readonly Skill GraspingEarth = new() { Id = 173, Name = "Grasping Earth", Profession = Profession.Elementalist };
    public static readonly Skill Aftershock = new() { Id = 174, Name = "Aftershock", Profession = Profession.Elementalist };
    public static readonly Skill WardAgainstElements = new() { Id = 175, Name = "Ward Against Elements", Profession = Profession.Elementalist };
    public static readonly Skill WardAgainstMelee = new() { Id = 176, Name = "Ward Against Melee", Profession = Profession.Elementalist };
    public static readonly Skill WardAgainstFoes = new() { Id = 177, Name = "Ward Against Foes", Profession = Profession.Elementalist };
    public static readonly Skill IronMist = new() { Id = 216, Name = "Iron Mist", Profession = Profession.Elementalist };
    public static readonly Skill CrystalWave = new() { Id = 217, Name = "Crystal Wave", Profession = Profession.Elementalist };
    public static readonly Skill ObsidianFlesh = new() { Id = 218, Name = "Obsidian Flesh", Profession = Profession.Elementalist };
    public static readonly Skill ObsidianFlame = new() { Id = 219, Name = "Obsidian Flame", Profession = Profession.Elementalist };
    public static readonly Skill ChurningEarth = new() { Id = 844, Name = "Churning Earth", Profession = Profession.Elementalist };
    public static readonly Skill Shockwave = new() { Id = 937, Name = "Shockwave", Profession = Profession.Elementalist };
    public static readonly Skill WardofStability = new() { Id = 938, Name = "Ward of Stability", Profession = Profession.Elementalist };
    public static readonly Skill UnsteadyGround = new() { Id = 1083, Name = "Unsteady Ground", Profession = Profession.Elementalist };
    public static readonly Skill SliverArmor = new() { Id = 1084, Name = "Sliver Armor", Profession = Profession.Elementalist };
    public static readonly Skill AshBlast = new() { Id = 1085, Name = "Ash Blast", Profession = Profession.Elementalist };
    public static readonly Skill DragonsStomp = new() { Id = 1086, Name = "Dragon's Stomp", Profession = Profession.Elementalist };
    public static readonly Skill TeinaisCrystals = new() { Id = 1099, Name = "Teinai's Crystals", Profession = Profession.Elementalist };
    public static readonly Skill StoneStriker = new() { Id = 1371, Name = "Stone Striker", Profession = Profession.Elementalist };
    public static readonly Skill Sandstorm = new() { Id = 1372, Name = "Sandstorm", Profession = Profession.Elementalist };
    public static readonly Skill StoneSheath = new() { Id = 1373, Name = "Stone Sheath", Profession = Profession.Elementalist };
    public static readonly Skill EbonHawk = new() { Id = 1374, Name = "Ebon Hawk", Profession = Profession.Elementalist };
    public static readonly Skill StonefleshAura = new() { Id = 1375, Name = "Stoneflesh Aura", Profession = Profession.Elementalist };
    public static readonly Skill Glowstone = new() { Id = 1661, Name = "Glowstone", Profession = Profession.Elementalist };
    public static readonly Skill EarthenShackles = new() { Id = 2000, Name = "Earthen Shackles", Profession = Profession.Elementalist };
    public static readonly Skill WardofWeakness = new() { Id = 2001, Name = "Ward of Weakness", Profession = Profession.Elementalist };
    public static readonly Skill MagneticSurge = new() { Id = 2190, Name = "Magnetic Surge", Profession = Profession.Elementalist };
    public static readonly Skill ElementalAttunement = new() { Id = 164, Name = "Elemental Attunement", Profession = Profession.Elementalist };
    public static readonly Skill EtherProdigy = new() { Id = 178, Name = "Ether Prodigy", Profession = Profession.Elementalist };
    public static readonly Skill AuraofRestoration = new() { Id = 180, Name = "Aura of Restoration", Profession = Profession.Elementalist };
    public static readonly Skill EtherRenewal = new() { Id = 181, Name = "Ether Renewal", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofEnergy = new() { Id = 199, Name = "Glyph of Energy", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofLesserEnergy = new() { Id = 200, Name = "Glyph of Lesser Energy", Profession = Profession.Elementalist };
    public static readonly Skill EnergyBoon = new() { Id = 837, Name = "Energy Boon", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofRestoration = new() { Id = 1376, Name = "Glyph of Restoration", Profession = Profession.Elementalist };
    public static readonly Skill EtherPrism = new() { Id = 1377, Name = "Ether Prism", Profession = Profession.Elementalist };
    public static readonly Skill MasterofMagic = new() { Id = 1378, Name = "Master of Magic", Profession = Profession.Elementalist };
    public static readonly Skill EnergyBlast = new() { Id = 2193, Name = "Energy Blast", Profession = Profession.Elementalist };
    public static readonly Skill IncendiaryBonds = new() { Id = 179, Name = "Incendiary Bonds", Profession = Profession.Elementalist };
    public static readonly Skill ConjureFlame = new() { Id = 182, Name = "Conjure Flame", Profession = Profession.Elementalist };
    public static readonly Skill Inferno = new() { Id = 183, Name = "Inferno", Profession = Profession.Elementalist };
    public static readonly Skill FireAttunement = new() { Id = 184, Name = "Fire Attunement", Profession = Profession.Elementalist };
    public static readonly Skill MindBurn = new() { Id = 185, Name = "Mind Burn", Profession = Profession.Elementalist };
    public static readonly Skill Fireball = new() { Id = 186, Name = "Fireball", Profession = Profession.Elementalist };
    public static readonly Skill Meteor = new() { Id = 187, Name = "Meteor", Profession = Profession.Elementalist };
    public static readonly Skill FlameBurst = new() { Id = 188, Name = "Flame Burst", Profession = Profession.Elementalist };
    public static readonly Skill RodgortsInvocation = new() { Id = 189, Name = "Rodgort's Invocation", Profession = Profession.Elementalist };
    public static readonly Skill MarkofRodgort = new() { Id = 190, Name = "Mark of Rodgort", Profession = Profession.Elementalist };
    public static readonly Skill Immolate = new() { Id = 191, Name = "Immolate", Profession = Profession.Elementalist };
    public static readonly Skill MeteorShower = new() { Id = 192, Name = "Meteor Shower", Profession = Profession.Elementalist };
    public static readonly Skill Phoenix = new() { Id = 193, Name = "Phoenix", Profession = Profession.Elementalist };
    public static readonly Skill Flare = new() { Id = 194, Name = "Flare", Profession = Profession.Elementalist };
    public static readonly Skill LavaFont = new() { Id = 195, Name = "Lava Font", Profession = Profession.Elementalist };
    public static readonly Skill SearingHeat = new() { Id = 196, Name = "Searing Heat", Profession = Profession.Elementalist };
    public static readonly Skill FireStorm = new() { Id = 197, Name = "Fire Storm", Profession = Profession.Elementalist };
    public static readonly Skill BurningSpeed = new() { Id = 823, Name = "Burning Speed", Profession = Profession.Elementalist };
    public static readonly Skill LavaArrows = new() { Id = 824, Name = "Lava Arrows", Profession = Profession.Elementalist };
    public static readonly Skill BedofCoals = new() { Id = 825, Name = "Bed of Coals", Profession = Profession.Elementalist };
    public static readonly Skill LiquidFlame = new() { Id = 845, Name = "Liquid Flame", Profession = Profession.Elementalist };
    public static readonly Skill SearingFlames = new() { Id = 884, Name = "Searing Flames", Profession = Profession.Elementalist };
    public static readonly Skill SmolderingEmbers = new() { Id = 1090, Name = "Smoldering Embers", Profession = Profession.Elementalist };
    public static readonly Skill DoubleDragon = new() { Id = 1091, Name = "Double Dragon", Profession = Profession.Elementalist };
    public static readonly Skill TeinaisHeat = new() { Id = 1093, Name = "Teinai's Heat", Profession = Profession.Elementalist };
    public static readonly Skill BreathofFire = new() { Id = 1094, Name = "Breath of Fire", Profession = Profession.Elementalist };
    public static readonly Skill StarBurst = new() { Id = 1095, Name = "Star Burst", Profession = Profession.Elementalist };
    public static readonly Skill GlowingGaze = new() { Id = 1379, Name = "Glowing Gaze", Profession = Profession.Elementalist };
    public static readonly Skill SavannahHeat = new() { Id = 1380, Name = "Savannah Heat", Profession = Profession.Elementalist };
    public static readonly Skill FlameDjinnsHaste = new() { Id = 1381, Name = "Flame Djinn's Haste", Profession = Profession.Elementalist };
    public static readonly Skill MindBlast = new() { Id = 1662, Name = "Mind Blast", Profession = Profession.Elementalist };
    public static readonly Skill ElementalFlame = new() { Id = 1663, Name = "Elemental Flame", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofImmolation = new() { Id = 2060, Name = "Glyph of Immolation", Profession = Profession.Elementalist };
    public static readonly Skill Rust = new() { Id = 204, Name = "Rust", Profession = Profession.Elementalist };
    public static readonly Skill ArmorofFrost = new() { Id = 206, Name = "Armor of Frost", Profession = Profession.Elementalist };
    public static readonly Skill ConjureFrost = new() { Id = 207, Name = "Conjure Frost", Profession = Profession.Elementalist };
    public static readonly Skill WaterAttunement = new() { Id = 208, Name = "Water Attunement", Profession = Profession.Elementalist };
    public static readonly Skill MindFreeze = new() { Id = 209, Name = "Mind Freeze", Profession = Profession.Elementalist };
    public static readonly Skill IcePrison = new() { Id = 210, Name = "Ice Prison", Profession = Profession.Elementalist };
    public static readonly Skill IceSpikes = new() { Id = 211, Name = "Ice Spikes", Profession = Profession.Elementalist };
    public static readonly Skill FrozenBurst = new() { Id = 212, Name = "Frozen Burst", Profession = Profession.Elementalist };
    public static readonly Skill ShardStorm = new() { Id = 213, Name = "Shard Storm", Profession = Profession.Elementalist };
    public static readonly Skill IceSpear = new() { Id = 214, Name = "Ice Spear", Profession = Profession.Elementalist };
    public static readonly Skill Maelstrom = new() { Id = 215, Name = "Maelstrom", Profession = Profession.Elementalist };
    public static readonly Skill SwirlingAura = new() { Id = 233, Name = "Swirling Aura", Profession = Profession.Elementalist };
    public static readonly Skill DeepFreeze = new() { Id = 234, Name = "Deep Freeze", Profession = Profession.Elementalist };
    public static readonly Skill BlurredVision = new() { Id = 235, Name = "Blurred Vision", Profession = Profession.Elementalist };
    public static readonly Skill MistForm = new() { Id = 236, Name = "Mist Form", Profession = Profession.Elementalist };
    public static readonly Skill WaterTrident = new() { Id = 237, Name = "Water Trident", Profession = Profession.Elementalist };
    public static readonly Skill ArmorofMist = new() { Id = 238, Name = "Armor of Mist", Profession = Profession.Elementalist };
    public static readonly Skill WardAgainstHarm = new() { Id = 239, Name = "Ward Against Harm", Profession = Profession.Elementalist };
    public static readonly Skill Shatterstone = new() { Id = 809, Name = "Shatterstone", Profession = Profession.Elementalist };
    public static readonly Skill Steam = new() { Id = 846, Name = "Steam", Profession = Profession.Elementalist };
    public static readonly Skill VaporBlade = new() { Id = 866, Name = "Vapor Blade", Profession = Profession.Elementalist };
    public static readonly Skill IcyPrism = new() { Id = 903, Name = "Icy Prism", Profession = Profession.Elementalist };
    public static readonly Skill IcyShackles = new() { Id = 939, Name = "Icy Shackles", Profession = Profession.Elementalist };
    public static readonly Skill TeinaisPrison = new() { Id = 1097, Name = "Teinai's Prison", Profession = Profession.Elementalist };
    public static readonly Skill MirrorofIce = new() { Id = 1098, Name = "Mirror of Ice", Profession = Profession.Elementalist };
    public static readonly Skill FrigidArmor = new() { Id = 1261, Name = "Frigid Armor", Profession = Profession.Elementalist };
    public static readonly Skill FreezingGust = new() { Id = 1382, Name = "Freezing Gust", Profession = Profession.Elementalist };
    public static readonly Skill WintersEmbrace = new() { Id = 1999, Name = "Winter's Embrace", Profession = Profession.Elementalist };
    public static readonly Skill SlipperyGround = new() { Id = 2191, Name = "Slippery Ground", Profession = Profession.Elementalist };
    public static readonly Skill GlowingIce = new() { Id = 2192, Name = "Glowing Ice", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofElementalPower = new() { Id = 198, Name = "Glyph of Elemental Power", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofConcentration = new() { Id = 201, Name = "Glyph of Concentration", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofSacrifice = new() { Id = 202, Name = "Glyph of Sacrifice", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofRenewal = new() { Id = 203, Name = "Glyph of Renewal", Profession = Profession.Elementalist };
    public static readonly Skill SecondWind = new() { Id = 1088, Name = "Second Wind", Profession = Profession.Elementalist };
    public static readonly Skill GlyphofEssence = new() { Id = 1096, Name = "Glyph of Essence", Profession = Profession.Elementalist };
    public static readonly Skill TwistingFangs = new() { Id = 776, Name = "Twisting Fangs", Profession = Profession.Assassin };
    public static readonly Skill BlackLotusStrike = new() { Id = 779, Name = "Black Lotus Strike", Profession = Profession.Assassin };
    public static readonly Skill UnsuspectingStrike = new() { Id = 783, Name = "Unsuspecting Strike", Profession = Profession.Assassin };
    public static readonly Skill SharpenDaggers = new() { Id = 926, Name = "Sharpen Daggers", Profession = Profession.Assassin };
    public static readonly Skill CriticalEye = new() { Id = 1018, Name = "Critical Eye", Profession = Profession.Assassin };
    public static readonly Skill CriticalStrike = new() { Id = 1019, Name = "Critical Strike", Profession = Profession.Assassin };
    public static readonly Skill CriticalDefenses = new() { Id = 1027, Name = "Critical Defenses", Profession = Profession.Assassin };
    public static readonly Skill DarkApostasy = new() { Id = 1029, Name = "Dark Apostasy", Profession = Profession.Assassin };
    public static readonly Skill LocustsFury = new() { Id = 1030, Name = "Locust's Fury", Profession = Profession.Assassin };
    public static readonly Skill SeepingWound = new() { Id = 1034, Name = "Seeping Wound", Profession = Profession.Assassin };
    public static readonly Skill PalmStrike = new() { Id = 1045, Name = "Palm Strike", Profession = Profession.Assassin };
    public static readonly Skill MaliciousStrike = new() { Id = 1633, Name = "Malicious Strike", Profession = Profession.Assassin };
    public static readonly Skill ShatteringAssault = new() { Id = 1634, Name = "Shattering Assault", Profession = Profession.Assassin };
    public static readonly Skill DeadlyHaste = new() { Id = 1638, Name = "Deadly Haste", Profession = Profession.Assassin };
    public static readonly Skill AssassinsRemedy = new() { Id = 1639, Name = "Assassin's Remedy", Profession = Profession.Assassin };
    public static readonly Skill WayoftheAssassin = new() { Id = 1649, Name = "Way of the Assassin", Profession = Profession.Assassin };
    public static readonly Skill WayoftheMaster = new() { Id = 2187, Name = "Way of the Master", Profession = Profession.Assassin };
    public static readonly Skill DeathBlossom = new() { Id = 775, Name = "Death Blossom", Profession = Profession.Assassin };
    public static readonly Skill HornsoftheOx = new() { Id = 777, Name = "Horns of the Ox", Profession = Profession.Assassin };
    public static readonly Skill FallingSpider = new() { Id = 778, Name = "Falling Spider", Profession = Profession.Assassin };
    public static readonly Skill FoxFangs = new() { Id = 780, Name = "Fox Fangs", Profession = Profession.Assassin };
    public static readonly Skill MoebiusStrike = new() { Id = 781, Name = "Moebius Strike", Profession = Profession.Assassin };
    public static readonly Skill JaggedStrike = new() { Id = 782, Name = "Jagged Strike", Profession = Profession.Assassin };
    public static readonly Skill DesperateStrike = new() { Id = 948, Name = "Desperate Strike", Profession = Profession.Assassin };
    public static readonly Skill ExhaustingAssault = new() { Id = 975, Name = "Exhausting Assault", Profession = Profession.Assassin };
    public static readonly Skill RepeatingStrike = new() { Id = 976, Name = "Repeating Strike", Profession = Profession.Assassin };
    public static readonly Skill NineTailStrike = new() { Id = 986, Name = "Nine Tail Strike", Profession = Profession.Assassin };
    public static readonly Skill TempleStrike = new() { Id = 988, Name = "Temple Strike", Profession = Profession.Assassin };
    public static readonly Skill GoldenPhoenixStrike = new() { Id = 989, Name = "Golden Phoenix Strike", Profession = Profession.Assassin };
    public static readonly Skill BladesofSteel = new() { Id = 1020, Name = "Blades of Steel", Profession = Profession.Assassin };
    public static readonly Skill JungleStrike = new() { Id = 1021, Name = "Jungle Strike", Profession = Profession.Assassin };
    public static readonly Skill WildStrike = new() { Id = 1022, Name = "Wild Strike", Profession = Profession.Assassin };
    public static readonly Skill LeapingMantisSting = new() { Id = 1023, Name = "Leaping Mantis Sting", Profession = Profession.Assassin };
    public static readonly Skill BlackMantisThrust = new() { Id = 1024, Name = "Black Mantis Thrust", Profession = Profession.Assassin };
    public static readonly Skill DisruptingStab = new() { Id = 1025, Name = "Disrupting Stab", Profession = Profession.Assassin };
    public static readonly Skill GoldenLotusStrike = new() { Id = 1026, Name = "Golden Lotus Strike", Profession = Profession.Assassin };
    public static readonly Skill FlashingBlades = new() { Id = 1042, Name = "Flashing Blades", Profession = Profession.Assassin };
    public static readonly Skill GoldenSkullStrike = new() { Id = 1635, Name = "Golden Skull Strike", Profession = Profession.Assassin };
    public static readonly Skill BlackSpiderStrike = new() { Id = 1636, Name = "Black Spider Strike", Profession = Profession.Assassin };
    public static readonly Skill GoldenFoxStrike = new() { Id = 1637, Name = "Golden Fox Strike", Profession = Profession.Assassin };
    public static readonly Skill FoxsPromise = new() { Id = 1640, Name = "Fox's Promise", Profession = Profession.Assassin };
    public static readonly Skill LotusStrike = new() { Id = 1987, Name = "Lotus Strike", Profession = Profession.Assassin };
    public static readonly Skill GoldenFangStrike = new() { Id = 1988, Name = "Golden Fang Strike", Profession = Profession.Assassin };
    public static readonly Skill FallingLotusStrike = new() { Id = 1990, Name = "Falling Lotus Strike", Profession = Profession.Assassin };
    public static readonly Skill TramplingOx = new() { Id = 2135, Name = "Trampling Ox", Profession = Profession.Assassin };
    public static readonly Skill MarkofInsecurity = new() { Id = 570, Name = "Mark of Insecurity", Profession = Profession.Assassin };
    public static readonly Skill DisruptingDagger = new() { Id = 571, Name = "Disrupting Dagger", Profession = Profession.Assassin };
    public static readonly Skill DeadlyParadox = new() { Id = 572, Name = "Deadly Paradox", Profession = Profession.Assassin };
    public static readonly Skill EntanglingAsp = new() { Id = 784, Name = "Entangling Asp", Profession = Profession.Assassin };
    public static readonly Skill MarkofDeath = new() { Id = 785, Name = "Mark of Death", Profession = Profession.Assassin };
    public static readonly Skill IronPalm = new() { Id = 786, Name = "Iron Palm", Profession = Profession.Assassin };
    public static readonly Skill EnduringToxin = new() { Id = 800, Name = "Enduring Toxin", Profession = Profession.Assassin };
    public static readonly Skill ShroudofSilence = new() { Id = 801, Name = "Shroud of Silence", Profession = Profession.Assassin };
    public static readonly Skill ExposeDefenses = new() { Id = 802, Name = "Expose Defenses", Profession = Profession.Assassin };
    public static readonly Skill ScorpionWire = new() { Id = 815, Name = "Scorpion Wire", Profession = Profession.Assassin };
    public static readonly Skill SiphonStrength = new() { Id = 827, Name = "Siphon Strength", Profession = Profession.Assassin };
    public static readonly Skill DancingDaggers = new() { Id = 858, Name = "Dancing Daggers", Profession = Profession.Assassin };
    public static readonly Skill SignetofShadows = new() { Id = 876, Name = "Signet of Shadows", Profession = Profession.Assassin };
    public static readonly Skill ShamefulFear = new() { Id = 927, Name = "Shameful Fear", Profession = Profession.Assassin };
    public static readonly Skill SiphonSpeed = new() { Id = 951, Name = "Siphon Speed", Profession = Profession.Assassin };
    public static readonly Skill MantisTouch = new() { Id = 974, Name = "Mantis Touch", Profession = Profession.Assassin };
    public static readonly Skill WayoftheEmptyPalm = new() { Id = 987, Name = "Way of the Empty Palm", Profession = Profession.Assassin };
    public static readonly Skill ExpungeEnchantments = new() { Id = 990, Name = "Expunge Enchantments", Profession = Profession.Assassin };
    public static readonly Skill Impale = new() { Id = 1033, Name = "Impale", Profession = Profession.Assassin };
    public static readonly Skill AssassinsPromise = new() { Id = 1035, Name = "Assassin's Promise", Profession = Profession.Assassin };
    public static readonly Skill CripplingDagger = new() { Id = 1038, Name = "Crippling Dagger", Profession = Profession.Assassin };
    public static readonly Skill DarkPrison = new() { Id = 1044, Name = "Dark Prison", Profession = Profession.Assassin };
    public static readonly Skill AuguryofDeath = new() { Id = 1646, Name = "Augury of Death", Profession = Profession.Assassin };
    public static readonly Skill SignetofToxicShock = new() { Id = 1647, Name = "Signet of Toxic Shock", Profession = Profession.Assassin };
    public static readonly Skill ShadowPrison = new() { Id = 1652, Name = "Shadow Prison", Profession = Profession.Assassin };
    public static readonly Skill VampiricAssault = new() { Id = 1986, Name = "Vampiric Assault", Profession = Profession.Assassin };
    public static readonly Skill SadistsSignet = new() { Id = 1991, Name = "Sadist's Signet", Profession = Profession.Assassin };
    public static readonly Skill ShadowFang = new() { Id = 2052, Name = "Shadow Fang", Profession = Profession.Assassin };
    public static readonly Skill SignetofDeadlyCorruption = new() { Id = 2186, Name = "Signet of Deadly Corruption", Profession = Profession.Assassin };
    public static readonly Skill VipersDefense = new() { Id = 769, Name = "Viper's Defense", Profession = Profession.Assassin };
    public static readonly Skill Return = new() { Id = 770, Name = "Return", Profession = Profession.Assassin };
    public static readonly Skill BeguilingHaze = new() { Id = 799, Name = "Beguiling Haze", Profession = Profession.Assassin };
    public static readonly Skill ShadowRefuge = new() { Id = 814, Name = "Shadow Refuge", Profession = Profession.Assassin };
    public static readonly Skill MirroredStance = new() { Id = 816, Name = "Mirrored Stance", Profession = Profession.Assassin };
    public static readonly Skill ShadowForm = new() { Id = 826, Name = "Shadow Form", Profession = Profession.Assassin };
    public static readonly Skill ShadowShroud = new() { Id = 928, Name = "Shadow Shroud", Profession = Profession.Assassin };
    public static readonly Skill ShadowofHaste = new() { Id = 929, Name = "Shadow of Haste", Profession = Profession.Assassin };
    public static readonly Skill WayoftheFox = new() { Id = 949, Name = "Way of the Fox", Profession = Profession.Assassin };
    public static readonly Skill ShadowyBurden = new() { Id = 950, Name = "Shadowy Burden", Profession = Profession.Assassin };
    public static readonly Skill DeathsCharge = new() { Id = 952, Name = "Death's Charge", Profession = Profession.Assassin };
    public static readonly Skill BlindingPowder = new() { Id = 973, Name = "Blinding Powder", Profession = Profession.Assassin };
    public static readonly Skill WayoftheLotus = new() { Id = 977, Name = "Way of the Lotus", Profession = Profession.Assassin };
    public static readonly Skill Caltrops = new() { Id = 985, Name = "Caltrops", Profession = Profession.Assassin };
    public static readonly Skill WayofPerfection = new() { Id = 1028, Name = "Way of Perfection", Profession = Profession.Assassin };
    public static readonly Skill ShroudofDistress = new() { Id = 1031, Name = "Shroud of Distress", Profession = Profession.Assassin };
    public static readonly Skill HeartofShadow = new() { Id = 1032, Name = "Heart of Shadow", Profession = Profession.Assassin };
    public static readonly Skill DarkEscape = new() { Id = 1037, Name = "Dark Escape", Profession = Profession.Assassin };
    public static readonly Skill UnseenFury = new() { Id = 1041, Name = "Unseen Fury", Profession = Profession.Assassin };
    public static readonly Skill FeignedNeutrality = new() { Id = 1641, Name = "Feigned Neutrality", Profession = Profession.Assassin };
    public static readonly Skill HiddenCaltrops = new() { Id = 1642, Name = "Hidden Caltrops", Profession = Profession.Assassin };
    public static readonly Skill DeathsRetreat = new() { Id = 1651, Name = "Death's Retreat", Profession = Profession.Assassin };
    public static readonly Skill SmokePowderDefense = new() { Id = 2136, Name = "Smoke Powder Defense", Profession = Profession.Assassin };
    public static readonly Skill AuraofDisplacement = new() { Id = 771, Name = "Aura of Displacement", Profession = Profession.Assassin };
    public static readonly Skill Recall = new() { Id = 925, Name = "Recall", Profession = Profession.Assassin };
    public static readonly Skill MarkofInstability = new() { Id = 978, Name = "Mark of Instability", Profession = Profession.Assassin };
    public static readonly Skill SignetofMalice = new() { Id = 1036, Name = "Signet of Malice", Profession = Profession.Assassin };
    public static readonly Skill SpiritWalk = new() { Id = 1040, Name = "Spirit Walk", Profession = Profession.Assassin };
    public static readonly Skill Dash = new() { Id = 1043, Name = "Dash", Profession = Profession.Assassin };
    public static readonly Skill AssaultEnchantments = new() { Id = 1643, Name = "Assault Enchantments", Profession = Profession.Assassin };
    public static readonly Skill WastrelsCollapse = new() { Id = 1644, Name = "Wastrel's Collapse", Profession = Profession.Assassin };
    public static readonly Skill LiftEnchantment = new() { Id = 1645, Name = "Lift Enchantment", Profession = Profession.Assassin };
    public static readonly Skill SignetofTwilight = new() { Id = 1648, Name = "Signet of Twilight", Profession = Profession.Assassin };
    public static readonly Skill ShadowWalk = new() { Id = 1650, Name = "Shadow Walk", Profession = Profession.Assassin };
    public static readonly Skill Swap = new() { Id = 1653, Name = "Swap", Profession = Profession.Assassin };
    public static readonly Skill ShadowMeld = new() { Id = 1654, Name = "Shadow Meld", Profession = Profession.Assassin };
    public static readonly Skill GraspingWasKuurong = new() { Id = 789, Name = "Grasping Was Kuurong", Profession = Profession.Ritualist };
    public static readonly Skill SplinterWeapon = new() { Id = 792, Name = "Splinter Weapon", Profession = Profession.Ritualist };
    public static readonly Skill WailingWeapon = new() { Id = 794, Name = "Wailing Weapon", Profession = Profession.Ritualist };
    public static readonly Skill NightmareWeapon = new() { Id = 795, Name = "Nightmare Weapon", Profession = Profession.Ritualist };
    public static readonly Skill SpiritRift = new() { Id = 910, Name = "Spirit Rift", Profession = Profession.Ritualist };
    public static readonly Skill Lamentation = new() { Id = 916, Name = "Lamentation", Profession = Profession.Ritualist };
    public static readonly Skill SpiritBurn = new() { Id = 919, Name = "Spirit Burn", Profession = Profession.Ritualist };
    public static readonly Skill Destruction = new() { Id = 920, Name = "Destruction", Profession = Profession.Ritualist };
    public static readonly Skill ClamorofSouls = new() { Id = 1215, Name = "Clamor of Souls", Profession = Profession.Ritualist };
    public static readonly Skill CruelWasDaoshen = new() { Id = 1218, Name = "Cruel Was Daoshen", Profession = Profession.Ritualist };
    public static readonly Skill ChanneledStrike = new() { Id = 1225, Name = "Channeled Strike", Profession = Profession.Ritualist };
    public static readonly Skill SpiritBoonStrike = new() { Id = 1226, Name = "Spirit Boon Strike", Profession = Profession.Ritualist };
    public static readonly Skill EssenceStrike = new() { Id = 1227, Name = "Essence Strike", Profession = Profession.Ritualist };
    public static readonly Skill SpiritSiphon = new() { Id = 1228, Name = "Spirit Siphon", Profession = Profession.Ritualist };
    public static readonly Skill PainfulBond = new() { Id = 1237, Name = "Painful Bond", Profession = Profession.Ritualist };
    public static readonly Skill SignetofSpirits = new() { Id = 1239, Name = "Signet of Spirits", Profession = Profession.Ritualist };
    public static readonly Skill GazefromBeyond = new() { Id = 1245, Name = "Gaze from Beyond", Profession = Profession.Ritualist };
    public static readonly Skill AncestorsRage = new() { Id = 1246, Name = "Ancestors' Rage", Profession = Profession.Ritualist };
    public static readonly Skill Bloodsong = new() { Id = 1253, Name = "Bloodsong", Profession = Profession.Ritualist };
    public static readonly Skill RenewingSurge = new() { Id = 1478, Name = "Renewing Surge", Profession = Profession.Ritualist };
    public static readonly Skill OfferingofSpirit = new() { Id = 1479, Name = "Offering of Spirit", Profession = Profession.Ritualist };
    public static readonly Skill DestructiveWasGlaive = new() { Id = 1732, Name = "Destructive Was Glaive", Profession = Profession.Ritualist };
    public static readonly Skill WieldersStrike = new() { Id = 1733, Name = "Wielder's Strike", Profession = Profession.Ritualist };
    public static readonly Skill GazeofFury = new() { Id = 1734, Name = "Gaze of Fury", Profession = Profession.Ritualist };
    public static readonly Skill CaretakersCharge = new() { Id = 1744, Name = "Caretaker's Charge", Profession = Profession.Ritualist };
    public static readonly Skill WeaponofFury = new() { Id = 1749, Name = "Weapon of Fury", Profession = Profession.Ritualist };
    public static readonly Skill WarmongersWeapon = new() { Id = 1751, Name = "Warmonger's Weapon", Profession = Profession.Ritualist };
    public static readonly Skill WeaponofAggression = new() { Id = 2073, Name = "Weapon of Aggression", Profession = Profession.Ritualist };
    public static readonly Skill Agony = new() { Id = 2205, Name = "Agony", Profession = Profession.Ritualist };
    public static readonly Skill MightyWasVorizun = new() { Id = 773, Name = "Mighty Was Vorizun", Profession = Profession.Ritualist };
    public static readonly Skill Shadowsong = new() { Id = 871, Name = "Shadowsong", Profession = Profession.Ritualist };
    public static readonly Skill Union = new() { Id = 911, Name = "Union", Profession = Profession.Ritualist };
    public static readonly Skill Dissonance = new() { Id = 921, Name = "Dissonance", Profession = Profession.Ritualist };
    public static readonly Skill Disenchantment = new() { Id = 923, Name = "Disenchantment", Profession = Profession.Ritualist };
    public static readonly Skill Restoration = new() { Id = 963, Name = "Restoration", Profession = Profession.Ritualist };
    public static readonly Skill Shelter = new() { Id = 982, Name = "Shelter", Profession = Profession.Ritualist };
    public static readonly Skill ArmorofUnfeeling = new() { Id = 1232, Name = "Armor of Unfeeling", Profession = Profession.Ritualist };
    public static readonly Skill DulledWeapon = new() { Id = 1235, Name = "Dulled Weapon", Profession = Profession.Ritualist };
    public static readonly Skill BindingChains = new() { Id = 1236, Name = "Binding Chains", Profession = Profession.Ritualist };
    public static readonly Skill Pain = new() { Id = 1247, Name = "Pain", Profession = Profession.Ritualist };
    public static readonly Skill Displacement = new() { Id = 1249, Name = "Displacement", Profession = Profession.Ritualist };
    public static readonly Skill Earthbind = new() { Id = 1252, Name = "Earthbind", Profession = Profession.Ritualist };
    public static readonly Skill Wanderlust = new() { Id = 1255, Name = "Wanderlust", Profession = Profession.Ritualist };
    public static readonly Skill BrutalWeapon = new() { Id = 1258, Name = "Brutal Weapon", Profession = Profession.Ritualist };
    public static readonly Skill GuidedWeapon = new() { Id = 1259, Name = "Guided Weapon", Profession = Profession.Ritualist };
    public static readonly Skill Soothing = new() { Id = 1266, Name = "Soothing", Profession = Profession.Ritualist };
    public static readonly Skill VitalWeapon = new() { Id = 1267, Name = "Vital Weapon", Profession = Profession.Ritualist };
    public static readonly Skill WeaponofQuickening = new() { Id = 1268, Name = "Weapon of Quickening", Profession = Profession.Ritualist };
    public static readonly Skill SignetofGhostlyMight = new() { Id = 1742, Name = "Signet of Ghostly Might", Profession = Profession.Ritualist };
    public static readonly Skill Anguish = new() { Id = 1745, Name = "Anguish", Profession = Profession.Ritualist };
    public static readonly Skill SunderingWeapon = new() { Id = 2148, Name = "Sundering Weapon", Profession = Profession.Ritualist };
    public static readonly Skill GhostlyWeapon = new() { Id = 2206, Name = "Ghostly Weapon", Profession = Profession.Ritualist };
    public static readonly Skill GenerousWasTsungrai = new() { Id = 772, Name = "Generous Was Tsungrai", Profession = Profession.Ritualist };
    public static readonly Skill ResilientWeapon = new() { Id = 787, Name = "Resilient Weapon", Profession = Profession.Ritualist };
    public static readonly Skill BlindWasMingson = new() { Id = 788, Name = "Blind Was Mingson", Profession = Profession.Ritualist };
    public static readonly Skill VengefulWasKhanhei = new() { Id = 790, Name = "Vengeful Was Khanhei", Profession = Profession.Ritualist };
    public static readonly Skill FleshofMyFlesh = new() { Id = 791, Name = "Flesh of My Flesh", Profession = Profession.Ritualist };
    public static readonly Skill WeaponofWarding = new() { Id = 793, Name = "Weapon of Warding", Profession = Profession.Ritualist };
    public static readonly Skill DefiantWasXinrae = new() { Id = 812, Name = "Defiant Was Xinrae", Profession = Profession.Ritualist };
    public static readonly Skill TranquilWasTanasen = new() { Id = 913, Name = "Tranquil Was Tanasen", Profession = Profession.Ritualist };
    public static readonly Skill SpiritLight = new() { Id = 915, Name = "Spirit Light", Profession = Profession.Ritualist };
    public static readonly Skill SpiritTransfer = new() { Id = 962, Name = "Spirit Transfer", Profession = Profession.Ritualist };
    public static readonly Skill VengefulWeapon = new() { Id = 964, Name = "Vengeful Weapon", Profession = Profession.Ritualist };
    public static readonly Skill Recuperation = new() { Id = 981, Name = "Recuperation", Profession = Profession.Ritualist };
    public static readonly Skill WeaponofShadow = new() { Id = 983, Name = "Weapon of Shadow", Profession = Profession.Ritualist };
    public static readonly Skill ProtectiveWasKaolai = new() { Id = 1219, Name = "Protective Was Kaolai", Profession = Profession.Ritualist };
    public static readonly Skill ResilientWasXiko = new() { Id = 1221, Name = "Resilient Was Xiko", Profession = Profession.Ritualist };
    public static readonly Skill LivelyWasNaomei = new() { Id = 1222, Name = "Lively Was Naomei", Profession = Profession.Ritualist };
    public static readonly Skill SoothingMemories = new() { Id = 1233, Name = "Soothing Memories", Profession = Profession.Ritualist };
    public static readonly Skill MendBodyandSoul = new() { Id = 1234, Name = "Mend Body and Soul", Profession = Profession.Ritualist };
    public static readonly Skill Preservation = new() { Id = 1250, Name = "Preservation", Profession = Profession.Ritualist };
    public static readonly Skill Life = new() { Id = 1251, Name = "Life", Profession = Profession.Ritualist };
    public static readonly Skill SpiritLightWeapon = new() { Id = 1257, Name = "Spirit Light Weapon", Profession = Profession.Ritualist };
    public static readonly Skill WieldersBoon = new() { Id = 1265, Name = "Wielder's Boon", Profession = Profession.Ritualist };
    public static readonly Skill DeathPactSignet = new() { Id = 1481, Name = "Death Pact Signet", Profession = Profession.Ritualist };
    public static readonly Skill VocalWasSogolon = new() { Id = 1731, Name = "Vocal Was Sogolon", Profession = Profession.Ritualist };
    public static readonly Skill GhostmirrorLight = new() { Id = 1741, Name = "Ghostmirror Light", Profession = Profession.Ritualist };
    public static readonly Skill Recovery = new() { Id = 1748, Name = "Recovery", Profession = Profession.Ritualist };
    public static readonly Skill XinraesWeapon = new() { Id = 1750, Name = "Xinrae's Weapon", Profession = Profession.Ritualist };
    public static readonly Skill WeaponofRemedy = new() { Id = 1752, Name = "Weapon of Remedy", Profession = Profession.Ritualist };
    public static readonly Skill PureWasLiMing = new() { Id = 2072, Name = "Pure Was Li Ming", Profession = Profession.Ritualist };
    public static readonly Skill MendingGrip = new() { Id = 2202, Name = "Mending Grip", Profession = Profession.Ritualist };
    public static readonly Skill SpiritleechAura = new() { Id = 2203, Name = "Spiritleech Aura", Profession = Profession.Ritualist };
    public static readonly Skill Rejuvenation = new() { Id = 2204, Name = "Rejuvenation", Profession = Profession.Ritualist };
    public static readonly Skill ConsumeSoul = new() { Id = 914, Name = "Consume Soul", Profession = Profession.Ritualist };
    public static readonly Skill RuptureSoul = new() { Id = 917, Name = "Rupture Soul", Profession = Profession.Ritualist };
    public static readonly Skill SpirittoFlesh = new() { Id = 918, Name = "Spirit to Flesh", Profession = Profession.Ritualist };
    public static readonly Skill FeastofSouls = new() { Id = 980, Name = "Feast of Souls", Profession = Profession.Ritualist };
    public static readonly Skill RitualLord = new() { Id = 1217, Name = "Ritual Lord", Profession = Profession.Ritualist };
    public static readonly Skill AttunedWasSongkai = new() { Id = 1220, Name = "Attuned Was Songkai", Profession = Profession.Ritualist };
    public static readonly Skill AnguishedWasLingwah = new() { Id = 1223, Name = "Anguished Was Lingwah", Profession = Profession.Ritualist };
    public static readonly Skill ExplosiveGrowth = new() { Id = 1229, Name = "Explosive Growth", Profession = Profession.Ritualist };
    public static readonly Skill BoonofCreation = new() { Id = 1230, Name = "Boon of Creation", Profession = Profession.Ritualist };
    public static readonly Skill SpiritChanneling = new() { Id = 1231, Name = "Spirit Channeling", Profession = Profession.Ritualist };
    public static readonly Skill SignetofCreation = new() { Id = 1238, Name = "Signet of Creation", Profession = Profession.Ritualist };
    public static readonly Skill SoulTwisting = new() { Id = 1240, Name = "Soul Twisting", Profession = Profession.Ritualist };
    public static readonly Skill GhostlyHaste = new() { Id = 1244, Name = "Ghostly Haste", Profession = Profession.Ritualist };
    public static readonly Skill Doom = new() { Id = 1264, Name = "Doom", Profession = Profession.Ritualist };
    public static readonly Skill SpiritsGift = new() { Id = 1480, Name = "Spirit's Gift", Profession = Profession.Ritualist };
    public static readonly Skill ReclaimEssence = new() { Id = 1482, Name = "Reclaim Essence", Profession = Profession.Ritualist };
    public static readonly Skill SpiritsStrength = new() { Id = 1736, Name = "Spirit's Strength", Profession = Profession.Ritualist };
    public static readonly Skill WieldersZeal = new() { Id = 1737, Name = "Wielder's Zeal", Profession = Profession.Ritualist };
    public static readonly Skill SightBeyondSight = new() { Id = 1738, Name = "Sight Beyond Sight", Profession = Profession.Ritualist };
    public static readonly Skill RenewingMemories = new() { Id = 1739, Name = "Renewing Memories", Profession = Profession.Ritualist };
    public static readonly Skill WieldersRemedy = new() { Id = 1740, Name = "Wielder's Remedy", Profession = Profession.Ritualist };
    public static readonly Skill SignetofBinding = new() { Id = 1743, Name = "Signet of Binding", Profession = Profession.Ritualist };
    public static readonly Skill Empowerment = new() { Id = 1747, Name = "Empowerment", Profession = Profession.Ritualist };
    public static readonly Skill EnergeticWasLeeSa = new() { Id = 2016, Name = "Energetic Was Lee Sa", Profession = Profession.Ritualist };
    public static readonly Skill WeaponofRenewal = new() { Id = 2149, Name = "Weapon of Renewal", Profession = Profession.Ritualist };
    public static readonly Skill DrawSpirit = new() { Id = 1224, Name = "Draw Spirit", Profession = Profession.Ritualist };
    public static readonly Skill CripplingAnthem = new() { Id = 1554, Name = "Crippling Anthem", Profession = Profession.Paragon };
    public static readonly Skill Godspeed = new() { Id = 1556, Name = "Godspeed", Profession = Profession.Paragon };
    public static readonly Skill GofortheEyes = new() { Id = 1558, Name = "\"Go for the Eyes!\"", Profession = Profession.Paragon };
    public static readonly Skill AnthemofEnvy = new() { Id = 1559, Name = "Anthem of Envy", Profession = Profession.Paragon };
    public static readonly Skill AnthemofGuidance = new() { Id = 1568, Name = "Anthem of Guidance", Profession = Profession.Paragon };
    public static readonly Skill BraceYourself = new() { Id = 1572, Name = "\"Brace Yourself!\"", Profession = Profession.Paragon };
    public static readonly Skill BladeturnRefrain = new() { Id = 1580, Name = "Bladeturn Refrain", Profession = Profession.Paragon };
    public static readonly Skill StandYourGround = new() { Id = 1589, Name = "\"Stand Your Ground!\"", Profession = Profession.Paragon };
    public static readonly Skill MakeHaste = new() { Id = 1591, Name = "\"Make Haste!\"", Profession = Profession.Paragon };
    public static readonly Skill WeShallReturn = new() { Id = 1592, Name = "\"We Shall Return!\"", Profession = Profession.Paragon };
    public static readonly Skill NeverGiveUp = new() { Id = 1593, Name = "\"Never Give Up!\"", Profession = Profession.Paragon };
    public static readonly Skill HelpMe = new() { Id = 1594, Name = "\"Help Me!\"", Profession = Profession.Paragon };
    public static readonly Skill FallBack = new() { Id = 1595, Name = "\"Fall Back!\"", Profession = Profession.Paragon };
    public static readonly Skill Incoming = new() { Id = 1596, Name = "\"Incoming!\"", Profession = Profession.Paragon };
    public static readonly Skill NeverSurrender = new() { Id = 1598, Name = "\"Never Surrender!\"", Profession = Profession.Paragon };
    public static readonly Skill CantTouchThis = new() { Id = 1780, Name = "\"Can't Touch This!\"", Profession = Profession.Paragon };
    public static readonly Skill FindTheirWeakness = new() { Id = 1781, Name = "\"Find Their Weakness!\"", Profession = Profession.Paragon };
    public static readonly Skill AnthemofWeariness = new() { Id = 2017, Name = "Anthem of Weariness", Profession = Profession.Paragon };
    public static readonly Skill AnthemofDisruption = new() { Id = 2018, Name = "Anthem of Disruption", Profession = Profession.Paragon };
    public static readonly Skill AnthemofFury = new() { Id = 1553, Name = "Anthem of Fury", Profession = Profession.Paragon };
    public static readonly Skill DefensiveAnthem = new() { Id = 1555, Name = "Defensive Anthem", Profession = Profession.Paragon };
    public static readonly Skill AnthemofFlame = new() { Id = 1557, Name = "Anthem of Flame", Profession = Profession.Paragon };
    public static readonly Skill Awe = new() { Id = 1573, Name = "Awe", Profession = Profession.Paragon };
    public static readonly Skill EnduringHarmony = new() { Id = 1574, Name = "Enduring Harmony", Profession = Profession.Paragon };
    public static readonly Skill BlazingFinale = new() { Id = 1575, Name = "Blazing Finale", Profession = Profession.Paragon };
    public static readonly Skill BurningRefrain = new() { Id = 1576, Name = "Burning Refrain", Profession = Profession.Paragon };
    public static readonly Skill GlowingSignet = new() { Id = 1581, Name = "Glowing Signet", Profession = Profession.Paragon };
    public static readonly Skill LeadersComfort = new() { Id = 1584, Name = "Leader's Comfort", Profession = Profession.Paragon };
    public static readonly Skill AngelicProtection = new() { Id = 1586, Name = "Angelic Protection", Profession = Profession.Paragon };
    public static readonly Skill AngelicBond = new() { Id = 1587, Name = "Angelic Bond", Profession = Profession.Paragon };
    public static readonly Skill LeadtheWay = new() { Id = 1590, Name = "\"Lead the Way!\"", Profession = Profession.Paragon };
    public static readonly Skill TheyreonFire = new() { Id = 1597, Name = "\"They're on Fire!\"", Profession = Profession.Paragon };
    public static readonly Skill FocusedAnger = new() { Id = 1769, Name = "Focused Anger", Profession = Profession.Paragon };
    public static readonly Skill NaturalTemper = new() { Id = 1770, Name = "Natural Temper", Profession = Profession.Paragon };
    public static readonly Skill SoldiersFury = new() { Id = 1773, Name = "Soldier's Fury", Profession = Profession.Paragon };
    public static readonly Skill AggressiveRefrain = new() { Id = 1774, Name = "Aggressive Refrain", Profession = Profession.Paragon };
    public static readonly Skill SignetofReturn = new() { Id = 1778, Name = "Signet of Return", Profession = Profession.Paragon };
    public static readonly Skill MakeYourTime = new() { Id = 1779, Name = "\"Make Your Time!\"", Profession = Profession.Paragon };
    public static readonly Skill HastyRefrain = new() { Id = 2075, Name = "Hasty Refrain", Profession = Profession.Paragon };
    public static readonly Skill BurningShield = new() { Id = 2208, Name = "Burning Shield", Profession = Profession.Paragon };
    public static readonly Skill SpearSwipe = new() { Id = 2210, Name = "Spear Swipe", Profession = Profession.Paragon };
    public static readonly Skill SongofPower = new() { Id = 1560, Name = "Song of Power", Profession = Profession.Paragon };
    public static readonly Skill ZealousAnthem = new() { Id = 1561, Name = "Zealous Anthem", Profession = Profession.Paragon };
    public static readonly Skill AriaofZeal = new() { Id = 1562, Name = "Aria of Zeal", Profession = Profession.Paragon };
    public static readonly Skill LyricofZeal = new() { Id = 1563, Name = "Lyric of Zeal", Profession = Profession.Paragon };
    public static readonly Skill BalladofRestoration = new() { Id = 1564, Name = "Ballad of Restoration", Profession = Profession.Paragon };
    public static readonly Skill ChorusofRestoration = new() { Id = 1565, Name = "Chorus of Restoration", Profession = Profession.Paragon };
    public static readonly Skill AriaofRestoration = new() { Id = 1566, Name = "Aria of Restoration", Profession = Profession.Paragon };
    public static readonly Skill EnergizingChorus = new() { Id = 1569, Name = "Energizing Chorus", Profession = Profession.Paragon };
    public static readonly Skill SongofPurification = new() { Id = 1570, Name = "Song of Purification", Profession = Profession.Paragon };
    public static readonly Skill FinaleofRestoration = new() { Id = 1577, Name = "Finale of Restoration", Profession = Profession.Paragon };
    public static readonly Skill MendingRefrain = new() { Id = 1578, Name = "Mending Refrain", Profession = Profession.Paragon };
    public static readonly Skill PurifyingFinale = new() { Id = 1579, Name = "Purifying Finale", Profession = Profession.Paragon };
    public static readonly Skill LeadersZeal = new() { Id = 1583, Name = "Leader's Zeal", Profession = Profession.Paragon };
    public static readonly Skill SignetofSynergy = new() { Id = 1585, Name = "Signet of Synergy", Profession = Profession.Paragon };
    public static readonly Skill ItsJustaFleshWound = new() { Id = 1599, Name = "\"It's Just a Flesh Wound.\"", Profession = Profession.Paragon };
    public static readonly Skill SongofRestoration = new() { Id = 1771, Name = "Song of Restoration", Profession = Profession.Paragon };
    public static readonly Skill LyricofPurification = new() { Id = 1772, Name = "Lyric of Purification", Profession = Profession.Paragon };
    public static readonly Skill EnergizingFinale = new() { Id = 1775, Name = "Energizing Finale", Profession = Profession.Paragon };
    public static readonly Skill ThePowerIsYours = new() { Id = 1782, Name = "\"The Power Is Yours!\"", Profession = Profession.Paragon };
    public static readonly Skill InspirationalSpeech = new() { Id = 2207, Name = "Inspirational Speech", Profession = Profession.Paragon };
    public static readonly Skill BlazingSpear = new() { Id = 1546, Name = "Blazing Spear", Profession = Profession.Paragon };
    public static readonly Skill MightyThrow = new() { Id = 1547, Name = "Mighty Throw", Profession = Profession.Paragon };
    public static readonly Skill CruelSpear = new() { Id = 1548, Name = "Cruel Spear", Profession = Profession.Paragon };
    public static readonly Skill HarriersToss = new() { Id = 1549, Name = "Harrier's Toss", Profession = Profession.Paragon };
    public static readonly Skill UnblockableThrow = new() { Id = 1550, Name = "Unblockable Throw", Profession = Profession.Paragon };
    public static readonly Skill SpearofLightning = new() { Id = 1551, Name = "Spear of Lightning", Profession = Profession.Paragon };
    public static readonly Skill WearyingSpear = new() { Id = 1552, Name = "Wearying Spear", Profession = Profession.Paragon };
    public static readonly Skill BarbedSpear = new() { Id = 1600, Name = "Barbed Spear", Profession = Profession.Paragon };
    public static readonly Skill ViciousAttack = new() { Id = 1601, Name = "Vicious Attack", Profession = Profession.Paragon };
    public static readonly Skill StunningStrike = new() { Id = 1602, Name = "Stunning Strike", Profession = Profession.Paragon };
    public static readonly Skill MercilessSpear = new() { Id = 1603, Name = "Merciless Spear", Profession = Profession.Paragon };
    public static readonly Skill DisruptingThrow = new() { Id = 1604, Name = "Disrupting Throw", Profession = Profession.Paragon };
    public static readonly Skill WildThrow = new() { Id = 1605, Name = "Wild Throw", Profession = Profession.Paragon };
    public static readonly Skill SlayersSpear = new() { Id = 1783, Name = "Slayer's Spear", Profession = Profession.Paragon };
    public static readonly Skill SwiftJavelin = new() { Id = 1784, Name = "Swift Javelin", Profession = Profession.Paragon };
    public static readonly Skill ChestThumper = new() { Id = 2074, Name = "Chest Thumper", Profession = Profession.Paragon };
    public static readonly Skill MaimingSpear = new() { Id = 2150, Name = "Maiming Spear", Profession = Profession.Paragon };
    public static readonly Skill HolySpear = new() { Id = 2209, Name = "Holy Spear", Profession = Profession.Paragon };
    public static readonly Skill SpearofRedemption = new() { Id = 2238, Name = "Spear of Redemption", Profession = Profession.Paragon };
    public static readonly Skill SongofConcentration = new() { Id = 1567, Name = "Song of Concentration", Profession = Profession.Paragon };
    public static readonly Skill HexbreakerAria = new() { Id = 1571, Name = "Hexbreaker Aria", Profession = Profession.Paragon };
    public static readonly Skill CauterySignet = new() { Id = 1588, Name = "Cautery Signet", Profession = Profession.Paragon };
    public static readonly Skill SignetofAggression = new() { Id = 1776, Name = "Signet of Aggression", Profession = Profession.Paragon };
    public static readonly Skill RemedySignet = new() { Id = 1777, Name = "Remedy Signet", Profession = Profession.Paragon };
    public static readonly Skill AuraofThorns = new() { Id = 1495, Name = "Aura of Thorns", Profession = Profession.Dervish };
    public static readonly Skill DustCloak = new() { Id = 1497, Name = "Dust Cloak", Profession = Profession.Dervish };
    public static readonly Skill StaggeringForce = new() { Id = 1498, Name = "Staggering Force", Profession = Profession.Dervish };
    public static readonly Skill MirageCloak = new() { Id = 1500, Name = "Mirage Cloak", Profession = Profession.Dervish };
    public static readonly Skill VitalBoon = new() { Id = 1506, Name = "Vital Boon", Profession = Profession.Dervish };
    public static readonly Skill SandShards = new() { Id = 1510, Name = "Sand Shards", Profession = Profession.Dervish };
    public static readonly Skill FleetingStability = new() { Id = 1514, Name = "Fleeting Stability", Profession = Profession.Dervish };
    public static readonly Skill ArmorofSanctity = new() { Id = 1515, Name = "Armor of Sanctity", Profession = Profession.Dervish };
    public static readonly Skill MysticRegeneration = new() { Id = 1516, Name = "Mystic Regeneration", Profession = Profession.Dervish };
    public static readonly Skill SignetofPiousLight = new() { Id = 1530, Name = "Signet of Pious Light", Profession = Profession.Dervish };
    public static readonly Skill MysticSandstorm = new() { Id = 1532, Name = "Mystic Sandstorm", Profession = Profession.Dervish };
    public static readonly Skill Conviction = new() { Id = 1540, Name = "Conviction", Profession = Profession.Dervish };
    public static readonly Skill PiousConcentration = new() { Id = 1542, Name = "Pious Concentration", Profession = Profession.Dervish };
    public static readonly Skill VeilofThorns = new() { Id = 1757, Name = "Veil of Thorns", Profession = Profession.Dervish };
    public static readonly Skill VowofStrength = new() { Id = 1759, Name = "Vow of Strength", Profession = Profession.Dervish };
    public static readonly Skill EbonDustAura = new() { Id = 1760, Name = "Ebon Dust Aura", Profession = Profession.Dervish };
    public static readonly Skill ShieldofForce = new() { Id = 2201, Name = "Shield of Force", Profession = Profession.Dervish };
    public static readonly Skill BanishingStrike = new() { Id = 1483, Name = "Banishing Strike", Profession = Profession.Dervish };
    public static readonly Skill MysticSweep = new() { Id = 1484, Name = "Mystic Sweep", Profession = Profession.Dervish };
    public static readonly Skill BalthazarsRage = new() { Id = 1496, Name = "Balthazar's Rage", Profession = Profession.Dervish };
    public static readonly Skill PiousRenewal = new() { Id = 1499, Name = "Pious Renewal", Profession = Profession.Dervish };
    public static readonly Skill ArcaneZeal = new() { Id = 1502, Name = "Arcane Zeal", Profession = Profession.Dervish };
    public static readonly Skill MysticVigor = new() { Id = 1503, Name = "Mystic Vigor", Profession = Profession.Dervish };
    public static readonly Skill WatchfulIntervention = new() { Id = 1504, Name = "Watchful Intervention", Profession = Profession.Dervish };
    public static readonly Skill HeartofHolyFlame = new() { Id = 1507, Name = "Heart of Holy Flame", Profession = Profession.Dervish };
    public static readonly Skill ExtendEnchantments = new() { Id = 1508, Name = "Extend Enchantments", Profession = Profession.Dervish };
    public static readonly Skill FaithfulIntervention = new() { Id = 1509, Name = "Faithful Intervention", Profession = Profession.Dervish };
    public static readonly Skill VowofSilence = new() { Id = 1517, Name = "Vow of Silence", Profession = Profession.Dervish };
    public static readonly Skill AvatarofBalthazar = new() { Id = 1518, Name = "Avatar of Balthazar", Profession = Profession.Dervish };
    public static readonly Skill AvatarofDwayna = new() { Id = 1519, Name = "Avatar of Dwayna", Profession = Profession.Dervish };
    public static readonly Skill AvatarofGrenth = new() { Id = 1520, Name = "Avatar of Grenth", Profession = Profession.Dervish };
    public static readonly Skill AvatarofLyssa = new() { Id = 1521, Name = "Avatar of Lyssa", Profession = Profession.Dervish };
    public static readonly Skill AvatarofMelandru = new() { Id = 1522, Name = "Avatar of Melandru", Profession = Profession.Dervish };
    public static readonly Skill Meditation = new() { Id = 1523, Name = "Meditation", Profession = Profession.Dervish };
    public static readonly Skill EremitesZeal = new() { Id = 1524, Name = "Eremite's Zeal", Profession = Profession.Dervish };
    public static readonly Skill ImbueHealth = new() { Id = 1526, Name = "Imbue Health", Profession = Profession.Dervish };
    public static readonly Skill IntimidatingAura = new() { Id = 1531, Name = "Intimidating Aura", Profession = Profession.Dervish };
    public static readonly Skill RendingTouch = new() { Id = 1534, Name = "Rending Touch", Profession = Profession.Dervish };
    public static readonly Skill PiousHaste = new() { Id = 1543, Name = "Pious Haste", Profession = Profession.Dervish };
    public static readonly Skill MysticCorruption = new() { Id = 1755, Name = "Mystic Corruption", Profession = Profession.Dervish };
    public static readonly Skill HeartofFury = new() { Id = 1762, Name = "Heart of Fury", Profession = Profession.Dervish };
    public static readonly Skill ZealousRenewal = new() { Id = 1763, Name = "Zealous Renewal", Profession = Profession.Dervish };
    public static readonly Skill AuraSlicer = new() { Id = 2070, Name = "Aura Slicer", Profession = Profession.Dervish };
    public static readonly Skill PiousFury = new() { Id = 2146, Name = "Pious Fury", Profession = Profession.Dervish };
    public static readonly Skill EremitesAttack = new() { Id = 1485, Name = "Eremite's Attack", Profession = Profession.Dervish };
    public static readonly Skill ReapImpurities = new() { Id = 1486, Name = "Reap Impurities", Profession = Profession.Dervish };
    public static readonly Skill TwinMoonSweep = new() { Id = 1487, Name = "Twin Moon Sweep", Profession = Profession.Dervish };
    public static readonly Skill VictoriousSweep = new() { Id = 1488, Name = "Victorious Sweep", Profession = Profession.Dervish };
    public static readonly Skill IrresistibleSweep = new() { Id = 1489, Name = "Irresistible Sweep", Profession = Profession.Dervish };
    public static readonly Skill PiousAssault = new() { Id = 1490, Name = "Pious Assault", Profession = Profession.Dervish };
    public static readonly Skill CripplingSweep = new() { Id = 1535, Name = "Crippling Sweep", Profession = Profession.Dervish };
    public static readonly Skill WoundingStrike = new() { Id = 1536, Name = "Wounding Strike", Profession = Profession.Dervish };
    public static readonly Skill WearyingStrike = new() { Id = 1537, Name = "Wearying Strike", Profession = Profession.Dervish };
    public static readonly Skill LyssasAssault = new() { Id = 1538, Name = "Lyssa's Assault", Profession = Profession.Dervish };
    public static readonly Skill ChillingVictory = new() { Id = 1539, Name = "Chilling Victory", Profession = Profession.Dervish };
    public static readonly Skill RendingSweep = new() { Id = 1753, Name = "Rending Sweep", Profession = Profession.Dervish };
    public static readonly Skill ReapersSweep = new() { Id = 1767, Name = "Reaper's Sweep", Profession = Profession.Dervish };
    public static readonly Skill RadiantScythe = new() { Id = 2012, Name = "Radiant Scythe", Profession = Profession.Dervish };
    public static readonly Skill FarmersScythe = new() { Id = 2015, Name = "Farmer's Scythe", Profession = Profession.Dervish };
    public static readonly Skill ZealousSweep = new() { Id = 2071, Name = "Zealous Sweep", Profession = Profession.Dervish };
    public static readonly Skill CripplingVictory = new() { Id = 2147, Name = "Crippling Victory", Profession = Profession.Dervish };
    public static readonly Skill MysticTwister = new() { Id = 1491, Name = "Mystic Twister", Profession = Profession.Dervish };
    public static readonly Skill GrenthsFingers = new() { Id = 1493, Name = "Grenth's Fingers", Profession = Profession.Dervish };
    public static readonly Skill VowofPiety = new() { Id = 1505, Name = "Vow of Piety", Profession = Profession.Dervish };
    public static readonly Skill LyssasHaste = new() { Id = 1512, Name = "Lyssa's Haste", Profession = Profession.Dervish };
    public static readonly Skill GuidingHands = new() { Id = 1513, Name = "Guiding Hands", Profession = Profession.Dervish };
    public static readonly Skill NaturalHealing = new() { Id = 1525, Name = "Natural Healing", Profession = Profession.Dervish };
    public static readonly Skill MysticHealing = new() { Id = 1527, Name = "Mystic Healing", Profession = Profession.Dervish };
    public static readonly Skill DwaynasTouch = new() { Id = 1528, Name = "Dwayna's Touch", Profession = Profession.Dervish };
    public static readonly Skill PiousRestoration = new() { Id = 1529, Name = "Pious Restoration", Profession = Profession.Dervish };
    public static readonly Skill WindsofDisenchantment = new() { Id = 1533, Name = "Winds of Disenchantment", Profession = Profession.Dervish };
    public static readonly Skill WhirlingCharge = new() { Id = 1544, Name = "Whirling Charge", Profession = Profession.Dervish };
    public static readonly Skill TestofFaith = new() { Id = 1545, Name = "Test of Faith", Profession = Profession.Dervish };
    public static readonly Skill Onslaught = new() { Id = 1754, Name = "Onslaught", Profession = Profession.Dervish };
    public static readonly Skill GrenthsGrasp = new() { Id = 1756, Name = "Grenth's Grasp", Profession = Profession.Dervish };
    public static readonly Skill HarriersGrasp = new() { Id = 1758, Name = "Harrier's Grasp", Profession = Profession.Dervish };
    public static readonly Skill ZealousVow = new() { Id = 1761, Name = "Zealous Vow", Profession = Profession.Dervish };
    public static readonly Skill AttackersInsight = new() { Id = 1764, Name = "Attacker's Insight", Profession = Profession.Dervish };
    public static readonly Skill RendingAura = new() { Id = 1765, Name = "Rending Aura", Profession = Profession.Dervish };
    public static readonly Skill FeatherfootGrace = new() { Id = 1766, Name = "Featherfoot Grace", Profession = Profession.Dervish };
    public static readonly Skill HarriersHaste = new() { Id = 1768, Name = "Harrier's Haste", Profession = Profession.Dervish };
    public static readonly Skill GrenthsAura = new() { Id = 2013, Name = "Grenth's Aura", Profession = Profession.Dervish };
    public static readonly Skill SignetofPiousRestraint = new() { Id = 2014, Name = "Signet of Pious Restraint", Profession = Profession.Dervish };
    public static readonly Skill SignetofMysticSpeed = new() { Id = 2200, Name = "Signet of Mystic Speed", Profession = Profession.Dervish };
    public static readonly Skill EnchantedHaste = new() { Id = 1541, Name = "Enchanted Haste" , Profession = Profession.Dervish};
    public static readonly Skill EnragedSmashPvP = new() { Id = 2808, Name = "Enraged Smash (PvP)", Profession = Profession.Warrior };
    public static readonly Skill RenewingSmashPvP = new() { Id = 3143, Name = "Renewing Smash (PvP)", Profession = Profession.Warrior};
    public static readonly Skill WarriorsEndurancePvP = new() { Id = 3002, Name = "Warrior's Endurance (PvP)", Profession = Profession.Warrior };
    public static readonly Skill DefyPainPvP = new() { Id = 3204, Name = "Defy Pain (PvP)", Profession = Profession.Warrior };
    public static readonly Skill WatchYourselfPvP = new() { Id = 2858, Name = "\"Watch Yourself!\" (PvP)", Profession = Profession.Warrior };
    public static readonly Skill SoldiersStancePvP = new() { Id = 3156, Name = "Soldier's Stance (PvP)", Profession = Profession.Warrior };
    public static readonly Skill ForGreatJusticePvP = new() { Id = 2883, Name = "\"For Great Justice!\" (PvP)", Profession = Profession.Warrior };
    public static readonly Skill CallofHastePvP = new() { Id = 2657, Name = "Call of Haste (PvP)", Profession = Profession.Ranger };
    public static readonly Skill ComfortAnimalPvP = new() { Id = 3045, Name = "Comfort Animal (PvP)", Profession = Profession.Ranger };
    public static readonly Skill MelandrusAssaultPvP = new() { Id = 3047, Name = "Melandru's Assault (PvP)", Profession = Profession.Ranger };
    public static readonly Skill PredatoryBondPvP = new() { Id = 3050, Name = "Predatory Bond (PvP)", Profession = Profession.Ranger };
    public static readonly Skill EnragedLungePvP = new() { Id = 3051, Name = "Enraged Lunge (PvP)", Profession = Profession.Ranger };
    public static readonly Skill CharmAnimalCodex = new() { Id = 3068, Name = "Charm Animal (Codex)", Profession = Profession.Ranger };
    public static readonly Skill HealasOnePvP = new() { Id = 3144, Name = "Heal as One (PvP)", Profession = Profession.Ranger };
    public static readonly Skill ExpertsDexterityPvP = new() { Id = 2959, Name = "Expert's Dexterity (PvP)", Profession = Profession.Ranger };
    public static readonly Skill EscapePvP = new() { Id = 3060, Name = "Escape (PvP)", Profession = Profession.Ranger };
    public static readonly Skill LightningReflexesPvP = new() { Id = 3141, Name = "Lightning Reflexes (PvP)", Profession = Profession.Ranger };
    public static readonly Skill GlassArrowsPvP = new() { Id = 3145, Name = "Glass Arrows (PvP)", Profession = Profession.Ranger };
    public static readonly Skill PenetratingAttackPvP = new() { Id = 2861, Name = "Penetrating Attack (PvP)", Profession = Profession.Ranger };
    public static readonly Skill SunderingAttackPvP = new() { Id = 2864, Name = "Sundering Attack (PvP)", Profession = Profession.Ranger };
    public static readonly Skill SlothHuntersShotPvP = new() { Id = 2925, Name = "Sloth Hunter's Shot (PvP)", Profession = Profession.Ranger };
    public static readonly Skill ReadtheWindPvP = new() { Id = 2969, Name = "Read the Wind (PvP)", Profession = Profession.Ranger };
    public static readonly Skill KeenArrowPvP = new() { Id = 3147, Name = "Keen Arrow (PvP)", Profession = Profession.Ranger };
    public static readonly Skill UnyieldingAuraPvP = new() { Id = 2891, Name = "Unyielding Aura (PvP)", Profession = Profession.Monk };
    public static readonly Skill SmitersBoonPvP = new() { Id = 2895, Name = "Smiter's Boon (PvP)", Profession = Profession.Monk };
    public static readonly Skill LightofDeliverancePvP = new() { Id = 2871, Name = "Light of Deliverance (PvP)", Profession = Profession.Monk };
    public static readonly Skill HealPartyPvP = new() { Id = 3232, Name = "Heal Party (PvP)", Profession = Profession.Monk };
    public static readonly Skill AegisPvP = new() { Id = 2857, Name = "Aegis (PvP)", Profession = Profession.Monk };
    public static readonly Skill SpiritBondPvP = new() { Id = 2892, Name = "Spirit Bond (PvP)", Profession = Profession.Monk };
    public static readonly Skill SignetofJudgmentPvP = new() { Id = 2887, Name = "Signet of Judgment (PvP)", Profession = Profession.Monk };
    public static readonly Skill StrengthofHonorPvP = new() { Id = 2999, Name = "Strength of Honor (PvP)", Profession = Profession.Monk };
    public static readonly Skill UnholyFeastPvP = new() { Id = 3058, Name = "Unholy Feast (PvP)", Profession = Profession.Necromancer };
    public static readonly Skill SignetofAgonyPvP = new() { Id = 3059, Name = "Signet of Agony (PvP)", Profession = Profession.Necromancer };
    public static readonly Skill SpoilVictorPvP = new() { Id = 3233, Name = "Spoil Victor (PvP)", Profession = Profession.Necromancer };
    public static readonly Skill EnfeeblePvP = new() { Id = 2859, Name = "Enfeeble (PvP)", Profession = Profession.Necromancer };
    public static readonly Skill EnfeeblingBloodPvP = new() { Id = 2885, Name = "Enfeebling Blood (PvP)", Profession = Profession.Necromancer };
    public static readonly Skill DiscordPvP = new() { Id = 2863, Name = "Discord (PvP)", Profession = Profession.Necromancer };
    public static readonly Skill MasochismPvP = new() { Id = 3054, Name = "Masochism (PvP)", Profession = Profession.Necromancer };
    public static readonly Skill MindWrackPvP = new() { Id = 2734, Name = "Mind Wrack (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill EmpathyPvP = new() { Id = 3151, Name = "Empathy (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill ShatterDelusionsPvP = new() { Id = 3180, Name = "Shatter Delusions (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill UnnaturalSignetPvP = new() { Id = 3188, Name = "Unnatural Signet (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill SpiritualPainPvP = new() { Id = 3189, Name = "Spiritual Pain (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill MistrustPvP = new() { Id = 3191, Name = "Mistrust (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill EnchantersConundrumPvP = new() { Id = 3192, Name = "Enchanter's Conundrum (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill VisionsofRegretPvP = new() { Id = 3234, Name = "Visions of Regret (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill PsychicInstabilityPvP = new() { Id = 3185, Name = "Psychic Instability (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill StolenSpeedPvP = new() { Id = 3187, Name = "Stolen Speed (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill FragilityPvP = new() { Id = 2998, Name = "Fragility (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill CripplingAnguishPvP = new() { Id = 3152, Name = "Crippling Anguish (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill IllusionaryWeaponryPvP = new() { Id = 3181, Name = "Illusionary Weaponry (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill MigrainePvP = new() { Id = 3183, Name = "Migraine (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill AccumulatedPainPvP = new() { Id = 3184, Name = "Accumulated Pain (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill SharedBurdenPvP = new() { Id = 3186, Name = "Shared Burden (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill FrustrationPvP = new() { Id = 3190, Name = "Frustration (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill SignetofClumsinessPvP = new() { Id = 3193, Name = "Signet of Clumsiness (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill WanderingEyePvP = new() { Id = 3195, Name = "Wandering Eye (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill CalculatedRiskPvP = new() { Id = 3196, Name = "Calculated Risk (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill FeveredDreamsPvP = new() { Id = 3289, Name = "Fevered Dreams (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill IllusionofHastePvP = new() { Id = 3373, Name = "Illusion of Haste (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill IllusionofPainPvP = new() { Id = 3374, Name = "Illusion of Pain (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill MantraofResolvePvP = new() { Id = 3063, Name = "Mantra of Resolve (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill MantraofSignetsPvP = new() { Id = 3179, Name = "Mantra of Signets (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill MirrorofDisenchantmentPvP = new() { Id = 3194, Name = "Mirror of Disenchantment (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill WebofDisruptionPvP = new() { Id = 3386, Name = "Web of Disruption (PvP)", Profession = Profession.Mesmer };
    public static readonly Skill MindShockPvP = new() { Id = 2804, Name = "Mind Shock (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill RidetheLightningPvP = new() { Id = 2807, Name = "Ride the Lightning (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill LightningHammerPvP = new() { Id = 3396, Name = "Lightning Hammer (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill ObsidianFlamePvP = new() { Id = 2809, Name = "Obsidian Flame (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill EtherRenewalPvP = new() { Id = 2860, Name = "Ether Renewal (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill AuraofRestorationPvP = new() { Id = 3375, Name = "Aura of Restoration (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill SavannahHeatPvP = new() { Id = 3021, Name = "Savannah Heat (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill ElementalFlamePvP = new() { Id = 3397, Name = "Elemental Flame (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill MindFreezePvP = new() { Id = 2803, Name = "Mind Freeze (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill MistFormPvP = new() { Id = 2805, Name = "Mist Form (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill WardAgainstHarmPvP = new() { Id = 2806, Name = "Ward Against Harm (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill SlipperyGroundPvP = new() { Id = 3398, Name = "Slippery Ground (PvP)", Profession = Profession.Elementalist };
    public static readonly Skill AssassinsRemedyPvP = new() { Id = 2869, Name = "Assassin's Remedy (PvP)", Profession = Profession.Assassin };
    public static readonly Skill DeathBlossomPvP = new() { Id = 3061, Name = "Death Blossom (PvP)", Profession = Profession.Assassin };
    public static readonly Skill FoxFangsPvP = new() { Id = 3251, Name = "Fox Fangs (PvP)", Profession = Profession.Assassin };
    public static readonly Skill WildStrikePvP = new() { Id = 3252, Name = "Wild Strike (PvP)", Profession = Profession.Assassin };
    public static readonly Skill SignetofDeadlyCorruptionPvP = new() { Id = 3053, Name = "Signet of Deadly Corruption (PvP)", Profession = Profession.Assassin };
    public static readonly Skill ShadowFormPvP = new() { Id = 2862, Name = "Shadow Form (PvP)", Profession = Profession.Assassin };
    public static readonly Skill ShroudofDistressPvP = new() { Id = 3048, Name = "Shroud of Distress (PvP)", Profession = Profession.Assassin };
    public static readonly Skill UnseenFuryPvP = new() { Id = 3049, Name = "Unseen Fury (PvP)", Profession = Profession.Assassin };
    public static readonly Skill AncestorsRagePvP = new() { Id = 2867, Name = "Ancestors' Rage (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill SplinterWeaponPvP = new() { Id = 2868, Name = "Splinter Weapon (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill SignetofSpiritsPvP = new() { Id = 2965, Name = "Signet of Spirits (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill DestructionPvP = new() { Id = 3008, Name = "Destruction (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill BloodsongPvP = new() { Id = 3019, Name = "Bloodsong (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill GazeofFuryPvP = new() { Id = 3022, Name = "Gaze of Fury (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill AgonyPvP = new() { Id = 3038, Name = "Agony (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill DestructiveWasGlaivePvP = new() { Id = 3157, Name = "Destructive Was Glaive (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill SignetofGhostlyMightPvP = new() { Id = 2966, Name = "Signet of Ghostly Might (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill ArmorofUnfeelingPvP = new() { Id = 3003, Name = "Armor of Unfeeling (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill UnionPvP = new() { Id = 3005, Name = "Union (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill ShadowsongPvP = new() { Id = 3006, Name = "Shadowsong (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill PainPvP = new() { Id = 3007, Name = "Pain (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill SoothingPvP = new() { Id = 3009, Name = "Soothing (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill DisplacementPvP = new() { Id = 3010, Name = "Displacement (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill DissonancePvP = new() { Id = 3014, Name = "Dissonance (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill EarthbindPvP = new() { Id = 3015, Name = "Earthbind (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill ShelterPvP = new() { Id = 3016, Name = "Shelter (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill DisenchantmentPvP = new() { Id = 3017, Name = "Disenchantment (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill RestorationPvP = new() { Id = 3018, Name = "Restoration (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill WanderlustPvP = new() { Id = 3020, Name = "Wanderlust (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill AnguishPvP = new() { Id = 3023, Name = "Anguish (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill FleshofMyFleshPvP = new() { Id = 2866, Name = "Flesh of My Flesh (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill DeathPactSignetPvP = new() { Id = 2872, Name = "Death Pact Signet (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill WeaponofWardingPvP = new() { Id = 2893, Name = "Weapon of Warding (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill PreservationPvP = new() { Id = 3011, Name = "Preservation (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill LifePvP = new() { Id = 3012, Name = "Life (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill RecuperationPvP = new() { Id = 3013, Name = "Recuperation (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill RecoveryPvP = new() { Id = 3025, Name = "Recovery (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill RejuvenationPvP = new() { Id = 3039, Name = "Rejuvenation (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill EmpowermentPvP = new() { Id = 3024, Name = "Empowerment (PvP)", Profession = Profession.Ritualist };
    public static readonly Skill IncomingPvP = new() { Id = 2879, Name = "\"Incoming!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill NeverSurrenderPvP = new() { Id = 2880, Name = "\"Never Surrender!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill GofortheEyesPvP = new() { Id = 3026, Name = "\"Go for the Eyes!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill BraceYourselfPvP = new() { Id = 3027, Name = "\"Brace Yourself!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill BladeturnRefrainPvP = new() { Id = 3029, Name = "Bladeturn Refrain (PvP)", Profession = Profession.Paragon };
    public static readonly Skill CantTouchThisPvP = new() { Id = 3031, Name = "\"Can't Touch This!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill StandYourGroundPvP = new() { Id = 3032, Name = "\"Stand Your Ground!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill WeShallReturnPvP = new() { Id = 3033, Name = "\"We Shall Return!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill FindTheirWeaknessPvP = new() { Id = 3034, Name = "\"Find Their Weakness!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill NeverGiveUpPvP = new() { Id = 3035, Name = "\"Never Give Up!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill HelpMePvP = new() { Id = 3036, Name = "\"Help Me!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill FallBackPvP = new() { Id = 3037, Name = "\"Fall Back!\" (PvP)", Profession = Profession.Paragon };
    public static readonly Skill AnthemofDisruptionPvP = new() { Id = 3040, Name = "Anthem of Disruption (PvP)", Profession = Profession.Paragon };
    public static readonly Skill AnthemofEnvyPvP = new() { Id = 3148, Name = "Anthem of Envy (PvP)", Profession = Profession.Paragon };
    public static readonly Skill DefensiveAnthemPvP = new() { Id = 2876, Name = "Defensive Anthem (PvP)", Profession = Profession.Paragon };
    public static readonly Skill BlazingFinalePvP = new() { Id = 3028, Name = "Blazing Finale (PvP)", Profession = Profession.Paragon };
    public static readonly Skill SignetofReturnPvP = new() { Id = 3030, Name = "Signet of Return (PvP)", Profession = Profession.Paragon };
    public static readonly Skill BalladofRestorationPvP = new() { Id = 2877, Name = "Ballad of Restoration (PvP)", Profession = Profession.Paragon };
    public static readonly Skill SongofRestorationPvP = new() { Id = 2878, Name = "Song of Restoration (PvP)", Profession = Profession.Paragon };
    public static readonly Skill FinaleofRestorationPvP = new() { Id = 3062, Name = "Finale of Restoration (PvP)", Profession = Profession.Paragon };
    public static readonly Skill MendingRefrainPvP = new() { Id = 3149, Name = "Mending Refrain (PvP)", Profession = Profession.Paragon };
    public static readonly Skill HarriersTossPvP = new() { Id = 2875, Name = "Harrier's Toss (PvP)", Profession = Profession.Paragon };
    public static readonly Skill MysticRegenerationPvP = new() { Id = 2884, Name = "Mystic Regeneration (PvP)", Profession = Profession.Dervish };
    public static readonly Skill AuraofThornsPvP = new() { Id = 3346, Name = "Aura of Thorns (PvP)", Profession = Profession.Dervish };
    public static readonly Skill DustCloakPvP = new() { Id = 3347, Name = "Dust Cloak (PvP)", Profession = Profession.Dervish };
    public static readonly Skill BanishingStrikePvP = new() { Id = 3263, Name = "Banishing Strike (PvP)", Profession = Profession.Dervish };
    public static readonly Skill AvatarofDwaynaPvP = new() { Id = 3270, Name = "Avatar of Dwayna (PvP)", Profession = Profession.Dervish };
    public static readonly Skill AvatarofMelandruPvP = new() { Id = 3271, Name = "Avatar of Melandru (PvP)", Profession = Profession.Dervish };
    public static readonly Skill HeartofFuryPvP = new() { Id = 3366, Name = "Heart of Fury (PvP)", Profession = Profession.Dervish };
    public static readonly Skill PiousFuryPvP = new() { Id = 3368, Name = "Pious Fury (PvP)", Profession = Profession.Dervish };
    public static readonly Skill TwinMoonSweepPvP = new() { Id = 3264, Name = "Twin Moon Sweep (PvP)", Profession = Profession.Dervish };
    public static readonly Skill IrresistibleSweepPvP = new() { Id = 3265, Name = "Irresistible Sweep (PvP)", Profession = Profession.Dervish };
    public static readonly Skill PiousAssaultPvP = new() { Id = 3266, Name = "Pious Assault (PvP)", Profession = Profession.Dervish };
    public static readonly Skill WoundingStrikePvP = new() { Id = 3367, Name = "Wounding Strike (PvP)", Profession = Profession.Dervish };
    public static readonly Skill GuidingHandsPvP = new() { Id = 3269, Name = "Guiding Hands (PvP)", Profession = Profession.Dervish };
    public static readonly Skill MysticHealingPvP = new() { Id = 3272, Name = "Mystic Healing (PvP)", Profession = Profession.Dervish };
    public static readonly Skill SignetofPiousRestraintPvP = new() { Id = 3273, Name = "Signet of Pious Restraint (PvP)", Profession = Profession.Dervish };
    public static readonly Skill LyssasHastePvP = new() { Id = 3348, Name = "Lyssa's Haste (PvP)", Profession = Profession.Dervish };
    public static readonly Skill OnslaughtPvP = new() { Id = 3365, Name = "Onslaught (PvP)", Profession = Profession.Dervish };
    public static readonly Skill SunspearRebirthSignet = new() { Id = 1816, Name = "Sunspear Rebirth Signet", Profession = Profession.None };
    public static readonly Skill WhirlwindAttack = new() { Id = 2107, Name = "Whirlwind Attack", Profession = Profession.Warrior };
    public static readonly Skill NeverRampageAlone = new() { Id = 2108, Name = "Never Rampage Alone", Profession = Profession.Ranger };
    public static readonly Skill SeedofLife = new() { Id = 2105, Name = "Seed of Life", Profession = Profession.Monk };
    public static readonly Skill Necrosis = new() { Id = 2103, Name = "Necrosis", Profession = Profession.Necromancer };
    public static readonly Skill CryofPain = new() { Id = 2102, Name = "Cry of Pain", Profession = Profession.Mesmer };
    public static readonly Skill Intensity = new() { Id = 2104, Name = "Intensity", Profession = Profession.Elementalist };
    public static readonly Skill CriticalAgility = new() { Id = 2101, Name = "Critical Agility", Profession = Profession.Assassin };
    public static readonly Skill Vampirism = new() { Id = 2110, Name = "Vampirism", Profession = Profession.Ritualist };
    public static readonly Skill TheresNothingtoFear = new() { Id = 2112, Name = "\"There's Nothing to Fear!\"", Profession = Profession.Paragon };
    public static readonly Skill EternalAura = new() { Id = 2109, Name = "Eternal Aura", Profession = Profession.Dervish };
    public static readonly Skill LightbringersGaze = new() { Id = 1814, Name = "Lightbringer's Gaze", Profession = Profession.None };
    public static readonly Skill LightbringerSignet = new() { Id = 1815, Name = "Lightbringer Signet", Profession = Profession.None };
    public static readonly Skill SaveYourselves = new() { Id = 1954, Name = "\"Save Yourselves!\"", Profession = Profession.Warrior };
    public static readonly Skill TripleShot = new() { Id = 1953, Name = "Triple Shot", Profession = Profession.Ranger };
    public static readonly Skill SelflessSpirit = new() { Id = 1952, Name = "Selfless Spirit", Profession = Profession.Monk };
    public static readonly Skill SignetofCorruption = new() { Id = 1950, Name = "Signet of Corruption", Profession = Profession.Necromancer };
    public static readonly Skill EtherNightmare = new() { Id = 1949, Name = "Ether Nightmare", Profession = Profession.Mesmer };
    public static readonly Skill ElementalLord = new() { Id = 1951, Name = "Elemental Lord", Profession = Profession.Elementalist };
    public static readonly Skill ShadowSanctuary = new() { Id = 1948, Name = "Shadow Sanctuary", Profession = Profession.Assassin };
    public static readonly Skill SummonSpirits = new() { Id = 2051, Name = "Summon Spirits", Profession = Profession.Ritualist };
    public static readonly Skill SpearofFury = new() { Id = 1957, Name = "Spear of Fury", Profession = Profession.Paragon };
    public static readonly Skill AuraofHolyMight = new() { Id = 1955, Name = "Aura of Holy Might", Profession = Profession.Dervish };
    public static readonly Skill SaveYourselves2 = new() { Id = 2097, Name = "\"Save Yourselves!\"", Profession = Profession.Warrior };
    public static readonly Skill TripleShot2 = new() { Id = 2096, Name = "Triple Shot", Profession = Profession.Ranger };
    public static readonly Skill SelflessSpirit2 = new() { Id = 2095, Name = "Selfless Spirit", Profession = Profession.Monk };
    public static readonly Skill SignetofCorruption2 = new() { Id = 2093, Name = "Signet of Corruption", Profession = Profession.Necromancer };
    public static readonly Skill EtherNightmare2 = new() { Id = 2092, Name = "Ether Nightmare", Profession = Profession.Mesmer };
    public static readonly Skill ElementalLord2 = new() { Id = 2094, Name = "Elemental Lord", Profession = Profession.Elementalist };
    public static readonly Skill ShadowSanctuary2 = new() { Id = 2091, Name = "Shadow Sanctuary", Profession = Profession.Assassin };
    public static readonly Skill SummonSpirits2 = new() { Id = 2100, Name = "Summon Spirits", Profession = Profession.Ritualist };
    public static readonly Skill SpearofFury2 = new() { Id = 2099, Name = "Spear of Fury", Profession = Profession.Paragon };
    public static readonly Skill AuraofHolyMight2 = new() { Id = 2098, Name = "Aura of Holy Might", Profession = Profession.Dervish };
    public static readonly Skill AirofSuperiority = new() { Id = 2416, Name = "Air of Superiority", Profession = Profession.None };
    public static readonly Skill AsuranScan = new() { Id = 2415, Name = "Asuran Scan", Profession = Profession.None };
    public static readonly Skill MentalBlock = new() { Id = 2417, Name = "Mental Block", Profession = Profession.None };
    public static readonly Skill Mindbender = new() { Id = 2411, Name = "Mindbender", Profession = Profession.None };
    public static readonly Skill PainInverter = new() { Id = 2418, Name = "Pain Inverter", Profession = Profession.None };
    public static readonly Skill RadiationField = new() { Id = 2414, Name = "Radiation Field", Profession = Profession.None };
    public static readonly Skill SmoothCriminal = new() { Id = 2412, Name = "Smooth Criminal", Profession = Profession.None };
    public static readonly Skill SummonMursaat = new() { Id = 2224, Name = "Summon Mursaat", Profession = Profession.None };
    public static readonly Skill SummonRubyDjinn = new() { Id = 2225, Name = "Summon Ruby Djinn", Profession = Profession.None };
    public static readonly Skill SummonIceImp = new() { Id = 2226, Name = "Summon Ice Imp", Profession = Profession.None };
    public static readonly Skill SummonNagaShaman = new() { Id = 2227, Name = "Summon Naga Shaman", Profession = Profession.None };
    public static readonly Skill Technobabble = new() { Id = 2413, Name = "Technobabble", Profession = Profession.None };
    public static readonly Skill ByUralsHammer = new() { Id = 2217, Name = "\"By Ural's Hammer!\"", Profession = Profession.None };
    public static readonly Skill DontTrip = new() { Id = 2216, Name = "\"Don't Trip!\"", Profession = Profession.None };
    public static readonly Skill AlkarsAlchemicalAcid = new() { Id = 2211, Name = "Alkar's Alchemical Acid", Profession = Profession.None };
    public static readonly Skill BlackPowderMine = new() { Id = 2223, Name = "Black Powder Mine", Profession = Profession.None };
    public static readonly Skill BrawlingHeadbutt = new() { Id = 2215, Name = "Brawling Headbutt", Profession = Profession.None };
    public static readonly Skill BreathoftheGreatDwarf = new() { Id = 2221, Name = "Breath of the Great Dwarf", Profession = Profession.None };
    public static readonly Skill DrunkenMaster = new() { Id = 2218, Name = "Drunken Master", Profession = Profession.None };
    public static readonly Skill DwarvenStability = new() { Id = 2423, Name = "Dwarven Stability", Profession = Profession.None };
    public static readonly Skill EarBite = new() { Id = 2213, Name = "Ear Bite", Profession = Profession.None };
    public static readonly Skill GreatDwarfArmor = new() { Id = 2220, Name = "Great Dwarf Armor", Profession = Profession.None };
    public static readonly Skill GreatDwarfWeapon = new() { Id = 2219, Name = "Great Dwarf Weapon", Profession = Profession.None };
    public static readonly Skill LightofDeldrimor = new() { Id = 2212, Name = "Light of Deldrimor", Profession = Profession.None };
    public static readonly Skill LowBlow = new() { Id = 2214, Name = "Low Blow", Profession = Profession.None };
    public static readonly Skill SnowStorm = new() { Id = 2222, Name = "Snow Storm", Profession = Profession.None };
    public static readonly Skill DeftStrike = new() { Id = 2228, Name = "Deft Strike", Profession = Profession.None };
    public static readonly Skill EbonBattleStandardofCourage = new() { Id = 2231, Name = "Ebon Battle Standard of Courage", Profession = Profession.None };
    public static readonly Skill EbonBattleStandardofWisdom = new() { Id = 2232, Name = "Ebon Battle Standard of Wisdom", Profession = Profession.None };
    public static readonly Skill EbonBattleStandardofHonor = new() { Id = 2233, Name = "Ebon Battle Standard of Honor", Profession = Profession.None };
    public static readonly Skill EbonEscape = new() { Id = 2420, Name = "Ebon Escape", Profession = Profession.None };
    public static readonly Skill EbonVanguardAssassinSupport = new() { Id = 2235, Name = "Ebon Vanguard Assassin Support", Profession = Profession.None };
    public static readonly Skill EbonVanguardSniperSupport = new() { Id = 2234, Name = "Ebon Vanguard Sniper Support", Profession = Profession.None };
    public static readonly Skill SignetofInfection = new() { Id = 2229, Name = "Signet of Infection", Profession = Profession.None };
    public static readonly Skill SneakAttack = new() { Id = 2116, Name = "Sneak Attack", Profession = Profession.None };
    public static readonly Skill TryptophanSignet = new() { Id = 2230, Name = "Tryptophan Signet", Profession = Profession.None };
    public static readonly Skill WeaknessTrap = new() { Id = 2421, Name = "Weakness Trap", Profession = Profession.None };
    public static readonly Skill Winds = new() { Id = 2422, Name = "Winds", Profession = Profession.None };
    public static readonly Skill DodgeThis = new() { Id = 2354, Name = "\"Dodge This!\"", Profession = Profession.None };
    public static readonly Skill FinishHim = new() { Id = 2353, Name = "\"Finish Him!\"", Profession = Profession.None };
    public static readonly Skill IAmUnstoppable = new() { Id = 2356, Name = "\"I Am Unstoppable!\"", Profession = Profession.None };
    public static readonly Skill IAmtheStrongest = new() { Id = 2355, Name = "\"I Am the Strongest!\"", Profession = Profession.None };
    public static readonly Skill YouAreAllWeaklings = new() { Id = 2359, Name = "\"You Are All Weaklings!\"", Profession = Profession.None };
    public static readonly Skill YouMoveLikeaDwarf = new() { Id = 2358, Name = "\"You Move Like a Dwarf!\"", Profession = Profession.None };
    public static readonly Skill ATouchofGuile = new() { Id = 2357, Name = "A Touch of Guile", Profession = Profession.None };
    public static readonly Skill ClubofaThousandBears = new() { Id = 2361, Name = "Club of a Thousand Bears", Profession = Profession.None };
    public static readonly Skill FeelNoPain = new() { Id = 2360, Name = "Feel No Pain", Profession = Profession.None };
    public static readonly Skill RavenBlessing = new() { Id = 2384, Name = "Raven Blessing", Profession = Profession.None };
    public static readonly Skill UrsanBlessing = new() { Id = 2374, Name = "Ursan Blessing", Profession = Profession.None };
    public static readonly Skill VolfenBlessing = new() { Id = 2379, Name = "Volfen Blessing", Profession = Profession.None };
    public static readonly Skill TimeWard = new() { Id = 3422, Name = "Time Ward", Profession = Profession.Mesmer };
    public static readonly Skill SoulTaker = new() { Id = 3423, Name = "Soul Taker", Profession = Profession.Necromancer };
    public static readonly Skill OverTheLimit = new() { Id = 3424, Name = "Over The Limit", AlternativeName = "Over the Limit", Profession = Profession.Elementalist };
    public static readonly Skill JudgementStrike = new() { Id = 3425, Name = "Judgement Strike", AlternativeName = "Judgment Strike", Profession = Profession.Monk };
    public static readonly Skill SevenWeaponsStance = new() { Id = 3426, Name = "Seven Weapons Stance", Profession = Profession.Warrior };
    public static readonly Skill Togetherasone = new() { Id = 3427, Name = "\"Together as one!\"", Profession = Profession.Ranger };
    public static readonly Skill ShadowTheft = new() { Id = 3428, Name = "Shadow Theft", Profession = Profession.Assassin };
    public static readonly Skill WeaponsofThreeForges = new() { Id = 3429, Name = "Weapons of Three Forges", Profession = Profession.Ritualist };
    public static readonly Skill VowofRevolution = new() { Id = 3430, Name = "Vow of Revolution", Profession = Profession.Dervish };
    public static readonly Skill HeroicRefrain = new() { Id = 3431, Name = "Heroic Refrain", Profession = Profession.Paragon };
    public static readonly IReadOnlyCollection<Skill> Skills = [.. new List<Skill>
    {
        NoSkill,
        ResurrectionSignet,
        SignetofCapture,
        CycloneAxe,
        AxeRake,
        Cleave,
        ExecutionersStrike,
        Dismember,
        Eviscerate,
        PenetratingBlow,
        DisruptingChop,
        SwiftChop,
        AxeTwist,
        LaceratingChop,
        WhirlingAxe,
        FuriousAxe,
        TripleChop,
        PenetratingChop,
        CriticalChop,
        AgonizingChop,
        Decapitate,
        KeenChop,
        HammerBash,
        BellySmash,
        MightyBlow,
        CrushingBlow,
        CrudeSwing,
        EarthShaker,
        DevastatingHammer,
        IrresistibleBlow,
        CounterBlow,
        Backbreaker,
        HeavyBlow,
        StaggeringBlow,
        FierceBlow,
        ForcefulBlow,
        AuspiciousBlow,
        EnragedSmash,
        RenewingSmash,
        YetiSmash,
        MokeleSmash,
        OverbearingSmash,
        MagehuntersSmash,
        PulverizingSmash,
        BattleRage,
        DefyPain,
        Rush,
        PowerAttack,
        ProtectorsStrike,
        GriffonsSweep,
        BullsStrike,
        IWillAvengeYou,
        EndurePain,
        Sprint,
        DolyakSignet,
        WarriorsCunning,
        ShieldBash,
        IWillSurvive,
        BerserkerStance,
        WarriorsEndurance,
        DwarvenBattleStance,
        BullsCharge,
        Flourish,
        PrimalRage,
        SignetofStrength,
        TigerStance,
        LeviathansSweep,
        YouWillDie,
        Flail,
        ChargingStrike,
        Headbutt,
        LionsComfort,
        RageoftheNtouka,
        SignetofStamina,
        BurstofAggression,
        EnragingCharge,
        Counterattack,
        MagehunterStrike,
        Disarm,
        IMeanttoDoThat,
        BodyBlow,
        Hamstring,
        PureStrike,
        HundredBlades,
        SeverArtery,
        GalrathSlash,
        Gash,
        FinalThrust,
        SeekingBlade,
        SavageSlash,
        SunandMoonSlash,
        QuiveringBlade,
        DragonSlash,
        StandingSlash,
        JaizhenjuStrike,
        SilverwingSlash,
        CripplingSlash,
        BarbarousSlice,
        SteelfangSlash,
        KneeCutter,
        HealingSignet,
        TotheLimit,
        DesperationBlow,
        ThrillofVictory,
        DefensiveStance,
        WatchYourself,
        Charge,
        VictoryIsMine,
        FearMe,
        ShieldsUp,
        BalancedStance,
        GladiatorsDefense,
        DeflectArrows,
        DisciplinedStance,
        WaryStance,
        ShieldStance,
        BonettisDefense,
        Riposte,
        DeadlyRiposte,
        ProtectorsDefense,
        Retreat,
        NoneShallPass,
        DrunkenBlow,
        AuspiciousParry,
        Shove,
        SoldiersStrike,
        SoldiersStance,
        SoldiersDefense,
        SteadyStance,
        SoldiersSpeed,
        WildBlow,
        DistractingBlow,
        SkullCrack,
        ForGreatJustice,
        Flurry,
        Frenzy,
        Coward,
        OnYourKnees,
        YoureAllAlone,
        FrenziedDefense,
        Grapple,
        DistractingStrike,
        SymbolicStrike,
        CharmAnimal,
        CallofProtection,
        CallofHaste,
        ReviveAnimal,
        SymbioticBond,
        ComfortAnimal,
        BestialPounce,
        MaimingStrike,
        FeralLunge,
        ScavengerStrike,
        MelandrusAssault,
        FerociousStrike,
        PredatorsPounce,
        BrutalStrike,
        DisruptingLunge,
        OtyughsCry,
        TigersFury,
        EdgeofExtinction,
        FertileSeason,
        Symbiosis,
        PrimalEchoes,
        PredatorySeason,
        EnergizingWind,
        RunasOne,
        Lacerate,
        PredatoryBond,
        HealasOne,
        SavagePounce,
        EnragedLunge,
        BestialMauling,
        PoisonousBite,
        Pounce,
        BestialFury,
        VipersNest,
        StrikeasOne,
        Toxicity,
        RampageasOne,
        HeketsRampage,
        Companionship,
        FeralAggression,
        DistractingShot,
        OathShot,
        PointBlankShot,
        ThrowDirt,
        Dodge,
        MarksmansWager,
        Escape,
        PracticedStance,
        WhirlingDefense,
        LightningReflexes,
        TrappersFocus,
        ZojunsShot,
        ZojunsHaste,
        GlassArrows,
        ArchersSignet,
        TrappersSpeed,
        ExpertsDexterity,
        InfuriatingHeat,
        ExpertFocus,
        HuntersShot,
        PinDown,
        CripplingShot,
        PowerShot,
        Barrage,
        PenetratingAttack,
        PrecisionShot,
        DeterminedShot,
        DebilitatingShot,
        ConcussionShot,
        PunishingShot,
        SavageShot,
        ReadtheWind,
        FavorableWinds,
        SplinterShot,
        MelandrusShot,
        SeekingArrows,
        MaraudersShot,
        FocusedShot,
        SunderingAttack,
        NeedlingShot,
        BroadHeadArrow,
        PreparedShot,
        BurningArrow,
        ArcingShot,
        Crossfire,
        ScreamingShot,
        KeenArrow,
        DisruptingAccuracy,
        RapidFire,
        SlothHuntersShot,
        DisruptingShot,
        Volley,
        BodyShot,
        PoisonArrow,
        IncendiaryArrows,
        MelandrusArrows,
        IgniteArrows,
        KindleArrows,
        ChokingGas,
        ApplyPoison,
        TrollUnguent,
        MelandrusResilience,
        DrydersDefenses,
        StormChaser,
        SerpentsQuickness,
        DustTrap,
        BarbedTrap,
        FlameTrap,
        HealingSpring,
        SpikeTrap,
        Winter,
        Winnowing,
        GreaterConflagration,
        Conflagration,
        FrozenSoil,
        QuickeningZephyr,
        NaturesRenewal,
        MuddyTerrain,
        Snare,
        Pestilence,
        Brambles,
        Famine,
        Equinox,
        Tranquility,
        BarbedArrows,
        ScavengersFocus,
        Quicksand,
        Tripwire,
        RoaringWinds,
        NaturalStride,
        SmokeTrap,
        PiercingTrap,
        PoisonTipSignet,
        DualShot,
        QuickShot,
        CalledShot,
        AntidoteSignet,
        StormsEmbrace,
        ForkedArrow,
        MagebaneShot,
        DivineIntervention,
        WatchfulSpirit,
        BlessedAura,
        PeaceandHarmony,
        UnyieldingAura,
        SpellBreaker,
        DivineHealing,
        DivineBoon,
        SignetofDevotion,
        BlessedSignet,
        ContemplationofPurity,
        DivineSpirit,
        BoonSignet,
        BlessedLight,
        WithdrawHexes,
        SpellShield,
        ReleaseEnchantments,
        DenyHexes,
        HeavensDelight,
        WatchfulHealing,
        HealersBoon,
        ScribesInsight,
        HolyHaste,
        SmitersBoon,
        VigorousSpirit,
        HealingSeed,
        HealArea,
        OrisonofHealing,
        WordofHealing,
        DwaynasKiss,
        HealingHands,
        HealOther,
        HealParty,
        HealingBreeze,
        Mending,
        LiveVicariously,
        InfuseHealth,
        HealingTouch,
        RestoreLife,
        DwaynasSorrow,
        HealingLight,
        RestfulBreeze,
        SignetofRejuvenation,
        HealingWhisper,
        EtherealLight,
        HealingBurst,
        KareisHealingCircle,
        JameisGaze,
        GiftofHealth,
        ResurrectionChant,
        HealingRing,
        RenewLife,
        SupportiveSpirit,
        HealersCovenant,
        WordsofComfort,
        LightofDeliverance,
        GlimmerofLight,
        CureHex,
        PatientSpirit,
        HealingRibbon,
        SpotlessMind,
        SpotlessSoul,
        LifeBond,
        LifeAttunement,
        ProtectiveSpirit,
        Aegis,
        Guardian,
        ShieldofDeflection,
        AuraofFaith,
        ShieldofRegeneration,
        ProtectiveBond,
        Pacifism,
        Amity,
        MarkofProtection,
        LifeBarrier,
        MendCondition,
        RestoreCondition,
        MendAilment,
        VitalBlessing,
        ShieldingHands,
        ConvertHexes,
        Rebirth,
        ReversalofFortune,
        DrawConditions,
        ReverseHex,
        ShieldGuardian,
        Extinguish,
        SpiritBond,
        AirofEnchantment,
        LifeSheath,
        ShieldofAbsorption,
        MendingTouch,
        PensiveGuardian,
        ZealousBenediction,
        DismissCondition,
        DivertHexes,
        PurifyingVeil,
        AuraofStability,
        Smite,
        BalthazarsSpirit,
        StrengthofHonor,
        SymbolofWrath,
        Retribution,
        HolyWrath,
        ScourgeHealing,
        Banish,
        ScourgeSacrifice,
        ShieldofJudgment,
        JudgesInsight,
        ZealotsFire,
        BalthazarsAura,
        SignetofJudgment,
        BaneSignet,
        SmiteHex,
        HolyStrike,
        RayofJudgment,
        KirinsWrath,
        WordofCensure,
        SpearofLight,
        StonesoulStrike,
        SignetofRage,
        JudgesIntervention,
        BalthazarsPendulum,
        ScourgeEnchantment,
        ReversalofDamage,
        DefendersZeal,
        SignetofMysticWrath,
        SmiteCondition,
        CastigationSignet,
        EssenceBond,
        PurgeConditions,
        PurgeSignet,
        Martyr,
        RemoveHex,
        LightofDwayna,
        Resurrect,
        Succor,
        HolyVeil,
        Vengeance,
        EmpathicRemoval,
        SignetofRemoval,
        WellofPower,
        WellofBlood,
        ShadowStrike,
        LifeSiphon,
        UnholyFeast,
        AwakentheBlood,
        BloodRenewal,
        BloodisPower,
        LifeTransfer,
        MarkofSubversion,
        SoulLeech,
        DemonicFlesh,
        BarbedSignet,
        DarkPact,
        OrderofPain,
        DarkBond,
        StripEnchantment,
        SignetofAgony,
        OfferingofBlood,
        DarkFury,
        OrderoftheVampire,
        VampiricGaze,
        VampiricTouch,
        BloodRitual,
        TouchofAgony,
        JaundicedGaze,
        CultistsFervor,
        VampiricSpirit,
        BloodBond,
        RavenousGaze,
        OppressiveGaze,
        BloodoftheAggressor,
        SpoilVictor,
        LifebaneStrike,
        VampiricSwarm,
        BloodDrinker,
        VampiricBite,
        WallowsBite,
        MarkofFury,
        SignetofSuffering,
        ParasiticBond,
        SoulBarbs,
        Barbs,
        PriceofFailure,
        Suffering,
        DesecrateEnchantments,
        Enfeeble,
        EnfeeblingBlood,
        SpitefulSpirit,
        InsidiousParasite,
        SpinalShivers,
        Wither,
        DefileFlesh,
        PlagueSignet,
        Faintheartedness,
        ShadowofFear,
        RigorMortis,
        Malaise,
        RendEnchantments,
        LingeringCurse,
        Chilblains,
        PlagueSending,
        MarkofPain,
        FeastofCorruption,
        PlagueTouch,
        WeakenArmor,
        WellofWeariness,
        Depravity,
        WeakenKnees,
        RecklessHaste,
        PoisonedHeart,
        OrderofApostasy,
        VocalMinority,
        SoulBind,
        EnvenomEnchantments,
        RipEnchantment,
        DefileEnchantments,
        ShiversofDread,
        EnfeeblingTouch,
        Meekness,
        UlcerousLungs,
        PainofDisenchantment,
        CorruptEnchantment,
        WellofDarkness,
        WellofSilence,
        Cacophony,
        DefileDefenses,
        WellofRuin,
        Atrophy,
        AnimateBoneHorror,
        AnimateBoneFiend,
        AnimateBoneMinions,
        VeratasGaze,
        VeratasAura,
        DeathlyChill,
        VeratasSacrifice,
        WellofSuffering,
        WelloftheProfane,
        PutridExplosion,
        SoulFeast,
        NecroticTraversal,
        ConsumeCorpse,
        DeathNova,
        DeathlySwarm,
        RottingFlesh,
        Virulence,
        TaintedFlesh,
        AuraoftheLich,
        DarkAura,
        BloodoftheMaster,
        MalignIntervention,
        InfuseCondition,
        TasteofDeath,
        VileTouch,
        AnimateVampiricHorror,
        Discord,
        VileMiasma,
        AnimateFleshGolem,
        FetidGround,
        RisingBile,
        BitterChill,
        TasteofPain,
        AnimateShamblingHorror,
        OrderofUndeath,
        PutridFlesh,
        FeastfortheDead,
        JaggedBones,
        Contagion,
        ToxicChill,
        WitheringAura,
        PutridBile,
        WailofDoom,
        ReapersMark,
        IcyVeins,
        SignetofSorrow,
        SignetofLostSouls,
        FoulFeast,
        HexersVigor,
        Masochism,
        AngorodonsGaze,
        GrenthsBalance,
        GazeofContempt,
        PowerBlock,
        HexBreaker,
        PowerSpike,
        PowerLeak,
        Empathy,
        ShatterDelusions,
        Backfire,
        Blackout,
        Diversion,
        Ignorance,
        EnergySurge,
        EnergyBurn,
        Guilt,
        MindWrack,
        WastrelsWorry,
        Shame,
        Panic,
        CryofFrustration,
        SignetofWeariness,
        ShatterHex,
        ShatterEnchantment,
        ChaosStorm,
        ArcaneThievery,
        SignetofDisruption,
        VisionsofRegret,
        Overload,
        Complicate,
        UnnaturalSignet,
        PowerFlux,
        Mistrust,
        PsychicDistraction,
        ArcaneLarceny,
        WastrelsDemise,
        SpiritualPain,
        EnchantersConundrum,
        HexEaterVortex,
        SimpleThievery,
        PriceofPride,
        SignetofDistraction,
        PowerLock,
        Aneurysm,
        MantraofRecovery,
        KeystoneSignet,
        ArcaneLanguor,
        StolenSpeed,
        PowerReturn,
        PsychicInstability,
        PersistenceofMemory,
        SymbolsofInspiration,
        SymbolicCelerity,
        SymbolicPosture,
        Distortion,
        Fragility,
        ConjurePhantasm,
        IllusionofWeakness,
        IllusionaryWeaponry,
        SympatheticVisage,
        ArcaneConundrum,
        IllusionofHaste,
        Clumsiness,
        PhantomPain,
        EtherealBurden,
        Ineptitude,
        Migraine,
        CripplingAnguish,
        FeveredDreams,
        SoothingImages,
        ImaginedBurden,
        ConjureNightmare,
        IllusionofPain,
        ImagesofRemorse,
        SharedBurden,
        AccumulatedPain,
        AncestorsVisage,
        RecurringInsecurity,
        KitahsBurden,
        Frustration,
        SignetofIllusions,
        AirofDisenchantment,
        SignetofClumsiness,
        SumofAllFears,
        CalculatedRisk,
        ShrinkingArmor,
        WanderingEye,
        ConfusingImages,
        MantraofEarth,
        MantraofFlame,
        MantraofFrost,
        MantraofLightning,
        MantraofPersistence,
        MantraofInscriptions,
        MantraofConcentration,
        MantraofResolve,
        MantraofSignets,
        InspiredEnchantment,
        InspiredHex,
        PowerDrain,
        Channeling,
        EtherFeast,
        EtherLord,
        SpiritofFailure,
        LeechSignet,
        SignetofHumility,
        SpiritShackles,
        DrainEnchantment,
        ElementalResistance,
        PhysicalResistance,
        EnergyDrain,
        EnergyTap,
        MantraofRecall,
        PowerLeech,
        LyssasAura,
        EtherSignet,
        AuspiciousIncantation,
        RevealedEnchantment,
        RevealedHex,
        HexEaterSignet,
        Feedback,
        ExtendConditions,
        DrainDelusions,
        Tease,
        EtherPhantom,
        DischargeEnchantment,
        SignetofRecall,
        WasteNotWantNot,
        SignetofMidnight,
        ArcaneMimicry,
        Echo,
        ArcaneEcho,
        Epidemic,
        LyssasBalance,
        SignetofDisenchantment,
        ShatterStorm,
        ExpelHexes,
        Hypochondria,
        WebofDisruption,
        MirrorofDisenchantment,
        WindborneSpeed,
        Gale,
        Whirlwind,
        LightningSurge,
        BlindingFlash,
        ConjureLightning,
        LightningStrike,
        ChainLightning,
        EnervatingCharge,
        AirAttunement,
        MindShock,
        GlimmeringMark,
        Thunderclap,
        LightningOrb,
        LightningJavelin,
        Shock,
        LightningTouch,
        RidetheLightning,
        ArcLightning,
        Gust,
        LightningHammer,
        TeinaisWind,
        ShockArrow,
        BlindingSurge,
        ChillingWinds,
        LightningBolt,
        StormDjinnsHaste,
        InvokeLightning,
        GlyphofSwiftness,
        ShellShock,
        ArmorofEarth,
        KineticArmor,
        Eruption,
        MagneticAura,
        EarthAttunement,
        Earthquake,
        Stoning,
        StoneDaggers,
        GraspingEarth,
        Aftershock,
        WardAgainstElements,
        WardAgainstMelee,
        WardAgainstFoes,
        IronMist,
        CrystalWave,
        ObsidianFlesh,
        ObsidianFlame,
        ChurningEarth,
        Shockwave,
        WardofStability,
        UnsteadyGround,
        SliverArmor,
        AshBlast,
        DragonsStomp,
        TeinaisCrystals,
        StoneStriker,
        Sandstorm,
        StoneSheath,
        EbonHawk,
        StonefleshAura,
        Glowstone,
        EarthenShackles,
        WardofWeakness,
        MagneticSurge,
        ElementalAttunement,
        EtherProdigy,
        AuraofRestoration,
        EtherRenewal,
        GlyphofEnergy,
        GlyphofLesserEnergy,
        EnergyBoon,
        GlyphofRestoration,
        EtherPrism,
        MasterofMagic,
        EnergyBlast,
        IncendiaryBonds,
        ConjureFlame,
        Inferno,
        FireAttunement,
        MindBurn,
        Fireball,
        Meteor,
        FlameBurst,
        RodgortsInvocation,
        MarkofRodgort,
        Immolate,
        MeteorShower,
        Phoenix,
        Flare,
        LavaFont,
        SearingHeat,
        FireStorm,
        BurningSpeed,
        LavaArrows,
        BedofCoals,
        LiquidFlame,
        SearingFlames,
        SmolderingEmbers,
        DoubleDragon,
        TeinaisHeat,
        BreathofFire,
        StarBurst,
        GlowingGaze,
        SavannahHeat,
        FlameDjinnsHaste,
        MindBlast,
        ElementalFlame,
        GlyphofImmolation,
        Rust,
        ArmorofFrost,
        ConjureFrost,
        WaterAttunement,
        MindFreeze,
        IcePrison,
        IceSpikes,
        FrozenBurst,
        ShardStorm,
        IceSpear,
        Maelstrom,
        SwirlingAura,
        DeepFreeze,
        BlurredVision,
        MistForm,
        WaterTrident,
        ArmorofMist,
        WardAgainstHarm,
        Shatterstone,
        Steam,
        VaporBlade,
        IcyPrism,
        IcyShackles,
        TeinaisPrison,
        MirrorofIce,
        FrigidArmor,
        FreezingGust,
        WintersEmbrace,
        SlipperyGround,
        GlowingIce,
        GlyphofElementalPower,
        GlyphofConcentration,
        GlyphofSacrifice,
        GlyphofRenewal,
        SecondWind,
        GlyphofEssence,
        TwistingFangs,
        BlackLotusStrike,
        UnsuspectingStrike,
        SharpenDaggers,
        CriticalEye,
        CriticalStrike,
        CriticalDefenses,
        DarkApostasy,
        LocustsFury,
        SeepingWound,
        PalmStrike,
        MaliciousStrike,
        ShatteringAssault,
        DeadlyHaste,
        AssassinsRemedy,
        WayoftheAssassin,
        WayoftheMaster,
        DeathBlossom,
        HornsoftheOx,
        FallingSpider,
        FoxFangs,
        MoebiusStrike,
        JaggedStrike,
        DesperateStrike,
        ExhaustingAssault,
        RepeatingStrike,
        NineTailStrike,
        TempleStrike,
        GoldenPhoenixStrike,
        BladesofSteel,
        JungleStrike,
        WildStrike,
        LeapingMantisSting,
        BlackMantisThrust,
        DisruptingStab,
        GoldenLotusStrike,
        FlashingBlades,
        GoldenSkullStrike,
        BlackSpiderStrike,
        GoldenFoxStrike,
        FoxsPromise,
        LotusStrike,
        GoldenFangStrike,
        FallingLotusStrike,
        TramplingOx,
        MarkofInsecurity,
        DisruptingDagger,
        DeadlyParadox,
        EntanglingAsp,
        MarkofDeath,
        IronPalm,
        EnduringToxin,
        ShroudofSilence,
        ExposeDefenses,
        ScorpionWire,
        SiphonStrength,
        DancingDaggers,
        SignetofShadows,
        ShamefulFear,
        SiphonSpeed,
        MantisTouch,
        WayoftheEmptyPalm,
        ExpungeEnchantments,
        Impale,
        AssassinsPromise,
        CripplingDagger,
        DarkPrison,
        AuguryofDeath,
        SignetofToxicShock,
        ShadowPrison,
        VampiricAssault,
        SadistsSignet,
        ShadowFang,
        SignetofDeadlyCorruption,
        VipersDefense,
        Return,
        BeguilingHaze,
        ShadowRefuge,
        MirroredStance,
        ShadowForm,
        ShadowShroud,
        ShadowofHaste,
        WayoftheFox,
        ShadowyBurden,
        DeathsCharge,
        BlindingPowder,
        WayoftheLotus,
        Caltrops,
        WayofPerfection,
        ShroudofDistress,
        HeartofShadow,
        DarkEscape,
        UnseenFury,
        FeignedNeutrality,
        HiddenCaltrops,
        DeathsRetreat,
        SmokePowderDefense,
        AuraofDisplacement,
        Recall,
        MarkofInstability,
        SignetofMalice,
        SpiritWalk,
        Dash,
        AssaultEnchantments,
        WastrelsCollapse,
        LiftEnchantment,
        SignetofTwilight,
        ShadowWalk,
        Swap,
        ShadowMeld,
        GraspingWasKuurong,
        SplinterWeapon,
        WailingWeapon,
        NightmareWeapon,
        SpiritRift,
        Lamentation,
        SpiritBurn,
        Destruction,
        ClamorofSouls,
        CruelWasDaoshen,
        ChanneledStrike,
        SpiritBoonStrike,
        EssenceStrike,
        SpiritSiphon,
        PainfulBond,
        SignetofSpirits,
        GazefromBeyond,
        AncestorsRage,
        Bloodsong,
        RenewingSurge,
        OfferingofSpirit,
        DestructiveWasGlaive,
        WieldersStrike,
        GazeofFury,
        CaretakersCharge,
        WeaponofFury,
        WarmongersWeapon,
        WeaponofAggression,
        Agony,
        MightyWasVorizun,
        Shadowsong,
        Union,
        Dissonance,
        Disenchantment,
        Restoration,
        Shelter,
        ArmorofUnfeeling,
        DulledWeapon,
        BindingChains,
        Pain,
        Displacement,
        Earthbind,
        Wanderlust,
        BrutalWeapon,
        GuidedWeapon,
        Soothing,
        VitalWeapon,
        WeaponofQuickening,
        SignetofGhostlyMight,
        Anguish,
        SunderingWeapon,
        GhostlyWeapon,
        GenerousWasTsungrai,
        ResilientWeapon,
        BlindWasMingson,
        VengefulWasKhanhei,
        FleshofMyFlesh,
        WeaponofWarding,
        DefiantWasXinrae,
        TranquilWasTanasen,
        SpiritLight,
        SpiritTransfer,
        VengefulWeapon,
        Recuperation,
        WeaponofShadow,
        ProtectiveWasKaolai,
        ResilientWasXiko,
        LivelyWasNaomei,
        SoothingMemories,
        MendBodyandSoul,
        Preservation,
        Life,
        SpiritLightWeapon,
        WieldersBoon,
        DeathPactSignet,
        VocalWasSogolon,
        GhostmirrorLight,
        Recovery,
        XinraesWeapon,
        WeaponofRemedy,
        PureWasLiMing,
        MendingGrip,
        SpiritleechAura,
        Rejuvenation,
        ConsumeSoul,
        RuptureSoul,
        SpirittoFlesh,
        FeastofSouls,
        RitualLord,
        AttunedWasSongkai,
        AnguishedWasLingwah,
        ExplosiveGrowth,
        BoonofCreation,
        SpiritChanneling,
        SignetofCreation,
        SoulTwisting,
        GhostlyHaste,
        Doom,
        SpiritsGift,
        ReclaimEssence,
        SpiritsStrength,
        WieldersZeal,
        SightBeyondSight,
        RenewingMemories,
        WieldersRemedy,
        SignetofBinding,
        Empowerment,
        EnergeticWasLeeSa,
        WeaponofRenewal,
        DrawSpirit,
        CripplingAnthem,
        Godspeed,
        GofortheEyes,
        AnthemofEnvy,
        AnthemofGuidance,
        BraceYourself,
        BladeturnRefrain,
        StandYourGround,
        MakeHaste,
        WeShallReturn,
        NeverGiveUp,
        HelpMe,
        FallBack,
        Incoming,
        NeverSurrender,
        CantTouchThis,
        FindTheirWeakness,
        AnthemofWeariness,
        AnthemofDisruption,
        AnthemofFury,
        DefensiveAnthem,
        AnthemofFlame,
        Awe,
        EnduringHarmony,
        BlazingFinale,
        BurningRefrain,
        GlowingSignet,
        LeadersComfort,
        AngelicProtection,
        AngelicBond,
        LeadtheWay,
        TheyreonFire,
        FocusedAnger,
        NaturalTemper,
        SoldiersFury,
        AggressiveRefrain,
        SignetofReturn,
        MakeYourTime,
        HastyRefrain,
        BurningShield,
        SpearSwipe,
        SongofPower,
        ZealousAnthem,
        AriaofZeal,
        LyricofZeal,
        BalladofRestoration,
        ChorusofRestoration,
        AriaofRestoration,
        EnergizingChorus,
        SongofPurification,
        FinaleofRestoration,
        MendingRefrain,
        PurifyingFinale,
        LeadersZeal,
        SignetofSynergy,
        ItsJustaFleshWound,
        SongofRestoration,
        LyricofPurification,
        EnergizingFinale,
        ThePowerIsYours,
        InspirationalSpeech,
        BlazingSpear,
        MightyThrow,
        CruelSpear,
        HarriersToss,
        UnblockableThrow,
        SpearofLightning,
        WearyingSpear,
        BarbedSpear,
        ViciousAttack,
        StunningStrike,
        MercilessSpear,
        DisruptingThrow,
        WildThrow,
        SlayersSpear,
        SwiftJavelin,
        ChestThumper,
        MaimingSpear,
        HolySpear,
        SpearofRedemption,
        SongofConcentration,
        HexbreakerAria,
        CauterySignet,
        SignetofAggression,
        RemedySignet,
        AuraofThorns,
        DustCloak,
        StaggeringForce,
        MirageCloak,
        VitalBoon,
        SandShards,
        FleetingStability,
        ArmorofSanctity,
        MysticRegeneration,
        SignetofPiousLight,
        MysticSandstorm,
        Conviction,
        PiousConcentration,
        VeilofThorns,
        VowofStrength,
        EbonDustAura,
        ShieldofForce,
        BanishingStrike,
        MysticSweep,
        BalthazarsRage,
        PiousRenewal,
        ArcaneZeal,
        MysticVigor,
        WatchfulIntervention,
        HeartofHolyFlame,
        ExtendEnchantments,
        FaithfulIntervention,
        VowofSilence,
        AvatarofBalthazar,
        AvatarofDwayna,
        AvatarofGrenth,
        AvatarofLyssa,
        AvatarofMelandru,
        Meditation,
        EremitesZeal,
        ImbueHealth,
        IntimidatingAura,
        RendingTouch,
        PiousHaste,
        MysticCorruption,
        HeartofFury,
        ZealousRenewal,
        AuraSlicer,
        PiousFury,
        EremitesAttack,
        ReapImpurities,
        TwinMoonSweep,
        VictoriousSweep,
        IrresistibleSweep,
        PiousAssault,
        CripplingSweep,
        WoundingStrike,
        WearyingStrike,
        LyssasAssault,
        ChillingVictory,
        RendingSweep,
        ReapersSweep,
        RadiantScythe,
        FarmersScythe,
        ZealousSweep,
        CripplingVictory,
        MysticTwister,
        GrenthsFingers,
        VowofPiety,
        LyssasHaste,
        GuidingHands,
        NaturalHealing,
        MysticHealing,
        DwaynasTouch,
        PiousRestoration,
        WindsofDisenchantment,
        WhirlingCharge,
        TestofFaith,
        Onslaught,
        GrenthsGrasp,
        HarriersGrasp,
        ZealousVow,
        AttackersInsight,
        RendingAura,
        FeatherfootGrace,
        HarriersHaste,
        GrenthsAura,
        SignetofPiousRestraint,
        SignetofMysticSpeed,
        EnchantedHaste,
        EnragedSmashPvP,
        RenewingSmashPvP,
        WarriorsEndurancePvP,
        DefyPainPvP,
        WatchYourselfPvP,
        SoldiersStancePvP,
        ForGreatJusticePvP,
        CallofHastePvP,
        ComfortAnimalPvP,
        MelandrusAssaultPvP,
        PredatoryBondPvP,
        EnragedLungePvP,
        CharmAnimalCodex,
        HealasOnePvP,
        ExpertsDexterityPvP,
        EscapePvP,
        LightningReflexesPvP,
        GlassArrowsPvP,
        PenetratingAttackPvP,
        SunderingAttackPvP,
        SlothHuntersShotPvP,
        ReadtheWindPvP,
        KeenArrowPvP,
        UnyieldingAuraPvP,
        SmitersBoonPvP,
        LightofDeliverancePvP,
        HealPartyPvP,
        AegisPvP,
        SpiritBondPvP,
        SignetofJudgmentPvP,
        StrengthofHonorPvP,
        UnholyFeastPvP,
        SignetofAgonyPvP,
        SpoilVictorPvP,
        EnfeeblePvP,
        EnfeeblingBloodPvP,
        DiscordPvP,
        MasochismPvP,
        MindWrackPvP,
        EmpathyPvP,
        ShatterDelusionsPvP,
        UnnaturalSignetPvP,
        SpiritualPainPvP,
        MistrustPvP,
        EnchantersConundrumPvP,
        VisionsofRegretPvP,
        PsychicInstabilityPvP,
        StolenSpeedPvP,
        FragilityPvP,
        CripplingAnguishPvP,
        IllusionaryWeaponryPvP,
        MigrainePvP,
        AccumulatedPainPvP,
        SharedBurdenPvP,
        FrustrationPvP,
        SignetofClumsinessPvP,
        WanderingEyePvP,
        CalculatedRiskPvP,
        FeveredDreamsPvP,
        IllusionofHastePvP,
        IllusionofPainPvP,
        MantraofResolvePvP,
        MantraofSignetsPvP,
        MirrorofDisenchantmentPvP,
        WebofDisruptionPvP,
        MindShockPvP,
        RidetheLightningPvP,
        LightningHammerPvP,
        ObsidianFlamePvP,
        EtherRenewalPvP,
        AuraofRestorationPvP,
        SavannahHeatPvP,
        ElementalFlamePvP,
        MindFreezePvP,
        MistFormPvP,
        WardAgainstHarmPvP,
        SlipperyGroundPvP,
        AssassinsRemedyPvP,
        DeathBlossomPvP,
        FoxFangsPvP,
        WildStrikePvP,
        SignetofDeadlyCorruptionPvP,
        ShadowFormPvP,
        ShroudofDistressPvP,
        UnseenFuryPvP,
        AncestorsRagePvP,
        SplinterWeaponPvP,
        SignetofSpiritsPvP,
        DestructionPvP,
        BloodsongPvP,
        GazeofFuryPvP,
        AgonyPvP,
        DestructiveWasGlaivePvP,
        SignetofGhostlyMightPvP,
        ArmorofUnfeelingPvP,
        UnionPvP,
        ShadowsongPvP,
        PainPvP,
        SoothingPvP,
        DisplacementPvP,
        DissonancePvP,
        EarthbindPvP,
        ShelterPvP,
        DisenchantmentPvP,
        RestorationPvP,
        WanderlustPvP,
        AnguishPvP,
        FleshofMyFleshPvP,
        DeathPactSignetPvP,
        WeaponofWardingPvP,
        PreservationPvP,
        LifePvP,
        RecuperationPvP,
        RecoveryPvP,
        RejuvenationPvP,
        EmpowermentPvP,
        IncomingPvP,
        NeverSurrenderPvP,
        GofortheEyesPvP,
        BraceYourselfPvP,
        BladeturnRefrainPvP,
        CantTouchThisPvP,
        StandYourGroundPvP,
        WeShallReturnPvP,
        FindTheirWeaknessPvP,
        NeverGiveUpPvP,
        HelpMePvP,
        FallBackPvP,
        AnthemofDisruptionPvP,
        AnthemofEnvyPvP,
        DefensiveAnthemPvP,
        BlazingFinalePvP,
        SignetofReturnPvP,
        BalladofRestorationPvP,
        SongofRestorationPvP,
        FinaleofRestorationPvP,
        MendingRefrainPvP,
        HarriersTossPvP,
        MysticRegenerationPvP,
        AuraofThornsPvP,
        DustCloakPvP,
        BanishingStrikePvP,
        AvatarofDwaynaPvP,
        AvatarofMelandruPvP,
        HeartofFuryPvP,
        PiousFuryPvP,
        TwinMoonSweepPvP,
        IrresistibleSweepPvP,
        PiousAssaultPvP,
        WoundingStrikePvP,
        GuidingHandsPvP,
        MysticHealingPvP,
        SignetofPiousRestraintPvP,
        LyssasHastePvP,
        OnslaughtPvP,
        SunspearRebirthSignet,
        WhirlwindAttack,
        NeverRampageAlone,
        SeedofLife,
        Necrosis,
        CryofPain,
        Intensity,
        CriticalAgility,
        Vampirism,
        TheresNothingtoFear,
        EternalAura,
        LightbringersGaze,
        LightbringerSignet,
        SaveYourselves,
        TripleShot,
        SelflessSpirit,
        SignetofCorruption,
        EtherNightmare,
        ElementalLord,
        ShadowSanctuary,
        SummonSpirits,
        SpearofFury,
        AuraofHolyMight,
        SaveYourselves2,
        TripleShot2,
        SelflessSpirit2,
        SignetofCorruption2,
        EtherNightmare2,
        ElementalLord2,
        ShadowSanctuary2,
        SummonSpirits2,
        SpearofFury2,
        AuraofHolyMight2,
        AirofSuperiority,
        AsuranScan,
        MentalBlock,
        Mindbender,
        PainInverter,
        RadiationField,
        SmoothCriminal,
        SummonMursaat,
        SummonRubyDjinn,
        SummonIceImp,
        SummonNagaShaman,
        Technobabble,
        ByUralsHammer,
        DontTrip,
        AlkarsAlchemicalAcid,
        BlackPowderMine,
        BrawlingHeadbutt,
        BreathoftheGreatDwarf,
        DrunkenMaster,
        DwarvenStability,
        EarBite,
        GreatDwarfArmor,
        GreatDwarfWeapon,
        LightofDeldrimor,
        LowBlow,
        SnowStorm,
        DeftStrike,
        EbonBattleStandardofCourage,
        EbonBattleStandardofWisdom,
        EbonBattleStandardofHonor,
        EbonEscape,
        EbonVanguardAssassinSupport,
        EbonVanguardSniperSupport,
        SignetofInfection,
        SneakAttack,
        TryptophanSignet,
        WeaknessTrap,
        Winds,
        DodgeThis,
        FinishHim,
        IAmUnstoppable,
        IAmtheStrongest,
        YouAreAllWeaklings,
        YouMoveLikeaDwarf,
        ATouchofGuile,
        ClubofaThousandBears,
        FeelNoPain,
        RavenBlessing,
        UrsanBlessing,
        VolfenBlessing,
        TimeWard,
        SoulTaker,
        OverTheLimit,
        JudgementStrike,
        SevenWeaponsStance,
        Togetherasone,
        ShadowTheft,
        WeaponsofThreeForges,
        VowofRevolution,
        HeroicRefrain,
    }.OrderBy(s => s.Name)];

    public static bool TryParse(int id, out Skill skill)
    {
        skill = Skills.Where(skill => skill.Id == id).FirstOrDefault()!;
        if (skill is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Skill skill)
    {
        skill = Skills.Where(skill => skill.Name == name).FirstOrDefault()!;
        if (skill is null)
        {
            return false;
        }

        return true;
    }
    public static Skill Parse(int id)
    {
        if (TryParse(id, out var skill) is false)
        {
            throw new InvalidOperationException($"Could not find a skill with id {id}");
        }

        return skill;
    }
    public static Skill Parse(string name)
    {
        if (TryParse(name, out var skill) is false)
        {
            throw new InvalidOperationException($"Could not find a skill with name {name}");
        }

        return skill;
    }

    public required Profession Profession { get; init; }
    public required string Name { get; init; }
    public required int Id { get; init; }
    public string? AlternativeName { get; private set; }
    public override string ToString() => this.Name ?? this.AlternativeName ?? nameof(Skill);
    private Skill()
    {
    }
}
