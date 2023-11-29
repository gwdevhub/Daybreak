using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using SharpNav;
using System.Collections.Generic;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

public interface IPathfinder
{
    Task<NavMesh?> GenerateNavMesh(List<Trapezoid> trapezoids, CancellationToken cancellationToken);
    Task<Result<PathfindingResponse, PathfindingFailure>> CalculatePath(PathingData map, Point startPoint, Point endPoint, CancellationToken cancellationToken);
}
