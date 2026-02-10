using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.Privilege;
using Microsoft.Extensions.Logging;
using Photino.NET;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Linux.Services.ApplicationLauncher;

/// <summary>
/// Linux-specific implementation for restarting the Daybreak application.
/// Uses bash for process spawning and pkexec/sudo for privilege elevation.
/// </summary>
internal sealed class DaybreakRestartingService(
    PhotinoWindow photinoWindow,
    IPrivilegeManager privilegeManager,
    ILogger<DaybreakRestartingService> logger
) : IDaybreakRestartingService
{
    private readonly PhotinoWindow photinoWindow = photinoWindow.ThrowIfNull();
    private readonly IPrivilegeManager privilegeManager = privilegeManager.ThrowIfNull();
    private readonly ILogger<DaybreakRestartingService> logger = logger.ThrowIfNull();

    public void RestartDaybreak()
    {
        if (this.privilegeManager.AdminPrivileges)
        {
            this.RestartDaybreakAsAdmin();
        }
        else
        {
            this.RestartDaybreakAsNormalUser();
        }
    }

    public void RestartDaybreakAsAdmin()
    {
        this.logger.LogInformation("Restarting daybreak with elevated privileges");
        var processName = Process.GetCurrentProcess()?.MainModule?.FileName;
        if (processName!.IsNullOrWhiteSpace() || File.Exists(processName) is false)
        {
            throw new InvalidOperationException("Unable to find executable. Aborting restart");
        }

        // On Linux, use pkexec for graphical privilege elevation
        // Fall back to sudo if pkexec is not available
        var elevationCommand = GetElevationCommand();
        if (elevationCommand is null)
        {
            this.logger.LogWarning("No elevation command (pkexec/sudo) found. Restarting without elevation");
            this.RestartDaybreakAsNormalUser();
            return;
        }

        var process = new Process()
        {
            StartInfo = new()
            {
                WorkingDirectory = Environment.CurrentDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = "/bin/bash",
                Arguments = $"-c \"sleep 1 && {elevationCommand} \\\"{processName}\\\" &\"",
            },
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to start {processName} with elevated privileges");
        }

        this.photinoWindow.Close();
    }

    public void RestartDaybreakAsNormalUser()
    {
        this.logger.LogInformation("Restarting daybreak as normal user");
        var processName = Process.GetCurrentProcess()?.MainModule?.FileName;
        if (processName!.IsNullOrWhiteSpace() || File.Exists(processName) is false)
        {
            throw new InvalidOperationException("Unable to find executable. Aborting restart");
        }

        // Use nohup to ensure the process survives parent exit, and disown to detach from terminal
        var process = new Process()
        {
            StartInfo = new()
            {
                WorkingDirectory = Environment.CurrentDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = "/bin/bash",
                Arguments = $"-c \"sleep 1 && nohup \\\"{processName}\\\" > /dev/null 2>&1 &\"",
            },
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to start {processName} as normal user");
        }

        this.photinoWindow.Close();
    }

    /// <summary>
    /// Gets the appropriate elevation command for the system.
    /// Prefers pkexec (graphical) over sudo (terminal-based).
    /// </summary>
    private static string? GetElevationCommand()
    {
        // Check for pkexec first (graphical polkit elevation)
        if (CommandExists("pkexec"))
        {
            return "pkexec";
        }

        // Fall back to sudo
        if (CommandExists("sudo"))
        {
            return "sudo";
        }

        return null;
    }

    /// <summary>
    /// Checks if a command exists in the system PATH.
    /// </summary>
    private static bool CommandExists(string command)
    {
        try
        {
            var process = new Process()
            {
                StartInfo = new()
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"command -v {command}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                },
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output);
        }
        catch
        {
            return false;
        }
    }
}
