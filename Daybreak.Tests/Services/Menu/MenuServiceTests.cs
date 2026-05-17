using Daybreak.Services.Menu;
using Daybreak.Shared.Models.Menu;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Daybreak.Tests.Services.Menu;

[TestClass]
public sealed class MenuServiceTests
{
    private readonly Dictionary<string, MenuCategory> categories;
    private readonly IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
    private readonly MenuService service;

    public MenuServiceTests()
    {
        var settings = new MenuCategoryBuilder("Settings").Build();
        this.categories = new Dictionary<string, MenuCategory> { ["Settings"] = settings };
        this.service = new MenuService(this.categories, this.serviceProvider, Substitute.For<ILogger<MenuService>>());
    }

    [TestMethod]
    public void GetCategories_ReturnsAllRegisteredCategories()
    {
        this.service.GetCategories().Should().ContainSingle(c => c.Name == "Settings");
    }

    [TestMethod]
    public void OpenMenu_AfterInitialize_InvokesOpenAction()
    {
        var openCalled = 0;
        this.service.InitializeMenuService(() => openCalled++, () => { }, () => { });

        this.service.OpenMenu();

        openCalled.Should().Be(1);
    }

    [TestMethod]
    public void CloseMenu_AfterInitialize_InvokesCloseAction()
    {
        var closeCalled = 0;
        this.service.InitializeMenuService(() => { }, () => closeCalled++, () => { });

        this.service.CloseMenu();

        closeCalled.Should().Be(1);
    }

    [TestMethod]
    public void ToggleMenu_AfterInitialize_InvokesToggleAction()
    {
        var toggleCalled = 0;
        this.service.InitializeMenuService(() => { }, () => { }, () => toggleCalled++);

        this.service.ToggleMenu();

        toggleCalled.Should().Be(1);
    }

    [TestMethod]
    public void OpenMenu_WithoutInitialize_DoesNotThrow()
    {
        var act = this.service.OpenMenu;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void HandleButton_InvokesButtonActionWithServiceProvider()
    {
        IServiceProvider? receivedSp = null;
        var button = new MenuButton("Open", "Opens", sp => receivedSp = sp);

        this.service.HandleButton(button);

        receivedSp.Should().BeSameAs(this.serviceProvider);
    }

    private sealed class MenuCategoryBuilder(string name)
    {
        public MenuCategory Build()
        {
            // MenuCategory has internal ctor in the Shared assembly; reach for it via reflection
            // to avoid exposing it just for tests.
            return (MenuCategory)Activator.CreateInstance(
                typeof(MenuCategory),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                binder: null,
                args: [name],
                culture: null)!;
        }
    }
}
