using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Builds
{
    public sealed class Skill
    {
        public static Skill NoSkill { get; } = new() { Id = 0, Name = "No Skill" };
        public static Skill ResurrectionSignet { get; } = new() { Id = 2, Name = "Resurrection Signet" };
        public static Skill SignetofCapture { get; } = new() { Id = 3, Name = "Signet of Capture" };
        public static Skill CycloneAxe { get; } = new() { Id = 330, Name = "Cyclone Axe" };
        public static Skill AxeRake { get; } = new() { Id = 334, Name = "Axe Rake" };
        public static Skill Cleave { get; } = new() { Id = 335, Name = "Cleave" };
        public static Skill ExecutionersStrike { get; } = new() { Id = 336, Name = "Executioner's Strike" };
        public static Skill Dismember { get; } = new() { Id = 337, Name = "Dismember" };
        public static Skill Eviscerate { get; } = new() { Id = 338, Name = "Eviscerate" };
        public static Skill PenetratingBlow { get; } = new() { Id = 339, Name = "Penetrating Blow" };
        public static Skill DisruptingChop { get; } = new() { Id = 340, Name = "Disrupting Chop" };
        public static Skill SwiftChop { get; } = new() { Id = 341, Name = "Swift Chop" };
        public static Skill AxeTwist { get; } = new() { Id = 342, Name = "Axe Twist" };
        public static Skill LaceratingChop { get; } = new() { Id = 849, Name = "Lacerating Chop" };
        public static Skill WhirlingAxe { get; } = new() { Id = 888, Name = "Whirling Axe" };
        public static Skill FuriousAxe { get; } = new() { Id = 904, Name = "Furious Axe" };
        public static Skill TripleChop { get; } = new() { Id = 992, Name = "Triple Chop" };
        public static Skill PenetratingChop { get; } = new() { Id = 1136, Name = "Penetrating Chop" };
        public static Skill CriticalChop { get; } = new() { Id = 1402, Name = "Critical Chop" };
        public static Skill AgonizingChop { get; } = new() { Id = 1403, Name = "Agonizing Chop" };
        public static Skill Decapitate { get; } = new() { Id = 1696, Name = "Decapitate" };
        public static Skill KeenChop { get; } = new() { Id = 2009, Name = "Keen Chop" };
        public static Skill HammerBash { get; } = new() { Id = 331, Name = "Hammer Bash" };
        public static Skill BellySmash { get; } = new() { Id = 350, Name = "Belly Smash" };
        public static Skill MightyBlow { get; } = new() { Id = 351, Name = "Mighty Blow" };
        public static Skill CrushingBlow { get; } = new() { Id = 352, Name = "Crushing Blow" };
        public static Skill CrudeSwing { get; } = new() { Id = 353, Name = "Crude Swing" };
        public static Skill EarthShaker { get; } = new() { Id = 354, Name = "Earth Shaker" };
        public static Skill DevastatingHammer { get; } = new() { Id = 355, Name = "Devastating Hammer" };
        public static Skill IrresistibleBlow { get; } = new() { Id = 356, Name = "Irresistible Blow" };
        public static Skill CounterBlow { get; } = new() { Id = 357, Name = "Counter Blow" };
        public static Skill Backbreaker { get; } = new() { Id = 358, Name = "Backbreaker" };
        public static Skill HeavyBlow { get; } = new() { Id = 359, Name = "Heavy Blow" };
        public static Skill StaggeringBlow { get; } = new() { Id = 360, Name = "Staggering Blow" };
        public static Skill FierceBlow { get; } = new() { Id = 850, Name = "Fierce Blow" };
        public static Skill ForcefulBlow { get; } = new() { Id = 889, Name = "Forceful Blow" };
        public static Skill AuspiciousBlow { get; } = new() { Id = 905, Name = "Auspicious Blow" };
        public static Skill EnragedSmash { get; } = new() { Id = 993, Name = "Enraged Smash" };
        public static Skill RenewingSmash { get; } = new() { Id = 994, Name = "Renewing Smash" };
        public static Skill YetiSmash { get; } = new() { Id = 1137, Name = "Yeti Smash" };
        public static Skill MokeleSmash { get; } = new() { Id = 1409, Name = "Mokele Smash" };
        public static Skill OverbearingSmash { get; } = new() { Id = 1410, Name = "Overbearing Smash" };
        public static Skill MagehuntersSmash { get; } = new() { Id = 1697, Name = "Magehunter's Smash" };
        public static Skill PulverizingSmash { get; } = new() { Id = 2008, Name = "Pulverizing Smash" };
        public static Skill BattleRage { get; } = new() { Id = 317, Name = "Battle Rage" };
        public static Skill DefyPain { get; } = new() { Id = 318, Name = "Defy Pain" };
        public static Skill Rush { get; } = new() { Id = 319, Name = "Rush" };
        public static Skill PowerAttack { get; } = new() { Id = 322, Name = "Power Attack" };
        public static Skill ProtectorsStrike { get; } = new() { Id = 326, Name = "Protector's Strike" };
        public static Skill GriffonsSweep { get; } = new() { Id = 327, Name = "Griffon's Sweep" };
        public static Skill BullsStrike { get; } = new() { Id = 332, Name = "Bull's Strike" };
        public static Skill IWillAvengeYou { get; } = new() { Id = 333, Name = "I Will Avenge You!" };
        public static Skill EndurePain { get; } = new() { Id = 347, Name = "Endure Pain" };
        public static Skill Sprint { get; } = new() { Id = 349, Name = "Sprint" };
        public static Skill DolyakSignet { get; } = new() { Id = 361, Name = "Dolyak Signet" };
        public static Skill WarriorsCunning { get; } = new() { Id = 362, Name = "Warrior's Cunning" };
        public static Skill ShieldBash { get; } = new() { Id = 363, Name = "Shield Bash" };
        public static Skill IWillSurvive { get; } = new() { Id = 368, Name = "I Will Survive!" };
        public static Skill BerserkerStance { get; } = new() { Id = 370, Name = "Berserker Stance" };
        public static Skill WarriorsEndurance { get; } = new() { Id = 374, Name = "Warrior's Endurance" };
        public static Skill DwarvenBattleStance { get; } = new() { Id = 375, Name = "Dwarven Battle Stance" };
        public static Skill BullsCharge { get; } = new() { Id = 379, Name = "Bull's Charge" };
        public static Skill Flourish { get; } = new() { Id = 389, Name = "Flourish" };
        public static Skill PrimalRage { get; } = new() { Id = 831, Name = "Primal Rage" };
        public static Skill SignetofStrength { get; } = new() { Id = 944, Name = "Signet of Strength" };
        public static Skill TigerStance { get; } = new() { Id = 995, Name = "Tiger Stance" };
        public static Skill LeviathansSweep { get; } = new() { Id = 1134, Name = "Leviathan's Sweep" };
        public static Skill YouWillDie { get; } = new() { Id = 1141, Name = "You Will Die!" };
        public static Skill Flail { get; } = new() { Id = 1404, Name = "Flail" };
        public static Skill ChargingStrike { get; } = new() { Id = 1405, Name = "Charging Strike" };
        public static Skill Headbutt { get; } = new() { Id = 1406, Name = "Headbutt" };
        public static Skill LionsComfort { get; } = new() { Id = 1407, Name = "Lion's Comfort" };
        public static Skill RageoftheNtouka { get; } = new() { Id = 1408, Name = "Rage of the Ntouka" };
        public static Skill SignetofStamina { get; } = new() { Id = 1411, Name = "Signet of Stamina" };
        public static Skill BurstofAggression { get; } = new() { Id = 1413, Name = "Burst of Aggression" };
        public static Skill EnragingCharge { get; } = new() { Id = 1414, Name = "Enraging Charge" };
        public static Skill Counterattack { get; } = new() { Id = 1693, Name = "Counterattack" };
        public static Skill MagehunterStrike { get; } = new() { Id = 1694, Name = "Magehunter Strike" };
        public static Skill Disarm { get; } = new() { Id = 2066, Name = "Disarm" };
        public static Skill IMeanttoDoThat { get; } = new() { Id = 2067, Name = "I Meant to Do That!" };
        public static Skill BodyBlow { get; } = new() { Id = 2197, Name = "Body Blow" };
        public static Skill Hamstring { get; } = new() { Id = 320, Name = "Hamstring" };
        public static Skill PureStrike { get; } = new() { Id = 328, Name = "Pure Strike" };
        public static Skill HundredBlades { get; } = new() { Id = 381, Name = "Hundred Blades" };
        public static Skill SeverArtery { get; } = new() { Id = 382, Name = "Sever Artery" };
        public static Skill GalrathSlash { get; } = new() { Id = 383, Name = "Galrath Slash" };
        public static Skill Gash { get; } = new() { Id = 384, Name = "Gash" };
        public static Skill FinalThrust { get; } = new() { Id = 385, Name = "Final Thrust" };
        public static Skill SeekingBlade { get; } = new() { Id = 386, Name = "Seeking Blade" };
        public static Skill SavageSlash { get; } = new() { Id = 390, Name = "Savage Slash" };
        public static Skill SunandMoonSlash { get; } = new() { Id = 851, Name = "Sun and Moon Slash" };
        public static Skill QuiveringBlade { get; } = new() { Id = 892, Name = "Quivering Blade" };
        public static Skill DragonSlash { get; } = new() { Id = 907, Name = "Dragon Slash" };
        public static Skill StandingSlash { get; } = new() { Id = 996, Name = "Standing Slash" };
        public static Skill JaizhenjuStrike { get; } = new() { Id = 1135, Name = "Jaizhenju Strike" };
        public static Skill SilverwingSlash { get; } = new() { Id = 1144, Name = "Silverwing Slash" };
        public static Skill CripplingSlash { get; } = new() { Id = 1415, Name = "Crippling Slash" };
        public static Skill BarbarousSlice { get; } = new() { Id = 1416, Name = "Barbarous Slice" };
        public static Skill SteelfangSlash { get; } = new() { Id = 1702, Name = "Steelfang Slash" };
        public static Skill KneeCutter { get; } = new() { Id = 2010, Name = "Knee Cutter" };
        public static Skill HealingSignet { get; } = new() { Id = 1, Name = "Healing Signet" };
        public static Skill TotheLimit { get; } = new() { Id = 316, Name = "To the Limit!" };
        public static Skill DesperationBlow { get; } = new() { Id = 323, Name = "Desperation Blow" };
        public static Skill ThrillofVictory { get; } = new() { Id = 324, Name = "Thrill of Victory" };
        public static Skill DefensiveStance { get; } = new() { Id = 345, Name = "Defensive Stance" };
        public static Skill WatchYourself { get; } = new() { Id = 348, Name = "Watch Yourself!" };
        public static Skill Charge { get; } = new() { Id = 364, Name = "Charge!" };
        public static Skill VictoryIsMine { get; } = new() { Id = 365, Name = "Victory Is Mine!" };
        public static Skill FearMe { get; } = new() { Id = 366, Name = "Fear Me!" };
        public static Skill ShieldsUp { get; } = new() { Id = 367, Name = "Shields Up!" };
        public static Skill BalancedStance { get; } = new() { Id = 371, Name = "Balanced Stance" };
        public static Skill GladiatorsDefense { get; } = new() { Id = 372, Name = "Gladiator's Defense" };
        public static Skill DeflectArrows { get; } = new() { Id = 373, Name = "Deflect Arrows" };
        public static Skill DisciplinedStance { get; } = new() { Id = 376, Name = "Disciplined Stance" };
        public static Skill WaryStance { get; } = new() { Id = 377, Name = "Wary Stance" };
        public static Skill ShieldStance { get; } = new() { Id = 378, Name = "Shield Stance" };
        public static Skill BonettisDefense { get; } = new() { Id = 380, Name = "Bonetti's Defense" };
        public static Skill Riposte { get; } = new() { Id = 387, Name = "Riposte" };
        public static Skill DeadlyRiposte { get; } = new() { Id = 388, Name = "Deadly Riposte" };
        public static Skill ProtectorsDefense { get; } = new() { Id = 810, Name = "Protector's Defense" };
        public static Skill Retreat { get; } = new() { Id = 839, Name = "Retreat!" };
        public static Skill NoneShallPass { get; } = new() { Id = 891, Name = "None Shall Pass!" };
        public static Skill DrunkenBlow { get; } = new() { Id = 1133, Name = "Drunken Blow" };
        public static Skill AuspiciousParry { get; } = new() { Id = 1142, Name = "Auspicious Parry" };
        public static Skill Shove { get; } = new() { Id = 1146, Name = "Shove" };
        public static Skill SoldiersStrike { get; } = new() { Id = 1695, Name = "Soldier's Strike" };
        public static Skill SoldiersStance { get; } = new() { Id = 1698, Name = "Soldier's Stance" };
        public static Skill SoldiersDefense { get; } = new() { Id = 1699, Name = "Soldier's Defense" };
        public static Skill SteadyStance { get; } = new() { Id = 1701, Name = "Steady Stance" };
        public static Skill SoldiersSpeed { get; } = new() { Id = 2196, Name = "Soldier's Speed" };
        public static Skill WildBlow { get; } = new() { Id = 321, Name = "Wild Blow" };
        public static Skill DistractingBlow { get; } = new() { Id = 325, Name = "Distracting Blow" };
        public static Skill SkullCrack { get; } = new() { Id = 329, Name = "Skull Crack" };
        public static Skill ForGreatJustice { get; } = new() { Id = 343, Name = "For Great Justice!" };
        public static Skill Flurry { get; } = new() { Id = 344, Name = "Flurry" };
        public static Skill Frenzy { get; } = new() { Id = 346, Name = "Frenzy" };
        public static Skill Coward { get; } = new() { Id = 869, Name = "Coward!" };
        public static Skill OnYourKnees { get; } = new() { Id = 906, Name = "On Your Knees!" };
        public static Skill YoureAllAlone { get; } = new() { Id = 1412, Name = "You're All Alone!" };
        public static Skill FrenziedDefense { get; } = new() { Id = 1700, Name = "Frenzied Defense" };
        public static Skill Grapple { get; } = new() { Id = 2011, Name = "Grapple" };
        public static Skill DistractingStrike { get; } = new() { Id = 2194, Name = "Distracting Strike" };
        public static Skill SymbolicStrike { get; } = new() { Id = 2195, Name = "Symbolic Strike" };
        public static Skill CharmAnimal { get; } = new() { Id = 411, Name = "Charm Animal" };
        public static Skill CallofProtection { get; } = new() { Id = 412, Name = "Call of Protection" };
        public static Skill CallofHaste { get; } = new() { Id = 415, Name = "Call of Haste" };
        public static Skill ReviveAnimal { get; } = new() { Id = 422, Name = "Revive Animal" };
        public static Skill SymbioticBond { get; } = new() { Id = 423, Name = "Symbiotic Bond" };
        public static Skill ComfortAnimal { get; } = new() { Id = 436, Name = "Comfort Animal" };
        public static Skill BestialPounce { get; } = new() { Id = 437, Name = "Bestial Pounce" };
        public static Skill MaimingStrike { get; } = new() { Id = 438, Name = "Maiming Strike" };
        public static Skill FeralLunge { get; } = new() { Id = 439, Name = "Feral Lunge" };
        public static Skill ScavengerStrike { get; } = new() { Id = 440, Name = "Scavenger Strike" };
        public static Skill MelandrusAssault { get; } = new() { Id = 441, Name = "Melandru's Assault" };
        public static Skill FerociousStrike { get; } = new() { Id = 442, Name = "Ferocious Strike" };
        public static Skill PredatorsPounce { get; } = new() { Id = 443, Name = "Predator's Pounce" };
        public static Skill BrutalStrike { get; } = new() { Id = 444, Name = "Brutal Strike" };
        public static Skill DisruptingLunge { get; } = new() { Id = 445, Name = "Disrupting Lunge" };
        public static Skill OtyughsCry { get; } = new() { Id = 447, Name = "Otyugh's Cry" };
        public static Skill TigersFury { get; } = new() { Id = 454, Name = "Tiger's Fury" };
        public static Skill EdgeofExtinction { get; } = new() { Id = 464, Name = "Edge of Extinction" };
        public static Skill FertileSeason { get; } = new() { Id = 467, Name = "Fertile Season" };
        public static Skill Symbiosis { get; } = new() { Id = 468, Name = "Symbiosis" };
        public static Skill PrimalEchoes { get; } = new() { Id = 469, Name = "Primal Echoes" };
        public static Skill PredatorySeason { get; } = new() { Id = 470, Name = "Predatory Season" };
        public static Skill EnergizingWind { get; } = new() { Id = 474, Name = "Energizing Wind" };
        public static Skill RunasOne { get; } = new() { Id = 811, Name = "Run as One" };
        public static Skill Lacerate { get; } = new() { Id = 961, Name = "Lacerate" };
        public static Skill PredatoryBond { get; } = new() { Id = 1194, Name = "Predatory Bond" };
        public static Skill HealasOne { get; } = new() { Id = 1195, Name = "Heal as One" };
        public static Skill SavagePounce { get; } = new() { Id = 1201, Name = "Savage Pounce" };
        public static Skill EnragedLunge { get; } = new() { Id = 1202, Name = "Enraged Lunge" };
        public static Skill BestialMauling { get; } = new() { Id = 1203, Name = "Bestial Mauling" };
        public static Skill PoisonousBite { get; } = new() { Id = 1205, Name = "Poisonous Bite" };
        public static Skill Pounce { get; } = new() { Id = 1206, Name = "Pounce" };
        public static Skill BestialFury { get; } = new() { Id = 1209, Name = "Bestial Fury" };
        public static Skill VipersNest { get; } = new() { Id = 1211, Name = "Viper's Nest" };
        public static Skill StrikeasOne { get; } = new() { Id = 1468, Name = "Strike as One" };
        public static Skill Toxicity { get; } = new() { Id = 1472, Name = "Toxicity" };
        public static Skill RampageasOne { get; } = new() { Id = 1721, Name = "Rampage as One" };
        public static Skill HeketsRampage { get; } = new() { Id = 1728, Name = "Heket's Rampage" };
        public static Skill Companionship { get; } = new() { Id = 2141, Name = "Companionship" };
        public static Skill FeralAggression { get; } = new() { Id = 2142, Name = "Feral Aggression" };
        public static Skill DistractingShot { get; } = new() { Id = 399, Name = "Distracting Shot" };
        public static Skill OathShot { get; } = new() { Id = 405, Name = "Oath Shot" };
        public static Skill PointBlankShot { get; } = new() { Id = 407, Name = "Point Blank Shot" };
        public static Skill ThrowDirt { get; } = new() { Id = 424, Name = "Throw Dirt" };
        public static Skill Dodge { get; } = new() { Id = 425, Name = "Dodge" };
        public static Skill MarksmansWager { get; } = new() { Id = 430, Name = "Marksman's Wager" };
        public static Skill Escape { get; } = new() { Id = 448, Name = "Escape" };
        public static Skill PracticedStance { get; } = new() { Id = 449, Name = "Practiced Stance" };
        public static Skill WhirlingDefense { get; } = new() { Id = 450, Name = "Whirling Defense" };
        public static Skill LightningReflexes { get; } = new() { Id = 453, Name = "Lightning Reflexes" };
        public static Skill TrappersFocus { get; } = new() { Id = 946, Name = "Trapper's Focus" };
        public static Skill ZojunsShot { get; } = new() { Id = 1192, Name = "Zojun's Shot" };
        public static Skill ZojunsHaste { get; } = new() { Id = 1196, Name = "Zojun's Haste" };
        public static Skill GlassArrows { get; } = new() { Id = 1199, Name = "Glass Arrows" };
        public static Skill ArchersSignet { get; } = new() { Id = 1200, Name = "Archer's Signet" };
        public static Skill TrappersSpeed { get; } = new() { Id = 1475, Name = "Trapper's Speed" };
        public static Skill ExpertsDexterity { get; } = new() { Id = 1724, Name = "Expert's Dexterity" };
        public static Skill InfuriatingHeat { get; } = new() { Id = 1730, Name = "Infuriating Heat" };
        public static Skill ExpertFocus { get; } = new() { Id = 2145, Name = "Expert Focus" };
        public static Skill HuntersShot { get; } = new() { Id = 391, Name = "Hunter's Shot" };
        public static Skill PinDown { get; } = new() { Id = 392, Name = "Pin Down" };
        public static Skill CripplingShot { get; } = new() { Id = 393, Name = "Crippling Shot" };
        public static Skill PowerShot { get; } = new() { Id = 394, Name = "Power Shot" };
        public static Skill Barrage { get; } = new() { Id = 395, Name = "Barrage" };
        public static Skill PenetratingAttack { get; } = new() { Id = 398, Name = "Penetrating Attack" };
        public static Skill PrecisionShot { get; } = new() { Id = 400, Name = "Precision Shot" };
        public static Skill DeterminedShot { get; } = new() { Id = 402, Name = "Determined Shot" };
        public static Skill DebilitatingShot { get; } = new() { Id = 406, Name = "Debilitating Shot" };
        public static Skill ConcussionShot { get; } = new() { Id = 408, Name = "Concussion Shot" };
        public static Skill PunishingShot { get; } = new() { Id = 409, Name = "Punishing Shot" };
        public static Skill SavageShot { get; } = new() { Id = 426, Name = "Savage Shot" };
        public static Skill ReadtheWind { get; } = new() { Id = 432, Name = "Read the Wind" };
        public static Skill FavorableWinds { get; } = new() { Id = 472, Name = "Favorable Winds" };
        public static Skill SplinterShot { get; } = new() { Id = 852, Name = "Splinter Shot" };
        public static Skill MelandrusShot { get; } = new() { Id = 853, Name = "Melandru's Shot" };
        public static Skill SeekingArrows { get; } = new() { Id = 893, Name = "Seeking Arrows" };
        public static Skill MaraudersShot { get; } = new() { Id = 908, Name = "Marauder's Shot" };
        public static Skill FocusedShot { get; } = new() { Id = 909, Name = "Focused Shot" };
        public static Skill SunderingAttack { get; } = new() { Id = 1191, Name = "Sundering Attack" };
        public static Skill NeedlingShot { get; } = new() { Id = 1197, Name = "Needling Shot" };
        public static Skill BroadHeadArrow { get; } = new() { Id = 1198, Name = "Broad Head Arrow" };
        public static Skill PreparedShot { get; } = new() { Id = 1465, Name = "Prepared Shot" };
        public static Skill BurningArrow { get; } = new() { Id = 1466, Name = "Burning Arrow" };
        public static Skill ArcingShot { get; } = new() { Id = 1467, Name = "Arcing Shot" };
        public static Skill Crossfire { get; } = new() { Id = 1469, Name = "Crossfire" };
        public static Skill ScreamingShot { get; } = new() { Id = 1719, Name = "Screaming Shot" };
        public static Skill KeenArrow { get; } = new() { Id = 1720, Name = "Keen Arrow" };
        public static Skill DisruptingAccuracy { get; } = new() { Id = 1723, Name = "Disrupting Accuracy" };
        public static Skill RapidFire { get; } = new() { Id = 2068, Name = "Rapid Fire" };
        public static Skill SlothHuntersShot { get; } = new() { Id = 2069, Name = "Sloth Hunter's Shot" };
        public static Skill DisruptingShot { get; } = new() { Id = 2143, Name = "Disrupting Shot" };
        public static Skill Volley { get; } = new() { Id = 2144, Name = "Volley" };
        public static Skill BodyShot { get; } = new() { Id = 2198, Name = "Body Shot" };
        public static Skill PoisonArrow { get; } = new() { Id = 404, Name = "Poison Arrow" };
        public static Skill IncendiaryArrows { get; } = new() { Id = 428, Name = "Incendiary Arrows" };
        public static Skill MelandrusArrows { get; } = new() { Id = 429, Name = "Melandru's Arrows" };
        public static Skill IgniteArrows { get; } = new() { Id = 431, Name = "Ignite Arrows" };
        public static Skill KindleArrows { get; } = new() { Id = 433, Name = "Kindle Arrows" };
        public static Skill ChokingGas { get; } = new() { Id = 434, Name = "Choking Gas" };
        public static Skill ApplyPoison { get; } = new() { Id = 435, Name = "Apply Poison" };
        public static Skill TrollUnguent { get; } = new() { Id = 446, Name = "Troll Unguent" };
        public static Skill MelandrusResilience { get; } = new() { Id = 451, Name = "Melandru's Resilience" };
        public static Skill DrydersDefenses { get; } = new() { Id = 452, Name = "Dryder's Defenses" };
        public static Skill StormChaser { get; } = new() { Id = 455, Name = "Storm Chaser" };
        public static Skill SerpentsQuickness { get; } = new() { Id = 456, Name = "Serpent's Quickness" };
        public static Skill DustTrap { get; } = new() { Id = 457, Name = "Dust Trap" };
        public static Skill BarbedTrap { get; } = new() { Id = 458, Name = "Barbed Trap" };
        public static Skill FlameTrap { get; } = new() { Id = 459, Name = "Flame Trap" };
        public static Skill HealingSpring { get; } = new() { Id = 460, Name = "Healing Spring" };
        public static Skill SpikeTrap { get; } = new() { Id = 461, Name = "Spike Trap" };
        public static Skill Winter { get; } = new() { Id = 462, Name = "Winter" };
        public static Skill Winnowing { get; } = new() { Id = 463, Name = "Winnowing" };
        public static Skill GreaterConflagration { get; } = new() { Id = 465, Name = "Greater Conflagration" };
        public static Skill Conflagration { get; } = new() { Id = 466, Name = "Conflagration" };
        public static Skill FrozenSoil { get; } = new() { Id = 471, Name = "Frozen Soil" };
        public static Skill QuickeningZephyr { get; } = new() { Id = 475, Name = "Quickening Zephyr" };
        public static Skill NaturesRenewal { get; } = new() { Id = 476, Name = "Nature's Renewal" };
        public static Skill MuddyTerrain { get; } = new() { Id = 477, Name = "Muddy Terrain" };
        public static Skill Snare { get; } = new() { Id = 854, Name = "Snare" };
        public static Skill Pestilence { get; } = new() { Id = 870, Name = "Pestilence" };
        public static Skill Brambles { get; } = new() { Id = 947, Name = "Brambles" };
        public static Skill Famine { get; } = new() { Id = 997, Name = "Famine" };
        public static Skill Equinox { get; } = new() { Id = 1212, Name = "Equinox" };
        public static Skill Tranquility { get; } = new() { Id = 1213, Name = "Tranquility" };
        public static Skill BarbedArrows { get; } = new() { Id = 1470, Name = "Barbed Arrows" };
        public static Skill ScavengersFocus { get; } = new() { Id = 1471, Name = "Scavenger's Focus" };
        public static Skill Quicksand { get; } = new() { Id = 1473, Name = "Quicksand" };
        public static Skill Tripwire { get; } = new() { Id = 1476, Name = "Tripwire" };
        public static Skill RoaringWinds { get; } = new() { Id = 1725, Name = "Roaring Winds" };
        public static Skill NaturalStride { get; } = new() { Id = 1727, Name = "Natural Stride" };
        public static Skill SmokeTrap { get; } = new() { Id = 1729, Name = "Smoke Trap" };
        public static Skill PiercingTrap { get; } = new() { Id = 2140, Name = "Piercing Trap" };
        public static Skill PoisonTipSignet { get; } = new() { Id = 2199, Name = "Poison Tip Signet" };
        public static Skill DualShot { get; } = new() { Id = 396, Name = "Dual Shot" };
        public static Skill QuickShot { get; } = new() { Id = 397, Name = "Quick Shot" };
        public static Skill CalledShot { get; } = new() { Id = 403, Name = "Called Shot" };
        public static Skill AntidoteSignet { get; } = new() { Id = 427, Name = "Antidote Signet" };
        public static Skill StormsEmbrace { get; } = new() { Id = 1474, Name = "Storm's Embrace" };
        public static Skill ForkedArrow { get; } = new() { Id = 1722, Name = "Forked Arrow" };
        public static Skill MagebaneShot { get; } = new() { Id = 1726, Name = "Magebane Shot" };
        public static Skill DivineIntervention { get; } = new() { Id = 246, Name = "Divine Intervention" };
        public static Skill WatchfulSpirit { get; } = new() { Id = 255, Name = "Watchful Spirit" };
        public static Skill BlessedAura { get; } = new() { Id = 256, Name = "Blessed Aura" };
        public static Skill PeaceandHarmony { get; } = new() { Id = 266, Name = "Peace and Harmony" };
        public static Skill UnyieldingAura { get; } = new() { Id = 268, Name = "Unyielding Aura" };
        public static Skill SpellBreaker { get; } = new() { Id = 273, Name = "Spell Breaker" };
        public static Skill DivineHealing { get; } = new() { Id = 279, Name = "Divine Healing" };
        public static Skill DivineBoon { get; } = new() { Id = 284, Name = "Divine Boon" };
        public static Skill SignetofDevotion { get; } = new() { Id = 293, Name = "Signet of Devotion" };
        public static Skill BlessedSignet { get; } = new() { Id = 297, Name = "Blessed Signet" };
        public static Skill ContemplationofPurity { get; } = new() { Id = 300, Name = "Contemplation of Purity" };
        public static Skill DivineSpirit { get; } = new() { Id = 310, Name = "Divine Spirit" };
        public static Skill BoonSignet { get; } = new() { Id = 847, Name = "Boon Signet" };
        public static Skill BlessedLight { get; } = new() { Id = 941, Name = "Blessed Light" };
        public static Skill WithdrawHexes { get; } = new() { Id = 942, Name = "Withdraw Hexes" };
        public static Skill SpellShield { get; } = new() { Id = 957, Name = "Spell Shield" };
        public static Skill ReleaseEnchantments { get; } = new() { Id = 960, Name = "Release Enchantments" };
        public static Skill DenyHexes { get; } = new() { Id = 991, Name = "Deny Hexes" };
        public static Skill HeavensDelight { get; } = new() { Id = 1117, Name = "Heaven's Delight" };
        public static Skill WatchfulHealing { get; } = new() { Id = 1392, Name = "Watchful Healing" };
        public static Skill HealersBoon { get; } = new() { Id = 1393, Name = "Healer's Boon" };
        public static Skill ScribesInsight { get; } = new() { Id = 1684, Name = "Scribe's Insight" };
        public static Skill HolyHaste { get; } = new() { Id = 1685, Name = "Holy Haste" };
        public static Skill SmitersBoon { get; } = new() { Id = 2005, Name = "Smiter's Boon" };
        public static Skill VigorousSpirit { get; } = new() { Id = 254, Name = "Vigorous Spirit" };
        public static Skill HealingSeed { get; } = new() { Id = 274, Name = "Healing Seed" };
        public static Skill HealArea { get; } = new() { Id = 280, Name = "Heal Area" };
        public static Skill OrisonofHealing { get; } = new() { Id = 281, Name = "Orison of Healing" };
        public static Skill WordofHealing { get; } = new() { Id = 282, Name = "Word of Healing" };
        public static Skill DwaynasKiss { get; } = new() { Id = 283, Name = "Dwayna's Kiss" };
        public static Skill HealingHands { get; } = new() { Id = 285, Name = "Healing Hands" };
        public static Skill HealOther { get; } = new() { Id = 286, Name = "Heal Other" };
        public static Skill HealParty { get; } = new() { Id = 287, Name = "Heal Party" };
        public static Skill HealingBreeze { get; } = new() { Id = 288, Name = "Healing Breeze" };
        public static Skill Mending { get; } = new() { Id = 290, Name = "Mending" };
        public static Skill LiveVicariously { get; } = new() { Id = 291, Name = "Live Vicariously" };
        public static Skill InfuseHealth { get; } = new() { Id = 292, Name = "Infuse Health" };
        public static Skill HealingTouch { get; } = new() { Id = 313, Name = "Healing Touch" };
        public static Skill RestoreLife { get; } = new() { Id = 314, Name = "Restore Life" };
        public static Skill DwaynasSorrow { get; } = new() { Id = 838, Name = "Dwayna's Sorrow" };
        public static Skill HealingLight { get; } = new() { Id = 867, Name = "Healing Light" };
        public static Skill RestfulBreeze { get; } = new() { Id = 886, Name = "Restful Breeze" };
        public static Skill SignetofRejuvenation { get; } = new() { Id = 887, Name = "Signet of Rejuvenation" };
        public static Skill HealingWhisper { get; } = new() { Id = 958, Name = "Healing Whisper" };
        public static Skill EtherealLight { get; } = new() { Id = 959, Name = "Ethereal Light" };
        public static Skill HealingBurst { get; } = new() { Id = 1118, Name = "Healing Burst" };
        public static Skill KareisHealingCircle { get; } = new() { Id = 1119, Name = "Karei's Healing Circle" };
        public static Skill JameisGaze { get; } = new() { Id = 1120, Name = "Jamei's Gaze" };
        public static Skill GiftofHealth { get; } = new() { Id = 1121, Name = "Gift of Health" };
        public static Skill ResurrectionChant { get; } = new() { Id = 1128, Name = "Resurrection Chant" };
        public static Skill HealingRing { get; } = new() { Id = 1262, Name = "Healing Ring" };
        public static Skill RenewLife { get; } = new() { Id = 1263, Name = "Renew Life" };
        public static Skill SupportiveSpirit { get; } = new() { Id = 1391, Name = "Supportive Spirit" };
        public static Skill HealersCovenant { get; } = new() { Id = 1394, Name = "Healer's Covenant" };
        public static Skill WordsofComfort { get; } = new() { Id = 1396, Name = "Words of Comfort" };
        public static Skill LightofDeliverance { get; } = new() { Id = 1397, Name = "Light of Deliverance" };
        public static Skill GlimmerofLight { get; } = new() { Id = 1686, Name = "Glimmer of Light" };
        public static Skill CureHex { get; } = new() { Id = 2003, Name = "Cure Hex" };
        public static Skill PatientSpirit { get; } = new() { Id = 2061, Name = "Patient Spirit" };
        public static Skill HealingRibbon { get; } = new() { Id = 2062, Name = "Healing Ribbon" };
        public static Skill SpotlessMind { get; } = new() { Id = 2064, Name = "Spotless Mind" };
        public static Skill SpotlessSoul { get; } = new() { Id = 2065, Name = "Spotless Soul" };
        public static Skill LifeBond { get; } = new() { Id = 241, Name = "Life Bond" };
        public static Skill LifeAttunement { get; } = new() { Id = 244, Name = "Life Attunement" };
        public static Skill ProtectiveSpirit { get; } = new() { Id = 245, Name = "Protective Spirit" };
        public static Skill Aegis { get; } = new() { Id = 257, Name = "Aegis" };
        public static Skill Guardian { get; } = new() { Id = 258, Name = "Guardian" };
        public static Skill ShieldofDeflection { get; } = new() { Id = 259, Name = "Shield of Deflection" };
        public static Skill AuraofFaith { get; } = new() { Id = 260, Name = "Aura of Faith" };
        public static Skill ShieldofRegeneration { get; } = new() { Id = 261, Name = "Shield of Regeneration" };
        public static Skill ProtectiveBond { get; } = new() { Id = 263, Name = "Protective Bond" };
        public static Skill Pacifism { get; } = new() { Id = 264, Name = "Pacifism" };
        public static Skill Amity { get; } = new() { Id = 265, Name = "Amity" };
        public static Skill MarkofProtection { get; } = new() { Id = 269, Name = "Mark of Protection" };
        public static Skill LifeBarrier { get; } = new() { Id = 270, Name = "Life Barrier" };
        public static Skill MendCondition { get; } = new() { Id = 275, Name = "Mend Condition" };
        public static Skill RestoreCondition { get; } = new() { Id = 276, Name = "Restore Condition" };
        public static Skill MendAilment { get; } = new() { Id = 277, Name = "Mend Ailment" };
        public static Skill VitalBlessing { get; } = new() { Id = 289, Name = "Vital Blessing" };
        public static Skill ShieldingHands { get; } = new() { Id = 299, Name = "Shielding Hands" };
        public static Skill ConvertHexes { get; } = new() { Id = 303, Name = "Convert Hexes" };
        public static Skill Rebirth { get; } = new() { Id = 306, Name = "Rebirth" };
        public static Skill ReversalofFortune { get; } = new() { Id = 307, Name = "Reversal of Fortune" };
        public static Skill DrawConditions { get; } = new() { Id = 311, Name = "Draw Conditions" };
        public static Skill ReverseHex { get; } = new() { Id = 848, Name = "Reverse Hex" };
        public static Skill ShieldGuardian { get; } = new() { Id = 885, Name = "Shield Guardian" };
        public static Skill Extinguish { get; } = new() { Id = 943, Name = "Extinguish" };
        public static Skill SpiritBond { get; } = new() { Id = 1114, Name = "Spirit Bond" };
        public static Skill AirofEnchantment { get; } = new() { Id = 1115, Name = "Air of Enchantment" };
        public static Skill LifeSheath { get; } = new() { Id = 1123, Name = "Life Sheath" };
        public static Skill ShieldofAbsorption { get; } = new() { Id = 1399, Name = "Shield of Absorption" };
        public static Skill MendingTouch { get; } = new() { Id = 1401, Name = "Mending Touch" };
        public static Skill PensiveGuardian { get; } = new() { Id = 1683, Name = "Pensive Guardian" };
        public static Skill ZealousBenediction { get; } = new() { Id = 1687, Name = "Zealous Benediction" };
        public static Skill DismissCondition { get; } = new() { Id = 1691, Name = "Dismiss Condition" };
        public static Skill DivertHexes { get; } = new() { Id = 1692, Name = "Divert Hexes" };
        public static Skill PurifyingVeil { get; } = new() { Id = 2007, Name = "Purifying Veil" };
        public static Skill AuraofStability { get; } = new() { Id = 2063, Name = "Aura of Stability" };
        public static Skill Smite { get; } = new() { Id = 240, Name = "Smite" };
        public static Skill BalthazarsSpirit { get; } = new() { Id = 242, Name = "Balthazar's Spirit" };
        public static Skill StrengthofHonor { get; } = new() { Id = 243, Name = "Strength of Honor" };
        public static Skill SymbolofWrath { get; } = new() { Id = 247, Name = "Symbol of Wrath" };
        public static Skill Retribution { get; } = new() { Id = 248, Name = "Retribution" };
        public static Skill HolyWrath { get; } = new() { Id = 249, Name = "Holy Wrath" };
        public static Skill ScourgeHealing { get; } = new() { Id = 251, Name = "Scourge Healing" };
        public static Skill Banish { get; } = new() { Id = 252, Name = "Banish" };
        public static Skill ScourgeSacrifice { get; } = new() { Id = 253, Name = "Scourge Sacrifice" };
        public static Skill ShieldofJudgment { get; } = new() { Id = 262, Name = "Shield of Judgment" };
        public static Skill JudgesInsight { get; } = new() { Id = 267, Name = "Judge's Insight" };
        public static Skill ZealotsFire { get; } = new() { Id = 271, Name = "Zealot's Fire" };
        public static Skill BalthazarsAura { get; } = new() { Id = 272, Name = "Balthazar's Aura" };
        public static Skill SignetofJudgment { get; } = new() { Id = 294, Name = "Signet of Judgment" };
        public static Skill BaneSignet { get; } = new() { Id = 296, Name = "Bane Signet" };
        public static Skill SmiteHex { get; } = new() { Id = 302, Name = "Smite Hex" };
        public static Skill HolyStrike { get; } = new() { Id = 312, Name = "Holy Strike" };
        public static Skill RayofJudgment { get; } = new() { Id = 830, Name = "Ray of Judgment" };
        public static Skill KirinsWrath { get; } = new() { Id = 1113, Name = "Kirin's Wrath" };
        public static Skill WordofCensure { get; } = new() { Id = 1129, Name = "Word of Censure" };
        public static Skill SpearofLight { get; } = new() { Id = 1130, Name = "Spear of Light" };
        public static Skill StonesoulStrike { get; } = new() { Id = 1131, Name = "Stonesoul Strike" };
        public static Skill SignetofRage { get; } = new() { Id = 1269, Name = "Signet of Rage" };
        public static Skill JudgesIntervention { get; } = new() { Id = 1390, Name = "Judge's Intervention" };
        public static Skill BalthazarsPendulum { get; } = new() { Id = 1395, Name = "Balthazar's Pendulum" };
        public static Skill ScourgeEnchantment { get; } = new() { Id = 1398, Name = "Scourge Enchantment" };
        public static Skill ReversalofDamage { get; } = new() { Id = 1400, Name = "Reversal of Damage" };
        public static Skill DefendersZeal { get; } = new() { Id = 1688, Name = "Defender's Zeal" };
        public static Skill SignetofMysticWrath { get; } = new() { Id = 1689, Name = "Signet of Mystic Wrath" };
        public static Skill SmiteCondition { get; } = new() { Id = 2004, Name = "Smite Condition" };
        public static Skill CastigationSignet { get; } = new() { Id = 2006, Name = "Castigation Signet" };
        public static Skill EssenceBond { get; } = new() { Id = 250, Name = "Essence Bond" };
        public static Skill PurgeConditions { get; } = new() { Id = 278, Name = "Purge Conditions" };
        public static Skill PurgeSignet { get; } = new() { Id = 295, Name = "Purge Signet" };
        public static Skill Martyr { get; } = new() { Id = 298, Name = "Martyr" };
        public static Skill RemoveHex { get; } = new() { Id = 301, Name = "Remove Hex" };
        public static Skill LightofDwayna { get; } = new() { Id = 304, Name = "Light of Dwayna" };
        public static Skill Resurrect { get; } = new() { Id = 305, Name = "Resurrect" };
        public static Skill Succor { get; } = new() { Id = 308, Name = "Succor" };
        public static Skill HolyVeil { get; } = new() { Id = 309, Name = "Holy Veil" };
        public static Skill Vengeance { get; } = new() { Id = 315, Name = "Vengeance" };
        public static Skill EmpathicRemoval { get; } = new() { Id = 1126, Name = "Empathic Removal" };
        public static Skill SignetofRemoval { get; } = new() { Id = 1690, Name = "Signet of Removal" };
        public static Skill WellofPower { get; } = new() { Id = 91, Name = "Well of Power" };
        public static Skill WellofBlood { get; } = new() { Id = 92, Name = "Well of Blood" };
        public static Skill ShadowStrike { get; } = new() { Id = 102, Name = "Shadow Strike" };
        public static Skill LifeSiphon { get; } = new() { Id = 109, Name = "Life Siphon" };
        public static Skill UnholyFeast { get; } = new() { Id = 110, Name = "Unholy Feast" };
        public static Skill AwakentheBlood { get; } = new() { Id = 111, Name = "Awaken the Blood" };
        public static Skill BloodRenewal { get; } = new() { Id = 115, Name = "Blood Renewal" };
        public static Skill BloodisPower { get; } = new() { Id = 119, Name = "Blood is Power" };
        public static Skill LifeTransfer { get; } = new() { Id = 126, Name = "Life Transfer" };
        public static Skill MarkofSubversion { get; } = new() { Id = 127, Name = "Mark of Subversion" };
        public static Skill SoulLeech { get; } = new() { Id = 128, Name = "Soul Leech" };
        public static Skill DemonicFlesh { get; } = new() { Id = 130, Name = "Demonic Flesh" };
        public static Skill BarbedSignet { get; } = new() { Id = 131, Name = "Barbed Signet" };
        public static Skill DarkPact { get; } = new() { Id = 133, Name = "Dark Pact" };
        public static Skill OrderofPain { get; } = new() { Id = 134, Name = "Order of Pain" };
        public static Skill DarkBond { get; } = new() { Id = 138, Name = "Dark Bond" };
        public static Skill StripEnchantment { get; } = new() { Id = 143, Name = "Strip Enchantment" };
        public static Skill SignetofAgony { get; } = new() { Id = 145, Name = "Signet of Agony" };
        public static Skill OfferingofBlood { get; } = new() { Id = 146, Name = "Offering of Blood" };
        public static Skill DarkFury { get; } = new() { Id = 147, Name = "Dark Fury" };
        public static Skill OrderoftheVampire { get; } = new() { Id = 148, Name = "Order of the Vampire" };
        public static Skill VampiricGaze { get; } = new() { Id = 153, Name = "Vampiric Gaze" };
        public static Skill VampiricTouch { get; } = new() { Id = 156, Name = "Vampiric Touch" };
        public static Skill BloodRitual { get; } = new() { Id = 157, Name = "Blood Ritual" };
        public static Skill TouchofAgony { get; } = new() { Id = 158, Name = "Touch of Agony" };
        public static Skill JaundicedGaze { get; } = new() { Id = 763, Name = "Jaundiced Gaze" };
        public static Skill CultistsFervor { get; } = new() { Id = 806, Name = "Cultist's Fervor" };
        public static Skill VampiricSpirit { get; } = new() { Id = 819, Name = "Vampiric Spirit" };
        public static Skill BloodBond { get; } = new() { Id = 835, Name = "Blood Bond" };
        public static Skill RavenousGaze { get; } = new() { Id = 862, Name = "Ravenous Gaze" };
        public static Skill OppressiveGaze { get; } = new() { Id = 864, Name = "Oppressive Gaze" };
        public static Skill BloodoftheAggressor { get; } = new() { Id = 902, Name = "Blood of the Aggressor" };
        public static Skill SpoilVictor { get; } = new() { Id = 1066, Name = "Spoil Victor" };
        public static Skill LifebaneStrike { get; } = new() { Id = 1067, Name = "Lifebane Strike" };
        public static Skill VampiricSwarm { get; } = new() { Id = 1075, Name = "Vampiric Swarm" };
        public static Skill BloodDrinker { get; } = new() { Id = 1076, Name = "Blood Drinker" };
        public static Skill VampiricBite { get; } = new() { Id = 1077, Name = "Vampiric Bite" };
        public static Skill WallowsBite { get; } = new() { Id = 1078, Name = "Wallow's Bite" };
        public static Skill MarkofFury { get; } = new() { Id = 1360, Name = "Mark of Fury" };
        public static Skill SignetofSuffering { get; } = new() { Id = 1364, Name = "Signet of Suffering" };
        public static Skill ParasiticBond { get; } = new() { Id = 99, Name = "Parasitic Bond" };
        public static Skill SoulBarbs { get; } = new() { Id = 100, Name = "Soul Barbs" };
        public static Skill Barbs { get; } = new() { Id = 101, Name = "Barbs" };
        public static Skill PriceofFailure { get; } = new() { Id = 103, Name = "Price of Failure" };
        public static Skill Suffering { get; } = new() { Id = 108, Name = "Suffering" };
        public static Skill DesecrateEnchantments { get; } = new() { Id = 112, Name = "Desecrate Enchantments" };
        public static Skill Enfeeble { get; } = new() { Id = 117, Name = "Enfeeble" };
        public static Skill EnfeeblingBlood { get; } = new() { Id = 118, Name = "Enfeebling Blood" };
        public static Skill SpitefulSpirit { get; } = new() { Id = 121, Name = "Spiteful Spirit" };
        public static Skill InsidiousParasite { get; } = new() { Id = 123, Name = "Insidious Parasite" };
        public static Skill SpinalShivers { get; } = new() { Id = 124, Name = "Spinal Shivers" };
        public static Skill Wither { get; } = new() { Id = 125, Name = "Wither" };
        public static Skill DefileFlesh { get; } = new() { Id = 129, Name = "Defile Flesh" };
        public static Skill PlagueSignet { get; } = new() { Id = 132, Name = "Plague Signet" };
        public static Skill Faintheartedness { get; } = new() { Id = 135, Name = "Faintheartedness" };
        public static Skill ShadowofFear { get; } = new() { Id = 136, Name = "Shadow of Fear" };
        public static Skill RigorMortis { get; } = new() { Id = 137, Name = "Rigor Mortis" };
        public static Skill Malaise { get; } = new() { Id = 140, Name = "Malaise" };
        public static Skill RendEnchantments { get; } = new() { Id = 141, Name = "Rend Enchantments" };
        public static Skill LingeringCurse { get; } = new() { Id = 142, Name = "Lingering Curse" };
        public static Skill Chilblains { get; } = new() { Id = 144, Name = "Chilblains" };
        public static Skill PlagueSending { get; } = new() { Id = 149, Name = "Plague Sending" };
        public static Skill MarkofPain { get; } = new() { Id = 150, Name = "Mark of Pain" };
        public static Skill FeastofCorruption { get; } = new() { Id = 151, Name = "Feast of Corruption" };
        public static Skill PlagueTouch { get; } = new() { Id = 154, Name = "Plague Touch" };
        public static Skill WeakenArmor { get; } = new() { Id = 159, Name = "Weaken Armor" };
        public static Skill WellofWeariness { get; } = new() { Id = 818, Name = "Well of Weariness" };
        public static Skill Depravity { get; } = new() { Id = 820, Name = "Depravity" };
        public static Skill WeakenKnees { get; } = new() { Id = 822, Name = "Weaken Knees" };
        public static Skill RecklessHaste { get; } = new() { Id = 834, Name = "Reckless Haste" };
        public static Skill PoisonedHeart { get; } = new() { Id = 840, Name = "Poisoned Heart" };
        public static Skill OrderofApostasy { get; } = new() { Id = 863, Name = "Order of Apostasy" };
        public static Skill VocalMinority { get; } = new() { Id = 883, Name = "Vocal Minority" };
        public static Skill SoulBind { get; } = new() { Id = 901, Name = "Soul Bind" };
        public static Skill EnvenomEnchantments { get; } = new() { Id = 936, Name = "Envenom Enchantments" };
        public static Skill RipEnchantment { get; } = new() { Id = 955, Name = "Rip Enchantment" };
        public static Skill DefileEnchantments { get; } = new() { Id = 1070, Name = "Defile Enchantments" };
        public static Skill ShiversofDread { get; } = new() { Id = 1071, Name = "Shivers of Dread" };
        public static Skill EnfeeblingTouch { get; } = new() { Id = 1079, Name = "Enfeebling Touch" };
        public static Skill Meekness { get; } = new() { Id = 1260, Name = "Meekness" };
        public static Skill UlcerousLungs { get; } = new() { Id = 1358, Name = "Ulcerous Lungs" };
        public static Skill PainofDisenchantment { get; } = new() { Id = 1359, Name = "Pain of Disenchantment" };
        public static Skill CorruptEnchantment { get; } = new() { Id = 1362, Name = "Corrupt Enchantment" };
        public static Skill WellofDarkness { get; } = new() { Id = 1366, Name = "Well of Darkness" };
        public static Skill WellofSilence { get; } = new() { Id = 1660, Name = "Well of Silence" };
        public static Skill Cacophony { get; } = new() { Id = 1998, Name = "Cacophony" };
        public static Skill DefileDefenses { get; } = new() { Id = 2188, Name = "Defile Defenses" };
        public static Skill WellofRuin { get; } = new() { Id = 2236, Name = "Well of Ruin" };
        public static Skill Atrophy { get; } = new() { Id = 2237, Name = "Atrophy" };
        public static Skill AnimateBoneHorror { get; } = new() { Id = 83, Name = "Animate Bone Horror" };
        public static Skill AnimateBoneFiend { get; } = new() { Id = 84, Name = "Animate Bone Fiend" };
        public static Skill AnimateBoneMinions { get; } = new() { Id = 85, Name = "Animate Bone Minions" };
        public static Skill VeratasGaze { get; } = new() { Id = 87, Name = "Verata's Gaze" };
        public static Skill VeratasAura { get; } = new() { Id = 88, Name = "Verata's Aura" };
        public static Skill DeathlyChill { get; } = new() { Id = 89, Name = "Deathly Chill" };
        public static Skill VeratasSacrifice { get; } = new() { Id = 90, Name = "Verata's Sacrifice" };
        public static Skill WellofSuffering { get; } = new() { Id = 93, Name = "Well of Suffering" };
        public static Skill WelloftheProfane { get; } = new() { Id = 94, Name = "Well of the Profane" };
        public static Skill PutridExplosion { get; } = new() { Id = 95, Name = "Putrid Explosion" };
        public static Skill SoulFeast { get; } = new() { Id = 96, Name = "Soul Feast" };
        public static Skill NecroticTraversal { get; } = new() { Id = 97, Name = "Necrotic Traversal" };
        public static Skill ConsumeCorpse { get; } = new() { Id = 98, Name = "Consume Corpse" };
        public static Skill DeathNova { get; } = new() { Id = 104, Name = "Death Nova" };
        public static Skill DeathlySwarm { get; } = new() { Id = 105, Name = "Deathly Swarm" };
        public static Skill RottingFlesh { get; } = new() { Id = 106, Name = "Rotting Flesh" };
        public static Skill Virulence { get; } = new() { Id = 107, Name = "Virulence" };
        public static Skill TaintedFlesh { get; } = new() { Id = 113, Name = "Tainted Flesh" };
        public static Skill AuraoftheLich { get; } = new() { Id = 114, Name = "Aura of the Lich" };
        public static Skill DarkAura { get; } = new() { Id = 116, Name = "Dark Aura" };
        public static Skill BloodoftheMaster { get; } = new() { Id = 120, Name = "Blood of the Master" };
        public static Skill MalignIntervention { get; } = new() { Id = 122, Name = "Malign Intervention" };
        public static Skill InfuseCondition { get; } = new() { Id = 139, Name = "Infuse Condition" };
        public static Skill TasteofDeath { get; } = new() { Id = 152, Name = "Taste of Death" };
        public static Skill VileTouch { get; } = new() { Id = 155, Name = "Vile Touch" };
        public static Skill AnimateVampiricHorror { get; } = new() { Id = 805, Name = "Animate Vampiric Horror" };
        public static Skill Discord { get; } = new() { Id = 817, Name = "Discord" };
        public static Skill VileMiasma { get; } = new() { Id = 828, Name = "Vile Miasma" };
        public static Skill AnimateFleshGolem { get; } = new() { Id = 832, Name = "Animate Flesh Golem" };
        public static Skill FetidGround { get; } = new() { Id = 841, Name = "Fetid Ground" };
        public static Skill RisingBile { get; } = new() { Id = 935, Name = "Rising Bile" };
        public static Skill BitterChill { get; } = new() { Id = 1068, Name = "Bitter Chill" };
        public static Skill TasteofPain { get; } = new() { Id = 1069, Name = "Taste of Pain" };
        public static Skill AnimateShamblingHorror { get; } = new() { Id = 1351, Name = "Animate Shambling Horror" };
        public static Skill OrderofUndeath { get; } = new() { Id = 1352, Name = "Order of Undeath" };
        public static Skill PutridFlesh { get; } = new() { Id = 1353, Name = "Putrid Flesh" };
        public static Skill FeastfortheDead { get; } = new() { Id = 1354, Name = "Feast for the Dead" };
        public static Skill JaggedBones { get; } = new() { Id = 1355, Name = "Jagged Bones" };
        public static Skill Contagion { get; } = new() { Id = 1356, Name = "Contagion" };
        public static Skill ToxicChill { get; } = new() { Id = 1659, Name = "Toxic Chill" };
        public static Skill WitheringAura { get; } = new() { Id = 1997, Name = "Withering Aura" };
        public static Skill PutridBile { get; } = new() { Id = 2058, Name = "Putrid Bile" };
        public static Skill WailofDoom { get; } = new() { Id = 764, Name = "Wail of Doom" };
        public static Skill ReapersMark { get; } = new() { Id = 808, Name = "Reaper's Mark" };
        public static Skill IcyVeins { get; } = new() { Id = 821, Name = "Icy Veins" };
        public static Skill SignetofSorrow { get; } = new() { Id = 1363, Name = "Signet of Sorrow" };
        public static Skill SignetofLostSouls { get; } = new() { Id = 1365, Name = "Signet of Lost Souls" };
        public static Skill FoulFeast { get; } = new() { Id = 2057, Name = "Foul Feast" };
        public static Skill HexersVigor { get; } = new() { Id = 2138, Name = "Hexer's Vigor" };
        public static Skill Masochism { get; } = new() { Id = 2139, Name = "Masochism" };
        public static Skill AngorodonsGaze { get; } = new() { Id = 2189, Name = "Angorodon's Gaze" };
        public static Skill GrenthsBalance { get; } = new() { Id = 86, Name = "Grenth's Balance" };
        public static Skill GazeofContempt { get; } = new() { Id = 766, Name = "Gaze of Contempt" };
        public static Skill PowerBlock { get; } = new() { Id = 5, Name = "Power Block" };
        public static Skill HexBreaker { get; } = new() { Id = 10, Name = "Hex Breaker" };
        public static Skill PowerSpike { get; } = new() { Id = 23, Name = "Power Spike" };
        public static Skill PowerLeak { get; } = new() { Id = 24, Name = "Power Leak" };
        public static Skill Empathy { get; } = new() { Id = 26, Name = "Empathy" };
        public static Skill ShatterDelusions { get; } = new() { Id = 27, Name = "Shatter Delusions" };
        public static Skill Backfire { get; } = new() { Id = 28, Name = "Backfire" };
        public static Skill Blackout { get; } = new() { Id = 29, Name = "Blackout" };
        public static Skill Diversion { get; } = new() { Id = 30, Name = "Diversion" };
        public static Skill Ignorance { get; } = new() { Id = 35, Name = "Ignorance" };
        public static Skill EnergySurge { get; } = new() { Id = 39, Name = "Energy Surge" };
        public static Skill EnergyBurn { get; } = new() { Id = 42, Name = "Energy Burn" };
        public static Skill Guilt { get; } = new() { Id = 46, Name = "Guilt" };
        public static Skill MindWrack { get; } = new() { Id = 49, Name = "Mind Wrack" };
        public static Skill WastrelsWorry { get; } = new() { Id = 50, Name = "Wastrel's Worry" };
        public static Skill Shame { get; } = new() { Id = 51, Name = "Shame" };
        public static Skill Panic { get; } = new() { Id = 52, Name = "Panic" };
        public static Skill CryofFrustration { get; } = new() { Id = 57, Name = "Cry of Frustration" };
        public static Skill SignetofWeariness { get; } = new() { Id = 59, Name = "Signet of Weariness" };
        public static Skill ShatterHex { get; } = new() { Id = 67, Name = "Shatter Hex" };
        public static Skill ShatterEnchantment { get; } = new() { Id = 69, Name = "Shatter Enchantment" };
        public static Skill ChaosStorm { get; } = new() { Id = 77, Name = "Chaos Storm" };
        public static Skill ArcaneThievery { get; } = new() { Id = 81, Name = "Arcane Thievery" };
        public static Skill SignetofDisruption { get; } = new() { Id = 860, Name = "Signet of Disruption" };
        public static Skill VisionsofRegret { get; } = new() { Id = 878, Name = "Visions of Regret" };
        public static Skill Overload { get; } = new() { Id = 898, Name = "Overload" };
        public static Skill Complicate { get; } = new() { Id = 932, Name = "Complicate" };
        public static Skill UnnaturalSignet { get; } = new() { Id = 934, Name = "Unnatural Signet" };
        public static Skill PowerFlux { get; } = new() { Id = 953, Name = "Power Flux" };
        public static Skill Mistrust { get; } = new() { Id = 979, Name = "Mistrust" };
        public static Skill PsychicDistraction { get; } = new() { Id = 1053, Name = "Psychic Distraction" };
        public static Skill ArcaneLarceny { get; } = new() { Id = 1062, Name = "Arcane Larceny" };
        public static Skill WastrelsDemise { get; } = new() { Id = 1335, Name = "Wastrel's Demise" };
        public static Skill SpiritualPain { get; } = new() { Id = 1336, Name = "Spiritual Pain" };
        public static Skill EnchantersConundrum { get; } = new() { Id = 1345, Name = "Enchanter's Conundrum" };
        public static Skill HexEaterVortex { get; } = new() { Id = 1348, Name = "Hex Eater Vortex" };
        public static Skill SimpleThievery { get; } = new() { Id = 1350, Name = "Simple Thievery" };
        public static Skill PriceofPride { get; } = new() { Id = 1655, Name = "Price of Pride" };
        public static Skill SignetofDistraction { get; } = new() { Id = 1992, Name = "Signet of Distraction" };
        public static Skill PowerLock { get; } = new() { Id = 1994, Name = "Power Lock" };
        public static Skill Aneurysm { get; } = new() { Id = 2055, Name = "Aneurysm" };
        public static Skill MantraofRecovery { get; } = new() { Id = 13, Name = "Mantra of Recovery" };
        public static Skill KeystoneSignet { get; } = new() { Id = 63, Name = "Keystone Signet" };
        public static Skill ArcaneLanguor { get; } = new() { Id = 804, Name = "Arcane Languor" };
        public static Skill StolenSpeed { get; } = new() { Id = 880, Name = "Stolen Speed" };
        public static Skill PowerReturn { get; } = new() { Id = 931, Name = "Power Return" };
        public static Skill PsychicInstability { get; } = new() { Id = 1057, Name = "Psychic Instability" };
        public static Skill PersistenceofMemory { get; } = new() { Id = 1338, Name = "Persistence of Memory" };
        public static Skill SymbolsofInspiration { get; } = new() { Id = 1339, Name = "Symbols of Inspiration" };
        public static Skill SymbolicCelerity { get; } = new() { Id = 1340, Name = "Symbolic Celerity" };
        public static Skill SymbolicPosture { get; } = new() { Id = 1658, Name = "Symbolic Posture" };
        public static Skill Distortion { get; } = new() { Id = 11, Name = "Distortion" };
        public static Skill Fragility { get; } = new() { Id = 19, Name = "Fragility" };
        public static Skill ConjurePhantasm { get; } = new() { Id = 31, Name = "Conjure Phantasm" };
        public static Skill IllusionofWeakness { get; } = new() { Id = 32, Name = "Illusion of Weakness" };
        public static Skill IllusionaryWeaponry { get; } = new() { Id = 33, Name = "Illusionary Weaponry" };
        public static Skill SympatheticVisage { get; } = new() { Id = 34, Name = "Sympathetic Visage" };
        public static Skill ArcaneConundrum { get; } = new() { Id = 36, Name = "Arcane Conundrum" };
        public static Skill IllusionofHaste { get; } = new() { Id = 37, Name = "Illusion of Haste" };
        public static Skill Clumsiness { get; } = new() { Id = 43, Name = "Clumsiness" };
        public static Skill PhantomPain { get; } = new() { Id = 44, Name = "Phantom Pain" };
        public static Skill EtherealBurden { get; } = new() { Id = 45, Name = "Ethereal Burden" };
        public static Skill Ineptitude { get; } = new() { Id = 47, Name = "Ineptitude" };
        public static Skill Migraine { get; } = new() { Id = 53, Name = "Migraine" };
        public static Skill CripplingAnguish { get; } = new() { Id = 54, Name = "Crippling Anguish" };
        public static Skill FeveredDreams { get; } = new() { Id = 55, Name = "Fevered Dreams" };
        public static Skill SoothingImages { get; } = new() { Id = 56, Name = "Soothing Images" };
        public static Skill ImaginedBurden { get; } = new() { Id = 76, Name = "Imagined Burden" };
        public static Skill ConjureNightmare { get; } = new() { Id = 859, Name = "Conjure Nightmare" };
        public static Skill IllusionofPain { get; } = new() { Id = 879, Name = "Illusion of Pain" };
        public static Skill ImagesofRemorse { get; } = new() { Id = 899, Name = "Images of Remorse" };
        public static Skill SharedBurden { get; } = new() { Id = 900, Name = "Shared Burden" };
        public static Skill AccumulatedPain { get; } = new() { Id = 1052, Name = "Accumulated Pain" };
        public static Skill AncestorsVisage { get; } = new() { Id = 1054, Name = "Ancestor's Visage" };
        public static Skill RecurringInsecurity { get; } = new() { Id = 1055, Name = "Recurring Insecurity" };
        public static Skill KitahsBurden { get; } = new() { Id = 1056, Name = "Kitah's Burden" };
        public static Skill Frustration { get; } = new() { Id = 1341, Name = "Frustration" };
        public static Skill SignetofIllusions { get; } = new() { Id = 1346, Name = "Signet of Illusions" };
        public static Skill AirofDisenchantment { get; } = new() { Id = 1656, Name = "Air of Disenchantment" };
        public static Skill SignetofClumsiness { get; } = new() { Id = 1657, Name = "Signet of Clumsiness" };
        public static Skill SumofAllFears { get; } = new() { Id = 1996, Name = "Sum of All Fears" };
        public static Skill CalculatedRisk { get; } = new() { Id = 2053, Name = "Calculated Risk" };
        public static Skill ShrinkingArmor { get; } = new() { Id = 2054, Name = "Shrinking Armor" };
        public static Skill WanderingEye { get; } = new() { Id = 2056, Name = "Wandering Eye" };
        public static Skill ConfusingImages { get; } = new() { Id = 2137, Name = "Confusing Images" };
        public static Skill MantraofEarth { get; } = new() { Id = 6, Name = "Mantra of Earth" };
        public static Skill MantraofFlame { get; } = new() { Id = 7, Name = "Mantra of Flame" };
        public static Skill MantraofFrost { get; } = new() { Id = 8, Name = "Mantra of Frost" };
        public static Skill MantraofLightning { get; } = new() { Id = 9, Name = "Mantra of Lightning" };
        public static Skill MantraofPersistence { get; } = new() { Id = 14, Name = "Mantra of Persistence" };
        public static Skill MantraofInscriptions { get; } = new() { Id = 15, Name = "Mantra of Inscriptions" };
        public static Skill MantraofConcentration { get; } = new() { Id = 16, Name = "Mantra of Concentration" };
        public static Skill MantraofResolve { get; } = new() { Id = 17, Name = "Mantra of Resolve" };
        public static Skill MantraofSignets { get; } = new() { Id = 18, Name = "Mantra of Signets" };
        public static Skill InspiredEnchantment { get; } = new() { Id = 21, Name = "Inspired Enchantment" };
        public static Skill InspiredHex { get; } = new() { Id = 22, Name = "Inspired Hex" };
        public static Skill PowerDrain { get; } = new() { Id = 25, Name = "Power Drain" };
        public static Skill Channeling { get; } = new() { Id = 38, Name = "Channeling" };
        public static Skill EtherFeast { get; } = new() { Id = 40, Name = "Ether Feast" };
        public static Skill EtherLord { get; } = new() { Id = 41, Name = "Ether Lord" };
        public static Skill SpiritofFailure { get; } = new() { Id = 48, Name = "Spirit of Failure" };
        public static Skill LeechSignet { get; } = new() { Id = 61, Name = "Leech Signet" };
        public static Skill SignetofHumility { get; } = new() { Id = 62, Name = "Signet of Humility" };
        public static Skill SpiritShackles { get; } = new() { Id = 66, Name = "Spirit Shackles" };
        public static Skill DrainEnchantment { get; } = new() { Id = 68, Name = "Drain Enchantment" };
        public static Skill ElementalResistance { get; } = new() { Id = 72, Name = "Elemental Resistance" };
        public static Skill PhysicalResistance { get; } = new() { Id = 73, Name = "Physical Resistance" };
        public static Skill EnergyDrain { get; } = new() { Id = 79, Name = "Energy Drain" };
        public static Skill EnergyTap { get; } = new() { Id = 80, Name = "Energy Tap" };
        public static Skill MantraofRecall { get; } = new() { Id = 82, Name = "Mantra of Recall" };
        public static Skill PowerLeech { get; } = new() { Id = 803, Name = "Power Leech" };
        public static Skill LyssasAura { get; } = new() { Id = 813, Name = "Lyssa's Aura" };
        public static Skill EtherSignet { get; } = new() { Id = 881, Name = "Ether Signet" };
        public static Skill AuspiciousIncantation { get; } = new() { Id = 930, Name = "Auspicious Incantation" };
        public static Skill RevealedEnchantment { get; } = new() { Id = 1048, Name = "Revealed Enchantment" };
        public static Skill RevealedHex { get; } = new() { Id = 1049, Name = "Revealed Hex" };
        public static Skill HexEaterSignet { get; } = new() { Id = 1059, Name = "Hex Eater Signet" };
        public static Skill Feedback { get; } = new() { Id = 1061, Name = "Feedback" };
        public static Skill ExtendConditions { get; } = new() { Id = 1333, Name = "Extend Conditions" };
        public static Skill DrainDelusions { get; } = new() { Id = 1337, Name = "Drain Delusions" };
        public static Skill Tease { get; } = new() { Id = 1342, Name = "Tease" };
        public static Skill EtherPhantom { get; } = new() { Id = 1343, Name = "Ether Phantom" };
        public static Skill DischargeEnchantment { get; } = new() { Id = 1347, Name = "Discharge Enchantment" };
        public static Skill SignetofRecall { get; } = new() { Id = 1993, Name = "Signet of Recall" };
        public static Skill WasteNotWantNot { get; } = new() { Id = 1995, Name = "Waste Not, Want Not" };
        public static Skill SignetofMidnight { get; } = new() { Id = 58, Name = "Signet of Midnight" };
        public static Skill ArcaneMimicry { get; } = new() { Id = 65, Name = "Arcane Mimicry" };
        public static Skill Echo { get; } = new() { Id = 74, Name = "Echo" };
        public static Skill ArcaneEcho { get; } = new() { Id = 75, Name = "Arcane Echo" };
        public static Skill Epidemic { get; } = new() { Id = 78, Name = "Epidemic" };
        public static Skill LyssasBalance { get; } = new() { Id = 877, Name = "Lyssa's Balance" };
        public static Skill SignetofDisenchantment { get; } = new() { Id = 882, Name = "Signet of Disenchantment" };
        public static Skill ShatterStorm { get; } = new() { Id = 933, Name = "Shatter Storm" };
        public static Skill ExpelHexes { get; } = new() { Id = 954, Name = "Expel Hexes" };
        public static Skill Hypochondria { get; } = new() { Id = 1334, Name = "Hypochondria" };
        public static Skill WebofDisruption { get; } = new() { Id = 1344, Name = "Web of Disruption" };
        public static Skill MirrorofDisenchantment { get; } = new() { Id = 1349, Name = "Mirror of Disenchantment" };
        public static Skill WindborneSpeed { get; } = new() { Id = 160, Name = "Windborne Speed" };
        public static Skill Gale { get; } = new() { Id = 162, Name = "Gale" };
        public static Skill Whirlwind { get; } = new() { Id = 163, Name = "Whirlwind" };
        public static Skill LightningSurge { get; } = new() { Id = 205, Name = "Lightning Surge" };
        public static Skill BlindingFlash { get; } = new() { Id = 220, Name = "Blinding Flash" };
        public static Skill ConjureLightning { get; } = new() { Id = 221, Name = "Conjure Lightning" };
        public static Skill LightningStrike { get; } = new() { Id = 222, Name = "Lightning Strike" };
        public static Skill ChainLightning { get; } = new() { Id = 223, Name = "Chain Lightning" };
        public static Skill EnervatingCharge { get; } = new() { Id = 224, Name = "Enervating Charge" };
        public static Skill AirAttunement { get; } = new() { Id = 225, Name = "Air Attunement" };
        public static Skill MindShock { get; } = new() { Id = 226, Name = "Mind Shock" };
        public static Skill GlimmeringMark { get; } = new() { Id = 227, Name = "Glimmering Mark" };
        public static Skill Thunderclap { get; } = new() { Id = 228, Name = "Thunderclap" };
        public static Skill LightningOrb { get; } = new() { Id = 229, Name = "Lightning Orb" };
        public static Skill LightningJavelin { get; } = new() { Id = 230, Name = "Lightning Javelin" };
        public static Skill Shock { get; } = new() { Id = 231, Name = "Shock" };
        public static Skill LightningTouch { get; } = new() { Id = 232, Name = "Lightning Touch" };
        public static Skill RidetheLightning { get; } = new() { Id = 836, Name = "Ride the Lightning" };
        public static Skill ArcLightning { get; } = new() { Id = 842, Name = "Arc Lightning" };
        public static Skill Gust { get; } = new() { Id = 843, Name = "Gust" };
        public static Skill LightningHammer { get; } = new() { Id = 865, Name = "Lightning Hammer" };
        public static Skill TeinaisWind { get; } = new() { Id = 1081, Name = "Teinai's Wind" };
        public static Skill ShockArrow { get; } = new() { Id = 1082, Name = "Shock Arrow" };
        public static Skill BlindingSurge { get; } = new() { Id = 1367, Name = "Blinding Surge" };
        public static Skill ChillingWinds { get; } = new() { Id = 1368, Name = "Chilling Winds" };
        public static Skill LightningBolt { get; } = new() { Id = 1369, Name = "Lightning Bolt" };
        public static Skill StormDjinnsHaste { get; } = new() { Id = 1370, Name = "Storm Djinn's Haste" };
        public static Skill InvokeLightning { get; } = new() { Id = 1664, Name = "Invoke Lightning" };
        public static Skill GlyphofSwiftness { get; } = new() { Id = 2002, Name = "Glyph of Swiftness" };
        public static Skill ShellShock { get; } = new() { Id = 2059, Name = "Shell Shock" };
        public static Skill ArmorofEarth { get; } = new() { Id = 165, Name = "Armor of Earth" };
        public static Skill KineticArmor { get; } = new() { Id = 166, Name = "Kinetic Armor" };
        public static Skill Eruption { get; } = new() { Id = 167, Name = "Eruption" };
        public static Skill MagneticAura { get; } = new() { Id = 168, Name = "Magnetic Aura" };
        public static Skill EarthAttunement { get; } = new() { Id = 169, Name = "Earth Attunement" };
        public static Skill Earthquake { get; } = new() { Id = 170, Name = "Earthquake" };
        public static Skill Stoning { get; } = new() { Id = 171, Name = "Stoning" };
        public static Skill StoneDaggers { get; } = new() { Id = 172, Name = "Stone Daggers" };
        public static Skill GraspingEarth { get; } = new() { Id = 173, Name = "Grasping Earth" };
        public static Skill Aftershock { get; } = new() { Id = 174, Name = "Aftershock" };
        public static Skill WardAgainstElements { get; } = new() { Id = 175, Name = "Ward Against Elements" };
        public static Skill WardAgainstMelee { get; } = new() { Id = 176, Name = "Ward Against Melee" };
        public static Skill WardAgainstFoes { get; } = new() { Id = 177, Name = "Ward Against Foes" };
        public static Skill IronMist { get; } = new() { Id = 216, Name = "Iron Mist" };
        public static Skill CrystalWave { get; } = new() { Id = 217, Name = "Crystal Wave" };
        public static Skill ObsidianFlesh { get; } = new() { Id = 218, Name = "Obsidian Flesh" };
        public static Skill ObsidianFlame { get; } = new() { Id = 219, Name = "Obsidian Flame" };
        public static Skill ChurningEarth { get; } = new() { Id = 844, Name = "Churning Earth" };
        public static Skill Shockwave { get; } = new() { Id = 937, Name = "Shockwave" };
        public static Skill WardofStability { get; } = new() { Id = 938, Name = "Ward of Stability" };
        public static Skill UnsteadyGround { get; } = new() { Id = 1083, Name = "Unsteady Ground" };
        public static Skill SliverArmor { get; } = new() { Id = 1084, Name = "Sliver Armor" };
        public static Skill AshBlast { get; } = new() { Id = 1085, Name = "Ash Blast" };
        public static Skill DragonsStomp { get; } = new() { Id = 1086, Name = "Dragon's Stomp" };
        public static Skill TeinaisCrystals { get; } = new() { Id = 1099, Name = "Teinai's Crystals" };
        public static Skill StoneStriker { get; } = new() { Id = 1371, Name = "Stone Striker" };
        public static Skill Sandstorm { get; } = new() { Id = 1372, Name = "Sandstorm" };
        public static Skill StoneSheath { get; } = new() { Id = 1373, Name = "Stone Sheath" };
        public static Skill EbonHawk { get; } = new() { Id = 1374, Name = "Ebon Hawk" };
        public static Skill StonefleshAura { get; } = new() { Id = 1375, Name = "Stoneflesh Aura" };
        public static Skill Glowstone { get; } = new() { Id = 1661, Name = "Glowstone" };
        public static Skill EarthenShackles { get; } = new() { Id = 2000, Name = "Earthen Shackles" };
        public static Skill WardofWeakness { get; } = new() { Id = 2001, Name = "Ward of Weakness" };
        public static Skill MagneticSurge { get; } = new() { Id = 2190, Name = "Magnetic Surge" };
        public static Skill ElementalAttunement { get; } = new() { Id = 164, Name = "Elemental Attunement" };
        public static Skill EtherProdigy { get; } = new() { Id = 178, Name = "Ether Prodigy" };
        public static Skill AuraofRestoration { get; } = new() { Id = 180, Name = "Aura of Restoration" };
        public static Skill EtherRenewal { get; } = new() { Id = 181, Name = "Ether Renewal" };
        public static Skill GlyphofEnergy { get; } = new() { Id = 199, Name = "Glyph of Energy" };
        public static Skill GlyphofLesserEnergy { get; } = new() { Id = 200, Name = "Glyph of Lesser Energy" };
        public static Skill EnergyBoon { get; } = new() { Id = 837, Name = "Energy Boon" };
        public static Skill GlyphofRestoration { get; } = new() { Id = 1376, Name = "Glyph of Restoration" };
        public static Skill EtherPrism { get; } = new() { Id = 1377, Name = "Ether Prism" };
        public static Skill MasterofMagic { get; } = new() { Id = 1378, Name = "Master of Magic" };
        public static Skill EnergyBlast { get; } = new() { Id = 2193, Name = "Energy Blast" };
        public static Skill IncendiaryBonds { get; } = new() { Id = 179, Name = "Incendiary Bonds" };
        public static Skill ConjureFlame { get; } = new() { Id = 182, Name = "Conjure Flame" };
        public static Skill Inferno { get; } = new() { Id = 183, Name = "Inferno" };
        public static Skill FireAttunement { get; } = new() { Id = 184, Name = "Fire Attunement" };
        public static Skill MindBurn { get; } = new() { Id = 185, Name = "Mind Burn" };
        public static Skill Fireball { get; } = new() { Id = 186, Name = "Fireball" };
        public static Skill Meteor { get; } = new() { Id = 187, Name = "Meteor" };
        public static Skill FlameBurst { get; } = new() { Id = 188, Name = "Flame Burst" };
        public static Skill RodgortsInvocation { get; } = new() { Id = 189, Name = "Rodgort's Invocation" };
        public static Skill MarkofRodgort { get; } = new() { Id = 190, Name = "Mark of Rodgort" };
        public static Skill Immolate { get; } = new() { Id = 191, Name = "Immolate" };
        public static Skill MeteorShower { get; } = new() { Id = 192, Name = "Meteor Shower" };
        public static Skill Phoenix { get; } = new() { Id = 193, Name = "Phoenix" };
        public static Skill Flare { get; } = new() { Id = 194, Name = "Flare" };
        public static Skill LavaFont { get; } = new() { Id = 195, Name = "Lava Font" };
        public static Skill SearingHeat { get; } = new() { Id = 196, Name = "Searing Heat" };
        public static Skill FireStorm { get; } = new() { Id = 197, Name = "Fire Storm" };
        public static Skill BurningSpeed { get; } = new() { Id = 823, Name = "Burning Speed" };
        public static Skill LavaArrows { get; } = new() { Id = 824, Name = "Lava Arrows" };
        public static Skill BedofCoals { get; } = new() { Id = 825, Name = "Bed of Coals" };
        public static Skill LiquidFlame { get; } = new() { Id = 845, Name = "Liquid Flame" };
        public static Skill SearingFlames { get; } = new() { Id = 884, Name = "Searing Flames" };
        public static Skill SmolderingEmbers { get; } = new() { Id = 1090, Name = "Smoldering Embers" };
        public static Skill DoubleDragon { get; } = new() { Id = 1091, Name = "Double Dragon" };
        public static Skill TeinaisHeat { get; } = new() { Id = 1093, Name = "Teinai's Heat" };
        public static Skill BreathofFire { get; } = new() { Id = 1094, Name = "Breath of Fire" };
        public static Skill StarBurst { get; } = new() { Id = 1095, Name = "Star Burst" };
        public static Skill GlowingGaze { get; } = new() { Id = 1379, Name = "Glowing Gaze" };
        public static Skill SavannahHeat { get; } = new() { Id = 1380, Name = "Savannah Heat" };
        public static Skill FlameDjinnsHaste { get; } = new() { Id = 1381, Name = "Flame Djinn's Haste" };
        public static Skill MindBlast { get; } = new() { Id = 1662, Name = "Mind Blast" };
        public static Skill ElementalFlame { get; } = new() { Id = 1663, Name = "Elemental Flame" };
        public static Skill GlyphofImmolation { get; } = new() { Id = 2060, Name = "Glyph of Immolation" };
        public static Skill Rust { get; } = new() { Id = 204, Name = "Rust" };
        public static Skill ArmorofFrost { get; } = new() { Id = 206, Name = "Armor of Frost" };
        public static Skill ConjureFrost { get; } = new() { Id = 207, Name = "Conjure Frost" };
        public static Skill WaterAttunement { get; } = new() { Id = 208, Name = "Water Attunement" };
        public static Skill MindFreeze { get; } = new() { Id = 209, Name = "Mind Freeze" };
        public static Skill IcePrison { get; } = new() { Id = 210, Name = "Ice Prison" };
        public static Skill IceSpikes { get; } = new() { Id = 211, Name = "Ice Spikes" };
        public static Skill FrozenBurst { get; } = new() { Id = 212, Name = "Frozen Burst" };
        public static Skill ShardStorm { get; } = new() { Id = 213, Name = "Shard Storm" };
        public static Skill IceSpear { get; } = new() { Id = 214, Name = "Ice Spear" };
        public static Skill Maelstrom { get; } = new() { Id = 215, Name = "Maelstrom" };
        public static Skill SwirlingAura { get; } = new() { Id = 233, Name = "Swirling Aura" };
        public static Skill DeepFreeze { get; } = new() { Id = 234, Name = "Deep Freeze" };
        public static Skill BlurredVision { get; } = new() { Id = 235, Name = "Blurred Vision" };
        public static Skill MistForm { get; } = new() { Id = 236, Name = "Mist Form" };
        public static Skill WaterTrident { get; } = new() { Id = 237, Name = "Water Trident" };
        public static Skill ArmorofMist { get; } = new() { Id = 238, Name = "Armor of Mist" };
        public static Skill WardAgainstHarm { get; } = new() { Id = 239, Name = "Ward Against Harm" };
        public static Skill Shatterstone { get; } = new() { Id = 809, Name = "Shatterstone" };
        public static Skill Steam { get; } = new() { Id = 846, Name = "Steam" };
        public static Skill VaporBlade { get; } = new() { Id = 866, Name = "Vapor Blade" };
        public static Skill IcyPrism { get; } = new() { Id = 903, Name = "Icy Prism" };
        public static Skill IcyShackles { get; } = new() { Id = 939, Name = "Icy Shackles" };
        public static Skill TeinaisPrison { get; } = new() { Id = 1097, Name = "Teinai's Prison" };
        public static Skill MirrorofIce { get; } = new() { Id = 1098, Name = "Mirror of Ice" };
        public static Skill FrigidArmor { get; } = new() { Id = 1261, Name = "Frigid Armor" };
        public static Skill FreezingGust { get; } = new() { Id = 1382, Name = "Freezing Gust" };
        public static Skill WintersEmbrace { get; } = new() { Id = 1999, Name = "Winter's Embrace" };
        public static Skill SlipperyGround { get; } = new() { Id = 2191, Name = "Slippery Ground" };
        public static Skill GlowingIce { get; } = new() { Id = 2192, Name = "Glowing Ice" };
        public static Skill GlyphofElementalPower { get; } = new() { Id = 198, Name = "Glyph of Elemental Power" };
        public static Skill GlyphofConcentration { get; } = new() { Id = 201, Name = "Glyph of Concentration" };
        public static Skill GlyphofSacrifice { get; } = new() { Id = 202, Name = "Glyph of Sacrifice" };
        public static Skill GlyphofRenewal { get; } = new() { Id = 203, Name = "Glyph of Renewal" };
        public static Skill SecondWind { get; } = new() { Id = 1088, Name = "Second Wind" };
        public static Skill GlyphofEssence { get; } = new() { Id = 1096, Name = "Glyph of Essence" };
        public static Skill TwistingFangs { get; } = new() { Id = 776, Name = "Twisting Fangs" };
        public static Skill BlackLotusStrike { get; } = new() { Id = 779, Name = "Black Lotus Strike" };
        public static Skill UnsuspectingStrike { get; } = new() { Id = 783, Name = "Unsuspecting Strike" };
        public static Skill SharpenDaggers { get; } = new() { Id = 926, Name = "Sharpen Daggers" };
        public static Skill CriticalEye { get; } = new() { Id = 1018, Name = "Critical Eye" };
        public static Skill CriticalStrike { get; } = new() { Id = 1019, Name = "Critical Strike" };
        public static Skill CriticalDefenses { get; } = new() { Id = 1027, Name = "Critical Defenses" };
        public static Skill DarkApostasy { get; } = new() { Id = 1029, Name = "Dark Apostasy" };
        public static Skill LocustsFury { get; } = new() { Id = 1030, Name = "Locust's Fury" };
        public static Skill SeepingWound { get; } = new() { Id = 1034, Name = "Seeping Wound" };
        public static Skill PalmStrike { get; } = new() { Id = 1045, Name = "Palm Strike" };
        public static Skill MaliciousStrike { get; } = new() { Id = 1633, Name = "Malicious Strike" };
        public static Skill ShatteringAssault { get; } = new() { Id = 1634, Name = "Shattering Assault" };
        public static Skill DeadlyHaste { get; } = new() { Id = 1638, Name = "Deadly Haste" };
        public static Skill AssassinsRemedy { get; } = new() { Id = 1639, Name = "Assassin's Remedy" };
        public static Skill WayoftheAssassin { get; } = new() { Id = 1649, Name = "Way of the Assassin" };
        public static Skill WayoftheMaster { get; } = new() { Id = 2187, Name = "Way of the Master" };
        public static Skill DeathBlossom { get; } = new() { Id = 775, Name = "Death Blossom" };
        public static Skill HornsoftheOx { get; } = new() { Id = 777, Name = "Horns of the Ox" };
        public static Skill FallingSpider { get; } = new() { Id = 778, Name = "Falling Spider" };
        public static Skill FoxFangs { get; } = new() { Id = 780, Name = "Fox Fangs" };
        public static Skill MoebiusStrike { get; } = new() { Id = 781, Name = "Moebius Strike" };
        public static Skill JaggedStrike { get; } = new() { Id = 782, Name = "Jagged Strike" };
        public static Skill DesperateStrike { get; } = new() { Id = 948, Name = "Desperate Strike" };
        public static Skill ExhaustingAssault { get; } = new() { Id = 975, Name = "Exhausting Assault" };
        public static Skill RepeatingStrike { get; } = new() { Id = 976, Name = "Repeating Strike" };
        public static Skill NineTailStrike { get; } = new() { Id = 986, Name = "Nine Tail Strike" };
        public static Skill TempleStrike { get; } = new() { Id = 988, Name = "Temple Strike" };
        public static Skill GoldenPhoenixStrike { get; } = new() { Id = 989, Name = "Golden Phoenix Strike" };
        public static Skill BladesofSteel { get; } = new() { Id = 1020, Name = "Blades of Steel" };
        public static Skill JungleStrike { get; } = new() { Id = 1021, Name = "Jungle Strike" };
        public static Skill WildStrike { get; } = new() { Id = 1022, Name = "Wild Strike" };
        public static Skill LeapingMantisSting { get; } = new() { Id = 1023, Name = "Leaping Mantis Sting" };
        public static Skill BlackMantisThrust { get; } = new() { Id = 1024, Name = "Black Mantis Thrust" };
        public static Skill DisruptingStab { get; } = new() { Id = 1025, Name = "Disrupting Stab" };
        public static Skill GoldenLotusStrike { get; } = new() { Id = 1026, Name = "Golden Lotus Strike" };
        public static Skill FlashingBlades { get; } = new() { Id = 1042, Name = "Flashing Blades" };
        public static Skill GoldenSkullStrike { get; } = new() { Id = 1635, Name = "Golden Skull Strike" };
        public static Skill BlackSpiderStrike { get; } = new() { Id = 1636, Name = "Black Spider Strike" };
        public static Skill GoldenFoxStrike { get; } = new() { Id = 1637, Name = "Golden Fox Strike" };
        public static Skill FoxsPromise { get; } = new() { Id = 1640, Name = "Fox's Promise" };
        public static Skill LotusStrike { get; } = new() { Id = 1987, Name = "Lotus Strike" };
        public static Skill GoldenFangStrike { get; } = new() { Id = 1988, Name = "Golden Fang Strike" };
        public static Skill FallingLotusStrike { get; } = new() { Id = 1990, Name = "Falling Lotus Strike" };
        public static Skill TramplingOx { get; } = new() { Id = 2135, Name = "Trampling Ox" };
        public static Skill MarkofInsecurity { get; } = new() { Id = 570, Name = "Mark of Insecurity" };
        public static Skill DisruptingDagger { get; } = new() { Id = 571, Name = "Disrupting Dagger" };
        public static Skill DeadlyParadox { get; } = new() { Id = 572, Name = "Deadly Paradox" };
        public static Skill EntanglingAsp { get; } = new() { Id = 784, Name = "Entangling Asp" };
        public static Skill MarkofDeath { get; } = new() { Id = 785, Name = "Mark of Death" };
        public static Skill IronPalm { get; } = new() { Id = 786, Name = "Iron Palm" };
        public static Skill EnduringToxin { get; } = new() { Id = 800, Name = "Enduring Toxin" };
        public static Skill ShroudofSilence { get; } = new() { Id = 801, Name = "Shroud of Silence" };
        public static Skill ExposeDefenses { get; } = new() { Id = 802, Name = "Expose Defenses" };
        public static Skill ScorpionWire { get; } = new() { Id = 815, Name = "Scorpion Wire" };
        public static Skill SiphonStrength { get; } = new() { Id = 827, Name = "Siphon Strength" };
        public static Skill DancingDaggers { get; } = new() { Id = 858, Name = "Dancing Daggers" };
        public static Skill SignetofShadows { get; } = new() { Id = 876, Name = "Signet of Shadows" };
        public static Skill ShamefulFear { get; } = new() { Id = 927, Name = "Shameful Fear" };
        public static Skill SiphonSpeed { get; } = new() { Id = 951, Name = "Siphon Speed" };
        public static Skill MantisTouch { get; } = new() { Id = 974, Name = "Mantis Touch" };
        public static Skill WayoftheEmptyPalm { get; } = new() { Id = 987, Name = "Way of the Empty Palm" };
        public static Skill ExpungeEnchantments { get; } = new() { Id = 990, Name = "Expunge Enchantments" };
        public static Skill Impale { get; } = new() { Id = 1033, Name = "Impale" };
        public static Skill AssassinsPromise { get; } = new() { Id = 1035, Name = "Assassin's Promise" };
        public static Skill CripplingDagger { get; } = new() { Id = 1038, Name = "Crippling Dagger" };
        public static Skill DarkPrison { get; } = new() { Id = 1044, Name = "Dark Prison" };
        public static Skill AuguryofDeath { get; } = new() { Id = 1646, Name = "Augury of Death" };
        public static Skill SignetofToxicShock { get; } = new() { Id = 1647, Name = "Signet of Toxic Shock" };
        public static Skill ShadowPrison { get; } = new() { Id = 1652, Name = "Shadow Prison" };
        public static Skill VampiricAssault { get; } = new() { Id = 1986, Name = "Vampiric Assault" };
        public static Skill SadistsSignet { get; } = new() { Id = 1991, Name = "Sadist's Signet" };
        public static Skill ShadowFang { get; } = new() { Id = 2052, Name = "Shadow Fang" };
        public static Skill SignetofDeadlyCorruption { get; } = new() { Id = 2186, Name = "Signet of Deadly Corruption" };
        public static Skill VipersDefense { get; } = new() { Id = 769, Name = "Viper's Defense" };
        public static Skill Return { get; } = new() { Id = 770, Name = "Return" };
        public static Skill BeguilingHaze { get; } = new() { Id = 799, Name = "Beguiling Haze" };
        public static Skill ShadowRefuge { get; } = new() { Id = 814, Name = "Shadow Refuge" };
        public static Skill MirroredStance { get; } = new() { Id = 816, Name = "Mirrored Stance" };
        public static Skill ShadowForm { get; } = new() { Id = 826, Name = "Shadow Form" };
        public static Skill ShadowShroud { get; } = new() { Id = 928, Name = "Shadow Shroud" };
        public static Skill ShadowofHaste { get; } = new() { Id = 929, Name = "Shadow of Haste" };
        public static Skill WayoftheFox { get; } = new() { Id = 949, Name = "Way of the Fox" };
        public static Skill ShadowyBurden { get; } = new() { Id = 950, Name = "Shadowy Burden" };
        public static Skill DeathsCharge { get; } = new() { Id = 952, Name = "Death's Charge" };
        public static Skill BlindingPowder { get; } = new() { Id = 973, Name = "Blinding Powder" };
        public static Skill WayoftheLotus { get; } = new() { Id = 977, Name = "Way of the Lotus" };
        public static Skill Caltrops { get; } = new() { Id = 985, Name = "Caltrops" };
        public static Skill WayofPerfection { get; } = new() { Id = 1028, Name = "Way of Perfection" };
        public static Skill ShroudofDistress { get; } = new() { Id = 1031, Name = "Shroud of Distress" };
        public static Skill HeartofShadow { get; } = new() { Id = 1032, Name = "Heart of Shadow" };
        public static Skill DarkEscape { get; } = new() { Id = 1037, Name = "Dark Escape" };
        public static Skill UnseenFury { get; } = new() { Id = 1041, Name = "Unseen Fury" };
        public static Skill FeignedNeutrality { get; } = new() { Id = 1641, Name = "Feigned Neutrality" };
        public static Skill HiddenCaltrops { get; } = new() { Id = 1642, Name = "Hidden Caltrops" };
        public static Skill DeathsRetreat { get; } = new() { Id = 1651, Name = "Death's Retreat" };
        public static Skill SmokePowderDefense { get; } = new() { Id = 2136, Name = "Smoke Powder Defense" };
        public static Skill AuraofDisplacement { get; } = new() { Id = 771, Name = "Aura of Displacement" };
        public static Skill Recall { get; } = new() { Id = 925, Name = "Recall" };
        public static Skill MarkofInstability { get; } = new() { Id = 978, Name = "Mark of Instability" };
        public static Skill SignetofMalice { get; } = new() { Id = 1036, Name = "Signet of Malice" };
        public static Skill SpiritWalk { get; } = new() { Id = 1040, Name = "Spirit Walk" };
        public static Skill Dash { get; } = new() { Id = 1043, Name = "Dash" };
        public static Skill AssaultEnchantments { get; } = new() { Id = 1643, Name = "Assault Enchantments" };
        public static Skill WastrelsCollapse { get; } = new() { Id = 1644, Name = "Wastrel's Collapse" };
        public static Skill LiftEnchantment { get; } = new() { Id = 1645, Name = "Lift Enchantment" };
        public static Skill SignetofTwilight { get; } = new() { Id = 1648, Name = "Signet of Twilight" };
        public static Skill ShadowWalk { get; } = new() { Id = 1650, Name = "Shadow Walk" };
        public static Skill Swap { get; } = new() { Id = 1653, Name = "Swap" };
        public static Skill ShadowMeld { get; } = new() { Id = 1654, Name = "Shadow Meld" };
        public static Skill GraspingWasKuurong { get; } = new() { Id = 789, Name = "Grasping Was Kuurong" };
        public static Skill SplinterWeapon { get; } = new() { Id = 792, Name = "Splinter Weapon" };
        public static Skill WailingWeapon { get; } = new() { Id = 794, Name = "Wailing Weapon" };
        public static Skill NightmareWeapon { get; } = new() { Id = 795, Name = "Nightmare Weapon" };
        public static Skill SpiritRift { get; } = new() { Id = 910, Name = "Spirit Rift" };
        public static Skill Lamentation { get; } = new() { Id = 916, Name = "Lamentation" };
        public static Skill SpiritBurn { get; } = new() { Id = 919, Name = "Spirit Burn" };
        public static Skill Destruction { get; } = new() { Id = 920, Name = "Destruction" };
        public static Skill ClamorofSouls { get; } = new() { Id = 1215, Name = "Clamor of Souls" };
        public static Skill CruelWasDaoshen { get; } = new() { Id = 1218, Name = "Cruel Was Daoshen" };
        public static Skill ChanneledStrike { get; } = new() { Id = 1225, Name = "Channeled Strike" };
        public static Skill SpiritBoonStrike { get; } = new() { Id = 1226, Name = "Spirit Boon Strike" };
        public static Skill EssenceStrike { get; } = new() { Id = 1227, Name = "Essence Strike" };
        public static Skill SpiritSiphon { get; } = new() { Id = 1228, Name = "Spirit Siphon" };
        public static Skill PainfulBond { get; } = new() { Id = 1237, Name = "Painful Bond" };
        public static Skill SignetofSpirits { get; } = new() { Id = 1239, Name = "Signet of Spirits" };
        public static Skill GazefromBeyond { get; } = new() { Id = 1245, Name = "Gaze from Beyond" };
        public static Skill AncestorsRage { get; } = new() { Id = 1246, Name = "Ancestors' Rage" };
        public static Skill Bloodsong { get; } = new() { Id = 1253, Name = "Bloodsong" };
        public static Skill RenewingSurge { get; } = new() { Id = 1478, Name = "Renewing Surge" };
        public static Skill OfferingofSpirit { get; } = new() { Id = 1479, Name = "Offering of Spirit" };
        public static Skill DestructiveWasGlaive { get; } = new() { Id = 1732, Name = "Destructive Was Glaive" };
        public static Skill WieldersStrike { get; } = new() { Id = 1733, Name = "Wielder's Strike" };
        public static Skill GazeofFury { get; } = new() { Id = 1734, Name = "Gaze of Fury" };
        public static Skill CaretakersCharge { get; } = new() { Id = 1744, Name = "Caretaker's Charge" };
        public static Skill WeaponofFury { get; } = new() { Id = 1749, Name = "Weapon of Fury" };
        public static Skill WarmongersWeapon { get; } = new() { Id = 1751, Name = "Warmonger's Weapon" };
        public static Skill WeaponofAggression { get; } = new() { Id = 2073, Name = "Weapon of Aggression" };
        public static Skill Agony { get; } = new() { Id = 2205, Name = "Agony" };
        public static Skill MightyWasVorizun { get; } = new() { Id = 773, Name = "Mighty Was Vorizun" };
        public static Skill Shadowsong { get; } = new() { Id = 871, Name = "Shadowsong" };
        public static Skill Union { get; } = new() { Id = 911, Name = "Union" };
        public static Skill Dissonance { get; } = new() { Id = 921, Name = "Dissonance" };
        public static Skill Disenchantment { get; } = new() { Id = 923, Name = "Disenchantment" };
        public static Skill Restoration { get; } = new() { Id = 963, Name = "Restoration" };
        public static Skill Shelter { get; } = new() { Id = 982, Name = "Shelter" };
        public static Skill ArmorofUnfeeling { get; } = new() { Id = 1232, Name = "Armor of Unfeeling" };
        public static Skill DulledWeapon { get; } = new() { Id = 1235, Name = "Dulled Weapon" };
        public static Skill BindingChains { get; } = new() { Id = 1236, Name = "Binding Chains" };
        public static Skill Pain { get; } = new() { Id = 1247, Name = "Pain" };
        public static Skill Displacement { get; } = new() { Id = 1249, Name = "Displacement" };
        public static Skill Earthbind { get; } = new() { Id = 1252, Name = "Earthbind" };
        public static Skill Wanderlust { get; } = new() { Id = 1255, Name = "Wanderlust" };
        public static Skill BrutalWeapon { get; } = new() { Id = 1258, Name = "Brutal Weapon" };
        public static Skill GuidedWeapon { get; } = new() { Id = 1259, Name = "Guided Weapon" };
        public static Skill Soothing { get; } = new() { Id = 1266, Name = "Soothing" };
        public static Skill VitalWeapon { get; } = new() { Id = 1267, Name = "Vital Weapon" };
        public static Skill WeaponofQuickening { get; } = new() { Id = 1268, Name = "Weapon of Quickening" };
        public static Skill SignetofGhostlyMight { get; } = new() { Id = 1742, Name = "Signet of Ghostly Might" };
        public static Skill Anguish { get; } = new() { Id = 1745, Name = "Anguish" };
        public static Skill SunderingWeapon { get; } = new() { Id = 2148, Name = "Sundering Weapon" };
        public static Skill GhostlyWeapon { get; } = new() { Id = 2206, Name = "Ghostly Weapon" };
        public static Skill GenerousWasTsungrai { get; } = new() { Id = 772, Name = "Generous Was Tsungrai" };
        public static Skill ResilientWeapon { get; } = new() { Id = 787, Name = "Resilient Weapon" };
        public static Skill BlindWasMingson { get; } = new() { Id = 788, Name = "Blind Was Mingson" };
        public static Skill VengefulWasKhanhei { get; } = new() { Id = 790, Name = "Vengeful Was Khanhei" };
        public static Skill FleshofMyFlesh { get; } = new() { Id = 791, Name = "Flesh of My Flesh" };
        public static Skill WeaponofWarding { get; } = new() { Id = 793, Name = "Weapon of Warding" };
        public static Skill DefiantWasXinrae { get; } = new() { Id = 812, Name = "Defiant Was Xinrae" };
        public static Skill TranquilWasTanasen { get; } = new() { Id = 913, Name = "Tranquil Was Tanasen" };
        public static Skill SpiritLight { get; } = new() { Id = 915, Name = "Spirit Light" };
        public static Skill SpiritTransfer { get; } = new() { Id = 962, Name = "Spirit Transfer" };
        public static Skill VengefulWeapon { get; } = new() { Id = 964, Name = "Vengeful Weapon" };
        public static Skill Recuperation { get; } = new() { Id = 981, Name = "Recuperation" };
        public static Skill WeaponofShadow { get; } = new() { Id = 983, Name = "Weapon of Shadow" };
        public static Skill ProtectiveWasKaolai { get; } = new() { Id = 1219, Name = "Protective Was Kaolai" };
        public static Skill ResilientWasXiko { get; } = new() { Id = 1221, Name = "Resilient Was Xiko" };
        public static Skill LivelyWasNaomei { get; } = new() { Id = 1222, Name = "Lively Was Naomei" };
        public static Skill SoothingMemories { get; } = new() { Id = 1233, Name = "Soothing Memories" };
        public static Skill MendBodyandSoul { get; } = new() { Id = 1234, Name = "Mend Body and Soul" };
        public static Skill Preservation { get; } = new() { Id = 1250, Name = "Preservation" };
        public static Skill Life { get; } = new() { Id = 1251, Name = "Life" };
        public static Skill SpiritLightWeapon { get; } = new() { Id = 1257, Name = "Spirit Light Weapon" };
        public static Skill WieldersBoon { get; } = new() { Id = 1265, Name = "Wielder's Boon" };
        public static Skill DeathPactSignet { get; } = new() { Id = 1481, Name = "Death Pact Signet" };
        public static Skill VocalWasSogolon { get; } = new() { Id = 1731, Name = "Vocal Was Sogolon" };
        public static Skill GhostmirrorLight { get; } = new() { Id = 1741, Name = "Ghostmirror Light" };
        public static Skill Recovery { get; } = new() { Id = 1748, Name = "Recovery" };
        public static Skill XinraesWeapon { get; } = new() { Id = 1750, Name = "Xinrae's Weapon" };
        public static Skill WeaponofRemedy { get; } = new() { Id = 1752, Name = "Weapon of Remedy" };
        public static Skill PureWasLiMing { get; } = new() { Id = 2072, Name = "Pure Was Li Ming" };
        public static Skill MendingGrip { get; } = new() { Id = 2202, Name = "Mending Grip" };
        public static Skill SpiritleechAura { get; } = new() { Id = 2203, Name = "Spiritleech Aura" };
        public static Skill Rejuvenation { get; } = new() { Id = 2204, Name = "Rejuvenation" };
        public static Skill ConsumeSoul { get; } = new() { Id = 914, Name = "Consume Soul" };
        public static Skill RuptureSoul { get; } = new() { Id = 917, Name = "Rupture Soul" };
        public static Skill SpirittoFlesh { get; } = new() { Id = 918, Name = "Spirit to Flesh" };
        public static Skill FeastofSouls { get; } = new() { Id = 980, Name = "Feast of Souls" };
        public static Skill RitualLord { get; } = new() { Id = 1217, Name = "Ritual Lord" };
        public static Skill AttunedWasSongkai { get; } = new() { Id = 1220, Name = "Attuned Was Songkai" };
        public static Skill AnguishedWasLingwah { get; } = new() { Id = 1223, Name = "Anguished Was Lingwah" };
        public static Skill ExplosiveGrowth { get; } = new() { Id = 1229, Name = "Explosive Growth" };
        public static Skill BoonofCreation { get; } = new() { Id = 1230, Name = "Boon of Creation" };
        public static Skill SpiritChanneling { get; } = new() { Id = 1231, Name = "Spirit Channeling" };
        public static Skill SignetofCreation { get; } = new() { Id = 1238, Name = "Signet of Creation" };
        public static Skill SoulTwisting { get; } = new() { Id = 1240, Name = "Soul Twisting" };
        public static Skill GhostlyHaste { get; } = new() { Id = 1244, Name = "Ghostly Haste" };
        public static Skill Doom { get; } = new() { Id = 1264, Name = "Doom" };
        public static Skill SpiritsGift { get; } = new() { Id = 1480, Name = "Spirit's Gift" };
        public static Skill ReclaimEssence { get; } = new() { Id = 1482, Name = "Reclaim Essence" };
        public static Skill SpiritsStrength { get; } = new() { Id = 1736, Name = "Spirit's Strength" };
        public static Skill WieldersZeal { get; } = new() { Id = 1737, Name = "Wielder's Zeal" };
        public static Skill SightBeyondSight { get; } = new() { Id = 1738, Name = "Sight Beyond Sight" };
        public static Skill RenewingMemories { get; } = new() { Id = 1739, Name = "Renewing Memories" };
        public static Skill WieldersRemedy { get; } = new() { Id = 1740, Name = "Wielder's Remedy" };
        public static Skill SignetofBinding { get; } = new() { Id = 1743, Name = "Signet of Binding" };
        public static Skill Empowerment { get; } = new() { Id = 1747, Name = "Empowerment" };
        public static Skill EnergeticWasLeeSa { get; } = new() { Id = 2016, Name = "Energetic Was Lee Sa" };
        public static Skill WeaponofRenewal { get; } = new() { Id = 2149, Name = "Weapon of Renewal" };
        public static Skill DrawSpirit { get; } = new() { Id = 1224, Name = "Draw Spirit" };
        public static Skill CripplingAnthem { get; } = new() { Id = 1554, Name = "Crippling Anthem" };
        public static Skill Godspeed { get; } = new() { Id = 1556, Name = "Godspeed" };
        public static Skill GofortheEyes { get; } = new() { Id = 1558, Name = "Go for the Eyes!" };
        public static Skill AnthemofEnvy { get; } = new() { Id = 1559, Name = "Anthem of Envy" };
        public static Skill AnthemofGuidance { get; } = new() { Id = 1568, Name = "Anthem of Guidance" };
        public static Skill BraceYourself { get; } = new() { Id = 1572, Name = "Brace Yourself!" };
        public static Skill BladeturnRefrain { get; } = new() { Id = 1580, Name = "Bladeturn Refrain" };
        public static Skill StandYourGround { get; } = new() { Id = 1589, Name = "Stand Your Ground!" };
        public static Skill MakeHaste { get; } = new() { Id = 1591, Name = "Make Haste!" };
        public static Skill WeShallReturn { get; } = new() { Id = 1592, Name = "We Shall Return!" };
        public static Skill NeverGiveUp { get; } = new() { Id = 1593, Name = "Never Give Up!" };
        public static Skill HelpMe { get; } = new() { Id = 1594, Name = "Help Me!" };
        public static Skill FallBack { get; } = new() { Id = 1595, Name = "Fall Back!" };
        public static Skill Incoming { get; } = new() { Id = 1596, Name = "Incoming!" };
        public static Skill NeverSurrender { get; } = new() { Id = 1598, Name = "Never Surrender!" };
        public static Skill CantTouchThis { get; } = new() { Id = 1780, Name = "Can't Touch This!" };
        public static Skill FindTheirWeakness { get; } = new() { Id = 1781, Name = "Find Their Weakness!" };
        public static Skill AnthemofWeariness { get; } = new() { Id = 2017, Name = "Anthem of Weariness" };
        public static Skill AnthemofDisruption { get; } = new() { Id = 2018, Name = "Anthem of Disruption" };
        public static Skill AnthemofFury { get; } = new() { Id = 1553, Name = "Anthem of Fury" };
        public static Skill DefensiveAnthem { get; } = new() { Id = 1555, Name = "Defensive Anthem" };
        public static Skill AnthemofFlame { get; } = new() { Id = 1557, Name = "Anthem of Flame" };
        public static Skill Awe { get; } = new() { Id = 1573, Name = "Awe" };
        public static Skill EnduringHarmony { get; } = new() { Id = 1574, Name = "Enduring Harmony" };
        public static Skill BlazingFinale { get; } = new() { Id = 1575, Name = "Blazing Finale" };
        public static Skill BurningRefrain { get; } = new() { Id = 1576, Name = "Burning Refrain" };
        public static Skill GlowingSignet { get; } = new() { Id = 1581, Name = "Glowing Signet" };
        public static Skill LeadersComfort { get; } = new() { Id = 1584, Name = "Leader's Comfort" };
        public static Skill AngelicProtection { get; } = new() { Id = 1586, Name = "Angelic Protection" };
        public static Skill AngelicBond { get; } = new() { Id = 1587, Name = "Angelic Bond" };
        public static Skill LeadtheWay { get; } = new() { Id = 1590, Name = "Lead the Way!" };
        public static Skill TheyreonFire { get; } = new() { Id = 1597, Name = "They're on Fire!" };
        public static Skill FocusedAnger { get; } = new() { Id = 1769, Name = "Focused Anger" };
        public static Skill NaturalTemper { get; } = new() { Id = 1770, Name = "Natural Temper" };
        public static Skill SoldiersFury { get; } = new() { Id = 1773, Name = "Soldier's Fury" };
        public static Skill AggressiveRefrain { get; } = new() { Id = 1774, Name = "Aggressive Refrain" };
        public static Skill SignetofReturn { get; } = new() { Id = 1778, Name = "Signet of Return" };
        public static Skill MakeYourTime { get; } = new() { Id = 1779, Name = "Make Your Time!" };
        public static Skill HastyRefrain { get; } = new() { Id = 2075, Name = "Hasty Refrain" };
        public static Skill BurningShield { get; } = new() { Id = 2208, Name = "Burning Shield" };
        public static Skill SpearSwipe { get; } = new() { Id = 2210, Name = "Spear Swipe" };
        public static Skill SongofPower { get; } = new() { Id = 1560, Name = "Song of Power" };
        public static Skill ZealousAnthem { get; } = new() { Id = 1561, Name = "Zealous Anthem" };
        public static Skill AriaofZeal { get; } = new() { Id = 1562, Name = "Aria of Zeal" };
        public static Skill LyricofZeal { get; } = new() { Id = 1563, Name = "Lyric of Zeal" };
        public static Skill BalladofRestoration { get; } = new() { Id = 1564, Name = "Ballad of Restoration" };
        public static Skill ChorusofRestoration { get; } = new() { Id = 1565, Name = "Chorus of Restoration" };
        public static Skill AriaofRestoration { get; } = new() { Id = 1566, Name = "Aria of Restoration" };
        public static Skill EnergizingChorus { get; } = new() { Id = 1569, Name = "Energizing Chorus" };
        public static Skill SongofPurification { get; } = new() { Id = 1570, Name = "Song of Purification" };
        public static Skill FinaleofRestoration { get; } = new() { Id = 1577, Name = "Finale of Restoration" };
        public static Skill MendingRefrain { get; } = new() { Id = 1578, Name = "Mending Refrain" };
        public static Skill PurifyingFinale { get; } = new() { Id = 1579, Name = "Purifying Finale" };
        public static Skill LeadersZeal { get; } = new() { Id = 1583, Name = "Leader's Zeal" };
        public static Skill SignetofSynergy { get; } = new() { Id = 1585, Name = "Signet of Synergy" };
        public static Skill ItsJustaFleshWound { get; } = new() { Id = 1599, Name = "It's Just a Flesh Wound." };
        public static Skill SongofRestoration { get; } = new() { Id = 1771, Name = "Song of Restoration" };
        public static Skill LyricofPurification { get; } = new() { Id = 1772, Name = "Lyric of Purification" };
        public static Skill EnergizingFinale { get; } = new() { Id = 1775, Name = "Energizing Finale" };
        public static Skill ThePowerIsYours { get; } = new() { Id = 1782, Name = "The Power Is Yours!" };
        public static Skill InspirationalSpeech { get; } = new() { Id = 2207, Name = "Inspirational Speech" };
        public static Skill BlazingSpear { get; } = new() { Id = 1546, Name = "Blazing Spear" };
        public static Skill MightyThrow { get; } = new() { Id = 1547, Name = "Mighty Throw" };
        public static Skill CruelSpear { get; } = new() { Id = 1548, Name = "Cruel Spear" };
        public static Skill HarriersToss { get; } = new() { Id = 1549, Name = "Harrier's Toss" };
        public static Skill UnblockableThrow { get; } = new() { Id = 1550, Name = "Unblockable Throw" };
        public static Skill SpearofLightning { get; } = new() { Id = 1551, Name = "Spear of Lightning" };
        public static Skill WearyingSpear { get; } = new() { Id = 1552, Name = "Wearying Spear" };
        public static Skill BarbedSpear { get; } = new() { Id = 1600, Name = "Barbed Spear" };
        public static Skill ViciousAttack { get; } = new() { Id = 1601, Name = "Vicious Attack" };
        public static Skill StunningStrike { get; } = new() { Id = 1602, Name = "Stunning Strike" };
        public static Skill MercilessSpear { get; } = new() { Id = 1603, Name = "Merciless Spear" };
        public static Skill DisruptingThrow { get; } = new() { Id = 1604, Name = "Disrupting Throw" };
        public static Skill WildThrow { get; } = new() { Id = 1605, Name = "Wild Throw" };
        public static Skill SlayersSpear { get; } = new() { Id = 1783, Name = "Slayer's Spear" };
        public static Skill SwiftJavelin { get; } = new() { Id = 1784, Name = "Swift Javelin" };
        public static Skill ChestThumper { get; } = new() { Id = 2074, Name = "Chest Thumper" };
        public static Skill MaimingSpear { get; } = new() { Id = 2150, Name = "Maiming Spear" };
        public static Skill HolySpear { get; } = new() { Id = 2209, Name = "Holy Spear" };
        public static Skill SpearofRedemption { get; } = new() { Id = 2238, Name = "Spear of Redemption" };
        public static Skill SongofConcentration { get; } = new() { Id = 1567, Name = "Song of Concentration" };
        public static Skill HexbreakerAria { get; } = new() { Id = 1571, Name = "Hexbreaker Aria" };
        public static Skill CauterySignet { get; } = new() { Id = 1588, Name = "Cautery Signet" };
        public static Skill SignetofAggression { get; } = new() { Id = 1776, Name = "Signet of Aggression" };
        public static Skill RemedySignet { get; } = new() { Id = 1777, Name = "Remedy Signet" };
        public static Skill AuraofThorns { get; } = new() { Id = 1495, Name = "Aura of Thorns" };
        public static Skill DustCloak { get; } = new() { Id = 1497, Name = "Dust Cloak" };
        public static Skill StaggeringForce { get; } = new() { Id = 1498, Name = "Staggering Force" };
        public static Skill MirageCloak { get; } = new() { Id = 1500, Name = "Mirage Cloak" };
        public static Skill VitalBoon { get; } = new() { Id = 1506, Name = "Vital Boon" };
        public static Skill SandShards { get; } = new() { Id = 1510, Name = "Sand Shards" };
        public static Skill FleetingStability { get; } = new() { Id = 1514, Name = "Fleeting Stability" };
        public static Skill ArmorofSanctity { get; } = new() { Id = 1515, Name = "Armor of Sanctity" };
        public static Skill MysticRegeneration { get; } = new() { Id = 1516, Name = "Mystic Regeneration" };
        public static Skill SignetofPiousLight { get; } = new() { Id = 1530, Name = "Signet of Pious Light" };
        public static Skill MysticSandstorm { get; } = new() { Id = 1532, Name = "Mystic Sandstorm" };
        public static Skill Conviction { get; } = new() { Id = 1540, Name = "Conviction" };
        public static Skill PiousConcentration { get; } = new() { Id = 1542, Name = "Pious Concentration" };
        public static Skill VeilofThorns { get; } = new() { Id = 1757, Name = "Veil of Thorns" };
        public static Skill VowofStrength { get; } = new() { Id = 1759, Name = "Vow of Strength" };
        public static Skill EbonDustAura { get; } = new() { Id = 1760, Name = "Ebon Dust Aura" };
        public static Skill ShieldofForce { get; } = new() { Id = 2201, Name = "Shield of Force" };
        public static Skill BanishingStrike { get; } = new() { Id = 1483, Name = "Banishing Strike" };
        public static Skill MysticSweep { get; } = new() { Id = 1484, Name = "Mystic Sweep" };
        public static Skill BalthazarsRage { get; } = new() { Id = 1496, Name = "Balthazar's Rage" };
        public static Skill PiousRenewal { get; } = new() { Id = 1499, Name = "Pious Renewal" };
        public static Skill ArcaneZeal { get; } = new() { Id = 1502, Name = "Arcane Zeal" };
        public static Skill MysticVigor { get; } = new() { Id = 1503, Name = "Mystic Vigor" };
        public static Skill WatchfulIntervention { get; } = new() { Id = 1504, Name = "Watchful Intervention" };
        public static Skill HeartofHolyFlame { get; } = new() { Id = 1507, Name = "Heart of Holy Flame" };
        public static Skill ExtendEnchantments { get; } = new() { Id = 1508, Name = "Extend Enchantments" };
        public static Skill FaithfulIntervention { get; } = new() { Id = 1509, Name = "Faithful Intervention" };
        public static Skill VowofSilence { get; } = new() { Id = 1517, Name = "Vow of Silence" };
        public static Skill AvatarofBalthazar { get; } = new() { Id = 1518, Name = "Avatar of Balthazar" };
        public static Skill AvatarofDwayna { get; } = new() { Id = 1519, Name = "Avatar of Dwayna" };
        public static Skill AvatarofGrenth { get; } = new() { Id = 1520, Name = "Avatar of Grenth" };
        public static Skill AvatarofLyssa { get; } = new() { Id = 1521, Name = "Avatar of Lyssa" };
        public static Skill AvatarofMelandru { get; } = new() { Id = 1522, Name = "Avatar of Melandru" };
        public static Skill Meditation { get; } = new() { Id = 1523, Name = "Meditation" };
        public static Skill EremitesZeal { get; } = new() { Id = 1524, Name = "Eremite's Zeal" };
        public static Skill ImbueHealth { get; } = new() { Id = 1526, Name = "Imbue Health" };
        public static Skill IntimidatingAura { get; } = new() { Id = 1531, Name = "Intimidating Aura" };
        public static Skill RendingTouch { get; } = new() { Id = 1534, Name = "Rending Touch" };
        public static Skill PiousHaste { get; } = new() { Id = 1543, Name = "Pious Haste" };
        public static Skill MysticCorruption { get; } = new() { Id = 1755, Name = "Mystic Corruption" };
        public static Skill HeartofFury { get; } = new() { Id = 1762, Name = "Heart of Fury" };
        public static Skill ZealousRenewal { get; } = new() { Id = 1763, Name = "Zealous Renewal" };
        public static Skill AuraSlicer { get; } = new() { Id = 2070, Name = "Aura Slicer" };
        public static Skill PiousFury { get; } = new() { Id = 2146, Name = "Pious Fury" };
        public static Skill EremitesAttack { get; } = new() { Id = 1485, Name = "Eremite's Attack" };
        public static Skill ReapImpurities { get; } = new() { Id = 1486, Name = "Reap Impurities" };
        public static Skill TwinMoonSweep { get; } = new() { Id = 1487, Name = "Twin Moon Sweep" };
        public static Skill VictoriousSweep { get; } = new() { Id = 1488, Name = "Victorious Sweep" };
        public static Skill IrresistibleSweep { get; } = new() { Id = 1489, Name = "Irresistible Sweep" };
        public static Skill PiousAssault { get; } = new() { Id = 1490, Name = "Pious Assault" };
        public static Skill CripplingSweep { get; } = new() { Id = 1535, Name = "Crippling Sweep" };
        public static Skill WoundingStrike { get; } = new() { Id = 1536, Name = "Wounding Strike" };
        public static Skill WearyingStrike { get; } = new() { Id = 1537, Name = "Wearying Strike" };
        public static Skill LyssasAssault { get; } = new() { Id = 1538, Name = "Lyssa's Assault" };
        public static Skill ChillingVictory { get; } = new() { Id = 1539, Name = "Chilling Victory" };
        public static Skill RendingSweep { get; } = new() { Id = 1753, Name = "Rending Sweep" };
        public static Skill ReapersSweep { get; } = new() { Id = 1767, Name = "Reaper's Sweep" };
        public static Skill RadiantScythe { get; } = new() { Id = 2012, Name = "Radiant Scythe" };
        public static Skill FarmersScythe { get; } = new() { Id = 2015, Name = "Farmer's Scythe" };
        public static Skill ZealousSweep { get; } = new() { Id = 2071, Name = "Zealous Sweep" };
        public static Skill CripplingVictory { get; } = new() { Id = 2147, Name = "Crippling Victory" };
        public static Skill MysticTwister { get; } = new() { Id = 1491, Name = "Mystic Twister" };
        public static Skill GrenthsFingers { get; } = new() { Id = 1493, Name = "Grenth's Fingers" };
        public static Skill VowofPiety { get; } = new() { Id = 1505, Name = "Vow of Piety" };
        public static Skill LyssasHaste { get; } = new() { Id = 1512, Name = "Lyssa's Haste" };
        public static Skill GuidingHands { get; } = new() { Id = 1513, Name = "Guiding Hands" };
        public static Skill NaturalHealing { get; } = new() { Id = 1525, Name = "Natural Healing" };
        public static Skill MysticHealing { get; } = new() { Id = 1527, Name = "Mystic Healing" };
        public static Skill DwaynasTouch { get; } = new() { Id = 1528, Name = "Dwayna's Touch" };
        public static Skill PiousRestoration { get; } = new() { Id = 1529, Name = "Pious Restoration" };
        public static Skill WindsofDisenchantment { get; } = new() { Id = 1533, Name = "Winds of Disenchantment" };
        public static Skill WhirlingCharge { get; } = new() { Id = 1544, Name = "Whirling Charge" };
        public static Skill TestofFaith { get; } = new() { Id = 1545, Name = "Test of Faith" };
        public static Skill Onslaught { get; } = new() { Id = 1754, Name = "Onslaught" };
        public static Skill GrenthsGrasp { get; } = new() { Id = 1756, Name = "Grenth's Grasp" };
        public static Skill HarriersGrasp { get; } = new() { Id = 1758, Name = "Harrier's Grasp" };
        public static Skill ZealousVow { get; } = new() { Id = 1761, Name = "Zealous Vow" };
        public static Skill AttackersInsight { get; } = new() { Id = 1764, Name = "Attacker's Insight" };
        public static Skill RendingAura { get; } = new() { Id = 1765, Name = "Rending Aura" };
        public static Skill FeatherfootGrace { get; } = new() { Id = 1766, Name = "Featherfoot Grace" };
        public static Skill HarriersHaste { get; } = new() { Id = 1768, Name = "Harrier's Haste" };
        public static Skill GrenthsAura { get; } = new() { Id = 2013, Name = "Grenth's Aura" };
        public static Skill SignetofPiousRestraint { get; } = new() { Id = 2014, Name = "Signet of Pious Restraint" };
        public static Skill SignetofMysticSpeed { get; } = new() { Id = 2200, Name = "Signet of Mystic Speed" };
        public static Skill EnchantedHaste { get; } = new() { Id = 1541, Name = "Enchanted Haste" };
        public static Skill EnragedSmashPvP { get; } = new() { Id = 2808, Name = "Enraged Smash (PvP)" };
        public static Skill RenewingSmashPvP { get; } = new() { Id = 3143, Name = "Renewing Smash (PvP)" };
        public static Skill WarriorsEndurancePvP { get; } = new() { Id = 3002, Name = "Warrior's Endurance (PvP)" };
        public static Skill DefyPainPvP { get; } = new() { Id = 3204, Name = "Defy Pain (PvP)" };
        public static Skill WatchYourselfPvP { get; } = new() { Id = 2858, Name = "Watch Yourself! (PvP)" };
        public static Skill SoldiersStancePvP { get; } = new() { Id = 3156, Name = "Soldier's Stance (PvP)" };
        public static Skill ForGreatJusticePvP { get; } = new() { Id = 2883, Name = "For Great Justice! (PvP)" };
        public static Skill CallofHastePvP { get; } = new() { Id = 2657, Name = "Call of Haste (PvP)" };
        public static Skill ComfortAnimalPvP { get; } = new() { Id = 3045, Name = "Comfort Animal (PvP)" };
        public static Skill MelandrusAssaultPvP { get; } = new() { Id = 3047, Name = "Melandru's Assault (PvP)" };
        public static Skill PredatoryBondPvP { get; } = new() { Id = 3050, Name = "Predatory Bond (PvP)" };
        public static Skill EnragedLungePvP { get; } = new() { Id = 3051, Name = "Enraged Lunge (PvP)" };
        public static Skill CharmAnimalCodex { get; } = new() { Id = 3068, Name = "Charm Animal (Codex)" };
        public static Skill HealasOnePvP { get; } = new() { Id = 3144, Name = "Heal as One (PvP)" };
        public static Skill ExpertsDexterityPvP { get; } = new() { Id = 2959, Name = "Expert's Dexterity (PvP)" };
        public static Skill EscapePvP { get; } = new() { Id = 3060, Name = "Escape (PvP)" };
        public static Skill LightningReflexesPvP { get; } = new() { Id = 3141, Name = "Lightning Reflexes (PvP)" };
        public static Skill GlassArrowsPvP { get; } = new() { Id = 3145, Name = "Glass Arrows (PvP)" };
        public static Skill PenetratingAttackPvP { get; } = new() { Id = 2861, Name = "Penetrating Attack (PvP)" };
        public static Skill SunderingAttackPvP { get; } = new() { Id = 2864, Name = "Sundering Attack (PvP)" };
        public static Skill SlothHuntersShotPvP { get; } = new() { Id = 2925, Name = "Sloth Hunter's Shot (PvP)" };
        public static Skill ReadtheWindPvP { get; } = new() { Id = 2969, Name = "Read the Wind (PvP)" };
        public static Skill KeenArrowPvP { get; } = new() { Id = 3147, Name = "Keen Arrow (PvP)" };
        public static Skill UnyieldingAuraPvP { get; } = new() { Id = 2891, Name = "Unyielding Aura (PvP)" };
        public static Skill SmitersBoonPvP { get; } = new() { Id = 2895, Name = "Smiter's Boon (PvP)" };
        public static Skill LightofDeliverancePvP { get; } = new() { Id = 2871, Name = "Light of Deliverance (PvP)" };
        public static Skill HealPartyPvP { get; } = new() { Id = 3232, Name = "Heal Party (PvP)" };
        public static Skill AegisPvP { get; } = new() { Id = 2857, Name = "Aegis (PvP)" };
        public static Skill SpiritBondPvP { get; } = new() { Id = 2892, Name = "Spirit Bond (PvP)" };
        public static Skill SignetofJudgmentPvP { get; } = new() { Id = 2887, Name = "Signet of Judgment (PvP)" };
        public static Skill StrengthofHonorPvP { get; } = new() { Id = 2999, Name = "Strength of Honor (PvP)" };
        public static Skill UnholyFeastPvP { get; } = new() { Id = 3058, Name = "Unholy Feast (PvP)" };
        public static Skill SignetofAgonyPvP { get; } = new() { Id = 3059, Name = "Signet of Agony (PvP)" };
        public static Skill SpoilVictorPvP { get; } = new() { Id = 3233, Name = "Spoil Victor (PvP)" };
        public static Skill EnfeeblePvP { get; } = new() { Id = 2859, Name = "Enfeeble (PvP)" };
        public static Skill EnfeeblingBloodPvP { get; } = new() { Id = 2885, Name = "Enfeebling Blood (PvP)" };
        public static Skill DiscordPvP { get; } = new() { Id = 2863, Name = "Discord (PvP)" };
        public static Skill MasochismPvP { get; } = new() { Id = 3054, Name = "Masochism (PvP)" };
        public static Skill MindWrackPvP { get; } = new() { Id = 2734, Name = "Mind Wrack (PvP)" };
        public static Skill EmpathyPvP { get; } = new() { Id = 3151, Name = "Empathy (PvP)" };
        public static Skill ShatterDelusionsPvP { get; } = new() { Id = 3180, Name = "Shatter Delusions (PvP)" };
        public static Skill UnnaturalSignetPvP { get; } = new() { Id = 3188, Name = "Unnatural Signet (PvP)" };
        public static Skill SpiritualPainPvP { get; } = new() { Id = 3189, Name = "Spiritual Pain (PvP)" };
        public static Skill MistrustPvP { get; } = new() { Id = 3191, Name = "Mistrust (PvP)" };
        public static Skill EnchantersConundrumPvP { get; } = new() { Id = 3192, Name = "Enchanter's Conundrum (PvP)" };
        public static Skill VisionsofRegretPvP { get; } = new() { Id = 3234, Name = "Visions of Regret (PvP)" };
        public static Skill PsychicInstabilityPvP { get; } = new() { Id = 3185, Name = "Psychic Instability (PvP)" };
        public static Skill StolenSpeedPvP { get; } = new() { Id = 3187, Name = "Stolen Speed (PvP)" };
        public static Skill FragilityPvP { get; } = new() { Id = 2998, Name = "Fragility (PvP)" };
        public static Skill CripplingAnguishPvP { get; } = new() { Id = 3152, Name = "Crippling Anguish (PvP)" };
        public static Skill IllusionaryWeaponryPvP { get; } = new() { Id = 3181, Name = "Illusionary Weaponry (PvP)" };
        public static Skill MigrainePvP { get; } = new() { Id = 3183, Name = "Migraine (PvP)" };
        public static Skill AccumulatedPainPvP { get; } = new() { Id = 3184, Name = "Accumulated Pain (PvP)" };
        public static Skill SharedBurdenPvP { get; } = new() { Id = 3186, Name = "Shared Burden (PvP)" };
        public static Skill FrustrationPvP { get; } = new() { Id = 3190, Name = "Frustration (PvP)" };
        public static Skill SignetofClumsinessPvP { get; } = new() { Id = 3193, Name = "Signet of Clumsiness (PvP)" };
        public static Skill WanderingEyePvP { get; } = new() { Id = 3195, Name = "Wandering Eye (PvP)" };
        public static Skill CalculatedRiskPvP { get; } = new() { Id = 3196, Name = "Calculated Risk (PvP)" };
        public static Skill FeveredDreamsPvP { get; } = new() { Id = 3289, Name = "Fevered Dreams (PvP)" };
        public static Skill IllusionofHastePvP { get; } = new() { Id = 3373, Name = "Illusion of Haste (PvP)" };
        public static Skill IllusionofPainPvP { get; } = new() { Id = 3374, Name = "Illusion of Pain (PvP)" };
        public static Skill MantraofResolvePvP { get; } = new() { Id = 3063, Name = "Mantra of Resolve (PvP)" };
        public static Skill MantraofSignetsPvP { get; } = new() { Id = 3179, Name = "Mantra of Signets (PvP)" };
        public static Skill MirrorofDisenchantmentPvP { get; } = new() { Id = 3194, Name = "Mirror of Disenchantment (PvP)" };
        public static Skill WebofDisruptionPvP { get; } = new() { Id = 3386, Name = "Web of Disruption (PvP)" };
        public static Skill MindShockPvP { get; } = new() { Id = 2804, Name = "Mind Shock (PvP)" };
        public static Skill RidetheLightningPvP { get; } = new() { Id = 2807, Name = "Ride the Lightning (PvP)" };
        public static Skill LightningHammerPvP { get; } = new() { Id = 3396, Name = "Lightning Hammer (PvP)" };
        public static Skill ObsidianFlamePvP { get; } = new() { Id = 2809, Name = "Obsidian Flame (PvP)" };
        public static Skill EtherRenewalPvP { get; } = new() { Id = 2860, Name = "Ether Renewal (PvP)" };
        public static Skill AuraofRestorationPvP { get; } = new() { Id = 3375, Name = "Aura of Restoration (PvP)" };
        public static Skill SavannahHeatPvP { get; } = new() { Id = 3021, Name = "Savannah Heat (PvP)" };
        public static Skill ElementalFlamePvP { get; } = new() { Id = 3397, Name = "Elemental Flame (PvP)" };
        public static Skill MindFreezePvP { get; } = new() { Id = 2803, Name = "Mind Freeze (PvP)" };
        public static Skill MistFormPvP { get; } = new() { Id = 2805, Name = "Mist Form (PvP)" };
        public static Skill WardAgainstHarmPvP { get; } = new() { Id = 2806, Name = "Ward Against Harm (PvP)" };
        public static Skill SlipperyGroundPvP { get; } = new() { Id = 3398, Name = "Slippery Ground (PvP)" };
        public static Skill AssassinsRemedyPvP { get; } = new() { Id = 2869, Name = "Assassin's Remedy (PvP)" };
        public static Skill DeathBlossomPvP { get; } = new() { Id = 3061, Name = "Death Blossom (PvP)" };
        public static Skill FoxFangsPvP { get; } = new() { Id = 3251, Name = "Fox Fangs (PvP)" };
        public static Skill WildStrikePvP { get; } = new() { Id = 3252, Name = "Wild Strike (PvP)" };
        public static Skill SignetofDeadlyCorruptionPvP { get; } = new() { Id = 3053, Name = "Signet of Deadly Corruption (PvP)" };
        public static Skill ShadowFormPvP { get; } = new() { Id = 2862, Name = "Shadow Form (PvP)" };
        public static Skill ShroudofDistressPvP { get; } = new() { Id = 3048, Name = "Shroud of Distress (PvP)" };
        public static Skill UnseenFuryPvP { get; } = new() { Id = 3049, Name = "Unseen Fury (PvP)" };
        public static Skill AncestorsRagePvP { get; } = new() { Id = 2867, Name = "Ancestors' Rage (PvP)" };
        public static Skill SplinterWeaponPvP { get; } = new() { Id = 2868, Name = "Splinter Weapon (PvP)" };
        public static Skill SignetofSpiritsPvP { get; } = new() { Id = 2965, Name = "Signet of Spirits (PvP)" };
        public static Skill DestructionPvP { get; } = new() { Id = 3008, Name = "Destruction (PvP)" };
        public static Skill BloodsongPvP { get; } = new() { Id = 3019, Name = "Bloodsong (PvP)" };
        public static Skill GazeofFuryPvP { get; } = new() { Id = 3022, Name = "Gaze of Fury (PvP)" };
        public static Skill AgonyPvP { get; } = new() { Id = 3038, Name = "Agony (PvP)" };
        public static Skill DestructiveWasGlaivePvP { get; } = new() { Id = 3157, Name = "Destructive Was Glaive (PvP)" };
        public static Skill SignetofGhostlyMightPvP { get; } = new() { Id = 2966, Name = "Signet of Ghostly Might (PvP)" };
        public static Skill ArmorofUnfeelingPvP { get; } = new() { Id = 3003, Name = "Armor of Unfeeling (PvP)" };
        public static Skill UnionPvP { get; } = new() { Id = 3005, Name = "Union (PvP)" };
        public static Skill ShadowsongPvP { get; } = new() { Id = 3006, Name = "Shadowsong (PvP)" };
        public static Skill PainPvP { get; } = new() { Id = 3007, Name = "Pain (PvP)" };
        public static Skill SoothingPvP { get; } = new() { Id = 3009, Name = "Soothing (PvP)" };
        public static Skill DisplacementPvP { get; } = new() { Id = 3010, Name = "Displacement (PvP)" };
        public static Skill DissonancePvP { get; } = new() { Id = 3014, Name = "Dissonance (PvP)" };
        public static Skill EarthbindPvP { get; } = new() { Id = 3015, Name = "Earthbind (PvP)" };
        public static Skill ShelterPvP { get; } = new() { Id = 3016, Name = "Shelter (PvP)" };
        public static Skill DisenchantmentPvP { get; } = new() { Id = 3017, Name = "Disenchantment (PvP)" };
        public static Skill RestorationPvP { get; } = new() { Id = 3018, Name = "Restoration (PvP)" };
        public static Skill WanderlustPvP { get; } = new() { Id = 3020, Name = "Wanderlust (PvP)" };
        public static Skill AnguishPvP { get; } = new() { Id = 3023, Name = "Anguish (PvP)" };
        public static Skill FleshofMyFleshPvP { get; } = new() { Id = 2866, Name = "Flesh of My Flesh (PvP)" };
        public static Skill DeathPactSignetPvP { get; } = new() { Id = 2872, Name = "Death Pact Signet (PvP)" };
        public static Skill WeaponofWardingPvP { get; } = new() { Id = 2893, Name = "Weapon of Warding (PvP)" };
        public static Skill PreservationPvP { get; } = new() { Id = 3011, Name = "Preservation (PvP)" };
        public static Skill LifePvP { get; } = new() { Id = 3012, Name = "Life (PvP)" };
        public static Skill RecuperationPvP { get; } = new() { Id = 3013, Name = "Recuperation (PvP)" };
        public static Skill RecoveryPvP { get; } = new() { Id = 3025, Name = "Recovery (PvP)" };
        public static Skill RejuvenationPvP { get; } = new() { Id = 3039, Name = "Rejuvenation (PvP)" };
        public static Skill EmpowermentPvP { get; } = new() { Id = 3024, Name = "Empowerment (PvP)" };
        public static Skill IncomingPvP { get; } = new() { Id = 2879, Name = "Incoming! (PvP)" };
        public static Skill NeverSurrenderPvP { get; } = new() { Id = 2880, Name = "Never Surrender! (PvP)" };
        public static Skill GofortheEyesPvP { get; } = new() { Id = 3026, Name = "Go for the Eyes! (PvP)" };
        public static Skill BraceYourselfPvP { get; } = new() { Id = 3027, Name = "Brace Yourself! (PvP)" };
        public static Skill BladeturnRefrainPvP { get; } = new() { Id = 3029, Name = "Bladeturn Refrain (PvP)" };
        public static Skill CantTouchThisPvP { get; } = new() { Id = 3031, Name = "Can't Touch This! (PvP)" };
        public static Skill StandYourGroundPvP { get; } = new() { Id = 3032, Name = "Stand Your Ground! (PvP)" };
        public static Skill WeShallReturnPvP { get; } = new() { Id = 3033, Name = "We Shall Return! (PvP)" };
        public static Skill FindTheirWeaknessPvP { get; } = new() { Id = 3034, Name = "Find Their Weakness! (PvP)" };
        public static Skill NeverGiveUpPvP { get; } = new() { Id = 3035, Name = "Never Give Up! (PvP)" };
        public static Skill HelpMePvP { get; } = new() { Id = 3036, Name = "Help Me! (PvP)" };
        public static Skill FallBackPvP { get; } = new() { Id = 3037, Name = "Fall Back! (PvP)" };
        public static Skill AnthemofDisruptionPvP { get; } = new() { Id = 3040, Name = "Anthem of Disruption (PvP)" };
        public static Skill AnthemofEnvyPvP { get; } = new() { Id = 3148, Name = "Anthem of Envy (PvP)" };
        public static Skill DefensiveAnthemPvP { get; } = new() { Id = 2876, Name = "Defensive Anthem (PvP)" };
        public static Skill BlazingFinalePvP { get; } = new() { Id = 3028, Name = "Blazing Finale (PvP)" };
        public static Skill SignetofReturnPvP { get; } = new() { Id = 3030, Name = "Signet of Return (PvP)" };
        public static Skill BalladofRestorationPvP { get; } = new() { Id = 2877, Name = "Ballad of Restoration (PvP)" };
        public static Skill SongofRestorationPvP { get; } = new() { Id = 2878, Name = "Song of Restoration (PvP)" };
        public static Skill FinaleofRestorationPvP { get; } = new() { Id = 3062, Name = "Finale of Restoration (PvP)" };
        public static Skill MendingRefrainPvP { get; } = new() { Id = 3149, Name = "Mending Refrain (PvP)" };
        public static Skill HarriersTossPvP { get; } = new() { Id = 2875, Name = "Harrier's Toss (PvP)" };
        public static Skill MysticRegenerationPvP { get; } = new() { Id = 2884, Name = "Mystic Regeneration (PvP)" };
        public static Skill AuraofThornsPvP { get; } = new() { Id = 3346, Name = "Aura of Thorns (PvP)" };
        public static Skill DustCloakPvP { get; } = new() { Id = 3347, Name = "Dust Cloak (PvP)" };
        public static Skill BanishingStrikePvP { get; } = new() { Id = 3263, Name = "Banishing Strike (PvP)" };
        public static Skill AvatarofDwaynaPvP { get; } = new() { Id = 3270, Name = "Avatar of Dwayna (PvP)" };
        public static Skill AvatarofMelandruPvP { get; } = new() { Id = 3271, Name = "Avatar of Melandru (PvP)" };
        public static Skill HeartofFuryPvP { get; } = new() { Id = 3366, Name = "Heart of Fury (PvP)" };
        public static Skill PiousFuryPvP { get; } = new() { Id = 3368, Name = "Pious Fury (PvP)" };
        public static Skill TwinMoonSweepPvP { get; } = new() { Id = 3264, Name = "Twin Moon Sweep (PvP)" };
        public static Skill IrresistibleSweepPvP { get; } = new() { Id = 3265, Name = "Irresistible Sweep (PvP)" };
        public static Skill PiousAssaultPvP { get; } = new() { Id = 3266, Name = "Pious Assault (PvP)" };
        public static Skill WoundingStrikePvP { get; } = new() { Id = 3367, Name = "Wounding Strike (PvP)" };
        public static Skill GuidingHandsPvP { get; } = new() { Id = 3269, Name = "Guiding Hands (PvP)" };
        public static Skill MysticHealingPvP { get; } = new() { Id = 3272, Name = "Mystic Healing (PvP)" };
        public static Skill SignetofPiousRestraintPvP { get; } = new() { Id = 3273, Name = "Signet of Pious Restraint (PvP)" };
        public static Skill LyssasHastePvP { get; } = new() { Id = 3348, Name = "Lyssa's Haste (PvP)" };
        public static Skill OnslaughtPvP { get; } = new() { Id = 3365, Name = "Onslaught (PvP)" };
        public static Skill SunspearRebirthSignet { get; } = new() { Id = 1816, Name = "Sunspear Rebirth Signet" };
        public static Skill WhirlwindAttack { get; } = new() { Id = 2107, Name = "Whirlwind Attack" };
        public static Skill NeverRampageAlone { get; } = new() { Id = 2108, Name = "Never Rampage Alone" };
        public static Skill SeedofLife { get; } = new() { Id = 2105, Name = "Seed of Life" };
        public static Skill Necrosis { get; } = new() { Id = 2103, Name = "Necrosis" };
        public static Skill CryofPain { get; } = new() { Id = 2102, Name = "Cry of Pain" };
        public static Skill Intensity { get; } = new() { Id = 2104, Name = "Intensity" };
        public static Skill CriticalAgility { get; } = new() { Id = 2101, Name = "Critical Agility" };
        public static Skill Vampirism { get; } = new() { Id = 2110, Name = "Vampirism" };
        public static Skill TheresNothingtoFear { get; } = new() { Id = 2112, Name = "There's Nothing to Fear!" };
        public static Skill EternalAura { get; } = new() { Id = 2109, Name = "Eternal Aura" };
        public static Skill LightbringersGaze { get; } = new() { Id = 1814, Name = "Lightbringer's Gaze" };
        public static Skill LightbringerSignet { get; } = new() { Id = 1815, Name = "Lightbringer Signet" };
        public static Skill SaveYourselves { get; } = new() { Id = 1954, Name = "Save Yourselves!" };
        public static Skill TripleShot { get; } = new() { Id = 1953, Name = "Triple Shot" };
        public static Skill SelflessSpirit { get; } = new() { Id = 1952, Name = "Selfless Spirit" };
        public static Skill SignetofCorruption { get; } = new() { Id = 1950, Name = "Signet of Corruption" };
        public static Skill EtherNightmare { get; } = new() { Id = 1949, Name = "Ether Nightmare" };
        public static Skill ElementalLord { get; } = new() { Id = 1951, Name = "Elemental Lord" };
        public static Skill ShadowSanctuary { get; } = new() { Id = 1948, Name = "Shadow Sanctuary" };
        public static Skill SummonSpirits { get; } = new() { Id = 2051, Name = "Summon Spirits" };
        public static Skill SpearofFury { get; } = new() { Id = 1957, Name = "Spear of Fury" };
        public static Skill AuraofHolyMight { get; } = new() { Id = 1955, Name = "Aura of Holy Might" };
        public static Skill SaveYourselves2 { get; } = new() { Id = 2097, Name = "Save Yourselves!" };
        public static Skill TripleShot2 { get; } = new() { Id = 2096, Name = "Triple Shot" };
        public static Skill SelflessSpirit2 { get; } = new() { Id = 2095, Name = "Selfless Spirit" };
        public static Skill SignetofCorruption2 { get; } = new() { Id = 2093, Name = "Signet of Corruption" };
        public static Skill EtherNightmare2 { get; } = new() { Id = 2092, Name = "Ether Nightmare" };
        public static Skill ElementalLord2 { get; } = new() { Id = 2094, Name = "Elemental Lord" };
        public static Skill ShadowSanctuary2 { get; } = new() { Id = 2091, Name = "Shadow Sanctuary" };
        public static Skill SummonSpirits2 { get; } = new() { Id = 2100, Name = "Summon Spirits" };
        public static Skill SpearofFury2 { get; } = new() { Id = 2099, Name = "Spear of Fury" };
        public static Skill AuraofHolyMight2 { get; } = new() { Id = 2098, Name = "Aura of Holy Might" };
        public static Skill AirofSuperiority { get; } = new() { Id = 2416, Name = "Air of Superiority" };
        public static Skill AsuranScan { get; } = new() { Id = 2415, Name = "Asuran Scan" };
        public static Skill MentalBlock { get; } = new() { Id = 2417, Name = "Mental Block" };
        public static Skill Mindbender { get; } = new() { Id = 2411, Name = "Mindbender" };
        public static Skill PainInverter { get; } = new() { Id = 2418, Name = "Pain Inverter" };
        public static Skill RadiationField { get; } = new() { Id = 2414, Name = "Radiation Field" };
        public static Skill SmoothCriminal { get; } = new() { Id = 2412, Name = "Smooth Criminal" };
        public static Skill SummonMursaat { get; } = new() { Id = 2224, Name = "Summon Mursaat" };
        public static Skill SummonRubyDjinn { get; } = new() { Id = 2225, Name = "Summon Ruby Djinn" };
        public static Skill SummonIceImp { get; } = new() { Id = 2226, Name = "Summon Ice Imp" };
        public static Skill SummonNagaShaman { get; } = new() { Id = 2227, Name = "Summon Naga Shaman" };
        public static Skill Technobabble { get; } = new() { Id = 2413, Name = "Technobabble" };
        public static Skill ByUralsHammer { get; } = new() { Id = 2217, Name = "By Ural's Hammer!" };
        public static Skill DontTrip { get; } = new() { Id = 2216, Name = "Don't Trip!" };
        public static Skill AlkarsAlchemicalAcid { get; } = new() { Id = 2211, Name = "Alkar's Alchemical Acid" };
        public static Skill BlackPowderMine { get; } = new() { Id = 2223, Name = "Black Powder Mine" };
        public static Skill BrawlingHeadbutt { get; } = new() { Id = 2215, Name = "Brawling Headbutt" };
        public static Skill BreathoftheGreatDwarf { get; } = new() { Id = 2221, Name = "Breath of the Great Dwarf" };
        public static Skill DrunkenMaster { get; } = new() { Id = 2218, Name = "Drunken Master" };
        public static Skill DwarvenStability { get; } = new() { Id = 2423, Name = "Dwarven Stability" };
        public static Skill EarBite { get; } = new() { Id = 2213, Name = "Ear Bite" };
        public static Skill GreatDwarfArmor { get; } = new() { Id = 2220, Name = "Great Dwarf Armor" };
        public static Skill GreatDwarfWeapon { get; } = new() { Id = 2219, Name = "Great Dwarf Weapon" };
        public static Skill LightofDeldrimor { get; } = new() { Id = 2212, Name = "Light of Deldrimor" };
        public static Skill LowBlow { get; } = new() { Id = 2214, Name = "Low Blow" };
        public static Skill SnowStorm { get; } = new() { Id = 2222, Name = "Snow Storm" };
        public static Skill DeftStrike { get; } = new() { Id = 2228, Name = "Deft Strike" };
        public static Skill EbonBattleStandardofCourage { get; } = new() { Id = 2231, Name = "Ebon Battle Standard of Courage" };
        public static Skill EbonBattleStandardofWisdom { get; } = new() { Id = 2232, Name = "Ebon Battle Standard of Wisdom" };
        public static Skill EbonBattleStandardofHonor { get; } = new() { Id = 2233, Name = "Ebon Battle Standard of Honor" };
        public static Skill EbonEscape { get; } = new() { Id = 2420, Name = "Ebon Escape" };
        public static Skill EbonVanguardAssassinSupport { get; } = new() { Id = 2235, Name = "Ebon Vanguard Assassin Support" };
        public static Skill EbonVanguardSniperSupport { get; } = new() { Id = 2234, Name = "Ebon Vanguard Sniper Support" };
        public static Skill SignetofInfection { get; } = new() { Id = 2229, Name = "Signet of Infection" };
        public static Skill SneakAttack { get; } = new() { Id = 2116, Name = "Sneak Attack" };
        public static Skill TryptophanSignet { get; } = new() { Id = 2230, Name = "Tryptophan Signet" };
        public static Skill WeaknessTrap { get; } = new() { Id = 2421, Name = "Weakness Trap" };
        public static Skill Winds { get; } = new() { Id = 2422, Name = "Winds" };
        public static Skill DodgeThis { get; } = new() { Id = 2354, Name = "Dodge This!" };
        public static Skill FinishHim { get; } = new() { Id = 2353, Name = "Finish Him!" };
        public static Skill IAmUnstoppable { get; } = new() { Id = 2356, Name = "I Am Unstoppable!" };
        public static Skill IAmtheStrongest { get; } = new() { Id = 2355, Name = "I Am the Strongest!" };
        public static Skill YouAreAllWeaklings { get; } = new() { Id = 2359, Name = "You Are All Weaklings!" };
        public static Skill YouMoveLikeaDwarf { get; } = new() { Id = 2358, Name = "You Move Like a Dwarf!" };
        public static Skill ATouchofGuile { get; } = new() { Id = 2357, Name = "A Touch of Guile" };
        public static Skill ClubofaThousandBears { get; } = new() { Id = 2361, Name = "Club of a Thousand Bears" };
        public static Skill FeelNoPain { get; } = new() { Id = 2360, Name = "Feel No Pain" };
        public static Skill RavenBlessing { get; } = new() { Id = 2384, Name = "Raven Blessing" };
        public static Skill UrsanBlessing { get; } = new() { Id = 2374, Name = "Ursan Blessing" };
        public static Skill VolfenBlessing { get; } = new() { Id = 2379, Name = "Volfen Blessing" };
        public static Skill TimeWard { get; } = new() { Id = 3422, Name = "Time Ward" };
        public static Skill SoulTaker { get; } = new() { Id = 3423, Name = "Soul Taker" };
        public static Skill OverTheLimit { get; } = new() { Id = 3424, Name = "Over The Limit" };
        public static Skill JudgementStrike { get; } = new() { Id = 3425, Name = "Judgement Strike" };
        public static Skill SevenWeaponsStance { get; } = new() { Id = 3426, Name = "Seven Weapons Stance" };
        public static Skill Togetherasone { get; } = new() { Id = 3427, Name = "Together as one!" };
        public static Skill ShadowTheft { get; } = new() { Id = 3428, Name = "Shadow Theft" };
        public static Skill WeaponsofThreeForges { get; } = new() { Id = 3429, Name = "Weapons of Three Forges" };
        public static Skill VowofRevolution { get; } = new() { Id = 3430, Name = "Vow of Revolution" };
        public static Skill HeroicRefrain { get; } = new() { Id = 3431, Name = "Heroic Refrain" };
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

        public string Name { get; private set; }
        public int Id { get; private set; }
        private Skill()
        {
        }
    }
}
