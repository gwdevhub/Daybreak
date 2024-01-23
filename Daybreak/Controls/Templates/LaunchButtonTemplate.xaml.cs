using Daybreak.Configuration.Options;
using Daybreak.Models.LaunchConfigurations;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.LaunchConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Slim.Attributes;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for LaunchButtonTemplate.xaml
/// </summary>
public partial class LaunchButtonTemplate : UserControl
{
    private static readonly TimeSpan CheckGameDelay = TimeSpan.FromSeconds(1);

    private readonly ILaunchConfigurationService launchConfigurationService;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly ILiveOptions<FocusViewOptions> liveOptions;

    [GenerateDependencyProperty]
    private bool canShowFocusView;

    [GenerateDependencyProperty]
    private bool gameRunning;

    private CancellationTokenSource? tokenSource;

    public LaunchButtonTemplate(
        ILaunchConfigurationService launchConfigurationService,
        IApplicationLauncher applicationLauncher,
        ILiveOptions<FocusViewOptions> liveOptions)
    {
        this.launchConfigurationService = launchConfigurationService.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.InitializeComponent();
    }

    [DoNotInject]
    public LaunchButtonTemplate()
        :this(
            Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILaunchConfigurationService>(),
            Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IApplicationLauncher>(),
            Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveOptions<FocusViewOptions>>())
    {
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.tokenSource?.Dispose();
        this.tokenSource = new CancellationTokenSource();
        this.CheckGameState(this.tokenSource.Token);
    }

    private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.tokenSource?.Dispose();
        this.tokenSource = default;
    }

    private async void CheckGameState(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (this.DataContext is not LaunchConfigurationWithCredentials config)
            {
                await Task.Delay(CheckGameDelay, cancellationToken);
                continue;
            }
            
            if (this.applicationLauncher.GetGuildwarsProcess(config) is null)
            {
                this.GameRunning = false;
            }
            else
            {
                this.GameRunning = true;
            }

            this.CanShowFocusView = this.liveOptions.Value.Enabled && this.GameRunning;

            await Task.Delay(CheckGameDelay, cancellationToken);
        }
    }
}
