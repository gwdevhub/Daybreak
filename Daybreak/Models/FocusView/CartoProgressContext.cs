using Daybreak.Models.Guildwars;
using System.ComponentModel;

namespace Daybreak.Models.FocusView;
public sealed class CartoProgressContext : INotifyPropertyChanged
{
    private TitleInformationExtended? titleInformationExtended;
    private double percentage;
    private Continent? continent;
    private string? resolvedName;
    private string? titleName;

    public event PropertyChangedEventHandler? PropertyChanged;

    public TitleInformationExtended? TitleInformationExtended
    {
        get => this.titleInformationExtended;
        set
        {
            this.titleInformationExtended = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TitleInformationExtended)));
        }
    }

    public double Percentage
    {
        get => this.percentage;
        set
        {
            this.percentage = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Percentage)));
        }
    }

    public Continent? Continent
    {
        get => this.continent;
        set
        {
            this.continent = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Continent)));
        }
    }

    public string? ResolvedName
    {
        get => this.resolvedName;
        set
        {
            this.resolvedName = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ResolvedName)));
        }
    }

    public string? TitleName
    {
        get => this.titleName;
        set
        {
            this.titleName = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TitleName)));
        }
    }
}
