using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class PathingData
{
    public List<Trapezoid> Trapezoids { get; init; } = [];
    public List<List<int>> OriginalPathingMaps { get; init; } = [];
    public List<List<int>> OriginalAdjacencyList { get; init; } = [];
}
