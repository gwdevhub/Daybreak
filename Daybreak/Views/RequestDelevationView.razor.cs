using Daybreak.Services.Privilege;
using Daybreak.Shared.Services.ApplicationLauncher;
using TrailBlazr.Services;

namespace Daybreak.Views;
public class RequestDelevationViewModel(IViewManager viewManager, IApplicationLauncher applicationLauncher, PrivilegeContext privilegeContext)
    : ElevationViewModelBase<RequestDelevationViewModel, RequestDelevationView>(viewManager, applicationLauncher, privilegeContext)
{
}
