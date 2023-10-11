using Daybreak.Models.Progress;
using Daybreak.Services.Guildwars;
using Daybreak.Services.Navigation;
using Daybreak.Views.Launch;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Core.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Copy;
/// <summary>
/// Interaction logic for GuildwarsCopyView.xaml
/// </summary>
public partial class GuildwarsCopyView : UserControl
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly ILogger<GuildwarsCopyView> logger;
    private readonly IViewManager viewManager;
    private readonly IGuildwarsCopyService guildwarsCopyService;
    private readonly CopyStatus copyStatus = new();

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty]
    private bool progressVisible;

    public GuildwarsCopyView(
        IGuildwarsCopyService guildwarsCopyService,
        ILogger<GuildwarsCopyView> logger,
        IViewManager viewManager)
    {
        this.guildwarsCopyService = guildwarsCopyService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.copyStatus.PropertyChanged += this.CopyStatus_PropertyChanged!;
        this.InitializeComponent();
    }

    private async void GuildwarsCopyView_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not string path)
        {
            return;
        }

        this.ContinueButtonEnabled = false;
        await this.guildwarsCopyService.CopyGuildwars(path, this.copyStatus, this.cancellationTokenSource.Token);
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.ContinueButtonEnabled = true;
        });
    }

    private void GuildwarsCopyView_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
    }

    private void CopyStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        this.Dispatcher.Invoke(() =>
        {
            this.ProgressVisible = false;
            if (this.copyStatus.CurrentStep is CopyStatus.CopyProgressStep copyProgressStep)
            {
                this.ProgressValue = copyProgressStep.Progress * 100;
                this.ProgressVisible = true;
            }

            this.Description = this.copyStatus.CurrentStep.Description;
        });
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<ExecutablesView>();
    }
}
