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

Console.WriteLine("Unpacking files...");
try
{
    ZipFile.ExtractToDirectory(tempFile, AppContext.BaseDirectory, true);
}
catch
{

}
Console.WriteLine("Deleting package");
try
{
    File.Delete(tempFile);
}
catch(Exception e)
{
    Console.WriteLine($"Failed to delete {tempFile}.\n{e}");
}

Console.WriteLine("Deleting browser caches");
try
{
    Directory.Delete("BrowserData", true);
}
catch(Exception e)
{
    Console.WriteLine($"Failed to delete BrowserData.\n{e}");
}
try
{
    Directory.Delete("Daybreak.exe.WebView2", true);
}
catch(Exception e)
{
    Console.WriteLine($"Failed to delete Daybreak.exe.WebView2.\n{e}");
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
