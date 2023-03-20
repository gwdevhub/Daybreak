using System;

namespace Daybreak.Models.Guildwars;

public readonly struct Position : IEquatable<Position>
{
    public float X { get; init; }
    public float Y { get; init; }

    public bool Equals(Position other)
    {
        return this.X == other.X && this.Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Position position && this.Equals(position);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.X, this.Y);
    }

    public static bool operator ==(Position left, Position right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !(left == right);
    }
}
