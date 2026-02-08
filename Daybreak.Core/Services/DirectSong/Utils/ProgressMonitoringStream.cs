using System.Core.Extensions;

namespace Daybreak.Services.DirectSong.Utils;
internal sealed class ProgressMonitoringStream(Stream stream) : Stream
{
    private readonly Stream stream = stream.ThrowIfNull();

    public long ReadBytes { get; private set; }
    public override bool CanRead => this.stream.CanRead;
    public override bool CanSeek => this.stream.CanSeek;
    public override bool CanWrite => this.stream.CanWrite;
    public override long Length => this.stream.Length;
    public override long Position
    {
        get => this.stream.Position;
        set => this.stream.Position = value;
    }

    public override void Flush()
    {
        this.stream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var readBytes = this.stream.Read(buffer, offset, count);
        this.ReadBytes += readBytes;
        return readBytes;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return this.stream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        this.stream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        this.stream.Write(buffer, offset, count);
    }
}
