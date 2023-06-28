﻿using Daybreak.Models.Guildwars;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public interface IGuildwarsMemoryCache
{
    Task<LoginData?> ReadLoginData(CancellationToken cancellationToken);
    Task<GameData?> ReadGameData(CancellationToken cancellationToken);
    Task<PathingData?> ReadPathingData(CancellationToken cancellationToken);
    Task<PathingMetadata?> ReadPathingMetaData(CancellationToken cancellationToken);
    Task<InventoryData?> ReadInventoryData(CancellationToken cancellationToken);
    Task<WorldData?> ReadWorldData(CancellationToken cancellationToken);
}