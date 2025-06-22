using MemoryPack;
using System.Buffers;

namespace Daybreak.API.Interop;

public static class MemoryPackExtensions
{
    /// <summary>Serialize <paramref name="value"/> into <paramref name="destination"/>.</summary>
    /// <returns>Number of bytes written.</returns>
    public static int SerializeToSpan<T>(T value, Span<byte> destination)
    {
        var dummy = DummyBufferWriter.Instance;

        using var state = MemoryPackWriterOptionalStatePool.Rent(null);

        var writer = new MemoryPackWriter<DummyBufferWriter>(ref dummy, destination, state);

        writer.WriteValue(value);
        writer.Flush();

        return writer.WrittenCount;
    }

    private sealed class DummyBufferWriter : IBufferWriter<byte>
    {
        public static readonly DummyBufferWriter Instance = new();
        public void Advance(int count) { /* ignore – we never overflow destination */ }
        public Memory<byte> GetMemory(int sizeHint = 0) => throw new NotSupportedException();
        public Span<byte> GetSpan(int sizeHint = 0) => throw new NotSupportedException();
    }
}
