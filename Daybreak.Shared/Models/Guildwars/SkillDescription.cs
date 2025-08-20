namespace Daybreak.Shared.Models.Guildwars;
public sealed record SkillDescription(
    int Id,
    string Name,
    Campaign Campaign,
    Profession Profession,
    Attribute Attribute,
    bool PveOnly,
    bool Pvp,
    bool Elite,
    string Type,
    string Energy,
    string Activation,
    string Recharge,
    string Overcast,
    string Adrenaline,
    string Sacrifice,
    string Description,
    string ConciseDescription)
{
}
