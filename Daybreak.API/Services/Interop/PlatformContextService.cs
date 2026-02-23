using Daybreak.API.Interop;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class PlatformContextService
{
    public nint GetWindowHandle()
    {
        var hwnd = GWCA.GW.MemoryMgr.GetGWWindowHandle();
        // HWND is defined as HWND__* in Windows - the pointer value itself IS the handle,
        // not a pointer to a struct containing the handle. Cast directly to nint.
        return (nint)hwnd;
    }
}
