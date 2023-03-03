using Daybreak.Models.Guildwars;

namespace Daybreak.Services.BuildTemplates;

public interface IAttributePointCalculator
{
    int MaximumAttributePoints { get; }
    
    int GetPointsRequiredToIncreaseRank(int currentRank);
    
    int GetRemainingFreePoints(Build build);
    
    int GetUsedPoints(Build build);
}
