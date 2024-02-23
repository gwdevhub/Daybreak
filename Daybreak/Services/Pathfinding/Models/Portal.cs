using Daybreak.Utils;
using System.Numerics;

namespace Daybreak.Services.Pathfinding.Models;
internal sealed class Portal
{
    public Vector2 Start { get; }
    public Vector2 Goal { get; }
    public BoundingBox Box1 { get; }
    public BoundingBox Box2 { get; }

    public Portal(Vector2 start, Vector2 goal, BoundingBox box1, BoundingBox box2)
    {
        this.Start = start;
        this.Goal = goal;
        this.Box1 = box1;
        this.Box2 = box2;
    }

    public bool Intersect(Vector2 p1, Vector2 p2)
    {
        return MathUtils.Intersect(this.Start, this.Goal, p1, p2);
    }
}
