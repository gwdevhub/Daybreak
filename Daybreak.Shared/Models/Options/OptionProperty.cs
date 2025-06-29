using Daybreak.Shared.Validators;
using System.ComponentModel;
using System.Core.Extensions;

namespace Daybreak.Shared.Models.Options;
public sealed class OptionProperty
{
    public string Name { get; }
    public string Description { get; }
    public IValidator Validator { get; }
    public Action<object> Setter { get; }
    public Func<object> Getter { get; }
    public TypeConverter Converter { get; }
    public Type Type { get; }

    public OptionProperty(string name, string description, IValidator validator, Action<object> setter, Func<object> getter, TypeConverter converter, Type type)
    {
        this.Name = name.ThrowIfNull();
        this.Description = description;
        this.Validator = validator.ThrowIfNull();
        this.Setter = setter.ThrowIfNull();
        this.Getter = getter.ThrowIfNull();
        this.Converter = converter.ThrowIfNull();
        this.Type = type.ThrowIfNull();
    }
}
