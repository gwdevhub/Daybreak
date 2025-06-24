using Daybreak.Shared.Models;
using MeaMod.DNS.Multicast;

namespace Daybreak.Shared.Services.MDns;

public sealed class MDomainNameService
    : IMDomainNameService
{
    public DnsRegistrationToken RegisterDomain(string service, ushort port, string subType, string protocol = "_http._tcp")
    {
        var profile = new ServiceProfile(service, protocol, port);
        profile.AddProperty("path", "/");
        profile.AddProperty("name", service);

        profile.Subtypes.Add(subType);
        return new DnsRegistrationToken(profile);
    }
}
