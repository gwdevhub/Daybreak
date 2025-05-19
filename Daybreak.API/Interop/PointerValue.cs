namespace Daybreak.API.Interop;

public readonly struct PointerValue(nuint address) : IEquatable<PointerValue>, IEquatable<nuint>, IEquatable<nint>
{
    public readonly nuint Address = address;

    public bool Equals(PointerValue other)
    {
        return this.Address.Equals(other.Address);
    }

    public bool Equals(nuint other)
    {
        return this.Address.Equals(other);
    }

    public bool Equals(nint other)
    {
        return this.Address.Equals((nuint)other);
    }

    public override string ToString()
    {
        return $"0x{this.Address:X8}";
    }

    public override bool Equals(object? obj) => obj is PointerValue value && this.Equals(value);

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Address);
    }

    public static bool operator ==(PointerValue left, PointerValue right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PointerValue left, PointerValue right)
    {
        return !(left == right);
    }
}
