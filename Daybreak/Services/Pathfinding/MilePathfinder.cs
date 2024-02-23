using Daybreak.Models.Guildwars;
using Daybreak.Services.Pathfinding.Models;
using Daybreak.Services.Pathfinding.Models.MapSpecific;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Pathfinding;
/// <summary>
/// Credit to tedy @https://github.com/gwdevhub/GWToolboxpp/commit/66d70a28a90aa3d3b149a679185518a2f3ee09ad#diff-1ea4fcf89246dcef04c15f57010942270b9cacf9950decc5b9e2fddc55d9062c
/// </summary>
public sealed class MilePathfinder : IPathfinder
{
    public Task<Result<PathfindingResponse, PathfindingFailure>> CalculatePath(PathingData map, System.Windows.Point startPoint, System.Windows.Point endPoint, CancellationToken cancellationToken)
    {
        
    }

    public Task<object?> GenerateNavMesh(List<Trapezoid> trapezoids, List<List<int>> adjacencyList, CancellationToken cancellationToken)
    {
        return Task.FromResult((object?)new MileNavmesh
        {
            // TODO: Teleports = MapSpecificData.MapSpecificDatas.FirstOrDefault(t => t.Map ==)

        });
    }

    private static List<BoundingBox> GenerateBoundingBoxes(List<Trapezoid> trapezoids, List<List<int>> adjacencyList)
    {

    }
}
