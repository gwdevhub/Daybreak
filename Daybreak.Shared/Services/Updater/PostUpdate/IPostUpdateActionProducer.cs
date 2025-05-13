using Daybreak.Models;

namespace Daybreak.Services.Updater.PostUpdate;

public interface IPostUpdateActionProducer
{
    void AddPostUpdateAction<T>()
        where T : PostUpdateActionBase;
}
