using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.ApplicationLauncher;
using static Daybreak.Windows.Utils.NativeMethods;

namespace Daybreak.Windows.Services.ApplicationLauncher;

/// <summary>
/// Windows-specific Guild Wars process finder.
/// Uses Process.GetProcessesByName and Win32 APIs for elevated process path matching.
/// </summary>
public sealed class GuildWarsProcessFinder : IGuildWarsProcessFinder
{
    private const string ProcessName = "gw";

    public Memory<Process> GetGuildWarsProcesses()
    {
        return Process.GetProcessesByName(ProcessName);
    }

    public GuildWarsApplicationLaunchContext? FindProcess(
        LaunchConfigurationWithCredentials configuration
    )
    {
        return this.FindProcesses(configuration).FirstOrDefault();
    }

    public IEnumerable<GuildWarsApplicationLaunchContext?> FindProcesses(
        params LaunchConfigurationWithCredentials[] configurations
    )
    {
        return Process
            .GetProcessesByName(ProcessName)
            .Select(process =>
            {
                (var AssociatedConfiguration, _, var ProcessId) = configurations
                    .Select(c =>
                        (c, ConfigurationMatchesProcess(c, process, out var processId), processId)
                    )
                    .FirstOrDefault(c => c.Item2);
                return (Process: process, AssociatedConfiguration, ProcessId);
            })
            .Where(tuple => tuple.AssociatedConfiguration is not null)
            .Select(tuple => new GuildWarsApplicationLaunchContext
            {
                GuildWarsProcess = tuple.Process,
                LaunchConfiguration = tuple.AssociatedConfiguration,
                ProcessId = tuple.ProcessId,
            });
    }

    private static bool ConfigurationMatchesProcess(
        LaunchConfigurationWithCredentials launchConfigurationWithCredentials,
        Process process,
        out uint processId
    )
    {
        try
        {
            processId = (uint)process.Id;
            return launchConfigurationWithCredentials.ExecutablePath
                == process.MainModule?.FileName;
        }
        catch (Win32Exception ex)
            when (ex.Message.Contains("Access is denied")
                || ex.Message.Contains(
                    "Only part of a ReadProcessMemory or WriteProcessMemory request was completed."
                )
            )
        {
            processId = 0;
            /*
             * The process is running elevated. There is no way to use the standard C# libraries
             * to figure out what is the path of the running process.
             * We have to resort to low-level Windows API to figure out the path of the elevated process.
             * We create a process snapshot and compare the paths
             */
            var hSnapshot = CreateToolhelp32Snapshot(0x00000002, 0); // TH32CS_SNAPPROCESS
            var pe32 = new ProcessEntry32 { dwSize = (uint)Marshal.SizeOf<ProcessEntry32>() };

            var nameBuffer = new StringBuilder(1024);
            if (Process32First(hSnapshot, ref pe32))
            {
                do
                {
                    var size = 1024U;
                    if (pe32.szExeFile == "Gw.exe")
                    {
                        var maybeDesiredProcessHandle = OpenProcess(
                            ProcessAccessFlags.QueryLimitedInformation,
                            false,
                            pe32.th32ProcessID
                        );
                        if (
                            QueryFullProcessImageName(
                                maybeDesiredProcessHandle,
                                0,
                                nameBuffer,
                                ref size
                            )
                            && nameBuffer.ToString()
                                == launchConfigurationWithCredentials.ExecutablePath
                        )
                        {
                            processId = pe32.th32ProcessID;
                            CloseHandle(maybeDesiredProcessHandle);
                            return true;
                        }

                        CloseHandle(maybeDesiredProcessHandle);
                    }
                } while (Process32Next(hSnapshot, ref pe32));
            }

            CloseHandle(hSnapshot);
            return false;
        }
    }
}
