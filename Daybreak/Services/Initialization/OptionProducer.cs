using Daybreak.Extensions;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Initialization;

public sealed class OptionProducer(IServiceCollection services, ILogger<OptionProducer> logger)
    : IOptionsProducer
{
    private readonly ILogger<OptionProducer> logger = logger;
    private readonly IServiceCollection services = services;

    public void RegisterOptions<TOptions>()
        where TOptions : class, new()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        this.services.AddDaybreakOptions<TOptions>();
        scopedLogger.LogDebug("Registered {Option.Name}", typeof(TOptions).Name);
    }
}
