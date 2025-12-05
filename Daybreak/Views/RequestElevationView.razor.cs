using Daybreak.Services.Privilege;
using Daybreak.Shared.Services.ApplicationLauncher;
using TrailBlazr.Services;

namespace Daybreak.Views;
public class RequestElevationViewModel(IViewManager viewManager, IApplicationLauncher applicationLauncher, PrivilegeContext privilegeContext)
    : ElevationViewModelBase<RequestElevationViewModel, RequestElevationView>(viewManager, applicationLauncher, privilegeContext)
{
}
