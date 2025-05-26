using System.Extensions;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly struct CharInfoContext
{
    [FieldOffset(0x0008)]
    public readonly Uuid Uuid;
    [FieldOffset(0x0018)]
    public readonly Array20Char Name;
    [FieldOffset(0x0040)]
    public readonly Array17Uint Props;

    public readonly uint MapId => (this.Props[0] >> 16) & 0xFFFF;
    public readonly uint Primary => (this.Props[2] >> 20) & 0xF;
    public readonly uint Secondary => (this.Props[7] >> 10) & 0xF;
    public readonly uint Campaign => this.Props[7] & 0xF;
    public readonly uint Level => (this.Props[7] >> 4) & 0x3F;
    public readonly bool IsPvp => ((this.Props[7] >> 9) & 0x1) == 0x1;
}
