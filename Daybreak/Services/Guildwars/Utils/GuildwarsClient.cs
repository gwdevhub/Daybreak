﻿using Daybreak.Services.Guildwars.Models;
using Daybreak.Services.GuildWars.Utils;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Daybreak.Services.Guildwars.Utils;
internal sealed class GuildWarsClient
{
    public static async Task<(GuildWarsClientContext, ManifestResponse)?> Connect(CancellationToken cancellationToken)
    {
        for(var i = 1; i < 13; i++)
        {
            var url = $"file{i}.arenanetworks.com";
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                Blocking = false,
                ReceiveTimeout = 100
            };
            try
            {
                await socket.ConnectAsync(url, 6112, cancellationToken);
                var context = new GuildWarsClientContext { Socket = socket };
                var handshakeRequest = new HandshakeRequest
                {
                    Field1 = 1,
                    Field2 = 0,
                    Field3 = 0xF1,
                    Field4 = 0x10,
                    Field5 = 1,
                    Field6 = 0,
                    Field7 = 0
                };

                await Send(handshakeRequest, context, cancellationToken);
                var manifest = await ReceiveWait<ManifestResponse>(context, cancellationToken);
                return (context, manifest);
            }
            catch
            {
                socket.Dispose();
            }
        }

        return default;
    }

    public static async Task<FileResponse> GetFileResponse(GuildWarsClientContext guildwarsClientContext, int fileId, int version = 0, CancellationToken cancellationToken = default)
    {
        await Send(new FileRequest { Field1 = 0x3F2, Field2 = 0xC, FileId = fileId, Version = version }, guildwarsClientContext, cancellationToken);
        var metadata = await ReceiveWait<FileMetadataResponse>(guildwarsClientContext, cancellationToken);
        if (metadata.Field1 == 0x4F2)
        {
            // Could not find file
            return default;
        }
        else if (metadata.Field1 != 0x5F2)
        {
            // Unknown response
            return default;
        }

        var response = await ReceiveWait<FileResponse>(guildwarsClientContext, cancellationToken);
        return response;
    }

    public async Task<GuildwarsFileStream?> GetFileStream(GuildWarsClientContext guildwarsClientContext, int fileId, int version = 0, CancellationToken cancellationToken = default)
    {
        var response = await GetFileResponse(guildwarsClientContext, fileId, version, cancellationToken);
        return new GuildwarsFileStream(guildwarsClientContext, this, response.FileId, response.SizeCompressed, response.SizeDecompressed, response.Crc);
    }

    public static async Task<T> ReceiveWait<T>(GuildWarsClientContext context, CancellationToken cancellationToken)
        where T : struct
    {
        var size = Marshal.SizeOf<T>();
        var buffer = new Memory<byte>(new byte[size]);
        var received = 0;
        while (received < size)
        {
            var receiveTask = context.Socket.ReceiveAsync(buffer, cancellationToken).AsTask();
            if (await Task.WhenAny(receiveTask, Task.Delay(5000, cancellationToken)) != receiveTask)
            {
                throw new TaskCanceledException($"Timed out while receiving {typeof(T).Name}");
            }

            received += await receiveTask;
        }

        var bytes = buffer.ToArray();
        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        var str = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
        handle.Free();
        return str;
    }

    public static async Task Send<T>(T str, GuildWarsClientContext context, CancellationToken cancellationToken)
        where T : struct
    {
        var size = Marshal.SizeOf<T>();
        var bytes = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, bytes, 0, size);
        Marshal.FreeHGlobal(ptr);

        _ = await context.Socket.SendAsync(bytes, cancellationToken);
    }
}
