using System.Collections.Generic;

namespace Daybreak.Services.Pathfinding.Models;

internal readonly struct PointVisibilityElement
{
    public int PointId { get; init; }
    public float Distance { get; init; }
    public List<uint> BlockingIds { get; init; }
}
