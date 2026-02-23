using System.Runtime.InteropServices;

namespace Daybreak.API.Interop;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("HookEntry")]
public readonly struct HookEntry
{
}