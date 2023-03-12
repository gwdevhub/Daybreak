using System;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GlobalContext
{
    [FieldOffset(0x0008)]
    public readonly IntPtr InstanceContext;

    [FieldOffset(0x0014)]
    public readonly IntPtr MapContext;

    [FieldOffset(0x002C)]
    public readonly IntPtr GameContext;

    [FieldOffset(0x0044)]
    public readonly IntPtr UserContext;
}
