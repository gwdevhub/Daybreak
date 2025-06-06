using System.Runtime.InteropServices;

namespace Daybreak.API.Interop;

public sealed class GWDelegateCache<TDelegate>(GWAddressCache cache)
    where TDelegate : Delegate
{
    public GWAddressCache Cache { get; } = cache;

    public TDelegate? GetDelegate()
    {
        if (this.Cache.GetAddress() is not nuint address)
        {
            return default;
        }

        return Marshal.GetDelegateForFunctionPointer<TDelegate>((nint)address);
    }
}
