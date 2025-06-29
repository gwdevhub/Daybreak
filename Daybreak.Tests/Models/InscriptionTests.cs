using Daybreak.Shared.Models.Guildwars;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class InscriptionTests : ItemTestsBase<Inscription>
{
    protected override IEnumerable<Inscription> AllItems { get; } = Inscription.Inscriptions;
}
