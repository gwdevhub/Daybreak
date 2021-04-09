using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Updater
{
    public class ReleaseClient
    {
        private const long OneKb = 1024;
        private const long OneMb = OneKb * 1024;
        private const long OneGb = OneMb * 1024;
        private const long OneTb = OneGb * 1024;

        private const string TempFile = "temp.zip";
        private const string ProcessName = "Daybreak";
        private const string VersionTag = "{VERSION}";
        private const string Url = "https://github.com/AlexMacocian/Daybreak/releases/latest";
        private const string DownloadUrl = "https://github.com/AlexMacocian/Daybreak/releases/download/{VERSION}/Daybreak{VERSION}.zip";
        private readonly HttpClient httpClient = new();
        
        public async Task<bool> CheckRelease(string currentVersion)
        {
            using var response = await this.httpClient.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                var versionTag = response.RequestMessage.RequestUri.ToString().Split('/').Last().TrimStart('v');
                if (string.Compare(currentVersion, versionTag, true) < 0)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task Update()
        {
            Console.WriteLine("Waiting for launcher to exit...");
            while (Process.GetProcessesByName(ProcessName).FirstOrDefault() is not null)
            {
                await Task.Delay(100);
            }

            Console.WriteLine("Getting latest version...");
            using var getLatestResponse = await this.httpClient.GetAsync(Url);
            if (getLatestResponse.IsSuccessStatusCode is false)
            {
                Console.WriteLine("Failed to get latest version. Restarting client...");
                Process.Start($"{ProcessName}.exe");
                return;
            }

            var versionTag = getLatestResponse.RequestMessage.RequestUri.ToString().Split('/').Last();
            Console.WriteLine($"Latest version is {versionTag}. Downloading latest version...");
            using var downloadLatestResponse = await this.httpClient.GetAsync(
                DownloadUrl.Replace(VersionTag, versionTag));
            
            if (downloadLatestResponse.IsSuccessStatusCode is false)
            {
                Console.WriteLine("Failed to download latest version. Restarting client...");
                Process.Start($"{ProcessName}.exe");
                return;
            }

            Console.WriteLine("Downloading latest version...");
            using var fs = File.OpenWrite(TempFile);
            var downloadStream = await downloadLatestResponse.Content.ReadAsStreamAsync();
            Console.WriteLine($"Downloading {downloadStream.Length} bytes...");
            Console.WriteLine($"Downloading 0b/s");
            var startTime = DateTime.Now;
            var tickTime = DateTime.Now;
            var downloaded = 0;
            var buffer = new byte[1024];
            int length = 0;
            while (downloadStream.CanRead && (length = await downloadStream.ReadAsync(buffer)) > 0)
            {
                downloaded += length;
                await fs.WriteAsync(buffer, 0, length, CancellationToken.None);
                if ((DateTime.Now - tickTime).TotalSeconds >= 1)
                {
                    tickTime = DateTime.Now;
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentConsoleLine();
                    Console.WriteLine($"Downloading {ToPrettySize(downloaded)}/s");
                    downloaded = 0;
                }
            }

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            ClearCurrentConsoleLine();
            Console.WriteLine("Downloaded latest version...");
            fs.Close();
            Console.WriteLine("Extracting zip file...");
            ZipFile.ExtractToDirectory(TempFile, Directory.GetCurrentDirectory(), true);
            Console.WriteLine("Launching client...");
            Process.Start($"{ProcessName}.exe");
        }

        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static string ToPrettySize(long value, int decimalPlaces = 0)
        {
            var asTb = Math.Round((double)value / OneTb, decimalPlaces);
            var asGb = Math.Round((double)value / OneGb, decimalPlaces);
            var asMb = Math.Round((double)value / OneMb, decimalPlaces);
            var asKb = Math.Round((double)value / OneKb, decimalPlaces);
            string chosenValue = asTb > 1 ? string.Format("{0}Tb", asTb)
                : asGb > 1 ? string.Format("{0}Gb", asGb)
                : asMb > 1 ? string.Format("{0}Mb", asMb)
                : asKb > 1 ? string.Format("{0}Kb", asKb)
                : string.Format("{0}B", Math.Round((double)value, decimalPlaces));
            return chosenValue;
        }
    }
}
