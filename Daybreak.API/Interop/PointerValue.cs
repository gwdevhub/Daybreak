using Daybreak.API.Converters;
using System.Text.Json.Serialization;

namespace Daybreak.API.Interop;

[JsonConverter(typeof(PointerValueConverter))]
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

    public static implicit operator PointerValue(nuint address)
    {
        return new PointerValue(address);
    }

    public static implicit operator PointerValue(nint address)
    {
        return new PointerValue((nuint)address);
    }

    public static implicit operator PointerValue(int address)
    {
        return new PointerValue((nuint)address);
    }

    public static implicit operator PointerValue(uint address)
    {
        return new PointerValue(address);
    }

    public static PointerValue Parse(string s)
    {
        if (s.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
            && nuint.TryParse(s.AsSpan(2), System.Globalization.NumberStyles.HexNumber, null, out var v))
        {
            return new PointerValue(v);
        }

        throw new FormatException($"Invalid pointer format: {s}");
    }
}
