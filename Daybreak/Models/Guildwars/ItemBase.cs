using System;
using System.Linq;

namespace Daybreak.Models.Guildwars;
public abstract class ItemBase
{
    public static bool TryParse<T>(int id, out T item)
        where T : ItemBase
    {
        item = default!;
        if (TryParse(id, out var itemBase) is false)
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
    public static T Parse<T>(int id)
        where T : ItemBase
    {
        var itemBase = Parse(id);
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

    public static bool TryParse(int id, out ItemBase? item)
    {
        item = FilterAndFirstOrDefault(i => i.Id == id);
        if (item is not null)
        {
            return true;
        }

        return false;
    }
    public static ItemBase Parse(int id)
    {
        if (TryParse(id, out var item) is false)
        {
            throw new InvalidOperationException($"Could not find a item with id {id}");
        }

        return item;
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

    private static ItemBase? FilterAndFirstOrDefault(Func<ItemBase, bool> filter) => Enumerable.Empty<ItemBase>()
        .Append(Unknown.Instance)
        .Concat(Material.All)
        .Where(filter)
        .FirstOrDefault();

    public int Id { get; init; }

    public string? Name { get; init; }

    public sealed class Unknown : ItemBase
    {
        public static readonly Unknown Instance = new() { Id = 0, Name = "Unknown" };

        private Unknown()
        {
        }
    }
}
