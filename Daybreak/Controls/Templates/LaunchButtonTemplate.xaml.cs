using Daybreak.Controls.Buttons;
using Daybreak.Shared.Models;
using Daybreak.Shared.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for LaunchButtonTemplate.xaml
/// </summary>
public partial class LaunchButtonTemplate : UserControl
{
    private static readonly TimeSpan CheckGameDelay = TimeSpan.FromSeconds(1);
    
    private CancellationTokenSource? tokenSource;

    public LaunchButtonTemplate()
    {
        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.tokenSource?.Cancel();
        this.tokenSource?.Dispose();
        this.tokenSource = new CancellationTokenSource();
        Task.Run(() => this.TryFetchDataContext(this.tokenSource.Token), this.tokenSource.Token).ConfigureAwait(false);
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.tokenSource?.Cancel();
        this.tokenSource?.Dispose();
        this.tokenSource = default;
    }

    // Due to a bug in the content presenter, the datacontext is not getting propagated
    private async ValueTask TryFetchDataContext(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var found = false;
            await this.Dispatcher.InvokeAsync(() =>
            {
                if (this.FindParent<HighlightButton>()?.DataContext is LauncherViewContext parentContext)
                {
                    this.DataContext = parentContext;
                }

                // This check has to be separate, for the case when the DataContext is correctly set by the presenter
                if (this.DataContext is LauncherViewContext)
                {
                    found = true;
                }
            }, DispatcherPriority.Background, token);
            
            if (found)
            {
                return;
            }

            await Task.Delay(CheckGameDelay, token).ConfigureAwait(false);
        }
    }
}
