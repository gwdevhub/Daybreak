using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Sequential)]
public readonly struct IPAddressContext
{
    public readonly byte Byte1;
    public readonly byte Byte2;
    public readonly byte Byte3;
    public readonly byte Byte4;
}
