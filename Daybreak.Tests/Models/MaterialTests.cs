using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class MaterialTests : ItemTestsBase<Material>
{
    protected override IEnumerable<Material> AllItems { get; } = Material.All;
}
