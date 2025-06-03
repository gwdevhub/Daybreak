using Daybreak.Shared.Models.Builds;

namespace Daybreak.Shared.Models;
public readonly struct BuildTemplateValidationRequest(
    SingleBuildEntry buildEntry,
    uint currentPrimary,
    uint unlockedCharacterProfessions,
    uint[] unlockedSkills
    )
{
    public readonly SingleBuildEntry BuildEntry = buildEntry;
    public readonly uint CurrentPrimary = currentPrimary;
    public readonly uint UnlockedCharacterProfessions = unlockedCharacterProfessions;
    public readonly uint[] UnlockedSkills = unlockedSkills ?? [];
}
