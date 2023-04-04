using System.Collections.Generic;

namespace Daybreak.Services.Pathfinding.Models;

public sealed class PathfindingResponse
{
    public List<PathSegment>? Pathing { get; init; } 
}
