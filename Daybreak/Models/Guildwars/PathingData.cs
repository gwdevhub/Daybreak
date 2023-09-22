using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class PathingData
{
    public List<Trapezoid> Trapezoids { get; init; } = new();
    public List<List<int>> ComputedPathingMaps { get; init; } = new();
    public List<List<int>> OriginalPathingMaps { get; init; } = new();
    public List<List<int>> OriginalAdjacencyList { get; init; } = new();
    public List<List<int>> ComputedAdjacencyList { get; init; } = new();
}
