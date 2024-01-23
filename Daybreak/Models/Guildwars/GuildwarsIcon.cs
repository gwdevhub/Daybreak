using Daybreak.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

[JsonConverter(typeof(GuildwarsIconJsonConverter))]
public sealed class GuildwarsIcon : IWikiEntity
{
    public static readonly GuildwarsIcon ResurrectionShrine = new() { Id = 191474, Name = "Resurrection Shrine", WikiUrl = "https://wiki.guildwars.com/wiki/Resurrection_Shrine" };
    public static readonly GuildwarsIcon Collector = new() { Id = 191482, Name = "Collector", WikiUrl = "https://wiki.guildwars.com/wiki/Collector" };
    public static readonly GuildwarsIcon Person = new() { Id = 191466, Name = "Person" };
    public static readonly GuildwarsIcon Flag = new() { Id = 191462, Name = "Flag" };
    public static readonly GuildwarsIcon AreaMap = new() { Id = 302781, Name = "Area Map" };
    public static readonly GuildwarsIcon StairsUp = new() { Id = 302783, Name = "Stairs Up" };
    public static readonly GuildwarsIcon StairsDown = new() { Id = 302784, Name = "Stairs Down" };
    public static readonly GuildwarsIcon Gate = new() { Id = 191464, Name = "Gate", WikiUrl = "" };
    public static readonly GuildwarsIcon Star = new() { Id = 191472, Name = "Star", WikiUrl = "" };
    public static readonly GuildwarsIcon DungeonBoss = new() { Id = 302779, Name = "Dungeon Boss" };
    public static readonly GuildwarsIcon DungeonKey = new() { Id = 302777, Name = "Dungeon Key" };

    public static readonly List<GuildwarsIcon> Icons =
    [
        ResurrectionShrine,
        Collector,
        AreaMap,
        StairsUp,
        StairsDown,
        Gate,
        Star,
        DungeonBoss,
        DungeonKey,
        Flag,
        Person
    ];

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
