using System.Extensions;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly struct CharContext
{
    [FieldOffset(0x0064)]
    public readonly Array4Uint PlayerUuid;

    [FieldOffset(0x0074)]
    public readonly Array20Char PlayerName;

    [FieldOffset(0x0198)]
    public readonly uint MapId;

    [FieldOffset(0x019C)]
    public readonly uint IsExporable;

    [FieldOffset(0x01A0)]
    public readonly Array24Byte Host;

    [FieldOffset(0x01B8)]
    public readonly uint PlayerId;

    [FieldOffset(0x0220)]
    public readonly uint DistrictNumber;

    [FieldOffset(0x0224)]
    public readonly Language Language;

    [FieldOffset(0x0228)]
    public readonly uint ObserveMapId;

    [FieldOffset(0x022C)]
    public readonly uint CurrentMapId;

    [FieldOffset(0x0230)]
    public readonly uint ObserveMapType;

    [FieldOffset(0x0234)]
    public readonly uint CurrentMapType;

    [FieldOffset(0x02A4)]
    public readonly uint PlayerNumber;

    [FieldOffset(0x03B8)]
    public readonly Array64Char PlayerEmail;
}
