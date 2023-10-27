using Daybreak.Launch;
using Daybreak.Models.FocusView;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for LivingEntityContextMenu.xaml
/// </summary>
public partial class LivingEntityContextMenu : UserControl
{
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;

    public event EventHandler<LivingEntity?>? LivingEntityContextMenuClicked;
    public event EventHandler<Profession?>? LivingEntityProfessionContextMenuClicked;

    [GenerateDependencyProperty]
    private string entityName = string.Empty;

    [GenerateDependencyProperty]
    private bool primaryProfessionVisible;

    public LivingEntityContextMenu()
        : this(Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IGuildwarsMemoryReader>())
    {
    }

    public LivingEntityContextMenu(
        IGuildwarsMemoryReader guildwarsMemoryReader)
    {
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.InitializeComponent();
        this.DataContextChanged += this.LivingEntityContextMenu_DataContextChanged;
    }

    private async void LivingEntityContextMenu_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (this.DataContext is not LivingEntityContextMenuContext context)
        {
            return;
        }

        if (context.LivingEntity?.PrimaryProfession != Profession.None &&
            context.LivingEntity?.PrimaryProfession is not null)
        {
            this.PrimaryProfessionVisible = true;
        }
        else
        {
            this.PrimaryProfessionVisible = false;
        }

        await this.guildwarsMemoryReader.EnsureInitialized(context.GuildWarsApplicationLaunchContext!.GuildWarsProcess, CancellationToken.None);
        this.EntityName = await this.guildwarsMemoryReader.GetNamedEntity(context.LivingEntity!, CancellationToken.None);
    }

    private void NpcDefinitionTextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.LivingEntityContextMenuClicked?.Invoke(this, this.DataContext.As<LivingEntityContextMenuContext>()?.LivingEntity ?? default);
    }

    private void PrimaryProfessionTextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.LivingEntityProfessionContextMenuClicked?.Invoke(this, this.DataContext.As<LivingEntityContextMenuContext>()?.LivingEntity?.PrimaryProfession ?? default);
    }
}
