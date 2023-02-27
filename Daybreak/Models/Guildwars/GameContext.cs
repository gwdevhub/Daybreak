using System;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Guildwars;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GameContext
{
    [FieldOffset(0x002C)]
    public readonly IntPtr WorldContext;
    
    [FieldOffset(0x0044)]
    public readonly IntPtr CharContext;
}
