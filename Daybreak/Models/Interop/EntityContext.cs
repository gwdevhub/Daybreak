using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct EntityContext
{
    public const uint EntityContextBaseOffset = 0x0014;

    [FieldOffset(0x0000)]
    public readonly uint Timer;

    [FieldOffset(0x0018)]
    public readonly uint AgentId;

    [FieldOffset(0x001C)]
    public readonly uint ZCoords;

    [FieldOffset(0x0044)]
    public readonly uint NameProperties;

    [FieldOffset(0x0060)]
    public readonly GamePosition Position;

    [FieldOffset(0x0088)]
    public readonly EntityType EntityType;

    [FieldOffset(0x00E0)]
    public readonly ushort EntityModelType;

    [FieldOffset(0x00E2)]
    public readonly ushort AgentModelType;

    [FieldOffset(0x00F6)]
    public readonly byte PrimaryProfessionId;

    [FieldOffset(0x00F7)]
    public readonly byte SecondaryProfessionId;

    [FieldOffset(0x00F8)]
    public readonly byte Level;

    [FieldOffset(0x00F9)]
    public readonly PvpTeam TeamId;

    [FieldOffset(0x0144)]
    public readonly EntityState State;

    [FieldOffset(0x019D)]
    public readonly EntityAllegiance Allegiance;
}
