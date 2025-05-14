using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct EntityContext
{
    [FieldOffset(0x0014)]
    public readonly uint Timer;

    [FieldOffset(0x002C)]
    public readonly uint AgentId;

    [FieldOffset(0x0030)]
    public readonly uint ZCoords;

    [FieldOffset(0x0058)]
    public readonly uint NameProperties;

    [FieldOffset(0x0074)]
    public readonly GamePosition Position;

    [FieldOffset(0x009C)]
    public readonly EntityType EntityType;

    [FieldOffset(0x00F4)]
    public readonly ushort EntityModelType;

    [FieldOffset(0x00F6)]
    public readonly ushort AgentModelType;

    [FieldOffset(0x010A)]
    public readonly byte PrimaryProfessionId;

    [FieldOffset(0x010B)]
    public readonly byte SecondaryProfessionId;

    [FieldOffset(0x010C)]
    public readonly byte Level;

    [FieldOffset(0x010D)]
    public readonly PvpTeam TeamId;

    [FieldOffset(0x0114)]
    public readonly float EnergyRegen; // Only works for main player

    [FieldOffset(0x011C)]
    public readonly float CurrentEnergyPercentage; // Only works for main player

    [FieldOffset(0x0120)]
    public readonly uint MaxEnergy; // Only works for main player

    [FieldOffset(0x0128)]
    public readonly float HealthRegen; // Only works for main player

    [FieldOffset(0x0130)]
    public readonly float CurrentHealthPercentage; // Only works for main player

    [FieldOffset(0x0134)]
    public readonly uint MaxHealth; // Only works for main player

    [FieldOffset(0x0158)]
    public readonly EntityState State;

    [FieldOffset(0x01B1)]
    public readonly EntityAllegiance Allegiance;
}
