using Daybreak.Models.Menu;
using System.Collections.Generic;

namespace Daybreak.Services.Menu;

public interface IMenuServiceProducer
{
    MenuCategory CreateIfNotExistCategory(string name);
    IEnumerable<MenuCategory> GetCategories();
}
