using Daybreak.Models.Guildwars;
using Daybreak.Models.GWCA;
using Daybreak.Services.GWCA;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Extensions;
using Daybreak.Services.Scanner.Models;
using System.Linq;
using System.Collections.Generic;
using Daybreak.Models.Builds;
using System.Windows;
using Daybreak.Utils;
using System.Text.RegularExpressions;
using Daybreak.Services.Pathfinding;

namespace Daybreak.Services.Scanner;

public sealed partial class GWCAMemoryReader : IGuildwarsMemoryReader
{
    private static readonly Regex ItemNameColorRegex = GenerateItemNameColorRegex();
    private readonly IPathfinder pathfinder;
    private readonly IGWCAClient client;
    private readonly ILogger<GWCAMemoryReader> logger;

    private bool faulty = false;

    private ConnectionContext? connectionContextCache;

    public GWCAMemoryReader(
        IPathfinder pathfinder,
        IGWCAClient gWCAClient,
        ILogger<GWCAMemoryReader> logger)
    {
        this.pathfinder = pathfinder.ThrowIfNull();
        this.client = gWCAClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task EnsureInitialized(uint processId, CancellationToken cancellationToken)
    {
        if (this.faulty is false &&
            this.connectionContextCache.HasValue &&
            this.connectionContextCache.Value.ProcessId == processId)
        {
            return;
        }

        var maybeConnectionContext = await this.client.Connect(processId, cancellationToken);
        if (maybeConnectionContext is not ConnectionContext)
        {
            throw new InvalidOperationException($"Unable to connect to desired process {processId}");
        }

        this.connectionContextCache = maybeConnectionContext;
    }

    public async Task<GameData?> ReadGameData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadGameData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "game", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var gameData = payload.Deserialize<GameDataPayload>();
            if (gameData is null)
            {
                return default;
            }

            return new GameData
            {
                Valid = true,
                CurrentTargetId = (int)gameData.TargetId,
                LivingEntities = gameData.LivingEntities?.Select(ParsePayload).ToList(),
                MainPlayer = gameData.MainPlayer is not null ? ParsePayload(gameData.MainPlayer) : new MainPlayerInformation(),
                Party = gameData.Party?.Select(ParsePayload).ToList() ?? [],
                WorldPlayers = gameData.WorldPlayers?.Select(ParsePayload).ToList() ?? [],
                MapIcons = gameData.MapIcons?.Select(m =>
                {
                    if (!GuildwarsIcon.TryParse((int)m.Id, out var icon))
                    {
                        return default;
                    }

                    return new MapIcon
                    {
                        Affiliation = icon.Affiliation,
                        Icon = icon,
                        Position = new Position
                        {
                            X = m.PosX,
                            Y = m.PosY
                        }
                    };
                }).OfType<MapIcon>().ToList() ?? []
            };
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<InventoryData?> ReadInventoryData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadInventoryData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "inventory", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var inventoryData = payload.Deserialize<InventoryPayload>();
            if (inventoryData is null)
            {
                return default;
            }

            return new InventoryData
            {
                Backpack = ParsePayload(inventoryData.Backpack!),
                BeltPouch = ParsePayload(inventoryData.BeltPouch!),
                EquipmentPack = ParsePayload(inventoryData.EquipmentPack!),
                EquippedItems = ParsePayload(inventoryData.EquippedItems!),
                MaterialStorage = ParsePayload(inventoryData.MaterialStorage!),
                UnclaimedItems = ParsePayload(inventoryData.UnclaimedItems!),
                Bags = inventoryData.Bags?.Select(ParsePayload)?.ToList()!,
                StoragePanes = inventoryData.StoragePanes?.Select(ParsePayload).ToList()!
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<LoginData?> ReadLoginData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadLoginData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "login", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var loginData = payload.Deserialize<LoginPayload>();
            if (loginData is null)
            {
                return default;
            }

            return new LoginData
            {
                Email = loginData.Email ?? string.Empty,
                PlayerName = loginData.PlayerName ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<MainPlayerData?> ReadMainPlayerData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadMainPlayerData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "game/mainplayer", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var playerData = payload.Deserialize<MainPlayerPayload>();
            if (playerData is null)
            {
                return default;
            }

            return new MainPlayerData
            {
                PlayerInformation = ParsePayload(playerData)
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<PathingData?> ReadPathingData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadPathingData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "pathing", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var pathingPayload = payload.Deserialize<PathingPayload>();
            if (pathingPayload is null)
            {
                return default;
            }

            var trapezoidList = pathingPayload.Trapezoids?.Select(pathingTrapezoid =>
            new Trapezoid
            {
                Id = (int)pathingTrapezoid.Id,
                PathingMapId = (int)pathingTrapezoid.PathingMapId,
                YT = pathingTrapezoid.YT,
                YB = pathingTrapezoid.YB,
                XBL = pathingTrapezoid.XBL,
                XBR = pathingTrapezoid.XBR,
                XTL = pathingTrapezoid.XTL,
                XTR = pathingTrapezoid.XTR
            }).ToList() ?? [];
            var adjacencyList = pathingPayload.AdjacencyList ?? [];
            var pathingMapsCount = pathingPayload.Trapezoids?.Max(p => p.PathingMapId) ?? 0;
            var originalPathingMaps = new List<List<int>>((int)pathingMapsCount);
            foreach(var trapezoid in trapezoidList)
            {
                while (originalPathingMaps.Count <= trapezoid.PathingMapId)
                {
                    originalPathingMaps.Add([]);
                }

                originalPathingMaps[trapezoid.PathingMapId].Add(trapezoid.Id);
            }

            return new PathingData
            {
                Trapezoids = trapezoidList,
                OriginalAdjacencyList = adjacencyList,
                OriginalPathingMaps = originalPathingMaps,
                NavMesh = await this.pathfinder.GenerateNavMesh(trapezoidList, cancellationToken)
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<PathingMetadata?> ReadPathingMetaData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadPathingMetaData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "pathing/metadata", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var pathingMetadataPayload = payload.Deserialize<PathingMetadataPayload>();
            if (pathingMetadataPayload is null)
            {
                return default;
            }

            return new PathingMetadata
            {
                TrapezoidCount = (int)pathingMetadataPayload.TrapezoidCount
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<PreGameData?> ReadPreGameData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadPreGameData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "pregame", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var preGamePayload = payload.Deserialize<PreGamePayload>();
            if (preGamePayload is null)
            {
                return default;
            }

            return new PreGameData
            {
                Characters = preGamePayload.Characters,
                ChosenCharacterIndex = preGamePayload.ChosenCharacterIndex
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<SessionData?> ReadSessionData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadSessionData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "session", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var sessionPayload = payload.Deserialize<SessionPayload>();
            if (sessionPayload is null)
            {
                return default;
            }

            _ = Map.TryParse((int)sessionPayload.MapId, out var map);
            return new SessionData
            {
                Session = new SessionInformation
                {
                    CurrentMap = map,
                    FoesKilled = sessionPayload.FoesKilled,
                    FoesToKill = sessionPayload.FoesToKill,
                    InstanceTimer = sessionPayload.InstanceTimer,
                    InstanceType = sessionPayload.InstanceType switch
                    {
                        0 => InstanceType.Outpost,
                        1 => InstanceType.Explorable,
                        2 => InstanceType.Loading,
                        _ => InstanceType.Undefined
                    }
                }
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<UserData?> ReadUserData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadUserData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "user", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var userPayload = payload.Deserialize<UserPayload>();
            if (userPayload is null)
            {
                return default;
            }

            return new UserData
            {
                User = new UserInformation
                {
                    Email = userPayload.Email,
                    CurrentKurzickPoints = userPayload.CurrentKurzickPoints,
                    CurrentLuxonPoints = userPayload.CurrentLuxonPoints,
                    CurrentImperialPoints = userPayload.CurrentImperialPoints,
                    CurrentBalthazarPoints = userPayload.CurrentBalthazarPoints,
                    CurrentSkillPoints = userPayload.CurrentSkillPoints,
                    TotalKurzickPoints = userPayload.TotalKurzickPoints,
                    TotalLuxonPoints = userPayload.TotalLuxonPoints,
                    TotalImperialPoints = userPayload.TotalImperialPoints,
                    TotalBalthazarPoints = userPayload.TotalBalthazarPoints,
                    TotalSkillPoints = userPayload.TotalSkillPoints,
                    MaxKurzickPoints = userPayload.MaxKurzickPoints,
                    MaxLuxonPoints = userPayload.MaxLuxonPoints,
                    MaxBalthazarPoints = userPayload.MaxBalthazarPoints,
                    MaxImperialPoints = userPayload.MaxImperialPoints
                }
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<WorldData?> ReadWorldData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReadWorldData), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, "map", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var mapPayload = payload.Deserialize<MapPayload>();
            if (mapPayload is null)
            {
                return default;
            }

            _ = Campaign.TryParse((int)mapPayload.Campaign, out var campaign);
            _ = Continent.TryParse((int)mapPayload.Continent, out var continent);
            _ = Daybreak.Models.Guildwars.Region.TryParse((int)mapPayload.Region, out var region);
            _ = Map.TryParse((int)mapPayload.Id, out var map);

            return new WorldData
            {
                Campaign = campaign,
                Continent = continent,
                Map = map,
                Region = region
            };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<GameState?> ReadGameState(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetEntityName), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, $"game/state", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var gameStatePayload = payload.Deserialize<GameStatePayload>();
            if (gameStatePayload is null ||
                gameStatePayload.States is null)
            {
                return default;
            }

            var states = gameStatePayload.States.Select(state =>
            {
                return new EntityGameState
                {
                    Id = (int)state.Id,
                    Position = new Position { X = state.PosX, Y = state.PosY },
                    State = state.State switch
                    {
                        0x08 => LivingEntityState.Dead,
                        0xC00 => LivingEntityState.Boss,
                        0x40000 => LivingEntityState.Spirit,
                        0x40008 => LivingEntityState.ToBeCleanedUp,
                        0x400000 => LivingEntityState.Player,
                        _ => LivingEntityState.Unknown
                    },
                    Health = state.Health,
                    Energy = state.Energy
                };
            }).ToList();

            return new GameState { States = states };
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<string?> GetEntityName(IEntity entity, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetEntityName), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, $"entities/name?id={entity.Id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var namePayload = payload.Deserialize<NamePayload>();
            if (namePayload is null ||
                namePayload.Id != entity.Id ||
                namePayload.Name!.IsNullOrWhiteSpace())
            {
                return default;
            }

            return namePayload.Name;
            
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public async Task<string?> GetItemName(int id, List<uint> modifiers, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetItemName), string.Empty);
        if (this.connectionContextCache is null)
        {
            return default;
        }

        var response = await this.client.GetAsync(this.connectionContextCache.Value, $"items/name?id={id}&modifiers={string.Join(',', modifiers?.Select(m => m.ToString()) ?? Array.Empty<string>())}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non-success response {response.StatusCode}");
            return default;
        }

        try
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            var namePayload = payload.Deserialize<NamePayload>();
            if (namePayload is null ||
                namePayload.Id != id ||
                namePayload.Name!.IsNullOrWhiteSpace())
            {
                return default;
            }

            var curedName = ItemNameColorRegex.Replace(namePayload.Name!, "");
            return curedName;

        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while parsing response");
            this.faulty = true;
        }

        return default;
    }

    public void Stop()
    {
        this.connectionContextCache = default;
    }

    private static IBagContent ParsePayload(BagContentPayload bagContentPayload)
    {
        var modifiers = bagContentPayload.Modifiers?.Select(mod => (ItemModifier)mod);
        return ItemBase.TryParse((int)bagContentPayload.Id, modifiers, out var item) ?
            new BagItem 
            { 
                Item = item!,
                Count = bagContentPayload.Count,
                Modifiers = modifiers!,
                Slot = bagContentPayload.Slot
            } :
            new UnknownBagItem
            { 
                ItemId = bagContentPayload.Id,
                Count = bagContentPayload.Count,
                Modifiers = modifiers!,
                Slot = bagContentPayload.Slot
            };
    }

    private static Bag ParsePayload(BagPayload bagPayload)
    {
        if (bagPayload is null)
        {
            return new Bag();
        }

        return new Bag
        {
            Capacity = bagPayload.Items?.Count ?? 0,
            Items = bagPayload.Items?.Select(ParsePayload).ToList() ?? []
        };
    }

    private static MainPlayerInformation ParsePayload(MainPlayerPayload mainPlayerPayload)
    {
        var worldPlayer = ParsePayload((WorldPlayerPayload)mainPlayerPayload);
        _ = Quest.TryParse((int)mainPlayerPayload.CurrentQuest, out var quest);
        return new MainPlayerInformation
        {
            Name = worldPlayer.Name,
            CurrentBuild = worldPlayer.CurrentBuild,
            CurrentEnergy = mainPlayerPayload.CurrentEnergy,
            CurrentHealth = mainPlayerPayload.CurrentHp,
            MaxEnergy = mainPlayerPayload.MaxEnergy,
            MaxHealth = mainPlayerPayload.MaxHp,
            Experience = mainPlayerPayload.Experience,
            HardModeUnlocked = mainPlayerPayload.HardModeUnlocked,
            Id = worldPlayer.Id,
            Level = worldPlayer.Level,
            Morale = mainPlayerPayload.Morale,
            Position = worldPlayer.Position,
            Quest = quest,
            PrimaryProfession = worldPlayer.PrimaryProfession,
            SecondaryProfession = worldPlayer.SecondaryProfession,
            Timer = worldPlayer.Timer,
            TitleInformation = worldPlayer.TitleInformation,
            UnlockedProfession = worldPlayer.UnlockedProfession,
            QuestLog = mainPlayerPayload.QuestLog?.Select(metadata =>
            {
                if (!Quest.TryParse((int)metadata.Id, out var q))
                {
                    return default;
                }

                _ = Map.TryParse((int)metadata.FromId, out var from);
                _ = Map.TryParse((int)metadata.ToId, out var to);
                return new QuestMetadata
                {
                    From = from,
                    To = to,
                    Position = new Position
                    {
                        X = metadata.PosX ?? 0,
                        Y = metadata.PosY ?? 0
                    },
                    Quest = q
                };
            }).OfType<QuestMetadata>().ToList()
        };
    }

    private static WorldPlayerInformation ParsePayload(WorldPlayerPayload worldPlayerPayload)
    {
        var partyPlayer = ParsePayload((PartyPlayerPayload)worldPlayerPayload);
        _ = Title.TryParse((int)(worldPlayerPayload.Title?.Id ?? (uint)Title.None.Id), out var title);
        return new WorldPlayerInformation
        {
            CurrentBuild = partyPlayer.CurrentBuild,
            Position = partyPlayer.Position,
            PrimaryProfession = partyPlayer.PrimaryProfession,
            SecondaryProfession = partyPlayer.SecondaryProfession,
            Id = partyPlayer.Id,
            Level = partyPlayer.Level,
            Name = worldPlayerPayload.Name,
            Timer = partyPlayer.Timer,
            UnlockedProfession = partyPlayer.UnlockedProfession,
            TitleInformation = new TitleInformation
            {
                CurrentPoints = worldPlayerPayload.Title?.CurrentPoints,
                IsPercentage = worldPlayerPayload.Title?.IsPercentage,
                MaxTierNumber = worldPlayerPayload.Title?.MaxTierNumber,
                TierNumber = worldPlayerPayload.Title?.TierNumber,
                PointsForCurrentRank = worldPlayerPayload.Title?.PointsForCurrentRank,
                PointsForNextRank = worldPlayerPayload.Title?.PointsForNextRank,
                Title = title
            }
        };
    }

    private static PlayerInformation ParsePayload(PartyPlayerPayload partyPlayerPayload)
    {
        var livingEntity = ParsePayload((LivingEntityPayload)partyPlayerPayload);
        var build = new Build
        {
            Primary = livingEntity.PrimaryProfession ?? Profession.None,
            Secondary = livingEntity.SecondaryProfession ?? Profession.None,
            Attributes = partyPlayerPayload.Build?.Attributes?.Select(a =>
            {
                if (!Daybreak.Models.Guildwars.Attribute.TryParse((int)a.Id, out var attribute))
                {
                    return default;
                }

                if (a.ActualLevel == 64)
                {
                    return default;
                }

                return new AttributeEntry
                {
                    Attribute = attribute,
                    Points = (int)a.BaseLevel
                };
            }).OfType<AttributeEntry>().ToList() ?? [],
            Skills = partyPlayerPayload.Build?.Skills?.Select(s =>
            {
                if(!Skill.TryParse((int)s, out var skill))
                {
                    return Skill.NoSkill;
                }

                return skill;
            }).ToList() ?? Enumerable.Repeat(Skill.NoSkill, 8).ToList()
        };
        return new PlayerInformation
        {
            Position = livingEntity.Position,
            PrimaryProfession = livingEntity.PrimaryProfession,
            SecondaryProfession = livingEntity.SecondaryProfession,
            CurrentBuild = build,
            Id = livingEntity.Id,
            Level = livingEntity.Level,
            NpcDefinition = livingEntity.NpcDefinition,
            ModelType = livingEntity.ModelType ?? 0,
            Timer = livingEntity.Timer,
            UnlockedProfession = partyPlayerPayload.UnlockedProfession?.Select(id =>
            {
                if (Profession.TryParse((int)id, out var profession))
                {
                    return profession;
                }

                return default;
            }).OfType<Profession>().ToList() ?? []
        };
    }

    private static LivingEntity ParsePayload(LivingEntityPayload livingEntityPayload)
    {
        _ = Npc.TryParse((int)livingEntityPayload.NpcDefinition, out var npc);
        _ = Profession.TryParse((int)livingEntityPayload.PrimaryProfessionId, out var primaryProfession);
        _ = Profession.TryParse((int)livingEntityPayload.SecondaryProfessionId, out var secondaryProfession);

        return new LivingEntity
        {
            Allegiance = (LivingEntityAllegiance)livingEntityPayload.EntityAllegiance,
            Id = (int)livingEntityPayload.Id,
            Level = (int)livingEntityPayload.Level,
            ModelType = livingEntityPayload.NpcDefinition,
            NpcDefinition = npc,
            PrimaryProfession = primaryProfession,
            SecondaryProfession = secondaryProfession,
            State = (LivingEntityState)livingEntityPayload.EntityState,
            Timer = livingEntityPayload.Timer,
            Position = new Position
            {
                X = livingEntityPayload.PosX,
                Y = livingEntityPayload.PosY,
            },
            Health = livingEntityPayload.Health,
            Energy = livingEntityPayload.Energy
        };
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
            adjacencyList.Add([.. originalAdjacencyList[i]]);
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

    [GeneratedRegex(@"<c=.*?>|</c>", RegexOptions.Compiled)]
    private static partial Regex GenerateItemNameColorRegex();
}
