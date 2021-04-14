using Daybreak.Controls;
using Daybreak.Exceptions;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Configuration;
using Daybreak.Services.ViewManagement;
using System;
using System.Extensions;
using System.Threading;
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
        public static readonly DependencyProperty LaunchButtonEnabledProperty =
            DependencyPropertyExtensions.Register<MainView, bool>(nameof(LaunchButtonEnabled));
        public static readonly DependencyProperty LaunchToolboxButtonEnabledProperty =
            DependencyPropertyExtensions.Register<MainView, bool>(nameof(LaunchToolboxButtonEnabled));
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
        private readonly CancellationTokenSource cancellationTokenSource = new();

        private bool leftBrowserMaximized = false;
        private bool rightBrowserMaximized = false;

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

        public MainView(
            IApplicationLauncher applicationDetector,
            IViewManager viewManager,
            IConfigurationManager configurationManager)
        {
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
            this.LeftBrowserFavoriteAddress = applicationConfiguration.LeftBrowserDefault;
            this.RightBrowserFavoriteAddress = applicationConfiguration.RightBrowserDefault;
            this.LeftBrowserAddress = applicationConfiguration.LeftBrowserDefault;
            this.RightBrowserAddress = applicationConfiguration.RightBrowserDefault;
        }

        private void PeriodicallyCheckGameState()
        {
            System.Extensions.TaskExtensions.RunPeriodicAsync(() => this.Dispatcher.Invoke(this.CheckGameState), TimeSpan.Zero, TimeSpan.FromSeconds(1), this.cancellationTokenSource.Token);
        }

        private void CheckGameState()
        {
            this.LaunchButtonEnabled = applicationDetector.IsGuildwarsRunning is false;
            this.LaunchToolboxButtonEnabled = applicationDetector.IsToolboxRunning is false;
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
                await this.applicationDetector.LaunchGuildwars();
            }
            catch (CredentialsNotFoundException)
            {
                this.viewManager.ShowView<AccountsView>();
            }
            catch (ExecutableNotFoundException)
            {

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
        }
    }
}
