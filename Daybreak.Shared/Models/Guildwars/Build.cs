using Daybreak.Shared.Models.Builds;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class Build
{
    public BuildMetadata? BuildMetadata { get; set; }
    public Profession Primary { get; set; } = Profession.None;
    public Profession Secondary { get; set; } = Profession.None;
    public List<AttributeEntry> Attributes { get; set; } = [];
    public List<Skill> Skills { get; set; } = [Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill];
}
