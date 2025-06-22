using System.Core.Extensions;

namespace Daybreak.API.Models;

public sealed class ByteConsumerEntry(
    Guid id, TimeSpan freq, Action<ReadOnlySpan<byte>> handler)
{
    private readonly Action<ReadOnlySpan<byte>> handler = handler.ThrowIfNull();

    private DateTimeOffset lastConsume = DateTimeOffset.MinValue;

    public Guid Id { get; } = id;
    public TimeSpan Frequency { get; } = freq;

    public void TryConsume(DateTimeOffset currentTime, ReadOnlySpan<byte> value)
    {
        if (currentTime - this.lastConsume >= this.Frequency)
        {
            this.handler(value);
            this.lastConsume = currentTime;
        }
    }
}
