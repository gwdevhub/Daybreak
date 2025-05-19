using System.Core.Extensions;

namespace Daybreak.API.Interop;

public sealed class GWAddressCache(Func<nuint> provider)
{
    private readonly Func<nuint> provider = provider.ThrowIfNull();
    private nuint? cachedAddress;

    public nuint? GetAddress()
    {
        if (this.cachedAddress.HasValue)
        {
            return this.cachedAddress.Value;
        }

        var addr = this.provider();
        if (addr is 0x0)
        {
            return null;
        }

        this.cachedAddress = addr;
        return addr;
    }
}
