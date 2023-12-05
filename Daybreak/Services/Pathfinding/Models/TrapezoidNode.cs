using Daybreak.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Services.Pathfinding.Models;
public sealed class TrapezoidNode
{
    public int Id { get; set; }
    public Trapezoid Trapezoid { get; set; }
    public List<TrapezoidNode> AdjacencyList { get; set; } = [];
}
