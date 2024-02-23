using System.Numerics;

namespace Daybreak.Services.Pathfinding.Models;
internal sealed class Point
{
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public BoundingBox Box { get; set; } = default!;
    public BoundingBox Box2 { get; set; } = default!;
    public Portal Portal { get; set; } = default!;

    public static implicit operator Vector3(Point point)
    {
        return new Vector3(point.Position.X, point.Position.Y, point.Box.Trapezoid.Layer);
    }
}
