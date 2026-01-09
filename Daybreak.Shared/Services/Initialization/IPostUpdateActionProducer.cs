using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Initialization;

public interface IPostUpdateActionProducer
{
    void AddPostUpdateAction<T>()
        where T : PostUpdateActionBase;
}
