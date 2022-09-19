using Daybreak.Models;
using Daybreak.Services.Logging;
using Daybreak.Services.ViewManagement;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : UserControl
    {
        private readonly ILogsManager logManager;
        private readonly ILogger<LogsView> logger;

        public ObservableCollection<Log> Logs { get; } = new ObservableCollection<Log>();

        public LogsView(
            ILogsManager logManager,
            ILogger<LogsView> logger)
        {
            this.logManager = logManager.ThrowIfNull(nameof(logManager));
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.InitializeComponent();
            this.UpdateLogs();
        }

        private void UpdateLogs()
        {
            this.Logs.ClearAnd().AddRange(this.logManager.GetLogs(l => l.LogLevel < Microsoft.Extensions.Logging.LogLevel.Trace));
        }
        private async void ExportButton_Clicked(object sender, EventArgs e)
        {
            this.logger.LogInformation("Exporting logs");
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "json",
                Filter = "Json files (*.json)|*.json",
                Title = "Export logs",
                ValidateNames = true,
                CreatePrompt = true
            };
            if (saveFileDialog.ShowDialog() is true)
            {
                var fileName = saveFileDialog.FileName;
                this.logger.LogInformation($"Exporting to {fileName}");
                await File.WriteAllTextAsync(fileName, this.logManager.GetLogs().ToList().Serialize());
            }
            else
            {
                this.logger.LogInformation("Exporting canceled");
            }
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
