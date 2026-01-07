using Daybreak.Injector;
using Daybreak.Shared.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

static void PrintUsage()
{
    Console.WriteLine("======================================================");
    Console.WriteLine("Usage: Daybreak.Injector [mode] <params>");
    Console.WriteLine("Modes: ");
    Console.WriteLine("- winapi");
    Console.WriteLine("- stub");
    Console.WriteLine("- launch");
    Console.WriteLine("- resume");
    Console.WriteLine("Examples:");
    Console.WriteLine("1) Daybreak.Injector winapi 1234 C:\\path\\to\\dll.dll");
    Console.WriteLine("2) Daybreak.Injector stub 1234 C:\\path\\to\\dll.dll");
    Console.WriteLine("3) Daybreak.Injector launch true C:\\path\\to\\dll.dll arg1 arg2 arg3");
    Console.WriteLine("4) Daybreak.Injector resume 1234");
    Console.WriteLine("======================================================");
}

static bool TryParseInjectArgs(
    string[] args,
    [NotNullWhen(true)] out Process? process,
    [NotNullWhen(true)] out string? dllPath,
    out InjectorResponses.InjectResult exitCode)
{
    process = default;
    dllPath = default;
    if (!int.TryParse(args[1], out var processId))
    {
        PrintUsage();
        exitCode = InjectorResponses.InjectResult.InvalidProcess;
        return false;
    }

    if (args.Length < 3)
    {
        PrintUsage();
        exitCode = InjectorResponses.InjectResult.InvalidArgs;
        return false;
    }

    dllPath = Path.GetFullPath(args[2]);
    process = Process.GetProcessById(processId);
    if (process is null)
    {
        Console.WriteLine($"Process {processId} could not be found");
        exitCode = InjectorResponses.InjectResult.InvalidProcess;
        return false;
    }


    if (!File.Exists(dllPath))
    {
        Console.WriteLine($"DLL path {dllPath} could not be found");
        exitCode = InjectorResponses.InjectResult.InvalidDllPath;
        return false;
    }

    exitCode = 0;
    return true;
}

static bool TryParseLaunchArgs(
    string[] args,
    [NotNullWhen(true)] out string? gwPath,
    [NotNullWhen(true)] out string? gwArgs,
    [NotNullWhen(true)] out bool? elevated,
    out InjectorResponses.LaunchResult exitCode)
{
    gwPath = default;
    gwArgs = default;
    elevated = default;
    if (args.Length < 3)
    {
        PrintUsage();
        exitCode = InjectorResponses.LaunchResult.InvalidArgs;
        return false;
    }

    gwPath = Path.GetFullPath(args[2]);
    if (!bool.TryParse(args[1], out var parsedElevated))
    {
        PrintUsage();
        exitCode = InjectorResponses.LaunchResult.InvalidElevated;
        return false;
    }

    elevated = parsedElevated;
    gwArgs = args.Length > 3 ? string.Join(" ", args.Skip(3)) : string.Empty;

    if (!File.Exists(gwPath))
    {
        Console.WriteLine($"Guild Wars path {gwPath} could not be found");
        exitCode = InjectorResponses.LaunchResult.InvalidPath;
        return false;
    }

    exitCode = 0;
    return true;
}

static bool TryParseThreadResumeArgs(
    string[] args,
    [NotNullWhen(true)] out IntPtr? threadHwnd,
    out InjectorResponses.ResumeResult exitCode)
{
    threadHwnd = default;
    if (args.Length < 2)
    {
        PrintUsage();
        exitCode = InjectorResponses.ResumeResult.InvalidArgs;
        return false;
    }

    if (!IntPtr.TryParse(args[1], out var threadInt))
    {
        PrintUsage();
        exitCode = InjectorResponses.ResumeResult.InvalidThreadHandle;
        return false;
    }

    threadHwnd = threadInt;
    exitCode = InjectorResponses.ResumeResult.Success;
    return true;
}

if (args.Length < 1)
{
    PrintUsage();
    return (int)InjectorResponses.GenericResults.InvalidArgs;
}

var mode = args[0];

switch (mode)
{
    case "winapi":
        {
            if (!TryParseInjectArgs(args, out var process, out var dllPath, out var parseResult))
            {
                return (int)parseResult;
            }

            Console.WriteLine($"Starting WinAPI injection. Process {process.Id}. DllPath: {dllPath}");
            return (int)ProcessInjector.InjectWithApi(process, dllPath);
        }
    case "stub":
        {
            if (!TryParseInjectArgs(args, out var process, out var dllPath, out var parseResult))
            {
                return (int)parseResult;
            }

            Console.WriteLine($"Starting stub injection. Process {process.Id}. DllPath: {dllPath}");
            var result = StubInjector.Inject(process, dllPath, out var exitCode);
            if (result is not InjectorResponses.InjectResult.Success)
            {
                return (int)result;
            }

            Console.WriteLine($"Stub returned {exitCode}");
            return exitCode;
        }
    case "launch":
        {
            if (!TryParseLaunchArgs(args, out var gwPath, out var gwArgs, out var elevated, out var parseResult))
            {
                return (int)parseResult;
            }

            Console.WriteLine($"Launching Guild Wars. Path {gwPath}. Elevated {elevated}. Args {gwArgs}");
            var result = ProcessLauncher.LaunchGuildWars(gwPath, gwArgs, elevated.Value, out var threadHandle, out var processId);
            Console.WriteLine($"ThreadHandle: {threadHandle}");
            Console.WriteLine($"ProcessId: {processId}");
            return (int)result;
        }
    case "resume":
        {
            if (!TryParseThreadResumeArgs(args, out var threadHwnd, out var parseResult))
            {
                return (int)parseResult;
            }

            Console.WriteLine($"Resuming thread {threadHwnd.Value}");
            return (int) ThreadResumer.Resume(threadHwnd.Value);
        }
    default:
        PrintUsage();
        return (int)InjectorResponses.GenericResults.InvalidMode;
}
