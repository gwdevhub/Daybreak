using Daybreak.Services.Logging;
using Daybreak.Services.ViewManagement;
using System;
using System.Collections.ObjectModel;
using System.Extensions;
using System.Windows.Controls;
using Daybreak.Models;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : UserControl
    {
        private readonly IViewManager viewManager;
        private readonly ILogsManager logManager;

        public ObservableCollection<Log> Logs { get; } = new ObservableCollection<Log>();

        public LogsView(
            IViewManager viewManager,
            ILogsManager logManager)
        {
            this.logManager = logManager.ThrowIfNull(nameof(logManager));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.UpdateLogs();
        }

        private void UpdateLogs()
        {
            this.Logs.ClearAnd().AddRange(this.logManager.GetLogs());
        }
        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<SettingsCategoryView>();
        }
        private void BinButton_Clicked(object sender, EventArgs e)
        {
            this.logManager.DeleteLogs();
            this.UpdateLogs();
        }
        private void RefreshGlyph_Clicked(object sender, EventArgs e)
        {
            this.UpdateLogs();
        }
    }
}
