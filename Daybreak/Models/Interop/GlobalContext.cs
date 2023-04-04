using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GlobalContext
{
    [FieldOffset(0x0008)]
    public readonly uint InstanceContext;

    [FieldOffset(0x0014)]
    public readonly uint MapContext;

    [FieldOffset(0x002C)]
    public readonly uint GameContext;

    [FieldOffset(0x0044)]
    public readonly uint UserContext;
}
