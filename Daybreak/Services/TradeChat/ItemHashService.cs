﻿using Daybreak.Models.Guildwars;
using System.Collections.Generic;
using System.Extensions;
using System.Text;

namespace Daybreak.Services.TradeChat;

public sealed class ItemHashService : IItemHashService
{
    public string? ComputeHash(ItemBase itemBase)
    {
        return ComputeHashInternal(itemBase.Modifiers);
    }

    public string? ComputeHash(IBagContent bagContent)
    {
        return ComputeHashInternal(bagContent.Modifiers);
    }

    private static string? ComputeHashInternal(IEnumerable<ItemModifier>? itemModifiers)
    {
        if (itemModifiers is null ||
            itemModifiers.None())
        {
            return default;
        }

        var sb = new StringBuilder();
        foreach (var modifier in itemModifiers)
        {
            sb.Append(modifier.Modifier.ToString("X"));
        }

        return sb.ToString();
    }
}
