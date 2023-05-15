using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Threading;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for SearchTextBox.xaml
/// </summary>
public partial class SearchTextBox : UserControl
{
    private readonly TimeSpan dispatcherCheckInterval = TimeSpan.FromMilliseconds(100);
    private readonly DispatcherTimer dispatcherTimer = new();

    private DateTime searchProcTime = DateTime.Now;

    public event EventHandler<string>? TextChanged;

    [GenerateDependencyProperty(InitialValue = true)]
    private bool placeholderVisibility;
    [GenerateDependencyProperty]
    private bool pendingSearch;
    [GenerateDependencyProperty]
    private string searchText = default!;
    [GenerateDependencyProperty]
    private TimeSpan debounceDelay = default!;
    [GenerateDependencyProperty]
    private double loadingWidgetSize;

    public SearchTextBox()
    {
        this.InitializeComponent();
        this.dispatcherTimer.Interval = this.dispatcherCheckInterval;
        this.dispatcherTimer.Tick += this.DispatcherTimer_Tick;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == ActualHeightProperty)
        {
            this.LoadingWidgetSize = this.ActualHeight / 3;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.Source.As<TextBox>().Text;
        if (searchText.IsNullOrWhiteSpace())
        {
            this.PlaceholderVisibility = true;
        }
        else
        {
            this.PlaceholderVisibility = false;
        }

        this.SearchText = searchText;
        this.EnableSearchTimer();
    }

    private void DispatcherTimer_Tick(object? _, EventArgs __)
    {
        if (DateTime.Now < this.searchProcTime)
        {
            return;
        }

        this.dispatcherTimer.Stop();
        this.PendingSearch = false;
        this.TextChanged?.Invoke(this, this.SearchText);
    }

    private void EnableSearchTimer()
    {
        this.searchProcTime = DateTime.Now + this.DebounceDelay;
        this.PendingSearch = true;
        this.dispatcherTimer.Start();
    }
}
