namespace Daybreak.Shared.Models;

/// <summary>
/// Represents a percentage value between 0.0 and 1.0
/// </summary>
public readonly struct Percentage
{
    public readonly double Value = 0;

    private Percentage(double value)
    {
        if (value > 1.0f ||
            value < 0.0f)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Percentage value must be between 0.0 and 1.0");
        }

        this.Value = value;
    }

    public static implicit operator double(Percentage percentage) => percentage.Value;

    public static implicit operator Percentage(double value) => new(value);
}
