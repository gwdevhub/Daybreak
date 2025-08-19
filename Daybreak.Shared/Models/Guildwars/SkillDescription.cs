namespace Daybreak.Shared.Models.Guildwars;
public sealed record SkillDescription(
    int Id,
    string Name,
    Campaign Campaign,
    Profession Profession,
    Attribute Attribute,
    string Type,
    string Energy,
    string Activation,
    string Recharge,
    string Description,
    string ConciseDescription)
{
}
