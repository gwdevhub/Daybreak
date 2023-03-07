using Daybreak.Configuration;
using Daybreak.Models;
using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Interop;
using Daybreak.Models.Metrics;
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
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Scanner;

public sealed class GuildwarsMemoryReader : IGuildwarsMemoryReader, IDisposable
{
    private const int RetryInitializationCount = 15;
    private const string LatencyMeterName = "Memory Reader Latency";
    private const string LatencyMeterUnitsName = "Milliseconds";
    private const string LatencyMeterDescription = "Amount of milliseconds elapsed while reading memory. P95 aggregation";

    private readonly IMemoryScanner memoryScanner;
    private readonly Histogram<double> latencyMeter;
    private readonly ILiveOptions<ApplicationConfiguration> liveOptions;
    private readonly ILogger<GuildwarsMemoryReader> logger;

    private IntPtr playerIdPointer;
    private IntPtr entityArrayPointer;
    private IntPtr titleDataPointer;
    private volatile CancellationTokenSource? cancellationTokenSource;

    public bool Faulty { get; private set; }

    public bool Running => this.cancellationTokenSource is not null && this.cancellationTokenSource.IsCancellationRequested is false;

    public GameData? GameData { get; private set; }

    public Process? TargetProcess => this.memoryScanner.Process;

    public GuildwarsMemoryReader(
        IMemoryScanner memoryScanner,
        IMetricsService metricsService,
        ILiveOptions<ApplicationConfiguration> liveOptions,
        ILogger<GuildwarsMemoryReader> logger)
    {
        this.memoryScanner = memoryScanner.ThrowIfNull();
        this.latencyMeter = metricsService.ThrowIfNull().CreateHistogram<double>(LatencyMeterName, LatencyMeterUnitsName, LatencyMeterDescription, AggregationTypes.P95);
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }
    
    public async void Initialize(Process process)
    {
        var scoppedLogger = this.logger.CreateScopedLogger(nameof(this.Initialize), default);
        if (process is null)
        {
            scoppedLogger.LogWarning($"Process is null. {nameof(GuildwarsMemoryReader)} will not start");
            return;
        }

        try
        {
            await this.InitializeSafe(process, scoppedLogger);
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
        this.cancellationTokenSource?.Cancel();
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

        this.cancellationTokenSource?.Cancel();
        this.PeriodicallyReadGuildwarsMemory();
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

    private void PeriodicallyReadGuildwarsMemory()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        this.cancellationTokenSource = cancellationTokenSource;
        _ = Task.Run(() => this.RecursivelyReadGameMemory(this.cancellationTokenSource.Token), this.cancellationTokenSource.Token);
    }
    
    private async Task RecursivelyReadGameMemory(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        this.SafeReadGameMemory(cancellationToken);
        await Task.Delay((int)this.liveOptions.Value.ExperimentalFeatures.MemoryReaderFrequency);
        _ = Task.Run(() => this.RecursivelyReadGameMemory(cancellationToken), cancellationToken);
    }

    private void SafeReadGameMemory(CancellationToken cancellationToken)
    {
        try
        {
            var stopWatch = Stopwatch.StartNew();
            this.ReadGameMemory(cancellationToken);
            stopWatch.Stop();
            this.latencyMeter.Record(stopWatch.Elapsed.TotalMilliseconds);
            this.Faulty = false;
        }
        catch(Exception e)
        {
            this.GameData = null;
            this.Faulty = true;
            this.logger.LogError(e, "Exception encountered when reading game memory");
        }
    }

    private void ReadGameMemory(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
        
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
            this.GameData = new GameData { Valid = false };
            return;
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
        
        
        // The following lines would retrieve all titles infos.
        //var titleDataPtr = this.memoryScanner.Read<IntPtr>(this.GetTitleDataPointer());
        //var titleInfos = this.memoryScanner.ReadArray<TitleInfo>(titleDataPtr, 48);
        
        
        // The following lines would retrieve all entities, including item entities.
        //var entityArray = this.memoryScanner.ReadPtrChain<GuildwarsArray>(this.GetEntityArrayPointer(), 0x0, 0x0);
        //var entities = this.memoryScanner.ReadArray<EntityContext>(entityArray);

        this.GameData = this.AggregateGameData(
            gameContext,
            instanceContext,
            mapEntities,
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
        MapEntityContext[] entities,
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
            .Select(p => GetPlayerInformation((int)p.AgentId, instanceContext, entities, professions, skills, partyAttributes))
            .ToList();

        var mainPlayer = this.GetMainPlayerInformation(
            gameContext,
            instanceContext,
            players.FirstOrDefault(p => p.AgentId == mainPlayerEntityId),
            entities,
            professions,
            quests,
            skills,
            partyAttributes,
            titles,
            titleTiers);

        var worldPlayers = players
            .Where(p => p.AgentId != mainPlayerEntityId)
            .Select(p => this.GetWorldPlayerInformation(p, instanceContext, entities, professions, skills, partyAttributes, titles, titleTiers))
            .ToList();

        
        
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
            Valid = entities.Length > 0 || players.Length > 0 || professions.Length > 0 || quests.Length > 0 || skills.Length > 0 || partyAttributes.Length > 0
        };
    }
    
    private MainPlayerInformation GetMainPlayerInformation(
        GameContext gameContext,
        InstanceContext instanceContext,
        PlayerContext playerContext,
        MapEntityContext[] entities,
        ProfessionsContext[] professions,
        QuestContext[] quests,
        SkillbarContext[] skillbars,
        PartyAttributesContext[] partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers)
    {
        var playerInformation = this.GetWorldPlayerInformation(playerContext, instanceContext, entities, professions, skillbars, partyAttributes, titles, titleTiers);
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
            Level = gameContext.Level,
            Morale = gameContext.Morale,
            CurrentBuild = playerInformation.CurrentBuild,
            TitleInformation = playerInformation.TitleInformation
        };
    }

    private WorldPlayerInformation GetWorldPlayerInformation(
        PlayerContext playerContext,
        InstanceContext instanceContext,
        MapEntityContext[] entities,
        ProfessionsContext[] professions,
        SkillbarContext[] skillbars,
        PartyAttributesContext[] partyAttributes,
        TitleContext[] titles,
        TitleTierContext[] titleTiers)
    {
        var name = this.memoryScanner.ReadWString(playerContext.NamePointer, 0x40);
        var playerInformation = GetPlayerInformation(playerContext.AgentId, instanceContext, entities, professions, skillbars, partyAttributes);
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
            TitleInformation = titleInformation
        };
    }

    private static PlayerInformation GetPlayerInformation(
        int playerId,
        InstanceContext instanceContext,
        MapEntityContext[] entities,
        ProfessionsContext[] professions,
        SkillbarContext[] skillbars,
        PartyAttributesContext[] partyAttributes)
    {
        var entityContext = entities.Skip(playerId).FirstOrDefault();
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
            var attributes = new List<Models.Guildwars.Attribute> { primaryProfession.PrimaryAttribute! }
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
                Skills = skillbarContext.Skills.Select(s => Skill.Parse((int)s.Id)).ToList()
            };
        }
        
        (var currentHp, var currentEnergy) = ApplyEnergyAndHealthRegen(instanceContext, entityContext);
        return new PlayerInformation
        {
            PrimaryProfession = primaryProfession,
            SecondaryProfession = secondaryProfession,
            UnlockedProfession = unlockedProfessions,
            CurrentHealth = currentHp,
            CurrentEnergy = currentEnergy,
            MaxHealth = entityContext.MaxHealth,
            MaxEnergy = entityContext.MaxEnergy,
            HealthRegen = entityContext.HealthRegen,
            EnergyRegen = entityContext.EnergyRegen,
            CurrentBuild = build
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

    public void Dispose()
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = null;
        this.Faulty = false;
    }
}
