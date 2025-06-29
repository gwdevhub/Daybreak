using Daybreak.Controls;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.Navigation;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Launch;

/// <summary>
/// Interaction logic for AccountsView.xaml
/// </summary>
public partial class AccountsView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly ICredentialManager credentialManager;

    public ObservableCollection<LoginCredentials> Accounts { get; } = [];

    public AccountsView(
        IViewManager viewManager,
        ICredentialManager credentialManager)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.credentialManager = credentialManager.ThrowIfNull();
        this.InitializeComponent();
        this.GetCredentials();
    }

    private void GetCredentials()
    {
        var creds = this.credentialManager.GetCredentialList().ToList();
        this.Accounts.AddRange(creds);
    }

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        var newCredentials = this.credentialManager.CreateUniqueCredentials();
        this.Accounts.Add(newCredentials);
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        this.credentialManager.StoreCredentials([.. this.Accounts]);
        this.viewManager.ShowView<LauncherView>();
    }

    private void AccountTemplate_RemoveClicked(object sender, EventArgs e)
    {
        var creds = sender.As<AccountTemplate>()?.DataContext?.As<LoginCredentials>();
        if (creds is null)
        {
            return;
        }

        this.Accounts.Remove(creds);
    }
}
