namespace Daybreak.Shared.Models;
public readonly struct BuildTemplateValidationRequest(
    uint buildPrimary,
    uint buildSecondary,
    uint[] buildSkills,
    uint currentPrimary,
    uint unlockedCharacterProfessions,
    uint[] unlockedSkills)
{
    public readonly uint BuildPrimary = buildPrimary;
    public readonly uint BuildSecondary = buildSecondary;
    public readonly uint[] BuildSkills = buildSkills ?? [];
    public readonly uint CurrentPrimary = currentPrimary;
    public readonly uint UnlockedCharacterProfessions = unlockedCharacterProfessions;
    public readonly uint[] UnlockedSkills = unlockedSkills ?? [];
}
