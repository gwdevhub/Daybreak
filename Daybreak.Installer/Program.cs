// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.IO.Compression;

const string tempFile = "tempfile.zip";
const string executableName = "Daybreak.exe";
Console.Title = "Daybreak Installer";
Console.WriteLine("Starting installation...");
if (File.Exists(tempFile) is false)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Unable to find launcher package. Aborting installation");
    Console.ReadKey();
    return;
}

while (Process.GetProcesses().Where(p => p.ProcessName == "Daybreak").Any())
{
    Console.WriteLine($"Detected Daybreak process is still running. Waiting 5s and retrying");
    await Task.Delay(5000);
}

Console.WriteLine("Unpacking files...");
try
{
    ZipFile.ExtractToDirectory(tempFile, AppContext.BaseDirectory, true);
}
catch
{

}

Console.WriteLine("Deleting browser caches");
try
{
    Directory.Delete("BrowserData", true);
}
catch(Exception)
{
}
try
{
    Directory.Delete("Daybreak.exe.WebView2", true);
}
catch(Exception)
{
}

Console.WriteLine("Deleting package");
try
{
    File.Delete(tempFile);
}
catch (Exception e)
{
    Console.WriteLine($"Failed to delete {tempFile}.\n{e}");
}

Console.WriteLine("Launching application");
var process = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = executableName
    }
};

if (process.Start() is false)
{
    Console.WriteLine("Failed to launch application");
    Console.ReadKey();
}
