using Daybreak.Shared.Services.Shell;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Daybreak.Linux.Services.Shell;

/// <summary>
/// Linux implementation of <see cref="IShellExecutor"/>.
/// Both URLs and file-system paths are delegated to <c>xdg-open</c>, which routes to the
/// user's default browser or file manager as appropriate.
/// </summary>
internal sealed class ShellExecutor(
    ILogger<ShellExecutor> logger) : IShellExecutor
{
    private const string XdgOpenExecutable = "xdg-open";

    private readonly ILogger<ShellExecutor> logger = logger;

    public void OpenUrl(string url) => this.Open(url);

    public void OpenPath(string path) => this.Open(path);

    private void Open(string target)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            return;
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = XdgOpenExecutable,
                UseShellExecute = false,
            };
            startInfo.ArgumentList.Add(target);
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to open target {target}", target);
        }
    }
}
