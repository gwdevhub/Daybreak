using Daybreak.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Guildwars;
using Photino.NET;
using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public class ExecutablesViewModel(
    PhotinoWindow window,
    IGuildWarsInstaller guildWarsInstaller,
    IGuildWarsExecutableManager guildWarsExecutableManager)
    : ViewModelBase<ExecutablesViewModel, ExecutablesView>
{
    private readonly PhotinoWindow window = window;
    private readonly IGuildWarsInstaller guildWarsInstaller = guildWarsInstaller;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager;
    private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

    private CancellationTokenSource? validationTokenSource;

    public bool AddButtonEnabled { get; set; } = true;

    public List<ExecutablePath> Executables { get; set; } = [];

    public override ValueTask ParametersSet(ExecutablesView view, CancellationToken cancellationToken)
    {
        this.Executables.ClearAnd().AddRange(this.guildWarsExecutableManager.GetExecutableList().Select(s => new ExecutablePath { Path = s, Validating = true, Valid = false }));
        _ = Task.Factory.StartNew(this.ValidateExecutables)
            .ContinueWith(t => this.AutoUpdate(t, view), CancellationToken.None);
        return base.ParametersSet(view, cancellationToken);
    }

    public void RemoveExecutable(ExecutablePath executable)
    {
        this.Executables.Remove(executable);
        this.guildWarsExecutableManager.RemoveExecutable(executable.Path);
    }

    public async Task CreateExecutable()
    {
        var path = await this.GetPath();
        if (path is null)
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

        this.Executables.Insert(0, newExecutable);
        this.guildWarsExecutableManager.AddExecutable(newExecutable.Path);
        await this.ValidateExecutables();
    }

    public async void ModifyPath(ExecutablePath executable)
    {
        var maybePath = await this.GetPath();
        if (maybePath is null)
        {
            return;
        }

        this.guildWarsExecutableManager.RemoveExecutable(executable.Path);
        executable.Path = maybePath;
        this.guildWarsExecutableManager.AddExecutable(executable.Path);
        await this.ValidateExecutables();
    }

    public async void UpdateExecutable(ExecutablePath executable)
    {
        foreach (var e in this.Executables)
        {
            e.Locked = true;
        }

        this.AddButtonEnabled = false;
        executable.Validating = true;
        await this.RefreshViewAsync();
        try
        {
            var progressUpdate = new Progress<ProgressUpdate>();
            progressUpdate.ProgressChanged += (_, e) =>
            {
                executable.UpdateProgress = $"{e.Percentage.Value * 100:F2}%";
                this.RefreshView();
            };

            executable.UpdateProgress = default;
            try
            {
                await this.semaphoreSlim.WaitAsync();
                if (!await this.guildWarsInstaller.UpdateGuildwars(executable.Path, progressUpdate, CancellationToken.None))
                {
                    executable.Validating = false;
                    executable.Valid = false;
                    executable.NeedsUpdate = true;
                }
                else
                {
                    executable.Validating = true;
                    executable.Valid = true;
                    executable.NeedsUpdate = false;
                    await this.ValidateExecutables();
                }
            }
            finally
            {
                this.semaphoreSlim.Release();
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

    private async Task ValidateExecutables()
    {
        this.validationTokenSource?.Cancel();
        this.validationTokenSource = new CancellationTokenSource();
        var token = this.validationTokenSource.Token;
        await await Task.Factory.StartNew(async () =>
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

    private async Task AutoUpdate(Task<Task> validationTask, ExecutablesView view)
    {
        await await validationTask;
        if (bool.TryParse(view.AutoRun, out var autoRun) &&
            autoRun is true)
        {
            var toUpdate = this.Executables.Where(e => e.NeedsUpdate).ToList();
            foreach (var exec in toUpdate)
            {
                this.UpdateExecutable(exec);
            }
        }
    }

    private async Task<string?> GetPath()
    {
        var path = await this.window.ShowOpenFileAsync("Select Guild Wars Executable", multiSelect: false, filters: [("Executables", ["exe"]), ("All Files", ["*"])]);
        return path.FirstOrDefault();
    }
}
