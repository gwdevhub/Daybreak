using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Models;
public sealed class BuildListEntry
{
    public required IBuildEntry BuildEntry { get; init; }
    public required Profession? PrimaryProfession { get; init; }
}
