using Daybreak.Shared.Models.Guildwars;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class InscriptionTests : ItemTestsBase<Inscription>
{
    protected override IEnumerable<Inscription> AllItems { get; } = Inscription.Inscriptions;
}
