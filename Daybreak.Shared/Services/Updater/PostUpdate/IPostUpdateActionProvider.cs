using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Updater.PostUpdate;

public interface IPostUpdateActionProvider
{
    IEnumerable<PostUpdateActionBase> GetPostUpdateActions();
}
