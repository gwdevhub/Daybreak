using Daybreak.Shared.Models.Menu;

namespace Daybreak.Shared.Services.Initialization;

public interface IMenuServiceProducer
{
    MenuCategory CreateIfNotExistCategory(string name);
}
