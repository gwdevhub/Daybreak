using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Initialization;

public sealed class PostUpdateActionProducer(IServiceCollection services, ILogger<PostUpdateActionProducer> logger)
    : IPostUpdateActionProducer
{
    private readonly ILogger<PostUpdateActionProducer> logger = logger;
    private readonly IServiceCollection services = services;

    public void AddPostUpdateAction<T>() where T : PostUpdateActionBase
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        this.services.AddSingleton<PostUpdateActionBase, T>();
        scopedLogger.LogDebug("Registered {PostUpdateAction.Name}", typeof(T).Name);
    }
}
