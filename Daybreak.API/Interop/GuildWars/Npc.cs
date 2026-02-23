using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit)]
[GWCAEquivalent("NPC")]
public readonly unsafe struct NpcContext
{
    [FieldOffset(0x0000)]
    public readonly uint ModelId;

    [FieldOffset(0x000C)]
    public readonly uint Sex;

    [FieldOffset(0x0010)]
    public readonly NpcFlags Flags;

    [FieldOffset(0x0014)]
    public readonly uint Primary;

    [FieldOffset(0x001C)]
    public readonly byte DefaultLevel;

    [FieldOffset(0x0020)]
    public readonly char* EncName;

    [FieldOffset(0x0028)]
    public readonly int FilesCount;

    [FieldOffset(0x002C)]
    public readonly int FilesCapacity;

    [Flags]
    public enum NpcFlags : uint
    {
        None = 0x0000,
        Henchman = 0x0010,
        Hero = 0x0020,
        Spirit = 0x4000,
        Minion = 0x0100,
        Pet = 0x000D
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("PetInfo")]
public readonly unsafe struct PetContext
{
    public readonly uint AgentId;
    public readonly uint OwnerAgentId;
    public readonly char* PetName;
    public readonly uint ModelFileId1;
    public readonly uint ModelFileId2;
    public readonly Behavior Behavior;
    public readonly uint LockedTargetId;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct ControlledMinion
{
    public readonly uint AgentId;
    public readonly uint MinionCount;
}
