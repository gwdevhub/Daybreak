using Daybreak.Configuration;
using Daybreak.Controls;
using Daybreak.Exceptions;
using Daybreak.Models.Builds;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Screens;
using Daybreak.Services.ViewManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for StartupView.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class MainView : UserControl
    {
        private readonly IApplicationLauncher applicationDetector;
        private readonly IViewManager viewManager;
        private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveOptions;
        private readonly IScreenManager screenManager;
        private readonly CancellationTokenSource cancellationTokenSource = new();

        private bool leftBrowserMaximized = false;
        private bool rightBrowserMaximized = false;

        [GenerateDependencyProperty(InitialValue = true)]
        private bool buttonsVisible;
        [GenerateDependencyProperty]
        private string rightBrowserFavoriteAddress;
        [GenerateDependencyProperty]
        private string leftBrowserFavoriteAddress;
        [GenerateDependencyProperty]
        private string rightBrowserAddress;
        [GenerateDependencyProperty]
        private string leftBrowserAddress;
        [GenerateDependencyProperty]
        private bool launchButtonEnabled;
        [GenerateDependencyProperty]
        private bool launchToolboxButtonEnabled;
        [GenerateDependencyProperty]
        private bool launchTexmodButtonEnabled;
        [GenerateDependencyProperty(InitialValue = true)]
        private bool browsersEnabled;

        public MainView(
            IApplicationLauncher applicationDetector,
            IViewManager viewManager,
            ILiveUpdateableOptions<ApplicationConfiguration> liveOptions,
            IScreenManager screenManager)
        {
            this.screenManager = screenManager.ThrowIfNull(nameof(screenManager));
            this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
            this.applicationDetector = applicationDetector.ThrowIfNull(nameof(applicationDetector));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.PeriodicallyCheckGameState();
            this.InitializeBrowsers();
        }

        private async void InitializeBrowsers()
        {
            await this.LeftWebBrowser.InitializeDefaultBrowser();
            await this.RightWebBrowser.InitializeDefaultBrowser();
            this.NavigateToDefaults();
        }

        private void NavigateToDefaults()
        {
            var applicationConfiguration = this.liveOptions.Value;
            if (applicationConfiguration.BrowsersEnabled)
            {
                this.LeftBrowserFavoriteAddress = applicationConfiguration.LeftBrowserDefault;
                this.RightBrowserFavoriteAddress = applicationConfiguration.RightBrowserDefault;
                this.LeftBrowserAddress = applicationConfiguration.LeftBrowserDefault;
                this.RightBrowserAddress = applicationConfiguration.RightBrowserDefault;
                this.LeftWebBrowser.WebBrowser.CoreWebView2.Navigate(applicationConfiguration.LeftBrowserDefault);
                this.RightWebBrowser.WebBrowser.CoreWebView2.Navigate(applicationConfiguration.RightBrowserDefault);
            }
            else
            {
                this.BrowsersEnabled = false;
            }
        }

        private void PeriodicallyCheckGameState()
        {
            System.Extensions.TaskExtensions.RunPeriodicAsync(() => this.Dispatcher.Invoke(this.CheckGameState), TimeSpan.Zero, TimeSpan.FromSeconds(1), this.cancellationTokenSource.Token);
        }

        private void CheckGameState()
        {
            this.LaunchButtonEnabled = this.applicationDetector.IsGuildwarsRunning is false;
            this.LaunchToolboxButtonEnabled = this.applicationDetector.IsToolboxRunning is false;
            this.LaunchTexmodButtonEnabled = this.applicationDetector.IsTexmodRunning is false;
        }

        private void StartupView_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void StartupView_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private async void LaunchButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await this.applicationDetector.LaunchGuildwars() is false)
                {
                    return;
                }

                if (this.liveOptions.Value.SetGuildwarsWindowSizeOnLaunch)
                {
                    var id = this.liveOptions.Value.DesiredGuildwarsScreen;
                    var desiredScreen = this.screenManager.Screens.Skip(id).FirstOrDefault();
                    if (desiredScreen is null)
                    {
                        throw new InvalidOperationException($"Unable to set guildwars on desired screen. No screen with id {id}");
                    }

                    await Task.Delay(1000);
                    this.screenManager.MoveGuildwarsToScreen(desiredScreen);
                }

                if (this.liveOptions.Value.ToolboxAutoLaunch is true)
                {
                    var delay = this.liveOptions.Value.ExperimentalFeatures.ToolboxAutoLaunchDelay;
                    await Task.Delay(delay);
                    await this.applicationDetector.LaunchGuildwarsToolbox();
                }
            }
            catch (CredentialsNotFoundException)
            {
                this.viewManager.ShowView<AccountsView>();
            }
            catch (ExecutableNotFoundException)
            {
                this.viewManager.ShowView<ExecutablesView>();
            }
        }

        private void LaunchToolboxButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.applicationDetector.LaunchGuildwarsToolbox();
            }
            catch
            {
                this.viewManager.ShowView<SettingsView>();
            }
        }

        private void LaunchTexmodButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.applicationDetector.LaunchTexmod();
            }
            catch
            {
                this.viewManager.ShowView<SettingsView>();
            }
        }

        private void ChromiumBrowserWrapper_BuildDecoded(object _, Build e)
        {
            this.viewManager.ShowView<BuildTemplateView>(new BuildEntry { Build = e, Name = string.Empty });
        }

        private void LeftBrowser_FavoriteUriChanged(object _, string e)
        {
            var config = this.liveOptions.Value;
            config.LeftBrowserDefault = e;
            this.liveOptions.UpdateOption();
        }

        private void RightBrowser_FavoriteUriChanged(object _, string e)
        {
            var config = this.liveOptions.Value;
            config.RightBrowserDefault = e;
            this.liveOptions.UpdateOption();
        }

        private void LeftChromiumBrowserWrapper_MaximizeClicked(object sender, EventArgs e)
        {
            if (this.leftBrowserMaximized)
            {
                this.ViewContainer.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                this.ViewContainer.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                this.ViewContainer.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                this.ViewContainer.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                this.ViewContainer.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
                this.ViewContainer.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Star);
            }

            this.leftBrowserMaximized = !this.leftBrowserMaximized;
            this.ButtonsVisible = !this.leftBrowserMaximized;
        }

        private void RightChromiumBrowserWrapper_MaximizeClicked(object sender, EventArgs e)
        {
            if (this.rightBrowserMaximized)
            {
                this.ViewContainer.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                this.ViewContainer.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                this.ViewContainer.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                this.ViewContainer.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Star);
                this.ViewContainer.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
                this.ViewContainer.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
            }

            this.rightBrowserMaximized = !this.rightBrowserMaximized;
            this.ButtonsVisible = !this.rightBrowserMaximized;
        }
    }
}
