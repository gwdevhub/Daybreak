using System.Collections.Generic;
using Daybreak.Models.Builds;

namespace Daybreak.Models.Guildwars;

public sealed class Build
{
    public BuildMetadata? BuildMetadata { get; set; }
    public Profession Primary { get; set; } = Profession.None;
    public Profession Secondary { get; set; } = Profession.None;
    public List<AttributeEntry> Attributes { get; set; } = new();
    public List<Skill> Skills { get; set; } = new() { Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill };
}
