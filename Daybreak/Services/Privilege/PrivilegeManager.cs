using Daybreak.Models;
using Daybreak.Services.Logging;
using Daybreak.Services.ViewManagement;
using Daybreak.Utils;
using Daybreak.Views;
using System.Extensions;
using System.Security.Principal;
using System.Windows.Controls;

namespace Daybreak.Services.Privilege
{
    public sealed class PrivilegeManager : IPrivilegeManager
    {
        public bool AdminPrivileges => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private readonly IViewManager viewManager;
        private readonly ILogger logger;

        public PrivilegeManager(
            IViewManager viewManager,
            ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
        }

        public void RequestAdminPrivileges<TCancelView>(string messageToUser, object dataContextOfCancelView = null)
            where TCancelView : UserControl
        {
            this.logger.LogInformation("Requesting admin privileges");
            this.viewManager.ShowView<RequestElevationView>(new ElevationRequest { View = typeof(TCancelView), DataContext = dataContextOfCancelView, MessageToUser = messageToUser });
        }
    }
}
