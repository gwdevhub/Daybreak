using Daybreak.Shared.Models.Guildwars;
using System.Collections.Immutable;

namespace Daybreak.Shared.Services.Items;

public interface IItemModifierParser
{
    ImmutableArray<ItemProperty> ParseItemModifiers(ImmutableArray<ItemModifier> modifiers);
}
