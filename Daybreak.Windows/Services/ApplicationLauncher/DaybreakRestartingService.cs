using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.Privilege;
using Microsoft.Extensions.Logging;
using Photino.NET;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Windows.Services.ApplicationLauncher;

/// <summary>
/// Windows-specific implementation for restarting the Daybreak application.
/// Uses cmd.exe with 'runas' verb for admin elevation.
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
        this.logger.LogInformation("Restarting daybreak with admin rights");
        var processName = Process.GetCurrentProcess()?.MainModule?.FileName;
        if (processName!.IsNullOrWhiteSpace() || File.Exists(processName) is false)
        {
            throw new InvalidOperationException("Unable to find executable. Aborting restart");
        }

        var process = new Process()
        {
            StartInfo = new()
            {
                Verb = "runas",
                WorkingDirectory = Environment.CurrentDirectory,
                UseShellExecute = true,
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments =
                    $"/c cd /d \"{Environment.CurrentDirectory}\" && timeout /t 1 /nobreak && start \"\" \"{processName}\" && exit",
            },
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to start {processName} as admin");
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

        var process = new Process()
        {
            StartInfo = new()
            {
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = processName,
                UseShellExecute = true,
                CreateNoWindow = true,
            },
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to start {processName} as normal user");
        }

        this.photinoWindow.Close();
    }
}
