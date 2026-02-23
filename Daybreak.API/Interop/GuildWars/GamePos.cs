using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("GamePos")]
public readonly struct GamePos
{
    public readonly float X;
    public readonly float Y;
    public readonly uint Plane;
}
