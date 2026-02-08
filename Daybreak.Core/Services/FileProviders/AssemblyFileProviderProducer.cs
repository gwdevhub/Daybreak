using Daybreak.Models;
using Daybreak.Shared.Services.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;
using System.Reflection;

namespace Daybreak.Services.FileProviders;

public sealed class AssemblyFileProviderProducer(
    IServiceCollection services,
    ILogger<AssemblyFileProviderProducer> logger) : IFileProviderProducer
{
    private readonly IServiceCollection services = services;
    private readonly ILogger<AssemblyFileProviderProducer> logger = logger;

    public void RegisterAssembly(Assembly assembly)
    {
        var assemblyName = assembly.FullName ?? string.Empty;
        var scopedLogger = this.logger.CreateScopedLogger();
        this.services.AddSingleton(new FileProviderAssembly(assembly, assemblyName));
        scopedLogger.LogDebug("Registered provider assembly {assemblyName}", assemblyName);
    }
}
