using Daybreak.Services.Initialization;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Daybreak.Tests.Services.Initialization;

[TestClass]
public sealed class MenuProducerTests
{
    private readonly MenuProducer producer = new(Substitute.For<ILogger<MenuProducer>>());

    [TestMethod]
    public void CreateIfNotExistCategory_NewName_CreatesAndReturnsCategory()
    {
        var category = this.producer.CreateIfNotExistCategory("Tools");

        category.Should().NotBeNull();
        category.Name.Should().Be("Tools");
        this.producer.categories.Should().ContainKey("Tools").WhoseValue.Should().BeSameAs(category);
    }

    [TestMethod]
    public void CreateIfNotExistCategory_ExistingName_ReturnsSameInstanceWithoutDuplicating()
    {
        var first = this.producer.CreateIfNotExistCategory("Tools");
        var second = this.producer.CreateIfNotExistCategory("Tools");

        second.Should().BeSameAs(first);
        this.producer.categories.Should().HaveCount(1);
    }

    [TestMethod]
    public void CreateIfNotExistCategory_DifferentNames_KeepsBothCategories()
    {
        var tools = this.producer.CreateIfNotExistCategory("Tools");
        var settings = this.producer.CreateIfNotExistCategory("Settings");

        tools.Should().NotBeSameAs(settings);
        this.producer.categories.Keys.Should().BeEquivalentTo("Tools", "Settings");
    }
}
