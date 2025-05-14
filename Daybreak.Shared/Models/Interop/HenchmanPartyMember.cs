using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct HenchmanPartyMember
{
    [FieldOffset(0x0000)]
    public readonly uint AgentId;
    [FieldOffset(0x002C)]
    public readonly uint Profession;
    [FieldOffset(0x0030)]
    public readonly uint Level;
}
