using Daybreak.Models;
using Daybreak.Shared.Services.Credentials;
using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class AccountsViewModel(
    ICredentialManager credentialManager)
    : ViewModelBase<AccountsViewModel, AccountsView>
{
    private readonly ICredentialManager credentialManager = credentialManager;

    public List<CredentialModel> LoginCredentials { get; private set; } = [];

    public override ValueTask ParametersSet(AccountsView view, CancellationToken cancellationToken)
    {
        this.LoginCredentials.ClearAnd().AddRange(this.credentialManager.GetCredentialList().Select(l => new CredentialModel { LoginCredentials = l, PasswordVisible = false }));
        this.RefreshView();
        return base.ParametersSet(view, cancellationToken);
    }

    public void UsernameChanged(CredentialModel credentials, string newUsername)
    {
        credentials.LoginCredentials.Username = newUsername;
        this.SaveCredentials();
    }

    public void PasswordChanged(CredentialModel credentials, string password)
    {
        credentials.LoginCredentials.Password = password;
        this.SaveCredentials();
    }

    public void TogglePasswordVisibility(CredentialModel credentials)
    {
        credentials.PasswordVisible = !credentials.PasswordVisible;
        this.RefreshView();
    }

    public void CreateCredential()
    {
        var newCredentials = this.credentialManager.CreateUniqueCredentials();
        this.LoginCredentials.Insert(0, new CredentialModel { LoginCredentials = newCredentials, PasswordVisible = false });
        this.SaveCredentials();
        this.RefreshView();
    }

    public void RemoveCredential(CredentialModel credentials)
    {
        this.LoginCredentials.Remove(credentials);
        this.SaveCredentials();
        this.RefreshView();
    }

    private void SaveCredentials()
    {
        this.credentialManager.StoreCredentials([.. this.LoginCredentials.Select(c => c.LoginCredentials)]);
    }
}
