using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.MDns;
public interface IMDomainNameService
{
    DnsRegistrationToken RegisterDomain(string service, ushort port, string subType, string protocol = "_http._tcp");
}
