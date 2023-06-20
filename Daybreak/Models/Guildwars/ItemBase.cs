using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;
public abstract class ItemBase
{
    public static IReadOnlyCollection<ItemBase> AllItems { get; } = Enumerable.Empty<ItemBase>()
        .Append(Unknown.Instance)
        .Concat(Material.All)
        .Concat(Inscription.Inscriptions)
        .Concat(Rune.Runes)
        .ToList();

    public static bool TryParse<T>(int id, IEnumerable<ItemModifier>? modifiers, out T item)
        where T : ItemBase
    {
        item = default!;
        if (TryParse(id, modifiers, out var itemBase) is false)
        {
            return false;
        }

        if (itemBase is not T)
        {
            return false;
        }

        item = (itemBase as T)!;
        return true;
    }
    public static bool TryParse<T>(string name, out T item)
        where T : ItemBase
    {
        item = default!;
        if (TryParse(name, out var itemBase) is false)
        {
            return false;
        }

        if (itemBase is not T)
        {
            return false;
        }

        item = (itemBase as T)!;
        return true;
    }
    public static T Parse<T>(int id, IEnumerable<ItemModifier>? modifiers)
        where T : ItemBase
    {
        var itemBase = Parse(id, modifiers);
        if (itemBase is not T)
        {
            throw new InvalidOperationException($"Invalid type. Item type is [{itemBase.GetType().Name}] but requested type is [{typeof(T).Name}]");
        }

        return (itemBase as T)!;
    }
    public static T Parse<T>(string name)
        where T : ItemBase
    {
        var itemBase = Parse(name);
        if (itemBase is not T)
        {
            throw new InvalidOperationException($"Invalid type. Item type is [{itemBase.GetType().Name}] but requested type is [{typeof(T).Name}]");
        }

        return (itemBase as T)!;
    }

    public static bool TryParse(int id, IEnumerable<ItemModifier>? modifiers, out ItemBase? item)
    {
        item = FilterAndFirstOrDefault(i =>
        {
            if (i.Id != id)
            {
                return false;
            }

            if (modifiers is not null &&
                i.Modifiers is not null)
            {
                return i.Modifiers.Count() == modifiers.Count() &&
                       i.Modifiers.All(modifiers.Contains);
            }
            else if (i.Modifiers is not null)
            {
                return false;
            }

            return true;
        });
        if (item is not null)
        {
            return true;
        }

        return false;
    }
    public static ItemBase Parse(int id, IEnumerable<ItemModifier>? modifiers)
    {
        if (TryParse(id, modifiers, out var item) is false)
        {
            throw new InvalidOperationException($"Could not find a item with id {id}");
        }

        return item ?? throw new InvalidOperationException($"Could not parse item with id {id}");
    }

    public static bool TryParse(string name, out ItemBase? item)
    {
        item = FilterAndFirstOrDefault(i => i.Name == name);
        if (item is not null)
        {
            return true;
        }

        return false;
    }
    public static ItemBase Parse(string name)
    {
        if (TryParse(name, out var item) is false)
        {
            throw new InvalidOperationException($"Could not find a item with name {name}");
        }

        return item;
    }

    private static ItemBase? FilterAndFirstOrDefault(Func<ItemBase, bool> filter) => AllItems
        .Where(filter)
        .FirstOrDefault();

    public int Id { get; init; }

    public string? Name { get; init; }

    public IEnumerable<ItemModifier>? Modifiers { get; init; }

    public sealed class Unknown : ItemBase
    {
        public static readonly Unknown Instance = new() { Id = 0, Name = "Unknown" };

        private Unknown()
        {
        }
    }
}
