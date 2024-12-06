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
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.Extensions.Core;
using System.Linq;
using System.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.Scanner;

public sealed class GuildwarsMemoryReader(
    IApplicationLauncher applicationLauncher,
    IMemoryScanner memoryScanner,
    IMetricsService metricsService,
    ILogger<GuildwarsMemoryReader> logger) : IGuildwarsMemoryReader
{
    private const int MaxTrapezoidCount = 1000000;
    private const int RetryInitializationCount = 5;
    private const string LatencyMeterName = "Memory Reader Latency";
    private const string LatencyMeterUnitsName = "Milliseconds";
    private const string LatencyMeterDescription = "Amount of milliseconds elapsed while reading memory. P95 aggregation";

    private readonly IApplicationLauncher applicationLauncher = applicationLauncher.ThrowIfNull();
    private readonly IMemoryScanner memoryScanner = memoryScanner.ThrowIfNull();
    private readonly Histogram<double> latencyMeter = metricsService.ThrowIfNull().CreateHistogram<double>(LatencyMeterName, LatencyMeterUnitsName, LatencyMeterDescription, AggregationTypes.P95);
    private readonly ILogger<GuildwarsMemoryReader> logger = logger.ThrowIfNull();

    private uint globalContextPointer;
    private uint playerIdPointer;
    private uint entityArrayPointer;
    private uint titleDataPointer;
    private uint targetIdPointer;
    private uint instanceInfoPointer;

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
        throw new NotImplementedException();
    }

    public Task<SessionData?> ReadSessionData(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
        catch (Exception e)
        {
            this.logger.LogError(e, "Exception encountered when reading game memory");
            return default;
        }
    }

    private LoginData? ReadLoginDataInternal()
    {
        var globalContext = this.memoryScanner.ReadPtrChain<GlobalContext>(this.GetGlobalContextPointer(), 0x0);
        if (!globalContext.GameContext.IsValid() ||
            !globalContext.UserContext.IsValid() ||
            !globalContext.InstanceContext.IsValid())
        {
            return default;
        }

        var userContext = this.memoryScanner.Read(globalContext.UserContext, UserContext.BaseOffset);
        return new LoginData
        {
            Email = userContext.PlayerEmailFirstChar + (userContext.PlayerEmailSecondChar + userContext.PlayerEmailRemaining),
            PlayerName = userContext.PlayerName
        };
    }

    private WorldData? ReadWorldDataInternal()
    {
        var globalContext = this.memoryScanner.ReadPtrChain<GlobalContext>(this.memoryScanner.ModuleStartAddress, finalPointerOffset: 0x0, 0x00629204, 0x18);

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

    private uint GetGlobalContextPointer()
    {
        if (this.globalContextPointer == 0)
        {
            this.globalContextPointer = this.memoryScanner.ScanForPtr([0x50, 0x6A, 0x0F, 0x6A, 0x00, 0xFF, 0x35], "xxxxxxx") + 0x7;
        }

        return this.globalContextPointer;
    }

    private uint GetPlayerIdPointer()
    {
        if (this.playerIdPointer == 0)
        {
            this.playerIdPointer = this.memoryScanner.ScanForPtr([0x5D, 0xE9, 0x00, 0x00, 0x00, 0x00, 0x55, 0x8B, 0xEC, 0x53], "xx????xxxx") - 0xE;
        }

        return this.playerIdPointer;
    }

    private uint GetTitleDataPointer()
    {
        if (this.titleDataPointer == 0)
        {
            this.titleDataPointer = this.memoryScanner.ScanForAssertion("p:\\code\\gw\\const\\consttitle.cpp", "index < arrsize(s_titleClientData)") + 0x12;
        }

        return this.titleDataPointer;
    }

    private uint GetInstanceInfoPointer()
    {
        if (this.instanceInfoPointer == 0)
        {
            this.instanceInfoPointer = this.memoryScanner.ScanForPtr([0x6A, 0x2C, 0x50, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x83, 0xC4, 0x08, 0xC7], "xxxx????xxxx") + 0xD;
        }

        return this.instanceInfoPointer;
    }

    private List<LivingEntity> GetLivingEntities(
        EntityContext[] livingEntities,
        NpcContext[] npcs)
    {
        var list = new List<LivingEntity>();
        foreach (var livingEntity in livingEntities)
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
        var name = this.memoryScanner.Read(playerContext.NamePointer);
        var playerInformation = GetPlayerInformation(playerContext.AgentId, instanceContext, mapEntities, professions, skillbars, partyAttributes, entities);
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
        var maybeSkillbarContext = skillbars.Select(s => (SkillbarContext?)s).FirstOrDefault(s => s?.AgentId == playerId);
        var maybePartyAttributesContext = partyAttributes.Select(p => (PartyAttributesContext?)p).FirstOrDefault(p => p?.AgentId == playerId);
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
            foreach (var attribute in attributesContext.Attributes)
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
        foreach (var mapIconContext in mapIconContexts)
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
        for (var i = 0; i < trapezoids.Count; i++)
        {
            adjacencyList.Add(originalAdjacencyList[i].ToList());
        }

        for (var i = 0; i < computedPathingMaps.Count; i++)
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
