using Daybreak.Models;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Guildwars;
using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public class ExecutablesViewModel(
    IGuildWarsInstaller guildWarsInstaller,
    IGuildWarsExecutableManager guildWarsExecutableManager)
    : ViewModelBase<ExecutablesViewModel, ExecutablesView>
{
    private readonly IGuildWarsInstaller guildWarsInstaller = guildWarsInstaller;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager;

    private CancellationTokenSource? validationTokenSource;

    public bool AddButtonEnabled { get; set; } = true;

    public List<ExecutablePath> Executables { get; set; } = [];

    public override ValueTask ParametersSet(ExecutablesView view, CancellationToken cancellationToken)
    {
        this.Executables.ClearAnd().AddRange(this.guildWarsExecutableManager.GetExecutableList().Select(s => new ExecutablePath { Path = s, Validating = true, Valid = false }));
        this.ValidateExecutables();
        return base.ParametersSet(view, cancellationToken);
    }

    public void RemoveExecutable(ExecutablePath executable)
    {
        this.Executables.Remove(executable);
        this.guildWarsExecutableManager.RemoveExecutable(executable.Path);
    }

    public void CreateExecutable()
    {
        if (GetPath() is not string path)
        {
            return;
        }

        var newExecutable = new ExecutablePath
        {
            Path = path,
            Validating = true,
            Valid = false,
            NeedsUpdate = false
        };

        this.Executables.Add(newExecutable);
        this.guildWarsExecutableManager.AddExecutable(newExecutable.Path);
        this.ValidateExecutables();
    }

    public void ModifyPath(ExecutablePath executable)
    {
        var maybePath = GetPath();
        if (maybePath is null)
        {
            return;
        }

        this.guildWarsExecutableManager.RemoveExecutable(executable.Path);
        executable.Path = maybePath;
        this.guildWarsExecutableManager.AddExecutable(executable.Path);
        this.ValidateExecutables();
    }

    public async void UpdateExecutable(ExecutablePath executable)
    {
        foreach(var e in this.Executables)
        {
            e.Locked = true;
        }

        this.AddButtonEnabled = false;
        executable.Validating = true;
        await this.RefreshViewAsync();
        try
        {
            var installationStatus = new GuildwarsInstallationStatus();
            installationStatus.PropertyChanged += (_, _) =>
            {
                if (installationStatus.CurrentStep is GuildwarsInstallationStatus.GuildwarsInstallationStep step &&
                    step.Final)
                {
                    executable.UpdateProgress = default;
                    executable.NeedsUpdate = false;
                    executable.Validating = true;
                    this.ValidateExecutables();
                    this.RefreshView();
                }
                else
                {
                    executable.UpdateProgress = installationStatus.CurrentStep switch
                    {
                        GuildwarsInstallationStatus.UnpackingProgressStep unpackingProgress => $"{50 + (unpackingProgress.Progress * 50):F1}%",
                        DownloadStatus.DownloadProgressStep downloadProgress => $"{downloadProgress.Progress * 50:F1}%",
                        _ => default
                    };
                    this.RefreshView();
                }
            };

            if (!await this.guildWarsInstaller.UpdateGuildwars(executable.Path, installationStatus, CancellationToken.None))
            {
                executable.Validating = false;
                executable.Valid = false;
                executable.NeedsUpdate = true;
                executable.UpdateProgress = default;
            }

            await this.RefreshViewAsync();
        }
        catch
        {
            
        }
        finally
        {
            foreach (var e in this.Executables)
            {
                e.Locked = false;
            }

            this.AddButtonEnabled = true;
            await this.RefreshViewAsync();
        }
    }

    private void ValidateExecutables()
    {
        this.validationTokenSource?.Cancel();
        this.validationTokenSource = new CancellationTokenSource();
        var token = this.validationTokenSource.Token;
        Task.Factory.StartNew(async () =>
        {
            if (this.Executables.None(e => e.Validating))
            {
                return;
            }

            var latestId = await this.guildWarsInstaller.GetLatestVersionId(token);
            if (latestId is null)
            {
                return;
            }

            foreach (var executable in this.Executables)
            {
                if (!executable.Validating)
                {
                    continue;
                }

                if (!this.guildWarsExecutableManager.IsValidExecutable(executable.Path))
                {
                    executable.Validating = false;
                    executable.Valid = false;
                    executable.NeedsUpdate = false;
                    await this.RefreshViewAsync();
                    continue;
                }

                var executableVersion = await this.guildWarsInstaller.GetVersionId(executable.Path, token);
                if (executableVersion is null)
                {
                    continue;
                }

                if (executableVersion != latestId)
                {
                    executable.Validating = false;
                    executable.Valid = false;
                    executable.NeedsUpdate = true;
                    await this.RefreshViewAsync();
                    continue;
                }

                executable.Validating = false;
                executable.Valid = true;
                executable.NeedsUpdate = false;
                await this.RefreshViewAsync();
            }
        }, token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    private static string? GetPath()
    {
        var filePicker = new Microsoft.Win32.OpenFileDialog()
        {
            CheckFileExists = true,
            CheckPathExists = true,
            DefaultExt = "exe",
            Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*",
            Multiselect = false,
            Title = "Select Guild Wars Executable"
        };

        if (filePicker.ShowDialog() == true)
        {
            return filePicker.FileName;
        }

        return default;
    }
}
