using System;
using System.Core.Extensions;
using System.IO;

namespace Daybreak.Services.ReShade.Utils;

/// <summary>
/// Stream that takes an input stream and treats the initial position of the input stream as the starting position
/// of the current stream.
/// </summary>
internal sealed class OffsetStream : Stream
{
    private readonly long startingPosition;
    private readonly Stream innerStream;

    public override bool CanRead { get; } = true;
    public override bool CanSeek { get; } = true;
    public override bool CanWrite { get; } = false;
    public override long Length => this.innerStream.Length - this.startingPosition;
    public override long Position
    {
        get => this.innerStream.Position - this.startingPosition;
        set => this.innerStream.Position = value + this.startingPosition;
    }

    public OffsetStream(Stream innerStream)
    {
        this.innerStream = innerStream.ThrowIfNull();
        this.startingPosition = this.innerStream.Position;
        if (!innerStream.CanSeek)
        {
            throw new ArgumentException($"Provided stream must have {nameof(this.innerStream.CanSeek)} set to true");
        }

        if (!innerStream.CanRead)
        {
            throw new ArgumentException($"Provided stream must have {nameof(this.innerStream.CanRead)} set to true");
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return this.innerStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        if (origin is SeekOrigin.Begin)
        {
            offset += this.startingPosition;
        }

        return this.innerStream.Seek(offset, origin) - this.startingPosition;
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override void Flush()
    {
        this.innerStream.Flush();
    }
}
