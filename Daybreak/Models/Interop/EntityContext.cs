using System;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct EntityContext
{
    [FieldOffset(0x2C)]
    public readonly uint EntityId;

    [FieldOffset(0x9C)]
    public readonly EntityType Type;
}
