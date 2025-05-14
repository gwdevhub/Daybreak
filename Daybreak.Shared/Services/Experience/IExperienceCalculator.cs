namespace Daybreak.Shared.Services.Experience;

public interface IExperienceCalculator
{
    uint GetExperienceForCurrentLevel(uint currentTotalExperience);
    uint GetTotalExperienceForNextLevel(uint currentTotalExperience);
    uint GetRemainingExperienceForNextLevel(uint currentTotalExperience);
    uint GetNextExperienceThreshold(uint currentTotalExperience);
}
