using Net.Sdk.Web.Websockets;
using System.Buffers;
using System.Net.WebSockets;

namespace Daybreak.API.WebSockets;

public abstract class UpdateWebSocketRoute
    : WebSocketRouteBase
{
    private readonly byte[] sharedBuffer;
    private readonly ArraySegment<byte> packetBuffer;
    private int sendingOperations = 0;

    protected abstract int BufferSize { get; }

    public UpdateWebSocketRoute()
    {
        this.sharedBuffer = ArrayPool<byte>.Shared.Rent(this.BufferSize);
        this.packetBuffer = new ArraySegment<byte>(this.sharedBuffer);
    }

    public override Task SocketClosed()
    {
        ArrayPool<byte>.Shared.Return(this.sharedBuffer);
        return base.SocketClosed();
    }

    public void SendUpdate(ReadOnlySpan<byte> state)
    {
        if (Interlocked.CompareExchange(ref this.sendingOperations, 1, 0) != 0)
        {
            return;
        }

        if (this.WebSocket is null)
        {
            this.sendingOperations = 0;
            return;
        }

        state.CopyTo(this.packetBuffer);
        var packet = this.packetBuffer.Slice(0, state.Length);
        this.WebSocket?.SendAsync(packet, WebSocketMessageType.Binary, true, this.Context?.RequestAborted ?? CancellationToken.None)
            .ContinueWith(_ => this.sendingOperations = 0);
    }
}
