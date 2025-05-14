namespace Daybreak.Shared.Models.Interop;

public enum EntityAllegiance : byte
{
    AllyNonAttackable = 0x1,
    Neutral = 0x2,
    Enemy = 0x3,
    SpiritOrPet = 0x4,
    Minion = 0x5,
    NpcOrMinipet = 0x6
}
