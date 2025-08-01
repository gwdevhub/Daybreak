using Daybreak.Shared.Services.Privilege;
using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Security.Principal;
using System.Windows.Controls;

namespace Daybreak.Services.Privilege;

internal sealed class PrivilegeManager(
    //IViewManager viewManager,
    ILogger<PrivilegeManager> logger) : IPrivilegeManager
{
    public bool AdminPrivileges => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    //private readonly IViewManager viewManager = viewManager.ThrowIfNull(nameof(viewManager));
    private readonly ILogger<PrivilegeManager> logger = logger.ThrowIfNull(nameof(logger));

    public void RequestAdminPrivileges<TCancelView>(string messageToUser, object? dataContextOfCancelView = default)
        where TCancelView : UserControl
    {
        this.logger.LogDebug("Requesting admin privileges");
        //this.viewManager.ShowView<RequestElevationView>(new ElevationRequest { View = typeof(TCancelView), DataContext = dataContextOfCancelView!, MessageToUser = messageToUser });
    }

    public void RequestNormalPrivileges<TCancelView>(string messageToUser, object? dataContextOfCancelView = default)
        where TCancelView : UserControl
    {
        this.logger.LogDebug("Requesting normal privileges");
        //this.viewManager.ShowView<RequestDelevationView>(new ElevationRequest { View = typeof(TCancelView), DataContext = dataContextOfCancelView!, MessageToUser = messageToUser });
    }
}
