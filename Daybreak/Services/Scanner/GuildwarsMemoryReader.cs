﻿using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Interop;
using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Services.Metrics;
using Daybreak.Shared.Services.Scanner;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Extensions.Core;
using System.Linq;
using System.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public sealed class GuildwarsMemoryReader(
    IMemoryScanner memoryScanner,
    IMetricsService metricsService,
    ILogger<GuildwarsMemoryReader> logger) : IGuildwarsMemoryReader
{
    private const int RetryInitializationCount = 5;
    private const string LatencyMeterName = "Memory Reader Latency";
    private const string LatencyMeterUnitsName = "Milliseconds";
    private const string LatencyMeterDescription = "Amount of milliseconds elapsed while reading memory. P95 aggregation";

    private readonly IMemoryScanner memoryScanner = memoryScanner.ThrowIfNull();
    private readonly Histogram<double> latencyMeter = metricsService.ThrowIfNull().CreateHistogram<double>(LatencyMeterName, LatencyMeterUnitsName, LatencyMeterDescription, AggregationTypes.P95);
    private readonly ILogger<GuildwarsMemoryReader> logger = logger.ThrowIfNull();

    private uint globalContextPointer;
    private uint instanceInfoPointer;
    private uint entityArrayPointer;

    public async Task EnsureInitialized(uint processId, CancellationToken cancellationToken)
    {
        var scoppedLogger = this.logger.CreateScopedLogger();
        var currentGuildwarsProcess = Process.GetProcessById((int)processId);
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
        catch (Exception e)
        {
            scoppedLogger.LogError(e, "Encountered exception during initialization");
        }
    }

    public void Stop()
    {
        var scoppedLogger = this.logger.CreateScopedLogger();
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

    public Task<WorldData?> ReadWorldData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<WorldData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadWorldDataInternal), cancellationToken);
    }

    public Task<bool> IsInitialized(uint processId, CancellationToken cancellationToken)
    {
        return Task.FromResult(this.memoryScanner.Scanning && processId == this.memoryScanner.Process?.Id);
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

    public Task<TeamBuildData?> ReadTeamBuildData(CancellationToken cancellationToken)
    {
        if (this.memoryScanner.Scanning is false)
        {
            return Task.FromResult<TeamBuildData?>(default);
        }

        return Task.Run(() => this.SafeReadGameMemory(this.ReadTeamBuildDataInternal), cancellationToken);
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
                this.globalContextPointer = 0x0;
                this.instanceInfoPointer = 0x0;
                this.entityArrayPointer = 0x0;
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
            if (this.memoryScanner.Scanning && process.Id == this.memoryScanner.Process?.Id)
            {
                scopedLogger.LogInformation("Scanner already initialized");
                return;
            }

            if (this.memoryScanner.Scanning)
            {
                this.memoryScanner.EndScanner();
                this.globalContextPointer = 0x0;
                this.instanceInfoPointer = 0x0;
                this.entityArrayPointer = 0x0;
            }

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
        catch (Exception e)
        {
            this.logger.LogError(e, "Exception encountered when reading game memory");
            return default;
        }
    }

    private UserData? ReadUserDataInternal()
    {
        var maybeGlobalContext = this.GetGlobalContext();
        if (!maybeGlobalContext.HasValue)
        {
            return default;
        }

        var globalContext = maybeGlobalContext.Value;
        var gameContext = this.memoryScanner.Read(globalContext.GameContext, GameContext.BaseOffset);
        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        return new UserData
        {
            User = new UserInformation
            {
                Email = userContext.PlayerEmailFirstChar + userContext.PlayerEmailSecondChar + userContext.PlayerEmailRemaining,
                CurrentBalthazarPoints = gameContext.CurrentBalthazar,
                CurrentImperialPoints = gameContext.CurrentImperial,
                CurrentKurzickPoints = gameContext.CurrentKurzick,
                CurrentLuxonPoints = gameContext.CurrentLuxon,
                CurrentSkillPoints = gameContext.CurrentSkillPoints,
                MaxBalthazarPoints = gameContext.MaxBalthazar,
                MaxImperialPoints = gameContext.MaxImperial,
                MaxKurzickPoints = gameContext.MaxKurzick,
                MaxLuxonPoints = gameContext.MaxLuxon,
                TotalBalthazarPoints = gameContext.TotalBalthazar,
                TotalImperialPoints = gameContext.TotalImperial,
                TotalKurzickPoints = gameContext.TotalKurzick,
                TotalLuxonPoints = gameContext.TotalLuxon,
                TotalSkillPoints = gameContext.TotalSkillPoints
            }
        };
    }

    private LoginData? ReadLoginDataInternal()
    {
        var maybeGlobalContext = this.GetGlobalContext();
        if (!maybeGlobalContext.HasValue)
        {
            return default;
        }

        var globalContext = maybeGlobalContext.Value;
        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        return new LoginData
        {
            Email = userContext.PlayerEmailFirstChar + (userContext.PlayerEmailSecondChar + userContext.PlayerEmailRemaining),
            PlayerName = userContext.PlayerName
        };
    }

    private WorldData? ReadWorldDataInternal()
    {
        var maybeGlobalContext = this.GetGlobalContext();
        if (!maybeGlobalContext.HasValue)
        {
            return default;
        }

        var globalContext = maybeGlobalContext.Value;
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

    private SessionData? ReadSessionDataInternal()
    {
        var maybeGlobalContext = this.GetGlobalContext();
        if (!maybeGlobalContext.HasValue)
        {
            return default;
        }

        var maybeInstanceInfoContext = this.GetInstanceInfoContext();
        if (!maybeInstanceInfoContext.HasValue)
        {
            return default;
        }

        var globalContext = maybeGlobalContext.Value;
        var instanceInfoContext = maybeInstanceInfoContext.Value;
        var gameContext = this.memoryScanner.Read(globalContext.GameContext, GameContext.BaseOffset);
        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        var instanceContext = this.memoryScanner.Read(globalContext.InstanceContext, InstanceContext.BaseOffset);
        _ = Map.TryParse((int)userContext.MapId, out var currentMap);
        return new SessionData
        {
            Session = new SessionInformation
            {
                CurrentMap = currentMap,
                FoesKilled = gameContext.FoesKilled,
                FoesToKill = gameContext.FoesToKill,
                InstanceTimer = instanceContext.Timer,
                InstanceType = instanceInfoContext.InstanceType switch
                {
                    Shared.Models.Interop.InstanceType.Explorable => Shared.Models.Guildwars.InstanceType.Explorable,
                    Shared.Models.Interop.InstanceType.Loading => Shared.Models.Guildwars.InstanceType.Loading,
                    Shared.Models.Interop.InstanceType.Outpost => Shared.Models.Guildwars.InstanceType.Outpost,
                    _ => Shared.Models.Guildwars.InstanceType.Undefined
                }
            }
        };
    }

    private MainPlayerData? ReadMainPlayerDataInternal()
    {
        var maybeGlobalContext = this.GetGlobalContext();
        if (!maybeGlobalContext.HasValue)
        {
            return default;
        }

        var maybeEntityArray = this.GetEntityArray();
        if (!maybeEntityArray.HasValue)
        {
            return default;
        }

        var globalContext = maybeGlobalContext.Value;
        var entityArray = maybeEntityArray.Value;
        var gameContext = this.memoryScanner.Read(globalContext.GameContext, GameContext.BaseOffset);
        var instanceContext = this.memoryScanner.Read(globalContext.InstanceContext, InstanceContext.BaseOffset);
        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        var playerControlledCharContext = this.memoryScanner.Read(gameContext.PlayerControlledChar, PlayerControlledCharContext.BaseOffset);
        var playerContexts = this.memoryScanner.ReadArray(gameContext.Players);
        var mapEntityContexts = this.memoryScanner.ReadArray(gameContext.MapEntities);
        var professionContexts = this.memoryScanner.ReadArray(gameContext.Professions);
        var questContexts = this.memoryScanner.ReadArray(gameContext.QuestLog);
        var skillBarContexts = this.memoryScanner.ReadArray(gameContext.Skillbars);
        var partyAttributesContexts = this.memoryScanner.ReadArray(gameContext.PartyAttributes);
        var titleContexts = this.memoryScanner.ReadArray(gameContext.Titles);
        var titleTiersContexts = this.memoryScanner.ReadArray(gameContext.TitlesTiers);
        var entityContextPtrs = this.memoryScanner.ReadArray<uint>(entityArray.Buffer, entityArray.Size);
        var entityContexts = entityContextPtrs.Select(this.memoryScanner.Read<EntityContext>).ToArray();

        var playerId = playerControlledCharContext.AgentId;
        var mapEntity = mapEntityContexts.Skip((int)playerId).FirstOrDefault();
        var playerContext = playerContexts.FirstOrDefault(p => p.AgentId == playerId);
        var professionContext = professionContexts.FirstOrDefault(p => p.AgentId == playerId);
        var skillbarContext = skillBarContexts.FirstOrDefault(p => p.AgentId == playerId);
        var partyAttributesContext = partyAttributesContexts.FirstOrDefault(p => p.AgentId == playerId);
        var entityContext = entityContexts.FirstOrDefault(p => p.AgentId == playerId);
        var mainPlayerInformation = this.GetMainPlayerInformation(
            gameContext,
            instanceContext,
            playerContext,
            mapEntity,
            professionContext,
            questContexts,
            skillbarContext,
            partyAttributesContext,
            titleContexts,
            titleTiersContexts,
            entityContext);
        return new MainPlayerData
        {
            PlayerInformation = mainPlayerInformation
        };
    }

    private TeamBuildData? ReadTeamBuildDataInternal()
    {
        var maybeGlobalContext = this.GetGlobalContext();
        if (!maybeGlobalContext.HasValue)
        {
            return default;
        }

        var maybeEntityArray = this.GetEntityArray();
        if (!maybeEntityArray.HasValue)
        {
            return default;
        }

        var globalContext = maybeGlobalContext.Value;
        var entityArray = maybeEntityArray.Value;
        var gameContext = this.memoryScanner.Read(globalContext.GameContext, GameContext.BaseOffset);
        var instanceContext = this.memoryScanner.Read(globalContext.InstanceContext, InstanceContext.BaseOffset);
        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        var playerControlledCharContext = this.memoryScanner.Read(gameContext.PlayerControlledChar, PlayerControlledCharContext.BaseOffset);
        var partyContext = this.memoryScanner.Read(globalContext.PartyContext);
        var playerPartyContext = this.memoryScanner.Read(partyContext.PlayerParty);
        var partyPlayers = this.memoryScanner.ReadArray(playerPartyContext.Players);
        var partyHeroes = this.memoryScanner.ReadArray(playerPartyContext.Heroes);
        var partyHench = this.memoryScanner.ReadArray(playerPartyContext.Henchmen);

        var playerContexts = this.memoryScanner.ReadArray(gameContext.Players);
        var mapEntityContexts = this.memoryScanner.ReadArray(gameContext.MapEntities);
        var professionContexts = this.memoryScanner.ReadArray(gameContext.Professions);
        var questContexts = this.memoryScanner.ReadArray(gameContext.QuestLog);
        var skillBarContexts = this.memoryScanner.ReadArray(gameContext.Skillbars);
        var partyAttributesContexts = this.memoryScanner.ReadArray(gameContext.PartyAttributes);
        var titleContexts = this.memoryScanner.ReadArray(gameContext.Titles);
        var titleTiersContexts = this.memoryScanner.ReadArray(gameContext.TitlesTiers);
        var entityContextPtrs = this.memoryScanner.ReadArray<uint>(entityArray.Buffer, entityArray.Size);
        var entityContexts = entityContextPtrs.Select(this.memoryScanner.Read<EntityContext>).ToArray();

        var entityAggregateWithBuilds = professionContexts
            .Select(p => (p.AgentId, p, partyAttributesContexts.FirstOrDefault(pa => pa.AgentId == p.AgentId), skillBarContexts.FirstOrDefault(s => s.AgentId == p.AgentId)))
            .Select(t => (t.AgentId, GetEntityBuild(t.p, t.Item3, t.Item4)))
            .Where(t => t.Item2 is not null)
            .OfType<(uint AgentId, Build Build)>();


        return new TeamBuildData
        {
            TeamBuildPlayers = [.. entityAggregateWithBuilds.Select<(uint AgentId, Build Build), TeamBuildPlayerData>(t =>
            {
                var agentId = t.AgentId;
                var build = t.Build;
                if (agentId == playerControlledCharContext.AgentId)
                {
                    return new TeamBuildMainPlayerData { Build = build };
                }

                if (partyHeroes.FirstOrDefault(h => h.AgentId == agentId) is HeroPartyMember heroPartyMember &&
                    heroPartyMember.HeroId != 0 &&
                    Hero.TryParse((int)heroPartyMember.HeroId, out var hero))
                {

                    return new TeamBuildHeroData { Build = build, Hero = hero };
                }

                if (partyHench.FirstOrDefault(h => h.AgentId == agentId) is HenchmanPartyMember henchmanPartyMember &&
                    henchmanPartyMember.AgentId != 0)
                {
                    return new TeamBuildHenchmanData { Build = build };
                }

                return new TeamBuildPartyMemberData { Build = build };
            }).OrderBy(t => t is TeamBuildMainPlayerData ? 0 : 1)]
        };
    }

    private GlobalContext? GetGlobalContext()
    {
        var basePtr = this.GetGlobalContextPointer();
        var baseContextAddress = this.memoryScanner.Read<uint>(basePtr);
        var gameContextPtr = this.memoryScanner.Read<uint>(baseContextAddress + (6 * 4));
        var globalContext = this.memoryScanner.Read<GlobalContext>(gameContextPtr);
        return globalContext;
    }

    private InstanceInfoContext? GetInstanceInfoContext()
    {
        var ptr = this.GetInstanceInfoPointer();
        var instanceInfoPtr = this.memoryScanner.Read<uint>(ptr);
        var instanceInfo = this.memoryScanner.Read<InstanceInfoContext>(instanceInfoPtr);
        return instanceInfo;
    }

    private GenericGuildwarsArray? GetEntityArray()
    {
        var ptr = this.GetEntityArrayPointer();
        var entityArrayPtr = this.memoryScanner.Read<uint>(ptr);
        var entityArray = this.memoryScanner.Read<GenericGuildwarsArray>(entityArrayPtr);
        return entityArray;
    }

    private uint GetGlobalContextPointer()
    {
        if (this.globalContextPointer == 0)
        {
            var ptr = this.memoryScanner.ScanForPtr([0x50, 0x6A, 0x0F, 0x6A, 0x00, 0xFF, 0x35], "xxxxxxx") + 0x7;
            this.globalContextPointer = this.memoryScanner.Read<uint>(ptr);
        }

        return this.globalContextPointer;
    }

    private uint GetInstanceInfoPointer()
    {
        if (this.instanceInfoPointer == 0)
        {
            this.instanceInfoPointer = this.memoryScanner.ScanForPtr([0x6A, 0x2C, 0x50, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x83, 0xC4, 0x08, 0xC7], "xxxx????xxxx") + 0xD;
        }

        return this.instanceInfoPointer;
    }

    private uint GetEntityArrayPointer()
    {
        if (this.entityArrayPointer == 0)
        {
            this.entityArrayPointer = this.memoryScanner.ScanForPtr([0x8B, 0x0C, 0x90, 0x85, 0xC9, 0x74, 0x19], "xxxxxxx") - 0x4;
        }

        return this.entityArrayPointer;
    }

    private MainPlayerInformation GetMainPlayerInformation(
        GameContext gameContext,
        InstanceContext instanceContext,
        PlayerContext playerContext,
        MapEntityContext mapEntity,
        ProfessionsContext professionContext,
        QuestContext[] quests,
        SkillbarContext skillbarContext,
        PartyAttributesContext partyAttributesContext,
        TitleContext[] titles,
        TitleTierContext[] titleTiers,
        EntityContext entityContext)
    {
        var playerInformation = this.GetWorldPlayerInformation(playerContext, instanceContext, mapEntity, professionContext, skillbarContext, partyAttributesContext, titles, titleTiers, entityContext);
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
        MapEntityContext mapEntity,
        ProfessionsContext profession,
        SkillbarContext skillbar,
        PartyAttributesContext partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers,
        EntityContext entity)
    {
        var name = this.memoryScanner.Read(playerContext.NamePointer);
        var playerInformation = GetPlayerInformation(playerContext.AgentId, instanceContext, mapEntity, profession, skillbar, partyAttributes, entity);
        var maybeCurrentTitleContext = (TitleContext?)null;
        var maybeCurrentTitle = (Title?)null;
        for (var i = 0; i < titles.Length; i++)
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
        ProfessionsContext profession,
        SkillbarContext skillbar,
        PartyAttributesContext partyAttributes,
        EntityContext entity)
    {
        var build = GetEntityBuild(profession, partyAttributes, skillbar);
        var unlockedProfessions = Profession.Professions
            .Where(p => profession.ProfessionUnlocked(p.Id))
            .Append(build?.Primary)
            .OfType<Profession>()
            .Where(p => p != Profession.None)
            .OrderBy(p => p.Id)
            .ToList();
        

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
            PrimaryProfession = build?.Primary ?? Profession.None,
            SecondaryProfession = build?.Secondary ?? Profession.None,
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

    private static Build? GetEntityBuild(
        ProfessionsContext professionsContext,
        PartyAttributesContext partyAttributesContext,
        SkillbarContext skillbarContext)
    {
        _ = Profession.TryParse((int)professionsContext.CurrentPrimary, out var primaryProfession);
        _ = Profession.TryParse((int)professionsContext.CurrentSecondary, out var secondaryProfession);

        Build? build = default;
        if (primaryProfession is not null &&
            secondaryProfession is not null)
        {
            var attributes = (primaryProfession.PrimaryAttribute is null ?
                [] :
                new List<Shared.Models.Guildwars.Attribute> { primaryProfession.PrimaryAttribute! })
                .Concat(primaryProfession.Attributes)
                .Concat(secondaryProfession.Attributes)
                .Select(a => new AttributeEntry { Attribute = a })
                .ToList();
            foreach(var entry in attributes)
            {
                var attributesContext = partyAttributesContext.Attributes.FirstOrDefault(p => p.Id == entry.Attribute?.Id);
                if (attributesContext.IncrementPoints == 0 && attributesContext.DecrementPoints == 0)
                {
                    continue;
                }

                entry.Points = (int)attributesContext.BaseLevel;
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
                Skills = [.. skillContexts.Select(s =>
                        Skill.TryParse((int)s.Id, out var parsedSkill) ?
                        parsedSkill :
                        Skill.NoSkill)]
            };
        }

        return build;
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
}
