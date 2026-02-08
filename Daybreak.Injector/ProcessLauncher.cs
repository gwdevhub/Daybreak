using Daybreak.Shared.Models;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Daybreak.Injector.NativeMethods;

namespace Daybreak.Injector;

public sealed class ProcessLauncher
{
    public static InjectorResponses.LaunchResult LaunchGuildWars(
        string path,
        string args,
        bool elevated,
        out int threadId,
        out int processId)
    {
        var sw = Stopwatch.StartNew();
        Process? process;
        processId = LaunchClient(path, string.Join(" ", args), elevated, out threadId);
        if (processId is 0)
        {
            Console.WriteLine("Failed to launch GuildWars process.");
            return InjectorResponses.LaunchResult.LaunchFailed;
        }

        do
        {
            if (sw.Elapsed.TotalSeconds > 10)
            {
                Console.WriteLine("Timed out waiting for GuildWars process to start.");
                return InjectorResponses.LaunchResult.LaunchTimeout;
            }

            process = Process.GetProcessById(processId);
            Thread.Sleep(100);
        } while (process is null);

        if (!McPatch(process.Handle))
        {
            var lastErr = Marshal.GetLastWin32Error();
            Console.WriteLine($"Failed to patch GuildWars process. Error code: {lastErr}");
            return InjectorResponses.LaunchResult.PatchFailed;
        }

        return InjectorResponses.LaunchResult.Success;
    }

    private static int LaunchClient(string path, string args, bool elevated, out int threadId)
    {
        var commandLine = $"\"{path}\" {args}";
        threadId = 0;

        ProcessInformation procinfo;
        var startinfo = new StartupInfo
        {
            cb = Marshal.SizeOf<StartupInfo>()
        };
        var saProcess = new SecurityAttributes();
        saProcess.nLength = (uint)Marshal.SizeOf(saProcess);
        var saThread = new SecurityAttributes();
        saThread.nLength = (uint)Marshal.SizeOf(saThread);

        var lastDirectory = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(Path.GetDirectoryName(path)!);

        SetRegistryValue(@"Software\ArenaNet\Guild Wars", "Path", path);
        SetRegistryValue(@"Software\ArenaNet\Guild Wars", "Src", path);

        if (!elevated)
        {
            if (!SaferCreateLevel(SaferLevelScope.User, SaferLevel.NormalUser, SaferOpen.Open, out var hLevel, IntPtr.Zero))
            {
                Debug.WriteLine("SaferCreateLevel");
                return 0;
            }

            if (!SaferComputeTokenFromLevel(hLevel, IntPtr.Zero, out var hRestrictedToken, 0, IntPtr.Zero))
            {
                Debug.WriteLine("SaferComputeTokenFromLevel");
                return 0;
            }

            SaferCloseLevel(hLevel);

            TokenMandatoryLabel tml;
            tml.Label.Attributes = 0x20; // SE_GROUP_INTEGRITY
            if (!ConvertStringSidToSid("S-1-16-8192", out tml.Label.Sid))
            {
                CloseHandle(hRestrictedToken);
                Debug.WriteLine("ConvertStringSidToSid");
            }

            if (!SetTokenInformation(
                    hRestrictedToken,
                    TokenInformationClass.TokenIntegrityLevel,
                    ref tml,
                    (uint)Marshal.SizeOf(tml) + GetLengthSid(tml.Label.Sid)))
            {
                LocalFree(tml.Label.Sid);
                CloseHandle(hRestrictedToken);
                return 0;
            }

            LocalFree(tml.Label.Sid);
            if (!CreateProcessAsUser(
                    hRestrictedToken,
                    null!,
                    commandLine,
                    ref saProcess,
                    ref saProcess,
                    false,
                    (uint)CreationFlags.CreateSuspended,
                    IntPtr.Zero,
                    null!,
                    ref startinfo,
                    out procinfo))
            {
                var error = Marshal.GetLastWin32Error();
                Debug.WriteLine($"CreateProcessAsUser {error}");
                CloseHandle(procinfo.hThread);
                return 0;
            }

            CloseHandle(hRestrictedToken);
        }
        else
        {
            if (!CreateProcess(
                    null!,
                    commandLine,
                    ref saProcess,
                    ref saThread,
                    false,
                    (uint)CreationFlags.CreateSuspended,
                    IntPtr.Zero,
                    null!,
                    ref startinfo,
                    out procinfo))
            {
                var error = Marshal.GetLastWin32Error();
                Debug.WriteLine($"CreateProcess {error}");
                _ = ResumeThread(procinfo.hThread);
                CloseHandle(procinfo.hThread);
                return 0;
            }
        }

        Directory.SetCurrentDirectory(lastDirectory);

        threadId = procinfo.dwThreadId;
        CloseHandle(procinfo.hThread);
        CloseHandle(procinfo.hProcess);
        return procinfo.dwProcessId;
    }

    /// <summary>
    /// https://github.com/GregLando113/gwlauncher/blob/master/GW%20Launcher/MulticlientPatch.cs
    /// </summary>
    private static bool McPatch(IntPtr processHandle)
    {
        byte[] sigPatch =
        [
            0x56, 0x57, 0x68, 0x00, 0x01, 0x00, 0x00, 0x89, 0x85, 0xF4, 0xFE, 0xFF, 0xFF, 0xC7, 0x00, 0x00, 0x00, 0x00,
            0x00
        ];
        var moduleBase = GetProcessModuleBase(processHandle);
        var gwdata = new byte[0x48D000];

        if (!NativeMethods.ReadProcessMemory(processHandle, moduleBase, gwdata, gwdata.Length, out _))
        {
            return false;
        }

        var idx = SearchBytes(gwdata, sigPatch);

        if (idx == -1)
        {
            return false;
        }

        var mcpatch = moduleBase + idx - 0x1A;

        byte[] payload = [0x31, 0xC0, 0x90, 0xC3];

        return NativeMethods.WriteProcessMemory(processHandle, mcpatch, payload, payload.Length, out _);
    }

    /// <summary>
    /// https://github.com/GregLando113/gwlauncher/blob/master/GW%20Launcher/MulticlientPatch.cs
    /// </summary>
    private static int SearchBytes(Memory<byte> haystack, Memory<byte> needle)
    {
        var len = needle.Length;
        var limit = haystack.Length - len;
        for (var i = 0; i <= limit; i++)
        {
            var k = 0;
            for (; k < len; k++)
            {
                if (needle.Span[k] != haystack.Span[i + k])
                {
                    break;
                }
            }

            if (k == len)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// https://github.com/GregLando113/gwlauncher/blob/master/GW%20Launcher/MulticlientPatch.cs
    /// </summary>
    private static IntPtr GetProcessModuleBase(IntPtr process)
    {
        if (NativeMethods.NtQueryInformationProcess(process, NativeMethods.ProcessInfoClass.ProcessBasicInformation, out var pbi,
                Marshal.SizeOf<ProcessBasicInformation>(), out _) != 0)
        {
            return IntPtr.Zero;
        }

        var buffer = new byte[Marshal.SizeOf<PEB>()];

        if (!NativeMethods.ReadProcessMemory(process, pbi.PebBaseAddress, buffer, Marshal.SizeOf<PEB>(), out _))
        {
            return IntPtr.Zero;
        }

        PEB peb = new()
        {
            ImageBaseAddress = (IntPtr)BitConverter.ToInt32(buffer, 8)
        };

        return peb.ImageBaseAddress + 0x1000;
    }

    private static void SetRegistryValue(string registryPath, string valueName, object newValue)
    {
        using var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(registryPath);
        key.SetValue(valueName, newValue, RegistryValueKind.String);
    }
}
