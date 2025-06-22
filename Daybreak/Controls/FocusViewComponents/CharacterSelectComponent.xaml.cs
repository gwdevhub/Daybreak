using Daybreak.Shared.Models.FocusView;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.FocusViewComponents;

public partial class CharacterSelectComponent : UserControl
{
    public event EventHandler<string>? NavigateToClicked;
    public event EventHandler<CharacterSelectComponentEntry>? SwitchCharacterClicked;

    public ObservableCollection<CharacterSelectComponentEntry> Characters { get; } = [];
    
    [GenerateDependencyProperty]
    private CharacterSelectComponentEntry currentCharacter = default!;


    public CharacterSelectComponent()
    {
        this.InitializeComponent();
    }

    private void UserControl_DataContextChanged(object _, System.Windows.DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not CharacterSelectComponentContext context)
        {
            return;
        }

        if (this.CurrentCharacter != context.CurrentCharacter)
        {
            this.CurrentCharacter = context.CurrentCharacter;
        }

        var charsToAdd = context.Characters
            .Where(c => !this.Characters.Contains(c))
            .ToList();
        var charsToRemove = this.Characters
            .Where(c => !context.Characters.Contains(c))
            .ToList();

        foreach (var character in charsToAdd)
        {
            this.Characters.Add(character);
        }

        foreach (var character in charsToRemove)
        {
            this.Characters.Remove(character);
        }
    }

    private void DropDownButton_SelectionChanged(object _, object newEntry)
    {
        if (newEntry is not CharacterSelectComponentEntry newCharacter)
        {
            return;
        }

        this.SwitchCharacterClicked?.Invoke(this, newCharacter);
    }
}
