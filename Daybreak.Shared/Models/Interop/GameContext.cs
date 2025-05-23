﻿using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GameContext
{
    public const uint BaseOffset = 0x007C;

    [FieldOffset(0x0000)]
    public readonly GuildwarsArray<MapEntityContext> MapEntities;

    [FieldOffset(0x0030)]
    public readonly GuildwarsArray<PartyAttributesContext> PartyAttributes;

    [FieldOffset(0x04AC)]
    public readonly uint QuestId;

    [FieldOffset(0x04B0)]
    public readonly GuildwarsArray<QuestContext> QuestLog;

    [FieldOffset(0x0604)]
    public readonly GuildwarsPointer<PlayerControlledCharContext> PlayerControlledChar;

    [FieldOffset(0x0608)]
    public readonly uint HardModeUnlocked;

    [FieldOffset(0x0640)]
    public readonly GuildwarsArray<ProfessionsContext> Professions;

    [FieldOffset(0x0674)]
    public readonly GuildwarsArray<SkillbarContext> Skillbars;

    [FieldOffset(0x06C4)]
    public readonly uint Experience;

    [FieldOffset(0x06CC)]
    public readonly uint CurrentKurzick;

    [FieldOffset(0x06D4)]
    public readonly uint TotalKurzick;

    [FieldOffset(0x06DC)]
    public readonly uint CurrentLuxon;

    [FieldOffset(0x06E4)]
    public readonly uint TotalLuxon;

    [FieldOffset(0x06EC)]
    public readonly uint CurrentImperial;

    [FieldOffset(0x06F4)]
    public readonly uint TotalImperial;

    [FieldOffset(0x070C)]
    public readonly uint Level;

    [FieldOffset(0x0714)]
    public readonly uint Morale;

    [FieldOffset(0x071C)]
    public readonly uint CurrentBalthazar;

    [FieldOffset(0x0724)]
    public readonly uint TotalBalthazar;

    [FieldOffset(0x072C)]
    public readonly uint CurrentSkillPoints;

    [FieldOffset(0x0734)]
    public readonly uint TotalSkillPoints;

    [FieldOffset(0x073C)]
    public readonly uint MaxKurzick;

    [FieldOffset(0x0740)]
    public readonly uint MaxLuxon;

    [FieldOffset(0x0744)]
    public readonly uint MaxBalthazar;

    [FieldOffset(0x0748)]
    public readonly uint MaxImperial;

    [FieldOffset(0x0770)]
    public readonly GuildwarsArray<MapIconContext> MissionMapIcons;

    [FieldOffset(0x0780)]
    public readonly GuildwarsArray<NpcContext> Npcs;

    [FieldOffset(0x790)]
    public readonly GuildwarsArray<PlayerContext> Players;

    [FieldOffset(0x7A0)]
    public readonly GuildwarsArray<TitleContext> Titles;

    [FieldOffset(0x7B0)]
    public readonly GuildwarsArray<TitleTierContext> TitlesTiers;

    [FieldOffset(0x07D0)]
    public readonly uint FoesKilled;

    [FieldOffset(0x07D4)]
    public readonly uint FoesToKill;
}
