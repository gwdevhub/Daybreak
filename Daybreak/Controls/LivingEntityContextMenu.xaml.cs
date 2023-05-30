using Daybreak.Models.Guildwars;
using System;
using System.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for LivingEntityContextMenu.xaml
/// </summary>
public partial class LivingEntityContextMenu : UserControl
{
    public event EventHandler<LivingEntity?>? LivingEntityContextMenuClicked;
    public event EventHandler<Profession?>? LivingEntityProfessionContextMenuClicked;

    [GenerateDependencyProperty]
    private bool primaryProfessionVisible;

    public LivingEntityContextMenu()
    {
        this.InitializeComponent();
        this.DataContextChanged += this.LivingEntityContextMenu_DataContextChanged;
    }

    private void LivingEntityContextMenu_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (this.DataContext is not LivingEntity entity)
        {
            return;
        }

        if (entity.PrimaryProfession != Profession.None &&
            entity.PrimaryProfession is not null)
        {
            this.PrimaryProfessionVisible = true;
        }
        else
        {
            this.PrimaryProfessionVisible = false;
        }
    }

    private void NpcDefinitionTextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.LivingEntityContextMenuClicked?.Invoke(this, this.DataContext as LivingEntity? ?? default);
    }

    private void PrimaryProfessionTextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.LivingEntityProfessionContextMenuClicked?.Invoke(this, this.DataContext.Cast<LivingEntity>().PrimaryProfession ?? default);
    }
}
