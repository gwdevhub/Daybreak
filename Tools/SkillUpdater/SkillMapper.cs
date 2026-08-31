namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// Turns one <see cref="GwToolboxSkill"/> into the literal C# tokens the writer
/// emits. Every value Daybreak stores comes from here; the wiki only supplies
/// the roster (which ids exist, under which display name) and the icon URL.
/// </summary>
/// <remarks>
/// Where the client's data model differs from Daybreak's, the translation is
/// derived from the client's own encoding rather than guessed:
/// <list type="bullet">
///   <item>energy is an encoded cost, not a literal one (11 → 15, 12 → 25);</item>
///   <item>adrenaline is stored in 25ths of a strike;</item>
///   <item>health cost is a whole percentage where Daybreak stores a fraction;</item>
///   <item>upkeep is not a field at all — it is what the client's "maintained"
///         duration sentinel means;</item>
///   <item><c>GW::Constants::SkillType</c> is a single value, where Daybreak's
///         <c>SkillType</c> is a flags enum, so the sub-type flags are recovered
///         from the accompanying weapon/combo/profession fields.</item>
/// </list>
/// </remarks>
internal static class SkillMapper
{
    /// <summary>
    /// <c>duration0</c> of a maintained enchantment. The client has no upkeep
    /// field; a maintained enchantment is exactly the set of skills Daybreak
    /// records as <c>Upkeep = -1</c>.
    /// </summary>
    public const int MaintainedDuration = 131072;

    /// <summary>Adrenaline units the client stores per strike.</summary>
    private const int AdrenalinePerStrike = 25;

    /// <summary>
    /// The client's "this skill has no attribute" placeholder, which the API
    /// used to publish verbatim before it started omitting the key instead.
    /// </summary>
    private const int NoAttribute = 51;

    public static ParsedSkill Map(int id, string name, GwToolboxSkill skill) =>
        new(
            Id: id,
            Name: name,
            CampaignIdentifier: ResolveCampaign(skill.Campaign),
            ProfessionIdentifier: ResolveProfession(skill.Profession),
            AttributeIdentifier: ResolveAttribute(skill.Attribute),
            PvEOnly: skill.PvEOnly != 0,
            PvP: skill.PvPOnly != 0,
            Elite: skill.Elite != 0,
            TypeExpression: ResolveSkillType(skill),
            Energy: NullIfZero(DecodeEnergyCost(skill.EnergyCost)),
            Activation: NullIfZero(skill.Activation),
            Recharge: NullIfZero(skill.Recharge),
            Overcast: NullIfZero(skill.Overcast),
            Adrenaline: NullIfZero(DecodeAdrenaline(skill.Adrenaline)),
            Sacrifice: NullIfZero(skill.HealthCost / 100.0),
            Upkeep: skill.Duration0 == MaintainedDuration ? -1 : null,
            Description: skill.Description ?? string.Empty,
            ConciseDescription: skill.Concise ?? string.Empty);

    private static double? NullIfZero(double value) => value == 0 ? null : value;

    /// <summary>
    /// The client stores energy as a code, not a cost: everything up to 10 is
    /// the cost itself, and the two costs above it get their own codes.
    /// </summary>
    private static double DecodeEnergyCost(int raw) => raw switch
    {
        11 => 15,
        12 => 25,
        _ => raw,
    };

    /// <summary>
    /// Adrenaline is stored in 25ths of a strike, but a skill costing a whole
    /// number of strikes can still hold a value that is not a clean multiple
    /// (an 80 that means four strikes), so the count rounds up.
    /// </summary>
    private static double DecodeAdrenaline(int raw) =>
        raw == 0 ? 0 : Math.Ceiling(raw / (double)AdrenalinePerStrike);

    private static string ResolveCampaign(int campaign) =>
        campaign >= 0 && campaign < Campaigns.Length ? Campaigns[campaign] : "None";

    private static string ResolveProfession(int profession) =>
        profession >= 0 && profession < Professions.Length ? Professions[profession] : "None";

    private static string ResolveAttribute(int? attribute) =>
        attribute is not int id || id == NoAttribute
            ? "None"
            : Attributes.GetValueOrDefault(id, "None");

    /// <summary>
    /// Expands the client's single skill type into Daybreak's flag set, adding
    /// the sub-type flags the type alone does not carry.
    /// </summary>
    private static string ResolveSkillType(GwToolboxSkill skill)
    {
        var flags = new List<string>();
        void Add(string flag)
        {
            if (!flags.Contains(flag, StringComparer.Ordinal))
            {
                flags.Add(flag);
            }
        }

        switch (skill.Type)
        {
            case GwSkillType.Stance: Add("Stance"); break;
            case GwSkillType.Hex: Add("Hex"); Add("Spell"); break;
            case GwSkillType.Spell: Add("Spell"); break;
            case GwSkillType.Enchantment:
                // A flash enchantment is one that takes no time to cast.
                if (skill.Activation == 0)
                {
                    Add("Flash");
                }

                Add("Enchantment");
                Add("Spell");
                break;
            case GwSkillType.Signet: Add("Signet"); break;
            case GwSkillType.Well: Add("Well"); Add("Spell"); break;
            case GwSkillType.Skill or GwSkillType.Skill2 or GwSkillType.Passive or GwSkillType.Environmental:
                Add("Skill");
                break;
            case GwSkillType.Ward: Add("Ward"); Add("Spell"); break;
            case GwSkillType.Glyph: Add("Glyph"); break;
            case GwSkillType.Attack:
                AddWeaponFlags(skill, Add);
                Add("Attack");
                break;
            case GwSkillType.Shout: Add("Shout"); break;
            case GwSkillType.Preparation: Add("Preparation"); break;
            case GwSkillType.PetAttack: Add("Pet"); Add("Attack"); break;
            case GwSkillType.Trap or GwSkillType.EnvironmentalTrap: Add("Trap"); break;
            case GwSkillType.Ritual:
                Add(ResolveRitualFlag(skill.Profession));
                Add("Ritual");
                break;
            case GwSkillType.ItemSpell: Add("Item"); Add("Spell"); break;
            case GwSkillType.WeaponSpell: Add("Weapon"); Add("Spell"); break;
            case GwSkillType.Form: Add("Form"); break;
            case GwSkillType.Chant: Add("Chant"); break;
            case GwSkillType.EchoRefrain: Add("Echo"); break;
            default:
                // Bounty, Scroll, Condition, Title and Disguise have no Daybreak
                // counterpart; they are effects rather than usable skills.
                break;
        }

        if (skill.TouchRange != 0)
        {
            flags.Insert(0, "Touch");
        }

        return flags.Count == 0
            ? "SkillType.None"
            : string.Join(" | ", flags.Select(f => $"SkillType.{f}"));
    }

    /// <summary>
    /// A ritual's kind is not stored; the profession that owns it is what tells
    /// binding, nature and Ebon Vanguard rituals apart.
    /// </summary>
    private static string ResolveRitualFlag(int profession) => profession switch
    {
        GwProfession.Ritualist => "Binding",
        GwProfession.Ranger => "Nature",
        _ => "EbonVanguard",
    };

    /// <summary>
    /// Recovers the attack's weapon flags from the requirement bitmask. A mask
    /// naming exactly one weapon is that weapon; one naming several is a
    /// generic melee or ranged attack, which is how Daybreak models it.
    /// </summary>
    private static void AddWeaponFlags(GwToolboxSkill skill, Action<string> add)
    {
        var requirement = skill.WeaponRequirement;
        if (requirement == 0)
        {
            return;
        }

        // Daggers have no flag of their own: a dagger attack is identified by
        // its place in the attack chain instead.
        if (requirement == GwWeapon.Daggers)
        {
            AddComboFlag(skill.Combo, add);
            return;
        }

        if (SingleWeaponFlags.TryGetValue(requirement, out var weapon))
        {
            add(weapon);
            return;
        }

        var meleeWeapons = System.Numerics.BitOperations.PopCount((uint)(requirement & GwWeapon.AnyMelee));
        var rangedWeapons = System.Numerics.BitOperations.PopCount((uint)(requirement & GwWeapon.AnyRanged));

        // A mask spanning both melee and ranged weapons is "any weapon at all",
        // which is no restriction to record.
        if (meleeWeapons > 1 && rangedWeapons > 1)
        {
            return;
        }

        if (meleeWeapons > 1)
        {
            add("Melee");
        }
        else if (rangedWeapons > 1)
        {
            add("Ranged");
        }
    }

    /// <summary>Dagger attacks additionally carry their place in the attack chain.</summary>
    private static void AddComboFlag(int combo, Action<string> add)
    {
        switch (combo)
        {
            case 1: add("Lead"); break;
            case 2: add("OffHand"); break;
            case 3: add("Dual"); break;
        }
    }

    /// <summary><c>GW::Constants::SkillType</c>.</summary>
    private static class GwSkillType
    {
        public const int Stance = 3;
        public const int Hex = 4;
        public const int Spell = 5;
        public const int Enchantment = 6;
        public const int Signet = 7;
        public const int Well = 9;
        public const int Skill = 10;
        public const int Ward = 11;
        public const int Glyph = 12;
        public const int Attack = 14;
        public const int Shout = 15;
        public const int Skill2 = 16;
        public const int Passive = 17;
        public const int Environmental = 18;
        public const int Preparation = 19;
        public const int PetAttack = 20;
        public const int Trap = 21;
        public const int Ritual = 22;
        public const int EnvironmentalTrap = 23;
        public const int ItemSpell = 24;
        public const int WeaponSpell = 25;
        public const int Form = 26;
        public const int Chant = 27;
        public const int EchoRefrain = 28;
    }

    private static class GwProfession
    {
        public const int Ranger = 2;
        public const int Ritualist = 8;
    }

    private static class GwWeapon
    {
        public const int Axe = 0x01;
        public const int Bow = 0x02;
        public const int Daggers = 0x08;
        public const int Hammer = 0x10;
        public const int Scythe = 0x20;
        public const int Spear = 0x40;
        public const int Sword = 0x80;

        /// <summary>Bit 0x04 has no Daybreak counterpart but is a ranged weapon.</summary>
        public const int AnyRanged = Bow | 0x04 | Spear;
        public const int AnyMelee = Axe | Daggers | Hammer | Scythe | Sword;
    }

    private static readonly Dictionary<int, string> SingleWeaponFlags = new()
    {
        [GwWeapon.Axe] = "Axe",
        [GwWeapon.Bow] = "Bow",
        [GwWeapon.Hammer] = "Hammer",
        [GwWeapon.Scythe] = "Scythe",
        [GwWeapon.Spear] = "Spear",
        [GwWeapon.Sword] = "Sword",
    };

    /// <summary>Indexed by the API's campaign id.</summary>
    private static readonly string[] Campaigns =
    [
        "Core",
        "Prophecies",
        "Factions",
        "Nightfall",
        "EyeOfTheNorth",
        "BonusMissionPack",
    ];

    /// <summary>Indexed by the API's profession id.</summary>
    private static readonly string[] Professions =
    [
        "None",
        "Warrior",
        "Ranger",
        "Monk",
        "Necromancer",
        "Mesmer",
        "Elementalist",
        "Assassin",
        "Ritualist",
        "Paragon",
        "Dervish",
    ];

    /// <summary>
    /// Keyed by the API's attribute id, which is
    /// <c>GW::Constants::AttributeByte</c> — the same id Daybreak's
    /// <c>Attribute</c> already carries. Ids with no attribute are absent and
    /// resolve to <c>None</c>.
    /// </summary>
    private static readonly Dictionary<int, string> Attributes = new()
    {
        [0] = "FastCasting",
        [1] = "IllusionMagic",
        [2] = "DominationMagic",
        [3] = "InspirationMagic",
        [4] = "BloodMagic",
        [5] = "DeathMagic",
        [6] = "SoulReaping",
        [7] = "Curses",
        [8] = "AirMagic",
        [9] = "EarthMagic",
        [10] = "FireMagic",
        [11] = "WaterMagic",
        [12] = "EnergyStorage",
        [13] = "HealingPrayers",
        [14] = "SmitingPrayers",
        [15] = "ProtectionPrayers",
        [16] = "DivineFavor",
        [17] = "Strength",
        [18] = "AxeMastery",
        [19] = "HammerMastery",
        [20] = "Swordsmanship",
        [21] = "Tactics",
        [22] = "BeastMastery",
        [23] = "Expertise",
        [24] = "WildernessSurvival",
        [25] = "Marksmanship",
        [29] = "DaggerMastery",
        [30] = "DeadlyArts",
        [31] = "ShadowArts",
        [32] = "Communing",
        [33] = "RestorationMagic",
        [34] = "ChannelingMagic",
        [35] = "CriticalStrikes",
        [36] = "SpawningPower",
        [37] = "SpearMastery",
        [38] = "Command",
        [39] = "Motivation",
        [40] = "Leadership",
        [41] = "ScytheMastery",
        [42] = "WindPrayers",
        [43] = "EarthPrayers",
        [44] = "Mysticism",
    };
}
