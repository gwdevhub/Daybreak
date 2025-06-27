namespace Daybreak.Shared.Models.Api;
public sealed record MainPlayerBuildContext(uint PrimaryProfessionId, uint UnlockedProfessions, uint[] UnlockedCharacterSkills, uint[] UnlockedAccountSkills)
{
}
