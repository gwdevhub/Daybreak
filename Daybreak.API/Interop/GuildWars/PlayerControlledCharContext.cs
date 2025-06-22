using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly struct PlayerControlledCharContext
{
    [FieldOffset(0x0014)]
    public readonly uint AgentId;
}
