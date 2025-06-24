using System;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.MDns;
public interface IMDomainRegistrar
{
    public IReadOnlyList<Uri>? Resolve(string service);

    public IReadOnlyList<Uri>? QueryByServiceName(Func<string, bool> query);
}
