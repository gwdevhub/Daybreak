using System.Runtime.InteropServices;

namespace Daybreak.API.Interop;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("HWND__")]
public readonly struct GWHwnd
{
    public readonly nint Handle;
}
