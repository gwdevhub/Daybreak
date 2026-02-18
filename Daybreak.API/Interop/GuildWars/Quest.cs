using System.Numerics;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x0034)]
[GWCAEquivalent("Quest")]
public readonly struct QuestContext
{
    [FieldOffset(0x0000)]
    public readonly uint QuestId;

    [FieldOffset(0x0014)]
    public readonly uint MapFrom;

    [FieldOffset(0x0018)]
    public readonly Vector3 Marker;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC)]
public readonly unsafe struct MissionObjectiveContext
{
    public readonly uint ObjectiveId;
    public readonly char* EncodedString;
    public readonly uint Type;
}
