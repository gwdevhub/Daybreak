using Daybreak.Shared.Models.Menu;

namespace Daybreak.Shared.Services.Menu;

public interface IMenuServiceProducer
{
    MenuCategory CreateIfNotExistCategory(string name);
    IEnumerable<MenuCategory> GetCategories();
}
