using Daybreak.Models.Guildwars;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class NpcTests
{
    [TestMethod]
    public void TryParse_UnknownId_Fails()
    {
        var found = Npc.TryParse(int.MaxValue, out var npc);
        found.Should().BeFalse();
        npc.Should().BeNull();
    }

    [TestMethod]
    public void Parse_UnknownId_ThrowsInvalidOperationException()
    {
        var action = () => Npc.Parse(int.MaxValue);
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TryParse_KnownId_Succeeds()
    {
        var found = Npc.TryParse(Npc.Kormir.Ids.FirstOrDefault(), out var npc);
        found.Should().BeTrue();
        npc.Should().Be(Npc.Kormir);
    }

    [TestMethod]
    public void Parse_KnownId_ReturnsExpected()
    {
        var npc = Npc.Parse(Npc.Kormir.Ids.FirstOrDefault());
        npc.Should().Be(Npc.Kormir);
    }

    [TestMethod]
    public void NpcList_Should_ContainOnlyUniqueItems()
    {
        var npcMap = new Dictionary<string, int>();

        foreach(var npc in Npc.Npcs)
        {
            if (!npcMap.TryGetValue(npc.Name.ToLower(), out _))
            {
                npcMap[npc.Name.ToLower()] = 0;
            }

            npcMap[npc.Name.ToLower()] += 1;
        }

        var errorMessage = new StringBuilder("Duplicated npc entries: ");
        foreach(var tuple in npcMap)
        {
            if (tuple.Value > 1)
            {
                errorMessage.Append(tuple.Key);
                errorMessage.Append(", ");
            }
        }

        if (npcMap.Any(kvp => kvp.Value > 1))
        {
            Assert.Fail(errorMessage.ToString());
        }
    }

    [TestMethod]
    public void NpcList_Should_ContainAllNpcs()
    {
        var npcMap = new Dictionary<Npc, bool>();
        var definedNpcsFields = typeof(Npc).GetFields(BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(Npc));
        foreach(var field in definedNpcsFields)
        {
            var npc = field.GetValue(default).Cast<Npc>();
            npcMap[npc] = false;
        }

        foreach(var npc in Npc.Npcs)
        {
            npcMap[npc] = true;
        }

        var errorMessage = new StringBuilder("Npcs not in list: ");
        foreach(var tuple in npcMap)
        {
            if (tuple.Value is false)
            {
                errorMessage.Append(tuple.Key.Name);
                errorMessage.Append(", ");
            }
        }

        if (npcMap.Any(kvp => kvp.Value is false))
        {
            Assert.Fail(errorMessage.ToString());
        }
    }
}
