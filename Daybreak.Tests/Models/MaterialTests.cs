using Daybreak.Models.Guildwars;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class MaterialTests : ItemTestsBase<Material>
{
    protected override IEnumerable<Material> AllItems { get; } = Material.All;
}
