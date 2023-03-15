using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Daybreak.Models.Guildwars;

public sealed class EntityEqualityComparer : IEqualityComparer<IEntity>
{
    public bool Equals(IEntity? x, IEntity? y) => x?.Id == y?.Id;

    public int GetHashCode([DisallowNull] IEntity obj) => obj.Id;
}
