using Daybreak.Models;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for AccountTemplate.xaml
    /// </summary>
    public partial class AccountTemplate : UserControl
    {
        public static readonly DependencyProperty UsernameProperty = DependencyPropertyExtensions.Register<AccountTemplate, string>(nameof(Username));
        public static readonly DependencyProperty CharacterNameProperty = DependencyPropertyExtensions.Register<AccountTemplate, string>(nameof(CharacterName));
        public static readonly DependencyProperty PasswordProperty = DependencyPropertyExtensions.Register<AccountTemplate, string>(nameof(Password));
        public static readonly DependencyProperty IsDefaultProperty = DependencyPropertyExtensions.Register<AccountTemplate, bool>(nameof(IsDefault));


        public event EventHandler RemoveClicked;
        public event EventHandler DefaultClicked;

        public string Username
        {
            get => this.GetTypedValue<string>(UsernameProperty);
            set => this.SetValue(UsernameProperty, value);
        }
        public string Password
        {
            get => this.GetTypedValue<string>(PasswordProperty);
            set => this.SetValue(PasswordProperty, value);
        }
        public string CharacterName
        {
            get => this.GetTypedValue<string>(CharacterNameProperty);
            set => this.SetValue(CharacterNameProperty, value);
        }
        public bool IsDefault
        {
            get => this.GetTypedValue<bool>(IsDefaultProperty);
            set => this.SetValue(IsDefaultProperty, value);
        }

        public AccountTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += AccountTemplate_DataContextChanged;
        }

        private void AccountTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is LoginCredentials loginCredentials)
            {
                this.PasswordBox.Password = loginCredentials.Password;
                this.Username = loginCredentials.Username;
                this.CharacterName = loginCredentials.CharacterName;
                this.IsDefault = loginCredentials.Default;
            }
        }

        private void BinButton_Clicked(object sender, EventArgs e)
        {
            this.RemoveClicked?.Invoke(this, e);
        }

        private void UsernameTextbox_TextChanged(object sender, EventArgs e)
        {
            this.DataContext.As<LoginCredentials>().Username = this.Username;
        }

        private void CharacterNameTextbox_TextChanged(object sender, EventArgs e)
        {
            this.DataContext.As<LoginCredentials>().CharacterName = this.CharacterName;
        }

        private void Passwordbox_PasswordChanged(object sender, EventArgs e)
        {
            this.Password = sender.As<PasswordBox>()?.Password;
            this.DataContext.As<LoginCredentials>().Password = this.Password;
        }

        private void StarGlyph_Clicked(object sender, EventArgs e)
        {
            this.DefaultClicked?.Invoke(this, e);
        }
    }
}
