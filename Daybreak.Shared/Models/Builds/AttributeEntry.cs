using System.ComponentModel;
using Attribute = Daybreak.Shared.Models.Guildwars.Attribute;

namespace Daybreak.Shared.Models.Builds;

public sealed class AttributeEntry : INotifyPropertyChanged
{
    private Attribute? attribute;
    private int points;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Attribute? Attribute
    {
        get => this.attribute;
        set
        {
            this.attribute = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Attribute)));
        }
    }
    public int Points
    {
        get => this.points;
        set
        {
            this.points = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Points)));
        }
    }
    public override string ToString() => $"{this.Attribute?.Name} - {this.Points}";
}
