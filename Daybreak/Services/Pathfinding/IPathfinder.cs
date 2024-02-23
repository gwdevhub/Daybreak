using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using System.Collections.Generic;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

public interface IPathfinder
{
    Task<object?> GenerateNavMesh(List<Trapezoid> trapezoids, List<List<int>> adjacencyList, CancellationToken cancellationToken);
    Task<Result<PathfindingResponse, PathfindingFailure>> CalculatePath(PathingData map, System.Windows.Point startPoint, System.Windows.Point endPoint, CancellationToken cancellationToken);
}
