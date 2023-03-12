using Daybreak.Configuration;
using Daybreak.Models;
using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Interop;
using Daybreak.Models.Metrics;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Metrics;
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
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public sealed class GuildwarsMemoryReader : IGuildwarsMemoryReader
{
    private const int RetryInitializationCount = 15;
    private const string LatencyMeterName = "Memory Reader Latency";
    private const string LatencyMeterUnitsName = "Milliseconds";
    private const string LatencyMeterDescription = "Amount of milliseconds elapsed while reading memory. P95 aggregation";

    private readonly IApplicationLauncher applicationLauncher;
    private readonly IMemoryScanner memoryScanner;
    private readonly Histogram<double> latencyMeter;
    private readonly ILiveOptions<ApplicationConfiguration> liveOptions;
    private readonly ILogger<GuildwarsMemoryReader> logger;

    private IntPtr playerIdPointer;
    private IntPtr entityArrayPointer;
    private IntPtr titleDataPointer;

    public GuildwarsMemoryReader(
        IApplicationLauncher applicationLauncher,
        IMemoryScanner memoryScanner,
        IMetricsService metricsService,
        ILiveOptions<ApplicationConfiguration> liveOptions,
        ILogger<GuildwarsMemoryReader> logger)
    {
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.memoryScanner = memoryScanner.ThrowIfNull();
        this.latencyMeter = metricsService.ThrowIfNull().CreateHistogram<double>(LatencyMeterName, LatencyMeterUnitsName, LatencyMeterDescription, AggregationTypes.P95);
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }
    
    public async Task EnsureInitialized()
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

    public Task<GameData?> ReadGameData()
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<GameData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadGameDataInternal));
    }

    public Task<PathingData?> ReadPathingData()
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<PathingData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadPathingDataInternal));
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

        if (!this.memoryScanner.Scanning)
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

        if (globalContext.GameContext.ToInt32() == 0x0 ||
            globalContext.UserContext.ToInt32() == 0x0 ||
            globalContext.InstanceContext.ToInt32() == 0x0)
        {
            return new GameData { Valid = false };
        }

        // GameContext struct is offset by 0x07C due to the memory layout of the structure.
        var gameContext = this.memoryScanner.Read<GameContext>(globalContext.GameContext + GameContext.BaseOffset);
        // UserContext struct is offset by 0x074 due to the memory layout of the structure.
        var userContext = this.memoryScanner.Read<UserContext>(globalContext.UserContext + UserContext.BaseOffset);
        // InstanceContext struct is offset by 0x01AC due to memory layout of the structure.
        var instanceContext = this.memoryScanner.Read<InstanceContext>(globalContext.InstanceContext + InstanceContext.BaseOffset);
        

        var mapEntities = this.memoryScanner.ReadArray<MapEntityContext>(gameContext.MapEntities);
        var professions = this.memoryScanner.ReadArray<ProfessionsContext>(gameContext.Professions);
        var players = this.memoryScanner.ReadArray<PlayerContext>(gameContext.Players);
        var quests = this.memoryScanner.ReadArray<QuestContext>(gameContext.QuestLog);
        var skills = this.memoryScanner.ReadArray<SkillbarContext>(gameContext.Skillbars);
        var partyAttributes = this.memoryScanner.ReadArray<PartyAttributesContext>(gameContext.PartyAttributes);
        var playerEntityId = this.memoryScanner.ReadPtrChain<int>(this.GetPlayerIdPointer(), 0x0, 0x0);
        var titles = this.memoryScanner.ReadArray<TitleContext>(gameContext.Titles);
        var titleTiers = this.memoryScanner.ReadArray<TitleTierContext>(gameContext.TitlesTiers);

        // The following lines would retrieve all entities, including item entities.
        var entityPointersArray = this.memoryScanner.ReadPtrChain<GuildwarsArray>(this.GetEntityArrayPointer(), 0x0, 0x0);
        var entityPointers = this.memoryScanner.ReadArray<IntPtr>(entityPointersArray);
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
            playerEntityId);
    }

    private PathingData ReadPathingDataInternal()
    {
        var globalContext = this.memoryScanner.ReadPtrChain<GlobalContext>(this.memoryScanner.ModuleStartAddress, finalPointerOffset: 0x0, 0x00629244, 0xC, 0xC);
        var mapContext = this.memoryScanner.Read<MapContext>(globalContext.MapContext);

        var pathingMapContext = this.memoryScanner.ReadPtrChain<PathingMapContext>(mapContext.PathingMapContextPtr, 0x0, 0x0);
        var pathingMaps = this.memoryScanner.ReadArray<PathingMap>(pathingMapContext.PathingMapArray);
        var pathingTrapezoidMapping = pathingMaps
            .Select(pathingMap => (pathingMap, this.memoryScanner.ReadArray<PathingTrapezoid>(pathingMap.TrapezoidArray, (int)pathingMap.TrapezoidCount)))
            .Select((mapping) => (mapping.pathingMap, mapping.Item2, mapping.Item2.Select(trapezoid => trapezoid.AdjacentPathingTrapezoids.Select(adjacentPtr => this.memoryScanner.Read<PathingTrapezoid>(adjacentPtr)).ToArray()).ToArray()))
            .ToArray();

        var trapezoidList = new List<Trapezoid>();
        foreach(var mapping in pathingTrapezoidMapping)
        {
            var (_, pathingTrapezoids, adjacentTrapezoids) = mapping;
            for (var i = 0; i < pathingTrapezoids.Length; i++)
            {
                var pathingTrapezoid = pathingTrapezoids[i];
                var adjacent = adjacentTrapezoids[i];
                trapezoidList.Add(new Trapezoid
                {
                    Id = pathingTrapezoid.Id,
                    XTL = pathingTrapezoid.XTL,
                    XTR = pathingTrapezoid.XTR,
                    YT = pathingTrapezoid.YT,
                    XBL = pathingTrapezoid.XBL,
                    XBR = pathingTrapezoid.XBR,
                    YB = pathingTrapezoid.YB,
                    AdjacentIds = adjacent.Select(a => a.Id).ToArray()
                });
            }
        }

        return new PathingData { Trapezoids = trapezoidList };
    }

    private IntPtr GetPlayerIdPointer()
    {
        if (this.playerIdPointer == IntPtr.Zero)
        {
            this.playerIdPointer = this.memoryScanner.ScanForPtr(new byte[] { 0x5D, 0xE9, 0x00, 0x00, 0x00, 0x00, 0x55, 0x8B, 0xEC, 0x53 }, "xx????xxxx") - 0xE;
        }

        return this.playerIdPointer;
    }

    private IntPtr GetEntityArrayPointer()
    {
        if (this.entityArrayPointer == IntPtr.Zero)
        {
            this.entityArrayPointer = this.memoryScanner.ScanForPtr(new byte[] { 0xFF, 0x50, 0x10, 0x47, 0x83, 0xC6, 0x04, 0x3B, 0xFB, 0x75, 0xE1 }, "xxxxxxxxxxx") + 0xD;
        }

        return this.entityArrayPointer;
    }

    private IntPtr GetTitleDataPointer()
    {
        if (this.titleDataPointer == IntPtr.Zero)
        {
            this.titleDataPointer = this.memoryScanner.ScanForAssertion("p:\\code\\gw\\const\\consttitle.cpp", "index < arrsize(s_titleClientData)") + 0x12;
        }

        return this.titleDataPointer;
    }
    
    private GameData AggregateGameData(
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
        int mainPlayerEntityId)
    {
        var email = ParseAndCleanWCharArray(userContext.PlayerEmailBytes);
        var name = ParseAndCleanWCharArray(userContext.PlayerNameBytes);
        _ = Map.TryParse((int)userContext.MapId, out var currentMap);
        var partyMembers = professions
            .Where(p => p.AgentId != mainPlayerEntityId)
            .Select(p => GetPlayerInformation((int)p.AgentId, instanceContext, mapEntities, professions, skills, partyAttributes, entities))
            .ToList();

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
            .Where(p => p.AgentId != mainPlayerEntityId)
            .Select(p => this.GetWorldPlayerInformation(p, instanceContext, mapEntities, professions, skills, partyAttributes, titles, titleTiers, entities))
            .ToList();

        var remainingEntities = entities
            .Where(e => e.EntityType == EntityType.Living)
            .Where(e => e.AgentId != mainPlayer.Id && partyMembers.None(p => p.Id == e.AgentId) && worldPlayers.None(p => p.Id == e.AgentId))
            .ToArray();

        var livingEntities = this.GetLivingEntities(remainingEntities);
        
        var userInformation = new UserInformation
        {
            Email = email,
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
            CurrentMap = currentMap
        };

        return new GameData
        {
            Party = partyMembers,
            MainPlayer = mainPlayer,
            Session = sessionInformation,
            User = userInformation,
            WorldPlayers = worldPlayers,
            LivingEntities = livingEntities,
            Valid = mapEntities.Length > 0 || players.Length > 0 || professions.Length > 0 || quests.Length > 0 || skills.Length > 0 || partyAttributes.Length > 0 || entities.Length > 0
        };
    }
    
    private List<LivingEntity> GetLivingEntities(EntityContext[] livingEntities)
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

            list.Add(new LivingEntity
            {
                Id = (int)livingEntity.AgentId,
                Level = (int)livingEntity.Level,
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
                return new QuestMetadata { Quest = parsedQuest, From = mapFrom, To = mapTo };
            })
            .Where(q => q?.Quest is not null)
            .ToList();

        return new MainPlayerInformation
        {
            Id = playerInformation.Id,
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
                new List<Models.Guildwars.Attribute>() :
                new List<Models.Guildwars.Attribute> { primaryProfession.PrimaryAttribute! })
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

            build = new Build
            {
                Primary = primaryProfession,
                Secondary = secondaryProfession,
                Attributes = attributes,
                Skills = skillbarContext.Skills.Select(s =>
                        Skill.TryParse((int)s.Id, out var parsedSkill) ?
                        parsedSkill :
                        Skill.NoSkill).ToList()
            };
        }
        
        (var currentHp, var currentEnergy) = ApplyEnergyAndHealthRegen(instanceContext, mapEntityContext);
        EntityContext? entityContext = entities.FirstOrDefault(e => e.AgentId == playerId);

        return new PlayerInformation
        {
            Id = playerId,
            Level = entityContext.Value.Level,
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
}
