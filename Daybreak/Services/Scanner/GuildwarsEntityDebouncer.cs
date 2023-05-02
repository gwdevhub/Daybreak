using Daybreak.Models;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Scanner.Models;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Services.Scanner;

public sealed class GuildwarsEntityDebouncer : IGuildwarsEntityDebouncer
{
    private const uint MaxMismatchTimer = 100;

    private DebouncePositionalEntityCache<MainPlayerInformation> mainPlayer = new();
    private List<DebouncePositionalEntityCache<WorldPlayerInformation>> worldPlayers = new();
    private List<DebouncePositionalEntityCache<PlayerInformation>> party = new();
    private List<DebouncePositionalEntityCache<LivingEntity>> livingEntities = new();

    public DebounceResponse DebounceEntities(GameData gameData)
    {
        this.DebounceEntitiesInternal(gameData);
        return new DebounceResponse
        {
            MainPlayer = this.mainPlayer.Entity,
            WorldPlayers = this.worldPlayers.Select(c => c.Entity),
            Party = this.party.Select(c => c.Entity),
            LivingEntities = this.livingEntities.Select(c => c.Entity)
        };
    }

    public void ClearCaches()
    {
        this.ClearInternal();
    }

    private void ClearInternal()
    {
        this.mainPlayer = new();
        this.worldPlayers.Clear();
        this.party.Clear();
        this.livingEntities.Clear();
    }

    private void DebounceEntitiesInternal(GameData gameData)
    {
        if (gameData.MainPlayer is MainPlayerInformation newMainPlayer)
        {
            this.DebounceEntityInternal(newMainPlayer, this.mainPlayer);
        }

        if (gameData.WorldPlayers is List<WorldPlayerInformation> newWorldPlayers)
        {
            this.worldPlayers = this.DebounceEntityListInternal(newWorldPlayers, this.worldPlayers).ToList();
        }

        if (gameData.Party is List<PlayerInformation> newPartyPlayers)
        {
            this.party = this.DebounceEntityListInternal(newPartyPlayers, this.party).ToList();
        }

        if (gameData.LivingEntities is List<LivingEntity> newLivingEntities)
        {
            this.livingEntities = this.DebounceEntityListInternal(newLivingEntities, this.livingEntities).ToList();
        }
    }

    private IEnumerable<DebouncePositionalEntityCache<T>> DebounceEntityListInternal<T>(List<T> newEntityList, List<DebouncePositionalEntityCache<T>> cacheList)
        where T : IEntity
    {
        var debouncingList = new List<(T, DebouncePositionalEntityCache<T>)>(newEntityList.Count);
        foreach(var entity in newEntityList)
        {
            if (cacheList.FirstOrDefault(e => e.Entity!.Id == entity.Id) is not DebouncePositionalEntityCache<T> cache)
            {
                cache = new DebouncePositionalEntityCache<T> { Entity = entity };
                cacheList.Add(cache);
            }

            cache.Entity = entity;
            debouncingList.Add((entity, cache));
        }

        foreach(var debouncingTuple in debouncingList)
        {
            this.DebounceEntityInternal(debouncingTuple.Item1, debouncingTuple.Item2);
        }

        return debouncingList.Select(e => e.Item2);
    }
        
    private void DebounceEntityInternal<T>(T newEntityData, DebouncePositionalEntityCache<T> cachedEntityData)
        where T : IEntity
    {
        // If the new entity has no position, ignore.
        if (newEntityData.Position is not Position newPosition)
        {
            return;
        }

        // If the cache is empty, initialize the cache.
        if (cachedEntityData.Entity is null)
        {
            cachedEntityData.Entity = newEntityData;
            cachedEntityData.PositionCache.Add(newPosition);
            return;
        }

        // If the new data is older than the old data, ignore the new data.
        // The second if clause is supposed to catch uint overflows.
        if (cachedEntityData.Entity.Timer - newEntityData.Timer > 0 &&
            cachedEntityData.Entity.Timer - newEntityData.Timer < MaxMismatchTimer)
        {
            return;
        }

        // If the position is already in the cache, ignore the new data.
        if (cachedEntityData.PositionCache.TryGetValue(newPosition, out _))
        {
            return;
        }

        // Add the position to the cache. If the cache is full, clear a portion of the cache.
        cachedEntityData.PositionCache.Add(newPosition);
        cachedEntityData.PositionList.AddLast(newPosition);
        cachedEntityData.Entity = newEntityData;
        if (cachedEntityData.PositionCache.Count > DebouncePositionalEntityCache<T>.CacheCapacity)
        {
            for (var i = 0; i < DebouncePositionalEntityCache<T>.CacheStep; i++)
            {
                cachedEntityData.PositionCache.Remove(cachedEntityData.PositionList.First());
                cachedEntityData.PositionList.RemoveFirst();
            }
        }
    }
}
