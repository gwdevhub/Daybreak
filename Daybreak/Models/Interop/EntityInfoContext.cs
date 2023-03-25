using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct EntityInfoContext
{
    [FieldOffset(0x004C)]
    public readonly int EncName;
}
