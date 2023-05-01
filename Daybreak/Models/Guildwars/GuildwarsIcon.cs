using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

public sealed class GuildwarsIcon
{
    public static GuildwarsIcon ResurrectionShrine { get; } = new GuildwarsIcon { Id = 191474, Name = "Resurrection Shrine", WikiUrl = "https://wiki.guildwars.com/wiki/Resurrection_Shrine" };
    public static GuildwarsIcon Collector { get; } = new GuildwarsIcon { Id = 191482, Name = "Collector", WikiUrl = "https://wiki.guildwars.com/wiki/Collector" };
    public static GuildwarsIcon AreaMap { get; } = new GuildwarsIcon { Id = 302781, Name = "Area Map" };
    public static GuildwarsIcon StairsUp { get; } = new GuildwarsIcon { Id = 302783, Name = "Stairs Up" };
    public static GuildwarsIcon StairsDown { get; } = new GuildwarsIcon { Id = 302784, Name = "Stairs Down" };
    public static GuildwarsIcon Gate { get; } = new GuildwarsIcon { Id = 191464, Name = "Gate", WikiUrl = "" };
    public static GuildwarsIcon Star { get; } = new GuildwarsIcon { Id = 191472, Name = "Star", WikiUrl = "" };
    public static GuildwarsIcon DungeonBoss { get; } = new GuildwarsIcon { Id = 302779, Name = "Dungeon Boss" };
    public static GuildwarsIcon DungeonKey { get; } = new GuildwarsIcon { Id = 302777, Name = "Dungeon Key" };

    public static List<GuildwarsIcon> Icons { get; } = new()
    {
        ResurrectionShrine,
        Collector,
        AreaMap,
        StairsUp,
        StairsDown,
        Gate,
        Star,
        DungeonBoss,
        DungeonKey
    };

    public static bool TryParse(int id, out GuildwarsIcon icon)
    {
        icon = Icons.Where(icon => icon.Id == id).FirstOrDefault()!;
        if (icon is null)
        {
            return false;
        }

        return true;
    }

    public static bool TryParse(string name, out GuildwarsIcon icon)
    {
        icon = Icons.Where(map => map.Name == name).FirstOrDefault()!;
        if (icon is null)
        {
            return false;
        }

        return true;
    }

    public static GuildwarsIcon Parse(int id)
    {
        if (TryParse(id, out var icon) is false)
        {
            throw new InvalidOperationException($"Could not find an icon with id {id}");
        }

        return icon;
    }

    public static GuildwarsIcon Parse(string name)
    {
        if (TryParse(name, out var icon) is false)
        {
            throw new InvalidOperationException($"Could not find an icon with name {name}");
        }

        return icon;
    }

    public int Id { get; init; }
    public Affiliation Affiliation { get; init; }
    public string? Name { get; init; }
    public string? WikiUrl { get; init; }

    private GuildwarsIcon()
    {
    }
}
