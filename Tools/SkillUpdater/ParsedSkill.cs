namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// A skill in the exact shape the writer will emit it: every value is already
/// the literal C# token it should appear as in <c>Skill.g.cs</c>. This is
/// deliberately tool-specific — the runtime <c>WikiService</c> has its own
/// parser in <c>Daybreak.Shared</c> built around typed model objects.
/// </summary>
public sealed record ParsedSkill(
    int Id,
    string Name,
    string CampaignIdentifier,
    string ProfessionIdentifier,
    string AttributeIdentifier,
    bool PvEOnly,
    bool PvP,
    bool Elite,
    string TypeExpression,
    double? Energy,
    double? Activation,
    double? Recharge,
    double? Overcast,
    double? Adrenaline,
    double? Sacrifice,
    double? Upkeep,
    string Description,
    string ConciseDescription);
