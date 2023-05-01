using System;

namespace Daybreak.Models.Guildwars;

public enum Affiliation
{
    Gray = 0b00000001,
    Blue = 0b00000010,
    Red = 0b00000100,
    Yellow = 0b00001000,
    Teal = 0b00010000,
    Purple = 0b00100000,
    Green = 0b01000000,
    GrayNeutral = 0b10000000,
    Any = 0b11111111
}
