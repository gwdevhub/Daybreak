using Daybreak.Shared.Models;
using Daybreak.Shared.Services.MDns;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Extensions.Core;

namespace Daybreak.API.Services;

public sealed class ApiAdvertisingService(
    IServer server,
    IMDnsService mDnsService,
    IHostApplicationLifetime lifetime,
    ILogger<ApiAdvertisingService> logger) : IHostedService
{
    private const string ProcessIdPlaceholder = "{PID}";
    private const string ServiceName = $"daybreak-api-{ProcessIdPlaceholder}";
    private const string ServiceSubType = "daybreak-api";
    private readonly IServerAddressesFeature serverAddressesFeature = server.Features.Get<IServerAddressesFeature>() ?? throw new InvalidOperationException("Server does not support addresses feature.");
    private readonly IMDnsService mDnsService = mDnsService;
    private readonly IHostApplicationLifetime lifetime = lifetime;
    private readonly ILogger<ApiAdvertisingService> logger = logger;

    private DnsRegistrationToken? dnsToken;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.lifetime.ApplicationStarted.Register(() =>
        {
            var scopedLogger = this.logger.CreateScopedLogger();
            var addresses = this.serverAddressesFeature?.Addresses.ToArray() ?? [];
            var port = addresses
                .Select(a => new Uri(a).Port)
                .Distinct()
                .FirstOrDefault();

            scopedLogger.LogInformation("Found port {port}", port);
            if (port is 0)
            {
                throw new InvalidOperationException("Unable to start advertising service. No port found");
            }

            var dnsEntry = ServiceName.Replace(ProcessIdPlaceholder, Environment.ProcessId.ToString());
            scopedLogger.LogInformation("Advertising service {service}:{port}", dnsEntry, port);
            this.dnsToken = this.mDnsService.RegisterDomain(dnsEntry, (ushort)port, ServiceSubType);
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.dnsToken?.Dispose();
        return Task.CompletedTask;
    }
}
