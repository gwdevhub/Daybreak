using Daybreak.Models;
using Daybreak.Services.Navigation;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Security.Principal;
using System.Windows.Controls;

namespace Daybreak.Services.Privilege;

internal sealed class PrivilegeManager : IPrivilegeManager
{
    public bool AdminPrivileges => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    private readonly IViewManager viewManager;
    private readonly ILogger<PrivilegeManager> logger;

    public PrivilegeManager(
        IViewManager viewManager,
        ILogger<PrivilegeManager> logger)
    {
        this.logger = logger.ThrowIfNull(nameof(logger));
        this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
    }

    public void RequestAdminPrivileges<TCancelView>(string messageToUser, object? dataContextOfCancelView = default)
        where TCancelView : UserControl
    {
        this.logger.LogInformation("Requesting admin privileges");
        this.viewManager.ShowView<RequestElevationView>(new ElevationRequest { View = typeof(TCancelView), DataContext = dataContextOfCancelView!, MessageToUser = messageToUser });
    }

    public void RequestNormalPrivileges<TCancelView>(string messageToUser, object? dataContextOfCancelView = default)
        where TCancelView : UserControl
    {
        this.logger.LogInformation("Requesting normal privileges");
        this.viewManager.ShowView<RequestDelevationView>(new ElevationRequest { View = typeof(TCancelView), DataContext = dataContextOfCancelView!, MessageToUser = messageToUser });
    }
}
