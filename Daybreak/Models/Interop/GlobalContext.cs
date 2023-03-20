using System;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GlobalContext
{
    [FieldOffset(0x0008)]
    public readonly int InstanceContext;

    [FieldOffset(0x0014)]
    public readonly int MapContext;

    [FieldOffset(0x002C)]
    public readonly int GameContext;

    [FieldOffset(0x0044)]
    public readonly int UserContext;
}
