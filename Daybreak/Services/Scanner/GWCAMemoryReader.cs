using Daybreak.Models.Guildwars;
using Daybreak.Models.GWCA;
using Daybreak.Services.GWCA;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public sealed class GWCAMemoryReader : IGuildwarsMemoryReader
{
    private readonly IGWCAClient client;
    private readonly ILogger logger;

    private ConnectionContext? connectionContextCache;

    public GWCAMemoryReader(
        IGWCAClient gWCAClient,
        ILogger<GWCAMemoryReader> logger)
    {
        this.client = gWCAClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task EnsureInitialized(Process process, CancellationToken cancellationToken)
    {
        process.ThrowIfNull();

        var maybeConnectionContext = await this.client.Connect(process, cancellationToken);
        if (maybeConnectionContext is not ConnectionContext)
        {
            throw new InvalidOperationException($"Unable to connect to desired process {process.Id}");
        }

        this.connectionContextCache = maybeConnectionContext;
    }

    public async Task<GameData?> ReadGameData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "game", cancellationToken);
        return default;
    }

    public async Task<InventoryData?> ReadInventoryData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "inventory", cancellationToken);
        return default;
    }

    public async Task<LoginData?> ReadLoginData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "game", cancellationToken);
        return default;
    }

    public async Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "game/mainplayer", cancellationToken);
        return default;
    }

    public async Task<PathingData?> ReadPathingData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "pathing", cancellationToken);
        return default;
    }

    public async Task<PathingMetadata?> ReadPathingMetaData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "pathing/metadata", cancellationToken);
        return default;
    }

    public async Task<PreGameData?> ReadPreGameData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "pregame", cancellationToken);
        return default;
    }

    public async Task<SessionData?> ReadSessionData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "session", cancellationToken);
        return default;
    }

    public async Task<UserData?> ReadUserData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "user", cancellationToken);
        return default;
    }

    public async Task<WorldData?> ReadWorldData(CancellationToken cancellationToken)
    {
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "map", cancellationToken);
        return default;
    }

    public void Stop()
    {
    }
}
