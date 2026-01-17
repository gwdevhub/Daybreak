using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

[JsonConverter(typeof(JsonStringEnumConverter<ConditionType>))]
public enum ConditionType
{
    Bleeding,
    Blind,
    Burning,
    Crippled,
    DeepWound,
    Disease,
    Poison,
    Dazed,
    Weakness
}

[JsonConverter(typeof(JsonStringEnumConverter<SkillIdToCondition>))]
public enum SkillIdToCondition
{
    Bleeding = 0x1DE,
    Blind = 0x1DF,
    Burning = 0x1E0,
    Crippled = 0x1E1,
    DeepWound = 0x1E2,
    Disease = 0x1E3,
    Poison = 0x1E4,
    Dazed = 0x1E5,
    Weakness = 0x1E6
}
