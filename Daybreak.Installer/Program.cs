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

Console.Title = "Daybreak Installer";
Console.WriteLine("Starting installation...");
while (Process.GetProcesses().Where(p => p.ProcessName == "Daybreak").FirstOrDefault()?.HasExited is false)
{
    Console.WriteLine($"Detected Daybreak process is still running. Waiting 5s and retrying");
    await Task.Delay(5000);
}

while (Process.GetProcessesByName("gw").FirstOrDefault()?.HasExited is false)
{
    Console.WriteLine($"Detected Guild Wars process is still running. Waiting 5s and retrying");
    await Task.Delay(5000);
}

if (args.Length < 1)
{
    Console.WriteLine("No working directory specified");
    Console.ReadKey();
    return;
}

var workingDirectory = args[0];
if (!Directory.Exists(workingDirectory))
{
    Console.WriteLine("Working directory does not exist");
    Console.ReadKey();
    return;
}

Console.WriteLine($"Working directory: {workingDirectory}");

var tempFile = Path.GetFullPath("tempfile.zip", workingDirectory);
var updatePkg = Path.GetFullPath("update.pkg", workingDirectory);
var executableName = Path.GetFullPath("Daybreak.exe", workingDirectory);

if (File.Exists(updatePkg))
{
    Console.WriteLine("Unpacking files...");

    using var fileStream = new FileStream(updatePkg, FileMode.Open);
    var copyBuffer = new Memory<byte>(new byte[4096]);
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

        var sizeBuffer = copyBuffer[..4];
        await fileStream.ReadExactlyAsync(sizeBuffer);
        var fileNameSize = BitConverter.ToInt32(sizeBuffer.Span);
        var fileNameBuffer = copyBuffer[..fileNameSize];
        await fileStream.ReadExactlyAsync(fileNameBuffer);
        var fileName = Encoding.UTF8.GetString(fileNameBuffer.Span);

        RenderProgressBar((int)fileStream.Position, (int)fileStream.Length, 40);

        await fileStream.ReadExactlyAsync(sizeBuffer);
        var relativePathSize = BitConverter.ToInt32(sizeBuffer.Span);
        var relativePathBuffer = copyBuffer[..relativePathSize];
        await fileStream.ReadExactlyAsync(relativePathBuffer);
        var relativePath = Encoding.UTF8.GetString(relativePathBuffer.Span);

        RenderProgressBar((int)fileStream.Position, (int)fileStream.Length, 40);

        await fileStream.ReadExactlyAsync(sizeBuffer);
        var binarySize = BitConverter.ToInt32(sizeBuffer.Span);
        var fileInfo = new FileInfo(relativePath);
        fileInfo.Directory!.Create();
        using var destinationStream = new FileStream(Path.GetFullPath(workingDirectory, relativePath), FileMode.Create);
        while(binarySize > 0)
        {
            var toRead = Math.Min(binarySize, copyBuffer.Length);
            var readBuffer = copyBuffer[..toRead];
            await fileStream.ReadExactlyAsync(readBuffer);
            await destinationStream.WriteAsync(readBuffer);
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
        ZipFile.ExtractToDirectory(tempFile, Path.GetFullPath(workingDirectory), true);
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
