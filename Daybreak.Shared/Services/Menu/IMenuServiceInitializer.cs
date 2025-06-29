namespace Daybreak.Shared.Services.Menu;

public interface IMenuServiceInitializer
{
    void InitializeMenuService(Action openMenuAction, Action closeMenuAction, Action toggleMenuAction);
}
