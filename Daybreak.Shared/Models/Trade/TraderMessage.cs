using System;

namespace Daybreak.Models.Trade;

public sealed class TraderMessage : IEquatable<TraderMessage>
{
    public string Message { get; init; } = string.Empty;

    public string Sender { get; init; } = string.Empty;

    public DateTime Timestamp { get; init; }

    public bool Equals(TraderMessage? other)
    {
        if (other is null)
        {
            return false;
        }

        return this.Timestamp == other.Timestamp &&
            this.Sender == other.Sender &&
            this.Message == other.Message;
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as TraderMessage);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Message, this.Sender, this.Timestamp);
    }
}
