using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using System.Collections.Generic;
using System.Extensions;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

public interface IPathfinder
{
    Result<PathfindingResponse, PathfindingFailure> CalculatePath(List<Trapezoid> map, Point startPoint, Point endPoint);
}
