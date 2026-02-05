using Daybreak.Shared.Services.MDns;
using MeaMod.DNS.Model;
using MeaMod.DNS.Multicast;
using System.Collections.Concurrent;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using Microsoft.Extensions.Hosting;

namespace Daybreak.Services.MDns;

public sealed class MDomainRegistrar(
    ILogger<MDomainRegistrar> logger)
    : IMDomainRegistrar, IHostedService
{
    private static readonly TimeSpan ScanFrequency = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan MaxTTL = TimeSpan.FromSeconds(20);

    private readonly ConcurrentDictionary<string, ServiceRegistration> serviceLookup = [];
    private readonly ServiceDiscovery serviceDiscovery = new();
    private readonly ILogger<MDomainRegistrar> logger = logger.ThrowIfNull();

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        this.serviceDiscovery.ServiceInstanceDiscovered += this.ServiceDiscovery_ServiceInstanceDiscovered;
        this.serviceDiscovery.ServiceInstanceShutdown += this.ServiceDiscovery_ServiceInstanceShutdown;
        this.serviceDiscovery.ServiceDiscovered += this.ServiceDiscovery_ServiceDiscovered;
        return Task.Factory.StartNew(() => this.QueryServicesPeriodically(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        this.serviceDiscovery.ServiceInstanceDiscovered -= this.ServiceDiscovery_ServiceInstanceDiscovered;
        this.serviceDiscovery.ServiceInstanceShutdown -= this.ServiceDiscovery_ServiceInstanceShutdown;
        this.serviceDiscovery.Dispose();
        return Task.CompletedTask;
    }

    public IReadOnlyList<Uri>? Resolve(string service)
    {
        if (this.serviceLookup.TryGetValue(service, out var registration) &&
            registration.Expiration > DateTime.UtcNow)
        {
            return [.. registration.Uris];
        }

        return default;
    }

    public IReadOnlyList<Uri>? QueryByServiceName(Func<string, bool> query)
    {
        var now = DateTime.UtcNow;
        var returnList = new List<Uri>();
        foreach (var serviceRegistration in this.serviceLookup.Values.Where(s => s.Expiration > now))
        {
            if (query(serviceRegistration.Name))
            {
                returnList.AddRange(serviceRegistration.Uris);
            }
        }

        return returnList;
    }

    public void QueryAllServices()
    {
        this.serviceDiscovery.QueryAllServices();
    }

    private void ServiceDiscovery_ServiceDiscovered(object? _, DomainName e)
    {
        var labels = e.Labels;
        e = labels.Count >= 1 && labels[^1] == "local"
             ? new DomainName([.. labels.Take(labels.Count - 1)])
             : e;

        this.serviceDiscovery.QueryServiceInstances(e);
    }

    private void ServiceDiscovery_ServiceInstanceShutdown(object? _, ServiceInstanceShutdownEventArgs e)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (e.ServiceInstanceName.Labels.Count <= 0 ||
            e.ServiceInstanceName.Labels[0] is not string name)
        {
            return;
        }

        scopedLogger.LogDebug("Service {serviceName} has been shut down", name);
        this.serviceLookup.TryRemove(name, out var __);
    }

    private void ServiceDiscovery_ServiceInstanceDiscovered(object? _, ServiceInstanceDiscoveryEventArgs e)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (e.ServiceInstanceName.Labels.Count <= 0 ||
            e.ServiceInstanceName.Labels[0] is not string name)
        {
            return;
        }


        if (e.Message.Answers.OfType<PTRRecord>().FirstOrDefault() is not PTRRecord ptrRecord)
        {
            return;
        }

        scopedLogger.LogDebug("Discovered service {serviceName}", name);
        var ttl = ptrRecord.TTL > MaxTTL
            ? MaxTTL
            : ptrRecord.TTL;
        var expiration = DateTime.UtcNow + ttl;
        this.serviceLookup[name] = new ServiceRegistration { Name = name, Expiration = expiration, Uris = [.. BuildUrisFromResponse(e.Message)] };
    }

    private async ValueTask QueryServicesPeriodically(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            this.serviceDiscovery.QueryAllServices();
            await Task.Delay(ScanFrequency, cancellationToken);
        }
    }

    private static IEnumerable<Uri> BuildUrisFromResponse(Message srvResponse)
    {
        var srv = srvResponse.Answers.OfType<PTRRecord>().FirstOrDefault() is not null
            ? srvResponse.AdditionalRecords.OfType<SRVRecord>().FirstOrDefault()
            : srvResponse.Answers.OfType<SRVRecord>().FirstOrDefault();
        if (srv is not null)
        {
            var port = srv.Port;
            var host = srv.Target;
            var scheme = ParseSchemeFromServiceType(srv);
            var ipv4 = srvResponse.AdditionalRecords
                           .OfType<ARecord>()
                           .Where(a => a.Name == host)
                           .Select(a => a.Address)
                           .Cast<IPAddress>();

            var ipv6 = srvResponse.AdditionalRecords
                           .OfType<AAAARecord>()
                           .Where(a => a.Name == host)
                           .Select(a => a.Address)
                           .Cast<IPAddress>();

            foreach (var ip in ipv4.Concat(ipv6))
            {
                var builder = new UriBuilder
                {
                    Scheme = scheme,
                    Host = ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6
                             ? $"[{ip}]"
                             : ip.ToString(),
                    Port = port
                };

                yield return builder.Uri;
            }
        }
    }

    private static string ParseSchemeFromServiceType(SRVRecord srvRecord)
    {
        // split "instance._http._tcp.local." → ["instance", "_http", "_tcp", "local"]
        var labels = srvRecord.Target.Labels;
        if (labels.Count <= 2) return "http";

        return labels[^3] switch
        {
            "_https" => "https",
            "_ws" => "ws",
            "_wss" => "wss",
            "_ftp" => "ftp",
            _ => "http"
        };
    }
}
