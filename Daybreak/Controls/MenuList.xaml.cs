using Daybreak.Configuration.Options;
using Daybreak.Controls.Buttons;
using Daybreak.Controls.Options;
using Daybreak.Services.Notifications;
using Daybreak.Shared;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for MenuList.xaml
/// </summary>
public partial class MenuList : UserControl
{
    private readonly IMenuServiceButtonHandler menuServiceButtonHandler;
    private readonly IMenuServiceProducer menuServiceProducer;
    private readonly IViewManager viewManager;
    private readonly INotificationStorage notificationStorage;
    private readonly ILiveOptions<LauncherOptions> liveOptions;

    private CancellationTokenSource? cancellationTokenSource;

    [GenerateDependencyProperty]
    private bool showingNotificationCount;
    [GenerateDependencyProperty]
    private int notificationCount;

    public MenuList()
        : this(
              Global.GlobalServiceProvider.GetRequiredService<IMenuServiceButtonHandler>(),
              Global.GlobalServiceProvider.GetRequiredService<IMenuServiceProducer>(),
              Global.GlobalServiceProvider.GetRequiredService<IViewManager>(),
              Global.GlobalServiceProvider.GetRequiredService<INotificationStorage>(),
              Global.GlobalServiceProvider.GetRequiredService<ILiveOptions<LauncherOptions>>())
    {
    }

    private MenuList(
        IMenuServiceButtonHandler menuServiceButtonHandler,
        IMenuServiceProducer menuServiceProducer,
        IViewManager viewManager,
        INotificationStorage notificationStorage,
        ILiveOptions<LauncherOptions> liveOptions)
    {
        this.menuServiceButtonHandler = menuServiceButtonHandler.ThrowIfNull();
        this.menuServiceProducer = menuServiceProducer.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.notificationStorage = notificationStorage.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationTokenSource = new CancellationTokenSource();
        this.PopulateMenuList();
        this.PeriodicallyCheckUnopenedNotifications(this.cancellationTokenSource.Token);
    }

    private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = default;
    }

    private void PopulateMenuList()
    {
        this.MenuStackPanel.Children.Clear();
        foreach (var category in this.menuServiceProducer.GetCategories())
        {
            var expandableMenuSection = new ExpandableMenuSection
            {
                SectionTitle = category.Name,
                FontSize = 16
            };

            expandableMenuSection.SetResourceReference(ForegroundProperty, "MahApps.Brushes.ThemeForeground");
            foreach(var button in category.Buttons)
            {
                var menuButton = new MenuButton
                {
                    Title = button.Name,
                    Height = 30,
                    Cursor = Cursors.Hand,
                    ToolTip = button.Hint
                };

                menuButton.SetResourceReference(ForegroundProperty, "MahApps.Brushes.ThemeForeground");
                menuButton.SetResourceReference(MenuButton.HighlightColorProperty, "MahApps.Brushes.Accent");
                menuButton.Clicked += (_, _) => this.menuServiceButtonHandler.HandleButton(button);
                if (button.Name is "Notifications" &&
                    category.Name is "Launcher")
                {
                    var grid = new Grid();
                    var textBlock = new TextBlock
                    {
                        FontSize = 16,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        Margin = new System.Windows.Thickness(0, 0, 10, 0)
                    };

                    textBlock.SetResourceReference(ForegroundProperty, "MahApps.Brushes.ThemeForeground");
                    textBlock.SetBinding(TextBlock.TextProperty, new Binding("NotificationCount") { Source = this, Mode = BindingMode.OneWay });
                    textBlock.SetBinding(VisibilityProperty, new Binding("ShowingNotificationCount") { Source = this, Mode = BindingMode.OneWay, Converter = (IValueConverter)this.Resources["BooleanToVisibilityConverter"] });
                    grid.Children.Add(textBlock);
                    grid.Children.Add(menuButton);
                    expandableMenuSection.Children.Add(grid);
                }
                else
                {
                    expandableMenuSection.Children.Add(menuButton);
                }
            }

            if (category.Name is "Settings")
            {
                expandableMenuSection.Children.Add(new OptionsSection());
            }

            this.MenuStackPanel.Children.Add(expandableMenuSection);
        }
    }

    private async void PeriodicallyCheckUnopenedNotifications(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var unopenedNotifications = await this.notificationStorage.GetPendingNotifications(cancellationToken);
            if (unopenedNotifications.Any())
            {
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.ShowingNotificationCount = true;
                    this.NotificationCount = unopenedNotifications.Count();
                });
            }
            else
            {
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.ShowingNotificationCount = false;
                });
            }

            await Task.Delay(5000, cancellationToken);
        }
    }
}
