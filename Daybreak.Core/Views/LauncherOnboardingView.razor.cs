using Daybreak.Shared.Models.Onboarding;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public class LauncherOnboardingViewModel(IViewManager viewManager)
    : ViewModelBase<LauncherOnboardingViewModel, LauncherOnboardingView>
{
    private readonly IViewManager viewManager = viewManager;

    public LauncherOnboardingStage CurrentStage { get; private set; } = LauncherOnboardingStage.Default;

    public string Message { get; private set; } = string.Empty;

    public override ValueTask ParametersSet(LauncherOnboardingView view, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<LauncherOnboardingStage>(view.Status, out var stage))
        {
            this.viewManager.ShowView<LaunchView>();
            return ValueTask.CompletedTask;
        }

        this.CurrentStage = stage;
        this.Message = stage switch
        {
            LauncherOnboardingStage.NeedsExecutable => "To get started, please add your Guild Wars executable",
            LauncherOnboardingStage.NeedsCredentials => "To get started, please add your Guild Wars account",
            LauncherOnboardingStage.NeedsConfiguration => "To get started, please create a launch configuration",
            LauncherOnboardingStage.Complete => "You're all set! Click continue to launch Guild Wars",
            _ => throw new InvalidOperationException()
        };

        return base.ParametersSet(view, cancellationToken);
    }

    public void ContinueToStage()
    {
        //TODO: Pass a parameter to indicate that we came from onboarding. Views would then enter a more beginner friendly mode/tutorial.
        switch (this.CurrentStage)
        {
            default:
            case LauncherOnboardingStage.Default:
                break;
            case LauncherOnboardingStage.NeedsCredentials:
                this.viewManager.ShowView<AccountsView>();
                break;
            case LauncherOnboardingStage.NeedsExecutable:
                this.viewManager.ShowView<ExecutablesView>();
                break;
            case LauncherOnboardingStage.NeedsConfiguration:
                this.viewManager.ShowView<LaunchConfigurationsView>();
                break;
            case LauncherOnboardingStage.Complete:
                this.viewManager.ShowView<LaunchView>();
                break;
        }
    }
}
