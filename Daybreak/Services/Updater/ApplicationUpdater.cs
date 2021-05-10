using Daybreak.Exceptions;
using Daybreak.Models;
using Daybreak.Models.Github;
using Daybreak.Services.Http;
using Daybreak.Services.Logging;
using Daybreak.Services.Runtime;
using Daybreak.Services.ViewManagement;
using Daybreak.Utils;
using Daybreak.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Version = Daybreak.Models.Versioning.Version;

namespace Daybreak.Services.Updater
{
    public sealed class ApplicationUpdater : IApplicationUpdater
    {
        private const string LaunchActionName = "Launch_Daybreak";
        private const string UpdateDesiredKey = "UpdateDesired";
        private const string ExecutionPolicyKey = "ExecutionPolicy";
        private const string UpdatedKey = "Updating";
        private const string RegistryKey = "Daybreak";
        private const string ExtractAndRunPs1 = "ExtractAndRun.ps1";
        private const string TempFile = "tempfile.zip";
        private const string VersionTag = "{VERSION}";
        private const string InputFileTag = "{INPUTFILE}";
        private const string OutputPathTag = "{OUTPUTPATH}";
        private const string ExecutionPolicyTag = "{EXECUTIONPOLICY}";
        private const string ProcessIdTag = "{PROCESSID}";
        private const string ExecutableNameTag = "{EXECUTABLE}";
        private const string WorkingDirectoryTag = "{WORKINGDIRECTORY}";
        private const string RefTagPrefix = "/refs/tags";
        private const string VersionListUrl = "https://api.github.com/repos/AlexMacocian/Daybreak/git/refs/tags";
        private const string Url = "https://github.com/AlexMacocian/Daybreak/releases/latest";
        private const string DownloadUrl = $"https://github.com/AlexMacocian/Daybreak/releases/download/{VersionTag}/Daybreak{VersionTag}.zip";
        private const string GetExecutionPolicyCommand = "Get-ExecutionPolicy -Scope CurrentUser";
        private const string SetExecutionPolicyCommand = $"Set-ExecutionPolicy {ExecutionPolicyTag} -Scope CurrentUser";
        private const string WaitCommand = $"Wait-Process -Id {ProcessIdTag}";
        private const string ExtractCommandTemplate = $"Expand-Archive -Path '{InputFileTag}' -DestinationPath '{OutputPathTag}' -Force";
        private const string PrepareScheduledAction = $"$action = New-ScheduledTaskAction -Execute {ExecutableNameTag} -WorkingDirectory {WorkingDirectoryTag}";
        private const string PrepareTriggerForAction = $"$trigger = New-ScheduledTaskTrigger -Once -At (Get-Date)";
        private const string RegisterScheduledAction = $"Register-ScheduledTask -Action $action -Trigger $trigger -TaskName {LaunchActionName} | Out-Null";
        private const string LaunchScheduledAction = $"Start-ScheduledTask -TaskName {LaunchActionName}";
        private const string SleepOneSecond = $"Start-Sleep -s 1";
        private const string UnregisterScheduledAction = $"Unregister-ScheduledTask -TaskName {LaunchActionName} -Confirm:$false";
        private const string RemoveTempFile = $"Remove-item {TempFile}";
        private const string RemovePs1 = $"Remove-item {ExtractAndRunPs1}";

        private readonly CancellationTokenSource updateCancellationTokenSource = new();
        private readonly ILogger logger;
        private readonly IViewManager viewManager;
        private readonly IRuntimeStore runtimeStore;
        private readonly IHttpClient<ApplicationUpdater> httpClient;

        public Version CurrentVersion { get; }

        public ApplicationUpdater(
            ILogger logger,
            IRuntimeStore runtimeStore,
            IViewManager viewManager,
            IHttpClient<ApplicationUpdater> httpClient)
        {
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.runtimeStore = runtimeStore.ThrowIfNull(nameof(runtimeStore));
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.httpClient = httpClient.ThrowIfNull(nameof(httpClient));
            this.httpClient.DefaultRequestHeaders.Add("user-agent", "Daybreak Client");
            if (Version.TryParse(Assembly.GetExecutingAssembly().GetName().Version.ToString(), out var currentVersion))
            {
                if (currentVersion.HasPrefix is false)
                {
                    currentVersion = Version.Parse("v" + currentVersion);
                }

                this.CurrentVersion = currentVersion;
            }
            else
            {
                throw new FatalException($"Application version is invalid: {Assembly.GetExecutingAssembly().GetName().Version}");
            }
        }

        public async Task<bool> DownloadUpdate(Version version, UpdateStatus updateStatus)
        {
            updateStatus.CurrentStep = UpdateStatus.InitializingDownload;
            var uri = DownloadUrl.Replace(VersionTag, version.ToString());
            using var response = await this.httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode is false)
            {
                updateStatus.CurrentStep = UpdateStatus.FailedDownload;
                this.logger.LogError($"Failed to download update. Details: {await response.Content.ReadAsStringAsync()}");
                return false;
            }

            using var downloadStream = await this.httpClient.GetStreamAsync(uri);
            this.logger.LogInformation("Beginning update download");
            var fileStream = File.OpenWrite(TempFile);
            var downloadSize = (double)response.Content.Headers.ContentLength;
            var buffer = new byte[1024];
            var length = 0;
            double downloaded = 0;
            var tickTime = DateTime.Now;
            while (downloadStream.CanRead && (length = await downloadStream.ReadAsync(buffer)) > 0)
            {
                downloaded += length;
                await fileStream.WriteAsync(buffer, 0, length);
                if ((DateTime.Now - tickTime).TotalMilliseconds > 50)
                {
                    tickTime = DateTime.Now;
                    updateStatus.CurrentStep = UpdateStatus.Downloading(downloaded / downloadSize);
                }
            }

            updateStatus.CurrentStep = UpdateStatus.Downloading(1);
            updateStatus.CurrentStep = UpdateStatus.DownloadFinished;
            fileStream.Close();
            this.logger.LogInformation("Downloaded update");
            return true;
        }

        public async Task<bool> DownloadLatestUpdate(UpdateStatus updateStatus)
        {
            updateStatus.CurrentStep = UpdateStatus.CheckingLatestVersion;
            var latestVersion = (await this.GetLatestVersion()).ExtractValue();
            if (latestVersion is null)
            {
                this.logger.LogWarning("Failed to retrieve latest version. Aborting update");
                return false;
            }

            if (Version.TryParse(latestVersion, out var parsedVersion) is false)
            {
                throw new InvalidOperationException($"Could not parse retrieved version: {latestVersion}");
            }

            return await this.DownloadUpdate(parsedVersion, updateStatus);
        }

        public async Task<bool> UpdateAvailable()
        {
            var maybeLatestVersion = await this.GetLatestVersion();
            return maybeLatestVersion.Switch(
                onSome: latestVersion => string.Compare(this.CurrentVersion.ToString().Trim('v'), latestVersion, true) < 0,
                onNone: () =>
                {
                    this.logger.LogWarning("Failed to retrieve latest version");
                    return false;
                }).ExtractValue();
        }

        public async Task<IEnumerable<Version>> GetVersions()
        {
            this.logger.LogInformation($"Retrieving version list from {VersionListUrl}");
            var response = await this.httpClient.GetAsync(VersionListUrl);
            if (response.IsSuccessStatusCode)
            {
                var serializedList = await response.Content.ReadAsStringAsync();
                var versionList = serializedList.Deserialize<GithubRefTag[]>();
                return versionList.Select(v => v.Ref.Remove(0, RefTagPrefix.Length)).Select(v => new Version(v));
            }

            return new List<Version>();
        }

        public void PeriodicallyCheckForUpdates()
        {
            System.Extensions.TaskExtensions.RunPeriodicAsync(async () =>
            {
                if (this.runtimeStore.TryGetValue<bool>(UpdateDesiredKey, out var desiringUpdate) && desiringUpdate is false)
                {
                    this.updateCancellationTokenSource.Cancel();
                    return;
                }

                if (await this.UpdateAvailable())
                {
                    Application.Current.Dispatcher.Invoke(() => this.viewManager.ShowView<AskUpdateView>());
                }
            }, TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(15), this.updateCancellationTokenSource.Token);
        }

        public void FinalizeUpdate()
        {
            var maybeExecutionPolicy = this.RetrieveExecutionPolicy();
            maybeExecutionPolicy.DoAny(
                onNone: () =>
                {
                    throw new InvalidOperationException("Failed to retrieve execution policy");
                });

            var executionPolicy = maybeExecutionPolicy.ExtractValue();
            if (executionPolicy is not ExecutionPolicies.Bypass ||
                executionPolicy is not ExecutionPolicies.Unrestricted)
            {
                this.logger.LogInformation($"Execution policy is set to {executionPolicy}. Setting to {ExecutionPolicies.Bypass}");
            }

            SaveExecutionPolicyValueToRegistry(executionPolicy);
            MarkUpdateInRegistry();
            this.SetExecutionPolicy(ExecutionPolicies.Bypass);
            this.LaunchExtractor();
        }

        public void OnStartup()
        {
            if (UpdateMarkedInRegistry())
            {
                UnmarkUpdateInRegistry();
                var maybeExecutionPolicy = LoadExecutionPolicyValueFromRegistry();
                maybeExecutionPolicy.Do(
                    onSome: policy =>
                    {
                        SetExecutionPolicy(policy);
                    },
                    onNone: () =>
                    {
                        throw new InvalidOperationException("Found update marked in registry but no execution policy");
                    });
            }
        }

        public void OnClosing()
        {
        }

        private async Task<Optional<string>> GetLatestVersion()
        {
            using var response = await this.httpClient.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                var versionTag = response.RequestMessage.RequestUri.ToString().Split('/').Last().TrimStart('v');
                return versionTag;
            }

            return Optional.None<string>();
        }

        private Optional<ExecutionPolicies> RetrieveExecutionPolicy()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = GetExecutionPolicyCommand,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            this.logger.LogInformation("Checking current execution policy");
            var output = process.StandardOutput.ReadToEnd();
            if (!Enum.TryParse(typeof(ExecutionPolicies), output, out var executionPolicy))
            {
                var error = process.StandardError.ReadToEnd();
                this.logger.LogError($"Failed to retrieve current user execution policy. Stdout: {output}. Stderr: {error}");
                return Optional.None<ExecutionPolicies>();
            }

            return executionPolicy.Cast<ExecutionPolicies>();
        }

        private void SetExecutionPolicy(ExecutionPolicies executionPolicy)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = SetExecutionPolicyCommand.Replace(ExecutionPolicyTag, executionPolicy.ToString()),
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            this.logger.LogInformation($"Setting execution policy to {executionPolicy}");
            var output = process.StandardOutput.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(output))
            {
                var error = process.StandardError.ReadToEnd();
                throw new InvalidOperationException($"Failed to set execution policy to {executionPolicy}. Stdout: {output}. Stderr: {error}");
            }
        }

        private void LaunchExtractor()
        {
            File.WriteAllLines(ExtractAndRunPs1, new List<string>()
            {
                WaitCommand.Replace(ProcessIdTag, Environment.ProcessId.ToString()),
                ExtractCommandTemplate
                    .Replace(InputFileTag, Path.GetFullPath(TempFile))
                    .Replace(OutputPathTag, Directory.GetCurrentDirectory()),
                RemoveTempFile,
                PrepareScheduledAction
                    .Replace(ExecutableNameTag, Process.GetCurrentProcess()?.MainModule?.FileName)
                    .Replace(WorkingDirectoryTag, Directory.GetCurrentDirectory()),
                PrepareTriggerForAction,
                RegisterScheduledAction,
                LaunchScheduledAction,
                SleepOneSecond,
                UnregisterScheduledAction,
                RemovePs1,
            });
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $@"{Directory.GetCurrentDirectory()}\{ExtractAndRunPs1}",
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Maximized,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    Verb = "runas"
                },
            };
            this.logger.LogInformation("Created extractor script. Attempting to launch powershell");
            if (process.Start() is false)
            {
                throw new InvalidOperationException("Failed to create and start powershell script");
            }
        }

        private static void MarkUpdateInRegistry()
        {
            var homeRegistryKey = GetOrCreateHomeKey();
            homeRegistryKey.SetValue(UpdatedKey, true);
            homeRegistryKey.Close();
        }

        private static void UnmarkUpdateInRegistry()
        {
            var homeRegistryKey = GetOrCreateHomeKey();
            homeRegistryKey.SetValue(UpdatedKey, false);
            homeRegistryKey.Close();
        }

        private static bool UpdateMarkedInRegistry()
        {
            var homeRegistryKey = GetOrCreateHomeKey();
            var update = homeRegistryKey.GetValue(UpdatedKey);
            homeRegistryKey.Close();
            if (update is string updateString)
            {
                if (bool.TryParse(updateString, out var updateValue))
                {
                    return updateValue;
                }
                else
                {
                    throw new InvalidOperationException($"Found update value {updateString} in registry");
                }
            }

            return false;
        }

        private static void SaveExecutionPolicyValueToRegistry(ExecutionPolicies executionPolicy)
        {
            var homeRegistryKey = GetOrCreateHomeKey();
            homeRegistryKey.SetValue(ExecutionPolicyKey, executionPolicy.ToString());
            homeRegistryKey.Close();
        }

        private static Optional<ExecutionPolicies> LoadExecutionPolicyValueFromRegistry()
        {
            var homeRegistryKey = GetOrCreateHomeKey();
            var executionPolicy = homeRegistryKey.GetValue(ExecutionPolicyKey);
            homeRegistryKey.Close();
            
            if (executionPolicy is null)
            {
                return Optional.None<ExecutionPolicies>();
            }
            else if (executionPolicy is string executionPolicyString)
            {
                if (Enum.TryParse<ExecutionPolicies>(executionPolicyString, out var executionPolicyValue))
                {
                    return executionPolicyValue;
                }
                else
                {
                    throw new InvalidOperationException($"Found execution policy with value {executionPolicy}");
                }
            }
            else
            {
                throw new InvalidOperationException($"Found execution policy of type {executionPolicy.GetType()}.");
            }
        }

        private static RegistryKey GetOrCreateHomeKey()
        {
            var homeRegistryKey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey(RegistryKey, true);
            if (homeRegistryKey is null)
            {
                homeRegistryKey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey(RegistryKey, true);
            }

            return homeRegistryKey;
        }
    }
}
