using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct SkillbarContext
{
    [FieldOffset(0x0000)]
    public readonly uint AgentId;

    [FieldOffset(0x0004)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x08)]
    public readonly SkillContext[] Skills;

    [FieldOffset(0x00A4)]
    public readonly uint Disabled;

    [FieldOffset(0x00B0)]
    public readonly uint Casting;

    [FieldOffset(0x00B8)]
    public readonly uint H00B8;
}
