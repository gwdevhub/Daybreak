using Daybreak.Shared.Validators;
using System.ComponentModel;

namespace Daybreak.Shared.Models.Options;
public sealed class OptionProperty
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IValidator Validator { get; init; }
    public required Func<List<object>>? ValuesFactory { get; init; }
    public required Action<OptionInstance, object?> Setter { get; init; }
    public required Func<OptionInstance, object?> Getter { get; init; }
    public required TypeConverter Converter { get; init; }
    public required Type Type { get; init; }
    public required bool IsSynchronized { get; init; }
    public required bool IsVisible { get; init; }
}
