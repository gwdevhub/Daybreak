using System.Windows;

namespace Daybreak.Services.Pathfinding.Models;

public sealed class PathSegment
{
    public Point StartPoint { get; init; }
    public Point EndPoint { get; init; }
}
