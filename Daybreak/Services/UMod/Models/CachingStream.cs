using System;
using System.Core.Extensions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod.Models;
/// <summary>
/// Stream that caches the contents until <see cref="Flush"/> is called.
/// </summary>
internal sealed class CachingStream : Stream
{
    private readonly Stream innerStream;
    private long cachedContentLength = 0;
    private byte[] innerBuffer;

    public override bool CanRead { get; } = false;
    public override bool CanSeek { get; } = false;
    public override bool CanWrite { get; } = true;
    public override long Length => this.cachedContentLength;
    public override long Position { get; set; }

    public CachingStream(Stream innerStream, int cacheSize = 1024)
    {
        this.innerStream = innerStream.ThrowIfNull();
        if (!innerStream.CanWrite)
        {
            throw new ArgumentException($"Provided stream must have {nameof(this.innerStream.CanWrite)} set to true");
        }

        this.innerBuffer = new byte[cacheSize];
    }

    public override void Flush()
    {
        this.innerStream.Write(this.innerBuffer, 0, (int)this.cachedContentLength);
        this.cachedContentLength = 0;
    }

    public override async Task FlushAsync(CancellationToken cancellationToken)
    {
        await this.innerStream.WriteAsync(this.innerBuffer, 0, (int)this.cachedContentLength, cancellationToken);
        this.cachedContentLength = 0;

    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (this.cachedContentLength + count > this.innerBuffer.Length)
        {
            var newBuffer = new byte[this.innerBuffer.Length * 2];
            Array.Copy(this.innerBuffer, newBuffer, count);
            this.innerBuffer = newBuffer;
        }

        Array.Copy(buffer, offset, this.innerBuffer, this.cachedContentLength, count);
        this.cachedContentLength += count;
    }

    public override void Close()
    {
        this.innerBuffer = null!;
        this.innerStream?.Dispose();
        base.Close();
    }
}
