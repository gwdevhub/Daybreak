using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x34)]
public unsafe struct PartySearchWindowContext
{
    [FieldOffset(0x0000)]
    public uint* Vtable;
    [FieldOffset(0x0004)]
    public uint FrameId;
    [FieldOffset(0x0020)]
    public uint SelectedPartySearchId;
    [FieldOffset(0x0024)]
    public uint SelectedHeroId;
    [FieldOffset(0x0028)]
    public uint SelectedHenchmanId;
    [FieldOffset(0x002C)]
    public uint CurrentTab;
}
