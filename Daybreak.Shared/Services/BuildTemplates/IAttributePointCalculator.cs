using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;

namespace Daybreak.Services.BuildTemplates;

public interface IAttributePointCalculator
{
    int MaximumAttributePoints { get; }
    
    int GetPointsRequiredToIncreaseRank(int currentRank);
    
    int GetRemainingFreePoints(Build build);

    int GetRemainingFreePoints(SingleBuildEntry build);

    int GetUsedPoints(Build build);

    int GetUsedPoints(SingleBuildEntry build);
}
