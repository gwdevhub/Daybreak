using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Sequential, Pack = 0, Size = 0xBC)]
public readonly unsafe struct SkillbarContext
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

    public readonly uint H00B8;
}
