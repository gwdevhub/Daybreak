namespace Daybreak.Shared.Models.FocusView;
public sealed class BuildComponentContext
{
    public required bool IsInOutpost { get; init; }
    public required uint PrimaryProfessionId { get; init; }
    public required uint UnlockedProfessions { get; init; }
    public required uint[] CharacterUnlockedSkills { get; init; }
    public required uint[] AccountUnlockedSkills { get; init; }
}
