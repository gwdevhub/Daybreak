using Daybreak.Models.Progress;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for UpdateView.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class IconDownloadView : UserControl
    {
        private readonly IIconDownloader iconDownloader;
        private readonly IViewManager viewManager;
        private readonly ILogger<IconDownloadView> logger;

        [GenerateDependencyProperty]
        private string description = string.Empty;
        [GenerateDependencyProperty]
        private double progressValue;

        public IconDownloadView(
            IIconDownloader iconDownloader,
            IViewManager viewManager,
            ILogger<IconDownloadView> logger)
        {
            this.iconDownloader = iconDownloader.ThrowIfNull();
            this.viewManager = viewManager.ThrowIfNull();
            this.logger = logger.ThrowIfNull();
            this.InitializeComponent();
        }

        private void OpaqueButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<BuildsListView>();
        }

        private async void IconDownloadView_Loaded(object sender, RoutedEventArgs e)
        {
            var iconDownloadStatus = await this.iconDownloader.StartIconDownload().ConfigureAwait(true);
            if (iconDownloadStatus.CurrentStep is IconDownloadStatus.FinishedIconDownloadStep or
                                                    IconDownloadStatus.StoppedIconDownloadStep)
            {
                this.viewManager.ShowView<BuildsListView>();
            }

            iconDownloadStatus.PropertyChanged += (_, _) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.Description = iconDownloadStatus.CurrentStep.Description;
                    this.ProgressValue = iconDownloadStatus.CurrentStep.Progress;
                });
            };

            this.Description = iconDownloadStatus.CurrentStep.Description;
            this.ProgressValue = iconDownloadStatus.CurrentStep.Progress;
        }
    }
}
