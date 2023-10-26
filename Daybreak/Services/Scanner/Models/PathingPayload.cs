using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;
internal sealed class PathingPayload
{
    public List<PathingTrapezoidPayload>? Trapezoids { get; set; }
    public List<List<int>>? AdjacencyList { get; set; }
}
