using Daybreak.Launch;
using Daybreak.Models.Builds;
using Daybreak.Services.IconRetrieve;
using System;
using System.Extensions;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for SkillTemplate.xaml
    /// </summary>
    public partial class SkillTemplate : UserControl
    {
        public readonly static DependencyProperty ImageSourceProperty =
            DependencyPropertyExtensions.Register<SkillTemplate, ImageSource>(nameof(ImageSource));
        public readonly static DependencyProperty BorderOpacityProperty =
            DependencyPropertyExtensions.Register<SkillTemplate, double>(nameof(BorderOpacity), new PropertyMetadata(0d));

        public event EventHandler<RoutedEventArgs> Clicked;
        public event EventHandler RemoveClicked;

        private readonly IIconRetriever iconRetriever;

        public ImageSource ImageSource
        {
            get => this.GetTypedValue<ImageSource>(ImageSourceProperty);
            set => this.SetValue(ImageSourceProperty, value);
        }
        public double BorderOpacity
        {
            get => this.GetTypedValue<double>(BorderOpacityProperty);
            set => this.SetValue(BorderOpacityProperty, value);
        }

        public SkillTemplate()
        {
            this.iconRetriever = Launcher.ApplicationServiceManager.GetService<IIconRetriever>();
            this.InitializeComponent();
            this.DataContextChanged += SkillTemplate_DataContextChanged;
        }

        private void SkillTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Skill skill)
            {
                if (skill != Skill.NoSkill)
                {
                    Task.Run(() => GetImageStream(skill)).ContinueWith((previousTask) =>
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ImageSource = GetImageSource(previousTask.Result);
                        });
                    });
                }
                else if (this.ImageSource is not null)
                {
                    this.ImageSource = null;
                }
            }
        }

        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.BorderOpacity = 1;
            this.CancelButton.Visibility = this.HasSkill() ? Visibility.Visible : Visibility.Hidden;
        }
        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.BorderOpacity = 0;
            this.CancelButton.Visibility = Visibility.Hidden;
        }
        private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Clicked?.Invoke(this, e);
        }
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            this.RemoveClicked?.Invoke(this, e);
        }

        private bool HasSkill()
        {
            if (this.DataContext is Skill skill)
            {
                return skill != Skill.NoSkill;
            }

            return false;
        }
        private async Task<Stream> GetImageStream(Skill skill)
        {
            var maybeStream = await this.iconRetriever.GetIcon(skill);
            return maybeStream.ExtractValue();
        }
        private ImageSource GetImageSource(Stream stream)
        {
            if (stream is null)
            {
                return null;
            }

            return this.Dispatcher.Invoke(() =>
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnDemand;
                bitmapImage.EndInit();
                return bitmapImage;
            });
        }
    }
}
