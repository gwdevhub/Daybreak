using System.Collections.Generic;
using Daybreak.Models.Builds;

namespace Daybreak.Models.Guildwars;

public sealed class Build
{
    public BuildMetadata? BuildMetadata { get; set; }
    public Profession Primary { get; set; } = Profession.None;
    public Profession Secondary { get; set; } = Profession.None;
    public List<AttributeEntry> Attributes { get; set; } = [];
    public List<Skill> Skills { get; set; } = [Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill];
    public string? SourceUrl { get; set; }
}
