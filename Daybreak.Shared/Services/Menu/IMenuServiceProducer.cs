using Daybreak.Shared.Models.Menu;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.Menu;

public interface IMenuServiceProducer
{
    MenuCategory CreateIfNotExistCategory(string name);
    IEnumerable<MenuCategory> GetCategories();
}
