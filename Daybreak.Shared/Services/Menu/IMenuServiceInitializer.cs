using System;

namespace Daybreak.Services.Menu;

public interface IMenuServiceInitializer
{
    void InitializeMenuService(Action openMenuAction, Action closeMenuAction, Action toggleMenuAction);
}
