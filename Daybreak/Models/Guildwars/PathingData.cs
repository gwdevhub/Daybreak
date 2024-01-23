using SharpNav;
using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class PathingData
{
    public NavMesh? NavMesh { get; init; }
    public List<Trapezoid> Trapezoids { get; init; } = [];
    public List<List<int>> OriginalPathingMaps { get; init; } = [];
    public List<List<int>> OriginalAdjacencyList { get; init; } = [];
}
