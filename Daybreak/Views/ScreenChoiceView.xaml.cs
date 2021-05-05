using Daybreak.Controls.Templates;
using Daybreak.Models;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Configuration;
using Daybreak.Services.Screens;
using Daybreak.Services.ViewManagement;
using System;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for ScreenChoiceView.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class ScreenChoiceView : UserControl
    {        
        private readonly IScreenManager screenManager;
        private readonly IViewManager viewManager;
        private readonly IConfigurationManager configurationManager;
        private readonly IApplicationLauncher applicationLauncher;
        private int selectedId;

        [GenerateDependencyProperty]
        private bool canTest;

        public ScreenChoiceView(
            IViewManager viewManager,
            IScreenManager screenManager,
            IConfigurationManager configurationManager,
            IApplicationLauncher applicationLauncher)
        {
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.screenManager = screenManager.ThrowIfNull(nameof(screenManager));
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
            this.applicationLauncher = applicationLauncher.ThrowIfNull(nameof(applicationLauncher));
            this.InitializeComponent();
            this.selectedId = configurationManager.GetConfiguration().DesiredGuildwarsScreen;
            this.CanTest = applicationLauncher.IsGuildwarsRunning;
            this.SetupView();
        }

        private void SetupView()
        {
            foreach(var screen in this.screenManager.Screens)
            {
                var screenTemplate = new ScreenTemplate
                {
                    DataContext = screen,
                    Margin = new System.Windows.Thickness(screen.Size.Left, screen.Size.Top, 0, 0),
                    Width = screen.Size.Width,
                    Height = screen.Size.Height,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Foreground = screen.Id == this.selectedId ? Brushes.LightGreen : Brushes.White
            };
                screenTemplate.Clicked += ScreenTemplate_Clicked;
                this.ScreenContainer.Children.Add(screenTemplate);
            }
        }

        private void ScreenTemplate_Clicked(object sender, Screen e)
        {
            this.SelectScreen(e);
        }

        private void SelectScreen(Screen screen)
        {
            this.selectedId = screen.Id;
            foreach(var template in this.ScreenContainer.Children.OfType<ScreenTemplate>())
            {
                template.Foreground = template.DataContext.As<Screen>().Id == this.selectedId ? Brushes.LightGreen : Brushes.White;
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<SettingsView>();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            this.configurationManager.GetConfiguration().DesiredGuildwarsScreen = this.selectedId;
            this.viewManager.ShowView<SettingsCategoryView>();
        }

        private void OpaqueButton_Clicked(object sender, EventArgs e)
        {
            var screen = this.screenManager.Screens.Skip(this.selectedId).FirstOrDefault();
            if (screen is null)
            {
                throw new InvalidOperationException($"Unable to test placement. No screen with id {this.selectedId}");
            }

            this.screenManager.MoveGuildwarsToScreen(screen);
        }
    }
}
