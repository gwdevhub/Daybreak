using System.Numerics;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
[GWCAEquivalent("WorldContext")]
public readonly unsafe struct WorldContext
{
    [FieldOffset(0x0000)]
    public readonly AccountInfoContext* AccountInfo;

    [FieldOffset(0x0024)]
    public readonly GuildWarsArray<uint> MerchItems;

    [FieldOffset(0x0034)]
    public readonly GuildWarsArray<uint> MerchItems2;

    [FieldOffset(0x007C)]
    public readonly GuildWarsArray<MapAgentContext> MapAgents;

    [FieldOffset(0x009C)]
    public readonly Vector3 AllFlag;

    [FieldOffset(0x00AC)]
    public readonly GuildWarsArray<PartyAttribute> Attributes;

    [FieldOffset(0x0508)]
    public readonly GuildWarsArray<AgentEffects> PartyEffects;

    [FieldOffset(0x0528)]
    public readonly uint ActiveQuestId;

    [FieldOffset(0x052C)]
    public readonly GuildWarsArray<QuestContext> QuestLog;

    [FieldOffset(0x0564)]
    public readonly GuildWarsArray<MissionObjectiveContext> MissionObjectives;

    [FieldOffset(0x0574)]
    public readonly GuildWarsArray<uint> HenchmenAgentIds;

    [FieldOffset(0x0584)]
    public readonly GuildWarsArray<HeroFlag> HeroFlags;

    [FieldOffset(0x0594)]
    public readonly GuildWarsArray<HeroInfo> HeroInfos;

    [FieldOffset(0x05A4)]
    public readonly GuildWarsArray<nuint> CartographedAreas;

    [FieldOffset(0x05BC)]
    public readonly GuildWarsArray<ControlledMinion> ControlledMinions;

    [FieldOffset(0x05CC)]
    public readonly GuildWarsArray<uint> MissionsCompleted;

    [FieldOffset(0x05DC)]
    public readonly GuildWarsArray<uint> MissionsBonus;

    [FieldOffset(0x05EC)]
    public readonly GuildWarsArray<uint> MissionsCompletedHardMode;

    [FieldOffset(0x05FC)]
    public readonly GuildWarsArray<uint> MissionsBonusHardMode;

    [FieldOffset(0x060C)]
    public readonly GuildWarsArray<uint> UnlockedMap;

    [FieldOffset(0x062C)]
    public readonly GuildWarsArray<PartyMoraleContext> PartyMorale;

    [FieldOffset(0x067C)]
    public readonly uint PlayerNumber;

    [FieldOffset(0x0680)]
    public readonly PlayerControlledCharContext* PlayerControlledChar;

    [FieldOffset(0x0684)]
    public readonly uint HardModeUnlocked;

    [FieldOffset(0x06AC)]
    public readonly GuildWarsArray<PetContext> Pets;

    [FieldOffset(0x06BC)]
    public readonly GuildWarsArray<ProfessionsContext> Professions;

    [FieldOffset(0x06F0)]
    public readonly GuildWarsArray<SkillbarContext> Skillbars;

    [FieldOffset(0x0700)]
    public readonly GuildWarsArray<uint> LearnableCharacterSkills;

    [FieldOffset(0x0710)]
    public readonly GuildWarsArray<uint> UnlockedCharacterSkills;

    [FieldOffset(0x0720)]
    public readonly GuildWarsArray<DupeSkill> DupeSkills;

    [FieldOffset(0x0740)]
    public readonly uint Experience;

    [FieldOffset(0x0748)]
    public readonly uint CurrentKurzick;

    [FieldOffset(0x0750)]
    public readonly uint TotalKurzick;

    [FieldOffset(0x0758)]
    public readonly uint CurrentLuxon;

    [FieldOffset(0x0760)]
    public readonly uint TotalLuxon;

    [FieldOffset(0x0768)]
    public readonly uint CurrentImperial;

    [FieldOffset(0x0770)]
    public readonly uint TotalImperial;

    [FieldOffset(0x0788)]
    public readonly uint Level;

    [FieldOffset(0x0790)]
    public readonly uint Morale;

    [FieldOffset(0x0798)]
    public readonly uint CurrentBalthazar;

    [FieldOffset(0x07A0)]
    public readonly uint TotalBalthazar;

    [FieldOffset(0x07A8)]
    public readonly uint CurrentSkillPoints;

    [FieldOffset(0x07B0)]
    public readonly uint TotalSkillPoints;

    [FieldOffset(0x07B8)]
    public readonly uint MaxKurzick;

    [FieldOffset(0x07BC)]
    public readonly uint MaxLuxon;

    [FieldOffset(0x07C0)]
    public readonly uint MaxBalthazar;

    [FieldOffset(0x07C4)]
    public readonly uint MaxImperial;

    [FieldOffset(0x07CC)]
    public readonly GuildWarsArray<AgentInfo> AgentInfos;

    [FieldOffset(0x07FC)]
    public readonly GuildWarsArray<NpcContext> Npcs;

    [FieldOffset(0x080C)]
    public readonly GuildWarsArray<PlayerContext> Players;

    [FieldOffset(0x081C)]
    public readonly GuildWarsArray<TitleContext> Titles;

    [FieldOffset(0x082C)]
    public readonly GuildWarsArray<TitleTier> TitleTiers;

    [FieldOffset(0x083C)]
    public readonly GuildWarsArray<uint> VanquishedAreas;

    [FieldOffset(0x084C)]
    public readonly uint FoesKilled;

    [FieldOffset(0x0850)]
    public readonly uint FoesToKill;
}
