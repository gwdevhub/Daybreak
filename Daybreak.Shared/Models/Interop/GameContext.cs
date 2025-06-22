using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GameContext
{
    private const int BaseOffset = 0x007C;

    [FieldOffset(BaseOffset + 0x0000)]
    public readonly GuildwarsArray<MapEntityContext> MapEntities;

    [FieldOffset(BaseOffset + 0x0030)]
    public readonly GuildwarsArray<PartyAttributesContext> PartyAttributes;

    [FieldOffset(BaseOffset + 0x04AC)]
    public readonly uint QuestId;

    [FieldOffset(BaseOffset + 0x04B0)]
    public readonly GuildwarsArray<QuestContext> QuestLog;

    [FieldOffset(BaseOffset + 0x0604)]
    public readonly GuildwarsPointer<PlayerControlledCharContext> PlayerControlledChar;

    [FieldOffset(BaseOffset + 0x0608)]
    public readonly uint HardModeUnlocked;

    [FieldOffset(BaseOffset + 0x0640)]
    public readonly GuildwarsArray<ProfessionsContext> Professions;

    [FieldOffset(BaseOffset + 0x0674)]
    public readonly GuildwarsArray<SkillbarContext> Skillbars;

    [FieldOffset(BaseOffset + 0x06C4)]
    public readonly uint Experience;

    [FieldOffset(BaseOffset + 0x06CC)]
    public readonly uint CurrentKurzick;

    [FieldOffset(BaseOffset + 0x06D4)]
    public readonly uint TotalKurzick;

    [FieldOffset(BaseOffset + 0x06DC)]
    public readonly uint CurrentLuxon;

    [FieldOffset(BaseOffset + 0x06E4)]
    public readonly uint TotalLuxon;

    [FieldOffset(BaseOffset + 0x06EC)]
    public readonly uint CurrentImperial;

    [FieldOffset(BaseOffset + 0x06F4)]
    public readonly uint TotalImperial;

    [FieldOffset(BaseOffset + 0x070C)]
    public readonly uint Level;

    [FieldOffset(BaseOffset + 0x0714)]
    public readonly uint Morale;

    [FieldOffset(BaseOffset + 0x071C)]
    public readonly uint CurrentBalthazar;

    [FieldOffset(BaseOffset + 0x0724)]
    public readonly uint TotalBalthazar;

    [FieldOffset(BaseOffset + 0x072C)]
    public readonly uint CurrentSkillPoints;

    [FieldOffset(BaseOffset + 0x0734)]
    public readonly uint TotalSkillPoints;

    [FieldOffset(BaseOffset + 0x073C)]
    public readonly uint MaxKurzick;

    [FieldOffset(BaseOffset + 0x0740)]
    public readonly uint MaxLuxon;

    [FieldOffset(BaseOffset + 0x0744)]
    public readonly uint MaxBalthazar;

    [FieldOffset(BaseOffset + 0x0748)]
    public readonly uint MaxImperial;

    [FieldOffset(BaseOffset + 0x0770)]
    public readonly GuildwarsArray<MapIconContext> MissionMapIcons;

    [FieldOffset(BaseOffset + 0x0780)]
    public readonly GuildwarsArray<NpcContext> Npcs;

    [FieldOffset(BaseOffset + 0x790)]
    public readonly GuildwarsArray<PlayerContext> Players;

    [FieldOffset(BaseOffset + 0x7A0)]
    public readonly GuildwarsArray<TitleContext> Titles;

    [FieldOffset(BaseOffset + 0x7B0)]
    public readonly GuildwarsArray<TitleTierContext> TitlesTiers;

    [FieldOffset(BaseOffset + 0x07D0)]
    public readonly uint FoesKilled;

    [FieldOffset(BaseOffset + 0x07D4)]
    public readonly uint FoesToKill;
}
