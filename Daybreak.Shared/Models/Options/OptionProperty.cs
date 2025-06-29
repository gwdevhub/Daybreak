using Daybreak.Shared.Validators;
using System.ComponentModel;
using System.Core.Extensions;

namespace Daybreak.Shared.Models.Options;
public sealed class OptionProperty(string name, string description, IValidator validator, Action<object> setter, Func<object> getter, TypeConverter converter, Type type)
{
    public string Name { get; } = name.ThrowIfNull();
    public string Description { get; } = description;
    public IValidator Validator { get; } = validator.ThrowIfNull();
    public Action<object> Setter { get; } = setter.ThrowIfNull();
    public Func<object> Getter { get; } = getter.ThrowIfNull();
    public TypeConverter Converter { get; } = converter.ThrowIfNull();
    public Type Type { get; } = type.ThrowIfNull();
}
