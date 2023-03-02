﻿using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class MainPlayerInformation : PlayerInformation
{
    public string? Name { get; init; }
    public bool HardModeUnlocked { get; init; }
    public uint Experience { get; init; }
    public uint Level { get; init; }
    public uint Morale { get; init; }
    public Quest? Quest { get; init; }
    public List<QuestMetadata>? QuestLog { get; init; }
}