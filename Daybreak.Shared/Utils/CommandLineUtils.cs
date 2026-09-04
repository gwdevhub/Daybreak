namespace Daybreak.Shared.Utils;

/// <summary>
/// Helpers for working with Windows style command line strings.
/// </summary>
public static class CommandLineUtils
{
    /// <summary>
    /// Splits a Windows style command line into the argument vector it represents.
    /// </summary>
    /// <remarks>
    /// This mirrors the parser .NET applies to <see cref="System.Diagnostics.ProcessStartInfo.Arguments"/>
    /// when starting a process on Unix. Anything that needs to hand those arguments to another launcher
    /// must produce the same vector, otherwise the process observes a different command line depending on
    /// how it was started.
    /// </remarks>
    /// <param name="commandLine">The command line to split. May be null or empty.</param>
    /// <returns>The parsed arguments, with quoting and backslash escapes resolved.</returns>
    public static List<string> SplitCommandLine(string? commandLine)
    {
        var results = new List<string>();
        if (string.IsNullOrEmpty(commandLine))
        {
            return results;
        }

        var i = 0;
        while (i < commandLine.Length)
        {
            while (i < commandLine.Length && (commandLine[i] is ' ' or '\t'))
            {
                i++;
            }

            if (i == commandLine.Length)
            {
                break;
            }

            results.Add(ReadArgument(commandLine, ref i));
        }

        return results;
    }

    private static string ReadArgument(string commandLine, ref int i)
    {
        var argument = new System.Text.StringBuilder();
        var inQuotes = false;

        while (i < commandLine.Length)
        {
            var backslashCount = 0;
            while (i < commandLine.Length && commandLine[i] is '\\')
            {
                i++;
                backslashCount++;
            }

            if (backslashCount > 0)
            {
                if (i >= commandLine.Length || commandLine[i] is not '"')
                {
                    argument.Append('\\', backslashCount);
                }
                else
                {
                    // Every pair of backslashes produces one literal backslash. A remaining
                    // backslash escapes the quote that follows it.
                    argument.Append('\\', backslashCount / 2);
                    if (backslashCount % 2 != 0)
                    {
                        argument.Append('"');
                        i++;
                    }
                }

                continue;
            }

            var c = commandLine[i];
            if (c is '"')
            {
                if (inQuotes && i + 1 < commandLine.Length && commandLine[i + 1] is '"')
                {
                    argument.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }

                i++;
                continue;
            }

            if (!inQuotes && c is ' ' or '\t')
            {
                break;
            }

            argument.Append(c);
            i++;
        }

        return argument.ToString();
    }
}
