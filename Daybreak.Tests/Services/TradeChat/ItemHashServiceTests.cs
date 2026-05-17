using Daybreak.Services.TradeChat;
using Daybreak.Shared.Models.Guildwars;
using FluentAssertions;

namespace Daybreak.Tests.Services.TradeChat;

[TestClass]
public sealed class ItemHashServiceTests
{
    private readonly ItemHashService service = new();

    [TestMethod]
    public void ComputeHash_NullModifiers_ReturnsNull()
    {
        this.service.ComputeHash(new TestItem { Modifiers = null }).Should().BeNull();
    }

    [TestMethod]
    public void ComputeHash_EmptyModifiers_ReturnsNull()
    {
        this.service.ComputeHash(new TestItem { Modifiers = [] }).Should().BeNull();
    }

    [TestMethod]
    public void ComputeHash_SingleModifier_ReturnsUppercaseHex()
    {
        var item = new TestItem { Modifiers = [(ItemModifier)0xABCu] };

        this.service.ComputeHash(item).Should().Be("ABC");
    }

    [TestMethod]
    public void ComputeHash_MultipleModifiers_ConcatenatesInOrder()
    {
        var item = new TestItem
        {
            Modifiers =
            [
                (ItemModifier)0x1u,
                (ItemModifier)0xDEADBEEFu,
                (ItemModifier)0x10u,
            ]
        };

        this.service.ComputeHash(item).Should().Be("1DEADBEEF10");
    }

    [TestMethod]
    public void ComputeHash_IsOrderSensitive()
    {
        var first = new TestItem { Modifiers = [(ItemModifier)0x1u, (ItemModifier)0x2u] };
        var second = new TestItem { Modifiers = [(ItemModifier)0x2u, (ItemModifier)0x1u] };

        this.service.ComputeHash(first).Should().NotBe(this.service.ComputeHash(second));
    }

    private sealed class TestItem : ItemBase
    {
    }
}
