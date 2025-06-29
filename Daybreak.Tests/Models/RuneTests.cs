using Daybreak.Shared.Models.Guildwars;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class RuneTests : ItemTestsBase<Rune>
{
    protected override IEnumerable<Rune> AllItems { get; } = Rune.Runes;
}
