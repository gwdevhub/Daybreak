using System;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.MDns;
public interface IMDomainRegistrar
{
    IReadOnlyList<Uri>? Resolve(string service);

    IReadOnlyList<Uri>? QueryByServiceName(Func<string, bool> query);

    void QueryAllServices();
}
