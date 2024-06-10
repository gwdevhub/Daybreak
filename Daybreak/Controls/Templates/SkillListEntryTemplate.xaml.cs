using Daybreak.Models.Guildwars;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.TradeChat;
using Daybreak.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for SkillListEntryTemplate.xaml
/// </summary>
public partial class SkillListEntryTemplate : UserControl
{
    private readonly IIconCache iconCache;

    private Skill? skillCache;

    private CancellationTokenSource? cancellationTokenSource;

    [GenerateDependencyProperty]
    private string skillImageUri = string.Empty;

    public SkillListEntryTemplate(
        IIconCache iconCache)
    {
        this.iconCache = iconCache.ThrowIfNull();
        this.InitializeComponent();
    }

    public SkillListEntryTemplate()
        : this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IIconCache>())
    {
    }

    private void UserControl_DataContextChanged(object _, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is Skill skill)
        {
            if (skill != Skill.NoSkill)
            {
                this.FetchImage();
            }
            else
            {
                this.SkillImageUri = string.Empty;
            }
        }
    }

    private async void FetchImage()
    {
        if (this.DataContext is not Skill skill)
        {
            return;
        }

        if (this.skillCache == skill)
        {
            return;
        }

        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var token = this.cancellationTokenSource.Token;
        await Task.Run(async () =>
        {
            // Wait for the control to be visible
            while(await this.Dispatcher.InvokeAsync(() =>
            {
                var container = this.FindParent<ItemsControl>();
                if (container is null ||
                    !this.IsElementVisible(container))
                {
                    // Don't load any image since the current element is not visible on the screen
                    return true;
                }

                return false;
            }, System.Windows.Threading.DispatcherPriority.Background, token))
            {
                await Task.Delay(100, token);
            }

            var maybeUri = await this.iconCache.GetIconUri(skill, false);
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.SkillImageUri = maybeUri;
            }, System.Windows.Threading.DispatcherPriority.Background, token);
            this.skillCache = skill;
        }, this.cancellationTokenSource.Token);
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
    }

    private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = null;
    }
}
