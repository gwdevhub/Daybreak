using System.Core.Extensions;
using System.Extensions;
using System.Reflection;
using System.Text;
using Daybreak.Shared.Models.Guildwars;
using FluentAssertions;

namespace Daybreak.Tests.Models;

[TestClass]
public sealed class QuestTests
{
    [TestMethod]
    public void TryParse_UnknownId_Fails()
    {
        var found = Quest.TryParse(int.MaxValue, out var quest);
        found.Should().BeFalse();
        quest.Should().BeNull();
    }

    [TestMethod]
    public void Parse_UnknownId_ThrowsInvalidOperationException()
    {
        var action = () => Quest.Parse(int.MaxValue);
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TryParse_KnownId_Succeeds()
    {
        var found = Quest.TryParse(Quest.RaisuPalaceZaishenQuest.Id, out var quest);
        found.Should().BeTrue();
        quest.Should().Be(Quest.RaisuPalaceZaishenQuest);
    }

    [TestMethod]
    public void Parse_KnownId_ReturnsExpected()
    {
        var quest = Quest.Parse(Quest.RaisuPalaceZaishenQuest.Id);
        quest.Should().Be(Quest.RaisuPalaceZaishenQuest);
    }

    [TestMethod]
    public void QuestList_Should_ContainOnlyUniqueItems()
    {
        var questMap = new Dictionary<int, int>();

        foreach (var quest in Quest.Quests)
        {
            if (!questMap.TryGetValue(quest.Id, out _))
            {
                questMap[quest.Id] = 0;
            }

            questMap[quest.Id] += 1;
        }

        var errorMessage = new StringBuilder("Duplicated quest entries: ");
        foreach (var tuple in questMap)
        {
            if (tuple.Value > 1)
            {
                errorMessage.Append(tuple.Key);
                errorMessage.Append(", ");
            }
        }

        if (questMap.Any(kvp => kvp.Value > 1))
        {
            Assert.Fail(errorMessage.ToString());
        }
    }

    [TestMethod]
    public void QuestList_Should_ContainAllNpcs()
    {
        var questMap = new Dictionary<Quest, bool>();
        var definedNpcsFields = typeof(Quest).GetFields(BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(Quest));
        foreach (var field in definedNpcsFields)
        {
            var quest = field.GetValue(default).ThrowIfNull().Cast<Quest>();
            questMap[quest] = false;
        }

        foreach (var quest in Quest.Quests)
        {
            questMap[quest] = true;
        }

        var errorMessage = new StringBuilder("Quests not in list: ");
        foreach (var tuple in questMap)
        {
            if (tuple.Value is false)
            {
                errorMessage.Append(tuple.Key.Name);
                errorMessage.Append(", ");
            }
        }

        if (questMap.Any(kvp => kvp.Value is false))
        {
            Assert.Fail(errorMessage.ToString());
        }
    }
}
