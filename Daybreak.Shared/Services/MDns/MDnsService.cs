using Daybreak.Shared.Models;
using MeaMod.DNS.Model;
using MeaMod.DNS.Multicast;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Daybreak.Shared.Services.MDns;

public sealed class MDnsService
    : IMDnsService
{
    private static readonly TimeSpan ScanTimeout = TimeSpan.FromSeconds(3);

    public DnsRegistrationToken RegisterDomain(string service, ushort port, string subType, string protocol = "_http._tcp")
    {
        var profile = new ServiceProfile(service, protocol, port);
        profile.AddProperty("path", "/");
        profile.AddProperty("name", service);

        profile.Subtypes.Add(subType);
        return new DnsRegistrationToken(profile);
    }

    public async Task<IReadOnlyList<Uri>?> FindLocalService(string service, string protocol = "_http._tcp", CancellationToken cancellationToken = default)
    {
        using var mdns = new MulticastService();
        mdns.Start();

        var message = new Message();
        message.Questions.Add(new Question { Name = $"{service}.{protocol}.local", Type = DnsType.ANY });

        var resp = await mdns.ResolveAsync(message, cancellationToken);
        var uris = BuildUrisFromResponse(resp).ToList();
        return uris;
    }

    public async Task<IReadOnlyList<Uri>?> FindLocalServices(string subType, string protocol = "_http._tcp", CancellationToken cancellationToken = default)
    {
        using var sd = new ServiceDiscovery();
        var uris = new ConcurrentBag<Uri>();

        sd.ServiceInstanceDiscovered += (s, e) =>
        {
            if (e.Message is not null)
            {
                var discoveredUris = BuildUrisFromResponse(e.Message);
                foreach(var uri in discoveredUris)
                {
                    uris.Add(uri);
                }
            }
        };

        var serviceType = new DomainName(protocol);
        sd.QueryServiceInstances(serviceType, subType);

        await Task.Delay(ScanTimeout, cancellationToken);
        return [.. uris.Distinct()];

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
        if (labels.Count < 2) return "http";

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
