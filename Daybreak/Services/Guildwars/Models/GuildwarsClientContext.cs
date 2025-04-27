using System;
using System.Net.Sockets;

namespace Daybreak.Services.Guildwars.Models;
internal readonly struct GuildWarsClientContext : IDisposable
{
    public Socket Socket { get; init; }

    public void Dispose()
    {
        this.Socket.Dispose();
    }
}
