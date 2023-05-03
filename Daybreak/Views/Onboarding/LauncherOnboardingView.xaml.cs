using Daybreak.Models.Onboarding;
using Daybreak.Services.Menu;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for LauncherOnboardingView.xaml
/// </summary>
public partial class LauncherOnboardingView : UserControl
{
    private readonly IMenuService menuService;
    private readonly IViewManager viewManager;
    private readonly ILogger<LauncherOnboardingView> logger;

    [GenerateDependencyProperty]
    private string description = string.Empty;

    private LauncherOnboardingStage onboardingStage;

    public LauncherOnboardingView(
        IMenuService menuService,
        IViewManager viewManager,
        ILogger<LauncherOnboardingView> logger)
    {
        this.menuService = menuService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        switch (this.onboardingStage)
        {
            case LauncherOnboardingStage.NeedsCredentials:
                this.viewManager.ShowView<AccountsView>();
                this.menuService.OpenMenu();
                break;
            case LauncherOnboardingStage.NeedsExecutable:
                this.viewManager.ShowView<ExecutablesView>();
                this.menuService.OpenMenu();
                break;
            default:
                this.logger.LogError($"Unexpected onboarding stage {this.onboardingStage}");
                throw new InvalidOperationException($"Unexpected onboarding stage {this.onboardingStage}");
        }
    }

    private void UserControl_DataContextChanged(object _, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not LauncherOnboardingStage onboardingStage)
        {
            this.logger.LogInformation($"{nameof(this.DataContext)} not set to {nameof(LauncherOnboardingStage)}");
            return;
        }

        this.onboardingStage = onboardingStage;
        this.UpdateOnboardingStage();
    }

    private void UpdateOnboardingStage()
    {
        switch (this.onboardingStage)
        {
            case LauncherOnboardingStage.Default:
                this.logger.LogError("Received default onboarding stage");
                throw new InvalidOperationException("Onboarding stage cannot be default.");
            case LauncherOnboardingStage.NeedsCredentials:
                this.Description = "No default credentials have been set. Please set up at least one credential set";
                break;
            case LauncherOnboardingStage.NeedsExecutable:
                this.Description = "No default Guildwars executable has been set. Please set up at least one Guildwars executable";
                break;
            case LauncherOnboardingStage.Complete:
                this.logger.LogError("Received complete onboarding stage");
                throw new InvalidOperationException("Onboarding stage cannot be complete");
        }
    }
}
