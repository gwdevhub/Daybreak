namespace Daybreak.Shared.Models.Interop;

public enum EntityState
{
    Dead = 0x08,
    Boss = 0xC00,
    Spirit = 0x40000,
    ToBeCleanedUp = 0x40008,
    Player = 0x400000,
}
