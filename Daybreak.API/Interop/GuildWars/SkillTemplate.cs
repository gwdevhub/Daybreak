using System.Extensions;

namespace Daybreak.API.Interop.GuildWars;

[GWCAEquivalent("SkillTemplate")]
public readonly struct SkillTemplate(uint primary, uint secondary, uint attributeCount, Array12Uint attributeIds, Array12Uint attributeValues, Array8Uint skills)
{
    public readonly uint Primary = primary;
    public readonly uint Secondary = secondary;
    public readonly uint AttributesCount = attributeCount;
    public readonly Array12Uint AttributeIds = attributeIds;
    public readonly Array12Uint AttributeValues = attributeValues;
    public readonly Array8Uint Skills = skills;
}
