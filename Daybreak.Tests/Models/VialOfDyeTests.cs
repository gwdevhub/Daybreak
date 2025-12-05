using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class VialOfDyeTests : ItemTestsBase<VialOfDye>
{
    protected override IEnumerable<VialOfDye> AllItems { get; } = VialOfDye.Vials;
}
