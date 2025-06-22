using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct SkillContext
{
    public readonly uint Adrenaline1;
    public readonly uint Adrenaline2;
    public readonly uint Recharge;
    public readonly uint Id;
    public readonly uint Event;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0xBC)]
public readonly struct SkillbarContext
{
    public readonly uint AgentId;
    public readonly SkillContext Skill0;
    public readonly SkillContext Skill1;
    public readonly SkillContext Skill2;
    public readonly SkillContext Skill3;
    public readonly SkillContext Skill4;
    public readonly SkillContext Skill5;
    public readonly SkillContext Skill6;
    public readonly SkillContext Skill7;
    public readonly uint Disabled;
    public readonly uint Casting;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x10)]
public readonly struct Buff
{
    [FieldOffset(0x0000)]
    public readonly uint SkillId;
    [FieldOffset(0x0008)]
    public readonly uint BuffId;
    [FieldOffset(0x000C)]
    public readonly uint TargetAgentId;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x18)]
public readonly struct Effect
{
    public readonly uint SkillId;
    public readonly uint AttributeLevel;
    public readonly uint EffectId;
    public readonly uint AgentId;
    public readonly float Duration;
    public readonly uint Timestamp;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct DupeSkill
{
    public readonly uint SkillId;
    public readonly uint Count;
}
