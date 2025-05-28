using System.Extensions;

namespace Daybreak.API.Interop.GuildWars;

public readonly struct SkillTemplate
{
    public readonly uint Primary;
    public readonly uint Secondary;
    public readonly uint AttributesCount;
    public readonly Array12Uint AttributeIds;
    public readonly Array12Uint AttributeValues;
    public readonly Array8Uint Skills;
}
