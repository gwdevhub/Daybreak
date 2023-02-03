using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;

namespace Daybreak.Services.Menu;

public sealed class MenuService : IMenuService, IMenuServiceInitializer
{
    private readonly ILogger<MenuService> logger;

    private Action? openMenuAction, closeMenuAction, toggleMenuAction;
    private bool initialized;

    public MenuService(
        ILogger<MenuService> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

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
            this.logger.LogError($"Called {nameof(this.CloseMenu)} before initializing {nameof(MenuService)}");
        }

        this.openMenuAction?.Invoke();
    }
    
    public void ToggleMenu()
    {
        if (this.initialized is false)
        {
            this.logger.LogError($"Called {nameof(this.CloseMenu)} before initializing {nameof(MenuService)}");
        }

        this.toggleMenuAction?.Invoke();
    }
}
