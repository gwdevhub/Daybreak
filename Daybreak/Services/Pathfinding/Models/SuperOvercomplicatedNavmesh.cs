using System.Collections.Generic;

namespace Daybreak.Services.Pathfinding.Models;
internal sealed class SuperOvercomplicatedNavmesh
{
    public List<Triangle> Triangles { get; set; } = [];
    public List<List<int>> AdjacencyList { get; set; } = [];
}
