using static Daybreak.Shared.Utils.NativeMethods;

namespace Daybreak.Shared.Models;

public readonly struct Screen(int id, RECT size)
    : IEquatable<Screen>
{
    public readonly int Id = id;
    public readonly RECT Size = size;

    public bool Equals(Screen other) => this.Id == other.Id
        && this.Size.Top == other.Size.Top
        && this.Size.Bottom == other.Size.Bottom
        && this.Size.Left == other.Size.Left
        && this.Size.Right == other.Size.Right;

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
