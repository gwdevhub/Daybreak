using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct MapIconContext
{
    [FieldOffset(0x0000)]
    public readonly uint Index;
    [FieldOffset(0x0004)]
    public readonly float X;
    [FieldOffset(0x0008)]
    public readonly float Y;
    [FieldOffset(0x0014)]
    public readonly TeamColor Affiliation;
    [FieldOffset(0x001C)]
    public readonly uint Id;
    [FieldOffset(0x0024)]
    private readonly uint H0024; // Needed for proper struct padding
}
