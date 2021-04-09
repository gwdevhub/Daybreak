using Daybreak.Models;
using Daybreak.Services.Configuration;
using Daybreak.Services.Credentials;
using Daybreak.Services.ViewManagement;
using Microsoft.Win32;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private readonly IConfigurationManager configurationManager;
        private readonly ICredentialManager credentialManager;
        private readonly IViewManager viewManager;

        public SettingsView(
            IConfigurationManager configurationManager,
            ICredentialManager credentialManager,
            IViewManager viewManager)
        {
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
            this.credentialManager = credentialManager.ThrowIfNull(nameof(credentialManager));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            var config = this.configurationManager.GetConfiguration();
            var creds = this.credentialManager.GetCredentials();
            creds.DoAny(
                onSome: (credentials) =>
                {
                    this.UsernameTextbox.Text = credentials.Username;
                    this.PasswordBox.Password = credentials.Password;

                });
            this.AddressBarReadonlyTextbox.Text = config.AddressBarReadonly.ToString();
            this.CharacterTextbox.Text = config.CharacterName;
            this.GamePathTextbox.Text = config.GamePath;
            this.ToolboxPathTextbox.Text = config.ToolboxPath;
        }

        private void SaveButton_Clicked(object sender, System.EventArgs e)
        {
            var currentConfig = this.configurationManager.GetConfiguration();
            currentConfig.CharacterName = this.CharacterTextbox.Text;
            currentConfig.GamePath = this.GamePathTextbox.Text;
            currentConfig.ToolboxPath = this.ToolboxPathTextbox.Text;
            if (bool.TryParse(this.AddressBarReadonlyTextbox.Text, out var addressBarReadonly))
            {
                currentConfig.AddressBarReadonly = addressBarReadonly;
            }

            this.configurationManager.SaveConfiguration(currentConfig);
            this.credentialManager.StoreCredentials(new LoginCredentials { Username = this.UsernameTextbox.Text, Password = this.PasswordBox.Password });
            this.viewManager.ShowView<MainView>();
        }

        private void GameFilePickerGlyph_Clicked(object sender, System.EventArgs e)
        {
            var filePicker = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "exe",
                Multiselect = false
            };
            if (filePicker.ShowDialog() is true)
            {
                this.GamePathTextbox.Text = filePicker.FileName;
            }
        }

        private void ToolboxFilePickerGlyph_Clicked(object sender, System.EventArgs e)
        {
            var filePicker = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "exe",
                Multiselect = false
            };
            if (filePicker.ShowDialog() is true)
            {
                this.ToolboxPathTextbox.Text = filePicker.FileName;
            }
        }

        private void BackButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<MainView>();
        }
    }
}
