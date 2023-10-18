using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public sealed class CompositeMemoryReader : IGuildwarsMemoryReader
{
    private readonly GuildwarsMemoryReader guildwarsMemoryReader;
    private readonly GWCAMemoryReader gWCAMemoryReader;
    private readonly ILiveOptions<FocusViewOptions> liveOptions;
    private readonly ILogger<CompositeMemoryReader> logger;

    public CompositeMemoryReader(
        GuildwarsMemoryReader guildwarsMemoryReader,
        GWCAMemoryReader gWCAMemoryReader,
        ILiveOptions<FocusViewOptions> liveOptions,
        ILogger<CompositeMemoryReader> logger)
    {
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.gWCAMemoryReader = gWCAMemoryReader.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public Task EnsureInitialized(Process process, CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.EnsureInitialized(process, cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.EnsureInitialized(process, cancellationToken);
        }
    }

    public Task<GameData?> ReadGameData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadGameData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadGameData(cancellationToken);
        }
    }

    public Task<InventoryData?> ReadInventoryData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadInventoryData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadInventoryData(cancellationToken);
        }
    }

    public Task<LoginData?> ReadLoginData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadLoginData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadLoginData(cancellationToken);
        }
    }

    public Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadMainPlayerData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadMainPlayerData(cancellationToken);
        }
    }

    public Task<PathingData?> ReadPathingData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadPathingData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadPathingData(cancellationToken);
        }
    }

    public Task<PathingMetadata?> ReadPathingMetaData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadPathingMetaData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadPathingMetaData(cancellationToken);
        }
    }

    public Task<PreGameData?> ReadPreGameData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadPreGameData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadPreGameData(cancellationToken);
        }
    }

    public Task<SessionData?> ReadSessionData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadSessionData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadSessionData(cancellationToken);
        }
    }

    public Task<UserData?> ReadUserData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadUserData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadUserData(cancellationToken);
        }
    }

    public Task<WorldData?> ReadWorldData(CancellationToken cancellationToken)
    {
        if (this.liveOptions.Value.GWCAIntegration)
        {
            return this.gWCAMemoryReader.ReadWorldData(cancellationToken);
        }
        else
        {
            return this.guildwarsMemoryReader.ReadWorldData(cancellationToken);
        }
    }

    public void Stop()
    {
        if (!this.liveOptions.Value.GWCAIntegration)
        {
            this.guildwarsMemoryReader.Stop();
        }
    }
}
