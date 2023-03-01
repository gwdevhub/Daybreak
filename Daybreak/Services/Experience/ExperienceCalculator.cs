using System.Collections.Generic;

namespace Daybreak.Services.Experience;

/// <summary>
/// Based on the explanation from here https://wiki.guildwars.com/wiki/Experience.
/// After 182600, the experience threshold is capped.
/// </summary>
public sealed class ExperienceCalculator : IExperienceCalculator
{
    private const uint ExperienceCalculationThreshold = 182600;
    private const uint MaxExperienceRequirement = 15000;
    private static readonly List<uint> ExperienceThreshold = new()
    {
        0,
        2000,
        4600,
        7800,
        11600,
        16000,
        21000,
        26600,
        32800,
        39600,
        47000,
        55000,
        63600,
        72800,
        82600,
        93000,
        104000,
        115600,
        127800,
        140600,
        154000,
        168000,
        182600
    };

    public uint GetExperienceForCurrentLevel(uint currentTotalExperience)
    {
        if (currentTotalExperience < ExperienceCalculationThreshold)
        {
            var previousLevelExperience = 0U;
            foreach (var threshold in ExperienceThreshold)
            {
                if (currentTotalExperience >= threshold)
                {
                    break;
                }

                previousLevelExperience = threshold;
            }

            return currentTotalExperience - previousLevelExperience;
        }
        else
        {
            var experienceOverThreshold = currentTotalExperience - ExperienceCalculationThreshold;
            var currentExperience = (experienceOverThreshold % MaxExperienceRequirement);
            return currentExperience;
        }
    }

    public uint GetRemainingExperienceForNextLevel(uint currentTotalExperience)
    {
        if (currentTotalExperience < ExperienceCalculationThreshold)
        {
            var totalXpForNextLevel = 0U;
            foreach(var threshold in ExperienceThreshold)
            {
                if (currentTotalExperience < threshold)
                {
                    totalXpForNextLevel = threshold;
                    break;
                }
            }

            return totalXpForNextLevel - currentTotalExperience;
        }
        else
        {
            var experienceOverThreshold = currentTotalExperience - ExperienceCalculationThreshold;
            var remaining = MaxExperienceRequirement - (experienceOverThreshold % MaxExperienceRequirement);
            return remaining;
        }
    }

    public uint GetTotalExperienceForNextLevel(uint currentTotalExperience)
    {
        return currentTotalExperience + this.GetRemainingExperienceForNextLevel(currentTotalExperience);
    }

    public uint GetNextExperienceThreshold(uint currentTotalExperience)
    {
        if (currentTotalExperience < ExperienceCalculationThreshold)
        {
            var totalXpForNextLevel = 0U;
            foreach (var threshold in ExperienceThreshold)
            {
                if (currentTotalExperience < threshold)
                {
                    totalXpForNextLevel = threshold;
                    break;
                }
            }

            return totalXpForNextLevel;
        }
        else
        {
            return MaxExperienceRequirement;
        }
    }
}
