using Daybreak.Services.Pathfinding.Models.MapSpecific;
using System.Collections.Generic;

namespace Daybreak.Services.Pathfinding.Models;

internal sealed class MileNavmesh
{
    public List<BoundingBox> BoundingBoxes { get; set; } = default!;
    public List<SimplePathingTrapezoid> Trapezoids { get; set; } = default!;
    public List<List<PointVisibilityElement>> VisibilityGraph { get; set; } = default!;
    public List<List<BoundingBox>> AABBGraph { get; set; } = default!;
    public List<Portal> Portals { get; set; } = default!;
    public List<List<Portal>> PortalsGraph { get; set; } = default!;
    public List<Point> Points { get; set; } = default!;
    public List<Teleport>? Teleports { get; set; } = default!;
    public List<TeleportNode>? TeleportGraph { get; set; } = default!;
}
