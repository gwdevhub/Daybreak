using Daybreak.Controls;
using Daybreak.Models;
using Daybreak.Services.Credentials;
using Daybreak.Services.ViewManagement;
using System;
using System.Collections.ObjectModel;
using System.Extensions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// </summary>
    public partial class AccountsView : UserControl
    {
        private readonly ICredentialManager credentialManager;
        private readonly IViewManager viewManager;

        public ObservableCollection<LoginCredentials> Accounts { get; } = new();

        public AccountsView(
            ICredentialManager credentialManager,
            IViewManager viewManager)
        {
            this.credentialManager = credentialManager.ThrowIfNull(nameof(credentialManager));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.GetCredentials();
        }

        private async void GetCredentials()
        {
            var creds = await this.credentialManager.GetCredentialList().ConfigureAwait(true);
            this.Accounts.AddRange(creds);
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            this.Accounts.Add(new LoginCredentials());
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            await this.credentialManager.StoreCredentials(this.Accounts.ToList()).ConfigureAwait(true);
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void AccountTemplate_RemoveClicked(object sender, EventArgs e)
        {
            var creds = sender.As<AccountTemplate>()?.DataContext?.As<LoginCredentials>();
            this.Accounts.Remove(creds);
        }

        private void AccountTemplate_DefaultClicked(object sender, EventArgs e)
        {
            var creds = sender.As<AccountTemplate>()?.DataContext?.As<LoginCredentials>();
            foreach(var cred in this.Accounts)
            {
                cred.Default = false;
            }
            creds.Default = true;
            var view = CollectionViewSource.GetDefaultView(this.Accounts);
            view.Refresh();
        }
    }
}
