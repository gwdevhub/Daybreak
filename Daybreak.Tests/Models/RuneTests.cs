using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class RuneTests : ItemTestsBase<Rune>
{
    protected override IEnumerable<Rune> AllItems { get; } = Rune.Runes;
}
