using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct MapEntityContext
{
    [FieldOffset(0x00)]
    public readonly float CurrentEnergy;
    [FieldOffset(0x04)]
    public readonly float MaxEnergy;
    [FieldOffset(0x08)]
    public readonly float EnergyRegen;
    [FieldOffset(0x0C)]
    public readonly int SkillTimestamp;
    [FieldOffset(0x20)]
    public readonly float CurrentHealth;
    [FieldOffset(0x24)]
    public readonly float MaxHealth;
    [FieldOffset(0x28)]
    public readonly float HealthRegen;
    /// <summary>
    /// Flags containing the current effects on the entity.
    /// 0x0001 Bleeding
    /// 0x0002 Conditioned
    /// 0x000A == 0xA Crippled
    /// 0x0010 Dead
    /// 0x0020 Deep Wound
    /// 0x0040 Poisoned
    /// 0x0080 Enchanted
    /// 0x0400 Degen Hexed
    /// 0x0800 Hexed
    /// 0x8000 Holding Item
    /// </summary>
    [FieldOffset(0x30)]
    public readonly uint Effects;
}
