using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Text;

const string LatestUrl = "https://github.com/gwdevhub/Daybreak/releases/latest";
var daybreakExecutable = OperatingSystem.IsWindows() ? "Daybreak.exe" : "Daybreak";
var daybreakProcessName = "Daybreak";
var releaseAssetSuffix = OperatingSystem.IsWindows() ? "" : "-linux";

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

static void CleanWorkingDirectory(string workingDirectory)
{
    if (!Directory.Exists(workingDirectory))
    {
        return;
    }

    var installerPath = Path.GetFullPath(Path.Combine(workingDirectory, "Installer"));
    var optionsPath = Path.GetFullPath(Path.Combine(workingDirectory, "Daybreak.options"));
    var preserve = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        installerPath,
        optionsPath,
    };
    var preserveExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".tpf" };

    foreach (var entry in Directory.GetFileSystemEntries(workingDirectory))
    {
        var fullPath = Path.GetFullPath(entry);
        if (preserve.Contains(fullPath))
        {
            continue;
        }

        if (preserveExtensions.Contains(Path.GetExtension(fullPath)))
        {
            continue;
        }

        try
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }
            else if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to delete {fullPath}.\n{e}");
        }
    }
}

async ValueTask PerformUpdate(string workingDirectory)
{
    if (!Directory.Exists(workingDirectory))
    {
        Console.WriteLine("Working directory does not exist");
        Console.ReadKey();
        return;
    }

    Console.WriteLine($"Working directory: {workingDirectory}");

    var tempFile = Path.GetFullPath("tempfile.zip", workingDirectory);
    var updatePkg = Path.GetFullPath("update.pkg", workingDirectory);
    var executableName = Path.GetFullPath(daybreakExecutable, workingDirectory);

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
            using var destinationStream = new FileStream(
                Path.GetFullPath(relativePath, workingDirectory),
                FileMode.Create
            );
            while (binarySize > 0)
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
        catch { }

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
    var process = new Process { StartInfo = new ProcessStartInfo { FileName = executableName } };

    if (process.Start() is false)
    {
        Console.WriteLine("Failed to launch application");
        Console.ReadKey();
    }
}

async ValueTask PerformFreshInstall(string workingDirectory)
{
    using var handler = new HttpClientHandler { AllowAutoRedirect = false };
    var client = new HttpClient(handler);
    using var latestResponse = await client.GetAsync(LatestUrl);

    var isRedirect =
        latestResponse.StatusCode
        is HttpStatusCode.Redirect
            or HttpStatusCode.MovedPermanently
            or HttpStatusCode.Found
            or HttpStatusCode.SeeOther
            or HttpStatusCode.TemporaryRedirect
            or HttpStatusCode.PermanentRedirect;

    if (!isRedirect)
    {
        Console.WriteLine(
            $"Failed to get latest release information. Status: {latestResponse.StatusCode}"
        );
        Console.ReadKey();
        return;
    }

    var redirectTarget = latestResponse.Headers.Location;
    if (redirectTarget is null)
    {
        Console.WriteLine("Latest release redirect location missing.");
        Console.ReadKey();
        return;
    }

    var finalUri = redirectTarget.IsAbsoluteUri
        ? redirectTarget
        : new Uri(latestResponse.RequestMessage!.RequestUri!, redirectTarget);

    var version = finalUri.Segments.Last().TrimEnd('/');
    var downloadUri = new Uri(
        $"https://github.com/gwdevhub/Daybreak/releases/download/{version}/daybreak{version}{releaseAssetSuffix}.zip"
    );
    var tempFile = Path.GetFullPath("tempfile.zip", workingDirectory);

    Console.WriteLine($"Cleaning installation directory {workingDirectory}");
    CleanWorkingDirectory(workingDirectory);

    Console.WriteLine($"Downloading Daybreak {version}...");

    handler.Dispose();
    client.Dispose();

    client = new HttpClient();
    using var downloadResponse = await client.GetAsync(
        downloadUri,
        HttpCompletionOption.ResponseHeadersRead
    );
    if (!downloadResponse.IsSuccessStatusCode)
    {
        Console.WriteLine(
            $"Failed to download Daybreak package. Status: {downloadResponse.StatusCode}"
        );
        Console.ReadKey();
        return;
    }

    var totalBytes = downloadResponse.Content.Headers.ContentLength ?? -1L;
    using var contentStream = await downloadResponse.Content.ReadAsStreamAsync();
    using var fileStream = new FileStream(
        tempFile,
        FileMode.Create,
        FileAccess.Write,
        FileShare.None
    );
    var totalRead = 0L;
    var buffer = new Memory<byte>(new byte[8192]);
    int bytesRead;
    while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
    {
        await fileStream.WriteAsync(buffer[..bytesRead]);
        totalRead += bytesRead;
        if (totalBytes != -1)
        {
            RenderProgressBar((int)totalRead, (int)totalBytes, 40);
        }
    }

    fileStream.Close();
    fileStream.Dispose();
    client.Dispose();
    Console.WriteLine("\nDownload complete. Extracting files...");
    try
    {
        ZipFile.ExtractToDirectory(tempFile, Path.GetFullPath(workingDirectory), true);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed to extract package.\n{e}");
        Console.ReadKey();
        return;
    }

    File.Delete(tempFile);

    Console.WriteLine("Installation complete. Launching application...");
    var executableName = Path.GetFullPath(daybreakExecutable, workingDirectory);
    var process = new Process { StartInfo = new ProcessStartInfo { FileName = executableName } };

    if (process.Start() is false)
    {
        Console.WriteLine("Failed to launch application");
        Console.ReadKey();
    }
}

Console.Title = "Daybreak Installer";
Console.WriteLine("Starting installation...");
while (Process.GetProcesses().FirstOrDefault(p => p.ProcessName == daybreakProcessName)?.HasExited is false)
{
    Console.WriteLine($"Detected Daybreak process is still running. Waiting 5s and retrying");
    await Task.Delay(5000);
}

if (OperatingSystem.IsWindows())
{
    while (Process.GetProcessesByName("gw").FirstOrDefault()?.HasExited is false)
    {
        Console.WriteLine($"Detected Guild Wars process is still running. Waiting 5s and retrying");
        await Task.Delay(5000);
    }
}

var mode = args.Length > 1 ? args[0] : "install";
var workingDirectory = args.Length > 2 ? args[1] : Path.GetFullPath("..", AppContext.BaseDirectory);

// Normalize working directory path
workingDirectory = Path.GetFullPath(workingDirectory);
if (mode is "install")
{
    await PerformFreshInstall(workingDirectory);
}
else
{
    await PerformUpdate(workingDirectory);
}
