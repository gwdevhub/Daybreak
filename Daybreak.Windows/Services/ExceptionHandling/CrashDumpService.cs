using Daybreak.Shared.Services.ExceptionHandling;
using Daybreak.Windows.Utils;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace Daybreak.Windows.Services.ExceptionHandling;

[SupportedOSPlatform("windows")]
internal sealed class CrashDumpService : ICrashDumpService
{
    public void WriteCrashDump(string dumpFilePath)
    {
        using var fs = new FileStream(dumpFilePath, FileMode.Create, FileAccess.Write);
        var process = Process.GetCurrentProcess();
        NativeMethods.MiniDumpWriteDump(
            process.Handle,
            process.Id,
            fs.SafeFileHandle,
            NativeMethods.MinidumpType.MiniDumpWithFullMemory,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);
    }
}
