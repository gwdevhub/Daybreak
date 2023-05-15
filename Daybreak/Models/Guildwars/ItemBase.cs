using System;
using System.Linq;

namespace Daybreak.Models.Guildwars;
public abstract class ItemBase
{
    public static bool TryParse(int id, out ItemBase item)
    {
        item = Material.All.Where(material => material.Id == id).FirstOrDefault()!;
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

    public static bool TryParse(string name, out ItemBase item)
    {
        item = Material.All.Where(material => material.Name == name).FirstOrDefault()!;
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

    public int Id { get; init; }

    public string? Name { get; init; }
}
