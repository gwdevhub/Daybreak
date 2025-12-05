namespace Daybreak.Shared.Models.FocusView;
public sealed class CharacterComponentContext
{
    public required CharacterSelectComponentEntry CurrentCharacter { get; init; }
    public required List<CharacterSelectComponentEntry> Characters { get; init; }
    public required uint CurrentTotalExperience { get; init; }
    public required uint ExperienceForCurrentLevel { get; init; }
    public required uint ExperienceForNextLevel { get; init; }
    public required uint NextExperienceThreshold { get; init; }
    public required uint TotalExperienceForNextLevel { get; init; }
    public ExperienceDisplay ExperienceDisplay { get; init; }
}
