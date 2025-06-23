using Daybreak.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.MDns;
public interface IMDnsService
{
    Task<IReadOnlyList<Uri>?> FindLocalService(string service, string protocol = "_http._tcp", CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Uri>?> FindLocalServices(string subType, string protocol = "_http._tcp", CancellationToken cancellationToken = default);
    DnsRegistrationToken RegisterDomain(string service, ushort port, string subType, string protocol = "_http._tcp");
}
