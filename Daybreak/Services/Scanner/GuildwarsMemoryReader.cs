using Daybreak.Models;
using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Interop;
using Daybreak.Models.Metrics;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Metrics;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Linq;
using System.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Scanner;

public sealed class GuildwarsMemoryReader : IGuildwarsMemoryReader
{
    private const int MaxTrapezoidCount = 1000000;
    private const int RetryInitializationCount = 15;
    private const string LatencyMeterName = "Memory Reader Latency";
    private const string LatencyMeterUnitsName = "Milliseconds";
    private const string LatencyMeterDescription = "Amount of milliseconds elapsed while reading memory. P95 aggregation";

    private readonly IApplicationLauncher applicationLauncher;
    private readonly IMemoryScanner memoryScanner;
    private readonly Histogram<double> latencyMeter;
    private readonly ILogger<GuildwarsMemoryReader> logger;

    private uint playerIdPointer;
    private uint entityArrayPointer;
    private uint titleDataPointer;
    private uint targetIdPointer;

    public GuildwarsMemoryReader(
        IApplicationLauncher applicationLauncher,
        IMemoryScanner memoryScanner,
        IMetricsService metricsService,
        ILogger<GuildwarsMemoryReader> logger)
    {
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.memoryScanner = memoryScanner.ThrowIfNull();
        this.latencyMeter = metricsService.ThrowIfNull().CreateHistogram<double>(LatencyMeterName, LatencyMeterUnitsName, LatencyMeterDescription, AggregationTypes.P95);
        this.logger = logger.ThrowIfNull();
    }
    
    public async Task EnsureInitialized(CancellationToken cancellationToken)
    {
        var scoppedLogger = this.logger.CreateScopedLogger(nameof(this.EnsureInitialized), default);
        var currentGuildwarsProcess = this.applicationLauncher.RunningGuildwarsProcess;
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
        var scoppedLogger = this.logger.CreateScopedLogger(nameof(this.Stop), default);
        scoppedLogger.LogInformation($"Stopping {nameof(GuildwarsMemoryReader)}");
        this.memoryScanner?.EndScanner();
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

    private async Task InitializeSafe(Process process, ScopedLogger<GuildwarsMemoryReader> scopedLogger)
    {
        scopedLogger.LogInformation($"Initializing {nameof(GuildwarsMemoryReader)}");
        if (this.memoryScanner.Process is null ||
            this.memoryScanner.Process.HasExited ||
            this.memoryScanner.Process.MainModule?.FileName != process?.MainModule?.FileName)
        {
            if (this.memoryScanner.Scanning)
            {
                scopedLogger.LogInformation("Scanner is already scanning a different process. Restart scanner and target the new process");
                this.memoryScanner.EndScanner();
            }
            
            await this.ResilientBeginScanner(scopedLogger, process!);
        }

        if (this.memoryScanner.Scanning is false &&
            process?.HasExited is false)
        {
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

    private GameData? ReadGameDataInternal()
    {
        /*
         * The following offsets were reverse-engineered using pointer scanning. All of the following ones seem to currently work.
         * If any breaks, try the other ones from the list below.
         * startAddress + 0061C118 -> +0x0 -> +0x18
         * startAddress + 00629244 -> +0xC -> +0xC
         * startAddress + 0062971C -> +0xC -> +0xC
         * startAddress + 00AA9D58 -> +0xC -> +0xC
         * startAddress + 00AA9D58 -> +0x0 -> +0xC -> +0xC
         */
        var globalContext = this.memoryScanner.ReadPtrChain<GlobalContext>(this.memoryScanner.ModuleStartAddress, finalPointerOffset: 0x0, 0x00629244, 0xC, 0xC);

        if (globalContext.GameContext == 0x0 ||
            globalContext.UserContext == 0x0 ||
            globalContext.InstanceContext == 0x0)
        {
            return new GameData { Valid = false };
        }

        // GameContext struct is offset by 0x07C due to the memory layout of the structure.
        var gameContext = this.memoryScanner.Read<GameContext>(globalContext.GameContext + GameContext.BaseOffset);
        // UserContext struct is offset by 0x074 due to the memory layout of the structure.
        var userContext = this.memoryScanner.Read<UserContext>(globalContext.UserContext + UserContext.BaseOffset);
        // InstanceContext struct is offset by 0x01AC due to memory layout of the structure.
        var instanceContext = this.memoryScanner.Read<InstanceContext>(globalContext.InstanceContext + InstanceContext.BaseOffset);
        
        if (gameContext.MapEntities.Size > 10000 ||
            gameContext.Professions.Size > 10000 ||
            gameContext.Players.Size > 1000 ||
            gameContext.QuestLog.Size > 10000 ||
            gameContext.Skillbars.Size > 100 ||
            gameContext.PartyAttributes.Size > 1000 ||
            gameContext.Titles.Size > 1000 ||
            gameContext.TitlesTiers.Size > 10000)
        {
            return new GameData { Valid = false };
        }

        var mapEntities = this.memoryScanner.ReadArray<MapEntityContext>(gameContext.MapEntities);
        var mapIcons = this.memoryScanner.ReadArray<MapIconContext>(gameContext.MissionMapIcons);
        var professions = this.memoryScanner.ReadArray<ProfessionsContext>(gameContext.Professions);
        var players = this.memoryScanner.ReadArray<PlayerContext>(gameContext.Players);
        var quests = this.memoryScanner.ReadArray<QuestContext>(gameContext.QuestLog);
        var skills = this.memoryScanner.ReadArray<SkillbarContext>(gameContext.Skillbars);
        var partyAttributes = this.memoryScanner.ReadArray<PartyAttributesContext>(gameContext.PartyAttributes);
        var playerEntityId = this.memoryScanner.ReadPtrChain<int>(this.GetPlayerIdPointer(), 0x0, 0x0);
        var targetEntityId = this.memoryScanner.ReadPtrChain<int>(this.GetTargetIdPointer(), 0x0, 0x0);
        var titles = this.memoryScanner.ReadArray<TitleContext>(gameContext.Titles);
        var titleTiers = this.memoryScanner.ReadArray<TitleTierContext>(gameContext.TitlesTiers);

        // The following lines would retrieve all entities, including item entities.
        var entityPointersArray = this.memoryScanner.ReadPtrChain<GuildwarsArray>(this.GetEntityArrayPointer(), 0x0, 0x0);
        if (entityPointersArray.Size > 10000)
        {
            return new GameData { Valid = false };
        }

        var entityPointers = this.memoryScanner.ReadArray<uint>(entityPointersArray);
        var entities = entityPointers.Select(ptr => this.memoryScanner.Read<EntityContext>(ptr + EntityContext.EntityContextBaseOffset)).ToArray();

        return this.AggregateGameData(
            gameContext,
            instanceContext,
            mapEntities,
            entities,
            players,
            professions,
            quests,
            userContext,
            skills,
            partyAttributes,
            titles,
            titleTiers,
            mapIcons,
            playerEntityId,
            targetEntityId);
    }

    private PathingData? ReadPathingDataInternal()
    {
        var globalContext = this.memoryScanner.ReadPtrChain<GlobalContext>(this.memoryScanner.ModuleStartAddress, finalPointerOffset: 0x0, 0x00629244, 0xC, 0xC);
        var mapContext = this.memoryScanner.Read<MapContext>(globalContext.MapContext);

        var pathingMapContext = this.memoryScanner.ReadPtrChain<PathingMapContext>(mapContext.PathingMapContextPtr, 0x0, 0x0);
        if (pathingMapContext.PathingMapArray.Size > 10000)
        {
            return default;
        }

        var pathingMaps = this.memoryScanner.ReadArray<PathingMap>(pathingMapContext.PathingMapArray);
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
        var globalContext = this.memoryScanner.ReadPtrChain<GlobalContext>(this.memoryScanner.ModuleStartAddress, finalPointerOffset: 0x0, 0x00629244, 0xC, 0xC);
        var mapContext = this.memoryScanner.Read<MapContext>(globalContext.MapContext);

        var pathingMapContext = this.memoryScanner.ReadPtrChain<PathingMapContext>(mapContext.PathingMapContextPtr, 0x0, 0x0);
        var pathingMaps = this.memoryScanner.ReadArray<PathingMap>(pathingMapContext.PathingMapArray);

        return new PathingMetadata { TrapezoidCount = (int)pathingMaps.Select(p => p.TrapezoidCount).Sum(count => count) };
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

    private uint GetTitleDataPointer()
    {
        if (this.titleDataPointer == 0)
        {
            this.titleDataPointer = this.memoryScanner.ScanForAssertion("p:\\code\\gw\\const\\consttitle.cpp", "index < arrsize(s_titleClientData)") + 0x12;
        }

        return this.titleDataPointer;
    }

    private uint GetTargetIdPointer()
    {
        if (this.targetIdPointer == 0)
        {
            this.targetIdPointer = this.memoryScanner.ScanForPtr(new byte[] { 0x3B, 0xDF, 0x0F, 0x95 }, "xxxx") + 0xB;
        }

        return this.targetIdPointer;
    }

    private unsafe GameData AggregateGameData(
        GameContext gameContext,
        InstanceContext instanceContext,
        MapEntityContext[] mapEntities,
        EntityContext[] entities,
        PlayerContext[] players,
        ProfessionsContext[] professions,
        QuestContext[] quests,
        UserContext userContext,
        SkillbarContext[] skills,
        PartyAttributesContext[] partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers,
        MapIconContext[] mapIcons,
        int mainPlayerEntityId,
        int targetEntityId)
    {
        var name = userContext.PlayerName;
        _ = Map.TryParse((int)userContext.MapId, out var currentMap);
        var partyMembers = professions
            .Where(p => p.AgentId != mainPlayerEntityId)
            .Select(p => GetPlayerInformation((int)p.AgentId, instanceContext, mapEntities, professions, skills, partyAttributes, entities))
            .ToList();

        var parsedMapIcons = GetMapIcons(mapIcons);

        var mainPlayer = this.GetMainPlayerInformation(
            gameContext,
            instanceContext,
            players.FirstOrDefault(p => p.AgentId == mainPlayerEntityId),
            mapEntities,
            professions,
            quests,
            skills,
            partyAttributes,
            titles,
            titleTiers,
            entities);

        var worldPlayers = players
            .Where(p => p.AgentId != mainPlayerEntityId && p.AgentId != 0)
            .Select(p => this.GetWorldPlayerInformation(p, instanceContext, mapEntities, professions, skills, partyAttributes, titles, titleTiers, entities))
            .ToList();

        var remainingEntities = entities
            .Where(e => e.EntityType == EntityType.Living)
            .Where(e => e.AgentId != mainPlayer.Id && partyMembers.None(p => p.Id == e.AgentId) && worldPlayers.None(p => p.Id == e.AgentId))
            .ToArray();

        var livingEntities = this.GetLivingEntities(remainingEntities);
        
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

        var sessionInformation = new SessionInformation
        {
            FoesKilled = gameContext.FoesKilled,
            FoesToKill = gameContext.FoesToKill,
            CurrentMap = currentMap,
            InstanceTimer = instanceContext.Timer,
            CurrentTargetId = targetEntityId
        };

        return new GameData
        {
            Party = partyMembers,
            MainPlayer = mainPlayer,
            Session = sessionInformation,
            User = userInformation,
            WorldPlayers = worldPlayers,
            LivingEntities = livingEntities,
            MapIcons = parsedMapIcons,
            Valid = mapEntities.Length > 0 || players.Length > 0 || professions.Length > 0 || partyAttributes.Length > 0 || entities.Length > 0
        };
    }

    private List<LivingEntity> GetLivingEntities(
        EntityContext[] livingEntities)
    {
        var list = new List<LivingEntity>();
        foreach(var livingEntity in livingEntities)
        {
            _ = Profession.TryParse((int)livingEntity.PrimaryProfessionId, out var primaryProfession);
            _ = Profession.TryParse((int)livingEntity.SecondaryProfessionId, out var secondaryProfession);
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
        MapEntityContext[] mapEntities,
        ProfessionsContext[] professions,
        QuestContext[] quests,
        SkillbarContext[] skillbars,
        PartyAttributesContext[] partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers,
        EntityContext[] entities)
    {
        var playerInformation = this.GetWorldPlayerInformation(playerContext, instanceContext, mapEntities, professions, skillbars, partyAttributes, titles, titleTiers, entities);
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
            CurrentEnergy = playerInformation.CurrentEnergy,
            CurrentHealth = playerInformation.CurrentHealth,
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
        MapEntityContext[] mapEntities,
        ProfessionsContext[] professions,
        SkillbarContext[] skillbars,
        PartyAttributesContext[] partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers,
        EntityContext[] entities)
    {
        var name = this.memoryScanner.ReadWString(playerContext.NamePointer, 0x40);
        var playerInformation = GetPlayerInformation(playerContext.AgentId, instanceContext, mapEntities, professions, skillbars, partyAttributes, entities);
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
        MapEntityContext[] mapEntities,
        ProfessionsContext[] professions,
        SkillbarContext[] skillbars,
        PartyAttributesContext[] partyAttributes,
        EntityContext[] entities)
    {
        var mapEntityContext = mapEntities.Skip(playerId).FirstOrDefault();
        var professionContext = professions.Where(p => p.AgentId == playerId).FirstOrDefault();
        _ = Profession.TryParse((int)professionContext.CurrentPrimary, out var primaryProfession);
        _ = Profession.TryParse((int)professionContext.CurrentSecondary, out var secondaryProfession);
        var unlockedProfessions = Profession.Professions
            .Where(p => professionContext.ProfessionUnlocked(p.Id))
            .Append(primaryProfession)
            .Where(p => p is not null && p != Profession.None)
            .OrderBy(p => p.Id)
            .ToList();
        var maybeSkillbarContext = skillbars.Select(s => (SkillbarContext?) s).FirstOrDefault(s => s?.AgentId == playerId);
        var maybePartyAttributesContext = partyAttributes.Select(p => (PartyAttributesContext?) p).FirstOrDefault(p => p?.AgentId == playerId);
        Build? build = null;
        if (maybeSkillbarContext is SkillbarContext skillbarContext &&
            maybePartyAttributesContext is PartyAttributesContext attributesContext &&
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
        
        (var currentHp, var currentEnergy) = ApplyEnergyAndHealthRegen(instanceContext, mapEntityContext);
        EntityContext? entityContext = entities.FirstOrDefault(e => e.AgentId == playerId);
        if (!Npc.TryParse(entityContext?.EntityModelType ?? 0, out var npc))
        {
            npc = Npc.Unknown;
        }

        return new PlayerInformation
        {
            Id = playerId,
            Timer = entityContext!.Value.Timer,
            Level = entityContext.Value.Level,
            NpcDefinition = npc,
            ModelType = entityContext.Value.EntityModelType,
            PrimaryProfession = primaryProfession,
            SecondaryProfession = secondaryProfession,
            UnlockedProfession = unlockedProfessions,
            CurrentHealth = currentHp,
            CurrentEnergy = currentEnergy,
            MaxHealth = mapEntityContext.MaxHealth,
            MaxEnergy = mapEntityContext.MaxEnergy,
            HealthRegen = mapEntityContext.HealthRegen,
            EnergyRegen = mapEntityContext.EnergyRegen,
            CurrentBuild = build,
            Position = entityContext is not null ?
                new Position
                {
                    X = entityContext.Value.Position.X,
                    Y = entityContext.Value.Position.Y
                } :
                default
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
}
