﻿using Daybreak.Models.Guildwars;
using Daybreak.Models.LaunchConfigurations;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public interface IGuildwarsMemoryCache
{
    Task EnsureInitialized(GuildWarsApplicationLaunchContext context, CancellationToken cancellationToken);
    Task<LoginData?> ReadLoginData(CancellationToken cancellationToken);
    Task<GameData?> ReadGameData(CancellationToken cancellationToken);
    Task<PathingData?> ReadPathingData(CancellationToken cancellationToken);
    Task<PathingMetadata?> ReadPathingMetaData(CancellationToken cancellationToken);
    Task<InventoryData?> ReadInventoryData(CancellationToken cancellationToken);
    Task<WorldData?> ReadWorldData(CancellationToken cancellationToken);
    Task<SessionData?> ReadSessionData(CancellationToken cancellationToken);
    Task<UserData?> ReadUserData(CancellationToken cancellationToken);
    Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken);
    Task<ConnectionData?> ReadConnectionData(CancellationToken cancellationToken);
    Task<PreGameData?> ReadPreGameData(CancellationToken cancellationToken);
}
