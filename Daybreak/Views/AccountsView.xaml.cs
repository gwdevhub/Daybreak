using Daybreak.Controls;
using Daybreak.Models;
using Daybreak.Services.Credentials;
using Daybreak.Services.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for AccountsView.xaml
/// </summary>
public partial class AccountsView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly ICredentialManager credentialManager;

    public ObservableCollection<LoginCredentials> Accounts { get; } = new();

    public AccountsView(
        IViewManager viewManager,
        ICredentialManager credentialManager)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.credentialManager = credentialManager.ThrowIfNull();
        this.InitializeComponent();
        this.GetCredentials();
    }

    private async void GetCredentials()
    {
        var creds = await this.credentialManager.GetCredentialList().ConfigureAwait(true);
        this.Accounts.AddRange(creds);
    }

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        var newCredentials = new LoginCredentials();
        this.Accounts.Add(newCredentials);
        if (this.Accounts.Count == 1)
        {
            this.SetAccountAsDefault(newCredentials);
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        await this.credentialManager.StoreCredentials(this.Accounts.ToList()).ConfigureAwait(true);
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
        if (this.Accounts.Count > 0 && creds.Default is true)
        {
            this.SetAccountAsDefault(this.Accounts.First());
        }
    }

    private void AccountTemplate_DefaultClicked(object sender, EventArgs e)
    {
        var creds = sender.As<AccountTemplate>()?.DataContext?.As<LoginCredentials>();
        if (creds is null)
        {
            return;
        }

        this.SetAccountAsDefault(creds);
    }

    private void SetAccountAsDefault(LoginCredentials loginCredentials)
    {
        foreach (var cred in this.Accounts)
        {
            cred.Default = false;
        }
        loginCredentials.Default = true;
        var view = CollectionViewSource.GetDefaultView(this.Accounts);
        view.Refresh();
    }
}
