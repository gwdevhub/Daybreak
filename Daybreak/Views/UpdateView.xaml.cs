using Daybreak.Models;
using Daybreak.Services.Logging;
using Daybreak.Services.Updater;
using Daybreak.Services.ViewManagement;
using Daybreak.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for UpdateView.xaml
    /// </summary>
    public partial class UpdateView : UserControl
    {
        public readonly static DependencyProperty DescriptionProperty =
            DependencyPropertyExtensions.Register<UpdateView, string>(nameof(Description));
        public readonly static DependencyProperty ProgressValueProperty =
            DependencyPropertyExtensions.Register<UpdateView, double>(nameof(ProgressValue));
        public readonly static DependencyProperty ContinueButtonEnabledProperty =
            DependencyPropertyExtensions.Register<UpdateView, bool>(nameof(ContinueButtonEnabled));

        private readonly ILogger logger;
        private readonly IViewManager viewManager;
        private readonly IApplicationUpdater applicationUpdater;
        private readonly UpdateStatus updateStatus = new();

        private bool success = false;

        public string Description
        {
            get => this.GetTypedValue<string>(DescriptionProperty);
            set => this.SetValue(DescriptionProperty, value);
        }
        public double ProgressValue
        {
            get => this.GetTypedValue<double>(ProgressValueProperty);
            set => this.SetValue(ProgressValueProperty, value);
        }
        public bool ContinueButtonEnabled
        {
            get => this.GetTypedValue<bool>(ContinueButtonEnabledProperty);
            set => this.SetValue(ContinueButtonEnabledProperty, value);
        }

        public UpdateView(
            IApplicationUpdater applicationUpdater,
            ILogger logger,
            IViewManager viewManager)
        {
            this.applicationUpdater = applicationUpdater;
            this.logger = logger;
            this.viewManager = viewManager;
            this.updateStatus.PropertyChanged += UpdateStatus_PropertyChanged;
            this.InitializeComponent();
        }

        private void UpdateStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (this.updateStatus.CurrentStep is UpdateStatus.DownloadUpdateStep downloadUpdateStep)
                {
                    this.ProgressValue = downloadUpdateStep.Progress * 100;
                }

                this.Description = this.updateStatus.CurrentStep.Name;
            });
        }

        private async void UpdateView_Loaded(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Starting update procedure");
            var success = await applicationUpdater.DownloadUpdate(updateStatus).ConfigureAwait(true);
            if (success is false)
            {
                this.logger.LogError("Update procedure failed");
            }
            else
            {
                this.success= true;
                this.logger.LogInformation("Downloaded update");
            }

            this.ContinueButtonEnabled = true;
        }

        private void OpaqueButton_Clicked(object sender, System.EventArgs e)
        {
            if (this.success)
            {
                this.applicationUpdater.FinalizeUpdate();
                Application.Current.Shutdown();
            }
            else
            {
                this.viewManager.ShowView<MainView>();
            }
        }
    }
}
