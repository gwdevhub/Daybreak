using Daybreak.Shared.Models.Builds;

namespace Daybreak.Shared.Services.BuildTemplates;

public interface IAttributePointCalculator
{
    int MaximumAttributePoints { get; }
    
    int GetPointsRequiredToIncreaseRank(int currentRank);

    int GetRemainingFreePoints(SingleBuildEntry build);

    int GetUsedPoints(SingleBuildEntry build);
}
