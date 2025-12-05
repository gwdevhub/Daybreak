using Daybreak.Shared.Models.Guildwars;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Daybreak.Shared.Converters;
public sealed class ProfessionTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) ||
               sourceType == typeof(int) ||
               sourceType == typeof(uint) ||
               sourceType == typeof(long) ||
               sourceType == typeof(ulong) ||
               sourceType == typeof(short) ||
               sourceType == typeof(ushort) ||
               sourceType == typeof(byte) ||
               sourceType == typeof(sbyte) ||
               base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
    {
        return destinationType == typeof(string) ||
               destinationType == typeof(int) ||
               destinationType == typeof(uint) ||
               destinationType == typeof(long) ||
               destinationType == typeof(ulong) ||
               destinationType == typeof(short) ||
               destinationType == typeof(ushort) ||
               destinationType == typeof(byte) ||
               destinationType == typeof(sbyte) ||
               base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is int intValue)
        {
            return Profession.Parse(intValue);
        }
        else if (value is uint uintValue)
        {
            return Profession.Parse((int)uintValue);
        }
        else if (value is long longValue)
        {
            return Profession.Parse((int)longValue);
        }
        else if (value is ulong ulongValue)
        {
            return Profession.Parse((int)ulongValue);
        }
        else if (value is short shortValue)
        {
            return Profession.Parse(shortValue);
        }
        else if (value is ushort ushortValue)
        {
            return Profession.Parse(ushortValue);
        }
        else if (value is byte byteValue)
        {
            return Profession.Parse(byteValue);
        }
        else if (value is sbyte sbyteValue)
        {
            return Profession.Parse(sbyteValue);
        }
        else if (value is string stringValue)
        {
            return Profession.Parse(stringValue);
        }
        else
        {
            return base.ConvertFrom(context, culture, value);
        }
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is not Profession profession)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        if (destinationType == typeof(int))
        {
            return profession.Id;
        }
        else if (destinationType == typeof(uint))
        {
            return (uint)profession.Id;
        }
        else if (destinationType == typeof(long))
        {
            return (long)profession.Id;
        }
        else if (destinationType == typeof(ulong))
        {
            return (ulong)profession.Id;
        }
        else if (destinationType == typeof(short))
        {
            return (short)profession.Id;
        }
        else if (destinationType == typeof(ushort))
        {
            return (ushort)profession.Id;
        }
        else if (destinationType == typeof(byte))
        {
            return (byte)profession.Id;
        }
        else if (destinationType == typeof(sbyte))
        {
            return (sbyte)profession.Id;
        }
        else if (destinationType == typeof(string))
        {
            return profession.Name ?? string.Empty;
        }
        else
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
