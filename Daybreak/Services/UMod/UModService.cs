using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Models.Progress;
using Daybreak.Models.UMod;
using Daybreak.Services.Downloads;
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

    private readonly IUModClient uModClient;
    private readonly IDownloadService downloadService;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ILiveUpdateableOptions<UModOptions> uModOptions;
    private readonly ILogger<UModService> logger;

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
        IUModClient uModClient,
        IDownloadService downloadService,
        ILiveOptions<LauncherOptions> launcherOptions,
        ILiveUpdateableOptions<UModOptions> uModOptions,
        ILogger<UModService> logger)
    {
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

    public async Task OnGuildwarsStarting(Process process)
    {
        this.uModClient.Initialize(CancellationToken.None);
        await this.LaunchUmod(process);
    }

    public async Task OnGuildwarsStarted(Process process)
    {
        foreach(var entry in this.uModOptions.Value.Mods.Where(e => e.Enabled && e.PathToFile is not null))
        {
            await this.uModClient.AddFile(entry.PathToFile!, CancellationToken.None);
        }

        await this.uModClient.Send(CancellationToken.None);
        this.uModClient.CloseConnection();
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

    private Task LaunchUmod(Process gwProcess)
    {
        if (this.uModOptions.Value.Enabled is false)
        {
            throw new InvalidOperationException("Cannot launch uMod. uMod is disabled");
        }

        var dll = this.uModOptions.Value.DllPath;
        if (File.Exists(dll) is false)
        {
            throw new ExecutableNotFoundException($"uMod executable doesn't exist at {dll}");
        }

        this.logger.LogInformation("Setting up uMod d3d9 dll");
        this.SetupD3D9Dll(gwProcess);
        return Task.CompletedTask;
    }

    private void SetupD3D9Dll(Process gwProcess)
    {
        if (gwProcess.StartInfo.FileName?.IsNullOrWhiteSpace() is true)
        {
            throw new InvalidOperationException("Unable to start uMod. Invalid Guild Wars process");
        }

        var guildWarsDirectory = Path.GetDirectoryName(gwProcess.StartInfo.FileName);
        var d3d9SourceFilePath = Path.Combine(UModDirectory, D3D9Dll);
        var d3d9DestinationFilePath = Path.Combine(guildWarsDirectory!, D3D9Dll);
        if (MustBackupD3D9Dll(d3d9SourceFilePath, d3d9DestinationFilePath))
        {
            this.logger.LogInformation($"Found an existing {D3D9Dll} file. Saving it as {D3D9DllBackup}");
            var d3d9DestinationBackupFilePath = Path.Combine(guildWarsDirectory!, D3D9DllBackup);
            if (File.Exists(d3d9DestinationBackupFilePath))
            {
                File.Delete(d3d9DestinationBackupFilePath);
            }

            File.Move(d3d9DestinationFilePath, d3d9DestinationBackupFilePath);
            File.Delete(d3d9DestinationFilePath);
        }

        if (!File.Exists(d3d9DestinationFilePath))
        {
            this.logger.LogInformation($"Copying {d3d9SourceFilePath} to {d3d9DestinationFilePath}");
            File.Copy(d3d9SourceFilePath, d3d9DestinationFilePath);
        }
    }

    private static bool MustBackupD3D9Dll(string sourcePath, string destinationPath)
    {
        if (!File.Exists(destinationPath))
        {
            return false;
        }

        var sourceInfo = new FileInfo(sourcePath);
        var destinationInfo = new FileInfo(destinationPath);
        return sourceInfo.Length != destinationInfo.Length;
    }
}
