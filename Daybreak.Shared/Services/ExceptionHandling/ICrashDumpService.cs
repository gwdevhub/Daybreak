namespace Daybreak.Shared.Services.ExceptionHandling;

/// <summary>
/// Provides platform-specific crash dump writing capabilities.
/// </summary>
public interface ICrashDumpService
{
    /// <summary>
    /// Writes a crash dump file at the specified path.
    /// </summary>
    void WriteCrashDump(string dumpFilePath);
}
