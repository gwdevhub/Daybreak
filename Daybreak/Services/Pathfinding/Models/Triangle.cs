using System.Windows;

namespace Daybreak.Services.Pathfinding.Models;
internal readonly struct Triangle
{
    public readonly int Id;
    public readonly Point A;
    public readonly Point B;
    public readonly Point C;

    public Triangle(int id, Point a, Point b, Point c)
    {
        this.Id = id;
        this.A = a;
        this.B = b;
        this.C = c;
    }
}
