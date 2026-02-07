using Daybreak.Shared.Models;
using System.Runtime.InteropServices;

namespace Daybreak.Injector;

public static class ThreadResumer
{
    public static InjectorResponses.ResumeResult Resume(IntPtr threadId)
    {
        if (threadId == 0)
        {
            return InjectorResponses.ResumeResult.InvalidThreadHandle;
        }

        var threadHandle = NativeMethods.OpenThread(NativeMethods.ThreadAccess.SuspendResume, false, (uint)threadId);
        if (threadHandle == IntPtr.Zero)
        {
            var openErr = Marshal.GetLastWin32Error();
            Console.WriteLine($"OpenThread failed. Error: {openErr}");
            return InjectorResponses.ResumeResult.InvalidThreadHandle;
        }

        var result = NativeMethods.ResumeThread(threadHandle);
        Console.WriteLine($"ResumeThread result: {result}");
        NativeMethods.CloseHandle(threadHandle);

        if (result == uint.MaxValue)
        {
            var resumeErr = Marshal.GetLastWin32Error();
            Console.WriteLine($"ResumeThread failed. Error: {resumeErr}");
            return InjectorResponses.ResumeResult.InvalidThreadHandle;
        }

        return InjectorResponses.ResumeResult.Success;
    }
}
