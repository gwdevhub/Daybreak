using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Builds
{
    public sealed class Skill
    {
        public static Skill NoSkill { get; } = new() { Id = 0, Name = "No Skill", Profession = Profession.None };
        public static Skill ResurrectionSignet { get; } = new() { Id = 2, Name = "Resurrection Signet", Profession = Profession.None };
        public static Skill SignetofCapture { get; } = new() { Id = 3, Name = "Signet of Capture", Profession = Profession.None };
        public static Skill CycloneAxe { get; } = new() { Id = 330, Name = "Cyclone Axe", Profession = Profession.Warrior };
        public static Skill AxeRake { get; } = new() { Id = 334, Name = "Axe Rake", Profession = Profession.Warrior };
        public static Skill Cleave { get; } = new() { Id = 335, Name = "Cleave", Profession = Profession.Warrior };
        public static Skill ExecutionersStrike { get; } = new() { Id = 336, Name = "Executioner's Strike", Profession = Profession.Warrior };
        public static Skill Dismember { get; } = new() { Id = 337, Name = "Dismember", Profession = Profession.Warrior };
        public static Skill Eviscerate { get; } = new() { Id = 338, Name = "Eviscerate", Profession = Profession.Warrior };
        public static Skill PenetratingBlow { get; } = new() { Id = 339, Name = "Penetrating Blow", Profession = Profession.Warrior };
        public static Skill DisruptingChop { get; } = new() { Id = 340, Name = "Disrupting Chop", Profession = Profession.Warrior };
        public static Skill SwiftChop { get; } = new() { Id = 341, Name = "Swift Chop", Profession = Profession.Warrior };
        public static Skill AxeTwist { get; } = new() { Id = 342, Name = "Axe Twist", Profession = Profession.Warrior };
        public static Skill LaceratingChop { get; } = new() { Id = 849, Name = "Lacerating Chop", Profession = Profession.Warrior };
        public static Skill WhirlingAxe { get; } = new() { Id = 888, Name = "Whirling Axe", Profession = Profession.Warrior };
        public static Skill FuriousAxe { get; } = new() { Id = 904, Name = "Furious Axe", Profession = Profession.Warrior };
        public static Skill TripleChop { get; } = new() { Id = 992, Name = "Triple Chop", Profession = Profession.Warrior };
        public static Skill PenetratingChop { get; } = new() { Id = 1136, Name = "Penetrating Chop", Profession = Profession.Warrior };
        public static Skill CriticalChop { get; } = new() { Id = 1402, Name = "Critical Chop", Profession = Profession.Warrior };
        public static Skill AgonizingChop { get; } = new() { Id = 1403, Name = "Agonizing Chop", Profession = Profession.Warrior };
        public static Skill Decapitate { get; } = new() { Id = 1696, Name = "Decapitate", Profession = Profession.Warrior };
        public static Skill KeenChop { get; } = new() { Id = 2009, Name = "Keen Chop", Profession = Profession.Warrior };
        public static Skill HammerBash { get; } = new() { Id = 331, Name = "Hammer Bash", Profession = Profession.Warrior };
        public static Skill BellySmash { get; } = new() { Id = 350, Name = "Belly Smash", Profession = Profession.Warrior };
        public static Skill MightyBlow { get; } = new() { Id = 351, Name = "Mighty Blow", Profession = Profession.Warrior };
        public static Skill CrushingBlow { get; } = new() { Id = 352, Name = "Crushing Blow", Profession = Profession.Warrior };
        public static Skill CrudeSwing { get; } = new() { Id = 353, Name = "Crude Swing", Profession = Profession.Warrior };
        public static Skill EarthShaker { get; } = new() { Id = 354, Name = "Earth Shaker", Profession = Profession.Warrior };
        public static Skill DevastatingHammer { get; } = new() { Id = 355, Name = "Devastating Hammer", Profession = Profession.Warrior };
        public static Skill IrresistibleBlow { get; } = new() { Id = 356, Name = "Irresistible Blow", Profession = Profession.Warrior };
        public static Skill CounterBlow { get; } = new() { Id = 357, Name = "Counter Blow", Profession = Profession.Warrior };
        public static Skill Backbreaker { get; } = new() { Id = 358, Name = "Backbreaker", Profession = Profession.Warrior };
        public static Skill HeavyBlow { get; } = new() { Id = 359, Name = "Heavy Blow", Profession = Profession.Warrior };
        public static Skill StaggeringBlow { get; } = new() { Id = 360, Name = "Staggering Blow", Profession = Profession.Warrior };
        public static Skill FierceBlow { get; } = new() { Id = 850, Name = "Fierce Blow", Profession = Profession.Warrior };
        public static Skill ForcefulBlow { get; } = new() { Id = 889, Name = "Forceful Blow", Profession = Profession.Warrior };
        public static Skill AuspiciousBlow { get; } = new() { Id = 905, Name = "Auspicious Blow", Profession = Profession.Warrior };
        public static Skill EnragedSmash { get; } = new() { Id = 993, Name = "Enraged Smash", Profession = Profession.Warrior };
        public static Skill RenewingSmash { get; } = new() { Id = 994, Name = "Renewing Smash", Profession = Profession.Warrior };
        public static Skill YetiSmash { get; } = new() { Id = 1137, Name = "Yeti Smash", Profession = Profession.Warrior };
        public static Skill MokeleSmash { get; } = new() { Id = 1409, Name = "Mokele Smash", Profession = Profession.Warrior };
        public static Skill OverbearingSmash { get; } = new() { Id = 1410, Name = "Overbearing Smash", Profession = Profession.Warrior };
        public static Skill MagehuntersSmash { get; } = new() { Id = 1697, Name = "Magehunter's Smash", Profession = Profession.Warrior };
        public static Skill PulverizingSmash { get; } = new() { Id = 2008, Name = "Pulverizing Smash", Profession = Profession.Warrior };
        public static Skill BattleRage { get; } = new() { Id = 317, Name = "Battle Rage", Profession = Profession.Warrior };
        public static Skill DefyPain { get; } = new() { Id = 318, Name = "Defy Pain", Profession = Profession.Warrior };
        public static Skill Rush { get; } = new() { Id = 319, Name = "Rush", Profession = Profession.Warrior };
        public static Skill PowerAttack { get; } = new() { Id = 322, Name = "Power Attack", Profession = Profession.Warrior };
        public static Skill ProtectorsStrike { get; } = new() { Id = 326, Name = "Protector's Strike", Profession = Profession.Warrior };
        public static Skill GriffonsSweep { get; } = new() { Id = 327, Name = "Griffon's Sweep", Profession = Profession.Warrior };
        public static Skill BullsStrike { get; } = new() { Id = 332, Name = "Bull's Strike", Profession = Profession.Warrior };
        public static Skill IWillAvengeYou { get; } = new() { Id = 333, Name = "\"I Will Avenge You!\"", Profession = Profession.Warrior };
        public static Skill EndurePain { get; } = new() { Id = 347, Name = "Endure Pain", Profession = Profession.Warrior };
        public static Skill Sprint { get; } = new() { Id = 349, Name = "Sprint", Profession = Profession.Warrior };
        public static Skill DolyakSignet { get; } = new() { Id = 361, Name = "Dolyak Signet", Profession = Profession.Warrior };
        public static Skill WarriorsCunning { get; } = new() { Id = 362, Name = "Warrior's Cunning", Profession = Profession.Warrior };
        public static Skill ShieldBash { get; } = new() { Id = 363, Name = "Shield Bash", Profession = Profession.Warrior };
        public static Skill IWillSurvive { get; } = new() { Id = 368, Name = "\"I Will Survive!\"", Profession = Profession.Warrior };
        public static Skill BerserkerStance { get; } = new() { Id = 370, Name = "Berserker Stance", Profession = Profession.Warrior };
        public static Skill WarriorsEndurance { get; } = new() { Id = 374, Name = "Warrior's Endurance", Profession = Profession.Warrior };
        public static Skill DwarvenBattleStance { get; } = new() { Id = 375, Name = "Dwarven Battle Stance", Profession = Profession.Warrior };
        public static Skill BullsCharge { get; } = new() { Id = 379, Name = "Bull's Charge", Profession = Profession.Warrior };
        public static Skill Flourish { get; } = new() { Id = 389, Name = "Flourish", Profession = Profession.Warrior };
        public static Skill PrimalRage { get; } = new() { Id = 831, Name = "Primal Rage", Profession = Profession.Warrior };
        public static Skill SignetofStrength { get; } = new() { Id = 944, Name = "Signet of Strength", Profession = Profession.Warrior };
        public static Skill TigerStance { get; } = new() { Id = 995, Name = "Tiger Stance", Profession = Profession.Warrior };
        public static Skill LeviathansSweep { get; } = new() { Id = 1134, Name = "Leviathan's Sweep", Profession = Profession.Warrior };
        public static Skill YouWillDie { get; } = new() { Id = 1141, Name = "\"You Will Die!\"", Profession = Profession.Warrior };
        public static Skill Flail { get; } = new() { Id = 1404, Name = "Flail", Profession = Profession.Warrior };
        public static Skill ChargingStrike { get; } = new() { Id = 1405, Name = "Charging Strike", Profession = Profession.Warrior };
        public static Skill Headbutt { get; } = new() { Id = 1406, Name = "Headbutt", Profession = Profession.Warrior };
        public static Skill LionsComfort { get; } = new() { Id = 1407, Name = "Lion's Comfort", Profession = Profession.Warrior };
        public static Skill RageoftheNtouka { get; } = new() { Id = 1408, Name = "Rage of the Ntouka", Profession = Profession.Warrior };
        public static Skill SignetofStamina { get; } = new() { Id = 1411, Name = "Signet of Stamina", Profession = Profession.Warrior };
        public static Skill BurstofAggression { get; } = new() { Id = 1413, Name = "Burst of Aggression", Profession = Profession.Warrior };
        public static Skill EnragingCharge { get; } = new() { Id = 1414, Name = "Enraging Charge", Profession = Profession.Warrior };
        public static Skill Counterattack { get; } = new() { Id = 1693, Name = "Counterattack", Profession = Profession.Warrior };
        public static Skill MagehunterStrike { get; } = new() { Id = 1694, Name = "Magehunter Strike", Profession = Profession.Warrior };
        public static Skill Disarm { get; } = new() { Id = 2066, Name = "Disarm", Profession = Profession.Warrior };
        public static Skill IMeanttoDoThat { get; } = new() { Id = 2067, Name = "\"I Meant to Do That!\"", Profession = Profession.Warrior };
        public static Skill BodyBlow { get; } = new() { Id = 2197, Name = "Body Blow", Profession = Profession.Warrior };
        public static Skill Hamstring { get; } = new() { Id = 320, Name = "Hamstring", Profession = Profession.Warrior };
        public static Skill PureStrike { get; } = new() { Id = 328, Name = "Pure Strike", Profession = Profession.Warrior };
        public static Skill HundredBlades { get; } = new() { Id = 381, Name = "Hundred Blades", Profession = Profession.Warrior };
        public static Skill SeverArtery { get; } = new() { Id = 382, Name = "Sever Artery", Profession = Profession.Warrior };
        public static Skill GalrathSlash { get; } = new() { Id = 383, Name = "Galrath Slash", Profession = Profession.Warrior };
        public static Skill Gash { get; } = new() { Id = 384, Name = "Gash", Profession = Profession.Warrior };
        public static Skill FinalThrust { get; } = new() { Id = 385, Name = "Final Thrust", Profession = Profession.Warrior };
        public static Skill SeekingBlade { get; } = new() { Id = 386, Name = "Seeking Blade", Profession = Profession.Warrior };
        public static Skill SavageSlash { get; } = new() { Id = 390, Name = "Savage Slash", Profession = Profession.Warrior };
        public static Skill SunandMoonSlash { get; } = new() { Id = 851, Name = "Sun and Moon Slash", Profession = Profession.Warrior };
        public static Skill QuiveringBlade { get; } = new() { Id = 892, Name = "Quivering Blade", Profession = Profession.Warrior };
        public static Skill DragonSlash { get; } = new() { Id = 907, Name = "Dragon Slash", Profession = Profession.Warrior };
        public static Skill StandingSlash { get; } = new() { Id = 996, Name = "Standing Slash", Profession = Profession.Warrior };
        public static Skill JaizhenjuStrike { get; } = new() { Id = 1135, Name = "Jaizhenju Strike", Profession = Profession.Warrior };
        public static Skill SilverwingSlash { get; } = new() { Id = 1144, Name = "Silverwing Slash", Profession = Profession.Warrior };
        public static Skill CripplingSlash { get; } = new() { Id = 1415, Name = "Crippling Slash", Profession = Profession.Warrior };
        public static Skill BarbarousSlice { get; } = new() { Id = 1416, Name = "Barbarous Slice", Profession = Profession.Warrior };
        public static Skill SteelfangSlash { get; } = new() { Id = 1702, Name = "Steelfang Slash", Profession = Profession.Warrior };
        public static Skill KneeCutter { get; } = new() { Id = 2010, Name = "Knee Cutter", Profession = Profession.Warrior };
        public static Skill HealingSignet { get; } = new() { Id = 1, Name = "Healing Signet", Profession = Profession.Warrior };
        public static Skill TotheLimit { get; } = new() { Id = 316, Name = "\"To the Limit!\"", Profession = Profession.Warrior };
        public static Skill DesperationBlow { get; } = new() { Id = 323, Name = "Desperation Blow", Profession = Profession.Warrior };
        public static Skill ThrillofVictory { get; } = new() { Id = 324, Name = "Thrill of Victory", Profession = Profession.Warrior };
        public static Skill DefensiveStance { get; } = new() { Id = 345, Name = "Defensive Stance", Profession = Profession.Warrior };
        public static Skill WatchYourself { get; } = new() { Id = 348, Name = "\"Watch Yourself!\"", Profession = Profession.Warrior };
        public static Skill Charge { get; } = new() { Id = 364, Name = "\"Charge!\"", Profession = Profession.Warrior };
        public static Skill VictoryIsMine { get; } = new() { Id = 365, Name = "\"Victory Is Mine!\"", Profession = Profession.Warrior };
        public static Skill FearMe { get; } = new() { Id = 366, Name = "\"Fear Me!\"", Profession = Profession.Warrior };
        public static Skill ShieldsUp { get; } = new() { Id = 367, Name = "\"Shields Up!\"", Profession = Profession.Warrior };
        public static Skill BalancedStance { get; } = new() { Id = 371, Name = "Balanced Stance", Profession = Profession.Warrior };
        public static Skill GladiatorsDefense { get; } = new() { Id = 372, Name = "Gladiator's Defense", Profession = Profession.Warrior };
        public static Skill DeflectArrows { get; } = new() { Id = 373, Name = "Deflect Arrows", Profession = Profession.Warrior };
        public static Skill DisciplinedStance { get; } = new() { Id = 376, Name = "Disciplined Stance", Profession = Profession.Warrior };
        public static Skill WaryStance { get; } = new() { Id = 377, Name = "Wary Stance", Profession = Profession.Warrior };
        public static Skill ShieldStance { get; } = new() { Id = 378, Name = "Shield Stance", Profession = Profession.Warrior };
        public static Skill BonettisDefense { get; } = new() { Id = 380, Name = "Bonetti's Defense", Profession = Profession.Warrior };
        public static Skill Riposte { get; } = new() { Id = 387, Name = "Riposte", Profession = Profession.Warrior };
        public static Skill DeadlyRiposte { get; } = new() { Id = 388, Name = "Deadly Riposte", Profession = Profession.Warrior };
        public static Skill ProtectorsDefense { get; } = new() { Id = 810, Name = "Protector's Defense", Profession = Profession.Warrior };
        public static Skill Retreat { get; } = new() { Id = 839, Name = "\"Retreat!\"", Profession = Profession.Warrior };
        public static Skill NoneShallPass { get; } = new() { Id = 891, Name = "\"None Shall Pass!\"", Profession = Profession.Warrior };
        public static Skill DrunkenBlow { get; } = new() { Id = 1133, Name = "Drunken Blow", Profession = Profession.Warrior };
        public static Skill AuspiciousParry { get; } = new() { Id = 1142, Name = "Auspicious Parry", Profession = Profession.Warrior };
        public static Skill Shove { get; } = new() { Id = 1146, Name = "Shove", Profession = Profession.Warrior };
        public static Skill SoldiersStrike { get; } = new() { Id = 1695, Name = "Soldier's Strike", Profession = Profession.Warrior };
        public static Skill SoldiersStance { get; } = new() { Id = 1698, Name = "Soldier's Stance", Profession = Profession.Warrior };
        public static Skill SoldiersDefense { get; } = new() { Id = 1699, Name = "Soldier's Defense", Profession = Profession.Warrior };
        public static Skill SteadyStance { get; } = new() { Id = 1701, Name = "Steady Stance", Profession = Profession.Warrior };
        public static Skill SoldiersSpeed { get; } = new() { Id = 2196, Name = "Soldier's Speed", Profession = Profession.Warrior };
        public static Skill WildBlow { get; } = new() { Id = 321, Name = "Wild Blow", Profession = Profession.Warrior };
        public static Skill DistractingBlow { get; } = new() { Id = 325, Name = "Distracting Blow", Profession = Profession.Warrior };
        public static Skill SkullCrack { get; } = new() { Id = 329, Name = "Skull Crack", Profession = Profession.Warrior };
        public static Skill ForGreatJustice { get; } = new() { Id = 343, Name = "\"For Great Justice!\"", Profession = Profession.Warrior };
        public static Skill Flurry { get; } = new() { Id = 344, Name = "Flurry", Profession = Profession.Warrior };
        public static Skill Frenzy { get; } = new() { Id = 346, Name = "Frenzy", Profession = Profession.Warrior };
        public static Skill Coward { get; } = new() { Id = 869, Name = "\"Coward!\"", Profession = Profession.Warrior };
        public static Skill OnYourKnees { get; } = new() { Id = 906, Name = "On Your Knees!", Profession = Profession.Warrior };
        public static Skill YoureAllAlone { get; } = new() { Id = 1412, Name = "\"You're All Alone!\"", Profession = Profession.Warrior };
        public static Skill FrenziedDefense { get; } = new() { Id = 1700, Name = "Frenzied Defense", Profession = Profession.Warrior };
        public static Skill Grapple { get; } = new() { Id = 2011, Name = "Grapple", Profession = Profession.Warrior };
        public static Skill DistractingStrike { get; } = new() { Id = 2194, Name = "Distracting Strike", Profession = Profession.Warrior };
        public static Skill SymbolicStrike { get; } = new() { Id = 2195, Name = "Symbolic Strike", Profession = Profession.Warrior };
        public static Skill CharmAnimal { get; } = new() { Id = 411, Name = "Charm Animal", Profession = Profession.Ranger };
        public static Skill CallofProtection { get; } = new() { Id = 412, Name = "Call of Protection", Profession = Profession.Ranger };
        public static Skill CallofHaste { get; } = new() { Id = 415, Name = "Call of Haste", Profession = Profession.Ranger };
        public static Skill ReviveAnimal { get; } = new() { Id = 422, Name = "Revive Animal", Profession = Profession.Ranger };
        public static Skill SymbioticBond { get; } = new() { Id = 423, Name = "Symbiotic Bond", Profession = Profession.Ranger };
        public static Skill ComfortAnimal { get; } = new() { Id = 436, Name = "Comfort Animal", Profession = Profession.Ranger };
        public static Skill BestialPounce { get; } = new() { Id = 437, Name = "Bestial Pounce", Profession = Profession.Ranger };
        public static Skill MaimingStrike { get; } = new() { Id = 438, Name = "Maiming Strike", Profession = Profession.Ranger };
        public static Skill FeralLunge { get; } = new() { Id = 439, Name = "Feral Lunge", Profession = Profession.Ranger };
        public static Skill ScavengerStrike { get; } = new() { Id = 440, Name = "Scavenger Strike", Profession = Profession.Ranger };
        public static Skill MelandrusAssault { get; } = new() { Id = 441, Name = "Melandru's Assault", Profession = Profession.Ranger };
        public static Skill FerociousStrike { get; } = new() { Id = 442, Name = "Ferocious Strike", Profession = Profession.Ranger };
        public static Skill PredatorsPounce { get; } = new() { Id = 443, Name = "Predator's Pounce", Profession = Profession.Ranger };
        public static Skill BrutalStrike { get; } = new() { Id = 444, Name = "Brutal Strike", Profession = Profession.Ranger };
        public static Skill DisruptingLunge { get; } = new() { Id = 445, Name = "Disrupting Lunge", Profession = Profession.Ranger };
        public static Skill OtyughsCry { get; } = new() { Id = 447, Name = "Otyugh's Cry", Profession = Profession.Ranger };
        public static Skill TigersFury { get; } = new() { Id = 454, Name = "Tiger's Fury", Profession = Profession.Ranger };
        public static Skill EdgeofExtinction { get; } = new() { Id = 464, Name = "Edge of Extinction", Profession = Profession.Ranger };
        public static Skill FertileSeason { get; } = new() { Id = 467, Name = "Fertile Season", Profession = Profession.Ranger };
        public static Skill Symbiosis { get; } = new() { Id = 468, Name = "Symbiosis", Profession = Profession.Ranger };
        public static Skill PrimalEchoes { get; } = new() { Id = 469, Name = "Primal Echoes", Profession = Profession.Ranger };
        public static Skill PredatorySeason { get; } = new() { Id = 470, Name = "Predatory Season", Profession = Profession.Ranger };
        public static Skill EnergizingWind { get; } = new() { Id = 474, Name = "Energizing Wind", Profession = Profession.Ranger };
        public static Skill RunasOne { get; } = new() { Id = 811, Name = "Run as One", Profession = Profession.Ranger };
        public static Skill Lacerate { get; } = new() { Id = 961, Name = "Lacerate", Profession = Profession.Ranger };
        public static Skill PredatoryBond { get; } = new() { Id = 1194, Name = "Predatory Bond", Profession = Profession.Ranger };
        public static Skill HealasOne { get; } = new() { Id = 1195, Name = "Heal as One", Profession = Profession.Ranger };
        public static Skill SavagePounce { get; } = new() { Id = 1201, Name = "Savage Pounce", Profession = Profession.Ranger };
        public static Skill EnragedLunge { get; } = new() { Id = 1202, Name = "Enraged Lunge", Profession = Profession.Ranger };
        public static Skill BestialMauling { get; } = new() { Id = 1203, Name = "Bestial Mauling", Profession = Profession.Ranger };
        public static Skill PoisonousBite { get; } = new() { Id = 1205, Name = "Poisonous Bite", Profession = Profession.Ranger };
        public static Skill Pounce { get; } = new() { Id = 1206, Name = "Pounce", Profession = Profession.Ranger };
        public static Skill BestialFury { get; } = new() { Id = 1209, Name = "Bestial Fury", Profession = Profession.Ranger };
        public static Skill VipersNest { get; } = new() { Id = 1211, Name = "Viper's Nest", Profession = Profession.Ranger };
        public static Skill StrikeasOne { get; } = new() { Id = 1468, Name = "Strike as One", Profession = Profession.Ranger };
        public static Skill Toxicity { get; } = new() { Id = 1472, Name = "Toxicity", Profession = Profession.Ranger };
        public static Skill RampageasOne { get; } = new() { Id = 1721, Name = "Rampage as One", Profession = Profession.Ranger };
        public static Skill HeketsRampage { get; } = new() { Id = 1728, Name = "Heket's Rampage", Profession = Profession.Ranger };
        public static Skill Companionship { get; } = new() { Id = 2141, Name = "Companionship", Profession = Profession.Ranger };
        public static Skill FeralAggression { get; } = new() { Id = 2142, Name = "Feral Aggression", Profession = Profession.Ranger };
        public static Skill DistractingShot { get; } = new() { Id = 399, Name = "Distracting Shot", Profession = Profession.Ranger };
        public static Skill OathShot { get; } = new() { Id = 405, Name = "Oath Shot", Profession = Profession.Ranger };
        public static Skill PointBlankShot { get; } = new() { Id = 407, Name = "Point Blank Shot", Profession = Profession.Ranger };
        public static Skill ThrowDirt { get; } = new() { Id = 424, Name = "Throw Dirt", Profession = Profession.Ranger };
        public static Skill Dodge { get; } = new() { Id = 425, Name = "Dodge", Profession = Profession.Ranger };
        public static Skill MarksmansWager { get; } = new() { Id = 430, Name = "Marksman's Wager", Profession = Profession.Ranger };
        public static Skill Escape { get; } = new() { Id = 448, Name = "Escape", Profession = Profession.Ranger };
        public static Skill PracticedStance { get; } = new() { Id = 449, Name = "Practiced Stance", Profession = Profession.Ranger };
        public static Skill WhirlingDefense { get; } = new() { Id = 450, Name = "Whirling Defense", Profession = Profession.Ranger };
        public static Skill LightningReflexes { get; } = new() { Id = 453, Name = "Lightning Reflexes", Profession = Profession.Ranger };
        public static Skill TrappersFocus { get; } = new() { Id = 946, Name = "Trapper's Focus", Profession = Profession.Ranger };
        public static Skill ZojunsShot { get; } = new() { Id = 1192, Name = "Zojun's Shot", Profession = Profession.Ranger };
        public static Skill ZojunsHaste { get; } = new() { Id = 1196, Name = "Zojun's Haste", Profession = Profession.Ranger };
        public static Skill GlassArrows { get; } = new() { Id = 1199, Name = "Glass Arrows", Profession = Profession.Ranger };
        public static Skill ArchersSignet { get; } = new() { Id = 1200, Name = "Archer's Signet", Profession = Profession.Ranger };
        public static Skill TrappersSpeed { get; } = new() { Id = 1475, Name = "Trapper's Speed", Profession = Profession.Ranger };
        public static Skill ExpertsDexterity { get; } = new() { Id = 1724, Name = "Expert's Dexterity", Profession = Profession.Ranger };
        public static Skill InfuriatingHeat { get; } = new() { Id = 1730, Name = "Infuriating Heat", Profession = Profession.Ranger };
        public static Skill ExpertFocus { get; } = new() { Id = 2145, Name = "Expert Focus", Profession = Profession.Ranger };
        public static Skill HuntersShot { get; } = new() { Id = 391, Name = "Hunter's Shot", Profession = Profession.Ranger };
        public static Skill PinDown { get; } = new() { Id = 392, Name = "Pin Down", Profession = Profession.Ranger };
        public static Skill CripplingShot { get; } = new() { Id = 393, Name = "Crippling Shot", Profession = Profession.Ranger };
        public static Skill PowerShot { get; } = new() { Id = 394, Name = "Power Shot", Profession = Profession.Ranger };
        public static Skill Barrage { get; } = new() { Id = 395, Name = "Barrage", Profession = Profession.Ranger };
        public static Skill PenetratingAttack { get; } = new() { Id = 398, Name = "Penetrating Attack", Profession = Profession.Ranger };
        public static Skill PrecisionShot { get; } = new() { Id = 400, Name = "Precision Shot", Profession = Profession.Ranger };
        public static Skill DeterminedShot { get; } = new() { Id = 402, Name = "Determined Shot", Profession = Profession.Ranger };
        public static Skill DebilitatingShot { get; } = new() { Id = 406, Name = "Debilitating Shot", Profession = Profession.Ranger };
        public static Skill ConcussionShot { get; } = new() { Id = 408, Name = "Concussion Shot", Profession = Profession.Ranger };
        public static Skill PunishingShot { get; } = new() { Id = 409, Name = "Punishing Shot", Profession = Profession.Ranger };
        public static Skill SavageShot { get; } = new() { Id = 426, Name = "Savage Shot", Profession = Profession.Ranger };
        public static Skill ReadtheWind { get; } = new() { Id = 432, Name = "Read the Wind", Profession = Profession.Ranger };
        public static Skill FavorableWinds { get; } = new() { Id = 472, Name = "Favorable Winds", Profession = Profession.Ranger };
        public static Skill SplinterShot { get; } = new() { Id = 852, Name = "Splinter Shot", Profession = Profession.Ranger };
        public static Skill MelandrusShot { get; } = new() { Id = 853, Name = "Melandru's Shot", Profession = Profession.Ranger };
        public static Skill SeekingArrows { get; } = new() { Id = 893, Name = "Seeking Arrows", Profession = Profession.Ranger };
        public static Skill MaraudersShot { get; } = new() { Id = 908, Name = "Marauder's Shot", Profession = Profession.Ranger };
        public static Skill FocusedShot { get; } = new() { Id = 909, Name = "Focused Shot", Profession = Profession.Ranger };
        public static Skill SunderingAttack { get; } = new() { Id = 1191, Name = "Sundering Attack", Profession = Profession.Ranger };
        public static Skill NeedlingShot { get; } = new() { Id = 1197, Name = "Needling Shot", Profession = Profession.Ranger };
        public static Skill BroadHeadArrow { get; } = new() { Id = 1198, Name = "Broad Head Arrow", Profession = Profession.Ranger };
        public static Skill PreparedShot { get; } = new() { Id = 1465, Name = "Prepared Shot", Profession = Profession.Ranger };
        public static Skill BurningArrow { get; } = new() { Id = 1466, Name = "Burning Arrow", Profession = Profession.Ranger };
        public static Skill ArcingShot { get; } = new() { Id = 1467, Name = "Arcing Shot", Profession = Profession.Ranger };
        public static Skill Crossfire { get; } = new() { Id = 1469, Name = "Crossfire", Profession = Profession.Ranger };
        public static Skill ScreamingShot { get; } = new() { Id = 1719, Name = "Screaming Shot", Profession = Profession.Ranger };
        public static Skill KeenArrow { get; } = new() { Id = 1720, Name = "Keen Arrow", Profession = Profession.Ranger };
        public static Skill DisruptingAccuracy { get; } = new() { Id = 1723, Name = "Disrupting Accuracy", Profession = Profession.Ranger };
        public static Skill RapidFire { get; } = new() { Id = 2068, Name = "Rapid Fire", Profession = Profession.Ranger };
        public static Skill SlothHuntersShot { get; } = new() { Id = 2069, Name = "Sloth Hunter's Shot", Profession = Profession.Ranger };
        public static Skill DisruptingShot { get; } = new() { Id = 2143, Name = "Disrupting Shot", Profession = Profession.Ranger };
        public static Skill Volley { get; } = new() { Id = 2144, Name = "Volley", Profession = Profession.Ranger };
        public static Skill BodyShot { get; } = new() { Id = 2198, Name = "Body Shot", Profession = Profession.Ranger };
        public static Skill PoisonArrow { get; } = new() { Id = 404, Name = "Poison Arrow", Profession = Profession.Ranger };
        public static Skill IncendiaryArrows { get; } = new() { Id = 428, Name = "Incendiary Arrows", Profession = Profession.Ranger };
        public static Skill MelandrusArrows { get; } = new() { Id = 429, Name = "Melandru's Arrows", Profession = Profession.Ranger };
        public static Skill IgniteArrows { get; } = new() { Id = 431, Name = "Ignite Arrows", Profession = Profession.Ranger };
        public static Skill KindleArrows { get; } = new() { Id = 433, Name = "Kindle Arrows", Profession = Profession.Ranger };
        public static Skill ChokingGas { get; } = new() { Id = 434, Name = "Choking Gas", Profession = Profession.Ranger };
        public static Skill ApplyPoison { get; } = new() { Id = 435, Name = "Apply Poison", Profession = Profession.Ranger };
        public static Skill TrollUnguent { get; } = new() { Id = 446, Name = "Troll Unguent", Profession = Profession.Ranger };
        public static Skill MelandrusResilience { get; } = new() { Id = 451, Name = "Melandru's Resilience", Profession = Profession.Ranger };
        public static Skill DrydersDefenses { get; } = new() { Id = 452, Name = "Dryder's Defenses", Profession = Profession.Ranger };
        public static Skill StormChaser { get; } = new() { Id = 455, Name = "Storm Chaser", Profession = Profession.Ranger };
        public static Skill SerpentsQuickness { get; } = new() { Id = 456, Name = "Serpent's Quickness", Profession = Profession.Ranger };
        public static Skill DustTrap { get; } = new() { Id = 457, Name = "Dust Trap", Profession = Profession.Ranger };
        public static Skill BarbedTrap { get; } = new() { Id = 458, Name = "Barbed Trap", Profession = Profession.Ranger };
        public static Skill FlameTrap { get; } = new() { Id = 459, Name = "Flame Trap", Profession = Profession.Ranger };
        public static Skill HealingSpring { get; } = new() { Id = 460, Name = "Healing Spring", Profession = Profession.Ranger };
        public static Skill SpikeTrap { get; } = new() { Id = 461, Name = "Spike Trap", Profession = Profession.Ranger };
        public static Skill Winter { get; } = new() { Id = 462, Name = "Winter", Profession = Profession.Ranger };
        public static Skill Winnowing { get; } = new() { Id = 463, Name = "Winnowing", Profession = Profession.Ranger };
        public static Skill GreaterConflagration { get; } = new() { Id = 465, Name = "Greater Conflagration", Profession = Profession.Ranger };
        public static Skill Conflagration { get; } = new() { Id = 466, Name = "Conflagration", Profession = Profession.Ranger };
        public static Skill FrozenSoil { get; } = new() { Id = 471, Name = "Frozen Soil", Profession = Profession.Ranger };
        public static Skill QuickeningZephyr { get; } = new() { Id = 475, Name = "Quickening Zephyr", Profession = Profession.Ranger };
        public static Skill NaturesRenewal { get; } = new() { Id = 476, Name = "Nature's Renewal", Profession = Profession.Ranger };
        public static Skill MuddyTerrain { get; } = new() { Id = 477, Name = "Muddy Terrain", Profession = Profession.Ranger };
        public static Skill Snare { get; } = new() { Id = 854, Name = "Snare", Profession = Profession.Ranger };
        public static Skill Pestilence { get; } = new() { Id = 870, Name = "Pestilence", Profession = Profession.Ranger };
        public static Skill Brambles { get; } = new() { Id = 947, Name = "Brambles", Profession = Profession.Ranger };
        public static Skill Famine { get; } = new() { Id = 997, Name = "Famine", Profession = Profession.Ranger };
        public static Skill Equinox { get; } = new() { Id = 1212, Name = "Equinox", Profession = Profession.Ranger };
        public static Skill Tranquility { get; } = new() { Id = 1213, Name = "Tranquility", Profession = Profession.Ranger };
        public static Skill BarbedArrows { get; } = new() { Id = 1470, Name = "Barbed Arrows", Profession = Profession.Ranger };
        public static Skill ScavengersFocus { get; } = new() { Id = 1471, Name = "Scavenger's Focus", Profession = Profession.Ranger };
        public static Skill Quicksand { get; } = new() { Id = 1473, Name = "Quicksand", Profession = Profession.Ranger };
        public static Skill Tripwire { get; } = new() { Id = 1476, Name = "Tripwire", Profession = Profession.Ranger };
        public static Skill RoaringWinds { get; } = new() { Id = 1725, Name = "Roaring Winds", Profession = Profession.Ranger };
        public static Skill NaturalStride { get; } = new() { Id = 1727, Name = "Natural Stride", Profession = Profession.Ranger };
        public static Skill SmokeTrap { get; } = new() { Id = 1729, Name = "Smoke Trap", Profession = Profession.Ranger };
        public static Skill PiercingTrap { get; } = new() { Id = 2140, Name = "Piercing Trap", Profession = Profession.Ranger };
        public static Skill PoisonTipSignet { get; } = new() { Id = 2199, Name = "Poison Tip Signet", Profession = Profession.Ranger };
        public static Skill DualShot { get; } = new() { Id = 396, Name = "Dual Shot", Profession = Profession.Ranger };
        public static Skill QuickShot { get; } = new() { Id = 397, Name = "Quick Shot", Profession = Profession.Ranger };
        public static Skill CalledShot { get; } = new() { Id = 403, Name = "Called Shot", Profession = Profession.Ranger };
        public static Skill AntidoteSignet { get; } = new() { Id = 427, Name = "Antidote Signet", Profession = Profession.Ranger };
        public static Skill StormsEmbrace { get; } = new() { Id = 1474, Name = "Storm's Embrace", Profession = Profession.Ranger };
        public static Skill ForkedArrow { get; } = new() { Id = 1722, Name = "Forked Arrow", Profession = Profession.Ranger };
        public static Skill MagebaneShot { get; } = new() { Id = 1726, Name = "Magebane Shot", Profession = Profession.Ranger };
        public static Skill DivineIntervention { get; } = new() { Id = 246, Name = "Divine Intervention", Profession = Profession.Monk };
        public static Skill WatchfulSpirit { get; } = new() { Id = 255, Name = "Watchful Spirit", Profession = Profession.Monk };
        public static Skill BlessedAura { get; } = new() { Id = 256, Name = "Blessed Aura", Profession = Profession.Monk };
        public static Skill PeaceandHarmony { get; } = new() { Id = 266, Name = "Peace and Harmony", Profession = Profession.Monk };
        public static Skill UnyieldingAura { get; } = new() { Id = 268, Name = "Unyielding Aura", Profession = Profession.Monk };
        public static Skill SpellBreaker { get; } = new() { Id = 273, Name = "Spell Breaker", Profession = Profession.Monk };
        public static Skill DivineHealing { get; } = new() { Id = 279, Name = "Divine Healing", Profession = Profession.Monk };
        public static Skill DivineBoon { get; } = new() { Id = 284, Name = "Divine Boon", Profession = Profession.Monk };
        public static Skill SignetofDevotion { get; } = new() { Id = 293, Name = "Signet of Devotion", Profession = Profession.Monk };
        public static Skill BlessedSignet { get; } = new() { Id = 297, Name = "Blessed Signet", Profession = Profession.Monk };
        public static Skill ContemplationofPurity { get; } = new() { Id = 300, Name = "Contemplation of Purity", Profession = Profession.Monk };
        public static Skill DivineSpirit { get; } = new() { Id = 310, Name = "Divine Spirit", Profession = Profession.Monk };
        public static Skill BoonSignet { get; } = new() { Id = 847, Name = "Boon Signet", Profession = Profession.Monk };
        public static Skill BlessedLight { get; } = new() { Id = 941, Name = "Blessed Light", Profession = Profession.Monk };
        public static Skill WithdrawHexes { get; } = new() { Id = 942, Name = "Withdraw Hexes", Profession = Profession.Monk };
        public static Skill SpellShield { get; } = new() { Id = 957, Name = "Spell Shield", Profession = Profession.Monk };
        public static Skill ReleaseEnchantments { get; } = new() { Id = 960, Name = "Release Enchantments", Profession = Profession.Monk };
        public static Skill DenyHexes { get; } = new() { Id = 991, Name = "Deny Hexes", Profession = Profession.Monk };
        public static Skill HeavensDelight { get; } = new() { Id = 1117, Name = "Heaven's Delight", Profession = Profession.Monk };
        public static Skill WatchfulHealing { get; } = new() { Id = 1392, Name = "Watchful Healing", Profession = Profession.Monk };
        public static Skill HealersBoon { get; } = new() { Id = 1393, Name = "Healer's Boon", Profession = Profession.Monk };
        public static Skill ScribesInsight { get; } = new() { Id = 1684, Name = "Scribe's Insight", Profession = Profession.Monk };
        public static Skill HolyHaste { get; } = new() { Id = 1685, Name = "Holy Haste", Profession = Profession.Monk };
        public static Skill SmitersBoon { get; } = new() { Id = 2005, Name = "Smiter's Boon", Profession = Profession.Monk };
        public static Skill VigorousSpirit { get; } = new() { Id = 254, Name = "Vigorous Spirit", Profession = Profession.Monk };
        public static Skill HealingSeed { get; } = new() { Id = 274, Name = "Healing Seed", Profession = Profession.Monk };
        public static Skill HealArea { get; } = new() { Id = 280, Name = "Heal Area", Profession = Profession.Monk };
        public static Skill OrisonofHealing { get; } = new() { Id = 281, Name = "Orison of Healing", Profession = Profession.Monk };
        public static Skill WordofHealing { get; } = new() { Id = 282, Name = "Word of Healing", Profession = Profession.Monk };
        public static Skill DwaynasKiss { get; } = new() { Id = 283, Name = "Dwayna's Kiss", Profession = Profession.Monk };
        public static Skill HealingHands { get; } = new() { Id = 285, Name = "Healing Hands", Profession = Profession.Monk };
        public static Skill HealOther { get; } = new() { Id = 286, Name = "Heal Other", Profession = Profession.Monk };
        public static Skill HealParty { get; } = new() { Id = 287, Name = "Heal Party", Profession = Profession.Monk };
        public static Skill HealingBreeze { get; } = new() { Id = 288, Name = "Healing Breeze", Profession = Profession.Monk };
        public static Skill Mending { get; } = new() { Id = 290, Name = "Mending", Profession = Profession.Monk };
        public static Skill LiveVicariously { get; } = new() { Id = 291, Name = "Live Vicariously", Profession = Profession.Monk };
        public static Skill InfuseHealth { get; } = new() { Id = 292, Name = "Infuse Health", Profession = Profession.Monk };
        public static Skill HealingTouch { get; } = new() { Id = 313, Name = "Healing Touch", Profession = Profession.Monk };
        public static Skill RestoreLife { get; } = new() { Id = 314, Name = "Restore Life", Profession = Profession.Monk };
        public static Skill DwaynasSorrow { get; } = new() { Id = 838, Name = "Dwayna's Sorrow", Profession = Profession.Monk };
        public static Skill HealingLight { get; } = new() { Id = 867, Name = "Healing Light", Profession = Profession.Monk };
        public static Skill RestfulBreeze { get; } = new() { Id = 886, Name = "Restful Breeze", Profession = Profession.Monk };
        public static Skill SignetofRejuvenation { get; } = new() { Id = 887, Name = "Signet of Rejuvenation", Profession = Profession.Monk };
        public static Skill HealingWhisper { get; } = new() { Id = 958, Name = "Healing Whisper", Profession = Profession.Monk };
        public static Skill EtherealLight { get; } = new() { Id = 959, Name = "Ethereal Light", Profession = Profession.Monk };
        public static Skill HealingBurst { get; } = new() { Id = 1118, Name = "Healing Burst", Profession = Profession.Monk };
        public static Skill KareisHealingCircle { get; } = new() { Id = 1119, Name = "Karei's Healing Circle", Profession = Profession.Monk };
        public static Skill JameisGaze { get; } = new() { Id = 1120, Name = "Jamei's Gaze", Profession = Profession.Monk };
        public static Skill GiftofHealth { get; } = new() { Id = 1121, Name = "Gift of Health", Profession = Profession.Monk };
        public static Skill ResurrectionChant { get; } = new() { Id = 1128, Name = "Resurrection Chant", Profession = Profession.Monk };
        public static Skill HealingRing { get; } = new() { Id = 1262, Name = "Healing Ring", Profession = Profession.Monk };
        public static Skill RenewLife { get; } = new() { Id = 1263, Name = "Renew Life", Profession = Profession.Monk };
        public static Skill SupportiveSpirit { get; } = new() { Id = 1391, Name = "Supportive Spirit", Profession = Profession.Monk };
        public static Skill HealersCovenant { get; } = new() { Id = 1394, Name = "Healer's Covenant", Profession = Profession.Monk };
        public static Skill WordsofComfort { get; } = new() { Id = 1396, Name = "Words of Comfort", Profession = Profession.Monk };
        public static Skill LightofDeliverance { get; } = new() { Id = 1397, Name = "Light of Deliverance", Profession = Profession.Monk };
        public static Skill GlimmerofLight { get; } = new() { Id = 1686, Name = "Glimmer of Light", Profession = Profession.Monk };
        public static Skill CureHex { get; } = new() { Id = 2003, Name = "Cure Hex", Profession = Profession.Monk };
        public static Skill PatientSpirit { get; } = new() { Id = 2061, Name = "Patient Spirit", Profession = Profession.Monk };
        public static Skill HealingRibbon { get; } = new() { Id = 2062, Name = "Healing Ribbon", Profession = Profession.Monk };
        public static Skill SpotlessMind { get; } = new() { Id = 2064, Name = "Spotless Mind", Profession = Profession.Monk };
        public static Skill SpotlessSoul { get; } = new() { Id = 2065, Name = "Spotless Soul", Profession = Profession.Monk };
        public static Skill LifeBond { get; } = new() { Id = 241, Name = "Life Bond", Profession = Profession.Monk };
        public static Skill LifeAttunement { get; } = new() { Id = 244, Name = "Life Attunement", Profession = Profession.Monk };
        public static Skill ProtectiveSpirit { get; } = new() { Id = 245, Name = "Protective Spirit", Profession = Profession.Monk };
        public static Skill Aegis { get; } = new() { Id = 257, Name = "Aegis", Profession = Profession.Monk };
        public static Skill Guardian { get; } = new() { Id = 258, Name = "Guardian", Profession = Profession.Monk };
        public static Skill ShieldofDeflection { get; } = new() { Id = 259, Name = "Shield of Deflection", Profession = Profession.Monk };
        public static Skill AuraofFaith { get; } = new() { Id = 260, Name = "Aura of Faith", Profession = Profession.Monk };
        public static Skill ShieldofRegeneration { get; } = new() { Id = 261, Name = "Shield of Regeneration", Profession = Profession.Monk };
        public static Skill ProtectiveBond { get; } = new() { Id = 263, Name = "Protective Bond", Profession = Profession.Monk };
        public static Skill Pacifism { get; } = new() { Id = 264, Name = "Pacifism", Profession = Profession.Monk };
        public static Skill Amity { get; } = new() { Id = 265, Name = "Amity", Profession = Profession.Monk };
        public static Skill MarkofProtection { get; } = new() { Id = 269, Name = "Mark of Protection", Profession = Profession.Monk };
        public static Skill LifeBarrier { get; } = new() { Id = 270, Name = "Life Barrier", Profession = Profession.Monk };
        public static Skill MendCondition { get; } = new() { Id = 275, Name = "Mend Condition", Profession = Profession.Monk };
        public static Skill RestoreCondition { get; } = new() { Id = 276, Name = "Restore Condition", Profession = Profession.Monk };
        public static Skill MendAilment { get; } = new() { Id = 277, Name = "Mend Ailment", Profession = Profession.Monk };
        public static Skill VitalBlessing { get; } = new() { Id = 289, Name = "Vital Blessing", Profession = Profession.Monk };
        public static Skill ShieldingHands { get; } = new() { Id = 299, Name = "Shielding Hands", Profession = Profession.Monk };
        public static Skill ConvertHexes { get; } = new() { Id = 303, Name = "Convert Hexes", Profession = Profession.Monk };
        public static Skill Rebirth { get; } = new() { Id = 306, Name = "Rebirth", Profession = Profession.Monk };
        public static Skill ReversalofFortune { get; } = new() { Id = 307, Name = "Reversal of Fortune", Profession = Profession.Monk };
        public static Skill DrawConditions { get; } = new() { Id = 311, Name = "Draw Conditions", Profession = Profession.Monk };
        public static Skill ReverseHex { get; } = new() { Id = 848, Name = "Reverse Hex", Profession = Profession.Monk };
        public static Skill ShieldGuardian { get; } = new() { Id = 885, Name = "Shield Guardian", Profession = Profession.Monk };
        public static Skill Extinguish { get; } = new() { Id = 943, Name = "Extinguish", Profession = Profession.Monk };
        public static Skill SpiritBond { get; } = new() { Id = 1114, Name = "Spirit Bond", Profession = Profession.Monk };
        public static Skill AirofEnchantment { get; } = new() { Id = 1115, Name = "Air of Enchantment", Profession = Profession.Monk };
        public static Skill LifeSheath { get; } = new() { Id = 1123, Name = "Life Sheath", Profession = Profession.Monk };
        public static Skill ShieldofAbsorption { get; } = new() { Id = 1399, Name = "Shield of Absorption", Profession = Profession.Monk };
        public static Skill MendingTouch { get; } = new() { Id = 1401, Name = "Mending Touch", Profession = Profession.Monk };
        public static Skill PensiveGuardian { get; } = new() { Id = 1683, Name = "Pensive Guardian", Profession = Profession.Monk };
        public static Skill ZealousBenediction { get; } = new() { Id = 1687, Name = "Zealous Benediction", Profession = Profession.Monk };
        public static Skill DismissCondition { get; } = new() { Id = 1691, Name = "Dismiss Condition", Profession = Profession.Monk };
        public static Skill DivertHexes { get; } = new() { Id = 1692, Name = "Divert Hexes", Profession = Profession.Monk };
        public static Skill PurifyingVeil { get; } = new() { Id = 2007, Name = "Purifying Veil", Profession = Profession.Monk };
        public static Skill AuraofStability { get; } = new() { Id = 2063, Name = "Aura of Stability", Profession = Profession.Monk };
        public static Skill Smite { get; } = new() { Id = 240, Name = "Smite", Profession = Profession.Monk };
        public static Skill BalthazarsSpirit { get; } = new() { Id = 242, Name = "Balthazar's Spirit", Profession = Profession.Monk };
        public static Skill StrengthofHonor { get; } = new() { Id = 243, Name = "Strength of Honor", Profession = Profession.Monk };
        public static Skill SymbolofWrath { get; } = new() { Id = 247, Name = "Symbol of Wrath", Profession = Profession.Monk };
        public static Skill Retribution { get; } = new() { Id = 248, Name = "Retribution", Profession = Profession.Monk };
        public static Skill HolyWrath { get; } = new() { Id = 249, Name = "Holy Wrath", Profession = Profession.Monk };
        public static Skill ScourgeHealing { get; } = new() { Id = 251, Name = "Scourge Healing", Profession = Profession.Monk };
        public static Skill Banish { get; } = new() { Id = 252, Name = "Banish", Profession = Profession.Monk };
        public static Skill ScourgeSacrifice { get; } = new() { Id = 253, Name = "Scourge Sacrifice", Profession = Profession.Monk };
        public static Skill ShieldofJudgment { get; } = new() { Id = 262, Name = "Shield of Judgment", Profession = Profession.Monk };
        public static Skill JudgesInsight { get; } = new() { Id = 267, Name = "Judge's Insight", Profession = Profession.Monk };
        public static Skill ZealotsFire { get; } = new() { Id = 271, Name = "Zealot's Fire", Profession = Profession.Monk };
        public static Skill BalthazarsAura { get; } = new() { Id = 272, Name = "Balthazar's Aura", Profession = Profession.Monk };
        public static Skill SignetofJudgment { get; } = new() { Id = 294, Name = "Signet of Judgment", Profession = Profession.Monk };
        public static Skill BaneSignet { get; } = new() { Id = 296, Name = "Bane Signet", Profession = Profession.Monk };
        public static Skill SmiteHex { get; } = new() { Id = 302, Name = "Smite Hex", Profession = Profession.Monk };
        public static Skill HolyStrike { get; } = new() { Id = 312, Name = "Holy Strike", Profession = Profession.Monk };
        public static Skill RayofJudgment { get; } = new() { Id = 830, Name = "Ray of Judgment", Profession = Profession.Monk };
        public static Skill KirinsWrath { get; } = new() { Id = 1113, Name = "Kirin's Wrath", Profession = Profession.Monk };
        public static Skill WordofCensure { get; } = new() { Id = 1129, Name = "Word of Censure", Profession = Profession.Monk };
        public static Skill SpearofLight { get; } = new() { Id = 1130, Name = "Spear of Light", Profession = Profession.Monk };
        public static Skill StonesoulStrike { get; } = new() { Id = 1131, Name = "Stonesoul Strike", Profession = Profession.Monk };
        public static Skill SignetofRage { get; } = new() { Id = 1269, Name = "Signet of Rage", Profession = Profession.Monk };
        public static Skill JudgesIntervention { get; } = new() { Id = 1390, Name = "Judge's Intervention", Profession = Profession.Monk };
        public static Skill BalthazarsPendulum { get; } = new() { Id = 1395, Name = "Balthazar's Pendulum", Profession = Profession.Monk };
        public static Skill ScourgeEnchantment { get; } = new() { Id = 1398, Name = "Scourge Enchantment", Profession = Profession.Monk };
        public static Skill ReversalofDamage { get; } = new() { Id = 1400, Name = "Reversal of Damage", Profession = Profession.Monk };
        public static Skill DefendersZeal { get; } = new() { Id = 1688, Name = "Defender's Zeal", Profession = Profession.Monk };
        public static Skill SignetofMysticWrath { get; } = new() { Id = 1689, Name = "Signet of Mystic Wrath", Profession = Profession.Monk };
        public static Skill SmiteCondition { get; } = new() { Id = 2004, Name = "Smite Condition", Profession = Profession.Monk };
        public static Skill CastigationSignet { get; } = new() { Id = 2006, Name = "Castigation Signet", Profession = Profession.Monk };
        public static Skill EssenceBond { get; } = new() { Id = 250, Name = "Essence Bond", Profession = Profession.Monk };
        public static Skill PurgeConditions { get; } = new() { Id = 278, Name = "Purge Conditions", Profession = Profession.Monk };
        public static Skill PurgeSignet { get; } = new() { Id = 295, Name = "Purge Signet", Profession = Profession.Monk };
        public static Skill Martyr { get; } = new() { Id = 298, Name = "Martyr", Profession = Profession.Monk };
        public static Skill RemoveHex { get; } = new() { Id = 301, Name = "Remove Hex", Profession = Profession.Monk };
        public static Skill LightofDwayna { get; } = new() { Id = 304, Name = "Light of Dwayna", Profession = Profession.Monk };
        public static Skill Resurrect { get; } = new() { Id = 305, Name = "Resurrect", Profession = Profession.Monk };
        public static Skill Succor { get; } = new() { Id = 308, Name = "Succor", Profession = Profession.Monk };
        public static Skill HolyVeil { get; } = new() { Id = 309, Name = "Holy Veil", Profession = Profession.Monk };
        public static Skill Vengeance { get; } = new() { Id = 315, Name = "Vengeance", Profession = Profession.Monk };
        public static Skill EmpathicRemoval { get; } = new() { Id = 1126, Name = "Empathic Removal", Profession = Profession.Monk };
        public static Skill SignetofRemoval { get; } = new() { Id = 1690, Name = "Signet of Removal", Profession = Profession.Monk };
        public static Skill WellofPower { get; } = new() { Id = 91, Name = "Well of Power", Profession = Profession.Necromancer };
        public static Skill WellofBlood { get; } = new() { Id = 92, Name = "Well of Blood", Profession = Profession.Necromancer };
        public static Skill ShadowStrike { get; } = new() { Id = 102, Name = "Shadow Strike", Profession = Profession.Necromancer };
        public static Skill LifeSiphon { get; } = new() { Id = 109, Name = "Life Siphon", Profession = Profession.Necromancer };
        public static Skill UnholyFeast { get; } = new() { Id = 110, Name = "Unholy Feast", Profession = Profession.Necromancer };
        public static Skill AwakentheBlood { get; } = new() { Id = 111, Name = "Awaken the Blood", Profession = Profession.Necromancer };
        public static Skill BloodRenewal { get; } = new() { Id = 115, Name = "Blood Renewal", Profession = Profession.Necromancer };
        public static Skill BloodisPower { get; } = new() { Id = 119, Name = "Blood is Power", Profession = Profession.Necromancer };
        public static Skill LifeTransfer { get; } = new() { Id = 126, Name = "Life Transfer", Profession = Profession.Necromancer };
        public static Skill MarkofSubversion { get; } = new() { Id = 127, Name = "Mark of Subversion", Profession = Profession.Necromancer };
        public static Skill SoulLeech { get; } = new() { Id = 128, Name = "Soul Leech", Profession = Profession.Necromancer };
        public static Skill DemonicFlesh { get; } = new() { Id = 130, Name = "Demonic Flesh", Profession = Profession.Necromancer };
        public static Skill BarbedSignet { get; } = new() { Id = 131, Name = "Barbed Signet", Profession = Profession.Necromancer };
        public static Skill DarkPact { get; } = new() { Id = 133, Name = "Dark Pact", Profession = Profession.Necromancer };
        public static Skill OrderofPain { get; } = new() { Id = 134, Name = "Order of Pain", Profession = Profession.Necromancer };
        public static Skill DarkBond { get; } = new() { Id = 138, Name = "Dark Bond", Profession = Profession.Necromancer };
        public static Skill StripEnchantment { get; } = new() { Id = 143, Name = "Strip Enchantment", Profession = Profession.Necromancer };
        public static Skill SignetofAgony { get; } = new() { Id = 145, Name = "Signet of Agony", Profession = Profession.Necromancer };
        public static Skill OfferingofBlood { get; } = new() { Id = 146, Name = "Offering of Blood", Profession = Profession.Necromancer };
        public static Skill DarkFury { get; } = new() { Id = 147, Name = "Dark Fury", Profession = Profession.Necromancer };
        public static Skill OrderoftheVampire { get; } = new() { Id = 148, Name = "Order of the Vampire", Profession = Profession.Necromancer };
        public static Skill VampiricGaze { get; } = new() { Id = 153, Name = "Vampiric Gaze", Profession = Profession.Necromancer };
        public static Skill VampiricTouch { get; } = new() { Id = 156, Name = "Vampiric Touch", Profession = Profession.Necromancer };
        public static Skill BloodRitual { get; } = new() { Id = 157, Name = "Blood Ritual", Profession = Profession.Necromancer };
        public static Skill TouchofAgony { get; } = new() { Id = 158, Name = "Touch of Agony", Profession = Profession.Necromancer };
        public static Skill JaundicedGaze { get; } = new() { Id = 763, Name = "Jaundiced Gaze", Profession = Profession.Necromancer };
        public static Skill CultistsFervor { get; } = new() { Id = 806, Name = "Cultist's Fervor", Profession = Profession.Necromancer };
        public static Skill VampiricSpirit { get; } = new() { Id = 819, Name = "Vampiric Spirit", Profession = Profession.Necromancer };
        public static Skill BloodBond { get; } = new() { Id = 835, Name = "Blood Bond", Profession = Profession.Necromancer };
        public static Skill RavenousGaze { get; } = new() { Id = 862, Name = "Ravenous Gaze", Profession = Profession.Necromancer };
        public static Skill OppressiveGaze { get; } = new() { Id = 864, Name = "Oppressive Gaze", Profession = Profession.Necromancer };
        public static Skill BloodoftheAggressor { get; } = new() { Id = 902, Name = "Blood of the Aggressor", Profession = Profession.Necromancer };
        public static Skill SpoilVictor { get; } = new() { Id = 1066, Name = "Spoil Victor", Profession = Profession.Necromancer };
        public static Skill LifebaneStrike { get; } = new() { Id = 1067, Name = "Lifebane Strike", Profession = Profession.Necromancer };
        public static Skill VampiricSwarm { get; } = new() { Id = 1075, Name = "Vampiric Swarm", Profession = Profession.Necromancer };
        public static Skill BloodDrinker { get; } = new() { Id = 1076, Name = "Blood Drinker", Profession = Profession.Necromancer };
        public static Skill VampiricBite { get; } = new() { Id = 1077, Name = "Vampiric Bite", Profession = Profession.Necromancer };
        public static Skill WallowsBite { get; } = new() { Id = 1078, Name = "Wallow's Bite", Profession = Profession.Necromancer };
        public static Skill MarkofFury { get; } = new() { Id = 1360, Name = "Mark of Fury", Profession = Profession.Necromancer };
        public static Skill SignetofSuffering { get; } = new() { Id = 1364, Name = "Signet of Suffering", Profession = Profession.Necromancer };
        public static Skill ParasiticBond { get; } = new() { Id = 99, Name = "Parasitic Bond", Profession = Profession.Necromancer };
        public static Skill SoulBarbs { get; } = new() { Id = 100, Name = "Soul Barbs", Profession = Profession.Necromancer };
        public static Skill Barbs { get; } = new() { Id = 101, Name = "Barbs", Profession = Profession.Necromancer };
        public static Skill PriceofFailure { get; } = new() { Id = 103, Name = "Price of Failure", Profession = Profession.Necromancer };
        public static Skill Suffering { get; } = new() { Id = 108, Name = "Suffering", Profession = Profession.Necromancer };
        public static Skill DesecrateEnchantments { get; } = new() { Id = 112, Name = "Desecrate Enchantments", Profession = Profession.Necromancer };
        public static Skill Enfeeble { get; } = new() { Id = 117, Name = "Enfeeble", Profession = Profession.Necromancer };
        public static Skill EnfeeblingBlood { get; } = new() { Id = 118, Name = "Enfeebling Blood", Profession = Profession.Necromancer };
        public static Skill SpitefulSpirit { get; } = new() { Id = 121, Name = "Spiteful Spirit", Profession = Profession.Necromancer };
        public static Skill InsidiousParasite { get; } = new() { Id = 123, Name = "Insidious Parasite", Profession = Profession.Necromancer };
        public static Skill SpinalShivers { get; } = new() { Id = 124, Name = "Spinal Shivers", Profession = Profession.Necromancer };
        public static Skill Wither { get; } = new() { Id = 125, Name = "Wither", Profession = Profession.Necromancer };
        public static Skill DefileFlesh { get; } = new() { Id = 129, Name = "Defile Flesh", Profession = Profession.Necromancer };
        public static Skill PlagueSignet { get; } = new() { Id = 132, Name = "Plague Signet", Profession = Profession.Necromancer };
        public static Skill Faintheartedness { get; } = new() { Id = 135, Name = "Faintheartedness", Profession = Profession.Necromancer };
        public static Skill ShadowofFear { get; } = new() { Id = 136, Name = "Shadow of Fear", Profession = Profession.Necromancer };
        public static Skill RigorMortis { get; } = new() { Id = 137, Name = "Rigor Mortis", Profession = Profession.Necromancer };
        public static Skill Malaise { get; } = new() { Id = 140, Name = "Malaise", Profession = Profession.Necromancer };
        public static Skill RendEnchantments { get; } = new() { Id = 141, Name = "Rend Enchantments", Profession = Profession.Necromancer };
        public static Skill LingeringCurse { get; } = new() { Id = 142, Name = "Lingering Curse", Profession = Profession.Necromancer };
        public static Skill Chilblains { get; } = new() { Id = 144, Name = "Chilblains", Profession = Profession.Necromancer };
        public static Skill PlagueSending { get; } = new() { Id = 149, Name = "Plague Sending", Profession = Profession.Necromancer };
        public static Skill MarkofPain { get; } = new() { Id = 150, Name = "Mark of Pain", Profession = Profession.Necromancer };
        public static Skill FeastofCorruption { get; } = new() { Id = 151, Name = "Feast of Corruption", Profession = Profession.Necromancer };
        public static Skill PlagueTouch { get; } = new() { Id = 154, Name = "Plague Touch", Profession = Profession.Necromancer };
        public static Skill WeakenArmor { get; } = new() { Id = 159, Name = "Weaken Armor", Profession = Profession.Necromancer };
        public static Skill WellofWeariness { get; } = new() { Id = 818, Name = "Well of Weariness", Profession = Profession.Necromancer };
        public static Skill Depravity { get; } = new() { Id = 820, Name = "Depravity", Profession = Profession.Necromancer };
        public static Skill WeakenKnees { get; } = new() { Id = 822, Name = "Weaken Knees", Profession = Profession.Necromancer };
        public static Skill RecklessHaste { get; } = new() { Id = 834, Name = "Reckless Haste", Profession = Profession.Necromancer };
        public static Skill PoisonedHeart { get; } = new() { Id = 840, Name = "Poisoned Heart", Profession = Profession.Necromancer };
        public static Skill OrderofApostasy { get; } = new() { Id = 863, Name = "Order of Apostasy", Profession = Profession.Necromancer };
        public static Skill VocalMinority { get; } = new() { Id = 883, Name = "Vocal Minority", Profession = Profession.Necromancer };
        public static Skill SoulBind { get; } = new() { Id = 901, Name = "Soul Bind", Profession = Profession.Necromancer };
        public static Skill EnvenomEnchantments { get; } = new() { Id = 936, Name = "Envenom Enchantments", Profession = Profession.Necromancer };
        public static Skill RipEnchantment { get; } = new() { Id = 955, Name = "Rip Enchantment", Profession = Profession.Necromancer };
        public static Skill DefileEnchantments { get; } = new() { Id = 1070, Name = "Defile Enchantments", Profession = Profession.Necromancer };
        public static Skill ShiversofDread { get; } = new() { Id = 1071, Name = "Shivers of Dread", Profession = Profession.Necromancer };
        public static Skill EnfeeblingTouch { get; } = new() { Id = 1079, Name = "Enfeebling Touch", Profession = Profession.Necromancer };
        public static Skill Meekness { get; } = new() { Id = 1260, Name = "Meekness", Profession = Profession.Necromancer };
        public static Skill UlcerousLungs { get; } = new() { Id = 1358, Name = "Ulcerous Lungs", Profession = Profession.Necromancer };
        public static Skill PainofDisenchantment { get; } = new() { Id = 1359, Name = "Pain of Disenchantment", Profession = Profession.Necromancer };
        public static Skill CorruptEnchantment { get; } = new() { Id = 1362, Name = "Corrupt Enchantment", Profession = Profession.Necromancer };
        public static Skill WellofDarkness { get; } = new() { Id = 1366, Name = "Well of Darkness", Profession = Profession.Necromancer };
        public static Skill WellofSilence { get; } = new() { Id = 1660, Name = "Well of Silence", Profession = Profession.Necromancer };
        public static Skill Cacophony { get; } = new() { Id = 1998, Name = "Cacophony", Profession = Profession.Necromancer };
        public static Skill DefileDefenses { get; } = new() { Id = 2188, Name = "Defile Defenses", Profession = Profession.Necromancer };
        public static Skill WellofRuin { get; } = new() { Id = 2236, Name = "Well of Ruin", Profession = Profession.Necromancer };
        public static Skill Atrophy { get; } = new() { Id = 2237, Name = "Atrophy", Profession = Profession.Necromancer };
        public static Skill AnimateBoneHorror { get; } = new() { Id = 83, Name = "Animate Bone Horror", Profession = Profession.Necromancer };
        public static Skill AnimateBoneFiend { get; } = new() { Id = 84, Name = "Animate Bone Fiend", Profession = Profession.Necromancer };
        public static Skill AnimateBoneMinions { get; } = new() { Id = 85, Name = "Animate Bone Minions", Profession = Profession.Necromancer };
        public static Skill VeratasGaze { get; } = new() { Id = 87, Name = "Verata's Gaze", Profession = Profession.Necromancer };
        public static Skill VeratasAura { get; } = new() { Id = 88, Name = "Verata's Aura", Profession = Profession.Necromancer };
        public static Skill DeathlyChill { get; } = new() { Id = 89, Name = "Deathly Chill", Profession = Profession.Necromancer };
        public static Skill VeratasSacrifice { get; } = new() { Id = 90, Name = "Verata's Sacrifice", Profession = Profession.Necromancer };
        public static Skill WellofSuffering { get; } = new() { Id = 93, Name = "Well of Suffering", Profession = Profession.Necromancer };
        public static Skill WelloftheProfane { get; } = new() { Id = 94, Name = "Well of the Profane", Profession = Profession.Necromancer };
        public static Skill PutridExplosion { get; } = new() { Id = 95, Name = "Putrid Explosion", Profession = Profession.Necromancer };
        public static Skill SoulFeast { get; } = new() { Id = 96, Name = "Soul Feast", Profession = Profession.Necromancer };
        public static Skill NecroticTraversal { get; } = new() { Id = 97, Name = "Necrotic Traversal", Profession = Profession.Necromancer };
        public static Skill ConsumeCorpse { get; } = new() { Id = 98, Name = "Consume Corpse", Profession = Profession.Necromancer };
        public static Skill DeathNova { get; } = new() { Id = 104, Name = "Death Nova", Profession = Profession.Necromancer };
        public static Skill DeathlySwarm { get; } = new() { Id = 105, Name = "Deathly Swarm", Profession = Profession.Necromancer };
        public static Skill RottingFlesh { get; } = new() { Id = 106, Name = "Rotting Flesh", Profession = Profession.Necromancer };
        public static Skill Virulence { get; } = new() { Id = 107, Name = "Virulence", Profession = Profession.Necromancer };
        public static Skill TaintedFlesh { get; } = new() { Id = 113, Name = "Tainted Flesh", Profession = Profession.Necromancer };
        public static Skill AuraoftheLich { get; } = new() { Id = 114, Name = "Aura of the Lich", Profession = Profession.Necromancer };
        public static Skill DarkAura { get; } = new() { Id = 116, Name = "Dark Aura", Profession = Profession.Necromancer };
        public static Skill BloodoftheMaster { get; } = new() { Id = 120, Name = "Blood of the Master", Profession = Profession.Necromancer };
        public static Skill MalignIntervention { get; } = new() { Id = 122, Name = "Malign Intervention", Profession = Profession.Necromancer };
        public static Skill InfuseCondition { get; } = new() { Id = 139, Name = "Infuse Condition", Profession = Profession.Necromancer };
        public static Skill TasteofDeath { get; } = new() { Id = 152, Name = "Taste of Death", Profession = Profession.Necromancer };
        public static Skill VileTouch { get; } = new() { Id = 155, Name = "Vile Touch", Profession = Profession.Necromancer };
        public static Skill AnimateVampiricHorror { get; } = new() { Id = 805, Name = "Animate Vampiric Horror", Profession = Profession.Necromancer };
        public static Skill Discord { get; } = new() { Id = 817, Name = "Discord", Profession = Profession.Necromancer };
        public static Skill VileMiasma { get; } = new() { Id = 828, Name = "Vile Miasma", Profession = Profession.Necromancer };
        public static Skill AnimateFleshGolem { get; } = new() { Id = 832, Name = "Animate Flesh Golem", Profession = Profession.Necromancer };
        public static Skill FetidGround { get; } = new() { Id = 841, Name = "Fetid Ground", Profession = Profession.Necromancer };
        public static Skill RisingBile { get; } = new() { Id = 935, Name = "Rising Bile", Profession = Profession.Necromancer };
        public static Skill BitterChill { get; } = new() { Id = 1068, Name = "Bitter Chill", Profession = Profession.Necromancer };
        public static Skill TasteofPain { get; } = new() { Id = 1069, Name = "Taste of Pain", Profession = Profession.Necromancer };
        public static Skill AnimateShamblingHorror { get; } = new() { Id = 1351, Name = "Animate Shambling Horror", Profession = Profession.Necromancer };
        public static Skill OrderofUndeath { get; } = new() { Id = 1352, Name = "Order of Undeath", Profession = Profession.Necromancer };
        public static Skill PutridFlesh { get; } = new() { Id = 1353, Name = "Putrid Flesh", Profession = Profession.Necromancer };
        public static Skill FeastfortheDead { get; } = new() { Id = 1354, Name = "Feast for the Dead", Profession = Profession.Necromancer };
        public static Skill JaggedBones { get; } = new() { Id = 1355, Name = "Jagged Bones", Profession = Profession.Necromancer };
        public static Skill Contagion { get; } = new() { Id = 1356, Name = "Contagion", Profession = Profession.Necromancer };
        public static Skill ToxicChill { get; } = new() { Id = 1659, Name = "Toxic Chill", Profession = Profession.Necromancer };
        public static Skill WitheringAura { get; } = new() { Id = 1997, Name = "Withering Aura", Profession = Profession.Necromancer };
        public static Skill PutridBile { get; } = new() { Id = 2058, Name = "Putrid Bile", Profession = Profession.Necromancer };
        public static Skill WailofDoom { get; } = new() { Id = 764, Name = "Wail of Doom", Profession = Profession.Necromancer };
        public static Skill ReapersMark { get; } = new() { Id = 808, Name = "Reaper's Mark", Profession = Profession.Necromancer };
        public static Skill IcyVeins { get; } = new() { Id = 821, Name = "Icy Veins", Profession = Profession.Necromancer };
        public static Skill SignetofSorrow { get; } = new() { Id = 1363, Name = "Signet of Sorrow", Profession = Profession.Necromancer };
        public static Skill SignetofLostSouls { get; } = new() { Id = 1365, Name = "Signet of Lost Souls", Profession = Profession.Necromancer };
        public static Skill FoulFeast { get; } = new() { Id = 2057, Name = "Foul Feast", Profession = Profession.Necromancer };
        public static Skill HexersVigor { get; } = new() { Id = 2138, Name = "Hexer's Vigor", Profession = Profession.Necromancer };
        public static Skill Masochism { get; } = new() { Id = 2139, Name = "Masochism", Profession = Profession.Necromancer };
        public static Skill AngorodonsGaze { get; } = new() { Id = 2189, Name = "Angorodon's Gaze", Profession = Profession.Necromancer };
        public static Skill GrenthsBalance { get; } = new() { Id = 86, Name = "Grenth's Balance", Profession = Profession.Necromancer };
        public static Skill GazeofContempt { get; } = new() { Id = 766, Name = "Gaze of Contempt", Profession = Profession.Necromancer };
        public static Skill PowerBlock { get; } = new() { Id = 5, Name = "Power Block", Profession = Profession.Mesmer };
        public static Skill HexBreaker { get; } = new() { Id = 10, Name = "Hex Breaker", Profession = Profession.Mesmer };
        public static Skill PowerSpike { get; } = new() { Id = 23, Name = "Power Spike", Profession = Profession.Mesmer };
        public static Skill PowerLeak { get; } = new() { Id = 24, Name = "Power Leak", Profession = Profession.Mesmer };
        public static Skill Empathy { get; } = new() { Id = 26, Name = "Empathy", Profession = Profession.Mesmer };
        public static Skill ShatterDelusions { get; } = new() { Id = 27, Name = "Shatter Delusions", Profession = Profession.Mesmer };
        public static Skill Backfire { get; } = new() { Id = 28, Name = "Backfire", Profession = Profession.Mesmer };
        public static Skill Blackout { get; } = new() { Id = 29, Name = "Blackout", Profession = Profession.Mesmer };
        public static Skill Diversion { get; } = new() { Id = 30, Name = "Diversion", Profession = Profession.Mesmer };
        public static Skill Ignorance { get; } = new() { Id = 35, Name = "Ignorance", Profession = Profession.Mesmer };
        public static Skill EnergySurge { get; } = new() { Id = 39, Name = "Energy Surge", Profession = Profession.Mesmer };
        public static Skill EnergyBurn { get; } = new() { Id = 42, Name = "Energy Burn", Profession = Profession.Mesmer };
        public static Skill Guilt { get; } = new() { Id = 46, Name = "Guilt", Profession = Profession.Mesmer };
        public static Skill MindWrack { get; } = new() { Id = 49, Name = "Mind Wrack", Profession = Profession.Mesmer };
        public static Skill WastrelsWorry { get; } = new() { Id = 50, Name = "Wastrel's Worry", Profession = Profession.Mesmer };
        public static Skill Shame { get; } = new() { Id = 51, Name = "Shame", Profession = Profession.Mesmer };
        public static Skill Panic { get; } = new() { Id = 52, Name = "Panic", Profession = Profession.Mesmer };
        public static Skill CryofFrustration { get; } = new() { Id = 57, Name = "Cry of Frustration", Profession = Profession.Mesmer };
        public static Skill SignetofWeariness { get; } = new() { Id = 59, Name = "Signet of Weariness", Profession = Profession.Mesmer };
        public static Skill ShatterHex { get; } = new() { Id = 67, Name = "Shatter Hex", Profession = Profession.Mesmer };
        public static Skill ShatterEnchantment { get; } = new() { Id = 69, Name = "Shatter Enchantment", Profession = Profession.Mesmer };
        public static Skill ChaosStorm { get; } = new() { Id = 77, Name = "Chaos Storm", Profession = Profession.Mesmer };
        public static Skill ArcaneThievery { get; } = new() { Id = 81, Name = "Arcane Thievery", Profession = Profession.Mesmer };
        public static Skill SignetofDisruption { get; } = new() { Id = 860, Name = "Signet of Disruption", Profession = Profession.Mesmer };
        public static Skill VisionsofRegret { get; } = new() { Id = 878, Name = "Visions of Regret", Profession = Profession.Mesmer };
        public static Skill Overload { get; } = new() { Id = 898, Name = "Overload", Profession = Profession.Mesmer };
        public static Skill Complicate { get; } = new() { Id = 932, Name = "Complicate", Profession = Profession.Mesmer };
        public static Skill UnnaturalSignet { get; } = new() { Id = 934, Name = "Unnatural Signet", Profession = Profession.Mesmer };
        public static Skill PowerFlux { get; } = new() { Id = 953, Name = "Power Flux", Profession = Profession.Mesmer };
        public static Skill Mistrust { get; } = new() { Id = 979, Name = "Mistrust", Profession = Profession.Mesmer };
        public static Skill PsychicDistraction { get; } = new() { Id = 1053, Name = "Psychic Distraction", Profession = Profession.Mesmer };
        public static Skill ArcaneLarceny { get; } = new() { Id = 1062, Name = "Arcane Larceny", Profession = Profession.Mesmer };
        public static Skill WastrelsDemise { get; } = new() { Id = 1335, Name = "Wastrel's Demise", Profession = Profession.Mesmer };
        public static Skill SpiritualPain { get; } = new() { Id = 1336, Name = "Spiritual Pain", Profession = Profession.Mesmer };
        public static Skill EnchantersConundrum { get; } = new() { Id = 1345, Name = "Enchanter's Conundrum", Profession = Profession.Mesmer };
        public static Skill HexEaterVortex { get; } = new() { Id = 1348, Name = "Hex Eater Vortex", Profession = Profession.Mesmer };
        public static Skill SimpleThievery { get; } = new() { Id = 1350, Name = "Simple Thievery", Profession = Profession.Mesmer };
        public static Skill PriceofPride { get; } = new() { Id = 1655, Name = "Price of Pride", Profession = Profession.Mesmer };
        public static Skill SignetofDistraction { get; } = new() { Id = 1992, Name = "Signet of Distraction", Profession = Profession.Mesmer };
        public static Skill PowerLock { get; } = new() { Id = 1994, Name = "Power Lock", Profession = Profession.Mesmer };
        public static Skill Aneurysm { get; } = new() { Id = 2055, Name = "Aneurysm", Profession = Profession.Mesmer };
        public static Skill MantraofRecovery { get; } = new() { Id = 13, Name = "Mantra of Recovery", Profession = Profession.Mesmer };
        public static Skill KeystoneSignet { get; } = new() { Id = 63, Name = "Keystone Signet", Profession = Profession.Mesmer };
        public static Skill ArcaneLanguor { get; } = new() { Id = 804, Name = "Arcane Languor", Profession = Profession.Mesmer };
        public static Skill StolenSpeed { get; } = new() { Id = 880, Name = "Stolen Speed", Profession = Profession.Mesmer };
        public static Skill PowerReturn { get; } = new() { Id = 931, Name = "Power Return", Profession = Profession.Mesmer };
        public static Skill PsychicInstability { get; } = new() { Id = 1057, Name = "Psychic Instability", Profession = Profession.Mesmer };
        public static Skill PersistenceofMemory { get; } = new() { Id = 1338, Name = "Persistence of Memory", Profession = Profession.Mesmer };
        public static Skill SymbolsofInspiration { get; } = new() { Id = 1339, Name = "Symbols of Inspiration", Profession = Profession.Mesmer };
        public static Skill SymbolicCelerity { get; } = new() { Id = 1340, Name = "Symbolic Celerity", Profession = Profession.Mesmer };
        public static Skill SymbolicPosture { get; } = new() { Id = 1658, Name = "Symbolic Posture", Profession = Profession.Mesmer };
        public static Skill Distortion { get; } = new() { Id = 11, Name = "Distortion", Profession = Profession.Mesmer };
        public static Skill Fragility { get; } = new() { Id = 19, Name = "Fragility", Profession = Profession.Mesmer };
        public static Skill ConjurePhantasm { get; } = new() { Id = 31, Name = "Conjure Phantasm", Profession = Profession.Mesmer };
        public static Skill IllusionofWeakness { get; } = new() { Id = 32, Name = "Illusion of Weakness", Profession = Profession.Mesmer };
        public static Skill IllusionaryWeaponry { get; } = new() { Id = 33, Name = "Illusionary Weaponry", Profession = Profession.Mesmer };
        public static Skill SympatheticVisage { get; } = new() { Id = 34, Name = "Sympathetic Visage", Profession = Profession.Mesmer };
        public static Skill ArcaneConundrum { get; } = new() { Id = 36, Name = "Arcane Conundrum", Profession = Profession.Mesmer };
        public static Skill IllusionofHaste { get; } = new() { Id = 37, Name = "Illusion of Haste", Profession = Profession.Mesmer };
        public static Skill Clumsiness { get; } = new() { Id = 43, Name = "Clumsiness", Profession = Profession.Mesmer };
        public static Skill PhantomPain { get; } = new() { Id = 44, Name = "Phantom Pain", Profession = Profession.Mesmer };
        public static Skill EtherealBurden { get; } = new() { Id = 45, Name = "Ethereal Burden", Profession = Profession.Mesmer };
        public static Skill Ineptitude { get; } = new() { Id = 47, Name = "Ineptitude", Profession = Profession.Mesmer };
        public static Skill Migraine { get; } = new() { Id = 53, Name = "Migraine", Profession = Profession.Mesmer };
        public static Skill CripplingAnguish { get; } = new() { Id = 54, Name = "Crippling Anguish", Profession = Profession.Mesmer };
        public static Skill FeveredDreams { get; } = new() { Id = 55, Name = "Fevered Dreams", Profession = Profession.Mesmer };
        public static Skill SoothingImages { get; } = new() { Id = 56, Name = "Soothing Images", Profession = Profession.Mesmer };
        public static Skill ImaginedBurden { get; } = new() { Id = 76, Name = "Imagined Burden", Profession = Profession.Mesmer };
        public static Skill ConjureNightmare { get; } = new() { Id = 859, Name = "Conjure Nightmare", Profession = Profession.Mesmer };
        public static Skill IllusionofPain { get; } = new() { Id = 879, Name = "Illusion of Pain", Profession = Profession.Mesmer };
        public static Skill ImagesofRemorse { get; } = new() { Id = 899, Name = "Images of Remorse", Profession = Profession.Mesmer };
        public static Skill SharedBurden { get; } = new() { Id = 900, Name = "Shared Burden", Profession = Profession.Mesmer };
        public static Skill AccumulatedPain { get; } = new() { Id = 1052, Name = "Accumulated Pain", Profession = Profession.Mesmer };
        public static Skill AncestorsVisage { get; } = new() { Id = 1054, Name = "Ancestor's Visage", Profession = Profession.Mesmer };
        public static Skill RecurringInsecurity { get; } = new() { Id = 1055, Name = "Recurring Insecurity", Profession = Profession.Mesmer };
        public static Skill KitahsBurden { get; } = new() { Id = 1056, Name = "Kitah's Burden", Profession = Profession.Mesmer };
        public static Skill Frustration { get; } = new() { Id = 1341, Name = "Frustration", Profession = Profession.Mesmer };
        public static Skill SignetofIllusions { get; } = new() { Id = 1346, Name = "Signet of Illusions", Profession = Profession.Mesmer };
        public static Skill AirofDisenchantment { get; } = new() { Id = 1656, Name = "Air of Disenchantment", Profession = Profession.Mesmer };
        public static Skill SignetofClumsiness { get; } = new() { Id = 1657, Name = "Signet of Clumsiness", Profession = Profession.Mesmer };
        public static Skill SumofAllFears { get; } = new() { Id = 1996, Name = "Sum of All Fears", Profession = Profession.Mesmer };
        public static Skill CalculatedRisk { get; } = new() { Id = 2053, Name = "Calculated Risk", Profession = Profession.Mesmer };
        public static Skill ShrinkingArmor { get; } = new() { Id = 2054, Name = "Shrinking Armor", Profession = Profession.Mesmer };
        public static Skill WanderingEye { get; } = new() { Id = 2056, Name = "Wandering Eye", Profession = Profession.Mesmer };
        public static Skill ConfusingImages { get; } = new() { Id = 2137, Name = "Confusing Images", Profession = Profession.Mesmer };
        public static Skill MantraofEarth { get; } = new() { Id = 6, Name = "Mantra of Earth", Profession = Profession.Mesmer };
        public static Skill MantraofFlame { get; } = new() { Id = 7, Name = "Mantra of Flame", Profession = Profession.Mesmer };
        public static Skill MantraofFrost { get; } = new() { Id = 8, Name = "Mantra of Frost", Profession = Profession.Mesmer };
        public static Skill MantraofLightning { get; } = new() { Id = 9, Name = "Mantra of Lightning", Profession = Profession.Mesmer };
        public static Skill MantraofPersistence { get; } = new() { Id = 14, Name = "Mantra of Persistence", Profession = Profession.Mesmer };
        public static Skill MantraofInscriptions { get; } = new() { Id = 15, Name = "Mantra of Inscriptions", Profession = Profession.Mesmer };
        public static Skill MantraofConcentration { get; } = new() { Id = 16, Name = "Mantra of Concentration", Profession = Profession.Mesmer };
        public static Skill MantraofResolve { get; } = new() { Id = 17, Name = "Mantra of Resolve", Profession = Profession.Mesmer };
        public static Skill MantraofSignets { get; } = new() { Id = 18, Name = "Mantra of Signets", Profession = Profession.Mesmer };
        public static Skill InspiredEnchantment { get; } = new() { Id = 21, Name = "Inspired Enchantment", Profession = Profession.Mesmer };
        public static Skill InspiredHex { get; } = new() { Id = 22, Name = "Inspired Hex", Profession = Profession.Mesmer };
        public static Skill PowerDrain { get; } = new() { Id = 25, Name = "Power Drain", Profession = Profession.Mesmer };
        public static Skill Channeling { get; } = new() { Id = 38, Name = "Channeling", Profession = Profession.Mesmer };
        public static Skill EtherFeast { get; } = new() { Id = 40, Name = "Ether Feast", Profession = Profession.Mesmer };
        public static Skill EtherLord { get; } = new() { Id = 41, Name = "Ether Lord", Profession = Profession.Mesmer };
        public static Skill SpiritofFailure { get; } = new() { Id = 48, Name = "Spirit of Failure", Profession = Profession.Mesmer };
        public static Skill LeechSignet { get; } = new() { Id = 61, Name = "Leech Signet", Profession = Profession.Mesmer };
        public static Skill SignetofHumility { get; } = new() { Id = 62, Name = "Signet of Humility", Profession = Profession.Mesmer };
        public static Skill SpiritShackles { get; } = new() { Id = 66, Name = "Spirit Shackles", Profession = Profession.Mesmer };
        public static Skill DrainEnchantment { get; } = new() { Id = 68, Name = "Drain Enchantment", Profession = Profession.Mesmer };
        public static Skill ElementalResistance { get; } = new() { Id = 72, Name = "Elemental Resistance", Profession = Profession.Mesmer };
        public static Skill PhysicalResistance { get; } = new() { Id = 73, Name = "Physical Resistance", Profession = Profession.Mesmer };
        public static Skill EnergyDrain { get; } = new() { Id = 79, Name = "Energy Drain", Profession = Profession.Mesmer };
        public static Skill EnergyTap { get; } = new() { Id = 80, Name = "Energy Tap", Profession = Profession.Mesmer };
        public static Skill MantraofRecall { get; } = new() { Id = 82, Name = "Mantra of Recall", Profession = Profession.Mesmer };
        public static Skill PowerLeech { get; } = new() { Id = 803, Name = "Power Leech", Profession = Profession.Mesmer };
        public static Skill LyssasAura { get; } = new() { Id = 813, Name = "Lyssa's Aura", Profession = Profession.Mesmer };
        public static Skill EtherSignet { get; } = new() { Id = 881, Name = "Ether Signet", Profession = Profession.Mesmer };
        public static Skill AuspiciousIncantation { get; } = new() { Id = 930, Name = "Auspicious Incantation", Profession = Profession.Mesmer };
        public static Skill RevealedEnchantment { get; } = new() { Id = 1048, Name = "Revealed Enchantment", Profession = Profession.Mesmer };
        public static Skill RevealedHex { get; } = new() { Id = 1049, Name = "Revealed Hex", Profession = Profession.Mesmer };
        public static Skill HexEaterSignet { get; } = new() { Id = 1059, Name = "Hex Eater Signet", Profession = Profession.Mesmer };
        public static Skill Feedback { get; } = new() { Id = 1061, Name = "Feedback", Profession = Profession.Mesmer };
        public static Skill ExtendConditions { get; } = new() { Id = 1333, Name = "Extend Conditions", Profession = Profession.Mesmer };
        public static Skill DrainDelusions { get; } = new() { Id = 1337, Name = "Drain Delusions", Profession = Profession.Mesmer };
        public static Skill Tease { get; } = new() { Id = 1342, Name = "Tease", Profession = Profession.Mesmer };
        public static Skill EtherPhantom { get; } = new() { Id = 1343, Name = "Ether Phantom", Profession = Profession.Mesmer };
        public static Skill DischargeEnchantment { get; } = new() { Id = 1347, Name = "Discharge Enchantment", Profession = Profession.Mesmer };
        public static Skill SignetofRecall { get; } = new() { Id = 1993, Name = "Signet of Recall", Profession = Profession.Mesmer };
        public static Skill WasteNotWantNot { get; } = new() { Id = 1995, Name = "Waste Not, Want Not", Profession = Profession.Mesmer };
        public static Skill SignetofMidnight { get; } = new() { Id = 58, Name = "Signet of Midnight", Profession = Profession.Mesmer };
        public static Skill ArcaneMimicry { get; } = new() { Id = 65, Name = "Arcane Mimicry", Profession = Profession.Mesmer };
        public static Skill Echo { get; } = new() { Id = 74, Name = "Echo", Profession = Profession.Mesmer };
        public static Skill ArcaneEcho { get; } = new() { Id = 75, Name = "Arcane Echo", Profession = Profession.Mesmer };
        public static Skill Epidemic { get; } = new() { Id = 78, Name = "Epidemic", Profession = Profession.Mesmer };
        public static Skill LyssasBalance { get; } = new() { Id = 877, Name = "Lyssa's Balance, Profession = Profession.Mesmer" };
        public static Skill SignetofDisenchantment { get; } = new() { Id = 882, Name = "Signet of Disenchantment", Profession = Profession.Mesmer };
        public static Skill ShatterStorm { get; } = new() { Id = 933, Name = "Shatter Storm", Profession = Profession.Mesmer };
        public static Skill ExpelHexes { get; } = new() { Id = 954, Name = "Expel Hexes", Profession = Profession.Mesmer };
        public static Skill Hypochondria { get; } = new() { Id = 1334, Name = "Hypochondria", Profession = Profession.Mesmer };
        public static Skill WebofDisruption { get; } = new() { Id = 1344, Name = "Web of Disruption", Profession = Profession.Mesmer };
        public static Skill MirrorofDisenchantment { get; } = new() { Id = 1349, Name = "Mirror of Disenchantment", Profession = Profession.Mesmer };
        public static Skill WindborneSpeed { get; } = new() { Id = 160, Name = "Windborne Speed", Profession = Profession.Elementalist };
        public static Skill Gale { get; } = new() { Id = 162, Name = "Gale", Profession = Profession.Elementalist };
        public static Skill Whirlwind { get; } = new() { Id = 163, Name = "Whirlwind", Profession = Profession.Elementalist };
        public static Skill LightningSurge { get; } = new() { Id = 205, Name = "Lightning Surge", Profession = Profession.Elementalist };
        public static Skill BlindingFlash { get; } = new() { Id = 220, Name = "Blinding Flash", Profession = Profession.Elementalist };
        public static Skill ConjureLightning { get; } = new() { Id = 221, Name = "Conjure Lightning", Profession = Profession.Elementalist };
        public static Skill LightningStrike { get; } = new() { Id = 222, Name = "Lightning Strike", Profession = Profession.Elementalist };
        public static Skill ChainLightning { get; } = new() { Id = 223, Name = "Chain Lightning", Profession = Profession.Elementalist };
        public static Skill EnervatingCharge { get; } = new() { Id = 224, Name = "Enervating Charge", Profession = Profession.Elementalist };
        public static Skill AirAttunement { get; } = new() { Id = 225, Name = "Air Attunement", Profession = Profession.Elementalist };
        public static Skill MindShock { get; } = new() { Id = 226, Name = "Mind Shock", Profession = Profession.Elementalist };
        public static Skill GlimmeringMark { get; } = new() { Id = 227, Name = "Glimmering Mark", Profession = Profession.Elementalist };
        public static Skill Thunderclap { get; } = new() { Id = 228, Name = "Thunderclap", Profession = Profession.Elementalist };
        public static Skill LightningOrb { get; } = new() { Id = 229, Name = "Lightning Orb", Profession = Profession.Elementalist };
        public static Skill LightningJavelin { get; } = new() { Id = 230, Name = "Lightning Javelin", Profession = Profession.Elementalist };
        public static Skill Shock { get; } = new() { Id = 231, Name = "Shock", Profession = Profession.Elementalist };
        public static Skill LightningTouch { get; } = new() { Id = 232, Name = "Lightning Touch", Profession = Profession.Elementalist };
        public static Skill RidetheLightning { get; } = new() { Id = 836, Name = "Ride the Lightning", Profession = Profession.Elementalist };
        public static Skill ArcLightning { get; } = new() { Id = 842, Name = "Arc Lightning", Profession = Profession.Elementalist };
        public static Skill Gust { get; } = new() { Id = 843, Name = "Gust", Profession = Profession.Elementalist };
        public static Skill LightningHammer { get; } = new() { Id = 865, Name = "Lightning Hammer", Profession = Profession.Elementalist };
        public static Skill TeinaisWind { get; } = new() { Id = 1081, Name = "Teinai's Wind", Profession = Profession.Elementalist };
        public static Skill ShockArrow { get; } = new() { Id = 1082, Name = "Shock Arrow", Profession = Profession.Elementalist };
        public static Skill BlindingSurge { get; } = new() { Id = 1367, Name = "Blinding Surge", Profession = Profession.Elementalist };
        public static Skill ChillingWinds { get; } = new() { Id = 1368, Name = "Chilling Winds", Profession = Profession.Elementalist };
        public static Skill LightningBolt { get; } = new() { Id = 1369, Name = "Lightning Bolt", Profession = Profession.Elementalist };
        public static Skill StormDjinnsHaste { get; } = new() { Id = 1370, Name = "Storm Djinn's Haste", Profession = Profession.Elementalist };
        public static Skill InvokeLightning { get; } = new() { Id = 1664, Name = "Invoke Lightning", Profession = Profession.Elementalist };
        public static Skill GlyphofSwiftness { get; } = new() { Id = 2002, Name = "Glyph of Swiftness", Profession = Profession.Elementalist };
        public static Skill ShellShock { get; } = new() { Id = 2059, Name = "Shell Shock", Profession = Profession.Elementalist };
        public static Skill ArmorofEarth { get; } = new() { Id = 165, Name = "Armor of Earth", Profession = Profession.Elementalist };
        public static Skill KineticArmor { get; } = new() { Id = 166, Name = "Kinetic Armor", Profession = Profession.Elementalist };
        public static Skill Eruption { get; } = new() { Id = 167, Name = "Eruption", Profession = Profession.Elementalist };
        public static Skill MagneticAura { get; } = new() { Id = 168, Name = "Magnetic Aura", Profession = Profession.Elementalist };
        public static Skill EarthAttunement { get; } = new() { Id = 169, Name = "Earth Attunement", Profession = Profession.Elementalist };
        public static Skill Earthquake { get; } = new() { Id = 170, Name = "Earthquake", Profession = Profession.Elementalist };
        public static Skill Stoning { get; } = new() { Id = 171, Name = "Stoning", Profession = Profession.Elementalist };
        public static Skill StoneDaggers { get; } = new() { Id = 172, Name = "Stone Daggers", Profession = Profession.Elementalist };
        public static Skill GraspingEarth { get; } = new() { Id = 173, Name = "Grasping Earth", Profession = Profession.Elementalist };
        public static Skill Aftershock { get; } = new() { Id = 174, Name = "Aftershock", Profession = Profession.Elementalist };
        public static Skill WardAgainstElements { get; } = new() { Id = 175, Name = "Ward Against Elements", Profession = Profession.Elementalist };
        public static Skill WardAgainstMelee { get; } = new() { Id = 176, Name = "Ward Against Melee", Profession = Profession.Elementalist };
        public static Skill WardAgainstFoes { get; } = new() { Id = 177, Name = "Ward Against Foes", Profession = Profession.Elementalist };
        public static Skill IronMist { get; } = new() { Id = 216, Name = "Iron Mist", Profession = Profession.Elementalist };
        public static Skill CrystalWave { get; } = new() { Id = 217, Name = "Crystal Wave", Profession = Profession.Elementalist };
        public static Skill ObsidianFlesh { get; } = new() { Id = 218, Name = "Obsidian Flesh", Profession = Profession.Elementalist };
        public static Skill ObsidianFlame { get; } = new() { Id = 219, Name = "Obsidian Flame", Profession = Profession.Elementalist };
        public static Skill ChurningEarth { get; } = new() { Id = 844, Name = "Churning Earth", Profession = Profession.Elementalist };
        public static Skill Shockwave { get; } = new() { Id = 937, Name = "Shockwave", Profession = Profession.Elementalist };
        public static Skill WardofStability { get; } = new() { Id = 938, Name = "Ward of Stability", Profession = Profession.Elementalist };
        public static Skill UnsteadyGround { get; } = new() { Id = 1083, Name = "Unsteady Ground", Profession = Profession.Elementalist };
        public static Skill SliverArmor { get; } = new() { Id = 1084, Name = "Sliver Armor", Profession = Profession.Elementalist };
        public static Skill AshBlast { get; } = new() { Id = 1085, Name = "Ash Blast", Profession = Profession.Elementalist };
        public static Skill DragonsStomp { get; } = new() { Id = 1086, Name = "Dragon's Stomp", Profession = Profession.Elementalist };
        public static Skill TeinaisCrystals { get; } = new() { Id = 1099, Name = "Teinai's Crystals", Profession = Profession.Elementalist };
        public static Skill StoneStriker { get; } = new() { Id = 1371, Name = "Stone Striker", Profession = Profession.Elementalist };
        public static Skill Sandstorm { get; } = new() { Id = 1372, Name = "Sandstorm", Profession = Profession.Elementalist };
        public static Skill StoneSheath { get; } = new() { Id = 1373, Name = "Stone Sheath", Profession = Profession.Elementalist };
        public static Skill EbonHawk { get; } = new() { Id = 1374, Name = "Ebon Hawk", Profession = Profession.Elementalist };
        public static Skill StonefleshAura { get; } = new() { Id = 1375, Name = "Stoneflesh Aura", Profession = Profession.Elementalist };
        public static Skill Glowstone { get; } = new() { Id = 1661, Name = "Glowstone", Profession = Profession.Elementalist };
        public static Skill EarthenShackles { get; } = new() { Id = 2000, Name = "Earthen Shackles", Profession = Profession.Elementalist };
        public static Skill WardofWeakness { get; } = new() { Id = 2001, Name = "Ward of Weakness", Profession = Profession.Elementalist };
        public static Skill MagneticSurge { get; } = new() { Id = 2190, Name = "Magnetic Surge", Profession = Profession.Elementalist };
        public static Skill ElementalAttunement { get; } = new() { Id = 164, Name = "Elemental Attunement", Profession = Profession.Elementalist };
        public static Skill EtherProdigy { get; } = new() { Id = 178, Name = "Ether Prodigy", Profession = Profession.Elementalist };
        public static Skill AuraofRestoration { get; } = new() { Id = 180, Name = "Aura of Restoration", Profession = Profession.Elementalist };
        public static Skill EtherRenewal { get; } = new() { Id = 181, Name = "Ether Renewal", Profession = Profession.Elementalist };
        public static Skill GlyphofEnergy { get; } = new() { Id = 199, Name = "Glyph of Energy", Profession = Profession.Elementalist };
        public static Skill GlyphofLesserEnergy { get; } = new() { Id = 200, Name = "Glyph of Lesser Energy", Profession = Profession.Elementalist };
        public static Skill EnergyBoon { get; } = new() { Id = 837, Name = "Energy Boon", Profession = Profession.Elementalist };
        public static Skill GlyphofRestoration { get; } = new() { Id = 1376, Name = "Glyph of Restoration", Profession = Profession.Elementalist };
        public static Skill EtherPrism { get; } = new() { Id = 1377, Name = "Ether Prism", Profession = Profession.Elementalist };
        public static Skill MasterofMagic { get; } = new() { Id = 1378, Name = "Master of Magic", Profession = Profession.Elementalist };
        public static Skill EnergyBlast { get; } = new() { Id = 2193, Name = "Energy Blast", Profession = Profession.Elementalist };
        public static Skill IncendiaryBonds { get; } = new() { Id = 179, Name = "Incendiary Bonds", Profession = Profession.Elementalist };
        public static Skill ConjureFlame { get; } = new() { Id = 182, Name = "Conjure Flame", Profession = Profession.Elementalist };
        public static Skill Inferno { get; } = new() { Id = 183, Name = "Inferno", Profession = Profession.Elementalist };
        public static Skill FireAttunement { get; } = new() { Id = 184, Name = "Fire Attunement", Profession = Profession.Elementalist };
        public static Skill MindBurn { get; } = new() { Id = 185, Name = "Mind Burn", Profession = Profession.Elementalist };
        public static Skill Fireball { get; } = new() { Id = 186, Name = "Fireball", Profession = Profession.Elementalist };
        public static Skill Meteor { get; } = new() { Id = 187, Name = "Meteor", Profession = Profession.Elementalist };
        public static Skill FlameBurst { get; } = new() { Id = 188, Name = "Flame Burst", Profession = Profession.Elementalist };
        public static Skill RodgortsInvocation { get; } = new() { Id = 189, Name = "Rodgort's Invocation", Profession = Profession.Elementalist };
        public static Skill MarkofRodgort { get; } = new() { Id = 190, Name = "Mark of Rodgort", Profession = Profession.Elementalist };
        public static Skill Immolate { get; } = new() { Id = 191, Name = "Immolate", Profession = Profession.Elementalist };
        public static Skill MeteorShower { get; } = new() { Id = 192, Name = "Meteor Shower", Profession = Profession.Elementalist };
        public static Skill Phoenix { get; } = new() { Id = 193, Name = "Phoenix", Profession = Profession.Elementalist };
        public static Skill Flare { get; } = new() { Id = 194, Name = "Flare", Profession = Profession.Elementalist };
        public static Skill LavaFont { get; } = new() { Id = 195, Name = "Lava Font", Profession = Profession.Elementalist };
        public static Skill SearingHeat { get; } = new() { Id = 196, Name = "Searing Heat", Profession = Profession.Elementalist };
        public static Skill FireStorm { get; } = new() { Id = 197, Name = "Fire Storm", Profession = Profession.Elementalist };
        public static Skill BurningSpeed { get; } = new() { Id = 823, Name = "Burning Speed", Profession = Profession.Elementalist };
        public static Skill LavaArrows { get; } = new() { Id = 824, Name = "Lava Arrows", Profession = Profession.Elementalist };
        public static Skill BedofCoals { get; } = new() { Id = 825, Name = "Bed of Coals", Profession = Profession.Elementalist };
        public static Skill LiquidFlame { get; } = new() { Id = 845, Name = "Liquid Flame", Profession = Profession.Elementalist };
        public static Skill SearingFlames { get; } = new() { Id = 884, Name = "Searing Flames", Profession = Profession.Elementalist };
        public static Skill SmolderingEmbers { get; } = new() { Id = 1090, Name = "Smoldering Embers", Profession = Profession.Elementalist };
        public static Skill DoubleDragon { get; } = new() { Id = 1091, Name = "Double Dragon", Profession = Profession.Elementalist };
        public static Skill TeinaisHeat { get; } = new() { Id = 1093, Name = "Teinai's Heat", Profession = Profession.Elementalist };
        public static Skill BreathofFire { get; } = new() { Id = 1094, Name = "Breath of Fire", Profession = Profession.Elementalist };
        public static Skill StarBurst { get; } = new() { Id = 1095, Name = "Star Burst", Profession = Profession.Elementalist };
        public static Skill GlowingGaze { get; } = new() { Id = 1379, Name = "Glowing Gaze", Profession = Profession.Elementalist };
        public static Skill SavannahHeat { get; } = new() { Id = 1380, Name = "Savannah Heat", Profession = Profession.Elementalist };
        public static Skill FlameDjinnsHaste { get; } = new() { Id = 1381, Name = "Flame Djinn's Haste", Profession = Profession.Elementalist };
        public static Skill MindBlast { get; } = new() { Id = 1662, Name = "Mind Blast", Profession = Profession.Elementalist };
        public static Skill ElementalFlame { get; } = new() { Id = 1663, Name = "Elemental Flame", Profession = Profession.Elementalist };
        public static Skill GlyphofImmolation { get; } = new() { Id = 2060, Name = "Glyph of Immolation", Profession = Profession.Elementalist };
        public static Skill Rust { get; } = new() { Id = 204, Name = "Rust", Profession = Profession.Elementalist };
        public static Skill ArmorofFrost { get; } = new() { Id = 206, Name = "Armor of Frost", Profession = Profession.Elementalist };
        public static Skill ConjureFrost { get; } = new() { Id = 207, Name = "Conjure Frost", Profession = Profession.Elementalist };
        public static Skill WaterAttunement { get; } = new() { Id = 208, Name = "Water Attunement", Profession = Profession.Elementalist };
        public static Skill MindFreeze { get; } = new() { Id = 209, Name = "Mind Freeze", Profession = Profession.Elementalist };
        public static Skill IcePrison { get; } = new() { Id = 210, Name = "Ice Prison", Profession = Profession.Elementalist };
        public static Skill IceSpikes { get; } = new() { Id = 211, Name = "Ice Spikes", Profession = Profession.Elementalist };
        public static Skill FrozenBurst { get; } = new() { Id = 212, Name = "Frozen Burst", Profession = Profession.Elementalist };
        public static Skill ShardStorm { get; } = new() { Id = 213, Name = "Shard Storm", Profession = Profession.Elementalist };
        public static Skill IceSpear { get; } = new() { Id = 214, Name = "Ice Spear", Profession = Profession.Elementalist };
        public static Skill Maelstrom { get; } = new() { Id = 215, Name = "Maelstrom", Profession = Profession.Elementalist };
        public static Skill SwirlingAura { get; } = new() { Id = 233, Name = "Swirling Aura", Profession = Profession.Elementalist };
        public static Skill DeepFreeze { get; } = new() { Id = 234, Name = "Deep Freeze", Profession = Profession.Elementalist };
        public static Skill BlurredVision { get; } = new() { Id = 235, Name = "Blurred Vision", Profession = Profession.Elementalist };
        public static Skill MistForm { get; } = new() { Id = 236, Name = "Mist Form", Profession = Profession.Elementalist };
        public static Skill WaterTrident { get; } = new() { Id = 237, Name = "Water Trident", Profession = Profession.Elementalist };
        public static Skill ArmorofMist { get; } = new() { Id = 238, Name = "Armor of Mist", Profession = Profession.Elementalist };
        public static Skill WardAgainstHarm { get; } = new() { Id = 239, Name = "Ward Against Harm", Profession = Profession.Elementalist };
        public static Skill Shatterstone { get; } = new() { Id = 809, Name = "Shatterstone", Profession = Profession.Elementalist };
        public static Skill Steam { get; } = new() { Id = 846, Name = "Steam", Profession = Profession.Elementalist };
        public static Skill VaporBlade { get; } = new() { Id = 866, Name = "Vapor Blade", Profession = Profession.Elementalist };
        public static Skill IcyPrism { get; } = new() { Id = 903, Name = "Icy Prism", Profession = Profession.Elementalist };
        public static Skill IcyShackles { get; } = new() { Id = 939, Name = "Icy Shackles", Profession = Profession.Elementalist };
        public static Skill TeinaisPrison { get; } = new() { Id = 1097, Name = "Teinai's Prison", Profession = Profession.Elementalist };
        public static Skill MirrorofIce { get; } = new() { Id = 1098, Name = "Mirror of Ice", Profession = Profession.Elementalist };
        public static Skill FrigidArmor { get; } = new() { Id = 1261, Name = "Frigid Armor", Profession = Profession.Elementalist };
        public static Skill FreezingGust { get; } = new() { Id = 1382, Name = "Freezing Gust", Profession = Profession.Elementalist };
        public static Skill WintersEmbrace { get; } = new() { Id = 1999, Name = "Winter's Embrace", Profession = Profession.Elementalist };
        public static Skill SlipperyGround { get; } = new() { Id = 2191, Name = "Slippery Ground", Profession = Profession.Elementalist };
        public static Skill GlowingIce { get; } = new() { Id = 2192, Name = "Glowing Ice", Profession = Profession.Elementalist };
        public static Skill GlyphofElementalPower { get; } = new() { Id = 198, Name = "Glyph of Elemental Power", Profession = Profession.Elementalist };
        public static Skill GlyphofConcentration { get; } = new() { Id = 201, Name = "Glyph of Concentration", Profession = Profession.Elementalist };
        public static Skill GlyphofSacrifice { get; } = new() { Id = 202, Name = "Glyph of Sacrifice", Profession = Profession.Elementalist };
        public static Skill GlyphofRenewal { get; } = new() { Id = 203, Name = "Glyph of Renewal", Profession = Profession.Elementalist };
        public static Skill SecondWind { get; } = new() { Id = 1088, Name = "Second Wind", Profession = Profession.Elementalist };
        public static Skill GlyphofEssence { get; } = new() { Id = 1096, Name = "Glyph of Essence", Profession = Profession.Elementalist };
        public static Skill TwistingFangs { get; } = new() { Id = 776, Name = "Twisting Fangs", Profession = Profession.Assassin };
        public static Skill BlackLotusStrike { get; } = new() { Id = 779, Name = "Black Lotus Strike", Profession = Profession.Assassin };
        public static Skill UnsuspectingStrike { get; } = new() { Id = 783, Name = "Unsuspecting Strike", Profession = Profession.Assassin };
        public static Skill SharpenDaggers { get; } = new() { Id = 926, Name = "Sharpen Daggers", Profession = Profession.Assassin };
        public static Skill CriticalEye { get; } = new() { Id = 1018, Name = "Critical Eye", Profession = Profession.Assassin };
        public static Skill CriticalStrike { get; } = new() { Id = 1019, Name = "Critical Strike", Profession = Profession.Assassin };
        public static Skill CriticalDefenses { get; } = new() { Id = 1027, Name = "Critical Defenses", Profession = Profession.Assassin };
        public static Skill DarkApostasy { get; } = new() { Id = 1029, Name = "Dark Apostasy", Profession = Profession.Assassin };
        public static Skill LocustsFury { get; } = new() { Id = 1030, Name = "Locust's Fury", Profession = Profession.Assassin };
        public static Skill SeepingWound { get; } = new() { Id = 1034, Name = "Seeping Wound", Profession = Profession.Assassin };
        public static Skill PalmStrike { get; } = new() { Id = 1045, Name = "Palm Strike", Profession = Profession.Assassin };
        public static Skill MaliciousStrike { get; } = new() { Id = 1633, Name = "Malicious Strike", Profession = Profession.Assassin };
        public static Skill ShatteringAssault { get; } = new() { Id = 1634, Name = "Shattering Assault", Profession = Profession.Assassin };
        public static Skill DeadlyHaste { get; } = new() { Id = 1638, Name = "Deadly Haste", Profession = Profession.Assassin };
        public static Skill AssassinsRemedy { get; } = new() { Id = 1639, Name = "Assassin's Remedy", Profession = Profession.Assassin };
        public static Skill WayoftheAssassin { get; } = new() { Id = 1649, Name = "Way of the Assassin", Profession = Profession.Assassin };
        public static Skill WayoftheMaster { get; } = new() { Id = 2187, Name = "Way of the Master", Profession = Profession.Assassin };
        public static Skill DeathBlossom { get; } = new() { Id = 775, Name = "Death Blossom", Profession = Profession.Assassin };
        public static Skill HornsoftheOx { get; } = new() { Id = 777, Name = "Horns of the Ox", Profession = Profession.Assassin };
        public static Skill FallingSpider { get; } = new() { Id = 778, Name = "Falling Spider", Profession = Profession.Assassin };
        public static Skill FoxFangs { get; } = new() { Id = 780, Name = "Fox Fangs", Profession = Profession.Assassin };
        public static Skill MoebiusStrike { get; } = new() { Id = 781, Name = "Moebius Strike", Profession = Profession.Assassin };
        public static Skill JaggedStrike { get; } = new() { Id = 782, Name = "Jagged Strike", Profession = Profession.Assassin };
        public static Skill DesperateStrike { get; } = new() { Id = 948, Name = "Desperate Strike", Profession = Profession.Assassin };
        public static Skill ExhaustingAssault { get; } = new() { Id = 975, Name = "Exhausting Assault", Profession = Profession.Assassin };
        public static Skill RepeatingStrike { get; } = new() { Id = 976, Name = "Repeating Strike", Profession = Profession.Assassin };
        public static Skill NineTailStrike { get; } = new() { Id = 986, Name = "Nine Tail Strike", Profession = Profession.Assassin };
        public static Skill TempleStrike { get; } = new() { Id = 988, Name = "Temple Strike", Profession = Profession.Assassin };
        public static Skill GoldenPhoenixStrike { get; } = new() { Id = 989, Name = "Golden Phoenix Strike", Profession = Profession.Assassin };
        public static Skill BladesofSteel { get; } = new() { Id = 1020, Name = "Blades of Steel", Profession = Profession.Assassin };
        public static Skill JungleStrike { get; } = new() { Id = 1021, Name = "Jungle Strike", Profession = Profession.Assassin };
        public static Skill WildStrike { get; } = new() { Id = 1022, Name = "Wild Strike", Profession = Profession.Assassin };
        public static Skill LeapingMantisSting { get; } = new() { Id = 1023, Name = "Leaping Mantis Sting", Profession = Profession.Assassin };
        public static Skill BlackMantisThrust { get; } = new() { Id = 1024, Name = "Black Mantis Thrust", Profession = Profession.Assassin };
        public static Skill DisruptingStab { get; } = new() { Id = 1025, Name = "Disrupting Stab", Profession = Profession.Assassin };
        public static Skill GoldenLotusStrike { get; } = new() { Id = 1026, Name = "Golden Lotus Strike", Profession = Profession.Assassin };
        public static Skill FlashingBlades { get; } = new() { Id = 1042, Name = "Flashing Blades", Profession = Profession.Assassin };
        public static Skill GoldenSkullStrike { get; } = new() { Id = 1635, Name = "Golden Skull Strike", Profession = Profession.Assassin };
        public static Skill BlackSpiderStrike { get; } = new() { Id = 1636, Name = "Black Spider Strike", Profession = Profession.Assassin };
        public static Skill GoldenFoxStrike { get; } = new() { Id = 1637, Name = "Golden Fox Strike", Profession = Profession.Assassin };
        public static Skill FoxsPromise { get; } = new() { Id = 1640, Name = "Fox's Promise", Profession = Profession.Assassin };
        public static Skill LotusStrike { get; } = new() { Id = 1987, Name = "Lotus Strike", Profession = Profession.Assassin };
        public static Skill GoldenFangStrike { get; } = new() { Id = 1988, Name = "Golden Fang Strike", Profession = Profession.Assassin };
        public static Skill FallingLotusStrike { get; } = new() { Id = 1990, Name = "Falling Lotus Strike", Profession = Profession.Assassin };
        public static Skill TramplingOx { get; } = new() { Id = 2135, Name = "Trampling Ox", Profession = Profession.Assassin };
        public static Skill MarkofInsecurity { get; } = new() { Id = 570, Name = "Mark of Insecurity", Profession = Profession.Assassin };
        public static Skill DisruptingDagger { get; } = new() { Id = 571, Name = "Disrupting Dagger", Profession = Profession.Assassin };
        public static Skill DeadlyParadox { get; } = new() { Id = 572, Name = "Deadly Paradox", Profession = Profession.Assassin };
        public static Skill EntanglingAsp { get; } = new() { Id = 784, Name = "Entangling Asp", Profession = Profession.Assassin };
        public static Skill MarkofDeath { get; } = new() { Id = 785, Name = "Mark of Death", Profession = Profession.Assassin };
        public static Skill IronPalm { get; } = new() { Id = 786, Name = "Iron Palm", Profession = Profession.Assassin };
        public static Skill EnduringToxin { get; } = new() { Id = 800, Name = "Enduring Toxin", Profession = Profession.Assassin };
        public static Skill ShroudofSilence { get; } = new() { Id = 801, Name = "Shroud of Silence", Profession = Profession.Assassin };
        public static Skill ExposeDefenses { get; } = new() { Id = 802, Name = "Expose Defenses", Profession = Profession.Assassin };
        public static Skill ScorpionWire { get; } = new() { Id = 815, Name = "Scorpion Wire", Profession = Profession.Assassin };
        public static Skill SiphonStrength { get; } = new() { Id = 827, Name = "Siphon Strength", Profession = Profession.Assassin };
        public static Skill DancingDaggers { get; } = new() { Id = 858, Name = "Dancing Daggers", Profession = Profession.Assassin };
        public static Skill SignetofShadows { get; } = new() { Id = 876, Name = "Signet of Shadows" };
        public static Skill ShamefulFear { get; } = new() { Id = 927, Name = "Shameful Fear", Profession = Profession.Assassin };
        public static Skill SiphonSpeed { get; } = new() { Id = 951, Name = "Siphon Speed", Profession = Profession.Assassin };
        public static Skill MantisTouch { get; } = new() { Id = 974, Name = "Mantis Touch", Profession = Profession.Assassin };
        public static Skill WayoftheEmptyPalm { get; } = new() { Id = 987, Name = "Way of the Empty Palm", Profession = Profession.Assassin };
        public static Skill ExpungeEnchantments { get; } = new() { Id = 990, Name = "Expunge Enchantments", Profession = Profession.Assassin };
        public static Skill Impale { get; } = new() { Id = 1033, Name = "Impale", Profession = Profession.Assassin };
        public static Skill AssassinsPromise { get; } = new() { Id = 1035, Name = "Assassin's Promise", Profession = Profession.Assassin };
        public static Skill CripplingDagger { get; } = new() { Id = 1038, Name = "Crippling Dagger", Profession = Profession.Assassin };
        public static Skill DarkPrison { get; } = new() { Id = 1044, Name = "Dark Prison", Profession = Profession.Assassin };
        public static Skill AuguryofDeath { get; } = new() { Id = 1646, Name = "Augury of Death", Profession = Profession.Assassin };
        public static Skill SignetofToxicShock { get; } = new() { Id = 1647, Name = "Signet of Toxic Shock", Profession = Profession.Assassin };
        public static Skill ShadowPrison { get; } = new() { Id = 1652, Name = "Shadow Prison", Profession = Profession.Assassin };
        public static Skill VampiricAssault { get; } = new() { Id = 1986, Name = "Vampiric Assault", Profession = Profession.Assassin };
        public static Skill SadistsSignet { get; } = new() { Id = 1991, Name = "Sadist's Signet", Profession = Profession.Assassin };
        public static Skill ShadowFang { get; } = new() { Id = 2052, Name = "Shadow Fang", Profession = Profession.Assassin };
        public static Skill SignetofDeadlyCorruption { get; } = new() { Id = 2186, Name = "Signet of Deadly Corruption", Profession = Profession.Assassin };
        public static Skill VipersDefense { get; } = new() { Id = 769, Name = "Viper's Defense", Profession = Profession.Assassin };
        public static Skill Return { get; } = new() { Id = 770, Name = "Return", Profession = Profession.Assassin };
        public static Skill BeguilingHaze { get; } = new() { Id = 799, Name = "Beguiling Haze", Profession = Profession.Assassin };
        public static Skill ShadowRefuge { get; } = new() { Id = 814, Name = "Shadow Refuge", Profession = Profession.Assassin };
        public static Skill MirroredStance { get; } = new() { Id = 816, Name = "Mirrored Stance", Profession = Profession.Assassin };
        public static Skill ShadowForm { get; } = new() { Id = 826, Name = "Shadow Form", Profession = Profession.Assassin };
        public static Skill ShadowShroud { get; } = new() { Id = 928, Name = "Shadow Shroud", Profession = Profession.Assassin };
        public static Skill ShadowofHaste { get; } = new() { Id = 929, Name = "Shadow of Haste", Profession = Profession.Assassin };
        public static Skill WayoftheFox { get; } = new() { Id = 949, Name = "Way of the Fox", Profession = Profession.Assassin };
        public static Skill ShadowyBurden { get; } = new() { Id = 950, Name = "Shadowy Burden", Profession = Profession.Assassin };
        public static Skill DeathsCharge { get; } = new() { Id = 952, Name = "Death's Charge", Profession = Profession.Assassin };
        public static Skill BlindingPowder { get; } = new() { Id = 973, Name = "Blinding Powder", Profession = Profession.Assassin };
        public static Skill WayoftheLotus { get; } = new() { Id = 977, Name = "Way of the Lotus", Profession = Profession.Assassin };
        public static Skill Caltrops { get; } = new() { Id = 985, Name = "Caltrops", Profession = Profession.Assassin };
        public static Skill WayofPerfection { get; } = new() { Id = 1028, Name = "Way of Perfection", Profession = Profession.Assassin };
        public static Skill ShroudofDistress { get; } = new() { Id = 1031, Name = "Shroud of Distress", Profession = Profession.Assassin };
        public static Skill HeartofShadow { get; } = new() { Id = 1032, Name = "Heart of Shadow", Profession = Profession.Assassin };
        public static Skill DarkEscape { get; } = new() { Id = 1037, Name = "Dark Escape", Profession = Profession.Assassin };
        public static Skill UnseenFury { get; } = new() { Id = 1041, Name = "Unseen Fury", Profession = Profession.Assassin };
        public static Skill FeignedNeutrality { get; } = new() { Id = 1641, Name = "Feigned Neutrality", Profession = Profession.Assassin };
        public static Skill HiddenCaltrops { get; } = new() { Id = 1642, Name = "Hidden Caltrops", Profession = Profession.Assassin };
        public static Skill DeathsRetreat { get; } = new() { Id = 1651, Name = "Death's Retreat", Profession = Profession.Assassin };
        public static Skill SmokePowderDefense { get; } = new() { Id = 2136, Name = "Smoke Powder Defense", Profession = Profession.Assassin };
        public static Skill AuraofDisplacement { get; } = new() { Id = 771, Name = "Aura of Displacement", Profession = Profession.Assassin };
        public static Skill Recall { get; } = new() { Id = 925, Name = "Recall", Profession = Profession.Assassin };
        public static Skill MarkofInstability { get; } = new() { Id = 978, Name = "Mark of Instability", Profession = Profession.Assassin };
        public static Skill SignetofMalice { get; } = new() { Id = 1036, Name = "Signet of Malice", Profession = Profession.Assassin };
        public static Skill SpiritWalk { get; } = new() { Id = 1040, Name = "Spirit Walk", Profession = Profession.Assassin };
        public static Skill Dash { get; } = new() { Id = 1043, Name = "Dash", Profession = Profession.Assassin };
        public static Skill AssaultEnchantments { get; } = new() { Id = 1643, Name = "Assault Enchantments", Profession = Profession.Assassin };
        public static Skill WastrelsCollapse { get; } = new() { Id = 1644, Name = "Wastrel's Collapse", Profession = Profession.Assassin };
        public static Skill LiftEnchantment { get; } = new() { Id = 1645, Name = "Lift Enchantment", Profession = Profession.Assassin };
        public static Skill SignetofTwilight { get; } = new() { Id = 1648, Name = "Signet of Twilight", Profession = Profession.Assassin };
        public static Skill ShadowWalk { get; } = new() { Id = 1650, Name = "Shadow Walk", Profession = Profession.Assassin };
        public static Skill Swap { get; } = new() { Id = 1653, Name = "Swap", Profession = Profession.Assassin };
        public static Skill ShadowMeld { get; } = new() { Id = 1654, Name = "Shadow Meld", Profession = Profession.Assassin };
        public static Skill GraspingWasKuurong { get; } = new() { Id = 789, Name = "Grasping Was Kuurong", Profession = Profession.Ritualist };
        public static Skill SplinterWeapon { get; } = new() { Id = 792, Name = "Splinter Weapon", Profession = Profession.Ritualist };
        public static Skill WailingWeapon { get; } = new() { Id = 794, Name = "Wailing Weapon", Profession = Profession.Ritualist };
        public static Skill NightmareWeapon { get; } = new() { Id = 795, Name = "Nightmare Weapon", Profession = Profession.Ritualist };
        public static Skill SpiritRift { get; } = new() { Id = 910, Name = "Spirit Rift", Profession = Profession.Ritualist };
        public static Skill Lamentation { get; } = new() { Id = 916, Name = "Lamentation", Profession = Profession.Ritualist };
        public static Skill SpiritBurn { get; } = new() { Id = 919, Name = "Spirit Burn", Profession = Profession.Ritualist };
        public static Skill Destruction { get; } = new() { Id = 920, Name = "Destruction", Profession = Profession.Ritualist };
        public static Skill ClamorofSouls { get; } = new() { Id = 1215, Name = "Clamor of Souls", Profession = Profession.Ritualist };
        public static Skill CruelWasDaoshen { get; } = new() { Id = 1218, Name = "Cruel Was Daoshen", Profession = Profession.Ritualist };
        public static Skill ChanneledStrike { get; } = new() { Id = 1225, Name = "Channeled Strike", Profession = Profession.Ritualist };
        public static Skill SpiritBoonStrike { get; } = new() { Id = 1226, Name = "Spirit Boon Strike", Profession = Profession.Ritualist };
        public static Skill EssenceStrike { get; } = new() { Id = 1227, Name = "Essence Strike", Profession = Profession.Ritualist };
        public static Skill SpiritSiphon { get; } = new() { Id = 1228, Name = "Spirit Siphon", Profession = Profession.Ritualist };
        public static Skill PainfulBond { get; } = new() { Id = 1237, Name = "Painful Bond", Profession = Profession.Ritualist };
        public static Skill SignetofSpirits { get; } = new() { Id = 1239, Name = "Signet of Spirits", Profession = Profession.Ritualist };
        public static Skill GazefromBeyond { get; } = new() { Id = 1245, Name = "Gaze from Beyond", Profession = Profession.Ritualist };
        public static Skill AncestorsRage { get; } = new() { Id = 1246, Name = "Ancestors' Rage", Profession = Profession.Ritualist };
        public static Skill Bloodsong { get; } = new() { Id = 1253, Name = "Bloodsong", Profession = Profession.Ritualist };
        public static Skill RenewingSurge { get; } = new() { Id = 1478, Name = "Renewing Surge", Profession = Profession.Ritualist };
        public static Skill OfferingofSpirit { get; } = new() { Id = 1479, Name = "Offering of Spirit", Profession = Profession.Ritualist };
        public static Skill DestructiveWasGlaive { get; } = new() { Id = 1732, Name = "Destructive Was Glaive", Profession = Profession.Ritualist };
        public static Skill WieldersStrike { get; } = new() { Id = 1733, Name = "Wielder's Strike", Profession = Profession.Ritualist };
        public static Skill GazeofFury { get; } = new() { Id = 1734, Name = "Gaze of Fury", Profession = Profession.Ritualist };
        public static Skill CaretakersCharge { get; } = new() { Id = 1744, Name = "Caretaker's Charge", Profession = Profession.Ritualist };
        public static Skill WeaponofFury { get; } = new() { Id = 1749, Name = "Weapon of Fury", Profession = Profession.Ritualist };
        public static Skill WarmongersWeapon { get; } = new() { Id = 1751, Name = "Warmonger's Weapon", Profession = Profession.Ritualist };
        public static Skill WeaponofAggression { get; } = new() { Id = 2073, Name = "Weapon of Aggression", Profession = Profession.Ritualist };
        public static Skill Agony { get; } = new() { Id = 2205, Name = "Agony", Profession = Profession.Ritualist };
        public static Skill MightyWasVorizun { get; } = new() { Id = 773, Name = "Mighty Was Vorizun", Profession = Profession.Ritualist };
        public static Skill Shadowsong { get; } = new() { Id = 871, Name = "Shadowsong", Profession = Profession.Ritualist };
        public static Skill Union { get; } = new() { Id = 911, Name = "Union", Profession = Profession.Ritualist };
        public static Skill Dissonance { get; } = new() { Id = 921, Name = "Dissonance", Profession = Profession.Ritualist };
        public static Skill Disenchantment { get; } = new() { Id = 923, Name = "Disenchantment", Profession = Profession.Ritualist };
        public static Skill Restoration { get; } = new() { Id = 963, Name = "Restoration", Profession = Profession.Ritualist };
        public static Skill Shelter { get; } = new() { Id = 982, Name = "Shelter", Profession = Profession.Ritualist };
        public static Skill ArmorofUnfeeling { get; } = new() { Id = 1232, Name = "Armor of Unfeeling", Profession = Profession.Ritualist };
        public static Skill DulledWeapon { get; } = new() { Id = 1235, Name = "Dulled Weapon", Profession = Profession.Ritualist };
        public static Skill BindingChains { get; } = new() { Id = 1236, Name = "Binding Chains", Profession = Profession.Ritualist };
        public static Skill Pain { get; } = new() { Id = 1247, Name = "Pain", Profession = Profession.Ritualist };
        public static Skill Displacement { get; } = new() { Id = 1249, Name = "Displacement", Profession = Profession.Ritualist };
        public static Skill Earthbind { get; } = new() { Id = 1252, Name = "Earthbind", Profession = Profession.Ritualist };
        public static Skill Wanderlust { get; } = new() { Id = 1255, Name = "Wanderlust", Profession = Profession.Ritualist };
        public static Skill BrutalWeapon { get; } = new() { Id = 1258, Name = "Brutal Weapon", Profession = Profession.Ritualist };
        public static Skill GuidedWeapon { get; } = new() { Id = 1259, Name = "Guided Weapon", Profession = Profession.Ritualist };
        public static Skill Soothing { get; } = new() { Id = 1266, Name = "Soothing", Profession = Profession.Ritualist };
        public static Skill VitalWeapon { get; } = new() { Id = 1267, Name = "Vital Weapon", Profession = Profession.Ritualist };
        public static Skill WeaponofQuickening { get; } = new() { Id = 1268, Name = "Weapon of Quickening", Profession = Profession.Ritualist };
        public static Skill SignetofGhostlyMight { get; } = new() { Id = 1742, Name = "Signet of Ghostly Might", Profession = Profession.Ritualist };
        public static Skill Anguish { get; } = new() { Id = 1745, Name = "Anguish", Profession = Profession.Ritualist };
        public static Skill SunderingWeapon { get; } = new() { Id = 2148, Name = "Sundering Weapon", Profession = Profession.Ritualist };
        public static Skill GhostlyWeapon { get; } = new() { Id = 2206, Name = "Ghostly Weapon", Profession = Profession.Ritualist };
        public static Skill GenerousWasTsungrai { get; } = new() { Id = 772, Name = "Generous Was Tsungrai", Profession = Profession.Ritualist };
        public static Skill ResilientWeapon { get; } = new() { Id = 787, Name = "Resilient Weapon", Profession = Profession.Ritualist };
        public static Skill BlindWasMingson { get; } = new() { Id = 788, Name = "Blind Was Mingson", Profession = Profession.Ritualist };
        public static Skill VengefulWasKhanhei { get; } = new() { Id = 790, Name = "Vengeful Was Khanhei", Profession = Profession.Ritualist };
        public static Skill FleshofMyFlesh { get; } = new() { Id = 791, Name = "Flesh of My Flesh", Profession = Profession.Ritualist };
        public static Skill WeaponofWarding { get; } = new() { Id = 793, Name = "Weapon of Warding", Profession = Profession.Ritualist };
        public static Skill DefiantWasXinrae { get; } = new() { Id = 812, Name = "Defiant Was Xinrae", Profession = Profession.Ritualist };
        public static Skill TranquilWasTanasen { get; } = new() { Id = 913, Name = "Tranquil Was Tanasen", Profession = Profession.Ritualist };
        public static Skill SpiritLight { get; } = new() { Id = 915, Name = "Spirit Light", Profession = Profession.Ritualist };
        public static Skill SpiritTransfer { get; } = new() { Id = 962, Name = "Spirit Transfer", Profession = Profession.Ritualist };
        public static Skill VengefulWeapon { get; } = new() { Id = 964, Name = "Vengeful Weapon", Profession = Profession.Ritualist };
        public static Skill Recuperation { get; } = new() { Id = 981, Name = "Recuperation", Profession = Profession.Ritualist };
        public static Skill WeaponofShadow { get; } = new() { Id = 983, Name = "Weapon of Shadow", Profession = Profession.Ritualist };
        public static Skill ProtectiveWasKaolai { get; } = new() { Id = 1219, Name = "Protective Was Kaolai", Profession = Profession.Ritualist };
        public static Skill ResilientWasXiko { get; } = new() { Id = 1221, Name = "Resilient Was Xiko", Profession = Profession.Ritualist };
        public static Skill LivelyWasNaomei { get; } = new() { Id = 1222, Name = "Lively Was Naomei", Profession = Profession.Ritualist };
        public static Skill SoothingMemories { get; } = new() { Id = 1233, Name = "Soothing Memories", Profession = Profession.Ritualist };
        public static Skill MendBodyandSoul { get; } = new() { Id = 1234, Name = "Mend Body and Soul", Profession = Profession.Ritualist };
        public static Skill Preservation { get; } = new() { Id = 1250, Name = "Preservation", Profession = Profession.Ritualist };
        public static Skill Life { get; } = new() { Id = 1251, Name = "Life", Profession = Profession.Ritualist };
        public static Skill SpiritLightWeapon { get; } = new() { Id = 1257, Name = "Spirit Light Weapon", Profession = Profession.Ritualist };
        public static Skill WieldersBoon { get; } = new() { Id = 1265, Name = "Wielder's Boon", Profession = Profession.Ritualist };
        public static Skill DeathPactSignet { get; } = new() { Id = 1481, Name = "Death Pact Signet", Profession = Profession.Ritualist };
        public static Skill VocalWasSogolon { get; } = new() { Id = 1731, Name = "Vocal Was Sogolon", Profession = Profession.Ritualist };
        public static Skill GhostmirrorLight { get; } = new() { Id = 1741, Name = "Ghostmirror Light", Profession = Profession.Ritualist };
        public static Skill Recovery { get; } = new() { Id = 1748, Name = "Recovery", Profession = Profession.Ritualist };
        public static Skill XinraesWeapon { get; } = new() { Id = 1750, Name = "Xinrae's Weapon", Profession = Profession.Ritualist };
        public static Skill WeaponofRemedy { get; } = new() { Id = 1752, Name = "Weapon of Remedy", Profession = Profession.Ritualist };
        public static Skill PureWasLiMing { get; } = new() { Id = 2072, Name = "Pure Was Li Ming", Profession = Profession.Ritualist };
        public static Skill MendingGrip { get; } = new() { Id = 2202, Name = "Mending Grip", Profession = Profession.Ritualist };
        public static Skill SpiritleechAura { get; } = new() { Id = 2203, Name = "Spiritleech Aura", Profession = Profession.Ritualist };
        public static Skill Rejuvenation { get; } = new() { Id = 2204, Name = "Rejuvenation", Profession = Profession.Ritualist };
        public static Skill ConsumeSoul { get; } = new() { Id = 914, Name = "Consume Soul", Profession = Profession.Ritualist };
        public static Skill RuptureSoul { get; } = new() { Id = 917, Name = "Rupture Soul", Profession = Profession.Ritualist };
        public static Skill SpirittoFlesh { get; } = new() { Id = 918, Name = "Spirit to Flesh", Profession = Profession.Ritualist };
        public static Skill FeastofSouls { get; } = new() { Id = 980, Name = "Feast of Souls", Profession = Profession.Ritualist };
        public static Skill RitualLord { get; } = new() { Id = 1217, Name = "Ritual Lord", Profession = Profession.Ritualist };
        public static Skill AttunedWasSongkai { get; } = new() { Id = 1220, Name = "Attuned Was Songkai", Profession = Profession.Ritualist };
        public static Skill AnguishedWasLingwah { get; } = new() { Id = 1223, Name = "Anguished Was Lingwah, Profession = Profession.Ritualist" };
        public static Skill ExplosiveGrowth { get; } = new() { Id = 1229, Name = "Explosive Growth", Profession = Profession.Ritualist };
        public static Skill BoonofCreation { get; } = new() { Id = 1230, Name = "Boon of Creation", Profession = Profession.Ritualist };
        public static Skill SpiritChanneling { get; } = new() { Id = 1231, Name = "Spirit Channeling", Profession = Profession.Ritualist };
        public static Skill SignetofCreation { get; } = new() { Id = 1238, Name = "Signet of Creation", Profession = Profession.Ritualist };
        public static Skill SoulTwisting { get; } = new() { Id = 1240, Name = "Soul Twisting", Profession = Profession.Ritualist };
        public static Skill GhostlyHaste { get; } = new() { Id = 1244, Name = "Ghostly Haste", Profession = Profession.Ritualist };
        public static Skill Doom { get; } = new() { Id = 1264, Name = "Doom", Profession = Profession.Ritualist };
        public static Skill SpiritsGift { get; } = new() { Id = 1480, Name = "Spirit's Gift", Profession = Profession.Ritualist };
        public static Skill ReclaimEssence { get; } = new() { Id = 1482, Name = "Reclaim Essence", Profession = Profession.Ritualist };
        public static Skill SpiritsStrength { get; } = new() { Id = 1736, Name = "Spirit's Strength", Profession = Profession.Ritualist };
        public static Skill WieldersZeal { get; } = new() { Id = 1737, Name = "Wielder's Zeal", Profession = Profession.Ritualist };
        public static Skill SightBeyondSight { get; } = new() { Id = 1738, Name = "Sight Beyond Sight", Profession = Profession.Ritualist };
        public static Skill RenewingMemories { get; } = new() { Id = 1739, Name = "Renewing Memories", Profession = Profession.Ritualist };
        public static Skill WieldersRemedy { get; } = new() { Id = 1740, Name = "Wielder's Remedy", Profession = Profession.Ritualist };
        public static Skill SignetofBinding { get; } = new() { Id = 1743, Name = "Signet of Binding", Profession = Profession.Ritualist };
        public static Skill Empowerment { get; } = new() { Id = 1747, Name = "Empowerment", Profession = Profession.Ritualist };
        public static Skill EnergeticWasLeeSa { get; } = new() { Id = 2016, Name = "Energetic Was Lee Sa", Profession = Profession.Ritualist };
        public static Skill WeaponofRenewal { get; } = new() { Id = 2149, Name = "Weapon of Renewal", Profession = Profession.Ritualist };
        public static Skill DrawSpirit { get; } = new() { Id = 1224, Name = "Draw Spirit", Profession = Profession.Ritualist };
        public static Skill CripplingAnthem { get; } = new() { Id = 1554, Name = "Crippling Anthem", Profession = Profession.Paragon };
        public static Skill Godspeed { get; } = new() { Id = 1556, Name = "Godspeed", Profession = Profession.Paragon };
        public static Skill GofortheEyes { get; } = new() { Id = 1558, Name = "\"Go for the Eyes!\"", Profession = Profession.Paragon };
        public static Skill AnthemofEnvy { get; } = new() { Id = 1559, Name = "Anthem of Envy", Profession = Profession.Paragon };
        public static Skill AnthemofGuidance { get; } = new() { Id = 1568, Name = "Anthem of Guidance", Profession = Profession.Paragon };
        public static Skill BraceYourself { get; } = new() { Id = 1572, Name = "\"Brace Yourself!\"", Profession = Profession.Paragon };
        public static Skill BladeturnRefrain { get; } = new() { Id = 1580, Name = "Bladeturn Refrain", Profession = Profession.Paragon };
        public static Skill StandYourGround { get; } = new() { Id = 1589, Name = "\"Stand Your Ground!\"", Profession = Profession.Paragon };
        public static Skill MakeHaste { get; } = new() { Id = 1591, Name = "\"Make Haste!\"", Profession = Profession.Paragon };
        public static Skill WeShallReturn { get; } = new() { Id = 1592, Name = "\"We Shall Return!\"", Profession = Profession.Paragon };
        public static Skill NeverGiveUp { get; } = new() { Id = 1593, Name = "\"Never Give Up!\"", Profession = Profession.Paragon };
        public static Skill HelpMe { get; } = new() { Id = 1594, Name = "\"Help Me!\"", Profession = Profession.Paragon };
        public static Skill FallBack { get; } = new() { Id = 1595, Name = "\"Fall Back!\"", Profession = Profession.Paragon };
        public static Skill Incoming { get; } = new() { Id = 1596, Name = "\"Incoming!\"", Profession = Profession.Paragon };
        public static Skill NeverSurrender { get; } = new() { Id = 1598, Name = "\"Never Surrender!\"", Profession = Profession.Paragon };
        public static Skill CantTouchThis { get; } = new() { Id = 1780, Name = "\"Can't Touch This!\"", Profession = Profession.Paragon };
        public static Skill FindTheirWeakness { get; } = new() { Id = 1781, Name = "\"Find Their Weakness!\"", Profession = Profession.Paragon };
        public static Skill AnthemofWeariness { get; } = new() { Id = 2017, Name = "Anthem of Weariness", Profession = Profession.Paragon };
        public static Skill AnthemofDisruption { get; } = new() { Id = 2018, Name = "Anthem of Disruption", Profession = Profession.Paragon };
        public static Skill AnthemofFury { get; } = new() { Id = 1553, Name = "Anthem of Fury", Profession = Profession.Paragon };
        public static Skill DefensiveAnthem { get; } = new() { Id = 1555, Name = "Defensive Anthem", Profession = Profession.Paragon };
        public static Skill AnthemofFlame { get; } = new() { Id = 1557, Name = "Anthem of Flame", Profession = Profession.Paragon };
        public static Skill Awe { get; } = new() { Id = 1573, Name = "Awe", Profession = Profession.Paragon };
        public static Skill EnduringHarmony { get; } = new() { Id = 1574, Name = "Enduring Harmony", Profession = Profession.Paragon };
        public static Skill BlazingFinale { get; } = new() { Id = 1575, Name = "Blazing Finale", Profession = Profession.Paragon };
        public static Skill BurningRefrain { get; } = new() { Id = 1576, Name = "Burning Refrain", Profession = Profession.Paragon };
        public static Skill GlowingSignet { get; } = new() { Id = 1581, Name = "Glowing Signet", Profession = Profession.Paragon };
        public static Skill LeadersComfort { get; } = new() { Id = 1584, Name = "Leader's Comfort", Profession = Profession.Paragon };
        public static Skill AngelicProtection { get; } = new() { Id = 1586, Name = "Angelic Protection", Profession = Profession.Paragon };
        public static Skill AngelicBond { get; } = new() { Id = 1587, Name = "Angelic Bond", Profession = Profession.Paragon };
        public static Skill LeadtheWay { get; } = new() { Id = 1590, Name = "\"Lead the Way!\"", Profession = Profession.Paragon };
        public static Skill TheyreonFire { get; } = new() { Id = 1597, Name = "\"They're on Fire!\"", Profession = Profession.Paragon };
        public static Skill FocusedAnger { get; } = new() { Id = 1769, Name = "Focused Anger", Profession = Profession.Paragon };
        public static Skill NaturalTemper { get; } = new() { Id = 1770, Name = "Natural Temper", Profession = Profession.Paragon };
        public static Skill SoldiersFury { get; } = new() { Id = 1773, Name = "Soldier's Fury", Profession = Profession.Paragon };
        public static Skill AggressiveRefrain { get; } = new() { Id = 1774, Name = "Aggressive Refrain", Profession = Profession.Paragon };
        public static Skill SignetofReturn { get; } = new() { Id = 1778, Name = "Signet of Return", Profession = Profession.Paragon };
        public static Skill MakeYourTime { get; } = new() { Id = 1779, Name = "\"Make Your Time!\"", Profession = Profession.Paragon };
        public static Skill HastyRefrain { get; } = new() { Id = 2075, Name = "Hasty Refrain", Profession = Profession.Paragon };
        public static Skill BurningShield { get; } = new() { Id = 2208, Name = "Burning Shield", Profession = Profession.Paragon };
        public static Skill SpearSwipe { get; } = new() { Id = 2210, Name = "Spear Swipe", Profession = Profession.Paragon };
        public static Skill SongofPower { get; } = new() { Id = 1560, Name = "Song of Power", Profession = Profession.Paragon };
        public static Skill ZealousAnthem { get; } = new() { Id = 1561, Name = "Zealous Anthem", Profession = Profession.Paragon };
        public static Skill AriaofZeal { get; } = new() { Id = 1562, Name = "Aria of Zeal", Profession = Profession.Paragon };
        public static Skill LyricofZeal { get; } = new() { Id = 1563, Name = "Lyric of Zeal", Profession = Profession.Paragon };
        public static Skill BalladofRestoration { get; } = new() { Id = 1564, Name = "Ballad of Restoration", Profession = Profession.Paragon };
        public static Skill ChorusofRestoration { get; } = new() { Id = 1565, Name = "Chorus of Restoration", Profession = Profession.Paragon };
        public static Skill AriaofRestoration { get; } = new() { Id = 1566, Name = "Aria of Restoration", Profession = Profession.Paragon };
        public static Skill EnergizingChorus { get; } = new() { Id = 1569, Name = "Energizing Chorus", Profession = Profession.Paragon };
        public static Skill SongofPurification { get; } = new() { Id = 1570, Name = "Song of Purification", Profession = Profession.Paragon };
        public static Skill FinaleofRestoration { get; } = new() { Id = 1577, Name = "Finale of Restoration", Profession = Profession.Paragon };
        public static Skill MendingRefrain { get; } = new() { Id = 1578, Name = "Mending Refrain", Profession = Profession.Paragon };
        public static Skill PurifyingFinale { get; } = new() { Id = 1579, Name = "Purifying Finale", Profession = Profession.Paragon };
        public static Skill LeadersZeal { get; } = new() { Id = 1583, Name = "Leader's Zeal", Profession = Profession.Paragon };
        public static Skill SignetofSynergy { get; } = new() { Id = 1585, Name = "Signet of Synergy", Profession = Profession.Paragon };
        public static Skill ItsJustaFleshWound { get; } = new() { Id = 1599, Name = "\"It's Just a Flesh Wound.\"", Profession = Profession.Paragon };
        public static Skill SongofRestoration { get; } = new() { Id = 1771, Name = "Song of Restoration", Profession = Profession.Paragon };
        public static Skill LyricofPurification { get; } = new() { Id = 1772, Name = "Lyric of Purification", Profession = Profession.Paragon };
        public static Skill EnergizingFinale { get; } = new() { Id = 1775, Name = "Energizing Finale", Profession = Profession.Paragon };
        public static Skill ThePowerIsYours { get; } = new() { Id = 1782, Name = "\"The Power Is Yours!\"", Profession = Profession.Paragon };
        public static Skill InspirationalSpeech { get; } = new() { Id = 2207, Name = "Inspirational Speech", Profession = Profession.Paragon };
        public static Skill BlazingSpear { get; } = new() { Id = 1546, Name = "Blazing Spear", Profession = Profession.Paragon };
        public static Skill MightyThrow { get; } = new() { Id = 1547, Name = "Mighty Throw", Profession = Profession.Paragon };
        public static Skill CruelSpear { get; } = new() { Id = 1548, Name = "Cruel Spear", Profession = Profession.Paragon };
        public static Skill HarriersToss { get; } = new() { Id = 1549, Name = "Harrier's Toss", Profession = Profession.Paragon };
        public static Skill UnblockableThrow { get; } = new() { Id = 1550, Name = "Unblockable Throw", Profession = Profession.Paragon };
        public static Skill SpearofLightning { get; } = new() { Id = 1551, Name = "Spear of Lightning", Profession = Profession.Paragon };
        public static Skill WearyingSpear { get; } = new() { Id = 1552, Name = "Wearying Spear", Profession = Profession.Paragon };
        public static Skill BarbedSpear { get; } = new() { Id = 1600, Name = "Barbed Spear", Profession = Profession.Paragon };
        public static Skill ViciousAttack { get; } = new() { Id = 1601, Name = "Vicious Attack", Profession = Profession.Paragon };
        public static Skill StunningStrike { get; } = new() { Id = 1602, Name = "Stunning Strike", Profession = Profession.Paragon };
        public static Skill MercilessSpear { get; } = new() { Id = 1603, Name = "Merciless Spear", Profession = Profession.Paragon };
        public static Skill DisruptingThrow { get; } = new() { Id = 1604, Name = "Disrupting Throw", Profession = Profession.Paragon };
        public static Skill WildThrow { get; } = new() { Id = 1605, Name = "Wild Throw", Profession = Profession.Paragon };
        public static Skill SlayersSpear { get; } = new() { Id = 1783, Name = "Slayer's Spear", Profession = Profession.Paragon };
        public static Skill SwiftJavelin { get; } = new() { Id = 1784, Name = "Swift Javelin", Profession = Profession.Paragon };
        public static Skill ChestThumper { get; } = new() { Id = 2074, Name = "Chest Thumper", Profession = Profession.Paragon };
        public static Skill MaimingSpear { get; } = new() { Id = 2150, Name = "Maiming Spear", Profession = Profession.Paragon };
        public static Skill HolySpear { get; } = new() { Id = 2209, Name = "Holy Spear", Profession = Profession.Paragon };
        public static Skill SpearofRedemption { get; } = new() { Id = 2238, Name = "Spear of Redemption", Profession = Profession.Paragon };
        public static Skill SongofConcentration { get; } = new() { Id = 1567, Name = "Song of Concentration", Profession = Profession.Paragon };
        public static Skill HexbreakerAria { get; } = new() { Id = 1571, Name = "Hexbreaker Aria", Profession = Profession.Paragon };
        public static Skill CauterySignet { get; } = new() { Id = 1588, Name = "Cautery Signet", Profession = Profession.Paragon };
        public static Skill SignetofAggression { get; } = new() { Id = 1776, Name = "Signet of Aggression", Profession = Profession.Paragon };
        public static Skill RemedySignet { get; } = new() { Id = 1777, Name = "Remedy Signet", Profession = Profession.Paragon };
        public static Skill AuraofThorns { get; } = new() { Id = 1495, Name = "Aura of Thorns", Profession = Profession.Dervish };
        public static Skill DustCloak { get; } = new() { Id = 1497, Name = "Dust Cloak", Profession = Profession.Dervish };
        public static Skill StaggeringForce { get; } = new() { Id = 1498, Name = "Staggering Force", Profession = Profession.Dervish };
        public static Skill MirageCloak { get; } = new() { Id = 1500, Name = "Mirage Cloak", Profession = Profession.Dervish };
        public static Skill VitalBoon { get; } = new() { Id = 1506, Name = "Vital Boon", Profession = Profession.Dervish };
        public static Skill SandShards { get; } = new() { Id = 1510, Name = "Sand Shards", Profession = Profession.Dervish };
        public static Skill FleetingStability { get; } = new() { Id = 1514, Name = "Fleeting Stability", Profession = Profession.Dervish };
        public static Skill ArmorofSanctity { get; } = new() { Id = 1515, Name = "Armor of Sanctity", Profession = Profession.Dervish };
        public static Skill MysticRegeneration { get; } = new() { Id = 1516, Name = "Mystic Regeneration", Profession = Profession.Dervish };
        public static Skill SignetofPiousLight { get; } = new() { Id = 1530, Name = "Signet of Pious Light", Profession = Profession.Dervish };
        public static Skill MysticSandstorm { get; } = new() { Id = 1532, Name = "Mystic Sandstorm", Profession = Profession.Dervish };
        public static Skill Conviction { get; } = new() { Id = 1540, Name = "Conviction", Profession = Profession.Dervish };
        public static Skill PiousConcentration { get; } = new() { Id = 1542, Name = "Pious Concentration", Profession = Profession.Dervish };
        public static Skill VeilofThorns { get; } = new() { Id = 1757, Name = "Veil of Thorns", Profession = Profession.Dervish };
        public static Skill VowofStrength { get; } = new() { Id = 1759, Name = "Vow of Strength", Profession = Profession.Dervish };
        public static Skill EbonDustAura { get; } = new() { Id = 1760, Name = "Ebon Dust Aura", Profession = Profession.Dervish };
        public static Skill ShieldofForce { get; } = new() { Id = 2201, Name = "Shield of Force", Profession = Profession.Dervish };
        public static Skill BanishingStrike { get; } = new() { Id = 1483, Name = "Banishing Strike", Profession = Profession.Dervish };
        public static Skill MysticSweep { get; } = new() { Id = 1484, Name = "Mystic Sweep", Profession = Profession.Dervish };
        public static Skill BalthazarsRage { get; } = new() { Id = 1496, Name = "Balthazar's Rage", Profession = Profession.Dervish };
        public static Skill PiousRenewal { get; } = new() { Id = 1499, Name = "Pious Renewal", Profession = Profession.Dervish };
        public static Skill ArcaneZeal { get; } = new() { Id = 1502, Name = "Arcane Zeal", Profession = Profession.Dervish };
        public static Skill MysticVigor { get; } = new() { Id = 1503, Name = "Mystic Vigor", Profession = Profession.Dervish };
        public static Skill WatchfulIntervention { get; } = new() { Id = 1504, Name = "Watchful Intervention", Profession = Profession.Dervish };
        public static Skill HeartofHolyFlame { get; } = new() { Id = 1507, Name = "Heart of Holy Flame", Profession = Profession.Dervish };
        public static Skill ExtendEnchantments { get; } = new() { Id = 1508, Name = "Extend Enchantments", Profession = Profession.Dervish };
        public static Skill FaithfulIntervention { get; } = new() { Id = 1509, Name = "Faithful Intervention", Profession = Profession.Dervish };
        public static Skill VowofSilence { get; } = new() { Id = 1517, Name = "Vow of Silence", Profession = Profession.Dervish };
        public static Skill AvatarofBalthazar { get; } = new() { Id = 1518, Name = "Avatar of Balthazar", Profession = Profession.Dervish };
        public static Skill AvatarofDwayna { get; } = new() { Id = 1519, Name = "Avatar of Dwayna", Profession = Profession.Dervish };
        public static Skill AvatarofGrenth { get; } = new() { Id = 1520, Name = "Avatar of Grenth", Profession = Profession.Dervish };
        public static Skill AvatarofLyssa { get; } = new() { Id = 1521, Name = "Avatar of Lyssa", Profession = Profession.Dervish };
        public static Skill AvatarofMelandru { get; } = new() { Id = 1522, Name = "Avatar of Melandru", Profession = Profession.Dervish };
        public static Skill Meditation { get; } = new() { Id = 1523, Name = "Meditation", Profession = Profession.Dervish };
        public static Skill EremitesZeal { get; } = new() { Id = 1524, Name = "Eremite's Zeal", Profession = Profession.Dervish };
        public static Skill ImbueHealth { get; } = new() { Id = 1526, Name = "Imbue Health", Profession = Profession.Dervish };
        public static Skill IntimidatingAura { get; } = new() { Id = 1531, Name = "Intimidating Aura", Profession = Profession.Dervish };
        public static Skill RendingTouch { get; } = new() { Id = 1534, Name = "Rending Touch", Profession = Profession.Dervish };
        public static Skill PiousHaste { get; } = new() { Id = 1543, Name = "Pious Haste", Profession = Profession.Dervish };
        public static Skill MysticCorruption { get; } = new() { Id = 1755, Name = "Mystic Corruption", Profession = Profession.Dervish };
        public static Skill HeartofFury { get; } = new() { Id = 1762, Name = "Heart of Fury", Profession = Profession.Dervish };
        public static Skill ZealousRenewal { get; } = new() { Id = 1763, Name = "Zealous Renewal", Profession = Profession.Dervish };
        public static Skill AuraSlicer { get; } = new() { Id = 2070, Name = "Aura Slicer", Profession = Profession.Dervish };
        public static Skill PiousFury { get; } = new() { Id = 2146, Name = "Pious Fury", Profession = Profession.Dervish };
        public static Skill EremitesAttack { get; } = new() { Id = 1485, Name = "Eremite's Attack", Profession = Profession.Dervish };
        public static Skill ReapImpurities { get; } = new() { Id = 1486, Name = "Reap Impurities", Profession = Profession.Dervish };
        public static Skill TwinMoonSweep { get; } = new() { Id = 1487, Name = "Twin Moon Sweep", Profession = Profession.Dervish };
        public static Skill VictoriousSweep { get; } = new() { Id = 1488, Name = "Victorious Sweep", Profession = Profession.Dervish };
        public static Skill IrresistibleSweep { get; } = new() { Id = 1489, Name = "Irresistible Sweep", Profession = Profession.Dervish };
        public static Skill PiousAssault { get; } = new() { Id = 1490, Name = "Pious Assault", Profession = Profession.Dervish };
        public static Skill CripplingSweep { get; } = new() { Id = 1535, Name = "Crippling Sweep", Profession = Profession.Dervish };
        public static Skill WoundingStrike { get; } = new() { Id = 1536, Name = "Wounding Strike", Profession = Profession.Dervish };
        public static Skill WearyingStrike { get; } = new() { Id = 1537, Name = "Wearying Strike", Profession = Profession.Dervish };
        public static Skill LyssasAssault { get; } = new() { Id = 1538, Name = "Lyssa's Assault", Profession = Profession.Dervish };
        public static Skill ChillingVictory { get; } = new() { Id = 1539, Name = "Chilling Victory", Profession = Profession.Dervish };
        public static Skill RendingSweep { get; } = new() { Id = 1753, Name = "Rending Sweep", Profession = Profession.Dervish };
        public static Skill ReapersSweep { get; } = new() { Id = 1767, Name = "Reaper's Sweep", Profession = Profession.Dervish };
        public static Skill RadiantScythe { get; } = new() { Id = 2012, Name = "Radiant Scythe", Profession = Profession.Dervish };
        public static Skill FarmersScythe { get; } = new() { Id = 2015, Name = "Farmer's Scythe", Profession = Profession.Dervish };
        public static Skill ZealousSweep { get; } = new() { Id = 2071, Name = "Zealous Sweep", Profession = Profession.Dervish };
        public static Skill CripplingVictory { get; } = new() { Id = 2147, Name = "Crippling Victory", Profession = Profession.Dervish };
        public static Skill MysticTwister { get; } = new() { Id = 1491, Name = "Mystic Twister", Profession = Profession.Dervish };
        public static Skill GrenthsFingers { get; } = new() { Id = 1493, Name = "Grenth's Fingers", Profession = Profession.Dervish };
        public static Skill VowofPiety { get; } = new() { Id = 1505, Name = "Vow of Piety", Profession = Profession.Dervish };
        public static Skill LyssasHaste { get; } = new() { Id = 1512, Name = "Lyssa's Haste", Profession = Profession.Dervish };
        public static Skill GuidingHands { get; } = new() { Id = 1513, Name = "Guiding Hands", Profession = Profession.Dervish };
        public static Skill NaturalHealing { get; } = new() { Id = 1525, Name = "Natural Healing", Profession = Profession.Dervish };
        public static Skill MysticHealing { get; } = new() { Id = 1527, Name = "Mystic Healing", Profession = Profession.Dervish };
        public static Skill DwaynasTouch { get; } = new() { Id = 1528, Name = "Dwayna's Touch", Profession = Profession.Dervish };
        public static Skill PiousRestoration { get; } = new() { Id = 1529, Name = "Pious Restoration", Profession = Profession.Dervish };
        public static Skill WindsofDisenchantment { get; } = new() { Id = 1533, Name = "Winds of Disenchantment", Profession = Profession.Dervish };
        public static Skill WhirlingCharge { get; } = new() { Id = 1544, Name = "Whirling Charge", Profession = Profession.Dervish };
        public static Skill TestofFaith { get; } = new() { Id = 1545, Name = "Test of Faith", Profession = Profession.Dervish };
        public static Skill Onslaught { get; } = new() { Id = 1754, Name = "Onslaught", Profession = Profession.Dervish };
        public static Skill GrenthsGrasp { get; } = new() { Id = 1756, Name = "Grenth's Grasp", Profession = Profession.Dervish };
        public static Skill HarriersGrasp { get; } = new() { Id = 1758, Name = "Harrier's Grasp", Profession = Profession.Dervish };
        public static Skill ZealousVow { get; } = new() { Id = 1761, Name = "Zealous Vow", Profession = Profession.Dervish };
        public static Skill AttackersInsight { get; } = new() { Id = 1764, Name = "Attacker's Insight", Profession = Profession.Dervish };
        public static Skill RendingAura { get; } = new() { Id = 1765, Name = "Rending Aura", Profession = Profession.Dervish };
        public static Skill FeatherfootGrace { get; } = new() { Id = 1766, Name = "Featherfoot Grace", Profession = Profession.Dervish };
        public static Skill HarriersHaste { get; } = new() { Id = 1768, Name = "Harrier's Haste", Profession = Profession.Dervish };
        public static Skill GrenthsAura { get; } = new() { Id = 2013, Name = "Grenth's Aura", Profession = Profession.Dervish };
        public static Skill SignetofPiousRestraint { get; } = new() { Id = 2014, Name = "Signet of Pious Restraint", Profession = Profession.Dervish };
        public static Skill SignetofMysticSpeed { get; } = new() { Id = 2200, Name = "Signet of Mystic Speed", Profession = Profession.Dervish };
        public static Skill EnchantedHaste { get; } = new() { Id = 1541, Name = "Enchanted Haste" , Profession = Profession.Dervish};
        public static Skill EnragedSmashPvP { get; } = new() { Id = 2808, Name = "Enraged Smash (PvP)", Profession = Profession.Warrior };
        public static Skill RenewingSmashPvP { get; } = new() { Id = 3143, Name = "Renewing Smash (PvP)", Profession = Profession.Warrior};
        public static Skill WarriorsEndurancePvP { get; } = new() { Id = 3002, Name = "Warrior's Endurance (PvP)", Profession = Profession.Warrior };
        public static Skill DefyPainPvP { get; } = new() { Id = 3204, Name = "Defy Pain (PvP)", Profession = Profession.Warrior };
        public static Skill WatchYourselfPvP { get; } = new() { Id = 2858, Name = "\"Watch Yourself!\" (PvP)", Profession = Profession.Warrior };
        public static Skill SoldiersStancePvP { get; } = new() { Id = 3156, Name = "Soldier's Stance (PvP)", Profession = Profession.Warrior };
        public static Skill ForGreatJusticePvP { get; } = new() { Id = 2883, Name = "\"For Great Justice!\" (PvP)", Profession = Profession.Warrior };
        public static Skill CallofHastePvP { get; } = new() { Id = 2657, Name = "Call of Haste (PvP)", Profession = Profession.Ranger };
        public static Skill ComfortAnimalPvP { get; } = new() { Id = 3045, Name = "Comfort Animal (PvP)", Profession = Profession.Ranger };
        public static Skill MelandrusAssaultPvP { get; } = new() { Id = 3047, Name = "Melandru's Assault (PvP)", Profession = Profession.Ranger };
        public static Skill PredatoryBondPvP { get; } = new() { Id = 3050, Name = "Predatory Bond (PvP)", Profession = Profession.Ranger };
        public static Skill EnragedLungePvP { get; } = new() { Id = 3051, Name = "Enraged Lunge (PvP)", Profession = Profession.Ranger };
        public static Skill CharmAnimalCodex { get; } = new() { Id = 3068, Name = "Charm Animal (Codex)", Profession = Profession.Ranger };
        public static Skill HealasOnePvP { get; } = new() { Id = 3144, Name = "Heal as One (PvP)", Profession = Profession.Ranger };
        public static Skill ExpertsDexterityPvP { get; } = new() { Id = 2959, Name = "Expert's Dexterity (PvP)", Profession = Profession.Ranger };
        public static Skill EscapePvP { get; } = new() { Id = 3060, Name = "Escape (PvP)", Profession = Profession.Ranger };
        public static Skill LightningReflexesPvP { get; } = new() { Id = 3141, Name = "Lightning Reflexes (PvP)", Profession = Profession.Ranger };
        public static Skill GlassArrowsPvP { get; } = new() { Id = 3145, Name = "Glass Arrows (PvP)", Profession = Profession.Ranger };
        public static Skill PenetratingAttackPvP { get; } = new() { Id = 2861, Name = "Penetrating Attack (PvP)", Profession = Profession.Ranger };
        public static Skill SunderingAttackPvP { get; } = new() { Id = 2864, Name = "Sundering Attack (PvP)", Profession = Profession.Ranger };
        public static Skill SlothHuntersShotPvP { get; } = new() { Id = 2925, Name = "Sloth Hunter's Shot (PvP)", Profession = Profession.Ranger };
        public static Skill ReadtheWindPvP { get; } = new() { Id = 2969, Name = "Read the Wind (PvP)", Profession = Profession.Ranger };
        public static Skill KeenArrowPvP { get; } = new() { Id = 3147, Name = "Keen Arrow (PvP)", Profession = Profession.Ranger };
        public static Skill UnyieldingAuraPvP { get; } = new() { Id = 2891, Name = "Unyielding Aura (PvP)", Profession = Profession.Monk };
        public static Skill SmitersBoonPvP { get; } = new() { Id = 2895, Name = "Smiter's Boon (PvP)", Profession = Profession.Monk };
        public static Skill LightofDeliverancePvP { get; } = new() { Id = 2871, Name = "Light of Deliverance (PvP)", Profession = Profession.Monk };
        public static Skill HealPartyPvP { get; } = new() { Id = 3232, Name = "Heal Party (PvP)", Profession = Profession.Monk };
        public static Skill AegisPvP { get; } = new() { Id = 2857, Name = "Aegis (PvP)", Profession = Profession.Monk };
        public static Skill SpiritBondPvP { get; } = new() { Id = 2892, Name = "Spirit Bond (PvP)", Profession = Profession.Monk };
        public static Skill SignetofJudgmentPvP { get; } = new() { Id = 2887, Name = "Signet of Judgment (PvP)", Profession = Profession.Monk };
        public static Skill StrengthofHonorPvP { get; } = new() { Id = 2999, Name = "Strength of Honor (PvP)", Profession = Profession.Monk };
        public static Skill UnholyFeastPvP { get; } = new() { Id = 3058, Name = "Unholy Feast (PvP)", Profession = Profession.Necromancer };
        public static Skill SignetofAgonyPvP { get; } = new() { Id = 3059, Name = "Signet of Agony (PvP)", Profession = Profession.Necromancer };
        public static Skill SpoilVictorPvP { get; } = new() { Id = 3233, Name = "Spoil Victor (PvP)", Profession = Profession.Necromancer };
        public static Skill EnfeeblePvP { get; } = new() { Id = 2859, Name = "Enfeeble (PvP)", Profession = Profession.Necromancer };
        public static Skill EnfeeblingBloodPvP { get; } = new() { Id = 2885, Name = "Enfeebling Blood (PvP)", Profession = Profession.Necromancer };
        public static Skill DiscordPvP { get; } = new() { Id = 2863, Name = "Discord (PvP)", Profession = Profession.Necromancer };
        public static Skill MasochismPvP { get; } = new() { Id = 3054, Name = "Masochism (PvP)", Profession = Profession.Necromancer };
        public static Skill MindWrackPvP { get; } = new() { Id = 2734, Name = "Mind Wrack (PvP)", Profession = Profession.Mesmer };
        public static Skill EmpathyPvP { get; } = new() { Id = 3151, Name = "Empathy (PvP)", Profession = Profession.Mesmer };
        public static Skill ShatterDelusionsPvP { get; } = new() { Id = 3180, Name = "Shatter Delusions (PvP)", Profession = Profession.Mesmer };
        public static Skill UnnaturalSignetPvP { get; } = new() { Id = 3188, Name = "Unnatural Signet (PvP)", Profession = Profession.Mesmer };
        public static Skill SpiritualPainPvP { get; } = new() { Id = 3189, Name = "Spiritual Pain (PvP)", Profession = Profession.Mesmer };
        public static Skill MistrustPvP { get; } = new() { Id = 3191, Name = "Mistrust (PvP)", Profession = Profession.Mesmer };
        public static Skill EnchantersConundrumPvP { get; } = new() { Id = 3192, Name = "Enchanter's Conundrum (PvP)", Profession = Profession.Mesmer };
        public static Skill VisionsofRegretPvP { get; } = new() { Id = 3234, Name = "Visions of Regret (PvP)", Profession = Profession.Mesmer };
        public static Skill PsychicInstabilityPvP { get; } = new() { Id = 3185, Name = "Psychic Instability (PvP)", Profession = Profession.Mesmer };
        public static Skill StolenSpeedPvP { get; } = new() { Id = 3187, Name = "Stolen Speed (PvP)", Profession = Profession.Mesmer };
        public static Skill FragilityPvP { get; } = new() { Id = 2998, Name = "Fragility (PvP)", Profession = Profession.Mesmer };
        public static Skill CripplingAnguishPvP { get; } = new() { Id = 3152, Name = "Crippling Anguish (PvP)", Profession = Profession.Mesmer };
        public static Skill IllusionaryWeaponryPvP { get; } = new() { Id = 3181, Name = "Illusionary Weaponry (PvP)", Profession = Profession.Mesmer };
        public static Skill MigrainePvP { get; } = new() { Id = 3183, Name = "Migraine (PvP)", Profession = Profession.Mesmer };
        public static Skill AccumulatedPainPvP { get; } = new() { Id = 3184, Name = "Accumulated Pain (PvP)", Profession = Profession.Mesmer };
        public static Skill SharedBurdenPvP { get; } = new() { Id = 3186, Name = "Shared Burden (PvP)", Profession = Profession.Mesmer };
        public static Skill FrustrationPvP { get; } = new() { Id = 3190, Name = "Frustration (PvP)", Profession = Profession.Mesmer };
        public static Skill SignetofClumsinessPvP { get; } = new() { Id = 3193, Name = "Signet of Clumsiness (PvP)", Profession = Profession.Mesmer };
        public static Skill WanderingEyePvP { get; } = new() { Id = 3195, Name = "Wandering Eye (PvP)", Profession = Profession.Mesmer };
        public static Skill CalculatedRiskPvP { get; } = new() { Id = 3196, Name = "Calculated Risk (PvP)", Profession = Profession.Mesmer };
        public static Skill FeveredDreamsPvP { get; } = new() { Id = 3289, Name = "Fevered Dreams (PvP)", Profession = Profession.Mesmer };
        public static Skill IllusionofHastePvP { get; } = new() { Id = 3373, Name = "Illusion of Haste (PvP)", Profession = Profession.Mesmer };
        public static Skill IllusionofPainPvP { get; } = new() { Id = 3374, Name = "Illusion of Pain (PvP)", Profession = Profession.Mesmer };
        public static Skill MantraofResolvePvP { get; } = new() { Id = 3063, Name = "Mantra of Resolve (PvP)", Profession = Profession.Mesmer };
        public static Skill MantraofSignetsPvP { get; } = new() { Id = 3179, Name = "Mantra of Signets (PvP)", Profession = Profession.Mesmer };
        public static Skill MirrorofDisenchantmentPvP { get; } = new() { Id = 3194, Name = "Mirror of Disenchantment (PvP)", Profession = Profession.Mesmer };
        public static Skill WebofDisruptionPvP { get; } = new() { Id = 3386, Name = "Web of Disruption (PvP)", Profession = Profession.Mesmer };
        public static Skill MindShockPvP { get; } = new() { Id = 2804, Name = "Mind Shock (PvP)", Profession = Profession.Elementalist };
        public static Skill RidetheLightningPvP { get; } = new() { Id = 2807, Name = "Ride the Lightning (PvP)", Profession = Profession.Elementalist };
        public static Skill LightningHammerPvP { get; } = new() { Id = 3396, Name = "Lightning Hammer (PvP)", Profession = Profession.Elementalist };
        public static Skill ObsidianFlamePvP { get; } = new() { Id = 2809, Name = "Obsidian Flame (PvP)", Profession = Profession.Elementalist };
        public static Skill EtherRenewalPvP { get; } = new() { Id = 2860, Name = "Ether Renewal (PvP)", Profession = Profession.Elementalist };
        public static Skill AuraofRestorationPvP { get; } = new() { Id = 3375, Name = "Aura of Restoration (PvP)", Profession = Profession.Elementalist };
        public static Skill SavannahHeatPvP { get; } = new() { Id = 3021, Name = "Savannah Heat (PvP)", Profession = Profession.Elementalist };
        public static Skill ElementalFlamePvP { get; } = new() { Id = 3397, Name = "Elemental Flame (PvP)", Profession = Profession.Elementalist };
        public static Skill MindFreezePvP { get; } = new() { Id = 2803, Name = "Mind Freeze (PvP)", Profession = Profession.Elementalist };
        public static Skill MistFormPvP { get; } = new() { Id = 2805, Name = "Mist Form (PvP)", Profession = Profession.Elementalist };
        public static Skill WardAgainstHarmPvP { get; } = new() { Id = 2806, Name = "Ward Against Harm (PvP)", Profession = Profession.Elementalist };
        public static Skill SlipperyGroundPvP { get; } = new() { Id = 3398, Name = "Slippery Ground (PvP)", Profession = Profession.Elementalist };
        public static Skill AssassinsRemedyPvP { get; } = new() { Id = 2869, Name = "Assassin's Remedy (PvP)", Profession = Profession.Assassin };
        public static Skill DeathBlossomPvP { get; } = new() { Id = 3061, Name = "Death Blossom (PvP)", Profession = Profession.Assassin };
        public static Skill FoxFangsPvP { get; } = new() { Id = 3251, Name = "Fox Fangs (PvP)", Profession = Profession.Assassin };
        public static Skill WildStrikePvP { get; } = new() { Id = 3252, Name = "Wild Strike (PvP)", Profession = Profession.Assassin };
        public static Skill SignetofDeadlyCorruptionPvP { get; } = new() { Id = 3053, Name = "Signet of Deadly Corruption (PvP)", Profession = Profession.Assassin };
        public static Skill ShadowFormPvP { get; } = new() { Id = 2862, Name = "Shadow Form (PvP)", Profession = Profession.Assassin };
        public static Skill ShroudofDistressPvP { get; } = new() { Id = 3048, Name = "Shroud of Distress (PvP)", Profession = Profession.Assassin };
        public static Skill UnseenFuryPvP { get; } = new() { Id = 3049, Name = "Unseen Fury (PvP)", Profession = Profession.Assassin };
        public static Skill AncestorsRagePvP { get; } = new() { Id = 2867, Name = "Ancestors' Rage (PvP)", Profession = Profession.Ritualist };
        public static Skill SplinterWeaponPvP { get; } = new() { Id = 2868, Name = "Splinter Weapon (PvP)", Profession = Profession.Ritualist };
        public static Skill SignetofSpiritsPvP { get; } = new() { Id = 2965, Name = "Signet of Spirits (PvP)", Profession = Profession.Ritualist };
        public static Skill DestructionPvP { get; } = new() { Id = 3008, Name = "Destruction (PvP)", Profession = Profession.Ritualist };
        public static Skill BloodsongPvP { get; } = new() { Id = 3019, Name = "Bloodsong (PvP)", Profession = Profession.Ritualist };
        public static Skill GazeofFuryPvP { get; } = new() { Id = 3022, Name = "Gaze of Fury (PvP)", Profession = Profession.Ritualist };
        public static Skill AgonyPvP { get; } = new() { Id = 3038, Name = "Agony (PvP)", Profession = Profession.Ritualist };
        public static Skill DestructiveWasGlaivePvP { get; } = new() { Id = 3157, Name = "Destructive Was Glaive (PvP)", Profession = Profession.Ritualist };
        public static Skill SignetofGhostlyMightPvP { get; } = new() { Id = 2966, Name = "Signet of Ghostly Might (PvP)", Profession = Profession.Ritualist };
        public static Skill ArmorofUnfeelingPvP { get; } = new() { Id = 3003, Name = "Armor of Unfeeling (PvP)", Profession = Profession.Ritualist };
        public static Skill UnionPvP { get; } = new() { Id = 3005, Name = "Union (PvP)", Profession = Profession.Ritualist };
        public static Skill ShadowsongPvP { get; } = new() { Id = 3006, Name = "Shadowsong (PvP)", Profession = Profession.Ritualist };
        public static Skill PainPvP { get; } = new() { Id = 3007, Name = "Pain (PvP)", Profession = Profession.Ritualist };
        public static Skill SoothingPvP { get; } = new() { Id = 3009, Name = "Soothing (PvP)", Profession = Profession.Ritualist };
        public static Skill DisplacementPvP { get; } = new() { Id = 3010, Name = "Displacement (PvP)", Profession = Profession.Ritualist };
        public static Skill DissonancePvP { get; } = new() { Id = 3014, Name = "Dissonance (PvP)", Profession = Profession.Ritualist };
        public static Skill EarthbindPvP { get; } = new() { Id = 3015, Name = "Earthbind (PvP)", Profession = Profession.Ritualist };
        public static Skill ShelterPvP { get; } = new() { Id = 3016, Name = "Shelter (PvP)", Profession = Profession.Ritualist };
        public static Skill DisenchantmentPvP { get; } = new() { Id = 3017, Name = "Disenchantment (PvP)", Profession = Profession.Ritualist };
        public static Skill RestorationPvP { get; } = new() { Id = 3018, Name = "Restoration (PvP)", Profession = Profession.Ritualist };
        public static Skill WanderlustPvP { get; } = new() { Id = 3020, Name = "Wanderlust (PvP)", Profession = Profession.Ritualist };
        public static Skill AnguishPvP { get; } = new() { Id = 3023, Name = "Anguish (PvP)", Profession = Profession.Ritualist };
        public static Skill FleshofMyFleshPvP { get; } = new() { Id = 2866, Name = "Flesh of My Flesh (PvP)", Profession = Profession.Ritualist };
        public static Skill DeathPactSignetPvP { get; } = new() { Id = 2872, Name = "Death Pact Signet (PvP)", Profession = Profession.Ritualist };
        public static Skill WeaponofWardingPvP { get; } = new() { Id = 2893, Name = "Weapon of Warding (PvP)", Profession = Profession.Ritualist };
        public static Skill PreservationPvP { get; } = new() { Id = 3011, Name = "Preservation (PvP)", Profession = Profession.Ritualist };
        public static Skill LifePvP { get; } = new() { Id = 3012, Name = "Life (PvP)", Profession = Profession.Ritualist };
        public static Skill RecuperationPvP { get; } = new() { Id = 3013, Name = "Recuperation (PvP)", Profession = Profession.Ritualist };
        public static Skill RecoveryPvP { get; } = new() { Id = 3025, Name = "Recovery (PvP)", Profession = Profession.Ritualist };
        public static Skill RejuvenationPvP { get; } = new() { Id = 3039, Name = "Rejuvenation (PvP)", Profession = Profession.Ritualist };
        public static Skill EmpowermentPvP { get; } = new() { Id = 3024, Name = "Empowerment (PvP)", Profession = Profession.Ritualist };
        public static Skill IncomingPvP { get; } = new() { Id = 2879, Name = "\"Incoming!\" (PvP)", Profession = Profession.Paragon };
        public static Skill NeverSurrenderPvP { get; } = new() { Id = 2880, Name = "\"Never Surrender!\" (PvP)", Profession = Profession.Paragon };
        public static Skill GofortheEyesPvP { get; } = new() { Id = 3026, Name = "\"Go for the Eyes!\" (PvP)", Profession = Profession.Paragon };
        public static Skill BraceYourselfPvP { get; } = new() { Id = 3027, Name = "\"Brace Yourself!\" (PvP)", Profession = Profession.Paragon };
        public static Skill BladeturnRefrainPvP { get; } = new() { Id = 3029, Name = "Bladeturn Refrain (PvP)", Profession = Profession.Paragon };
        public static Skill CantTouchThisPvP { get; } = new() { Id = 3031, Name = "\"Can't Touch This!\" (PvP)", Profession = Profession.Paragon };
        public static Skill StandYourGroundPvP { get; } = new() { Id = 3032, Name = "\"Stand Your Ground!\" (PvP)", Profession = Profession.Paragon };
        public static Skill WeShallReturnPvP { get; } = new() { Id = 3033, Name = "\"We Shall Return!\" (PvP)", Profession = Profession.Paragon };
        public static Skill FindTheirWeaknessPvP { get; } = new() { Id = 3034, Name = "\"Find Their Weakness!\" (PvP)", Profession = Profession.Paragon };
        public static Skill NeverGiveUpPvP { get; } = new() { Id = 3035, Name = "\"Never Give Up!\" (PvP)", Profession = Profession.Paragon };
        public static Skill HelpMePvP { get; } = new() { Id = 3036, Name = "\"Help Me!\" (PvP)", Profession = Profession.Paragon };
        public static Skill FallBackPvP { get; } = new() { Id = 3037, Name = "\"Fall Back!\" (PvP)", Profession = Profession.Paragon };
        public static Skill AnthemofDisruptionPvP { get; } = new() { Id = 3040, Name = "Anthem of Disruption (PvP)", Profession = Profession.Paragon };
        public static Skill AnthemofEnvyPvP { get; } = new() { Id = 3148, Name = "Anthem of Envy (PvP)", Profession = Profession.Paragon };
        public static Skill DefensiveAnthemPvP { get; } = new() { Id = 2876, Name = "Defensive Anthem (PvP)", Profession = Profession.Paragon };
        public static Skill BlazingFinalePvP { get; } = new() { Id = 3028, Name = "Blazing Finale (PvP)", Profession = Profession.Paragon };
        public static Skill SignetofReturnPvP { get; } = new() { Id = 3030, Name = "Signet of Return (PvP)", Profession = Profession.Paragon };
        public static Skill BalladofRestorationPvP { get; } = new() { Id = 2877, Name = "Ballad of Restoration (PvP)", Profession = Profession.Paragon };
        public static Skill SongofRestorationPvP { get; } = new() { Id = 2878, Name = "Song of Restoration (PvP)", Profession = Profession.Paragon };
        public static Skill FinaleofRestorationPvP { get; } = new() { Id = 3062, Name = "Finale of Restoration (PvP)", Profession = Profession.Paragon };
        public static Skill MendingRefrainPvP { get; } = new() { Id = 3149, Name = "Mending Refrain (PvP)", Profession = Profession.Paragon };
        public static Skill HarriersTossPvP { get; } = new() { Id = 2875, Name = "Harrier's Toss (PvP)", Profession = Profession.Paragon };
        public static Skill MysticRegenerationPvP { get; } = new() { Id = 2884, Name = "Mystic Regeneration (PvP)", Profession = Profession.Dervish };
        public static Skill AuraofThornsPvP { get; } = new() { Id = 3346, Name = "Aura of Thorns (PvP)", Profession = Profession.Dervish };
        public static Skill DustCloakPvP { get; } = new() { Id = 3347, Name = "Dust Cloak (PvP)", Profession = Profession.Dervish };
        public static Skill BanishingStrikePvP { get; } = new() { Id = 3263, Name = "Banishing Strike (PvP)", Profession = Profession.Dervish };
        public static Skill AvatarofDwaynaPvP { get; } = new() { Id = 3270, Name = "Avatar of Dwayna (PvP)", Profession = Profession.Dervish };
        public static Skill AvatarofMelandruPvP { get; } = new() { Id = 3271, Name = "Avatar of Melandru (PvP)", Profession = Profession.Dervish };
        public static Skill HeartofFuryPvP { get; } = new() { Id = 3366, Name = "Heart of Fury (PvP)", Profession = Profession.Dervish };
        public static Skill PiousFuryPvP { get; } = new() { Id = 3368, Name = "Pious Fury (PvP)", Profession = Profession.Dervish };
        public static Skill TwinMoonSweepPvP { get; } = new() { Id = 3264, Name = "Twin Moon Sweep (PvP)", Profession = Profession.Dervish };
        public static Skill IrresistibleSweepPvP { get; } = new() { Id = 3265, Name = "Irresistible Sweep (PvP)", Profession = Profession.Dervish };
        public static Skill PiousAssaultPvP { get; } = new() { Id = 3266, Name = "Pious Assault (PvP)", Profession = Profession.Dervish };
        public static Skill WoundingStrikePvP { get; } = new() { Id = 3367, Name = "Wounding Strike (PvP)", Profession = Profession.Dervish };
        public static Skill GuidingHandsPvP { get; } = new() { Id = 3269, Name = "Guiding Hands (PvP)", Profession = Profession.Dervish };
        public static Skill MysticHealingPvP { get; } = new() { Id = 3272, Name = "Mystic Healing (PvP)", Profession = Profession.Dervish };
        public static Skill SignetofPiousRestraintPvP { get; } = new() { Id = 3273, Name = "Signet of Pious Restraint (PvP)", Profession = Profession.Dervish };
        public static Skill LyssasHastePvP { get; } = new() { Id = 3348, Name = "Lyssa's Haste (PvP)", Profession = Profession.Dervish };
        public static Skill OnslaughtPvP { get; } = new() { Id = 3365, Name = "Onslaught (PvP)", Profession = Profession.Dervish };
        public static Skill SunspearRebirthSignet { get; } = new() { Id = 1816, Name = "Sunspear Rebirth Signet", Profession = Profession.None };
        public static Skill WhirlwindAttack { get; } = new() { Id = 2107, Name = "Whirlwind Attack", Profession = Profession.Warrior };
        public static Skill NeverRampageAlone { get; } = new() { Id = 2108, Name = "Never Rampage Alone", Profession = Profession.Ranger };
        public static Skill SeedofLife { get; } = new() { Id = 2105, Name = "Seed of Life", Profession = Profession.Monk };
        public static Skill Necrosis { get; } = new() { Id = 2103, Name = "Necrosis", Profession = Profession.Necromancer };
        public static Skill CryofPain { get; } = new() { Id = 2102, Name = "Cry of Pain", Profession = Profession.Mesmer };
        public static Skill Intensity { get; } = new() { Id = 2104, Name = "Intensity", Profession = Profession.Elementalist };
        public static Skill CriticalAgility { get; } = new() { Id = 2101, Name = "Critical Agility", Profession = Profession.Assassin };
        public static Skill Vampirism { get; } = new() { Id = 2110, Name = "Vampirism", Profession = Profession.Ritualist };
        public static Skill TheresNothingtoFear { get; } = new() { Id = 2112, Name = "\"There's Nothing to Fear!\"", Profession = Profession.Paragon };
        public static Skill EternalAura { get; } = new() { Id = 2109, Name = "Eternal Aura", Profession = Profession.Dervish };
        public static Skill LightbringersGaze { get; } = new() { Id = 1814, Name = "Lightbringer's Gaze", Profession = Profession.None };
        public static Skill LightbringerSignet { get; } = new() { Id = 1815, Name = "Lightbringer Signet", Profession = Profession.None };
        public static Skill SaveYourselves { get; } = new() { Id = 1954, Name = "\"Save Yourselves!\"", Profession = Profession.Warrior };
        public static Skill TripleShot { get; } = new() { Id = 1953, Name = "Triple Shot", Profession = Profession.Ranger };
        public static Skill SelflessSpirit { get; } = new() { Id = 1952, Name = "Selfless Spirit", Profession = Profession.Monk };
        public static Skill SignetofCorruption { get; } = new() { Id = 1950, Name = "Signet of Corruption", Profession = Profession.Necromancer };
        public static Skill EtherNightmare { get; } = new() { Id = 1949, Name = "Ether Nightmare", Profession = Profession.Mesmer };
        public static Skill ElementalLord { get; } = new() { Id = 1951, Name = "Elemental Lord", Profession = Profession.Elementalist };
        public static Skill ShadowSanctuary { get; } = new() { Id = 1948, Name = "Shadow Sanctuary", Profession = Profession.Assassin };
        public static Skill SummonSpirits { get; } = new() { Id = 2051, Name = "Summon Spirits", Profession = Profession.Ritualist };
        public static Skill SpearofFury { get; } = new() { Id = 1957, Name = "Spear of Fury", Profession = Profession.Paragon };
        public static Skill AuraofHolyMight { get; } = new() { Id = 1955, Name = "Aura of Holy Might", Profession = Profession.Dervish };
        public static Skill SaveYourselves2 { get; } = new() { Id = 2097, Name = "\"Save Yourselves!\"", Profession = Profession.Warrior };
        public static Skill TripleShot2 { get; } = new() { Id = 2096, Name = "Triple Shot", Profession = Profession.Ranger };
        public static Skill SelflessSpirit2 { get; } = new() { Id = 2095, Name = "Selfless Spirit", Profession = Profession.Monk };
        public static Skill SignetofCorruption2 { get; } = new() { Id = 2093, Name = "Signet of Corruption", Profession = Profession.Necromancer };
        public static Skill EtherNightmare2 { get; } = new() { Id = 2092, Name = "Ether Nightmare", Profession = Profession.Mesmer };
        public static Skill ElementalLord2 { get; } = new() { Id = 2094, Name = "Elemental Lord", Profession = Profession.Elementalist };
        public static Skill ShadowSanctuary2 { get; } = new() { Id = 2091, Name = "Shadow Sanctuary", Profession = Profession.Assassin };
        public static Skill SummonSpirits2 { get; } = new() { Id = 2100, Name = "Summon Spirits", Profession = Profession.Ritualist };
        public static Skill SpearofFury2 { get; } = new() { Id = 2099, Name = "Spear of Fury", Profession = Profession.Paragon };
        public static Skill AuraofHolyMight2 { get; } = new() { Id = 2098, Name = "Aura of Holy Might", Profession = Profession.Dervish };
        public static Skill AirofSuperiority { get; } = new() { Id = 2416, Name = "Air of Superiority", Profession = Profession.None };
        public static Skill AsuranScan { get; } = new() { Id = 2415, Name = "Asuran Scan", Profession = Profession.None };
        public static Skill MentalBlock { get; } = new() { Id = 2417, Name = "Mental Block", Profession = Profession.None };
        public static Skill Mindbender { get; } = new() { Id = 2411, Name = "Mindbender", Profession = Profession.None };
        public static Skill PainInverter { get; } = new() { Id = 2418, Name = "Pain Inverter", Profession = Profession.None };
        public static Skill RadiationField { get; } = new() { Id = 2414, Name = "Radiation Field", Profession = Profession.None };
        public static Skill SmoothCriminal { get; } = new() { Id = 2412, Name = "Smooth Criminal", Profession = Profession.None };
        public static Skill SummonMursaat { get; } = new() { Id = 2224, Name = "Summon Mursaat", Profession = Profession.None };
        public static Skill SummonRubyDjinn { get; } = new() { Id = 2225, Name = "Summon Ruby Djinn", Profession = Profession.None };
        public static Skill SummonIceImp { get; } = new() { Id = 2226, Name = "Summon Ice Imp", Profession = Profession.None };
        public static Skill SummonNagaShaman { get; } = new() { Id = 2227, Name = "Summon Naga Shaman", Profession = Profession.None };
        public static Skill Technobabble { get; } = new() { Id = 2413, Name = "Technobabble", Profession = Profession.None };
        public static Skill ByUralsHammer { get; } = new() { Id = 2217, Name = "\"By Ural's Hammer!\"", Profession = Profession.None };
        public static Skill DontTrip { get; } = new() { Id = 2216, Name = "\"Don't Trip!\"", Profession = Profession.None };
        public static Skill AlkarsAlchemicalAcid { get; } = new() { Id = 2211, Name = "Alkar's Alchemical Acid", Profession = Profession.None };
        public static Skill BlackPowderMine { get; } = new() { Id = 2223, Name = "Black Powder Mine", Profession = Profession.None };
        public static Skill BrawlingHeadbutt { get; } = new() { Id = 2215, Name = "Brawling Headbutt", Profession = Profession.None };
        public static Skill BreathoftheGreatDwarf { get; } = new() { Id = 2221, Name = "Breath of the Great Dwarf", Profession = Profession.None };
        public static Skill DrunkenMaster { get; } = new() { Id = 2218, Name = "Drunken Master", Profession = Profession.None };
        public static Skill DwarvenStability { get; } = new() { Id = 2423, Name = "Dwarven Stability", Profession = Profession.None };
        public static Skill EarBite { get; } = new() { Id = 2213, Name = "Ear Bite", Profession = Profession.None };
        public static Skill GreatDwarfArmor { get; } = new() { Id = 2220, Name = "Great Dwarf Armor", Profession = Profession.None };
        public static Skill GreatDwarfWeapon { get; } = new() { Id = 2219, Name = "Great Dwarf Weapon", Profession = Profession.None };
        public static Skill LightofDeldrimor { get; } = new() { Id = 2212, Name = "Light of Deldrimor", Profession = Profession.None };
        public static Skill LowBlow { get; } = new() { Id = 2214, Name = "Low Blow", Profession = Profession.None };
        public static Skill SnowStorm { get; } = new() { Id = 2222, Name = "Snow Storm", Profession = Profession.None };
        public static Skill DeftStrike { get; } = new() { Id = 2228, Name = "Deft Strike", Profession = Profession.None };
        public static Skill EbonBattleStandardofCourage { get; } = new() { Id = 2231, Name = "Ebon Battle Standard of Courage", Profession = Profession.None };
        public static Skill EbonBattleStandardofWisdom { get; } = new() { Id = 2232, Name = "Ebon Battle Standard of Wisdom", Profession = Profession.None };
        public static Skill EbonBattleStandardofHonor { get; } = new() { Id = 2233, Name = "Ebon Battle Standard of Honor", Profession = Profession.None };
        public static Skill EbonEscape { get; } = new() { Id = 2420, Name = "Ebon Escape", Profession = Profession.None };
        public static Skill EbonVanguardAssassinSupport { get; } = new() { Id = 2235, Name = "Ebon Vanguard Assassin Support", Profession = Profession.None };
        public static Skill EbonVanguardSniperSupport { get; } = new() { Id = 2234, Name = "Ebon Vanguard Sniper Support", Profession = Profession.None };
        public static Skill SignetofInfection { get; } = new() { Id = 2229, Name = "Signet of Infection", Profession = Profession.None };
        public static Skill SneakAttack { get; } = new() { Id = 2116, Name = "Sneak Attack", Profession = Profession.None };
        public static Skill TryptophanSignet { get; } = new() { Id = 2230, Name = "Tryptophan Signet", Profession = Profession.None };
        public static Skill WeaknessTrap { get; } = new() { Id = 2421, Name = "Weakness Trap", Profession = Profession.None };
        public static Skill Winds { get; } = new() { Id = 2422, Name = "Winds", Profession = Profession.None };
        public static Skill DodgeThis { get; } = new() { Id = 2354, Name = "\"Dodge This!\"", Profession = Profession.None };
        public static Skill FinishHim { get; } = new() { Id = 2353, Name = "\"Finish Him!\"", Profession = Profession.None };
        public static Skill IAmUnstoppable { get; } = new() { Id = 2356, Name = "\"I Am Unstoppable!\"", Profession = Profession.None };
        public static Skill IAmtheStrongest { get; } = new() { Id = 2355, Name = "\"I Am the Strongest!\"", Profession = Profession.None };
        public static Skill YouAreAllWeaklings { get; } = new() { Id = 2359, Name = "\"You Are All Weaklings!\"", Profession = Profession.None };
        public static Skill YouMoveLikeaDwarf { get; } = new() { Id = 2358, Name = "\"You Move Like a Dwarf!\"", Profession = Profession.None };
        public static Skill ATouchofGuile { get; } = new() { Id = 2357, Name = "A Touch of Guile", Profession = Profession.None };
        public static Skill ClubofaThousandBears { get; } = new() { Id = 2361, Name = "Club of a Thousand Bears", Profession = Profession.None };
        public static Skill FeelNoPain { get; } = new() { Id = 2360, Name = "Feel No Pain", Profession = Profession.None };
        public static Skill RavenBlessing { get; } = new() { Id = 2384, Name = "Raven Blessing", Profession = Profession.None };
        public static Skill UrsanBlessing { get; } = new() { Id = 2374, Name = "Ursan Blessing", Profession = Profession.None };
        public static Skill VolfenBlessing { get; } = new() { Id = 2379, Name = "Volfen Blessing", Profession = Profession.None };
        public static Skill TimeWard { get; } = new() { Id = 3422, Name = "Time Ward", Profession = Profession.Mesmer };
        public static Skill SoulTaker { get; } = new() { Id = 3423, Name = "Soul Taker", Profession = Profession.Necromancer };
        public static Skill OverTheLimit { get; } = new() { Id = 3424, Name = "Over The Limit", Profession = Profession.Elementalist };
        public static Skill JudgementStrike { get; } = new() { Id = 3425, Name = "Judgement Strike", Profession = Profession.Monk };
        public static Skill SevenWeaponsStance { get; } = new() { Id = 3426, Name = "Seven Weapons Stance", Profession = Profession.Warrior };
        public static Skill Togetherasone { get; } = new() { Id = 3427, Name = "\"Together as one!\"", Profession = Profession.Ranger };
        public static Skill ShadowTheft { get; } = new() { Id = 3428, Name = "Shadow Theft", Profession = Profession.Assassin };
        public static Skill WeaponsofThreeForges { get; } = new() { Id = 3429, Name = "Weapons of Three Forges", Profession = Profession.Ritualist };
        public static Skill VowofRevolution { get; } = new() { Id = 3430, Name = "Vow of Revolution", Profession = Profession.Dervish };
        public static Skill HeroicRefrain { get; } = new() { Id = 3431, Name = "Heroic Refrain", Profession = Profession.Paragon };
        public static IEnumerable<Skill> Skills { get; } = new List<Skill>
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
        };

        public static bool TryParse(int id, out Skill skill)
        {
            skill = Skills.Where(skill => skill.Id == id).FirstOrDefault();
            if (skill is null)
            {
                return false;
            }

            return true;
        }
        public static bool TryParse(string name, out Skill skill)
        {
            skill = Skills.Where(skill => skill.Name == name).FirstOrDefault();
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

        public Profession Profession { get; private set; }
        public string Name { get; private set; }
        public int Id { get; private set; }
        private Skill()
        {
        }
    }
}
