using Daybreak.Shared.Validators;
using System;

namespace Daybreak.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionCustomValidatorAttribute<T> : Attribute
    where T : IValidator, new()
{
    public IValidator Validator { get; }

    public OptionCustomValidatorAttribute()
    {
        this.Validator = Activator.CreateInstance<T>();
    }
}
