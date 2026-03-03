using System.Runtime.InteropServices;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Interop;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1AC)]
[GWCAEquivalent("Frame")]
public readonly struct Frame
{
    [FieldOffset(0x0008)]
    public readonly uint FrameLayout;

    [FieldOffset(0x0018)]
    public readonly uint VisibilityFlags;

    [FieldOffset(0x0020)]
    public readonly uint Type;

    [FieldOffset(0x0024)]
    public readonly uint TemplateType;

    [FieldOffset(0x00B8)]
    public readonly uint ChildOffsetId;

    [FieldOffset(0x00BC)]
    public readonly uint FrameId;

    [FieldOffset(0x00D8)]
    public readonly FramePositionData Position;

    [FieldOffset(0x018C)]
    public readonly uint FrameState;

    public bool IsCreated => (this.FrameState & 0x4) != 0;
    public bool IsHidden => (this.FrameState & 0x200) != 0;
    public bool IsVisible => !this.IsHidden;
    public bool IsDisabled => (this.FrameState & 0x10) != 0;
}
