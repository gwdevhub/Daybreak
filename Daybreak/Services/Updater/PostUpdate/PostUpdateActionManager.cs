using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Updater.PostUpdate;
using Microsoft.Extensions.Logging;
using Slim;
using System.Core.Extensions;

namespace Daybreak.Services.Updater.PostUpdate;

internal sealed class PostUpdateActionManager(
    IServiceManager serviceManager,
    ILogger<PostUpdateActionManager> logger) : IPostUpdateActionManager
{
    private readonly IServiceManager serviceManager = serviceManager.ThrowIfNull();
    private readonly ILogger<PostUpdateActionManager> logger = logger.ThrowIfNull();

    public void AddPostUpdateAction<T>()
        where T : PostUpdateActionBase
    {
        this.serviceManager.RegisterSingleton<T>();
        this.logger.LogDebug($"Added post update action {typeof(T).Name}");
    }

    public IEnumerable<PostUpdateActionBase> GetPostUpdateActions()
    {
        return this.serviceManager.GetServicesOfType<PostUpdateActionBase>();
    }
}
