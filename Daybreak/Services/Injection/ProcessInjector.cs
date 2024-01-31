using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Injection;
internal sealed class ProcessInjector : IProcessInjector
{
    private const string InjectorExe = "Daybreak.Injector.exe";

    private readonly ILogger<ProcessInjector> logger;
    
    public ProcessInjector(
        ILogger<ProcessInjector> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public Task<bool> Inject(Process process, string pathToDll, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() => this.InjectWithInjector(process, pathToDll), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }
    
    private bool InjectWithInjector(Process process, string pathToDll)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(InjectWithInjector), pathToDll);
        var modulefullpath = Path.GetFullPath(pathToDll);
        if (!File.Exists(modulefullpath))
        {
            scopedLogger.LogError("Dll to inject not found");
            return false;
        }

        var injectorPath = Path.GetFullPath(InjectorExe);
        if (!File.Exists(injectorPath))
        {
            scopedLogger.LogError("Could not find Daybreak injector");
            return false;
        }

        var injectionProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                Arguments = $"-p {process.Id} -d {pathToDll}",
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                FileName = injectorPath,
            }
        };

        injectionProcess.Start();
        injectionProcess.WaitForExit();
        var result = injectionProcess.ExitCode;
        if (result == 0)
        {
            scopedLogger.LogInformation("Injected into process");
            return true;
        }

        var stdout = injectionProcess.StandardOutput.ReadToEnd();
        var stderr = injectionProcess.StandardError.ReadToEnd();
        scopedLogger.LogError($"Failed to inject. Details:\n{stdout}\n{stderr}");
        return false;
    }
}
