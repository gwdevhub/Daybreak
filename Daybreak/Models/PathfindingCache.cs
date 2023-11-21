using Daybreak.Services.Pathfinding.Models;
using System.Collections.Generic;
using System.Windows.Media;

namespace Daybreak.Models;
public sealed class PathfindingCache
{
    public List<PathfindingResponse> PathfindingResponses { get; set; } = [];
    public List<Color> Colors { get; set; } = [];
}
