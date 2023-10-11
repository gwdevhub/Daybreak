using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Interop;
using Daybreak.Models.LaunchConfigurations;
using Daybreak.Models.Metrics;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Metrics;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Globalization;
using System.Linq;
using System.Logging;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Scanner;

public sealed class GuildwarsMemoryReader : IGuildwarsMemoryReader
{
    private const int MaxTrapezoidCount = 1000000;
    private const int RetryInitializationCount = 5;
    private const string LatencyMeterName = "Memory Reader Latency";
    private const string LatencyMeterUnitsName = "Milliseconds";
    private const string LatencyMeterDescription = "Amount of milliseconds elapsed while reading memory. P95 aggregation";

    private readonly IMemoryScanner memoryScanner;
    private readonly Histogram<double> latencyMeter;
    private readonly ILogger<GuildwarsMemoryReader> logger;

    private uint playerIdPointer;
    private uint entityArrayPointer;
    private uint targetIdPointer;
    private uint instanceInfoPointer;
    private uint preGameContextPointer;

    public GuildwarsMemoryReader(
        IMemoryScanner memoryScanner,
        IMetricsService metricsService,
        ILogger<GuildwarsMemoryReader> logger)
    {
        this.memoryScanner = memoryScanner.ThrowIfNull();
        this.latencyMeter = metricsService.ThrowIfNull().CreateHistogram<double>(LatencyMeterName, LatencyMeterUnitsName, LatencyMeterDescription, AggregationTypes.P95);
        this.logger = logger.ThrowIfNull();
    }
    
    public async Task EnsureInitialized(Process process, CancellationToken cancellationToken)
    {
        var scoppedLogger = this.logger.CreateScopedLogger(nameof(this.EnsureInitialized), default!);
        var currentGuildwarsProcess = process.ThrowIfNull();
        if (currentGuildwarsProcess is null)
        {
            scoppedLogger.LogWarning($"Process is null. {nameof(GuildwarsMemoryReader)} will not start");
            return;
        }

        try
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            await this.InitializeSafe(currentGuildwarsProcess, scoppedLogger);
        }
        catch(Exception e)
        {
            scoppedLogger.LogError(e, "Encountered exception during initialization");
        }
    }
    
    public void Stop()
    {
        var scoppedLogger = this.logger.CreateScopedLogger(nameof(this.Stop), default!);
        scoppedLogger.LogInformation($"Stopping {nameof(GuildwarsMemoryReader)}");
        this.memoryScanner?.EndScanner();
    }

    public Task<LoginData?> ReadLoginData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<LoginData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadLoginDataInternal), cancellationToken);
    }

    public Task<GameData?> ReadGameData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<GameData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadGameDataInternal), cancellationToken);
    }

    public Task<PathingData?> ReadPathingData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<PathingData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadPathingDataInternal), cancellationToken);
    }

    public Task<PathingMetadata?> ReadPathingMetaData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<PathingMetadata?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadPathingMetaDataInternal), cancellationToken);
    }

    public Task<InventoryData?> ReadInventoryData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<InventoryData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadInventoryDataInternal), cancellationToken);
    }

    public Task<WorldData?> ReadWorldData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<WorldData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadWorldDataInternal), cancellationToken);
    }

    public Task<UserData?> ReadUserData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<UserData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadUserDataInternal), cancellationToken);
    }

    public Task<SessionData?> ReadSessionData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<SessionData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadSessionDataInternal), cancellationToken);
    }

    public Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<MainPlayerData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadMainPlayerDataInternal), cancellationToken);
    }

    public Task<ConnectionData?> ReadConnectionData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<ConnectionData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadConnectionData), cancellationToken);
    }

    public Task<PreGameData?> ReadPreGameData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<PreGameData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadPreGameData), cancellationToken);
    }

    private async Task InitializeSafe(Process process, ScopedLogger<GuildwarsMemoryReader> scopedLogger)
    {
        if (this.memoryScanner.Process is null ||
            this.memoryScanner.Process.HasExited ||
            this.memoryScanner.Process.MainModule?.FileName != process?.MainModule?.FileName)
        {
            if (this.memoryScanner.Scanning)
            {
                scopedLogger.LogInformation("Scanner is already scanning a different process. Restart scanner and target the new process");
                this.memoryScanner.EndScanner();
            }

            scopedLogger.LogInformation($"Initializing {nameof(GuildwarsMemoryReader)}");
            await this.ResilientBeginScanner(scopedLogger, process!);
        }

        if (this.memoryScanner.Scanning is false &&
            process?.HasExited is false)
        {
            scopedLogger.LogInformation($"Initializing {nameof(GuildwarsMemoryReader)}");
            await this.ResilientBeginScanner(scopedLogger, process!);
        }
    }

    private async Task ResilientBeginScanner(ScopedLogger<GuildwarsMemoryReader> scopedLogger, Process process)
    {
        for (var i = 0; i < RetryInitializationCount; i++)
        {
            try
            {
                scopedLogger.LogInformation("Initializing scanner");
                this.memoryScanner.BeginScanner(process!);
                break;
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "Error during initialization");
                await Task.Delay(1000);
            }
        }
    }

    private T? SafeReadGameMemory<T>(Func<T> readingFunction)
    {
        try
        {
            var stopWatch = Stopwatch.StartNew();
            var ret = readingFunction();
            stopWatch.Stop();
            this.latencyMeter.Record(stopWatch.Elapsed.TotalMilliseconds);

            return ret;
        }
        catch(Exception e)
        {
            this.logger.LogError(e, "Exception encountered when reading game memory");
            return default;
        }
    }

    private LoginData? ReadLoginDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        if (!globalContext.GameContext.IsValid() ||
            !globalContext.UserContext.IsValid() ||
            !globalContext.InstanceContext.IsValid())
        {
            return default;
        }

        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        var loginData = new LoginData
        {
            Email = userContext.PlayerEmailFirstChar + (userContext.PlayerEmailSecondChar + userContext.PlayerEmailRemaining),
            PlayerName = userContext.PlayerName
        };

        if (!IsValidEmail(loginData.Email))
        {
            return default;
        }

        return loginData;
    }

    private GameData? ReadGameDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        if (!globalContext.GameContext.IsValid() ||
            !globalContext.UserContext.IsValid() ||
            !globalContext.InstanceContext.IsValid())
        {
            return default;
        }

        // GameContext struct is offset by 0x07C due to the memory layout of the structure.
        var gameContext = this.memoryScanner.Read(globalContext.GameContext, GameContext.BaseOffset);
        // InstanceContext struct is offset by 0x01AC due to memory layout of the structure.
        var instanceContext = this.memoryScanner.Read(globalContext.InstanceContext, InstanceContext.BaseOffset);
        
        if (gameContext.MapEntities.Size > 10000 ||
            gameContext.Professions.Size > 10000 ||
            gameContext.Players.Size > 1000 ||
            gameContext.QuestLog.Size > 10000 ||
            gameContext.Skillbars.Size > 100 ||
            gameContext.PartyAttributes.Size > 1000 ||
            gameContext.Titles.Size > 1000 ||
            gameContext.TitlesTiers.Size > 10000)
        {
            return default;
        }

        var mapEntities = this.memoryScanner.ReadArray(gameContext.MapEntities);
        var mapIcons = this.memoryScanner.ReadArray(gameContext.MissionMapIcons);
        var professions = this.memoryScanner.ReadArray(gameContext.Professions);
        var players = this.memoryScanner.ReadArray(gameContext.Players);
        var quests = this.memoryScanner.ReadArray(gameContext.QuestLog);
        var skills = this.memoryScanner.ReadArray(gameContext.Skillbars);
        var partyAttributes = this.memoryScanner.ReadArray(gameContext.PartyAttributes);
        var playerEntityId = this.memoryScanner.ReadPtrChain<int>(this.GetPlayerIdPointer(), 0x0, 0x0);
        var targetEntityId = this.memoryScanner.ReadPtrChain<int>(this.GetTargetIdPointer(), 0x0, 0x0);
        var titles = this.memoryScanner.ReadArray(gameContext.Titles);
        var titleTiers = this.memoryScanner.ReadArray(gameContext.TitlesTiers);
        var npcs = this.memoryScanner.ReadArray(gameContext.Npcs);

        // The following lines would retrieve all entities, including item entities.
        var entityPointersArray = this.memoryScanner.ReadPtrChain<GenericGuildwarsArray>(this.GetEntityArrayPointer(), 0x0, 0x0);
        if (entityPointersArray.Size > 10000)
        {
            return new GameData { Valid = false };
        }

        var entityPointers = this.memoryScanner.ReadArray<uint>(entityPointersArray.Buffer, entityPointersArray.Size);
        var entities = entityPointers.Select(ptr => (ptr, this.memoryScanner.Read<EntityContext>(ptr))).ToArray();

        return this.AggregateGameData(
            gameContext,
            instanceContext,
            mapEntities,
            entities.Select(e => e.Item2).ToArray(),
            players,
            professions,
            quests,
            skills,
            partyAttributes,
            titles,
            titleTiers,
            mapIcons,
            npcs,
            playerEntityId,
            targetEntityId);
    }

    private PathingData? ReadPathingDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        var mapContext = this.memoryScanner.Read(globalContext.MapContext);

        var pathingMapContext = this.memoryScanner.ReadPtrChain<PathingMapContext>(mapContext.PathingMapContextPtr, 0x0, 0x0);
        if (!pathingMapContext.PathingMapArray.IsValidArray(true) ||
            pathingMapContext.PathingMapArray.Size > 10000)
        {
            return default;
        }

        var pathingMaps = this.memoryScanner.ReadArray(pathingMapContext.PathingMapArray);
        var trapezoidsCount = (int)pathingMaps.Select(p => p.TrapezoidCount).Sum(count => count);
        if (trapezoidsCount > MaxTrapezoidCount)
        {
            return default;
        }

        var trapezoidList = new List<Trapezoid>();
        var adjacencyList = new List<List<int>>();
        var ogPathingMapList = new List<List<int>>();
        for (var pathingMapIndex = 0; pathingMapIndex < pathingMaps.Length; pathingMapIndex++)
        {
            var pathingMap = pathingMaps[pathingMapIndex];
            var pathingMapList = new List<int>();
            ogPathingMapList.Add(pathingMapList);
            var pathingTrapezoids = this.memoryScanner.ReadArray<PathingTrapezoid>(pathingMap.TrapezoidArray, pathingMap.TrapezoidCount);
            foreach(var trapezoid in pathingTrapezoids)
            {
                pathingMapList.Add((int)trapezoid.Id);
                trapezoidList.Add(new Trapezoid
                {
                    Id = (int)trapezoid.Id,
                    PathingMapId = pathingMapIndex,
                    XTL = trapezoid.XTL,
                    XTR = trapezoid.XTR,
                    YT = trapezoid.YT,
                    XBL = trapezoid.XBL,
                    XBR = trapezoid.XBR,
                    YB = trapezoid.YB,
                });

                var trapezoidAdjacencyList = new List<int>();
                adjacencyList.Add(trapezoidAdjacencyList);
                foreach(var adjacentAddress in new uint[]
                    {
                        trapezoid.AdjacentPathingTrapezoid1,
                        trapezoid.AdjacentPathingTrapezoid2,
                        trapezoid.AdjacentPathingTrapezoid3,
                        trapezoid.AdjacentPathingTrapezoid4
                    })
                {
                    if (adjacentAddress == 0)
                    {
                        continue;
                    }

                    var adjacentTrapezoid = this.memoryScanner.Read<PathingTrapezoid>(adjacentAddress);
                    trapezoidAdjacencyList.Add((int)adjacentTrapezoid.Id);
                }
            }
        }

        var computedPathingMaps = BuildPathingMaps(trapezoidList, adjacencyList);
        var computedAdjacencyList = BuildFinalAdjacencyList(trapezoidList, computedPathingMaps, adjacencyList);

        return new PathingData
        {
            Trapezoids = trapezoidList,
            OriginalAdjacencyList = adjacencyList,
            ComputedPathingMaps = computedPathingMaps,
            OriginalPathingMaps = ogPathingMapList,
            ComputedAdjacencyList = computedAdjacencyList
        };
    }

    private PathingMetadata? ReadPathingMetaDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        var mapContext = this.memoryScanner.Read(globalContext.MapContext);

        var pathingMapContext = this.memoryScanner.ReadPtrChain<PathingMapContext>(mapContext.PathingMapContextPtr, 0x0, 0x0);
        if (!pathingMapContext.PathingMapArray.IsValidArray(true) ||
            pathingMapContext.PathingMapArray.Size > 10000)
        {
            return default;
        }

        var pathingMaps = this.memoryScanner.ReadArray(pathingMapContext.PathingMapArray);
        return new PathingMetadata { TrapezoidCount = (int)pathingMaps.Select(p => p.TrapezoidCount).Sum(count => count) };
    }

    private InventoryData? ReadInventoryDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        if (!globalContext.GameContext.IsValid() ||
            !globalContext.UserContext.IsValid() ||
            !globalContext.InstanceContext.IsValid())
        {
            return default;
        }

        var itemContext = this.memoryScanner.Read(globalContext.ItemContext);
        var inventory = this.memoryScanner.Read(itemContext.Inventory);
        var inventoryData = new InventoryData
        {
            Backpack = this.GetBag(inventory.Backpack, 1, true),
            BeltPouch = this.GetBag(inventory.BeltPouch, 1, true),
            Bags = new List<Bag?>
            {
                this.GetBag(inventory.Bag1, 1, true),
                this.GetBag(inventory.Bag2, 1, true)
            },
            EquipmentPack = this.GetBag(inventory.EquipmentPack, 1, true),
            MaterialStorage = this.GetBag(inventory.MaterialStorage, 5, true),
            UnclaimedItems = this.GetBag(inventory.UnclaimedItems, 3, true),
            EquippedItems = this.GetBag(inventory.EquippedItems, 2, true),
            StoragePanes = inventory.StoragePanes.Select(b => this.GetBag(b, 4, false)).ToList()
        };

        return inventoryData;
    }

    private WorldData? ReadWorldDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        if (!globalContext.GameContext.IsValid() ||
            !globalContext.UserContext.IsValid() ||
            !globalContext.InstanceContext.IsValid())
        {
            return default;
        }

        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        var instanceInfo = this.memoryScanner.ReadPtrChain<InstanceInfoContext>(this.GetInstanceInfoPointer(), 0x0, 0x0);
        var areaInfo = this.memoryScanner.Read(instanceInfo.AreaInfo);

        _ = Map.TryParse((int)userContext.MapId, out var map);
        _ = Region.TryParse((int)areaInfo.RegionId, out var region);
        _ = Continent.TryParse((int)areaInfo.ContinentId, out var continent);
        _ = Campaign.TryParse((int)areaInfo.CampaignId, out var campaign);

        return new WorldData
        {
            Campaign = campaign,
            Continent = continent,
            Region = region,
            Map = map
        };
    }

    private UserData? ReadUserDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        if (!globalContext.GameContext.IsValid() ||
            !globalContext.UserContext.IsValid() ||
            !globalContext.InstanceContext.IsValid())
        {
            return default;
        }

        // GameContext struct is offset by 0x07C due to the memory layout of the structure.
        var gameContext = this.memoryScanner.Read(globalContext.GameContext, GameContext.BaseOffset);
        // UserContext struct is offset by 0x074 due to the memory layout of the structure.
        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);

        var userInformation = new UserInformation
        {
            Email = userContext.PlayerEmailFirstChar + (userContext.PlayerEmailSecondChar + userContext.PlayerEmailRemaining),
            CurrentKurzickPoints = gameContext.CurrentKurzick,
            TotalKurzickPoints = gameContext.TotalKurzick,
            MaxKurzickPoints = gameContext.MaxKurzick,
            CurrentLuxonPoints = gameContext.CurrentLuxon,
            TotalLuxonPoints = gameContext.TotalLuxon,
            MaxLuxonPoints = gameContext.MaxLuxon,
            CurrentImperialPoints = gameContext.CurrentImperial,
            TotalImperialPoints = gameContext.TotalImperial,
            MaxImperialPoints = gameContext.MaxImperial,
            CurrentBalthazarPoints = gameContext.CurrentBalthazar,
            TotalBalthazarPoints = gameContext.TotalBalthazar,
            MaxBalthazarPoints = gameContext.MaxBalthazar,
            CurrentSkillPoints = gameContext.CurrentSkillPoints,
            TotalSkillPoints = gameContext.TotalSkillPoints
        };

        return new UserData { User = userInformation };
    }

    private SessionData? ReadSessionDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        if (!globalContext.GameContext.IsValid() ||
            !globalContext.UserContext.IsValid() ||
            !globalContext.InstanceContext.IsValid())
        {
            return default;
        }

        // GameContext struct is offset by 0x07C due to the memory layout of the structure.
        var gameContext = this.memoryScanner.Read(globalContext.GameContext, GameContext.BaseOffset);
        // InstanceContext struct is offset by 0x01AC due to memory layout of the structure.
        var instanceContext = this.memoryScanner.Read(globalContext.InstanceContext, InstanceContext.BaseOffset);
        // UserContext struct is offset by 0x074 due to the memory layout of the structure.
        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        var instanceInfo = this.memoryScanner.ReadPtrChain<InstanceInfoContext>(this.GetInstanceInfoPointer(), 0x0, 0x0);

        _ = Map.TryParse((int)userContext.MapId, out var currentMap);
        var sessionInformation = new SessionInformation
        {
            FoesKilled = gameContext.FoesKilled,
            FoesToKill = gameContext.FoesToKill,
            CurrentMap = currentMap,
            InstanceTimer = instanceContext.Timer,
            InstanceType = instanceInfo.InstanceType switch
            {
                Daybreak.Models.Interop.InstanceType.Outpost => Daybreak.Models.Guildwars.InstanceType.Outpost,
                Daybreak.Models.Interop.InstanceType.Explorable => Daybreak.Models.Guildwars.InstanceType.Explorable,
                Daybreak.Models.Interop.InstanceType.Loading => Daybreak.Models.Guildwars.InstanceType.Loading,
                _ => Daybreak.Models.Guildwars.InstanceType.Undefined
            }
        };

        return new SessionData { Session = sessionInformation };
    }

    private MainPlayerData? ReadMainPlayerDataInternal()
    {
        var globalContext = this.GetGlobalContext();
        if (!globalContext.GameContext.IsValid() ||
            !globalContext.UserContext.IsValid() ||
            !globalContext.InstanceContext.IsValid())
        {
            return default;
        }

        // GameContext struct is offset by 0x07C due to the memory layout of the structure.
        var gameContext = this.memoryScanner.Read(globalContext.GameContext, GameContext.BaseOffset);
        // InstanceContext struct is offset by 0x01AC due to memory layout of the structure.
        var instanceContext = this.memoryScanner.Read(globalContext.InstanceContext, InstanceContext.BaseOffset);

        var playerEntityId = this.memoryScanner.ReadPtrChain<int>(this.GetPlayerIdPointer(), 0x0, 0x0);
        var mapEntity = this.memoryScanner.ReadItemAtIndex(gameContext.MapEntities, playerEntityId);
        var entityPointersArray = this.memoryScanner.ReadPtrChain<GenericGuildwarsArray>(this.GetEntityArrayPointer(), 0x0, 0x0);
        var entity = this.memoryScanner.ReadPtrChain<EntityContext>((uint)(entityPointersArray.Buffer + (sizeof(uint) * playerEntityId)), 0x0, 0x0);
        if (mapEntity.MaxHealth != entity.MaxHealth ||
            mapEntity.MaxEnergy != entity.MaxEnergy ||
            mapEntity.MaxHealth < 0 || mapEntity.MaxHealth > 10000 ||
            mapEntity.MaxEnergy < 0 || mapEntity.MaxEnergy > 10000 ||
            gameContext.Professions.Size == 0 ||
            gameContext.Players.Size == 0 ||
            gameContext.Skillbars.Size == 0 ||
            gameContext.PartyAttributes.Size == 0 ||
            gameContext.Titles.Size == 0 ||
            gameContext.TitlesTiers.Size == 0)
        {
            return default;
        }

        var professions = this.memoryScanner.ReadArray(gameContext.Professions);
        var players = this.memoryScanner.ReadArray(gameContext.Players);
        var quests = this.memoryScanner.ReadArray(gameContext.QuestLog);
        var skills = this.memoryScanner.ReadArray(gameContext.Skillbars);
        var partyAttributes = this.memoryScanner.ReadArray(gameContext.PartyAttributes);
        var titles = this.memoryScanner.ReadArray(gameContext.Titles);
        var titleTiers = this.memoryScanner.ReadArray(gameContext.TitlesTiers);
        var mainPlayerInfo = this.GetMainPlayerInformation(
            gameContext,
            instanceContext,
            players.FirstOrDefault(p => p.AgentId == playerEntityId),
            mapEntity,
            professions.FirstOrDefault(p => p.AgentId == playerEntityId),
            quests,
            skills.FirstOrDefault(p => p.AgentId == playerEntityId),
            partyAttributes.FirstOrDefault(p => p.AgentId == playerEntityId),
            titles,
            titleTiers,
            entity);

        return new MainPlayerData
        {
            PlayerInformation = mainPlayerInfo
        };
    }

    private ConnectionData? ReadConnectionData()
    {
        /*
         * IP Address in long format is at one of the following addresses:
         * startAddress + 00629204 -> +0x18 -> +0x44 -> +0x1A4
         */

        var ipAddressContext = this.memoryScanner.ReadPtrChain<IPAddressContext>(this.memoryScanner.ModuleStartAddress, finalPointerOffset: 0x1A4, 0x00629204, 0x18, 0x44);

        if (!IPAddress.TryParse($"{ipAddressContext.Byte1}.{ipAddressContext.Byte2}.{ipAddressContext.Byte3}.{ipAddressContext.Byte4}", out var ipAddress))
        {
            return default;
        }

        return new ConnectionData { IPAddress = ipAddress };
    }

    private PreGameData? ReadPreGameData()
    {
        var preGameContextPtr = this.GetPreGameContextPointer();
        if (preGameContextPtr == 0)
        {
            return default;
        }

        var preGameContext = this.memoryScanner.ReadPtrChain<PreGameContext>(preGameContextPtr, 0x0, 0x0, 0x0);
        if (preGameContext.LoginCharacters.Capacity > 20 ||
            preGameContext.LoginCharacters.Size > 20 ||
            !preGameContext.LoginCharacters.Buffer.IsValid())
        {
            return default;
        }

        // Detect corrupt memory by checking that player names are longer than 3 characters
        var loginCharacters = this.memoryScanner.ReadArray(preGameContext.LoginCharacters);
        if (loginCharacters.Any(c => c.CharacterName != "" && c.CharacterName.Length < 2))
        {
            return default;
        }

        return new PreGameData
        {
            ChosenCharacterIndex = preGameContext.LoginSelectionIndex > 0 && preGameContext.LoginSelectionIndex < loginCharacters.Length ?
                (int)preGameContext.LoginSelectionIndex :
                -1,
            Characters = loginCharacters.Select(c => c.CharacterName).ToList()
        };
    }

    private uint GetPlayerIdPointer()
    {
        if (this.playerIdPointer == 0)
        {
            this.playerIdPointer = this.memoryScanner.ScanForPtr(new byte[] { 0x5D, 0xE9, 0x00, 0x00, 0x00, 0x00, 0x55, 0x8B, 0xEC, 0x53 }, "xx????xxxx") - 0xE;
        }

        return this.playerIdPointer;
    }

    private uint GetEntityArrayPointer()
    {
        if (this.entityArrayPointer == 0)
        {
            this.entityArrayPointer = this.memoryScanner.ScanForPtr(new byte[] { 0xFF, 0x50, 0x10, 0x47, 0x83, 0xC6, 0x04, 0x3B, 0xFB, 0x75, 0xE1 }, "xxxxxxxxxxx") + 0xD;
        }

        return this.entityArrayPointer;
    }

    private uint GetTargetIdPointer()
    {
        if (this.targetIdPointer == 0)
        {
            this.targetIdPointer = this.memoryScanner.ScanForPtr(new byte[] { 0x3B, 0xDF, 0x0F, 0x95 }, "xxxx") + 0xB;
        }

        return this.targetIdPointer;
    }

    private uint GetInstanceInfoPointer()
    {
        if (this.instanceInfoPointer == 0)
        {
            this.instanceInfoPointer = this.memoryScanner.ScanForPtr(new byte[] { 0x6A, 0x2C, 0x50, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x83, 0xC4, 0x08, 0xC7 }, "xxxx????xxxx") + 0xD;
        }

        return this.instanceInfoPointer;
    }

    private uint GetPreGameContextPointer()
    {
        if (this.preGameContextPointer == 0)
        {
            this.preGameContextPointer = this.memoryScanner.ScanForAssertion("p:\\code\\gw\\ui\\uipregame.cpp", "!s_scene") + 0x34;
        }

        return this.preGameContextPointer;
    }

    private Bag? GetBag(GuildwarsPointer<BagInfo> bagInfoPtr, uint expectedBagType, bool returnEmptyBag)
    {
        var bagInfo = this.memoryScanner.Read(bagInfoPtr);
        if (!bagInfo.Items.Buffer.IsValid() ||
            bagInfo.Items.Size > 100 ||
            bagInfo.Type != expectedBagType)
        {
            return default;
        }

        var itemInfos = this.memoryScanner.ReadArray(bagInfo.Items);
        var items = new List<IBagContent>();
        for(var i = 0; i < itemInfos.Length; i++)
        {
            var itemInfo = itemInfos[i];
            if (itemInfo.Quantity == 0 ||
                itemInfo.Quantity > 250 ||
                itemInfo.ModifierArrayAddress == 0 ||
                itemInfo.ModifierCount > 200 ||
                itemInfo.ModelId == 0)
            {
                continue;
            }

            if (itemInfo.ContainingBagAddress != bagInfoPtr.Address)
            {
                continue;
            }

            var modifiers = this.memoryScanner.ReadArray<Daybreak.Models.Interop.ItemModifier>(itemInfo.ModifierArrayAddress, itemInfo.ModifierCount);
            var parsedModifiers = modifiers.Select(modifier => new Daybreak.Models.Guildwars.ItemModifier { Modifier = modifier.Modifier }).ToList();
            if (ItemBase.TryParse((int)itemInfo.ModelId, parsedModifiers, out var item) &&
                item is not ItemBase.Unknown)
            {
                items.Add(new BagItem
                {
                    Item = item ?? throw new InvalidOperationException($"Unable to create {nameof(BagItem)}. Expected item returned null"),
                    Slot = itemInfo.Slot,
                    Count = itemInfo.Quantity,
                    Modifiers = parsedModifiers
                });
            }
            else
            {
                items.Add(new UnknownBagItem
                {
                    ItemId = itemInfo.ModelId,
                    Slot = itemInfo.Slot,
                    Count = itemInfo.Quantity,
                    Modifiers = parsedModifiers
                });
            }
        }

        if (items.Count == 0 &&
            !returnEmptyBag)
        {
            return default;
        }

        return new Bag
        {
            Items = items,
            Capacity = (int)Math.Max(bagInfo.ItemsCount, itemInfos.Length)
        };
    }

    private GlobalContext GetGlobalContext()
    {
        /*
         * The following offsets were reverse-engineered using pointer scanning. All of the following ones seem to currently work.
         * If any breaks, try the other ones from the list below.
         * startAddress + 00629204 -> +0x18
         * startAddress + 006296B4 -> +0x18
         * startAddress + 00AA9CE0 -> +0xC -> +0xC
         */
        return this.memoryScanner.ReadPtrChain<GlobalContext>(this.memoryScanner.ModuleStartAddress, finalPointerOffset: 0x0, 0x00629204, 0x18);
    }

    private unsafe GameData AggregateGameData(
        GameContext gameContext,
        InstanceContext instanceContext,
        MapEntityContext[] mapEntities,
        EntityContext[] entities,
        PlayerContext[] players,
        ProfessionsContext[] professions,
        QuestContext[] quests,
        SkillbarContext[] skills,
        PartyAttributesContext[] partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers,
        MapIconContext[] mapIcons,
        NpcContext[] npcs,
        int mainPlayerEntityId,
        int targetEntityId)
    {
        var partyMembers = professions
            .Where(p => p.AgentId != mainPlayerEntityId)
            .Select(p => GetPlayerInformation(
                (int)p.AgentId, 
                instanceContext,
                mapEntities.Skip((int)p.AgentId).FirstOrDefault(),
                professions.FirstOrDefault(prof => prof.AgentId == p.AgentId),
                skills.FirstOrDefault(s => s.AgentId == p.AgentId),
                partyAttributes.OfType<PartyAttributesContext>().FirstOrDefault(attr => attr.AgentId == p.AgentId),
                entities.OfType<EntityContext>().FirstOrDefault(e => e.AgentId == p.AgentId)))
            .ToList();

        var parsedMapIcons = GetMapIcons(mapIcons);

        var mainPlayer = this.GetMainPlayerInformation(
            gameContext,
            instanceContext,
            players.FirstOrDefault(p => p.AgentId == mainPlayerEntityId),
            mapEntities.Skip(mainPlayerEntityId).FirstOrDefault(),
            professions.FirstOrDefault(p => p.AgentId == mainPlayerEntityId),
            quests,
            skills.OfType<SkillbarContext>().FirstOrDefault(s => s.AgentId == mainPlayerEntityId),
            partyAttributes.OfType<PartyAttributesContext>().FirstOrDefault(p => p.AgentId == mainPlayerEntityId),
            titles,
            titleTiers,
            entities.FirstOrDefault(e => e.AgentId == mainPlayerEntityId));

        var worldPlayers = players
            .Where(p => p.AgentId != mainPlayerEntityId && p.AgentId != 0)
            .Select(p => 
                this.GetWorldPlayerInformation(
                    p,
                    instanceContext,
                    mapEntities.Skip(p.AgentId).First(),
                    professions.FirstOrDefault(prof => prof.AgentId == p.AgentId),
                    skills.OfType<SkillbarContext>().FirstOrDefault(s => s.AgentId == p.AgentId),
                    partyAttributes.OfType<PartyAttributesContext>().FirstOrDefault(attr => attr.AgentId == p.AgentId),
                    titles,
                    titleTiers,
                    entities.FirstOrDefault(e => e.AgentId == p.AgentId)))
            .ToList();

        var remainingEntities = entities
            .Where(e => e.EntityType == EntityType.Living)
            .Where(e => e.AgentId != mainPlayer.Id && partyMembers.None(p => p.Id == e.AgentId) && worldPlayers.None(p => p.Id == e.AgentId))
            .ToArray();

        var livingEntities = this.GetLivingEntities(remainingEntities, npcs);

        return new GameData
        {
            Party = partyMembers,
            MainPlayer = mainPlayer,
            WorldPlayers = worldPlayers,
            LivingEntities = livingEntities,
            MapIcons = parsedMapIcons,
            CurrentTargetId = targetEntityId,
            Valid = mapEntities.Length > 0 || players.Length > 0 || professions.Length > 0 || partyAttributes.Length > 0 || entities.Length > 0
        };
    }

    private List<LivingEntity> GetLivingEntities(
        EntityContext[] livingEntities,
        NpcContext[] npcs)
    {
        var list = new List<LivingEntity>();
        foreach(var livingEntity in livingEntities)
        {
            _ = Profession.TryParse((int)livingEntity.PrimaryProfessionId, out var primaryProfession);
            _ = Profession.TryParse((int)livingEntity.SecondaryProfessionId, out var secondaryProfession);
            if (primaryProfession == Profession.None)
            {
                var maybeNpcContext = npcs.Skip(livingEntity.EntityModelType).FirstOrDefault();
                if (Profession.TryParse((int)maybeNpcContext.Primary, out var actualPrimaryProfession))
                {
                    primaryProfession = actualPrimaryProfession;
                }
            }

            var state = LivingEntityState.Unknown;
            switch (livingEntity.State)
            {
                case EntityState.Player:
                    state = LivingEntityState.Player;
                    break;
                case EntityState.Spirit:
                    state = LivingEntityState.Spirit;
                    break;
                case EntityState.Boss:
                    state = LivingEntityState.Boss;
                    break;
                case EntityState.Dead:
                    state = LivingEntityState.Dead;
                    break;
                case EntityState.ToBeCleanedUp:
                    state = LivingEntityState.ToBeCleanedUp;
                    break;
            }

            var allegiance = LivingEntityAllegiance.Unknown;
            switch (livingEntity.Allegiance)
            {
                case EntityAllegiance.AllyNonAttackable:
                    allegiance = LivingEntityAllegiance.AllyNonAttackable;
                    break;
                case EntityAllegiance.Neutral:
                    allegiance = LivingEntityAllegiance.Neutral;
                    break;
                case EntityAllegiance.Enemy:
                    allegiance = LivingEntityAllegiance.Enemy;
                    break;
                case EntityAllegiance.SpiritOrPet:
                    allegiance = LivingEntityAllegiance.SpiritOrPet;
                    break;
                case EntityAllegiance.Minion:
                    allegiance = LivingEntityAllegiance.Minion;
                    break;
                case EntityAllegiance.NpcOrMinipet:
                    allegiance = LivingEntityAllegiance.NpcOrMinipet;
                    break;
            }

            if (!Npc.TryParse(livingEntity.EntityModelType, out var npc))
            {
                npc = Npc.Unknown;
            }

            list.Add(new LivingEntity
            {
                Id = (int)livingEntity.AgentId,
                Timer = livingEntity.Timer,
                Level = (int)livingEntity.Level,
                NpcDefinition = npc,
                ModelType = livingEntity.EntityModelType,
                Position = new Position { X = livingEntity.Position.X, Y = livingEntity.Position.Y },
                PrimaryProfession = primaryProfession,
                SecondaryProfession = secondaryProfession,
                State = state,
                Allegiance = allegiance
            });
        }

        return list;
    }

    private MainPlayerInformation GetMainPlayerInformation(
        GameContext gameContext,
        InstanceContext instanceContext,
        PlayerContext playerContext,
        MapEntityContext mapEntity,
        ProfessionsContext professions,
        QuestContext[] quests,
        SkillbarContext skillbar,
        PartyAttributesContext partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers,
        EntityContext entity)
    {
        var playerInformation = this.GetWorldPlayerInformation(playerContext, instanceContext, mapEntity, professions, skillbar, partyAttributes, titles, titleTiers, entity);
        _ = Quest.TryParse((int)gameContext.QuestId, out var quest);
        var questLog = quests
            .Select(q =>
            {
                _ = Quest.TryParse((int)q.QuestId, out var parsedQuest);
                _ = Map.TryParse((int)q.MapFrom, out var mapFrom);
                _ = Map.TryParse((int)q.MapTo, out var mapTo);
                return new QuestMetadata
                {
                    Quest = parsedQuest,
                    From = mapFrom,
                    To = mapTo,
                    Position = float.IsFinite(q.Marker.X) && float.IsFinite(q.Marker.Y) ?
                        new Position { X = q.Marker.X, Y = q.Marker.Y } :
                        default
                };
            })
            .Where(q => q.Quest is not null)
            .ToList();

        return new MainPlayerInformation
        {
            Id = playerInformation.Id,
            Timer = playerInformation.Timer,
            Position = playerInformation.Position,
            PrimaryProfession = playerInformation.PrimaryProfession,
            SecondaryProfession = playerInformation.SecondaryProfession,
            UnlockedProfession = playerInformation.UnlockedProfession,
            HardModeUnlocked = gameContext.HardModeUnlocked == 1,
            CurrentEnergy = playerInformation.MaxEnergy * entity.CurrentEnergyPercentage,
            CurrentHealth = playerInformation.MaxHealth * entity.CurrentHealthPercentage,
            MaxEnergy = playerInformation.MaxEnergy,
            MaxHealth = playerInformation.MaxHealth,
            EnergyRegen = playerInformation.EnergyRegen,
            HealthRegen = playerInformation.HealthRegen,
            Quest = quest,
            QuestLog = questLog,
            Name = playerInformation.Name,
            Experience = gameContext.Experience,
            Level = (int)gameContext.Level,
            Morale = gameContext.Morale,
            CurrentBuild = playerInformation.CurrentBuild,
            TitleInformation = playerInformation.TitleInformation
        };
    }

    private WorldPlayerInformation GetWorldPlayerInformation(
        PlayerContext playerContext,
        InstanceContext instanceContext,
        MapEntityContext mapEntity,
        ProfessionsContext professions,
        SkillbarContext skillbar,
        PartyAttributesContext partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers,
        EntityContext entity)
    {
        var name = this.memoryScanner.Read(playerContext.NamePointer);
        var playerInformation = GetPlayerInformation(playerContext.AgentId, instanceContext, mapEntity, professions, skillbar, partyAttributes, entity);
        var maybeCurrentTitleContext = (TitleContext?)null;
        var maybeCurrentTitle = (Title?)null;
        for(var i = 0; i < titles.Length; i++)
        {
            var title = titles[i];
            if (title.CurrentTitleTierIndex == playerContext.ActiveTitleTier)
            {
                maybeCurrentTitleContext = title;
                _ = Title.TryParse(i, out maybeCurrentTitle);
                break;
            }
        }

        var maybeTitleTier = (TitleTierContext?)null;
        if (maybeCurrentTitleContext is TitleContext currentTitle)
        {
            maybeTitleTier = titleTiers[currentTitle.CurrentTitleTierIndex];
        }

        var titleInformation = (TitleInformation?)null;
        if (maybeCurrentTitleContext is not null && maybeTitleTier is not null)
        {
            titleInformation = new TitleInformation
            {
                CurrentPoints = maybeCurrentTitleContext?.CurrentPoints,
                IsPercentage = maybeCurrentTitleContext?.IsPercentage,
                PointsForCurrentRank = maybeCurrentTitleContext?.PointsNeededForCurrentRank,
                PointsForNextRank = maybeTitleTier?.TierNumber == maybeCurrentTitleContext?.MaxTitleRank ?
                    maybeCurrentTitleContext?.CurrentPoints :
                    maybeCurrentTitleContext?.PointsNeededForNextRank,
                TierNumber = maybeTitleTier?.TierNumber,
                MaxTierNumber = maybeCurrentTitleContext?.MaxTitleRank,
                Title = maybeCurrentTitle
            };
        }

        return new WorldPlayerInformation
        {
            Id = playerInformation.Id,
            Timer = playerInformation.Timer,
            Level = playerInformation.Level,
            Position = playerInformation.Position,
            PrimaryProfession = playerInformation.PrimaryProfession,
            SecondaryProfession = playerInformation.SecondaryProfession,
            UnlockedProfession = playerInformation.UnlockedProfession,
            CurrentEnergy = playerInformation.CurrentEnergy,
            CurrentHealth = playerInformation.CurrentHealth,
            MaxEnergy = playerInformation.MaxEnergy,
            MaxHealth = playerInformation.MaxHealth,
            EnergyRegen = playerInformation.EnergyRegen,
            HealthRegen = playerInformation.HealthRegen,
            Name = name,
            CurrentBuild = playerInformation.CurrentBuild,
            TitleInformation = titleInformation,
        };
    }

    private static PlayerInformation GetPlayerInformation(
        int playerId,
        InstanceContext instanceContext,
        MapEntityContext mapEntity,
        ProfessionsContext professionContext,
        SkillbarContext? skillbar,
        PartyAttributesContext? partyAttributes,
        EntityContext entity)
    {
        _ = Profession.TryParse((int)professionContext.CurrentPrimary, out var primaryProfession);
        _ = Profession.TryParse((int)professionContext.CurrentSecondary, out var secondaryProfession);
        var unlockedProfessions = Profession.Professions
            .Where(p => professionContext.ProfessionUnlocked(p.Id))
            .Append(primaryProfession)
            .Where(p => p is not null && p != Profession.None)
            .OrderBy(p => p.Id)
            .ToList();
        Build? build = null;
        if (skillbar is SkillbarContext skillbarContext &&
            partyAttributes is PartyAttributesContext attributesContext &&
            partyAttributes.Value.Attributes is not null &&
            primaryProfession is not null &&
            secondaryProfession is not null)
        {
            var attributes = (primaryProfession.PrimaryAttribute is null ?
                new List<Daybreak.Models.Guildwars.Attribute>() :
                new List<Daybreak.Models.Guildwars.Attribute> { primaryProfession.PrimaryAttribute! })
                .Concat(primaryProfession.Attributes)
                .Concat(secondaryProfession.Attributes)
                .Select(a => new AttributeEntry { Attribute = a })
                .ToList();
            foreach(var attribute in attributesContext.Attributes)
            {
                if (attribute.Id < 0 || attribute.Id > 44)
                {
                    continue;
                }

                var maybeAttributeEntry = attributes.FirstOrDefault(a => a.Attribute!.Id == attribute.Id);
                if (maybeAttributeEntry is not AttributeEntry attributeEntry)
                {
                    continue;
                }

                attributeEntry.Points = (int)attribute.BaseLevel;
            }

            var skillContexts = new SkillContext[]
            {
                skillbarContext.Skill0,
                skillbarContext.Skill1,
                skillbarContext.Skill2,
                skillbarContext.Skill3,
                skillbarContext.Skill4,
                skillbarContext.Skill5,
                skillbarContext.Skill6,
                skillbarContext.Skill7,
            };

            build = new Build
            {
                Primary = primaryProfession,
                Secondary = secondaryProfession,
                Attributes = attributes,
                Skills = skillContexts.Select(s =>
                        Skill.TryParse((int)s.Id, out var parsedSkill) ?
                        parsedSkill :
                        Skill.NoSkill).ToList()
            };
        }
        
        (var currentHp, var currentEnergy) = ApplyEnergyAndHealthRegen(instanceContext, mapEntity);
        if (!Npc.TryParse(entity.EntityModelType, out var npc))
        {
            npc = Npc.Unknown;
        }

        return new PlayerInformation
        {
            Id = playerId,
            Timer = entity.Timer,
            Level = entity.Level,
            NpcDefinition = npc,
            ModelType = entity.EntityModelType,
            PrimaryProfession = primaryProfession,
            SecondaryProfession = secondaryProfession,
            UnlockedProfession = unlockedProfessions,
            CurrentHealth = currentHp,
            CurrentEnergy = currentEnergy,
            MaxHealth = mapEntity.MaxHealth,
            MaxEnergy = mapEntity.MaxEnergy,
            HealthRegen = mapEntity.HealthRegen,
            EnergyRegen = mapEntity.EnergyRegen,
            CurrentBuild = build,
            Position = new Position
            {
                X = entity.Position.X,
                Y = entity.Position.Y
            }
        };
    }

    private static List<MapIcon> GetMapIcons(MapIconContext[] mapIconContexts)
    {
        var retList = new List<MapIcon>();
        foreach(var mapIconContext in mapIconContexts)
        {
            var affiliation = mapIconContext.Affiliation switch
            {
                TeamColor.Gray => Affiliation.Gray,
                TeamColor.GrayNeutral => Affiliation.GrayNeutral,
                TeamColor.Teal => Affiliation.Teal,
                TeamColor.Yellow => Affiliation.Yellow,
                TeamColor.Purple => Affiliation.Purple,
                TeamColor.Blue => Affiliation.Blue,
                TeamColor.Red => Affiliation.Red,
                TeamColor.Green => Affiliation.Green,
                _ => throw new InvalidOperationException($"Unknown affiliation {mapIconContext.Affiliation}")
            };

            if (GuildwarsIcon.TryParse((int)mapIconContext.Id, out var icon))
            {
                retList.Add(new MapIcon
                {
                    Icon = icon,
                    Position = new Position { X = mapIconContext.X, Y = mapIconContext.Y },
                    Affiliation = affiliation
                });
            }
        }

        return retList;
    }

    private static (float CurrentHp, float CurrentEnergy) ApplyEnergyAndHealthRegen(InstanceContext instanceContext, MapEntityContext entityContext)
    {
        var lastKnownHp = entityContext.CurrentHealth;
        var lastKnownEnergy = entityContext.CurrentEnergy;
        var hpRegen = entityContext.HealthRegen;
        var energyRegen = entityContext.EnergyRegen;
        var millisSinceLastKnownInformation = instanceContext.Timer - (uint)entityContext.SkillTimestamp;
        var currentHp = lastKnownHp + (hpRegen * ((float)millisSinceLastKnownInformation / 1000));
        var currentEnergy = lastKnownEnergy + (energyRegen * ((float)millisSinceLastKnownInformation / 1000));
        return (
            currentHp > entityContext.MaxHealth ? entityContext.MaxHealth : currentHp,
            currentEnergy > entityContext.MaxEnergy ? entityContext.MaxEnergy : currentEnergy);
    }

    private static string ParseAndCleanWCharArray(byte[] bytes)
    {
        var str = Encoding.Unicode.GetString(bytes);
        var indexOfNull = str.IndexOf('\0');
        if (indexOfNull > 0)
        {
            str = str[..indexOfNull];
        }

        return str;
    }

    private static List<List<int>> BuildPathingMaps(List<Trapezoid> trapezoids, List<List<int>> adjacencyArray)
    {
        var visited = new bool[trapezoids.Count];
        var pathingMaps = new List<List<int>>();
        foreach (var trapezoid in trapezoids)
        {
            if (visited[trapezoid.Id] is false)
            {
                var currentPathingMap = new List<int>();
                pathingMaps.Add(currentPathingMap);
                var queue = new Queue<int>();
                queue.Enqueue(trapezoid.Id);
                while (queue.TryDequeue(out var currentId))
                {
                    if (visited[currentId] is true)
                    {
                        continue;
                    }

                    visited[currentId] = true;
                    currentPathingMap.Add(currentId);
                    foreach (var adjacentId in adjacencyArray[currentId])
                    {
                        queue.Enqueue(adjacentId);
                    }
                }
            }
        }

        return pathingMaps;
    }

    private static List<List<int>> BuildFinalAdjacencyList(List<Trapezoid> trapezoids, List<List<int>> computedPathingMaps, List<List<int>> originalAdjacencyList)
    {
        var adjacencyList = new List<List<int>>();
        for(var i = 0; i < trapezoids.Count; i++)
        {
            adjacencyList.Add(originalAdjacencyList[i].ToList());
        }

        for(var i = 0; i < computedPathingMaps.Count; i++)
        {
            var currentPathingMap = computedPathingMaps[i];
            Parallel.For(i + 1, computedPathingMaps.Count - 1, j =>
            {
                var otherPathingMap = computedPathingMaps[j];
                foreach (var currentTrapezoidId in currentPathingMap)
                {
                    var currentPoints = MathUtils.GetTrapezoidPoints(trapezoids[currentTrapezoidId]);
                    foreach (var otherTrapezoidId in otherPathingMap)
                    {
                        var otherPoints = MathUtils.GetTrapezoidPoints(trapezoids[otherTrapezoidId]);
                        if (TrapezoidsAdjacent(currentPoints, otherPoints))
                        {
                            adjacencyList[currentTrapezoidId].Add(otherTrapezoidId);
                            adjacencyList[otherTrapezoidId].Add(currentTrapezoidId);
                        }
                    }
                }
            });
        }

        return adjacencyList;
    }

    private static bool TrapezoidsAdjacent(Point[] currentPoints, Point[] otherPoints)
    {
        var curBoundingRectangle = GetBoundingRectangle(currentPoints);
        var otherBoundingRectangle = GetBoundingRectangle(otherPoints);
        if (!curBoundingRectangle.IntersectsWith(otherBoundingRectangle))
        {
            return false;
        }

        for (var x = 0; x < currentPoints.Length; x++)
        {
            for (var y = 0; y < otherPoints.Length; y++)
            {
                if (MathUtils.LineSegmentsIntersect(currentPoints[x], currentPoints[(x + 1) % currentPoints.Length],
                    otherPoints[y], otherPoints[(y + 1) % otherPoints.Length], out _, epsilon: 0.1))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static Rect GetBoundingRectangle(Point[] points)
    {
        var minX = double.MaxValue;
        var maxX = double.MinValue;
        var minY = double.MaxValue;
        var maxY = double.MinValue;
        for (var i = 0; i < points.Length; i++)
        {
            var curPoint = points[i];
            if (curPoint.X < minX) minX = curPoint.X;
            if (curPoint.X > maxX) maxX = curPoint.X;
            if (curPoint.Y < minY) minY = curPoint.Y;
            if (curPoint.Y > maxY) maxY = curPoint.Y;
        }

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    private static unsafe string ParseAndCleanWCharPointer(byte* bytes, int byteCount)
    {
        var str = Encoding.Unicode.GetString(bytes, byteCount);
        var indexOfNull = str.IndexOf('\0');
        if (indexOfNull > 0)
        {
            str = str[..indexOfNull];
        }

        return str;
    }

    // https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
