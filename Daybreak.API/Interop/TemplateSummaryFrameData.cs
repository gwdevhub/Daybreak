using System.Runtime.InteropServices;

namespace Daybreak.API.Interop;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x94)]
public struct TemplateSummaryFrameData
{
    [FieldOffset(0x00)]
    public uint FrameId;

    [FieldOffset(0x04)]
    public GuildWars.SkillTemplate SkillTemplate;

    [FieldOffset(0x90)]
    public uint AgentId;
}
