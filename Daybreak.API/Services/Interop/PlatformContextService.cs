using System.Text;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class PlatformContextService
{
    private const string GuildWarsWindowClassName = "ArenaNet_Dx_Window_Class";

    private nint cachedHwnd = 0;

    public nint GetWindowHandle()
    {
        if (this.cachedHwnd == 0)
        {
            NativeMethods.EnumWindows(this.EnumWindowsCallback, 0);
        }

        return this.cachedHwnd;
    }

    private bool EnumWindowsCallback(nint hwnd, nint lParam)
    {
        NativeMethods.GetWindowThreadProcessId(hwnd, out var processId);

        if (processId == NativeMethods.GetCurrentProcessId())
        {
            var classNameBuffer = stackalloc byte[256];
            NativeMethods.GetClassNameA(hwnd, classNameBuffer, 256);
            var className = Encoding.UTF8.GetString(classNameBuffer, GetNullTerminatedLength(classNameBuffer, 256));

            if (className == GuildWarsWindowClassName)
            {
                this.cachedHwnd = hwnd;
                return false;
            }
        }

        return true;
    }

    private static int GetNullTerminatedLength(byte* buffer, int maxLength)
    {
        for (var i = 0; i < maxLength; i++)
        {
            if (buffer[i] == 0)
            {
                return i;
            }
        }

        return maxLength;
    }
}
