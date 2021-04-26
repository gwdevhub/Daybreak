using Daybreak.Exceptions;
using Daybreak.Models.Builds;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Configuration;
using Daybreak.Services.Screens;
using Daybreak.Services.ViewManagement;
using System;
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
    public partial class MainView : UserControl
    {
        public static readonly DependencyProperty ButtonsVisibleProperty =
            DependencyPropertyExtensions.Register<MainView, bool>(nameof(ButtonsVisible), new PropertyMetadata(true));
        public static readonly DependencyProperty LaunchButtonEnabledProperty =
            DependencyPropertyExtensions.Register<MainView, bool>(nameof(LaunchButtonEnabled));
        public static readonly DependencyProperty LaunchToolboxButtonEnabledProperty =
            DependencyPropertyExtensions.Register<MainView, bool>(nameof(LaunchToolboxButtonEnabled));
        public static readonly DependencyProperty LaunchTexmodButtonEnabledProperty =
            DependencyPropertyExtensions.Register<MainView, bool>(nameof(LaunchTexmodButtonEnabled));
        public static readonly DependencyProperty BrowsersEnabledProperty =
            DependencyPropertyExtensions.Register<MainView, bool>(nameof(BrowsersEnabled), new PropertyMetadata(true));
        public static readonly DependencyProperty RightBrowserAddressProperty =
            DependencyPropertyExtensions.Register<MainView, string>(nameof(RightBrowserAddress));
        public static readonly DependencyProperty LeftBrowserAddressProperty =
            DependencyPropertyExtensions.Register<MainView, string>(nameof(LeftBrowserAddress));
        public static readonly DependencyProperty RightBrowserFavoriteAddressProperty =
            DependencyPropertyExtensions.Register<MainView, string>(nameof(RightBrowserFavoriteAddress));
        public static readonly DependencyProperty LeftBrowserFavoriteAddressProperty =
            DependencyPropertyExtensions.Register<MainView, string>(nameof(LeftBrowserFavoriteAddress));

        private readonly IApplicationLauncher applicationDetector;
        private readonly IViewManager viewManager;
        private readonly IConfigurationManager configurationManager;
        private readonly IScreenManager screenManager;
        private readonly CancellationTokenSource cancellationTokenSource = new();

        private bool leftBrowserMaximized = false;
        private bool rightBrowserMaximized = false;

        public bool ButtonsVisible
        {
            get => this.GetTypedValue<bool>(ButtonsVisibleProperty);
            set => this.SetTypedValue(ButtonsVisibleProperty, value);
        }
        public string RightBrowserFavoriteAddress
        {
            get => this.GetTypedValue<string>(RightBrowserFavoriteAddressProperty);
            set => this.SetTypedValue(RightBrowserFavoriteAddressProperty, value);
        }
        public string LeftBrowserFavoriteAddress
        {
            get => this.GetTypedValue<string>(LeftBrowserFavoriteAddressProperty);
            set => this.SetTypedValue(LeftBrowserFavoriteAddressProperty, value);
        }
        public string RightBrowserAddress
        {
            get => this.GetTypedValue<string>(RightBrowserAddressProperty);
            set => this.SetTypedValue(RightBrowserAddressProperty, value);
        }
        public string LeftBrowserAddress
        {
            get => this.GetTypedValue<string>(LeftBrowserAddressProperty);
            set => this.SetTypedValue(LeftBrowserAddressProperty, value);
        }
        public bool LaunchButtonEnabled
        {
            get => this.GetTypedValue<bool>(LaunchButtonEnabledProperty);
            set => this.SetTypedValue(LaunchButtonEnabledProperty, value);
        }
        public bool LaunchToolboxButtonEnabled
        {
            get => this.GetTypedValue<bool>(LaunchToolboxButtonEnabledProperty);
            set => this.SetTypedValue(LaunchToolboxButtonEnabledProperty, value);
        }
        public bool LaunchTexmodButtonEnabled
        {
            get => this.GetTypedValue<bool>(LaunchTexmodButtonEnabledProperty);
            set => this.SetTypedValue(LaunchTexmodButtonEnabledProperty, value);
        }
        public bool BrowsersEnabled
        {
            get => this.GetTypedValue<bool>(BrowsersEnabledProperty);
            set => this.SetTypedValue(BrowsersEnabledProperty, value);
        }

        public MainView(
            IApplicationLauncher applicationDetector,
            IViewManager viewManager,
            IConfigurationManager configurationManager,
            IScreenManager screenManager)
        {
            this.screenManager = screenManager.ThrowIfNull(nameof(screenManager));
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
            this.applicationDetector = applicationDetector.ThrowIfNull(nameof(applicationDetector));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.PeriodicallyCheckGameState();
            this.NavigateToDefaults();
        }

        private void NavigateToDefaults()
        {
            var applicationConfiguration = this.configurationManager.GetConfiguration();
            if (applicationConfiguration.BrowsersEnabled)
            {
                this.LeftBrowserFavoriteAddress = applicationConfiguration.LeftBrowserDefault;
                this.RightBrowserFavoriteAddress = applicationConfiguration.RightBrowserDefault;
                this.LeftBrowserAddress = applicationConfiguration.LeftBrowserDefault;
                this.RightBrowserAddress = applicationConfiguration.RightBrowserDefault;
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
            this.cancellationTokenSource.Cancel();
        }

        private async void LaunchButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await this.applicationDetector.LaunchGuildwars() is false)
                {
                    return;
                }

                if (this.configurationManager.GetConfiguration().SetGuildwarsWindowSizeOnLaunch)
                {
                    var id = this.configurationManager.GetConfiguration().DesiredGuildwarsScreen;
                    var desiredScreen = this.screenManager.Screens.Skip(id).FirstOrDefault();
                    if (desiredScreen is null)
                    {
                        throw new InvalidOperationException($"Unable to set guildwars on desired screen. No screen with id {id}");
                    }
                    await Task.Delay(1000);
                    this.screenManager.MoveGuildwarsToScreen(desiredScreen);
                }

                if (this.configurationManager.GetConfiguration().ToolboxAutoLaunch is true)
                {
                    var delay = this.configurationManager.GetConfiguration().ExperimentalFeatures.ToolboxAutoLaunchDelay;
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

        private void ChromiumBrowserWrapper_BuildDecoded(object sender, Models.Builds.Build e)
        {
            this.viewManager.ShowView<BuildTemplateView>(new BuildEntry { Build = e, Name = string.Empty });
        }

        private void LeftBrowser_FavoriteUriChanged(object sender, string e)
        {
            var config = this.configurationManager.GetConfiguration();
            config.LeftBrowserDefault = e;
            this.configurationManager.SaveConfiguration(config);
        }

        private void RightBrowser_FavoriteUriChanged(object sender, string e)
        {
            var config = this.configurationManager.GetConfiguration();
            config.RightBrowserDefault = e;
            this.configurationManager.SaveConfiguration(config);
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
