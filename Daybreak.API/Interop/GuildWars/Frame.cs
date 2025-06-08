using Daybreak.API.Models;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

// Very incomplete. GWCA contains a much better definition
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1AC)]
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

    [FieldOffset(0x00A0)]
    public readonly GuildWarsArray<FrameInteractionCallback> FrameCallbacks;

    [FieldOffset(0x00b0)]
    public readonly uint ChildOffsetId;

    [FieldOffset(0x00b4)]
    public readonly uint FrameId;

    [FieldOffset(0x0120)]
    public readonly FrameRelation Relation;

    [FieldOffset(0x0184)]
    public readonly uint Field91;

    public bool IsCreated => (this.Field91 & 0x4) != 0;
    public bool IsHidden => (this.Field91 & 0x200) != 0;
    public bool IsVisible => !this.IsHidden;
    public bool IsDIsabled => (this.Field91 & 0x10) != 0;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1C)]
public readonly unsafe struct FrameRelation
{
    [FieldOffset(0x0000)]
    public readonly FrameRelation* Parent;

    [FieldOffset(0x000C)]
    public readonly uint FrameHashId;
}

[StructLayout(LayoutKind.Sequential, Pack =1)]
public readonly unsafe struct InteractionMessage
{
    public readonly uint FrameId;
    public readonly UIMessage MessageId;
    public readonly void** WParam;
}

public unsafe delegate void UIInteractionCallback(InteractionMessage* message, void* wParam, void* lParam);

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xC)]
public unsafe readonly struct FrameInteractionCallback
{
    public readonly void* Callback; // UIInteractionCallback
    public readonly void* UiCtl_Context;
}
