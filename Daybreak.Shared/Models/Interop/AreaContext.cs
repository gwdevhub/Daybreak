using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct AreaContext
{
    [FieldOffset(0x0000)]
    public readonly uint Campaign;

    [FieldOffset(0x0004)]
    public readonly uint Continent;

    [FieldOffset(0x0008)]
    public readonly uint RegionId;

    [FieldOffset(0x000C)]
    public readonly uint RegionType;

    [FieldOffset(0x0010)]
    public readonly uint Flags;

    [FieldOffset(0x0018)]
    public readonly uint MinPartySize;

    [FieldOffset(0x001C)]
    public readonly uint MaxPartySize;

    [FieldOffset(0x0020)]
    public readonly uint MinPlayerSize;

    [FieldOffset(0x0024)]
    public readonly uint MaxPlayerSize;

    [FieldOffset(0x0028)]
    public readonly uint ControlledOutpostId;

    [FieldOffset(0x0030)]
    public readonly uint MinLevel;

    [FieldOffset(0x0034)]
    public readonly uint MaxLevel;

    [FieldOffset(0x003C)]
    public readonly uint MissionMapsTo;

    [FieldOffset(0x0040)]
    public readonly uint IconPositionX;

    [FieldOffset(0x0044)]
    public readonly uint IconPositionY;

    [FieldOffset(0x0074)]
    public readonly uint NameId;

    [FieldOffset(0x0078)]
    public readonly uint DescriptionId;
}
