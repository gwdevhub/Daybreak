using System;

namespace Daybreak.Shared.Models.Menu;

public sealed class MenuButton(string name, string hint, Action<IServiceProvider> action)
{
    public string Name { get; } = name;
    public string Hint { get; } = hint;
    public Action<IServiceProvider> Action { get; } = action;
}
