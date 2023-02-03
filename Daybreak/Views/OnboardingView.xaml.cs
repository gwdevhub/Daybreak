using Daybreak.Models;
using Daybreak.Services.Menu;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for OnboardingView.xaml
/// </summary>
public partial class OnboardingView : UserControl
{
    private readonly IMenuService menuService;
    private readonly IViewManager viewManager;
    private readonly ILogger<OnboardingView> logger;

    [GenerateDependencyProperty]
    private string description = string.Empty;

    private OnboardingStage onboardingStage;

    public OnboardingView(
        IMenuService menuService,
        IViewManager viewManager,
        ILogger<OnboardingView> logger)
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
            case OnboardingStage.NeedsCredentials:
                this.viewManager.ShowView<AccountsView>();
                this.menuService.OpenMenu();
                break;
            case OnboardingStage.NeedsExecutable:
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
        if (e.NewValue is not OnboardingStage onboardingStage)
        {
            this.logger.LogInformation($"{nameof(this.DataContext)} not set to {nameof(OnboardingStage)}");
            return;
        }

        this.onboardingStage = onboardingStage;
        this.UpdateOnboardingStage();
    }

    private void UpdateOnboardingStage()
    {
        switch (this.onboardingStage)
        {
            case OnboardingStage.Default:
                this.logger.LogError("Received default onboarding stage");
                throw new InvalidOperationException("Onboarding stage cannot be default.");
            case OnboardingStage.NeedsCredentials:
                this.Description = "No default credentials have been set. Please set up at least one credential set";
                break;
            case OnboardingStage.NeedsExecutable:
                this.Description = "No default Guildwars executable has been set. Please set up at least one Guildwars executable";
                break;
            case OnboardingStage.Complete:
                this.logger.LogError("Received complete onboarding stage");
                throw new InvalidOperationException("Onboarding stage cannot be complete");
        }
    }
}
