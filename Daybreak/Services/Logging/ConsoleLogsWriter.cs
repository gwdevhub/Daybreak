using Daybreak.Shared.Services.Logging;
using Microsoft.Extensions.Logging;
using System.Logging;

namespace Daybreak.Services.Logging;

internal sealed class ConsoleLogsWriter : IConsoleLogsWriter
{
    public void WriteLog(Log log)
    {
        var colorCode = GetAnsiColorForLogLevel(log.LogLevel);
        Console.WriteLine(
            $"[{log.LogTime}]\t[\u001b[{colorCode}m{log.LogLevel}\u001b[0m]\t[{log.Category}]\n{log.Message}");
    }

    private static string GetAnsiColorForLogLevel(LogLevel level) => level switch
    {
        LogLevel.Trace => "90",      // Bright Black
        LogLevel.Debug => "36",      // Cyan
        LogLevel.Information => "32",      // Green
        LogLevel.Warning => "33",      // Yellow
        LogLevel.Error => "31",      // Red
        LogLevel.Critical => "30;41",   // Black on Red
        _ => "37"       // White
    };
}
