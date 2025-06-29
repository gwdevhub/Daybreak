using Daybreak.Shared.Models.Guildwars;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;
using System.Text;

namespace Daybreak.Tests.Models;

public abstract class ItemTestsBase<T>
    where T : ItemBase
{
    protected abstract IEnumerable<T> AllItems { get; }

    [TestMethod]
    public void List_Should_ContainOnlyUniqueItems()
    {
        var itemMap = new Dictionary<T, int>();

        foreach (var item in this.AllItems)
        {
            if (!itemMap.TryGetValue(item, out _))
            {
                itemMap[item] = 0;
            }

            itemMap[item] += 1;
        }

        var errorMessage = new StringBuilder($"Duplicated {typeof(T).Name} entries: ");
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

    [TestMethod]
    public void List_Should_ContainAllItems()
    {
        var itemMap = new Dictionary<T, bool>();
        var definedItemFields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(T));
        foreach (var field in definedItemFields)
        {
            var item = field.GetValue(default).ThrowIfNull().Cast<T>();
            itemMap[item] = false;
        }

        foreach (var item in this.AllItems)
        {
            itemMap[item] = true;
        }

        var errorMessage = new StringBuilder($"{typeof(T).Name} entries not in list: ");
        foreach (var tuple in itemMap)
        {
            if (tuple.Value is false)
            {
                errorMessage.Append(tuple.Key.Name);
                errorMessage.Append(", ");
            }
        }

        if (itemMap.Any(kvp => kvp.Value is false))
        {
            Assert.Fail(errorMessage.ToString());
        }
    }
}
