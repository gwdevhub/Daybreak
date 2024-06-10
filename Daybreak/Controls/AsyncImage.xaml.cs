﻿using Daybreak.Services.Images;
using Microsoft.Extensions.DependencyInjection;
using System.Core.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for AsyncImage.xaml
/// </summary>
public partial class AsyncImage : UserControl
{
    private readonly IImageCache imageCache;

    private string? imageUriCache;
    private CancellationTokenSource? cancellationTokenSource;

    [GenerateDependencyProperty]
    private bool loading;
    [GenerateDependencyProperty]
    private ImageSource imageSource = default!;
    [GenerateDependencyProperty]
    private string imageUri = string.Empty;
    [GenerateDependencyProperty]
    private Stretch stretch;
    [GenerateDependencyProperty]
    private int desiredImageWidth;
    [GenerateDependencyProperty]
    private int desiredImageHeight;
    [GenerateDependencyProperty(InitialValue = BitmapScalingMode.HighQuality)]
    private BitmapScalingMode scalingMode = BitmapScalingMode.HighQuality;

    public AsyncImage(
        IImageCache imageCache)
    {
        this.imageCache = imageCache.ThrowIfNull();
        this.InitializeComponent();
        this.SetImageScalingMode();
    }

    public AsyncImage()
        : this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IImageCache>())
    {
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == ScalingModeProperty)
        {
            this.SetImageScalingMode();
        }
        else if (e.Property == ImageSourceProperty)
        {
            this.Loading = false;
        }
        else if (e.Property == ImageUriProperty &&
            this.ImageUri != this.imageUriCache)
        {
            this.Loading = true;
            this.UpdateImage(this.ImageUri);
        }
    }

    private void SetImageScalingMode()
    {
        RenderOptions.SetBitmapScalingMode(this.Image, this.ScalingMode);
    }

    private void UpdateImage(string maybeUri)
    {
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new();
        var token = this.cancellationTokenSource.Token;
        var desiredWidth = this.DesiredImageWidth;
        var desiredHeight = this.DesiredImageHeight;
        new TaskFactory().StartNew(() => this.imageCache.GetImage(maybeUri, desiredWidth, desiredHeight), token, TaskCreationOptions.LongRunning, TaskScheduler.Current)
            .ContinueWith(async t =>
            {
                var result = await await t;
                this.Dispatcher.Invoke(() =>
                {
                    this.ImageSource = result;
                    this.Loading = false;
                    this.imageUriCache = maybeUri;
                });
            });
    }

    private void AsyncImage_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void AsyncImage_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = null;
    }
}
