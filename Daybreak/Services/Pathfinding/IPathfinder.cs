using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using System.Extensions;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

public interface IPathfinder
{
    Result<PathfindingResponse, PathfindingFailure> CalculatePath(PathingData map, Point startPoint, Point endPoint);
}
