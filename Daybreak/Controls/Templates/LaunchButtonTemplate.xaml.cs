using Daybreak.Configuration.Options;
using Daybreak.Controls.Buttons;
using Daybreak.Models;
using Daybreak.Models.LaunchConfigurations;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.GWCA;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Services.Scanner;
using Daybreak.Utils;
using Microsoft.Extensions.DependencyInjection;
using Slim.Attributes;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for LaunchButtonTemplate.xaml
/// </summary>
public partial class LaunchButtonTemplate : UserControl
{
    private static readonly TimeSpan CheckGameDelay = TimeSpan.FromSeconds(1);

    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly ILaunchConfigurationService launchConfigurationService;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly ILiveOptions<FocusViewOptions> liveOptions;

    [GenerateDependencyProperty(InitialValue = true)]
    private bool canLaunch;

    [GenerateDependencyProperty]
    private bool canKill;

    [GenerateDependencyProperty]
    private bool canAttach;

    [GenerateDependencyProperty]
    private bool gameRunning;

    private CancellationTokenSource? tokenSource;

    public LaunchButtonTemplate(
        IGuildwarsMemoryReader guildwarsMemoryReader,
        ILaunchConfigurationService launchConfigurationService,
        IApplicationLauncher applicationLauncher,
        ILiveOptions<FocusViewOptions> liveOptions)
    {
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.launchConfigurationService = launchConfigurationService.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.InitializeComponent();
    }

    [DoNotInject]
    public LaunchButtonTemplate()
        :this(
            Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IGuildwarsMemoryReader>(),
            Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILaunchConfigurationService>(),
            Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IApplicationLauncher>(),
            Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveOptions<FocusViewOptions>>())
    {
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.tokenSource?.Dispose();
        this.tokenSource = new CancellationTokenSource();
        this.PeriodicallyCheckGameState(this.tokenSource.Token);
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.tokenSource?.Dispose();
        this.tokenSource = default;
    }

    private async void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs e)
    {
        await this.CheckGameState(this.tokenSource?.Token ?? CancellationToken.None);
    }

    private async void PeriodicallyCheckGameState(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // The following if case is needed due to a fault in the ContentPresenter not passing the DataContext to the content it presents
            if (this.FindParent<HighlightButton>()?.DataContext is LauncherViewContext parentContext &&
                this.DataContext != parentContext)
            {
                this.DataContext = parentContext;
            }

            await this.CheckGameState(cancellationToken);
            await Task.Delay(CheckGameDelay, cancellationToken);
        }
    }

    private async Task CheckGameState(CancellationToken cancellationToken)
    {
        if (this.DataContext is not LauncherViewContext launcherViewContext ||
                launcherViewContext.Configuration is null)
        {
            this.CanLaunch = false;
            this.CanAttach = false;
            this.CanKill = false;
            return;
        }

        if (this.applicationLauncher.GetGuildwarsProcess(launcherViewContext.Configuration) is not GuildWarsApplicationLaunchContext context)
        {
            this.GameRunning = false;
            this.CanLaunch = true;
            this.CanAttach = this.liveOptions.Value.Enabled && this.GameRunning;
            this.CanKill = false;
            launcherViewContext.CanLaunch = true;
            launcherViewContext.CanKill = false;
            return;
        }

        try
        {
            await this.guildwarsMemoryReader.EnsureInitialized(context.ProcessId, cancellationToken);
        }
        catch
        {
            this.GameRunning = false;
            this.CanLaunch = false;
            this.CanAttach = this.liveOptions.Value.Enabled && this.GameRunning;
            this.CanKill = this.CanAttach ? false : true;
            launcherViewContext.CanLaunch = false;
            launcherViewContext.CanKill = true;
            return;
        }

        var loginInfo = await this.guildwarsMemoryReader.ReadLoginData(cancellationToken);
        if (loginInfo?.Email != context.LaunchConfiguration.Credentials?.Username)
        {
            this.GameRunning = false;
            this.CanAttach = this.liveOptions.Value.Enabled && this.GameRunning;
            this.CanLaunch = true;
            this.CanKill = false;
            launcherViewContext.CanLaunch = false;
            launcherViewContext.CanKill = false;
            return;
        }

        this.GameRunning = true;
        this.CanKill = false;
        this.CanLaunch = false;
        launcherViewContext.CanLaunch = true;
        launcherViewContext.CanKill = false;
        this.CanAttach = this.liveOptions.Value.Enabled && this.GameRunning;
    }
}
