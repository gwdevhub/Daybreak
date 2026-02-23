using Daybreak.API.Interop;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class PlatformContextService
{
    public nint GetWindowHandle()
    {
        var hwnd = GWCA.GW.MemoryMgr.GetGWWindowHandle();
        if (hwnd is null)
        {
            return default;
        }

        return hwnd->Handle;
    }
}
