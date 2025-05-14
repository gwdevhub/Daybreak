using Daybreak.Configuration.Options;
using Daybreak.Services.Scanner.Models;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Scanner;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

internal sealed class GuildwarsMemoryCache : IGuildwarsMemoryCache
{
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly ILiveOptions<MemoryReaderOptions> liveOptions;
    private readonly CachedData<LoginData?> loginDataCache = new();
    private readonly CachedData<WorldData?> worldDataCache = new();
    private readonly CachedData<SessionData?> sessionDataCache = new();
    private readonly CachedData<UserData?> userDataCache = new();
    private readonly CachedData<MainPlayerData?> mainPlayerDataCache = new();
    private readonly CachedData<TeamBuildData?> teamBuildDataCache = new();

    public GuildwarsMemoryCache(
        IGuildwarsMemoryReader guildwarsMemoryReader,
        ILiveOptions<MemoryReaderOptions> liveOptions)
    {
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
    }

    public async Task EnsureInitialized(GuildWarsApplicationLaunchContext context, CancellationToken cancellationToken)
    {
        await this.guildwarsMemoryReader.EnsureInitialized(context.ThrowIfNull().ProcessId, cancellationToken);
    }

    public Task<LoginData?> ReadLoginData(CancellationToken cancellationToken)
    {
        return this.ReadDataInternal(this.loginDataCache, this.guildwarsMemoryReader.ReadLoginData, cancellationToken);
    }

    public Task<WorldData?> ReadWorldData(CancellationToken cancellationToken)
    {
        return this.ReadDataInternal(this.worldDataCache, this.guildwarsMemoryReader.ReadWorldData, cancellationToken);
    }

    public Task<SessionData?> ReadSessionData(CancellationToken cancellationToken)
    {
        return this.ReadDataInternal(this.sessionDataCache, this.guildwarsMemoryReader.ReadSessionData, cancellationToken);
    }

    public Task<UserData?> ReadUserData(CancellationToken cancellationToken)
    {
        return this.ReadDataInternal(this.userDataCache, this.guildwarsMemoryReader.ReadUserData, cancellationToken);
    }

    public Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken)
    {
        return this.ReadDataInternal(this.mainPlayerDataCache, this.guildwarsMemoryReader.ReadMainPlayerData, cancellationToken);
    }

    public Task<TeamBuildData?> ReadTeamBuildData(CancellationToken cancellationToken)
    {
        return this.ReadDataInternal(this.teamBuildDataCache, this.guildwarsMemoryReader.ReadTeamBuildData, cancellationToken);
    }

    private async Task<T?> ReadDataInternal<T>(CachedData<T?> cachedData, Func<CancellationToken, Task<T?>> task, CancellationToken cancellationToken)
    {
        if (DateTime.Now - cachedData.SetTime <= TimeSpan.FromMilliseconds(this.liveOptions.Value.MemoryReaderFrequency))
        {
            return cachedData.Data;
        }

        var data = await task(cancellationToken);
        if (data is null)
        {
            return data;
        }

        cachedData.SetData(data);
        return cachedData.Data;
    }
}
