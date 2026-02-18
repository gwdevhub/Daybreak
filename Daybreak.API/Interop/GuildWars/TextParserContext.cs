using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
[GWCAEquivalent("TextParser")]
public struct TextParserContext
{
    [FieldOffset(0x01D0)]
    public Language Language;
}
