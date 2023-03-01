using System;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct EntityContext
{
    [FieldOffset(0x2C)]
    public readonly uint EntityId;

    [FieldOffset(0x9C)]
    public readonly EntityType Type;

    // From here, all fields are only valid for living entities.
    [FieldOffset(0xC4)]
    public readonly uint OwnerId;

    [FieldOffset(0x114)]
    public readonly float EnergyRegen;

    [FieldOffset(0x11C)]
    public readonly float Energy;

    [FieldOffset(0x120)]
    public readonly uint MaxEnergy;

    [FieldOffset(0x128)]
    public readonly float HealthRegen;

    [FieldOffset(0x130)]
    public readonly float Health;

    [FieldOffset(0x134)]
    public readonly uint MaxHealth;

    [FieldOffset(0x1BC)]
    public readonly ushort H01BC;
}
