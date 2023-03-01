using System;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GlobalContext
{
    [FieldOffset(0x002C)]
    public readonly IntPtr CharacterContext;

    [FieldOffset(0x0044)]
    public readonly IntPtr UserContext;
}
