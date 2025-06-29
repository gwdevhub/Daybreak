using Daybreak.Services.Guildwars.Models;
using Daybreak.Services.Guildwars.Utils;
using System.Core.Extensions;
using System.IO;

namespace Daybreak.Services.GuildWars.Utils;
internal sealed class GuildwarsFileStream(GuildWarsClientContext guildwarsClientContext, GuildWarsClient guildwarsClient, int fileId, int sizeCompressed, int sizeDecompressed, int crc) : Stream
{
    private readonly GuildWarsClient guildwarsClient = guildwarsClient.ThrowIfNull();
    private readonly GuildWarsClientContext guildwarsClientContext = guildwarsClientContext;

    private readonly Memory<byte> downloadBuffer = new(new byte[4096]);

    private Memory<byte>? chunkBuffer;
    private int positionInBuffer = 0;
    private int chunkSize = 0;

    public int FileId { get; init; } = fileId;
    public int SizeCompressed { get; init; } = sizeCompressed;
    public int SizeDecompressed { get; init; } = sizeDecompressed;
    public int Crc { get; init; } = crc;

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => this.SizeCompressed;
    public override long Position { get; set; }

    public override void Flush()
    {
        throw new System.NotImplementedException();
    }

    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (this.Position >= this.Length)
        {
            return 0;
        }

        if (this.positionInBuffer < this.chunkSize)
        {
            var read = this.ReadCurrentChunkBytes(buffer);
            this.Position += read;
            return read;
        }

        // If we have already requested a previous chunk, we need to request more data
        if (this.chunkSize > 0)
        {
            await GuildWarsClient.Send(new FileRequestNextChunk { Field1 = 0x7F3, Field2 = 0x8, Field3 = (uint)this.chunkSize }, this.guildwarsClientContext, cancellationToken);
        }

        var meta = await GuildWarsClient.ReceiveWait<FileMetadataResponse>(this.guildwarsClientContext, cancellationToken);
        if (meta.Field1 != 0x6F2 && meta.Field1 != 0x6F3)
        {
            throw new InvalidOperationException($"Unknown header in response {meta.Field1:X4}");
        }

        this.chunkSize = meta.Field2 - 4;
        if (this.chunkBuffer is null ||
            this.chunkBuffer.Value.Length != this.chunkSize)
        {
            this.chunkBuffer = new Memory<byte>(new byte[this.chunkSize]);
        }

        var downloadedChunkSize = 0;
        do
        {
            var readTask = this.guildwarsClientContext.Socket.ReceiveAsync(this.downloadBuffer, cancellationToken).AsTask();
            if (await Task.WhenAny(readTask, Task.Delay(5000, cancellationToken)) != readTask)
            {
                throw new TaskCanceledException("Timed out waiting for download");
            }

            var read = await readTask;
            this.downloadBuffer[..read].CopyTo(this.chunkBuffer.Value[downloadedChunkSize..]);
            downloadedChunkSize += read;
        } while (downloadedChunkSize < this.chunkSize);

        this.positionInBuffer = 0;
        var chunkRead = this.ReadCurrentChunkBytes(buffer);
        this.Position += chunkRead;
        return chunkRead;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return System.Extensions.TaskExtensions.RunSync(() => this.ReadAsync(buffer, offset, count));
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new System.NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new System.NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new System.NotImplementedException();
    }

    private int ReadCurrentChunkBytes(Memory<byte> buffer)
    {
        if (this.chunkBuffer is null)
        {
            throw new InvalidOperationException("No chunk buffer ready");
        }

        var bytesToRead = Math.Min(buffer.Length, this.chunkSize - this.positionInBuffer);
        this.chunkBuffer.Value.Slice(this.positionInBuffer, bytesToRead).CopyTo(buffer);
        this.positionInBuffer += bytesToRead;
        return bytesToRead;
    }
}
