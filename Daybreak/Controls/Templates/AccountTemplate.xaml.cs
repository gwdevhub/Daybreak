using Daybreak.Models;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for AccountTemplate.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class AccountTemplate : UserControl
{
    public event EventHandler? RemoveClicked;
    public event EventHandler? DefaultClicked;
    
    [GenerateDependencyProperty]
    private string username = string.Empty;
    [GenerateDependencyProperty]
    private string password = string.Empty;
    [GenerateDependencyProperty]
    private string characterName = string.Empty;

    public AccountTemplate()
    {
        this.InitializeComponent();
        this.DataContextChanged += this.AccountTemplate_DataContextChanged;
    }

    private void AccountTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is LoginCredentials loginCredentials)
        {
            this.PasswordBox.Password = loginCredentials.Password;
            this.Username = loginCredentials.Username;
            this.CharacterName = loginCredentials.CharacterName;
        }
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        this.RemoveClicked?.Invoke(this, e);
    }

    private void UsernameTextbox_TextChanged(object sender, EventArgs e)
    {
        this.DataContext.As<LoginCredentials>()!.Username = this.Username;
    }

    private void CharacterNameTextbox_TextChanged(object sender, EventArgs e)
    {
        this.DataContext.As<LoginCredentials>()!.CharacterName = this.CharacterName;
    }

    private void Passwordbox_PasswordChanged(object sender, EventArgs e)
    {
        this.Password = sender.As<PasswordBox>()?.Password;
        this.DataContext.As<LoginCredentials>()!.Password = this.Password!;
    }

    private void StarGlyph_Clicked(object sender, EventArgs e)
    {
        this.DefaultClicked?.Invoke(this, e);
    }
}
