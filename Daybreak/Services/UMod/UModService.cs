using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Models.Progress;
using Daybreak.Models.UMod;
using Daybreak.Services.Downloads;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using Daybreak.Services.UMod.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod;

public sealed class UModService : IUModService
{
    private const string DownloadUrl = "https://storage.googleapis.com/google-code-archive-downloads/v2/code.google.com/texmod/uMod_v1_r44.zip";
    private const string ArchiveName = "uMod_v1_r44.zip";
    private const string D3D9EntryName = "uMod/d3d9.dll";
    private const string UModDirectory = "uMod";
    private const string D3D9Dll = "d3d9.dll";
    private const string D3D9DllBackup = "d3d9.dll.backup";

    private readonly IProcessInjector processInjector;
    private readonly INotificationService notificationService;
    private readonly IUModClient uModClient;
    private readonly IDownloadService downloadService;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ILiveUpdateableOptions<UModOptions> uModOptions;
    private readonly ILogger<UModService> logger;

    public string Name => "uMod";

    public bool IsEnabled
    {
        get => this.uModOptions.Value.Enabled;
        set
        {
            this.uModOptions.Value.Enabled = value;
            this.uModOptions.UpdateOption();
        }
    }

    public bool IsInstalled => File.Exists(this.uModOptions.Value.DllPath);

    public UModService(
        IProcessInjector processInjector,
        INotificationService notificationService,
        IUModClient uModClient,
        IDownloadService downloadService,
        ILiveOptions<LauncherOptions> launcherOptions,
        ILiveUpdateableOptions<UModOptions> uModOptions,
        ILogger<UModService> logger)
    {
        this.processInjector = processInjector.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.uModClient = uModClient.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.uModOptions = uModOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public IEnumerable<string> GetCustomArguments()
    {
        return Enumerable.Empty<string>();
    }

    public Task OnGuildWarsStarting(Process process, CancellationToken cancellationToken)
    {
        this.uModClient.Initialize(cancellationToken);
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(Process process, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsCreated(Process process, CancellationToken cancellationToken)
    {
        return this.processInjector.Inject(process, Path.Combine(Path.GetFullPath(UModDirectory), D3D9Dll), cancellationToken);
    }

    public async Task OnGuildWarsStarted(Process process, CancellationToken cancellationToken)
    {
        try
        {
            await this.uModClient.WaitForInitialize(cancellationToken);
        }
        catch(TimeoutException)
        {
            this.NotifyFaultyInstallation();
            return;
        }

        foreach(var entry in this.uModOptions.Value.Mods.Where(e => e.Enabled && e.PathToFile is not null))
        {
            await this.uModClient.AddFile(entry.PathToFile!, cancellationToken);
        }

        try
        {
            await this.uModClient.Send(cancellationToken);
        }
        catch(IOException e)
        {
            if (!e.Message.Contains("Pipe is broken", StringComparison.OrdinalIgnoreCase))
            {
                throw;
            }

            this.NotifyFaultyInstallation();
            return;
        }

        this.uModClient.CloseConnection();
        this.notificationService.NotifyInformation(
                title: "uMod started",
                description: "uMod textures have been loaded");
    }

    public bool LoadUModFromDisk()
    {
        var filePicker = new OpenFileDialog
        {
            Filter = "Dll Files (d3d9.dll)|d3d9.dll",
            Multiselect = false,
            RestoreDirectory = true,
            Title = "Please select the uMod d3d9.dll"
        };
        if (filePicker.ShowDialog() is false)
        {
            return false;
        }

        var fileName = filePicker.FileName;
        this.uModOptions.Value.DllPath = Path.GetFullPath(fileName);
        this.uModOptions.UpdateOption();
        return true;
    }

    public async Task<bool> SetupUMod(UModInstallationStatus uModInstallationStatus)
    {
        if ((await this.SetupUModDll(uModInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to setup the uMod executable");
            return false;
        }

        uModInstallationStatus.CurrentStep = UModInstallationStatus.Finished;
        return true;
    }

    public bool AddMod(string pathToTpf, bool? imported = default)
    {
        var fullPath = Path.GetFullPath(pathToTpf);
        if (!File.Exists(pathToTpf))
        {
            return false;
        }

        if (this.uModOptions.Value.Mods.Any(m => m.PathToFile == fullPath))
        {
            return true;
        }

        var entry = new UModEntry
        {
            Enabled = this.uModOptions.Value.AutoEnableMods,
            PathToFile = fullPath,
            Name = Path.GetFileNameWithoutExtension(fullPath),
            Imported = imported is true
        };

        this.uModOptions.Value.Mods.Add(entry);
        this.uModOptions.UpdateOption();
        return true;
    }

    public bool RemoveMod(string pathToTpf)
    {
        var fullPath = Path.GetFullPath(pathToTpf);
        var mods = this.uModOptions.Value.Mods;
        var maybeMod = mods.FirstOrDefault(m => m.PathToFile == fullPath);
        if (maybeMod is null)
        {
            return true;
        }

        mods.Remove(maybeMod);
        this.SaveMods(mods);

        // If the mod was downloaded and managed entirely through Daybreak, we can safely delete it
        if (!maybeMod.Imported)
        {
            File.Delete(fullPath);
        }

        return true;
    }

    public List<UModEntry> GetMods()
    {
        return this.uModOptions.Value.Mods;
    }

    public void SaveMods(List<UModEntry> list)
    {
        this.uModOptions.Value.Mods = list;
        this.uModOptions.UpdateOption();
    }

    private void NotifyFaultyInstallation()
    {
        /*
             * Known issue where Guild Wars updater breaks the executable, which in turn breaks the integration with uMod.
             * Prompt the user to manually reinstall Guild Wars.
             */
        this.notificationService.NotifyInformation(
            title: "uMod failed to start",
            description: "uMod failed to start due to a known issue with Guild Wars updating process. Please manually re-install Guild Wars in order to restore uMod functionality");
    }

    private async Task<bool> SetupUModDll(UModInstallationStatus uModInstallationStatus)
    {
        if (File.Exists(this.uModOptions.Value.DllPath))
        {
            return true;
        }

        if ((await this.downloadService.DownloadFile(DownloadUrl, ArchiveName, uModInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to install uMod. Failed to download uMod archive");
            return false;
        }

        using var archiveFileStream = new FileStream(ArchiveName, FileMode.Open);
        using var archive = new ZipArchive(archiveFileStream);
        var d3d9Entry = archive.Entries.FirstOrDefault(e => e.FullName == D3D9EntryName);
        if (d3d9Entry is null)
        {
            this.logger.LogError("Failed to install uMod. Failed to find d3d9.dll entry in uMod archive");
            return false;
        }

        var d3d9Path = Path.GetFullPath(Path.Combine(UModDirectory, D3D9Dll));
        Directory.CreateDirectory(Path.GetFullPath(UModDirectory));
        d3d9Entry.ExtractToFile(d3d9Path, true);
        
        var uModOptions = this.uModOptions.Value;
        uModOptions.DllPath = d3d9Path;
        this.uModOptions.UpdateOption();

        archive.Dispose();
        archiveFileStream.Dispose();
        File.Delete(ArchiveName);
        return true;
    }
}
