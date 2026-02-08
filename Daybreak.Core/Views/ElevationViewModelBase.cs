using Daybreak.Services.Privilege;
using Daybreak.Shared.Services.ApplicationLauncher;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public abstract class ElevationViewModelBase<TViewModel, TView>(
    IViewManager viewManager,
    IApplicationLauncher applicationLauncher,
    PrivilegeContext privilegeContext)
    : ViewModelBase<TViewModel, TView>
        where TViewModel : ElevationViewModelBase<TViewModel, TView>
        where TView : ElevationViewBase<TView, TViewModel>
{
    private readonly IViewManager viewManager = viewManager;
    private readonly IApplicationLauncher applicationLauncher = applicationLauncher;
    private readonly PrivilegeContext privilegeContext = privilegeContext;

    private TaskCompletionSource<bool>? elevationTaskCompletionSource;
    private Type? cancelViewType;
    private (string, object)[]? cancelViewParams;
    public string Message { get; set; } = string.Empty;

    public override ValueTask ParametersSet(TView view, CancellationToken cancellationToken)
    {
        this.elevationTaskCompletionSource = this.privilegeContext.PrivilegeRequestOperation;
        this.Message = this.privilegeContext.UserMessage ?? string.Empty;
        this.cancelViewParams = this.privilegeContext.CancelViewParams;
        this.cancelViewType = this.privilegeContext.CancelViewType;
        return base.ParametersSet(view, cancellationToken);
    }

    public void CancelRequest()
    {
        this.elevationTaskCompletionSource?.SetResult(false);
        if (this.cancelViewType is not Type cancelViewType)
        {
            cancelViewType = typeof(LaunchView);
        }

        this.viewManager.ShowView(cancelViewType, this.cancelViewParams ?? []);
    }

    public void RestartApplicationAsAdmin()
    {
        this.applicationLauncher.RestartDaybreakAsAdmin();
    }

    public void RestartApplicationAsNormalUser()
    {
        this.applicationLauncher.RestartDaybreakAsNormalUser();
    }
}
