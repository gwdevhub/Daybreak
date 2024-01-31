using Daybreak.Injector;
using System.Diagnostics;

var processId = -1;
var pathToDll = string.Empty;

if (args.Length != 4)
{
    Console.WriteLine("Daybreak Injector");
    Console.WriteLine("Usage:");
    Console.WriteLine("Daybreak.Injector.exe -p [PROCESS_ID] -d [PATH_TO_DLL]");
    return -1;
}

for (var i = 0; i < args.Length - 1; i++)
{
    if (args[i].ToLower() == "-p" &&
        int.TryParse(args[i + 1], out var parsedId))
    {
        processId = parsedId;
        i++;
    }
    else if (args[i].ToLower() == "-d")
    {
        pathToDll = args[i + 1];
        i++;
    }
}

if (processId == -1)
{
    Console.WriteLine("Error: Process id was not specified");
    return -1;
}

if (pathToDll == string.Empty)
{
    Console.WriteLine("Error: Path to dll was not specified");
    return -1;
}

if (!File.Exists(pathToDll))
{
    Console.WriteLine("Error: Provided dll could not be found");
    return -1;
}

var maybeProcess = Process.GetProcessById(processId);
if (maybeProcess is null)
{
    Console.WriteLine("Error: Could not find desired process");
    return -1;
}

var result = await ProcessInjector.Inject(maybeProcess, pathToDll, CancellationToken.None);
if (result is false)
{
    Console.WriteLine("Error: Failed to inject dll");
    return -1;
}

Console.WriteLine("Injected dll");
return 0;
