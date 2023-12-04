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
    private readonly ILogger<ProcessInjector> logger;
    
    public ProcessInjector(
        ILogger<ProcessInjector> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public async Task<bool> Inject(Process process, string pathToDll, CancellationToken cancellationToken)
    {
        return await this.InjectWithWinApi(process, pathToDll, cancellationToken);
    }

    private async Task<bool> InjectWithWinApi(Process process, string pathToDll, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(InjectWithWinApi), pathToDll);
        if (!File.Exists(pathToDll))
        {
            scopedLogger.LogError("File does not exist");
            return false;
        }

        if (process is null)
        {
            scopedLogger.LogError("Provided process is null");
            return false;
        }

        var injectionProcess = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                FileName = Path.GetFullPath("Daybreak.Injector.exe"),
                UseShellExecute = false,
                Arguments = $"-p {process.Id} -f {pathToDll}"
            }
        };

        injectionProcess.Start();
        await injectionProcess.WaitForExitAsync(cancellationToken);
        var output = await injectionProcess.StandardOutput.ReadToEndAsync(cancellationToken);
        var error = await injectionProcess.StandardError.ReadToEndAsync(cancellationToken);
        if (injectionProcess.ExitCode != 0)
        {
            scopedLogger.LogError($"Failed to inject\nOutput: {output}\nError: {error}");
            return false;
        }

        scopedLogger.LogInformation("Injected");
        return true;
    }
}
