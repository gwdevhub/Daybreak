using Daybreak.Shared.Converters;
using System.ComponentModel;

namespace Daybreak.Services.TradeChat.Models;

[TypeConverter(typeof(EnumToStringConverter<TraderSource>))]
public enum TraderSource
{
    Kamadan,
    Ascalon
}
