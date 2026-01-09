using System.Drawing;

namespace Daybreak.Shared.Models;

public readonly struct Screen(int id, Rectangle size)
    : IEquatable<Screen>
{
    public readonly int Id = id;
    public readonly Rectangle Size = size;

    public bool Equals(Screen other) => this.Id == other.Id
        && this.Size == other.Size;

    public override int GetHashCode() => HashCode.Combine(this.Id, this.Size);

    public override bool Equals(object? obj)
    {
        if (obj is Screen screen)
        {
            return this.Equals(screen);
        }

        return false;
    }

    public static bool operator ==(Screen? left, Screen? right) => Equals(left, right);
    public static bool operator !=(Screen? left, Screen? right) => !Equals(left, right);
}
