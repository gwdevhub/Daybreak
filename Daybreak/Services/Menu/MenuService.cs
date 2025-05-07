using Daybreak.Models.Menu;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;

namespace Daybreak.Services.Menu;

internal sealed class MenuService(
    IServiceProvider serviceProvider,
    ILogger<MenuService> logger) : IMenuService, IMenuServiceInitializer, IMenuServiceProducer, IMenuServiceButtonHandler
{
    private readonly IServiceProvider serviceProvider = serviceProvider.ThrowIfNull();
    private readonly ILogger<MenuService> logger = logger.ThrowIfNull();
    private readonly Dictionary<string, MenuCategory> categories = [];

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

    public MenuCategory CreateIfNotExistCategory(string name)
    {
        if (!this.categories.TryGetValue(name, out var category))
        {
            category = new MenuCategory(name);
            this.categories.Add(name, category);
        }

        return category;
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
