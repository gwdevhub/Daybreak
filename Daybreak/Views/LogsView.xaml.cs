using Daybreak.Models;
using Daybreak.Services.Logging;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for LogsView.xaml
/// </summary>
public partial class LogsView : UserControl
{
    private readonly StringBuilder cachedText = new();
    private readonly RichTextModel richTextModel = new();
    private readonly SimpleHighlightingBrush simpleRedHighlightingBrush;
    private readonly SimpleHighlightingBrush simpleGreenHighlightingBrush;
    private readonly SimpleHighlightingBrush simpleOrangeHighlightingBrush;
    private readonly Color foregroundColor;
    private readonly ILogsManager logManager;
    private readonly ILogger<LogsView> logger;

    private ScrollViewer? textEditorScrollViewer;

    public LogsView(
        ILogsManager logManager,
        ILogger<LogsView> logger)
    {
        this.logManager = logManager.ThrowIfNull(nameof(logManager));
        this.logger = logger.ThrowIfNull(nameof(logger));
        this.InitializeComponent();
        this.simpleRedHighlightingBrush = new SimpleHighlightingBrush(ColorPalette.Red);
        this.simpleOrangeHighlightingBrush = new SimpleHighlightingBrush(ColorPalette.Orange);
        this.simpleGreenHighlightingBrush = new SimpleHighlightingBrush(ColorPalette.Green);
        this.TextEditor.TextArea.TextView.LineTransformers.Add(new RichTextColorizer(this.richTextModel));
        this.UpdateLogs();
    }

    private async void LogManager_ReceivedLog(object? _, Log e)
    {
        await this.WriteLogs(false, e);
    }

    private async void UpdateLogs()
    {
        var logs = this.logManager.GetLogs(l => l.LogLevel < LogLevel.Trace).ToArray();
        this.TextEditor.Clear();
        await this.WriteLogs(true, logs);
    }

    private async Task WriteLogs(bool forceScrollToEnd, params Log[] logs)
    {
        await this.Dispatcher.InvokeAsync(() =>
        {
            foreach (var log in logs)
            {
                var logTimeComponent = $"[{log.LogTime}]\t";
                this.TextEditor.Document.Insert(this.cachedText.Length, logTimeComponent);
                this.cachedText.Append(logTimeComponent);

                var logLevelComponent = $"[{log.LogLevel}]\t";
                this.richTextModel.SetForeground(this.cachedText.Length, logLevelComponent.Length, log.LogLevel switch
                {
                    LogLevel.Warning => this.simpleOrangeHighlightingBrush,
                    LogLevel.Error => this.simpleRedHighlightingBrush,
                    LogLevel.Critical => this.simpleRedHighlightingBrush,
                    _ => this.simpleGreenHighlightingBrush,
                });
                this.TextEditor.Document.Insert(this.cachedText.Length, logLevelComponent);
                this.cachedText.Append(logLevelComponent);

                var logDetailsComponent = $"[{log.Category}]\t[{log.CorrelationVector}]\n";
                this.TextEditor.Document.Insert(this.cachedText.Length, logDetailsComponent);
                this.cachedText.Append(logDetailsComponent);

                var logMessageComponent = $"{log.Message}\n\n";
                this.TextEditor.Document.Insert(this.cachedText.Length, logMessageComponent);
                this.cachedText.Append(logMessageComponent);
            }

            if (this.textEditorScrollViewer is not null &&
                this.TextEditor.VerticalOffset == this.textEditorScrollViewer.ScrollableHeight)
            {
                this.TextEditor.ScrollToEnd();
            }

            if (forceScrollToEnd)
            {
                this.TextEditor.ScrollToEnd();
            }
        });
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

    private void LogsView_Loaded(object sender, RoutedEventArgs _)
    {
        this.textEditorScrollViewer = this.TextEditor.GetType()
            .GetField("scrollViewer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(this.TextEditor)?.Cast<ScrollViewer>();
        this.logManager.ReceivedLog += this.LogManager_ReceivedLog;
    }

    private void LogsView_Unloaded(object sender, RoutedEventArgs _)
    {
        this.logManager.ReceivedLog -= this.LogManager_ReceivedLog;
    }
}
