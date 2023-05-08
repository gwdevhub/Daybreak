using System;

namespace Daybreak.Validators;

public sealed class AllGoesValidator : IValidator
{
    public bool IsValid(object value)
    {
        return true;
    }
}
