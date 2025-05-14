using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class ItemBaseTests
{
    [TestMethod]
    public void TryParse_UnknownId_Fails()
    {
        var found = ItemBase.TryParse(int.MaxValue, modifiers: default, out var item);
        found.Should().BeFalse();
        item.Should().BeNull();
    }

    [TestMethod]
    public void Parse_UnknownId_ThrowsInvalidOperationException()
    {
        var action = () => ItemBase.Parse(int.MaxValue, modifiers: default);
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TryParse_KnownId_Succeeds()
    {
        var found = ItemBase.TryParse(Inscription.WeaponInscription.Id, Inscription.WeaponInscription.Modifiers, out var inscription);
        found.Should().BeTrue();
        inscription.Should().Be(Inscription.WeaponInscription);
    }

    [TestMethod]
    public void Parse_KnownId_ReturnsExpected()
    {
        var inscription = ItemBase.Parse(Inscription.WeaponInscription.Id, Inscription.WeaponInscription.Modifiers);
        inscription.Should().Be(Inscription.WeaponInscription);
    }

    [TestMethod]
    public void ItemList_Should_ContainOnlyUniqueItems()
    {
        var itemMap = new Dictionary<ItemBase, int>();

        foreach (var item in ItemBase.AllItems)
        {
            if (!itemMap.TryGetValue(item, out _))
            {
                itemMap[item] = 0;
            }

            itemMap[item] += 1;
        }

        var errorMessage = new StringBuilder("Duplicated item entries: ");
        foreach (var tuple in itemMap)
        {
            if (tuple.Value > 1)
            {
                errorMessage.Append(tuple.Key.Name);
                errorMessage.Append(", ");
            }
        }

        if (itemMap.Any(kvp => kvp.Value > 1))
        {
            Assert.Fail(errorMessage.ToString());
        }
    }
}
