using Daybreak.Services.Graph;
using Daybreak.Services.Graph.Models;
using Daybreak.Services.ViewManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for GraphAuthorizationView.xaml
/// </summary>
public partial class GraphAuthorizationView : UserControl
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly IGraphClient graphClient;
    private readonly IViewManager viewManager;
    private readonly ILogger<GraphAuthorizationView> logger;

    public GraphAuthorizationView(
        IGraphClient graphClient,
        IViewManager viewManager,
        ILogger<GraphAuthorizationView> logger)
    {
        this.graphClient = graphClient.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object _, RoutedEventArgs e)
    {
        var authorizationResult = await this.graphClient.PerformAuthorizationFlow(this.BrowserWrapper, this.cancellationTokenSource.Token);
        if (authorizationResult.TryExtractFailure(out var failure))
        {
            this.logger.LogError(failure, "Authorization failed");
            this.viewManager.ShowView<CompanionView>();
            return;
        }

        if (this.DataContext is not ViewRedirectContext redirectContext)
        {
            this.logger.LogError("Cannot redirect to proper view. No view set in context");
            this.viewManager.ShowView<CompanionView>();
            return;
        }

        this.viewManager.ShowView(redirectContext.CallingView);
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource.Cancel();
    }
}
