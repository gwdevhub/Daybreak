﻿using System;

namespace Daybreak.Validators;

public sealed class EnumValidator<T> : IValidator
    where T : struct, Enum
{
    public bool IsValid(object value)
    {
        if (value is T enumValue)
        {
            return Enum.IsDefined<T>(enumValue);
        }

        if (value is string stringValue)
        {
            return Enum.TryParse<T>(stringValue, true, out _);
        }

        return false;
    }
}
