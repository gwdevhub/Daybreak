using Daybreak.Shared.Models;
using MeaMod.DNS.Model;
using MeaMod.DNS.Multicast;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.MDns;

public sealed class MDnsService
    : IMDnsService
{
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
        var message = new Message();
        message.Questions.Add(new Question { Name = $"{service}.{protocol}.local", Type = DnsType.ANY });
        using var mdns = new MulticastService();
        mdns.Start();

        var resp = await mdns.ResolveAsync(message, cancellationToken);

        var srv = resp.Answers.OfType<SRVRecord>().FirstOrDefault();
        if (srv is null)
        {
            return default;
        }

        var port = srv.Port;
        var host = srv.Target;
        var scheme = ParseSchemeFromServiceType(srv);
        var ipv4 = resp.AdditionalRecords
                       .OfType<ARecord>()
                       .Where(a => a.Name == host)
                       .Select(a => a.Address)
                       .Cast<IPAddress>();

        var ipv6 = resp.AdditionalRecords
                       .OfType<AAAARecord>()
                       .Where(a => a.Name == host)
                       .Select(a => a.Address)
                       .Cast<IPAddress>();

        var uris = new List<Uri>();
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

            uris.Add(builder.Uri);
        }

        return uris;
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
