using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Pathfinding;

public interface IPathfinder
{
    Task<Result<PathfindingResponse, PathfindingFailure>> CalculatePath(PathingData map, Point startPoint, Point endPoint, CancellationToken cancellationToken);
}
