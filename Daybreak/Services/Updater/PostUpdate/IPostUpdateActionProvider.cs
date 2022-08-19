using System.Collections.Generic;

namespace Daybreak.Services.Updater.PostUpdate
{
    public interface IPostUpdateActionProvider
    {
        IEnumerable<PostUpdateActionBase> GetPostUpdateActions();
    }
}
