namespace Daybreak.Shared.Models;

public sealed record KeyboardEventArgs(VirtualKey Key);

public enum VirtualKey
{
    F1 = 0x70,
    F2 = 0x71,
    F3 = 0x72,
    F4 = 0x73,
    F5 = 0x74,
    F6 = 0x75,
    F7 = 0x76,
    F8 = 0x77,
    F9 = 0x78,
    F10 = 0x79,
    F11 = 0x7A,
    F12 = 0x7B,
    Escape = 0x1B,
    Enter = 0x0D,
    Space = 0x20
}
