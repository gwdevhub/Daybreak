using Daybreak.Shared.Services.Shell;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Daybreak.Windows.Services.Shell;

/// <summary>
/// Windows implementation of <see cref="IShellExecutor"/>.
/// URLs are opened via shell execution (default browser); paths are opened via <c>explorer.exe</c>.
/// </summary>
internal sealed class ShellExecutor(
    ILogger<ShellExecutor> logger) : IShellExecutor
{
    private const string ExplorerExecutable = "explorer.exe";

    private readonly ILogger<ShellExecutor> logger = logger;

    public void OpenUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to open url {url}", url);
        }
    }

    public void OpenPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        try
        {
            Process.Start(ExplorerExecutable, path);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to open path {path}", path);
        }
    }
}
