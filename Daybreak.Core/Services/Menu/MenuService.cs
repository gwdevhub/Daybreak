using Daybreak.Shared.Models.Menu;
using Daybreak.Shared.Services.Menu;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;

namespace Daybreak.Services.Menu;

internal sealed class MenuService(
    IReadOnlyDictionary<string, MenuCategory> menuCategories,
    IServiceProvider serviceProvider,
    ILogger<MenuService> logger) : IMenuService, IMenuServiceInitializer, IMenuServiceButtonHandler
{
    private readonly IReadOnlyDictionary<string, MenuCategory> categories = menuCategories.ThrowIfNull();
    private readonly IServiceProvider serviceProvider = serviceProvider.ThrowIfNull();
    private readonly ILogger<MenuService> logger = logger.ThrowIfNull();

    private Action? openMenuAction, closeMenuAction, toggleMenuAction;
    private bool initialized;

    public void InitializeMenuService(Action openMenuAction, Action closeMenuAction, Action toggleMenuAction)
    {
        this.openMenuAction = openMenuAction.ThrowIfNull();
        this.closeMenuAction = closeMenuAction.ThrowIfNull();
        this.toggleMenuAction = toggleMenuAction.ThrowIfNull();
        this.initialized = true;
    }

    public void CloseMenu()
    {
        if (this.initialized is false)
        {
            this.logger.LogError($"Called {nameof(this.CloseMenu)} before initializing {nameof(MenuService)}");
        }

        this.closeMenuAction?.Invoke();
    }

    public void OpenMenu()
    {
        if (this.initialized is false)
        {
            this.logger.LogError($"Called {nameof(this.OpenMenu)} before initializing {nameof(MenuService)}");
        }

        this.openMenuAction?.Invoke();
    }
    
    public void ToggleMenu()
    {
        if (this.initialized is false)
        {
            this.logger.LogError($"Called {nameof(this.ToggleMenu)} before initializing {nameof(MenuService)}");
        }

        this.toggleMenuAction?.Invoke();
    }

    public IEnumerable<MenuCategory> GetCategories()
    {
        return this.categories.Values;
    }

    public void HandleButton(MenuButton button)
    {
        button.Action(this.serviceProvider);
    }
}
