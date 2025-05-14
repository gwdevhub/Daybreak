using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Sequential)]
public readonly struct SkillContext
{
    public readonly uint Adrenaline1;

    public readonly uint Adrenaline2;

    public readonly uint Recharge;

    public readonly uint Id;

    public readonly uint Event;
}
