using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct AreaInfoContext
{
    [FieldOffset(0x0000)]
    public readonly uint CampaignId;
    [FieldOffset(0x0004)]
    public readonly uint ContinentId;
    [FieldOffset(0x0008)]
    public readonly uint RegionId;
    [FieldOffset(0x000C)]
    public readonly RegionType RegionType;
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
    [FieldOffset(0x0030)]
    public readonly uint MinLevel;
    [FieldOffset(0x0034)]
    public readonly uint MaxLevel;
    [FieldOffset(0x0040)]
    public readonly uint IconPositionX;
    [FieldOffset(0x0044)]
    public readonly uint IconPositionY;
    [FieldOffset(0x0048)]
    public readonly uint IconStartX;
    [FieldOffset(0x004C)]
    public readonly uint IconStartY;
    [FieldOffset(0x0050)]
    public readonly uint IconEndX;
    [FieldOffset(0x0054)]
    public readonly uint IconEndY;

    public bool HasEnterButton => (this.Flags & 0x100) != 0 || (this.Flags & 0x40000) != 0;
    public bool IsOnWorldMap => (this.Flags & 0x20) == 0;
    public bool IsPvp => (this.Flags & 0x1) != 0;
    public bool IsGuildHall => (this.Flags & 0x800000) != 0;
}
