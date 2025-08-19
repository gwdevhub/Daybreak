using Daybreak.Shared.Converters;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Daybreak.Shared.Models.Guildwars;

/// <summary>
/// Definition of a Guild Wars skill.
/// High-resolution skill icons can be found at:
/// https://wiki.guildwars.com/wiki/Gallery_of_high_resolution_skill_icons/large
/// </summary>
[JsonConverter(typeof(SkillJsonConverter))]
public sealed class Skill : IIconUrlEntity
{
    public static readonly Skill ATouchofGuile = new() { Id = 2357, Name = "A Touch of Guile", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/2/2d/A_Touch_of_Guile.jpg" };
    public static readonly Skill AccumulatedPain = new() { Id = 1052, Name = "Accumulated Pain", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/72/Accumulated_Pain_%28large%29.jpg?20081212200951" };
    public static readonly Skill AccumulatedPainPvP = new() { Id = 3184, Name = "Accumulated Pain (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = AccumulatedPain };
    public static readonly Skill Aegis = new() { Id = 257, Name = "Aegis", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/93/Aegis_%28large%29.jpg?20081212202310" };
    public static readonly Skill AegisPvP = new() { Id = 2857, Name = "Aegis (PvP)", AlternativeName = "", Profession = Profession.Monk, IconUrl = "", IsPvP = true, AlternativeSkill = Aegis };
    public static readonly Skill Aftershock = new() { Id = 174, Name = "Aftershock", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/ca/Aftershock_%28large%29.jpg?20081212195824" };
    public static readonly Skill AggressiveRefrain = new() { Id = 1774, Name = "Aggressive Refrain", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/a/a7/Aggressive_Refrain_%28large%29.jpg?20081212203705" };
    public static readonly Skill AgonizingChop = new() { Id = 1403, Name = "Agonizing Chop", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/3e/Agonizing_Chop_%28large%29.jpg?20081212210417" };
    public static readonly Skill Agony = new() { Id = 2205, Name = "Agony", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/43/Agony_%28large%29.jpg?20081212205855" };
    public static readonly Skill AgonyPvP = new() { Id = 3038, Name = "Agony (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Agony };
    public static readonly Skill AirAttunement = new() { Id = 225, Name = "Air Attunement", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/cc/Air_Attunement_%28large%29.jpg?20081212195915" };
    public static readonly Skill AirofDisenchantment = new() { Id = 1656, Name = "Air of Disenchantment", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/4/4f/Air_of_Disenchantment_%28large%29.jpg?20081212200321" };
    public static readonly Skill AirofEnchantment = new() { Id = 1115, Name = "Air of Enchantment", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/bf/Air_of_Enchantment_%28large%29.jpg?20081212201838" };
    public static readonly Skill AirofSuperiority = new() { Id = 2416, Name = "Air of Superiority", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/9/9f/Air_of_Superiority.jpg" };
    public static readonly Skill AlkarsAlchemicalAcid = new() { Id = 2211, Name = "Alkar's Alchemical Acid", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/4/43/Alkar%27s_Alchemical_Acid.jpg" };
    public static readonly Skill Amity = new() { Id = 265, Name = "Amity", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/98/Amity_%28large%29.jpg?20081212202033" };
    public static readonly Skill AncestorsVisage = new() { Id = 1054, Name = "Ancestor's Visage", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/6/63/Ancestor%27s_Visage_%28large%29.jpg?20081212201148" };
    public static readonly Skill AncestorsRage = new() { Id = 1246, Name = "Ancestors' Rage", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/4e/Ancestors%27_Rage_%28large%29.jpg?20081212205820" };
    public static readonly Skill AncestorsRagePvP = new() { Id = 2867, Name = "Ancestors' Rage (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = AncestorsRage };
    public static readonly Skill Aneurysm = new() { Id = 2055, Name = "Aneurysm", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/d7/Aneurysm_%28large%29.jpg?20081212200937" };
    public static readonly Skill AngelicBond = new() { Id = 1587, Name = "Angelic Bond", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/b2/Angelic_Bond_%28large%29.jpg?20081212204109" };
    public static readonly Skill AngelicProtection = new() { Id = 1586, Name = "Angelic Protection", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/d/d6/Angelic_Protection_%28large%29.jpg?20081212204155" };
    public static readonly Skill AngorodonsGaze = new() { Id = 2189, Name = "Angorodon's Gaze", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e1/Angorodon%27s_Gaze_%28large%29.jpg?20081212203154" };
    public static readonly Skill Anguish = new() { Id = 1745, Name = "Anguish", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/45/Anguish_%28large%29.jpg?20081212205250" };
    public static readonly Skill AnguishPvP = new() { Id = 3023, Name = "Anguish (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Anguish };
    public static readonly Skill AnguishedWasLingwah = new() { Id = 1223, Name = "Anguished Was Lingwah", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/4d/Anguished_Was_Lingwah_%28large%29.jpg?20081212205354" };
    public static readonly Skill AnimateBoneFiend = new() { Id = 84, Name = "Animate Bone Fiend", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/bd/Animate_Bone_Fiend_%28large%29.jpg?20081212202852" };
    public static readonly Skill AnimateBoneHorror = new() { Id = 83, Name = "Animate Bone Horror", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/a/af/Animate_Bone_Horror_%28large%29.jpg?20081212203142" };
    public static readonly Skill AnimateBoneMinions = new() { Id = 85, Name = "Animate Bone Minions", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/75/Animate_Bone_Minions_%28large%29.jpg?20081212203048" };
    public static readonly Skill AnimateFleshGolem = new() { Id = 832, Name = "Animate Flesh Golem", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/f2/Animate_Flesh_Golem_%28large%29.jpg?20081212202836" };
    public static readonly Skill AnimateShamblingHorror = new() { Id = 1351, Name = "Animate Shambling Horror", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/a/a0/Animate_Shambling_Horror_%28large%29.jpg?20081212202715" };
    public static readonly Skill AnimateVampiricHorror = new() { Id = 805, Name = "Animate Vampiric Horror", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/35/Animate_Vampiric_Horror_%28large%29.jpg?20081212202901" };
    public static readonly Skill AnthemofDisruption = new() { Id = 2018, Name = "Anthem of Disruption", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/6/6f/Anthem_of_Disruption_%28large%29.jpg?20081212204112" };
    public static readonly Skill AnthemofDisruptionPvP = new() { Id = 3040, Name = "Anthem of Disruption (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = AnthemofDisruption };
    public static readonly Skill AnthemofEnvy = new() { Id = 1559, Name = "Anthem of Envy", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/4/46/Anthem_of_Envy_%28large%29.jpg?20081212203717" };
    public static readonly Skill AnthemofEnvyPvP = new() { Id = 3148, Name = "Anthem of Envy (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = AnthemofEnvy };
    public static readonly Skill AnthemofFlame = new() { Id = 1557, Name = "Anthem of Flame", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/37/Anthem_of_Flame_%28large%29.jpg?20081212204043" };
    public static readonly Skill AnthemofFury = new() { Id = 1553, Name = "Anthem of Fury", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/e7/Anthem_of_Fury_%28large%29.jpg?20081212203732" };
    public static readonly Skill AnthemofGuidance = new() { Id = 1568, Name = "Anthem of Guidance", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/35/Anthem_of_Guidance_%28large%29.jpg?20081212203726" };
    public static readonly Skill AnthemofWeariness = new() { Id = 2017, Name = "Anthem of Weariness", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/bd/Anthem_of_Weariness_%28large%29.jpg?20081212203842" };
    public static readonly Skill AntidoteSignet = new() { Id = 427, Name = "Antidote Signet", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/4/47/Antidote_Signet_%28large%29.jpg?20090530231424" };
    public static readonly Skill ApplyPoison = new() { Id = 435, Name = "Apply Poison", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/9/9f/Apply_Poison_%28large%29.jpg?20081212204727" };
    public static readonly Skill ArcLightning = new() { Id = 842, Name = "Arc Lightning", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/e/e5/Arc_Lightning_%28large%29.jpg?20081212195333" };
    public static readonly Skill ArcaneConundrum = new() { Id = 36, Name = "Arcane Conundrum", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/4/42/Arcane_Conundrum_%28large%29.jpg?20081212201042" };
    public static readonly Skill ArcaneEcho = new() { Id = 75, Name = "Arcane Echo", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/76/Arcane_Echo_%28large%29.jpg?20081212201328" };
    public static readonly Skill ArcaneLanguor = new() { Id = 804, Name = "Arcane Languor", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/c7/Arcane_Languor_%28large%29.jpg?20081212195956" };
    public static readonly Skill ArcaneLarceny = new() { Id = 1062, Name = "Arcane Larceny", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/18/Arcane_Larceny_%28large%29.jpg?20081212200049" };
    public static readonly Skill ArcaneMimicry = new() { Id = 65, Name = "Arcane Mimicry", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/18/Arcane_Mimicry_%28large%29.jpg?20081212200111" };
    public static readonly Skill ArcaneThievery = new() { Id = 81, Name = "Arcane Thievery", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/7f/Arcane_Thievery_%28large%29.jpg?20081212200223" };
    public static readonly Skill ArcaneZeal = new() { Id = 1502, Name = "Arcane Zeal", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/55/Arcane_Zeal_%28large%29.jpg?20081212195009" };
    public static readonly Skill ArchersSignet = new() { Id = 1200, Name = "Archer's Signet", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/fb/Archer%27s_Signet_%28large%29.jpg?20081212204344" };
    public static readonly Skill ArcingShot = new() { Id = 1467, Name = "Arcing Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/77/Arcing_Shot_%28large%29.jpg?20081212204606" };
    public static readonly Skill AriaofRestoration = new() { Id = 1566, Name = "Aria of Restoration", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/2/26/Aria_of_Restoration_%28large%29.jpg?20081212203916" };
    public static readonly Skill AriaofZeal = new() { Id = 1562, Name = "Aria of Zeal", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/2/20/Aria_of_Zeal_%28large%29.jpg?20081212203828" };
    public static readonly Skill ArmorofEarth = new() { Id = 165, Name = "Armor of Earth", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/51/Armor_of_Earth_%28large%29.jpg?20081212195145" };
    public static readonly Skill ArmorofFrost = new() { Id = 206, Name = "Armor of Frost", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/f/f5/Armor_of_Frost_%28large%29.jpg?20081212195455" };
    public static readonly Skill ArmorofMist = new() { Id = 238, Name = "Armor of Mist", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/cf/Armor_of_Mist_%28large%29.jpg?20081212195509" };
    public static readonly Skill ArmorofSanctity = new() { Id = 1515, Name = "Armor of Sanctity", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/7/74/Armor_of_Sanctity_%28large%29.jpg?20081212194854" };
    public static readonly Skill ArmorofUnfeeling = new() { Id = 1232, Name = "Armor of Unfeeling", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/a/a6/Armor_of_Unfeeling_%28large%29.jpg?20081212205809" };
    public static readonly Skill ArmorofUnfeelingPvP = new() { Id = 3003, Name = "Armor of Unfeeling (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = ArmorofUnfeeling };
    public static readonly Skill AshBlast = new() { Id = 1085, Name = "Ash Blast", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/3/33/Ash_Blast_%28large%29.jpg?20081212195219" };
    public static readonly Skill AssassinsPromise = new() { Id = 1035, Name = "Assassin's Promise", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/29/Assassin%27s_Promise_%28large%29.jpg?20081212194547" };
    public static readonly Skill AssassinsRemedy = new() { Id = 1639, Name = "Assassin's Remedy", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/6/66/Assassin%27s_Remedy_%28large%29.jpg?20081212194248" };
    public static readonly Skill AssassinsRemedyPvP = new() { Id = 2869, Name = "Assassin's Remedy (PvP)", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "", IsPvP = true, AlternativeSkill = AssassinsRemedy };
    public static readonly Skill AssaultEnchantments = new() { Id = 1643, Name = "Assault Enchantments", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/b8/Assault_Enchantments_%28large%29.jpg?20081212194153" };
    public static readonly Skill AsuranScan = new() { Id = 2415, Name = "Asuran Scan", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/a/a0/Asuran_Scan.jpg" };
    public static readonly Skill Atrophy = new() { Id = 2237, Name = "Atrophy", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/6c/Atrophy_%28large%29.jpg?20081212202948" };
    public static readonly Skill AttackersInsight = new() { Id = 1764, Name = "Attacker's Insight", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/7/75/Attacker%27s_Insight_%28large%29.jpg?20081212195043" };
    public static readonly Skill AttunedWasSongkai = new() { Id = 1220, Name = "Attuned Was Songkai", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/5/50/Attuned_Was_Songkai_%28large%29.jpg?20081212205457" };
    public static readonly Skill AuguryofDeath = new() { Id = 1646, Name = "Augury of Death", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/3/35/Augury_of_Death_%28large%29.jpg?20081212194013" };
    public static readonly Skill AuraofDisplacement = new() { Id = 771, Name = "Aura of Displacement", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/3/33/Aura_of_Displacement_%28large%29.jpg?20081212194025" };
    public static readonly Skill AuraofFaith = new() { Id = 260, Name = "Aura of Faith", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/2/29/Aura_of_Faith_%28large%29.jpg?20081212202117" };
    public static readonly Skill AuraofHolyMight = new() { Id = 1955, Name = "Aura of Holy Might", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/5c/Aura_of_Holy_Might.jpg" };
    public static readonly Skill AuraofHolyMight2 = new() { Id = 2098, Name = "Aura of Holy Might", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/5c/Aura_of_Holy_Might.jpg" };
    public static readonly Skill AuraofRestoration = new() { Id = 180, Name = "Aura of Restoration", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/4e/Aura_of_Restoration_%28large%29.jpg?20081212195822" };
    public static readonly Skill AuraofRestorationPvP = new() { Id = 3375, Name = "Aura of Restoration (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = AuraofRestoration };
    public static readonly Skill AuraofStability = new() { Id = 2063, Name = "Aura of Stability", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/52/Aura_of_Stability_%28large%29.jpg?20081212201830" };
    public static readonly Skill AuraoftheLich = new() { Id = 114, Name = "Aura of the Lich", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/67/Aura_of_the_Lich_%28large%29.jpg?20081212203632" };
    public static readonly Skill AuraofThorns = new() { Id = 1495, Name = "Aura of Thorns", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/c/c4/Aura_of_Thorns_%28large%29.jpg?20081212195030" };
    public static readonly Skill AuraofThornsPvP = new() { Id = 3346, Name = "Aura of Thorns (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = AuraofThorns };
    public static readonly Skill AuraSlicer = new() { Id = 2070, Name = "Aura Slicer", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/d/de/Aura_Slicer_%28large%29.jpg?20081212194910" };
    public static readonly Skill AuspiciousBlow = new() { Id = 905, Name = "Auspicious Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/8/84/Auspicious_Blow_%28large%29.jpg?20081212210451" };
    public static readonly Skill AuspiciousIncantation = new() { Id = 930, Name = "Auspicious Incantation", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/3/34/Auspicious_Incantation_%28large%29.jpg?20081212200241" };
    public static readonly Skill AuspiciousParry = new() { Id = 1142, Name = "Auspicious Parry", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/61/Auspicious_Parry_%28large%29.jpg?20081212210743" };
    public static readonly Skill AvatarofBalthazar = new() { Id = 1518, Name = "Avatar of Balthazar", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/a/af/Avatar_of_Balthazar_%28large%29.jpg?20081212194612" };
    public static readonly Skill AvatarofDwayna = new() { Id = 1519, Name = "Avatar of Dwayna", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/d/d6/Avatar_of_Dwayna_%28large%29.jpg?20081212194704" };
    public static readonly Skill AvatarofDwaynaPvP = new() { Id = 3270, Name = "Avatar of Dwayna (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = AvatarofDwayna };
    public static readonly Skill AvatarofGrenth = new() { Id = 1520, Name = "Avatar of Grenth", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/c/cf/Avatar_of_Grenth_%28large%29.jpg?20081212195025" };
    public static readonly Skill AvatarofLyssa = new() { Id = 1521, Name = "Avatar of Lyssa", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/2/2a/Avatar_of_Lyssa_%28large%29.jpg?20081212195045" };
    public static readonly Skill AvatarofMelandru = new() { Id = 1522, Name = "Avatar of Melandru", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/a/a5/Avatar_of_Melandru_%28large%29.jpg?20081212194758" };
    public static readonly Skill AvatarofMelandruPvP = new() { Id = 3271, Name = "Avatar of Melandru (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = AvatarofMelandru };
    public static readonly Skill AwakentheBlood = new() { Id = 111, Name = "Awaken the Blood", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/5e/Awaken_the_Blood_%28large%29.jpg?20081212203330" };
    public static readonly Skill Awe = new() { Id = 1573, Name = "Awe", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/5c/Awe_%28large%29.jpg?20081212204231" };
    public static readonly Skill AxeRake = new() { Id = 334, Name = "Axe Rake", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/67/Axe_Rake_%28large%29.jpg?20090531013616" };
    public static readonly Skill AxeTwist = new() { Id = 342, Name = "Axe Twist", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/a9/Axe_Twist_%28large%29.jpg?20081212210753" };
    public static readonly Skill Backbreaker = new() { Id = 358, Name = "Backbreaker", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/36/Backbreaker_%28large%29.jpg?20081212210249" };
    public static readonly Skill Backfire = new() { Id = 28, Name = "Backfire", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/15/Backfire_%28large%29.jpg?20081212201609" };
    public static readonly Skill BalancedStance = new() { Id = 371, Name = "Balanced Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/7/75/Balanced_Stance_%28large%29.jpg?20081212210805" };
    public static readonly Skill BalladofRestoration = new() { Id = 1564, Name = "Ballad of Restoration", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/e5/Ballad_of_Restoration_%28large%29.jpg?20081212204056" };
    public static readonly Skill BalladofRestorationPvP = new() { Id = 2877, Name = "Ballad of Restoration (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = BalladofRestoration };
    public static readonly Skill BalthazarsAura = new() { Id = 272, Name = "Balthazar's Aura", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/1/12/Balthazar%27s_Aura_%28large%29.jpg?20081212201810" };
    public static readonly Skill BalthazarsPendulum = new() { Id = 1395, Name = "Balthazar's Pendulum", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/1/17/Balthazar%27s_Pendulum_%28large%29.jpg?20081212202353" };
    public static readonly Skill BalthazarsRage = new() { Id = 1496, Name = "Balthazar's Rage", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/5c/Balthazar%27s_Rage_%28large%29.jpg?20081212195014" };
    public static readonly Skill BalthazarsSpirit = new() { Id = 242, Name = "Balthazar's Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c6/Balthazar%27s_Spirit_%28large%29.jpg?20081212202132" };
    public static readonly Skill BaneSignet = new() { Id = 296, Name = "Bane Signet", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/9c/Bane_Signet_%28large%29.jpg?20081212201952" };
    public static readonly Skill Banish = new() { Id = 252, Name = "Banish", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/4/4a/Banish_%28large%29.jpg?20081212202340" };
    public static readonly Skill BanishingStrike = new() { Id = 1483, Name = "Banishing Strike", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/1d/Banishing_Strike_%28large%29.jpg?20081212194709" };
    public static readonly Skill BanishingStrikePvP = new() { Id = 3263, Name = "Banishing Strike (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = BanishingStrike };
    public static readonly Skill BarbarousSlice = new() { Id = 1416, Name = "Barbarous Slice", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/4/47/Barbarous_Slice_%28large%29.jpg?20081212210147" };
    public static readonly Skill BarbedArrows = new() { Id = 1470, Name = "Barbed Arrows", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/9/94/Barbed_Arrows_%28large%29.jpg?20081212204934" };
    public static readonly Skill BarbedSignet = new() { Id = 131, Name = "Barbed Signet", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/8c/Barbed_Signet_%28large%29.jpg?20081212203313" };
    public static readonly Skill BarbedSpear = new() { Id = 1600, Name = "Barbed Spear", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/50/Barbed_Spear_%28large%29.jpg?20081212204233" };
    public static readonly Skill BarbedTrap = new() { Id = 458, Name = "Barbed Trap", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/9/92/Barbed_Trap_%28large%29.jpg?20081212204428" };
    public static readonly Skill Barbs = new() { Id = 101, Name = "Barbs", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/85/Barbs_%28large%29.jpg?20081212203310" };
    public static readonly Skill Barrage = new() { Id = 395, Name = "Barrage", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/13/Barrage_%28large%29.jpg?20081212205103" };
    public static readonly Skill BattleRage = new() { Id = 317, Name = "Battle Rage", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/9/92/Battle_Rage_%28large%29.jpg?20081212210802" };
    public static readonly Skill BedofCoals = new() { Id = 825, Name = "Bed of Coals", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/c5/Bed_of_Coals_%28large%29.jpg?20081212195148" };
    public static readonly Skill BeguilingHaze = new() { Id = 799, Name = "Beguiling Haze", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/c5/Beguiling_Haze_%28large%29.jpg?20081212194413" };
    public static readonly Skill BellySmash = new() { Id = 350, Name = "Belly Smash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/2/2c/Belly_Smash_%28large%29.jpg?20081212210145" };
    public static readonly Skill BerserkerStance = new() { Id = 370, Name = "Berserker Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/0f/Berserker_Stance_%28large%29.jpg?20081212211216" };
    public static readonly Skill BestialFury = new() { Id = 1209, Name = "Bestial Fury", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5a/Bestial_Fury_%28large%29.jpg?20081212204708" };
    public static readonly Skill BestialMauling = new() { Id = 1203, Name = "Bestial Mauling", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/e/e5/Bestial_Mauling_%28large%29.jpg?20081212204406" };
    public static readonly Skill BestialPounce = new() { Id = 437, Name = "Bestial Pounce", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/b/b1/Bestial_Pounce_%28large%29.jpg?20081212204542" };
    public static readonly Skill BindingChains = new() { Id = 1236, Name = "Binding Chains", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/2/26/Binding_Chains_%28large%29.jpg?20081212205638" };
    public static readonly Skill BitterChill = new() { Id = 1068, Name = "Bitter Chill", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/b8/Bitter_Chill_%28large%29.jpg?20081212203017" };
    public static readonly Skill BlackLotusStrike = new() { Id = 779, Name = "Black Lotus Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/26/Black_Lotus_Strike_%28large%29.jpg?20081212193954" };
    public static readonly Skill BlackMantisThrust = new() { Id = 1024, Name = "Black Mantis Thrust", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/26/Black_Mantis_Thrust_%28large%29.jpg?20081212194545" };
    public static readonly Skill BlackPowderMine = new() { Id = 2223, Name = "Black Powder Mine", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/5/50/Black_Powder_Mine.jpg" };
    public static readonly Skill BlackSpiderStrike = new() { Id = 1636, Name = "Black Spider Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/c6/Black_Spider_Strike_%28large%29.jpg?20081212194445" };
    public static readonly Skill Blackout = new() { Id = 29, Name = "Blackout", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/6/6d/Blackout_%28large%29.jpg?20081212201131" };
    public static readonly Skill BladesofSteel = new() { Id = 1020, Name = "Blades of Steel", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/bf/Blades_of_Steel_%28large%29.jpg?20081212194418" };
    public static readonly Skill BladeturnRefrain = new() { Id = 1580, Name = "Bladeturn Refrain", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/33/Bladeturn_Refrain_%28large%29.jpg?20081212204236" };
    public static readonly Skill BladeturnRefrainPvP = new() { Id = 3029, Name = "Bladeturn Refrain (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = BladeturnRefrain };
    public static readonly Skill BlazingFinale = new() { Id = 1575, Name = "Blazing Finale", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/f/fd/Blazing_Finale_%28large%29.jpg?20081212203904" };
    public static readonly Skill BlazingFinalePvP = new() { Id = 3028, Name = "Blazing Finale (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = BlazingFinale };
    public static readonly Skill BlazingSpear = new() { Id = 1546, Name = "Blazing Spear", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/59/Blazing_Spear_%28large%29.jpg?20081212204212" };
    public static readonly Skill BlessedAura = new() { Id = 256, Name = "Blessed Aura", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/97/Blessed_Aura_%28large%29.jpg?20081212202124" };
    public static readonly Skill BlessedLight = new() { Id = 941, Name = "Blessed Light", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/5d/Blessed_Light_%28large%29.jpg?20081212202526" };
    public static readonly Skill BlessedSignet = new() { Id = 297, Name = "Blessed Signet", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/e2/Blessed_Signet_%28large%29.jpg?20081212202259" };
    public static readonly Skill BlindWasMingson = new() { Id = 788, Name = "Blind Was Mingson", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/8a/Blind_Was_Mingson_%28large%29.jpg?20081212205316" };
    public static readonly Skill BlindingFlash = new() { Id = 220, Name = "Blinding Flash", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/0/07/Blinding_Flash_%28large%29.jpg?20081212195625" };
    public static readonly Skill BlindingPowder = new() { Id = 973, Name = "Blinding Powder", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/ba/Blinding_Powder_%28large%29.jpg?20081212194252" };
    public static readonly Skill BlindingSurge = new() { Id = 1367, Name = "Blinding Surge", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/c6/Blinding_Surge_%28large%29.jpg?20081212195314" };
    public static readonly Skill BloodBond = new() { Id = 835, Name = "Blood Bond", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/1/10/Blood_Bond_%28large%29.jpg?20081212203552" };
    public static readonly Skill BloodDrinker = new() { Id = 1076, Name = "Blood Drinker", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/99/Blood_Drinker_%28large%29.jpg?20081212203009" };
    public static readonly Skill BloodisPower = new() { Id = 119, Name = "Blood is Power", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/4f/Blood_is_Power_%28large%29.jpg?20100404020852" };
    public static readonly Skill BloodoftheAggressor = new() { Id = 902, Name = "Blood of the Aggressor", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e2/Blood_of_the_Aggressor_%28large%29.jpg?20081212203035" };
    public static readonly Skill BloodoftheMaster = new() { Id = 120, Name = "Blood of the Master", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/b0/Blood_of_the_Master_%28large%29.jpg?20081212202813" };
    public static readonly Skill BloodRenewal = new() { Id = 115, Name = "Blood Renewal", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/64/Blood_Renewal_%28large%29.jpg?20081212203127" };
    public static readonly Skill BloodRitual = new() { Id = 157, Name = "Blood Ritual", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/fd/Blood_Ritual_%28large%29.jpg?20081212202703" };
    public static readonly Skill Bloodsong = new() { Id = 1253, Name = "Bloodsong", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/4a/Bloodsong_%28large%29.jpg?20081212205606" };
    public static readonly Skill BloodsongPvP = new() { Id = 3019, Name = "Bloodsong (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Bloodsong };
    public static readonly Skill BlurredVision = new() { Id = 235, Name = "Blurred Vision", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a6/Blurred_Vision_%28large%29.jpg?20081212195908" };
    public static readonly Skill BodyBlow = new() { Id = 2197, Name = "Body Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/e/e7/Body_Blow_%28large%29.jpg?20081212211114" };
    public static readonly Skill BodyShot = new() { Id = 2198, Name = "Body Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/b/b8/Body_Shot_%28large%29.jpg?20081212204545" };
    public static readonly Skill BonettisDefense = new() { Id = 380, Name = "Bonetti's Defense", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/b/b8/Bonetti%27s_Defense_%28large%29.jpg?20081212210150" };
    public static readonly Skill BoonofCreation = new() { Id = 1230, Name = "Boon of Creation", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/3/37/Boon_of_Creation_%28large%29.jpg?20081212205602" };
    public static readonly Skill BoonSignet = new() { Id = 847, Name = "Boon Signet", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/b8/Boon_Signet_%28large%29.jpg?20081212202004" };
    public static readonly Skill BraceYourself = new() { Id = 1572, Name = "Brace Yourself!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/4/47/%22Brace_Yourself%21%22_%28large%29.jpg" };
    public static readonly Skill BraceYourselfPvP = new() { Id = 3027, Name = "Brace Yourself! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = BraceYourself };
    public static readonly Skill Brambles = new() { Id = 947, Name = "Brambles", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d3/Brambles_%28large%29.jpg?20081212204251" };
    public static readonly Skill BrawlingHeadbutt = new() { Id = 2215, Name = "Brawling Headbutt", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/b/be/Brawling_Headbutt.jpg" };
    public static readonly Skill BreathofFire = new() { Id = 1094, Name = "Breath of Fire", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/db/Breath_of_Fire_%28large%29.jpg?20081212195930" };
    public static readonly Skill BreathoftheGreatDwarf = new() { Id = 2221, Name = "Breath of the Great Dwarf", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/0/0e/Breath_of_the_Great_Dwarf.jpg" };
    public static readonly Skill BroadHeadArrow = new() { Id = 1198, Name = "Broad Head Arrow", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/35/Broad_Head_Arrow_%28large%29.jpg?20081212204257" };
    public static readonly Skill BrutalStrike = new() { Id = 444, Name = "Brutal Strike", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5e/Brutal_Strike_%28large%29.jpg?20081212204804" };
    public static readonly Skill BrutalWeapon = new() { Id = 1258, Name = "Brutal Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/1/1f/Brutal_Weapon_%28large%29.jpg?20081212205239" };
    public static readonly Skill BullsCharge = new() { Id = 379, Name = "Bull's Charge", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/3c/Bull%27s_Charge_%28large%29.jpg?20081212210353" };
    public static readonly Skill BullsStrike = new() { Id = 332, Name = "Bull's Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/ad/Bull%27s_Strike_%28large%29.jpg?20081212210130" };
    public static readonly Skill BurningArrow = new() { Id = 1466, Name = "Burning Arrow", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/7e/Burning_Arrow_%28large%29.jpg?20081212205112" };
    public static readonly Skill BurningRefrain = new() { Id = 1576, Name = "Burning Refrain", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/31/Burning_Refrain_%28large%29.jpg?20081212204038" };
    public static readonly Skill BurningShield = new() { Id = 2208, Name = "Burning Shield", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/2/2e/Burning_Shield_%28large%29.jpg?20081212203907" };
    public static readonly Skill BurningSpeed = new() { Id = 823, Name = "Burning Speed", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/0/07/Burning_Speed_%28large%29.jpg?20081212195316" };
    public static readonly Skill BurstofAggression = new() { Id = 1413, Name = "Burst of Aggression", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/10/Burst_of_Aggression_%28large%29.jpg?20081212210133" };
    public static readonly Skill ByUralsHammer = new() { Id = 2217, Name = "By Ural's Hammer!", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/d/df/%22By_Ural%27s_Hammer%21%22.jpg" };
    public static readonly Skill Cacophony = new() { Id = 1998, Name = "Cacophony", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/f7/Cacophony_%28large%29.jpg?20081212203525" };
    public static readonly Skill CalculatedRisk = new() { Id = 2053, Name = "Calculated Risk", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/e/ee/Calculated_Risk_%28large%29.jpg?20081212201527" };
    public static readonly Skill CalculatedRiskPvP = new() { Id = 3196, Name = "Calculated Risk (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = CalculatedRisk };
    public static readonly Skill CallofHaste = new() { Id = 415, Name = "Call of Haste", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/c/c1/Call_of_Haste_%28large%29.jpg?20081212204557" };
    public static readonly Skill CallofHastePvP = new() { Id = 2657, Name = "Call of Haste (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = CallofHaste };
    public static readonly Skill CallofProtection = new() { Id = 412, Name = "Call of Protection", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/a/af/Call_of_Protection_%28large%29.jpg?20081212204534" };
    public static readonly Skill CalledShot = new() { Id = 403, Name = "Called Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/79/Called_Shot_%28large%29.jpg?20081212204744" };
    public static readonly Skill Caltrops = new() { Id = 985, Name = "Caltrops", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/b3/Caltrops_%28large%29.jpg?20081212193945" };
    public static readonly Skill CantTouchThis = new() { Id = 1780, Name = "Can't Touch This!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/3d/%22Can%27t_Touch_This%21%22.jpg" };
    public static readonly Skill CantTouchThisPvP = new() { Id = 3031, Name = "Can't Touch This! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = CantTouchThis };
    public static readonly Skill CaretakersCharge = new() { Id = 1744, Name = "Caretaker's Charge", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/7/73/Caretaker%27s_Charge_%28large%29.jpg?20081212205548" };
    public static readonly Skill CastigationSignet = new() { Id = 2006, Name = "Castigation Signet", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/8c/Castigation_Signet_%28large%29.jpg?20081212201753" };
    public static readonly Skill CauterySignet = new() { Id = 1588, Name = "Cautery Signet", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/e9/Cautery_Signet_%28large%29.jpg?20081212204050" };
    public static readonly Skill ChainLightning = new() { Id = 223, Name = "Chain Lightning", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/3/3a/Chain_Lightning_%28large%29.jpg?20081212195226" };
    public static readonly Skill ChanneledStrike = new() { Id = 1225, Name = "Channeled Strike", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/d/da/Channeled_Strike_%28large%29.jpg?20081212205755" };
    public static readonly Skill Channeling = new() { Id = 38, Name = "Channeling", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/b/b4/Channeling_%28large%29.jpg?20081212201751" };
    public static readonly Skill ChaosStorm = new() { Id = 77, Name = "Chaos Storm", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/15/Chaos_Storm_%28large%29.jpg?20081212201433" };
    public static readonly Skill Charge = new() { Id = 364, Name = "Charge!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/32/%22Charge%21%22.jpg" };
    public static readonly Skill ChargingStrike = new() { Id = 1405, Name = "Charging Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/c/c6/Charging_Strike_%28large%29.jpg?20081212210519" };
    public static readonly Skill CharmAnimal = new() { Id = 411, Name = "Charm Animal", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/12/Charm_Animal_%28large%29.jpg?20081212204515" };
    public static readonly Skill CharmAnimalCodex = new () { Id = 3068, Name = "Charm Animal (Codex)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = CharmAnimal };
    public static readonly Skill ChestThumper = new() { Id = 2074, Name = "Chest Thumper", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/58/Chest_Thumper_%28large%29.jpg?20081212204031" };
    public static readonly Skill Chilblains = new() { Id = 144, Name = "Chilblains", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/71/Chilblains_%28large%29.jpg?20081212203629" };
    public static readonly Skill ChillingVictory = new() { Id = 1539, Name = "Chilling Victory", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/10/Chilling_Victory_%28large%29.jpg?20081212194605" };
    public static readonly Skill ChillingWinds = new() { Id = 1368, Name = "Chilling Winds", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/3/3e/Chilling_Winds_%28large%29.jpg?20081212195659" };
    public static readonly Skill ChokingGas = new() { Id = 434, Name = "Choking Gas", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/6/67/Choking_Gas_%28large%29.jpg?20081212204422" };
    public static readonly Skill ChorusofRestoration = new() { Id = 1565, Name = "Chorus of Restoration", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/32/Chorus_of_Restoration_%28large%29.jpg?20081212203814" };
    public static readonly Skill ChurningEarth = new() { Id = 844, Name = "Churning Earth", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/f/fa/Churning_Earth_%28large%29.jpg?20081212195440" };
    public static readonly Skill ClamorofSouls = new() { Id = 1215, Name = "Clamor of Souls", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/a/ae/Clamor_of_Souls_%28large%29.jpg?20081212205757" };
    public static readonly Skill Cleave = new() { Id = 335, Name = "Cleave", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/c/c5/Cleave_%28large%29.jpg?20081212210456" };
    public static readonly Skill ClubofaThousandBears = new() { Id = 2361, Name = "Club of a Thousand Bears", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/d/dc/Club_of_a_Thousand_Bears.jpg" };
    public static readonly Skill Clumsiness = new() { Id = 43, Name = "Clumsiness", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/2f/Clumsiness_%28large%29.jpg?20081212201717" };
    public static readonly Skill ComfortAnimal = new() { Id = 436, Name = "Comfort Animal", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/6/61/Comfort_Animal_%28large%29.jpg?20081212204402" };
    public static readonly Skill ComfortAnimalPvP = new() { Id = 3045, Name = "Comfort Animal (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = ComfortAnimal };
    public static readonly Skill Companionship = new() { Id = 2141, Name = "Companionship", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/b/b7/Companionship_%28large%29.jpg?20081212204332" };
    public static readonly Skill Complicate = new() { Id = 932, Name = "Complicate", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/2c/Complicate_%28large%29.jpg?20081212200829" };
    public static readonly Skill ConcussionShot = new() { Id = 408, Name = "Concussion Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/3f/Concussion_Shot_%28large%29.jpg?20090530233814" };
    public static readonly Skill Conflagration = new() { Id = 466, Name = "Conflagration", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/80/Conflagration_%28large%29.jpg?20081212205028" };
    public static readonly Skill ConfusingImages = new() { Id = 2137, Name = "Confusing Images", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/e/ee/Confusing_Images_%28large%29.jpg?20081212201414" };
    public static readonly Skill ConjureFlame = new() { Id = 182, Name = "Conjure Flame", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/92/Conjure_Flame_%28large%29.jpg?20081212195623" };
    public static readonly Skill ConjureFrost = new() { Id = 207, Name = "Conjure Frost", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a6/Conjure_Frost_%28large%29.jpg?20081212195321" };
    public static readonly Skill ConjureLightning = new() { Id = 221, Name = "Conjure Lightning", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b1/Conjure_Lightning_%28large%29.jpg?20081212195802" };
    public static readonly Skill ConjureNightmare = new() { Id = 859, Name = "Conjure Nightmare", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/e/e9/Conjure_Nightmare_%28large%29.jpg?20081212200702" };
    public static readonly Skill ConjurePhantasm = new() { Id = 31, Name = "Conjure Phantasm", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/ad/Conjure_Phantasm_%28large%29.jpg?20081212195947" };
    public static readonly Skill ConsumeCorpse = new() { Id = 98, Name = "Consume Corpse", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/2/28/Consume_Corpse_%28large%29.jpg?20081212203151" };
    public static readonly Skill ConsumeSoul = new() { Id = 914, Name = "Consume Soul", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/f/fa/Consume_Soul_%28large%29.jpg?20081212205746" };
    public static readonly Skill Contagion = new() { Id = 1356, Name = "Contagion", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/d4/Contagion_%28large%29.jpg?20081212202705" };
    public static readonly Skill ContemplationofPurity = new() { Id = 300, Name = "Contemplation of Purity", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/2/2a/Contemplation_of_Purity_%28large%29.jpg?20081212202640" };
    public static readonly Skill ConvertHexes = new() { Id = 303, Name = "Convert Hexes", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/0b/Convert_Hexes_%28large%29.jpg?20081212202657" };
    public static readonly Skill Conviction = new() { Id = 1540, Name = "Conviction", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/12/Conviction_%28large%29.jpg?20081212194600" };
    public static readonly Skill CorruptEnchantment = new() { Id = 1362, Name = "Corrupt Enchantment", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/58/Corrupt_Enchantment_%28large%29.jpg?20081212202830" };
    public static readonly Skill CounterBlow = new() { Id = 357, Name = "Counter Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/16/Counter_Blow_%28large%29.jpg?20081212210229" };
    public static readonly Skill Counterattack = new() { Id = 1693, Name = "Counterattack", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/3d/Counterattack_%28large%29.jpg?20081212210959" };
    public static readonly Skill Coward = new() { Id = 869, Name = "Coward!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/9/92/%22Coward%21%22_%28large%29.jpg" };
    public static readonly Skill CripplingAnguish = new() { Id = 54, Name = "Crippling Anguish", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/13/Crippling_Anguish_%28large%29.jpg?20081212200650" };
    public static readonly Skill CripplingAnguishPvP = new() { Id = 3152, Name = "Crippling Anguish (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = CripplingAnguish };
    public static readonly Skill CripplingAnthem = new() { Id = 1554, Name = "Crippling Anthem", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/4/48/Crippling_Anthem_%28large%29.jpg?20081212204101" };
    public static readonly Skill CripplingDagger = new() { Id = 1038, Name = "Crippling Dagger", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/e/ee/Crippling_Dagger_%28large%29.jpg?20081212194541" };
    public static readonly Skill CripplingShot = new() { Id = 393, Name = "Crippling Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/72/Crippling_Shot_%28large%29.jpg?20081212205233" };
    public static readonly Skill CripplingSlash = new() { Id = 1415, Name = "Crippling Slash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/7/74/Crippling_Slash_%28large%29.jpg?20081212211136" };
    public static readonly Skill CripplingSweep = new() { Id = 1535, Name = "Crippling Sweep", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/d/d2/Crippling_Sweep_%28large%29.jpg?20081212195022" };
    public static readonly Skill CripplingVictory = new() { Id = 2147, Name = "Crippling Victory", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/9/9e/Crippling_Victory_%28large%29.jpg?20081212194900" };
    public static readonly Skill CriticalAgility = new() { Id = 2101, Name = "Critical Agility", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/e/e8/Critical_Agility.jpg" };
    public static readonly Skill CriticalChop = new() { Id = 1402, Name = "Critical Chop", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/f/f7/Critical_Chop_%28large%29.jpg?20081212210646" };
    public static readonly Skill CriticalDefenses = new() { Id = 1027, Name = "Critical Defenses", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/0/03/Critical_Defenses_%28large%29.jpg?20081212194424" };
    public static readonly Skill CriticalEye = new() { Id = 1018, Name = "Critical Eye", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/b1/Critical_Eye_%28large%29.jpg?20081212194254" };
    public static readonly Skill CriticalStrike = new() { Id = 1019, Name = "Critical Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/3/38/Critical_Strike_%28large%29.jpg?20081212194313" };
    public static readonly Skill Crossfire = new() { Id = 1469, Name = "Crossfire", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/f0/Crossfire_%28large%29.jpg?20081212205051" };
    public static readonly Skill CrudeSwing = new() { Id = 353, Name = "Crude Swing", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/4/44/Crude_Swing_%28large%29.jpg?20090701021445" };
    public static readonly Skill CruelSpear = new() { Id = 1548, Name = "Cruel Spear", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/e7/Cruel_Spear_%28large%29.jpg?20081212204239" };
    public static readonly Skill CruelWasDaoshen = new() { Id = 1218, Name = "Cruel Was Daoshen", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/0e/Cruel_Was_Daoshen_%28large%29.jpg?20081212205923" };
    public static readonly Skill CrushingBlow = new() { Id = 352, Name = "Crushing Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/ab/Crushing_Blow_%28large%29.jpg?20090531014625" };
    public static readonly Skill CryofFrustration = new() { Id = 57, Name = "Cry of Frustration", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/ad/Cry_of_Frustration_%28large%29.jpg?20081212201452" };
    public static readonly Skill CryofPain = new() { Id = 2102, Name = "Cry of Pain", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/93/Cry_of_Pain.jpg" };
    public static readonly Skill CrystalWave = new() { Id = 217, Name = "Crystal Wave", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/91/Crystal_Wave_%28large%29.jpg?20090530231115" };
    public static readonly Skill CultistsFervor = new() { Id = 806, Name = "Cultist's Fervor", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/85/Cultist%27s_Fervor_%28large%29.jpg?20081212203543" };
    public static readonly Skill CureHex = new() { Id = 2003, Name = "Cure Hex", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/b0/Cure_Hex_%28large%29.jpg?20081212201844" };
    public static readonly Skill CycloneAxe = new() { Id = 330, Name = "Cyclone Axe", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/18/Cyclone_Axe_%28large%29.jpg?20090531014732" };
    public static readonly Skill DancingDaggers = new() { Id = 858, Name = "Dancing Daggers", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/df/Dancing_Daggers_%28large%29.jpg?20081212194531" };
    public static readonly Skill DarkApostasy = new() { Id = 1029, Name = "Dark Apostasy", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/a/ac/Dark_Apostasy_%28large%29.jpg?20081212194411" };
    public static readonly Skill DarkAura = new() { Id = 116, Name = "Dark Aura", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/d0/Dark_Aura_%28large%29.jpg?20081212203217" };
    public static readonly Skill DarkBond = new() { Id = 138, Name = "Dark Bond", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/8d/Dark_Bond_%28large%29.jpg?20081212202856" };
    public static readonly Skill DarkEscape = new() { Id = 1037, Name = "Dark Escape", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/1/10/Dark_Escape_%28large%29.jpg?20081212194130" };
    public static readonly Skill DarkFury = new() { Id = 147, Name = "Dark Fury", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/7d/Dark_Fury_%28large%29.jpg?20081212203534" };
    public static readonly Skill DarkPact = new() { Id = 133, Name = "Dark Pact", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/07/Dark_Pact_%28large%29.jpg?20081212203355" };
    public static readonly Skill DarkPrison = new() { Id = 1044, Name = "Dark Prison", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/b0/Dark_Prison_%28large%29.jpg?20081212194115" };
    public static readonly Skill Dash = new() { Id = 1043, Name = "Dash", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/9/9b/Dash_%28large%29.jpg?20081212194459" };
    public static readonly Skill DeadlyHaste = new() { Id = 1638, Name = "Deadly Haste", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/5/57/Deadly_Haste_%28large%29.jpg?20081212194154" };
    public static readonly Skill DeadlyParadox = new() { Id = 572, Name = "Deadly Paradox", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/a/a7/Deadly_Paradox_%28large%29.jpg?20081212194009" };
    public static readonly Skill DeadlyRiposte = new() { Id = 388, Name = "Deadly Riposte", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/2/2b/Deadly_Riposte_%28large%29.jpg?20081212210233" };
    public static readonly Skill DeathBlossom = new() { Id = 775, Name = "Death Blossom", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/a/ae/Death_Blossom_%28large%29.jpg?20081212194136" };
    public static readonly Skill DeathBlossomPvP = new() { Id = 3061, Name = "Death Blossom (PvP)", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "", IsPvP = true, AlternativeSkill = DeathBlossom };
    public static readonly Skill DeathNova = new() { Id = 104, Name = "Death Nova", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/05/Death_Nova_%28large%29.jpg?20090602140800" };
    public static readonly Skill DeathPactSignet = new() { Id = 1481, Name = "Death Pact Signet", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/9f/Death_Pact_Signet_%28large%29.jpg?20081212205544" };
    public static readonly Skill DeathPactSignetPvP = new() { Id = 2872, Name = "Death Pact Signet (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = DeathPactSignet };
    public static readonly Skill DeathsCharge = new() { Id = 952, Name = "Death's Charge", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/e/ea/Death%27s_Charge_%28large%29.jpg?20081212194441" };
    public static readonly Skill DeathsRetreat = new() { Id = 1651, Name = "Death's Retreat", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/d6/Death%27s_Retreat_%28large%29.jpg?20081212193935" };
    public static readonly Skill DeathlyChill = new() { Id = 89, Name = "Deathly Chill", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/71/Deathly_Chill_%28large%29.jpg?20081212202906" };
    public static readonly Skill DeathlySwarm = new() { Id = 105, Name = "Deathly Swarm", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/b8/Deathly_Swarm_%28large%29.jpg?20081212202721" };
    public static readonly Skill DebilitatingShot = new() { Id = 406, Name = "Debilitating Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/9/9f/Debilitating_Shot_%28large%29.jpg?20081212204439" };
    public static readonly Skill Decapitate = new() { Id = 1696, Name = "Decapitate", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/e/e3/Decapitate_%28large%29.jpg?20081212210407" };
    public static readonly Skill DeepFreeze = new() { Id = 234, Name = "Deep Freeze", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/9e/Deep_Freeze_%28large%29.jpg?20081212195639" };
    public static readonly Skill DefendersZeal = new() { Id = 1688, Name = "Defender's Zeal", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/b1/Defender%27s_Zeal_%28large%29.jpg?20081212202634" };
    public static readonly Skill DefensiveAnthem = new() { Id = 1555, Name = "Defensive Anthem", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/f/f9/Defensive_Anthem_%28large%29.jpg?20081212203702" };
    public static readonly Skill DefensiveAnthemPvP = new() { Id = 2876, Name = "Defensive Anthem (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = DefensiveAnthem };
    public static readonly Skill DefensiveStance = new() { Id = 345, Name = "Defensive Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/2/27/Defensive_Stance_%28large%29.jpg?20081212210839" };
    public static readonly Skill DefiantWasXinrae = new() { Id = 812, Name = "Defiant Was Xinrae", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/4d/Defiant_Was_Xinrae_%28large%29.jpg?20081212205910" };
    public static readonly Skill DefileDefenses = new() { Id = 2188, Name = "Defile Defenses", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/fb/Defile_Defenses_%28large%29.jpg?20081212203649" };
    public static readonly Skill DefileEnchantments = new() { Id = 1070, Name = "Defile Enchantments", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/57/Defile_Enchantments_%28large%29.jpg?20081212203323" };
    public static readonly Skill DefileFlesh = new() { Id = 129, Name = "Defile Flesh", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/1/18/Defile_Flesh_%28large%29.jpg?20081212203458" };
    public static readonly Skill DeflectArrows = new() { Id = 373, Name = "Deflect Arrows", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/8/8d/Deflect_Arrows_%28large%29.jpg?20081212210009" };
    public static readonly Skill DeftStrike = new() { Id = 2228, Name = "Deft Strike", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/6/62/Deft_Strike.jpg" };
    public static readonly Skill DefyPain = new() { Id = 318, Name = "Defy Pain", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/8/89/Defy_Pain_%28large%29.jpg?20081212210827" };
    public static readonly Skill DefyPainPvP = new() { Id = 3204, Name = "Defy Pain (PvP)", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "", IsPvP = true, AlternativeSkill = DefyPain };
    public static readonly Skill DemonicFlesh = new() { Id = 130, Name = "Demonic Flesh", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/98/Demonic_Flesh_%28large%29.jpg?20081212203342" };
    public static readonly Skill DenyHexes = new() { Id = 991, Name = "Deny Hexes", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/a/ac/Deny_Hexes_%28large%29.jpg?20081212202655" };
    public static readonly Skill Depravity = new() { Id = 820, Name = "Depravity", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/40/Depravity_%28large%29.jpg?20081212203352" };
    public static readonly Skill DesecrateEnchantments = new() { Id = 112, Name = "Desecrate Enchantments", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/39/Desecrate_Enchantments_%28large%29.jpg?20081212203001" };
    public static readonly Skill DesperateStrike = new() { Id = 948, Name = "Desperate Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/f3/Desperate_Strike_%28large%29.jpg?20081212194235" };
    public static readonly Skill DesperationBlow = new() { Id = 323, Name = "Desperation Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/1f/Desperation_Blow_%28large%29.jpg?20081212211027" };
    public static readonly Skill Destruction = new() { Id = 920, Name = "Destruction", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/07/Destruction_%28large%29.jpg?20081212205723" };
    public static readonly Skill DestructionPvP = new() { Id = 3008, Name = "Destruction (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Destruction };
    public static readonly Skill DestructiveWasGlaive = new() { Id = 1732, Name = "Destructive Was Glaive", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/e9/Destructive_Was_Glaive_%28large%29.jpg?20081212205248" };
    public static readonly Skill DestructiveWasGlaivePvP = new() { Id = 3157, Name = "Destructive Was Glaive (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = DestructiveWasGlaive };
    public static readonly Skill DeterminedShot = new() { Id = 402, Name = "Determined Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/31/Determined_Shot_%28large%29.jpg?20081212204747" };
    public static readonly Skill DevastatingHammer = new() { Id = 355, Name = "Devastating Hammer", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/5/55/Devastating_Hammer_%28large%29.jpg?20081212210901" };
    public static readonly Skill Disarm = new() { Id = 2066, Name = "Disarm", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/09/Disarm_%28large%29.jpg?20081212211104" };
    public static readonly Skill DischargeEnchantment = new() { Id = 1347, Name = "Discharge Enchantment", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/0/0e/Discharge_Enchantment_%28large%29.jpg?20081212200142" };
    public static readonly Skill DisciplinedStance = new() { Id = 376, Name = "Disciplined Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/c/c9/Disciplined_Stance_%28large%29.jpg?20081212210624" };
    public static readonly Skill Discord = new() { Id = 817, Name = "Discord", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/2/2c/Discord_%28large%29.jpg?20081212203027" };
    public static readonly Skill DiscordPvP = new() { Id = 2863, Name = "Discord (PvP)", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "", IsPvP = true, AlternativeSkill = Discord };
    public static readonly Skill Disenchantment = new() { Id = 923, Name = "Disenchantment", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/7/79/Disenchantment_%28large%29.jpg?20081212205444" };
    public static readonly Skill DisenchantmentPvP = new() { Id = 3017, Name = "Disenchantment (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Disenchantment };
    public static readonly Skill Dismember = new() { Id = 337, Name = "Dismember", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/b/bd/Dismember_%28large%29.jpg?20081212210139" };
    public static readonly Skill DismissCondition = new() { Id = 1691, Name = "Dismiss Condition", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/ea/Dismiss_Condition_%28large%29.jpg?20081212202326" };
    public static readonly Skill Displacement = new() { Id = 1249, Name = "Displacement", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/7/7f/Displacement_%28large%29.jpg?20081212205306" };
    public static readonly Skill DisplacementPvP = new() { Id = 3010, Name = "Displacement (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Displacement };
    public static readonly Skill DisruptingAccuracy = new() { Id = 1723, Name = "Disrupting Accuracy", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/4/4c/Disrupting_Accuracy_%28large%29.jpg?20081212204848" };
    public static readonly Skill DisruptingChop = new() { Id = 340, Name = "Disrupting Chop", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/4/45/Disrupting_Chop_%28large%29.jpg?20081212210239" };
    public static readonly Skill DisruptingDagger = new() { Id = 571, Name = "Disrupting Dagger", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/b6/Disrupting_Dagger_%28large%29.jpg?20081212194158" };
    public static readonly Skill DisruptingLunge = new() { Id = 445, Name = "Disrupting Lunge", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/b/b2/Disrupting_Lunge_%28large%29.jpg?20081212204841" };
    public static readonly Skill DisruptingShot = new() { Id = 2143, Name = "Disrupting Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/7d/Disrupting_Shot_%28large%29.jpg?20081212204601" };
    public static readonly Skill DisruptingStab = new() { Id = 1025, Name = "Disrupting Stab", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/5/5c/Disrupting_Stab_%28large%29.jpg?20081212193958" };
    public static readonly Skill DisruptingThrow = new() { Id = 1604, Name = "Disrupting Throw", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/9/90/Disrupting_Throw_%28large%29.jpg?20081212203723" };
    public static readonly Skill Dissonance = new() { Id = 921, Name = "Dissonance", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/9e/Dissonance_%28large%29.jpg?20081212205814" };
    public static readonly Skill DissonancePvP = new() { Id = 3014, Name = "Dissonance (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Dissonance };
    public static readonly Skill Distortion = new() { Id = 11, Name = "Distortion", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/78/Distortion_%28large%29.jpg?20081212201153" };
    public static readonly Skill DistractingBlow = new() { Id = 325, Name = "Distracting Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/66/Distracting_Blow_%28large%29.jpg?20081212210153" };
    public static readonly Skill DistractingShot = new() { Id = 399, Name = "Distracting Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/57/Distracting_Shot_%28large%29.jpg?20081212204615" };
    public static readonly Skill DistractingStrike = new() { Id = 2194, Name = "Distracting Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/a7/Distracting_Strike_%28large%29.jpg?20081212210818" };
    public static readonly Skill Diversion = new() { Id = 30, Name = "Diversion", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/1e/Diversion_%28large%29.jpg?20081212201642" };
    public static readonly Skill DivertHexes = new() { Id = 1692, Name = "Divert Hexes", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/7/74/Divert_Hexes_%28large%29.jpg?20081212202650" };
    public static readonly Skill DivineBoon = new() { Id = 284, Name = "Divine Boon", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/3c/Divine_Boon_%28large%29.jpg?20081212202329" };
    public static readonly Skill DivineHealing = new() { Id = 279, Name = "Divine Healing", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/08/Divine_Healing_%28large%29.jpg?20081212202149" };
    public static readonly Skill DivineIntervention = new() { Id = 246, Name = "Divine Intervention", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/a/af/Divine_Intervention_%28large%29.jpg?20081212202122" };
    public static readonly Skill DivineSpirit = new() { Id = 310, Name = "Divine Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/f8/Divine_Spirit_%28large%29.jpg?20081212202653" };
    public static readonly Skill Dodge = new() { Id = 425, Name = "Dodge", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/b/be/Dodge_%28large%29.jpg?20081225000412" };
    public static readonly Skill DodgeThis = new() { Id = 2354, Name = "Dodge This!", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/4/4b/%22Dodge_This%21%22.jpg" };
    public static readonly Skill DolyakSignet = new() { Id = 361, Name = "Dolyak Signet", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/b/b1/Dolyak_Signet_%28large%29.jpg?20081212210605" };
    public static readonly Skill DontTrip = new() { Id = 2216, Name = "Don't Trip!", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/c/c1/%22Don%27t_Trip%21%22.jpg" };
    public static readonly Skill Doom = new() { Id = 1264, Name = "Doom", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/07/Doom_%28large%29.jpg?20081212205255" };
    public static readonly Skill DoubleDragon = new() { Id = 1091, Name = "Double Dragon", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/c8/Double_Dragon_%28large%29.jpg?20081212195207" };
    public static readonly Skill DragonSlash = new() { Id = 907, Name = "Dragon Slash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/4/49/Dragon_Slash_%28large%29.jpg?20081212211059" };
    public static readonly Skill DragonsStomp = new() { Id = 1086, Name = "Dragon's Stomp", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/e/e8/Dragon%27s_Stomp_%28large%29.jpg?20081212195349" };
    public static readonly Skill DrainDelusions = new() { Id = 1337, Name = "Drain Delusions", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/8/80/Drain_Delusions_%28large%29.jpg?20081212195938" };
    public static readonly Skill DrainEnchantment = new() { Id = 68, Name = "Drain Enchantment", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/8/80/Drain_Enchantment_%28large%29.jpg?20081212195942" };
    public static readonly Skill DrawConditions = new() { Id = 311, Name = "Draw Conditions", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/f3/Draw_Conditions_%28large%29.jpg?20090530234158" };
    public static readonly Skill DrawSpirit = new() { Id = 1224, Name = "Draw Spirit", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/2/2c/Draw_Spirit_%28large%29.jpg?20081212205749" };
    public static readonly Skill DrunkenBlow = new() { Id = 1133, Name = "Drunken Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/16/Drunken_Blow_%28large%29.jpg?20081212210252" };
    public static readonly Skill DrunkenMaster = new() { Id = 2218, Name = "Drunken Master", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/b/b3/Drunken_Master.jpg" };
    public static readonly Skill DrydersDefenses = new() { Id = 452, Name = "Dryder's Defenses", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/70/Dryder%27s_Defenses_%28large%29.jpg?20081212204559" };
    public static readonly Skill DualShot = new() { Id = 396, Name = "Dual Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/7f/Dual_Shot_%28large%29.jpg?20081212204711" };
    public static readonly Skill DulledWeapon = new() { Id = 1235, Name = "Dulled Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/b/b1/Dulled_Weapon_%28large%29.jpg?20081212205319" };
    public static readonly Skill DustCloak = new() { Id = 1497, Name = "Dust Cloak", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/0/03/Dust_Cloak_%28large%29.jpg?20081212194852" };
    public static readonly Skill DustCloakPvP = new() { Id = 3347, Name = "Dust Cloak (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = DustCloak };
    public static readonly Skill DustTrap = new() { Id = 457, Name = "Dust Trap", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/0/0a/Dust_Trap_%28large%29.jpg?20081212205048" };
    public static readonly Skill DwarvenBattleStance = new() { Id = 375, Name = "Dwarven Battle Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/6d/Dwarven_Battle_Stance_%28large%29.jpg?20081212210555" };
    public static readonly Skill DwarvenStability = new() { Id = 2423, Name = "Dwarven Stability", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/4/4c/Dwarven_Stability.jpg" };
    public static readonly Skill DwaynasKiss = new() { Id = 283, Name = "Dwayna's Kiss", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/f8/Dwayna%27s_Kiss_%28large%29.jpg?20081212202645" };
    public static readonly Skill DwaynasSorrow = new() { Id = 838, Name = "Dwayna's Sorrow", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/6/61/Dwayna%27s_Sorrow_%28large%29.jpg?20081212201816" };
    public static readonly Skill DwaynasTouch = new() { Id = 1528, Name = "Dwayna's Touch", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/54/Dwayna%27s_Touch_%28large%29.jpg?20081212194624" };
    public static readonly Skill EarBite = new() { Id = 2213, Name = "Ear Bite", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/c/c6/Ear_Bite.jpg" };
    public static readonly Skill EarthAttunement = new() { Id = 169, Name = "Earth Attunement", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/2a/Earth_Attunement_%28large%29.jpg?20081212195216" };
    public static readonly Skill EarthShaker = new() { Id = 354, Name = "Earth Shaker", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/b/bf/Earth_Shaker_%28large%29.jpg?20081212210749" };
    public static readonly Skill Earthbind = new() { Id = 1252, Name = "Earthbind", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/e3/Earthbind_%28large%29.jpg?20081212205943" };
    public static readonly Skill EarthbindPvP = new() { Id = 3015, Name = "Earthbind (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Earthbind };
    public static readonly Skill EarthenShackles = new() { Id = 2000, Name = "Earthen Shackles", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/2e/Earthen_Shackles_%28large%29.jpg?20081212195657" };
    public static readonly Skill Earthquake = new() { Id = 170, Name = "Earthquake", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/f/f4/Earthquake_%28large%29.jpg?20090601025915" };
    public static readonly Skill EbonBattleStandardofCourage = new() { Id = 2231, Name = "Ebon Battle Standard of Courage", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/5/53/Ebon_Battle_Standard_of_Courage.jpg" };
    public static readonly Skill EbonBattleStandardofHonor = new() { Id = 2233, Name = "Ebon Battle Standard of Honor", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/5/51/Ebon_Battle_Standard_of_Honor.jpg" };
    public static readonly Skill EbonBattleStandardofWisdom = new() { Id = 2232, Name = "Ebon Battle Standard of Wisdom", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/e/eb/Ebon_Battle_Standard_of_Wisdom.jpg" };
    public static readonly Skill EbonDustAura = new() { Id = 1760, Name = "Ebon Dust Aura", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/b/b5/Ebon_Dust_Aura_%28large%29.jpg?20081212194751" };
    public static readonly Skill EbonEscape = new() { Id = 2420, Name = "Ebon Escape", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/b/bb/Ebon_Escape.jpg" };
    public static readonly Skill EbonHawk = new() { Id = 1374, Name = "Ebon Hawk", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/44/Ebon_Hawk_%28large%29.jpg?20081212195337" };
    public static readonly Skill EbonVanguardAssassinSupport = new() { Id = 2235, Name = "Ebon Vanguard Assassin Support", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/0/03/Ebon_Vanguard_Assassin_Support.jpg" };
    public static readonly Skill EbonVanguardSniperSupport = new() { Id = 2234, Name = "Ebon Vanguard Sniper Support", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/1/16/Ebon_Vanguard_Sniper_Support.jpg" };
    public static readonly Skill Echo = new() { Id = 74, Name = "Echo", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/12/Echo_%28large%29.jpg?20081212201430" };
    public static readonly Skill EdgeofExtinction = new() { Id = 464, Name = "Edge of Extinction", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/f9/Edge_of_Extinction_%28large%29.jpg?20081212204245" };
    public static readonly Skill ElementalAttunement = new() { Id = 164, Name = "Elemental Attunement", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b2/Elemental_Attunement_%28large%29.jpg?20081212195808" };
    public static readonly Skill ElementalFlame = new() { Id = 1663, Name = "Elemental Flame", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/7/7b/Elemental_Flame_%28large%29.jpg?20081212195214" };
    public static readonly Skill ElementalFlamePvP = new() { Id = 3397, Name = "Elemental Flame (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = ElementalFlame };
    public static readonly Skill ElementalLord = new() { Id = 1951, Name = "Elemental Lord", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/27/Elemental_Lord.jpg" };
    public static readonly Skill ElementalLord2 = new() { Id = 2094, Name = "Elemental Lord", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/27/Elemental_Lord.jpg" };
    public static readonly Skill ElementalResistance = new() { Id = 72, Name = "Elemental Resistance", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/b/b4/Elemental_Resistance_%28large%29.jpg?20081212201648" };
    public static readonly Skill EmpathicRemoval = new() { Id = 1126, Name = "Empathic Removal", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/d/d4/Empathic_Removal_%28large%29.jpg?20081212202022" };
    public static readonly Skill Empathy = new() { Id = 26, Name = "Empathy", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/21/Empathy_%28large%29.jpg?20081212200749" };
    public static readonly Skill EmpathyPvP = new() { Id = 3151, Name = "Empathy (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = Empathy };
    public static readonly Skill Empowerment = new() { Id = 1747, Name = "Empowerment", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/d/d1/Empowerment_%28large%29.jpg?20081212205804" };
    public static readonly Skill EmpowermentPvP = new() { Id = 3024, Name = "Empowerment (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Empowerment };
    public static readonly Skill EnchantedHaste = new() { Id = 1541, Name = "Enchanted Haste", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/0/00/Enchanted_Haste_%28large%29.jpg?20081212194622" };
    public static readonly Skill EnchantersConundrum = new() { Id = 1345, Name = "Enchanter's Conundrum", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/d6/Enchanter%27s_Conundrum_%28large%29.jpg?20081212201518" };
    public static readonly Skill EnchantersConundrumPvP = new() { Id = 3192, Name = "Enchanter's Conundrum (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = EnchantersConundrum };
    public static readonly Skill EndurePain = new() { Id = 347, Name = "Endure Pain", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/d/d5/Endure_Pain_%28large%29.jpg?20081212211228" };
    public static readonly Skill EnduringHarmony = new() { Id = 1574, Name = "Enduring Harmony", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/bf/Enduring_Harmony_%28large%29.jpg?20081212203850" };
    public static readonly Skill EnduringToxin = new() { Id = 800, Name = "Enduring Toxin", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/0/03/Enduring_Toxin_%28large%29.jpg?20081212194315" };
    public static readonly Skill EnergeticWasLeeSa = new() { Id = 2016, Name = "Energetic Was Lee Sa", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/d/d4/Energetic_Was_Lee_Sa_%28large%29.jpg?20081212205632" };
    public static readonly Skill EnergizingChorus = new() { Id = 1569, Name = "Energizing Chorus", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/35/Energizing_Chorus_%28large%29.jpg?20081212203823" };
    public static readonly Skill EnergizingFinale = new() { Id = 1775, Name = "Energizing Finale", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/bb/Energizing_Finale_%28large%29.jpg?20081212204152" };
    public static readonly Skill EnergizingWind = new() { Id = 474, Name = "Energizing Wind", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/9/98/Energizing_Wind_%28large%29.jpg?20090530231703" };
    public static readonly Skill EnergyBlast = new() { Id = 2193, Name = "Energy Blast", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/52/Energy_Blast_%28large%29.jpg?20081212195221" };
    public static readonly Skill EnergyBoon = new() { Id = 837, Name = "Energy Boon", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/df/Energy_Boon_%28large%29.jpg?20081212195913" };
    public static readonly Skill EnergyBurn = new() { Id = 42, Name = "Energy Burn", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/5/54/Energy_Burn_%28large%29.jpg?20081212200753" };
    public static readonly Skill EnergyDrain = new() { Id = 79, Name = "Energy Drain", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/c6/Energy_Drain_%28large%29.jpg?20081212201639" };
    public static readonly Skill EnergySurge = new() { Id = 39, Name = "Energy Surge", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/b/b9/Energy_Surge_%28large%29.jpg?20081212201441" };
    public static readonly Skill EnergyTap = new() { Id = 80, Name = "Energy Tap", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/98/Energy_Tap_%28large%29.jpg?20081212201705" };
    public static readonly Skill EnervatingCharge = new() { Id = 224, Name = "Enervating Charge", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/ba/Enervating_Charge_%28large%29.jpg?20081212195603" };
    public static readonly Skill Enfeeble = new() { Id = 117, Name = "Enfeeble", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/58/Enfeeble_%28large%29.jpg?20081212203346" };
    public static readonly Skill EnfeeblePvP = new() { Id = 2859, Name = "Enfeeble (PvP)", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "", IsPvP = true, AlternativeSkill = Enfeeble };
    public static readonly Skill EnfeeblingBlood = new() { Id = 118, Name = "Enfeebling Blood", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/f9/Enfeebling_Blood_%28large%29.jpg?20081212203411" };
    public static readonly Skill EnfeeblingBloodPvP = new() { Id = 2885, Name = "Enfeebling Blood (PvP)", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "", IsPvP = true, AlternativeSkill = EnfeeblingBlood };
    public static readonly Skill EnfeeblingTouch = new() { Id = 1079, Name = "Enfeebling Touch", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/ee/Enfeebling_Touch_%28large%29.jpg?20081212203640" };
    public static readonly Skill EnragedLunge = new() { Id = 1202, Name = "Enraged Lunge", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/a/ab/Enraged_Lunge_%28large%29.jpg?20081212204528" };
    public static readonly Skill EnragedLungePvP = new() { Id = 3051, Name = "Enraged Lunge (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = EnragedLunge };
    public static readonly Skill EnragedSmash = new() { Id = 993, Name = "Enraged Smash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/67/Enraged_Smash_%28large%29.jpg?20081212210120" };
    public static readonly Skill EnragedSmashPvP = new() { Id = 2808, Name = "Enraged Smash (PvP)", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "", IsPvP = true, AlternativeSkill = EnragedSmash };
    public static readonly Skill EnragingCharge = new() { Id = 1414, Name = "Enraging Charge", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/8/86/Enraging_Charge_%28large%29.jpg?20081212210320" };
    public static readonly Skill EntanglingAsp = new() { Id = 784, Name = "Entangling Asp", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/3/31/Entangling_Asp_%28large%29.jpg?20081212194028" };
    public static readonly Skill EnvenomEnchantments = new() { Id = 936, Name = "Envenom Enchantments", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/04/Envenom_Enchantments_%28large%29.jpg?20081212202754" };
    public static readonly Skill Epidemic = new() { Id = 78, Name = "Epidemic", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/f/fc/Epidemic_%28large%29.jpg?20081212201308" };
    public static readonly Skill Equinox = new() { Id = 1212, Name = "Equinox", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/54/Equinox_%28large%29.jpg?20081212204903" };
    public static readonly Skill EremitesAttack = new() { Id = 1485, Name = "Eremite's Attack", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/0/0f/Eremite%27s_Attack_%28large%29.jpg?20081212194558" };
    public static readonly Skill EremitesZeal = new() { Id = 1524, Name = "Eremite's Zeal", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/f/f5/Eremite%27s_Zeal_%28large%29.jpg?20081212194916" };
    public static readonly Skill Eruption = new() { Id = 167, Name = "Eruption", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/dc/Eruption_%28large%29.jpg?20081212195928" };
    public static readonly Skill Escape = new() { Id = 448, Name = "Escape", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/db/Escape_%28large%29.jpg?20081212204714" };
    public static readonly Skill EscapePvP = new() { Id = 3060, Name = "Escape (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = Escape };
    public static readonly Skill EssenceBond = new() { Id = 250, Name = "Essence Bond", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/8b/Essence_Bond_%28large%29.jpg?20081212202114" };
    public static readonly Skill EssenceStrike = new() { Id = 1227, Name = "Essence Strike", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/d/de/Essence_Strike_%28large%29.jpg?20081212205806" };
    public static readonly Skill EternalAura = new() { Id = 2109, Name = "Eternal Aura", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/a/ab/Eternal_Aura.jpg" };
    public static readonly Skill EtherFeast = new() { Id = 40, Name = "Ether Feast", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/0/01/Ether_Feast_%28large%29.jpg?20081212200123" };
    public static readonly Skill EtherLord = new() { Id = 41, Name = "Ether Lord", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/f/fb/Ether_Lord_%28large%29.jpg?20081212201658" };
    public static readonly Skill EtherNightmare = new() { Id = 1949, Name = "Ether Nightmare", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/6/68/Ether_Nightmare.jpg" };
    public static readonly Skill EtherNightmare2 = new() { Id = 2092, Name = "Ether Nightmare", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/6/68/Ether_Nightmare.jpg" };
    public static readonly Skill EtherPhantom = new() { Id = 1343, Name = "Ether Phantom", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/cd/Ether_Phantom_%28large%29.jpg?20081212201507" };
    public static readonly Skill EtherPrism = new() { Id = 1377, Name = "Ether Prism", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/e/e8/Ether_Prism_%28large%29.jpg?20081212195608" };
    public static readonly Skill EtherProdigy = new() { Id = 178, Name = "Ether Prodigy", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/42/Ether_Prodigy_%28large%29.jpg?20081212195523" };
    public static readonly Skill EtherRenewal = new() { Id = 181, Name = "Ether Renewal", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/40/Ether_Renewal_%28large%29.jpg?20081212195752" };
    public static readonly Skill EtherRenewalPvP = new() { Id = 2860, Name = "Ether Renewal (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = EtherRenewal };
    public static readonly Skill EtherSignet = new() { Id = 881, Name = "Ether Signet", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/0/08/Ether_Signet_%28large%29.jpg?20081212200813" };
    public static readonly Skill EtherealBurden = new() { Id = 45, Name = "Ethereal Burden", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/c9/Ethereal_Burden_%28large%29.jpg?20081212200411" };
    public static readonly Skill EtherealLight = new() { Id = 959, Name = "Ethereal Light", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/38/Ethereal_Light_%28large%29.jpg?20081212202533" };
    public static readonly Skill Eviscerate = new() { Id = 338, Name = "Eviscerate", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/18/Eviscerate_%28large%29.jpg?20081212211223" };
    public static readonly Skill ExecutionersStrike = new() { Id = 336, Name = "Executioner's Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/1a/Executioner%27s_Strike_%28large%29.jpg?20081212210830" };
    public static readonly Skill ExhaustingAssault = new() { Id = 975, Name = "Exhausting Assault", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/a/a9/Exhausting_Assault_%28large%29.jpg?20081212194428" };
    public static readonly Skill ExpelHexes = new() { Id = 954, Name = "Expel Hexes", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/18/Expel_Hexes_%28large%29.jpg?20081212201126" };
    public static readonly Skill ExpertFocus = new() { Id = 2145, Name = "Expert Focus", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/84/Expert_Focus_%28large%29.jpg?20081212204434" };
    public static readonly Skill ExpertsDexterity = new() { Id = 1724, Name = "Expert's Dexterity", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/19/Expert%27s_Dexterity_%28large%29.jpg?20081212204539" };
    public static readonly Skill ExpertsDexterityPvP = new() { Id = 2959, Name = "Expert's Dexterity (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = ExpertsDexterity };
    public static readonly Skill ExplosiveGrowth = new() { Id = 1229, Name = "Explosive Growth", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/e8/Explosive_Growth_%28large%29.jpg?20081212205600" };
    public static readonly Skill ExposeDefenses = new() { Id = 802, Name = "Expose Defenses", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/8/85/Expose_Defenses_%28large%29.jpg?20081212193948" };
    public static readonly Skill ExpungeEnchantments = new() { Id = 990, Name = "Expunge Enchantments", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/1/1b/Expunge_Enchantments_%28large%29.jpg?20081212194237" };
    public static readonly Skill ExtendConditions = new() { Id = 1333, Name = "Extend Conditions", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/a3/Extend_Conditions_%28large%29.jpg?20081212200947" };
    public static readonly Skill ExtendEnchantments = new() { Id = 1508, Name = "Extend Enchantments", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/18/Extend_Enchantments_%28large%29.jpg?20081212194753" };
    public static readonly Skill Extinguish = new() { Id = 943, Name = "Extinguish", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/a/ad/Extinguish_%28large%29.jpg?20081212202144" };
    public static readonly Skill Faintheartedness = new() { Id = 135, Name = "Faintheartedness", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/c/c5/Faintheartedness_%28large%29.jpg?20081212203011" };
    public static readonly Skill FaithfulIntervention = new() { Id = 1509, Name = "Faithful Intervention", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/7/74/Faithful_Intervention_%28large%29.jpg?20081212194614" };
    public static readonly Skill FallBack = new() { Id = 1595, Name = "Fall Back!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/9/9f/%22Fall_Back%21%22_%28large%29.jpg" };
    public static readonly Skill FallBackPvP = new() { Id = 3037, Name = "Fall Back! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = FallBack };
    public static readonly Skill FallingLotusStrike = new() { Id = 1990, Name = "Falling Lotus Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/26/Falling_Lotus_Strike_%28large%29.jpg?20081212194019" };
    public static readonly Skill FallingSpider = new() { Id = 778, Name = "Falling Spider", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/1/1b/Falling_Spider_%28large%29.jpg?20081212194258" };
    public static readonly Skill Famine = new() { Id = 997, Name = "Famine", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/0/0b/Famine_%28large%29.jpg?20081212204845" };
    public static readonly Skill FarmersScythe = new() { Id = 2015, Name = "Farmer's Scythe", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/c/c1/Farmer%27s_Scythe_%28large%29.jpg?20081212194841" };
    public static readonly Skill FavorableWinds = new() { Id = 472, Name = "Favorable Winds", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/f7/Favorable_Winds_%28large%29.jpg?20081212205045" };
    public static readonly Skill FearMe = new() { Id = 366, Name = "Fear Me!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/9/90/Fear_Me%21_%28large%29.jpg?20081212210611" };
    public static readonly Skill FeastfortheDead = new() { Id = 1354, Name = "Feast for the Dead", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/49/Feast_for_the_Dead_%28large%29.jpg?20081212203414" };
    public static readonly Skill FeastofCorruption = new() { Id = 151, Name = "Feast of Corruption", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/68/Feast_of_Corruption_%28large%29.jpg?20081212203033" };
    public static readonly Skill FeastofSouls = new() { Id = 980, Name = "Feast of Souls", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/3/3c/Feast_of_Souls_%28large%29.jpg?20081212205858" };
    public static readonly Skill FeatherfootGrace = new() { Id = 1766, Name = "Featherfoot Grace", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/15/Featherfoot_Grace_%28large%29.jpg?20081212194902" };
    public static readonly Skill Feedback = new() { Id = 1061, Name = "Feedback", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/2a/Feedback_%28large%29.jpg?20081212201311" };
    public static readonly Skill FeelNoPain = new() { Id = 2360, Name = "Feel No Pain", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/f/fe/Feel_No_Pain.jpg" };
    public static readonly Skill FeignedNeutrality = new() { Id = 1641, Name = "Feigned Neutrality", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/fc/Feigned_Neutrality_%28large%29.jpg?20081212194534" };
    public static readonly Skill FeralAggression = new() { Id = 2142, Name = "Feral Aggression", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/74/Feral_Aggression_%28large%29.jpg?20081212204536" };
    public static readonly Skill FeralLunge = new() { Id = 439, Name = "Feral Lunge", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/fd/Feral_Lunge_%28large%29.jpg?20081212205131" };
    public static readonly Skill FerociousStrike = new() { Id = 442, Name = "Ferocious Strike", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/74/Ferocious_Strike_%28large%29.jpg?20081212205054" };
    public static readonly Skill FertileSeason = new() { Id = 467, Name = "Fertile Season", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/6/68/Fertile_Season_%28large%29.jpg?20081212205025" };
    public static readonly Skill FetidGround = new() { Id = 841, Name = "Fetid Ground", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/0c/Fetid_Ground_%28large%29.jpg?20081212203527" };
    public static readonly Skill FeveredDreams = new() { Id = 55, Name = "Fevered Dreams", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/c4/Fevered_Dreams_%28large%29.jpg?20081212200028" };
    public static readonly Skill FeveredDreamsPvP = new() { Id = 3289, Name = "Fevered Dreams (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = FeveredDreams };
    public static readonly Skill FierceBlow = new() { Id = 850, Name = "Fierce Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/03/Fierce_Blow_%28large%29.jpg?20081212211237" };
    public static readonly Skill FinalThrust = new() { Id = 385, Name = "Final Thrust", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/9/96/Final_Thrust_%28large%29.jpg?20081212210052" };
    public static readonly Skill FinaleofRestoration = new() { Id = 1577, Name = "Finale of Restoration", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/0/0b/Finale_of_Restoration_%28large%29.jpg?20081212204046" };
    public static readonly Skill FinaleofRestorationPvP = new() { Id = 3062, Name = "Finale of Restoration (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = FinaleofRestoration };
    public static readonly Skill FindTheirWeakness = new() { Id = 1781, Name = "Find Their Weakness!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/e7/%22Find_Their_Weakness%21%22_%28large%29.jpg" };
    public static readonly Skill FindTheirWeaknessPvP = new() { Id = 3034, Name = "Find Their Weakness! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = FindTheirWeakness };
    public static readonly Skill FinishHim = new() { Id = 2353, Name = "Finish Him!", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/6/61/%22Finish_Him%21%22.jpg" };
    public static readonly Skill FireAttunement = new() { Id = 184, Name = "Fire Attunement", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b6/Fire_Attunement_%28large%29.jpg?20081212195910" };
    public static readonly Skill FireStorm = new() { Id = 197, Name = "Fire Storm", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/83/Fire_Storm_%28large%29.jpg?20081212195611" };
    public static readonly Skill Fireball = new() { Id = 186, Name = "Fireball", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a1/Fireball_%28large%29.jpg?20081212195325" };
    public static readonly Skill Flail = new() { Id = 1404, Name = "Flail", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/f/f2/Flail_%28large%29.jpg?20081212210342" };
    public static readonly Skill FlameBurst = new() { Id = 188, Name = "Flame Burst", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/6/67/Flame_Burst_%28large%29.jpg?20090530230756" };
    public static readonly Skill FlameDjinnsHaste = new() { Id = 1381, Name = "Flame Djinn's Haste", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/1/1f/Flame_Djinn%27s_Haste_%28large%29.jpg?20081212195209" };
    public static readonly Skill FlameTrap = new() { Id = 459, Name = "Flame Trap", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/4/49/Flame_Trap_%28large%29.jpg?20081212204838" };
    public static readonly Skill Flare = new() { Id = 194, Name = "Flare", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/1/13/Flare_%28large%29.jpg?20081212195650" };
    public static readonly Skill FlashingBlades = new() { Id = 1042, Name = "Flashing Blades", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/9/9d/Flashing_Blades_%28large%29.jpg?20081212194539" };
    public static readonly Skill FleetingStability = new() { Id = 1514, Name = "Fleeting Stability", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/b/b9/Fleeting_Stability_%28large%29.jpg?20081212194912" };
    public static readonly Skill FleshofMyFlesh = new() { Id = 791, Name = "Flesh of My Flesh", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/2/28/Flesh_of_My_Flesh_%28large%29.jpg?20081212205725" };
    public static readonly Skill FleshofMyFleshPvP = new() { Id = 2866, Name = "Flesh of My Flesh (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = FleshofMyFlesh };
    public static readonly Skill Flourish = new() { Id = 389, Name = "Flourish", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/9/94/Flourish_%28large%29.jpg?20081212210244" };
    public static readonly Skill Flurry = new() { Id = 344, Name = "Flurry", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/30/Flurry_%28large%29.jpg?20081212210644" };
    public static readonly Skill FocusedAnger = new() { Id = 1769, Name = "Focused Anger", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/a/a0/Focused_Anger_%28large%29.jpg?20081212204029" };
    public static readonly Skill FocusedShot = new() { Id = 909, Name = "Focused Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/8b/Focused_Shot_%28large%29.jpg?20081212205215" };
    public static readonly Skill ForGreatJustice = new() { Id = 343, Name = "For Great Justice!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/b/bc/%22For_Great_Justice%21%22_%28large%29.jpg" };
    public static readonly Skill ForGreatJusticePvP = new() { Id = 2883, Name = "For Great Justice! (PvP)", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "", IsPvP = true, AlternativeSkill = ForGreatJustice };
    public static readonly Skill ForcefulBlow = new() { Id = 889, Name = "Forceful Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/64/Forceful_Blow_%28large%29.jpg?20081212210042" };
    public static readonly Skill ForkedArrow = new() { Id = 1722, Name = "Forked Arrow", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/e/eb/Forked_Arrow_%28large%29.jpg?20081212204425" };
    public static readonly Skill FoulFeast = new() { Id = 2057, Name = "Foul Feast", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/dc/Foul_Feast_%28large%29.jpg?20081212203637" };
    public static readonly Skill FoxFangs = new() { Id = 780, Name = "Fox Fangs", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/3/34/Fox_Fangs_%28large%29.jpg?20081212194017" };
    public static readonly Skill FoxFangsPvP = new() { Id = 3251, Name = "Fox Fangs (PvP)", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "", IsPvP = true, AlternativeSkill = FoxFangs };
    public static readonly Skill FoxsPromise = new() { Id = 1640, Name = "Fox's Promise", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/d7/Fox%27s_Promise_%28large%29.jpg?20081212194455" };
    public static readonly Skill Fragility = new() { Id = 19, Name = "Fragility", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/91/Fragility_%28large%29.jpg?20081212201456" };
    public static readonly Skill FragilityPvP = new() { Id = 2998, Name = "Fragility (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = Fragility };
    public static readonly Skill FreezingGust = new() { Id = 1382, Name = "Freezing Gust", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b7/Freezing_Gust_%28large%29.jpg?20081212195648" };
    public static readonly Skill FrenziedDefense = new() { Id = 1700, Name = "Frenzied Defense", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/0a/Frenzied_Defense_%28large%29.jpg?20081212211042" };
    public static readonly Skill Frenzy = new() { Id = 346, Name = "Frenzy", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/e/e0/Frenzy_%28large%29.jpg?20081212210226" };
    public static readonly Skill FrigidArmor = new() { Id = 1261, Name = "Frigid Armor", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/20/Frigid_Armor_%28large%29.jpg?20081212195654" };
    public static readonly Skill FrozenBurst = new() { Id = 212, Name = "Frozen Burst", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/e/e1/Frozen_Burst_%28large%29.jpg?20081212195135" };
    public static readonly Skill FrozenSoil = new() { Id = 471, Name = "Frozen Soil", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/1b/Frozen_Soil_%28large%29.jpg?20081212204330" };
    public static readonly Skill Frustration = new() { Id = 1341, Name = "Frustration", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/4/48/Frustration_%28large%29.jpg?20081212200722" };
    public static readonly Skill FrustrationPvP = new() { Id = 3190, Name = "Frustration (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = Frustration };
    public static readonly Skill FuriousAxe = new() { Id = 904, Name = "Furious Axe", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/d/de/Furious_Axe_%28large%29.jpg?20081212210746" };
    public static readonly Skill Gale = new() { Id = 162, Name = "Gale", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/58/Gale_%28large%29.jpg?20081212195754" };
    public static readonly Skill GalrathSlash = new() { Id = 383, Name = "Galrath Slash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/aa/Galrath_Slash_%28large%29.jpg?20090626203636" };
    public static readonly Skill Gash = new() { Id = 384, Name = "Gash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/6c/Gash_%28large%29.jpg?20081212210656" };
    public static readonly Skill GazefromBeyond = new() { Id = 1245, Name = "Gaze from Beyond", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/2/29/Gaze_from_Beyond_%28large%29.jpg?20081212205737" };
    public static readonly Skill GazeofContempt = new() { Id = 766, Name = "Gaze of Contempt", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/8f/Gaze_of_Contempt_%28large%29.jpg?20081212203455" };
    public static readonly Skill GazeofFury = new() { Id = 1734, Name = "Gaze of Fury", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/0a/Gaze_of_Fury_%28large%29.jpg?20081212205430" };
    public static readonly Skill GazeofFuryPvP = new() { Id = 3022, Name = "Gaze of Fury (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = GazeofFury };
    public static readonly Skill GenerousWasTsungrai = new() { Id = 772, Name = "Generous Was Tsungrai", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/2/24/Generous_Was_Tsungrai_%28large%29.jpg?20081212205728" };
    public static readonly Skill GhostlyHaste = new() { Id = 1244, Name = "Ghostly Haste", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/05/Ghostly_Haste_%28large%29.jpg?20081212205301" };
    public static readonly Skill GhostlyWeapon = new() { Id = 2206, Name = "Ghostly Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/a/a1/Ghostly_Weapon_%28large%29.jpg?20081212205541" };
    public static readonly Skill GhostmirrorLight = new() { Id = 1741, Name = "Ghostmirror Light", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/0d/Ghostmirror_Light_%28large%29.jpg?20081212205917" };
    public static readonly Skill GiftofHealth = new() { Id = 1121, Name = "Gift of Health", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/0a/Gift_of_Health_%28large%29.jpg?20081212202302" };
    public static readonly Skill GladiatorsDefense = new() { Id = 372, Name = "Gladiator's Defense", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/0e/Gladiator%27s_Defense_%28large%29.jpg?20081212210254" };
    public static readonly Skill GlassArrows = new() { Id = 1199, Name = "Glass Arrows", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/81/Glass_Arrows_%28large%29.jpg?20081212204749" };
    public static readonly Skill GlassArrowsPvP = new() { Id = 3145, Name = "Glass Arrows (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = GlassArrows };
    public static readonly Skill GlimmerofLight = new() { Id = 1686, Name = "Glimmer of Light", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/d/dd/Glimmer_of_Light_%28large%29.jpg?20081212202127" };
    public static readonly Skill GlimmeringMark = new() { Id = 227, Name = "Glimmering Mark", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a4/Glimmering_Mark_%28large%29.jpg?20081212195503" };
    public static readonly Skill GlowingGaze = new() { Id = 1379, Name = "Glowing Gaze", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/20/Glowing_Gaze_%28large%29.jpg?20081212195741" };
    public static readonly Skill GlowingIce = new() { Id = 2192, Name = "Glowing Ice", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/23/Glowing_Ice_%28large%29.jpg?20081212195049" };
    public static readonly Skill GlowingSignet = new() { Id = 1581, Name = "Glowing Signet", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/e1/Glowing_Signet_%28large%29.jpg?20081212203714" };
    public static readonly Skill Glowstone = new() { Id = 1661, Name = "Glowstone", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/9f/Glowstone_%28large%29.jpg?20081212195630" };
    public static readonly Skill GlyphofConcentration = new() { Id = 201, Name = "Glyph of Concentration", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/4e/Glyph_of_Concentration_%28large%29.jpg?20081212195620" };
    public static readonly Skill GlyphofElementalPower = new() { Id = 198, Name = "Glyph of Elemental Power", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/7/75/Glyph_of_Elemental_Power_%28large%29.jpg?20081212195150" };
    public static readonly Skill GlyphofEnergy = new() { Id = 199, Name = "Glyph of Energy", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/95/Glyph_of_Energy_%28large%29.jpg?20081212195518" };
    public static readonly Skill GlyphofEssence = new() { Id = 1096, Name = "Glyph of Essence", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/f/f0/Glyph_of_Essence_%28large%29.jpg?20081212195436" };
    public static readonly Skill GlyphofImmolation = new() { Id = 2060, Name = "Glyph of Immolation", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/2e/Glyph_of_Immolation_%28large%29.jpg?20081212195358" };
    public static readonly Skill GlyphofLesserEnergy = new() { Id = 200, Name = "Glyph of Lesser Energy", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/d5/Glyph_of_Lesser_Energy_%28large%29.jpg?20081212195901" };
    public static readonly Skill GlyphofRenewal = new() { Id = 203, Name = "Glyph of Renewal", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/8e/Glyph_of_Renewal_%28large%29.jpg?20081212195736" };
    public static readonly Skill GlyphofRestoration = new() { Id = 1376, Name = "Glyph of Restoration", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/0/0b/Glyph_of_Restoration_%28large%29.jpg?20081212195347" };
    public static readonly Skill GlyphofSacrifice = new() { Id = 202, Name = "Glyph of Sacrifice", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/db/Glyph_of_Sacrifice_%28large%29.jpg?20081212195904" };
    public static readonly Skill GlyphofSwiftness = new() { Id = 2002, Name = "Glyph of Swiftness", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/41/Glyph_of_Swiftness_%28large%29.jpg?20081212195634" };
    public static readonly Skill GofortheEyes = new() { Id = 1558, Name = "Go for the Eyes!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/c/c4/%22Go_for_the_Eyes%21%22_%28large%29.jpg" };
    public static readonly Skill GofortheEyesPvP = new() { Id = 3026, Name = "Go for the Eyes! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = GofortheEyes };
    public static readonly Skill Godspeed = new() { Id = 1556, Name = "Godspeed", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/57/Godspeed_%28large%29.jpg?20081212204035" };
    public static readonly Skill GoldenFangStrike = new() { Id = 1988, Name = "Golden Fang Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/df/Golden_Fang_Strike_%28large%29.jpg?20081212194200" };
    public static readonly Skill GoldenFoxStrike = new() { Id = 1637, Name = "Golden Fox Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/1/1f/Golden_Fox_Strike_%28large%29.jpg?20081212194128" };
    public static readonly Skill GoldenLotusStrike = new() { Id = 1026, Name = "Golden Lotus Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/cd/Golden_Lotus_Strike_%28large%29.jpg?20081212194453" };
    public static readonly Skill GoldenPhoenixStrike = new() { Id = 989, Name = "Golden Phoenix Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/c9/Golden_Phoenix_Strike_%28large%29.jpg?20081212194147" };
    public static readonly Skill GoldenSkullStrike = new() { Id = 1635, Name = "Golden Skull Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/9/94/Golden_Skull_Strike_%28large%29.jpg?20081212193939" };
    public static readonly Skill Grapple = new() { Id = 2011, Name = "Grapple", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/7/75/Grapple_%28large%29.jpg?20081212210332" };
    public static readonly Skill GraspingEarth = new() { Id = 173, Name = "Grasping Earth", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/ae/Grasping_Earth_%28large%29.jpg?20081212195817" };
    public static readonly Skill GraspingWasKuurong = new() { Id = 789, Name = "Grasping Was Kuurong", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/4a/Grasping_Was_Kuurong_%28large%29.jpg?20081212205538" };
    public static readonly Skill GreatDwarfArmor = new() { Id = 2220, Name = "Great Dwarf Armor", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/e/e5/Great_Dwarf_Armor.jpg" };
    public static readonly Skill GreatDwarfWeapon = new() { Id = 2219, Name = "Great Dwarf Weapon", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/a/ab/Great_Dwarf_Weapon.jpg" };
    public static readonly Skill GreaterConflagration = new() { Id = 465, Name = "Greater Conflagration", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/3a/Greater_Conflagration_%28large%29.jpg?20081212205019" };
    public static readonly Skill GrenthsAura = new() { Id = 2013, Name = "Grenth's Aura", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/f/fb/Grenth%27s_Aura_%28large%29.jpg?20081212195027" };
    public static readonly Skill GrenthsBalance = new() { Id = 86, Name = "Grenth's Balance", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/03/Grenth%27s_Balance_%28large%29.jpg?20081212203004" };
    public static readonly Skill GrenthsFingers = new() { Id = 1493, Name = "Grenth's Fingers", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/c/ca/Grenth%27s_Fingers_%28large%29.jpg?20081212194657" };
    public static readonly Skill GrenthsGrasp = new() { Id = 1756, Name = "Grenth's Grasp", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/3/3e/Grenth%27s_Grasp_%28large%29.jpg?20081212194727" };
    public static readonly Skill GriffonsSweep = new() { Id = 327, Name = "Griffon's Sweep", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/9/94/Griffon%27s_Sweep_%28large%29.jpg?20081212211005" };
    public static readonly Skill Guardian = new() { Id = 258, Name = "Guardian", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/a/ae/Guardian_%28large%29.jpg?20081212202035" };
    public static readonly Skill GuidedWeapon = new() { Id = 1259, Name = "Guided Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/a/a7/Guided_Weapon_%28large%29.jpg?20081212205424" };
    public static readonly Skill GuidingHands = new() { Id = 1513, Name = "Guiding Hands", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/6/68/Guiding_Hands_%28large%29.jpg?20081212194714" };
    public static readonly Skill GuidingHandsPvP = new() { Id = 3269, Name = "Guiding Hands (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = GuidingHands };
    public static readonly Skill Guilt = new() { Id = 46, Name = "Guilt", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/aa/Guilt_%28large%29.jpg?20081212200758" };
    public static readonly Skill Gust = new() { Id = 843, Name = "Gust", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/aa/Gust_%28large%29.jpg?20081212195302" };
    public static readonly Skill HammerBash = new() { Id = 331, Name = "Hammer Bash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/d/dd/Hammer_Bash_%28large%29.jpg?20081212210305" };
    public static readonly Skill Hamstring = new() { Id = 320, Name = "Hamstring", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/4/42/Hamstring_%28large%29.jpg?20090531021136" };
    public static readonly Skill HarriersGrasp = new() { Id = 1758, Name = "Harrier's Grasp", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/55/Harrier%27s_Grasp_%28large%29.jpg?20081212195041" };
    public static readonly Skill HarriersHaste = new() { Id = 1768, Name = "Harrier's Haste", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/4/46/Harrier%27s_Haste_%28large%29.jpg?20081212195016" };
    public static readonly Skill HarriersToss = new() { Id = 1549, Name = "Harrier's Toss", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/ec/Harrier%27s_Toss_%28large%29.jpg?20081212203837" };
    public static readonly Skill HarriersTossPvP = new() { Id = 2875, Name = "Harrier's Toss (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = HarriersToss };
    public static readonly Skill HastyRefrain = new() { Id = 2075, Name = "Hasty Refrain", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/58/Hasty_Refrain_%28large%29.jpg?20081212203846" };
    public static readonly Skill Headbutt = new() { Id = 1406, Name = "Headbutt", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/36/Headbutt_%28large%29.jpg?20081212211107" };
    public static readonly Skill HealArea = new() { Id = 280, Name = "Heal Area", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/85/Heal_Area_%28large%29.jpg?20090530234643" };
    public static readonly Skill HealasOne = new() { Id = 1195, Name = "Heal as One", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/50/Heal_as_One_%28large%29.jpg?20081212205123" };
    public static readonly Skill HealasOnePvP = new() { Id = 3144, Name = "Heal as One (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = HealasOne };
    public static readonly Skill HealOther = new() { Id = 286, Name = "Heal Other", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/5c/Heal_Other_%28large%29.jpg?20090530235300" };
    public static readonly Skill HealParty = new() { Id = 287, Name = "Heal Party", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/39/Heal_Party_%28large%29.jpg?20081212201851" };
    public static readonly Skill HealPartyPvP = new() { Id = 3232, Name = "Heal Party (PvP)", AlternativeName = "", Profession = Profession.Monk, IconUrl = "", IsPvP = true, AlternativeSkill = HealParty };
    public static readonly Skill HealersBoon = new() { Id = 1393, Name = "Healer's Boon", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c2/Healer%27s_Boon_%28large%29.jpg?20081212201934" };
    public static readonly Skill HealersCovenant = new() { Id = 1394, Name = "Healer's Covenant", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/9e/Healer%27s_Covenant_%28large%29.jpg?20081212202223" };
    public static readonly Skill HealingBreeze = new() { Id = 288, Name = "Healing Breeze", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/6/6b/Healing_Breeze_%28large%29.jpg?20090530235646" };
    public static readonly Skill HealingBurst = new() { Id = 1118, Name = "Healing Burst", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/e3/Healing_Burst_%28large%29.jpg?20081212202637" };
    public static readonly Skill HealingHands = new() { Id = 285, Name = "Healing Hands", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/8c/Healing_Hands_%28large%29.jpg?20081212202208" };
    public static readonly Skill HealingLight = new() { Id = 867, Name = "Healing Light", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/7/72/Healing_Light_%28large%29.jpg?20081212202025" };
    public static readonly Skill HealingRibbon = new() { Id = 2062, Name = "Healing Ribbon", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/cc/Healing_Ribbon_%28large%29.jpg?20081212202141" };
    public static readonly Skill HealingRing = new() { Id = 1262, Name = "Healing Ring", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/81/Healing_Ring_%28large%29.jpg?20081212202450" };
    public static readonly Skill HealingSeed = new() { Id = 274, Name = "Healing Seed", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/ec/Healing_Seed_%28large%29.jpg?20081212202009" };
    public static readonly Skill HealingSignet = new() { Id = 1, Name = "Healing Signet", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/2/27/Healing_Signet_%28large%29.jpg?20100404015221" };
    public static readonly Skill HealingSpring = new() { Id = 460, Name = "Healing Spring", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5b/Healing_Spring_%28large%29.jpg?20081212204703" };
    public static readonly Skill HealingTouch = new() { Id = 313, Name = "Healing Touch", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c3/Healing_Touch_%28large%29.jpg?20081212202256" };
    public static readonly Skill HealingWhisper = new() { Id = 958, Name = "Healing Whisper", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/f7/Healing_Whisper_%28large%29.jpg?20081212202459" };
    public static readonly Skill HeartofFury = new() { Id = 1762, Name = "Heart of Fury", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/e/e6/Heart_of_Fury_%28large%29.jpg?20081212194834" };
    public static readonly Skill HeartofFuryPvP = new() { Id = 3366, Name = "Heart of Fury (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = HeartofFury };
    public static readonly Skill HeartofHolyFlame = new() { Id = 1507, Name = "Heart of Holy Flame", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/c/ce/Heart_of_Holy_Flame_%28large%29.jpg?20081212195004" };
    public static readonly Skill HeartofShadow = new() { Id = 1032, Name = "Heart of Shadow", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/2d/Heart_of_Shadow_%28large%29.jpg?20081212194003" };
    public static readonly Skill HeavensDelight = new() { Id = 1117, Name = "Heaven's Delight", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/6/64/Heaven%27s_Delight_%28large%29.jpg?20081212202632" };
    public static readonly Skill HeavyBlow = new() { Id = 359, Name = "Heavy Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/e/ec/Heavy_Blow_%28large%29.jpg?20081212210049" };
    public static readonly Skill HeketsRampage = new() { Id = 1728, Name = "Heket's Rampage", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/c/c8/Heket%27s_Rampage_%28large%29.jpg?20081212204922" };
    public static readonly Skill HelpMe = new() { Id = 1594, Name = "Help Me!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/8/8b/%22Help_Me%21%22_%28large%29.jpg" };
    public static readonly Skill HelpMePvP = new() { Id = 3036, Name = "Help Me! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = HelpMe };
    public static readonly Skill HeroicRefrain = new() { Id = 3431, Name = "Heroic Refrain", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/6/6e/Heroic_Refrain.jpg" };
    public static readonly Skill HexBreaker = new() { Id = 10, Name = "Hex Breaker", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/21/Hex_Breaker_%28large%29.jpg?20081212201438" };
    public static readonly Skill HexEaterSignet = new() { Id = 1059, Name = "Hex Eater Signet", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/90/Hex_Eater_Signet_%28large%29.jpg?20081212201644" };
    public static readonly Skill HexEaterVortex = new() { Id = 1348, Name = "Hex Eater Vortex", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/98/Hex_Eater_Vortex_%28large%29.jpg?20081212201123" };
    public static readonly Skill HexbreakerAria = new() { Id = 1571, Name = "Hexbreaker Aria", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/a/a8/Hexbreaker_Aria_%28large%29.jpg?20081212203826" };
    public static readonly Skill HexersVigor = new() { Id = 2138, Name = "Hexer's Vigor", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/04/Hexer%27s_Vigor_%28large%29.jpg?20081212203307" };
    public static readonly Skill HiddenCaltrops = new() { Id = 1642, Name = "Hidden Caltrops", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/e/e6/Hidden_Caltrops_%28large%29.jpg?20081212194326" };
    public static readonly Skill HolyHaste = new() { Id = 1685, Name = "Holy Haste", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/5b/Holy_Haste_%28large%29.jpg?20081212202348" };
    public static readonly Skill HolySpear = new() { Id = 2209, Name = "Holy Spear", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/7/7b/Holy_Spear_%28large%29.jpg?20081212203900" };
    public static readonly Skill HolyStrike = new() { Id = 312, Name = "Holy Strike", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/58/Holy_Strike_%28large%29.jpg?20081212202444" };
    public static readonly Skill HolyVeil = new() { Id = 309, Name = "Holy Veil", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/f1/Holy_Veil_%28large%29.jpg?20090626051839" };
    public static readonly Skill HolyWrath = new() { Id = 249, Name = "Holy Wrath", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/b3/Holy_Wrath_%28large%29.jpg?20081212201950" };
    public static readonly Skill HornsoftheOx = new() { Id = 777, Name = "Horns of the Ox", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/a/af/Horns_of_the_Ox_%28large%29.jpg?20081212193930" };
    public static readonly Skill HundredBlades = new() { Id = 381, Name = "Hundred Blades", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/0a/Hundred_Blades_%28large%29.jpg?20081212210242" };
    public static readonly Skill HuntersShot = new() { Id = 391, Name = "Hunter's Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/33/Hunter%27s_Shot_%28large%29.jpg?20090530232128" };
    public static readonly Skill Hypochondria = new() { Id = 1334, Name = "Hypochondria", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/16/Hypochondria_%28large%29.jpg?20081212200338" };
    public static readonly Skill IAmtheStrongest = new() { Id = 2355, Name = "I Am the Strongest!", AlternativeName = "", Profession = Profession.None, IconUrl = "", IsPvP = true, AlternativeSkill = IAmtheStrongest };
    public static readonly Skill IAmUnstoppable = new() { Id = 2356, Name = "I Am Unstoppable!", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/e/ed/%22I_Am_Unstoppable%21%22.jpg" };
    public static readonly Skill IMeanttoDoThat = new() { Id = 2067, Name = "I Meant to Do That!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "", IsPvP = true, AlternativeSkill = IMeanttoDoThat };
    public static readonly Skill IWillAvengeYou = new() { Id = 333, Name = "I Will Avenge You!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/1d/I_Will_Avenge_You%21_%28large%29.jpg?20081212205956" };
    public static readonly Skill IWillSurvive = new() { Id = 368, Name = "I Will Survive!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/8/8f/I_Will_Survive%21_%28large%29.jpg?20081212211008" };
    public static readonly Skill IcePrison = new() { Id = 210, Name = "Ice Prison", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/d2/Ice_Prison_%28large%29.jpg?20090530230540" };
    public static readonly Skill IceSpear = new() { Id = 214, Name = "Ice Spear", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/c1/Ice_Spear_%28large%29.jpg?20081212195627" };
    public static readonly Skill IceSpikes = new() { Id = 211, Name = "Ice Spikes", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/7/7d/Ice_Spikes_%28large%29.jpg?20081212195319" };
    public static readonly Skill IcyPrism = new() { Id = 903, Name = "Icy Prism", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b1/Icy_Prism_%28large%29.jpg?20081212195306" };
    public static readonly Skill IcyShackles = new() { Id = 939, Name = "Icy Shackles", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/6/6c/Icy_Shackles_%28large%29.jpg?20081212195158" };
    public static readonly Skill IcyVeins = new() { Id = 821, Name = "Icy Veins", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/34/Icy_Veins_%28large%29.jpg?20081212203519" };
    public static readonly Skill IgniteArrows = new() { Id = 431, Name = "Ignite Arrows", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/52/Ignite_Arrows_%28large%29.jpg?20081212204347" };
    public static readonly Skill Ignorance = new() { Id = 35, Name = "Ignorance", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/f/fe/Ignorance_%28large%29.jpg?20081212201158" };
    public static readonly Skill IllusionofHaste = new() { Id = 37, Name = "Illusion of Haste", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/c9/Illusion_of_Haste_%28large%29.jpg?20081212201342" };
    public static readonly Skill IllusionofHastePvP = new() { Id = 3373, Name = "Illusion of Haste (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = IllusionofHaste };
    public static readonly Skill IllusionofPain = new() { Id = 879, Name = "Illusion of Pain", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/9b/Illusion_of_Pain_%28large%29.jpg?20081212201636" };
    public static readonly Skill IllusionofPainPvP = new() { Id = 3374, Name = "Illusion of Pain (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = IllusionofPain };
    public static readonly Skill IllusionofWeakness = new() { Id = 32, Name = "Illusion of Weakness", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/6/6e/Illusion_of_Weakness_%28large%29.jpg?20081212201529" };
    public static readonly Skill IllusionaryWeaponry = new() { Id = 33, Name = "Illusionary Weaponry", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/8/8a/Illusionary_Weaponry_%28large%29.jpg?20081212201318" };
    public static readonly Skill IllusionaryWeaponryPvP = new() { Id = 3181, Name = "Illusionary Weaponry (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = IllusionaryWeaponry };
    public static readonly Skill ImagesofRemorse = new() { Id = 899, Name = "Images of Remorse", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/6/65/Images_of_Remorse_%28large%29.jpg?20081212200302" };
    public static readonly Skill ImaginedBurden = new() { Id = 76, Name = "Imagined Burden", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/e/eb/Imagined_Burden_%28large%29.jpg?20081212201700" };
    public static readonly Skill ImbueHealth = new() { Id = 1526, Name = "Imbue Health", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/9/96/Imbue_Health_%28large%29.jpg?20081212194840" };
    public static readonly Skill Immolate = new() { Id = 191, Name = "Immolate", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a7/Immolate_%28large%29.jpg?20081212195800" };
    public static readonly Skill Impale = new() { Id = 1033, Name = "Impale", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/f9/Impale_%28large%29.jpg?20081212194121" };
    public static readonly Skill IncendiaryArrows = new() { Id = 428, Name = "Incendiary Arrows", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/37/Incendiary_Arrows_%28large%29.jpg?20081212205036" };
    public static readonly Skill IncendiaryBonds = new() { Id = 179, Name = "Incendiary Bonds", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/9d/Incendiary_Bonds_%28large%29.jpg?20081212195438" };
    public static readonly Skill Incoming = new() { Id = 1596, Name = "Incoming!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/50/%22Incoming%21%22_%28large%29.jpg" };
    public static readonly Skill IncomingPvP = new() { Id = 2879, Name = "Incoming! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = Incoming };
    public static readonly Skill Ineptitude = new() { Id = 47, Name = "Ineptitude", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/af/Ineptitude_%28large%29.jpg?20081212200040" };
    public static readonly Skill Inferno = new() { Id = 183, Name = "Inferno", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/1/1c/Inferno_%28large%29.jpg?20081212195446" };
    public static readonly Skill InfuriatingHeat = new() { Id = 1730, Name = "Infuriating Heat", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/a/ab/Infuriating_Heat_%28large%29.jpg?20081212204931" };
    public static readonly Skill InfuseCondition = new() { Id = 139, Name = "Infuse Condition", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/6f/Infuse_Condition_%28large%29.jpg?20081212203540" };
    public static readonly Skill InfuseHealth = new() { Id = 292, Name = "Infuse Health", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/6/6e/Infuse_Health_%28large%29.jpg?20081212201946" };
    public static readonly Skill InsidiousParasite = new() { Id = 123, Name = "Insidious Parasite", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/c/cb/Insidious_Parasite_%28large%29.jpg?20081212203537" };
    public static readonly Skill InspirationalSpeech = new() { Id = 2207, Name = "Inspirational Speech", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/c/c3/Inspirational_Speech_%28large%29.jpg?20081212204147" };
    public static readonly Skill InspiredEnchantment = new() { Id = 21, Name = "Inspired Enchantment", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/5/5b/Inspired_Enchantment_%28large%29.jpg?20081212200032" };
    public static readonly Skill InspiredHex = new() { Id = 22, Name = "Inspired Hex", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/16/Inspired_Hex_%28large%29.jpg?20081212200244" };
    public static readonly Skill Intensity = new() { Id = 2104, Name = "Intensity", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/dc/Intensity.jpg" };
    public static readonly Skill IntimidatingAura = new() { Id = 1531, Name = "Intimidating Aura", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/9/93/Intimidating_Aura_%28large%29.jpg?20081212195039" };
    public static readonly Skill InvokeLightning = new() { Id = 1664, Name = "Invoke Lightning", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/e/e5/Invoke_Lightning_%28large%29.jpg?20081212195644" };
    public static readonly Skill IronMist = new() { Id = 216, Name = "Iron Mist", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/4a/Iron_Mist_%28large%29.jpg?20081212195501" };
    public static readonly Skill IronPalm = new() { Id = 786, Name = "Iron Palm", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/7/75/Iron_Palm_%28large%29.jpg?20081212193941" };
    public static readonly Skill IrresistibleBlow = new() { Id = 356, Name = "Irresistible Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/a5/Irresistible_Blow_%28large%29.jpg?20081212211233" };
    public static readonly Skill IrresistibleSweep = new() { Id = 1489, Name = "Irresistible Sweep", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/8/84/Irresistible_Sweep_%28large%29.jpg?20081212194929" };
    public static readonly Skill IrresistibleSweepPvP = new() { Id = 3265, Name = "Irresistible Sweep (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = IrresistibleSweep };
    public static readonly Skill ItsJustaFleshWound = new () { Id = 1599, Name = "It's Just a Flesh Wound", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/6/69/%22It%27s_Just_a_Flesh_Wound.%22.jpg" };
    public static readonly Skill JaggedBones = new() { Id = 1355, Name = "Jagged Bones", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/b3/Jagged_Bones_%28large%29.jpg?20081212202951" };
    public static readonly Skill JaggedStrike = new() { Id = 782, Name = "Jagged Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/d4/Jagged_Strike_%28large%29.jpg?20081212194333" };
    public static readonly Skill JaizhenjuStrike = new() { Id = 1135, Name = "Jaizhenju Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/4/4e/Jaizhenju_Strike_%28large%29.jpg?20081212210142" };
    public static readonly Skill JameisGaze = new() { Id = 1120, Name = "Jamei's Gaze", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/1/16/Jamei%27s_Gaze_%28large%29.jpg?20081212202614" };
    public static readonly Skill JaundicedGaze = new() { Id = 763, Name = "Jaundiced Gaze", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/eb/Jaundiced_Gaze_%28large%29.jpg?20081212203358" };
    public static readonly Skill JudgesInsight = new() { Id = 267, Name = "Judge's Insight", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/5d/Judge%27s_Insight_%28large%29.jpg?20081212202643" };
    public static readonly Skill JudgesIntervention = new() { Id = 1390, Name = "Judge's Intervention", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/31/Judge%27s_Intervention_%28large%29.jpg?20081212202135" };
    public static readonly Skill JudgementStrike = new() { Id = 3425, Name = "Judgement Strike", AlternativeName = "Judgment Strike", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/6/63/Judgment_Strike.jpg" };
    public static readonly Skill JungleStrike = new() { Id = 1021, Name = "Jungle Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/cd/Jungle_Strike_%28large%29.jpg?20081212194415" };
    public static readonly Skill KareisHealingCircle = new() { Id = 1119, Name = "Karei's Healing Circle", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/7/74/Karei%27s_Healing_Circle_%28large%29.jpg?20081212202536" };
    public static readonly Skill KeenArrow = new() { Id = 1720, Name = "Keen Arrow", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/e/e4/Keen_Arrow_%28large%29.jpg?20081212204735" };
    public static readonly Skill KeenArrowPvP = new() { Id = 3147, Name = "Keen Arrow (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = KeenArrow };
    public static readonly Skill KeenChop = new() { Id = 2009, Name = "Keen Chop", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/15/Keen_Chop_%28large%29.jpg?20081212210618" };
    public static readonly Skill KeystoneSignet = new() { Id = 63, Name = "Keystone Signet", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/e/e6/Keystone_Signet_%28large%29.jpg?20081212201144" };
    public static readonly Skill KindleArrows = new() { Id = 433, Name = "Kindle Arrows", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/7b/Kindle_Arrows_%28large%29.jpg?20081212204916" };
    public static readonly Skill KineticArmor = new() { Id = 166, Name = "Kinetic Armor", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/3/37/Kinetic_Armor_%28large%29.jpg?20081212195756" };
    public static readonly Skill KirinsWrath = new() { Id = 1113, Name = "Kirin's Wrath", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/57/Kirin%27s_Wrath_%28large%29.jpg?20081212201823" };
    public static readonly Skill KitahsBurden = new() { Id = 1056, Name = "Kitah's Burden", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/af/Kitah%27s_Burden_%28large%29.jpg?20081212201521" };
    public static readonly Skill KneeCutter = new() { Id = 2010, Name = "Knee Cutter", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/c/ca/Knee_Cutter_%28large%29.jpg?20081212210630" };
    public static readonly Skill Lacerate = new() { Id = 961, Name = "Lacerate", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/6/63/Lacerate_%28large%29.jpg?20081212205218" };
    public static readonly Skill LaceratingChop = new() { Id = 849, Name = "Lacerating Chop", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/a4/Lacerating_Chop_%28large%29.jpg?20081212210552" };
    public static readonly Skill Lamentation = new() { Id = 916, Name = "Lamentation", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/e9/Lamentation_%28large%29.jpg?20081212205439" };
    public static readonly Skill LavaArrows = new() { Id = 824, Name = "Lava Arrows", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/87/Lava_Arrows_%28large%29.jpg?20081212195330" };
    public static readonly Skill LavaFont = new() { Id = 195, Name = "Lava Font", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/c7/Lava_Font_%28large%29.jpg?20090626202552" };
    public static readonly Skill LeadtheWay = new() { Id = 1590, Name = "Lead the Way!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/4/4c/%22Lead_the_Way%21%22_%28large%29.jpg" };
    public static readonly Skill LeadersComfort = new() { Id = 1584, Name = "Leader's Comfort", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/b4/Leader%27s_Comfort_%28large%29.jpg?20081212203816" };
    public static readonly Skill LeadersZeal = new() { Id = 1583, Name = "Leader's Zeal", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/f/f4/Leader%27s_Zeal_%28large%29.jpg?20081212204149" };
    public static readonly Skill LeapingMantisSting = new() { Id = 1023, Name = "Leaping Mantis Sting", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/fe/Leaping_Mantis_Sting_%28large%29.jpg?20081212194107" };
    public static readonly Skill LeechSignet = new() { Id = 61, Name = "Leech Signet", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/af/Leech_Signet_%28large%29.jpg?20081212201715" };
    public static readonly Skill LeviathansSweep = new() { Id = 1134, Name = "Leviathan's Sweep", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/0c/Leviathan%27s_Sweep_%28large%29.jpg?20081212210601" };
    public static readonly Skill Life = new() { Id = 1251, Name = "Life", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/7/77/Life_%28large%29.jpg?20081212205500" };
    public static readonly Skill LifePvP = new() { Id = 3012, Name = "Life (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Life };
    public static readonly Skill LifeAttunement = new() { Id = 244, Name = "Life Attunement", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/53/Life_Attunement_%28large%29.jpg?20081212201956" };
    public static readonly Skill LifeBarrier = new() { Id = 270, Name = "Life Barrier", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/32/Life_Barrier_%28large%29.jpg?20081212202217" };
    public static readonly Skill LifeBond = new() { Id = 241, Name = "Life Bond", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/3e/Life_Bond_%28large%29.jpg?20090531001956" };
    public static readonly Skill LifeSheath = new() { Id = 1123, Name = "Life Sheath", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/6/69/Life_Sheath_%28large%29.jpg?20081212202001" };
    public static readonly Skill LifeSiphon = new() { Id = 109, Name = "Life Siphon", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/c/c0/Life_Siphon_%28large%29.jpg?20081212203317" };
    public static readonly Skill LifeTransfer = new() { Id = 126, Name = "Life Transfer", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/33/Life_Transfer_%28large%29.jpg?20081212203124" };
    public static readonly Skill LifebaneStrike = new() { Id = 1067, Name = "Lifebane Strike", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/b4/Lifebane_Strike_%28large%29.jpg?20081212203214" };
    public static readonly Skill LiftEnchantment = new() { Id = 1645, Name = "Lift Enchantment", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/7/7e/Lift_Enchantment_%28large%29.jpg?20081212194143" };
    public static readonly Skill LightofDeldrimor = new() { Id = 2212, Name = "Light of Deldrimor", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/1/11/Light_of_Deldrimor.jpg" };
    public static readonly Skill LightofDeliverance = new() { Id = 1397, Name = "Light of Deliverance", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/ee/Light_of_Deliverance_%28large%29.jpg?20081212202510" };
    public static readonly Skill LightofDeliverancePvP = new() { Id = 2871, Name = "Light of Deliverance (PvP)", AlternativeName = "", Profession = Profession.Monk, IconUrl = "", IsPvP = true, AlternativeSkill = LightofDeliverance };
    public static readonly Skill LightofDwayna = new() { Id = 304, Name = "Light of Dwayna", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/d/d1/Light_of_Dwayna_%28large%29.jpg?20081212202455" };
    public static readonly Skill LightbringerSignet = new() { Id = 1815, Name = "Lightbringer Signet", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/4/43/Lightbringer_Signet.jpg" };
    public static readonly Skill LightbringersGaze = new() { Id = 1814, Name = "Lightbringer's Gaze", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/c/c6/Lightbringer%27s_Gaze.jpg" };
    public static readonly Skill LightningBolt = new() { Id = 1369, Name = "Lightning Bolt", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/6/6c/Lightning_Bolt_%28large%29.jpg?20081212195734" };
    public static readonly Skill LightningHammer = new() { Id = 865, Name = "Lightning Hammer", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/0/00/Lightning_Hammer_%28large%29.jpg?20081212195053" };
    public static readonly Skill LightningHammerPvP = new() { Id = 3396, Name = "Lightning Hammer (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = LightningHammer };
    public static readonly Skill LightningJavelin = new() { Id = 230, Name = "Lightning Javelin", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/8e/Lightning_Javelin_%28large%29.jpg?20081212195211" };
    public static readonly Skill LightningOrb = new() { Id = 229, Name = "Lightning Orb", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/1/13/Lightning_Orb_%28large%29.jpg?20081212195056" };
    public static readonly Skill LightningReflexes = new() { Id = 453, Name = "Lightning Reflexes", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/c/c5/Lightning_Reflexes_%28large%29.jpg?20081212204855" };
    public static readonly Skill LightningReflexesPvP = new() { Id = 3141, Name = "Lightning Reflexes (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = LightningReflexes };
    public static readonly Skill LightningStrike = new() { Id = 222, Name = "Lightning Strike", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/f/f9/Lightning_Strike_%28large%29.jpg?20081212195133" };
    public static readonly Skill LightningSurge = new() { Id = 205, Name = "Lightning Surge", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/88/Lightning_Surge_%28large%29.jpg?20081212195453" };
    public static readonly Skill LightningTouch = new() { Id = 232, Name = "Lightning Touch", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/1/18/Lightning_Touch_%28large%29.jpg?20081212195129" };
    public static readonly Skill LingeringCurse = new() { Id = 142, Name = "Lingering Curse", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/08/Lingering_Curse_%28large%29.jpg?20081212203507" };
    public static readonly Skill LionsComfort = new() { Id = 1407, Name = "Lion's Comfort", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/7/7f/Lion%27s_Comfort_%28large%29.jpg?20081212210914" };
    public static readonly Skill LiquidFlame = new() { Id = 845, Name = "Liquid Flame", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/24/Liquid_Flame_%28large%29.jpg?20081212195828" };
    public static readonly Skill LiveVicariously = new() { Id = 291, Name = "Live Vicariously", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/34/Live_Vicariously_%28large%29.jpg?20081212202516" };
    public static readonly Skill LivelyWasNaomei = new() { Id = 1222, Name = "Lively Was Naomei", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/1/1b/Lively_Was_Naomei_%28large%29.jpg?20081212205811" };
    public static readonly Skill LocustsFury = new() { Id = 1030, Name = "Locust's Fury", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/5/5c/Locust%27s_Fury_%28large%29.jpg?20081212194145" };
    public static readonly Skill LotusStrike = new() { Id = 1987, Name = "Lotus Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/9/9d/Lotus_Strike_%28large%29.jpg?20081212194105" };
    public static readonly Skill LowBlow = new() { Id = 2214, Name = "Low Blow", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/8/86/Low_Blow.jpg" };
    public static readonly Skill LyricofPurification = new() { Id = 1772, Name = "Lyric of Purification", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/1/18/Lyric_of_Purification_%28large%29.jpg?20081212204025" };
    public static readonly Skill LyricofZeal = new() { Id = 1563, Name = "Lyric of Zeal", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/9/91/Lyric_of_Zeal_%28large%29.jpg?20081212204007" };
    public static readonly Skill LyssasAssault = new() { Id = 1538, Name = "Lyssa's Assault", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/53/Lyssa%27s_Assault_%28large%29.jpg?20081212194749" };
    public static readonly Skill LyssasAura = new() { Id = 813, Name = "Lyssa's Aura", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/6/60/Lyssa%27s_Aura_%28large%29.jpg?20081212211322" };
    public static readonly Skill LyssasBalance = new() { Id = 877, Name = "Lyssa's Balance", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/1e/Lyssa%27s_Balance_%28large%29.jpg?20081212200743" };
    public static readonly Skill LyssasHaste = new() { Id = 1512, Name = "Lyssa's Haste", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/f/f3/Lyssa%27s_Haste_%28large%29.jpg?20081212194755" };
    public static readonly Skill LyssasHastePvP = new() { Id = 3348, Name = "Lyssa's Haste (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = LyssasHaste };
    public static readonly Skill Maelstrom = new() { Id = 215, Name = "Maelstrom", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/54/Maelstrom_%28large%29.jpg?20081212195811" };
    public static readonly Skill MagebaneShot = new() { Id = 1726, Name = "Magebane Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/54/Magebane_Shot_%28large%29.jpg?20081212204851" };
    public static readonly Skill MagehunterStrike = new() { Id = 1694, Name = "Magehunter Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/7/74/Magehunter_Strike_%28large%29.jpg?20081212210504" };
    public static readonly Skill MagehuntersSmash = new() { Id = 1697, Name = "Magehunter's Smash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/e/eb/Magehunter%27s_Smash_%28large%29.jpg?20081212210621" };
    public static readonly Skill MagneticAura = new() { Id = 168, Name = "Magnetic Aura", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b5/Magnetic_Aura_%28large%29.jpg?20081212195139" };
    public static readonly Skill MagneticSurge = new() { Id = 2190, Name = "Magnetic Surge", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/3/3a/Magnetic_Surge_%28large%29.jpg?20081212195229" };
    public static readonly Skill MaimingSpear = new() { Id = 2150, Name = "Maiming Spear", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/3a/Maiming_Spear_%28large%29.jpg?20081212203738" };
    public static readonly Skill MaimingStrike = new() { Id = 438, Name = "Maiming Strike", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/fa/Maiming_Strike_%28large%29.jpg?20081212204724" };
    public static readonly Skill MakeHaste = new() { Id = 1591, Name = "Make Haste!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/9/91/%22Make_Haste%21%22_%28large%29.jpg" };
    public static readonly Skill MakeYourTime = new() { Id = 1779, Name = "Make Your Time!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/4/41/%22Make_Your_Time%21%22_%28large%29.jpg" };
    public static readonly Skill Malaise = new() { Id = 140, Name = "Malaise", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/d6/Malaise_%28large%29.jpg?20081212202821" };
    public static readonly Skill MaliciousStrike = new() { Id = 1633, Name = "Malicious Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/e/e8/Malicious_Strike_%28large%29.jpg?20081212193951" };
    public static readonly Skill MalignIntervention = new() { Id = 122, Name = "Malign Intervention", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/56/Malign_Intervention_%28large%29.jpg?20081212202712" };
    public static readonly Skill MantisTouch = new() { Id = 974, Name = "Mantis Touch", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/dd/Mantis_Touch_%28large%29.jpg?20081212194543" };
    public static readonly Skill MantraofConcentration = new() { Id = 16, Name = "Mantra of Concentration", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/5/5c/Mantra_of_Concentration_%28large%29.jpg?20081212201449" };
    public static readonly Skill MantraofEarth = new() { Id = 6, Name = "Mantra of Earth", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/13/Mantra_of_Earth_%28large%29.jpg?20081212195951" };
    public static readonly Skill MantraofFlame = new() { Id = 7, Name = "Mantra of Flame", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/3/31/Mantra_of_Flame_%28large%29.jpg?20081212201339" };
    public static readonly Skill MantraofFrost = new() { Id = 8, Name = "Mantra of Frost", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/93/Mantra_of_Frost_%28large%29.jpg?20081212201627" };
    public static readonly Skill MantraofInscriptions = new() { Id = 15, Name = "Mantra of Inscriptions", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/c4/Mantra_of_Inscriptions_%28large%29.jpg?20081212201709" };
    public static readonly Skill MantraofLightning = new() { Id = 9, Name = "Mantra of Lightning", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/1d/Mantra_of_Lightning_%28large%29.jpg?20081212195935" };
    public static readonly Skill MantraofPersistence = new() { Id = 14, Name = "Mantra of Persistence", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/90/Mantra_of_Persistence_%28large%29.jpg?20081212195932" };
    public static readonly Skill MantraofRecall = new() { Id = 82, Name = "Mantra of Recall", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/11/Mantra_of_Recall_%28large%29.jpg?20081212201524" };
    public static readonly Skill MantraofRecovery = new() { Id = 13, Name = "Mantra of Recovery", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/b/bb/Mantra_of_Recovery_%28large%29.jpg?20081212201331" };
    public static readonly Skill MantraofResolve = new() { Id = 17, Name = "Mantra of Resolve", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/3/38/Mantra_of_Resolve_%28large%29.jpg?20081212201322" };
    public static readonly Skill MantraofResolvePvP = new() { Id = 3063, Name = "Mantra of Resolve (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = MantraofResolve };
    public static readonly Skill MantraofSignets = new() { Id = 18, Name = "Mantra of Signets", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/9c/Mantra_of_Signets_%28large%29.jpg?20081212201633" };
    public static readonly Skill MantraofSignetsPvP = new() { Id = 3179, Name = "Mantra of Signets (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = MantraofSignets };
    public static readonly Skill MaraudersShot = new() { Id = 908, Name = "Marauder's Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5d/Marauder%27s_Shot_%28large%29.jpg?20081212204656" };
    public static readonly Skill MarkofDeath = new() { Id = 785, Name = "Mark of Death", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/fc/Mark_of_Death_%28large%29.jpg?20081212194000" };
    public static readonly Skill MarkofFury = new() { Id = 1360, Name = "Mark of Fury", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/81/Mark_of_Fury_%28large%29.jpg?20081212203625" };
    public static readonly Skill MarkofInsecurity = new() { Id = 570, Name = "Mark of Insecurity", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/2d/Mark_of_Insecurity_%28large%29.jpg?20081212194318" };
    public static readonly Skill MarkofInstability = new() { Id = 978, Name = "Mark of Instability", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/f3/Mark_of_Instability_%28large%29.jpg?20081212194033" };
    public static readonly Skill MarkofPain = new() { Id = 150, Name = "Mark of Pain", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/35/Mark_of_Pain_%28large%29.jpg?20090107193022" };
    public static readonly Skill MarkofProtection = new() { Id = 269, Name = "Mark of Protection", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/09/Mark_of_Protection_%28large%29.jpg?20081212202647" };
    public static readonly Skill MarkofRodgort = new() { Id = 190, Name = "Mark of Rodgort", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/0/01/Mark_of_Rodgort_%28large%29.jpg?20081212195632" };
    public static readonly Skill MarkofSubversion = new() { Id = 127, Name = "Mark of Subversion", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/67/Mark_of_Subversion_%28large%29.jpg?20081212203039" };
    public static readonly Skill MarksmansWager = new() { Id = 430, Name = "Marksman's Wager", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/f4/Marksman%27s_Wager_%28large%29.jpg?20081212204913" };
    public static readonly Skill Martyr = new() { Id = 298, Name = "Martyr", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/05/Martyr_%28large%29.jpg?20081212202320" };
    public static readonly Skill Masochism = new() { Id = 2139, Name = "Masochism", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/c/c5/Masochism_%28large%29.jpg?20081212202818" };
    public static readonly Skill MasochismPvP = new() { Id = 3054, Name = "Masochism (PvP)", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "", IsPvP = true, AlternativeSkill = Masochism };
    public static readonly Skill MasterofMagic = new() { Id = 1378, Name = "Master of Magic", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/6/6e/Master_of_Magic_%28large%29.jpg?20081212195922" };
    public static readonly Skill Meditation = new() { Id = 1523, Name = "Meditation", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/2/24/Meditation_%28large%29.jpg?20081212194733" };
    public static readonly Skill Meekness = new() { Id = 1260, Name = "Meekness", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/47/Meekness_%28large%29.jpg?20081212203133" };
    public static readonly Skill MelandrusArrows = new() { Id = 429, Name = "Melandru's Arrows", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5b/Melandru%27s_Arrows_%28large%29.jpg?20081212205226" };
    public static readonly Skill MelandrusAssault = new() { Id = 441, Name = "Melandru's Assault", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/11/Melandru%27s_Assault_%28large%29.jpg?20081212205114" };
    public static readonly Skill MelandrusAssaultPvP = new() { Id = 3047, Name = "Melandru's Assault (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = MelandrusAssault };
    public static readonly Skill MelandrusResilience = new() { Id = 451, Name = "Melandru's Resilience", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/6/6d/Melandru%27s_Resilience_%28large%29.jpg?20081212204755" };
    public static readonly Skill MelandrusShot = new() { Id = 853, Name = "Melandru's Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/53/Melandru%27s_Shot_%28large%29.jpg?20081212205022" };
    public static readonly Skill MendAilment = new() { Id = 277, Name = "Mend Ailment", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/e2/Mend_Ailment_%28large%29.jpg?20081212202028" };
    public static readonly Skill MendBodyandSoul = new() { Id = 1234, Name = "Mend Body and Soul", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/2/27/Mend_Body_and_Soul_%28large%29.jpg?20081212205913" };
    public static readonly Skill MendCondition = new() { Id = 275, Name = "Mend Condition", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/7/78/Mend_Condition_%28large%29.jpg?20081212201849" };
    public static readonly Skill Mending = new() { Id = 290, Name = "Mending", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/1/14/Mending_%28large%29.jpg?20090531061653" };
    public static readonly Skill MendingGrip = new() { Id = 2202, Name = "Mending Grip", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/f/f5/Mending_Grip_%28large%29.jpg?20081212205357" };
    public static readonly Skill MendingRefrain = new() { Id = 1578, Name = "Mending Refrain", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/b9/Mending_Refrain_%28large%29.jpg?20081212203925" };
    public static readonly Skill MendingRefrainPvP = new() { Id = 3149, Name = "Mending Refrain (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = MendingRefrain };
    public static readonly Skill MendingTouch = new() { Id = 1401, Name = "Mending Touch", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/8c/Mending_Touch_%28large%29.jpg?20081212202220" };
    public static readonly Skill MentalBlock = new() { Id = 2417, Name = "Mental Block", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/e/ed/Mental_Block.jpg" };
    public static readonly Skill MercilessSpear = new() { Id = 1603, Name = "Merciless Spear", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/5e/Merciless_Spear_%28large%29.jpg?20081212203921" };
    public static readonly Skill Meteor = new() { Id = 187, Name = "Meteor", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b6/Meteor_%28large%29.jpg?20081212195340" };
    public static readonly Skill MeteorShower = new() { Id = 192, Name = "Meteor Shower", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/9c/Meteor_Shower_%28large%29.jpg?20081212195312" };
    public static readonly Skill MightyBlow = new() { Id = 351, Name = "Mighty Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/ab/Mighty_Blow_%28large%29.jpg?20081212210651" };
    public static readonly Skill MightyThrow = new() { Id = 1547, Name = "Mighty Throw", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/51/Mighty_Throw_%28large%29.jpg?20081212203856" };
    public static readonly Skill MightyWasVorizun = new() { Id = 773, Name = "Mighty Was Vorizun", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/e0/Mighty_Was_Vorizun_%28large%29.jpg?20081212205454" };
    public static readonly Skill Migraine = new() { Id = 53, Name = "Migraine", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/1e/Migraine_%28large%29.jpg?20081212200249" };
    public static readonly Skill MigrainePvP = new() { Id = 3183, Name = "Migraine (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = Migraine };
    public static readonly Skill MindBlast = new() { Id = 1662, Name = "Mind Blast", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/f/f1/Mind_Blast_%28large%29.jpg?20081212195155" };
    public static readonly Skill MindBurn = new() { Id = 185, Name = "Mind Burn", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/8f/Mind_Burn_%28large%29.jpg?20081212195433" };
    public static readonly Skill MindFreeze = new() { Id = 209, Name = "Mind Freeze", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a8/Mind_Freeze_%28large%29.jpg?20081212195813" };
    public static readonly Skill MindFreezePvP = new() { Id = 2803, Name = "Mind Freeze (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = MindFreeze };
    public static readonly Skill MindShock = new() { Id = 226, Name = "Mind Shock", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/2f/Mind_Shock_%28large%29.jpg?20081212195613" };
    public static readonly Skill MindShockPvP = new() { Id = 2804, Name = "Mind Shock (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = MindShock };
    public static readonly Skill MindWrack = new() { Id = 49, Name = "Mind Wrack", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/9f/Mind_Wrack_%28large%29.jpg?20081212200857" };
    public static readonly Skill MindWrackPvP = new() { Id = 2734, Name = "Mind Wrack (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = MindWrack };
    public static readonly Skill Mindbender = new() { Id = 2411, Name = "Mindbender", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/c/c0/Mindbender.jpg" };
    public static readonly Skill MirageCloak = new() { Id = 1500, Name = "Mirage Cloak", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/18/Mirage_Cloak_%28large%29.jpg?20081212194707" };
    public static readonly Skill MirrorofDisenchantment = new() { Id = 1349, Name = "Mirror of Disenchantment", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/4/40/Mirror_of_Disenchantment_%28large%29.jpg?20081212201536" };
    public static readonly Skill MirrorofDisenchantmentPvP = new() { Id = 3194, Name = "Mirror of Disenchantment (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = MirrorofDisenchantment };
    public static readonly Skill MirrorofIce = new() { Id = 1098, Name = "Mirror of Ice", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/41/Mirror_of_Ice_%28large%29.jpg?20081212195826" };
    public static readonly Skill MirroredStance = new() { Id = 816, Name = "Mirrored Stance", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/3/37/Mirrored_Stance_%28large%29.jpg?20081212194407" };
    public static readonly Skill MistForm = new() { Id = 236, Name = "Mist Form", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/86/Mist_Form_%28large%29.jpg?20081212195926" };
    public static readonly Skill MistFormPvP = new() { Id = 2805, Name = "Mist Form (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = MistForm };
    public static readonly Skill Mistrust = new() { Id = 979, Name = "Mistrust", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/0/06/Mistrust_%28large%29.jpg?20081212201611" };
    public static readonly Skill MistrustPvP = new() { Id = 3191, Name = "Mistrust (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = Mistrust };
    public static readonly Skill MoebiusStrike = new() { Id = 781, Name = "Moebius Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/7/73/Moebius_Strike_%28large%29.jpg?20081212194321" };
    public static readonly Skill MokeleSmash = new() { Id = 1409, Name = "Mokele Smash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/00/Mokele_Smash_%28large%29.jpg?20081212211132" };
    public static readonly Skill MuddyTerrain = new() { Id = 477, Name = "Muddy Terrain", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/a/a0/Muddy_Terrain_%28large%29.jpg?20081212205109" };
    public static readonly Skill MysticCorruption = new() { Id = 1755, Name = "Mystic Corruption", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/e/ee/Mystic_Corruption_%28large%29.jpg?20081212194742" };
    public static readonly Skill MysticHealing = new() { Id = 1527, Name = "Mystic Healing", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/7/71/Mystic_Healing_%28large%29.jpg?20081212194922" };
    public static readonly Skill MysticHealingPvP = new() { Id = 3272, Name = "Mystic Healing (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = MysticHealing };
    public static readonly Skill MysticRegeneration = new() { Id = 1516, Name = "Mystic Regeneration", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/4/43/Mystic_Regeneration_%28large%29.jpg?20081212194920" };
    public static readonly Skill MysticRegenerationPvP = new() { Id = 2884, Name = "Mystic Regeneration (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = MysticRegeneration };
    public static readonly Skill MysticSandstorm = new() { Id = 1532, Name = "Mystic Sandstorm", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/c/c1/Mystic_Sandstorm_%28large%29.jpg?20081212194927" };
    public static readonly Skill MysticSweep = new() { Id = 1484, Name = "Mystic Sweep", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/8/8b/Mystic_Sweep_%28large%29.jpg?20081212194905" };
    public static readonly Skill MysticTwister = new() { Id = 1491, Name = "Mystic Twister", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/a/ae/Mystic_Twister_%28large%29.jpg?20081212194856" };
    public static readonly Skill MysticVigor = new() { Id = 1503, Name = "Mystic Vigor", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/1d/Mystic_Vigor_%28large%29.jpg?20081212194556" };
    public static readonly Skill NaturalHealing = new() { Id = 1525, Name = "Natural Healing", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/4/43/Natural_Healing_%28large%29.jpg?20081212194712" };
    public static readonly Skill NaturalStride = new() { Id = 1727, Name = "Natural Stride", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/59/Natural_Stride_%28large%29.jpg?20081212204937" };
    public static readonly Skill NaturalTemper = new() { Id = 1770, Name = "Natural Temper", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/7/70/Natural_Temper_%28large%29.jpg?20081212204209" };
    public static readonly Skill NaturesRenewal = new() { Id = 476, Name = "Nature's Renewal", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/12/Nature%27s_Renewal_%28large%29.jpg?20081212204705" };
    public static readonly Skill Necrosis = new() { Id = 2103, Name = "Necrosis", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/99/Necrosis.jpg" };
    public static readonly Skill NecroticTraversal = new() { Id = 97, Name = "Necrotic Traversal", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/8f/Necrotic_Traversal_%28large%29.jpg?20081212202958" };
    public static readonly Skill NeedlingShot = new() { Id = 1197, Name = "Needling Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d4/Needling_Shot_%28large%29.jpg?20081212204511" };
    public static readonly Skill NeverGiveUp = new() { Id = 1593, Name = "Never Give Up!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/0/02/%22Never_Give_Up%21%22_%28large%29.jpg" };
    public static readonly Skill NeverGiveUpPvP = new() { Id = 3035, Name = "Never Give Up! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = NeverGiveUp };
    public static readonly Skill NeverRampageAlone = new() { Id = 2108, Name = "Never Rampage Alone", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d1/Never_Rampage_Alone.jpg" };
    public static readonly Skill NeverSurrender = new() { Id = 1598, Name = "Never Surrender!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/1/1d/%22Never_Surrender%21%22_%28large%29.jpg" };
    public static readonly Skill NeverSurrenderPvP = new() { Id = 2880, Name = "Never Surrender! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = NeverSurrender };
    public static readonly Skill NightmareWeapon = new() { Id = 795, Name = "Nightmare Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/0d/Nightmare_Weapon_%28large%29.jpg?20081212205351" };
    public static readonly Skill NineTailStrike = new() { Id = 986, Name = "Nine Tail Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/c8/Nine_Tail_Strike_%28large%29.jpg?20081212194450" };
    public static readonly Skill None = new() { Id = 0, Name = "None", AlternativeName = "", Profession = Profession.None, IconUrl = "" };
    public static readonly Skill NoneShallPass = new() { Id = 891, Name = "None Shall Pass!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/3c/%22None_Shall_Pass%21%22_%28large%29.jpg" };
    public static readonly Skill OathShot = new() { Id = 405, Name = "Oath Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/55/Oath_Shot_%28large%29.jpg?20081212205211" };
    public static readonly Skill ObsidianFlame = new() { Id = 219, Name = "Obsidian Flame", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/49/Obsidian_Flame_%28large%29.jpg?20081212195153" };
    public static readonly Skill ObsidianFlamePvP = new() { Id = 2809, Name = "Obsidian Flame (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = ObsidianFlame };
    public static readonly Skill ObsidianFlesh = new() { Id = 218, Name = "Obsidian Flesh", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/1/1c/Obsidian_Flesh_%28large%29.jpg?20081212195344" };
    public static readonly Skill OfferingofBlood = new() { Id = 146, Name = "Offering of Blood", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/c/c8/Offering_of_Blood_%28large%29.jpg?20081212203336" };
    public static readonly Skill OfferingofSpirit = new() { Id = 1479, Name = "Offering of Spirit", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/b/bd/Offering_of_Spirit_%28large%29.jpg?20081212205402" };
    public static readonly Skill OnYourKnees = new() { Id = 906, Name = "On Your Knees!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/c/c9/%22On_Your_Knees%21%22_%28large%29.jpg" };
    public static readonly Skill Onslaught = new() { Id = 1754, Name = "Onslaught", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/5a/Onslaught_%28large%29.jpg?20081212195032" };
    public static readonly Skill OnslaughtPvP = new() { Id = 3365, Name = "Onslaught (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = Onslaught };
    public static readonly Skill OppressiveGaze = new() { Id = 864, Name = "Oppressive Gaze", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/9e/Oppressive_Gaze_%28large%29.jpg?20081212202939" };
    public static readonly Skill OrderofApostasy = new() { Id = 863, Name = "Order of Apostasy", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/c/c2/Order_of_Apostasy_%28large%29.jpg?20081212203226" };
    public static readonly Skill OrderofPain = new() { Id = 134, Name = "Order of Pain", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e0/Order_of_Pain_%28large%29.jpg?20081212203130" };
    public static readonly Skill OrderoftheVampire = new() { Id = 148, Name = "Order of the Vampire", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/91/Order_of_the_Vampire_%28large%29.jpg?20081212203451" };
    public static readonly Skill OrderofUndeath = new() { Id = 1352, Name = "Order of Undeath", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/a/a9/Order_of_Undeath_%28large%29.jpg?20081212203022" };
    public static readonly Skill OrisonofHealing = new() { Id = 281, Name = "Orison of Healing", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/b8/Orison_of_Healing_%28large%29.jpg?20081212202620" };
    public static readonly Skill OtyughsCry = new() { Id = 447, Name = "Otyugh's Cry", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/2/2e/Otyugh%27s_Cry_%28large%29.jpg?20081212204612" };
    public static readonly Skill OverTheLimit = new() { Id = 3424, Name = "Over The Limit", AlternativeName = "Over the Limit", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/5a/Over_the_Limit.jpg" };
    public static readonly Skill OverbearingSmash = new() { Id = 1410, Name = "Overbearing Smash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/5/5e/Overbearing_Smash_%28large%29.jpg?20081212210911" };
    public static readonly Skill Overload = new() { Id = 898, Name = "Overload", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/e/ee/Overload_%28large%29.jpg?20081212200833" };
    public static readonly Skill Pacifism = new() { Id = 264, Name = "Pacifism", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/05/Pacifism_%28large%29.jpg?20081212202529" };
    public static readonly Skill Pain = new() { Id = 1247, Name = "Pain", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/03/Pain_%28large%29.jpg?20081212205556" };
    public static readonly Skill PainPvP = new() { Id = 3007, Name = "Pain (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Pain };
    public static readonly Skill PainInverter = new() { Id = 2418, Name = "Pain Inverter", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/9/91/Pain_Inverter.jpg" };
    public static readonly Skill PainofDisenchantment = new() { Id = 1359, Name = "Pain of Disenchantment", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/ba/Pain_of_Disenchantment_%28large%29.jpg?20081212203325" };
    public static readonly Skill PainfulBond = new() { Id = 1237, Name = "Painful Bond", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/f/fb/Painful_Bond_%28large%29.jpg?20081212205447" };
    public static readonly Skill PalmStrike = new() { Id = 1045, Name = "Palm Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/7/73/Palm_Strike_%28large%29.jpg?20081212194311" };
    public static readonly Skill Panic = new() { Id = 52, Name = "Panic", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/6/65/Panic_%28large%29.jpg?20081212201509" };
    public static readonly Skill ParasiticBond = new() { Id = 99, Name = "Parasitic Bond", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/57/Parasitic_Bond_%28large%29.jpg?20081212203159" };
    public static readonly Skill PatientSpirit = new() { Id = 2061, Name = "Patient Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/e4/Patient_Spirit_%28large%29.jpg?20081212202539" };
    public static readonly Skill PeaceandHarmony = new() { Id = 266, Name = "Peace and Harmony", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/d/d1/Peace_and_Harmony_%28large%29.jpg?20081212201958" };
    public static readonly Skill PenetratingAttack = new() { Id = 398, Name = "Penetrating Attack", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/31/Penetrating_Attack_%28large%29.jpg?20081212204548" };
    public static readonly Skill PenetratingAttackPvP = new() { Id = 2861, Name = "Penetrating Attack (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = PenetratingAttack };
    public static readonly Skill PenetratingBlow = new() { Id = 339, Name = "Penetrating Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/6b/Penetrating_Blow_%28large%29.jpg?20081212211055" };
    public static readonly Skill PenetratingChop = new() { Id = 1136, Name = "Penetrating Chop", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/31/Penetrating_Chop_%28large%29.jpg?20081212210710" };
    public static readonly Skill PensiveGuardian = new() { Id = 1683, Name = "Pensive Guardian", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/ee/Pensive_Guardian_%28large%29.jpg?20081212202305" };
    public static readonly Skill PersistenceofMemory = new() { Id = 1338, Name = "Persistence of Memory", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/da/Persistence_of_Memory_%28large%29.jpg?20081212200710" };
    public static readonly Skill Pestilence = new() { Id = 870, Name = "Pestilence", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/e/e2/Pestilence_%28large%29.jpg?20081212204525" };
    public static readonly Skill PhantomPain = new() { Id = 44, Name = "Phantom Pain", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/94/Phantom_Pain_%28large%29.jpg?20081212195954" };
    public static readonly Skill Phoenix = new() { Id = 193, Name = "Phoenix", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/57/Phoenix_%28large%29.jpg?20081212195513" };
    public static readonly Skill PhysicalResistance = new() { Id = 73, Name = "Physical Resistance", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/4/4d/Physical_Resistance_%28large%29.jpg?20081212200941" };
    public static readonly Skill PiercingTrap = new() { Id = 2140, Name = "Piercing Trap", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/9/91/Piercing_Trap_%28large%29.jpg?20081212204341" };
    public static readonly Skill PinDown = new() { Id = 392, Name = "Pin Down", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/c/c8/Pin_Down_%28large%29.jpg?20081212204349" };
    public static readonly Skill PiousAssault = new() { Id = 1490, Name = "Pious Assault", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/b/b3/Pious_Assault_%28large%29.jpg?20081212194844" };
    public static readonly Skill PiousAssaultPvP = new() { Id = 3266, Name = "Pious Assault (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = PiousAssault };
    public static readonly Skill PiousConcentration = new() { Id = 1542, Name = "Pious Concentration", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/8/85/Pious_Concentration_%28large%29.jpg?20081212194554" };
    public static readonly Skill PiousFury = new() { Id = 2146, Name = "Pious Fury", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/7/79/Pious_Fury_%28large%29.jpg?20081212194717" };
    public static readonly Skill PiousFuryPvP = new() { Id = 3368, Name = "Pious Fury (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = PiousFury };
    public static readonly Skill PiousHaste = new() { Id = 1543, Name = "Pious Haste", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/a/a8/Pious_Haste_%28large%29.jpg?20081212194858" };
    public static readonly Skill PiousRenewal = new() { Id = 1499, Name = "Pious Renewal", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/b/ba/Pious_Renewal_%28large%29.jpg?20081212195037" };
    public static readonly Skill PiousRestoration = new() { Id = 1529, Name = "Pious Restoration", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/13/Pious_Restoration_%28large%29.jpg?20081212194721" };
    public static readonly Skill PlagueSending = new() { Id = 149, Name = "Plague Sending", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/d0/Plague_Sending_%28large%29.jpg?20081212203400" };
    public static readonly Skill PlagueSignet = new() { Id = 132, Name = "Plague Signet", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/dc/Plague_Signet_%28large%29.jpg?20081212203211" };
    public static readonly Skill PlagueTouch = new() { Id = 154, Name = "Plague Touch", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/c/cf/Plague_Touch_%28large%29.jpg?20081212203302" };
    public static readonly Skill PointBlankShot = new() { Id = 407, Name = "Point Blank Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/a/a2/Point_Blank_Shot_%28large%29.jpg?20081225003553" };
    public static readonly Skill PoisonArrow = new() { Id = 404, Name = "Poison Arrow", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/4/40/Poison_Arrow_%28large%29.jpg?20081212205042" };
    public static readonly Skill PoisonTipSignet = new() { Id = 2199, Name = "Poison Tip Signet", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/fe/Poison_Tip_Signet_%28large%29.jpg?20081212204610" };
    public static readonly Skill PoisonedHeart = new() { Id = 840, Name = "Poisoned Heart", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/a/ab/Poisoned_Heart_%28large%29.jpg?20081212203029" };
    public static readonly Skill PoisonousBite = new() { Id = 1205, Name = "Poisonous Bite", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/9/9a/Poisonous_Bite_%28large%29.jpg?20081212205119" };
    public static readonly Skill Pounce = new() { Id = 1206, Name = "Pounce", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/fe/Pounce_%28large%29.jpg?20081212204411" };
    public static readonly Skill PowerAttack = new() { Id = 322, Name = "Power Attack", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/c/c6/Power_Attack_%28large%29.jpg?20081212210400" };
    public static readonly Skill PowerBlock = new() { Id = 5, Name = "Power Block", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/2e/Power_Block_%28large%29.jpg?20081212201416" };
    public static readonly Skill PowerDrain = new() { Id = 25, Name = "Power Drain", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/d0/Power_Drain_%28large%29.jpg?20081212201502" };
    public static readonly Skill PowerFlux = new() { Id = 953, Name = "Power Flux", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/22/Power_Flux_%28large%29.jpg?20081212201650" };
    public static readonly Skill PowerLeak = new() { Id = 24, Name = "Power Leak", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/dc/Power_Leak_%28large%29.jpg?20081212195949" };
    public static readonly Skill PowerLeech = new() { Id = 803, Name = "Power Leech", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/72/Power_Leech_%28large%29.jpg?20081212200958" };
    public static readonly Skill PowerLock = new() { Id = 1994, Name = "Power Lock", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/b/b2/Power_Lock_%28large%29.jpg?20081212200043" };
    public static readonly Skill PowerReturn = new() { Id = 931, Name = "Power Return", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/dc/Power_Return_%28large%29.jpg?20081212201217" };
    public static readonly Skill PowerShot = new() { Id = 394, Name = "Power Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/e/e4/Power_Shot_%28large%29.jpg?20081212204551" };
    public static readonly Skill PowerSpike = new() { Id = 23, Name = "Power Spike", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/92/Power_Spike_%28large%29.jpg?20081212195940" };
    public static readonly Skill PracticedStance = new() { Id = 449, Name = "Practiced Stance", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d6/Practiced_Stance_%28large%29.jpg?20081212204531" };
    public static readonly Skill PrecisionShot = new() { Id = 400, Name = "Precision Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/a/a8/Precision_Shot_%28large%29.jpg?20081212204717" };
    public static readonly Skill PredatorsPounce = new() { Id = 443, Name = "Predator's Pounce", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/c/c0/Predator%27s_Pounce_%28large%29.jpg?20081212204946" };
    public static readonly Skill PredatoryBond = new() { Id = 1194, Name = "Predatory Bond", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/2/2f/Predatory_Bond_%28large%29.jpg?20081212204801" };
    public static readonly Skill PredatoryBondPvP = new() { Id = 3050, Name = "Predatory Bond (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = PredatoryBond };
    public static readonly Skill PredatorySeason = new() { Id = 470, Name = "Predatory Season", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/38/Predatory_Season_%28large%29.jpg?20081212204417" };
    public static readonly Skill PreparedShot = new() { Id = 1465, Name = "Prepared Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d5/Prepared_Shot_%28large%29.jpg?20081212204431" };
    public static readonly Skill Preservation = new() { Id = 1250, Name = "Preservation", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/98/Preservation_%28large%29.jpg?20081212205441" };
    public static readonly Skill PreservationPvP = new() { Id = 3011, Name = "Preservation (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Preservation };
    public static readonly Skill PriceofFailure = new() { Id = 103, Name = "Price of Failure", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/4d/Price_of_Failure_%28large%29.jpg?20081212203522" };
    public static readonly Skill PriceofPride = new() { Id = 1655, Name = "Price of Pride", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/f/fd/Price_of_Pride_%28large%29.jpg?20081212200408" };
    public static readonly Skill PrimalEchoes = new() { Id = 469, Name = "Primal Echoes", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/16/Primal_Echoes_%28large%29.jpg?20081212204943" };
    public static readonly Skill PrimalRage = new() { Id = 831, Name = "Primal Rage", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/7/7f/Primal_Rage_%28large%29.jpg?20081212211037" };
    public static readonly Skill ProtectiveBond = new() { Id = 263, Name = "Protective Bond", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/9e/Protective_Bond_%28large%29.jpg?20081212202502" };
    public static readonly Skill ProtectiveSpirit = new() { Id = 245, Name = "Protective Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/99/Protective_Spirit_%28large%29.jpg?20081212202629" };
    public static readonly Skill ProtectiveWasKaolai = new() { Id = 1219, Name = "Protective Was Kaolai", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/9a/Protective_Was_Kaolai_%28large%29.jpg?20081212205731" };
    public static readonly Skill ProtectorsDefense = new() { Id = 810, Name = "Protector's Defense", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/8/8d/Protector%27s_Defense_%28large%29.jpg?20081212210512" };
    public static readonly Skill ProtectorsStrike = new() { Id = 326, Name = "Protector's Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/0b/Protector%27s_Strike_%28large%29.jpg?20081212210823" };
    public static readonly Skill PsychicDistraction = new() { Id = 1053, Name = "Psychic Distraction", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/a7/Psychic_Distraction_%28large%29.jpg?20081212201711" };
    public static readonly Skill PsychicInstability = new() { Id = 1057, Name = "Psychic Instability", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/f/fe/Psychic_Instability_%28large%29.jpg?20081212200850" };
    public static readonly Skill PsychicInstabilityPvP = new() { Id = 3185, Name = "Psychic Instability (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = PsychicInstability };
    public static readonly Skill PulverizingSmash = new() { Id = 2008, Name = "Pulverizing Smash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/04/Pulverizing_Smash_%28large%29.jpg?20081212210812" };
    public static readonly Skill PunishingShot = new() { Id = 409, Name = "Punishing Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d0/Punishing_Shot_%28large%29.jpg?20081212205204" };
    public static readonly Skill PureStrike = new() { Id = 328, Name = "Pure Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/62/Pure_Strike_%28large%29.jpg?20100404015442" };
    public static readonly Skill PureWasLiMing = new() { Id = 2072, Name = "Pure Was Li Ming", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/6/64/Pure_Was_Li_Ming_%28large%29.jpg?20081212205920" };
    public static readonly Skill PurgeConditions = new() { Id = 278, Name = "Purge Conditions", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/83/Purge_Conditions_%28large%29.jpg?20081212201808" };
    public static readonly Skill PurgeSignet = new() { Id = 295, Name = "Purge Signet", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c2/Purge_Signet_%28large%29.jpg?20081212201841" };
    public static readonly Skill PurifyingFinale = new() { Id = 1579, Name = "Purifying Finale", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/1/1a/Purifying_Finale_%28large%29.jpg?20081212203831" };
    public static readonly Skill PurifyingVeil = new() { Id = 2007, Name = "Purifying Veil", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/d/dd/Purifying_Veil_%28large%29.jpg?20081212202626" };
    public static readonly Skill PutridBile = new() { Id = 2058, Name = "Putrid Bile", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/0a/Putrid_Bile_%28large%29.jpg?20081212203643" };
    public static readonly Skill PutridExplosion = new() { Id = 95, Name = "Putrid Explosion", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/60/Putrid_Explosion_%28large%29.jpg?20081212203504" };
    public static readonly Skill PutridFlesh = new() { Id = 1353, Name = "Putrid Flesh", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/36/Putrid_Flesh_%28large%29.jpg?20081212203006" };
    public static readonly Skill QuickShot = new() { Id = 397, Name = "Quick Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/a/af/Quick_Shot_%28large%29.jpg?20090530232836" };
    public static readonly Skill QuickeningZephyr = new() { Id = 475, Name = "Quickening Zephyr", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/0/02/Quickening_Zephyr_%28large%29.jpg?20081212204758" };
    public static readonly Skill Quicksand = new() { Id = 1473, Name = "Quicksand", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/8b/Quicksand_%28large%29.jpg?20081212204910" };
    public static readonly Skill QuiveringBlade = new() { Id = 892, Name = "Quivering Blade", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/14/Quivering_Blade_%28large%29.jpg?20081212210101" };
    public static readonly Skill RadiantScythe = new() { Id = 2012, Name = "Radiant Scythe", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/a/a8/Radiant_Scythe_%28large%29.jpg?20081212194608" };
    public static readonly Skill RadiationField = new() { Id = 2414, Name = "Radiation Field", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/3/31/Radiation_Field.jpg" };
    public static readonly Skill RageoftheNtouka = new() { Id = 1408, Name = "Rage of the Ntouka", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/08/Rage_of_the_Ntouka_%28large%29.jpg?20081212210844" };
    public static readonly Skill RampageasOne = new() { Id = 1721, Name = "Rampage as One", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5a/Rampage_as_One_%28large%29.jpg?20081212204843" };
    public static readonly Skill RapidFire = new() { Id = 2068, Name = "Rapid Fire", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/b/b5/Rapid_Fire_%28large%29.jpg?20081212204741" };
    public static readonly Skill RavenBlessing = new() { Id = 2384, Name = "Raven Blessing", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/0/0a/Raven_Blessing.jpg" };
    public static readonly Skill RavenousGaze = new() { Id = 862, Name = "Ravenous Gaze", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/40/Ravenous_Gaze_%28large%29.jpg?20081212203205" };
    public static readonly Skill RayofJudgment = new() { Id = 830, Name = "Ray of Judgment", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/a/a7/Ray_of_Judgment_%28large%29.jpg?20081212202542" };
    public static readonly Skill ReadtheWind = new() { Id = 432, Name = "Read the Wind", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/8b/Read_the_Wind_%28large%29.jpg?20090530233041" };
    public static readonly Skill ReadtheWindPvP = new() { Id = 2969, Name = "Read the Wind (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = ReadtheWind };
    public static readonly Skill ReapImpurities = new() { Id = 1486, Name = "Reap Impurities", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/b/b9/Reap_Impurities_%28large%29.jpg?20081212194730" };
    public static readonly Skill ReapersMark = new() { Id = 808, Name = "Reaper's Mark", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e2/Reaper%27s_Mark_%28large%29.jpg?20081212202824" };
    public static readonly Skill ReapersSweep = new() { Id = 1767, Name = "Reaper's Sweep", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/f/fa/Reaper%27s_Sweep_%28large%29.jpg?20081212195007" };
    public static readonly Skill Rebirth = new() { Id = 306, Name = "Rebirth", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/9f/Rebirth_%28large%29.jpg?20081212201930" };
    public static readonly Skill Recall = new() { Id = 925, Name = "Recall", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/ff/Recall_%28large%29.jpg?20081212194420" };
    public static readonly Skill RecklessHaste = new() { Id = 834, Name = "Reckless Haste", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/d2/Reckless_Haste_%28large%29.jpg?20081212203220" };
    public static readonly Skill ReclaimEssence = new() { Id = 1482, Name = "Reclaim Essence", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/d/d5/Reclaim_Essence_%28large%29.jpg?20081212205940" };
    public static readonly Skill Recovery = new() { Id = 1748, Name = "Recovery", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/02/Recovery_%28large%29.jpg?20081212205252" };
    public static readonly Skill RecoveryPvP = new() { Id = 3025, Name = "Recovery (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Recovery };
    public static readonly Skill Recuperation = new() { Id = 981, Name = "Recuperation", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/f/fc/Recuperation_%28large%29.jpg?20081212205744" };
    public static readonly Skill RecuperationPvP = new() { Id = 3013, Name = "Recuperation (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Recuperation };
    public static readonly Skill RecurringInsecurity = new() { Id = 1055, Name = "Recurring Insecurity", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/aa/Recurring_Insecurity_%28large%29.jpg?20081212201511" };
    public static readonly Skill Rejuvenation = new() { Id = 2204, Name = "Rejuvenation", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/b/b5/Rejuvenation_%28large%29.jpg?20081212205741" };
    public static readonly Skill RejuvenationPvP = new() { Id = 3039, Name = "Rejuvenation (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Rejuvenation };
    public static readonly Skill ReleaseEnchantments = new() { Id = 960, Name = "Release Enchantments", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/6/66/Release_Enchantments_%28large%29.jpg?20081212202507" };
    public static readonly Skill RemedySignet = new() { Id = 1777, Name = "Remedy Signet", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/5e/Remedy_Signet_%28large%29.jpg?20081212204048" };
    public static readonly Skill RemoveHex = new() { Id = 301, Name = "Remove Hex", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/b0/Remove_Hex_%28large%29.jpg?20081212202013" };
    public static readonly Skill RendEnchantments = new() { Id = 141, Name = "Rend Enchantments", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/a/ac/Rend_Enchantments_%28large%29.jpg?20081212203014" };
    public static readonly Skill RendingAura = new() { Id = 1765, Name = "Rending Aura", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/3/31/Rending_Aura_%28large%29.jpg?20081212194837" };
    public static readonly Skill RendingSweep = new() { Id = 1753, Name = "Rending Sweep", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/7/74/Rending_Sweep_%28large%29.jpg?20081212194918" };
    public static readonly Skill RendingTouch = new() { Id = 1534, Name = "Rending Touch", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/8/8e/Rending_Touch_%28large%29.jpg?20081212194801" };
    public static readonly Skill RenewLife = new() { Id = 1263, Name = "Renew Life", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/4/46/Renew_Life_%28large%29.jpg?20081212202205" };
    public static readonly Skill RenewingMemories = new() { Id = 1739, Name = "Renewing Memories", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/8f/Renewing_Memories_%28large%29.jpg?20081212205619" };
    public static readonly Skill RenewingSmash = new() { Id = 994, Name = "Renewing Smash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/6a/Renewing_Smash_%28large%29.jpg?20081212210258" };
    public static readonly Skill RenewingSmashPvP = new() { Id = 3143, Name = "Renewing Smash (PvP)", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "", IsPvP = true, AlternativeSkill = RenewingSmash };
    public static readonly Skill RenewingSurge = new() { Id = 1478, Name = "Renewing Surge", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/90/Renewing_Surge_%28large%29.jpg?20081212205421" };
    public static readonly Skill RepeatingStrike = new() { Id = 976, Name = "Repeating Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/2e/Repeating_Strike_%28large%29.jpg?20081212194151" };
    public static readonly Skill ResilientWasXiko = new() { Id = 1221, Name = "Resilient Was Xiko", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/4e/Resilient_Was_Xiko_%28large%29.jpg?20081212205550" };
    public static readonly Skill ResilientWeapon = new() { Id = 787, Name = "Resilient Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/a/ae/Resilient_Weapon_%28large%29.jpg?20090523005626" };
    public static readonly Skill RestfulBreeze = new() { Id = 886, Name = "Restful Breeze", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/06/Restful_Breeze_%28large%29.jpg?20081212202457" };
    public static readonly Skill Restoration = new() { Id = 963, Name = "Restoration", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/3/31/Restoration_%28large%29.jpg?20081212205309" };
    public static readonly Skill RestorationPvP = new() { Id = 3018, Name = "Restoration (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Restoration };
    public static readonly Skill RestoreCondition = new() { Id = 276, Name = "Restore Condition", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/9d/Restore_Condition_%28large%29.jpg?20081212201857" };
    public static readonly Skill RestoreLife = new() { Id = 314, Name = "Restore Life", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/8d/Restore_Life_%28large%29.jpg?20081212202019" };
    public static readonly Skill Resurrect = new() { Id = 305, Name = "Resurrect", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/6/64/Resurrect_%28large%29.jpg?20090531002537" };
    public static readonly Skill ResurrectionChant = new() { Id = 1128, Name = "Resurrection Chant", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/01/Resurrection_Chant_%28large%29.jpg?20081212202317" };
    public static readonly Skill ResurrectionSignet = new() { Id = 2, Name = "Resurrection Signet", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/e/e0/Resurrection_Signet.jpg" };
    public static readonly Skill Retreat = new() { Id = 839, Name = "Retreat!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/64/\"Retreat % 21\".jpg" };
    public static readonly Skill Retribution = new() { Id = 248, Name = "Retribution", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/f0/Retribution_%28large%29.jpg?20090531003231" };
    public static readonly Skill Return = new() { Id = 770, Name = "Return", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/0/05/Return_%28large%29.jpg?20081212194132" };
    public static readonly Skill RevealedEnchantment = new() { Id = 1048, Name = "Revealed Enchantment", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/8/8f/Revealed_Enchantment_%28large%29.jpg?20081212201655" };
    public static readonly Skill RevealedHex = new() { Id = 1049, Name = "Revealed Hex", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/4/40/Revealed_Hex_%28large%29.jpg?20081212200705" };
    public static readonly Skill ReversalofDamage = new() { Id = 1400, Name = "Reversal of Damage", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/7/77/Reversal_of_Damage_%28large%29.jpg?20081212202622" };
    public static readonly Skill ReversalofFortune = new() { Id = 307, Name = "Reversal of Fortune", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/5c/Reversal_of_Fortune_%28large%29.jpg?20081212202452" };
    public static readonly Skill ReverseHex = new() { Id = 848, Name = "Reverse Hex", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/2/27/Reverse_Hex_%28large%29.jpg?20081212201854" };
    public static readonly Skill ReviveAnimal = new() { Id = 422, Name = "Revive Animal", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/0/07/Revive_Animal_%28large%29.jpg?20081212204553" };
    public static readonly Skill RidetheLightning = new() { Id = 836, Name = "Ride the Lightning", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b8/Ride_the_Lightning_%28large%29.jpg?20081212195459" };
    public static readonly Skill RidetheLightningPvP = new() { Id = 2807, Name = "Ride the Lightning (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = RidetheLightning };
    public static readonly Skill RigorMortis = new() { Id = 137, Name = "Rigor Mortis", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/2/21/Rigor_Mortis_%28large%29.jpg?20081212202806" };
    public static readonly Skill RipEnchantment = new() { Id = 955, Name = "Rip Enchantment", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/ed/Rip_Enchantment_%28large%29.jpg?20081212202838" };
    public static readonly Skill Riposte = new() { Id = 387, Name = "Riposte", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/05/Riposte_%28large%29.jpg?20090531022105" };
    public static readonly Skill RisingBile = new() { Id = 935, Name = "Rising Bile", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/1/11/Rising_Bile_%28large%29.jpg?20081212203453" };
    public static readonly Skill RitualLord = new() { Id = 1217, Name = "Ritual Lord", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/0d/Ritual_Lord_%28large%29.jpg?20081212205536" };
    public static readonly Skill RoaringWinds = new() { Id = 1725, Name = "Roaring Winds", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/da/Roaring_Winds_%28large%29.jpg?20081212204752" };
    public static readonly Skill RodgortsInvocation = new() { Id = 189, Name = "Rodgort's Invocation", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/2/2c/Rodgort%27s_Invocation_%28large%29.jpg?20081212195520" };
    public static readonly Skill RottingFlesh = new() { Id = 106, Name = "Rotting Flesh", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/07/Rotting_Flesh_%28large%29.jpg?20090531004428" };
    public static readonly Skill RunasOne = new() { Id = 811, Name = "Run as One", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/83/Run_as_One_%28large%29.jpg?20081212204659" };
    public static readonly Skill RuptureSoul = new() { Id = 917, Name = "Rupture Soul", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/c/c7/Rupture_Soul_%28large%29.jpg?20081212205904" };
    public static readonly Skill Rush = new() { Id = 319, Name = "Rush", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/05/Rush_%28large%29.jpg?20090531022219" };
    public static readonly Skill Rust = new() { Id = 204, Name = "Rust", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/d2/Rust_%28large%29.jpg?20081212195805" };
    public static readonly Skill SadistsSignet = new() { Id = 1991, Name = "Sadist's Signet", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/4/49/Sadist%27s_Signet_%28large%29.jpg?20081212194006" };
    public static readonly Skill SandShards = new() { Id = 1510, Name = "Sand Shards", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/5c/Sand_Shards_%28large%29.jpg?20081212194914" };
    public static readonly Skill Sandstorm = new() { Id = 1372, Name = "Sandstorm", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/7/76/Sandstorm_%28large%29.jpg?20081212195401" };
    public static readonly Skill SavagePounce = new() { Id = 1201, Name = "Savage Pounce", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d6/Savage_Pounce_%28large%29.jpg?20081212204620" };
    public static readonly Skill SavageShot = new() { Id = 426, Name = "Savage Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/2/28/Savage_Shot_%28large%29.jpg?20081212205033" };
    public static readonly Skill SavageSlash = new() { Id = 390, Name = "Savage Slash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/c/cb/Savage_Slash_%28large%29.jpg?20081212210809" };
    public static readonly Skill SavannahHeat = new() { Id = 1380, Name = "Savannah Heat", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/9e/Savannah_Heat_%28large%29.jpg?20081212195615" };
    public static readonly Skill SavannahHeatPvP = new() { Id = 3021, Name = "Savannah Heat (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = SavannahHeat };
    public static readonly Skill SaveYourselves = new() { Id = 1954, Name = "Save Yourselves!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/1c/%22Save_Yourselves%21%22.jpg" };
    public static readonly Skill SaveYourselves2 = new() { Id = 2097, Name = "Save Yourselves!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/1c/%22Save_Yourselves%21%22.jpg" };
    public static readonly Skill ScavengerStrike = new() { Id = 440, Name = "Scavenger Strike", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/ff/Scavenger_Strike_%28large%29.jpg?20081212204522" };
    public static readonly Skill ScavengersFocus = new() { Id = 1471, Name = "Scavenger's Focus", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/7f/Scavenger%27s_Focus_%28large%29.jpg?20081212205031" };
    public static readonly Skill ScorpionWire = new() { Id = 815, Name = "Scorpion Wire", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/f1/Scorpion_Wire_%28large%29.jpg?20081212194256" };
    public static readonly Skill ScourgeEnchantment = new() { Id = 1398, Name = "Scourge Enchantment", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/5/55/Scourge_Enchantment_%28large%29.jpg?20081212202158" };
    public static readonly Skill ScourgeHealing = new() { Id = 251, Name = "Scourge Healing", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/31/Scourge_Healing_%28large%29.jpg?20081212202359" };
    public static readonly Skill ScourgeSacrifice = new() { Id = 253, Name = "Scourge Sacrifice", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/08/Scourge_Sacrifice_%28large%29.jpg?20081212202041" };
    public static readonly Skill ScreamingShot = new() { Id = 1719, Name = "Screaming Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/b/bf/Screaming_Shot_%28large%29.jpg?20081212204436" };
    public static readonly Skill ScribesInsight = new() { Id = 1684, Name = "Scribe's Insight", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/d/d1/Scribe%27s_Insight_%28large%29.jpg?20081212201835" };
    public static readonly Skill SearingFlames = new() { Id = 884, Name = "Searing Flames", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/3/3a/Searing_Flames_%28large%29.jpg?20081212195354" };
    public static readonly Skill SearingHeat = new() { Id = 196, Name = "Searing Heat", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/57/Searing_Heat_%28large%29.jpg?20090530230258" };
    public static readonly Skill SecondWind = new() { Id = 1088, Name = "Second Wind", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/81/Second_Wind_%28large%29.jpg?20081212195605" };
    public static readonly Skill SeedofLife = new() { Id = 2105, Name = "Seed of Life", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/7/74/Seed_of_Life.jpg" };
    public static readonly Skill SeekingArrows = new() { Id = 893, Name = "Seeking Arrows", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/83/Seeking_Arrows_%28large%29.jpg?20081212204420" };
    public static readonly Skill SeekingBlade = new() { Id = 386, Name = "Seeking Blade", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/9/9b/Seeking_Blade_%28large%29.jpg?20081212210108" };
    public static readonly Skill SeepingWound = new() { Id = 1034, Name = "Seeping Wound", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/f7/Seeping_Wound_%28large%29.jpg?20081212194301" };
    public static readonly Skill SelflessSpirit = new() { Id = 1952, Name = "Selfless Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/3e/Selfless_Spirit.jpg" };
    public static readonly Skill SelflessSpirit2 = new() { Id = 2095, Name = "Selfless Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/3e/Selfless_Spirit.jpg" };
    public static readonly Skill SerpentsQuickness = new() { Id = 456, Name = "Serpent's Quickness", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/8d/Serpent%27s_Quickness_%28large%29.jpg?20081212204359" };
    public static readonly Skill SevenWeaponsStance = new() { Id = 3426, Name = "Seven Weapons Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/d/d7/Seven_Weapons_Stance.jpg" };
    public static readonly Skill SeverArtery = new() { Id = 382, Name = "Sever Artery", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/8/83/Sever_Artery_%28large%29.jpg?20081212210055" };
    public static readonly Skill ShadowFang = new() { Id = 2052, Name = "Shadow Fang", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/9/9b/Shadow_Fang_%28large%29.jpg?20081212194109" };
    public static readonly Skill ShadowForm = new() { Id = 826, Name = "Shadow Form", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/d8/Shadow_Form_%28large%29.jpg?20081212194435" };
    public static readonly Skill ShadowFormPvP = new() { Id = 2862, Name = "Shadow Form (PvP)", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "", IsPvP = true, AlternativeSkill = ShadowForm };
    public static readonly Skill ShadowMeld = new() { Id = 1654, Name = "Shadow Meld", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/0/09/Shadow_Meld_%28large%29.jpg?20081212193933" };
    public static readonly Skill ShadowofFear = new() { Id = 136, Name = "Shadow of Fear", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/53/Shadow_of_Fear_%28large%29.jpg?20081212202757" };
    public static readonly Skill ShadowofHaste = new() { Id = 929, Name = "Shadow of Haste", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/a/a6/Shadow_of_Haste_%28large%29.jpg?20081212194011" };
    public static readonly Skill ShadowPrison = new() { Id = 1652, Name = "Shadow Prison", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/c2/Shadow_Prison_%28large%29.jpg?20081212194022" };
    public static readonly Skill ShadowRefuge = new() { Id = 814, Name = "Shadow Refuge", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/d1/Shadow_Refuge_%28large%29.jpg?20081212194552" };
    public static readonly Skill ShadowSanctuary = new() { Id = 1948, Name = "Shadow Sanctuary", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/1/17/Shadow_Sanctuary.jpg" };
    public static readonly Skill ShadowSanctuary2 = new() { Id = 2091, Name = "Shadow Sanctuary", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/1/17/Shadow_Sanctuary.jpg" };
    public static readonly Skill ShadowShroud = new() { Id = 928, Name = "Shadow Shroud", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/7/7d/Shadow_Shroud_%28large%29.jpg?20081212193937" };
    public static readonly Skill ShadowStrike = new() { Id = 102, Name = "Shadow Strike", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/1/1a/Shadow_Strike_%28large%29.jpg?20081212203207" };
    public static readonly Skill ShadowTheft = new() { Id = 3428, Name = "Shadow Theft", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/9/91/Shadow_Theft.jpg" };
    public static readonly Skill ShadowWalk = new() { Id = 1650, Name = "Shadow Walk", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/1/1e/Shadow_Walk_%28large%29.jpg?20081212194233" };
    public static readonly Skill Shadowsong = new() { Id = 871, Name = "Shadowsong", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/1/18/Shadowsong_%28large%29.jpg?20081212205450" };
    public static readonly Skill ShadowsongPvP = new() { Id = 3006, Name = "Shadowsong (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Shadowsong };
    public static readonly Skill ShadowyBurden = new() { Id = 950, Name = "Shadowy Burden", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/6/6b/Shadowy_Burden_%28large%29.jpg?20081212194443" };
    public static readonly Skill Shame = new() { Id = 51, Name = "Shame", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/a6/Shame_%28large%29.jpg?20081212201515" };
    public static readonly Skill ShamefulFear = new() { Id = 927, Name = "Shameful Fear", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/8/8c/Shameful_Fear_%28large%29.jpg?20081212194457" };
    public static readonly Skill ShardStorm = new() { Id = 213, Name = "Shard Storm", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/c2/Shard_Storm_%28large%29.jpg?20081212195819" };
    public static readonly Skill SharedBurden = new() { Id = 900, Name = "Shared Burden", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/4/48/Shared_Burden_%28large%29.jpg?20081212201250" };
    public static readonly Skill SharedBurdenPvP = new() { Id = 3186, Name = "Shared Burden (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = SharedBurden };
    public static readonly Skill SharpenDaggers = new() { Id = 926, Name = "Sharpen Daggers", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/5/56/Sharpen_Daggers_%28large%29.jpg?20081212194433" };
    public static readonly Skill ShatterDelusions = new() { Id = 27, Name = "Shatter Delusions", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/d1/Shatter_Delusions_%28large%29.jpg?20081212201703" };
    public static readonly Skill ShatterDelusionsPvP = new() { Id = 3180, Name = "Shatter Delusions (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = ShatterDelusions };
    public static readonly Skill ShatterEnchantment = new() { Id = 69, Name = "Shatter Enchantment", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/19/Shatter_Enchantment_%28large%29.jpg?20081212201303" };
    public static readonly Skill ShatterHex = new() { Id = 67, Name = "Shatter Hex", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/3/3f/Shatter_Hex_%28large%29.jpg?20081212201005" };
    public static readonly Skill ShatterStorm = new() { Id = 933, Name = "Shatter Storm", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/93/Shatter_Storm_%28large%29.jpg?20081212200313" };
    public static readonly Skill ShatteringAssault = new() { Id = 1634, Name = "Shattering Assault", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/bc/Shattering_Assault_%28large%29.jpg?20081212194126" };
    public static readonly Skill Shatterstone = new() { Id = 809, Name = "Shatterstone", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/58/Shatterstone_%28large%29.jpg?20081212195758" };
    public static readonly Skill ShellShock = new() { Id = 2059, Name = "Shell Shock", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/0/01/Shell_Shock_%28large%29.jpg?20081212195815" };
    public static readonly Skill Shelter = new() { Id = 982, Name = "Shelter", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/7/77/Shelter_%28large%29.jpg?20081212205245" };
    public static readonly Skill ShelterPvP = new() { Id = 3016, Name = "Shelter (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Shelter };
    public static readonly Skill ShieldBash = new() { Id = 363, Name = "Shield Bash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/8/8c/Shield_Bash_%28large%29.jpg?20081212210111" };
    public static readonly Skill ShieldGuardian = new() { Id = 885, Name = "Shield Guardian", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/ff/Shield_Guardian_%28large%29.jpg?20100404015655" };
    public static readonly Skill ShieldofAbsorption = new() { Id = 1399, Name = "Shield of Absorption", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/3f/Shield_of_Absorption_%28large%29.jpg?20081212202519" };
    public static readonly Skill ShieldofDeflection = new() { Id = 259, Name = "Shield of Deflection", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/1/1e/Shield_of_Deflection_%28large%29.jpg?20081212201820" };
    public static readonly Skill ShieldofForce = new() { Id = 2201, Name = "Shield of Force", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/f/fe/Shield_of_Force_%28large%29.jpg?20081212194725" };
    public static readonly Skill ShieldofJudgment = new() { Id = 262, Name = "Shield of Judgment", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/93/Shield_of_Judgment_%28large%29.jpg?20081212202447" };
    public static readonly Skill ShieldofRegeneration = new() { Id = 261, Name = "Shield of Regeneration", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c3/Shield_of_Regeneration_%28large%29.jpg?20081212202342" };
    public static readonly Skill ShieldStance = new() { Id = 378, Name = "Shield Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/4/41/Shield_Stance_%28large%29.jpg?20081212211016" };
    public static readonly Skill ShieldingHands = new() { Id = 299, Name = "Shielding Hands", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c6/Shielding_Hands_%28large%29.jpg?20081212201827" };
    public static readonly Skill ShieldsUp = new() { Id = 367, Name = "Shields Up!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/11/%22Shields_Up%21%22_%28large%29.jpg" };
    public static readonly Skill ShiversofDread = new() { Id = 1071, Name = "Shivers of Dread", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/6a/Shivers_of_Dread_%28large%29.jpg?20081212203139" };
    public static readonly Skill Shock = new() { Id = 231, Name = "Shock", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/8a/Shock_%28large%29.jpg?20081212195047" };
    public static readonly Skill ShockArrow = new() { Id = 1082, Name = "Shock Arrow", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/bc/Shock_Arrow_%28large%29.jpg?20081212195636" };
    public static readonly Skill Shockwave = new() { Id = 937, Name = "Shockwave", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/46/Shockwave_%28large%29.jpg?20081212195448" };
    public static readonly Skill Shove = new() { Id = 1146, Name = "Shove", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/01/Shove_%28large%29.jpg?20081212210356" };
    public static readonly Skill ShrinkingArmor = new() { Id = 2054, Name = "Shrinking Armor", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/7f/Shrinking_Armor_%28large%29.jpg?20081212201621" };
    public static readonly Skill ShroudofDistress = new() { Id = 1031, Name = "Shroud of Distress", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/20/Shroud_of_Distress_%28large%29.jpg?20081212194426" };
    public static readonly Skill ShroudofDistressPvP = new() { Id = 3048, Name = "Shroud of Distress (PvP)", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "", IsPvP = true, AlternativeSkill = ShroudofDistress };
    public static readonly Skill ShroudofSilence = new() { Id = 801, Name = "Shroud of Silence", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/8/87/Shroud_of_Silence_%28large%29.jpg?20081212194134" };
    public static readonly Skill SightBeyondSight = new() { Id = 1738, Name = "Sight Beyond Sight", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/a/af/Sight_Beyond_Sight_%28large%29.jpg?20081212205907" };
    public static readonly Skill SignetofAggression = new() { Id = 1776, Name = "Signet of Aggression", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/a/a6/Signet_of_Aggression_%28large%29.jpg?20081212204022" };
    public static readonly Skill SignetofAgony = new() { Id = 145, Name = "Signet of Agony", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/7e/Signet_of_Agony_%28large%29.jpg?20081212202845" };
    public static readonly Skill SignetofAgonyPvP = new() { Id = 3059, Name = "Signet of Agony (PvP)", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "", IsPvP = true, AlternativeSkill = SignetofAgony };
    public static readonly Skill SignetofBinding = new() { Id = 1743, Name = "Signet of Binding", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/f/fd/Signet_of_Binding_%28large%29.jpg?20081212205926" };
    public static readonly Skill SignetofCapture = new() { Id = 3, Name = "Signet of Capture", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/8/8d/Signet_of_Capture.jpg" };
    public static readonly Skill SignetofClumsiness = new() { Id = 1657, Name = "Signet of Clumsiness", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/14/Signet_of_Clumsiness_%28large%29.jpg?20081212195945" };
    public static readonly Skill SignetofClumsinessPvP = new() { Id = 3193, Name = "Signet of Clumsiness (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = SignetofClumsiness };
    public static readonly Skill SignetofCorruption = new() { Id = 1950, Name = "Signet of Corruption", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/1/18/Signet_of_Corruption.jpg" };
    public static readonly Skill SignetofCorruption2 = new() { Id = 2093, Name = "Signet of Corruption", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/1/18/Signet_of_Corruption.jpg" };
    public static readonly Skill SignetofCreation = new() { Id = 1238, Name = "Signet of Creation", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/1/14/Signet_of_Creation_%28large%29.jpg?20081212205719" };
    public static readonly Skill SignetofDeadlyCorruption = new() { Id = 2186, Name = "Signet of Deadly Corruption", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/4/49/Signet_of_Deadly_Corruption_%28large%29.jpg?20081212194323" };
    public static readonly Skill SignetofDeadlyCorruptionPvP = new() { Id = 3053, Name = "Signet of Deadly Corruption (PvP)", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "", IsPvP = true, AlternativeSkill = SignetofDeadlyCorruption };
    public static readonly Skill SignetofDevotion = new() { Id = 293, Name = "Signet of Devotion", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c0/Signet_of_Devotion_%28large%29.jpg?20081212202505" };
    public static readonly Skill SignetofDisenchantment = new() { Id = 882, Name = "Signet of Disenchantment", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/76/Signet_of_Disenchantment_%28large%29.jpg?20081212201420" };
    public static readonly Skill SignetofDisruption = new() { Id = 860, Name = "Signet of Disruption", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/c0/Signet_of_Disruption_%28large%29.jpg?20081212200035" };
    public static readonly Skill SignetofDistraction = new() { Id = 1992, Name = "Signet of Distraction", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/d5/Signet_of_Distraction_%28large%29.jpg?20081212201533" };
    public static readonly Skill SignetofGhostlyMight = new() { Id = 1742, Name = "Signet of Ghostly Might", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/1/18/Signet_of_Ghostly_Might_%28large%29.jpg?20081212205613" };
    public static readonly Skill SignetofGhostlyMightPvP = new() { Id = 2966, Name = "Signet of Ghostly Might (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = SignetofGhostlyMight };
    public static readonly Skill SignetofHumility = new() { Id = 62, Name = "Signet of Humility", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/5/56/Signet_of_Humility_%28large%29.jpg?20081212200053" };
    public static readonly Skill SignetofIllusions = new() { Id = 1346, Name = "Signet of Illusions", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/b/bf/Signet_of_Illusions_%28large%29.jpg?20081212201435" };
    public static readonly Skill SignetofInfection = new() { Id = 2229, Name = "Signet of Infection", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/6/66/Signet_of_Infection.jpg" };
    public static readonly Skill SignetofJudgment = new() { Id = 294, Name = "Signet of Judgment", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/f4/Signet_of_Judgment_%28large%29.jpg?20081212202402" };
    public static readonly Skill SignetofJudgmentPvP = new() { Id = 2887, Name = "Signet of Judgment (PvP)", AlternativeName = "", Profession = Profession.Monk, IconUrl = "", IsPvP = true, AlternativeSkill = SignetofJudgment };
    public static readonly Skill SignetofLostSouls = new() { Id = 1365, Name = "Signet of Lost Souls", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/f7/Signet_of_Lost_Souls_%28large%29.jpg?20081212203223" };
    public static readonly Skill SignetofMalice = new() { Id = 1036, Name = "Signet of Malice", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/8/80/Signet_of_Malice_%28large%29.jpg?20081212194303" };
    public static readonly Skill SignetofMidnight = new() { Id = 58, Name = "Signet of Midnight", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/e/e2/Signet_of_Midnight_%28large%29.jpg?20081212201630" };
    public static readonly Skill SignetofMysticSpeed = new() { Id = 2200, Name = "Signet of Mystic Speed", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/5b/Signet_of_Mystic_Speed_%28large%29.jpg?20081212195034" };
    public static readonly Skill SignetofMysticWrath = new() { Id = 1689, Name = "Signet of Mystic Wrath", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/fe/Signet_of_Mystic_Wrath_%28large%29.jpg?20081212202307" };
    public static readonly Skill SignetofPiousLight = new() { Id = 1530, Name = "Signet of Pious Light", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/7/7f/Signet_of_Pious_Light_%28large%29.jpg?20081212194702" };
    public static readonly Skill SignetofPiousRestraint = new() { Id = 2014, Name = "Signet of Pious Restraint", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/2/2c/Signet_of_Pious_Restraint_%28large%29.jpg?20081212194738" };
    public static readonly Skill SignetofPiousRestraintPvP = new() { Id = 3273, Name = "Signet of Pious Restraint (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = SignetofPiousRestraint };
    public static readonly Skill SignetofRage = new() { Id = 1269, Name = "Signet of Rage", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/1/1a/Signet_of_Rage_%28large%29.jpg?20081212202214" };
    public static readonly Skill SignetofRecall = new() { Id = 1993, Name = "Signet of Recall", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/a5/Signet_of_Recall_%28large%29.jpg?20081212201618" };
    public static readonly Skill SignetofRejuvenation = new() { Id = 887, Name = "Signet of Rejuvenation", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/bf/Signet_of_Rejuvenation_%28large%29.jpg?20081212201937" };
    public static readonly Skill SignetofRemoval = new() { Id = 1690, Name = "Signet of Removal", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c0/Signet_of_Removal_%28large%29.jpg?20081212201800" };
    public static readonly Skill SignetofReturn = new() { Id = 1778, Name = "Signet of Return", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/f/f9/Signet_of_Return_%28large%29.jpg?20081212204107" };
    public static readonly Skill SignetofReturnPvP = new() { Id = 3030, Name = "Signet of Return (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = SignetofReturn };
    public static readonly Skill SignetofShadows = new() { Id = 876, Name = "Signet of Shadows", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/e/e2/Signet_of_Shadows_%28large%29.jpg?20081212194335" };
    public static readonly Skill SignetofSorrow = new() { Id = 1363, Name = "Signet of Sorrow", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/98/Signet_of_Sorrow_%28large%29.jpg?20081212202816" };
    public static readonly Skill SignetofSpirits = new() { Id = 1239, Name = "Signet of Spirits", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/91/Signet_of_Spirits_%28large%29.jpg?20081212205635" };
    public static readonly Skill SignetofSpiritsPvP = new() { Id = 2965, Name = "Signet of Spirits (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = SignetofSpirits };
    public static readonly Skill SignetofStamina = new() { Id = 1411, Name = "Signet of Stamina", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/9/9b/Signet_of_Stamina_%28large%29.jpg?20081212211119" };
    public static readonly Skill SignetofStrength = new() { Id = 944, Name = "Signet of Strength", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/d/d1/Signet_of_Strength_%28large%29.jpg?20081212210412" };
    public static readonly Skill SignetofSuffering = new() { Id = 1364, Name = "Signet of Suffering", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/00/Signet_of_Suffering_%28large%29.jpg?20081212202759" };
    public static readonly Skill SignetofSynergy = new() { Id = 1585, Name = "Signet of Synergy", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/d/dc/Signet_of_Synergy_%28large%29.jpg?20081212204225" };
    public static readonly Skill SignetofToxicShock = new() { Id = 1647, Name = "Signet of Toxic Shock", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/4/4f/Signet_of_Toxic_Shock_%28large%29.jpg?20081212194241" };
    public static readonly Skill SignetofTwilight = new() { Id = 1648, Name = "Signet of Twilight", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/cd/Signet_of_Twilight_%28large%29.jpg?20081212194448" };
    public static readonly Skill SignetofWeariness = new() { Id = 59, Name = "Signet of Weariness", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/20/Signet_of_Weariness_%28large%29.jpg?20081212200156" };
    public static readonly Skill SilverwingSlash = new() { Id = 1144, Name = "Silverwing Slash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/f/f9/Silverwing_Slash_%28large%29.jpg?20081212210044" };
    public static readonly Skill SimpleThievery = new() { Id = 1350, Name = "Simple Thievery", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/7a/Simple_Thievery_%28large%29.jpg?20081212201614" };
    public static readonly Skill SiphonSpeed = new() { Id = 951, Name = "Siphon Speed", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/f9/Siphon_Speed_%28large%29.jpg?20081212194308" };
    public static readonly Skill SiphonStrength = new() { Id = 827, Name = "Siphon Strength", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/b/b5/Siphon_Strength_%28large%29.jpg?20081212194246" };
    public static readonly Skill SkullCrack = new() { Id = 329, Name = "Skull Crack", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/b/b5/Skull_Crack_%28large%29.jpg?20081212205959" };
    public static readonly Skill SlayersSpear = new() { Id = 1783, Name = "Slayer's Spear", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/7/74/Slayer%27s_Spear_%28large%29.jpg?20081212204219" };
    public static readonly Skill SlipperyGround = new() { Id = 2191, Name = "Slippery Ground", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/5/5d/Slippery_Ground_%28large%29.jpg?20081212195323" };
    public static readonly Skill SlipperyGroundPvP = new() { Id = 3398, Name = "Slippery Ground (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = SlipperyGround };
    public static readonly Skill SliverArmor = new() { Id = 1084, Name = "Sliver Armor", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/ae/Sliver_Armor_%28large%29.jpg?20081212195131" };
    public static readonly Skill SlothHuntersShot = new() { Id = 2069, Name = "Sloth Hunter's Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/f7/Sloth_Hunter%27s_Shot_%28large%29.jpg?20081212204732" };
    public static readonly Skill SlothHuntersShotPvP = new() { Id = 2925, Name = "Sloth Hunter's Shot (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = SlothHuntersShot };
    public static readonly Skill Smite = new() { Id = 240, Name = "Smite", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/1/1d/Smite_%28large%29.jpg?20090531003448" };
    public static readonly Skill SmiteCondition = new() { Id = 2004, Name = "Smite Condition", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/30/Smite_Condition_%28large%29.jpg?20081212202513" };
    public static readonly Skill SmiteHex = new() { Id = 302, Name = "Smite Hex", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/83/Smite_Hex_%28large%29.jpg?20081212201756" };
    public static readonly Skill SmitersBoon = new() { Id = 2005, Name = "Smiter's Boon", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c9/Smiter%27s_Boon_%28large%29.jpg?20081212201825" };
    public static readonly Skill SmitersBoonPvP = new() { Id = 2895, Name = "Smiter's Boon (PvP)", AlternativeName = "", Profession = Profession.Monk, IconUrl = "", IsPvP = true, AlternativeSkill = SmitersBoon };
    public static readonly Skill SmokePowderDefense = new() { Id = 2136, Name = "Smoke Powder Defense", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/29/Smoke_Powder_Defense_%28large%29.jpg?20081212194138" };
    public static readonly Skill SmokeTrap = new() { Id = 1729, Name = "Smoke Trap", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/18/Smoke_Trap_%28large%29.jpg?20081212205117" };
    public static readonly Skill SmolderingEmbers = new() { Id = 1090, Name = "Smoldering Embers", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/de/Smoldering_Embers_%28large%29.jpg?20081212195335" };
    public static readonly Skill SmoothCriminal = new() { Id = 2412, Name = "Smooth Criminal", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/3/33/Smooth_Criminal.jpg" };
    public static readonly Skill Snare = new() { Id = 854, Name = "Snare", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/6/67/Snare_%28large%29.jpg?20081212205222" };
    public static readonly Skill SneakAttack = new() { Id = 2116, Name = "Sneak Attack", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/8/87/Sneak_Attack.jpg" };
    public static readonly Skill SnowStorm = new() { Id = 2222, Name = "Snow Storm", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/a/a0/Snow_Storm.jpg" };
    public static readonly Skill SoldiersDefense = new() { Id = 1699, Name = "Soldier's Defense", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/c/cd/Soldier%27s_Defense_%28large%29.jpg?20081212211045" };
    public static readonly Skill SoldiersFury = new() { Id = 1773, Name = "Soldier's Fury", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/6/68/Soldier%27s_Fury_%28large%29.jpg?20081212203834" };
    public static readonly Skill SoldiersSpeed = new() { Id = 2196, Name = "Soldier's Speed", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/e/e6/Soldier%27s_Speed_%28large%29.jpg?20081212210136" };
    public static readonly Skill SoldiersStance = new() { Id = 1698, Name = "Soldier's Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/2/2d/Soldier%27s_Stance_%28large%29.jpg?20081212210114" };
    public static readonly Skill SoldiersStancePvP = new() { Id = 3156, Name = "Soldier's Stance (PvP)", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "", IsPvP = true, AlternativeSkill = SoldiersStance };
    public static readonly Skill SoldiersStrike = new() { Id = 1695, Name = "Soldier's Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/67/Soldier%27s_Strike_%28large%29.jpg?20081212210236" };
    public static readonly Skill SongofConcentration = new() { Id = 1567, Name = "Song of Concentration", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/8/85/Song_of_Concentration_%28large%29.jpg?20081212204003" };
    public static readonly Skill SongofPower = new() { Id = 1560, Name = "Song of Power", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/3/36/Song_of_Power_%28large%29.jpg?20081212203912" };
    public static readonly Skill SongofPurification = new() { Id = 1570, Name = "Song of Purification", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/5f/Song_of_Purification_%28large%29.jpg?20081212203929" };
    public static readonly Skill SongofRestoration = new() { Id = 1771, Name = "Song of Restoration", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/e2/Song_of_Restoration_%28large%29.jpg?20081212204204" };
    public static readonly Skill SongofRestorationPvP = new() { Id = 2878, Name = "Song of Restoration (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = SongofRestoration };
    public static readonly Skill Soothing = new() { Id = 1266, Name = "Soothing", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/6/6b/Soothing_%28large%29.jpg?20081212205716" };
    public static readonly Skill SoothingPvP = new() { Id = 3009, Name = "Soothing (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true , AlternativeSkill = Soothing };
    public static readonly Skill SoothingImages = new() { Id = 56, Name = "Soothing Images", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/d2/Soothing_Images_%28large%29.jpg?20081212200238" };
    public static readonly Skill SoothingMemories = new() { Id = 1233, Name = "Soothing Memories", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/9e/Soothing_Memories_%28large%29.jpg?20081212205312" };
    public static readonly Skill SoulBarbs = new() { Id = 100, Name = "Soul Barbs", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/5c/Soul_Barbs_%28large%29.jpg?20081212202827" };
    public static readonly Skill SoulBind = new() { Id = 901, Name = "Soul Bind", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/de/Soul_Bind_%28large%29.jpg?20081212203501" };
    public static readonly Skill SoulFeast = new() { Id = 96, Name = "Soul Feast", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/f4/Soul_Feast_%28large%29.jpg?20081212203408" };
    public static readonly Skill SoulLeech = new() { Id = 128, Name = "Soul Leech", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/4b/Soul_Leech_%28large%29.jpg?20081212203024" };
    public static readonly Skill SoulTaker = new() { Id = 3423, Name = "Soul Taker", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/4e/Soul_Taker.jpg" };
    public static readonly Skill SoulTwisting = new() { Id = 1240, Name = "Soul Twisting", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/1/18/Soul_Twisting_%28large%29.jpg?20081212205643" };
    public static readonly Skill SpearofFury = new() { Id = 1957, Name = "Spear of Fury", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/be/Spear_of_Fury.jpg" };
    public static readonly Skill SpearofFury2 = new() { Id = 2099, Name = "Spear of Fury", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/be/Spear_of_Fury.jpg" };
    public static readonly Skill SpearofLight = new() { Id = 1130, Name = "Spear of Light", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/91/Spear_of_Light_%28large%29.jpg?20081212202440" };
    public static readonly Skill SpearofLightning = new() { Id = 1551, Name = "Spear of Lightning", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/c/c9/Spear_of_Lightning_%28large%29.jpg?20081212204011" };
    public static readonly Skill SpearofRedemption = new() { Id = 2238, Name = "Spear of Redemption", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/d/d4/Spear_of_Redemption_%28large%29.jpg?20081212203735" };
    public static readonly Skill SpearSwipe = new() { Id = 2210, Name = "Spear Swipe", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/c/c9/Spear_Swipe_%28large%29.jpg?20081212204040" };
    public static readonly Skill SpellBreaker = new() { Id = 273, Name = "Spell Breaker", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/0a/Spell_Breaker_%28large%29.jpg?20081212202315" };
    public static readonly Skill SpellShield = new() { Id = 957, Name = "Spell Shield", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/39/Spell_Shield_%28large%29.jpg?20081212201813" };
    public static readonly Skill SpikeTrap = new() { Id = 461, Name = "Spike Trap", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5c/Spike_Trap_%28large%29.jpg?20081212204858" };
    public static readonly Skill SpinalShivers = new() { Id = 124, Name = "Spinal Shivers", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/ef/Spinal_Shivers_%28large%29.jpg?20081212203333" };
    public static readonly Skill SpiritBond = new() { Id = 1114, Name = "Spirit Bond", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/31/Spirit_Bond_%28large%29.jpg?20081212201802" };
    public static readonly Skill SpiritBondPvP = new() { Id = 2892, Name = "Spirit Bond (PvP)", AlternativeName = "", Profession = Profession.Monk, IconUrl = "", IsPvP = true, AlternativeSkill = SpiritBond };
    public static readonly Skill SpiritBoonStrike = new() { Id = 1226, Name = "Spirit Boon Strike", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/ef/Spirit_Boon_Strike_%28large%29.jpg?20081212205433" };
    public static readonly Skill SpiritBurn = new() { Id = 919, Name = "Spirit Burn", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/89/Spirit_Burn_%28large%29.jpg?20081212205901" };
    public static readonly Skill SpiritChanneling = new() { Id = 1231, Name = "Spirit Channeling", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/ea/Spirit_Channeling_%28large%29.jpg?20081212205412" };
    public static readonly Skill SpiritLight = new() { Id = 915, Name = "Spirit Light", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/f/fa/Spirit_Light_%28large%29.jpg?20081212205938" };
    public static readonly Skill SpiritLightWeapon = new() { Id = 1257, Name = "Spirit Light Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/8c/Spirit_Light_Weapon_%28large%29.jpg?20081212205604" };
    public static readonly Skill SpiritofFailure = new() { Id = 48, Name = "Spirit of Failure", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/2/22/Spirit_of_Failure_%28large%29.jpg?20081212200445" };
    public static readonly Skill SpiritRift = new() { Id = 910, Name = "Spirit Rift", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/5/55/Spirit_Rift_%28large%29.jpg?20081212205400" };
    public static readonly Skill SpiritShackles = new() { Id = 66, Name = "Spirit Shackles", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/0/00/Spirit_Shackles_%28large%29.jpg?20081212201459" };
    public static readonly Skill SpiritSiphon = new() { Id = 1228, Name = "Spirit Siphon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/d/d3/Spirit_Siphon_%28large%29.jpg?20081212205734" };
    public static readonly Skill SpirittoFlesh = new() { Id = 918, Name = "Spirit to Flesh", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/1/12/Spirit_to_Flesh_%28large%29.jpg?20081212205553" };
    public static readonly Skill SpiritTransfer = new() { Id = 962, Name = "Spirit Transfer", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/8f/Spirit_Transfer_%28large%29.jpg?20081212205503" };
    public static readonly Skill SpiritWalk = new() { Id = 1040, Name = "Spirit Walk", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/de/Spirit_Walk_%28large%29.jpg?20081212194156" };
    public static readonly Skill SpiritsGift = new() { Id = 1480, Name = "Spirit's Gift", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/ef/Spirit%27s_Gift_%28large%29.jpg?20081212205405" };
    public static readonly Skill SpiritsStrength = new() { Id = 1736, Name = "Spirit's Strength", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/84/Spirit%27s_Strength_%28large%29.jpg?20081212205259" };
    public static readonly Skill SpiritleechAura = new() { Id = 2203, Name = "Spiritleech Aura", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/5/58/Spiritleech_Aura_%28large%29.jpg?20081212205610" };
    public static readonly Skill SpiritualPain = new() { Id = 1336, Name = "Spiritual Pain", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/d/d8/Spiritual_Pain_%28large%29.jpg?20081212200047" };
    public static readonly Skill SpiritualPainPvP = new() { Id = 3189, Name = "Spiritual Pain (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = SpiritualPain };
    public static readonly Skill SpitefulSpirit = new() { Id = 121, Name = "Spiteful Spirit", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/37/Spiteful_Spirit_%28large%29.jpg?20081212202718" };
    public static readonly Skill SplinterShot = new() { Id = 852, Name = "Splinter Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/dc/Splinter_Shot_%28large%29.jpg?20081212204519" };
    public static readonly Skill SplinterWeapon = new() { Id = 792, Name = "Splinter Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/3/3d/Splinter_Weapon_%28large%29.jpg?20081212205418" };
    public static readonly Skill SplinterWeaponPvP = new() { Id = 2868, Name = "Splinter Weapon (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = SplinterWeapon };
    public static readonly Skill SpoilVictor = new() { Id = 1066, Name = "Spoil Victor", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/89/Spoil_Victor_%28large%29.jpg?20081212203136" };
    public static readonly Skill SpoilVictorPvP = new() { Id = 3233, Name = "Spoil Victor (PvP)", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "", IsPvP = true, AlternativeSkill = SpoilVictor };
    public static readonly Skill SpotlessMind = new() { Id = 2064, Name = "Spotless Mind", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/d/d0/Spotless_Mind_%28large%29.jpg?20081212201833" };
    public static readonly Skill SpotlessSoul = new() { Id = 2065, Name = "Spotless Soul", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c8/Spotless_Soul_%28large%29.jpg?20081212201847" };
    public static readonly Skill Sprint = new() { Id = 349, Name = "Sprint", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/64/Sprint_%28large%29.jpg?20090531021823" };
    public static readonly Skill StaggeringBlow = new() { Id = 360, Name = "Staggering Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/b/b9/Staggering_Blow_%28large%29.jpg?20081212211123" };
    public static readonly Skill StaggeringForce = new() { Id = 1498, Name = "Staggering Force", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/e/e7/Staggering_Force_%28large%29.jpg?20081212194907" };
    public static readonly Skill StandYourGround = new() { Id = 1589, Name = "Stand Your Ground!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/b/b5/%22Stand_Your_Ground%21%22_%28large%29.jpg" };
    public static readonly Skill StandYourGroundPvP = new() { Id = 3032, Name = "Stand Your Ground! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = StandYourGround };
    public static readonly Skill StandingSlash = new() { Id = 996, Name = "Standing Slash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/3a/Standing_Slash_%28large%29.jpg?20081212210002" };
    public static readonly Skill StarBurst = new() { Id = 1095, Name = "Star Burst", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/7/75/Star_Burst_%28large%29.jpg?20081212195617" };
    public static readonly Skill SteadyStance = new() { Id = 1701, Name = "Steady Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/d/d7/Steady_Stance_%28large%29.jpg?20081212210301" };
    public static readonly Skill Steam = new() { Id = 846, Name = "Steam", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a9/Steam_%28large%29.jpg?20081212195442" };
    public static readonly Skill SteelfangSlash = new() { Id = 1702, Name = "Steelfang Slash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/7/75/Steelfang_Slash_%28large%29.jpg?20081212210633" };
    public static readonly Skill StolenSpeed = new() { Id = 880, Name = "Stolen Speed", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/9e/Stolen_Speed_%28large%29.jpg?20081212201505" };
    public static readonly Skill StolenSpeedPvP = new() { Id = 3187, Name = "Stolen Speed (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = StolenSpeed };
    public static readonly Skill StoneDaggers = new() { Id = 172, Name = "Stone Daggers", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/7/7d/Stone_Daggers_%28large%29.jpg?20081212195526" };
    public static readonly Skill StoneSheath = new() { Id = 1373, Name = "Stone Sheath", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/4/41/Stone_Sheath_%28large%29.jpg?20081212195142" };
    public static readonly Skill StoneStriker = new() { Id = 1371, Name = "Stone Striker", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/3/32/Stone_Striker_%28large%29.jpg?20081212195450" };
    public static readonly Skill StonefleshAura = new() { Id = 1375, Name = "Stoneflesh Aura", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/e/e3/Stoneflesh_Aura_%28large%29.jpg?20081212195738" };
    public static readonly Skill StonesoulStrike = new() { Id = 1131, Name = "Stonesoul Strike", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/05/Stonesoul_Strike_%28large%29.jpg?20081212201940" };
    public static readonly Skill Stoning = new() { Id = 171, Name = "Stoning", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/8d/Stoning_%28large%29.jpg?20081212195051" };
    public static readonly Skill StormChaser = new() { Id = 455, Name = "Storm Chaser", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/15/Storm_Chaser_%28large%29.jpg?20090530233304" };
    public static readonly Skill StormDjinnsHaste = new() { Id = 1370, Name = "Storm Djinn's Haste", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/81/Storm_Djinn%27s_Haste_%28large%29.jpg?20081212195917" };
    public static readonly Skill StormsEmbrace = new() { Id = 1474, Name = "Storm's Embrace", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/0/04/Storm%27s_Embrace_%28large%29.jpg?20081212204920" };
    public static readonly Skill StrengthofHonor = new() { Id = 243, Name = "Strength of Honor", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c2/Strength_of_Honor_%28large%29.jpg?20081212202438" };
    public static readonly Skill StrengthofHonorPvP = new() { Id = 2999, Name = "Strength of Honor (PvP)", AlternativeName = "", Profession = Profession.Monk, IconUrl = "", IsPvP = true, AlternativeSkill = StrengthofHonor };
    public static readonly Skill StrikeasOne = new() { Id = 1468, Name = "Strike as One", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/7/73/Strike_as_One_%28large%29.jpg?20081212204414" };
    public static readonly Skill StripEnchantment = new() { Id = 143, Name = "Strip Enchantment", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/c/c5/Strip_Enchantment_%28large%29.jpg?20081212203647" };
    public static readonly Skill StunningStrike = new() { Id = 1602, Name = "Stunning Strike", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/1/1f/Stunning_Strike_%28large%29.jpg?20081212203853" };
    public static readonly Skill Succor = new() { Id = 308, Name = "Succor", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/b3/Succor_%28large%29.jpg?20081212202006" };
    public static readonly Skill Suffering = new() { Id = 108, Name = "Suffering", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/6b/Suffering_%28large%29.jpg?20081212203349" };
    public static readonly Skill SumofAllFears = new() { Id = 1996, Name = "Sum of All Fears", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/af/Sum_of_All_Fears_%28large%29.jpg?20081212200819" };
    public static readonly Skill SummonIceImp = new() { Id = 2226, Name = "Summon Ice Imp", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/2/2a/Summon_Ice_Imp.jpg" };
    public static readonly Skill SummonMursaat = new() { Id = 2224, Name = "Summon Mursaat", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/6/61/Summon_Mursaat.jpg" };
    public static readonly Skill SummonNagaShaman = new() { Id = 2227, Name = "Summon Naga Shaman", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/f/f0/Summon_Naga_Shaman.jpg" };
    public static readonly Skill SummonRubyDjinn = new() { Id = 2225, Name = "Summon Ruby Djinn", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/a/a0/Summon_Ruby_Djinn.jpg" };
    public static readonly Skill SummonSpirits = new() { Id = 2051, Name = "Summon Spirits", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/9c/Summon_Spirits.jpg" };
    public static readonly Skill SummonSpirits2 = new() { Id = 2100, Name = "Summon Spirits", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/9/9c/Summon_Spirits.jpg" };
    public static readonly Skill SunandMoonSlash = new() { Id = 851, Name = "Sun and Moon Slash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/4/4c/Sun_and_Moon_Slash_%28large%29.jpg?20081212211213" };
    public static readonly Skill SunderingAttack = new() { Id = 1191, Name = "Sundering Attack", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/12/Sundering_Attack_%28large%29.jpg?20081212204653" };
    public static readonly Skill SunderingAttackPvP = new() { Id = 2864, Name = "Sundering Attack (PvP)", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "", IsPvP = true, AlternativeSkill = SunderingAttack };
    public static readonly Skill SunderingWeapon = new() { Id = 2148, Name = "Sundering Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/6/6d/Sundering_Weapon_%28large%29.jpg?20081212205933" };
    public static readonly Skill SunspearRebirthSignet = new() { Id = 1816, Name = "Sunspear Rebirth Signet", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/e/e0/Sunspear_Rebirth_Signet.jpg" };
    public static readonly Skill SupportiveSpirit = new() { Id = 1391, Name = "Supportive Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/b/b1/Supportive_Spirit_%28large%29.jpg?20081212201806" };
    public static readonly Skill Swap = new() { Id = 1653, Name = "Swap", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/cb/Swap_%28large%29.jpg?20081212194430" };
    public static readonly Skill SwiftChop = new() { Id = 341, Name = "Swift Chop", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/f/f1/Swift_Chop_%28large%29.jpg?20081212210701" };
    public static readonly Skill SwiftJavelin = new() { Id = 1784, Name = "Swift Javelin", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/8/82/Swift_Javelin_%28large%29.jpg?20081212204217" };
    public static readonly Skill SwirlingAura = new() { Id = 233, Name = "Swirling Aura", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/0/03/Swirling_Aura_%28large%29.jpg?20081212195328" };
    public static readonly Skill Symbiosis = new() { Id = 468, Name = "Symbiosis", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5b/Symbiosis_%28large%29.jpg?20081212204408" };
    public static readonly Skill SymbioticBond = new() { Id = 423, Name = "Symbiotic Bond", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/8c/Symbiotic_Bond_%28large%29.jpg?20081212204927" };
    public static readonly Skill SymbolofWrath = new() { Id = 247, Name = "Symbol of Wrath", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/2/2b/Symbol_of_Wrath_%28large%29.jpg?20081212202038" };
    public static readonly Skill SymbolicCelerity = new() { Id = 1340, Name = "Symbolic Celerity", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/c5/Symbolic_Celerity_%28large%29.jpg?20081212201336" };
    public static readonly Skill SymbolicPosture = new() { Id = 1658, Name = "Symbolic Posture", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/7/75/Symbolic_Posture_%28large%29.jpg?20081212200103" };
    public static readonly Skill SymbolicStrike = new() { Id = 2195, Name = "Symbolic Strike", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/09/Symbolic_Strike_%28large%29.jpg?20081212210851" };
    public static readonly Skill SymbolsofInspiration = new() { Id = 1339, Name = "Symbols of Inspiration", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/a7/Symbols_of_Inspiration_%28large%29.jpg?20081212200247" };
    public static readonly Skill SympatheticVisage = new() { Id = 34, Name = "Sympathetic Visage", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/8/86/Sympathetic_Visage_%28large%29.jpg?20081212201118" };
    public static readonly Skill TaintedFlesh = new() { Id = 113, Name = "Tainted Flesh", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/69/Tainted_Flesh_%28large%29.jpg?20081212203448" };
    public static readonly Skill TasteofDeath = new() { Id = 152, Name = "Taste of Death", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/b2/Taste_of_Death_%28large%29.jpg?20081212203547" };
    public static readonly Skill TasteofPain = new() { Id = 1069, Name = "Taste of Pain", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/45/Taste_of_Pain_%28large%29.jpg?20081212203144" };
    public static readonly Skill Tease = new() { Id = 1342, Name = "Tease", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/8/82/Tease_%28large%29.jpg?20081212201653" };
    public static readonly Skill Technobabble = new() { Id = 2413, Name = "Technobabble", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/0/0a/Technobabble.jpg" };
    public static readonly Skill TeinaisCrystals = new() { Id = 1099, Name = "Teinai's Crystals", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/88/Teinai%27s_Crystals_%28large%29.jpg?20081212195351" };
    public static readonly Skill TeinaisHeat = new() { Id = 1093, Name = "Teinai's Heat", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a2/Teinai%27s_Heat_%28large%29.jpg?20081212195907" };
    public static readonly Skill TeinaisPrison = new() { Id = 1097, Name = "Teinai's Prison", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/aa/Teinai%27s_Prison_%28large%29.jpg?20081212195516" };
    public static readonly Skill TeinaisWind = new() { Id = 1081, Name = "Teinai's Wind", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/d/d4/Teinai%27s_Wind_%28large%29.jpg?20081212195642" };
    public static readonly Skill TempleStrike = new() { Id = 988, Name = "Temple Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/8/8f/Temple_Strike_%28large%29.jpg?20081212194437" };
    public static readonly Skill TestofFaith = new() { Id = 1545, Name = "Test of Faith", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/8/8f/Test_of_Faith_%28large%29.jpg?20081212194747" };
    public static readonly Skill ThePowerIsYours = new() { Id = 1782, Name = "The Power Is Yours!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/c/c5/%22The_Power_Is_Yours%21%22_%28large%29.jpg" };
    public static readonly Skill TheresNothingtoFear = new() { Id = 2112, Name = "There's Nothing to Fear!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/e/e6/%22There%27s_Nothing_to_Fear%21%22.jpg" };
    public static readonly Skill TheyreonFire = new() { Id = 1597, Name = "They're on Fire!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/8/81/%22They%27re_on_Fire%21%22_%28large%29.jpg" };
    public static readonly Skill ThrillofVictory = new() { Id = 324, Name = "Thrill of Victory", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/6/62/Thrill_of_Victory_%28large%29.jpg?20081212210047" };
    public static readonly Skill ThrowDirt = new() { Id = 424, Name = "Throw Dirt", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/da/Throw_Dirt_%28large%29.jpg?20081212204738" };
    public static readonly Skill Thunderclap = new() { Id = 228, Name = "Thunderclap", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/f/f8/Thunderclap_%28large%29.jpg?20081212195137" };
    public static readonly Skill TigerStance = new() { Id = 995, Name = "Tiger Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/1c/Tiger_Stance_%28large%29.jpg?20081212211050" };
    public static readonly Skill TigersFury = new() { Id = 454, Name = "Tiger's Fury", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/c/c6/Tiger%27s_Fury.jpg" };
    public static readonly Skill TimeWard = new() { Id = 3422, Name = "Time Ward", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/9/90/Time_Ward.jpg" };
    public static readonly Skill TotheLimit = new() { Id = 316, Name = "To the Limit!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/d/db/To_the_Limit%21_%28large%29.jpg?20081212211220" };
    public static readonly Skill Togetherasone = new() { Id = 3427, Name = "Together as one!", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/ff/%22Together_as_One%21%22.jpg" };
    public static readonly Skill TouchofAgony = new() { Id = 158, Name = "Touch of Agony", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/9a/Touch_of_Agony_%28large%29.jpg?20081212202809" };
    public static readonly Skill ToxicChill = new() { Id = 1659, Name = "Toxic Chill", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/3c/Toxic_Chill_%28large%29.jpg?20081212203202" };
    public static readonly Skill Toxicity = new() { Id = 1472, Name = "Toxicity", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/c/c3/Toxicity_%28large%29.jpg?20081212204900" };
    public static readonly Skill TramplingOx = new() { Id = 2135, Name = "Trampling Ox", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/f/f7/Trampling_Ox_%28large%29.jpg?20081212194409" };
    public static readonly Skill TranquilWasTanasen = new() { Id = 913, Name = "Tranquil Was Tanasen", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/e8/Tranquil_Was_Tanasen_%28large%29.jpg?20081212205415" };
    public static readonly Skill Tranquility = new() { Id = 1213, Name = "Tranquility", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/55/Tranquility_%28large%29.jpg?20081212204924" };
    public static readonly Skill TrappersFocus = new() { Id = 946, Name = "Trapper's Focus", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/3/3d/Trapper%27s_Focus_%28large%29.jpg?20081212204617" };
    public static readonly Skill TrappersSpeed = new() { Id = 1475, Name = "Trapper's Speed", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/1/1c/Trapper%27s_Speed_%28large%29.jpg?20081212204720" };
    public static readonly Skill TripleChop = new() { Id = 992, Name = "Triple Chop", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/0e/Triple_Chop_%28large%29.jpg?20081212210952" };
    public static readonly Skill TripleShot = new() { Id = 1953, Name = "Triple Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/f0/Triple_Shot.jpg" };
    public static readonly Skill TripleShot2 = new() { Id = 2096, Name = "Triple Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/f/f0/Triple_Shot.jpg" };
    public static readonly Skill Tripwire = new() { Id = 1476, Name = "Tripwire", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d2/Tripwire_%28large%29.jpg?20081212204357" };
    public static readonly Skill TrollUnguent = new() { Id = 446, Name = "Troll Unguent", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d3/Troll_Unguent_%28large%29.jpg?20081212204905" };
    public static readonly Skill TryptophanSignet = new() { Id = 2230, Name = "Tryptophan Signet", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/7/70/Tryptophan_Signet.jpg" };
    public static readonly Skill TwinMoonSweep = new() { Id = 1487, Name = "Twin Moon Sweep", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/2/2a/Twin_Moon_Sweep_%28large%29.jpg?20081212194849" };
    public static readonly Skill TwinMoonSweepPvP = new() { Id = 3264, Name = "Twin Moon Sweep (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = TwinMoonSweep };
    public static readonly Skill TwistingFangs = new() { Id = 776, Name = "Twisting Fangs", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/9/9e/Twisting_Fangs_%28large%29.jpg?20081212194141" };
    public static readonly Skill UlcerousLungs = new() { Id = 1358, Name = "Ulcerous Lungs", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/f7/Ulcerous_Lungs_%28large%29.jpg?20081212203402" };
    public static readonly Skill UnblockableThrow = new() { Id = 1550, Name = "Unblockable Throw", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/5/55/Unblockable_Throw_%28large%29.jpg?20081212204158" };
    public static readonly Skill UnholyFeast = new() { Id = 110, Name = "Unholy Feast", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e1/Unholy_Feast_%28large%29.jpg?20081212203339" };
    public static readonly Skill UnholyFeastPvP = new() { Id = 3058, Name = "Unholy Feast (PvP)", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "", IsPvP = true, AlternativeSkill = UnholyFeast };
    public static readonly Skill Union = new() { Id = 911, Name = "Union", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/b/b7/Union_%28large%29.jpg?20081212205630" };
    public static readonly Skill UnionPvP = new() { Id = 3005, Name = "Union (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Union };
    public static readonly Skill UnnaturalSignet = new() { Id = 934, Name = "Unnatural Signet", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/c/cd/Unnatural_Signet_%28large%29.jpg?20081212201320" };
    public static readonly Skill UnnaturalSignetPvP = new() { Id = 3188, Name = "Unnatural Signet (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = UnnaturalSignet };
    public static readonly Skill UnseenFury = new() { Id = 1041, Name = "Unseen Fury", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/0/0c/Unseen_Fury_%28large%29.jpg?20081212194123" };
    public static readonly Skill UnseenFuryPvP = new() { Id = 3049, Name = "Unseen Fury (PvP)", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "", IsPvP = true, AlternativeSkill = UnseenFury };
    public static readonly Skill UnsteadyGround = new() { Id = 1083, Name = "Unsteady Ground", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/a/a0/Unsteady_Ground_%28large%29.jpg?20081212195646" };
    public static readonly Skill UnsuspectingStrike = new() { Id = 783, Name = "Unsuspecting Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/7/71/Unsuspecting_Strike_%28large%29.jpg?20081212193956" };
    public static readonly Skill UnyieldingAura = new() { Id = 268, Name = "Unyielding Aura", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/e/e7/Unyielding_Aura_%28large%29.jpg?20081212202522" };
    public static readonly Skill UnyieldingAuraPvP = new() { Id = 2891, Name = "Unyielding Aura (PvP)", AlternativeName = "", Profession = Profession.Monk, IconUrl = "", IsPvP = true, AlternativeSkill = UnyieldingAura };
    public static readonly Skill UrsanBlessing = new() { Id = 2374, Name = "Ursan Blessing", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/7/7b/Ursan_Blessing.jpg" };
    public static readonly Skill VampiricAssault = new() { Id = 1986, Name = "Vampiric Assault", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/d/dc/Vampiric_Assault_%28large%29.jpg?20081212194549" };
    public static readonly Skill VampiricBite = new() { Id = 1077, Name = "Vampiric Bite", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/6/68/Vampiric_Bite_%28large%29.jpg?20081212203545" };
    public static readonly Skill VampiricGaze = new() { Id = 153, Name = "Vampiric Gaze", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e2/Vampiric_Gaze_%28large%29.jpg?20081212202847" };
    public static readonly Skill VampiricSpirit = new() { Id = 819, Name = "Vampiric Spirit", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/78/Vampiric_Spirit_%28large%29.jpg?20081212202841" };
    public static readonly Skill VampiricSwarm = new() { Id = 1075, Name = "Vampiric Swarm", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/03/Vampiric_Swarm_%28large%29.jpg?20081212203229" };
    public static readonly Skill VampiricTouch = new() { Id = 156, Name = "Vampiric Touch", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/75/Vampiric_Touch_%28large%29.jpg?20081212203148" };
    public static readonly Skill Vampirism = new() { Id = 2110, Name = "Vampirism", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/5/59/Vampirism.jpg" };
    public static readonly Skill VaporBlade = new() { Id = 866, Name = "Vapor Blade", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/1/16/Vapor_Blade_%28large%29.jpg?20081212195923" };
    public static readonly Skill VeilofThorns = new() { Id = 1757, Name = "Veil of Thorns", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/8/8e/Veil_of_Thorns_%28large%29.jpg?20081212195001" };
    public static readonly Skill Vengeance = new() { Id = 315, Name = "Vengeance", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/c/c1/Vengeance_%28large%29.jpg?20081212202323" };
    public static readonly Skill VengefulWasKhanhei = new() { Id = 790, Name = "Vengeful Was Khanhei", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/d/de/Vengeful_Was_Khanhei_%28large%29.jpg?20081212205930" };
    public static readonly Skill VengefulWeapon = new() { Id = 964, Name = "Vengeful Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/4/4e/Vengeful_Weapon_%28large%29.jpg?20081212205427" };
    public static readonly Skill VeratasAura = new() { Id = 88, Name = "Verata's Aura", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/f/fc/Verata%27s_Aura_%28large%29.jpg?20081212203122" };
    public static readonly Skill VeratasGaze = new() { Id = 87, Name = "Verata's Gaze", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/2/2c/Verata%27s_Gaze_%28large%29.jpg?20081212203549" };
    public static readonly Skill VeratasSacrifice = new() { Id = 90, Name = "Verata's Sacrifice", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/7d/Verata%27s_Sacrifice_%28large%29.jpg?20081212203516" };
    public static readonly Skill ViciousAttack = new() { Id = 1601, Name = "Vicious Attack", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/1/1e/Vicious_Attack_%28large%29.jpg?20081212204221" };
    public static readonly Skill VictoriousSweep = new() { Id = 1488, Name = "Victorious Sweep", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/0/09/Victorious_Sweep_%28large%29.jpg?20081212194700" };
    public static readonly Skill VictoryIsMine = new() { Id = 365, Name = "Victory Is Mine!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/0/0b/%22Victory_Is_Mine%21%22.jpg" };
    public static readonly Skill VigorousSpirit = new() { Id = 254, Name = "Vigorous Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/1/15/Vigorous_Spirit_%28large%29.jpg?20081212202031" };
    public static readonly Skill VileMiasma = new() { Id = 828, Name = "Vile Miasma", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/b0/Vile_Miasma_%28large%29.jpg?20081212203156" };
    public static readonly Skill VileTouch = new() { Id = 155, Name = "Vile Touch", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/81/Vile_Touch_%28large%29.jpg?20081212203320" };
    public static readonly Skill VipersDefense = new() { Id = 769, Name = "Viper's Defense", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/c/ce/Viper%27s_Defense_%28large%29.jpg?20081212194536" };
    public static readonly Skill VipersNest = new() { Id = 1211, Name = "Viper's Nest", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/c/c5/Viper%27s_Nest_%28large%29.jpg?20081212204247" };
    public static readonly Skill Virulence = new() { Id = 107, Name = "Virulence", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/3/31/Virulence_%28large%29.jpg?20081212203634" };
    public static readonly Skill VisionsofRegret = new() { Id = 878, Name = "Visions of Regret", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/1c/Visions_of_Regret_%28large%29.jpg?20081212201111" };
    public static readonly Skill VisionsofRegretPvP = new() { Id = 3234, Name = "Visions of Regret (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = VisionsofRegret };
    public static readonly Skill VitalBlessing = new() { Id = 289, Name = "Vital Blessing", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/d/df/Vital_Blessing_%28large%29.jpg?20081212202153" };
    public static readonly Skill VitalBoon = new() { Id = 1506, Name = "Vital Boon", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/5/50/Vital_Boon_%28large%29.jpg?20081212194620" };
    public static readonly Skill VitalWeapon = new() { Id = 1267, Name = "Vital Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/c/c0/Vital_Weapon_%28large%29.jpg?20081212205237" };
    public static readonly Skill VocalMinority = new() { Id = 883, Name = "Vocal Minority", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/d/d9/Vocal_Minority_%28large%29.jpg?20081212202803" };
    public static readonly Skill VocalWasSogolon = new() { Id = 1731, Name = "Vocal Was Sogolon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/5/55/Vocal_Was_Sogolon_%28large%29.jpg?20081212205621" };
    public static readonly Skill VolfenBlessing = new() { Id = 2379, Name = "Volfen Blessing", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/b/b2/Volfen_Blessing.jpg" };
    public static readonly Skill Volley = new() { Id = 2144, Name = "Volley", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/8/8e/Volley_%28large%29.jpg?20081212204335" };
    public static readonly Skill VowofPiety = new() { Id = 1505, Name = "Vow of Piety", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/2/25/Vow_of_Piety_%28large%29.jpg?20081212194924" };
    public static readonly Skill VowofRevolution = new() { Id = 3430, Name = "Vow of Revolution", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/4/48/Vow_of_Revolution.jpg" };
    public static readonly Skill VowofSilence = new() { Id = 1517, Name = "Vow of Silence", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/9/9c/Vow_of_Silence_%28large%29.jpg?20081212195024" };
    public static readonly Skill VowofStrength = new() { Id = 1759, Name = "Vow of Strength", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/c/c1/Vow_of_Strength_%28large%29.jpg?20081212194603" };
    public static readonly Skill WailofDoom = new() { Id = 764, Name = "Wail of Doom", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/bc/Wail_of_Doom_%28large%29.jpg?20081212202833" };
    public static readonly Skill WailingWeapon = new() { Id = 794, Name = "Wailing Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/84/Wailing_Weapon_%28large%29.jpg?20081212205624" };
    public static readonly Skill WallowsBite = new() { Id = 1078, Name = "Wallow's Bite", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/b/b3/Wallow%27s_Bite_%28large%29.jpg?20081212203146" };
    public static readonly Skill WanderingEye = new() { Id = 2056, Name = "Wandering Eye", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/a/a1/Wandering_Eye_%28large%29.jpg?20081212200714" };
    public static readonly Skill WanderingEyePvP = new() { Id = 3195, Name = "Wandering Eye (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = WanderingEye };
    public static readonly Skill Wanderlust = new() { Id = 1255, Name = "Wanderlust", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/88/Wanderlust_%28large%29.jpg?20081212205949" };
    public static readonly Skill WanderlustPvP = new() { Id = 3020, Name = "Wanderlust (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = Wanderlust };
    public static readonly Skill WardAgainstElements = new() { Id = 175, Name = "Ward Against Elements", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/7/71/Ward_Against_Elements_%28large%29.jpg?20081212195745" };
    public static readonly Skill WardAgainstFoes = new() { Id = 177, Name = "Ward Against Foes", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/b/b8/Ward_Against_Foes_%28large%29.jpg?20081212195732" };
    public static readonly Skill WardAgainstHarm = new() { Id = 239, Name = "Ward Against Harm", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/c8/Ward_Against_Harm_%28large%29.jpg?20081212195743" };
    public static readonly Skill WardAgainstHarmPvP = new() { Id = 2806, Name = "Ward Against Harm (PvP)", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "", IsPvP = true, AlternativeSkill = WardAgainstHarm };
    public static readonly Skill WardAgainstMelee = new() { Id = 176, Name = "Ward Against Melee", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/cc/Ward_Against_Melee_%28large%29.jpg?20081212195920" };
    public static readonly Skill WardofStability = new() { Id = 938, Name = "Ward of Stability", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/e/ea/Ward_of_Stability_%28large%29.jpg?20081212195310" };
    public static readonly Skill WardofWeakness = new() { Id = 2001, Name = "Ward of Weakness", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/0/05/Ward_of_Weakness_%28large%29.jpg?20081212195507" };
    public static readonly Skill WarmongersWeapon = new() { Id = 1751, Name = "Warmonger's Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/2/20/Warmonger%27s_Weapon_%28large%29.jpg?20081212205946" };
    public static readonly Skill WarriorsCunning = new() { Id = 362, Name = "Warrior's Cunning", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/e/e6/Warrior%27s_Cunning_%28large%29.jpg?20081212211033" };
    public static readonly Skill WarriorsEndurance = new() { Id = 374, Name = "Warrior's Endurance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/2/29/Warrior%27s_Endurance_%28large%29.jpg?20081212210117" };
    public static readonly Skill WarriorsEndurancePvP = new() { Id = 3002, Name = "Warrior's Endurance (PvP)", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "", IsPvP = true, AlternativeSkill = WarriorsEndurance };
    public static readonly Skill WaryStance = new() { Id = 377, Name = "Wary Stance", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/1/13/Wary_Stance_%28large%29.jpg?20081212211231" };
    public static readonly Skill WasteNotWantNot = new() { Id = 1995, Name = "Waste Not, Want Not", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/5/5c/Waste_Not%2C_Want_Not.jpg" };
    public static readonly Skill WastrelsCollapse = new() { Id = 1644, Name = "Wastrel's Collapse", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/e/e7/Wastrel%27s_Collapse_%28large%29.jpg?20081212194439" };
    public static readonly Skill WastrelsDemise = new() { Id = 1335, Name = "Wastrel's Demise", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/3/3d/Wastrel%27s_Demise_%28large%29.jpg?20081212201325" };
    public static readonly Skill WastrelsWorry = new() { Id = 50, Name = "Wastrel's Worry", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/4/4c/Wastrel%27s_Worry_%28large%29.jpg?20081212201623" };
    public static readonly Skill WatchYourself = new() { Id = 348, Name = "Watch Yourself!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/31/%22Watch_Yourself%21%22_%28large%29.jpg" };
    public static readonly Skill WatchYourselfPvP = new() { Id = 2858, Name = "Watch Yourself! (PvP)", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "", IsPvP = true, AlternativeSkill = WatchYourself };
    public static readonly Skill WatchfulHealing = new() { Id = 1392, Name = "Watchful Healing", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/3/3b/Watchful_Healing_%28large%29.jpg?20081212202345" };
    public static readonly Skill WatchfulIntervention = new() { Id = 1504, Name = "Watchful Intervention", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/4/4e/Watchful_Intervention_%28large%29.jpg?20081212194616" };
    public static readonly Skill WatchfulSpirit = new() { Id = 255, Name = "Watchful Spirit", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/9/99/Watchful_Spirit_%28large%29.jpg?20081212202435" };
    public static readonly Skill WaterAttunement = new() { Id = 208, Name = "Water Attunement", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/3/32/Water_Attunement_%28large%29.jpg?20081212195204" };
    public static readonly Skill WaterTrident = new() { Id = 237, Name = "Water Trident", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/c/c6/Water_Trident_%28large%29.jpg?20081212195505" };
    public static readonly Skill WayofPerfection = new() { Id = 1028, Name = "Way of Perfection", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/9/99/Way_of_Perfection_%28large%29.jpg?20081212194031" };
    public static readonly Skill WayoftheAssassin = new() { Id = 1649, Name = "Way of the Assassin", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/0/02/Way_of_the_Assassin_%28large%29.jpg?20081212194118" };
    public static readonly Skill WayoftheEmptyPalm = new() { Id = 987, Name = "Way of the Empty Palm", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/2/2b/Way_of_the_Empty_Palm_%28large%29.jpg?20081212194112" };
    public static readonly Skill WayoftheFox = new() { Id = 949, Name = "Way of the Fox", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/a/a3/Way_of_the_Fox_%28large%29.jpg?20081212194306" };
    public static readonly Skill WayoftheLotus = new() { Id = 977, Name = "Way of the Lotus", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/6/61/Way_of_the_Lotus_%28large%29.jpg?20081212194243" };
    public static readonly Skill WayoftheMaster = new() { Id = 2187, Name = "Way of the Master", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/e/eb/Way_of_the_Master_%28large%29.jpg?20081212194330" };
    public static readonly Skill WeShallReturn = new() { Id = 1592, Name = "We Shall Return!", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/a/a1/%22We_Shall_Return%21%22_%28large%29.jpg" };
    public static readonly Skill WeShallReturnPvP = new() { Id = 3033, Name = "We Shall Return! (PvP)", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "", IsPvP = true, AlternativeSkill = WeShallReturn };
    public static readonly Skill WeakenArmor = new() { Id = 159, Name = "Weaken Armor", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/5/53/Weaken_Armor_%28large%29.jpg?20081212203041" };
    public static readonly Skill WeakenKnees = new() { Id = 822, Name = "Weaken Knees", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/0e/Weaken_Knees_%28large%29.jpg?20081212203532" };
    public static readonly Skill WeaknessTrap = new() { Id = 2421, Name = "Weakness Trap", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/0/0d/Weakness_Trap.jpg" };
    public static readonly Skill WeaponofAggression = new() { Id = 2073, Name = "Weapon of Aggression", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/c/c3/Weapon_of_Aggression_%28large%29.jpg?20081212205935" };
    public static readonly Skill WeaponofFury = new() { Id = 1749, Name = "Weapon of Fury", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/7/70/Weapon_of_Fury_%28large%29.jpg?20081212205823" };
    public static readonly Skill WeaponofQuickening = new() { Id = 1268, Name = "Weapon of Quickening", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/d/d8/Weapon_of_Quickening_%28large%29.jpg?20081212205436" };
    public static readonly Skill WeaponofRemedy = new() { Id = 1752, Name = "Weapon of Remedy", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/c/c2/Weapon_of_Remedy_%28large%29.jpg?20081212205243" };
    public static readonly Skill WeaponofRenewal = new() { Id = 2149, Name = "Weapon of Renewal", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/e/e0/Weapon_of_Renewal_%28large%29.jpg?20081212205314" };
    public static readonly Skill WeaponofShadow = new() { Id = 983, Name = "Weapon of Shadow", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/00/Weapon_of_Shadow_%28large%29.jpg?20081212205739" };
    public static readonly Skill WeaponofWarding = new() { Id = 793, Name = "Weapon of Warding", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/5/58/Weapon_of_Warding_%28large%29.jpg?20081212205816" };
    public static readonly Skill WeaponofWardingPvP = new() { Id = 2893, Name = "Weapon of Warding (PvP)", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "", IsPvP = true, AlternativeSkill = WeaponofWarding };
    public static readonly Skill WeaponsofThreeForges = new() { Id = 3429, Name = "Weapons of Three Forges", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/0/08/Weapons_of_Three_Forges.jpg" };
    public static readonly Skill WearyingSpear = new() { Id = 1552, Name = "Wearying Spear", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/8/85/Wearying_Spear_%28large%29.jpg?20081212203658" };
    public static readonly Skill WearyingStrike = new() { Id = 1537, Name = "Wearying Strike", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/0/01/Wearying_Strike_%28large%29.jpg?20081212194618" };
    public static readonly Skill WebofDisruption = new() { Id = 1344, Name = "Web of Disruption", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "https://wiki.guildwars.com/images/1/11/Web_of_Disruption_%28large%29.jpg?20081212200926" };
    public static readonly Skill WebofDisruptionPvP = new() { Id = 3386, Name = "Web of Disruption (PvP)", AlternativeName = "", Profession = Profession.Mesmer, IconUrl = "", IsPvP = true, AlternativeSkill = WebofDisruption };
    public static readonly Skill WellofBlood = new() { Id = 92, Name = "Well of Blood", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e8/Well_of_Blood_%28large%29.jpg?20090531005205" };
    public static readonly Skill WellofDarkness = new() { Id = 1366, Name = "Well of Darkness", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/4/4b/Well_of_Darkness_%28large%29.jpg?20081212202942" };
    public static readonly Skill WellofPower = new() { Id = 91, Name = "Well of Power", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/a/a7/Well_of_Power_%28large%29.jpg?20081212203529" };
    public static readonly Skill WellofRuin = new() { Id = 2236, Name = "Well of Ruin", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/93/Well_of_Ruin_%28large%29.jpg?20081212203045" };
    public static readonly Skill WellofSilence = new() { Id = 1660, Name = "Well of Silence", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e3/Well_of_Silence_%28large%29.jpg?20081212202945" };
    public static readonly Skill WellofSuffering = new() { Id = 93, Name = "Well of Suffering", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/7/7b/Well_of_Suffering_%28large%29.jpg?20081212202954" };
    public static readonly Skill WelloftheProfane = new() { Id = 94, Name = "Well of the Profane", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/e/e9/Well_of_the_Profane_%28large%29.jpg?20081212203513" };
    public static readonly Skill WellofWeariness = new() { Id = 818, Name = "Well of Weariness", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/8/8c/Well_of_Weariness_%28large%29.jpg?20081212202850" };
    public static readonly Skill WhirlingAxe = new() { Id = 888, Name = "Whirling Axe", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/3/3f/Whirling_Axe_%28large%29.jpg?20081212211021" };
    public static readonly Skill WhirlingCharge = new() { Id = 1544, Name = "Whirling Charge", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/6/67/Whirling_Charge_%28large%29.jpg?20081212194735" };
    public static readonly Skill WhirlingDefense = new() { Id = 450, Name = "Whirling Defense", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/d/d4/Whirling_Defense_%28large%29.jpg?20081212204254" };
    public static readonly Skill Whirlwind = new() { Id = 163, Name = "Whirlwind", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/1/1e/Whirlwind_%28large%29.jpg?20090530225456" };
    public static readonly Skill WhirlwindAttack = new() { Id = 2107, Name = "Whirlwind Attack", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/2/2c/Whirlwind_Attack.jpg" };
    public static readonly Skill WieldersBoon = new() { Id = 1265, Name = "Wielder's Boon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/3/3b/Wielder%27s_Boon_%28large%29.jpg?20081212205408" };
    public static readonly Skill WieldersRemedy = new() { Id = 1740, Name = "Wielder's Remedy", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/8/8a/Wielder%27s_Remedy_%28large%29.jpg?20081212205801" };
    public static readonly Skill WieldersStrike = new() { Id = 1733, Name = "Wielder's Strike", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/b/b9/Wielder%27s_Strike_%28large%29.jpg?20081212205627" };
    public static readonly Skill WieldersZeal = new() { Id = 1737, Name = "Wielder's Zeal", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/1/1b/Wielder%27s_Zeal_%28large%29.jpg?20081212205752" };
    public static readonly Skill WildBlow = new() { Id = 321, Name = "Wild Blow", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/d/df/Wild_Blow_%28large%29.jpg?20081212210058" };
    public static readonly Skill WildStrike = new() { Id = 1022, Name = "Wild Strike", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "https://wiki.guildwars.com/images/5/5a/Wild_Strike_%28large%29.jpg?20081212194422" };
    public static readonly Skill WildStrikePvP = new() { Id = 3252, Name = "Wild Strike (PvP)", AlternativeName = "", Profession = Profession.Assassin, IconUrl = "", IsPvP = true, AlternativeSkill = WildStrike };
    public static readonly Skill WildThrow = new() { Id = 1605, Name = "Wild Throw", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/1/1f/Wild_Throw_%28large%29.jpg?20081212203652" };
    public static readonly Skill WindborneSpeed = new() { Id = 160, Name = "Windborne Speed", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/8/87/Windborne_Speed_%28large%29.jpg?20090530225823" };
    public static readonly Skill Winds = new() { Id = 2422, Name = "Winds", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/0/0e/Winds.jpg" };
    public static readonly Skill WindsofDisenchantment = new() { Id = 1533, Name = "Winds of Disenchantment", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/3/3d/Winds_of_Disenchantment_%28large%29.jpg?20081212195019" };
    public static readonly Skill Winnowing = new() { Id = 463, Name = "Winnowing", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/5/5a/Winnowing_%28large%29.jpg?20081212204729" };
    public static readonly Skill Winter = new() { Id = 462, Name = "Winter", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/e/e1/Winter_%28large%29.jpg?20081212204353" };
    public static readonly Skill WintersEmbrace = new() { Id = 1999, Name = "Winter's Embrace", AlternativeName = "", Profession = Profession.Elementalist, IconUrl = "https://wiki.guildwars.com/images/9/9d/Winter%27s_Embrace_%28large%29.jpg?20081212195308" };
    public static readonly Skill WithdrawHexes = new() { Id = 942, Name = "Withdraw Hexes", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/f/f1/Withdraw_Hexes_%28large%29.jpg?20081212202356" };
    public static readonly Skill Wither = new() { Id = 125, Name = "Wither", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/9/97/Wither_%28large%29.jpg?20081212203510" };
    public static readonly Skill WitheringAura = new() { Id = 1997, Name = "Withering Aura", AlternativeName = "", Profession = Profession.Necromancer, IconUrl = "https://wiki.guildwars.com/images/0/08/Withering_Aura_%28large%29.jpg?20081212202904" };
    public static readonly Skill WordofCensure = new() { Id = 1129, Name = "Word of Censure", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/7/76/Word_of_Censure_%28large%29.jpg?20081212202147" };
    public static readonly Skill WordofHealing = new() { Id = 282, Name = "Word of Healing", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/0/07/Word_of_Healing_%28large%29.jpg?20081212202334" };
    public static readonly Skill WordsofComfort = new() { Id = 1396, Name = "Words of Comfort", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/a/ab/Words_of_Comfort_%28large%29.jpg?20081212202617" };
    public static readonly Skill WoundingStrike = new() { Id = 1536, Name = "Wounding Strike", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/1/1b/Wounding_Strike_%28large%29.jpg?20081212194847" };
    public static readonly Skill WoundingStrikePvP = new() { Id = 3367, Name = "Wounding Strike (PvP)", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "", IsPvP = true, AlternativeSkill = WoundingStrike };
    public static readonly Skill XinraesWeapon = new() { Id = 1750, Name = "Xinrae's Weapon", AlternativeName = "", Profession = Profession.Ritualist, IconUrl = "https://wiki.guildwars.com/images/5/51/Xinrae%27s_Weapon_%28large%29.jpg?20081212205640" };
    public static readonly Skill YetiSmash = new() { Id = 1137, Name = "Yeti Smash", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/7/73/Yeti_Smash_%28large%29.jpg?20081212210835" };
    public static readonly Skill YouAreAllWeaklings = new() { Id = 2359, Name = "You Are All Weaklings!", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/a/a4/%22You_Are_All_Weaklings%21%22.jpg" };
    public static readonly Skill YouMoveLikeaDwarf = new() { Id = 2358, Name = "You Move Like a Dwarf!", AlternativeName = "", Profession = Profession.None, IconUrl = "https://wiki.guildwars.com/images/6/6a/%22You_Move_Like_a_Dwarf%21%22.jpg" };
    public static readonly Skill YouWillDie = new() { Id = 1141, Name = "You Will Die!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/a2/%22You_Will_Die%21%22_%28large%29.jpg" };
    public static readonly Skill YoureAllAlone = new() { Id = 1412, Name = "You're All Alone!", AlternativeName = "", Profession = Profession.Warrior, IconUrl = "https://wiki.guildwars.com/images/a/ad/%22You%27re_All_Alone%21%22_%28large%29.jpg" };
    public static readonly Skill ZealotsFire = new() { Id = 271, Name = "Zealot's Fire", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/4/4f/Zealot%27s_Fire_%28large%29.jpg?20081212202211" };
    public static readonly Skill ZealousAnthem = new() { Id = 1561, Name = "Zealous Anthem", AlternativeName = "", Profession = Profession.Paragon, IconUrl = "https://wiki.guildwars.com/images/6/6e/Zealous_Anthem_%28large%29.jpg?20081212203819" };
    public static readonly Skill ZealousBenediction = new() { Id = 1687, Name = "Zealous Benediction", AlternativeName = "", Profession = Profession.Monk, IconUrl = "https://wiki.guildwars.com/images/8/81/Zealous_Benediction_%28large%29.jpg?20081212202202" };
    public static readonly Skill ZealousRenewal = new() { Id = 1763, Name = "Zealous Renewal", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/a/a0/Zealous_Renewal_%28large%29.jpg?20081212194611" };
    public static readonly Skill ZealousSweep = new() { Id = 2071, Name = "Zealous Sweep", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/d/db/Zealous_Sweep_%28large%29.jpg?20081212194744" };
    public static readonly Skill ZealousVow = new() { Id = 1761, Name = "Zealous Vow", AlternativeName = "", Profession = Profession.Dervish, IconUrl = "https://wiki.guildwars.com/images/0/0c/Zealous_Vow_%28large%29.jpg?20081212195011" };
    public static readonly Skill ZojunsHaste = new() { Id = 1196, Name = "Zojun's Haste", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/b/bd/Zojun%27s_Haste_%28large%29.jpg?20081225000409" };
    public static readonly Skill ZojunsShot = new() { Id = 1192, Name = "Zojun's Shot", AlternativeName = "", Profession = Profession.Ranger, IconUrl = "https://wiki.guildwars.com/images/a/a4/Zojun%27s_Shot_%28large%29.jpg?20081225003554" };

    public static readonly IReadOnlyCollection<Skill> Skills = [.. new List<Skill>
    {
        None,
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

    public static bool TryParse(int id, [NotNullWhen(true)] out Skill? skill)
    {
        skill = Skills.Where(skill => skill.Id == id).FirstOrDefault();
        if (skill is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, [NotNullWhen(true)] out Skill? skill)
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

    public required Profession Profession { get; init; }
    public required string Name { get; init; }
    public required int Id { get; init; }
    public required string IconUrl { get; init; }
    public string? AlternativeName { get; private set; }
    public Skill? AlternativeSkill { get; private set; }
    public bool IsPvP { get; private set; }
    public override string ToString() => this.Name ?? this.AlternativeName ?? nameof(Skill);
    private Skill()
    {
    }
}
