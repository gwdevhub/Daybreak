using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x78)]
[GWCAEquivalent("GameplayContext")]
public readonly struct GameplayContext
{
    [FieldOffset(0x4C)]
    public readonly float MissionMapZoom;
}