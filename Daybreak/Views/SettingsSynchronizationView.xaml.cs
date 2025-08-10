using Daybreak.Services.Graph;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Options;
using DiffPlex;
using DiffPlex.DiffBuilder;
using ICSharpCode.AvalonEdit.Highlighting;
using Newtonsoft.Json;
using System.Core.Extensions;
using System.Extensions;
using System.Text;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for SettingsSynchronizationView.xaml
/// </summary>
public partial class SettingsSynchronizationView : UserControl
{
    private readonly SimpleHighlightingBrush simpleGreenHighlightingBrush;
    private readonly SimpleHighlightingBrush simpleRedHighlightingBrush;
    private readonly SimpleHighlightingBrush simpleForegroundHighlightingBrush;
    //private readonly IViewManager viewManager;
    private readonly IGraphClient graphClient;
    private readonly IOptionsSynchronizationService optionsSynchronizationService;

    [GenerateDependencyProperty]
    private bool loading;

    [GenerateDependencyProperty]
    private string displayName = default!;

    [GenerateDependencyProperty]
    private string email = default!;

    [GenerateDependencyProperty]
    private string currentOptions = default!;

    [GenerateDependencyProperty]
    private string remoteOptions = default!;

    [GenerateDependencyProperty]
    private bool synchronized;

    public SettingsSynchronizationView(
        //IViewManager viewManager,
        IGraphClient graphClient,
        IOptionsSynchronizationService optionsSynchronizationService)
    {
        //this.viewManager = viewManager.ThrowIfNull();
        this.graphClient = graphClient.ThrowIfNull();
        this.optionsSynchronizationService = optionsSynchronizationService.ThrowIfNull();

        this.simpleGreenHighlightingBrush = new SimpleHighlightingBrush(ColorPalette.AccentColor.Green.Color);
        this.simpleRedHighlightingBrush = new SimpleHighlightingBrush(ColorPalette.AccentColor.Red.Color);
        this.simpleForegroundHighlightingBrush = new SimpleHighlightingBrush(this.FindResource("MahApps.Colors.ThemeForeground").Cast<Color>());
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        await this.UpdateInfo();
    }

    private async void DownloadButton_Clicked(object sender, System.EventArgs e)
    {
        await this.optionsSynchronizationService.RestoreOptions(CancellationToken.None);
        await this.UpdateInfo();
    }

    private async void UploadButton_Clicked(object sender, System.EventArgs e)
    {
        await this.optionsSynchronizationService.BackupOptions(CancellationToken.None);
        await this.UpdateInfo();
    }

    private async Task UpdateInfo()
    {
        var userProfile = await this.graphClient.GetUserProfile<SettingsSynchronizationView>();
        userProfile.Do(
            onSuccess: async user =>
            {
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.DisplayName = user.DisplayName;
                    this.Email = user.Email;
                    this.Loading = true;
                });
                var currentOptions = await this.optionsSynchronizationService.GetLocalOptions(CancellationToken.None);
                var remoteOptions = await this.optionsSynchronizationService.GetRemoteOptions(CancellationToken.None);
                await this.Dispatcher.InvokeAsync(() =>
                {
                    var richTextModelLocal = new RichTextModel();
                    this.LocalTextEditor.TextArea.TextView.LineTransformers.ClearAnd().Add(new RichTextColorizer(richTextModelLocal));
                    var richTextModelRemote = new RichTextModel();
                    this.RemoteTextEditor.TextArea.TextView.LineTransformers.ClearAnd().Add(new RichTextColorizer(richTextModelRemote));

                    var currentOptionsString = currentOptions is null ? string.Empty : JsonConvert.SerializeObject(currentOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
                    var remoteOptionsString = remoteOptions is null ? string.Empty : JsonConvert.SerializeObject(remoteOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
                    var diffBuilder1 = new InlineDiffBuilder(new Differ());
                    var diff1 = diffBuilder1.BuildDiffModel(currentOptionsString, remoteOptionsString);
                    var currentOptionsSb = new StringBuilder();
                    var remoteOptionsSb = new StringBuilder();
                    var diffBuilder2 = new InlineDiffBuilder(new Differ());
                    var diff2 = diffBuilder2.BuildDiffModel(remoteOptionsString, currentOptionsString);
                    foreach(var diff in diff1.Lines)
                    {
                        switch (diff.Type)
                        {
                            case DiffPlex.DiffBuilder.Model.ChangeType.Inserted:
                                continue;
                            case DiffPlex.DiffBuilder.Model.ChangeType.Imaginary:
                            case DiffPlex.DiffBuilder.Model.ChangeType.Unchanged:
                                richTextModelLocal.SetForeground(currentOptionsSb.Length, diff.Text.Length, this.simpleForegroundHighlightingBrush);
                                break;
                            case DiffPlex.DiffBuilder.Model.ChangeType.Deleted:
                            case DiffPlex.DiffBuilder.Model.ChangeType.Modified:
                                richTextModelLocal.SetForeground(currentOptionsSb.Length, diff.Text.Length, this.simpleGreenHighlightingBrush);
                                break;
                        }

                        currentOptionsSb.AppendLine(diff.Text);
                    }

                    this.LocalTextEditor.Text = currentOptionsSb.ToString();
                    foreach (var diff in diff2.Lines)
                    {
                        switch (diff.Type)
                        {
                            case DiffPlex.DiffBuilder.Model.ChangeType.Inserted:
                                continue;
                            case DiffPlex.DiffBuilder.Model.ChangeType.Imaginary:
                            case DiffPlex.DiffBuilder.Model.ChangeType.Unchanged:
                                richTextModelRemote.SetForeground(remoteOptionsSb.Length, diff.Text.Length, this.simpleForegroundHighlightingBrush);
                                break;
                            case DiffPlex.DiffBuilder.Model.ChangeType.Deleted:
                            case DiffPlex.DiffBuilder.Model.ChangeType.Modified:
                                richTextModelRemote.SetForeground(remoteOptionsSb.Length, diff.Text.Length, this.simpleRedHighlightingBrush);
                                break;
                        }

                        remoteOptionsSb.AppendLine(diff.Text);
                    }

                    this.RemoteTextEditor.Text = remoteOptionsSb.ToString();
                    this.Loading = false;
                    this.Synchronized = currentOptionsSb.ToString() == remoteOptionsSb.ToString();
                });
            },
            onFailure: exception =>
            {
                throw exception;
            });
    }
}
