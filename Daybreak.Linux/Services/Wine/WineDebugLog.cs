using System.Diagnostics;
using Daybreak.Shared.Utils;

namespace Daybreak.Linux.Services.Wine;

/// <summary>
/// Debug-build support for capturing Wine diagnostics, most importantly the
/// <c>winedbg --auto</c> backtrace produced when Guild Wars crashes.
///
/// Guild Wars is launched by Daybreak.Injector.exe, so it is a *grandchild* of the
/// Wine process Daybreak starts and inherits that process's stderr. Daybreak stops
/// reading that pipe as soon as the injector prints its result, which means a later
/// crash backtrace is written into a pipe nobody drains: the output is lost, and if
/// the pipe buffer fills, the game blocks inside a stderr write.
///
/// Redirecting stderr to a file inside the shell makes the descriptor outlive
/// Daybreak's interest in the process, so every descendant (injector, Gw.exe,
/// winedbg) appends to a durable log instead.
/// </summary>
internal static class WineDebugLog
{
    private const string LogFileName = "wine-debug.log";
    private const string EnableVariable = "DAYBREAK_WINE_DEBUG";
    private const string ChannelsVariable = "DAYBREAK_WINE_DEBUG_CHANNELS";

    /// <summary>
    /// Enabled by default for Debug builds. Either build configuration can opt in or
    /// out explicitly with <c>DAYBREAK_WINE_DEBUG=1</c> / <c>=0</c>.
    /// </summary>
    public static bool IsEnabled =>
        Environment.GetEnvironmentVariable(EnableVariable) switch
        {
            "1" or "true" or "TRUE" => true,
            "0" or "false" or "FALSE" => false,
#if DEBUG
            _ => true,
#else
            _ => false,
#endif
        };

    public static string LogPath => PathUtils.GetAbsolutePathFromRoot(LogFileName);

    /// <summary>
    /// Extra WINEDEBUG channels. Left unset by default: Wine's default <c>err</c> class
    /// already reports unhandled exceptions and drives winedbg, while trace channels such
    /// as <c>+seh</c> or <c>+relay</c> slow the game to a crawl. Set
    /// <c>DAYBREAK_WINE_DEBUG_CHANNELS=+seh</c> when a specific investigation needs them.
    /// </summary>
    public static string? Channels =>
        Environment.GetEnvironmentVariable(ChannelsVariable) is { Length: > 0 } channels
            ? channels
            : null;

    /// <summary>
    /// Rewrites <paramref name="startInfo"/> to run the original command under
    /// <c>/bin/sh</c> with stderr appended to <see cref="LogPath"/>.
    /// </summary>
    /// <remarks>
    /// The command is split here and handed to the shell as separate positional parameters,
    /// then executed with <c>exec "$@"</c>. The shell must never re-parse the command as source:
    /// doing so applies a second round of expansion to values Daybreak has already quoted, so a
    /// launch argument containing <c>$</c>, a backtick or a glob would be rewritten before the
    /// game ever saw it. Splitting with <see cref="CommandLineUtils.SplitCommandLine"/> keeps the
    /// argument vector identical to the one .NET builds when the log is disabled.
    /// </remarks>
    public static void Apply(ProcessStartInfo startInfo)
    {
        var logPath = LogPath;
        var arguments = CommandLineUtils.SplitCommandLine(startInfo.Arguments);
        var fileName = startInfo.FileName;

        startInfo.FileName = "/bin/sh";
        startInfo.Arguments = string.Empty;
        startInfo.ArgumentList.Add("-c");
        startInfo.ArgumentList.Add("exec \"$@\" 2>>\"$0\"");
        startInfo.ArgumentList.Add(logPath);
        startInfo.ArgumentList.Add(fileName);
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        if (Channels is { } channels)
        {
            startInfo.Environment["WINEDEBUG"] = channels;
        }
    }

    public static void WriteSessionHeader(string description)
    {
        try
        {
            File.AppendAllText(
                LogPath,
                $"{Environment.NewLine}===== {DateTime.Now:yyyy-MM-dd HH:mm:ss} {description} ====={Environment.NewLine}");
        }
        catch
        {
            // Diagnostics only - never fail a launch because the log is unwritable.
        }
    }
}
