using System.ComponentModel;
using Attribute = Daybreak.Shared.Models.Guildwars.Attribute;

namespace Daybreak.Shared.Models.Builds;

public sealed class AttributeEntry : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public Attribute? Attribute
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Attribute)));
        }
    }
    public int Points
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Points)));
        }
    }
    public override string ToString() => $"{this.Attribute?.Name} - {this.Points}";
}
