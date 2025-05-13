using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct MapContext
{
    [FieldOffset(0x0000)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x14)]
    public readonly float[] Boundaries;

    /// <summary>
    /// Ptr for the PathingMap. Needs to follow this pointer and then next pointer as well to reach the PathingMapContext.
    /// </summary>
    [FieldOffset(0x0074)]
    public readonly uint PathingMapContextPtr;
}
