using System.Extensions;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x24)]
public readonly struct HeroFlag
{
    [FieldOffset(0x0000)]
    public readonly uint HeroId;
    [FieldOffset(0x0004)]
    public readonly uint AgentId;
    [FieldOffset(0x0008)]
    public readonly uint Level;
    [FieldOffset(0x000C)]
    public readonly Behavior Behavior;
    [FieldOffset(0x0010)]
    public readonly Vector2 Flag;
    [FieldOffset(0x0020)]
    public readonly uint LockedTargetId;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x78)]
[GWCAEquivalent("HeroInfo")]
public readonly struct HeroInfo
{
    [FieldOffset(0x0000)]
    public readonly uint HeroId;
    [FieldOffset(0x0004)]
    public readonly uint AgentId;
    [FieldOffset(0x0008)]
    public readonly uint Level;
    [FieldOffset(0x000C)]
    public readonly Profession Primary;
    [FieldOffset(0x0010)]
    public readonly Profession Secondary;
    [FieldOffset(0x0014)]
    public readonly uint HeroFileId;
    [FieldOffset(0x0018)]
    public readonly uint ModelFileId;
    [FieldOffset(0x0050)]
    public readonly Array20Char Name;
}
