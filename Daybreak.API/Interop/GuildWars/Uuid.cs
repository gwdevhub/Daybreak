using System.Globalization;

namespace Daybreak.API.Interop.GuildWars;

public readonly struct Uuid(
    uint timeLow,
    ushort timeMid,
    ushort timeHighAndVersion,
    byte clockSqHi,
    byte clockSqLow,
    byte node0,
    byte node1,
    byte node2,
    byte node3,
    byte node4,
    byte node5) : IEquatable<Uuid>
{
    public const uint NodeLength = 6;

    public readonly uint TimeLow = timeLow;
    public readonly ushort TimeMid = timeMid;
    public readonly ushort TimeHighAndVersion = timeHighAndVersion;
    public readonly byte ClockSqHiAndRes = clockSqHi;
    public readonly byte ClockSqLow = clockSqLow;
    public readonly byte Node0 = node0;
    public readonly byte Node1 = node1;
    public readonly byte Node2 = node2;
    public readonly byte Node3 = node3;
    public readonly byte Node4 = node4;
    public readonly byte Node5 = node5;

    public bool Equals(Uuid other)
    {
        return this.TimeLow == other.TimeLow &&
            this.TimeMid == other.TimeMid &&
            this.TimeHighAndVersion == other.TimeHighAndVersion &&
            this.ClockSqHiAndRes == other.ClockSqHiAndRes &&
            this.ClockSqLow == other.ClockSqLow &&
            this.Node0 == other.Node0 &&
            this.Node1 == other.Node1 &&
            this.Node2 == other.Node2 &&
            this.Node3 == other.Node3 &&
            this.Node4 == other.Node4 &&
            this.Node5 == other.Node5;
    }

    public override bool Equals(object? obj)
    {
        return obj is Uuid uuid && this.Equals(uuid);
    }

    public static bool operator ==(Uuid left, Uuid right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Uuid left, Uuid right)
    {
        return !(left == right);
    }

    public static bool TryParse(string uuidString, out Uuid uuid)
    {
        if (uuidString.Length != 36)
        {
            uuid = default;
            return false;
        }

        var parts = uuidString.Split('-');
        if (parts.Length != 5)
        {
            uuid = default;
            return false;
        }

        if (!uint.TryParse(parts[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var timeLow) ||
            !ushort.TryParse(parts[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var timeMid) ||
            !ushort.TryParse(parts[2], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var timeHighAndVersion) ||
            !byte.TryParse(parts[3].AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var clockSqHi) ||
            !byte.TryParse(parts[3].AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var clockSqLow) ||
            !byte.TryParse(parts[4].AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var node0) ||
            !byte.TryParse(parts[4].AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var node1) ||
            !byte.TryParse(parts[4].AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var node2) ||
            !byte.TryParse(parts[4].AsSpan(6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var node3) ||
            !byte.TryParse(parts[4].AsSpan(8, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var node4) ||
            !byte.TryParse(parts[4].AsSpan(10, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var node5))
        {
            uuid = default;
            return false;
        }

        uuid = new Uuid(timeLow, timeMid, timeHighAndVersion, clockSqHi, clockSqLow, node0, node1, node2, node3, node4, node5);
        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine<uint, ushort, ushort, byte, byte, byte[]>(this.TimeLow, this.TimeMid, this.TimeHighAndVersion, this.ClockSqHiAndRes, this.ClockSqLow, [this.Node0, this.Node1, this.Node2, this.Node3, this.Node4, this.Node5]);
    }

    public override string ToString()
    {
        return $"{this.TimeLow:X8}-{this.TimeMid:X4}-{this.TimeHighAndVersion:X4}-{this.ClockSqHiAndRes:X2}{this.ClockSqLow:X2}-{this.Node0:X2}{this.Node1:X2}{this.Node2:X2}{this.Node3:X2}{this.Node4:X2}{this.Node5:X2}";
    }

    public readonly static Uuid Zero = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
}
