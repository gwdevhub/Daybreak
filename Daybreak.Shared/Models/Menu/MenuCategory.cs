namespace Daybreak.Shared.Models.Menu;
public sealed class MenuCategory
{
    private readonly List<MenuButton> buttons = [];

    public string Name { get; init; }
    public IReadOnlyList<MenuButton> Buttons => this.buttons;

    internal MenuCategory(string name)
    {
        this.Name = name;
    }

    public MenuCategory RegisterButton(string name, string hint, Action<IServiceProvider> action)
    {
        this.buttons.Add(new MenuButton(name, hint, action));
        return this;
    }
}
