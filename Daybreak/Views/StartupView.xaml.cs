using Daybreak.Services.ApplicationDetection;
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
    public partial class StartupView : UserControl
    {
        public static readonly DependencyProperty LaunchButtonEnabledProperty =
            DependencyPropertyExtensions.Register<StartupView, bool>(nameof(LaunchButtonEnabled));
        public static readonly DependencyProperty RightBrowserAddressProperty =
            DependencyPropertyExtensions.Register<StartupView, string>(nameof(RightBrowserAddress));
        public static readonly DependencyProperty LeftBrowserAddressProperty =
            DependencyPropertyExtensions.Register<StartupView, string>(nameof(LeftBrowserAddress));
        public static readonly DependencyProperty RightBrowserFavoriteAddressProperty =
            DependencyPropertyExtensions.Register<StartupView, string>(nameof(RightBrowserFavoriteAddress));
        public static readonly DependencyProperty LeftBrowserFavoriteAddressProperty =
            DependencyPropertyExtensions.Register<StartupView, string>(nameof(LeftBrowserFavoriteAddress));

        private readonly IApplicationDetector applicationDetector;
        private readonly IViewManager viewManager;
        private readonly IConfigurationManager configurationManager;
        private readonly CancellationTokenSource cancellationTokenSource = new();

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

        public StartupView(
            IApplicationDetector applicationDetector,
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
            if (applicationDetector.IsGuildwarsRunning)
            {
                this.LaunchButtonEnabled = false;
            }
            else
            {
                this.LaunchButtonEnabled = true;
            }
        }

        private void StartupView_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void StartupView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.cancellationTokenSource.Cancel();
        }

        private void OpaqueButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.applicationDetector.LaunchGuildwars();
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
    }
}
