using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("TagInfo")]
public readonly struct TagInfo
{
    public readonly ushort GuildId;
    public readonly byte Primary;
    public readonly byte Secondary;
    public readonly ushort Level;
}
