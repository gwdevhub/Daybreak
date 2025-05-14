using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Updater.PostUpdate;

public interface IPostUpdateActionProducer
{
    void AddPostUpdateAction<T>()
        where T : PostUpdateActionBase;
}
