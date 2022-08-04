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
File.Delete(tempFile);
Console.WriteLine("Launching application");
Process.Start(executableName);