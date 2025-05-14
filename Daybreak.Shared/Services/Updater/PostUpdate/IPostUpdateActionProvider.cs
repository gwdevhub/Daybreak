using Daybreak.Shared.Models;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.Updater.PostUpdate;

public interface IPostUpdateActionProvider
{
    IEnumerable<PostUpdateActionBase> GetPostUpdateActions();
}
