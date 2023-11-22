// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.IO.Compression;
using System.Text;

static void RenderProgressBar(int currentStep, int totalSteps, int barSize)
{
    Console.CursorLeft = 0; // Reset cursor position to the start of the line
    Console.Write("["); // Start of the progress bar

    double pctComplete = (double)currentStep / totalSteps;
    int chars = (int)Math.Round(pctComplete * barSize);

    Console.Write(new string('=', chars)); // The 'completed' part of the bar
    Console.Write(new string(' ', barSize - chars)); // The 'remaining' part of the bar

    Console.Write("] ");
    Console.Write($"{pctComplete:P0}"); // Display the percentage completed
}

const string tempFile = "tempfile.zip";
const string updatePkg = "update.pkg";
const string executableName = "Daybreak.exe";
Console.Title = "Daybreak Installer";
Console.WriteLine("Starting installation...");
while (Process.GetProcesses().Where(p => p.ProcessName == "Daybreak").FirstOrDefault()?.HasExited is false)
{
    Console.WriteLine($"Detected Daybreak process is still running. Waiting 5s and retrying");
    await Task.Delay(5000);
}

if (File.Exists(updatePkg))
{
    Console.WriteLine("Unpacking files...");

    using var fileStream = new FileStream(updatePkg, FileMode.Open);
    var sizeBuffer = new byte[4];
    var copyBuffer = new byte[1024];
    while (fileStream.Position < fileStream.Length - 1)
    {
        /*
         * For each file downloaded, we write to the package the following:
         * Size of the name string
         * UTF-8 encoded name string
         * Size of the relative path string
         * UTF-8 encoded relative path string
         * Size of the binary
         * Binary data
         */

        await fileStream.ReadAsync(sizeBuffer, 0, 4);
        var fileNameSize = BitConverter.ToInt32(sizeBuffer, 0);
        var fileNameBuffer = new byte[fileNameSize];
        await fileStream.ReadAsync(fileNameBuffer, 0, fileNameSize);
        var fileName = Encoding.UTF8.GetString(fileNameBuffer);

        RenderProgressBar((int)fileStream.Position, (int)fileStream.Length, 40);

        await fileStream.ReadAsync(sizeBuffer, 0, 4);
        var relativePathSize = BitConverter.ToInt32(sizeBuffer, 0);
        var relativePathBuffer = new byte[relativePathSize];
        await fileStream.ReadAsync(relativePathBuffer, 0, relativePathSize);
        var relativePath = Encoding.UTF8.GetString(relativePathBuffer);

        RenderProgressBar((int)fileStream.Position, (int)fileStream.Length, 40);

        await fileStream.ReadAsync(sizeBuffer, 0, 4);
        var binarySize = BitConverter.ToInt32(sizeBuffer, 0);
        var fileInfo = new FileInfo(relativePath);
        fileInfo.Directory!.Create();
        using var destinationStream = new FileStream(relativePath, FileMode.Create);
        while(binarySize > 0)
        {
            var toRead = Math.Min(binarySize, copyBuffer.Length);
            await fileStream.ReadAsync(copyBuffer, 0, toRead);
            await destinationStream.WriteAsync(copyBuffer, 0, toRead);
            binarySize -= toRead;

            RenderProgressBar((int)fileStream.Position, (int)fileStream.Length, 40);
        }
    }

    fileStream.Close();
    fileStream.Dispose();
    File.Delete(updatePkg);
}
else if (File.Exists(tempFile))
{
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
    catch (Exception e)
    {
        Console.WriteLine($"Failed to delete {tempFile}.\n{e}");
    }
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Unable to find launcher package. Aborting installation");
    Console.ReadKey();
    return;
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
