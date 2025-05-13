using Daybreak.Controls.Buttons;
using Daybreak.Models;
using Daybreak.Models.Progress;
using Daybreak.Services.Guildwars;
using Daybreak.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for GuildwarsPathTemplate.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class GuildwarsPathTemplate : UserControl
{
    private readonly IGuildWarsInstaller guildWarsInstaller;

    private CancellationTokenSource? tokenSource;

    public event EventHandler? RemoveClicked;

    [GenerateDependencyProperty]
    private bool noUpdateResult;
    [GenerateDependencyProperty]
    private bool checkingVersion;
    [GenerateDependencyProperty]
    private bool upToDate;
    [GenerateDependencyProperty]
    private string updateProgress = string.Empty;

    public GuildwarsPathTemplate() :
        this(Global.GlobalServiceProvider.GetRequiredService<IGuildWarsInstaller>())
    {
    }

    public GuildwarsPathTemplate(
        IGuildWarsInstaller guildWarsInstaller)
    {
        this.guildWarsInstaller = guildWarsInstaller.ThrowIfNull();
        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == DataContextProperty)
        {
            if (e.OldValue is ExecutablePath oldPath)
            {
                oldPath.PropertyChanged -= this.ExecutablePath_PropertyChanged;
            }

            if (e.NewValue is ExecutablePath newPath)
            {
                newPath.PropertyChanged += this.ExecutablePath_PropertyChanged;
            }

            this.CheckExecutable();
        }
    }

    private async void ExecutablePath_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (this.DataContext is not ExecutablePath)
        {
            return;
        }

        this.tokenSource?.Cancel();
        this.tokenSource?.Dispose();
        this.tokenSource = new CancellationTokenSource();
        await Task.Delay(1000, this.tokenSource.Token);
        this.CheckExecutable();
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        this.RemoveClicked?.Invoke(this, e);
    }

    private void FilePickerGlyph_Clicked(object sender, EventArgs e)
    {
        var filePicker = new OpenFileDialog()
        {
            CheckFileExists = true,
            CheckPathExists = true,
            DefaultExt = "exe",
            Multiselect = false
        };
        if (filePicker.ShowDialog() is true)
        {
            this.DataContext.As<ExecutablePath>()!.Path = filePicker.FileName;
            this.CheckExecutable();
        }
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.tokenSource?.Dispose();
    }

    private void CheckExecutable()
    {
        if (this.DataContext is not ExecutablePath executablePath)
        {
            this.Dispatcher.Invoke(() => this.NoUpdateResult = true);
            return;
        }

        if (!File.Exists(executablePath.Path))
        {
            this.Dispatcher.Invoke(() => this.NoUpdateResult = true);
            return;
        }

        this.tokenSource?.Dispose();
        this.tokenSource = new CancellationTokenSource();
        new TaskFactory().StartNew(async () =>
        {
            await this.Dispatcher.InvokeAsync(() => this.CheckingVersion = true);
            await this.Dispatcher.InvokeAsync(() => this.NoUpdateResult = false);
            if (await this.guildWarsInstaller.GetLatestVersionId(this.tokenSource.Token) is not int latestVersion)
            {
                await this.Dispatcher.InvokeAsync(() => this.CheckingVersion = false);
                await this.Dispatcher.InvokeAsync(() => this.NoUpdateResult = true);
                return;
            }

            if (await this.guildWarsInstaller.GetVersionId(executablePath.Path, this.tokenSource.Token) is not int version)
            {
                await this.Dispatcher.InvokeAsync(() => this.CheckingVersion = false);
                await this.Dispatcher.InvokeAsync(() => this.NoUpdateResult = false);
                await this.Dispatcher.InvokeAsync(() => this.UpToDate = false);
                return;
            }

            await this.Dispatcher.InvokeAsync(() => this.CheckingVersion = false);
            await this.Dispatcher.InvokeAsync(() => this.NoUpdateResult = false);
            await this.Dispatcher.InvokeAsync(() => this.UpToDate = version == latestVersion);
        }, this.tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not ExecutablePath path)
        {
            return;
        }

        if (sender is not BackButton updateButton)
        {
            return;
        }

        await this.Dispatcher.InvokeAsync(() => updateButton.IsEnabled = false);
        await this.Dispatcher.InvokeAsync(() => this.CheckingVersion = true);
        await this.Dispatcher.InvokeAsync(() => this.NoUpdateResult = false);
        this.tokenSource?.Dispose();
        this.tokenSource = new CancellationTokenSource();
        var status = new GuildwarsInstallationStatus();
        status.PropertyChanged += this.UpdateStatus_PropertyChanged;
        try
        {
            var result = await this.guildWarsInstaller.UpdateGuildwars(path.Path, status, this.tokenSource.Token);
            await this.Dispatcher.InvokeAsync(() => this.CheckingVersion = false);
            await this.Dispatcher.InvokeAsync(() => this.NoUpdateResult = false);
            await this.Dispatcher.InvokeAsync(() => this.UpToDate = result);
        }
        finally
        {
            status.PropertyChanged -= this.UpdateStatus_PropertyChanged;
        }
    }

    private void UpdateStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not GuildwarsInstallationStatus status)
        {
            return;
        }

        if (status.CurrentStep is DownloadStatus.DownloadProgressStep progressStep)
        {
            // TODO: Kinda hacky way to display a continuous progress widget
            if (progressStep is GuildwarsInstallationStatus.UnpackingProgressStep)
            {
                this.Dispatcher.Invoke(() => this.UpdateProgress = $"{(int)(50 + (progressStep.Progress * 50))}%");
            }
            else
            {
                this.Dispatcher.Invoke(() => this.UpdateProgress = $"{(int)(progressStep.Progress * 50)}%");
            }
        }
    }
}
