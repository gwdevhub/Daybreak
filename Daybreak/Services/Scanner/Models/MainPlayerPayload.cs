using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;

internal class MainPlayerPayload : WorldPlayerPayload
{
    public uint CurrentEnergy { get; set; }
    public uint CurrentHp { get; set; }
    public uint CurrentQuest { get; set; }
    public uint Experience { get; set; }
    public bool HardModeUnlocked { get; set; }
    public uint MaxHp { get; set; }
    public uint MaxEnergy { get; set; }
    public uint Morale { get; set; }
    public List<QuestMetadataPayload>? QuestLog { get; set; }
}
